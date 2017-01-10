using System.Web.Mvc;

namespace bi42.Areas.Employer
{
    public class EmployerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Employee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                   "Employee_order",
                   "Employee/{controller}/Order{orderid}/{action}/{id}",
                   new { action = "Index", id = UrlParameter.Optional },
                   new { orderid = @"\d+" }
               );
            context.MapRoute(
                    "Employee_default",
                    "Employee/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                    //new { action = new NotStartsWith(@"Order") }
                );

        }
    }
}
