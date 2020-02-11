namespace bi42.Migrations
{
    using Bi42.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Bi42.Models.DbModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Bi42.Models.DbModel context)
        {
            /*Заполним справочники*/
            context.OrderAreas.AddOrUpdate(x => x.OrderAreaID, new OrderArea[] {
                new OrderArea { OrderAreaID=1, Name = "Разработка ПО и ИТ консалтинг", Description = "Разработка программного обеспечения и консультирование в этой области" },
                new OrderArea { OrderAreaID=2, Name = "Строительные и монтажные работы", Description = "Строительные и монтажные работы всех видов" },
                new OrderArea { OrderAreaID=3, Name = "Грузоперевозки", Description = "Грузовые перевозки для физических и юридических лиц" },
                new OrderArea { OrderAreaID=4, Name = "Все для праздника", Description = "Проведение мероприятий, украшение помещений" },
            });
            context.SaveChanges();

            context.BudgetLevels.AddOrUpdate(x => x.Name, new BudgetLevel[] {
                new BudgetLevel { Name = "< 5000" },
                new BudgetLevel { Name = "5000-10000" },
                new BudgetLevel { Name = "10000-50000" },
                new BudgetLevel { Name = "> 50000" }
            });
            context.SaveChanges();

            context.OrderStatuses.AddOrUpdate(x => x.Name, new OrderStatus[] {
                new OrderStatus { Name = "Черновик" },
                new OrderStatus { Name = "Тендер" },
                new OrderStatus { Name = "Заключён контракт" },
                new OrderStatus { Name = "Заключёны контракты" },
                new OrderStatus { Name = "Отменён" },
                new OrderStatus { Name = "Завершён" }
            });
            context.SaveChanges();

            context.OrderWatchStatuses.AddOrUpdate(x => x.Name, new OrderWatchStatus[] {
                new OrderWatchStatus { Name = "Нет заявок" },
                new OrderWatchStatus { Name="Заявка"}
            });
            context.SaveChanges();

            context.BidStatuses.AddOrUpdate(x => x.Name, new BidStatus[] { 
                new BidStatus { Name = "Заявка" },
                new BidStatus { Name = "Заявка принята заказчиком" },
                new BidStatus { Name = "Заключён контракт" },
                new BidStatus { Name = "Выбран другой" },
                new BidStatus { Name = "Отказ заказчика" },
                new BidStatus { Name = "Отказ подрядчика" }
            });
            context.SaveChanges();

            /*Тестовые данные*/
            context.Profiles.AddOrUpdate(x => x.Name, new Profile[] {
                new Profile { Name = "Заказчик", IsActive = true, ProfileTypeID = 1, OrderAreaID = 1, UserID = "penart@nm.ru" },
                new Profile { Name = "Подрядчик", IsActive = true, ProfileTypeID = 2, OrderAreaID = 1, UserID = "penart@nm.ru" }
            });
            context.SaveChanges();

            context.Orders.AddOrUpdate(x => x.Name, new Order[] {
                new Order { Name = "Тестовый заказ", ProfileID = 1, OrderStatusID = 2, BudgetLevelID = 1, Status = 0, UserID = "penart@nm.ru" }
            });
            context.SaveChanges();


            context.ShopAreas.AddOrUpdate(x => x.Name, new ShopArea[] {
                new ShopArea { Name = "Электроника", Description = "Подарки, сувениры, открытки, шарики и т.п" },
                new ShopArea { Name = "Свадебные принадлежности", Description = "Свадебные платья" }
            });
            context.SaveChanges();

        }
    }
}
