
/****** Object:  Table [dbo].[RegistrationSync]    Script Date: 10/25/2018 3:20:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegistrationSync]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RegistrationSync](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GuidId] [uniqueidentifier] NOT NULL,
	[IsSyncCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_RegistrationSync] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[RegistrationSync] ADD  CONSTRAINT [DF_RegistrationSync_GuidId]  DEFAULT (newid()) FOR [GuidId]

ALTER TABLE [dbo].[RegistrationSync] ADD  CONSTRAINT [DF_RegistrationSync_IsSyncCompleted]  DEFAULT ((0)) FOR [IsSyncCompleted]
END
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_HotelRoomRegistration_GuidId DEFAULT (newid()) WITH VALUES
END
GO

/****** Object:  Table [dbo].[RestaurantBillSync]    Script Date: 10/26/2018 7:24:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBillSync]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBillSync](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GuidId] [uniqueidentifier] NOT NULL,
	[IsSyncCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_RestaurantSync] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[RestaurantBillSync] ADD  CONSTRAINT [DF_RestaurantBillSync_GuidId]  DEFAULT (newid()) FOR [GuidId]


ALTER TABLE [dbo].[RestaurantBillSync] ADD  CONSTRAINT [DF_RestaurantBillSync_IsSyncCompleted]  DEFAULT ((0)) FOR [IsSyncCompleted]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_RestaurantBill_GuidId DEFAULT (newid()) WITH VALUES
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceBillSync]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServiceBillSync](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GuidId] [uniqueidentifier] NOT NULL,
	[IsSyncCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_ServiceBillSync] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[ServiceBillSync] ADD  CONSTRAINT [DF_ServiceBillSync_GuidId]  DEFAULT (newid()) FOR [GuidId]

ALTER TABLE [dbo].[ServiceBillSync] ADD  CONSTRAINT [DF_ServiceBillSync_IsSyncCompleted]  DEFAULT ((0)) FOR [IsSyncCompleted]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestServiceBill' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.HotelGuestServiceBill
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_HotelGuestServiceBill_GuidId DEFAULT (newid()) WITH VALUES
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetSync]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetSync](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GuidId] [uniqueidentifier] NOT NULL,
	[IsSyncCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_BanquetSync] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[BanquetSync] ADD  CONSTRAINT [DF_BanquetSync_GuidId]  DEFAULT (newid()) FOR [GuidId]

ALTER TABLE [dbo].[BanquetSync] ADD  CONSTRAINT [DF_BanquetSync_IsSyncCompleted]  DEFAULT ((0)) FOR [IsSyncCompleted]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.BanquetReservation
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_BanquetReservation_GuidId DEFAULT (newid()) WITH VALUES
END
GO