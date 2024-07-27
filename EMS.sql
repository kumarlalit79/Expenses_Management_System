USE [master]
GO
/****** Object:  Database [EMS]    Script Date: 27-07-2024 04:53:40 PM ******/
CREATE DATABASE [EMS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EMS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EMS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EMS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EMS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EMS] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EMS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EMS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EMS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EMS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EMS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EMS] SET ARITHABORT OFF 
GO
ALTER DATABASE [EMS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [EMS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EMS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EMS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EMS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EMS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EMS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EMS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EMS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EMS] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EMS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EMS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EMS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EMS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EMS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EMS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EMS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EMS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EMS] SET  MULTI_USER 
GO
ALTER DATABASE [EMS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EMS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EMS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EMS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EMS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EMS] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EMS] SET QUERY_STORE = ON
GO
ALTER DATABASE [EMS] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EMS]
GO
/****** Object:  Table [dbo].[category_tbl]    Script Date: 27-07-2024 04:53:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category_tbl](
	[cat_id] [int] IDENTITY(1,1) NOT NULL,
	[cat_name] [varchar](50) NOT NULL,
	[created_on] [datetime] NOT NULL,
	[created_by] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[expenses_tbl]    Script Date: 27-07-2024 04:53:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[expenses_tbl](
	[exp_id] [int] IDENTITY(1,1) NOT NULL,
	[monthly_income] [varchar](50) NOT NULL,
	[item_name] [varchar](50) NOT NULL,
	[item_qty] [varchar](50) NOT NULL,
	[total_price] [varchar](50) NOT NULL,
	[remark] [varchar](50) NOT NULL,
	[sdate] [date] NOT NULL,
	[created_on] [date] NOT NULL,
	[created_by] [varchar](50) NOT NULL,
	[fkUserId] [int] NULL,
	[fkCatId] [int] NULL,
	[fkSubCatId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[exp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sub_category_tbl]    Script Date: 27-07-2024 04:53:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sub_category_tbl](
	[subcat_id] [int] IDENTITY(1,1) NOT NULL,
	[subcat_name] [varchar](50) NOT NULL,
	[created_on] [datetime] NOT NULL,
	[created_by] [varchar](50) NOT NULL,
	[fkcat_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[subcat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sub_sub_category_tbl]    Script Date: 27-07-2024 04:53:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sub_sub_category_tbl](
	[sub_sub_catId] [int] IDENTITY(1,1) NOT NULL,
	[sub_sub_catName] [varchar](50) NOT NULL,
	[created_on] [date] NOT NULL,
	[created_by] [varchar](50) NOT NULL,
	[fkCatId] [int] NULL,
	[fkSubCatId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[sub_sub_catId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_tbl]    Script Date: 27-07-2024 04:53:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_tbl](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](50) NOT NULL,
	[address] [varchar](50) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[statename] [varchar](50) NOT NULL,
	[pincode] [varchar](50) NOT NULL,
	[mobile_num] [varchar](50) NOT NULL,
	[email_id] [varchar](50) NOT NULL,
	[created_on] [datetime] NOT NULL,
	[created_by] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[category_tbl] ON 

INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (1, N'Recharge', CAST(N'2024-07-22T09:13:05.803' AS DateTime), N'Gaj')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (4, N'Groceries', CAST(N'2024-07-20T17:21:44.440' AS DateTime), N'Gaj')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (8, N'travel', CAST(N'2024-07-20T07:17:57.810' AS DateTime), N'GAJ')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (9, N'shopping', CAST(N'2024-07-20T08:46:22.700' AS DateTime), N'GAJ')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (10, N'Subscription', CAST(N'2024-07-20T16:52:25.830' AS DateTime), N'GAJ')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (11, N'Rent', CAST(N'2024-07-20T16:52:39.497' AS DateTime), N'GAJ')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (12, N'Medicine', CAST(N'2024-07-20T16:52:47.813' AS DateTime), N'GAJ')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (13, N'Baby Care', CAST(N'2024-07-20T16:52:54.760' AS DateTime), N'GAJ')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (14, N'Entertainment', CAST(N'2024-07-21T08:04:46.003' AS DateTime), N'GAJ')
INSERT [dbo].[category_tbl] ([cat_id], [cat_name], [created_on], [created_by]) VALUES (15, N'Education', CAST(N'2024-07-21T08:04:59.660' AS DateTime), N'GAJ')
SET IDENTITY_INSERT [dbo].[category_tbl] OFF
GO
SET IDENTITY_INSERT [dbo].[expenses_tbl] ON 

INSERT [dbo].[expenses_tbl] ([exp_id], [monthly_income], [item_name], [item_qty], [total_price], [remark], [sdate], [created_on], [created_by], [fkUserId], [fkCatId], [fkSubCatId]) VALUES (7, N'50000', N'jacket', N'1', N'1000', N'good', CAST(N'0001-01-01' AS Date), CAST(N'2024-07-27' AS Date), N'gaj', 1, 9, 6)
INSERT [dbo].[expenses_tbl] ([exp_id], [monthly_income], [item_name], [item_qty], [total_price], [remark], [sdate], [created_on], [created_by], [fkUserId], [fkCatId], [fkSubCatId]) VALUES (8, N'550000', N'home ', N'1', N'10000', N'ok', CAST(N'0001-01-01' AS Date), CAST(N'2024-07-27' AS Date), N'gaj', 1, 11, 5)
INSERT [dbo].[expenses_tbl] ([exp_id], [monthly_income], [item_name], [item_qty], [total_price], [remark], [sdate], [created_on], [created_by], [fkUserId], [fkCatId], [fkSubCatId]) VALUES (9, N'550000', N'tshirt', N'1', N'500', N'good', CAST(N'0001-01-01' AS Date), CAST(N'2024-07-27' AS Date), N'Gaj', 20, 1, 5)
INSERT [dbo].[expenses_tbl] ([exp_id], [monthly_income], [item_name], [item_qty], [total_price], [remark], [sdate], [created_on], [created_by], [fkUserId], [fkCatId], [fkSubCatId]) VALUES (10, N'10000', N'shirt', N'1', N'10000', N'ok', CAST(N'0001-01-01' AS Date), CAST(N'2024-07-27' AS Date), N'Gaj', 22, 1, 5)
SET IDENTITY_INSERT [dbo].[expenses_tbl] OFF
GO
SET IDENTITY_INSERT [dbo].[sub_category_tbl] ON 

INSERT [dbo].[sub_category_tbl] ([subcat_id], [subcat_name], [created_on], [created_by], [fkcat_id]) VALUES (1, N'mobile 123', CAST(N'2024-07-22T17:27:57.623' AS DateTime), N'Gaj', 1)
INSERT [dbo].[sub_category_tbl] ([subcat_id], [subcat_name], [created_on], [created_by], [fkcat_id]) VALUES (3, N'cloths', CAST(N'2024-07-20T12:36:46.093' AS DateTime), N'Lalit', 4)
INSERT [dbo].[sub_category_tbl] ([subcat_id], [subcat_name], [created_on], [created_by], [fkcat_id]) VALUES (4, N'wifi', CAST(N'2024-07-20T17:44:18.147' AS DateTime), N'Lalit', 1)
INSERT [dbo].[sub_category_tbl] ([subcat_id], [subcat_name], [created_on], [created_by], [fkcat_id]) VALUES (5, N'online', CAST(N'2024-07-20T17:45:36.033' AS DateTime), N'Lalit', 4)
INSERT [dbo].[sub_category_tbl] ([subcat_id], [subcat_name], [created_on], [created_by], [fkcat_id]) VALUES (6, N'amazon', CAST(N'2024-07-20T17:46:41.590' AS DateTime), N'Lalit', 10)
INSERT [dbo].[sub_category_tbl] ([subcat_id], [subcat_name], [created_on], [created_by], [fkcat_id]) VALUES (10, N'online', CAST(N'2024-07-23T07:23:59.713' AS DateTime), N'Lalit', 9)
SET IDENTITY_INSERT [dbo].[sub_category_tbl] OFF
GO
SET IDENTITY_INSERT [dbo].[sub_sub_category_tbl] ON 

INSERT [dbo].[sub_sub_category_tbl] ([sub_sub_catId], [sub_sub_catName], [created_on], [created_by], [fkCatId], [fkSubCatId]) VALUES (1, N'jio', CAST(N'2024-07-22' AS Date), N'Gaj', 1, 1)
INSERT [dbo].[sub_sub_category_tbl] ([sub_sub_catId], [sub_sub_catName], [created_on], [created_by], [fkCatId], [fkSubCatId]) VALUES (3, N'pants', CAST(N'2024-07-23' AS Date), N'Gaj', 9, 6)
INSERT [dbo].[sub_sub_category_tbl] ([sub_sub_catId], [sub_sub_catName], [created_on], [created_by], [fkCatId], [fkSubCatId]) VALUES (8, N'Home Rent', CAST(N'2024-07-23' AS Date), N'gaj', 11, 5)
SET IDENTITY_INSERT [dbo].[sub_sub_category_tbl] OFF
GO
SET IDENTITY_INSERT [dbo].[user_tbl] ON 

INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (1, N'Lalit', N'Himmatpur Block', N'Ramnagar', N'Uttrakhand', N'244715', N'7900678792', N'lalit123@gmail.com', CAST(N'2024-07-19T14:55:23.620' AS DateTime), N'son')
INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (3, N'Gajendra Kumar', N'delhi', N'Ramnagar', N'up', N'123654', N'8974563215', N'gaj@gmail.com', CAST(N'2024-07-20T20:31:23.000' AS DateTime), N'gaj')
INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (4, N'Yugal Kishor', N'HimmatPur Block', N'Ramnagar', N'Uttrakhand', N'244715', N'9837128512', N'Yugal123@gmail.com', CAST(N'2024-07-21T12:44:43.000' AS DateTime), N'gaj')
INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (5, N'Lucky', N'HimmatPur Block', N'Peerumadara', N'Uttrakhand', N'244715', N'7900678799', N'lucky@gmail.in', CAST(N'2024-07-25T11:55:19.373' AS DateTime), N'gaj')
INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (6, N'Gajendra', N'delhi', N'noida', N'up', N'123654', N'7900678791', N'gaj@gmail.com', CAST(N'2024-07-26T16:59:43.403' AS DateTime), N'gaj')
INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (20, N'Rishabh Kumar', N'HimmatPur Block', N'Ramnagar', N'Uttarakhand', N'244715', N'8474918288', N'Rishabh123@gmail.com', CAST(N'2024-07-27T12:53:09.927' AS DateTime), N'gaj')
INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (21, N'Gajendra', N'HimmatPur Block', N'Ramnagar', N'Uttarakhand', N'244715', N'8474918289', N'gaj@gmail.com', CAST(N'2024-07-27T13:21:23.687' AS DateTime), N'gaj')
INSERT [dbo].[user_tbl] ([user_id], [user_name], [address], [city], [statename], [pincode], [mobile_num], [email_id], [created_on], [created_by]) VALUES (22, N'Gourav', N'Ghurdauri', N'Pauri', N'Uttarakhand', N'652314', N'9032698541', N'gaurav@gmail.com', CAST(N'2024-07-27T16:25:38.957' AS DateTime), N'gaj')
SET IDENTITY_INSERT [dbo].[user_tbl] OFF
GO
ALTER TABLE [dbo].[expenses_tbl]  WITH CHECK ADD FOREIGN KEY([fkCatId])
REFERENCES [dbo].[category_tbl] ([cat_id])
GO
ALTER TABLE [dbo].[expenses_tbl]  WITH CHECK ADD FOREIGN KEY([fkSubCatId])
REFERENCES [dbo].[sub_category_tbl] ([subcat_id])
GO
ALTER TABLE [dbo].[expenses_tbl]  WITH CHECK ADD FOREIGN KEY([fkUserId])
REFERENCES [dbo].[user_tbl] ([user_id])
GO
ALTER TABLE [dbo].[sub_category_tbl]  WITH CHECK ADD FOREIGN KEY([fkcat_id])
REFERENCES [dbo].[category_tbl] ([cat_id])
GO
ALTER TABLE [dbo].[sub_sub_category_tbl]  WITH CHECK ADD FOREIGN KEY([fkCatId])
REFERENCES [dbo].[category_tbl] ([cat_id])
GO
ALTER TABLE [dbo].[sub_sub_category_tbl]  WITH CHECK ADD FOREIGN KEY([fkSubCatId])
REFERENCES [dbo].[sub_category_tbl] ([subcat_id])
GO
USE [master]
GO
ALTER DATABASE [EMS] SET  READ_WRITE 
GO
