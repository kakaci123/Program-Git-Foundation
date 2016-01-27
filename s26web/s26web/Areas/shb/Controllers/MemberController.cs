using s26web.Areas.shb.Models;
using s26web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace s26web.Areas.shb.Controllers
{
    [MyAuthorize(function = "CheckSession")]
    public class MemberController : Controller
    {
        MemberModel data = new MemberModel();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (s26web.Models.Method.Is_Login_Admin(Request.Cookies))
            {
                if (returnUrl == "")
                {
                    returnUrl = s26web.Models.Method.RootPath + "/shb/";
                }
                if (TempData["err"] != null)
                {
                    return View();
                }
                return RedirectToLocal(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(MemberModel.LoginModel login, string returnUrl, int time_offset)
        {
            if (ModelState.IsValid)
            {
                var account = data.Get_One(login.UserName, login.Password);
                if (account == null)
                {
                    LoginRecordModel.Login_Record(login.UserName, Request, false);
                    TempData["err"] = "使用者名稱或密碼不正確";
                    return View();
                }
                Session.RemoveAll();
                var competence = data.Get_Competence(account.Level);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    account.Id == 0 ? s26web.Models.Method.CookieVersion_Admin : s26web.Models.Method.CookieVersion_Home,
                    login.UserName,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(60),
                    login.RememberMe,
                    account.Name + "," + account.Password,
                    FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                Response.Cookies.Add(new HttpCookie(s26web.Models.Method.CookieName_Admin, encTicket));
                Response.Cookies[s26web.Models.Method.CookieName_Admin].Path = "/";
                Session.Add(s26web.Models.Method.SessionUserId_Admin, account.Id);
                Session.Add(s26web.Models.Method.SessionUserAccount_Admin, account.Account_Phone);
                Session.Add(s26web.Models.Method.SessionUserName_Admin, account.Name);
                Session.Add(s26web.Models.Method.SessionLevel_Admin, account.Level);
                Session.Add(s26web.Models.Method.SessionComptpence, competence);
                Session.Add(s26web.Models.Method.SessionLevelName_Admin, account.Level_Name);
                Session.Add(s26web.Models.Method.SessionUserAccount_Admin, account.Enable);
                LoginRecordModel.Login_Record(login.UserName, Request, true);
                return RedirectToLocal(returnUrl);
            }
            if (login.UserName != null)
            {
                LoginRecordModel.Login_Record(login.UserName, Request, false);
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            if (Request.Cookies.AllKeys.Contains(s26web.Models.Method.CookieName_Admin))
            {
                Session.RemoveAll();
                HttpCookie hc = Request.Cookies[s26web.Models.Method.CookieName_Admin];
                hc.Expires = DateTime.UtcNow.AddDays(-30);
                Response.AppendCookie(hc);
            }
            return RedirectToAction("Index", "Member");
        }

        [AllowAnonymous]
        public bool LogOff2()
        {
            Session.RemoveAll();
            return true;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Member");
            }
        }

        //
        // GET: /shb/Memeber/

        [MyAuthorize(function = "帳號管理")]
        public ActionResult Index(int[] user_level, int enable = 0, int disable = 0, string keyword = "", int p = 1, int show_number = 10)
        {
            try
            {
                string get = s26web.Models.Method.Get_URLGet("keyword", keyword);
                data.Search_Enable = false;
                if (user_level != null)
                {
                    data.Level.AddRange(user_level);
                    foreach (var i in user_level)
                    {
                        get += s26web.Models.Method.Get_URLGet("user_level", i.ToString());
                    }
                }
                if (enable == 1 && disable == 1)
                {
                    data.Search_Enable = false;
                }
                else
                {
                    if (enable == 1)
                    {
                        data.Search_Enable = true;
                        data.Enable = true;
                        get += s26web.Models.Method.Get_URLGet("enable", enable.ToString());
                    }
                    if (disable == 1)
                    {
                        data.Search_Enable = true;
                        data.Enable = false;
                        get += s26web.Models.Method.Get_URLGet("disable", disable.ToString());
                    }
                }
                ViewData["user_level"] = data.Get_UserLevel();
                data.Keyword = keyword;
                ViewData["p"] = p;
                ViewData["page"] = data.Get_Page(p, show_number);
                ViewData["keyword"] = keyword;
                ViewData["user_level_value"] = user_level;
                ViewData["enable"] = enable;
                ViewData["disable"] = disable;
                ViewData["get"] = get;

                return View(data.Get_Data(p, show_number));
            }
            catch
            {
                TempData["err"] = "Member_0";
                return View();
            }
        }

        public ActionResult Create()
        {
            ViewData["user_level"] = Get_UserLevel();
            ViewBag.Title = "新增帳號";
            return View();
        }

        [MyAuthorize(Com = s26web.Areas.shb.Models.MyAuthorizeAttribute.Competence.Insert, function = "帳號管理")]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Create(MemberModel.MemberShow item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (item.Level == 0)
                    {
                        TempData["err"] = "Member_0，請選擇群組權限";
                    }
                    else if (Regex.IsMatch(item.Account_Phone, @"[\W_]+"))
                    {
                        TempData["err"] = "Member_1，帳號請勿使用特殊字元";
                    }
                    else
                    {
                        if (data.Insert(item) <= 0)
                        {
                            TempData["err"] = "Member_2，帳號新增失敗(請避免帳號重複)";
                        }
                    }
                }
                else
                {
                    TempData["err"] = "Member_3，帳號新增失敗(請輸入完整資訊)";
                }
            }
            catch { TempData["err"] = "Member_4，帳號新增失敗(請輸入完整資訊)"; }

            if (TempData["err"] != null)
            {
                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }

        [MyAuthorize(function = "帳號管理")]
        public ActionResult Edit(int id = 0, int p = 1)
        {
            ViewData["user_level"] = Get_UserLevel();
            ViewData["p"] = p;
            var item = data.Get_One(id);
            if (item == null)
            {
                return RedirectToAction("Create");
            }
            return View(item);
        }

        [MyAuthorize(function = "帳號管理")]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Edit(MemberModel.MemberShow aItem, int p = 1)
        {
            if (ModelState.IsValid)
            {
                var item = data.Get_One(aItem.Id);

                if (item == null)
                {
                    TempData["err"] = "Member_4";
                    return RedirectToAction("Index");
                }

                int result = data.Update(aItem);

                if (result <= 0)
                {
                    TempData["err"] = "Member_5";
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index", new { p = p });
        }

        [MyAuthorize(function = "帳號管理")]
        public ActionResult Delete(int item, int p = 1)
        {
            try
            {
                data.Delete(item);
            }
            catch { TempData["err"] = "Member_6"; }

            return RedirectToAction("Index", new { p = p });
        }

        private List<SelectListItem> Get_UserLevel()
        {
            var user_level = data.Get_UserLevel();
            List<SelectListItem> temp = new List<SelectListItem>();
            temp.Add(new SelectListItem
            {
                Selected = false,
                Text = "請選擇",
                Value = "0"
            });
            temp.AddRange(user_level.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = false
            }).ToList());
            return temp;
        }

        [MyAuthorize(function = "帳號管理")]
        public ActionResult Category(int id = 0, string keyword = "", int p = 1, int show_number = 10)
        {
            var result = data.Get_UserLevel(keyword, p, 10).Select(s => new MemberModel.UserLevelShow
            {
                Id = s.Id,
                Name = s.Name,
                //Format type [2005-11-5 14:23:23]
                CreateTime = s.CreateTime.AddHours(8)
            }).ToList();

            ViewData["page"] = s26web.Models.Method.Get_Page(data.Get_UserLevel_Count(keyword), p, show_number);
            ViewData["keyword"] = keyword;
            ViewData["number"] = show_number;
            ViewData["get"] = s26web.Models.Method.Get_URLGet("keyword", keyword);
            return View(result);
        }

        [MyAuthorize(function = "帳號管理")]
        public ActionResult CategoryCreate()
        {
            var item = new MemberModel.UserLevelShow
            {
                Competence = data.Initial_Competence(),
                CompetenceName = data.Get_CompetenceName()
            };
            return View(item);
        }

        [MyAuthorize(function = "帳號管理")]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult CategoryCreate(MemberModel.UserLevelShow item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    data.Insert_UserLevel(item);
                }
                else
                {
                    TempData["err"] = "Member_10";
                }
            }
            catch { TempData["err"] = "Member_11"; }
            return RedirectToAction("Category");
        }

        [MyAuthorize(function = "帳號管理")]
        public ActionResult CategoryEdit(int id = 0, int p = 1)
        {
            ViewData["p"] = p;
            var item = data.Get_UserLevel_One(id);
            if (item == null)
            {
                return RedirectToAction("CategoryCreate");
            }
            return View(item);
        }

        [MyAuthorize(function = "帳號管理")]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult CategoryEdit(MemberModel.UserLevelShow aItem, int p = 1)
        {
            if (ModelState.IsValid)
            {
                var item = data.Get_UserLevel_One(aItem.Id);
                if (item == null)
                {
                    TempData["err"] = "Member_12";
                    return RedirectToAction("Category");
                }
                int result = data.Update_UserLevel(aItem);
                if (result < 0)
                {
                    TempData["err"] = "Member_13";
                    return RedirectToAction("Category");
                }
            }
            return RedirectToAction("Category", new { p = p });
        }

        [MyAuthorize(function = "帳號管理")]
        public ActionResult CategoryDelete(int item, int p = 1)
        {
            try
            {
                data.Delete_UserLevel(item);
            }
            catch { TempData["err"] = "Member_14"; }

            return RedirectToAction("Category", new { p = p });
        }
    }
}