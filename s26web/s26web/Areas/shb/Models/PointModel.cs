using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using s26web.Models;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace s26web.Areas.shb.Models
{
    public class PointModel
    {
        public class PointModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("會員編號")]
            public int Vid { get; set; }
            [DisplayName("會員姓名")]
            public string Name { get; set; }
            [DisplayName("手機號碼")]
            public string Mobile { get; set; }
            [DisplayName("點數種類")]
            public int Category { get; set; }
            [DisplayName("點數種類")]
            public string Category_Name { get; set; }
            [DisplayName("發送點數原因")]
            public string CategoryReason { get; set; }
            [DisplayName("點數")]
            public int Point { get; set; }
            [DisplayName("發送結果")]
            public bool Result { get; set; }
            [DisplayName("發送失敗原因")]
            public string FailReason { get; set; }
            [DisplayName("發送管理者")]
            public int UId { get; set; }
            [DisplayName("發送管理者")]
            public string UserName { get; set; }
            [DisplayName("點數發送時間")]
            public DateTime SendTime { get; set; }

            public List<Category> Point_Category { get; set; }
        }
        public class Export
        {
            public string Id { get; set; }
            public string Vid { get; set; }
            public string Name { get; set; }
            public string Mobile { get; set; }
            public string Category { get; set; }
            public string Category_Name { get; set; }
            public string CategoryReason { get; set; }
            public string Point { get; set; }
            public string Result { get; set; }
            public string FailReason { get; set; }
            public string UserName { get; set; }
            public string SendTime { get; set; }
        }

        public string Keyword = "";
        public int result = 0;
        public int CategorySelectList = 0;
        public DateTime? SendTime_start = null;
        public DateTime? SendTime_end = null;

        public List<int> Cid=new List<int>();
        private int Fun_Id;

        public void Clear_Params()
        {
            Keyword = "";
            Cid = new List<int>();
            SendTime_start = null;
            SendTime_end = null;
        }

        public List<Category> Get_Category()
        {
                s26webDataContext db = new s26webDataContext();
                PointModelShow data = new PointModelShow();
                data.Point_Category = db.Category.Where(w => w.Fun_Id == 3).ToList();
                db.Connection.Close();
                return data.Point_Category;
        }
        public List<PointModelShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = Get().OrderByDescending(o => o.Id).Skip((p - 1) * take).Take(take);
                List<PointModelShow> result = new List<PointModelShow>();
                foreach (var i in data)
                {
                    result.Add(i);
                }
                db.Connection.Close();
                return result;
            }
            catch { return new List<PointModelShow>(); }
        }
        private IQueryable<PointModelShow> Get()
        {
            s26webDataContext db = new s26webDataContext();
            var data = from p in db.Point_List
                       select new PointModelShow
                       {
                           Id = p.Id,
                           Vid = p.Vid,
                           Name = p.Name,
                           Mobile = p.Mobile,
                           Category = p.Category,
                           Category_Name = p.Category_Name,
                           CategoryReason = p.CategoryReason,
                           Point = p.Point,
                           Result = p.Result,
                           FailReason = p.FailReason,
                           UserName = p.UserName,
                           SendTime = p.SendTime.AddHours(8)
                       };
            if (Keyword != "")
            {
                data = data.Where(Query(Keyword));
            }
            if (SendTime_start != null && SendTime_end != null)
            {
                data = data.Where(w => w.SendTime <= SendTime_end.Value.AddHours(-8) && w.SendTime >= SendTime_start.Value.AddHours(-8));
            }
            if (CategorySelectList > 0)
            {
                data = data.Where(w => w.Category == CategorySelectList);
            }
            if(result == 1)
            {
                data = data.Where(w => w.Result == true);
            }
            if (result == 2)
            {
                data = data.Where(w => w.Result == false);
            }

            db.Connection.Close();
            int i = data.Count();
            return data;
        }
        private string Query(string query = "")
        {
            string sql = "";

            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "Name", "OR", "( \"" + query + "\")", ".Contains", false);
                //sql = Method.SQL_Combin(sql, "Email", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Mobile", "OR", "( \"" + query + "\")", ".Contains", false);
            }
            return sql;
        }
        public Method.Paging Get_Page(int p = 1, int take = 10)
        {
            return Method.Get_Page(Get_Count(), p, take);
        }

        public int Get_Count()
        {
            return Get().Count();
        }

        #region Insert
        public int Insert(PointModel.PointModelShow item)
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                Point new_item = new Point
               {
                   VId = db.Volunteers.FirstOrDefault(f => f.Mobile == item.Mobile).Id,
                   Category = item.Category,
                   CategoryReason = item.CategoryReason,
                   Point1 = item.Point,
                   Result = item.Result,
                   FailReason = item.FailReason,
                   UId = item.UId,
                   SendTime = DateTime.UtcNow
               };
                db.Point.InsertOnSubmit(new_item);
                db.SubmitChanges();

                var vol = db.Volunteers.FirstOrDefault(f => f.Mobile == item.Mobile);
                if (vol != null)
                {
                    if (vol.Point == null)
                        vol.Point = 0;
                    vol.Point += item.Point;
                    db.SubmitChanges();
                }
                db.Connection.Close();
                return new_item.Id;
            }
            catch { return -1; }
        }
        #endregion 
        
        #region update
        public int Update(PointModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Product.FirstOrDefault(f => f.Id == item.Id);
                if (data != null)
                {
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

        #region Export
        public static List<Export> Get_Export(PointModel datas)
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
                export.Id = d.Id.ToString();
                export.Name = d.Name;
                export.Mobile = d.Mobile;
                export.Category_Name = d.Category_Name;
                export.Point = d.Point.ToString();
                export.Result = d.Result.ToString();
                export.FailReason = d.FailReason;
                export.UserName = d.UserName;
                export.SendTime = d.SendTime.ToString("yyyy/MM/dd HH:mm");

                exp.Add(export);
                export = new Export();
            }
            db.Connection.Close();
            return exp;
        }

        public MemoryStream Get_ExcelData_main(List<Export> data)
        {
            List<string> header = new List<string>();
            header.Add("點數編號");
            header.Add("姓名");
            header.Add("手機");
            header.Add("點數種類");
            header.Add("點數");
            header.Add("發送結果");
            header.Add("失敗原因");
            header.Add("發送管理者");
            header.Add("點數發送時間");

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
                sheet.GetRow(i + 1).CreateCell(1).SetCellValue(data[i].Name);
                sheet.GetRow(i + 1).CreateCell(2).SetCellValue(data[i].Mobile);
                sheet.GetRow(i + 1).CreateCell(3).SetCellValue(data[i].Category_Name);
                sheet.GetRow(i + 1).CreateCell(4).SetCellValue(data[i].Point);
                sheet.GetRow(i + 1).CreateCell(5).SetCellValue(data[i].Result);
                sheet.GetRow(i + 1).CreateCell(6).SetCellValue(data[i].FailReason);
                sheet.GetRow(i + 1).CreateCell(7).SetCellValue(data[i].UserName);
                sheet.GetRow(i + 1).CreateCell(8).SetCellValue(data[i].SendTime);
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
    }
}