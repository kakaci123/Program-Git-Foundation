using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using s26web.Models;
using s26web.Areas.shb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace s26web.Areas.shb.Controllers
{
    public class SalesCodeController : Controller
    {
        SalesCodeModel data = new SalesCodeModel();

        [MyAuthorize(function = "促銷碼管理")]
        public ActionResult Index(int p = 1, int show_number = 10, string keyword = "", DateTime? volunteers_time_start = null, DateTime? volunteers_time_end = null, DateTime? exchange_time_start = null, DateTime? exchange_time_end = null, int Search_SalesPromotionId = 0, int Search_SalesPromotionDeadline = 1, int Search_ExchangeStatus = 1)
        {
            try
            {
                string get = s26web.Models.Method.Get_URLGet("keyword", keyword);
                data.Keyword = keyword;
                //加入會員起訖
                if (volunteers_time_start != null)
                {
                    get += s26web.Models.Method.Get_URLGet("volunteers_time_start", volunteers_time_start.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    ViewData["volunteers_time_start"] = volunteers_time_start;
                    data.volunteers_time_start = volunteers_time_start.Value;
                }
                if (volunteers_time_end != null)
                {
                    get += s26web.Models.Method.Get_URLGet("volunteers_time_end", volunteers_time_end.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    ViewData["volunteers_time_end"] = volunteers_time_end;
                    data.volunteers_time_end = volunteers_time_end.Value;
                }
                //驗證時間起訖
                if (exchange_time_start != null)
                {
                    get += s26web.Models.Method.Get_URLGet("exchange_time_start", exchange_time_start.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    ViewData["exchange_time_start"] = exchange_time_start;
                    data.exchange_time_start = exchange_time_start.Value;
                }
                if (exchange_time_end != null)
                {
                    get += s26web.Models.Method.Get_URLGet("exchange_time_end", exchange_time_end.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    ViewData["exchange_time_end"] = exchange_time_end;
                    data.exchange_time_end = exchange_time_end.Value;
                }
                //促銷活動
                if (Search_SalesPromotionId != 0)
                {
                    get += s26web.Models.Method.Get_URLGet("Search_SalesPromotionId", Search_SalesPromotionId.ToString(""));
                    ViewData["Search_SalesPromotionId"] = Search_SalesPromotionId;
                    data.Search_SalesPromotionId = Search_SalesPromotionId;
                }
                //序號到期
                if (Search_SalesPromotionDeadline != 0)
                {
                    get += s26web.Models.Method.Get_URLGet("Search_SalesPromotionDeadline", Search_SalesPromotionDeadline.ToString(""));
                    ViewData["Search_SalesPromotionDeadline"] = Search_SalesPromotionDeadline;
                    data.Search_SalesPromotionDeadline = Search_SalesPromotionDeadline;
                }
                //兌換狀態
                if (Search_ExchangeStatus != 0)
                {
                    get += s26web.Models.Method.Get_URLGet("Search_ExchangeStatus", Search_ExchangeStatus.ToString(""));
                    ViewData["Search_ExchangeStatus"] = Search_ExchangeStatus;
                    data.Search_ExchangeStatus = Search_ExchangeStatus;
                }
                ViewBag.Title = "促銷碼管理";
                ViewData["p"] = p;
                ViewData["page"] = data.Get_Page(p, show_number);
                return View(data.Get_Data());
            }
            catch
            {
                TempData["err"] = "SalesCode_0";
                return View("~/Areas/shb/Views/SalesCode/Index.cshtml");
            }
        }

        [MyAuthorize(function = "促銷碼管理")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Create(SalesCodeModel.SalesCodeModelShow item)
        {
            try
            {
                if (data.Insert(item) <= 0)
                {
                    TempData["err"] = "SalesCode_0";
                    return RedirectToAction("Index");
                }
            }
            catch { TempData["err"] = "SalesCode_1"; }
            return RedirectToAction("Index");
        }

        [MyAuthorize(function = "促銷碼管理")]
        public ActionResult Edit(int id = 0, int p = 1)
        {
            ViewData["p"] = p;
            var item = data.Get_One(id);

            if (item == null)
            {
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [HttpPost]
        [MyAuthorize(function = "促銷碼管理")]
        public ActionResult Edit(SalesCodeModel.SalesCodeModelShow item, int p = 1)
        {
            if (data.Update(item) <= 0)
            {
                TempData["err"] = "SalesCode_5";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", new { p = p });
        }

        [MyAuthorize(function = "促銷碼管理")]
        public ActionResult Delete(int Id, int p = 1)
        {
            data.Delete(Id);

            return RedirectToAction("Index", new { p = p });
        }
        [MyAuthorize(function = "促銷碼管理")]
        public ActionResult Export_main(string keyword = "", DateTime? volunteers_time_start = null, DateTime? volunteers_time_end = null, DateTime? exchange_time_start = null, DateTime? exchange_time_end = null, int Search_SalesPromotionId = 0, int Search_SalesPromotionDeadline = 1, int Search_ExchangeStatus = 1)
        {
            //---------------匯出條件搜索---------------//

            data.Keyword = keyword;

            //加入會員起訖
            if (volunteers_time_start != null)
            {
                data.volunteers_time_start = volunteers_time_start.Value;
            }
            if (volunteers_time_end != null)
            {
                data.volunteers_time_end = volunteers_time_end.Value;
            }
            //驗證時間起訖
            if (exchange_time_start != null)
            {
                data.exchange_time_start = exchange_time_start.Value;
            }
            if (exchange_time_end != null)
            {
                data.exchange_time_end = exchange_time_end.Value;
            }
            //促銷活動
            if (Search_SalesPromotionId != 0)
            {
                data.Search_SalesPromotionId = Search_SalesPromotionId;
            }
            //序號到期
            if (Search_SalesPromotionDeadline != 0)
            {
                data.Search_SalesPromotionDeadline = Search_SalesPromotionDeadline;
            }
            //兌換狀態
            if (Search_ExchangeStatus != 0)
            {
                data.Search_ExchangeStatus = Search_ExchangeStatus;
            }

            //---------------匯出條件搜索end---------------//
            return File(data.Get_ExcelData_main(SalesCodeModel.Get_Export(data)), "application/vnd.ms-excel", "促銷碼管理資料報表.xls");
        }

        private string fileSavedPath = "/upload";

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, int SalesPromotionId = 0)
        {
            JObject jo = new JObject();
            string result = string.Empty;

            if (file == null)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳檔案!");
                jo.Add("SalesPromotionId", SalesPromotionId);
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }
            if (file.ContentLength <= 0)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳正確的檔案.");
                jo.Add("SalesPromotionId", SalesPromotionId);
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            string fileExtName = Path.GetExtension(file.FileName).ToLower();

            if (!fileExtName.Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳 .csv格式的檔案");
                jo.Add("SalesPromotionId", SalesPromotionId);
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            try
            {
                var uploadResult = this.FileUploadHandler(file);

                jo.Add("Result", !string.IsNullOrWhiteSpace(uploadResult));
                jo.Add("Msg", !string.IsNullOrWhiteSpace(uploadResult) ? uploadResult : "");
                jo.Add("SalesPromotionId", SalesPromotionId);
                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                jo.Add("Result", false);
                jo.Add("Msg", ex.Message);
                result = JsonConvert.SerializeObject(jo);
            }
            return Content(result, "application/json");
        }

        /// <summary>
        /// Files the upload handler.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">file;上傳失敗：沒有檔案！</exception>
        /// <exception cref="System.InvalidOperationException">上傳失敗：檔案沒有內容！</exception>
        private string FileUploadHandler(HttpPostedFileBase file)
        {
            string result;

            if (file == null)
            {
                throw new ArgumentNullException("file", "上傳失敗：沒有檔案！");
            }
            if (file.ContentLength <= 0)
            {
                throw new InvalidOperationException("上傳失敗：檔案沒有內容！");
            }

            try
            {
                string virtualBaseFilePath = Url.Content(fileSavedPath);
                string filePath = HttpContext.Server.MapPath(virtualBaseFilePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string newFileName = string.Concat(
                    DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Path.GetExtension(file.FileName).ToLower());

                string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath), newFileName);
                file.SaveAs(fullFilePath);

                result = newFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        [HttpPost]
        public ActionResult Import(string savedFileName, int SalesPromotionId = 0)
        {
            var jo = new JObject();
            string result;
            try
            {
                var fileName = string.Concat(Server.MapPath(fileSavedPath), "/", savedFileName);

                var importSalesCodes = new List<SalesCodeModel.ImportClass>();

                var helper = new SalesCodeModel();
                var checkResult = helper.CheckImportData(fileName, importSalesCodes);

                jo.Add("Result", checkResult.Success);
                jo.Add("Msg", checkResult.Success ? string.Empty : checkResult.ErrorMessage);
                jo.Add("RowCount", checkResult.RowCount);
                if (checkResult.Success)
                {
                    //儲存匯入的資料
                    helper.SaveImportData(importSalesCodes, SalesPromotionId, fileName);
                }
                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Content(result, "application/json");
        }
    }
}
