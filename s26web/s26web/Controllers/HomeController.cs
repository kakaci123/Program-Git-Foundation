using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        VolunteersModel data = new VolunteersModel();

        public ActionResult Index()
        {
            //取得線上活動列表
            OnlineModel data = new OnlineModel();
            List<OnlineModel.OnlineModelShow> OnlineList = new List<OnlineModel.OnlineModelShow>();
            OnlineList = data.Get_Data_Front();
            ViewData["OnlineList"] = OnlineList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VolunteersModel.RegisterModel item, int year, int month, int date) //HttpSessionStateBase Session,
        {
            try
            {
                DateTime bir = new DateTime(year, month, date);
                string Session_name = Method.SessionUserId_Home;
                string account = Method.Get_Session(Session, Session_name);

                item.Id = int.Parse(account);

                if (data.Update(item, bir) <= 0)
                {
                    return RedirectToAction("Index");
                }
            }
            catch { TempData["err"] = "Login，更新失敗(請輸入完整資訊)"; }

            return RedirectToAction("Index");
        }


        public ActionResult CheckMobile(string Mobile)
        {
            s26webDataContext db = new s26webDataContext();
            var result = db.Volunteers.FirstOrDefault(f => f.Mobile == Mobile);
            db.Connection.Close();
            if (result != null)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
