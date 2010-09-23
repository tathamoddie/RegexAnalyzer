using System;
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
            var layers = new Stack<int>();
            while (nodesToProcess.Any())
            {
                var needToCloseLayer = false;
                if (layers.Any())
                {
                    var currentLayer = layers.Pop();

                    if (currentLayer > 0)
                        currentLayer--;

                    if (currentLayer == 0)
                        needToCloseLayer = true;
                    else
                        layers.Push(currentLayer);
                }

                var currentNode = nodesToProcess.Pop();

                markupBuilder.AppendFormat("<li><span class=\"ast-node-data\"><code>{0}</code></span> <span class=\"ast-node-description\"><span>{1}</span></span></li>", currentNode.Data, currentNode.GetType().Name);

                if (needToCloseLayer)
                    markupBuilder.Append("</ol>");

                if (!currentNode.Children.Any()) continue;

                markupBuilder.Append("<ol>");

                foreach (var childNode in currentNode.Children.Reverse())
                    nodesToProcess.Push(childNode);

                layers.Push(currentNode.Children.Count());
            }

            markupBuilder.Append("</ol>");

            return new HtmlString(markupBuilder.ToString());
        }
    }
}