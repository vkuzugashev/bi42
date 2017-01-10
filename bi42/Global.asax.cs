using Bi42.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace bi42
{
    public class MvcApplication : System.Web.HttpApplication
    {
        IScheduler scheduler = null;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
    
            //Запуск планировщика заданий
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler scheduler = schedFact.GetScheduler();
            scheduler.Start();
            IJobDetail jobDetail = JobBuilder.Create<SendMailJob>()
                    .WithIdentity("SendMail", "Alert")
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                               .WithIdentity("trigger1", "Alert")
                               .StartNow()
                               //.WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(new TimeOfDay(0,0)))
                               .WithSimpleSchedule(x=>x.WithIntervalInSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["jobSchedule.interval"])).RepeatForever())
                               .Build();

            scheduler.ScheduleJob(jobDetail, trigger);         
        }

        protected void Application_Stop()
        {
            scheduler.Shutdown(true);
        }


            //protected void Application_EndRequest()
            //{
            //    //Сохранять время последнего запроса к серверу
            //    if (User.Identity.IsAuthenticated)
            //    {
            //        using (var db = new DbModel())
            //        {
            //            var row = db.UserLastAcess.Find(User.Identity.Name);
            //            if (row == null)
            //                db.UserLastAcess.Add(new UserLastAccess { UserID = User.Identity.Name, LastAccess = DateTime.Now });
            //            else
            //                row.LastAccess = DateTime.Now;
            //            db.SaveChanges();
            //        }
            //    }
            //}
        }
}
