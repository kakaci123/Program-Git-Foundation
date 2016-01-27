using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using CaptchaMvc.Attributes;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc.Interface;
using Newtonsoft.Json;
using System.Collections.Specialized;
using s26web.Models;
//using WebMatrix.WebData;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Text;
using s26web.Areas.shb.Models;

namespace s26web.Controllers
{
    public class LoginController : Controller
    {
        VolunteersModel data = new VolunteersModel();

        public ActionResult Index(string ReturnUrl)
        {
            //return View();


            if (ReturnUrl == "" || ReturnUrl == null)
            {
                ReturnUrl = Method.RootPath + "/Login/Index";
            }

            if (Method.Is_Login(Request.Cookies))
            {
                return RedirectToLocal(ReturnUrl);
            }
            else
            {
                ViewBag.ReturnUrl = ReturnUrl;
                if (Session["Login"] == null)
                    SignOut();
                return View();
            }
        }

        private void SignOut()
        {
            if (Request.Cookies.AllKeys.Contains(s26web.Models.Method.CookieName_Home))
            {
                Session[Method.SessionUserId_Home] = null;
                Session[Method.SessionUserAccount_Home] = null;
                Session[Method.SessionUserName_Home] = null;
                Session[Method.SessionUserEmail_Home] = null;
                //Session.RemoveAll();
                //FormsAuthentication.SignOut();
                HttpCookie hc = Request.Cookies[s26web.Models.Method.CookieName_Home];
                hc.Expires = DateTime.UtcNow.AddDays(-30);
                Response.AppendCookie(hc);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VolunteersModel.LoginModel item, int year, int month, int date, string returnurl = "")
        {
            item.BabaBirthday = new DateTime(year, month, date);

            if (ModelState.IsValid)
            {
                if (Session["Login"] == null)
                    Session["Login"] = 0;
                else
                {
                    if ((int)Session["Login"] < 3)
                        Session["Login"] = Convert.ToInt32(Session["Login"]) + 1;
                }

                if ((int)Session["Login"] < 3)
                {
                    if (returnurl == null || returnurl == "")
                    {
                        returnurl = Method.RootPath + "/Login/Index";
                    }
                    var accunt = data.Get_One_Mobile(item.Mobile);
                    if (accunt != null)
                    {
                        //if (accunt.FaceBookId != null & data.Get_HashPasswords(item.Password) == data.Get_PWByMobile(item.UserName))
                        //{
                        //    TempData["err"] = "此帳號是使用FB註冊，請使用FB帳號登入。";
                        //    ViewBag.ReturnUrl = returnurl;

                        //    return View(item);
                        //}

                        //if (accunt.Status == 1 & data.Get_HashPassword(item.Password) == data.Get_PWByMobile(item.Mobile))
                        //{
                        //    TempData["err"] = "帳號尚未完成審核，請等候惠氏VIP服務專員與您聯繫。";
                        //    ViewBag.ReturnUrl = returnurl;

                        //    return View(item);
                        //    //return RedirectToAction("Error");
                        //}
                        //else if (accunt.Password != data.Get_HashPassword(item.Password))
                        //{
                        //    TempData["err"] = "手機號碼或密碼錯誤。"/*Session["Login"]*/;

                        //    return View(item);
                        //}
                        //else if (accunt.Status == 3 & data.Get_HashPassword(item.Password) == data.Get_PWByMobile(item.Mobile))
                        //{
                        //    TempData["err"] = "帳號審核失敗，請重新註冊。";
                        //    ViewBag.ReturnUrl = returnurl;

                        //    return RedirectToAction("Register", "Login", new { error = "Enable" });
                        //}
                        //else
                        //{
                        //    LoginAccount(accunt);
                        //    returnurl = Method.RootPath + "/Home/Index";
                        //    return RedirectToLocal(returnurl);
                        //}

                        //20150327 改為用寶寶生日當作密碼
                        if (accunt.Status == 1 & accunt.BabyBirthday == item.BabaBirthday)
                        {
                            TempData["err"] = "帳號尚未完成審核，請等候惠氏VIP服務專員與您聯繫。";
                            ViewBag.ReturnUrl = returnurl;

                            return View(item);
                            //return RedirectToAction("Error");
                        }
                        else if (accunt.BabyBirthday != item.BabaBirthday)
                        {
                            TempData["err"] = "手機號碼或密碼錯誤。"/*Session["Login"]*/;

                            return View(item);
                        }
                        else if (accunt.Status == 3 & accunt.BabyBirthday == item.BabaBirthday)
                        {
                            TempData["err"] = "帳號審核失敗，請重新註冊。";
                            ViewBag.ReturnUrl = returnurl;

                            return RedirectToAction("Register", "Login", new { error = "Enable" });
                        }
                        else
                        {
                            LoginAccount(accunt);
                            returnurl = Method.RootPath + "/Home/Index";
                            return RedirectToLocal(returnurl);
                        }
                    }
                    else
                    {
                        TempData["err"] = "手機號碼或密碼錯誤。"/*Session["Login"]*/;
                        ViewBag.ReturnUrl = returnurl;

                    }
                    return RedirectToLocal(returnurl);
                }
                else
                {
                    TempData["err"] = "您登入失敗太多次，帳號已被鎖定30分鐘，請稍後再試。";
                    ViewBag.ReturnUrl = returnurl;

                    return View(item);
                }


            }
            if (item.BabaBirthday == null & item.Mobile == null)
            {
                TempData["err"] = "請輸入手機號碼及密碼";
            }
            else if (item.Mobile == null)
            {
                TempData["err"] = "請輸入手機號碼";
            }
            else 
            {
                TempData["err"] = "請輸入密碼";
            }

            ViewBag.ReturnUrl = returnurl;
            return View(item);
        }

        private void LoginAccount(Volunteers item, bool RememberMe = false)
        {
            //Session.RemoveAll();
            Session[Method.SessionUserId_Home] = null;
            Session[Method.SessionUserAccount_Home] = null;
            Session[Method.SessionUserName_Home] = null;
            Session[Method.SessionUserEmail_Home] = null;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                s26web.Models.Method.CookieVersion_Home,
                item.Mobile,//item.Account,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(60),
                RememberMe,
                item.Name + "," + item.Password,
                FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            Response.Cookies.Add(new HttpCookie(s26web.Models.Method.CookieName_Home, encTicket));
            Response.Cookies[s26web.Models.Method.CookieName_Home].Path = "/";
            Session.Add(Method.SessionUserId_Home, item.Id);
            Session.Add(Method.SessionUserAccount_Home, item.Mobile);
            Session.Add(Method.SessionUserName_Home, item.Name);
            Session.Add(Method.SessionUserEmail_Home, item.Email);
        }

        public ActionResult LogOff()
        {
            SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(VolunteersModel.RegisterModel item, int year, int month, int date, HttpPostedFileBase id_image = null)
        {
            try
            {
                int vid = Method.Get_UserId(Request.Cookies, Session);
                data.Insert(item, id_image, "Manual/" + vid, Server, year, month, date);
            }
            catch { TempData["err"] = "Login，帳號註冊失敗(請輸入完整資訊)"; }

            return RedirectToAction("member_complete", "Login");
        }

        public ActionResult member_complete()
        {
            return View();
        }


        public static bool IsNumericOrLetter(string input)
        {
            return Regex.IsMatch(input, "^[A-Za-z0-9]+$");
        }

        public ActionResult forgot_Sign_in()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult forgot_Sign_in(string username, string mobile, string CaptchaInputText, bool IsMobile = true)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (mobile != null && mobile != "" && username != null && username != "")
                {
                    s26webDataContext db = new s26webDataContext();
                    var user = db.Volunteers.FirstOrDefault(f => f.Mobile == mobile && f.Name.Equals(username)); // && f.Review == true && f.Enable == true
                    if (user == null)
                    {
                        TempData["err"] = "姓名或手機錯誤。";
                    }
                    else
                    {
                        //if (user.FaceBookId != null)
                        //{
                        //    TempData["err"] = "此帳號使用臉書登入，無法使用查詢密碼。";
                        //    if (IsMobile)
                        //        return RedirectToAction("search_pw", "Mobile");
                        //    return View();
                        //}


                        //--------------------------------------------------------------------------------------------------
                        if (user.LastSendTime.HasValue)
                        {

                            if (user.LastSendTime.Value.AddHours(1) >= DateTime.UtcNow)
                            {
                                TempData["err"] = "一個小時內只能寄發一封簡訊，下次可以寄發的時間是 " + user.LastSendTime.Value.AddHours(9).ToString("yyyy-MM-dd HH:mm") + "。";
                                if (IsMobile)
                                    return RedirectToAction("forgot_Sign_in", "Login");
                                return View();
                            }
                            else
                            {
                                var pw = Method.RandomKey(8, DateTime.Now.Millisecond * 2);

                                user.Password = data.Get_HashPassword(pw);
                                user.SendCount += 1;
                                user.LastSendTime = DateTime.UtcNow;
                                db.SubmitChanges();

                                if (MobileAPIModel.sendToServer(2, user.Mobile, pw).isSuccess)
                                    TempData["success"] = "已將新的密碼寄至您的手機" + mobile + " 。";
                                else
                                    TempData["success"] = "寄送失敗，請稍候再試。";
                            }
                        }
                        else
                        {
                            var pw = Method.RandomKey(8, DateTime.Now.Millisecond * 2);

                            user.Password = data.Get_HashPassword(pw);
                            user.SendCount += 1;
                            user.LastSendTime = DateTime.UtcNow;
                            db.SubmitChanges();

                            if (MobileAPIModel.sendToServer(2, user.Mobile, pw).isSuccess)
                                TempData["success"] = "已將新的密碼寄至您的手機" + mobile + " 。";
                            else
                                TempData["success"] = "寄送失敗，請稍候再試。";
                        }
                        //--------------------------------------------------------------------------------------------------
                    }
                }
            }
            else
            {
                TempData["err"] = "Error: 錯誤的驗證碼。";
                if (IsMobile)
                    return RedirectToAction("forgot_Sign_in", "Login");
                return View();
            }
            if (IsMobile)
                return RedirectToAction("forgot_Sign_in", "Login");
            return View();
        }

        //public ActionResult RegisterComplete()
        //{
        //    ViewBag.Title = "註冊成功";
        //    TempData["msg"] = "註冊成功，請至信箱查收驗證信件。";
        //    return View("Complete");
        //}

        //public ActionResult PasswordForgot()
        //{
        //    return View();
        //}

        ///*
        //[HttpPost][ValidateInput(true)]
        //public ActionResult PasswordForgot(VolunteersModel.PasswordForgotByMobile item)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        if (data.Update_PasswordForgot_ByMobile(item) > 0)
        //        {
        //            return RedirectToAction("PasswordForgot_Complete");
        //        }
        //    }
        //    TempData["err"] = "寄送失敗請稍後再試。";
        //    return View(item);
        //}*/

        //public ActionResult PasswordForgot_Complete()
        //{
        //    ViewBag.Title = "寄送成功";
        //    TempData["msg"] = "寄送成功! 請收信確認，謝謝!";
        //    return View("Complete");
        //}

        //public ActionResult ConfirmNotFound()
        //{
        //    ViewBag.Title = "驗證失敗";
        //    TempData["err"] = "找不到此帳號!";
        //    return View("ConfirmFail");
        //}

        //public ActionResult ConfirmSuccess()
        //{
        //    ViewBag.Title = "驗證成功";
        //    TempData["msg"] = "驗證成功! 謝謝!";
        //    return View("Complete");
        //}

        //public ActionResult ConfirmFail()
        //{
        //    ViewBag.Title = "驗證失敗";
        //    TempData["err"] = "信箱驗證失敗!";
        //    return View();
        //}

        //public ActionResult EmailConfirm()
        //{
        //    return View();
        //}

        //[HttpPost][ValidateInput(true)]
        //public ActionResult EmailConfirm(VolunteersModel.PasswordForgot item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (data.Update_EmailConfirm(item) > 0)
        //        {
        //            return RedirectToAction("EmailConfirm_Complete");
        //        }
        //    }
        //    TempData["err"] = "寄送失敗請稍後再試。";
        //    return View(item);
        //}

        //public ActionResult EmailConfirm_Complete()
        //{
        //    ViewBag.Title = "補發信箱驗證";
        //    TempData["msg"] = "補發信箱驗證寄送成功! 請收信確認，謝謝!";
        //    return View("Complete");
        //}

        ////Facebook Auth


        //public ActionResult FacebookConnection()
        //{
        //    // 省略了 response_type 參數
        //    // 就不會授權過還會在出現授權畫面
        //    string Url = "https://www.facebook.com/dialog/oauth?scope={0}&redirect_uri={1}&client_id={2}";
        //    // email 是取得信箱的權限
        //    // read_stream 是讀取動態牆的權限
        //    // 權限之間用半型逗號(,)做分隔
        //    string scope = "email";
        //    string redirect_uri_encode = Utitity.UrlEncode(redirect_uri);

        //    Response.Redirect(string.Format(Url, scope, redirect_uri_encode, client_id));

        //    return null;
        //}

        //public ActionResult CallBack(string Code)
        //{
        //    // 沒有接收到參數
        //    if (string.IsNullOrEmpty(Code))
        //        return Content("沒有收到 Code");

        //    // 沒有 grant_type 參數
        //    string Url = "https://graph.facebook.com/oauth/access_token?code={0}&client_id={1}&client_secret={2}&redirect_uri={3}";
        //    string redirect_uri_encode = Utitity.UrlEncode(redirect_uri);

        //    HttpWebRequest request = HttpWebRequest.Create(string.Format(Url, Code, client_id, client_secret, redirect_uri_encode)) as HttpWebRequest;
        //    string result = null;
        //    request.Method = "Get";    // 方法
        //    request.KeepAlive = true; //是否保持連線

        //    using (WebResponse response = request.GetResponse())
        //    {
        //        StreamReader sr = new StreamReader(response.GetResponseStream());
        //        result = sr.ReadToEnd();
        //        sr.Close();
        //    }
        //    // 回傳的網頁格式會是 QueryString 格式
        //    // access_token={access_token}&expires={expires}
        //    NameValueCollection qscoll = HttpUtility.ParseQueryString(result);

        //    Session["token"] = qscoll["access_token"];  // 記錄 access_token

        //    // 這邊不建議直接把 Token 當做參數傳給 CallAPI 可以避免 Token 洩漏
        //    return RedirectToAction("CallAPI");
        //}
    }
}
