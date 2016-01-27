using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Areas.shb.Models;
using s26web.Models;
namespace s26web.Areas.shb.Controllers
{
    public class OrderController : Controller
    {
        OrdersModel data = new OrdersModel();

        [MyAuthorize(function = "訂單管理")]
        public ActionResult Index(string keyword = "", string OrdersStates = "", string Product = "", DateTime? time_start = null, DateTime? time_end = null, int p = 1, int show_number = 10)
        {
            try
            {
                ViewBag.Title = "問卷內容查看";
                if (keyword != null && !(keyword.Equals("")))
                {
                    data.Keyword = keyword;
                    ViewData["keyword"] = keyword;
                }
                if (time_start != null && !(time_start.ToString().Equals("")))
                {
                    data.time_start = time_start.Value;
                    ViewData["time_start"] = time_start;
                }
                if (time_end != null && !(time_end.ToString().Equals("")))
                {
                    data.time_end = time_end.Value;
                    ViewData["time_end"] = time_end;
                }
                if (OrdersStates != null && !(OrdersStates.Equals("")))
                {
                    data.StatesSelect = OrdersStates;
                    ViewData["orderselect"] = OrdersStates;
                }
                if (Product != null && !(Product.Equals("")))
                {
                    data.ProductSelect = Product;
                    ViewData["Product"] = Product;
                }
                ViewData["p"] = p;
                ViewData["page"] = data.Get_PageIO(p, show_number);
                return View(data.Get_Data(p, show_number));
            }
            catch
            {
                return View();
            }
        }
        [MyAuthorize(function = "訂單管理")]
        public ActionResult OrderView(string id = "", int p = 1)
        {
            try
            {
                ViewBag.Title = "訂單詳細內容";
                var item = data.Get_Detail(id);
                if (item == null) { return RedirectToAction("Index"); }
                ViewData["OrdersProductOption"] = data.Get_OrdersProductOption();
                ViewData["OrdersStatesOption"] = data.Get_OrdersStatesOption();
                return View(item);
            }
            catch
            {
                return View();
            }
        }

        [MyAuthorize(function = "訂單管理")]
        public ActionResult UpdateOrder(OrdersModel.OrdersModelShow data)
        {
            try
            {
                int user_id = Method.Get_UserId_Admin(Request.Cookies, Session);
                data.ScoreUpdate(data,user_id);
                return RedirectToAction("Index");
            }
            catch
            {

                return RedirectToAction("Index");
            }
        }


        [MyAuthorize(function = "訂單管理")]
        public ActionResult Export3(string keyword = "", string ProductSelect = "", DateTime? time_start = null, DateTime? time_end = null)
        {
            if (time_start != null && !(time_start.ToString().Equals("")))
            {
                data.time_start = time_start.Value;
            }
            if (time_end != null && !(time_end.ToString().Equals("")))
            {
                data.time_end = time_end.Value;
            }
            if (ProductSelect != null && !(ProductSelect.Equals("")))
            {
                data.ProductSelect = ProductSelect;
            }
            return File(data.Get_ExcelData2(data.Get_All_Export()), "application/vnd.ms-excel", SiteOptionModel.Get_Title() + "資料匯出.xls");
        }
    }
}
