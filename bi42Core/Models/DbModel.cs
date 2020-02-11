using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bi42.Models
{
   

    public class PrivateMessage 
    {
        public int id;
        public string publishtime;
        public string profile;
        public string message;

    }

    public class PublicMessage
    {
        public int id;
        public string publishtime;
        public string profile;
        public string message;

    }

    //Базовый класс со служебными полями
    public class BaseEntity
    {
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateUserID { get; set; }

        public virtual void OnBeforeInsert()
        {
            CreateTime = DateTime.Now;
        }

        public virtual void OnBeforeUpdate()
        {
            UpdateTime = DateTime.Now;
        }

    }

    [Table("UserLastAccess")]
    public class UserLastAccess
    {
        [Key, Column(Order = 1)]
        public string UserID {get;set;}
        [Key, Column(Order = 2)]
        public string Key { get; set; }
        public DateTime? LastAccess { get; set; }
        public DateTime? LastEmail { get; set; }
        public int Count { get; set; }
    }

    #region Раздел заказы, справочники

    /**
     * Таблица областей профилей
     **/
    [Table("OrderAreas")]
    public class OrderArea : BaseEntity
    {
        [Key]
        public int OrderAreaID { get; set; }
        [Display(Name = "Область")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    /**
     * Таблица статусов заказа
     **/
    [Table("OrderStatus")]
    public class OrderStatus : BaseEntity
    {
        [Key]
        public int OrderStatusID { get; set; }
        [Display(Name = "Статус")]
        public string Name { get; set; }
    }


    /**
     * Таблица бюджетов заказа
     **/
    [Table("BudgetLevels")]
    public class BudgetLevel : BaseEntity
    {
        [Key]
        public int BudgetLevelID { get; set; }
        [Display(Name = "Бюджет, руб.")]
        public string Name { get; set; }
    }

    /**
     * Таблица статусов ставок
     **/
    [Table("BidStatuses")]
    public class BidStatus : BaseEntity
    {
        [Key]
        public int BidStatusID { get; set; }
        [Display(Name = "Статус")]
        public string Name { get; set; }
    }

    /**
     * Таблица статусов просмотра проектов
     **/
    [Table("OrderWatchStatuses")]
    public class OrderWatchStatus : BaseEntity
    {
        [Key]
        public int OrderWatchStatusID { get; set; }
        [Display(Name = "Статус")]
        public string Name { get; set; }
    }

    /**
    * Таблица статусов задач
     **/
    [Table("TaskStatuses")]
    public class TaskStatus : BaseEntity
    {
        [Key]
        public int TaskStatusID { get; set; }
        [Display(Name = "Статус")]
        public string Name { get; set; }
    }



    #endregion

    #region Раздел заказы
    /**
     * Таблица профилей пользователей
     **/
    [Table("Profiles")]
    public class Profile : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProfileID { get; set; }

        [Display(Name="Область заказа работ/услуг")]
        public int OrderAreaID { get; set; }
        [Display(Name = "Тип профиля")]
        public int ProfileTypeID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "Профиль"), Required(ErrorMessage = "Введите название профиля!")]
        public string Name { get; set; }
        
        [Display(Name = "Слоган")]
        public string Slogan { get; set; }
        
        [Display(Name = "Изображение логотипа")]
        public string LogoImage { get; set; }
        
        [Display(Name = "Адрес сайта")]
        public string SiteUrl { get; set; }
        
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Опубликован")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }
        
        [Display(Name = "Статус")]
        public int? Status { get; set; }
        
        [Display(Name = "Ставок")]
        public int? Bids { get; set; }
        
        [Display(Name = "Текущий профиль")]
        public bool IsActive { get; set; }
        
        [Display(Name = "Описание профиля")]
        public string Description { get; set; }

        [ForeignKey("OrderAreaID")]
        public virtual OrderArea OrderArea { get; set; }

        public virtual ICollection<FeedBack> FeedBacks { get; set; }

        public virtual ICollection<FeedBack> FeedBackFroms { get; set; }

        public virtual ICollection<MyWork> MyWorks { get; set; }

        [Display(Name = "Область")]
        [NotMapped]
        public string OrderAreaName { get { return OrderArea.Name; } }
    }

    /**
    * Таблица проектов пользователей
    **/
    [Table("Orders")]
    public class Order : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderID  { get; set; }

        public int ProfileID { get; set; }

        [Display(Name = "Область заказа")]
        public int OrderAreaID { get; set; }

        [Display(Name = "Статус заказа")]
        public int OrderStatusID { get; set; }
        [Display(Name = "Бюджетный уровень")]
        public int BudgetLevelID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "Заказ"), Required(ErrorMessage = "Введите название заказа!")]
        public string Name { get; set; }

        [Display(Name = "Файл с описанием задачи")]
        public string OrderFile { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        [Display(Name = "Опубликован")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]       
        public DateTime? PublishTime { get; set; }

        [Display(Name = "Старт")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime { get; set; }
       
        [Display(Name = "Фикс. бюджет, руб")]
        public decimal? Budget { get; set; }
        
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Обязательные требования"), DisplayFormat(NullDisplayText="Нет")]
        public string Requirement { get; set; }

        [Display(Name = "Архивный статус")]
        public int Status { get; set; }

        [Display(Name = "Профиль")]
        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

        [Display(Name = "Область")]
        [ForeignKey("OrderAreaID")]
        public virtual OrderArea OrderArea { get; set; }

        [ForeignKey("OrderStatusID")]
        public virtual OrderStatus OrderStatus { get; set; }

        [Display(Name = "Бюджет, руб")]
        [ForeignKey("BudgetLevelID")]
        public virtual BudgetLevel BudgetLevel { get; set; }

        [Display(Name = "Заявки")]
        public virtual ICollection<Bid> Bids { get; set; }

        public virtual ICollection<OrderWatch> OrderWatches { get; set; }
        
        [Display(Name = "Контракты")]
        public virtual ICollection<Contract> Contracts { get; set; }

        [NotMapped]
        [Display(Name = "Профиль")]
        public string ProfileName { get { return Profile.Name; } }

        [NotMapped]
        [Display(Name = "Область")]
        public string OrderAreaName { get { return OrderArea.Name; } }

        [NotMapped]
        [Display(Name = "Статус заказа")]
        public string OrderStatusName { get { return OrderStatus.Name; } }

        [NotMapped]
        [Display(Name = "Бюджет заказа, руб")]
        public string BudgetLevelName { get { return BudgetLevel.Name; } }

        [NotMapped]
        [Display(Name = "Заявки")]
        public int CountBids { get { return (Bids!=null)?Bids.Count:0; } }

    }

    /**
     * Таблица отобранных для просмотра заказов
     **/
    [Table("OrderWatches")]
    public class OrderWatch : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderWatchID { get; set; }
        
        public int? ProfileID { get; set; }
        public int OrderID { get; set; }
        public int OrderWatchStatusID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "Опубликован")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
        
        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }
        
        [ForeignKey("OrderWatchStatusID")]
        public virtual OrderWatchStatus OrderWatchStatus { get; set; }

        [NotMapped]
        [Display(Name = "Профиль")]
        public string ProfileName { get { return Profile.Name; } }

        [NotMapped]
        [Display(Name = "Заказ")]        
        public string OrderName { get { return Order.Name; } }
        
        [NotMapped]
        [Display(Name = "Архив с описанием заказа")]
        public string OrderFile { get { return Order.OrderFile; } }

        [NotMapped]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Старт")]
        public DateTime? StartTime { get { return Order.StartTime; } }

        [NotMapped]
        [Display(Name = "Статус")]
        public int? OrderStatusID { get { return Order.OrderStatus.OrderStatusID; } }

        [NotMapped]
        [Display(Name = "Статус")]
        public string OrderStatusName { get { return Order.OrderStatus.Name; } }

        [NotMapped]
        [Display(Name = "Бюджет, руб")]
        public int BudgetLevelID { get { return Order.BudgetLevel.BudgetLevelID; } }

        [NotMapped]
        [Display(Name = "Бюджет, руб")]
        public string BudgetLevelName { get { return Order.BudgetLevel.Name; } }

        [NotMapped]
        [Display(Name = "Фикс. бюджет, руб")]
        public decimal? Budget { get { return Order.Budget; } }

        [NotMapped]
        [Display(Name = "Описание")]
        public string Description { get { return Order.Description; } }
    }

    /**
     * Таблица ставок
     **/
    [Table("Bids")]
    public class Bid : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BidID  { get; set; }

        public int? ProfileID { get; set; }
        public int OrderID { get; set; }
        public int BidStatusID { get; set; }
        public string UserID { get; set; }
              
        [Display(Name = "Заказ")]
        public string OrderName { get { return Order.Name; } }

        [Display(Name = "Дата подачи")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }

        [Display(Name = "Дата принятия")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AcceptTime { get; set; }

        [Display(Name = "Дата ответа")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ResponseTime { get; set; }

        [Display(Name = "Стоимость работ, руб")]
        public decimal? CostTotal { get; set; }

        [Display(Name = "Стоимость часа, руб")]
        public decimal? TimeRate { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Архивный статус")]
        public int Status { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

        [ForeignKey("BidStatusID")]
        public virtual BidStatus BidStatus { get; set; }

        [NotMapped]
        [Display(Name = "Профиль")]
        public string ProfileName { get { return Profile.Name; } }

        [NotMapped]
        [Display(Name = "Старт")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime { get { return (Order!=null)?Order.StartTime:null; } }

        [NotMapped]
        [Display(Name = "Статус заказа")]
        public int? OrderStatusID { get { return (int?)((Order.OrderStatus != null) ? (int?)Order.OrderStatus.OrderStatusID : null); } }

        [NotMapped]
        [Display(Name = "Статус заказа")]
        public string OrderStatusName { get { return Order.OrderStatus.Name; } }

        [NotMapped]
        public int? BudgetLevelID { get { return Order.BudgetLevel.BudgetLevelID; } }

        [NotMapped]
        [Display(Name = "Бюджет, руб")]
        public string BudgetLevelName { get { return Order.BudgetLevel.Name; } }

        [NotMapped]
        [Display(Name = "Фикс. бюджет, руб")]
        public decimal? Budget { get { return Order.Budget; } }

        [NotMapped]
        [Display(Name = "Описание")]
        public string OrderDescription { get { return Order.Description; } }

        [NotMapped]
        [Display(Name = "Статус заявки")]
        public string BidStatusName { get { return BidStatus.Name; } }
        
    }

    /**
     * Таблица отзывов
     **/
    [Table("FeedBacks")]
    public class FeedBack : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FeedBackID { get; set; }
        public string UserID { get; set; }
        public int OrderID { get; set; }
        public int ProfileID { get; set; }
        public int? FromProfileID { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }
        [Display(Name="Отзыв")]
        public string Description {get;set;}
        [Display(Name = "Оценка")]
        public int Estimation { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }
        [ForeignKey("FromProfileID")]
        public virtual Profile FromProfile { get; set; }
    }

    /**
     * Таблица публичных сообщений
     **/
    [Table("PublicOrderMessages")]
    public class PublicOrderMessage : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PublicOrderMessageID { get; set; }

        public int? ProfileID { get; set; }
        public int OrderID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "Текст сообщения"), Required(ErrorMessage = "Введите сообщение!")]
        public string Message { get; set; }
        
        [Display(Name = "Опубликовано")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }

    }

    /**
     * Таблица приватных сообщений
     **/
    [Table("PrivateOrderMessages")]
    public class PrivateOrderMessage : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PrivateOrderMessageID { get; set; }

        public int OrderID { get; set; }
        public int? FromProfileID { get; set; }
        public int? ToProfileID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "Текст сообщения"), Required(ErrorMessage = "Введите сообщение!")]
        public string Message { get; set; }

        [Display(Name = "Опубликовано")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("FromProfileID")]
        public virtual Profile FromProfile { get; set; }

        [ForeignKey("ToProfileID")]
        public virtual Profile ToProfile { get; set; }
    }

    /**
     * Таблица задач по заказу
     **/
    [Table("OrderTasks")]
    public class OrderTask : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderTaskID { get; set; }
        public int OrderID { get; set; }
        //public int? FromProfileID { get; set; }
        [Display(Name = "Назначено")]
        public int? ToProfileID { get; set; }
        [Display(Name = "Статус задачи")]
        public int? TaskStatusID { get; set; }

        [Display(Name = "Наименование задачи"), Required(ErrorMessage = "Введите наименование!")]
        public string Name { get; set; }

        [Display(Name = "Описание задания"), Required(ErrorMessage = "Введите описание!")]
        public string Description { get; set; }

        [Display(Name = "Опубликовано")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }

        [Display(Name = "Начать")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime { get; set; }

        [Display(Name = "Выполнить до")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? StopTime { get; set; }

        [Display(Name = "Стоимость, руб")]
        public decimal? CostTotal { get; set; }

        [Display(Name = "Трудозатраты, чел-час")]
        public int ManHour { get; set; }

        [Display(Name = "Коментарий исполнителя")]
        public string Comment { get; set; }

        [Display(Name = "Файл с описанием задачи")]
        public string TaskFile { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        //[ForeignKey("FromProfileID")]
        //public virtual Profile FromProfile { get; set; }
        [ForeignKey("ToProfileID")]
        [Display(Name = "Назначено")]
        public virtual Profile ToProfile { get; set; }

        [ForeignKey("TaskStatusID")]
        [Display(Name = "Статус задачи")]
        public virtual TaskStatus TaskStatus { get; set; }
        //public DateTime? UpdateTime { get; set; }
    }




    /**
     * Таблица контрактов
     **/
    public class Contract : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Контракт №")]
        public int ContractID { get; set; }
        [Display(Name = "Заказ №")]
        public int OrderID { get; set; }
        [Display(Name = "Заявка №")]
        public int BidID { get; set; }

        [Display(Name = "Опубликован")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }

        [Display(Name = "Дата заключения")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? AcceptTime { get; set; }

        [Display(Name = "Дата отчёта о выполнении")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? ReportTime { get; set; }

        [Display(Name = "Дата закрытия")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? FinishTime { get; set; }

        [Display(Name = "Архивный статус")]
        public int Status { get; set; }

        [Display(Name = "Текст контракта")]
        public string Description { get; set; }

        [Display(Name = "Отчёт о выполнение контракта")]
        public string Report { get; set; }
        
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("BidID")]
        public virtual Bid Bid { get; set; }

    }

    /**
    * Таблица примеров моей работы
    **/
    [Table("MyWorks")]
    public class MyWork : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MyWorkID { get; set; }
        [Display(Name = "Профиль")]
        public int ProfileID { get; set; }
        public string UserID { get; set; }
        [Display(Name = "Название"), Required(ErrorMessage = "Введите название работы!")]
        public string Name { get; set; }
        [Display(Name = "Картинка")]
        public string MyWorkImage { get; set; }
        [Display(Name = "Сайт URL")]
        public string SiteUrl { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Показывать")]
        public bool IsActive { get; set; }
        [ForeignKey("ProfileID")]
        public virtual Profile Profile { get; set; }
    }

    #endregion

    #region Управление почтовой рассылкой

    [Table("MessageEvents")]
    public class MessageEvent : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }
    }

    #endregion

    #region раздел магазин, справочники

    /**
    * Таблица областей торговли
    **/
    [Table("ShopAreas")]
    public class ShopArea
    {
        [Key]
        public int ShopAreaID { get; set; }
        [Display(Name = "Область"), Required(ErrorMessage = "Введите значение!")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }

    #endregion

    #region Раздел магазин
    /**
     * Таблица мой магазин
     **/

    [Table("Shops")]
    public class Shop
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ShopID { get; set; }

        [Display(Name = "Сфера торговли")]
        public int ShopAreaID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "Магазин"), Required(ErrorMessage = "Введите название магазина!")]
        public string Name { get; set; }

        [Display(Name = "Логотип")]
        public string Logo { get; set; }

        [Display(Name = "Logo URL")]
        public string LogoImage { get; set; }

        [Display(Name = "Сайт URL")]
        public string SiteUrl { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Опубликован")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PublishTime { get; set; }

        [Display(Name = "Активность")]
        public bool IsActive { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [ForeignKey("ShopAreaID")]
        public virtual ShopArea ShopArea { get; set; }

    }

    /**
    * Таблица товаров торговли
    **/
    [Table("Commodities")]
    public class Commodity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CommodityID { get; set; }
        [Display(Name = "Магазин")]
        public int ShopID { get; set; }
        [Display(Name = "Товар"), Required(ErrorMessage = "Введите название товара!")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Цена, руб.")]
        public decimal Price { get; set; }
        [Display(Name = "Фото")]
        public string PhotoImage { get; set; }
        [Display(Name = "Наличие")]
        public bool IsActive { get; set; }
        [ForeignKey("ShopID")]
        public virtual Shop Shop { get; set; }
    }

    /**
    * Таблица новости торговли
    **/
    [Table("ShopBoard")]
    public class ShopBoard
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ShopBoardID { get; set; }
        [Display(Name = "Магазин")]
        public int ShopID { get; set; }
        [Display(Name = "Краткое описание"), Required(ErrorMessage = "Введите анонс новости!")]
        public string Name { get; set; }
        [Display(Name = "полное описание")]
        public string Description { get; set; }
        [Display(Name = "Показать")]
        public bool IsActive { get; set; }
        [ForeignKey("ShopID")]
        public virtual Shop Shop { get; set; }
    }

    #endregion

    public class DbModel : DbContext
    {
        //public DbModel():base("DefaultConnection")
        //{
        //}

        public DbSet<UserLastAccess> UserLastAccess { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<OrderArea> OrderAreas { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<BudgetLevel> BudgetLevels { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PublicOrderMessage> PublicOrderMessages { get; set; }
        public DbSet<PrivateOrderMessage> PrivateOrderMessages { get; set; }
        public DbSet<OrderWatch> OrderWatches { get; set; }
        public DbSet<OrderWatchStatus> OrderWatchStatuses { get; set; }
        public DbSet<BidStatus> BidStatuses { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }
        public DbSet<OrderTask> OrderTasks { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<MyWork> MyWorks { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        //магазин
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopArea> ShopAreas { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<ShopBoard> ShopBoards { get; set; }

        //Рассылка почтовая

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasRequired(t => t.OrderArea)
                .WithMany(t => t.Orders)
                .HasForeignKey(t => t.OrderAreaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FeedBack>()
                .HasRequired(t => t.Profile)
                .WithMany(t => t.FeedBacks)
                .HasForeignKey(t => t.ProfileID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FeedBack>()
               .HasRequired(t => t.FromProfile)
               .WithMany(t => t.FeedBackFroms)
               .HasForeignKey(t => t.FromProfileID)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contract>()
                .HasRequired(t => t.Order)
                .WithMany(t => t.Contracts)
                .HasForeignKey(t => t.OrderID)
                .WillCascadeOnDelete(false);
        }

        /// <summary>
        /// переопределим метод сохранения данных для задействования служебной логики
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker.Entries();

            foreach (var changedEntity in changedEntities)
            {
                if (changedEntity.Entity is BaseEntity)
                {
                    var entity = (BaseEntity)changedEntity.Entity;

                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.OnBeforeInsert();
                            break;

                        case EntityState.Modified:
                            entity.OnBeforeUpdate();
                            break;

                    }
                }
            }

            return base.SaveChanges();
        }


    }
}