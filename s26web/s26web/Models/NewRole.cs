using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using s26web.Models;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using s26web.Areas.shb.Models;

namespace s26web.Models
{
    public class NewAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            //String[] users = Users.Split(',');//取得輸入user清單
            //String[] roles = Roles.Split(',');//取得輸入role清單
            /*if (!httpContext.User.Identity.IsAuthenticated)//判斷是否已驗證
                return false;
            if (!httpContext.Request.Cookies.AllKeys.Contains("Home"))
                return false;*/

            /*if (!Method.Check_Login(httpContext.Request.Cookies, lluminate.Models.Method.CookieName_Home))
                return false;*/

            try
            {
                FormsAuthenticationTicket authTicket = Method.Get_Cookie_Ticket(httpContext.Request.Cookies, Method.CookieName_Home);

                if (authTicket == null)
                    return false;
                if (!authTicket.IsPersistent && authTicket.Expired)
                    return false;
                if (authTicket.Version != Method.CookieVersion_Home)
                    return false;

                HttpCookie hc = httpContext.Request.Cookies[Method.CookieName_Home];

                if ((DateTime.UtcNow - authTicket.IssueDate).TotalDays > 30)
                {
                    httpContext.Session.RemoveAll();
                    hc.Expires = DateTime.UtcNow.AddDays(-30);
                    httpContext.Response.AppendCookie(hc);
                    return false;
                }
                //string account = Method.Get_Account(httpContext.Request.Cookies, Method.CookieName_Home, httpContext.Session, Method.SessionUserAccount_Home);

                /*if (authTicket == null)
                    return false;*/
                //string account = authTicket.Name;
                //string account = httpContext.User.Identity.Name.ToString(); //登入的使用者帳號

                //var role = VolunteersModel.Get_One(account);
                
                var data = VolunteersModel.Get_One(authTicket.Name);
                //var data = MemberModel.Get_One(authTicket.Name);

                if (data == null)
                    return false;

                if (data.Name + "," + data.Password != authTicket.UserData)
                {
                    httpContext.Session.RemoveAll();
                    hc.Expires = DateTime.UtcNow.AddDays(-30);
                    httpContext.Response.AppendCookie(hc);
                    return false;
                }

                if (httpContext.Session.IsNewSession)
                {
                    //MemberModel.Update_LoginTime(mm.Id);
                    //LoginRecordModel.Login_Record(mm.Account_Phone, httpContext.Request, true);
                }
                else
                {
                    LoginCookie(httpContext, data, authTicket.IsPersistent);
                }

                if (Method.Get_Session(httpContext.Session, Method.SessionUserAccount_Home) == "")
                {
                    LoginSession(httpContext, data);
                }

                return true;
            }
            catch { return false; }
        }

        private void LoginCookie(HttpContextBase httpContext, Volunteers item, bool RememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                Method.CookieVersion_Home,
                item.Mobile,//item.Account,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(60),
                RememberMe,
                item.Name + "," + item.Password,
                FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            httpContext.Response.Cookies.Add(new HttpCookie(Method.CookieName_Home, encTicket));
            httpContext.Response.Cookies[Method.CookieName_Home].Path = "/";
        }

        private void LoginSession(HttpContextBase httpContext, Volunteers item)
        {
            httpContext.Session.RemoveAll();
            httpContext.Session.Add(Method.SessionUserId_Home, item.Id);
            httpContext.Session.Add(Method.SessionUserAccount_Home, item.MemberNumber);
            httpContext.Session.Add(Method.SessionUserName_Home, item.Name);
            httpContext.Session.Add(Method.SessionUserEmail_Home, item.Email);
        }
    }
}