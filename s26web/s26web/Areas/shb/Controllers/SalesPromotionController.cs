using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{
    public class SalesPromotionController : Controller
    {
        SalesPromotionModel data = new SalesPromotionModel();

        [MyAuthorize(function = "促銷碼活動管理")]
        public ActionResult Index(int p = 1, int show_number = 10)
        {
            try
            {
                ViewBag.Title = "促銷碼活動管理";
                ViewData["p"] = p;
                ViewData["page"] = data.Get_Page(p, show_number);
                return View(data.Get_Data());
            }
            catch
            {
                TempData["err"] = "SalesPromotion_0";
                return View("~/Areas/shb/Views/SalesPromotion/Index.cshtml");
            }
        }

        [MyAuthorize(function = "促銷碼活動管理")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "促銷碼活動管理")]
        public ActionResult Create(SalesPromotionModel.SalesPromotionModelShow item)
        {
            try
            {
                string deadline = item.Deadline.ToString("").Substring(0,9) + " 下午 03:59:59";
                item.Deadline = DateTime.Parse(deadline);
                if (data.Insert(item) <= 0)
                {
                    TempData["err"] = "SalesPromotion_0";
                    return RedirectToAction("Index");
                }
            }
            catch { TempData["err"] = "SalesPromotion_1"; }
            return RedirectToAction("Index");
        }

        [MyAuthorize(function = "促銷碼活動管理")]
        public ActionResult Edit(int id = 0, int p = 1)
        {
            ViewBag.Title = "促銷碼活動管理 > 編輯";
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
        [MyAuthorize(function = "促銷碼活動管理")]
        public ActionResult Edit(SalesPromotionModel.SalesPromotionModelShow item, int p = 1)
        {
            {
                s26webDataContext db = new s26webDataContext();
                var old = db.SalesPromotion.FirstOrDefault(f => f.Id == item.Id);
                db.Connection.Close();
                if (data.Update(item) <= 0)
                {
                    TempData["err"] = "SalesPromotion_5";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index", new { p = p });
        }

        [MyAuthorize(function = "促銷碼活動管理")]
        public ActionResult Delete(int Id, int p = 1)
        {
            data.Delete(Id);

            return RedirectToAction("Index", new { p = p });
        }

    }
}
