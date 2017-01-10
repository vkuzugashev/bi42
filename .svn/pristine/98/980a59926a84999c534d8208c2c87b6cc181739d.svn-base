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
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Bi42.Models.DbModel context)
        {
            /*Заполним справочники*/
            context.OrderAreas.AddOrUpdate(x => x.OrderAreaID, new OrderArea[] {
                new OrderArea { OrderAreaID=1, Name = "Услуги разработки ПО и ИТ консалтинг", Description = "Разработка программного обеспечения и консультирование в этой области" },
                new OrderArea { OrderAreaID=2, Name = "Услуги ремонта электронной техники", Description = "Ремонт радио, теле, видео, и прочей электронной техники" },
                new OrderArea { OrderAreaID=3, Name = "Услуги ремонта бытовой техники", Description = "Ремонт стиральных машин, холодильников, и прочей бытовой техники" },
                new OrderArea { OrderAreaID=4, Name = "Услуги ремонта квартир, домов", Description = "Ремонт квартир, домов" },
                new OrderArea { OrderAreaID=5, Name = "Услуги ремонта и обслуживания автотранспорта", Description = "Ремонт автомашин и прочего транспорта" },
                new OrderArea { OrderAreaID=6, Name = "Услуги грузоперевозчиков", Description = "Грузовые перевозки для физических и юридических лиц" },
                new OrderArea { OrderAreaID=7, Name = "Услуги проведения праздников", Description = "Проведение мероприятий, украшение помещений" },
                new OrderArea { OrderAreaID=8, Name = "Услуги преподавателей, репититоров, переводчиков", Description = "Услуги преподавателей, репититоров и прочих обучающих услуг" },
                new OrderArea { OrderAreaID=9, Name = "Услуги домохозяек, нянь, сиделок", Description = "Услуги репититоров, нянь, сиделок и прочих личных услуг" },
                new OrderArea { OrderAreaID=100, Name = "Прочие услуги", Description = "Прочие услуги не входящие в другие категории" },
            });
            context.SaveChanges();

            context.BudgetLevels.AddOrUpdate(x => x.BudgetLevelID, new BudgetLevel[] {
                new BudgetLevel { Name = "менее 5000", BudgetLevelID =1 },
                new BudgetLevel { Name = "5000-10000", BudgetLevelID = 2},
                new BudgetLevel { Name = "10000-50000", BudgetLevelID = 3 },
                new BudgetLevel { Name = "более 50000", BudgetLevelID = 4 }
            });
            context.SaveChanges();

            context.OrderStatuses.AddOrUpdate(x => x.OrderStatusID, new OrderStatus[] {
                new OrderStatus { Name = "Черновик", OrderStatusID = 1 },
                new OrderStatus { Name = "Приём заявок", OrderStatusID = 2 },
                new OrderStatus { Name = "Заключён контракт", OrderStatusID = 3 },
                new OrderStatus { Name = "Заключёны контракты", OrderStatusID = 4 },
                new OrderStatus { Name = "Запрос на закрытие", OrderStatusID = 5 },
                new OrderStatus { Name = "Закрыт", OrderStatusID = 6 }
            });
            context.SaveChanges();

            context.OrderWatchStatuses.AddOrUpdate(x => x.OrderWatchStatusID, new OrderWatchStatus[] {
                new OrderWatchStatus { Name = "Нет заявок", OrderWatchStatusID = 1 },
                new OrderWatchStatus { Name="Заявка", OrderWatchStatusID = 2 }
            });
            context.SaveChanges();

            context.BidStatuses.AddOrUpdate(x => x.BidStatusID, new BidStatus[] { 
                new BidStatus { Name = "Заявка", BidStatusID=1 },
                new BidStatus { Name = "Заказчик предлагает контракт", BidStatusID=2 },
                new BidStatus { Name = "Заключён контракт", BidStatusID=3 },
                new BidStatus { Name = "Выбран другой исполнитель", BidStatusID=4 },
                new BidStatus { Name = "Отказ заказчика", BidStatusID=5 },
                new BidStatus { Name = "Отказ исполнителя", BidStatusID=6 },
                new BidStatus { Name = "Контракт выполнен", BidStatusID=7 }
            });
            context.SaveChanges();

            /*Тестовые данные*/
            context.Profiles.AddOrUpdate(x => x.Name, new Profile[] {
                new Profile { Name = "Заказчик", IsActive = true, ProfileTypeID = 1, OrderAreaID = 1, UserID = "penart@nm.ru" },
                new Profile { Name = "Исполнитель", IsActive = true, ProfileTypeID = 2, OrderAreaID = 1, UserID = "penart@nm.ru" }
            });
            context.SaveChanges();

            context.Orders.AddOrUpdate(x => x.Name, new Order[] {
                new Order { Name = "Тестовый заказ", ProfileID = 1, OrderAreaID = 1, OrderStatusID = 2, BudgetLevelID = 1, Status = 0, UserID = "penart@nm.ru" }
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
