using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{
    public class IntroductionController : Controller
    {
        IntroductionModel data = new IntroductionModel();

        public ActionResult Join_0()
        {
            var result = data.Get_One(12);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            ViewData["temp_dir"] = "Join_0";
            return View("~/Areas/shb/Views/Introduction/Introduction2.cshtml", result);
        }

        [HttpPost]
        [ValidateInput(true)]
        //[ValidateInput(true)]
        public ActionResult Join_0(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (data.Set(item) > 0)
                {
                    return RedirectToAction("Join_0");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/Introduction2.cshtml", item);
        }

        public ActionResult Join_1()
        {
            var result = data.Get_One(13);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            ViewData["temp_dir"] = "Join_1";
            return View("~/Areas/shb/Views/Introduction/Introduction2.cshtml", result);
        }

        [HttpPost]
        [ValidateInput(true)]
        //[ValidateInput(true)]
        public ActionResult Join_1(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (data.Set(item) > 0)
                {
                    return RedirectToAction("Join_1");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/Introduction2.cshtml", item);
        }
    }
}
