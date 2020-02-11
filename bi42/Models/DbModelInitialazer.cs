using System.Collections.Generic;
using System.Data.Entity;

namespace Bi42.Models
{
    public class DbModelInitializer : DropCreateDatabaseIfModelChanges<DbModel>
    {
        protected override void Seed(DbModel context)
        {
            /*Заполним справочники*/
            var projectAreas = new List<OrderArea>
            {
                new OrderArea{ Name="Грузоперевозки", Description="Грузовые перевозки для физических и юридических лиц"},
                new OrderArea{ Name="Строительные работы", Description="Строительные работы для физических и юридических лиц"},
            };
            projectAreas.ForEach(s => context.OrderAreas.Add(s));
            context.SaveChanges();        

            var budgetLevels = new List<BudgetLevel>
            {
                new BudgetLevel { BudgetLevelID=1, Name="< 5000"},
                new BudgetLevel { BudgetLevelID=2, Name="5000-10000"},
                new BudgetLevel { BudgetLevelID=3, Name="10000-50000"},
                new BudgetLevel { BudgetLevelID=4, Name="> 50000"},
            };
            budgetLevels.ForEach(s => context.BudgetLevels.Add(s));
            context.SaveChanges();        

            var projectStatuses = new List<OrderStatus>
            {
                new OrderStatus { OrderStatusID=1, Name="Черновик"},
                new OrderStatus { OrderStatusID=2, Name="Тендер"},
                new OrderStatus { OrderStatusID=3, Name="Заключён контракт"},
                new OrderStatus { OrderStatusID=4, Name="Заключёны контракты"},
                new OrderStatus { OrderStatusID=5, Name="Отменён"},
                new OrderStatus { OrderStatusID=6, Name="Завершён"},
            };
            projectStatuses.ForEach(s => context.OrderStatuses.Add(s));
            context.SaveChanges();

            var projectWatchStatuses = new List<OrderWatchStatus>
            {
                new OrderWatchStatus { OrderWatchStatusID=1, Name="Нет ставок"},
                new OrderWatchStatus { OrderWatchStatusID=2, Name="Ставка"},
            };
            projectWatchStatuses.ForEach(s => context.OrderWatchStatuses.Add(s));
            context.SaveChanges();

            var bidStatuses = new List<BidStatus>
            {
                new BidStatus { BidStatusID=1, Name="Ставка"},
                new BidStatus { BidStatusID=2, Name="Ставка принята заказчиком"},
                new BidStatus { BidStatusID=3, Name="Заключён контракт"},
                new BidStatus { BidStatusID=4, Name="Выбран другой"},
                new BidStatus { BidStatusID=5, Name="Отказ заказчика"},
                new BidStatus { BidStatusID=6, Name="Отказ подрядчика"},
            };
            bidStatuses.ForEach(s => context.BidStatuses.Add(s));
            context.SaveChanges();

            var taskStatuses = new List<TaskStatus>
            {
                new TaskStatus { TaskStatusID=1, Name="На оценку"},
                new TaskStatus { TaskStatusID=2, Name="Оценено"},
                new TaskStatus { TaskStatusID=3, Name="В работе"},
                new TaskStatus { TaskStatusID=4, Name="Тестирование"},
                new TaskStatus { TaskStatusID=5, Name="Устранение замечаний"},
                new TaskStatus { TaskStatusID=6, Name="Выполнено"},
                new TaskStatus { TaskStatusID=7, Name="Отменена"},
            };
            taskStatuses.ForEach(s => context.TaskStatuses.Add(s));
            context.SaveChanges();

            /*Тестовые данные*/
            var profiles = new List<Profile>(){
                new Profile { Name = "Заказчик", IsActive = true, ProfileTypeID=1, OrderAreaID=1, UserID="penart@nm.ru" },
                new Profile { Name = "Подрядчик", IsActive = true, ProfileTypeID=2, OrderAreaID=1, UserID="penart@nm.ru" },
            };
            profiles.ForEach(s => context.Profiles.Add(s));
            context.SaveChanges();
        
            var projects = new List<Order>(){
                new Order { Name = "Тестовый проект", ProfileID=1, OrderStatusID=2, BudgetLevelID=1, Status=0, UserID="penart@nm.ru" },
            };
            projects.ForEach(s => context.Orders.Add(s));
            context.SaveChanges();


            var shopAreas = new List<ShopArea>
            {
                new ShopArea{ Name="Подарки, сувениры, свадебные украшения", Description="Подарки, сувениры, открытки, шарики и т.п"},
                new ShopArea{ Name="Свадебные принадлежности", Description="Свадебные платья"},
            };
            shopAreas.ForEach(s => context.ShopAreas.Add(s));
            context.SaveChanges();        
        
        
        
        }
     }

}