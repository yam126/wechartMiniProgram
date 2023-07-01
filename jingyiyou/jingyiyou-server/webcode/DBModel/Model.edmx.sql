
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/07/2023 18:32:17
-- Generated from EDMX file: E:\code\项目\新彩俱乐部\web\DBModel\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ncc2019db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[ActionLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActionLog];
GO
IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[Apply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Apply];
GO
IF OBJECT_ID(N'[dbo].[CarCall]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarCall];
GO
IF OBJECT_ID(N'[dbo].[CFJControl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CFJControl];
GO
IF OBJECT_ID(N'[dbo].[CFJMachine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CFJMachine];
GO
IF OBJECT_ID(N'[dbo].[CFJMemberType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CFJMemberType];
GO
IF OBJECT_ID(N'[dbo].[CFJPay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CFJPay];
GO
IF OBJECT_ID(N'[dbo].[CFJQrCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CFJQrCode];
GO
IF OBJECT_ID(N'[dbo].[Channel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Channel];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Comments_copy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments_copy];
GO
IF OBJECT_ID(N'[dbo].[CommonLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommonLog];
GO
IF OBJECT_ID(N'[dbo].[CommonPay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommonPay];
GO
IF OBJECT_ID(N'[dbo].[FeedBack]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FeedBack];
GO
IF OBJECT_ID(N'[dbo].[GetPassword]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GetPassword];
GO
IF OBJECT_ID(N'[dbo].[GongGao]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GongGao];
GO
IF OBJECT_ID(N'[dbo].[GoodProperty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GoodProperty];
GO
IF OBJECT_ID(N'[dbo].[GoodRight]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GoodRight];
GO
IF OBJECT_ID(N'[dbo].[Goods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Goods];
GO
IF OBJECT_ID(N'[dbo].[GoodSort]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GoodSort];
GO
IF OBJECT_ID(N'[dbo].[GoodSortMapping]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GoodSortMapping];
GO
IF OBJECT_ID(N'[dbo].[Info]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Info];
GO
IF OBJECT_ID(N'[dbo].[KeFuMessage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KeFuMessage];
GO
IF OBJECT_ID(N'[dbo].[KuaiDiSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KuaiDiSet];
GO
IF OBJECT_ID(N'[dbo].[LoginLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoginLog];
GO
IF OBJECT_ID(N'[dbo].[Lottery]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lottery];
GO
IF OBJECT_ID(N'[dbo].[LotterySellerRef]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LotterySellerRef];
GO
IF OBJECT_ID(N'[dbo].[Members]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Members];
GO
IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[NCCLottery]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NCCLottery];
GO
IF OBJECT_ID(N'[dbo].[NCCOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NCCOrders];
GO
IF OBJECT_ID(N'[dbo].[News]', 'U') IS NOT NULL
    DROP TABLE [dbo].[News];
GO
IF OBJECT_ID(N'[dbo].[OnLineVersion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OnLineVersion];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[PayLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PayLog];
GO
IF OBJECT_ID(N'[dbo].[Pin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pin];
GO
IF OBJECT_ID(N'[dbo].[QuanZi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuanZi];
GO
IF OBJECT_ID(N'[dbo].[Seller]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Seller];
GO
IF OBJECT_ID(N'[dbo].[ShangJia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShangJia];
GO
IF OBJECT_ID(N'[dbo].[TongZhi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TongZhi];
GO
IF OBJECT_ID(N'[dbo].[VIPUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VIPUser];
GO
IF OBJECT_ID(N'[dbo].[XiaoQu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[XiaoQu];
GO
IF OBJECT_ID(N'[dbo].[ZGGBBS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZGGBBS];
GO
IF OBJECT_ID(N'[dbo].[ZGGKey]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZGGKey];
GO
IF OBJECT_ID(N'[dbo].[ZGGLocation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZGGLocation];
GO
IF OBJECT_ID(N'[dbo].[ZGGMachine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZGGMachine];
GO
IF OBJECT_ID(N'[dbo].[ZGGPay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZGGPay];
GO
IF OBJECT_ID(N'[dbo].[ZGGUseControl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZGGUseControl];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Address'
CREATE TABLE [dbo].[Address] (
    [AddressID] int IDENTITY(1,1) NOT NULL,
    [Address1] varchar(100)  NULL,
    [ZipCode] varchar(10)  NULL,
    [RecName] varchar(50)  NULL,
    [Memo] varchar(300)  NULL,
    [AddDate] datetime  NULL
);
GO

-- Creating table 'GoodSort'
CREATE TABLE [dbo].[GoodSort] (
    [GoodSortID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [ParentID] int  NULL,
    [AddDate] datetime  NULL,
    [Enabled] int  NULL,
    [GoodSortOrder] int  NULL,
    [ShowInIndex] int  NULL,
    [ShowName] varchar(50)  NULL,
    [AddMemberID] int  NULL
);
GO

-- Creating table 'LoginLog'
CREATE TABLE [dbo].[LoginLog] (
    [LoginLogID] int IDENTITY(1,1) NOT NULL,
    [LoginTime] datetime  NULL,
    [IP] varchar(100)  NULL,
    [Agent] varchar(1000)  NULL,
    [SystemInfo] varchar(100)  NULL,
    [MemberID] int  NULL,
    [RefMemberID] int  NULL
);
GO

-- Creating table 'Info'
CREATE TABLE [dbo].[Info] (
    [InfoID] int IDENTITY(1,1) NOT NULL,
    [TheContent] varchar(max)  NULL,
    [AddDate] datetime  NULL,
    [OpeUserID] int  NULL,
    [Title] varchar(500)  NULL,
    [UpeDate] datetime  NULL,
    [ViewCount] int  NULL,
    [ShareImgUrl] varchar(300)  NULL,
    [ShareTitle] varchar(100)  NULL,
    [ShareIntro] varchar(500)  NULL,
    [IsShowInBanner] int  NULL,
    [BannerImgUrl] varchar(300)  NULL
);
GO

-- Creating table 'FeedBack'
CREATE TABLE [dbo].[FeedBack] (
    [FeedBackID] int IDENTITY(1,1) NOT NULL,
    [TheContent] varchar(2000)  NULL,
    [AddDate] datetime  NULL,
    [OperID] int  NULL
);
GO

-- Creating table 'GetPassword'
CREATE TABLE [dbo].[GetPassword] (
    [GetPasswordID] int IDENTITY(1,1) NOT NULL,
    [GUID] varchar(100)  NULL,
    [RefEmail] varchar(100)  NULL,
    [BeginDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [GetDate] datetime  NULL
);
GO

-- Creating table 'PayLog'
CREATE TABLE [dbo].[PayLog] (
    [PayLogID] int IDENTITY(1,1) NOT NULL,
    [Payment] decimal(8,2)  NULL,
    [InDate] datetime  NULL,
    [MemberID] int  NULL,
    [PayDirection] int  NULL,
    [RefOrderID] int  NULL,
    [Param] varchar(50)  NULL
);
GO

-- Creating table 'ActionLog'
CREATE TABLE [dbo].[ActionLog] (
    [ActionLogID] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(100)  NULL,
    [OrderID] int  NULL,
    [ActionDate] datetime  NULL,
    [MemberID] int  NULL,
    [AtionType] int  NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [CommentID] int IDENTITY(1,1) NOT NULL,
    [OrderID] int  NULL,
    [AddDate] datetime  NULL,
    [MemberID] int  NULL,
    [Content] varchar(1000)  NULL,
    [WxVioceMediaID] varchar(100)  NULL,
    [WxVioceUploadDate] datetime  NULL,
    [Name] varchar(50)  NULL,
    [HeadImgUrl] varchar(300)  NULL,
    [ToMemberID] int  NULL
);
GO

-- Creating table 'GoodProperty'
CREATE TABLE [dbo].[GoodProperty] (
    [GoodPropertyID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Content] varchar(500)  NULL,
    [AddDate] datetime  NULL,
    [GoodID] int  NULL,
    [AddMemberID] int  NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [MessageID] int IDENTITY(1,1) NOT NULL,
    [FromMemberID] int  NULL,
    [ToMenberID] int  NULL,
    [Content] nvarchar(max)  NULL,
    [AddDate] datetime  NULL,
    [FromName] varchar(50)  NULL,
    [FromImgUrl] varchar(300)  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'CommonLog'
CREATE TABLE [dbo].[CommonLog] (
    [CommonLogID] int IDENTITY(1,1) NOT NULL,
    [Agent] varchar(500)  NULL,
    [Content] varchar(500)  NULL,
    [AddDate] datetime  NULL
);
GO

-- Creating table 'GoodRight'
CREATE TABLE [dbo].[GoodRight] (
    [GoodRightID] int IDENTITY(1,1) NOT NULL,
    [GoodID] int  NULL,
    [MemberID] int  NULL,
    [BeginDate] datetime  NULL,
    [Param] varchar(200)  NULL,
    [EndDate] datetime  NULL
);
GO

-- Creating table 'KuaiDiSet'
CREATE TABLE [dbo].[KuaiDiSet] (
    [KuaiDiSetID] int IDENTITY(1,1) NOT NULL,
    [CompanyName] varchar(100)  NULL,
    [CompanyCode] varchar(100)  NULL,
    [AddDate] datetime  NULL,
    [MemberID] int  NULL
);
GO

-- Creating table 'CommonPay'
CREATE TABLE [dbo].[CommonPay] (
    [Payment] decimal(8,2)  NULL,
    [MemberID] int  NULL,
    [AddDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [PayType] int  NULL,
    [NeedPay] decimal(8,2)  NULL,
    [HeadImgUrl] varchar(300)  NULL,
    [Name] varchar(50)  NULL,
    [CommonPayID] int IDENTITY(1,1) NOT NULL,
    [GoodID] int  NULL,
    [PayDirection] int  NULL,
    [PayStatus] int  NULL,
    [RefOrderID] int  NULL,
    [TotalPayment] decimal(8,2)  NULL,
    [PayDate] datetime  NULL
);
GO

-- Creating table 'CFJQrCode'
CREATE TABLE [dbo].[CFJQrCode] (
    [CFJQrCodeID] int  NOT NULL,
    [Code] varchar(100)  NULL,
    [AddDate] datetime  NULL,
    [IsUsed] int  NULL,
    [UsedDate] datetime  NULL,
    [MachineCode] varchar(50)  NULL,
    [GroupCode] varchar(100)  NULL
);
GO

-- Creating table 'CFJMemberType'
CREATE TABLE [dbo].[CFJMemberType] (
    [CFJMemberTypeID] int IDENTITY(1,1) NOT NULL,
    [MemberType] int  NULL,
    [DelayTime] int  NULL,
    [DayCount] int  NULL,
    [YearCount] int  NULL,
    [Name] varchar(50)  NULL,
    [MonthCount] int  NULL,
    [Payment] decimal(18,2)  NULL,
    [Memo] varchar(100)  NULL,
    [IsPublic] int  NULL
);
GO

-- Creating table 'CFJPay'
CREATE TABLE [dbo].[CFJPay] (
    [CFJPayID] int IDENTITY(1,1) NOT NULL,
    [Payment] decimal(8,2)  NULL,
    [MemberID] int  NULL,
    [MemberTypeID] int  NULL,
    [State] int  NULL,
    [AddDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [NeedPay] decimal(8,2)  NULL,
    [HeadImgUrl] varchar(300)  NULL,
    [Name] varchar(50)  NULL
);
GO

-- Creating table 'GoodSortMapping'
CREATE TABLE [dbo].[GoodSortMapping] (
    [GoodSortMapping1] int IDENTITY(1,1) NOT NULL,
    [GoodID] int  NULL,
    [GoodSortID] int  NULL
);
GO

-- Creating table 'Comments_copy'
CREATE TABLE [dbo].[Comments_copy] (
    [CommentID] int IDENTITY(1,1) NOT NULL,
    [OrderID] int  NULL,
    [AddDate] datetime  NULL,
    [MemberID] int  NULL,
    [Content] varchar(1000)  NULL,
    [WxVioceMediaID] varchar(100)  NULL,
    [WxVioceUploadDate] datetime  NULL,
    [Name] varchar(50)  NULL,
    [HeadImgUrl] varchar(300)  NULL,
    [ToMemberID] int  NULL
);
GO

-- Creating table 'ZGGKey'
CREATE TABLE [dbo].[ZGGKey] (
    [ZGGKeyID] int IDENTITY(1,1) NOT NULL,
    [Code] varchar(50)  NULL,
    [OwnerWXID] varchar(50)  NULL,
    [UserWXID] varchar(50)  NULL,
    [AddDate] datetime  NULL,
    [UseDate] datetime  NULL,
    [AccpteDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [ZGGMachineID] int  NULL
);
GO

-- Creating table 'ZGGMachine'
CREATE TABLE [dbo].[ZGGMachine] (
    [MachineID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [ShowCode] varchar(20)  NULL,
    [BackCode] varchar(50)  NULL,
    [WxOpenID] varchar(50)  NULL,
    [QrUrl] varchar(500)  NULL,
    [CreateDate] datetime  NULL,
    [Address] varchar(200)  NULL,
    [Phone] varchar(50)  NULL,
    [OwnerName] varchar(50)  NULL,
    [IsShared] int  NULL,
    [IsLocked] int  NULL,
    [OpenUserID] int  NULL,
    [PassWord] varchar(50)  NULL,
    [GuiGe] varchar(50)  NULL,
    [UserPayment] decimal(8,2)  NULL,
    [CompanyPayment] decimal(8,2)  NULL,
    [StrategyPayment] int  NULL,
    [ZGGUseControlID] int  NULL,
    [Latitude] float  NULL,
    [Longitude] float  NULL,
    [Precision] float  NULL
);
GO

-- Creating table 'ZGGPay'
CREATE TABLE [dbo].[ZGGPay] (
    [ZGGPayID] int IDENTITY(1,1) NOT NULL,
    [Payment] decimal(8,2)  NULL,
    [MemberID] int  NULL,
    [MemberTypeID] int  NULL,
    [State] int  NULL,
    [AddDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [NeedPay] decimal(8,2)  NULL,
    [HeadImgUrl] varchar(300)  NULL,
    [Name] varchar(50)  NULL,
    [UseControlID] int  NULL
);
GO

-- Creating table 'ZGGUseControl'
CREATE TABLE [dbo].[ZGGUseControl] (
    [ZGGUseControlID] int IDENTITY(1,1) NOT NULL,
    [NeighborUserID] int  NULL,
    [PutInDate] datetime  NULL,
    [KDUserID] int  NULL,
    [GetOutDate] datetime  NULL,
    [IsPrivate] int  NULL,
    [PaymentType] int  NULL,
    [Payed] decimal(8,2)  NULL,
    [PayState] int  NULL,
    [ZGGPayID] int  NULL,
    [ZGGMachineID] int  NULL,
    [PassWord] nvarchar(50)  NULL
);
GO

-- Creating table 'CFJControl'
CREATE TABLE [dbo].[CFJControl] (
    [CFJControlID] int IDENTITY(1,1) NOT NULL,
    [AddDate] datetime  NULL,
    [DelayTime] int  NULL,
    [IsRun] int  NULL,
    [ExeTime] datetime  NULL,
    [MachineCode] varchar(50)  NULL,
    [Command] varchar(50)  NULL,
    [WXOpenID] varchar(100)  NULL,
    [CFJQrCode] varchar(50)  NULL,
    [CZIP] varchar(100)  NULL,
    [GroupCode] varchar(100)  NULL,
    [Param] varchar(100)  NULL,
    [AddMemberID] int  NULL,
    [Func] varchar(100)  NULL
);
GO

-- Creating table 'CFJMachine'
CREATE TABLE [dbo].[CFJMachine] (
    [CFJMachineID] int IDENTITY(1,1) NOT NULL,
    [MachineCode] varchar(50)  NULL,
    [AddDate] datetime  NULL,
    [MachineName] varchar(50)  NULL,
    [UniversityName] varchar(50)  NULL,
    [RoomNo] varchar(50)  NULL,
    [IP] varchar(100)  NULL,
    [CZIP] varchar(100)  NULL,
    [GroupCode] varchar(100)  NULL,
    [LastDate] datetime  NULL,
    [enabled] int  NULL,
    [DelayTime] int  NULL
);
GO

-- Creating table 'Goods'
CREATE TABLE [dbo].[Goods] (
    [GoodID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(500)  NULL,
    [Payment] decimal(8,2)  NULL,
    [TotalNum] int  NULL,
    [BuyCount] int  NULL,
    [ImgUrl] varchar(300)  NULL,
    [Intro] varchar(500)  NULL,
    [Tags] varchar(500)  NULL,
    [Desc] nvarchar(max)  NULL,
    [Status] int  NULL,
    [BannerImgUrl] varchar(300)  NULL,
    [IsShowBanner] int  NULL,
    [BannerOrder] int  NULL,
    [ViewCount] int  NULL,
    [AddDate] datetime  NULL,
    [GoodOrder] int  NULL,
    [AddMemberID] int  NULL,
    [OrgPayment] decimal(8,2)  NULL,
    [Memo] varchar(2000)  NULL,
    [GoodType] int  NULL,
    [Param] varchar(300)  NULL,
    [NeedTiLiNum] int  NULL,
    [DownPayment] decimal(8,2)  NULL,
    [ExpressFee] decimal(8,2)  NULL,
    [JoinPeopleNum] int  NULL,
    [EndDate] datetime  NULL,
    [BeginDate] datetime  NULL,
    [Payment2] decimal(8,2)  NULL,
    [DayPrice] decimal(8,2)  NULL,
    [SecurityPrice] decimal(8,2)  NULL,
    [WuLiuType] varchar(1)  NULL,
    [RefMemberID] int  NULL,
    [SellType] int  NULL,
    [XiaoQuID] int  NULL,
    [Call] varchar(300)  NULL,
    [ImgList] varchar(1000)  NULL,
    [NiceName] varchar(50)  NULL,
    [AddMemberHeadImgURL] varchar(300)  NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [OrderID] int IDENTITY(1,1) NOT NULL,
    [MemberID] int  NULL,
    [ToMemberID] int  NULL,
    [ToWeChatOpenid] varchar(50)  NULL,
    [GoodID] int  NULL,
    [AddDate] datetime  NULL,
    [BuyNum] int  NULL,
    [Payment] decimal(8,2)  NULL,
    [TotalPayment] decimal(8,2)  NULL,
    [ToName] varchar(50)  NULL,
    [ToAddress] varchar(300)  NULL,
    [ToPhone] varchar(50)  NULL,
    [ToDate] datetime  NULL,
    [PayStatus] int  NULL,
    [TranceStatus] int  NULL,
    [OrderStatus] int  NULL,
    [Memo] varchar(500)  NULL,
    [ShortUrl] varchar(20)  NULL,
    [SayEtc] varchar(1000)  NULL,
    [FromName] varchar(50)  NULL,
    [FromPhone] varchar(50)  NULL,
    [ThePass] varchar(100)  NULL,
    [DeliveryNo] varchar(50)  NULL,
    [DeliveryCompany] varchar(50)  NULL,
    [GivenStatus] int  NULL,
    [PayType] int  NULL,
    [NeedPay] decimal(8,2)  NULL,
    [DeliveryPay] decimal(8,2)  NULL,
    [SayEtcBack] varchar(1000)  NULL,
    [WxVioceMediaID] varchar(100)  NULL,
    [WxVioceUploadDate] datetime  NULL,
    [ThePassTip] varchar(1000)  NULL,
    [IsForMe] int  NULL,
    [EndDate] datetime  NULL,
    [Property] varchar(100)  NULL,
    [ToGoodSort] varchar(100)  NULL,
    [ToPriceZone] varchar(50)  NULL,
    [ToShowPrice] int  NULL,
    [PayLate] int  NULL,
    [ToSurprise] int  NULL,
    [OrderType] int  NULL,
    [ZhongChouPay] decimal(8,2)  NULL,
    [WxImageMediaID] varchar(300)  NULL,
    [RentBeginDate] datetime  NULL,
    [RentEndDate] datetime  NULL
);
GO

-- Creating table 'Members'
CREATE TABLE [dbo].[Members] (
    [MemberID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [LoginName] varchar(50)  NULL,
    [Password] varchar(50)  NULL,
    [LoginCount] int  NULL,
    [RegDate] datetime  NULL,
    [LastDate] datetime  NULL,
    [Status] int  NULL,
    [Balance] decimal(8,2)  NULL,
    [Email] varchar(100)  NULL,
    [Phone] varchar(50)  NULL,
    [NiceName] varchar(50)  NULL,
    [Sex] int  NULL,
    [Birth] datetime  NULL,
    [Balance_back] decimal(8,2)  NULL,
    [WechatOpenid] varchar(50)  NULL,
    [HeadImgUrl] varchar(300)  NULL,
    [Country] varchar(50)  NULL,
    [City] varchar(50)  NULL,
    [Province] varchar(50)  NULL,
    [TheType] int  NULL,
    [IsGuanZhu] int  NULL,
    [Unionid] varchar(100)  NULL,
    [UserLevel] int  NULL,
    [RefMemberID] int  NULL,
    [TiLiNum] int  NULL,
    [ISCFJUser] int  NULL,
    [CFJMemberTypeID] int  NULL,
    [CFJDayCount] int  NULL,
    [CFJYearCount] int  NULL,
    [Token] varchar(50)  NULL,
    [Session_key] varchar(100)  NULL,
    [XiaoQuID] int  NULL,
    [QuanIDs] varchar(500)  NULL,
    [RenZhengUrl] varchar(500)  NULL,
    [IsRenZhengPass] int  NULL,
    [RefSellerID] int  NULL,
    [IsAuthen] int  NULL,
    [RealName] varchar(50)  NULL,
    [IDCard] varchar(50)  NULL,
    [OwnerSellerID] int  NULL
);
GO

-- Creating table 'ZGGLocation'
CREATE TABLE [dbo].[ZGGLocation] (
    [ZGGLocationID] int IDENTITY(1,1) NOT NULL,
    [Latitude] float  NULL,
    [Longitude] float  NULL,
    [Precision] float  NULL,
    [CreateTime] datetime  NULL,
    [WXOpenID] varchar(50)  NULL
);
GO

-- Creating table 'ZGGBBS'
CREATE TABLE [dbo].[ZGGBBS] (
    [BBSID] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(50)  NULL,
    [Content] varchar(300)  NULL,
    [AddDate] datetime  NULL,
    [OpenID] varchar(50)  NULL,
    [ParentID] int  NULL,
    [UserID] int  NULL,
    [ImgList] varchar(500)  NULL,
    [QuanZiID] int  NULL,
    [XiaoQuID] int  NULL
);
GO

-- Creating table 'News'
CREATE TABLE [dbo].[News] (
    [NewsID] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(300)  NULL,
    [Content] nvarchar(max)  NULL,
    [AddDate] datetime  NULL,
    [UserID] int  NULL,
    [ImgURL] varchar(300)  NULL,
    [Author] varchar(30)  NULL,
    [IsPublish] int  NULL
);
GO

-- Creating table 'Apply'
CREATE TABLE [dbo].[Apply] (
    [ApplyID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Phone] varchar(50)  NULL,
    [Address] varchar(100)  NULL,
    [Good] varchar(2000)  NULL,
    [Intro] varchar(2000)  NULL,
    [MemberID] int  NULL,
    [AddDate] datetime  NULL
);
GO

-- Creating table 'Pin'
CREATE TABLE [dbo].[Pin] (
    [PinID] int IDENTITY(1,1) NOT NULL,
    [Type] varchar(50)  NULL,
    [AddDate] datetime  NULL,
    [Content] varchar(500)  NULL,
    [Call] varchar(300)  NULL,
    [MemberID] int  NULL
);
GO

-- Creating table 'OnLineVersion'
CREATE TABLE [dbo].[OnLineVersion] (
    [OnLineVersionID] int IDENTITY(1,1) NOT NULL,
    [Version] varchar(50)  NULL,
    [State] int  NULL,
    [AddDate] datetime  NULL,
    [AddMemberID] int  NULL,
    [Memo] varchar(100)  NULL
);
GO

-- Creating table 'QuanZi'
CREATE TABLE [dbo].[QuanZi] (
    [QuanZiID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [PeopleNum] int  NULL,
    [XiaoQuID] int  NULL,
    [AddDate] datetime  NULL,
    [AddMemberID] int  NULL,
    [GongGao] varchar(500)  NULL
);
GO

-- Creating table 'XiaoQu'
CREATE TABLE [dbo].[XiaoQu] (
    [XiaoQuID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [AddDate] datetime  NULL,
    [AddMemberID] int  NULL,
    [SampleName] varchar(50)  NULL,
    [Memo] varchar(300)  NULL
);
GO

-- Creating table 'GongGao'
CREATE TABLE [dbo].[GongGao] (
    [GongGaoID] int IDENTITY(1,1) NOT NULL,
    [Content] varchar(500)  NULL,
    [AddDate] datetime  NULL,
    [AddMemberID] int  NULL,
    [QuanZiID] int  NULL
);
GO

-- Creating table 'TongZhi'
CREATE TABLE [dbo].[TongZhi] (
    [TongZhiID] int IDENTITY(1,1) NOT NULL,
    [Content] varchar(500)  NULL,
    [AddDate] datetime  NULL,
    [AddMemberID] int  NULL,
    [QuanZiID] int  NULL
);
GO

-- Creating table 'ShangJia'
CREATE TABLE [dbo].[ShangJia] (
    [ShangJiaID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(300)  NULL,
    [Address] varchar(300)  NULL,
    [Call] varchar(300)  NULL,
    [Desc] varchar(2000)  NULL,
    [ImgList] varchar(3000)  NULL,
    [NeedPayQr] int  NULL,
    [ManagerName] varchar(100)  NULL,
    [ManagerCall] varchar(100)  NULL,
    [QrCodeUrl] varchar(300)  NULL,
    [ISPassed] int  NULL,
    [AddDate] datetime  NULL,
    [AddMemberID] int  NULL,
    [XiaoQuID] int  NULL,
    [ImgUrl] varchar(500)  NULL
);
GO

-- Creating table 'CarCall'
CREATE TABLE [dbo].[CarCall] (
    [CarCallID] int IDENTITY(1,1) NOT NULL,
    [CarNo] varchar(50)  NULL,
    [CarName] varchar(50)  NULL,
    [Phone] varchar(50)  NULL,
    [Name] varchar(50)  NULL,
    [ISRecWXMessage] int  NULL,
    [ISShowPhone] int  NULL,
    [ISHidePhone] int  NULL,
    [QrCodeURL] varchar(500)  NULL,
    [WXOpenID] varchar(50)  NULL,
    [MemberID] int  NULL,
    [AddDate] datetime  NULL,
    [Code] varchar(100)  NULL,
    [Memo] varchar(500)  NULL
);
GO

-- Creating table 'Account'
CREATE TABLE [dbo].[Account] (
    [AccountID] int IDENTITY(1,1) NOT NULL,
    [Phone] varchar(50)  NULL,
    [PassWord] varchar(50)  NULL,
    [ChannelID] int  NULL,
    [TheType] int  NULL,
    [State] int  NULL,
    [AUserNum] int  NULL,
    [AccEndDate] datetime  NULL,
    [VIPEndDate] datetime  NULL,
    [Payment] decimal(8,2)  NULL,
    [VUserCount] int  NULL,
    [MaxVUserCount] int  NULL
);
GO

-- Creating table 'Channel'
CREATE TABLE [dbo].[Channel] (
    [ChannelID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Desc] varchar(500)  NULL,
    [Payment] decimal(8,2)  NULL,
    [SMSTag] varchar(50)  NULL,
    [ImgURL] varchar(500)  NULL,
    [Title] varchar(100)  NULL,
    [Intro] varchar(300)  NULL,
    [IsKaMi] int  NULL
);
GO

-- Creating table 'VIPUser'
CREATE TABLE [dbo].[VIPUser] (
    [VIPUserID] int IDENTITY(1,1) NOT NULL,
    [Phone] varchar(11)  NULL,
    [AccountID] int  NULL,
    [VerifyCode] varchar(50)  NULL,
    [State] int  NULL,
    [EndDate] datetime  NULL,
    [AddDate] datetime  NULL,
    [BeginDate] datetime  NULL,
    [MemberID] int  NULL,
    [GetVerifyCodeCount] int  NULL,
    [PayState] int  NULL,
    [PayEndDate] datetime  NULL,
    [SMSState] int  NULL,
    [VerifyContent] varchar(500)  NULL
);
GO

-- Creating table 'Lottery'
CREATE TABLE [dbo].[Lottery] (
    [LotteryID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Payment] decimal(8,2)  NULL,
    [Intro] varchar(50)  NULL,
    [TheOrder] int  NULL,
    [ImgUrl] varchar(500)  NULL,
    [SellerID] int  NULL,
    [Enabled] int  NULL
);
GO

-- Creating table 'Seller'
CREATE TABLE [dbo].[Seller] (
    [SellerID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(100)  NULL,
    [Address] varchar(300)  NULL,
    [Phone] varchar(100)  NULL,
    [SellerNo] varchar(100)  NULL,
    [IsOnline] int  NULL,
    [TheOrder] int  NULL,
    [QueueNum] int  NULL,
    [Status] int  NULL,
    [VideoShowURL] varchar(500)  NULL,
    [VideoPushURL] varchar(500)  NULL,
    [ShortName] varchar(50)  NULL,
    [LastDate] datetime  NULL,
    [ManagerMemberID] int  NULL
);
GO

-- Creating table 'NCCLottery'
CREATE TABLE [dbo].[NCCLottery] (
    [NCCLotteryID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Payment] decimal(8,2)  NULL,
    [Intro] varchar(50)  NULL,
    [TheOrder] int  NULL,
    [ImgUrl] varchar(500)  NULL,
    [ImgUrl1] varchar(500)  NULL,
    [ImgUrl2] varchar(500)  NULL,
    [OrgImgUrl1] varchar(500)  NULL,
    [OrgImgUrl2] varchar(500)  NULL,
    [LotteryNo] varchar(100)  NULL,
    [PrizeStatus] int  NULL,
    [PrizeDate] datetime  NULL,
    [RefOrderID] int  NULL,
    [RefLotteryID] int  NULL,
    [Bonus] decimal(8,2)  NULL,
    [RefMemberID] int  NULL,
    [RefSellerID] int  NULL,
    [AddDate] datetime  NULL,
    [IsBeep] int  NULL,
    [BeginOpenDate] datetime  NULL,
    [EndOpenDate] datetime  NULL,
    [LotteryStatus] int  NULL,
    [PayBackReason] varchar(100)  NULL,
    [IsNotifyBuyerPrizeInfo] int  NULL,
    [NotifyBuyerPrizeInfoDate] datetime  NULL
);
GO

-- Creating table 'NCCOrders'
CREATE TABLE [dbo].[NCCOrders] (
    [OrderID] int IDENTITY(1,1) NOT NULL,
    [MemberID] int  NULL,
    [AddDate] datetime  NULL,
    [PayStatus] int  NULL,
    [OrderStatus] int  NULL,
    [Memo] varchar(300)  NULL,
    [NeedPay] decimal(8,2)  NULL,
    [EndDate] datetime  NULL,
    [OrderType] int  NULL,
    [Payment] decimal(8,2)  NULL,
    [TotalPayment] decimal(8,2)  NULL,
    [PayDate] datetime  NULL,
    [Phone] varchar(50)  NULL,
    [PlayBeginDate] datetime  NULL,
    [PlayEndDate] datetime  NULL,
    [BuyNum] int  NULL,
    [LotteryID] int  NULL,
    [Exchanged] int  NULL,
    [ExchangeDate] datetime  NULL,
    [ExchangeMemberID] int  NULL,
    [SellerID] int  NULL,
    [MemberHeadUrl] varchar(500)  NULL,
    [OrderNo] varchar(100)  NULL,
    [NCCLotteryID] int  NULL
);
GO

-- Creating table 'LotterySellerRef'
CREATE TABLE [dbo].[LotterySellerRef] (
    [LotterySellerRefID] int IDENTITY(1,1) NOT NULL,
    [LotteryID] int  NULL,
    [SellerID] int  NULL,
    [AddDate] datetime  NULL,
    [OpeMemberID] int  NULL
);
GO

-- Creating table 'KeFuMessage'
CREATE TABLE [dbo].[KeFuMessage] (
    [KeFuMessageID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Phone] varchar(50)  NULL,
    [Desc] varchar(500)  NULL,
    [FromMemberID] int  NULL,
    [AddDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AddressID] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [PK_Address]
    PRIMARY KEY CLUSTERED ([AddressID] ASC);
GO

-- Creating primary key on [GoodSortID] in table 'GoodSort'
ALTER TABLE [dbo].[GoodSort]
ADD CONSTRAINT [PK_GoodSort]
    PRIMARY KEY CLUSTERED ([GoodSortID] ASC);
GO

-- Creating primary key on [LoginLogID] in table 'LoginLog'
ALTER TABLE [dbo].[LoginLog]
ADD CONSTRAINT [PK_LoginLog]
    PRIMARY KEY CLUSTERED ([LoginLogID] ASC);
GO

-- Creating primary key on [InfoID] in table 'Info'
ALTER TABLE [dbo].[Info]
ADD CONSTRAINT [PK_Info]
    PRIMARY KEY CLUSTERED ([InfoID] ASC);
GO

-- Creating primary key on [FeedBackID] in table 'FeedBack'
ALTER TABLE [dbo].[FeedBack]
ADD CONSTRAINT [PK_FeedBack]
    PRIMARY KEY CLUSTERED ([FeedBackID] ASC);
GO

-- Creating primary key on [GetPasswordID] in table 'GetPassword'
ALTER TABLE [dbo].[GetPassword]
ADD CONSTRAINT [PK_GetPassword]
    PRIMARY KEY CLUSTERED ([GetPasswordID] ASC);
GO

-- Creating primary key on [PayLogID] in table 'PayLog'
ALTER TABLE [dbo].[PayLog]
ADD CONSTRAINT [PK_PayLog]
    PRIMARY KEY CLUSTERED ([PayLogID] ASC);
GO

-- Creating primary key on [ActionLogID] in table 'ActionLog'
ALTER TABLE [dbo].[ActionLog]
ADD CONSTRAINT [PK_ActionLog]
    PRIMARY KEY CLUSTERED ([ActionLogID] ASC);
GO

-- Creating primary key on [CommentID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([CommentID] ASC);
GO

-- Creating primary key on [GoodPropertyID] in table 'GoodProperty'
ALTER TABLE [dbo].[GoodProperty]
ADD CONSTRAINT [PK_GoodProperty]
    PRIMARY KEY CLUSTERED ([GoodPropertyID] ASC);
GO

-- Creating primary key on [MessageID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([MessageID] ASC);
GO

-- Creating primary key on [CommonLogID] in table 'CommonLog'
ALTER TABLE [dbo].[CommonLog]
ADD CONSTRAINT [PK_CommonLog]
    PRIMARY KEY CLUSTERED ([CommonLogID] ASC);
GO

-- Creating primary key on [GoodRightID] in table 'GoodRight'
ALTER TABLE [dbo].[GoodRight]
ADD CONSTRAINT [PK_GoodRight]
    PRIMARY KEY CLUSTERED ([GoodRightID] ASC);
GO

-- Creating primary key on [KuaiDiSetID] in table 'KuaiDiSet'
ALTER TABLE [dbo].[KuaiDiSet]
ADD CONSTRAINT [PK_KuaiDiSet]
    PRIMARY KEY CLUSTERED ([KuaiDiSetID] ASC);
GO

-- Creating primary key on [CommonPayID] in table 'CommonPay'
ALTER TABLE [dbo].[CommonPay]
ADD CONSTRAINT [PK_CommonPay]
    PRIMARY KEY CLUSTERED ([CommonPayID] ASC);
GO

-- Creating primary key on [CFJQrCodeID] in table 'CFJQrCode'
ALTER TABLE [dbo].[CFJQrCode]
ADD CONSTRAINT [PK_CFJQrCode]
    PRIMARY KEY CLUSTERED ([CFJQrCodeID] ASC);
GO

-- Creating primary key on [CFJMemberTypeID] in table 'CFJMemberType'
ALTER TABLE [dbo].[CFJMemberType]
ADD CONSTRAINT [PK_CFJMemberType]
    PRIMARY KEY CLUSTERED ([CFJMemberTypeID] ASC);
GO

-- Creating primary key on [CFJPayID] in table 'CFJPay'
ALTER TABLE [dbo].[CFJPay]
ADD CONSTRAINT [PK_CFJPay]
    PRIMARY KEY CLUSTERED ([CFJPayID] ASC);
GO

-- Creating primary key on [GoodSortMapping1] in table 'GoodSortMapping'
ALTER TABLE [dbo].[GoodSortMapping]
ADD CONSTRAINT [PK_GoodSortMapping]
    PRIMARY KEY CLUSTERED ([GoodSortMapping1] ASC);
GO

-- Creating primary key on [CommentID] in table 'Comments_copy'
ALTER TABLE [dbo].[Comments_copy]
ADD CONSTRAINT [PK_Comments_copy]
    PRIMARY KEY CLUSTERED ([CommentID] ASC);
GO

-- Creating primary key on [ZGGKeyID] in table 'ZGGKey'
ALTER TABLE [dbo].[ZGGKey]
ADD CONSTRAINT [PK_ZGGKey]
    PRIMARY KEY CLUSTERED ([ZGGKeyID] ASC);
GO

-- Creating primary key on [MachineID] in table 'ZGGMachine'
ALTER TABLE [dbo].[ZGGMachine]
ADD CONSTRAINT [PK_ZGGMachine]
    PRIMARY KEY CLUSTERED ([MachineID] ASC);
GO

-- Creating primary key on [ZGGPayID] in table 'ZGGPay'
ALTER TABLE [dbo].[ZGGPay]
ADD CONSTRAINT [PK_ZGGPay]
    PRIMARY KEY CLUSTERED ([ZGGPayID] ASC);
GO

-- Creating primary key on [ZGGUseControlID] in table 'ZGGUseControl'
ALTER TABLE [dbo].[ZGGUseControl]
ADD CONSTRAINT [PK_ZGGUseControl]
    PRIMARY KEY CLUSTERED ([ZGGUseControlID] ASC);
GO

-- Creating primary key on [CFJControlID] in table 'CFJControl'
ALTER TABLE [dbo].[CFJControl]
ADD CONSTRAINT [PK_CFJControl]
    PRIMARY KEY CLUSTERED ([CFJControlID] ASC);
GO

-- Creating primary key on [CFJMachineID] in table 'CFJMachine'
ALTER TABLE [dbo].[CFJMachine]
ADD CONSTRAINT [PK_CFJMachine]
    PRIMARY KEY CLUSTERED ([CFJMachineID] ASC);
GO

-- Creating primary key on [GoodID] in table 'Goods'
ALTER TABLE [dbo].[Goods]
ADD CONSTRAINT [PK_Goods]
    PRIMARY KEY CLUSTERED ([GoodID] ASC);
GO

-- Creating primary key on [OrderID] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([OrderID] ASC);
GO

-- Creating primary key on [MemberID] in table 'Members'
ALTER TABLE [dbo].[Members]
ADD CONSTRAINT [PK_Members]
    PRIMARY KEY CLUSTERED ([MemberID] ASC);
GO

-- Creating primary key on [ZGGLocationID] in table 'ZGGLocation'
ALTER TABLE [dbo].[ZGGLocation]
ADD CONSTRAINT [PK_ZGGLocation]
    PRIMARY KEY CLUSTERED ([ZGGLocationID] ASC);
GO

-- Creating primary key on [BBSID] in table 'ZGGBBS'
ALTER TABLE [dbo].[ZGGBBS]
ADD CONSTRAINT [PK_ZGGBBS]
    PRIMARY KEY CLUSTERED ([BBSID] ASC);
GO

-- Creating primary key on [NewsID] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [PK_News]
    PRIMARY KEY CLUSTERED ([NewsID] ASC);
GO

-- Creating primary key on [ApplyID] in table 'Apply'
ALTER TABLE [dbo].[Apply]
ADD CONSTRAINT [PK_Apply]
    PRIMARY KEY CLUSTERED ([ApplyID] ASC);
GO

-- Creating primary key on [PinID] in table 'Pin'
ALTER TABLE [dbo].[Pin]
ADD CONSTRAINT [PK_Pin]
    PRIMARY KEY CLUSTERED ([PinID] ASC);
GO

-- Creating primary key on [OnLineVersionID] in table 'OnLineVersion'
ALTER TABLE [dbo].[OnLineVersion]
ADD CONSTRAINT [PK_OnLineVersion]
    PRIMARY KEY CLUSTERED ([OnLineVersionID] ASC);
GO

-- Creating primary key on [QuanZiID] in table 'QuanZi'
ALTER TABLE [dbo].[QuanZi]
ADD CONSTRAINT [PK_QuanZi]
    PRIMARY KEY CLUSTERED ([QuanZiID] ASC);
GO

-- Creating primary key on [XiaoQuID] in table 'XiaoQu'
ALTER TABLE [dbo].[XiaoQu]
ADD CONSTRAINT [PK_XiaoQu]
    PRIMARY KEY CLUSTERED ([XiaoQuID] ASC);
GO

-- Creating primary key on [GongGaoID] in table 'GongGao'
ALTER TABLE [dbo].[GongGao]
ADD CONSTRAINT [PK_GongGao]
    PRIMARY KEY CLUSTERED ([GongGaoID] ASC);
GO

-- Creating primary key on [TongZhiID] in table 'TongZhi'
ALTER TABLE [dbo].[TongZhi]
ADD CONSTRAINT [PK_TongZhi]
    PRIMARY KEY CLUSTERED ([TongZhiID] ASC);
GO

-- Creating primary key on [ShangJiaID] in table 'ShangJia'
ALTER TABLE [dbo].[ShangJia]
ADD CONSTRAINT [PK_ShangJia]
    PRIMARY KEY CLUSTERED ([ShangJiaID] ASC);
GO

-- Creating primary key on [CarCallID] in table 'CarCall'
ALTER TABLE [dbo].[CarCall]
ADD CONSTRAINT [PK_CarCall]
    PRIMARY KEY CLUSTERED ([CarCallID] ASC);
GO

-- Creating primary key on [AccountID] in table 'Account'
ALTER TABLE [dbo].[Account]
ADD CONSTRAINT [PK_Account]
    PRIMARY KEY CLUSTERED ([AccountID] ASC);
GO

-- Creating primary key on [ChannelID] in table 'Channel'
ALTER TABLE [dbo].[Channel]
ADD CONSTRAINT [PK_Channel]
    PRIMARY KEY CLUSTERED ([ChannelID] ASC);
GO

-- Creating primary key on [VIPUserID] in table 'VIPUser'
ALTER TABLE [dbo].[VIPUser]
ADD CONSTRAINT [PK_VIPUser]
    PRIMARY KEY CLUSTERED ([VIPUserID] ASC);
GO

-- Creating primary key on [LotteryID] in table 'Lottery'
ALTER TABLE [dbo].[Lottery]
ADD CONSTRAINT [PK_Lottery]
    PRIMARY KEY CLUSTERED ([LotteryID] ASC);
GO

-- Creating primary key on [SellerID] in table 'Seller'
ALTER TABLE [dbo].[Seller]
ADD CONSTRAINT [PK_Seller]
    PRIMARY KEY CLUSTERED ([SellerID] ASC);
GO

-- Creating primary key on [NCCLotteryID] in table 'NCCLottery'
ALTER TABLE [dbo].[NCCLottery]
ADD CONSTRAINT [PK_NCCLottery]
    PRIMARY KEY CLUSTERED ([NCCLotteryID] ASC);
GO

-- Creating primary key on [OrderID] in table 'NCCOrders'
ALTER TABLE [dbo].[NCCOrders]
ADD CONSTRAINT [PK_NCCOrders]
    PRIMARY KEY CLUSTERED ([OrderID] ASC);
GO

-- Creating primary key on [LotterySellerRefID] in table 'LotterySellerRef'
ALTER TABLE [dbo].[LotterySellerRef]
ADD CONSTRAINT [PK_LotterySellerRef]
    PRIMARY KEY CLUSTERED ([LotterySellerRefID] ASC);
GO

-- Creating primary key on [KeFuMessageID] in table 'KeFuMessage'
ALTER TABLE [dbo].[KeFuMessage]
ADD CONSTRAINT [PK_KeFuMessage]
    PRIMARY KEY CLUSTERED ([KeFuMessageID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------