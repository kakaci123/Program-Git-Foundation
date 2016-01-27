using s26web.Areas.shb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace s26web.Areas.shb.Controllers
{
    public class eCRMController : Controller
    {
        CRMModel data = new CRMModel();
        //
        // GET: /shb/eCRM/

        [MyAuthorize(function = "e-CRM管理")]
        public ActionResult Index()
        {
            var item = data.Get_Data();
            return View(item);
        }

        [MyAuthorize(function = "e-CRM管理")]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Index(CRMModel.CRMMemberInfo item) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    data.Update(item.UserNamelist,item.FBUrllist);
                }
                else
                {
                    TempData["err"] = "eCRM_10";
                }
            }
            catch { TempData["err"] = "eCRM_11"; }
            return RedirectToAction("Index");
        }
    }
}
