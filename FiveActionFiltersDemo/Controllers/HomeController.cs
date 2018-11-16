using System.Web.Mvc;
using FiveActionFiltersDemo.ActionFilters;

namespace FiveActionFiltersDemo.Controllers
{
    public class HomeController : Controller
    {
        [TidyHtml]
        public ActionResult Index()
        {
            return View();
        }

        [CompressFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [WhitespaceFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}