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
            /*�������� �����������*/
            context.OrderAreas.AddOrUpdate(x => x.OrderAreaID, new OrderArea[] {
                new OrderArea { OrderAreaID=1, Name = "���������� �� � �� ����������", Description = "���������� ������������ ����������� � ���������������� � ���� �������" },
                new OrderArea { OrderAreaID=2, Name = "������������ � ��������� ������", Description = "������������ � ��������� ������ ���� �����" },
                new OrderArea { OrderAreaID=3, Name = "��������������", Description = "�������� ��������� ��� ���������� � ����������� ���" },
                new OrderArea { OrderAreaID=4, Name = "��� ��� ���������", Description = "���������� �����������, ��������� ���������" },
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
                new OrderStatus { Name = "��������" },
                new OrderStatus { Name = "������" },
                new OrderStatus { Name = "�������� ��������" },
                new OrderStatus { Name = "��������� ���������" },
                new OrderStatus { Name = "������" },
                new OrderStatus { Name = "��������" }
            });
            context.SaveChanges();

            context.OrderWatchStatuses.AddOrUpdate(x => x.Name, new OrderWatchStatus[] {
                new OrderWatchStatus { Name = "��� ������" },
                new OrderWatchStatus { Name="������"}
            });
            context.SaveChanges();

            context.BidStatuses.AddOrUpdate(x => x.Name, new BidStatus[] { 
                new BidStatus { Name = "������" },
                new BidStatus { Name = "������ ������� ����������" },
                new BidStatus { Name = "�������� ��������" },
                new BidStatus { Name = "������ ������" },
                new BidStatus { Name = "����� ���������" },
                new BidStatus { Name = "����� ����������" }
            });
            context.SaveChanges();

            /*�������� ������*/
            context.Profiles.AddOrUpdate(x => x.Name, new Profile[] {
                new Profile { Name = "��������", IsActive = true, ProfileTypeID = 1, OrderAreaID = 1, UserID = "penart@nm.ru" },
                new Profile { Name = "���������", IsActive = true, ProfileTypeID = 2, OrderAreaID = 1, UserID = "penart@nm.ru" }
            });
            context.SaveChanges();

            context.Orders.AddOrUpdate(x => x.Name, new Order[] {
                new Order { Name = "�������� �����", ProfileID = 1, OrderStatusID = 2, BudgetLevelID = 1, Status = 0, UserID = "penart@nm.ru" }
            });
            context.SaveChanges();


            context.ShopAreas.AddOrUpdate(x => x.Name, new ShopArea[] {
                new ShopArea { Name = "�����������", Description = "�������, ��������, ��������, ������ � �.�" },
                new ShopArea { Name = "��������� ��������������", Description = "��������� ������" }
            });
            context.SaveChanges();

        }
    }
}
