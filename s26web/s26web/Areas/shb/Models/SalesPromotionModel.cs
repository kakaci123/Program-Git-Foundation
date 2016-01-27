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
    public class SalesPromotionModel
    {
        public class SalesPromotionModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("活動代碼")]
            [Required]
            public string Code { get; set; }
            [DisplayName("活動名稱")]
            [Required]
            public string Name { get; set; }
            [DisplayName("序號到期日")]
            [Required]
            public DateTime Deadline { get; set; }
            [DisplayName("發放點數")]
            [Required]
            public int Point { get; set; }
            [DisplayName("資料建立時間")]
            public DateTime CreateTime { get; set; }
        }

        #region Get
        public List<SalesPromotionModelShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                var data = Get().OrderByDescending(o=>o.Id).Skip((p - 1) * take).Take(take);
                List<SalesPromotionModelShow> item = Convert(data.ToList());
                db.Connection.Close();
                return item;
            }
            catch { return new List<SalesPromotionModelShow>(); }
        }

        public IQueryable<SalesPromotion> Get()
        {
            s26webDataContext db = new s26webDataContext();
            IQueryable<SalesPromotion> data = db.SalesPromotion;
            db.Connection.Close();
            return data;
        }

        public List<SalesPromotionModelShow> Convert(List<SalesPromotion> item)
        {
            
            List<SalesPromotionModelShow> result = new List<SalesPromotionModelShow>();
            foreach (var i in item)
            {
                var Sales = new SalesPromotionModelShow
                {
                    Id = i.Id,
                    Code = i.Code,
                    Name = i.Name,
                    Deadline = i.Deadline,
                    Point = i.Point,
                    CreateTime = i.CreateTime.AddHours(8),
                };
                result.Add(Sales);
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

        public SalesPromotionModelShow Get_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            SalesPromotionModelShow data = new SalesPromotionModelShow();

            var item = db.SalesPromotion.FirstOrDefault(f => f.Id == id);
            data.Code = item.Code;
            data.Name = item.Name;
            data.Point = item.Point;
            data.Deadline = item.Deadline.AddHours(8);
            data.CreateTime = item.CreateTime.AddHours(8);
            db.Connection.Close();
            return data;
        }
        #endregion

        #region Insert
        public int Insert(SalesPromotionModelShow item)
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                SalesPromotion new_item = new SalesPromotion
                {
                    Code = item.Code,
                    Name = item.Name,
                    Point = item.Point,
                    Deadline = item.Deadline,
                    CreateTime = DateTime.UtcNow
                };
                db.SalesPromotion.InsertOnSubmit(new_item);
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
        public int Update(SalesPromotionModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.SalesPromotion.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
                    data.Code = item.Code;
                    data.Name = item.Name;
                    data.Point = item.Point;
                    data.Deadline = item.Deadline;                    
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

        #region Delete

        public void Delete(int Id)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.SalesPromotion.Where(w => w.Id == Id);
            if (data.Any())
            {
                db.SalesPromotion.DeleteAllOnSubmit(data);
                db.SubmitChanges();
            }
            db.Connection.Close();
        }
        #endregion
    }
}