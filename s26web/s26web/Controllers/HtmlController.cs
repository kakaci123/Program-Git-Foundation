using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace s26web.Controllers
{
    public class HtmlController : Controller
    {
        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermsConditions()
        {
            return View();
        }
    }
}
