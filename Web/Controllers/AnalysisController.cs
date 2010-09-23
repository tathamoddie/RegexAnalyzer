using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Mvc;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace Web.Controllers
{
    public class AnalysisController : Controller
    {
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

            return View("Verbose");
        }
    }
}