using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using s26web.Models;
using System.IO;

namespace s26web.Areas.shb.Models
{
    public class LoginRecordModel
    {
        public class LoginRecordModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("IP")]
            public string Ip { get; set; }
            [DisplayName("帳號")]
            public string Account { get; set; }
            [DisplayName("登入狀態")]
            public bool Login { get; set; }
            [DisplayName("瀏覽器")]
            public string Browser { get; set; }
            [DisplayName("作業系統")]
            public string OS { get; set; }
            [DisplayName("建立時間")]
            public DateTime CreateTime { get; set; }
        }

        public LoginRecordModel()
        {
        }

        public string Keyword = "";
        public bool Enable_Login = false;
        public bool Login = false;
        public DateTime? create_time_start = null;
        public DateTime? create_time_end = null;

        public void Clear_Params()
        {
            Enable_Login = false;
            Login = false;
            Keyword = "";
            create_time_start = null;
            create_time_end = null;
        }

        private LoginRecordModelShow Convert(LoginRecord item)
        {
            return new LoginRecordModelShow
            {
                Id = item.Id,
                Account = item.Account,
                Browser = item.Browser,
                Ip = item.Ip,
                Login = item.Login,
                OS = item.OS,
                CreateTime = item.CreateTime
            };
        }

        private static LoginRecord Convert(string account, HttpRequestBase Request, bool login)
        {
            return new LoginRecord
            {
                Account = account,
                Browser = Request.UserAgent,
                OS = Request.Browser.Platform,
                Ip = Request.UserHostAddress,
                Login = login,
                CreateTime = DateTime.UtcNow
            };
        }

        #region get
        public LoginRecordModelShow Get_One(int id)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.LoginRecord.FirstOrDefault(f => f.Id == id);
                if (data != null)
                {
                    return Convert(data);
                }
                db.Connection.Close();
            }
            catch { }
            return null;
        }

        public List<LoginRecordModelShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                var data = Get().Skip((p - 1) * take).Take(take).Select(s => new LoginRecordModelShow
                {
                    Id = s.Id,
                    Account = s.Account,
                    Browser = s.Browser,
                    Ip = s.Ip,
                    Login = s.Login,
                    OS = s.OS,
                    CreateTime = s.CreateTime
                }).ToList();
                db.Connection.Close();
                return data;
            }
            catch { return new List<LoginRecordModelShow>(); }
        }

        public Method.Paging Get_Page(int p = 1, int take = 10, int pages = 5)
        {
            return Method.Get_Page(Get_Count(), p, take, pages);
        }

        public int Get_Count()
        {
            try
            {
                return Get().Count();
            }
            catch { return 0; }
        }

        private IQueryable<LoginRecord> Get()
        {
            s26webDataContext db = new s26webDataContext();
            IQueryable<LoginRecord> data = db.LoginRecord;
            if (Keyword != "")
            {
                data = data.Where(Query(Keyword));
            }
            if (Enable_Login)
            {
                data = data.Where(w => w.Login == Login);
            }
            if (create_time_start != null)
            {
                data = data.Where(w => w.CreateTime >= create_time_start.Value);
            }
            if (create_time_end != null)
            {
                data = data.Where(w => w.CreateTime <= create_time_end.Value);
            }
            db.Connection.Close();
            return data.OrderByDescending(o=>o.Id);
        }

        private string Query(string query = "")
        {
            string sql = "";

            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "OS", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Account", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Ip", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Browser", "OR", "( \"" + query + "\")", ".Contains", false);
            }

            return sql;
        }
        #endregion

        #region insert
        public static int Login_Record(string account, HttpRequestBase Request, bool login)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                LoginRecord item = Convert(account, Request, login);
                db.LoginRecord.InsertOnSubmit(item);
                db.SubmitChanges();
                db.Connection.Close();
                return item.Id;
            }
            catch { return -1; }
        }
        #endregion

        #region delete
        public void Delete(int id)
        {
            if (id > 0)
            {
                Delete(new int[] { id });
            }
        }

        public void Delete(int[] id)
        {
            if (id != null)
            {
                if (id.Any())
                {
                    s26webDataContext db = new s26webDataContext();
                    var data = db.LoginRecord.Where(w => id.Contains(w.Id));
                    if (data.Any())
                    {
                        db.LoginRecord.DeleteAllOnSubmit(data);
                        db.SubmitChanges();
                    }
                    db.Connection.Close();
                }
            }
        }
        #endregion
    }
}