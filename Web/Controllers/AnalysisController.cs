using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;
using Web.Models;

namespace Web.Controllers
{
    public class AnalysisController : Controller
    {
        [ValidateInput(false)]
        public ActionResult Analyze(
            [DefaultValue(false)]bool verbose,
            string q)
        {
            if (string.IsNullOrEmpty(q))
                return View("NoExpression");

            ViewData["Expression"] = q;

            var result = verbose ? AnalyzeVerbose(q) : AnalyzeBasic(q);

            return result;
        }

        ActionResult AnalyzeBasic(string expression)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tokens = Tokenizer.Tokenize(expression);
            var rootNode = new TreeBuilder().Build(tokens);

            stopwatch.Stop();

            ViewData["TimeTaken"] = stopwatch.Elapsed;

            ViewData["ExpressionMarkup"] = RenderExpressionAsHtml(rootNode);
            ViewData["NodesMarkup"] = RenderNodesAsHtml(rootNode.Children);

            return View("Basic");
        }

        ActionResult AnalyzeVerbose(string expression)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tokens = Tokenizer.Tokenize(expression);
            var rootNode = new TreeBuilder().Build(tokens);

            stopwatch.Stop();

            ViewData["TimeTaken"] = stopwatch.Elapsed;

            ViewData["Tokens"] = tokens;

            ViewData["ExpressionMarkup"] = RenderExpressionAsHtml(rootNode);
            ViewData["AllNodes"] = FlattenNodes(rootNode.Children);
            ViewData["NodesMarkup"] = RenderNodesAsHtml(rootNode.Children);

            return View("Verbose");
        }

        static IEnumerable<NodeViewModel> FlattenNodes(IEnumerable<Node> nodes)
        {
            var nodesToProcess = new Stack<Node>(nodes.Reverse());
            var depths = new Stack<int>(new [] { nodes.Count() });
            while (nodesToProcess.Any())
            {
                var currentNode = nodesToProcess.Pop();

                while (depths.Any() && depths.Peek() == 0)
                    depths.Pop();

                depths.Push(depths.Pop() - 1);

                yield return new NodeViewModel(currentNode)
                {
                    CssClass = BuildNodeClass(currentNode),
                    Depth = depths.Count() - 1
                };

                var children = currentNode.Children;
                if (!children.Any()) continue;

                depths.Push(children.Count());
                foreach(var childNode in children.Reverse())
                    nodesToProcess.Push(childNode);
            }
        }

        internal static IHtmlString RenderExpressionAsHtml(Node rootNode)
        {
            var markupBuilder = new StringBuilder();

            var nodes = rootNode.Children;

            var nodesToProcess = new Stack<Node>(nodes.Reverse());
            var layers = new Stack<int>(new[] { nodes.Count() });
            var layerNodes = new Stack<Node>(new[] { rootNode });
            var nodeToParentDictionary = nodes.ToDictionary(n => n, p => rootNode);

            while (nodesToProcess.Any())
            {
                var layersToClose = 0;
                while (layers.Any() && layers.Peek() == 0)
                {
                    layersToClose++;
                    layers.Pop();
                }

                layers.Push(layers.Pop() - 1);

                for (var i = 0; i < layersToClose; i++)
                {
                    RenderRemainingDataIfThisIsTheLastNode(markupBuilder, layerNodes.Pop(), nodeToParentDictionary);
                    markupBuilder.Append("</span>");
                }

                var currentNode = nodesToProcess.Pop();

                RenderDataBetweenThisAndPreviousNode(markupBuilder, currentNode, nodeToParentDictionary);

                markupBuilder.AppendFormat(
                    "<span class=\"{0}\">",
                    BuildNodeClass(currentNode));
                
                if (currentNode.Children.Any())
                {
                    layers.Push(currentNode.Children.Count());
                    layerNodes.Push(currentNode);

                    foreach (var childNode in currentNode.Children.Reverse())
                    {
                        nodesToProcess.Push(childNode);
                        nodeToParentDictionary[childNode] = currentNode;
                    }
                }
                else
                {
                    markupBuilder.Append(HttpUtility.HtmlEncode(currentNode.Data));
                    markupBuilder.Append("</span>");
                    RenderRemainingDataIfThisIsTheLastNode(markupBuilder, currentNode, nodeToParentDictionary);
                }
            }

            for (var i = 0; i < layers.Count() - 1; i++)
            {
                markupBuilder.Append("</span>");
                RenderRemainingDataIfThisIsTheLastNode(markupBuilder, layerNodes.Pop(), nodeToParentDictionary);
            }

            return new HtmlString(markupBuilder.ToString());
        }

        static void RenderDataBetweenThisAndPreviousNode(StringBuilder markupBuilder, Node currentNode, IDictionary<Node, Node> nodeToParentDictionary)
        {
            var parentNode = nodeToParentDictionary[currentNode];
            if (parentNode == null) return;

            var indexOfCurrentNodeAtThisLevel = parentNode.Children.ToList().IndexOf(currentNode);
            if (indexOfCurrentNodeAtThisLevel == 0)
            {
                if (currentNode.StartIndex == 0) return;

                var charactersBeforeThisFirstNode = parentNode.Data.Substring(0, currentNode.StartIndex - parentNode.StartIndex);

                markupBuilder.Append(HttpUtility.HtmlEncode(charactersBeforeThisFirstNode));
            }
            else
            {
                var previousNodeAtThisLevel = parentNode.Children.ElementAt(indexOfCurrentNodeAtThisLevel - 1);
                var endIndexOfPreviousNodeAtThisLevel = previousNodeAtThisLevel.StartIndex + previousNodeAtThisLevel.Data.Length;
                var numberOfCharactersBetweenPreviousAndCurrentNode = currentNode.StartIndex - endIndexOfPreviousNodeAtThisLevel;
                if (numberOfCharactersBetweenPreviousAndCurrentNode <= 0) return;

                var charactersBetweenPreviousAndCurrentNode = parentNode.Data.Substring(endIndexOfPreviousNodeAtThisLevel, numberOfCharactersBetweenPreviousAndCurrentNode);

                markupBuilder.Append(HttpUtility.HtmlEncode(charactersBetweenPreviousAndCurrentNode));
            }
        }

        static void RenderRemainingDataIfThisIsTheLastNode(StringBuilder markupBuilder, Node currentNode, IDictionary<Node, Node> nodeToParentDictionary)
        {
            var parentNode = nodeToParentDictionary[currentNode];
            if (parentNode == null) return;

            var siblings = parentNode.Children.ToList();

            var indexOfCurrentNodeAtThisLevel = siblings.IndexOf(currentNode);
            if (indexOfCurrentNodeAtThisLevel < siblings.Count() - 1) return;

            var endIndexOfCurrentNode = currentNode.StartIndex + currentNode.Data.Length;
            var numberOfRemainingCharacters = parentNode.StartIndex + parentNode.Data.Length - endIndexOfCurrentNode;
            if (numberOfRemainingCharacters <= 0) return;

            var remainingCharacters = parentNode.Data.Substring(endIndexOfCurrentNode - parentNode.StartIndex, numberOfRemainingCharacters);

            markupBuilder.Append(HttpUtility.HtmlEncode(remainingCharacters));
        }

        static IHtmlString RenderNodesAsHtml(IEnumerable<Node> nodes)
        {
            var markupBuilder = new StringBuilder();
            markupBuilder.Append("<ol class=\"ast\">");

            var nodesToProcess = new Stack<Node>(nodes.Reverse());
            var layers = new Stack<int>(new[] { nodes.Count() });
            while (nodesToProcess.Any())
            {
                var layersToClose = 0;
                while (layers.Any() && layers.Peek() == 0)
                {
                    layersToClose++;
                    layers.Pop();
                }

                layers.Push(layers.Pop() - 1);

                for (var i = 0; i < layersToClose; i++)
                    markupBuilder.Append("</ol></li>");
                
                var currentNode = nodesToProcess.Pop();

                markupBuilder.Append("<li>");

                markupBuilder.AppendFormat(
                    "<span class=\"{0}\"><span class=\"ast-node-data\"><code title=\"{1}\">{2}</code></span><span class=\"ast-node-description\"><span>{3}</span></span></span>",
                    BuildNodeClass(currentNode),
                    HttpUtility.HtmlAttributeEncode(currentNode.Data),
                    HttpUtility.HtmlEncode(currentNode.Data),
                    HttpUtility.HtmlEncode(currentNode.Description));

                if (currentNode.Children.Any())
                {
                    markupBuilder.Append("<ol>");
                    layers.Push(currentNode.Children.Count());

                    foreach (var childNode in currentNode.Children.Reverse())
                        nodesToProcess.Push(childNode);
                }
                else
                {
                    markupBuilder.Append("</li>");
                }
            }

            for (var i = 0; i < layers.Count(); i++)
                markupBuilder.Append("</ol>");

            return new HtmlString(markupBuilder.ToString());
        }

        internal static string BuildNodeClass(Node currentNode)
        {
            var nodeClasses = new List<string>
            {
                "ast-node",
                "ast-node-" + currentNode.NodeId
            };

            if (currentNode is ParseFailureNode)
                nodeClasses.Add("ast-parse-failure-node");

            return string.Join(" ", nodeClasses.Where(n => !string.IsNullOrEmpty(n)).ToArray());
        }
    }
}