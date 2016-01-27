using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using System.Collections.Generic;
using s26web.Areas.shb.Models;

namespace s26web.Areas.shb.Controllers
{

    public class VolunteersController : Controller
    {
        VolunteersModel data = new VolunteersModel();
        [MyAuthorize(function = "會員管理")]
        public ActionResult Index(string keyword = "", DateTime? create_time_start = null, DateTime? create_time_end = null, int search_time_offset = 0, 
            int NowBrand = 0, int status = 0, bool NotchangeBrand = false, int p = 1, int show_number = 10, int CategorySelectList = 0)
        {
            var categories = data.Get_eCRM();
            SelectList selectlist = new SelectList(categories, "Id", "UserName");

            string get = s26web.Models.Method.Get_URLGet("keyword", keyword);
            data.Keyword = keyword;
            if (create_time_start != null)
            {
                get += s26web.Models.Method.Get_URLGet("create_time_start", create_time_start.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                ViewData["create_time_start"] = create_time_start;
                data.create_time_start = create_time_start.Value;
            }
            if (create_time_end != null)
            {
                get += s26web.Models.Method.Get_URLGet("create_time_end", create_time_end.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                ViewData["create_time_end"] = create_time_end;
                data.create_time_end = create_time_end.Value;
            }
            if (status >= 0)
            {
                get += s26web.Models.Method.Get_URLGet("status", status.ToString());
                ViewData["status"] = status;
                data.status = status;
            }
            if (NowBrand >= 0)
            {
                get += s26web.Models.Method.Get_URLGet("NowBrand", NowBrand.ToString());
                ViewData["NowBrand"] = NowBrand;
                data.NowBrand = NowBrand;
            }
            if (NotchangeBrand)
            {
                get += s26web.Models.Method.Get_URLGet("NotchangeBrand", NotchangeBrand.ToString());
                ViewData["NotchangeBrand"] = NotchangeBrand;
                data.NotchangeBrand = NotchangeBrand;
            }
            if (CategorySelectList >= 0)
            {
                get += s26web.Models.Method.Get_URLGet("CategorySelectList", CategorySelectList.ToString());
                ViewBag.CategorySelectList = selectlist;
                data.CategorySelectList = CategorySelectList;
            }
            ViewData["p"] = p;
            ViewData["page"] = data.Get_Page(p, show_number);
            ViewData["keyword"] = keyword;
            List<s26web.Models.VolunteersModel.VolunteersShow> list = data.Get_Data(p, show_number);
            return View(list);
        }

        [MyAuthorize(function = "會員管理")]
        public ActionResult Edit(int id = 0, int p = 1)
        {
            ViewBag.Title = "會員管理 > 編輯";
            ViewBag.c_title = "會員管理資料";
            ViewData["p"] = p;
            var item = data.Get_One(id);
            
            if (item == null)
            {
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [MyAuthorize(function = "會員管理")]
        public ActionResult Edit(VolunteersModel.VolunteersShowEdit item, int p = 1)
        {
            //if (ModelState.IsValid)
            {
                s26webDataContext db = new s26webDataContext();
                var old = db.Volunteers.FirstOrDefault(f => f.Id == item.Id);
                db.Connection.Close();

                int user_id = Method.Get_UserId_Admin(Request.Cookies, Session);
                item.LastMemberId = user_id;

                if (data.Update(item) <= 0)
                {
                    TempData["err"] = "Volunteers_5";
                    return RedirectToAction("Index");
                }
                //if (item.SendSMS)
                //{
                //    IntroductionModel intro = new IntroductionModel();
                //    var results = intro.Get_One(38);
                //    var result = MobileAPIModel.sendToServer(38, item.VolunteersMobile, null, 0, "", item.Id);
                //}
            }
            return RedirectToAction("Index", new { p = p });
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult Create()
        {
            //ViewData["config"] = data.config;
            return View();
        }
        //[MyAuthorize(Com = Competence.Insert, function = "會員管理")]
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult Create(VolunteersModel.VolunteersShow item, HttpPostedFileBase file = null)
        {
            try
            {
                    int vid = Method.Get_UserId(Request.Cookies, Session);
                    item.LastMemberId = vid;
                        if (data.Insert(item, file, "Manual/" + vid, Server) <= 0)
                        {
                            TempData["err"] = "Volunteers_2，帳號新增失敗(請避免帳號重複)";
                            return RedirectToAction("Index");
                        }
            }
            catch { TempData["err"] = "Volunteers_4，帳號新增失敗(請輸入完整資訊)"; }
            return RedirectToAction("Index");
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult Export_main(string keyword = "", DateTime? create_time_start = null, DateTime? create_time_end = null, int search_time_offset = 0,
            int NowBrand = 0, int status = 0, bool NotchangeBrand = false, int CategorySelectList = 0)
        {
            //---------------匯出條件搜索---------------//

            data.Keyword = keyword;

            if (status > 0)
            {
                data.status = status;
            }
            if (NowBrand > 0)
            {
                data.NowBrand = NowBrand;
            }
            if (create_time_start != null)
            {
                data.create_time_start = create_time_start.Value.AddHours(8);
            }
            if (create_time_end != null)
            {
                data.create_time_end = create_time_end.Value.AddHours(8);
            }
            if (NotchangeBrand)
            {
                data.NotchangeBrand = NotchangeBrand;
            }
            if (CategorySelectList > 0)
            {
                data.CategorySelectList = CategorySelectList;
            }

            //---------------匯出條件搜索end---------------//
            return File(data.Get_ExcelData_main(VolunteersModel.Get_Export(data)), "application/vnd.ms-excel", SiteOptionModel.Get_Title() + "_會員資料報表.xls");
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult SMSConfirm()
        {
            IntroductionModel intro = new IntroductionModel();
            var result = intro.Get_One(1);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            return View("~/Areas/shb/Views/Introduction/Introduction.cshtml", result);
        }

        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult SMSConfirm(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                IntroductionModel intro = new IntroductionModel();
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (intro.Set(item) > 0)
                {
                    return RedirectToAction("SMSConfirm");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/SMSConfirm.cshtml", item);
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult SMSPassword()
        {
            IntroductionModel intro = new IntroductionModel();
            var result = intro.Get_One(2);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            return View("~/Areas/shb/Views/Introduction/Introduction.cshtml", result);
        }

        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult SMSPassword(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                IntroductionModel intro = new IntroductionModel();
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (intro.Set(item) > 0)
                {
                    return RedirectToAction("SMSPassword");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/SMSForgotpwd.cshtml", item);
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult SMSFailed1()
        {
            IntroductionModel intro = new IntroductionModel();
            var result = intro.Get_One(3);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            return View("~/Areas/shb/Views/Introduction/Introduction.cshtml", result);
        }

        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult SMSFailed1(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                IntroductionModel intro = new IntroductionModel();
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (intro.Set(item) > 0)
                {
                    return RedirectToAction("SMSFailed1");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/SMSForgotpwd.cshtml", item);
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult OrderEmail()
        {
            IntroductionModel intro = new IntroductionModel();
            var result = intro.Get_One(4);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", result);
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult OrderEmail(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                IntroductionModel intro = new IntroductionModel();
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (intro.Set(item) > 0)
                {
                    return RedirectToAction("OrderEmail");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", item);
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult DeliveryEmail()
        {
            IntroductionModel intro = new IntroductionModel();
            var result = intro.Get_One(5);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", result);
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult DeliveryEmail(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                IntroductionModel intro = new IntroductionModel();
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (intro.Set(item) > 0)
                {
                    return RedirectToAction("DeliveryEmail");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", item);
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult DeliveryGiftEmail()
        {
            IntroductionModel intro = new IntroductionModel();
            var result = intro.Get_One(6);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", result);
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult DeliveryGiftEmail(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                IntroductionModel intro = new IntroductionModel();
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (intro.Set(item) > 0)
                {
                    return RedirectToAction("DeliveryGiftEmail");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", item);
        }
        [MyAuthorize(function = "會員管理")]
        public ActionResult CountEmail()
        {
            IntroductionModel intro = new IntroductionModel();
            var result = intro.Get_One(7);
            if (result == null)
            {
                ViewBag.Title = "Error";
                TempData["err"] = 1;
                return View();
            }
            ViewBag.Title = result.Title;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", result);
        }
        [HttpPost]
        [ValidateInput(true)]
        [MyAuthorize(function = "會員管理")]
        public ActionResult CountEmail(IntroductionModel.IntroductionModelShow item)
        {
            if (ModelState.IsValid)
            {
                IntroductionModel intro = new IntroductionModel();
                if (item.Content == null)
                {
                    item.Content = "";
                }
                if (intro.Set(item) > 0)
                {
                    return RedirectToAction("CountEmail");
                }
            }
            ViewBag.Title = "Error";
            //Sanitizer.GetSafeHtmlFragment();
            TempData["err"] = 2;
            return View("~/Areas/shb/Views/Introduction/Introduction3.cshtml", item);
        }

        [MyAuthorize(function = "會員管理")]
        public ActionResult Delete(int item, int p = 1)
        {
            try
            {
                data.Delete(item);
            }
            catch { TempData["err"] = "Volunteer_6"; }

            return RedirectToAction("Index", new { p = p });
        }

        [MyAuthorize(function = "會員管理")]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Delete(int[] item)
        {
            try
            {
                data.Delete(item);
            }
            catch { TempData["err"] = "Volunteer_7"; }

            return RedirectToAction("Index");
        }
    }
}
