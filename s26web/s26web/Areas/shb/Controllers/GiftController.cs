using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{
    public class GiftController : Controller
    {
        GiftModel data = new GiftModel();

        [MyAuthorize(function = "贈品管理")]
        public ActionResult Index(int p = 1, int show_number = 10)
        {
            try
            {
                ViewBag.Title = "贈品管理";
                ViewData["path"] = "Product";
                ViewData["p"] = p;
                ViewData["page"] = data.Get_Page(p, show_number);
                return View(data.Get_Data());
            }
            catch
            {
                TempData["err"] = "Gift_0";
                return View("~/Areas/shb/Views/Gift/Index.cshtml");
            }
        }

        [MyAuthorize(function = "贈品管理")]
        public ActionResult Create()
        {
            ViewBag.Get_MemberType = data.Get_MemberType();
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "贈品管理")]
        public ActionResult Create(GiftModel.GiftModelShow item, HttpPostedFileBase file = null, string[] MemberTypelist = null)
        {
            try
            {
                foreach (var i in MemberTypelist)
                { item.MemberType += i + ","; }

                //刪除最後一個逗號
                item.MemberType = item.MemberType.Remove(item.MemberType.LastIndexOf(","), 1);
               
                if (data.Insert(item, file, "Gift/" + item.Id, Server) <= 0)
                {
                    TempData["err"] = "Gift_0";
                    return RedirectToAction("Index");
                }
            }
            catch { TempData["err"] = "Gift_1"; }
            return RedirectToAction("Index");
        }

        [MyAuthorize(function = "贈品管理")]
        public ActionResult Edit(int id = 0, int p = 1)
        {
            ViewBag.Title = "贈品管理 > 編輯";
            ViewBag.c_title = "贈品資料";
            ViewData["p"] = p;
            var item = data.Get_One(id);

            if (item == null)
            {
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [HttpPost]
        [MyAuthorize(function = "贈品管理")]
        public ActionResult Edit(GiftModel.GiftModelShow item, int p = 1, string[] MemberTypelist = null)
        {
            {
                s26webDataContext db = new s26webDataContext();
                var old = db.Gift.FirstOrDefault(f => f.Id == item.Id);
                db.Connection.Close();
                foreach (var i in MemberTypelist)
                { item.MemberType += i + ","; }

                //刪除最後一個逗號
                item.MemberType = item.MemberType.Remove(item.MemberType.LastIndexOf(","), 1);

                if (data.Update(item) <= 0)
                {
                    TempData["err"] = "Gift_5";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index", new { p = p });
        }
    }
}
