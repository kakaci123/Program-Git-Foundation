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
    public class ProductModel
    {
        public class ProductModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("產品名稱")]
            public string Name { get; set; }
            [DisplayName("組合內容")]
            public string Content { get; set; }
            [DisplayName("產品圖片")]
            public string PictureLink { get; set; }
            [DisplayName("價錢")]
            public int  Price { get; set; }
            [DisplayName("贈品點數")]
            public int  Point { get; set; }
            [DisplayName("限購組數")]
            public int Limit { get; set; }
            [DisplayName("前台顯示")]
            public bool Display { get; set; }
            [DisplayName("新增時間")]
            public DateTime CreateTime { get; set; }
            [DisplayName("更新時間")]
            public DateTime UpdateTime { get; set; }
            [DisplayName("最後更新者")]
            public int? LastUserId { get; set; }
            [DisplayName("產品連結")]
            public string Link { get; set; }
            [DisplayName("備註")]
            public string Memo { get; set; }
            [DisplayName("產品內容圖片")]
            public string ContentPic { get; set; }
        }

        //public static NewsConfig Get_Config(int fun_id)
        //{
        //    s26webDataContext db = new s26webDataContext();
        //    var data = db.NewsConfig.FirstOrDefault(f => f.Fun_Id == fun_id);
        //    db.Connection.Close();
        //    return data == null ? new NewsConfig() : data;
        //}

        //public ProductModel(int fun)
        //{
        //    Fun_Id = fun;
        //    config = Get_Config(fun);
        //}

        //public NewsConfig config = new NewsConfig();
        //public string Keyword = "";
        //public bool Home_Show = false;
        //public string Sort = "update";
        //public DateTime? create_time_start = null;
        //public DateTime? create_time_end = null;
        //public DateTime? update_time_start = null;
        //public DateTime? update_time_end = null;
        //public List<int> Cid=new List<int>();
        //private int Fun_Id;

        //public void Clear_Params()
        //{
        //    Keyword = "";
        //    Sort = "update";
        //    Home_Show = false;
        //    Cid = new List<int>();
        //    create_time_start = null;
        //    create_time_end = null;
        //    update_time_start = null;
        //    update_time_end = null;
        //}
        public List<ProductModelShow> Convert(List<Product> item)
        {
            List<ProductModelShow> result = new List<ProductModelShow>();
            foreach (var i in item)
            {
                var product = new ProductModelShow
                {
                    Id = i.Id,
                    Name = i.Name,
                    Content = i.Content,
                    PictureLink = i.PictureLink,
                    Price = i.Price,
                    Point = i.Point,
                    Limit = i.Limit,
                    Display = i.Display,
                    CreateTime = i.CreateTime.AddHours(8),
                    UpdateTime = i.UpdateTime.AddHours(8),
                    LastUserId = i.LastUserId,
                    Link = i.Link,
                    Memo = i.Memo,
                    ContentPic = i.ContentPic
                };
                result.Add(product);
            }
            return result;
        }
        public List<ProductModelShow> Get_Data()
        {
            s26webDataContext db = new s26webDataContext();
            var product = db.Product.ToList();
            return Convert(product);
        }
        public List<ProductModelShow> Get_DisplayData()
        {
            s26webDataContext db = new s26webDataContext();
            var product = db.Product.Where(w => w.Display==true).ToList();
            return Convert(product);
        }

        public Method.Paging Get_Page(int p = 1, int take = 10)
        {
            return Method.Get_Page(Get_Count(), p, take);
        }

        public int Get_Count()
        {
            return Get_Data().Count();
        }

        public ProductModelShow Get_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            ProductModelShow data = new ProductModelShow();
            try
            {
                var item = db.Product.FirstOrDefault(f => f.Id == id);

                data.Name = item.Name;
                data.Content = item.Content;
                data.PictureLink = item.PictureLink;
                data.Price = item.Price;
                data.Point = item.Point;
                data.Limit = item.Limit;
                data.Display = item.Display;
                data.CreateTime = item.CreateTime.AddHours(8);
                data.UpdateTime = item.UpdateTime.AddHours(8);
                data.Link = item.Link;
                data.Memo = item.Memo;
                data.ContentPic = item.ContentPic;

                db.Connection.Close();
                return data;
            }
            catch
            {
                return null;
            }
        }

        #region update
        public int Update(ProductModelShow item, int LastUserId, HttpPostedFileBase file, string vid, HttpServerUtilityBase Server, HttpPostedFileBase ContentPic)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Product.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
                    data.Content = item.Content;

                    if (file != null)
                        if (file.ContentLength > 0 && file.ContentType.ToLower() == "image/jpeg" || file.ContentType == "image/png")
                            data.PictureLink = Method.Upload_File(file, vid, Server);
                        else
                            data.PictureLink = "";
                    if (ContentPic != null)
                        if (ContentPic.ContentLength > 0 && ContentPic.ContentType.ToLower() == "image/jpeg" || ContentPic.ContentType == "image/png")
                            data.ContentPic = Method.Upload_File(ContentPic, vid, Server);
                        else
                            data.ContentPic = "";

                    data.Price = item.Price;
                    data.Point = item.Point;
                    data.Limit = item.Limit;
                    data.Display = item.Display;
                    data.UpdateTime = DateTime.UtcNow;
                    data.LastUserId = LastUserId;
                    data.Link = item.Link;
                    data.Memo = item.Memo;
                    
                    db.SubmitChanges();
                    db.Connection.Close();
                    return data.Id;
                }
                db.Connection.Close();
                return -1;
            }
            catch { return -1; }
        }
        #endregion
    }
}