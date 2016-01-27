using System.Web.Mvc;

namespace s26web.Areas.shb
{
    public class shbAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "shb";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "shb/{controller}/{action}/{id}",
                new { action = "Index", controller = "S26", id = UrlParameter.Optional },
                namespaces: new string[] { "s26web.Areas.shb.Controllers" }
            );
        }
    }
}
