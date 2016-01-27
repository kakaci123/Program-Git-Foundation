using s26web.Areas.shb.Models;
using s26web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace s26web.Areas.shb.Models
{

    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public enum Competence
        {
            Read,
            Insert,
            Update,
            Delete,
            Admin
        }
        public Competence Com { get; set; }

        public string function { get; set; }

        public override void OnAuthorization(AuthorizationContext context)
        {
            base.OnAuthorization(context);
            //判斷!! 當是未登入使用者時
            if (context.Result is HttpUnauthorizedResult)
            {
                //自己寫的Method，用來做判斷未登入使用者是從哪個頁面來的
                CheckPermission(context);
                return;
            }
        }
        private void CheckPermission(AuthorizationContext filterContext)
        {
            object areaName = null;
            //當使用者是從area來的，並且這個area名稱是Admin時(也就是後台管理者未登入時)
            if (filterContext.RouteData.DataTokens.TryGetValue("area", out areaName)
                && (areaName as string) == "shb")
            {
                filterContext.Controller.TempData["err"] = "網址不正確或權限不足!";
                //導向 /Default/LogOn

                filterContext.Result = new RedirectToRouteResult("Admin_default",
                        new RouteValueDictionary
                      {
                          { "controller", "Member" },
                          { "action", "Login" },
                          {"ReturnUrl",filterContext.HttpContext.Request.Url.LocalPath}
                          //{ "id", UrlParameter.Optional }
                      });
            }
            else
            {
                //否則就直接用預設的登入頁面
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            /*if (!httpContext.User.Identity.IsAuthenticated)//判斷是否已驗證
               return false;
            if (!httpContext.Request.Cookies.AllKeys.Contains(lluminate.Models.Method.CookieName_Admin))
                return false;*/

            /*if(!Method.Check_Login(httpContext.Request.Cookies, lluminate.Models.Method.CookieName_Admin))
                return false;*/
            //string account = httpContext.User.Identity.Name.ToString(); //登入的使用者帳號
            //if (httpContext.Session[Method.SessionLevel_Admin] == null)//讓session消失後，可以回到首頁
            if (!Method.Is_Login_Admin(httpContext.Request.Cookies))
            {
                return false;
            }

            try
            {
                /*if (!Method.Is_Login_Admin(httpContext.Request.Cookies))
                    return false;*/

                FormsAuthenticationTicket authTicket = Method.Get_Cookie_Ticket(httpContext.Request.Cookies, Method.CookieName_Admin);

                if (authTicket == null)
                    return false;
                if (!authTicket.IsPersistent && authTicket.Expired)
                    return false;

                HttpCookie hc = httpContext.Request.Cookies[Method.CookieName_Admin];

                if ((DateTime.UtcNow - authTicket.IssueDate).TotalDays > 1)
                {
                    httpContext.Session.RemoveAll();
                    hc.Expires = DateTime.UtcNow.AddDays(-30);
                    httpContext.Response.AppendCookie(hc);
                    return false;
                }

                var mm = MemberModel.Get_One(authTicket.Name);
                if (mm == null)
                    return false;

                if (mm.Name + "," + mm.Password != authTicket.UserData)
                {
                    httpContext.Session.RemoveAll();
                    hc.Expires = DateTime.UtcNow.AddDays(-30);
                    httpContext.Response.AppendCookie(hc);
                    return false;
                }

                if (mm.Id == 0)
                    return authTicket.Version == Method.CookieVersion_Admin;

                if (authTicket.Version != Method.CookieVersion_Home)
                    return false;

                if (httpContext.Session.IsNewSession)
                {
                    //MemberModel.Update_LoginTime(mm.Id);
                    //LoginRecordModel.Login_Record(mm.Account_Phone, httpContext.Request, true);
                }
                else
                {
                    LoginCookie(httpContext, mm, authTicket.IsPersistent);
                }

                if (Method.Get_Session(httpContext.Session, Method.SessionUserAccount_Admin) == "")
                {
                    LoginSession(httpContext, mm);
                }
                if (function != null && function == "CheckSession") return true;//
                int user_level = mm.Level;
                int fid = Get_FunctionId(function);

                s26webDataContext db = new s26webDataContext();
                var ct = db.CompetenceTable.FirstOrDefault(w => w.FunctionId == fid && w.UserLevelId == user_level && w.Enable);
                db.Connection.Close();
                return ct != null;
            }
            catch { return false; }
            /*String[] users = Users.Split(',');//取得輸入user清單
            String[] roles = Roles.Split(',');//取得輸入role清單
            if (!httpContext.User.Identity.IsAuthenticated)//判斷是否已驗證
                return false;
            if (roles.Any())
            {
                String account = httpContext.User.Identity.Name.ToString(); //登入的使用者帳號

                s26webDataContext db = new s26webDataContext();

                var role = (from i in db.UserProfile
                            join j in db.UserLevel on i.Level equals j.Id
                            where i.Account_Phone == account
                            select j.Name).FirstOrDefault();

                var result = roles.Where(w => w.Trim() == role);
                return result.Any();
            }
            return true;*/
        }

        private int Get_FunctionId(string fun)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.Function.FirstOrDefault(f => f.Name == fun);
            db.Connection.Close();
            return data == null ? 0 : data.Id;
        }

        private void LoginCookie(HttpContextBase httpContext, MemberModel.MemberShow item, bool RememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                item.Id == 0 ? Method.CookieVersion_Admin : Method.CookieVersion_Home,
                item.Account_Phone,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(60),
                RememberMe,
                item.Name + "," + item.Password,
                FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            httpContext.Response.Cookies.Add(new HttpCookie(Method.CookieName_Admin, encTicket));
            httpContext.Response.Cookies[Method.CookieName_Admin].Path = "/";
        }

        private void LoginSession(HttpContextBase httpContext, MemberModel.MemberShow item)
        {
            MemberModel data = new MemberModel();

            var competence = data.Get_Competence(item.Level);
            httpContext.Session.RemoveAll();

            httpContext.Session.Add(Method.SessionUserId_Admin, item.Id);
            httpContext.Session.Add(Method.SessionUserAccount_Admin, item.Account_Phone);
            httpContext.Session.Add(Method.SessionUserName_Admin, item.Name);
            httpContext.Session.Add(Method.SessionLevel_Admin, item.Level);
            httpContext.Session.Add(Method.SessionLevelName_Admin, item.Level_Name);
            httpContext.Session.Add(Method.SessionUserAccount_Admin, item.Enable);
            httpContext.Session.Add(Method.SessionComptpence, competence);
        }
    }
}