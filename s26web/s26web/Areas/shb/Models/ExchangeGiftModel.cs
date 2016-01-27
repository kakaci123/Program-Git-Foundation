using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using s26web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace s26web.Areas.shb.Models
{
    public class ExchangeGiftModel
    {
        public string Egid = "";
        public string Keyword = "";
        public string StatesSelect = "";
        public DateTime? time_start = null;
        public DateTime? time_end = null;

        public class MyList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class GiftList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string MemberType { get; set; }
            public int Point { get; set; }
            public int Amount { get; set; }
        }

        public class InsertExchangeGift
        {
            [DisplayName("贈品兌換編號")]
            public string Egid { get; set; }
            [DisplayName("會員手機號碼")]
            public string Vol_Mobile { get; set; }
            [DisplayName("收件人姓名")]
            public string Name { get; set; }
            [DisplayName("連絡電話")]
            public string Mobile { get; set; }
            public int CityId { get; set; }
            public int AreaId { get; set; }
            [DisplayName("收件地址")]
            public string Address { get; set; }
            [DisplayName("贈品選擇")]
            public int GiftId { get; set; }
            [DisplayName("備註")]
            public string Remark { get; set; }
            public int UpdateUserId { get; set; }
            public DateTime CreateTime { get; set; }

            //public List<GiftList> ExchangeGiftOption { get; set; }
        }

        public class ExchangeGiftShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("兌換編號")]
            public string Egid { get; set; }
            [DisplayName("會員編號")]
            public int VolunteersId { get; set; }
            [DisplayName("會員姓名")]
            public string Vol_Name { get; set; }
            [DisplayName("手機")]
            public string Vol_Mobile { get; set; }
            [DisplayName("兌換贈品代碼")]
            public int ExchangeGift { get; set; }
            [DisplayName("兌換贈品名稱")]
            public string ExchangeGiftName { get; set; }
            [DisplayName("兌換贈品內容")]
            public string ExchangeGiftContent { get; set; }
            [DisplayName("兌換所需代幣")]
            public int Coin { get; set; }
            [DisplayName("收件人")]
            public string Name { get; set; }
            [DisplayName("收件人手機")]
            public string Mobile { get; set; }
            [DisplayName("收貨縣市代碼")]
            public int CityId { get; set; }
            [DisplayName("收貨地區代碼")]
            public int AreaId { get; set; }
            [DisplayName("收貨區域")]
            public string AreaName { get; set; }
            [DisplayName("收貨縣市")]
            public string CityName { get; set; }
            [DisplayName("收貨地址")]
            public string Address { get; set; }
            [DisplayName("收貨郵遞區號")]
            public string Zip { get; set; }
            [DisplayName("備註")]
            public string Remarks { get; set; }
            [DisplayName("訂單狀態")]
            public int ExchangeGiftStates { get; set; }
            [DisplayName("訂單狀態")]
            public string States { get; set; }
            [DisplayName("訂單時間")]
            public DateTime SubmitTime { get; set; }
            [DisplayName("最後更新管理者")]
            public int? UpdateUserId { get; set; }
            [DisplayName("最後更新管理者")]
            public string UpdateUserName { get; set; }

            public ExchangeGiftShow Set_Other()
            {
                try
                {
                    s26webDataContext db = new s26webDataContext();

                    //設定Volunteers
                    Volunteers Temp_Volunteers = (Volunteers)db.Volunteers.FirstOrDefault(w => w.Id == this.VolunteersId);
                    this.Vol_Name = Temp_Volunteers.Name;
                    this.Vol_Mobile = Temp_Volunteers.Mobile;

                    //設定Area,City
                    Area Temp_Area = db.Area.FirstOrDefault(w => w.Id == this.AreaId);
                    this.CityName = db.City.FirstOrDefault(w => w.Id == this.CityId).Name;
                    this.AreaName = Temp_Area.Name;
                    this.Zip = Temp_Area.ZipCode;

                    //設定Gift
                    Gift Temp_Gift = db.Gift.FirstOrDefault(w => w.Id == this.ExchangeGift);
                    this.ExchangeGiftName = Temp_Gift.Name;
                    this.Coin = Temp_Gift.Point;
                    return this;
                }
                catch
                {
                    return null;
                }
            }

            public class Export_Order
            {
                [Key]
                [DisplayName("兌換編號")]
                public string Egid { get; set; }
                [DisplayName("會員姓名")]
                public string MemberName { get; set; }
                [DisplayName("手機")]
                public string MemberMobile { get; set; }
                [DisplayName("收件人")]
                public string Name { get; set; }
                [DisplayName("收件人手機")]
                public string Mobile { get; set; }
                [DisplayName("收件人地址")]
                public string Address { get; set; }
                [DisplayName("兌換贈品")]
                public string ExchangeGift { get; set; }
                [DisplayName("訂單備註")]
                public string Memo { get; set; }
                [DisplayName("寄送狀態")]
                public string State { get; set; }
                [DisplayName("最後更新")]
                public string UpdateName { get; set; }
                [DisplayName("訂單時間")]
                public string OrderTime { get; set; }
            }
        }
       
        #region Insert

        /// <summary>
        /// 將資料筆數寫入資料表中
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Insert_Score(InsertExchangeGift item)
        {
            s26webDataContext db = new s26webDataContext();
            ExchangeGift result = new ExchangeGift
            {
                Egid = Get_TheDateScoreCount(DateTime.Now.AddDays(-8).ToString("yyyyMMdd")),
                Vol_Id = Get_VolId(item.Vol_Mobile),
                Name = item.Name,
                Mobile = item.Mobile,
                CityId = item.CityId,
                AreaId = item.AreaId,
                Address = item.Address,
                GiftId = item.GiftId,
                Remark = item.Remark,
                Status = 29,
                UpdateUserId = item.UpdateUserId,
                CreateTime = DateTime.Now.AddHours(-8)
            };
            db.ExchangeGift.InsertOnSubmit(result);
            db.SubmitChanges();
            Reduce_Gift(result.GiftId);
            return 1;
        }

        #endregion

        #region Update

        /// <summary>
        /// 資料筆數更新，提供多筆同時更新的功能
        /// </summary>
        /// <param name="EgidSource"></param>
        /// <param name="StateSoucre"></param>
        /// <param name="Updater"></param>
        public void ScoreUpdate(string EgidSource, string StateSoucre, int Updater)
        {
            s26webDataContext db = new s26webDataContext();
            string[] TempId = EgidSource.Split(',').ToArray();
            string[] TempState = StateSoucre.Split(',').ToArray();
            for (int i = 0; i < TempId.Length; i++)
            {
                ExchangeGift compareTemp = db.ExchangeGift.FirstOrDefault(w => w.Egid == TempId[i]);
                if (compareTemp.Status != int.Parse(TempState[i]))
                {
                    compareTemp.Status = int.Parse(TempState[i]);
                    compareTemp.UpdateUserId = Updater;
                    compareTemp.CreateTime = DateTime.Now.AddHours(-8);
                    db.SubmitChanges();
                }
            }
            db.Connection.Close();
        }

        /// <summary>
        /// 若成功將資料寫入ExchangeGift資料表中後，進入Gift資料表扣除所選擇之Amount
        /// </summary>
        /// <param name="id"></param>
        private void Reduce_Gift(int id)
        {
            s26webDataContext db = new s26webDataContext();
            var temp = db.Gift.FirstOrDefault(w => w.Id == id);
            temp.Amount -= 1;
            db.SubmitChanges();
            db.Connection.Close();
        }

        #endregion

        #region Query

        /// <summary>
        /// 利用電話查找使用者代碼
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private int Get_VolId(string phone)
        {
            s26webDataContext db = new s26webDataContext();
            return db.Volunteers.FirstOrDefault(w => w.Mobile.Equals(phone)).Id;
        }

        /// <summary>
        /// 查找資料筆並回傳新的資料編號(提供13碼，前八碼為時間(yyyyMMdd)，後五碼為序號(Range:00001-99999))
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string Get_TheDateScoreCount(string source)
        {
            s26webDataContext db = new s26webDataContext();
            string temp = (db.ExchangeGift.Where(w => w.Egid.Contains(source)).Count() + 1).ToString().PadLeft(5, '0');
            return source + temp;
        }

        /// <summary>
        /// 取得提供兌換贈品的項目
        /// </summary>
        /// <returns></returns>
        public static List<GiftList> Get_ExchangeGiftOption()
        {
            s26webDataContext db = new s26webDataContext();
            var temp = db.Gift;
            var rlt = new List<GiftList>();
            foreach (var i in temp)
            {
                rlt.Add(new GiftList
                {
                    Id = i.Id,
                    MemberType = i.MemberType,
                    Name = i.Name,
                    Amount = i.Amount,
                    Point = i.Point
                });
            }
            return rlt;
        }

        //public ExchangeGiftShow Set_ExchangeGiftOption()
        //{
        //    try
        //    {
        //        s26webDataContext db = new s26webDataContext();
        //        var temp = db.Gift;
        //        this.ExchangeGiftOption = new List<GiftList>();
        //        foreach (var i in temp)
        //        {
        //            this.ExchangeGiftOption.Add(new GiftList
        //            {
        //                Id = i.Id,
        //                MemberType = i.MemberType,
        //                Name = i.Name,
        //                Amount = i.Amount,
        //                Point = i.Point
        //            });
        //        }
        //        return this;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 查找會員名稱
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string Get_UserName(int source)
        {
            return new s26webDataContext().UserProfile.FirstOrDefault(w => w.Id == source).Name;
        }

        private string Get_State(int source)
        {
            return new s26webDataContext().Category.FirstOrDefault(w => w.Id == source).Name;
        }

        public int Get_MemberInfo(string source)
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                return db.Volunteers.FirstOrDefault(w => w.Mobile.Equals(source)).NowBrand;
            }
            catch {
                return -1;
            }
        }

        public List<MyList> Get_ExchangeGiftStatesOption()
        {
            s26webDataContext db = new s26webDataContext();
            var Temp_Category = db.Category.Where(w => w.Fun_Id == 7 && w.Memo.Equals("ExchangeState"));
            List<MyList> rlt = new List<MyList>();
            foreach (var i in Temp_Category)
            {
                rlt.Add(new MyList
                {
                    Id = i.Id,
                    Name = i.Name
                });
            }
            db.Connection.Close();
            return rlt;
        }

        public Method.Paging Get_PageIO(int p = 1, int take = 10, int pages = 5)
        {
            return Method.Get_Page(Get_CountIO(), p, take, pages);
        }

        public int Get_CountIO()
        {
            try
            {
                return Get_All().Count();
            }
            catch
            {
                return 0;
            }
        }
        
        /// <summary>
        /// 建立SQL語法，提供查找準則
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string Query(string query = "")
        {
            string sql = "";
            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "Egid", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Vol_Name", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Vol_Mobile", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Name", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Mobile", "OR", "( \"" + query + "\")", ".Contains", false);
            }

            return sql;
        }

        #endregion

        #region Get_Select_Set

        /// <summary>
        /// 取得所有資料，並依照Id排序
        /// </summary>
        /// <param name="p"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<ExchangeGiftShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = this.Get_All().OrderByDescending(o => o.Id).Skip((p - 1) * take).Take(take);
                db.Connection.Close();
                return data.ToList();
            }
            catch
            {
                return new List<ExchangeGiftShow>();
            }
        }

        /// <summary>
        /// 取得所有資料筆數
        /// </summary>
        /// <returns></returns>
        public List<ExchangeGiftShow> Get_All()
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = Get_Data_And_Filter();
                List<ExchangeGiftShow> result = new List<ExchangeGiftShow>();
                foreach (var i in data)
                {
                    result.Add(i);
                }
                db.Connection.Close();
                return result;
            }
            catch { return new List<ExchangeGiftShow>(); }
        }

        /// <summary>
        /// 取得詳細資料
        /// </summary>
        /// <param name="EgidFrom"></param>
        /// <returns></returns>
        public ExchangeGiftShow Get_Detail(string EgidFrom)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                ExchangeGiftShow data = Get_Data_And_Filter().Where(i => i.Egid == EgidFrom).ToArray()[0];
                return data;
            }
            catch
            {
                return new ExchangeGiftShow();
            }
        }

        /// <summary>
        /// 取得並篩選資料
        /// </summary>
        /// <returns></returns>
        private List<ExchangeGiftShow> Get_Data_And_Filter()
        {
            s26webDataContext db = new s26webDataContext();
            var temp = db.ExchangeGift;
            List<ExchangeGiftShow> data = new List<ExchangeGiftShow>();
            foreach (var index in temp)
            {
                data.Add(Set_Info(index));
            }
            data = Select_Data(data);
            db.Connection.Close();
            return data;
        }

        /// <summary>
        /// 篩選資料
        /// </summary>
        /// <param name="Source"></param>
        private List<ExchangeGiftShow> Select_Data(List<ExchangeGiftShow> Source)
        {
            if (Keyword != null && !(Keyword.Equals("")))
            {
                Source = Source.Where(Query(Keyword)).ToList();
            }

            if (time_start != null && time_end != null && (time_end.Value >= time_start.Value))
            {
                Source = Source.Where(w => w.SubmitTime >= time_start.Value && w.SubmitTime <= time_end.Value).ToList();
            }

            if (StatesSelect != null && !(StatesSelect.Equals("")) && !(StatesSelect.Equals("0")))
            {
                Source = Source.Where(w => w.ExchangeGiftStates == int.Parse(StatesSelect)).ToList();
            }

            return Source;
        }

        /// <summary>
        /// 設定資料內值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ExchangeGiftShow Set_Info(ExchangeGift source)
        {
            try
            {
                ExchangeGiftShow rlt = new ExchangeGiftShow()
                {
                    Id = source.Id,
                    Egid = source.Egid,
                    VolunteersId = source.Vol_Id,
                    ExchangeGift = source.GiftId,
                    Name = source.Name,
                    Mobile = source.Mobile,
                    CityId = source.CityId,
                    AreaId = source.AreaId,
                    Address = source.Address,
                    Remarks = source.Remark,
                    ExchangeGiftStates = source.Status,
                    States = Get_State(source.Status),
                    SubmitTime = source.CreateTime,
                    UpdateUserId = source.UpdateUserId,
                    UpdateUserName = Get_UserName(source.UpdateUserId.Value)
                };
                if (rlt.UpdateUserId != null)
                {
                    rlt.UpdateUserName = Get_UserName(rlt.UpdateUserId.Value);
                }
                rlt.Set_Other();
                return rlt;
            }
            catch
            {
                return new ExchangeGiftShow();
            }
        }

        #endregion
      
        #region ExcelExport

        /// <summary>
        /// Excel Export Function
        /// </summary>
        /// <returns></returns>
        public List<ExchangeGiftShow.Export_Order> Get_All_Export()
        {
            ExchangeGiftShow.Export_Order export = new ExchangeGiftShow.Export_Order();
            List<ExchangeGiftShow.Export_Order> exp = new List<ExchangeGiftShow.Export_Order>();
            var data = this.Get_All();
            foreach (var d in data)
            {
                export.Egid = d.Egid;
                export.MemberName = d.Vol_Name;
                export.MemberMobile = d.Vol_Mobile;
                export.Name = d.Name;
                export.Mobile = d.Mobile;
                export.Address = d.Address;
                export.ExchangeGift = d.ExchangeGiftName;
                export.Memo = d.Remarks;
                export.State = d.States;
                export.UpdateName = d.UpdateUserName;
                export.OrderTime = d.SubmitTime.AddHours(8).ToString("yyyy/MM/dd HH:mm:ss");
                exp.Add(export);
                export = new ExchangeGiftShow.Export_Order();
            }
            return exp;
        }

        public MemoryStream Get_ExcelData2(List<ExchangeGiftShow.Export_Order> data)
        {
            List<string> header = new List<string>();
            header.Add("兌換編號");
            header.Add("會員姓名");
            header.Add("手機");
            header.Add("收件人");
            header.Add("收件人手機");
            header.Add("收貨地址");
            header.Add("兌換贈品名稱");
            header.Add("備註");
            header.Add("訂單狀態");
            header.Add("最後更新管理者");
            header.Add("訂單時間");
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
                sheet.GetRow(i + 1).CreateCell(0).SetCellValue(data[i].Egid);
                sheet.GetRow(i + 1).CreateCell(1).SetCellValue(data[i].MemberName);
                sheet.GetRow(i + 1).CreateCell(2).SetCellValue(data[i].MemberMobile);
                sheet.GetRow(i + 1).CreateCell(3).SetCellValue(data[i].Name);
                sheet.GetRow(i + 1).CreateCell(4).SetCellValue(data[i].Mobile);
                sheet.GetRow(i + 1).CreateCell(5).SetCellValue(data[i].Address);
                sheet.GetRow(i + 1).CreateCell(6).SetCellValue(data[i].ExchangeGift);
                sheet.GetRow(i + 1).CreateCell(7).SetCellValue(data[i].Memo);
                sheet.GetRow(i + 1).CreateCell(8).SetCellValue(data[i].State);
                sheet.GetRow(i + 1).CreateCell(9).SetCellValue(data[i].UpdateName);
                sheet.GetRow(i + 1).CreateCell(10).SetCellValue(data[i].OrderTime);
            }
            foreach (var i in header)
            {
                sheet.AutoSizeColumn(header.IndexOf(i));
            }
            workbook.Write(ms);
            ms.Position = 0;
            ms.Flush();
            return ms;
        }

        #endregion
    }
}