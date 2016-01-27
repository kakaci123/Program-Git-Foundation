using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using s26web.Models;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using LinqToExcel;

namespace s26web.Areas.shb.Models
{
    public class SalesCodeModel
    {
        public class SalesCodeModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("促銷碼活動")]
            public int SalesPromotionId { get; set; }
            [DisplayName("促銷碼活動")]
            public string SalesPromotionName { get; set; }
            [DisplayName("點數")]
            public int SalesPromotionPoint { get; set; }
            [DisplayName("序號到期日")]
            public DateTime SalesPromotionDeadline { get; set; }
            [DisplayName("序號")]
            [Required]
            [StringLength(14, MinimumLength = 14, ErrorMessage = "序號欄位 只能輸入14位數")]
            public string Code { get; set; }
            [DisplayName("兌換狀態")]
            public bool? ExchangeStatus { get; set; }
            [DisplayName("驗證時間")]
            public DateTime? ExchangeTime { get; set; }
            [DisplayName("領取會員資訊")]
            public int? VolunteersId { get; set; }
            [DisplayName("會員註冊時間")]
            public DateTime VolunteersCreateTime { get; set; }
            [DisplayName("序號建立時間")]
            public DateTime CreateTime { get; set; }
        }

        public class Export
        {
            public string Id { get; set; }
            public string SalesPromotionName { get; set; }
            public string Code { get; set; }
            public string SalesPromotionDeadline { get; set; }
            public string SalesPromotionPoint { get; set; }
            public string ExchangeStatus { get; set; }
            public string ExchangeTime { get; set; }
            public string VolunteersId { get; set; }
        }

        public partial class ImportClass
        {
            public string Code { get; set; }
        }

        public class CheckResult
        {
            public Guid ID { get; set; }

            public bool Success { get; set; }

            public int RowCount { get; set; }

            public int ErrorCount { get; set; }

            public string ErrorMessage { get; set; }

        }

        public string Keyword = "";
        public DateTime? volunteers_time_start = null;
        public DateTime? volunteers_time_end = null;
        public DateTime? exchange_time_start = null;
        public DateTime? exchange_time_end = null;
        public int Search_SalesPromotionId = 0;
        public int Search_SalesPromotionDeadline = 0;
        public int Search_ExchangeStatus = 0;

        #region Get
        public List<SalesCodeModelShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                var data = Get().OrderByDescending(o => o.Id).Skip((p - 1) * take).Take(take);
                List<SalesCodeModelShow> item = Convert(data.ToList());
                db.Connection.Close();
                return item;
            }
            catch { return new List<SalesCodeModelShow>(); }
        }

        public IQueryable<SalesCode> Get()
        {
            s26webDataContext db = new s26webDataContext();
            IQueryable<SalesCode> data = db.SalesCode;


            if (Keyword != null && !(Keyword.Equals("")))
            {
                data = data.Where(Query(Keyword));
            }

            if (volunteers_time_start != null)
            {
                data = data.Where(w => w.VolunteersCreateTime.Value.AddHours(8) >= volunteers_time_start);
            }

            if (volunteers_time_end != null)
            {
                data = data.Where(w => w.VolunteersCreateTime.Value.AddHours(8) <= volunteers_time_end);
            }

            if (exchange_time_start != null)
            {
                data = data.Where(w => w.ExchangeTime.Value.AddHours(8) >= exchange_time_start);
            }

            if (exchange_time_end != null)
            {
                data = data.Where(w => w.ExchangeTime.Value.AddHours(8) <= exchange_time_end);
            }

            if (Search_SalesPromotionId != 0)
            {
                data = data.Where(w => w.SalesPromotionId == Search_SalesPromotionId);
            }

            if (Search_SalesPromotionDeadline == 1)
            {
                data = data.Where(w => w.SalesPromotionDeadline != null);
            }
            else if (Search_SalesPromotionDeadline == 2)
            {
                data = data.Where(w => w.SalesPromotionDeadline.AddHours(8) >= DateTime.UtcNow.AddHours(8));
            }
            else
            {
                data = data.Where(w => w.SalesPromotionDeadline.AddHours(8) < DateTime.UtcNow.AddHours(8));
            }

            if (Search_ExchangeStatus == 1)
            {
                data = data.Where(w => w.ExchangeStatus != null);
            }
            else if (Search_ExchangeStatus == 2)
            {
                data = data.Where(w => w.ExchangeStatus == true);
            }
            else
            {
                data = data.Where(w => w.ExchangeStatus == false);
            }
            db.Connection.Close();
            return data;
        }

        public List<SalesCodeModelShow> Convert(List<SalesCode> item)
        {

            List<SalesCodeModelShow> result = new List<SalesCodeModelShow>();
            foreach (var i in item)
            {
                if (i.ExchangeTime != null && i.VolunteersCreateTime != null)
                {
                    var Sales = new SalesCodeModelShow
                    {
                        Id = i.Id,
                        Code = i.Code,
                        SalesPromotionId = i.SalesPromotionId,
                        SalesPromotionName = Get_SalesPromotionName(i.SalesPromotionId),
                        SalesPromotionDeadline = i.SalesPromotionDeadline,
                        SalesPromotionPoint = i.SalesPromotionPoint,
                        ExchangeStatus = i.ExchangeStatus,
                        ExchangeTime = i.ExchangeTime.Value.AddHours(8),
                        VolunteersId = i.VolunteersId,
                        VolunteersCreateTime = i.VolunteersCreateTime.Value.AddHours(8),
                        CreateTime = i.CreateTime.AddHours(8),
                    };
                    result.Add(Sales);
                }
                else
                {
                    var Sales = new SalesCodeModelShow
                    {
                        Id = i.Id,
                        Code = i.Code,
                        SalesPromotionId = i.SalesPromotionId,
                        SalesPromotionName = Get_SalesPromotionName(i.SalesPromotionId),
                        SalesPromotionDeadline = i.SalesPromotionDeadline,
                        SalesPromotionPoint = i.SalesPromotionPoint,
                        ExchangeStatus = i.ExchangeStatus,
                        VolunteersId = i.VolunteersId,
                        CreateTime = i.CreateTime.AddHours(8),
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

        public SalesCodeModelShow Get_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            SalesCodeModelShow data = new SalesCodeModelShow();

            var item = db.SalesCode.FirstOrDefault(f => f.Id == id);
            data.Code = item.Code;
            data.SalesPromotionId = item.SalesPromotionId;
            data.SalesPromotionPoint = item.SalesPromotionPoint;
            data.SalesPromotionDeadline = item.SalesPromotionDeadline.AddHours(8);
            data.CreateTime = item.CreateTime.AddHours(8);
            db.Connection.Close();
            return data;
        }
        #endregion

        #region Insert
        public int Insert(SalesCodeModelShow item)
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                SalesCode new_item = new SalesCode
                {
                    Code = item.Code,
                    SalesPromotionId = item.SalesPromotionId,
                    SalesPromotionPoint = Get_SalesPromotionPoint(item.SalesPromotionId),
                    SalesPromotionDeadline = Get_SalesPromotionDeadline(item.SalesPromotionId),
                    ExchangeStatus = false,
                    VolunteersId = 0,
                    CreateTime = DateTime.UtcNow
                };
                db.SalesCode.InsertOnSubmit(new_item);
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
        public int Update(SalesCodeModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.SalesCode.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
                    data.Code = item.Code;
                    data.SalesPromotionId = item.SalesPromotionId;
                    data.SalesPromotionPoint = item.SalesPromotionPoint;
                    data.SalesPromotionDeadline = item.SalesPromotionDeadline;
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
            var data = db.SalesCode.Where(w => w.Id == Id);
            if (data.Any())
            {
                db.SalesCode.DeleteAllOnSubmit(data);
                db.SubmitChanges();
            }
            db.Connection.Close();
        }
        #endregion

        #region query

        private string Query(string query = "")
        {
            string sql = "";
            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "Code", "OR", "( \"" + query + "\")", ".Contains", false);
            }
            return sql;
        }
        #endregion

        #region 匯入
        /// <summary>
        /// 檢查匯入的 CSV資料.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="importSalesCodes">The import zip codes.</param>
        /// <returns></returns>
        public CheckResult CheckImportData(
            string fileName,
            List<ImportClass> importSalesCodes)
        {
            var result = new CheckResult();

            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {
                result.ID = Guid.NewGuid();
                result.Success = false;
                result.ErrorCount = 0;
                result.ErrorMessage = "匯入的資料檔案不存在";
                return result;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //欄位對映
            excelFile.AddMapping<ImportClass>(x => x.Code, "序號");


            //SheetName
            var excelContent = excelFile.Worksheet<ImportClass>("促銷碼管理");
            int errorCount = 0;
            int rowIndex = 1;
            var importErrorMessages = new List<string>();

            //檢查資料
            foreach (var row in excelContent)
            {

                var errorMessage = new StringBuilder();
                var SalesCode = new ImportClass();

                //CityName
                if (string.IsNullOrWhiteSpace(row.Code))
                {
                    errorMessage.Append("不能為空白 ");
                }
                else
                {
                    bool Check = IsNumOrEn(row.Code);
                    if (row.Code.Length == 14 && Check == true)
                    {
                    }
                    else if (row.Code.Length == 14 && Check == false)
                    {
                        errorMessage.Append("包含特殊字元 ");
                    }
                    else if (row.Code.Length > 14 && Check == true)
                    {
                        errorMessage.Append("大於14位數 ");
                    }
                    else if (row.Code.Length > 14 && Check == false)
                    {
                        errorMessage.Append("大於14位數，且包含特殊字元 ");
                    }
                    else if (row.Code.Length < 14 && Check == true)
                    {
                        errorMessage.Append("小於14位數 ");
                    }
                    else
                    {
                        errorMessage.Append("小於14位數，且包含特殊字元 ");
                    }
                }

                SalesCode.Code = row.Code;
                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errorCount += 1;
                    importErrorMessages.Add(string.Format(
                        "第 {0} 列資料發現錯誤：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                importSalesCodes.Add(SalesCode);
                rowIndex += 1;
            }

            try
            {
                result.ID = Guid.NewGuid();
                result.Success = errorCount.Equals(0);
                result.RowCount = importSalesCodes.Count;
                result.ErrorCount = errorCount;

                string allErrorMessage = string.Empty;

                foreach (var message in importErrorMessages)
                {
                    allErrorMessage += message;
                }

                result.ErrorMessage = allErrorMessage;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveImportData(IEnumerable<ImportClass> importSalesCodes, int SalesPromotionId = 0, string fileName = "")
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                foreach (var item in importSalesCodes)
                {
                    SalesCode new_item = new SalesCode
                    {
                        SalesPromotionId = SalesPromotionId,
                        SalesPromotionPoint = Get_SalesPromotionPoint(SalesPromotionId),
                        SalesPromotionDeadline = Get_SalesPromotionDeadline(SalesPromotionId),
                        Code = item.Code,
                        ExchangeStatus = false,
                        VolunteersId = 0,
                        CreateTime = DateTime.UtcNow
                    };
                    db.SalesCode.InsertOnSubmit(new_item);
                    db.SubmitChanges();
                    db.Connection.Close();
                }
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            catch
            {
                throw;
            }
        }

        public static bool IsAlphaNumeric(String str)
        {
            return (str != string.Empty && !Regex.IsMatch(str, "[^a-zA-Z0-9]"))
                ? true : false;
        }

        public bool IsNumOrEn(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }
        #endregion

        #region 匯出
        public static List<Export> Get_Export(SalesCodeModel data)
        {

            /*-------------------匯出宣告-------------------*/
            s26webDataContext db = new s26webDataContext();
            Export export = new Export();
            List<Export> exp = new List<Export>();
            List<string> bir = new List<string> { "", "", "" };
            var newdata = data.Get().OrderByDescending(o => o.Id);
            /*-------------------匯出宣告End-------------------*/
            foreach (var d in newdata)
            {

                export.Id = d.Id.ToString();
                export.SalesPromotionName = Get_SalesPromotionName(d.SalesPromotionId);
                export.Code = d.Code;
                export.SalesPromotionDeadline = d.SalesPromotionDeadline.ToString("yyyy/MM/dd");
                export.SalesPromotionPoint = d.SalesPromotionPoint.ToString();
                if (d.ExchangeStatus == true)
                {
                    export.ExchangeStatus = "已兌換";
                }
                else
                {
                    export.ExchangeStatus = "未兌換";
                }
                if (d.ExchangeTime != null)
                {
                    export.ExchangeTime = d.ExchangeTime.Value.ToString("yyyy/MM/dd HH:mm:ss");
                }
                if (d.VolunteersId != 0)
                {
                    export.VolunteersId = d.VolunteersId.Value.ToString("");
                }
                exp.Add(export);
                export = new Export();
            }
            db.Connection.Close();
            return exp;
        }

        public MemoryStream Get_ExcelData_main(List<Export> data)
        {
            List<string> header = new List<string>();
            header.Add("編號");
            header.Add("促銷碼活動");
            header.Add("序號");
            header.Add("序號到期日");
            header.Add("點數");
            header.Add("兌換狀態");
            header.Add("驗證時間");
            header.Add("領取會員編號");

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
                sheet.GetRow(i + 1).CreateCell(0).SetCellValue(data[i].Id);
                sheet.GetRow(i + 1).CreateCell(1).SetCellValue(data[i].SalesPromotionName);
                sheet.GetRow(i + 1).CreateCell(2).SetCellValue(data[i].Code);
                sheet.GetRow(i + 1).CreateCell(3).SetCellValue(data[i].SalesPromotionDeadline);
                sheet.GetRow(i + 1).CreateCell(4).SetCellValue(data[i].SalesPromotionPoint);
                sheet.GetRow(i + 1).CreateCell(5).SetCellValue(data[i].ExchangeStatus);
                sheet.GetRow(i + 1).CreateCell(6).SetCellValue(data[i].ExchangeTime);
                sheet.GetRow(i + 1).CreateCell(7).SetCellValue(data[i].VolunteersId);
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

        #region 取得促銷碼活動資料

        //取得促銷碼活動名稱
        public static string Get_SalesPromotionName(int SalesPromotionNameId)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                string str = db.SalesPromotion.FirstOrDefault(w => w.Id == SalesPromotionNameId).Name;
                db.Connection.Close();
                return str;
            }
            catch { }
            return null;
        }

        //取得促銷碼活動點數
        public static int Get_SalesPromotionPoint(int SalesPromotionNameId)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                int point = db.SalesPromotion.FirstOrDefault(w => w.Id == SalesPromotionNameId).Point;
                db.Connection.Close();
                return point;
            }
            catch { }
            return 0;
        }

        //取得促銷碼活動到期日
        public static DateTime Get_SalesPromotionDeadline(int SalesPromotionNameId)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                DateTime deadline = db.SalesPromotion.FirstOrDefault(w => w.Id == SalesPromotionNameId).Deadline;
                db.Connection.Close();
                return deadline;
            }
            catch { }
            return DateTime.UtcNow;
        }

        //促銷碼活動 - 下拉式選單
        public static List<SelectListItem> Select_SalesPromotion(int SalesPromotionNameId, bool all)
        {
            s26webDataContext db = new s26webDataContext();
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Selected = SalesPromotionNameId == 0,
                Text = all ? "全部" : "請選擇",
                Value = "0"
            });
            data.AddRange(db.SalesPromotion.Where(w => w.Id != null).OrderBy(o => o.Id).Select(s =>
                new SelectListItem
                {
                    Selected = SalesPromotionNameId == s.Id,
                    Text = s.Name,
                    Value = s.Id.ToString()
                }));
            db.Connection.Close();
            return data;
        }
        #endregion

        #region 前台 - 輸入促銷碼
        public int Check_Code_Front(SalesCodeModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                //取得促銷碼資料
                var Sale = db.SalesCode.FirstOrDefault(w => w.Code == item.Code);
                //取得會員資料
                var Vol = db.Volunteers.FirstOrDefault(f => f.Id == item.VolunteersId);

                if (Sale != null && Vol != null)
                {
                    if (Sale.ExchangeStatus == true)
                    {
                        return -1;
                    }
                    if (Vol.Point == null)
                    {
                        Vol.Point = 0;
                    }
                  
                    Vol.Point += Sale.SalesPromotionPoint;
                    Sale.ExchangeStatus = true;
                    Sale.ExchangeTime = DateTime.UtcNow.AddHours(-8);
                    Sale.VolunteersId = item.VolunteersId;
                    Sale.VolunteersCreateTime = Vol.CreateTime;

                    db.SubmitChanges();
                    db.Connection.Close();
                    return Sale.Id;
                }
                db.Connection.Close();
                return -2;
            }
            catch { return -2; }
        }
        #endregion

    }
}