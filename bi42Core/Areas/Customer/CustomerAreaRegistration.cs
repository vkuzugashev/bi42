using System.Web.Mvc;

namespace bi42.Areas.Customer
{
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                    "Customer_order",
                    "Customer/{controller}/Order{orderid}/{action}/{id}",
                    new { action = "Index", id = UrlParameter.Optional },
                    new { orderid = @"\d+" }
                );
            context.MapRoute(
                    "Customer_find",
                    "Customer/{controller}/{page}",
                    new { action = "Index", page = UrlParameter.Optional },
                    new { page = @"\d+" }
                );
            context.MapRoute(
                    "Customer_default",
                    "Customer/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
