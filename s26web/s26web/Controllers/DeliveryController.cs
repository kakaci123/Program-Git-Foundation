using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;



namespace s26web.Controllers
{
    public class DeliveryController : Controller
    {
        //
        // GET: /Delivery/

        [NewAuthorize]
        public ActionResult Index()//HttpSessionStateBase Session
        {
            s26webDataContext db = new s26webDataContext();
            string Session_name = Method.SessionUserId_Home;
            string account = Method.Get_Session(Session, Session_name);

            var vol = db.Volunteers.FirstOrDefault(w => w.Id == int.Parse(account));
            ViewData["Bir_year"] = vol.BabyBirthday.ToString("yyyy");
            ViewData["Bir_month"] = vol.BabyBirthday.ToString("MM");
            ViewData["Bir_date"] = vol.BabyBirthday.ToString("dd");

            return View();
        }

        [HttpPost]
        [NewAuthorize]
        public ActionResult Index(int year, int month, int date)
        {
            s26webDataContext db = new s26webDataContext();
            DeliveryModel data = new DeliveryModel();

            string Session_name = Method.SessionUserId_Home;
            string account = Method.Get_Session(Session, Session_name);
            DateTime New_BabyBirthday = new DateTime(year, month, date);
            //string New_BabyBirthday = year+"/"+month+"/"+date;
            DateTime vol_BabyBirthday = db.Volunteers.FirstOrDefault(w => w.Id == int.Parse(account)).BabyBirthday;

            if (vol_BabyBirthday != New_BabyBirthday)
            {
                data.Update_BabyBirthday(account, New_BabyBirthday);
            }
            return RedirectToAction("article");
        }

        [NewAuthorize]
        public ActionResult article()
        {
            ViewBag.Get_Product = DeliveryModel.Get_Product_html(Session);
            return View();
        }

        [HttpPost]
        [NewAuthorize]
        public ActionResult article(DeliveryModel.OrdersModel item)
        {
            Session.Add("Order_Id", item.Orders_ID);

            if (item.Orders_ID == 3)
            {
                Session.Add("Order_Quantity", item.Order_Quantity_3);
            }
            else if (item.Orders_ID == 6)
            {
                Session.Add("Order_Quantity", item.Order_Quantity_6);
            }
            else if (item.Orders_ID == 7)
            {
                Session.Add("Order_Quantity", item.Order_Quantity_7);
            }

            return RedirectToAction("Orders", "Delivery");
        }

        [NewAuthorize]
        public ActionResult Orders()
        {
            ProductModel data = new ProductModel();
            var item = data.Get_One(int.Parse(Session["Order_Id"].ToString()));
            if (item == null)
            {
                return RedirectToAction("Index");
            }
            Session["Product_Name"] = item.Name;
            Session["Total_Price"] = item.Price * int.Parse(Session["Order_Quantity"].ToString());
            Session["Product_Id"] = int.Parse(Session["Order_Id"].ToString());

            return View(item);
        }

        [NewAuthorize]
        public ActionResult Recipient()
        {
            Session["Order_Quantity"] = int.Parse(Session["Order_Quantity"].ToString());
            Session["Total_Price"] = int.Parse(Session["Total_Price"].ToString());
            Session["Product_Id"] = int.Parse(Session["Order_Id"].ToString());

            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Recipient(DeliveryModel.OrdersModel item)
        {
            DeliveryModel data = new DeliveryModel();
            string Session_name = Method.SessionUserId_Home;
            string account = Method.Get_Session(Session, Session_name);
            item.VolunteersId = int.Parse(account);
            try
            {
                if (data.Insert(item) <= 0)
                {
                    TempData["err"] = "Recipient_0";
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                TempData["err"] = "Recipient_0";
            }
            return RedirectToAction("Recipient_complete");
        }

        [NewAuthorize]
        public ActionResult Premiums_complete()
        {
            return View();
        }
        [NewAuthorize]
        public ActionResult Recipient_complete()
        {
            return View();
        }
    }
}
