using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Helpers;
using s26web.Models;
namespace s26web.Areas.shb.Models
{
    public class MobileAPIModel
    {
        //const String Username = "0910490555";
        //const String Password = "illuma";
        const String Username = "dcitr";
        const String Password = "dcsms%23234";
        const int Method = 1;
        public class MobileModel
        {
            public bool isSuccess { get; set; }
            public string error_code { get; set; }
            public string error_msg { get; set; }
            public DateTime createTime { get; set; }
        }
        #region 轉換BIG5
          /// <summary>
          /// 轉換BIG5
          /// </summary>
          /// <param name=”strUtf”>輸入UTF-8</param>
          /// <returns></returns>
          public static string ConvertBig5(string strUtf)
          {
           Encoding utf81 = Encoding.GetEncoding("utf-8");
           Encoding big51 = Encoding.GetEncoding("big5");
           //Response.ContentEncoding = big51;
           byte [] strUtf81 = utf81.GetBytes(strUtf.Trim());
           byte [] strBig51 = Encoding.Convert(utf81, big51, strUtf81);
 
           char[] big5Chars1 = new char[big51.GetCharCount(strBig51, 0, strBig51.Length)];
           big51.GetChars(strBig51, 0, strBig51.Length, big5Chars1, 0);
           string tempString1 = new string(big5Chars1);
           return tempString1;
          }
        #endregion
        public static int getSMSPoint()
        {
            s26web.Models.s26webDataContext db = new s26web.Models.s26webDataContext();
            int point = 0;
            var result = db.SMS.Where(w=>w.sms_id.Equals("000")).OrderByDescending(o => o.CreateTime).FirstOrDefault();
            List<string> sms_content = new List<string>();
            if(result != null)
                sms_content = result.content.Split('|').ToList();
            point = Convert.ToInt32(sms_content.LastOrDefault());
            return point;
        }
        public static MobileModel Sale_SMS(int Id, String mobile, int collected, DateTime ExcTime, string detail = null)
        {
            s26webDataContext db = new s26webDataContext();
            IntroductionModel intro = new IntroductionModel();
            var msg = intro.Get_One(Id);
            var data = db.Volunteers.FirstOrDefault(f => f.Mobile == mobile);

            /////////////////////
            msg.Content = msg.Content.Replace("<--end_time-->", ExcTime.AddHours(8).ToString("yyyy/MM/dd HH:mm"));
            msg.Content = msg.Content.Replace("<--collected-->", collected.ToString());
            /////////////////////
            var mm = SMS_Send(msg, mobile);
            SMS sms = new SMS
            {
                sms_id = mm.error_code,
                content = mm.error_msg,
                CreateTime = DateTime.UtcNow,
                mobile = mobile,
                detail = detail
            };
            db.SMS.InsertOnSubmit(sms);
            db.SubmitChanges();
            db.Connection.Close();
            //return the response
            return mm;
        }
        public static MobileModel SMS_Send(IntroductionModel.IntroductionModelShow msg = null, string mobile = null)
        {
            HttpWebRequest request;
            //----三竹----//
            /*
            //string postData = "";         
            string msg_Convert = HttpUtility.UrlEncode(msg.Content, Encoding.GetEncoding("big5")).Replace("+"," ");
            request = (HttpWebRequest)WebRequest.Create("http://smexpress.mitake.com.tw:7003/SpLmGet?username=" + Username + "&password=" + Password + "&dstaddr=" + mobile + "&smbody=" + msg_Convert);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();            
            stream.Close();

            MobileModel mm = new MobileModel();
            if (text.Contains("statuscode=1"))
            {
                mm.isSuccess = true;
            }
            db.Connection.Close();            
            */
            //----三竹end----//

            //---old----//
            request = (HttpWebRequest)WebRequest.Create("http://sms-get.com/api_send.php?username=" + Username + "&password=" + Password + "&method=" + Method + "&sms_msg=" + msg.Content + "&phone=" + mobile);
            // HttpWebRequest class is used to Make a request to a Uniform Resource Identifier (URI).  
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentType property of the WebRequest. 
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            MobileModel mm = new MobileModel();
            foreach (var p in Json.Decode(text))
            {
                if (p.Key == "stats")
                    mm.isSuccess = p.Value;
                else if (p.Key == "error_code")
                    mm.error_code = p.Value;
                else
                    mm.error_msg = p.Value;
            }
            return mm;
            //---old----//            
        }
        public static MobileModel sendToServer(int Id, String mobile, string pwd = null, int signupId = 0, string OrdersStates = "",int InvoiceId = 0,string detail = null)
        {
            s26webDataContext db = new s26webDataContext();
            IntroductionModel intro = new IntroductionModel();
            var msg = intro.Get_One(Id);
            var data = db.Volunteers.FirstOrDefault(f => f.Mobile == mobile);

            /////////////////////
            if (OrdersStates != "")
                if (msg.Content.IndexOf("<--orders_status-->") != -1)
                    msg.Content = msg.Content.Replace("<--orders_status-->", OrdersStates);
            if (msg.Content.IndexOf("<--user_id-->") != -1)
                msg.Content = msg.Content.Replace("<--user_id-->", mobile);
            if (msg.Content.IndexOf("<--user_password-->") != -1)
                msg.Content = msg.Content.Replace("<--user_password-->", pwd);
            if (msg.Content.IndexOf("<--user_name-->") != -1)
                msg.Content = msg.Content.Replace("<--user_name-->", data.Name);
            if (msg.Content.IndexOf("<--user_mobile-->") != -1)
                msg.Content = msg.Content.Replace("<--user_mobile-->", data.Mobile);
            
            //課程
            //if (signupId != 0)
            //{
            //    var signups = (from c in db.Course
            //                   join s in db.SignUp on c.Id equals s.CourseId
            //                   join city in db.City on c.CityId equals city.Id
            //                   where s.Id == signupId
            //                   select new { course = c, signup = s, city = city }).FirstOrDefault();
            //    db.SignUp.FirstOrDefault(f => f.Id == signupId);
            //    if (msg.Content.IndexOf("<--course_name-->") != -1)
            //        msg.Content = msg.Content.Replace("<--course_name-->", signups.signup.Name);
            //    if (msg.Content.IndexOf("<--course_title-->") != -1)
            //        msg.Content = msg.Content.Replace("<--course_title-->", signups.course.Title);
            //    if (msg.Content.IndexOf("<--course_count-->") != -1)
            //        msg.Content = msg.Content.Replace("<--course_count-->", signups.signup.SignUpNumbers.ToString());
            //    if (msg.Content.IndexOf("<--course_address-->") != -1)
            //        msg.Content = msg.Content.Replace("<--course_address-->", signups.city.Name + signups.course.Address);
            //    if (msg.Content.IndexOf("<--course_time-->") != -1)
            //        msg.Content = msg.Content.Replace("<--course_time-->", signups.course.StartTime.AddHours(8).ToString("MM/dd(ddd) HH:mm") + "~" + signups.course.EndTime.AddHours(8).ToString("HH:mm"));
            //}

            //發票
            //if (InvoiceId != 0)
            //{
            //    var inv = db.Invoice.FirstOrDefault(f => f.Id == InvoiceId);
            //    if (inv.FailChoice == 0)
            //        inv.FailReason = "發票無法辨識";
            //    else if (inv.FailChoice == 1)
            //        inv.FailReason = "發票號碼重複";
            //    if (msg.Content.IndexOf("<--fail-->") != -1)
            //        msg.Content = msg.Content.Replace("<--fail-->", inv.FailReason);
            //}

            var mm = SMS_Send(msg, mobile);
            if (detail == "SaleAct")
            {
                //msg.Content = msg.Content.Replace("",);
            }
            SMS sms = new SMS
            {
                sms_id = mm.error_code,
                content = mm.error_msg,
                CreateTime = DateTime.UtcNow,
                mobile = mobile,
                detail = detail
            };
            db.SMS.InsertOnSubmit(sms);
            db.SubmitChanges();
            db.Connection.Close();
            //return the response
            return mm;
            
        }
    }
}