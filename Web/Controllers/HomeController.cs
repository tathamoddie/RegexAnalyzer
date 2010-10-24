using System.Web.Mvc;

namespace TathamOddie.RegexAnalyzer.Web.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View(Views.Index);
        }
    }
}