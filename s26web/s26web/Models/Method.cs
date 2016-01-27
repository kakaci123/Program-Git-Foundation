using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using s26web.Areas.shb.Models;
using System.Text.RegularExpressions;

namespace s26web.Models
{
    public class Method
    {
        //public static string RootPath = "/s26";
        public static string RootPath = "";
        public static string CookieName_Home = "shb_s26_home";
        public static string CookieName_Admin = "shb_s26_admin";

        public static int CookieVersion_Home = 94;
        public static int CookieVersion_Admin = 121;

        public static string SessionUserId_Home = "Shb_s26_UserId";
        public static string SessionUserAccount_Home = "Shb_s26_UserAccount";
        public static string SessionUserName_Home = "Shb_s26_UserName";
        public static string SessionUserEmail_Home = "Shb_s26_UserEmail";

        public static string SessionUserId_Admin = "Shb_UserId_admin";
        public static string SessionUserAccount_Admin = "Shb_UserAccount_admin";
        public static string SessionUserName_Admin = "Shb_UserName_admin";
        public static string SessionUserEmail_Admin = "Shb_UserEmail_admin";
        public static string SessionLevel_Admin = "Shb_Level_admin";
        public static string SessionLevelName_Admin = "Shb_LevelName_admin";
        public static string SessionComptpence = "Shb_Comptpence_admin";

        public class Paging
        {
            public int First = 1;
            public int Last = 1;
            public int Total = 1;
            public int Now = 1;
            public int Back = 1;
            public int Next = 1;
            public int Start = 1;
            public int End = 1;
        }
        public static byte[] FileToByte(string fileName)
        {
            // 打開文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 讀取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return bytes;
        }
        /// <summary>
        /// 分頁
        /// </summary>
        /// <param name="total">總數量</param>
        /// <param name="current_page">目前頁數</param>
        /// <param name="page_count">單頁數量</param>
        /// <returns>分頁</returns>
        public static Paging Get_Page(int total, int current_page = 1, int page_count = 10, int pages = 5)
        {
            Paging page = new Paging();

            int count = pages;
            int count_com = Convert.ToInt32(Math.Floor((double)count / 2));

            page.First = 1;
            int last = Convert.ToInt32(Math.Ceiling((double)total / (double)page_count));
            page.Last = last <= 0 ? 1 : last;
            page.Total = total;
            page.Now = current_page;

            page.Back = (current_page > 1) ? current_page - 1 : current_page;
            page.Next = (current_page < last) ? current_page + 1 : current_page;

            int start = current_page - count_com;
            page.Start = (start < 1) ? 1 : start;

            int end = page.Start + count - 1;
            end = (end >= last) ? last : end;
            page.End = (end == 0) ? 1 : end;

            return page;
        }
        public static string GetMD5(string input, bool small)
        {
            System.Security.Cryptography.MD5 m = System.Security.Cryptography.MD5.Create();
            if (small)
                return BitConverter.ToString(m.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
            else
                return BitConverter.ToString(m.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
        }
        /*
        public static string GetDeMD5(string input, bool small)
        {
            System.Security.Cryptography.MD5 m = System.Security.Cryptography.MD5.Create();
            if (small)
                return BitConverter.ToString(m.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
            else
                return BitConverter.ToString(m.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
        }
         * */
        /// <summary>
        /// 組合SQL
        /// </summary>
        /// <param name="sql">原始字串</param>
        /// <param name="col">欄位名稱</param>
        /// <param name="logic">連接邏輯(AND,OR)</param>
        /// <param name="value">判斷值</param>
        /// <param name="op">判斷式(=,>)</param>
        /// <param name="value_com">判斷值是否需要加上"</param>
        /// <returns>組合結果</returns>
        public static string SQL_Combin(string sql, string col, string logic, string value, string op, bool value_com)
        {
            op = " " + op + " ";
            logic = " " + logic + " ";
            if (value_com)
            {
                value = "\"" + value + "\"";
            }
            if (sql == "")
            {
                sql = col + op + value;
            }
            else
            {
                sql += logic + col + op + value;
            }

            sql = "(" + sql + ")";

            return sql;
        }

        public static bool Send_Post(string url, Dictionary<string, string> param, Encoding e)
        {
            try
            {
                byte[] bs = Encoding.ASCII.GetBytes(Get_Form_Params(param, e));

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded;charset=" + e.WebName;
                req.ContentLength = bs.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {
                    int pos = 0;
                    Stream sr = wr.GetResponseStream();
                    byte[] response = new byte[(int)wr.ContentLength];

                    while (pos < response.Length)
                    {
                        int bytesRead = sr.Read(response, pos, response.Length - pos);
                        if (bytesRead == 0)
                        {
                            // End of data and we didn't finish reading. Oops.
                            //throw new IOException("Premature end of data");
                            return false;
                        }
                        pos += bytesRead;
                        //OnReceiveProgress(response.Length, pos);
                    }

                    int temp = Convert.ToInt32(Encoding.Default.GetString(response));

                    if (temp >= 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch { return false; }
        }

        public static string Get_URLGet(string field, string value)
        {
            if (value != "" && value != null)
            {
                return "&" + field + "=" + value;
            }
            return "";
        }
        public static string Get_URLGet(string field, int[] value)
        {
            if (value != null)
            {
                if (value.Any())
                {
                    string tmp = "";
                    foreach (var i in value)
                    {
                        tmp += "&" + field + "=" + i;
                    }
                    return tmp;
                }
            }
            return "";
        }

        public static string Get_Form_Params(Dictionary<string, string> param, Encoding e)
        {
            if (param.Any())
            {
                string temp = "";
                foreach (string key in param.Keys)
                {
                    temp += HttpUtility.UrlEncode(key, e) + "=" + HttpUtility.UrlEncode(param[key], e) + "&";
                }

                return temp.TrimEnd('&');
            }
            return "";
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 設置當前流的位置為流的開始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 轉換成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 設置當前流的位置為流的開始 
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 寫入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        public static Stream FileToStream(string fileName)
        {
            // 打開文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 讀取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 轉換成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public static byte[] Get_File(string url)
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadData(url);
        }

        public static void Get_File(string url, string path)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, path);
        }

        private static string Get_Value(JObject j, string key)
        {
            if (j[key] != null)
            {
                return j[key].ToString();
            }
            return "";
        }

        public static string RandomNumber(int length, int start, int end)
        {
            Random r = new Random(DateTime.UtcNow.Millisecond);
            int result = r.Next(start, end);

            return string.Format("{0:0000}", result);
        }

        public static string RandomKey(int length, int random)
        {
            string[] Code ={  "a", "b", "c", "d", "e", "f", "T", "U", "V", "W", "X", "Y", "Z",
                            "N", "O", "P", "Q", "R", "S", "v", "w", "x", "y", "z",
                             "5", "6", "7", "8", "9" ,"n",  "p", "q", "r", "s", "t", "u",
                             "A", "B", "C", "D", "E", "F", "g", "h", "i", "j", "k", "l", "m",
                           "0", "1", "2", "3", "4", "G", "H", "I", "J", "K", "L", "M"};

            int size = Code.Length - 1;

            string result = "";

            for (int i = 0; i < length; i++)
            {
                Random temp = new Random(i + random);
                int op = temp.Next(1, 4);
                int go = 0;
                switch (op)
                {
                    case 1:
                        go = DateTime.UtcNow.Hour + random - i;
                        break;
                    case 2:
                        go = random - DateTime.UtcNow.Millisecond + i;
                        break;
                    case 3:
                        go = DateTime.UtcNow.Minute * i + random;
                        break;
                    case 4:
                        go = DateTime.UtcNow.Second - random - i;
                        break;
                }

                Random r = new Random(go);
                result += Code[r.Next(0, size)];
            }

            return result;
        }

        public static bool Is_Login_Admin(HttpCookieCollection hkc)
        {
            FormsAuthenticationTicket authTicket = Method.Get_Cookie_Ticket(hkc, Method.CookieName_Admin);
            if (authTicket == null)
                return false;
            if (!authTicket.IsPersistent && authTicket.Expired)
                return false;

            var mm = MemberModel.Get_One(authTicket.Name);
            if (mm == null)
                return false;

            if (mm.Id == 0)
                return authTicket.Version == Method.CookieVersion_Admin;


            return authTicket.Version == Method.CookieVersion_Home;
        }

        public static bool Is_Login(HttpCookieCollection hkc)
        {
            FormsAuthenticationTicket authTicket = Method.Get_Cookie_Ticket(hkc, Method.CookieName_Home);

            if (authTicket == null)
                return false;
            if (!authTicket.IsPersistent && authTicket.Expired)
                return false;
            return authTicket.Version == Method.CookieVersion_Home;
        }

        //判斷會員登入進入首頁是否要跳出Popup視窗
        public static bool Is_Popup(HttpSessionStateBase Session)
        {
            string Session_name = Method.SessionUserId_Home;
            string account = Method.Get_Session(Session, Session_name);

            //string x = Session[Session_name].ToString();

            if (account == "")
            {
                return false;
            }
            else
            {
                s26webDataContext db = new s26webDataContext();
                var vol = db.Volunteers.FirstOrDefault(w => w.Id == int.Parse(account));

                if (vol.NowBrand == 1 && DateTime.Compare(vol.BabyBirthday, DateTime.UtcNow) < 0)
                {
                    db.Connection.Close();
                    return true;
                }
                else
                {
                    db.Connection.Close();
                    return false;
                }
            }
        }

        //宅配訂購功能 依會員判斷顯示那些產品
        public static int NowBrand_Status(HttpSessionStateBase Session)
        {
            string Session_name = Method.SessionUserId_Home;
            string account = Method.Get_Session(Session, Session_name);

            if (account == "")
            {
                return -1;
            }
            else
            {
                /* 2015/3/16
                    ※ 身分為母乳哺餵的會員， 不論選擇什麼生日，都是帶金幼兒樂和DHA。
 
                    ※ 其它身分( 懷孕期，使用S-26產品)
                          1. 寶寶生日/預產期小於今日，帶金幼兒樂和DHA選擇畫面
                          2. 寶寶生日/預產期大於今日 & 小於一年，直接進入購買金愛兒產品確認畫面
                          3. 寶寶生日/預產期大於今日 & 大於一年，帶金幼兒樂和DHA選擇畫面
                 */

                s26webDataContext db = new s26webDataContext();
                var vol = db.Volunteers.FirstOrDefault(w => w.Id == int.Parse(account));

                if (DateTime.Compare(vol.BabyBirthday, DateTime.UtcNow) > 0 && DateTime.Compare(vol.BabyBirthday.AddYears(1), DateTime.UtcNow) < 0)
                {
                    //2.寶寶生日/預產期大於今日 & 小於一年，直接進入購買金愛兒產品確認畫面
                    db.Connection.Close();
                    return 1;
                }
                else
                {
                    db.Connection.Close();
                    return 2;
                }
            }
        }

        public static List<SelectListItem> Select_SalesPromotion(int SalesPromotionNameId, bool all)
        {
            s26webDataContext db = new s26webDataContext();
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Selected = SalesPromotionNameId == 0,
                Text = all ? "全部" : "請選擇",
                Value = "0"
            });

            int this_Year = int.Parse(DateTime.Now.ToString("yyyy"));

            //for (var i = this_Year - 7; i <= this_Year + 1; i++)
            //{
            //    data.Add(new SelectListItem
            //    {
            //        Text = i.ToString(),
            //        Value = i.ToString()
            //    });
            //}



            //data.AddRange(db.Product.Where(w => w.Id != null).OrderBy(o => o.Id).Select(s =>  new SelectListItem
            //    {
            //        Selected = SalesPromotionNameId == s.Id,
            //        Text = s.Name,
            //        Value = s.Id.ToString()
            //    }));
            db.Connection.Close();
            return data;
        }

        public static FormsAuthenticationTicket Get_Cookie_Ticket(HttpCookieCollection hkc, string name)
        {
            if (!hkc.AllKeys.Contains(name))
                return null;

            try
            {
                HttpCookie hc = hkc[name];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(hc.Value);
                return authTicket;
            }
            catch { }
            return null;
        }

        public static string Get_Session(HttpSessionStateBase Session, string name)
        {
            if (Session[name] == null)
            {
                return "";
            }
            else
            {
                return Session[name].ToString();
            }
        }

        public static int Get_Session_Int(HttpSessionStateBase Session, string name)
        {
            try
            {
                if (Session[name] == null)
                {
                    return -1;
                }
                else
                {
                    int result = -1;
                    if (Int32.TryParse(Session[name].ToString(), out result))
                    {
                        return result > 0 ? result : -1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch { return -1; }
        }

        public static string Get_UserName(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                string hkc_name = Method.CookieName_Home;
                string Session_name = Method.SessionUserName_Home;
                string account = Method.Get_Session(Session, Session_name);
                if (account == "")
                {
                    FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                    if (tk == null)
                    {
                        return "";
                    }
                    account = tk.UserData.Split(',')[0];
                }
                return account;
            }
            catch { return ""; }
        }
        //public static Sale_Act GetDisplaySale()
        //{
        //    lluminateDataContext db = new lluminateDataContext();
        //    var item = db.Sale_Act.FirstOrDefault(f => f.Display);
        //    db.Connection.Close();
        //    return item;
        //}
        //public static Sale_Act GetNowSale() 
        //{
        //    lluminateDataContext db = new lluminateDataContext();
        //    var item = db.Sale_Act.FirstOrDefault(f => f.Display);
        //    db.Connection.Close();
        //    return item;
        //}
        public static string Get_UserNameAdmin(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                string hkc_name = Method.CookieName_Admin;
                string Session_name = Method.SessionUserName_Admin;
                string account = Method.Get_Session(Session, Session_name);
                if (account == "")
                {
                    FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                    if (tk == null)
                    {
                        return "";
                    }
                    account = tk.UserData.Split(',')[0];
                }
                return account;
            }
            catch { return ""; }
        }
        public static string Convert_States(string str)
        {
            if (str == "2" || str == "已出貨")
                str = "<label style='color:blue;'>已核准</label>";
            else if (str == "1")
                str = "未核准";
            else if (str == "退貨")
                str = "已退貨";
            else if (str == "取消")
                str = "已取消";
            else if (str == "0" || str == "處理中")
                str = "<label style='color:red;'>審核中</label>";
            return str;
        }
        public static string Convert_StatesForAPI(string str)
        {
            if (str == "2" || str == "已出貨")
                str = "已核准";
            else if (str == "1")
                str = "未核准";
            else if (str == "退貨")
                str = "已退貨";
            else if (str == "取消")
                str = "已取消";
            else if (str == "0" || str == "處理中")
                str = "審核中";
            return str;
        }
        public static int Get_Point(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                string hkc_name = Method.CookieName_Home;
                string Session_name = Method.SessionUserAccount_Home;
                string account = Method.Get_Session(Session, Session_name);
                int? point;

                FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                if (tk == null)
                {
                    return 0;
                }
                point = db.Volunteers.FirstOrDefault(f => f.Mobile == tk.Name).Point;
                return point ?? 0;


            }
            catch { return -1; }
        }
        public static string Get_Account(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                string hkc_name = Method.CookieName_Home;
                string Session_name = Method.SessionUserAccount_Home;
                string account = Method.Get_Session(Session, Session_name);
                if (account == "")
                {
                    FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                    if (tk == null)
                    {
                        return "";
                    }
                    account = tk.Name;
                }
                return account;
            }
            catch { return ""; }
        }
        public static string Get_AccountAdmin(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                string hkc_name = Method.CookieName_Admin;
                string Session_name = Method.SessionUserAccount_Admin;
                string account = Method.Get_Session(Session, Session_name);
                if (account == "")
                {
                    FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                    if (tk == null)
                    {
                        return "";
                    }
                    account = tk.Name;
                }
                return account;
            }
            catch { return ""; }
        }

        public static int Get_UserId(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                string hkc_name = Method.CookieName_Home;
                string Session_name = Method.SessionUserId_Home;
                int user_id = Method.Get_Session_Int(Session, Session_name);
                if (user_id < 0)
                {
                    FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                    if (tk == null)
                    {
                        return -1;
                    }
                    //var data = VolunteersModel.Get_One(tk.Name);
                    //if (data == null)
                    //{
                    //    return -1;
                    //}
                    //user_id = data.Id;
                }
                return user_id;
            }
            catch { return -1; }
        }

        public static int Get_UserId2(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                string hkc_name = Method.CookieName_Home;
                string Session_name = Method.SessionUserName_Home;
                string account = Method.Get_Session(Session, Session_name);
                FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                if (tk == null)
                {
                    return 0;
                }
                account = tk.Name;
                s26webDataContext db = new s26webDataContext();
                int VolunteersId = db.Volunteers.FirstOrDefault(f => f.Mobile == account).Id;
                return VolunteersId;

            }
            catch { return -1; }
        }

        public static int Get_UserId_Admin(HttpCookieCollection hkc, HttpSessionStateBase Session)
        {
            try
            {
                string hkc_name = Method.CookieName_Admin;
                string Session_name = Method.SessionUserId_Admin;
                int user_id = Method.Get_Session_Int(Session, Session_name);
                if (user_id < 0)
                {
                    FormsAuthenticationTicket tk = Method.Get_Cookie_Ticket(hkc, hkc_name);
                    if (tk == null)
                    {
                        return -1;
                    }

                    var data = MemberModel.Get_One(tk.Name);
                    if (data == null)
                    {
                        return -1;
                    }
                    user_id = data.Id;
                }
                return user_id;
            }
            catch { return -1; }
        }

        public static string Upload_File(HttpPostedFileBase upload, string dir, HttpServerUtilityBase Server)
        {

            string real_url = "/upload/" + dir + "/";
            if (upload != null && upload.ContentLength > 0)
            {
                string img_dir = "~" + real_url;
                if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(img_dir)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(img_dir));
                }
                var filename = Path.GetFileNameWithoutExtension(upload.FileName);
                string url = Server.MapPath(img_dir + filename);
                int count = 1;
                string temp = "";
                do
                {
                    string name1 = System.IO.Path.GetFileNameWithoutExtension(upload.FileName);
                    string name2 = System.IO.Path.GetExtension(upload.FileName);
                    DateTime dt = DateTime.UtcNow;
                    temp = name1 + "_" + dt.Year + dt.Month + dt.Day + dt.Hour + dt.Minute + dt.Second + count + name2;
                    url = Server.MapPath(img_dir + temp);
                    count++;
                } while (System.IO.File.Exists(url));
                upload.SaveAs(url);
                real_url += temp;
            }
            else
            {
                real_url = "";
            }

            return real_url;
        }

        public static string Upload_Logo(HttpPostedFileBase upload, HttpServerUtilityBase Server, string name)
        {

            string real_url = Method.RootPath + "/upload/";
            if (upload != null && upload.ContentLength > 0)
            {
                string img_dir = "~" + real_url;
                if (!System.IO.Directory.Exists(Server.MapPath(img_dir)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(img_dir));
                }
                Image img = ResizeImage(Image.FromStream(upload.InputStream), new Size(165, 75), ResizeType.Fixed);
                Stream s = CompressionImage(img, 80, ImageFormat.Png);
                string url = Server.MapPath(img_dir + name);
                StreamToFile(s, url);
                //upload.SaveAs(url);
                real_url += name;
            }
            else
            {
                real_url = "";
            }

            return real_url;
        }

        public enum ResizeType
        {
            Auto,
            Height,
            Width,
            Fixed
        }

        public static Image ResizeImage(Image imgToResize, Size size, ResizeType rt = ResizeType.Auto)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            int destWidth = size.Width;
            int destHeight = size.Height;

            if (rt != ResizeType.Fixed)
            {
                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                if (rt == ResizeType.Height)
                {
                    nPercent = nPercentH;
                }
                else if (rt == ResizeType.Width)
                {
                    nPercent = nPercentW;
                }
                else
                {
                    if (nPercentH < nPercentW)
                        nPercent = nPercentH;
                    else
                        nPercent = nPercentW;
                }

                if (nPercent > 1.0f)
                    nPercent = 1.0f;

                destWidth = (int)Math.Round(((float)sourceWidth * nPercent));
                destHeight = (int)Math.Round(((float)sourceHeight * nPercent));
            }
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static Stream CompressionImage(Image img, int quality, ImageFormat img_format)
        {
            Stream s = new MemoryStream();

            ImageCodecInfo code = ImageCodecInfo.GetImageDecoders().FirstOrDefault(f => f.FormatID == img_format.Guid);
            if (code == null)
            {
                return null;
            }

            EncoderParameters myEncoderParameter = new EncoderParameters(1);
            myEncoderParameter.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            img.Save(s, code, myEncoderParameter);
            s.Position = 0;
            return s;
        }

        public static string Format_String(int target, int length)
        {
            string format = "";
            for (int i = 0; i < length; i++)
            {
                format += "0";
            }
            return string.Format("{0:" + format + "}", target);
        }
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            if (Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool[] Get_FunctionCompetence(int id)
        {
            return new bool[3] { true, false, true };
        }
    }
}