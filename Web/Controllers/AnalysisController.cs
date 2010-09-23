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

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = verbose ? AnalyzeVerbose(q) : AnalyzeBasic(q);
            
            stopwatch.Stop();

            ViewData["TimeTaken"] = stopwatch.Elapsed;

            return result;
        }

        ActionResult AnalyzeBasic(string expression)
        {
            return View("Basic");
        }

        ActionResult AnalyzeVerbose(string expression)
        {
            var tokens = Tokenizer.Tokenize(expression);

            ViewData["Tokens"] = tokens;

            var nodes = new TreeBuilder().Build(tokens);

            ViewData["NodesMarkup"] = RenderNodesAsHtml(nodes);

            return View("Verbose");
        }

        static IHtmlString RenderNodesAsHtml(IEnumerable<Node> nodes)
        {
            var markupBuilder = new StringBuilder();
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

                markupBuilder.AppendFormat("<li>{0}</li>", currentNode.Data);

                if (needToCloseLayer)
                    markupBuilder.Append("</ol>");

                if (!currentNode.Children.Any()) continue;

                markupBuilder.Append("<ol>");

                foreach (var childNode in currentNode.Children.Reverse())
                    nodesToProcess.Push(childNode);

                layers.Push(currentNode.Children.Count());
            }
            return new HtmlString(markupBuilder.ToString());
        }
    }
}