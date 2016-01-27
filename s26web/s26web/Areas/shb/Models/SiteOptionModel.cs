using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using s26web.Models;

namespace s26web.Areas.shb.Models
{
    public class SiteOptionModel
    {
        public class SiteOptionShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            [DisplayName("網站名稱")]
            public string WebTitle { get; set; }
            [DisplayName("網站Icon")]
            public string WebIcon { get; set; }
            [DisplayName("網站Logo(提示:將自動縮放為寬165px高75px之png格式)")]
            public string WebLogo { get; set; }
            [DisplayName("SEO:網站關鍵字(請用半形逗號,隔開)")]
            [Required(ErrorMessage = "必填欄位")]
            public string WebSEO { get; set; }
            [DisplayName("SEO:網站描述")]
            [Required(ErrorMessage = "必填欄位")]
            public string WebDescription { get; set; }
            [DisplayName("SMTP Server位置")]
            [Required(ErrorMessage = "必填欄位")]
            public string SMTP_Server { get; set; }
            [DisplayName("SMTP 是否需要帳密")]
            [Required(ErrorMessage = "必填欄位")]
            public bool SMTP_Login { get; set; }
            [DisplayName("SMTP 是否使用SSL")]
            [Required(ErrorMessage = "必填欄位")]
            public bool SMTP_SSL { get; set; }
            [DisplayName("SMTP Server埠號")]
            [Required(ErrorMessage = "必填欄位")]
            public int SMTP_Port { get; set; }
            [DisplayName("SMTP 帳號")]
            public string SMTP_Account { get; set; }
            [DisplayName("SMTP 密碼")]
            [DataType(DataType.Password)]
            public string SMTP_Password { get; set; }
            [DisplayName("客服信箱(寄件者信箱)")]
            [Required(ErrorMessage = "必填欄位")]
            [DataType(DataType.EmailAddress)]
            //[EmailAddress(ErrorMessage = "請輸入正確電子信箱。")]
            public string Service_Mail { get; set; }
            [DisplayName("寄件者姓名")]
            [Required(ErrorMessage = "必填欄位")]
            public string FromName { get; set; }

            [DisplayName("每日匯出資料收件者信箱")]
            [Required(ErrorMessage = "必填欄位")]
            public string ExportMail { get; set; }


            [DisplayName("訂單客服信箱(包含訂單三日未更改)")]
            [Required(ErrorMessage = "必填欄位")]
            public string OrdersMail { get; set; }

            [DisplayName("iOS版本號")]
            [Required(ErrorMessage = "必填欄位")]
            public string iOS_Version { get; set; }
            [DisplayName("Android版本號")]
            [Required(ErrorMessage = "必填欄位")]
            public string Android_Version { get; set; }
        }

        public SiteOptionModel()
        {
        }

        public SiteOptionShow Get()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.WebConfig.FirstOrDefault();
            db.Connection.Close();
            if (data != null)
            {
                return new SiteOptionShow
                {
                    Id = data.Id,
                    WebTitle = data.WebTitle,
                    WebIcon = data.WebIcon,
                    WebLogo = Method.RootPath + data.WebLogo,
                    WebSEO = data.WebSEO,
                    WebDescription = data.WebDescription,
                    SMTP_Server = data.SMTP_Server,
                    SMTP_Port = data.SMTP_Port,
                    SMTP_Login = data.SMTP_Login,
                    SMTP_SSL = data.SMTP_SSL,
                    SMTP_Account = data.SMTP_Account,
                    SMTP_Password = data.SMTP_Password,
                    Service_Mail = data.Service_Mail,
                    FromName = data.FromName,
                    ExportMail = data.Export_Mail,
                    OrdersMail = data.OrdersMail,
                    iOS_Version = data.iOS_Version,
                    Android_Version = data.Android_Version
                };
            }
            return new SiteOptionShow();
        }

        public int Set(SiteOptionShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.WebConfig.FirstOrDefault();
                if (data != null)
                {
                    if (item.Android_Version != null)
                    {
                        data.Android_Version = item.Android_Version;
                    }
                    if (item.iOS_Version != null)
                    {
                        data.iOS_Version = item.iOS_Version;
                    }
                    data.WebTitle = item.WebTitle;
                    if (item.WebLogo != "")
                    {
                        data.WebLogo = item.WebLogo;
                    }
                    if (item.WebIcon != "")
                    {
                        data.WebIcon = item.WebIcon;
                    }
                    data.WebSEO = item.WebSEO;
                    data.WebDescription = item.WebDescription;
                    data.SMTP_Server = item.SMTP_Server;
                    data.SMTP_Port = item.SMTP_Port;
                    data.SMTP_Login = item.SMTP_Login;
                    data.SMTP_SSL = item.SMTP_SSL;
                    data.SMTP_Account = item.SMTP_Account;
                    data.SMTP_Password = item.SMTP_Password;
                    data.Service_Mail = item.Service_Mail;
                    data.FromName = item.FromName;
                    data.Export_Mail = item.ExportMail;
                    data.OrdersMail = item.OrdersMail;
                    db.SubmitChanges();
                    db.Connection.Close();
                    return data.Id;
                }
                else
                {
                    WebConfig wc = new WebConfig
                    {
                        WebTitle = item.WebTitle,
                        WebLogo = item.WebLogo,
                        WebIcon = item.WebIcon,
                        WebSEO = item.WebSEO,
                        WebDescription = item.WebDescription,
                        SMTP_Server = item.SMTP_Server,
                        SMTP_Port = item.SMTP_Port,
                        SMTP_SSL= item.SMTP_SSL,
                        SMTP_Login = item.SMTP_Login,
                        SMTP_Account = item.SMTP_Account,
                        SMTP_Password = item.SMTP_Password,
                        Service_Mail = item.Service_Mail,
                        FromName = item.FromName,
                        Export_Mail = item.ExportMail
                    };
                    db.WebConfig.InsertOnSubmit(wc);
                    db.SubmitChanges();
                    db.Connection.Close();
                    return wc.Id;
                }
            }
            catch { }
            return 0;
        }

        public static string Get_Title()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.WebConfig.FirstOrDefault();
            db.Connection.Close();
            if (data != null)
            {
                return data.WebTitle;
            }
            return "";
        }

        public static string Get_Logo()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.WebConfig.FirstOrDefault();
            db.Connection.Close();
            if (data != null)
            {
                return Method.RootPath+data.WebLogo;
            }
            return "";
        }

        public static string Get_Icon()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.WebConfig.FirstOrDefault();
            db.Connection.Close();
            if (data != null)
            {
                return Method.RootPath + data.WebIcon;
            }
            return "";
        }

        public static string Get_WebSEO()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.WebConfig.FirstOrDefault();
            db.Connection.Close();
            if (data != null)
            {
                return data.WebSEO;
            }
            return "";
        }

        public static string Get_WebDescription()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.WebConfig.FirstOrDefault();
            db.Connection.Close();
            if (data != null)
            {
                return data.WebDescription;
            }
            return "";
        }
        public string Get_ExportMail()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.WebConfig.FirstOrDefault();
            db.Connection.Close();
            if (data != null)
            {
                return data.Export_Mail;
            }
            return "";
        }
    }
}