using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{
    public class ProductController : Controller
    {
        ProductModel data = new ProductModel();

        [MyAuthorize(function = "產品內容管理")]
        public ActionResult Index(int[] cid, string keyword = "", string sort = "",
            int p = 1, int show_number = 10)
        {
            try
            {
                ViewBag.Title = "產品內容管理";
                //ViewData["fun_id"] = fun_id;
                ViewData["path"] = "Product";
                ViewData["p"] = p;
                ViewData["page"] = data.Get_Page(p, show_number);
                return View(data.Get_Data());
            }
            catch
            {
                TempData["err"] = "Product_0";
                return View("~/Areas/shb/Views/Product/Index.cshtml");
            }
        }

        [MyAuthorize(function = "產品內容管理")]
        public ActionResult Edit(int id = 0)
        {
            var item = data.Get_One(id);
            if (item == null)
            {
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "產品內容管理")]
        public ActionResult Edit(ProductModel.ProductModelShow item, HttpPostedFileBase file = null, HttpPostedFileBase ContentPic = null)
        {
            if (ModelState.IsValid)
            {
                if (item.Content == null)
                {
                    item.Content = "";
                }
                int vid = Method.Get_UserId(Request.Cookies, Session);
                if (data.Update(item, 0, file, "Product/" + vid, Server, ContentPic) > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Title = "Error";
            TempData["err"] = 2;
            return View("Index", item);
        }
    }
}
