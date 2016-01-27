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
using System.Data.Linq.SqlClient;

namespace s26web.Areas.shb.Models
{
    public class QuestionnaireModel
    {

        public string Keyword = "";
        public string InvKeyword = "";
        public string choice = "noassign";
        public DateTime? order_time_start = null;
        public DateTime? order_time_end = null;
        public DateTime? time_start = null;
        public DateTime? time_end = null;

        //=======Declare, but not used=======
        //public int order_states = 0;
        //public int order_from = 3;
        //public int City = 0;
        //public int Area = 0;
        //public int Osid = 0;
        //public string Sort = "OrdersTime";
        //===================================

        /// <summary>
        /// Using [MainMenu] class to join [QuestionnaireMain] and [Volunteers]
        /// </summary>
        public class MainMenu
        {
            public QuestionnaireMain qm = new QuestionnaireMain();
            public Volunteers vl = new Volunteers();
        }

        public class QuestionnaireMain
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("會員編號")]
            public int UserId { get; set; }
            [DisplayName("活動問卷編號")]
            public int CategoryId { get; set; }
            [DisplayName("問卷送出時間")]
            public DateTime SubmitTime { get; set; }
        }

        public class QuestionnaireDetail
        {
            [Key]
            [DisplayName("會員姓名")]
            public string UserName { get; set; }
            [DisplayName("問題名稱")]
            public string QuestionId { get; set; }
            [DisplayName("使用者填答")]
            public string UserAns { get; set; }
        }

        public class QuestionnaireCategoryList
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("活動問卷名稱")]
            public string ActivityName { get; set; }
        }

        public class QuestionnaireData
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("活動問卷名稱")]
            public string QuestionName { get; set; }
        }

        public class Export_ordinv
        {
            [Key]
            [DisplayName("會員身份")]
            public string BrandName { get; set; }
            [DisplayName("會員姓名")]
            public string MemberName { get; set; }
            [DisplayName("手機號碼")]
            public string MemberMobile { get; set; }
            [DisplayName("Email")]
            public string MemberEmail { get; set; }
            [DisplayName("寶寶生日")]
            public string BabyBirthday { get; set; }
            [DisplayName("問卷編號")]
            public string SheetNo { get; set; }
            [DisplayName("問卷送出時間")]
            public string SubmitTime { get; set; }
        }

        public QuestionnaireModel()
        { }

        public QuestionnaireModel(DateTime start, DateTime end)
        {
            this.time_start = start;
            this.time_end = end;
        }

        #region get

        /// <summary>
        /// Get all data use [MainMenu] class.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<MainMenu> Get_Menu(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = this.GetOI().OrderByDescending(o=>o.qm.Id).Skip((p - 1) * take).Take(take);
                List<MainMenu> mm = new List<MainMenu>();
                foreach (var i in data)
                {
                    mm.Add(new MainMenu
                    {
                        qm = new QuestionnaireMain
                        {
                            Id = i.qm.Id,
                            CategoryId = i.qm.CategoryId,
                            UserId = i.qm.UserId,
                            SubmitTime = i.qm.SubmitTime
                        },
                        vl = Get_Volunteers(i.qm.UserId)
                    });
                }
                db.Connection.Close();
                return mm;
            }
            catch
            {
                return new List<MainMenu>();
            }
        }

        public List<MainMenu> GetOI()
        {
            //QuestionnaireModel ord = new QuestionnaireModel();
            //ord.order_time_start = time_start;
            //ord.order_time_end = time_end;
            //ord.InvKeyword = Keyword;
            //ord.choice = choice;
            var idata = Get_All();
            return idata;
        }

        /// <summary>
        /// Get All Data
        /// </summary>
        /// <returns></returns>
        public List<MainMenu> Get_All()
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = Get();
                List<MainMenu> result = new List<MainMenu>();
                foreach (var i in data)
                {
                    result.Add(i);
                }
                db.Connection.Close();
                return result;
            }
            catch { return new List<MainMenu>(); }
        }

        private List<MainMenu> Get()
        {
            s26webDataContext db = new s26webDataContext();
            var data1 = db.QuestionnaireMain.ToList();
            List<Volunteers> data2 = null;
            if (Keyword != null && !(Keyword.Equals("")))
            {
                data2 = db.Volunteers.Where(Query(Keyword)).ToList();
            }
            else { data2 = db.Volunteers.ToList(); }
            List<MainMenu> rlt = new List<MainMenu>();
            foreach (var index2 in data2)
            {
                foreach (var index1 in data1)
                {
                    if (index2.Id == index1.UserId)
                    {
                        rlt.Add(new MainMenu
                        {
                            vl = new Volunteers
                            {
                                BrandName = index2.BrandName,
                                Name = index2.Name,
                                Mobile = index2.Mobile,
                                Email = index2.Email,
                                BabyBirthday = index2.BabyBirthday
                            },
                            qm = new QuestionnaireMain
                            {
                                CategoryId = index1.CategoryId,
                                Id = index1.Id,
                                SubmitTime = index1.SubmitTime,
                                UserId = index1.UserId
                            }
                        });
                    }
                }
            }
            if (time_start != null)
            {
                rlt = rlt.Where(w => w.qm.SubmitTime >= time_start.Value).ToList();
            }
            if (time_end != null)
            {
                rlt = rlt.Where(w => w.qm.SubmitTime <= time_end.Value).ToList();
            }
            if (choice != null && !(choice.Equals("noassign")) && !(choice.Equals("0")))
            {
                rlt = rlt.Where(w => w.qm.CategoryId == int.Parse(choice)).ToList();
            }
            db.Connection.Close();
            return rlt;
        }

        public List<QuestionnaireDetail> Get_Detail(int MainId)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.QuestionnaireDetail.Where(w => w.MainId == MainId);
            List<QuestionnaireDetail> mm = new List<QuestionnaireDetail>();
            foreach (var QDetail in data)
            {
                mm.Add(new QuestionnaireDetail
                {
                    QuestionId = Get_QuestionName(QDetail.QuestionId),
                    UserAns = QDetail.UserAnswer
                });
            }
            db.Connection.Close();
            return mm;
        }

        private Volunteers Get_Volunteers(int sourceId)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.Volunteers.FirstOrDefault(w => w.Id == sourceId);
            db.Connection.Close();
            if (data != null)
            {
                data.BrandName = Traslate_BrandName(data.BrandName);
                return data;
            }
            return null;
        }

        public Method.Paging Get_PageIO(int p = 1, int take = 10, int pages = 5)
        {
            return Method.Get_Page(Get_CountIO(), p, take, pages);
        }

        /// <summary>
        /// Count the data
        /// </summary>
        /// <returns></returns>
        public int Get_CountIO()
        {
            try
            {
                return GetOI().Count();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Excel Export Function
        /// </summary>
        /// <returns></returns>
        public List<Export_ordinv> Get_All_Export()
        {
            Export_ordinv export = new Export_ordinv();
            List<Export_ordinv> exp = new List<Export_ordinv>();
            var data = this.GetOI();
            foreach (var d in data)
            {
                export.BabyBirthday = d.vl.BabyBirthday.ToShortDateString();
                export.BrandName = Traslate_BrandName(d.vl.BrandName);
                export.MemberMobile = d.vl.Mobile;
                export.MemberEmail = d.vl.Email;
                export.MemberName = d.vl.Name;
                export.SheetNo = d.qm.Id.ToString();
                export.SubmitTime = d.qm.SubmitTime.AddHours(8).ToString("yyyy/MM/dd HH:mm:ss");
                exp.Add(export);
                export = new Export_ordinv();
            }
            return exp;
        }

        //public List<QuestionnaireMain> Get_Data()
        //{
        //    s26webDataContext db = new s26webDataContext();
        //    var data = db.QuestionnaireMain;
        //    List<QuestionnaireMain> mm = new List<QuestionnaireMain>();
        //    foreach (var i in data)
        //    {
        //        mm.Add(new QuestionnaireMain
        //        {
        //            Id = i.Id,
        //            UserId = i.UserId,
        //            CategoryId = i.CategoryId,
        //            SubmitTime = i.SubmitTime
        //        });
        //    }
        //    db.Connection.Close();
        //    return mm;
        //}

        #endregion

        #region query

        private string Query(string query = "")
        {
            string sql = "";
            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "Name", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Mobile", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Email", "OR", "( \"" + query + "\")", ".Contains", false);
            }
            return sql;
        }

        public List<QuestionnaireCategoryList> Get_Activity()
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.Category.Where(a => a.Fun_Id == 11).OrderBy(i => i.Id);
            List<QuestionnaireCategoryList> rltList = new List<QuestionnaireCategoryList>();
            foreach (var item in data)
            {
                rltList.Add(new QuestionnaireCategoryList
                {
                    Id = item.Id,
                    ActivityName = item.Name
                });
            }
            db.Connection.Close();
            return rltList;
        }

        public MemoryStream Get_ExcelData2(List<Export_ordinv> data)
        {
            List<string> header = new List<string>();
            header.Add("會員身份");
            header.Add("會員姓名");
            header.Add("手機");
            header.Add("Email");
            header.Add("寶寶生日");
            header.Add("問卷代碼");
            header.Add("問卷送出時間");
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
                sheet.GetRow(i + 1).CreateCell(0).SetCellValue(data[i].BrandName);
                sheet.GetRow(i + 1).CreateCell(1).SetCellValue(data[i].MemberName);
                sheet.GetRow(i + 1).CreateCell(2).SetCellValue(data[i].MemberMobile);
                sheet.GetRow(i + 1).CreateCell(3).SetCellValue(data[i].MemberEmail);
                sheet.GetRow(i + 1).CreateCell(4).SetCellValue(data[i].BabyBirthday);
                sheet.GetRow(i + 1).CreateCell(5).SetCellValue(data[i].SheetNo);
                sheet.GetRow(i + 1).CreateCell(6).SetCellValue(data[i].SubmitTime);
            }
            foreach (var i in header)
            {
                sheet.AutoSizeColumn(header.IndexOf(i));
                //if (header.IndexOf(i) <= 6)
                //    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 14);
                //else if (header.IndexOf(i) == 7)
                //{
                //    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 31);
                //}
                //else if (header.IndexOf(i) == 8 || header.IndexOf(i) == 9)
                //    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 50);
                //else if (header.IndexOf(i) == 12)
                //    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 50);
                //else if (header.IndexOf(i) > 9 && header.IndexOf(i) <= 10)
                //    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 9);
                //else
                //    sheet.SetColumnWidth(header.IndexOf(i), (short)256 * 50);
            }
            workbook.Write(ms);
            ms.Position = 0;
            ms.Flush();
            return ms;
        }

        private string Traslate_BrandName(string source)
        {
            try
            {
                int temp = int.Parse(source);
                s26webDataContext db = new s26webDataContext();
                return db.Category.FirstOrDefault(w => w.Id == temp).Name;
            }
            catch { return "Error"; }
        }
        private string AnotherTableQuery(int tempId)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.Volunteers.FirstOrDefault(w => w.Id == tempId);
            db.Connection.Close();
            if (data != null) { return data.Name; }
            return "can not found! please connect RD";
        }

        private string Get_QuestionName(int sourceId)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.QuestionData.FirstOrDefault(w => w.Id == sourceId);
            db.Connection.Close();
            if (data != null) { return data.Content; }
            return "can not found! please connect RD";
        }

        public string Get_UserName(int sourceId)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.QuestionnaireMain.FirstOrDefault(w => w.Id == sourceId);
            db.Connection.Close();
            if (data != null) { return AnotherTableQuery(data.UserId); }
            return "can not found! please connect RD";
        }

        #endregion
    }
}