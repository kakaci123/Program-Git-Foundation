using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;

namespace s26web.Controllers
{
    public class Online2Controller : Controller
    {

        public ActionResult Index()
        {
            OnlineModel data = new OnlineModel();
            ViewBag.Title = "線上活動管理";
            return View(data.Get_Data_Front());
        }
        public ActionResult Count(int Code_Error = 0, string Code = null)
        {
            ViewData["Code_Error"] = Code_Error;
            ViewData["Code"] = Code;
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Count(SalesCodeModel.SalesCodeModelShow item)
        {
            SalesCodeModel data = new SalesCodeModel();

            int Num = data.Check_Code_Front(item);
            //促銷碼已經兌換過 
            if (Num == -1)
            {
                return RedirectToAction("Count", new { Code_Error = -1, Code = item.Code });
            }
            //促銷碼輸入錯誤
            else if (Num == -2)
            {
                return RedirectToAction("Count", new { Code_Error = -2, Code = item.Code });
            }
            //兌換成功
            else
            {
                s26webDataContext db = new s26webDataContext();
                int Point = db.SalesCode.FirstOrDefault(w => w.Code == item.Code).SalesPromotionPoint;
                return RedirectToAction("Premiums_complete", new { Point = Point });
            }
        }
        public ActionResult Premiums_article()
        {
            return View();
        }
        public ActionResult Premiums_complete(int Point = 0)
        {
            ViewData["Point"] = Point;
            return View();
        }
        public ActionResult ECRM_complete()
        {
            return View();
        }

        public ActionResult Q_vouchers()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Q_vouchers(QuestionnaireModel2.QuestionnaireModelShow item)
        {
            QuestionnaireModel2 data = new QuestionnaireModel2();
            string Session_name = Method.SessionUserId_Home;
            string account = Method.Get_Session(Session, Session_name);
            item.VolunteersId = int.Parse(account);
            try
            {

                //第二題
                if (item.Q2 != "網站")
                {
                    item.Q2_Note = null;
                }
                else
                {
                    item.Q2 += " " + item.Q2_Note;
                }

                //第三題
                if (item.Q3 != "有。品牌")
                {
                    item.Q3_Note = null;
                }
                else
                {
                    item.Q3 += " : " + item.Q3_Note;
                }

                //第七題
                if (item.Q7 != "不會。原因")
                {
                    item.Q7_Note = null;
                }
                else
                {
                    item.Q7 += " : " + item.Q7_Note;
                }

                //第八題
                if (item.Q8 != "其他")
                {
                    item.Q8_Note = null;
                }
                else
                {
                    item.Q8 += " : " + item.Q8_Note;
                }

                if (data.Insert(item) <= 0)
                {
                    TempData["err"] = "SalesCode_0";
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                TempData["err"] = "SalesCode_1";
            }
            data.AddPoint(item);
            return RedirectToAction("ECRM_complete");
        }
    }
}
