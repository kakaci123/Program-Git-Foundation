using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI;
using System.Collections;
using System.Text;
using NPOI.HSSF.Util;
using System.Globalization;

namespace s26web.Models
{
    public class VolunteersModel
    {
        public class LoginModel
        {
            [Required(ErrorMessage = "必填欄位")]
            [DisplayName("手機")]
            public string Mobile { get; set; }

            //[Required(ErrorMessage = "必填欄位")]
            //[DataType(DataType.Password)]
            //[Display(Name = "密碼")]
            //public string Password { get; set; }

            [Required(ErrorMessage = "必填欄位")]
            [DataType(DataType.Date)]
            [Display(Name = "密碼")]
            public DateTime BabaBirthday { get; set; }
        }
        public class RegisterModel
        {
            //[Required(ErrorMessage = "必填欄位")]
            [StringLength(30, ErrorMessage = "{0} 電子郵件長度不得大於 30 個字元。")]
            [Display(Name = "電子信箱")]
            [Remote("CheckEmail", "Home", ErrorMessage = "電子信箱重複")]
            [DataType(DataType.EmailAddress)]
            // [EmailAddress(ErrorMessage = "請輸入正確電子信箱。")]
            public string Account { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,30}$", ErrorMessage = "密碼需包含英文大小寫及數字，至少八個字元以上")]
            [StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "密碼")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "確認密碼")]
            [Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "必填欄位")]
            [Display(Name = "備用電子信箱")]
            [DataType(DataType.EmailAddress)]
            // [EmailAddress(ErrorMessage = "請輸入正確電子信箱。")]
            public string Email { get; set; }

            [Required(ErrorMessage = "必填欄位")]
            [StringLength(10, ErrorMessage = "{0} 姓名長度不得大於 10 個字元。")]
            [Display(Name = "姓名")]
            public string Name { get; set; }

            //[Required(ErrorMessage = "必填欄位")]
            [Display(Name = "驗證碼")]
            //[ValidateCaptcha(ErrorMessage="驗證碼錯誤!")]
            public string Code { get; set; }

            [DisplayName("備註")]
            public string IntroductionName { get; set; }

            [DisplayName("生日")]
            public List<DateTime?> Birthday { get; set; }

            [DisplayName("電話")]
            public string Phone { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            [RegularExpression("^09[0-9]{8}$", ErrorMessage = "手機格式錯誤")]
            [DisplayName("手機")]
            [Remote("CheckMobile", "Home", ErrorMessage = "手機號碼重複")]
            public string Mobile { get; set; }

            [DisplayName("發票編號")]
            public string InvoiceNumber { get; set; }
            [DisplayName("店家編號")]
            public int StoreId { get; set; }
            [DisplayName("地址")]
            public string Address { get; set; }
            public int year { get; set; }
            public int month { get; set; }
            public int date { get; set; }
            public string Notice { get; set; }
            public int NowBrand { get; set; }
            public string BrandName { get; set; }
            public string SerialNo { get; set; }
            public int Product { get; set; }
            public int Id { get; set; }
        }
        #region show
        /// <summary>
        /// 後台
        /// </summary> 
        public class VolunteersShow
        {
            [Key]
            [DisplayName("序號")]
            public int Id { get; set; }

            [DisplayName("手機")]
            [Required(ErrorMessage = "必填欄位")]
            public string Mobile { get; set; }

            [DisplayName("密碼")]
            [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,30}$", ErrorMessage = "密碼需包含英文大小寫及數字，至少八個字元以上")]
            [StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DisplayName("姓名")]
            [StringLength(10, ErrorMessage = "{0} 長度不得大於 10 個字元。")]
            [Required(ErrorMessage = "必填欄位")]
            public string Name { get; set; }

            [DisplayName("寶寶生日")]
            public DateTime? BabyBirthday { get; set; }

            [DisplayName("目前使用產品")]
            public int NowBrand { get; set; }
            [DisplayName("目前使用產品")]
            public string BrandName { get; set; }
            [DisplayName("電子信箱")]
            [Required(ErrorMessage = "必填欄位")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            public int CityId { get; set; }
            public int AreaId { get; set; }
            public string Address { get; set; }
            public string GiftMobile { get; set; }
            public int CityId2 { get; set; }
            public int AreaId2 { get; set; }
            public string CityName { get; set; }
            public string AreaName { get; set; }
            public string Address2 { get; set; }
            public string City2Name { get; set; }
            public string Area2Name { get; set; }

            [DisplayName("審核狀態")]
            [Required(ErrorMessage = "必填欄位")]
            public int Status { get; set; }

            public int SendCount { get; set; }
            public string SerialNo { get; set; }
            public string Photo { get; set; }
            public int? MemberNumber { get; set; }
            [DisplayName("最後更新管理者")]
            public string MemberNumberName { get; set; }
            [DisplayName("FB有聯絡")]
            public bool FBConnect { get; set; }
            [DisplayName("FB有加好友")]
            public bool FBFriend { get; set; }
            [DisplayName("客服備註")]
            public string Memo { get; set; }

            [DisplayName("註冊時間")]
            public DateTime CreateTime { get; set; }
            [DisplayName("更新時間")]
            public DateTime UpdateTime { get; set; }
            [DisplayName("管理員更新時間")]
            public DateTime? AdminUpdateTime { get; set; }
            [DisplayName("最後更新者")]
            public int? LastMemberId { get; set; }
            [DisplayName("最後更新者")]
            public string LastMember { get; set; }
            [DisplayName("審核時間")]
            public DateTime? EnableTime { get; set; }
            [DisplayName("審核失敗原因")]
            public string FailReason { get; set; }
            [DisplayName("點數")]
            public int? Point { get; set; }
            public List<eCRM> eCRM_list { get; set; }
        }

        public class VolunteersShowEdit
        {
            [Key]
            [DisplayName("序號")]
            public int Id { get; set; }

            [DisplayName("手機")]
            [Required(ErrorMessage = "必填欄位")]
            public string Mobile { get; set; }

            [DisplayName("密碼")]
            [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,30}$", ErrorMessage = "密碼需包含英文大小寫及數字，至少八個字元以上")]
            [StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "確認密碼")]
            [Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
            public string ConfirmPassword { get; set; }

            [DisplayName("姓名")]
            [StringLength(10, ErrorMessage = "{0} 長度不得大於 10 個字元。")]
            [Required(ErrorMessage = "必填欄位")]
            public string Name { get; set; }

            [DisplayName("寶寶生日/預產期")]
            public DateTime BabyBirthday { get; set; }

            [DisplayName("目前使用產品")]
            public int NowBrand { get; set; }
            [DisplayName("目前使用產品")]
            public string BrandName { get; set; }
            [DisplayName("電子信箱")]
            //[Required(ErrorMessage = "必填欄位")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            public int CityId { get; set; }
            public int AreaId { get; set; }
            public string Address { get; set; }
            [DisplayName("宅配 & 贈品電話")]
            public string GiftMobile { get; set; }
            public int CityId2 { get; set; }
            public int AreaId2 { get; set; }
            public string CityName { get; set; }
            public string AreaName { get; set; }
            public string Address2 { get; set; }
            public string City2Name { get; set; }
            public string Area2Name { get; set; }

            [DisplayName("審核狀態")]
            [Required(ErrorMessage = "必填欄位")]
            public int Status { get; set; }

            public int SendCount { get; set; }
            [DisplayName("驗證序號")]
            public string SerialNo { get; set; }
            [DisplayName("媽媽 /寶寶手冊圖片")]
            public string Photos { get; set; }
            [DisplayName("eCRM")]
            public int? MemberNumber { get; set; }
            [DisplayName("最後更新管理者")]
            public string MemberNumberName { get; set; }
            [DisplayName("FB有聯絡")]
            public bool FBConnect { get; set; }
            [DisplayName("FB有加好友")]
            public bool FBFriend { get; set; }
            [DisplayName("客服備註")]
            public string Memo { get; set; }

            [DisplayName("註冊時間")]
            public DateTime CreateTime { get; set; }
            [DisplayName("更新時間")]
            public DateTime UpdateTime { get; set; }
            [DisplayName("管理員更新時間")]
            public DateTime? AdminUpdateTime { get; set; }
            [DisplayName("最後更新管理者")]
            public int? LastMemberId { get; set; }
            [DisplayName("最後更新管理者")]
            public string LastMember { get; set; }
            [DisplayName("審核時間")]
            public DateTime? EnableTime { get; set; }
            [DisplayName("審核失敗原因")]
            public string FailReason { get; set; }
            [DisplayName("點數")]
            public int? Point { get; set; }

            public List<Point_List> Point_List { get; set; }
            public List<Orders_List> Orders_List { get; set; }
        }
        public class Export
        {
            public string Id { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string BabyBirthday { get; set; }
            public string NowBrand { get; set; }
            public string BrandName { get; set; }
            public string Email { get; set; }
            public string CityId { get; set; }
            public string AreaId { get; set; }
            public string Address { get; set; }
            public string GiftMobile { get; set; }
            public string CityId2 { get; set; }
            public string AreaId2 { get; set; }
            public string CityName { get; set; }
            public string AreaName { get; set; }
            public string Address2 { get; set; }
            public string City2Name { get; set; }
            public string Area2Name { get; set; }
            public string Status { get; set; }
            public string SendCount { get; set; }
            public string SerialNo { get; set; }
            public string Photo { get; set; }
            public string MemberNumber { get; set; }
            public string FBConnect { get; set; }
            public string FBFriend { get; set; }
            public string Memo { get; set; }
            public string CreateTime { get; set; }
            public string UpdateTime { get; set; }
            public string AdminUpdateTime { get; set; }
            public string LastMemberId { get; set; }
            public string LastMember { get; set; }
            public string EnableTime { get; set; }
            public string FailReason { get; set; }
        }

        public const int MaxDayPoint = 9;
        private static string hash = "sOoHoOBooK!!";
        public string Keyword = "";
        public string source = "0";
        public bool Search_Enable = false;
        public bool Search_Review = false;
        public bool Search_FirstOrder = false;
        public DateTime? create_time_start = null;
        public DateTime? create_time_end = null;
        public DateTime? enable_time_start = null;
        public DateTime? enable_time_end = null;
        public int status = 0;
        public int NowBrand = 0;
        public bool NotchangeBrand = false;
        public string from = "";
        public int CategorySelectList = 0;
        #endregion
        public int LoginCheck(LoginModel item)
        {
            s26webDataContext db = new s26webDataContext();
            var vol = db.Volunteers.Where(w => w.Mobile == item.Mobile && w.BabyBirthday == item.BabaBirthday);
            db.Connection.Close();
            if (vol == null)
                return -1;
            else
                return 1;
        }
        public Volunteers Get_One_Mobile(string membernumber)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.Volunteers.FirstOrDefault(f => f.Mobile == membernumber);
            if (data != null)
                data.SendCount = 0;
            db.SubmitChanges();
            //var data = db.Volunteers.FirstOrDefault(f => f.Email == email && Get_HashPassword(pwd) == f.Password && mobile == f.Mobile);
            //var data = db.Volunteers.Where(f => Get_HashPassword(password) == f.Password).AsEnumerable().FirstOrDefault(f => f.MemberNumber + string.Format("{0:00000}", f.Id) == account);
            db.Connection.Close();
            return data;
        }

        public string Get_PWByMobile(string mobile)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.Volunteers.Where(w => w.Mobile == mobile).Select(s => s.Password).FirstOrDefault();
            return data;
        }
        public string Get_HashPassword(string input)
        {
            return Method.GetMD5(hash + input, true);
        }

        private string Query(string query = "")
        {
            string sql = "";

            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "Name", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Email", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "MemberNumber", "OR", "( \"" + query + "\")", ".Value.ToString().Contains", false);
                sql = Method.SQL_Combin(sql, "Mobile", "OR", "( \"" + query + "\")", ".Contains", false);

            }

            return sql;
        }

        public VolunteersShowEdit Get_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            VolunteersShowEdit data = new VolunteersShowEdit();

            var item = db.Volunteers.FirstOrDefault(f => f.Id == id);
            var eCRM = db.eCRM.FirstOrDefault(f => f.Id == item.MemberNumber);
            var user = db.UserProfile.FirstOrDefault(f => f.Id == item.LastMemberId);

            //var u = db.UserProfile.Where(w => w.Id == i.LastUserId).Select(s => s.Name).FirstOrDefault();
            data.Mobile = item.Mobile;
            data.Password = item.Password;
            data.Name = item.Name;
            data.BabyBirthday = item.BabyBirthday;
            data.NowBrand = item.NowBrand;
            data.BrandName = item.BrandName;
            data.Email = item.Email;
            data.CityId = item.CityId.HasValue?item.CityId.Value:0;
            data.AreaId = item.AreaId.HasValue ? item.AreaId.Value : 0;
            data.Address = item.Address;
            //data.GiftMobile = item.GiftMobile;
            //data.CityId2 = item.CityId2;
            //data.AreaId2 = item.AreaId2;
            //data.Address2 = item.Address2;
            data.Status = item.Status;
            //data.SendCount = v.SendCount;
            data.SerialNo = item.SerialNo;
            data.Photos = item.Photos;
            data.MemberNumber = item.MemberNumber;
            if (eCRM != null)
                data.MemberNumberName = eCRM.UserName;
            data.FBConnect = item.FBConnect;
            data.FBFriend = item.FBFriend;
            data.Memo = item.Memo;
            data.CreateTime = item.CreateTime.AddHours(8);
            data.UpdateTime = item.UpdateTime.AddHours(8);
            data.LastMemberId = item.LastMemberId;
            if (item.LastMemberId == null)
                data.LastMember = "";
            else
                data.LastMember = user.Name;
            if (item.AdminUpdateTime == null)
            { data.AdminUpdateTime = null; }
            else
            { data.AdminUpdateTime = item.AdminUpdateTime.Value.AddHours(8); }
            data.Point = item.Point;

            data.Point_List = db.Point_List.Where(w => w.Vid == id).OrderByDescending(o => o.SendTime).Select(s => new Point_List
            {
                SendTime = s.SendTime.AddHours(8),
                Category_Name = s.Category_Name,
                Point = s.Point
            }).ToList();

            data.Orders_List = db.Orders_List.Where(w => w.VolunteersId == id).OrderByDescending(o => o.Id).Select(s => new Orders_List
            {
               Id = s.Id,
               Osid = s.Osid,
               OrdersTime = s.OrdersTime.AddHours(8),
               ProductName = s.ProductName,
               Order_Quantity = s.Order_Quantity,
               TotalPrice = s.TotalPrice,
               OrdersStates = s.OrdersStates,
               OrderstateName = s.OrderstateName,
               PerProductPrice = s.PerProductPrice
            }).ToList();
            db.Connection.Close();
            return data;
        }

        public static Volunteers Get_One(string account)
        {
            s26webDataContext db = new s26webDataContext();
            var data = (from i in db.Volunteers
                        where i.Mobile == account  //i.Account == account &&
                        //&& i.Enable == true
                        select i).FirstOrDefault();
            db.Connection.Close();
            return data;
        }

        public List<VolunteersShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = Get().OrderByDescending(o => o.CreateTime).Skip((p - 1) * take).Take(take);
                List<VolunteersShow> result = new List<VolunteersShow>();
                foreach (var i in data)
                {
                    result.Add(i);
                }
                db.Connection.Close();
                return result;
            }
            catch { return new List<VolunteersShow>(); }
        }
        private IQueryable<VolunteersShow> Get()
        {
            s26webDataContext db = new s26webDataContext();
            var data = from v in db.VolunteersAndAddress
                       select new VolunteersShow
                       {
                           Id = v.Id,
                           Mobile = v.Mobile,
                           Password = v.Password,
                           Name = v.Name,
                           BabyBirthday = v.BabyBirthday,
                           NowBrand = v.NowBrand,
                           BrandName = v.BrandName,
                           Email = v.Email,
                           CityName = v.Name,
                           AreaName = v.Name,
                           Address = v.Address,
                           GiftMobile = v.GiftMobile,
                           City2Name = v.Name,
                           Area2Name = v.Name,
                           Address2 = v.Address2,
                           Status = v.Status,
                           SendCount = v.SendCount,
                           SerialNo = v.SerialNo,
                           Photo = v.Photos,
                           MemberNumber = v.MemberNumber,
                           
                           MemberNumberName = db.eCRM.FirstOrDefault(f => f.Id == v.MemberNumber).UserName,
                           //FBConnect = v.FBConnect,
                           //FBFriend = v.FBFriend,
                           Memo = v.Memo,
                           CreateTime = v.CreateTime,
                           UpdateTime = v.UpdateTime,
                           AdminUpdateTime = v.AdminUpdateTime,
                           //LastMember
                           EnableTime = v.EnableTime,
                           FailReason = v.FailReason,
                           Point = v.Point
                       };
            if (Keyword != "")
            {
                data = data.Where(Query(Keyword));
            }
            if (status > 0)
            {
                data = data.Where(w => w.Status == status);
            }
            if (NowBrand >0)
            {
                data = data.Where(w => w.NowBrand == NowBrand);
            }
            if (create_time_start != null)
            {
                data = data.Where(w => w.CreateTime >= create_time_start.Value.AddHours(-8));
            }
            if (create_time_end != null)
            {
                data = data.Where(w => w.CreateTime <= create_time_end.Value.AddHours(-8));
            }
            if (NotchangeBrand)
            {
                data = data.Where(w => w.BabyBirthday > DateTime.UtcNow && w.NowBrand == 1);
            }
            if (CategorySelectList > 0)
            {
                data = data.Where(w => w.MemberNumber == CategorySelectList);
            }
            db.Connection.Close();
            return data;
        }
        public Method.Paging Get_Page(int p = 1, int take = 10)
        {
            return Method.Get_Page(Get_Count(), p, take);
        }

        public int Get_Count()
        {
            return Get().Count();
        }
        public List<eCRM> Get_eCRM()
        {
            s26webDataContext db = new s26webDataContext();
            VolunteersShow data = new VolunteersShow();
            data.eCRM_list = db.eCRM.ToList();
            db.Connection.Close();
            return data.eCRM_list;
        }


        #region Insert
        public Volunteers Insert(RegisterModel model, HttpPostedFileBase file, string vid, HttpServerUtilityBase Server, int year , int month , int date)
        {
            var photos = "";
            if (file != null)
                if (file.ContentLength > 0 && file.ContentType.ToLower() == "image/jpeg" || file.ContentType == "image/png")
                    photos = Method.Upload_File(file, vid, Server);
                else
                    photos = "";
            try
            {
                DateTime bir = new DateTime(year, month, date);
                s26webDataContext db = new s26webDataContext();

                //若畫面日期大於當下日期表示懷孕中
                int int_NowBrand;
                int Dat_Compare = DateTime.Compare(bir, DateTime.UtcNow);
                if (Dat_Compare > 0)
                {
                    int_NowBrand = 1;
                }
                else
                {
                    int_NowBrand = model.Product;
                }

                var vol = new Volunteers
                {
                    Mobile = model.Mobile,
                    Password = Get_HashPassword(model.Password),
                    Name = model.Name,
                    BabyBirthday = bir,
                    Email = model.Email,
                    Status = 1,
                    NowBrand = int_NowBrand,
                    BrandName = model.BrandName ,
                    MemberNumber = int_NowBrand,
                    Photos = photos,
                    SerialNo = model.SerialNo,
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,
                    Point = 0
                };
                db.Volunteers.InsertOnSubmit(vol);
                db.SubmitChanges();
                db.Connection.Close();
                return vol;
            }
            catch
            {
                return null;
            }

        }

        public int Insert(VolunteersShow item, HttpPostedFileBase file, string vid, HttpServerUtilityBase Server)
        {
            var photos = "";
            s26webDataContext db = new s26webDataContext();
            if (file != null)
                if (file.ContentLength > 0 && file.ContentType.ToLower() == "image/jpeg" || file.ContentType == "image/png")
                    photos = Method.Upload_File(file, vid, Server);
                else
                    photos = "";
            if (item.LastMemberId > 0)
                item.AdminUpdateTime = DateTime.Now;
            try
            {
                 Volunteers new_item = new Volunteers
                {
                    Mobile = item.Mobile,
                    Password = Get_HashPassword(item.Password),
                    Name = item.Name,
                    BabyBirthday = item.BabyBirthday.Value,
                    NowBrand = item.NowBrand,
                    BrandName = item.BrandName,
                    Email = item.Email,
                    CityId = item.CityId,
                    AreaId = item.AreaId,
                    Address = item.Address,
                    GiftMobile = item.GiftMobile,
                    CityId2 = item.CityId2,
                    AreaId2 = item.AreaId2,
                    Address2 = item.Address2,
                    Status = item.Status,
                    SendCount = item.SendCount,
                    Photos = photos,
                    SerialNo = item.SerialNo,
                    MemberNumber = item.NowBrand,
                    FBConnect = item.FBConnect,
                    FBFriend = item.FBFriend,
                    Memo = item.Memo,
                    AdminUpdateTime = item.AdminUpdateTime,                    
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,   
                    Point = 0  
                };
                db.Volunteers.InsertOnSubmit(new_item);
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

        #region Update
        public int Update(VolunteersShowEdit item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Volunteers.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
                    data.Mobile = item.Mobile;
                    if (item.Password != "" && item.Password != null)
                    {
                        data.Password = Get_HashPassword(item.Password);
                    }
                    data.Name = item.Name;
                    data.BabyBirthday = item.BabyBirthday;
                    data.NowBrand = item.NowBrand;
                    data.BrandName = item.BrandName;
                    data.Email = item.Email;
                    data.CityId = item.CityId;
                    data.AreaId = item.AreaId;
                    data.Address = item.Address;
                    data.GiftMobile = item.GiftMobile;
                    data.CityId2 = item.CityId2;
                    data.AreaId2 = item.AreaId2;
                    data.Address2 = item.Address2;
                    data.Status = item.Status;
                    //data.SendCount = v.SendCount;
                    data.SerialNo = item.SerialNo;
                    //data.Photo = v.Photos;
                    data.MemberNumber = item.NowBrand;
                    data.FBConnect = item.FBConnect;
                    data.FBFriend = item.FBFriend;
                    data.Memo = item.Memo;
                    //data.UpdateTime = DateTime.UtcNow; //使用者更新時間
                    data.AdminUpdateTime = DateTime.UtcNow; //管理者更新
                    data.LastMemberId = item.LastMemberId;

                    if (item.Status == 2 && data.EnableTime == null)
                    { data.EnableTime = DateTime.UtcNow; }
                    else
                        data.EnableTime = data.EnableTime;

                    db.SubmitChanges();
                    db.Connection.Close();
                    return data.Id;
                }
                db.Connection.Close();
                return -1;
            }
            catch { return -1; }
        }

        public int Update(VolunteersModel.RegisterModel item, DateTime Birthday)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Volunteers.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
                    data.BabyBirthday = Birthday;
                    data.NowBrand = item.Product;
                    data.BrandName = item.BrandName;
                    
                    data.UpdateTime = DateTime.UtcNow; //使用者更新時間
                    data.LastMemberId = item.Id;

                    db.SubmitChanges();
                    db.Connection.Close();
                    return data.Id;
                }
                db.Connection.Close();
                return -1;
            }
            catch { return -1; }
        }

        public int MemberUpdate(VolunteersShowEdit item, HttpPostedFileBase file, string vid, HttpServerUtilityBase Server)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Volunteers.FirstOrDefault(f => f.Id == item.Id);
                if (file != null)
                    if (file.ContentLength > 0 && file.ContentType.ToLower() == "image/jpeg" || file.ContentType == "image/png")
                        data.Photos = Method.Upload_File(file, vid, Server);
                
                data.Name = item.Name;
                data.BabyBirthday = item.BabyBirthday;
                data.Email = item.Email;
                data.NowBrand = item.NowBrand;
                data.MemberNumber = item.NowBrand;
                data.BrandName = item.BrandName;
                data.SerialNo = item.SerialNo;
                data.UpdateTime = DateTime.UtcNow;

                db.SubmitChanges();
                db.Connection.Close();
                return data.Id;
            }
            catch { return -1; }
            
        }
        #endregion

        #region Export
        public static List<Export> Get_Export(VolunteersModel datas)
        {

            /*-------------------匯出宣告-------------------*/
            s26webDataContext db = new s26webDataContext();
            Export export = new Export();
            List<Export> exp = new List<Export>();
            List<string> bir = new List<string> { "", "", "" };
            var newdata = datas.Get().OrderByDescending(o => o.Id);
            /*-------------------匯出宣告End-------------------*/
            foreach (var d in newdata)
            {
                export.Phone = d.Mobile;

                export.Name = d.Name;
                //export.BabyBirthday = d.BabyBirthday.Value.Date.ToString();

                if (d.NowBrand == 1)
                    export.NowBrand = "懷孕中";
                else if (d.NowBrand == 2)
                    export.NowBrand = "使用S-26產品";
                else if (d.NowBrand == 3)
                    export.NowBrand = "全母乳餵哺";
                else if (d.NowBrand == 4)
                    export.NowBrand = "其他";

                export.BrandName = d.BrandName;
                export.Email = d.Email;

                if (d.Status == 1)
                    export.Status = "審核中";
                else if (d.Status == 2)
                    export.Status = "通過";
                else if (d.Status == 3)
                    export.Status = "未通過";

                export.FBConnect = d.FBConnect ? "是" : "否";
                export.FBFriend = d.FBFriend ? "是" : "否";
                export.CreateTime = d.CreateTime.AddHours(8).ToString("yyyy/MM/dd HH:mm");
                export.UpdateTime = d.UpdateTime.AddHours(8).ToString("yyyy/MM/dd HH:mm");
                export.Memo = d.Memo;
                export.FailReason = d.FailReason;
                
                exp.Add(export);
                export = new Export();
            }
            db.Connection.Close();
            return exp;
        }

        public MemoryStream Get_ExcelData_main(List<Export> data)
        {
            List<string> header = new List<string>();
            header.Add("電話");
            header.Add("姓名");
            //header.Add("寶寶生日");
            header.Add("目前使用產品");
            header.Add("產品名稱");
            header.Add("E-mail");
            header.Add("審核狀態");
            header.Add("FB有聯絡");
            header.Add("FB有加好友");
            header.Add("註冊時間");
            header.Add("更新時間");
            header.Add("備註");
            header.Add("不通過原因");
            
            MemoryStream ms = new MemoryStream();
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("sheet");

            //宣告headStyle、內容style
            HSSFCellStyle headStyle = null;
            HSSFCellStyle contStyle = null;
            headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            contStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFFont font = null;
            font = (HSSFFont)workbook.CreateFont();
            HSSFCell cell = null;


            //標題粗體、黃色背景
            headStyle.FillForegroundColor = HSSFColor.Yellow.Index;
            headStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            //標題字型樣式
            font.FontHeightInPoints = 10;
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            font.FontName = "新細明體";
            headStyle.SetFont(font);
            //內容字型樣式(自動換行)
            contStyle.WrapText = true;
            //contStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            //宣告headRow
            sheet.CreateRow(0);
            //設定head背景、粗體、內容
            foreach (var i in header)
            {
                cell = (HSSFCell)sheet.GetRow(0).CreateCell(header.IndexOf(i));
                cell.SetCellValue(i);
                cell.CellStyle = headStyle;
            }
            for (int i = 0; i < data.Count; i++)
            {
                sheet.CreateRow(i + 1);
                sheet.GetRow(i + 1).CreateCell(0).SetCellValue(data[i].Phone);
                sheet.GetRow(i + 1).CreateCell(1).SetCellValue(data[i].Name);
                //sheet.GetRow(i + 1).CreateCell(12).SetCellValue(data[i].BabyBirthday);
                sheet.GetRow(i + 1).CreateCell(2).SetCellValue(data[i].NowBrand);
                sheet.GetRow(i + 1).CreateCell(3).SetCellValue(data[i].BrandName);
                sheet.GetRow(i + 1).CreateCell(4).SetCellValue(data[i].Email);
                sheet.GetRow(i + 1).CreateCell(5).SetCellValue(data[i].Status);
                sheet.GetRow(i + 1).CreateCell(6).SetCellValue(data[i].FBConnect);
                sheet.GetRow(i + 1).CreateCell(7).SetCellValue(data[i].FBFriend);
                sheet.GetRow(i + 1).CreateCell(8).SetCellValue(data[i].CreateTime);
                sheet.GetRow(i + 1).CreateCell(9).SetCellValue(data[i].UpdateTime);
                sheet.GetRow(i + 1).CreateCell(10).SetCellValue(data[i].Memo);
                sheet.GetRow(i + 1).CreateCell(11).SetCellValue(data[i].FailReason);
            }
            foreach (var i in header)
            {
                //sheet.AutoSizeColumn(header.IndexOf(i));
                if (header.IndexOf(i) == 0)
                    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 20);
                else if (header.IndexOf(i) == 2 || header.IndexOf(i) == 3 || header.IndexOf(i) == 6 || header.IndexOf(i) == 10)
                    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 13);
                else if (header.IndexOf(i) >= 4 && header.IndexOf(i) <= 6 || header.IndexOf(i) == 11)
                    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 25);
                else
                    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 10);
            }


            workbook.Write(ms);
            ms.Position = 0;
            ms.Flush();
            return ms;
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
                    var data = db.Volunteers.Where(w => id.Contains(w.Id));
                    if (data.Any())
                    {
                        db.Volunteers.DeleteAllOnSubmit(data);
                        db.SubmitChanges();
                    }
                    db.Connection.Close();
                }
            }
        }
        #endregion
    }
}