using s26web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace s26web.Areas.shb.Controllers
{
    public class S26Controller : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "首頁";
            if (!Method.Is_Login_Admin(Request.Cookies))
            {
                return RedirectToAction("Login", "Member", new { ReturnUrl = Method.RootPath + "/shb/S26/Index" });
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Ajax_Area(int city, bool zip, bool all)
        {
            return Content(CityArea.Get_Area_Ajax(city, 0, zip, all));
        }
        public ActionResult Ajax_Area1(int categoryId, bool all)
        {
            return Content(s26web.Areas.shb.Models.IntroductionModel.Get_FrontTitle_Ajax(categoryId, 0, all));
        }

    }
}
