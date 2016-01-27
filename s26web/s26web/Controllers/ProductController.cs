using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Controllers
{
    public class ProductController : Controller
    {
        ProductModel data = new ProductModel();

        // GET: /Product/
        public ActionResult Index()
        {
            try
            {
                return View(data.Get_DisplayData());
            }
            catch
            {
                TempData["err"] = "Product_0";
                return View();
            }
        }
        public ActionResult View(int id)
        {
            var product = data.Get_One(id);
            return View(product);
        }
    }
}
