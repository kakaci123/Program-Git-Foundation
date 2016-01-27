using s26web.Areas.shb.Models;
using s26web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Security;


namespace s26web.Models
{
    public class DeliveryModel
    {
        public class OrdersModel
        {
            public int Order_Quantity { get; set; }
            public int Order_Quantity_3 { get; set; }
            public int Order_Quantity_6 { get; set; }
            public int Order_Quantity_7 { get; set; }
            public int Orders_ID { get; set; }
            public int ProductId { get; set; }
            [DisplayName("會員編號")]
            public int VolunteersId { get; set; }
            [DisplayName("收件人")]
            public string Name { get; set; }
            [DisplayName("收件人手機")]
            public string Mobile { get; set; }
            [DisplayName("收件人地址")]
            public string Address { get; set; }
            [DisplayName("收件人縣市")]
            public int CityId { get; set; }
            [DisplayName("收件人區域")]
            public int AreaId { get; set; }
            [DisplayName("備註")]
            public string Remarks { get; set; }
              [DisplayName("總額")]
            public int TotalPrice { get; set; }
              [DisplayName("發票格式")]
            public int InvoiceCategory { get; set; }
            [DisplayName("公司抬頭")]
            public string InvoiceName { get; set; }
            [DisplayName("統一編號")]
            public string InvoiceNo { get; set; }
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
                    output += "<td><a class=\"btn_m\" href=\"\\Delivery\\Orders\\" + i.Id + "\">購買</a></td>";
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

                    output += "<select name=\"Order_Quantity_" + i.Id + "\">";                            //購買組數
                    for (int j = 1; j <= 12; j++)
                    {
                        output += "<option>" + j.ToString() + "</option>";
                    }
                    output += "</select>";

                    output += "<span class=\"year\">組</span>";
                    output += "</td>";
                    output += "<td><input type=\"submit\" class=\"btn_m\" value=\"購買\" onclick=\"Set_ID(" + i.Id + ");\" /></td>";
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
                    output += "<td><a class=\"btn_m\" href=\"\\Delivery\\Orders\\" + i.Id + "\">購買</a></td>";
                    output += "</tr>";
                }
            }

            db.Connection.Close();


            return output;
        }
        public static string Set_Orders_Detail(DeliveryModel.OrdersModel item, HttpSessionStateBase Session)
        {
            return "";
        }

        #region Insert
        public int Insert(DeliveryModel.OrdersModel item)
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                Orders orders = new Orders
                    {
                        VolunteersId = item.VolunteersId,
                        Osid = DateTime.UtcNow.AddHours(8).ToString("yyyyMMddHHmm"),
                        ProductId = item.ProductId,
                        Order_Quantity = item.Order_Quantity,
                        TotalPrice = item.TotalPrice,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        CityId = item.CityId,
                        AreaId = item.AreaId,
                        Address = item.Address,
                        SharePoint=  true,
                        Remarks = item.Remarks,
                        OrdersStates = 22,
                        OrdersTime = DateTime.UtcNow,
                        ReciveTimeChoice = 15,
                        OrdersFrom = 0,
                    };

                db.Orders.InsertOnSubmit(orders);
                db.SubmitChanges();

                Invoice invoice = new Invoice
                {
                    OrdersId = db.Orders.OrderByDescending(o => o.Id).FirstOrDefault(f => f.Id != null).Id,
                    InvoiceCategory = item.InvoiceCategory
                };

                if (item.InvoiceCategory == db.Category.FirstOrDefault(w => w.Fun_Id == 5 && w.Memo.Equals("Invoice")).Id)
                {
                    invoice.InvoiceName = item.InvoiceName;
                    invoice.InvoiceNo = item.InvoiceNo;
                }

                db.Invoice.InsertOnSubmit(invoice);
                db.SubmitChanges();
                db.Connection.Close();
                return orders.Id;

            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region update
        public int Update_BabyBirthday(string account, DateTime New_BabyBirthday)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var data =   db.Volunteers.FirstOrDefault(w => w.Id == int.Parse(account));
                if (data != null)
                {
                    data.BabyBirthday = New_BabyBirthday;
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
    }
}