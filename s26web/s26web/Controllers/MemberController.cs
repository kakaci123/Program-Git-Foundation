using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Controllers
{
    public class MemberController : Controller
    {
        VolunteersModel data = new VolunteersModel();

        // GET: /Member/
        [NewAuthorize]
        public ActionResult Index(string error = null)
        {

            string Session_name = Method.SessionUserId_Home;
            int memberid = int.Parse(Method.Get_Session(Session, Session_name));

            var item = data.Get_One(memberid);
            if (error == null)
            {
                if (item.Mobile == null | item.Mobile == "")
                {
                    return RedirectToAction("Index", "Login", new { @error = "Incompletes" });
                }
            }
            //ViewData["isEdit"] = isEdit;
            return View(item);
        }
        [NewAuthorize]
        public ActionResult Edit()
        {
            string Session_name = Method.SessionUserId_Home;
            int memberid = int.Parse(Method.Get_Session(Session, Session_name));
            
            var item = data.Get_One(memberid);
            ViewData["Bir_year"] = item.BabyBirthday.ToString("yyyy");
            ViewData["Bir_month"] = item.BabyBirthday.ToString("MM");
            ViewData["Bir_date"] = item.BabyBirthday.ToString("dd");
            return View(item);
        }

        [HttpPost]
        [NewAuthorize]
        public ActionResult Edit(VolunteersModel.VolunteersShowEdit item, int year, int month, int date, HttpPostedFileBase Photos = null)
        {
            DateTime bir = new DateTime(year, month, date);
            item.BabyBirthday = bir;
            string Session_name = Method.SessionUserId_Home;
            int memberid = int.Parse(Method.Get_Session(Session, Session_name));
            item.Id = memberid;

            if(item.BabyBirthday > DateTime.Today)
            { item.NowBrand = 1; }

            if (data.MemberUpdate(item, Photos, "Manual/" + item.Id, Server) <= 0)
            {
                TempData["err"] = "MemberUpdate_Err";
                return RedirectToAction("Index", "Member");
            }
            return RedirectToAction("Index", "Member");
        }
        [NewAuthorize]
        public ActionResult Delivery_Exchange()
        {
            string Session_name = Method.SessionUserId_Home;
            int memberid = int.Parse(Method.Get_Session(Session, Session_name));
            OrdersModel orders = new OrdersModel();
            var item = orders.Get_Data().Where(w => w.VolunteersId == memberid).ToList();

            return View(item);
        }
        [NewAuthorize]
        public ActionResult Delivery_Order(string Osid, int Id)
        {
            string Session_name = Method.SessionUserId_Home;
            int memberid = int.Parse(Method.Get_Session(Session, Session_name));
            s26webDataContext db = new s26webDataContext();
            Osid = db.Orders.FirstOrDefault(f => f.Id == Id).Osid;
            db.Connection.Close();

            OrdersModel orders = new OrdersModel();
            var orderdetail = orders.Get_Detail(Osid);
            return View(orderdetail);
        }
    }
}
