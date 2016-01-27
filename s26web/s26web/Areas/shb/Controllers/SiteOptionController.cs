using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{
    public class SiteOptionController : Controller
    {
        SiteOptionModel data = new SiteOptionModel();

        public ActionResult Index()
        {
            return View(data.Get());
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Index(SiteOptionModel.SiteOptionShow item, HttpPostedFileBase WebLogo, HttpPostedFileBase WebIcon)
        {
            if (ModelState.IsValid)
            {
                string url = s26web.Models.Method.Upload_Logo(WebLogo, Server, "logo.png");
                string icon = s26web.Models.Method.Upload_Logo(WebIcon, Server, "icon.ico");
                item.WebLogo = url;
                item.WebIcon = icon;
                if (item.SMTP_Account == null)
                {
                    item.SMTP_Account = "";
                }
                if (data.Set(item) > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            TempData["err"] = 1;
            return View(item);
        }
    }
}
