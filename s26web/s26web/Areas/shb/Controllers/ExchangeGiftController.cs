using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Areas.shb.Models;
using s26web.Models;

namespace s26web.Areas.shb.Controllers
{
    public class ExchangeGiftController : Controller
    {
        ExchangeGiftModel data = new ExchangeGiftModel();

        [MyAuthorize(function = "贈品兌換管理")]
        public ActionResult Index(string Update_Id = "", string Update_State = "", string keyword = "", string States = "", DateTime? time_start = null, DateTime? time_end = null, int p = 1, int show_number = 10)
        {
            try
            {
                ViewBag.Title = "贈品兌換管理";

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
                if (States != null && !(States.Equals("")))
                {
                    data.StatesSelect = States;
                    ViewData["States"] = States;
                }

                ViewData["p"] = p;
                ViewData["StateOption"] = data.Get_ExchangeGiftStatesOption();
                ViewData["page"] = data.Get_PageIO(p, show_number);

                return View(data.Get_Data(p, show_number));
            }
            catch
            {
                return View();
            }
        }

        [MyAuthorize(function = "贈品兌換管理")]
        public ActionResult MemberCheck(string mobile = "")
        {
            ViewBag.Title = "輸入會員手機";

            if (!(mobile.Equals("")))
            {
                int temp = data.Get_MemberInfo(mobile);

                if (temp != -1)
                {
                    return RedirectToAction("Create", "ExchangeGift", new { vol_mobile = mobile, id = temp });
                }
                else
                {
                    TempData["err"] = "找不到此會員資料，請重新確認";
                }
            }
            return View();
        }

        [MyAuthorize(function = "贈品兌換管理")]
        public ActionResult Create(string vol_mobile, int id)
        {
            ViewBag.Title = "新增贈品兌換";

            if (vol_mobile == null || vol_mobile.Equals(""))
            {
                TempData["err"] = "會員資料代入錯誤";
                return RedirectToAction("MemberCheck", "ExchangeGift");
            }

            ViewData["Vol_Mobile"] = vol_mobile;
            ViewData["Vol_Id"] = id.ToString();
            ViewData["ExchangeGiftOption"] = ExchangeGiftModel.Get_ExchangeGiftOption();
            return View();
        }

        [MyAuthorize(function = "贈品兌換管理")]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Create(ExchangeGiftModel.InsertExchangeGift item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.UpdateUserId = Method.Get_UserId_Admin(Request.Cookies, Session);
                    data.Insert_Score(item);
                }
                else
                {
                    TempData["err"] = "Gift_10";
                }
            }
            catch
            {
                TempData["err"] = "Gift_11";
            }
            return RedirectToAction("Index", "ExchangeGift");
        }

        [MyAuthorize(function = "贈品兌換管理")]
        public ActionResult Export3(string keyword = "", string States = "", DateTime? time_start = null, DateTime? time_end = null)
        {
            if (time_start != null && !(time_start.ToString().Equals("")))
            {
                data.time_start = time_start.Value;
            }
            if (time_end != null && !(time_end.ToString().Equals("")))
            {
                data.time_end = time_end.Value;
            }
            if (States != null && !(States.Equals("")))
            {
                data.StatesSelect = States;
            }
            return File(data.Get_ExcelData2(data.Get_All_Export()), "application/vnd.ms-excel", SiteOptionModel.Get_Title() + "資料匯出.xls");
        }

        [MyAuthorize(function = "贈品兌換管理")]
        public ActionResult UpdateScore(string Update_Id = "", string Update_State = "", int UpdateUser = 0)
        {
            if (Update_Id != null && !(Update_Id.Equals("")) && Update_State != null && !(Update_State.Equals("")))
            {
                int user_id = Method.Get_UserId_Admin(Request.Cookies, Session);
                data.ScoreUpdate(Update_Id, Update_State, user_id);
            }
            return RedirectToAction("Index", "ExchangeGift");
        }
    }
}
