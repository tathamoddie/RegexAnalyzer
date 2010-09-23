using System;
using System.ComponentModel;
using System.Web.Mvc;

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

            return verbose ? AnalyzeVerbose(q) : AnalyzeBasic(q);
        }

        ActionResult AnalyzeBasic(string expression)
        {
            return View("Basic");
        }

        ActionResult AnalyzeVerbose(string expression)
        {
            return View("Verbose");
        }
    }
}