using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{
    public class OnlineController : Controller
    {
        OnlineModel data = new OnlineModel();

        [MyAuthorize(function = "線上活動管理")]
        public ActionResult Index(int p = 1, int show_number = 10)
        {
            try
            {
                ViewBag.Title = "線上活動管理";
                ViewData["p"] = p;
                ViewData["page"] = data.Get_Page(p, show_number);
                return View(data.Get_Data());
            }
            catch
            {
                TempData["err"] = "Online_0";
                return View("~/Areas/shb/Views/Online/Index.cshtml");
            }
        }

        [MyAuthorize(function = "線上活動管理")]
        public ActionResult Create()
        {

            ViewBag.Get_Type = data.Get_Type();
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "線上活動管理")]
        public ActionResult Create(OnlineModel.OnlineModelShow item, HttpPostedFileBase Banner_PC = null, HttpPostedFileBase Banner_Mobile = null, string[] Typelist = null)
        {
            try
            {

                foreach (var i in Typelist)
                { item.Type += i + ","; }

                //刪除最後一個逗號
                item.Type = item.Type.Remove(item.Type.LastIndexOf(","), 1);

                string img_pc = data.Upload_Image(Banner_PC, Server);
                item.Banner_PC = (img_pc == "") ? null : img_pc;

                string img_mobile = data.Upload_Image(Banner_Mobile, Server);
                item.Banner_Mobile = (img_mobile == "") ? null : img_mobile;

                if (data.Insert(item) <= 0)
                {
                    TempData["err"] = "Online_0";
                    return RedirectToAction("Index");
                }
            }
            catch { TempData["err"] = "Online_1"; }
            return RedirectToAction("Index");
        }

        [MyAuthorize(function = "線上活動管理")]
        public ActionResult Edit(int id = 0, int p = 1)
        {
            ViewBag.Title = "線上活動管理 > 編輯";
            ViewData["p"] = p;

            var item = data.Get_One(id);
            ViewBag.Get_Type = data.Get_Type();
            if (item == null)
            {
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "線上活動管理")]
        public ActionResult Edit(OnlineModel.OnlineModelShow item, HttpPostedFileBase Banner_PC = null, HttpPostedFileBase Banner_Mobile = null, int p = 1, string[] Typelist = null)
        {
            foreach (var i in Typelist)
            { item.Type += i + ","; }

            //刪除最後一個逗號
            item.Type = item.Type.Remove(item.Type.LastIndexOf(","), 1);

            string img_pc = data.Upload_Image(Banner_PC, Server);
            item.Banner_PC = (img_pc == "") ? null : img_pc;

            string img_mobile = data.Upload_Image(Banner_Mobile, Server);
            item.Banner_Mobile = (img_mobile == "") ? null : img_mobile;

            if (data.Update(item) <= 0)
            {
                TempData["err"] = "Online_5";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { p = p });
        }

        [MyAuthorize(function = "線上活動管理")]
        public ActionResult Delete(int Id, int p = 1)
        {
            data.Delete(Id);

            return RedirectToAction("Index", new { p = p });
        }

    }
}
