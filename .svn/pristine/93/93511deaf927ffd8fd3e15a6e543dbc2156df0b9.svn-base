using System.Web;
using System.Web.Optimization;

namespace bi42
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js", "~/Scripts/jquery-ui-i18n.js" ));


            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css","~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/UI/themes/base").Include(
                      "~/Content/themes/base/all.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/PagedList/css").Include(
                      "~/Content/PagedList.css"));

            // Присвойте EnableOptimizations значение false для отладки. Дополнительные сведения
            // см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
            //BundleTable.EnableOptimizations = true;
        }
    }
}
