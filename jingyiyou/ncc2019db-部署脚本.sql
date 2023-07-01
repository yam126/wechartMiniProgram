USE [master]
GO
/****** Object:  Database [ncc2019db]    Script Date: 2023/6/24 20:16:31 ******/
CREATE DATABASE [ncc2019db] ON  PRIMARY 
( NAME = N'ncc2019db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\ncc2019db.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ncc2019db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\ncc2019db_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ncc2019db] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ncc2019db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ncc2019db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ncc2019db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ncc2019db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ncc2019db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ncc2019db] SET ARITHABORT OFF 
GO
ALTER DATABASE [ncc2019db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ncc2019db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ncc2019db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ncc2019db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ncc2019db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ncc2019db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ncc2019db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ncc2019db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ncc2019db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ncc2019db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ncc2019db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ncc2019db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ncc2019db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ncc2019db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ncc2019db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ncc2019db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ncc2019db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ncc2019db] SET RECOVERY FULL 
GO
ALTER DATABASE [ncc2019db] SET  MULTI_USER 
GO
ALTER DATABASE [ncc2019db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ncc2019db] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ncc2019db', N'ON'
GO
USE [ncc2019db]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [varchar](50) NULL,
	[PassWord] [varchar](50) NULL,
	[ChannelID] [int] NULL,
	[TheType] [int] NULL,
	[State] [int] NULL,
	[AUserNum] [int] NULL,
	[AccEndDate] [datetime] NULL,
	[VIPEndDate] [datetime] NULL,
	[Payment] [decimal](8, 2) NULL,
	[VUserCount] [int] NULL,
	[MaxVUserCount] [int] NULL,
	[RegistDate] [datetime] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActionLog]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionLog](
	[ActionLogID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NULL,
	[OrderID] [int] NULL,
	[ActionDate] [datetime] NULL,
	[MemberID] [int] NULL,
	[AtionType] [int] NULL,
 CONSTRAINT [PK_ActionLog] PRIMARY KEY CLUSTERED 
(
	[ActionLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[Address1] [varchar](100) NULL,
	[ZipCode] [varchar](10) NULL,
	[RecName] [varchar](50) NULL,
	[Memo] [varchar](300) NULL,
	[AddDate] [datetime] NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Apply]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Apply](
	[ApplyID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Address] [varchar](100) NULL,
	[Good] [varchar](2000) NULL,
	[Intro] [varchar](2000) NULL,
	[MemberID] [int] NULL,
	[AddDate] [datetime] NULL,
 CONSTRAINT [PK_Apply] PRIMARY KEY CLUSTERED 
(
	[ApplyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarCall]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarCall](
	[CarCallID] [int] IDENTITY(1,1) NOT NULL,
	[CarNo] [varchar](50) NULL,
	[CarName] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[ISRecWXMessage] [int] NULL,
	[ISShowPhone] [int] NULL,
	[ISHidePhone] [int] NULL,
	[QrCodeURL] [varchar](500) NULL,
	[WXOpenID] [varchar](50) NULL,
	[MemberID] [int] NULL,
	[AddDate] [datetime] NULL,
	[Code] [varchar](100) NULL,
	[Memo] [varchar](500) NULL,
 CONSTRAINT [PK_CarCall] PRIMARY KEY CLUSTERED 
(
	[CarCallID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CFJControl]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CFJControl](
	[CFJControlID] [int] IDENTITY(1,1) NOT NULL,
	[AddDate] [datetime] NULL,
	[DelayTime] [int] NULL,
	[IsRun] [int] NULL,
	[ExeTime] [datetime] NULL,
	[MachineCode] [varchar](50) NULL,
	[Command] [varchar](50) NULL,
	[WXOpenID] [varchar](100) NULL,
	[CFJQrCode] [varchar](50) NULL,
	[CZIP] [varchar](100) NULL,
	[GroupCode] [varchar](100) NULL,
	[Param] [varchar](100) NULL,
	[AddMemberID] [int] NULL,
	[Func] [varchar](100) NULL,
 CONSTRAINT [PK_CFJControl] PRIMARY KEY CLUSTERED 
(
	[CFJControlID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CFJMachine]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CFJMachine](
	[CFJMachineID] [int] IDENTITY(1,1) NOT NULL,
	[MachineCode] [varchar](50) NULL,
	[AddDate] [datetime] NULL,
	[MachineName] [varchar](50) NULL,
	[UniversityName] [varchar](50) NULL,
	[RoomNo] [varchar](50) NULL,
	[IP] [varchar](100) NULL,
	[CZIP] [varchar](100) NULL,
	[GroupCode] [varchar](100) NULL,
	[LastDate] [datetime] NULL,
	[enabled] [int] NULL,
	[DelayTime] [int] NULL,
 CONSTRAINT [PK_CFJMachine] PRIMARY KEY CLUSTERED 
(
	[CFJMachineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CFJMemberType]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CFJMemberType](
	[CFJMemberTypeID] [int] IDENTITY(1,1) NOT NULL,
	[MemberType] [int] NULL,
	[DelayTime] [int] NULL,
	[DayCount] [int] NULL,
	[YearCount] [int] NULL,
	[Name] [varchar](50) NULL,
	[MonthCount] [int] NULL,
	[Payment] [decimal](18, 2) NULL,
	[Memo] [varchar](100) NULL,
	[IsPublic] [int] NULL,
 CONSTRAINT [PK_CFJMemberType] PRIMARY KEY CLUSTERED 
(
	[CFJMemberTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CFJPay]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CFJPay](
	[CFJPayID] [int] IDENTITY(1,1) NOT NULL,
	[Payment] [decimal](8, 2) NULL,
	[MemberID] [int] NULL,
	[MemberTypeID] [int] NULL,
	[State] [int] NULL,
	[AddDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[NeedPay] [decimal](8, 2) NULL,
	[HeadImgUrl] [varchar](300) NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_CFJPay] PRIMARY KEY CLUSTERED 
(
	[CFJPayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CFJQrCode]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CFJQrCode](
	[CFJQrCodeID] [int] NOT NULL,
	[Code] [varchar](100) NULL,
	[AddDate] [datetime] NULL,
	[IsUsed] [int] NULL,
	[UsedDate] [datetime] NULL,
	[MachineCode] [varchar](50) NULL,
	[GroupCode] [varchar](100) NULL,
 CONSTRAINT [PK_CFJQrCode] PRIMARY KEY CLUSTERED 
(
	[CFJQrCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Channel]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Channel](
	[ChannelID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Desc] [varchar](500) NULL,
	[Payment] [decimal](8, 2) NULL,
	[SMSTag] [varchar](50) NULL,
	[ImgURL] [varchar](500) NULL,
	[Title] [varchar](100) NULL,
	[Intro] [varchar](300) NULL,
	[IsKaMi] [int] NULL,
 CONSTRAINT [PK_Channel] PRIMARY KEY CLUSTERED 
(
	[ChannelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[AddDate] [datetime] NULL,
	[MemberID] [int] NULL,
	[Content] [varchar](1000) NULL,
	[WxVioceMediaID] [varchar](100) NULL,
	[WxVioceUploadDate] [datetime] NULL,
	[Name] [varchar](50) NULL,
	[HeadImgUrl] [varchar](300) NULL,
	[ToMemberID] [int] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments_copy]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments_copy](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[AddDate] [datetime] NULL,
	[MemberID] [int] NULL,
	[Content] [varchar](1000) NULL,
	[WxVioceMediaID] [varchar](100) NULL,
	[WxVioceUploadDate] [datetime] NULL,
	[Name] [varchar](50) NULL,
	[HeadImgUrl] [varchar](300) NULL,
	[ToMemberID] [int] NULL,
 CONSTRAINT [PK_Comments_copy] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommonLog]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommonLog](
	[CommonLogID] [int] IDENTITY(1,1) NOT NULL,
	[Agent] [varchar](500) NULL,
	[Content] [varchar](500) NULL,
	[AddDate] [datetime] NULL,
 CONSTRAINT [PK_CommonLog] PRIMARY KEY CLUSTERED 
(
	[CommonLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommonPay]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommonPay](
	[Payment] [decimal](8, 2) NULL,
	[MemberID] [int] NULL,
	[AddDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[PayType] [int] NULL,
	[NeedPay] [decimal](8, 2) NULL,
	[HeadImgUrl] [varchar](300) NULL,
	[Name] [varchar](50) NULL,
	[CommonPayID] [int] IDENTITY(1,1) NOT NULL,
	[GoodID] [int] NULL,
	[PayDirection] [int] NULL,
	[PayStatus] [int] NULL,
	[RefOrderID] [int] NULL,
	[TotalPayment] [decimal](8, 2) NULL,
	[PayDate] [datetime] NULL,
 CONSTRAINT [PK_CommonPay] PRIMARY KEY CLUSTERED 
(
	[CommonPayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeedBack]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedBack](
	[FeedBackID] [int] IDENTITY(1,1) NOT NULL,
	[TheContent] [varchar](2000) NULL,
	[AddDate] [datetime] NULL,
	[OperID] [int] NULL,
 CONSTRAINT [PK_FeedBack] PRIMARY KEY CLUSTERED 
(
	[FeedBackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GetPassword]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GetPassword](
	[GetPasswordID] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [varchar](100) NULL,
	[RefEmail] [varchar](100) NULL,
	[BeginDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[GetDate] [datetime] NULL,
 CONSTRAINT [PK_GetPassword] PRIMARY KEY CLUSTERED 
(
	[GetPasswordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GongGao]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GongGao](
	[GongGaoID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [varchar](500) NULL,
	[AddDate] [datetime] NULL,
	[AddMemberID] [int] NULL,
	[QuanZiID] [int] NULL,
 CONSTRAINT [PK_GongGao] PRIMARY KEY CLUSTERED 
(
	[GongGaoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoodProperty]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoodProperty](
	[GoodPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Content] [varchar](500) NULL,
	[AddDate] [datetime] NULL,
	[GoodID] [int] NULL,
	[AddMemberID] [int] NULL,
 CONSTRAINT [PK_GoodProperty] PRIMARY KEY CLUSTERED 
(
	[GoodPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoodRight]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoodRight](
	[GoodRightID] [int] IDENTITY(1,1) NOT NULL,
	[GoodID] [int] NULL,
	[MemberID] [int] NULL,
	[BeginDate] [datetime] NULL,
	[Param] [varchar](200) NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_GoodRight] PRIMARY KEY CLUSTERED 
(
	[GoodRightID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Goods]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Goods](
	[GoodID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NULL,
	[Payment] [decimal](8, 2) NULL,
	[TotalNum] [int] NULL,
	[BuyCount] [int] NULL,
	[ImgUrl] [varchar](300) NULL,
	[Intro] [varchar](500) NULL,
	[Tags] [varchar](500) NULL,
	[Desc] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[BannerImgUrl] [varchar](300) NULL,
	[IsShowBanner] [int] NULL,
	[BannerOrder] [int] NULL,
	[ViewCount] [int] NULL,
	[AddDate] [datetime] NULL,
	[GoodOrder] [int] NULL,
	[AddMemberID] [int] NULL,
	[OrgPayment] [decimal](8, 2) NULL,
	[Memo] [varchar](2000) NULL,
	[GoodType] [int] NULL,
	[Param] [varchar](300) NULL,
	[NeedTiLiNum] [int] NULL,
	[DownPayment] [decimal](8, 2) NULL,
	[ExpressFee] [decimal](8, 2) NULL,
	[JoinPeopleNum] [int] NULL,
	[EndDate] [datetime] NULL,
	[BeginDate] [datetime] NULL,
	[Payment2] [decimal](8, 2) NULL,
	[DayPrice] [decimal](8, 2) NULL,
	[SecurityPrice] [decimal](8, 2) NULL,
	[WuLiuType] [varchar](1) NULL,
	[RefMemberID] [int] NULL,
	[SellType] [int] NULL,
	[XiaoQuID] [int] NULL,
	[Call] [varchar](300) NULL,
	[ImgList] [varchar](1000) NULL,
	[NiceName] [varchar](50) NULL,
	[AddMemberHeadImgURL] [varchar](300) NULL,
 CONSTRAINT [PK_Goods] PRIMARY KEY CLUSTERED 
(
	[GoodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoodSort]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoodSort](
	[GoodSortID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[ParentID] [int] NULL,
	[AddDate] [datetime] NULL,
	[Enabled] [int] NULL,
	[GoodSortOrder] [int] NULL,
	[ShowInIndex] [int] NULL,
	[ShowName] [varchar](50) NULL,
	[AddMemberID] [int] NULL,
 CONSTRAINT [PK_GoodSort] PRIMARY KEY CLUSTERED 
(
	[GoodSortID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoodSortMapping]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoodSortMapping](
	[GoodSortMapping1] [int] IDENTITY(1,1) NOT NULL,
	[GoodID] [int] NULL,
	[GoodSortID] [int] NULL,
 CONSTRAINT [PK_GoodSortMapping] PRIMARY KEY CLUSTERED 
(
	[GoodSortMapping1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Guide]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guide](
	[GuideID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[GuideNo] [varchar](100) NULL,
	[Memo] [varchar](500) NULL,
	[Status] [int] NULL,
	[Skills] [varchar](1000) NULL,
	[ImageUrl] [varchar](1000) NULL,
	[Payment] [decimal](18, 2) NULL,
	[StartLevel] [float] NULL,
	[Sex] [int] NULL,
	[Times] [int] NULL,
	[BusyStatus] [int] NULL,
	[HeadUrl] [varchar](1000) NULL,
	[VideoShowUrl] [varchar](1000) NULL,
	[SkillPoints] [varchar](1000) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[IDCard] [varchar](50) NULL,
	[Intorduction] [varchar](300) NULL,
 CONSTRAINT [PK__Guide__E77EE03E5EBF139D] PRIMARY KEY CLUSTERED 
(
	[GuideID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Info]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Info](
	[InfoID] [int] IDENTITY(1,1) NOT NULL,
	[TheContent] [varchar](max) NULL,
	[AddDate] [datetime] NULL,
	[OpeUserID] [int] NULL,
	[Title] [varchar](500) NULL,
	[UpeDate] [datetime] NULL,
	[ViewCount] [int] NULL,
	[ShareImgUrl] [varchar](300) NULL,
	[ShareTitle] [varchar](100) NULL,
	[ShareIntro] [varchar](500) NULL,
	[IsShowInBanner] [int] NULL,
	[BannerImgUrl] [varchar](300) NULL,
 CONSTRAINT [PK_Info] PRIMARY KEY CLUSTERED 
(
	[InfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeFuMessage]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeFuMessage](
	[KeFuMessageID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Desc] [varchar](500) NULL,
	[FromMemberID] [int] NULL,
	[AddDate] [datetime] NULL,
 CONSTRAINT [PK_KeFuMessage] PRIMARY KEY CLUSTERED 
(
	[KeFuMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KuaiDiSet]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KuaiDiSet](
	[KuaiDiSetID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](100) NULL,
	[CompanyCode] [varchar](100) NULL,
	[AddDate] [datetime] NULL,
	[MemberID] [int] NULL,
 CONSTRAINT [PK_KuaiDiSet] PRIMARY KEY CLUSTERED 
(
	[KuaiDiSetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoginLog]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginLog](
	[LoginLogID] [int] IDENTITY(1,1) NOT NULL,
	[LoginTime] [datetime] NULL,
	[IP] [varchar](100) NULL,
	[Agent] [varchar](1000) NULL,
	[SystemInfo] [varchar](100) NULL,
	[MemberID] [int] NULL,
	[RefMemberID] [int] NULL,
 CONSTRAINT [PK_LoginLog] PRIMARY KEY CLUSTERED 
(
	[LoginLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lottery]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lottery](
	[LotteryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Payment] [decimal](8, 2) NULL,
	[Intro] [varchar](50) NULL,
	[TheOrder] [int] NULL,
	[ImgUrl] [varchar](500) NULL,
	[SellerID] [int] NULL,
	[Enabled] [int] NULL,
 CONSTRAINT [PK_Lottery] PRIMARY KEY CLUSTERED 
(
	[LotteryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LotterySellerRef]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LotterySellerRef](
	[LotterySellerRefID] [int] IDENTITY(1,1) NOT NULL,
	[LotteryID] [int] NULL,
	[SellerID] [int] NULL,
	[AddDate] [datetime] NULL,
	[OpeMemberID] [int] NULL,
 CONSTRAINT [PK_LotterySellerRef] PRIMARY KEY CLUSTERED 
(
	[LotterySellerRefID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[MemberID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[LoginName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[LoginCount] [int] NULL,
	[RegDate] [datetime] NULL,
	[LastDate] [datetime] NULL,
	[Status] [int] NULL,
	[Balance] [decimal](8, 2) NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](50) NULL,
	[NiceName] [varchar](50) NULL,
	[Sex] [int] NULL,
	[Birth] [datetime] NULL,
	[Balance_back] [decimal](8, 2) NULL,
	[WechatOpenid] [varchar](50) NULL,
	[HeadImgUrl] [varchar](300) NULL,
	[Country] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[Province] [varchar](50) NULL,
	[TheType] [int] NULL,
	[IsGuanZhu] [int] NULL,
	[Unionid] [varchar](100) NULL,
	[UserLevel] [int] NULL,
	[RefMemberID] [int] NULL,
	[TiLiNum] [int] NULL,
	[ISCFJUser] [int] NULL,
	[CFJMemberTypeID] [int] NULL,
	[CFJDayCount] [int] NULL,
	[CFJYearCount] [int] NULL,
	[Token] [varchar](50) NULL,
	[Session_key] [varchar](100) NULL,
	[XiaoQuID] [int] NULL,
	[QuanIDs] [varchar](500) NULL,
	[RenZhengUrl] [varchar](500) NULL,
	[IsRenZhengPass] [int] NULL,
	[RefSellerID] [int] NULL,
	[IsAuthen] [int] NULL,
	[RealName] [varchar](50) NULL,
	[IDCard] [varchar](50) NULL,
	[OwnerSellerID] [int] NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[FromMemberID] [int] NULL,
	[ToMenberID] [int] NULL,
	[Content] [nvarchar](max) NULL,
	[AddDate] [datetime] NULL,
	[FromName] [varchar](50) NULL,
	[FromImgUrl] [varchar](300) NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NCCLottery]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NCCLottery](
	[NCCLotteryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Payment] [decimal](8, 2) NULL,
	[Intro] [varchar](50) NULL,
	[TheOrder] [int] NULL,
	[ImgUrl] [varchar](500) NULL,
	[ImgUrl1] [varchar](500) NULL,
	[ImgUrl2] [varchar](500) NULL,
	[OrgImgUrl1] [varchar](500) NULL,
	[OrgImgUrl2] [varchar](500) NULL,
	[LotteryNo] [varchar](100) NULL,
	[PrizeStatus] [int] NULL,
	[PrizeDate] [datetime] NULL,
	[RefOrderID] [int] NULL,
	[RefLotteryID] [int] NULL,
	[Bonus] [decimal](8, 2) NULL,
	[RefMemberID] [int] NULL,
	[RefSellerID] [int] NULL,
	[AddDate] [datetime] NULL,
	[IsBeep] [int] NULL,
	[BeginOpenDate] [datetime] NULL,
	[EndOpenDate] [datetime] NULL,
	[LotteryStatus] [int] NULL,
	[PayBackReason] [varchar](100) NULL,
	[IsNotifyBuyerPrizeInfo] [int] NULL,
	[NotifyBuyerPrizeInfoDate] [datetime] NULL,
 CONSTRAINT [PK_NCCLottery] PRIMARY KEY CLUSTERED 
(
	[NCCLotteryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NCCOrders]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NCCOrders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NULL,
	[AddDate] [datetime] NULL,
	[PayStatus] [int] NULL,
	[OrderStatus] [int] NULL,
	[Memo] [varchar](300) NULL,
	[NeedPay] [decimal](8, 2) NULL,
	[EndDate] [datetime] NULL,
	[OrderType] [int] NULL,
	[Payment] [decimal](8, 2) NULL,
	[TotalPayment] [decimal](8, 2) NULL,
	[PayDate] [datetime] NULL,
	[Phone] [varchar](50) NULL,
	[PlayBeginDate] [datetime] NULL,
	[PlayEndDate] [datetime] NULL,
	[BuyNum] [int] NULL,
	[LotteryID] [int] NULL,
	[Exchanged] [int] NULL,
	[ExchangeDate] [datetime] NULL,
	[ExchangeMemberID] [int] NULL,
	[SellerID] [int] NULL,
	[MemberHeadUrl] [varchar](500) NULL,
	[OrderNo] [varchar](100) NULL,
	[NCCLotteryID] [int] NULL,
 CONSTRAINT [PK_NCCOrders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[NewsID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](300) NULL,
	[Content] [nvarchar](max) NULL,
	[AddDate] [datetime] NULL,
	[UserID] [int] NULL,
	[ImgURL] [varchar](300) NULL,
	[Author] [varchar](30) NULL,
	[IsPublish] [int] NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[NewsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OnLineVersion]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OnLineVersion](
	[OnLineVersionID] [int] IDENTITY(1,1) NOT NULL,
	[Version] [varchar](50) NULL,
	[State] [int] NULL,
	[AddDate] [datetime] NULL,
	[AddMemberID] [int] NULL,
	[Memo] [varchar](100) NULL,
 CONSTRAINT [PK_OnLineVersion] PRIMARY KEY CLUSTERED 
(
	[OnLineVersionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NULL,
	[GuideID] [int] NULL,
	[MemberOpenid] [varchar](100) NULL,
	[ServiceMinute] [int] NULL,
	[AddDate] [datetime] NULL,
	[Payment] [decimal](8, 2) NULL,
	[TotalPayment] [decimal](8, 2) NULL,
	[PayStatus] [int] NULL,
	[OrderStatus] [int] NULL,
	[Memo] [varchar](500) NULL,
	[ShortUrl] [varchar](20) NULL,
	[PayType] [int] NULL,
	[NeedPay] [decimal](8, 2) NULL,
	[EndDate] [datetime] NULL,
	[Property] [varchar](100) NULL,
	[OrderType] [int] NULL,
	[GuideOpenid] [varchar](100) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayLog]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayLog](
	[PayLogID] [int] IDENTITY(1,1) NOT NULL,
	[Payment] [decimal](8, 2) NULL,
	[InDate] [datetime] NULL,
	[MemberID] [int] NULL,
	[PayDirection] [int] NULL,
	[RefOrderID] [int] NULL,
	[Param] [varchar](50) NULL,
 CONSTRAINT [PK_PayLog] PRIMARY KEY CLUSTERED 
(
	[PayLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pin]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pin](
	[PinID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NULL,
	[AddDate] [datetime] NULL,
	[Content] [varchar](500) NULL,
	[Call] [varchar](300) NULL,
	[MemberID] [int] NULL,
 CONSTRAINT [PK_Pin] PRIMARY KEY CLUSTERED 
(
	[PinID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuanZi]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuanZi](
	[QuanZiID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[PeopleNum] [int] NULL,
	[XiaoQuID] [int] NULL,
	[AddDate] [datetime] NULL,
	[AddMemberID] [int] NULL,
	[GongGao] [varchar](500) NULL,
 CONSTRAINT [PK_QuanZi] PRIMARY KEY CLUSTERED 
(
	[QuanZiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seller]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seller](
	[SellerID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Address] [varchar](300) NULL,
	[Phone] [varchar](100) NULL,
	[SellerNo] [varchar](100) NULL,
	[IsOnline] [int] NULL,
	[TheOrder] [int] NULL,
	[QueueNum] [int] NULL,
	[Status] [int] NULL,
	[VideoShowURL] [varchar](500) NULL,
	[VideoPushURL] [varchar](500) NULL,
	[ShortName] [varchar](50) NULL,
	[LastDate] [datetime] NULL,
	[ManagerMemberID] [int] NULL,
 CONSTRAINT [PK_Seller] PRIMARY KEY CLUSTERED 
(
	[SellerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShangJia]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShangJia](
	[ShangJiaID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](300) NULL,
	[Address] [varchar](300) NULL,
	[Call] [varchar](300) NULL,
	[Desc] [varchar](2000) NULL,
	[ImgList] [varchar](3000) NULL,
	[NeedPayQr] [int] NULL,
	[ManagerName] [varchar](100) NULL,
	[ManagerCall] [varchar](100) NULL,
	[QrCodeUrl] [varchar](300) NULL,
	[ISPassed] [int] NULL,
	[AddDate] [datetime] NULL,
	[AddMemberID] [int] NULL,
	[XiaoQuID] [int] NULL,
	[ImgUrl] [varchar](500) NULL,
 CONSTRAINT [PK_ShangJia] PRIMARY KEY CLUSTERED 
(
	[ShangJiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TongZhi]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TongZhi](
	[TongZhiID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [varchar](500) NULL,
	[AddDate] [datetime] NULL,
	[AddMemberID] [int] NULL,
	[QuanZiID] [int] NULL,
 CONSTRAINT [PK_TongZhi] PRIMARY KEY CLUSTERED 
(
	[TongZhiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VIPUser]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VIPUser](
	[VIPUserID] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [varchar](11) NULL,
	[AccountID] [int] NULL,
	[VerifyCode] [varchar](50) NULL,
	[State] [int] NULL,
	[EndDate] [datetime] NULL,
	[AddDate] [datetime] NULL,
	[BeginDate] [datetime] NULL,
	[MemberID] [int] NULL,
	[GetVerifyCodeCount] [int] NULL,
	[PayState] [int] NULL,
	[PayEndDate] [datetime] NULL,
	[SMSState] [int] NULL,
	[VerifyContent] [varchar](500) NULL,
 CONSTRAINT [PK_VIPUser] PRIMARY KEY CLUSTERED 
(
	[VIPUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XiaoQu]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XiaoQu](
	[XiaoQuID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[AddDate] [datetime] NULL,
	[AddMemberID] [int] NULL,
	[SampleName] [varchar](50) NULL,
	[Memo] [varchar](300) NULL,
 CONSTRAINT [PK_XiaoQu] PRIMARY KEY CLUSTERED 
(
	[XiaoQuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZGGBBS]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZGGBBS](
	[BBSID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NULL,
	[Content] [varchar](300) NULL,
	[AddDate] [datetime] NULL,
	[OpenID] [varchar](50) NULL,
	[ParentID] [int] NULL,
	[UserID] [int] NULL,
	[ImgList] [varchar](500) NULL,
	[QuanZiID] [int] NULL,
	[XiaoQuID] [int] NULL,
 CONSTRAINT [PK_ZGGBBS] PRIMARY KEY CLUSTERED 
(
	[BBSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZGGKey]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZGGKey](
	[ZGGKeyID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](50) NULL,
	[OwnerWXID] [varchar](50) NULL,
	[UserWXID] [varchar](50) NULL,
	[AddDate] [datetime] NULL,
	[UseDate] [datetime] NULL,
	[AccpteDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ZGGMachineID] [int] NULL,
 CONSTRAINT [PK_ZGGKey] PRIMARY KEY CLUSTERED 
(
	[ZGGKeyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZGGLocation]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZGGLocation](
	[ZGGLocationID] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Precision] [float] NULL,
	[CreateTime] [datetime] NULL,
	[WXOpenID] [varchar](50) NULL,
 CONSTRAINT [PK_ZGGLocation] PRIMARY KEY CLUSTERED 
(
	[ZGGLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZGGMachine]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZGGMachine](
	[MachineID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[ShowCode] [varchar](20) NULL,
	[BackCode] [varchar](50) NULL,
	[WxOpenID] [varchar](50) NULL,
	[QrUrl] [varchar](500) NULL,
	[CreateDate] [datetime] NULL,
	[Address] [varchar](200) NULL,
	[Phone] [varchar](50) NULL,
	[OwnerName] [varchar](50) NULL,
	[IsShared] [int] NULL,
	[IsLocked] [int] NULL,
	[OpenUserID] [int] NULL,
	[PassWord] [varchar](50) NULL,
	[GuiGe] [varchar](50) NULL,
	[UserPayment] [decimal](8, 2) NULL,
	[CompanyPayment] [decimal](8, 2) NULL,
	[StrategyPayment] [int] NULL,
	[ZGGUseControlID] [int] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Precision] [float] NULL,
 CONSTRAINT [PK_ZGGMachine] PRIMARY KEY CLUSTERED 
(
	[MachineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZGGPay]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZGGPay](
	[ZGGPayID] [int] IDENTITY(1,1) NOT NULL,
	[Payment] [decimal](8, 2) NULL,
	[MemberID] [int] NULL,
	[MemberTypeID] [int] NULL,
	[State] [int] NULL,
	[AddDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[NeedPay] [decimal](8, 2) NULL,
	[HeadImgUrl] [varchar](300) NULL,
	[Name] [varchar](50) NULL,
	[UseControlID] [int] NULL,
 CONSTRAINT [PK_ZGGPay] PRIMARY KEY CLUSTERED 
(
	[ZGGPayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZGGUseControl]    Script Date: 2023/6/24 20:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZGGUseControl](
	[ZGGUseControlID] [int] IDENTITY(1,1) NOT NULL,
	[NeighborUserID] [int] NULL,
	[PutInDate] [datetime] NULL,
	[KDUserID] [int] NULL,
	[GetOutDate] [datetime] NULL,
	[IsPrivate] [int] NULL,
	[PaymentType] [int] NULL,
	[Payed] [decimal](8, 2) NULL,
	[PayState] [int] NULL,
	[ZGGPayID] [int] NULL,
	[ZGGMachineID] [int] NULL,
	[PassWord] [nvarchar](50) NULL,
 CONSTRAINT [PK_ZGGUseControl] PRIMARY KEY CLUSTERED 
(
	[ZGGUseControlID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 
GO
INSERT [dbo].[Account] ([AccountID], [Phone], [PassWord], [ChannelID], [TheType], [State], [AUserNum], [AccEndDate], [VIPEndDate], [Payment], [VUserCount], [MaxVUserCount], [RegistDate]) VALUES (1, N'136123456789', N'123123', 1, 1, 1, 1, CAST(N'2023-06-22T12:59:08.000' AS DateTime), CAST(N'2023-06-22T12:59:11.000' AS DateTime), CAST(111.00 AS Decimal(8, 2)), 11, 11, NULL)
GO
INSERT [dbo].[Account] ([AccountID], [Phone], [PassWord], [ChannelID], [TheType], [State], [AUserNum], [AccEndDate], [VIPEndDate], [Payment], [VUserCount], [MaxVUserCount], [RegistDate]) VALUES (2, N'136123456189', N'123123', 1, 1, 1, 1, CAST(N'2023-06-22T12:59:45.000' AS DateTime), CAST(N'2023-06-22T12:59:11.000' AS DateTime), CAST(111.00 AS Decimal(8, 2)), 11, 11, NULL)
GO
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[CommonPay] ON 
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 2, CAST(N'2023-06-10T15:01:52.593' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'18600527610', 1, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 4, CAST(N'2023-06-11T12:19:48.437' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/TmXRGRVvfTI2l86PnLZs4hnDhRLfuh3LKZ4FMYlz3VE7sfJPuxMfWHj6mrsuBiazKpZIjXge5cWD0icicLZZyOA5w/132', N'超_越梦想', 2, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:19:49.730' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 3, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:31:56.120' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 4, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:00.857' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 5, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:01.947' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 6, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:03.057' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 7, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:04.150' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 8, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:05.260' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 9, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:07.667' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 10, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:08.197' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 11, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:09.637' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 12, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:10.150' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 13, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:11.620' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 14, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:13.150' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 15, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:18.040' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 16, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:18.833' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 17, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:20.180' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 18, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:22.813' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 19, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:27.570' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 20, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:29.010' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 21, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:31.353' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 22, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:32.150' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 23, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:32.977' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 24, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:39.150' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 25, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:49.133' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 26, NULL, 2, 2, NULL, NULL, NULL)
GO
INSERT [dbo].[CommonPay] ([Payment], [MemberID], [AddDate], [EndDate], [PayType], [NeedPay], [HeadImgUrl], [Name], [CommonPayID], [GoodID], [PayDirection], [PayStatus], [RefOrderID], [TotalPayment], [PayDate]) VALUES (CAST(5000.00 AS Decimal(8, 2)), 3, CAST(N'2023-06-11T12:32:49.477' AS DateTime), NULL, 2, CAST(5000.00 AS Decimal(8, 2)), N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'母笑阳', 27, NULL, 2, 2, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[CommonPay] OFF
GO
SET IDENTITY_INSERT [dbo].[Guide] ON 
GO
INSERT [dbo].[Guide] ([GuideID], [Name], [GuideNo], [Memo], [Status], [Skills], [ImageUrl], [Payment], [StartLevel], [Sex], [Times], [BusyStatus], [HeadUrl], [VideoShowUrl], [SkillPoints], [PhoneNumber], [IDCard], [Intorduction]) VALUES (1, N'张三', N'1', N'11', 1, N'导游三级,中英文全会,男,声音甜面,厉害', N'https://img95.699pic.com/element/40192/1309.png_860.png', CAST(11.00 AS Decimal(18, 2)), 3, 1, 100, 0, NULL, NULL, N'故宫,天安门,颐和园', NULL, N'110106202306250015', N'aaa')
GO
INSERT [dbo].[Guide] ([GuideID], [Name], [GuideNo], [Memo], [Status], [Skills], [ImageUrl], [Payment], [StartLevel], [Sex], [Times], [BusyStatus], [HeadUrl], [VideoShowUrl], [SkillPoints], [PhoneNumber], [IDCard], [Intorduction]) VALUES (2, N'李四', N'2', N'11', 1, N'导游三级,中英文全会,男,声音甜面,厉害', N'https://img95.699pic.com/element/40192/1309.png_860.png', CAST(11.00 AS Decimal(18, 2)), 3, 1, 100, 1, NULL, NULL, N'故宫,天安门,颐和园', NULL, N'110106202306250013', N'bbb')
GO
INSERT [dbo].[Guide] ([GuideID], [Name], [GuideNo], [Memo], [Status], [Skills], [ImageUrl], [Payment], [StartLevel], [Sex], [Times], [BusyStatus], [HeadUrl], [VideoShowUrl], [SkillPoints], [PhoneNumber], [IDCard], [Intorduction]) VALUES (3, N'王五', N'3', N'11', 1, N'导游三级,中英文全会,男,声音甜面,厉害', N'https://img95.699pic.com/element/40192/1309.png_860.png', CAST(11.00 AS Decimal(18, 2)), 3, 1, 100, 2, NULL, NULL, N'故宫,天安门,颐和园', N'13916785678', N'110106202306250012', N'ccc')
GO
INSERT [dbo].[Guide] ([GuideID], [Name], [GuideNo], [Memo], [Status], [Skills], [ImageUrl], [Payment], [StartLevel], [Sex], [Times], [BusyStatus], [HeadUrl], [VideoShowUrl], [SkillPoints], [PhoneNumber], [IDCard], [Intorduction]) VALUES (5, N'杨明', N'85547894', NULL, NULL, N'导游三级,中英文全会,男,声音甜面,厉害', N'http://localhost:1285/upFiles/xUMFnULWAxQR5e2d12556a386306745eb6150d006fb6.png', NULL, 1, NULL, NULL, 0, N'http://localhost:1285/upFiles/xUMFnULWAxQR5e2d12556a386306745eb6150d006fb6.png', N'http://localhost:1285/upFiles/FoB18PUu2a3Nf13004eed4251c602bbe15737e8a1ecb.mp4', N'故宫,天安门,颐和园,八大处', N'13661276416', N'110106202306250011', N'ddd')
GO
SET IDENTITY_INSERT [dbo].[Guide] OFF
GO
SET IDENTITY_INSERT [dbo].[LoginLog] ON 
GO
INSERT [dbo].[LoginLog] ([LoginLogID], [LoginTime], [IP], [Agent], [SystemInfo], [MemberID], [RefMemberID]) VALUES (1, CAST(N'2023-06-10T11:59:53.633' AS DateTime), N'125.34.123.73', N'Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Mobile Safari/537.36', N'Android', 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[LoginLog] OFF
GO
SET IDENTITY_INSERT [dbo].[Lottery] ON 
GO
INSERT [dbo].[Lottery] ([LotteryID], [Name], [Payment], [Intro], [TheOrder], [ImgUrl], [SellerID], [Enabled]) VALUES (1, N'11', CAST(2.00 AS Decimal(8, 2)), N'22', 1, N'11', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Lottery] OFF
GO
SET IDENTITY_INSERT [dbo].[LotterySellerRef] ON 
GO
INSERT [dbo].[LotterySellerRef] ([LotterySellerRefID], [LotteryID], [SellerID], [AddDate], [OpeMemberID]) VALUES (1, 1, 1, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[LotterySellerRef] OFF
GO
SET IDENTITY_INSERT [dbo].[Members] ON 
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (1, N'1111', N'zhoubin_vip@qq.com', N'killer007', 1, CAST(N'2023-06-10T11:59:53.133' AS DateTime), CAST(N'2023-06-10T11:59:53.133' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), N'zhoubin_vip@qq.com', NULL, NULL, NULL, NULL, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (2, N'18600527610', NULL, NULL, 459, CAST(N'2023-06-10T12:28:36.383' AS DateTime), CAST(N'2023-06-20T18:27:49.883' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55O-MVYAArfxoQS5zjt2bK1E', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'59199365feb94e56be00008832c61c08', N'AooV6sxw4Nr30Rsqn2FbaQ==', NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (3, N'母笑阳', NULL, NULL, 1, CAST(N'2023-06-11T12:18:53.603' AS DateTime), CAST(N'2023-06-11T12:18:53.603' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'母笑阳', 0, NULL, NULL, N'oe5Y55PgTMjLxqxY8-Ulqd0y1D48', N'https://thirdwx.qlogo.cn/mmopen/vi_32/PiajxSqBRaEKPJLYCXZQxL0rbmL5JXA3f5HUn5yTcBtxiaB4EQCpnl51zDy6tsJCOoicKxnksrJpsoViayVsAdV9Ng/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'3f4b4ad3220544da9989c5d58d2e2d64', N'Twf/fEg5jNmWNRarDOLriQ==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (4, N'超_越梦想', NULL, NULL, 1, CAST(N'2023-06-11T12:18:53.603' AS DateTime), CAST(N'2023-06-11T12:18:53.603' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'超_越梦想', 0, NULL, NULL, N'oe5Y55Hrt60j8QSULS557v7YJdus', N'https://thirdwx.qlogo.cn/mmopen/vi_32/TmXRGRVvfTI2l86PnLZs4hnDhRLfuh3LKZ4FMYlz3VE7sfJPuxMfWHj6mrsuBiazKpZIjXge5cWD0icicLZZyOA5w/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'aba20bb7d8d04f91938db7e0cd317414', N'GdujYtxeeytvgpTrcNDYYg==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (5, N'微信用户', NULL, NULL, 10, CAST(N'2023-06-11T13:56:24.800' AS DateTime), CAST(N'2023-06-11T16:58:13.580' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55I5uUWoTB1YCzcEdLMlg3SQ', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'920196e7edd6490c8d81b9f81308f935', N'oqfuVJ1SxHzzYeRo+2LYlQ==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (6, N'微信用户', NULL, NULL, 10, CAST(N'2023-06-11T13:57:05.827' AS DateTime), CAST(N'2023-06-11T16:58:51.893' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55Am8gbyaUfoTGiVGMb2BsFI', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'4263c06ae47a418eba83eb22025dea0a', N'+Rw9BjIUG51qAfZLsRxuFw==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (7, N'微信用户', NULL, NULL, 2, CAST(N'2023-06-11T14:04:17.617' AS DateTime), CAST(N'2023-06-11T14:07:11.020' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55OYjNxX1_oEFoGnIWXEDj9c', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'da09fc61881946b5b6ae23df2ed9c71c', N'+2/NNIhn2sCy0zDMF9t5iQ==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (8, N'微信用户', NULL, NULL, 1, CAST(N'2023-06-11T17:01:46.707' AS DateTime), CAST(N'2023-06-11T17:01:46.707' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55H8KMeGGI15k3rLKarawbjA', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'64805a8e19da46eaad24486bed1f1219', N'c4E/aKYxt7Joam05HwG8aw==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (9, N'微信用户', NULL, NULL, 5, CAST(N'2023-06-11T21:22:42.167' AS DateTime), CAST(N'2023-06-11T22:23:04.633' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55PYl9oWnO2tyC4gQNlWBkF8', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'0e0d2cd3e6974433b2cd9b37542f2b34', N'O+o2/savGBiPAwU7+Ulzcw==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (10, N'微信用户', NULL, NULL, 1, CAST(N'2023-06-12T10:37:08.560' AS DateTime), CAST(N'2023-06-12T10:37:08.560' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55AhI2tnYsXJAr17iP60NYr4', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'06fe4741fd51414cbece128b3713643d', N'AHh1RMODQbAH7n3MJBPWkA==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (11, N'微信用户', NULL, NULL, 1, CAST(N'2023-06-14T14:54:17.253' AS DateTime), CAST(N'2023-06-14T14:54:17.253' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55KvPNMUOMdPMcytwdOwYt8o', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'31910266a31744f587b3980b09c4ffc2', N'CWk2vV0/GQd6jq++TC0W9Q==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (12, N'微信用户', NULL, NULL, 187, CAST(N'2023-06-20T10:40:50.740' AS DateTime), CAST(N'2023-06-24T20:14:35.197' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55OB-xZz1fw4IWS5Ly5JRqVg', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'65d3b39dff024c8f94465ab6e821434d', N'WTJ8s745T8+25RIr/7Ju+g==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Members] ([MemberID], [Name], [LoginName], [Password], [LoginCount], [RegDate], [LastDate], [Status], [Balance], [Email], [Phone], [NiceName], [Sex], [Birth], [Balance_back], [WechatOpenid], [HeadImgUrl], [Country], [City], [Province], [TheType], [IsGuanZhu], [Unionid], [UserLevel], [RefMemberID], [TiLiNum], [ISCFJUser], [CFJMemberTypeID], [CFJDayCount], [CFJYearCount], [Token], [Session_key], [XiaoQuID], [QuanIDs], [RenZhengUrl], [IsRenZhengPass], [RefSellerID], [IsAuthen], [RealName], [IDCard], [OwnerSellerID]) VALUES (13, N'微信用户', NULL, NULL, 2, CAST(N'2023-06-20T22:24:31.310' AS DateTime), CAST(N'2023-06-20T22:24:56.967' AS DateTime), 1, CAST(0.00 AS Decimal(8, 2)), NULL, NULL, N'微信用户', 0, NULL, NULL, N'oe5Y55JHmMrjN7sg_WcLJxWF8n2I', N'https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132', N'', N'', N'', NULL, 1, N'', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'191717bb207346c69e7c8cfa9fc84d68', N'zGPbp2eUbaEm0E8o8Nwm6w==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Members] OFF
GO
SET IDENTITY_INSERT [dbo].[NCCLottery] ON 
GO
INSERT [dbo].[NCCLottery] ([NCCLotteryID], [Name], [Payment], [Intro], [TheOrder], [ImgUrl], [ImgUrl1], [ImgUrl2], [OrgImgUrl1], [OrgImgUrl2], [LotteryNo], [PrizeStatus], [PrizeDate], [RefOrderID], [RefLotteryID], [Bonus], [RefMemberID], [RefSellerID], [AddDate], [IsBeep], [BeginOpenDate], [EndOpenDate], [LotteryStatus], [PayBackReason], [IsNotifyBuyerPrizeInfo], [NotifyBuyerPrizeInfoDate]) VALUES (1, N'1', CAST(1.00 AS Decimal(8, 2)), N'11', 1, NULL, NULL, NULL, NULL, NULL, N'122', 1, NULL, 1, 1, CAST(12.00 AS Decimal(8, 2)), 2, 1, CAST(N'2023-06-10T17:44:41.000' AS DateTime), 1, NULL, NULL, 2, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[NCCLottery] OFF
GO
SET IDENTITY_INSERT [dbo].[Seller] ON 
GO
INSERT [dbo].[Seller] ([SellerID], [Name], [Address], [Phone], [SellerNo], [IsOnline], [TheOrder], [QueueNum], [Status], [VideoShowURL], [VideoPushURL], [ShortName], [LastDate], [ManagerMemberID]) VALUES (1, N'sdf', N'', NULL, N'1', 0, NULL, 0, NULL, N'rtmp://pull.aliyunlive.com/ar/1?auth_key=1686376913-0-0-ab140a5635f0165d8064c542b9a99c3a', N'rtmp://push.aliyunlive.com/ar/1?auth_key=1686376913-0-0-ab140a5635f0165d8064c542b9a99c3a', NULL, CAST(N'2023-06-19T16:32:19.017' AS DateTime), NULL)
GO
INSERT [dbo].[Seller] ([SellerID], [Name], [Address], [Phone], [SellerNo], [IsOnline], [TheOrder], [QueueNum], [Status], [VideoShowURL], [VideoPushURL], [ShortName], [LastDate], [ManagerMemberID]) VALUES (2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'artc://push.ar.jyweip.com/ar/test?auth_key=1686376913-0-0-ab140a5635f0165d8064c542b9a99c3a', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Seller] OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 启用  0  禁用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'技能  标签' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'Skills'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'星级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'StartLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别 1 男 0 女' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'讲解了多少次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'Times'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 工作中  0 空闲中 2已离线' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'BusyStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'HeadUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视频展示地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'VideoShowUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'讲解景点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guide', @level2type=N'COLUMN',@level2name=N'SkillPoints'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'MemberID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导游ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'GuideID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务时长_分钟' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'ServiceMinute'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'AddDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'Payment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总付款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'TotalPayment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'PayStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OrderStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'OrderType'
GO
USE [master]
GO
ALTER DATABASE [ncc2019db] SET  READ_WRITE 
GO
