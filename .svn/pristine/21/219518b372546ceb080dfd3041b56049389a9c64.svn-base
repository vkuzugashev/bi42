using bi42.Models;
using Bi42.Models;
using Common.Logging;
using Microsoft.AspNet.Identity;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bi42
{
    public class SendMailJob : IJob
    {
        private static ILog log = LogManager.GetLogger<SendMailJob>();

        private class data
        {
            public int orderid;
            public string name;
            public DateTime? publishtime;
        };

        public void Execute(IJobExecutionContext context)
        {
            SendMail();
        }

        private async void SendMail()
        {
            using (var db = new DbModel())
            {
                EmailService emailService = new EmailService();

                var profiles = from p in db.Profiles
                               where p.IsActive && p.Email != null 
                               select p;

                foreach(Profile profile in profiles.ToList())
                {
                    int newPublicMessages;
                    int newPrivateMessages;
                    int newOrderTasks;
                    string prefix;
                    string messageBody;
                    List<data> orders;
                    UserLastAccess lastAccess;

                    messageBody = "";

                    if (profile.ProfileTypeID == 2)
                    {
                        prefix = "emp_";
                        orders = (from b in db.Bids
                                  where b.UserID == profile.UserID && b.Status == 0
                                  select new data { orderid = b.OrderID, name = b.Order.Name }).ToList();
                    }
                    else
                    {
                        prefix = "cstm_";
                        orders = (from o in db.Orders
                                  where o.UserID == profile.UserID && o.Status == 0
                                  select new data { orderid = o.OrderID, name = o.Name, publishtime = o.PublishTime }).ToList();
                    }


                    foreach (var order in orders.ToList()) {

                        string message = "";
                        newPublicMessages = 0;
                        newPrivateMessages = 0;
                        newOrderTasks = 0;

                        lastAccess = db.GetLastAccess(prefix + "pub_msg_" + order.orderid, profile.UserID);
                        if (lastAccess != null) {
                            newPublicMessages = db.PublicOrderMessages.Count(m => m.PublishTime > lastAccess.LastAccess && m.OrderID == order.orderid);
                            if (newPublicMessages > lastAccess.Count)
                            {
                                message += "<b>Новые сообщения в публичной ленте - " + newPublicMessages + " шт.</b><br/>";
                                db.SetLastEmail(prefix + "pub_msg_" + order.orderid, profile.UserID, newPublicMessages);

                                //Взять все новые сообщения
                                var pubMsg = (from m in db.PublicOrderMessages
                                              where m.PublishTime > lastAccess.LastAccess && m.OrderID == order.orderid
                                              select m).ToList<PublicOrderMessage>();
                                foreach (PublicOrderMessage row in pubMsg)
                                {
                                    message+="<b>"+row.PublishTime.ToString()+", " + row.Profile.Name + ":</b><br/><b>Сообщение:</b><br/>" + row.Message+"<br/>";
                                }

                            }
                            else
                                newPublicMessages = 0;
                        }

                        lastAccess = db.GetLastAccess(prefix + "prv_msg_" + order.orderid, profile.UserID);
                        if (lastAccess != null) {
                            newPrivateMessages = db.PrivateOrderMessages.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderID == order.orderid);
                            if (newPrivateMessages > lastAccess.Count)
                            {
                                message += "<b>Новые сообщения в приватной ленте - " + newPrivateMessages + " шт.</b><br/>";
                                db.SetLastEmail(prefix + "prv_msg_" + order.orderid, profile.UserID, newPrivateMessages);

                                //Взять все новые сообщения
                                var privMsg = (from m in db.PrivateOrderMessages
                                              where m.PublishTime > lastAccess.LastAccess && m.OrderID == order.orderid
                                              select m).ToList<PrivateOrderMessage>();
                                foreach (PrivateOrderMessage row in privMsg)
                                {
                                    message += "<b>"+row.PublishTime.ToString() + ", " + row.FromProfile.Name + ":</b><br/><b>Сообщение:</b><br/>" + row.Message + "<br/>";
                                }
                            }
                            else
                                newPrivateMessages = 0;
                        }

                        lastAccess = db.GetLastAccess(prefix + "order_task_" + order.orderid, profile.UserID);
                        if (lastAccess != null) {
                            newOrderTasks = db.OrderTasks.Count(x => x.UpdateTime > lastAccess.LastAccess && x.OrderID == order.orderid);
                            if (newOrderTasks > lastAccess.Count)
                            {
                                message += "<b>Новые задачи - " + newOrderTasks + " шт.</b><br/>";
                                db.SetLastEmail(prefix + "order_task_" + order.orderid, profile.UserID, newOrderTasks);

                                //Взять все новые сообщения
                                var newTasks = (from m in db.OrderTasks
                                              where m.UpdateTime > lastAccess.LastAccess && m.OrderID == order.orderid
                                              select m).ToList<OrderTask>();
                                foreach (OrderTask row in newTasks)
                                {
                                    message += "<b>"+row.UpdateTime.ToString() + ", " + row.Name + ":</b><br/><b>Описание:</b><br/>" + row.Description + "<br/>";
                                }

                            }
                            else
                                newOrderTasks = 0;
                        }

                        if (newPublicMessages > 0 || newPrivateMessages > 0 || newOrderTasks > 0)
                        {
                            messageBody += string.Format("<br/><b>Заказ №{0}, {1}</b><br/><b>Сообщение:</b>{2}<br/></br/>", order.orderid, order.name, message);
                        }
                    }

                    //Сформируем список новых заказов для пользователя
                    if (profile.ProfileTypeID == 2)
                    {
                        lastAccess = db.GetLastAccess(prefix + "order_new", profile.UserID);
                        if (lastAccess != null)
                        {
                            int newOrders = db.Orders.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderAreaID == profile.OrderAreaID);
                            if (newOrders > lastAccess.Count)
                            {
                                messageBody += "<br/><br/><b>Новые заказы - " + newOrders + " шт.</b><br/>";
                                db.SetLastEmail(prefix + "order_new", profile.UserID, newOrders);

                                //Взять все новые сообщения
                                var orderLists = (from o in db.Orders
                                                  where o.PublishTime > lastAccess.LastAccess && o.OrderAreaID == profile.OrderAreaID
                                                  select new { OrderID = o.OrderID, PublishTime = o.PublishTime, StartTime = o.StartTime, Name = o.Name, BudgetLevel= (o.BudgetLevel != null ? o.BudgetLevel.Name : ""), Budget=o.Budget, Description = o.Description }).ToList();
                                foreach (var row in orderLists)
                                {
                                    messageBody += string.Format("<br/><a href=\"http://www.bi42.ru/Employee/FindOrder?OrderID={0}\"><b>Заказ №{0}</b></a> от {1}, старт {2}, бюджет {3} {4}<br/><b>Тема: {5}</b><br/><b>Описание:</b><br/>{6}<br/>", row.OrderID, row.PublishTime, ((DateTime)row.StartTime).ToString("dd.MM.yyyy"), row.BudgetLevel, row.Budget, row.Name, row.Description);
                                }
                            }
                        }
                        else
                            db.SetLastEmail(prefix + "order_new", profile.UserID, 0);
                    }


                    //Сформируем письмо
                    if (!"".Equals(messageBody))
                    {
                        messageBody = string.Format("Уважаемый пользователь, <b>{0}</b><br/><br/>Есть новые события на сайте <a href=\"http://www.bi42.ru/\">WWW.BI42.RU</a>:<br/>{1}<br/><br/>С уважением администрация сайта<br/>Пожалуйста не отвечайте на это письмо.<br/>", profile.Name, messageBody);
                        IdentityMessage imessage = new IdentityMessage { Destination = profile.Email, Subject = "Есть новые события на сайте www.bi42.ru", Body = messageBody };
                        await emailService.SendAsync(imessage);
                        log.DebugFormat("Отправлено сообщение получателю {0}: \r\nsubject={1}\r\n{2}", imessage.Destination, imessage.Subject, imessage.Body);
                    }

                    
                }
            }
          
        }
    }
}