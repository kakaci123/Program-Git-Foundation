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
    public class GiftModel
    {
        public class GiftModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("可兌換會員類型")]
            public string MemberType { get; set; }
            [DisplayName("贈品名稱")]
            public string Name { get; set; }
            [DisplayName("兌換點數")]
            public int Point { get; set; }
            [DisplayName("贈品數量")]
            public int Amount { get; set; }
            [DisplayName("贈品圖片")]
            public string Picture { get; set; }
            [DisplayName("前端是否顯示")]
            public bool Display { get; set; }
            [DisplayName("資料建立時間")]
            public DateTime CreateTime { get; set; }
            public List<Category> membertype_list { get; set; }
            public List<Gift> membertype_check { get; set; }
        }
        public List<Category> Get_MemberType()
        {
            s26webDataContext db = new s26webDataContext();
            GiftModelShow data = new GiftModelShow();
            data.membertype_list = db.Category.Where(w => w.Fun_Id == 2).ToList();
            db.Connection.Close();
            return data.membertype_list;
        }

        public List<GiftModelShow> Get_Data()
        {
            s26webDataContext db = new s26webDataContext();
            var gift = Convert(db.Gift.OrderByDescending(o => o.Id).ToList());
            foreach(var i in gift)
            {
                string[] typestr= i.MemberType.Split(',');
                string list = "";
                string item = "";
                foreach(var j in typestr)
                {
                if (j != "") 
                {
                    int typeint = int.Parse(j);
                    
                    if(typeint == 1)
                    { item = "懷孕中"; }
                    else if (typeint == 2)
                    { item = "使用S-26產品"; }
                    else if (typeint == 3)
                    { item = "全母乳餵哺"; }
                    else if (typeint == 4)
                    { item = "其他"; }
                    
                    //list += db.Category.FirstOrDefault(f => f.Id == typeint).Name + ",";
                }
                list += item + ",";
                }
                i.MemberType = list;
            };
            db.Connection.Close();
            return gift;
        }
        public List<GiftModelShow> Convert(List<Gift> item)
        {
            
            List<GiftModelShow> result = new List<GiftModelShow>();
            foreach (var i in item)
            {
                var product = new GiftModelShow
                {
                    Id = i.Id,
                    MemberType = i.MemberType,
                    Name = i.Name,
                    Point = i.Point,
                    Amount = i.Amount,
                    Picture = i.Picture,
                    Display = i.Display,
                    CreateTime = i.CreateTime.AddHours(8),
                };
                result.Add(product);
            }
            return result;
        }
        public Method.Paging Get_Page(int p = 1, int take = 10)
        {
            return Method.Get_Page(Get_Count(), p, take);
        }

        public int Get_Count()
        {
            return Get_Data().Count();
        }

        public GiftModelShow Get_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            GiftModelShow data = new GiftModelShow();

            var item = db.Gift.FirstOrDefault(f => f.Id == id);
            data.MemberType = item.MemberType;
            data.Name = item.Name;
            data.Point = item.Point;
            data.Amount = item.Amount;
            data.Picture = item.Picture;
            data.Display = item.Display;
            data.CreateTime = item.CreateTime.AddHours(8);
            db.Connection.Close();
            return data;
        }
        #region Insert
        public int Insert(GiftModelShow item, HttpPostedFileBase file, string vid, HttpServerUtilityBase Server)
        {
            var Picture = "";
            s26webDataContext db = new s26webDataContext();
            if (file != null)
                if (file.ContentLength > 0 && file.ContentType.ToLower() == "image/jpeg" || file.ContentType == "image/png")
                    Picture = Method.Upload_File(file, vid, Server);
                else
                    Picture = "";
            try
            {
                Gift new_item = new Gift
                {
                    MemberType = item.MemberType,
                    Name = item.Name,
                    Point = item.Point,
                    Amount = item.Amount,
                    Picture = Picture,
                    Display = item.Display,
                    CreateTime = DateTime.UtcNow
                };
                db.Gift.InsertOnSubmit(new_item);
                db.SubmitChanges();
                db.Connection.Close();
                return new_item.Id;
            }
            catch
            {
                return -1;
            }
        }   
        #endregion

        #region update
        public int Update(GiftModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Gift.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
                    data.MemberType = item.MemberType;
                    data.Name = item.Name;
                    data.Point = item.Point;
                    data.Amount = item.Amount;
                    if(item.Picture!= null)
                    {
                        data.Picture = item.Picture;
                    }
                    data.Display = item.Display;
                    
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