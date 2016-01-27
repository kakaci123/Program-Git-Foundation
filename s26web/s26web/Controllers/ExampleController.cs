using System.Linq;
using System.Web.Mvc;

namespace MvcWithStyle.Controllers
{
    public class ExampleController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult Grid()
        {
            return View();
        }
    }
}
