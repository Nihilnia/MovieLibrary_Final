using System.Web.Mvc;

namespace Overture.Areas.JQMFGrid
{
    public class JQMFGridAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JQMFGrid";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JQMFGrid_default",
                "JQMFGrid/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}