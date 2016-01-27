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
    public class OrdersModel
    {
        public string Keyword = "";
        public string InvKeyword = "";
        public string StatesSelect = "";
        public string ProductSelect = "";
        public DateTime? time_start = null;
        public DateTime? time_end = null;

        public class MyList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class OrdersModelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("訂單編號")]
            public string Osid { get; set; }
            [DisplayName("會員編號")]
            public int VolunteersId { get; set; }
            [DisplayName("訂購會員")]
            public string Vol_Name { get; set; }
            [DisplayName("手機")]
            public string Vol_Mobile { get; set; }
            [DisplayName("訂購產品代碼")]
            public int OrdersProduct { get; set; }
            [DisplayName("訂購產品名稱")]
            public string OrdersProductName { get; set; }
            [DisplayName("訂購產品內容")]
            public string OrdersProductContent { get; set; }
            [DisplayName("單價")]
            public int Price { get; set; }
            [DisplayName("訂購數量")]
            public int Quantity { get; set; }
            [DisplayName("總價格")]
            public int TotalPrice { get; set; }
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
            [DisplayName("分享點數")]
            public bool SharePoint { get; set; }
            [DisplayName("備註")]
            public string Remarks { get; set; }
            [DisplayName("訂單狀態")]
            public int OrdersStates { get; set; }
            [DisplayName("訂單狀態")]
            public string States { get; set; }
            [DisplayName("訂單時間")]
            public DateTime OrdersTime { get; set; }
            [DisplayName("收貨時間")]
            [Required]
            public int ReciveTimeChoice { get; set; }
            [DisplayName("發票種類代碼")]
            public int RecieptId { get; set; }
            [DisplayName("發票種類")]
            public string RecieptCategory { get; set; }
            [DisplayName("發票抬頭")]
            public string RecieptName { get; set; }
            [DisplayName("發票統編")]
            public string RecieptNo { get; set; }
            [DisplayName("最後更新管理者")]
            public int? UpdateUserId { get; set; }
            [DisplayName("最後更新管理者")]
            public string UpdateUserName { get; set; }

            public class Export_Order
            {
                [Key]
                [DisplayName("訂單編號")]
                public string Osid { get; set; }
                [DisplayName("訂購會員")]
                public string MemberName { get; set; }
                [DisplayName("手機")]
                public string MemberMobile { get; set; }
                [DisplayName("訂購產品名稱")]
                public string Product { get; set; }
                [DisplayName("總價格")]
                public string TotalPrice { get; set; }
                [DisplayName("收件人")]
                public string Name { get; set; }
                [DisplayName("收件人手機")]
                public string Phone { get; set; }
                [DisplayName("分享點數")]
                public string SharePoint { get; set; }
                [DisplayName("備註")]
                public string Memo { get; set; }
                [DisplayName("訂單狀態")]
                public string State { get; set; }
                [DisplayName("訂單時間")]
                public string OrderTime { get; set; }
            }

            public OrdersModelShow Set_Other()
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

                    //設定Product
                    Product Temp_Product = (Product)db.Product.FirstOrDefault(w => w.Id == this.OrdersProduct);
                    this.OrdersProductName = Temp_Product.Name;
                    this.OrdersProductContent = Temp_Product.Content;
                    this.Price = Temp_Product.Price;

                    //設定Gift
                    Invoice Temp_Gift = (Invoice)db.Invoice.FirstOrDefault(w => w.OrdersId == this.Id);
                    this.RecieptCategory = Get_RecipetCategory(Temp_Gift.InvoiceCategory);
                    this.RecieptName = Temp_Gift.InvoiceName;
                    this.RecieptNo = Temp_Gift.InvoiceNo;

                    return this;
                }
                catch
                {
                    return null;
                }
            }

            public string Get_RecipetCategory(int id)
            {
                try
                {
                    s26webDataContext db = new s26webDataContext();
                    Category temp = (Category)db.Category.FirstOrDefault(i => i.Id == id);
                    return temp.Name;
                }
                catch
                {
                    return "Not Found";
                }
            }

            public void ScoreUpdate(OrdersModelShow source, int userId)
            {
                s26webDataContext db = new s26webDataContext();
                Orders compareTemp = db.Orders.FirstOrDefault(w => w.Osid == source.Osid);

                if (this.OrdersStates != compareTemp.OrdersStates)
                {
                    OrdersStatesUpdate(source.Osid, source.OrdersStates, userId);
                }
                //Expand Here and build new funtion
            }

            /// <summary>
            /// OrdersStates Update Function
            /// </summary>
            /// <param name="Osid"></param>
            /// <param name="changeState"></param>
            /// <param name="userId"></param>
            private void OrdersStatesUpdate(string Osid, int changeState, int userId)
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.Orders.FirstOrDefault(w => w.Osid == Osid);
                data.OrdersStates = changeState;
                data.UpdateUserId = userId;
                data.UpdateTime = DateTime.Now.AddHours(-8);
                db.SubmitChanges();
            }
        }

        public List<OrdersModelShow> Get_Data(int p = 1, int take = 10)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = this.Get_All().OrderByDescending(o => o.Id).Skip((p - 1) * take).Take(take);
                return data.ToList();
            }
            catch
            {
                return new List<OrdersModelShow>();
            }

        }

        public OrdersModelShow Get_Detail(string OsidFrom)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                OrdersModelShow data = Get_Data_And_Filter().Where(i => i.Osid == OsidFrom).ToArray()[0];
                return data;
            }
            catch
            {
                return new OrdersModelShow();
            }
        }

        /// <summary>
        /// Get All Data
        /// </summary>
        /// <returns></returns>
        public List<OrdersModelShow> Get_All()
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data = Get_Data_And_Filter();
                List<OrdersModelShow> result = new List<OrdersModelShow>();
                foreach (var i in data)
                {
                    result.Add(i);
                }
                db.Connection.Close();
                return result;
            }
            catch { return new List<OrdersModelShow>(); }
        }

        /// <summary>
        /// Get And Filter Data
        /// </summary>
        /// <returns></returns>
        private List<OrdersModelShow> Get_Data_And_Filter()
        {
            s26webDataContext db = new s26webDataContext();
            var temp = db.Orders;
            List<OrdersModelShow> data = new List<OrdersModelShow>();
            foreach (var index in temp)
            {
                data.Add(Set_Info(index));
            }
            data=Select_Data(data);
            db.Connection.Close();
            return data;
        }

        /// <summary>
        /// Filter Data
        /// </summary>
        /// <param name="Source"></param>
        private List<OrdersModelShow> Select_Data(List<OrdersModelShow> Source)
        {
            if (Keyword != null && !(Keyword.Equals("")))
            {
                Source = Source.Where(Query(Keyword)).ToList();
            }

            if (time_start != null && time_end != null&&(time_end.Value >= time_start.Value))
            {
                Source = Source.Where(w => w.OrdersTime >= time_start.Value && w.OrdersTime <= time_end.Value).ToList();
            }

            if (StatesSelect != null && !(StatesSelect.Equals("")) && !(StatesSelect.Equals("0")))
            {
                Source = Source.Where(w => w.OrdersStates == int.Parse(StatesSelect)).ToList();
            }

            if (ProductSelect != null && !(ProductSelect.Equals("")) && !(ProductSelect.Equals("0")))
            {
                Source = Source.Where(w => w.OrdersProduct == int.Parse(ProductSelect)).ToList();
            }
            return Source;
        }


        public List<MyList> Get_OrdersStatesOption()
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var temp = db.Category.Where(w => w.Fun_Id == 5 && w.Memo.Equals("OrdersState"));
                List<MyList> rlt = new List<MyList>();
                foreach (var i in temp)
                {
                    rlt.Add(new MyList
                    {
                        Id = i.Id,
                        Name = i.Name
                    });
                }
                return rlt;
            }
            catch
            {
                return null;
            }
        }

        public List<MyList> Get_OrdersProductOption()
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var temp = db.Product;
                List<MyList> rlt = new List<MyList>();
                foreach (var i in temp)
                {
                    rlt.Add(new MyList
                    {
                        Id = i.Id,
                        Name = i.Name
                    });
                }
                return rlt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Build SQL-Sentence
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string Query(string query = "")
        {
            string sql = "";
            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "Osid", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Vol_Name", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Vol_Mobile", "OR", "( \"" + query + "\")", ".Contains", false);
            }

            return sql;
        }

        public OrdersModelShow Set_Info(Orders source)
        {
            try
            {
                OrdersModelShow rlt = new OrdersModelShow()
                {
                    Id = source.Id,
                    Osid = source.Osid,
                    VolunteersId = source.VolunteersId.Value,
                    OrdersProduct = source.ProductId,
                    Quantity = source.Order_Quantity,
                    TotalPrice = source.TotalPrice,
                    Name = source.Name,
                    Mobile = source.Mobile,
                    CityId = source.CityId,
                    AreaId = source.AreaId,
                    Address = source.Address,
                    SharePoint = source.SharePoint,
                    Remarks = source.Remarks,
                    OrdersStates = source.OrdersStates,
                    States = Get_State(source.OrdersStates),
                    OrdersTime = source.OrdersTime,
                    ReciveTimeChoice = source.ReciveTimeChoice,
                    UpdateUserId = source.UpdateUserId,
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
                return new OrdersModelShow();
            }
        }

        #region DataUpdate

        private string Get_UserName(int source)
        {
            return new s26webDataContext().UserProfile.FirstOrDefault(w => w.Id == source).Name;
        }

        private string Get_State(int source)
        {
            return new s26webDataContext().Category.FirstOrDefault(w => w.Id == source).Name;
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

        public static string Get_Product_html(HttpSessionStateBase Session)
        {
            string output = "";

            s26webDataContext db = new s26webDataContext();
            //var vol = db.Product;
            if (s26web.Models.Method.NowBrand_Status(Session) == 1)
            {

                var vol = db.Product.Where(w => w.Id == 6);

                foreach (var i in vol)
                {
                    output += "<tr>";
                    output += "<td>" + i.Name + "</td>";                        //商品名稱
                    output += "<td><img src=\"" + i.PictureLink + "\"></td>";   //商品圖片
                    output += "<td>$" + i.Price + "/組</td>";                   //價格
                    output += "<td class=\"msg\">";
                    output += "<select name=\"1\">";                            //購買組數
                    for (int j = 1; j <= 12; j++)
                    {
                        output += "<option>" + j.ToString() + "</option>";
                    }
                    output += "</select>";

                    output += "<span class=\"year\">組</span>";
                    output += "</td>";
                    output += "<td><a class=\"btn_m\" href=\"\\Delivery\\Orders\\" + i.Id + "\" style=\"cursor: pointer\">購買</a></td>";
                    output += "</tr>";
                }
            }
            else if (s26web.Models.Method.NowBrand_Status(Session) == 2)
            {
                var vol = db.Product.Where(w => w.Id == 3 || w.Id == 7);

                foreach (var i in vol)
                {
                    output += "<tr>";
                    output += "<td>" + i.Name + "</td>";                        //商品名稱
                    output += "<td><img src=\"" + i.PictureLink + "\"></td>";   //商品圖片
                    output += "<td>$" + i.Price + "/組</td>";                   //價格
                    output += "<td class=\"msg\">";

                    output += "<select name=\"1\">";                            //購買組數
                    for (int j = 1; j <= 12; j++)
                    {
                        output += "<option>" + j.ToString() + "</option>";
                    }
                    output += "</select>";

                    output += "<span class=\"year\">組</span>";
                    output += "</td>";
                    output += "<td><a class=\"btn_m\" href=\"\\Delivery\\Orders\\" + i.Id + "\"  style=\"cursor: pointer\">購買</a></td>";
                    output += "</tr>";
                }
            }
            else
            {
                var vol = db.Product;
                foreach (var i in vol)
                {
                    output += "<tr>";
                    output += "<td>" + i.Name + "</td>";                        //商品名稱
                    output += "<td><img src=\"" + i.PictureLink + "\"></td>";   //商品圖片
                    output += "<td>$" + i.Price + "/組</td>";                   //價格
                    output += "<td class=\"msg\">";

                    output += "<select name=\"1\">";                            //購買組數
                    for (int j = 1; j <= 12; j++)
                    {
                        output += "<option>" + j.ToString() + "</option>";
                    }
                    output += "</select>";

                    output += "<span class=\"year\">組</span>";
                    output += "</td>";
                    output += "<td><a class=\"btn_m\" href=\"\\Delivery\\Orders\\" + i.Id + "\"  style=\"cursor: pointer\">購買</a></td>";
                    output += "</tr>";
                }
            }
            db.Connection.Close();
            return output;
        }

        /// <summary>
        /// Excel Export Function
        /// </summary>
        /// <returns></returns>

        public List<OrdersModelShow.Export_Order> Get_All_Export()
        {
            OrdersModelShow.Export_Order export = new OrdersModelShow.Export_Order();
            List<OrdersModelShow.Export_Order> exp = new List<OrdersModelShow.Export_Order>();
            var data = this.Get_All();
            foreach (var d in data)
            {
                export.Osid = d.Osid;
                export.MemberName = d.Vol_Name;
                export.MemberMobile = d.Vol_Mobile;
                export.Product = d.OrdersProductName;
                export.TotalPrice = d.TotalPrice + "元";
                export.Name = d.Name;
                export.Phone = d.Mobile;
                export.SharePoint = (d.SharePoint) ? "分享" : "未分享";
                export.Memo = d.Remarks;
                export.State = d.States;
                export.OrderTime = d.OrdersTime.ToLongDateString();
                exp.Add(export);
                export = new OrdersModelShow.Export_Order();
            }
            return exp;
        }

        public MemoryStream Get_ExcelData2(List<OrdersModelShow.Export_Order> data)
        {
            List<string> header = new List<string>();
            header.Add("訂單編號");
            header.Add("訂購會員");
            header.Add("手機");
            header.Add("訂購產品名稱");
            header.Add("總價格");
            header.Add("收件人");
            header.Add("收件人手機");
            header.Add("分享點數");
            header.Add("備註");
            header.Add("訂單狀態");
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
                sheet.GetRow(i + 1).CreateCell(0).SetCellValue(data[i].Osid);
                sheet.GetRow(i + 1).CreateCell(1).SetCellValue(data[i].MemberName);
                sheet.GetRow(i + 1).CreateCell(2).SetCellValue(data[i].MemberMobile);
                sheet.GetRow(i + 1).CreateCell(3).SetCellValue(data[i].Product);
                sheet.GetRow(i + 1).CreateCell(4).SetCellValue(data[i].TotalPrice);
                sheet.GetRow(i + 1).CreateCell(5).SetCellValue(data[i].Name);
                sheet.GetRow(i + 1).CreateCell(6).SetCellValue(data[i].Phone);
                sheet.GetRow(i + 1).CreateCell(7).SetCellValue(data[i].SharePoint);
                sheet.GetRow(i + 1).CreateCell(8).SetCellValue(data[i].Memo);
                sheet.GetRow(i + 1).CreateCell(9).SetCellValue(data[i].State);
                sheet.GetRow(i + 1).CreateCell(10).SetCellValue(data[i].OrderTime);
            }
            workbook.Write(ms);
            ms.Position = 0;
            ms.Flush();
            return ms;
        }
    }
}
#endregion