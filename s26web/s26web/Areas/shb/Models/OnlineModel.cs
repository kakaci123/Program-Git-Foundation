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
    public class OnlineModel
    {
        public class OnlineModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("活動身分限制")]
            [Required]
            public string Type { get; set; }
            [DisplayName("活動名稱")]
            [Required]
            public string Name { get; set; }
            [DisplayName("活動內容")]
            [Required]
            public string Content { get; set; }
            [DisplayName("Banner圖片")]
            [Required]
            public string Banner_PC { get; set; }
            [DisplayName("Banner圖片")]
            [Required]
            public string Banner_Mobile { get; set; }
            [DisplayName("活動連結")]
            [Required]
            public string Url { get; set; }
            [DisplayName("前端是否顯示")]
            public bool Display { get; set; }
            [DisplayName("上線時間")]
            [Required]
            public DateTime StartTime { get; set; }
            [DisplayName("下線時間")]
            public DateTime EndTime { get; set; }
            [DisplayName("更新時間")]
            public DateTime UpdateTime { get; set; }

            public List<Category> type_list { get; set; }
            public List<Online> type_check { get; set; }
        }

        #region Get 後台
        public List<OnlineModelShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                var data = Get().OrderByDescending(o => o.Id).Skip((p - 1) * take).Take(take);
                foreach (var i in data)
                {
                    string[] typestr = i.Type.Split(',');
                    string list = "";
                    foreach (var j in typestr)
                    {
                        if (j != "")
                        {
                            int typeint = int.Parse(j);
                            list += db.Category.FirstOrDefault(f => f.Id == typeint).Name + ",";
                        }
                    }
                    i.Type = list;
                };

                List<OnlineModelShow> item = Convert(data.ToList());
                db.Connection.Close();
                return item;
            }
            catch { return new List<OnlineModelShow>(); }
        }

        public IQueryable<Online> Get()
        {
            s26webDataContext db = new s26webDataContext();
            IQueryable<Online> data = db.Online;
            db.Connection.Close();
            return data;
        }

        public List<OnlineModelShow> Convert(List<Online> item)
        {

            List<OnlineModelShow> result = new List<OnlineModelShow>();
            foreach (var i in item)
            {
                if (i.EndTime != null)
                {
                    var Sales = new OnlineModelShow
                    {
                        Id = i.Id,
                        Type = i.Type,
                        Name = i.Name,
                        Content = HttpUtility.HtmlDecode(i.Content),
                        Banner_PC = i.Banner_PC,
                        Banner_Mobile = i.Banner_Mobile,
                        Url = i.Url,
                        Display = i.Display,
                        StartTime = i.StartTime.AddHours(8),
                        EndTime = i.EndTime.Value.AddHours(8),
                        UpdateTime = i.UpdateTime.AddHours(8),
                    };
                    result.Add(Sales);
                }
                else
                {
                    var Sales = new OnlineModelShow
                    {
                        Id = i.Id,
                        Type = i.Type,
                        Name = i.Name,
                        Content = HttpUtility.HtmlDecode(i.Content),
                        Banner_PC = i.Banner_PC,
                        Banner_Mobile = i.Banner_Mobile,
                        Url = i.Url,
                        Display = i.Display,
                        StartTime = i.StartTime.AddHours(8),
                        UpdateTime = i.UpdateTime.AddHours(8),
                    };
                    result.Add(Sales);
                }

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

        public OnlineModelShow Get_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            OnlineModelShow data = new OnlineModelShow();

            var item = db.Online.FirstOrDefault(f => f.Id == id);
            data.Type = item.Type;
            data.Name = item.Name;
            data.Content = HttpUtility.HtmlDecode(item.Content);
            data.Banner_PC = item.Banner_PC;
            data.Banner_Mobile = item.Banner_Mobile;
            data.Url = item.Url;
            data.Display = item.Display;
            data.StartTime = item.StartTime.AddHours(8);
            if (item.EndTime != null)
            {
                data.EndTime = item.EndTime.Value.AddHours(8);
            }
            data.UpdateTime = item.UpdateTime.AddHours(8);
            db.Connection.Close();
            return data;
        }
        #endregion

        #region Get 前台
        public List<OnlineModelShow> Get_Data_Front(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                var data = Get_Front().OrderByDescending(o => o.Id);
                //foreach (var i in data)
                //{
                //    string[] typestr = i.Type.Split(',');
                //    string list = "";
                //    foreach (var j in typestr)
                //    {
                //        if (j != "")
                //        {
                //            int typeint = int.Parse(j);
                //            list += db.Category.FirstOrDefault(f => f.Id == typeint).Name + ",";
                //        }
                //    }
                //    i.Type = list;
                //};

                List<OnlineModelShow> item = Convert_Front(data.ToList());
                db.Connection.Close();
                return item;
            }
            catch { return new List<OnlineModelShow>(); }
        }

        public IQueryable<Online> Get_Front()
        {
            s26webDataContext db = new s26webDataContext();
            DateTime Now = DateTime.UtcNow.AddHours(8);

            //前台 - 條件為顯示，且今天日期小於上線時間
            IQueryable<Online> data = db.Online.Where(w => w.Display == true && Now >= w.StartTime.AddHours(8));

            db.Connection.Close();
            return data;
        }

        public List<OnlineModelShow> Convert_Front(List<Online> item)
        {

            List<OnlineModelShow> result = new List<OnlineModelShow>();
            foreach (var i in item)
            {
                if (i.EndTime != null)
                {
                    var Sales = new OnlineModelShow
                    {
                        Id = i.Id,
                        Type = i.Type,
                        Name = i.Name,
                        Content = HttpUtility.HtmlDecode(i.Content),
                        Banner_PC = i.Banner_PC,
                        Banner_Mobile = i.Banner_Mobile,
                        Url = i.Url,
                        Display = i.Display,
                        StartTime = i.StartTime.AddHours(8),
                        EndTime = i.EndTime.Value.AddHours(8),
                        UpdateTime = i.UpdateTime.AddHours(8),
                    };
                    result.Add(Sales);
                }
                else
                {
                    var Sales = new OnlineModelShow
                    {
                        Id = i.Id,
                        Type = i.Type,
                        Name = i.Name,
                        Content = HttpUtility.HtmlDecode(i.Content),
                        Banner_PC = i.Banner_PC,
                        Banner_Mobile = i.Banner_Mobile,
                        Url = i.Url,
                        Display = i.Display,
                        StartTime = i.StartTime.AddHours(8),
                        UpdateTime = i.UpdateTime.AddHours(8),
                    };
                    result.Add(Sales);
                }

            }
            return result;
        }



        public OnlineModelShow Get_One_Front(int id)
        {
            s26webDataContext db = new s26webDataContext();
            OnlineModelShow data = new OnlineModelShow();

            var item = db.Online.FirstOrDefault(f => f.Id == id);
            data.Type = item.Type;
            data.Name = item.Name;
            data.Content = HttpUtility.HtmlDecode(item.Content);
            data.Banner_PC = item.Banner_PC;
            data.Banner_Mobile = item.Banner_Mobile;
            data.Url = item.Url;
            data.Display = item.Display;
            data.StartTime = item.StartTime.AddHours(8);
            if (item.EndTime != null)
            {
                data.EndTime = item.EndTime.Value.AddHours(8);
            }
            data.UpdateTime = item.UpdateTime.AddHours(8);
            db.Connection.Close();
            return data;
        }
        #endregion

        #region Insert
        public int Insert(OnlineModelShow item)
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                //有輸入下線時間
                if (item.EndTime != DateTime.Parse("0001/1/1 上午 12:00:00"))
                {
                    Online new_item = new Online
                    {
                        Type = item.Type,
                        Name = item.Name,
                        Content = item.Content,
                        Banner_PC = item.Banner_PC,
                        Banner_Mobile = item.Banner_Mobile,
                        Url = item.Url,
                        Display = item.Display,
                        StartTime = item.StartTime.AddHours(-8),
                        EndTime = item.EndTime.AddHours(-8),
                        UpdateTime = DateTime.UtcNow
                    };

                    db.Online.InsertOnSubmit(new_item);
                    db.SubmitChanges();
                    db.Connection.Close();
                    return new_item.Id;
                }
                else
                {
                    Online new_item = new Online
                    {
                        Type = item.Type,
                        Name = item.Name,
                        Content = item.Content,
                        Banner_PC = item.Banner_PC,
                        Banner_Mobile = item.Banner_Mobile,
                        Url = item.Url,
                        Display = item.Display,
                        StartTime = item.StartTime.AddHours(-8),
                        UpdateTime = DateTime.UtcNow
                    };

                    db.Online.InsertOnSubmit(new_item);
                    db.SubmitChanges();
                    db.Connection.Close();
                    return new_item.Id;
                }
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region update
        public int Update(OnlineModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Online.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
                    data.Type = item.Type;
                    data.Name = item.Name;
                    data.Content = item.Content;
                    if (item.Banner_PC != null)
                    {
                        data.Banner_PC = item.Banner_PC;
                    }
                    if (item.Banner_Mobile != null)
                    {
                        data.Banner_Mobile = item.Banner_Mobile;
                    }
                    data.Url = item.Url;
                    data.Display = item.Display;
                    data.StartTime = item.StartTime.AddHours(-8);
                    if (item.EndTime != DateTime.Parse("0001/1/1 上午 12:00:00"))
                    {
                        data.EndTime = item.EndTime.AddHours(-8);
                    }
                    else
                    {
                        data.EndTime = null;
                    }
                    data.UpdateTime = DateTime.UtcNow;
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
            var data = db.Online.Where(w => w.Id == Id);
            if (data.Any())
            {
                db.Online.DeleteAllOnSubmit(data);
                db.SubmitChanges();
            }
            db.Connection.Close();
        }
        #endregion

        #region Upload
        public string Upload_Image(HttpPostedFileBase upload, HttpServerUtilityBase Server)
        {
            string real_url = "/upload/Online/";
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    if (!upload.ContentType.ToLower().StartsWith("image"))
                    {
                        return "";
                    }
                    string img_dir = "~" + real_url;
                    if (!System.IO.Directory.Exists(Server.MapPath(img_dir)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(img_dir));
                    }
                    if (!System.IO.Directory.Exists(Server.MapPath(img_dir.Replace("/File/", "/Middle/"))))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(img_dir.Replace("/File/", "/Middle/")));
                    }
                    if (!System.IO.Directory.Exists(Server.MapPath(img_dir.Replace("/File/", "/Small/"))))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(img_dir.Replace("/File/", "/Small/")));
                    }
                    string url = "";// Server.MapPath(img_dir + upload.FileName);
                    int count = 1;
                    string temp = "";
                    string time = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    //string name1 = time;// System.IO.Path.GetFileNameWithoutExtension(upload.FileName);
                    string name2 = System.IO.Path.GetExtension(upload.FileName);

                    do
                    {
                        temp = time + count + name2;
                        url = Server.MapPath(img_dir + temp);
                        count++;
                    } while (System.IO.File.Exists(url));
                    upload.SaveAs(url);
                    //upload.SaveAs(Server.MapPath(img_dir.Replace("/File/", "/Middle/") + temp));
                    //upload.SaveAs(Server.MapPath(img_dir.Replace("/File/", "/Small/") + temp));
                    real_url += temp;
                }
                else
                {
                    real_url = "";
                }
            }
            catch (Exception e)
            {
            }

            return real_url;
        }
        #endregion

        public List<Category> Get_Type()
        {
            s26webDataContext db = new s26webDataContext();
            OnlineModelShow data = new OnlineModelShow();
            data.type_list = db.Category.Where(w => w.Fun_Id == 10).ToList();
            db.Connection.Close();
            return data.type_list;
        }
    }
}