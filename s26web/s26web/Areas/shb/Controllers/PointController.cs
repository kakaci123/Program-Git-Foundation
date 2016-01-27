using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{
    public class PointController : Controller
    {
        PointModel data = new PointModel();

        [MyAuthorize(function = "點數管理")]
        public ActionResult Index(int[] cid, string keyword = "", int result = 0, DateTime? SendTime_start = null, DateTime? SendTime_end = null,
            int CategorySelectList = 0,int p = 1, int show_number = 10)
        {
            try
            {
                string get = s26web.Models.Method.Get_URLGet("keyword", keyword);
                ViewBag.Title = "產品介紹管理";
                //ViewData["fun_id"] = fun_id;
                var categories = data.Get_Category();
                SelectList selectlist = new SelectList(categories, "Id", "Name");

                if (result >= 0)
                {
                    get += s26web.Models.Method.Get_URLGet("result", result.ToString());
                    ViewData["result"] = result;
                    data.result = result;
                }
                if (CategorySelectList >= 0)
                {
                    get += s26web.Models.Method.Get_URLGet("CategorySelectList", CategorySelectList.ToString());
                    ViewBag.CategorySelectList = selectlist;
                    data.CategorySelectList = CategorySelectList;
                }
                if (SendTime_start != null)
                {
                    get += s26web.Models.Method.Get_URLGet("SendTime_start", SendTime_start.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    ViewData["SendTime_start"] = SendTime_start;
                    data.SendTime_start = SendTime_start.Value;
                }
                if (SendTime_end != null)
                {
                    get += s26web.Models.Method.Get_URLGet("SendTime_end", SendTime_end.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    ViewData["SendTime_end"] = SendTime_end;
                    data.SendTime_end = SendTime_end.Value;
                }
                data.Keyword = keyword;
                ViewData["path"] = "Product";
                ViewData["p"] = p;
                ViewData["page"] = data.Get_Page(p, show_number);
                ViewData["keyword"] = keyword;
                return View(data.Get_Data(p, show_number));
            }
            catch
            {
                TempData["err"] = "Point_0";
                return View("Index");
            }
        }
        [MyAuthorize(function = "點數管理")]
        public ActionResult HandPoint()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Create(PointModel.PointModelShow item, string phone)
        {
            s26webDataContext db = new s26webDataContext();
            List<string> mobile_list = new List<string>();
            mobile_list = phone.Split(',').ToList();

            int user_id = Method.Get_UserId_Admin(Request.Cookies, Session);
            item.UId = user_id;
            item.Category = 2;
            item.Result = true;
            foreach (var i in mobile_list)
            {
                item.Mobile = i;
                if (data.Insert(item) <= 0)
                {
                    TempData["err"] = "";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        [MyAuthorize(function = "點數管理")]
        public ActionResult Export_main(string keyword = "", int result = 0, DateTime? SendTime_start = null, DateTime? SendTime_end = null, int CategorySelectList = 0)
        {
            //---------------匯出條件搜索---------------//

            data.Keyword = keyword;

            if (SendTime_start != null && SendTime_end != null)
            {
                data.SendTime_start = SendTime_start;
                data.SendTime_end = SendTime_end;
            }
            if (CategorySelectList > 0)
            {
                data.CategorySelectList = CategorySelectList;
            }
            if (result > 0)
            {
                data.result = result;
            }

            //---------------匯出條件搜索end---------------//
            return File(data.Get_ExcelData_main(PointModel.Get_Export(data)), "application/vnd.ms-excel", SiteOptionModel.Get_Title() + "_點數管理資料報表.xls");
        }

        public ActionResult GetPoint(PointModel.PointModelShow item)
        {
            item.Vid = Method.Get_UserId(Request.Cookies, Session);
            item.Mobile = Method.Get_Account(Request.Cookies, Session);
            item.Category = 33;
            item.Point = 100;
            item.Result = true;
            item.UId = 0;
            data.Insert(item);
            return View();
        }
    }
}
