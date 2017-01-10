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
            /*�������� �����������*/
            context.OrderAreas.AddOrUpdate(x => x.OrderAreaID, new OrderArea[] {
                new OrderArea { OrderAreaID=1, Name = "������ ���������� �� � �� ����������", Description = "���������� ������������ ����������� � ���������������� � ���� �������" },
                new OrderArea { OrderAreaID=2, Name = "������ ������� ����������� �������", Description = "������ �����, ����, �����, � ������ ����������� �������" },
                new OrderArea { OrderAreaID=3, Name = "������ ������� ������� �������", Description = "������ ���������� �����, �������������, � ������ ������� �������" },
                new OrderArea { OrderAreaID=4, Name = "������ ������� �������, �����", Description = "������ �������, �����" },
                new OrderArea { OrderAreaID=5, Name = "������ ������� � ������������ ��������������", Description = "������ ��������� � ������� ����������" },
                new OrderArea { OrderAreaID=6, Name = "������ �����������������", Description = "�������� ��������� ��� ���������� � ����������� ���" },
                new OrderArea { OrderAreaID=7, Name = "������ ���������� ����������", Description = "���������� �����������, ��������� ���������" },
                new OrderArea { OrderAreaID=8, Name = "������ ��������������, �����������, ������������", Description = "������ ��������������, ����������� � ������ ��������� �����" },
                new OrderArea { OrderAreaID=9, Name = "������ ����������, ����, �������", Description = "������ �����������, ����, ������� � ������ ������ �����" },
                new OrderArea { OrderAreaID=100, Name = "������ ������", Description = "������ ������ �� �������� � ������ ���������" },
            });
            context.SaveChanges();

            context.BudgetLevels.AddOrUpdate(x => x.BudgetLevelID, new BudgetLevel[] {
                new BudgetLevel { Name = "����� 5000", BudgetLevelID =1 },
                new BudgetLevel { Name = "5000-10000", BudgetLevelID = 2},
                new BudgetLevel { Name = "10000-50000", BudgetLevelID = 3 },
                new BudgetLevel { Name = "����� 50000", BudgetLevelID = 4 }
            });
            context.SaveChanges();

            context.OrderStatuses.AddOrUpdate(x => x.OrderStatusID, new OrderStatus[] {
                new OrderStatus { Name = "��������", OrderStatusID = 1 },
                new OrderStatus { Name = "���� ������", OrderStatusID = 2 },
                new OrderStatus { Name = "�������� ��������", OrderStatusID = 3 },
                new OrderStatus { Name = "��������� ���������", OrderStatusID = 4 },
                new OrderStatus { Name = "������ �� ��������", OrderStatusID = 5 },
                new OrderStatus { Name = "������", OrderStatusID = 6 }
            });
            context.SaveChanges();

            context.OrderWatchStatuses.AddOrUpdate(x => x.OrderWatchStatusID, new OrderWatchStatus[] {
                new OrderWatchStatus { Name = "��� ������", OrderWatchStatusID = 1 },
                new OrderWatchStatus { Name="������", OrderWatchStatusID = 2 }
            });
            context.SaveChanges();

            context.BidStatuses.AddOrUpdate(x => x.BidStatusID, new BidStatus[] { 
                new BidStatus { Name = "������", BidStatusID=1 },
                new BidStatus { Name = "�������� ���������� ��������", BidStatusID=2 },
                new BidStatus { Name = "�������� ��������", BidStatusID=3 },
                new BidStatus { Name = "������ ������ �����������", BidStatusID=4 },
                new BidStatus { Name = "����� ���������", BidStatusID=5 },
                new BidStatus { Name = "����� �����������", BidStatusID=6 },
                new BidStatus { Name = "�������� ��������", BidStatusID=7 }
            });
            context.SaveChanges();

            /*�������� ������*/
            context.Profiles.AddOrUpdate(x => x.Name, new Profile[] {
                new Profile { Name = "��������", IsActive = true, ProfileTypeID = 1, OrderAreaID = 1, UserID = "penart@nm.ru" },
                new Profile { Name = "�����������", IsActive = true, ProfileTypeID = 2, OrderAreaID = 1, UserID = "penart@nm.ru" }
            });
            context.SaveChanges();

            context.Orders.AddOrUpdate(x => x.Name, new Order[] {
                new Order { Name = "�������� �����", ProfileID = 1, OrderAreaID = 1, OrderStatusID = 2, BudgetLevelID = 1, Status = 0, UserID = "penart@nm.ru" }
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
