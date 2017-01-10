namespace bi42.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        BidID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(),
                        OrderID = c.Int(nullable: false),
                        BidStatusID = c.Int(nullable: false),
                        UserID = c.String(),
                        PublishTime = c.DateTime(),
                        AcceptTime = c.DateTime(),
                        ResponseTime = c.DateTime(),
                        CostTotal = c.Decimal(precision: 18, scale: 2),
                        TimeRate = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BidID)
                .ForeignKey("dbo.BidStatuses", t => t.BidStatusID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.OrderID)
                .Index(t => t.BidStatusID);
            
            CreateTable(
                "dbo.BidStatuses",
                c => new
                    {
                        BidStatusID = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BidStatusID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(nullable: false),
                        OrderStatusID = c.Int(nullable: false),
                        BudgetLevelID = c.Int(nullable: false),
                        UserID = c.String(),
                        Name = c.String(nullable: false),
                        OrderFile = c.String(),
                        PublishTime = c.DateTime(),
                        StartTime = c.DateTime(),
                        Budget = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(),
                        Requirement = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.BudgetLevels", t => t.BudgetLevelID, cascadeDelete: true)
                .ForeignKey("dbo.OrderStatus", t => t.OrderStatusID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.ProfileID)
                .Index(t => t.OrderStatusID)
                .Index(t => t.BudgetLevelID);
            
            CreateTable(
                "dbo.BudgetLevels",
                c => new
                    {
                        BudgetLevelID = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BudgetLevelID);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        OrderStatusID = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.OrderStatusID);
            
            CreateTable(
                "dbo.OrderWatches",
                c => new
                    {
                        OrderWatchID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(),
                        OrderID = c.Int(nullable: false),
                        OrderWatchStatusID = c.Int(nullable: false),
                        UserID = c.String(),
                        PublishTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrderWatchID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.OrderWatchStatuses", t => t.OrderWatchStatusID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.OrderID)
                .Index(t => t.OrderWatchStatusID);
            
            CreateTable(
                "dbo.OrderWatchStatuses",
                c => new
                    {
                        OrderWatchStatusID = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.OrderWatchStatusID);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ProfileID = c.Int(nullable: false, identity: true),
                        OrderAreaID = c.Int(nullable: false),
                        ProfileTypeID = c.Int(nullable: false),
                        UserID = c.String(),
                        Name = c.String(nullable: false),
                        Logo = c.String(),
                        LogoImage = c.String(),
                        SiteUrl = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        PublishTime = c.DateTime(),
                        Status = c.Int(),
                        Bids = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ProfileID)
                .ForeignKey("dbo.OrderAreas", t => t.OrderAreaID, cascadeDelete: true)
                .Index(t => t.OrderAreaID);
            
            CreateTable(
                "dbo.OrderAreas",
                c => new
                    {
                        OrderAreaID = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.OrderAreaID);
            
            CreateTable(
                "dbo.Commodities",
                c => new
                    {
                        CommodityID = c.Int(nullable: false, identity: true),
                        ShopID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PhotoImage = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommodityID)
                .ForeignKey("dbo.Shops", t => t.ShopID, cascadeDelete: true)
                .Index(t => t.ShopID);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        ShopID = c.Int(nullable: false, identity: true),
                        ShopAreaID = c.Int(nullable: false),
                        UserID = c.String(),
                        Name = c.String(nullable: false),
                        Logo = c.String(),
                        LogoImage = c.String(),
                        SiteUrl = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        PublishTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ShopID)
                .ForeignKey("dbo.ShopAreas", t => t.ShopAreaID, cascadeDelete: true)
                .Index(t => t.ShopAreaID);
            
            CreateTable(
                "dbo.ShopAreas",
                c => new
                    {
                        ShopAreaID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ShopAreaID);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        ContractID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        BidID = c.Int(),
                        Description = c.String(),
                        PublishTime = c.DateTime(),
                        AcceptTime = c.DateTime(),
                        FinishTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContractID)
                .ForeignKey("dbo.Bids", t => t.BidID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.BidID);
            
            CreateTable(
                "dbo.MyWorks",
                c => new
                    {
                        MyWorkID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(nullable: false),
                        UserID = c.String(),
                        Name = c.String(nullable: false),
                        MyWorkImage = c.String(),
                        SiteUrl = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MyWorkID)
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.ProfileID);
            
            CreateTable(
                "dbo.PrivateOrderMessages",
                c => new
                    {
                        PrivateOrderMessageID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        FromProfileID = c.Int(),
                        ToProfileID = c.Int(),
                        UserID = c.String(),
                        Message = c.String(nullable: false),
                        PublishTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.PrivateOrderMessageID)
                .ForeignKey("dbo.Profiles", t => t.FromProfileID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ToProfileID)
                .Index(t => t.OrderID)
                .Index(t => t.FromProfileID)
                .Index(t => t.ToProfileID);
            
            CreateTable(
                "dbo.PublicOrderMessages",
                c => new
                    {
                        PublicOrderMessageID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(),
                        OrderID = c.Int(nullable: false),
                        UserID = c.String(),
                        Message = c.String(nullable: false),
                        PublishTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.PublicOrderMessageID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.ShopBoard",
                c => new
                    {
                        ShopBoardID = c.Int(nullable: false, identity: true),
                        ShopID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShopBoardID)
                .ForeignKey("dbo.Shops", t => t.ShopID, cascadeDelete: true)
                .Index(t => t.ShopID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopBoard", "ShopID", "dbo.Shops");
            DropForeignKey("dbo.PublicOrderMessages", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.PublicOrderMessages", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.PrivateOrderMessages", "ToProfileID", "dbo.Profiles");
            DropForeignKey("dbo.PrivateOrderMessages", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.PrivateOrderMessages", "FromProfileID", "dbo.Profiles");
            DropForeignKey("dbo.MyWorks", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Contracts", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Contracts", "BidID", "dbo.Bids");
            DropForeignKey("dbo.Commodities", "ShopID", "dbo.Shops");
            DropForeignKey("dbo.Shops", "ShopAreaID", "dbo.ShopAreas");
            DropForeignKey("dbo.Bids", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Orders", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.OrderWatches", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "OrderAreaID", "dbo.OrderAreas");
            DropForeignKey("dbo.OrderWatches", "OrderWatchStatusID", "dbo.OrderWatchStatuses");
            DropForeignKey("dbo.OrderWatches", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OrderStatusID", "dbo.OrderStatus");
            DropForeignKey("dbo.Orders", "BudgetLevelID", "dbo.BudgetLevels");
            DropForeignKey("dbo.Bids", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Bids", "BidStatusID", "dbo.BidStatuses");
            DropIndex("dbo.ShopBoard", new[] { "ShopID" });
            DropIndex("dbo.PublicOrderMessages", new[] { "OrderID" });
            DropIndex("dbo.PublicOrderMessages", new[] { "ProfileID" });
            DropIndex("dbo.PrivateOrderMessages", new[] { "ToProfileID" });
            DropIndex("dbo.PrivateOrderMessages", new[] { "FromProfileID" });
            DropIndex("dbo.PrivateOrderMessages", new[] { "OrderID" });
            DropIndex("dbo.MyWorks", new[] { "ProfileID" });
            DropIndex("dbo.Contracts", new[] { "BidID" });
            DropIndex("dbo.Contracts", new[] { "OrderID" });
            DropIndex("dbo.Shops", new[] { "ShopAreaID" });
            DropIndex("dbo.Commodities", new[] { "ShopID" });
            DropIndex("dbo.Profiles", new[] { "OrderAreaID" });
            DropIndex("dbo.OrderWatches", new[] { "OrderWatchStatusID" });
            DropIndex("dbo.OrderWatches", new[] { "OrderID" });
            DropIndex("dbo.OrderWatches", new[] { "ProfileID" });
            DropIndex("dbo.Orders", new[] { "BudgetLevelID" });
            DropIndex("dbo.Orders", new[] { "OrderStatusID" });
            DropIndex("dbo.Orders", new[] { "ProfileID" });
            DropIndex("dbo.Bids", new[] { "BidStatusID" });
            DropIndex("dbo.Bids", new[] { "OrderID" });
            DropIndex("dbo.Bids", new[] { "ProfileID" });
            DropTable("dbo.ShopBoard");
            DropTable("dbo.PublicOrderMessages");
            DropTable("dbo.PrivateOrderMessages");
            DropTable("dbo.MyWorks");
            DropTable("dbo.Contracts");
            DropTable("dbo.ShopAreas");
            DropTable("dbo.Shops");
            DropTable("dbo.Commodities");
            DropTable("dbo.OrderAreas");
            DropTable("dbo.Profiles");
            DropTable("dbo.OrderWatchStatuses");
            DropTable("dbo.OrderWatches");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.BudgetLevels");
            DropTable("dbo.Orders");
            DropTable("dbo.BidStatuses");
            DropTable("dbo.Bids");
        }
    }
}
