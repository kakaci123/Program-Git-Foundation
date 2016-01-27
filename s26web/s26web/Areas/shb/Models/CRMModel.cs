using s26web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace s26web.Areas.shb.Models
{
    public class CRMModel
    {
        public class CRMMemberInfo
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("類別名稱")]
            public string CategoryName { get; set; }
            [DisplayName("客戶人員名稱")]
            public string UserName { get; set; }
            public string[] UserNamelist { get; set; }
            [DisplayName("FB首頁網址")]
            public string FBUrl { get; set; }
            public string[] FBUrllist { get; set; }
        }

        public List<CRMMemberInfo> Get_Data()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.eCRM;
            List<CRMMemberInfo> mm = new List<CRMMemberInfo>();
            foreach (var i in data)
            {
                mm.Add(new CRMMemberInfo
                {
                    Id = i.Id,
                    CategoryName = i.CategoryName,
                    FBUrl = i.FBUrl,
                    UserName = i.UserName
                });
            }
            db.Connection.Close();
            return mm;
        }

        #region update

        public void Update(string[] UserName, string[] FBUrl)
        {
            s26webDataContext db = new s26webDataContext();
            for (int i = 0; i < UserName.Count(); i++)
            {
                var data = db.eCRM.FirstOrDefault(f => f.Id == (i + 1));
                data.UserName = UserName[i];
                data.FBUrl = FBUrl[i];
            }
            db.SubmitChanges();
            db.Connection.Close();
        }

        #endregion
    }
}
    