using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using s26web.Models;
using System.Web.Mvc;

namespace s26web.Areas.shb.Models
{
    public class IntroductionModel
    {
        public class IntroductionModelShow
        {
            [Key]
            public int Id { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            [DisplayName("標題")]
            public string Title { get; set; }

            [DisplayName("內容")]
            [DataType(DataType.MultilineText)]
            public string Content { get; set; }

            [DisplayName("建立時間")]
            public DateTime CreateTime { get; set; }
            [DisplayName("更新時間")]
            public DateTime UpdateTime { get; set; }
        }

        public static string Get_Title(int id)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Introduction.FirstOrDefault(f => f.Id == id);
                db.Connection.Close();
                if (data != null)
                {
                    return data.Title;
                }
            }
            catch { }
            return "";
        }

        public IntroductionModelShow Get_One(int id)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Introduction.FirstOrDefault(f => f.Id == id);
                if (data != null)
                {
                    return new IntroductionModelShow
                    {
                        Id = data.Id,
                        Title = data.Title,
                        Content = HttpUtility.HtmlDecode(data.Content),
                        CreateTime = data.CreateTime.AddHours(8),
                        UpdateTime = data.UpdateTime.AddHours(8)
                    };
                }
                return null;
            }
            catch { return null; }
        }

        public int Set(IntroductionModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                if (item.Id > 0)
                {
                    var data = db.Introduction.FirstOrDefault(f => f.Id == item.Id);
                    if (data != null)
                    {
                        data.Title = item.Title;
                        data.Content = item.Content;
                        data.UpdateTime = DateTime.UtcNow;
                        db.SubmitChanges();
                        db.Connection.Close();
                        return item.Id;
                    }
                }
                db.Connection.Close();
                return -1;
            }
            catch { return -1; }
        }
        public static string Get_FrontTitle_Ajax(int categoryId, int fronttilte, bool all)
        {
            var data = Get_FrontTitle_Select(categoryId, fronttilte, all);
            string result = "";
            foreach (var i in data)
            {
                result += "<option value=\"" + i.Value + "\" " + (i.Selected ? "selected=\"selected\"" : "") + ">" + i.Text + "</option>";
            }
            return result;
        }

        public static List<SelectListItem> Get_FrontTitle_Select(int categoryId, int fronttitle, bool all)
        {
            s26webDataContext db = new s26webDataContext();
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Selected = fronttitle == 0,
                Text = all ? "全部" : "請選擇",
                Value = "0"
            });
            if (categoryId == 0)
            {
                return data;
            }
            db.Connection.Close();
            return data;
        }
    }
}