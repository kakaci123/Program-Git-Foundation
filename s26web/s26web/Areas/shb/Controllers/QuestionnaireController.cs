using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using s26web.Areas.shb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace s26web.Areas.shb.Controllers
{
    public class QuestionnaireController : Controller
    {
        QuestionnaireModel data = new QuestionnaireModel();

        [MyAuthorize(function = "問卷管理")]
        public ActionResult Index(string keyword = "", string Level = "", DateTime? time_start = null, DateTime? time_end = null, int p = 1, int show_number = 10)
        {
            try
            {
                ViewBag.Title = "問卷管理";
                string get = s26web.Models.Method.Get_URLGet("keyword", keyword);
                data.Keyword = keyword;
                if (time_start != null && !(time_start.ToString().Equals("")))
                {
                    data.time_start = time_start.Value;
                    ViewData["time_start"] = time_start;
                    get += s26web.Models.Method.Get_URLGet("time_start", time_start.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                if (time_end != null && !(time_end.ToString().Equals("")))
                {
                    data.time_end = time_end.Value;
                    ViewData["time_end"] = time_end;
                    get += s26web.Models.Method.Get_URLGet("time_end", time_end.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                if (Level != null && !(Level.Equals("")))
                {
                    data.choice = Level;
                    ViewData["choice"] = Level;
                    get += s26web.Models.Method.Get_URLGet("choice", Level);
                }
                ViewData["p"] = p;
                ViewData["page"] = data.Get_PageIO(p, show_number);
                ViewData["keyword"] = keyword;
                ViewData["get"] = get;
                ViewData["Acitivity_List"] = Get_AcitivityList();
                return View(data.Get_Menu(p, show_number));
            }
            catch
            {
                TempData["err"] = "Member_0";
                return View();
            }
        }

        public ActionResult Create()
        {

            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }

        [MyAuthorize(function = "問卷管理")]
        public ActionResult AnswerView(int id = 0, int p = 1)
        {
            var item = data.Get_Detail(id);
            if (item == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["UserName"] = data.Get_UserName(id);
            return View(item);
        }

        private List<SelectListItem> Get_AcitivityList()
        {
            var theList = data.Get_Activity();
            List<SelectListItem> temp = new List<SelectListItem>();
            temp.Add(new SelectListItem
            {
                Selected = false,
                Text = "請選擇",
                Value = "0"
            });
            temp.AddRange(theList.Select(s => new SelectListItem
            {
                Text = s.ActivityName,
                Value = s.Id.ToString(),
                Selected = false
            }).ToList());
            return temp;
        }

        [MyAuthorize(function = "問卷管理")]
        public ActionResult Export3(string keyword = "", string Level = "", DateTime? time_start = null, DateTime? time_end = null)
        {
            if (time_start != null && !(time_start.ToString().Equals("")))
            {
                data.time_start = time_start.Value;
            }
            if (time_end != null && !(time_end.ToString().Equals("")))
            {
                data.time_end = time_end.Value;
            }
            if (Level != null && !(Level.Equals("")))
            {
                data.choice = Level;
            }
            return File(data.Get_ExcelData2(data.Get_All_Export()), "application/vnd.ms-excel", SiteOptionModel.Get_Title() + "資料匯出.xls");
        }

    }
}