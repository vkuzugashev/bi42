using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bi42.Properties;
using Microsoft.AspNet.Identity;
using bi42;
using Bi42.Models;

namespace bi42.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SendMessageController : Controller
    {
        // GET: Admin/SendMessage
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        public ActionResult SendMessage(string subject, string recipients, string message)
        {
            const string MessageFromAdminInCache = "MessageFromAdminInCache";
            EmailService emailService = new EmailService();

            if (!Engine.Razor.IsTemplateCached(MessageFromAdminInCache, null))
            {
                string template = System.IO.File.ReadAllText(Settings.Default.SITEDIR + "/Views/EmailTemplates/MessageFromAdmin.cshtml");
                Engine.Razor.Compile(template, MessageFromAdminInCache);
            }

            string[] emails = recipients.Split(',');
            if (emails.Count() > 0)
            {
                using (var db = new DbModel())
                {
                    foreach (string email in emails)
                    {
                        var profile = (from o in db.Profiles where o.Email == email || o.UserID == email select o).ToList().FirstOrDefault();
                        if (profile != null)
                        {
                            //Получим текст по шаблону
                            string messageBody = Engine.Razor.Run(MessageFromAdminInCache, null, new { Name = profile.Name, Body = message, Time = DateTime.Now });

                            IdentityMessage imessage = new IdentityMessage { Destination = profile.Email ?? profile.UserID, Subject = subject, Body = messageBody };
                            emailService.Send(imessage);
                        }
                        else
                        {
                            //Получим текст по шаблону
                            string messageBody = Engine.Razor.Run(MessageFromAdminInCache, null, new { Name = "", Body = message, Time = DateTime.Now });
                            IdentityMessage imessage = new IdentityMessage { Destination = email, Subject = subject, Body = messageBody };
                            emailService.Send(imessage);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

       
    }
}