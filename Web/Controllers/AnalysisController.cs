using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;

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

            stopwatch.Stop();

            ViewData["TimeTaken"] = stopwatch.Elapsed;

            return View("Basic");
        }

        ActionResult AnalyzeVerbose(string expression)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tokens = Tokenizer.Tokenize(expression);
            var nodes = new TreeBuilder().Build(tokens);

            stopwatch.Stop();

            ViewData["TimeTaken"] = stopwatch.Elapsed;

            ViewData["Tokens"] = tokens;
            
            ViewData["AllNodes"] = FlattenNodes(nodes);
            ViewData["NodesMarkup"] = RenderNodesAsHtml(nodes);

            return View("Verbose");
        }

        static IEnumerable<KeyValuePair<int, Node>> FlattenNodes(IEnumerable<Node> nodes)
        {
            var nodesToProcess = new Stack<Node>(nodes.Reverse());
            var depths = new Stack<int>(new [] { nodes.Count() });
            while (nodesToProcess.Any())
            {
                var currentNode = nodesToProcess.Pop();

                while (depths.Any() && depths.Peek() == 0)
                    depths.Pop();

                depths.Push(depths.Pop() - 1);

                yield return new KeyValuePair<int, Node>(depths.Count() - 1, currentNode);

                var children = currentNode.Children;
                if (!children.Any()) continue;

                depths.Push(children.Count());
                foreach(var childNode in children.Reverse())
                    nodesToProcess.Push(childNode);
            }
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