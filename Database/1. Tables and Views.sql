/***** Object:  Table [dbo].[GLNodeMatrix]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLNodeMatrix]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLNodeMatrix](
	[NodeId] [int] IDENTITY(1,1) NOT NULL,
	[AncestorId] [int] NULL,
	[NodeNumber] [varchar](50) NOT NULL,
	[NodeHead] [varchar](256) NOT NULL,
	[Lvl] [int] NOT NULL,
	[Hierarchy] [varchar](900) NULL,
	[HierarchyIndex] [varchar](900) NULL,
	[NodeMode] [bit] NOT NULL,
	[NodeType] [nvarchar](25) NULL,
	[NotesNumber] [varchar](50) NULL,
	[IsTransactionalHead] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_NodeMatrix] PRIMARY KEY CLUSTERED 
(
	[NodeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UnNodeMatrix] UNIQUE NONCLUSTERED 
(
	[NodeNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_PADDING OFF
GO
-- Primary key NodeId type changed from INT to BIGINT

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLNodeMatrix' AND column_name = 'NodeId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[GLNodeMatrix]') AND name = N'PK_NodeMatrix')
 --BEGIN
	--IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetRequisites_GLNodeMatrix') AND parent_object_id = OBJECT_ID(N'dbo.BanquetRequisites'))
	--	ALTER TABLE [dbo].BanquetInformation DROP CONSTRAINT [FK_BanquetInformation_GLNodeMatrix]
 --   IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_HotelRoomType_GLNodeMatrix') AND parent_object_id = OBJECT_ID(N'dbo.HotelRoomType'))
	--	ALTER TABLE [dbo].[HotelRoomType]  DROP  CONSTRAINT [FK_HotelRoomType_GLNodeMatrix] 
	--IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetRequisites_GLNodeMatrix') AND parent_object_id = OBJECT_ID(N'dbo.BanquetRequisites'))
	--	ALTER TABLE [dbo].[BanquetRequisites]  DROP CONSTRAINT [FK_BanquetRequisites_GLNodeMatrix]
 ALTER TABLE [dbo].GLNodeMatrix DROP CONSTRAINT [PK_NodeMatrix]

 ALTER TABLE dbo.GLNodeMatrix
    ALTER COLUMN NodeId bigint NOT NULL;
 
 ALTER TABLE [dbo].[GLNodeMatrix] ADD  CONSTRAINT [PK_NodeMatrix] PRIMARY KEY CLUSTERED 
 (
  [NodeId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   
END 
GO

/****** Object:  Table [dbo].[BanquetReservationClientParticipants]    Script Date: 9/1/2021 5:54:22 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationClientParticipants]') AND type in (N'U'))
DROP TABLE [dbo].[BanquetReservationClientParticipants]
GO

/****** Object:  Table [dbo].[BanquetReservationClientParticipants]    Script Date: 9/1/2021 5:54:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BanquetReservationClientParticipants](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BanquetReservationId] [int] NULL,
	[ClientParticipantId] [int] NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[GLSupplierCompanyBalanceTransfer]    Script Date: 12/05/2021 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLSupplierCompanyBalanceTransfer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLSupplierCompanyBalanceTransfer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionType] [nvarchar](300) NOT NULL,
	[FromTransactionId] [int] NOT NULL,
	[ToTransactionId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	PRIMARY KEY (Id)
	)
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD LastModifiedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'LastModifiedDate')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD LastModifiedDate DATETIME NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD CheckedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD ApprovedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD CheckedByUsers VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD ApprovedByUsers VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'ApprovedStatus')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD ApprovedStatus VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD Remarks VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLSupplierCompanyBalanceTransfer' AND column_name = 'TransactionDate')
BEGIN
	ALTER TABLE dbo.GLSupplierCompanyBalanceTransfer
		ADD TransactionDate DATETIME NULL
END
GO
/****** Object:  Table [dbo].[UserDashboardItemMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserDashboardItemMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserDashboardItemMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[Panel] [int] NULL,
	[Div] [int] NULL,
 CONSTRAINT [PK_UserDashboardItemMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SMServiceType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMServiceType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMServiceType](
	[ServiceTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [varchar](250) NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMServiceType] PRIMARY KEY CLUSTERED 
(
	[ServiceTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMSalesOrderDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSalesOrderDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSalesOrderDetails](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[SOrderId] [int] NULL,
	[CostCenterId] [int] NULL,
	[StockById] [int] NULL,
	[ProductId] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[Quantity] [decimal](18, 2) NULL,
 CONSTRAINT [PK_SMSalesOrderDetails] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SMSalesOrder]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSalesOrder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSalesOrder](
	[SOrderId] [int] IDENTITY(1,1) NOT NULL,
	[SODate] [datetime] NULL,
	[DeliveryDate] [datetime] NULL,
	[SONumber] [varchar](50) NULL,
	[CompanyId] [int] NULL,
	[ApprovedStatus] [varchar](20) NULL,
	[DeliveryStatus] [varchar](50) NULL,
	[Remarks] [varchar](250) NULL,
	[CheckedBy] [int] NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMSalesOrder] PRIMARY KEY CLUSTERED 
(
	[SOrderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMSalesOrder' AND column_name = 'CostCenterId')
BEGIN
	ALTER TABLE dbo.SMSalesOrder
		ADD CostCenterId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMSalesOrder' AND column_name = 'BillId')
BEGIN
	ALTER TABLE dbo.SMSalesOrder
		ADD BillId BIGINT NULL
END
GO
/****** Object:  Table [dbo].[SMQuotationDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMQuotationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMQuotationDetails](
	[QuotationDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[QuotationId] [bigint] NOT NULL,
	[ItemType] [nvarchar](25) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ServicePackageId] [int] NULL,
	[ServiceBandWidthId] [int] NULL,
	[ServiceTypeId] [int] NULL,
	[ItemId] [int] NOT NULL,
	[StockBy] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[TotalPrice] [money] NOT NULL,
	[UpLink] [int] NULL,
 CONSTRAINT [PK_SMQuotationDetails] PRIMARY KEY CLUSTERED 
(
	[QuotationDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotationDetails' AND column_name = 'SalesNote')
BEGIN
	ALTER TABLE dbo.SMQuotationDetails
		ADD SalesNote VARCHAR(250) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBearer' AND column_name = 'IsChef')
BEGIN
	ALTER TABLE dbo.RestaurantBearer
		ADD IsChef BIT NOT NULL CONSTRAINT DF_RestaurantBearer_IsChef DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotationDetails' AND column_name = 'RemainingDeliveryQuantity')
BEGIN
	ALTER TABLE dbo.SMQuotationDetails
		ADD RemainingDeliveryQuantity decimal(18,2) NULL
ALTER TABLE [dbo].[SMQuotationDetails] ADD  CONSTRAINT [DF_SMQuotationDetails_RemainingDeliveryQuantity]  DEFAULT ((0)) FOR [RemainingDeliveryQuantity]

END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotationDetails' AND column_name = 'DeliveredQuantity')
BEGIN
	ALTER TABLE dbo.SMQuotationDetails
		ADD DeliveredQuantity decimal(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotationDetails' AND column_name = 'DiscountType')
BEGIN
	ALTER TABLE dbo.SMQuotationDetails
		ADD DiscountType VARCHAR(20) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotationDetails' AND column_name = 'DiscountAmount')
BEGIN
	ALTER TABLE dbo.SMQuotationDetails
		ADD DiscountAmount DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotationDetails' AND column_name = 'DiscountAmountUSD')
BEGIN
	ALTER TABLE dbo.SMQuotationDetails
		ADD DiscountAmountUSD DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotationDetails' AND column_name = 'IsDiscountForAll')
BEGIN
	ALTER TABLE dbo.SMQuotationDetails
		ADD IsDiscountForAll BIT NOT NULL CONSTRAINT DF_SMQuotationDetails_IsDiscountForAll DEFAULT 1 WITH VALUES 
END
GO
/****** Object:  Table [dbo].[SMQuotation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMQuotation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMQuotation](
	[QuotationId] [bigint] IDENTITY(1,1) NOT NULL,
	[QuotationNo] [nvarchar](25) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ProposalDate] [date] NOT NULL,
	[ServiceTypeId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[TotalDeviceOrUser] [smallint] NOT NULL,
	[ContractPeriodId] [int] NOT NULL,
	[BillingPeriodId] [int] NOT NULL,
	[ItemServiceDeliveryId] [int] NOT NULL,
	[CurrentVendorId] [int] NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMQuotation] PRIMARY KEY CLUSTERED 
(
	[QuotationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'SalesNote')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD SalesNote VARCHAR(250) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'DealId')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD DealId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'IsAccepted')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD IsAccepted BIT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'IsSalesNoteFinal')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD IsSalesNoteFinal BIT NOT NULL CONSTRAINT DF_SMQuotation_IsSalesNoteFinal DEFAULT 0 WITH VALUES
END
GO
-- add column GLCompanyId
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'GLCompanyId')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD GLCompanyId INT NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD ProjectId INT NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'IsDiscountEnable')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD IsDiscountEnable BIT NOT NULL CONSTRAINT DF_CommonCostCenter_IsDiscountEnable DEFAULT 1 WITH VALUES
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'IsEnableItemAutoDeductFromStore')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD IsEnableItemAutoDeductFromStore BIT NOT NULL CONSTRAINT DF_CommonCostCenter_IsEnableItemAutoDeductFromStore DEFAULT 1 WITH VALUES
END
GO
-- add column IsLocalOrForeignTransactionType
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'IsLocalOrForeignTransactionType')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD IsLocalOrForeignTransactionType INT NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'IsApprovedFromBilling')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD IsApprovedFromBilling BIT NOT NULL CONSTRAINT DF_SMQuotation_IsApprovedFromBilling DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'PriceValidity')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD PriceValidity VARCHAR(150) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'DeployLocation')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD DeployLocation VARCHAR(150) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'ContactId')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD ContactId BIGINT NULL
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'LocationId')
BEGIN
	ALTER TABLE dbo.SMQuotation
		DROP COLUMN LocationId
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'SiteId')
BEGIN
	ALTER TABLE dbo.SMQuotation
		DROP COLUMN SiteId 
END
GO
/****** Object:  Table [dbo].[SMQuotationDiscountDetails]    Script Date: 23-Oct-19 4:53:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMQuotationDiscountDetails]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMQuotationDiscountDetails](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[SMQuotationDetailsId] [bigint] NULL,
		[OutLetId] [bigint] NULL,
		[Type] [varchar](50) NULL,
		[TypeId] [int] NULL,
		[DiscountType] [varchar](20) NULL,
		[DiscountAmount] [decimal](18, 2) NULL,
		[DiscountAmountUSD] [decimal](18, 2) NULL,
	 CONSTRAINT [PK_SMQuotationDiscountDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SMItemOrServiceDelivery]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMItemOrServiceDelivery]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMItemOrServiceDelivery](
	[ItemServiceDeliveryId] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryTypeName] [varchar](250) NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMItemOrServiceDelivery] PRIMARY KEY CLUSTERED 
(
	[ItemServiceDeliveryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMIndustry]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMIndustry]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMIndustry](
	[IndustryId] [int] IDENTITY(1,1) NOT NULL,
	[IndustryName] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMIndustry] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMCurrentVendor]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCurrentVendor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMCurrentVendor](
	[CurrentVendorId] [int] IDENTITY(1,1) NOT NULL,
	[VendorName] [varchar](250) NOT NULL,
	[Address] [nvarchar](250) NULL,
	[ContactNo] [nvarchar](25) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMCurrentVendor] PRIMARY KEY CLUSTERED 
(
	[CurrentVendorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMContractPeriod]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMContractPeriod]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMContractPeriod](
	[ContractPeriodId] [int] IDENTITY(1,1) NOT NULL,
	[ContractPeriodName] [varchar](250) NOT NULL,
	[ContractPeriodValue] [smallint] NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMContractPeriod] PRIMARY KEY CLUSTERED 
(
	[ContractPeriodId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMCompanySite]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCompanySite]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMCompanySite](
	[SiteId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[SiteName] [varchar](250) NOT NULL,
	[BusinessContactName] [varchar](250) NULL,
	[BusinessContactEmail] [varchar](150) NULL,
	[BusinessContactPhone] [varchar](25) NULL,
	[TechnicalContactName] [varchar](250) NULL,
	[TechnicalContactEmail] [varchar](150) NULL,
	[TechnicalContactPhone] [varchar](25) NULL,
	[BillingContactName] [varchar](250) NULL,
	[BillingContactEmail] [varchar](150) NULL,
	[BillingContactPhone] [varchar](25) NULL,
	[Remarks] [varchar](350) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMCompanySite] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMCompanySalesCallDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCompanySalesCallDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMCompanySalesCallDetail](
	[SalesCallDetailId] [int] IDENTITY(1,1) NOT NULL,
	[SalesCallId] [int] NULL,
	[EmpId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMCompanySalesCallDetail] PRIMARY KEY CLUSTERED 
(
	[SalesCallDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SMCompanySalesCall]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCompanySalesCall]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMCompanySalesCall](
	[SalesCallId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NULL,
	[SiteId] [int] NULL,
	[InitialDate] [datetime] NULL,
	[FollowupDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[FollowupTypeId] [int] NULL,
	[FollowupType] [varchar](250) NULL,
	[PurposeId] [int] NULL,
	[Purpose] [varchar](250) NULL,
	[LocationId] [int] NULL,
	[CityId] [int] NULL,
	[IndustryId] [int] NULL,
	[CITypeId] [int] NULL,
	[ActionPlanId] [int] NULL,
	[OpportunityStatusId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMCompanySalesCall] PRIMARY KEY CLUSTERED 
(
	[SalesCallId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMBillingPeriod]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMBillingPeriod]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMBillingPeriod](
	[BillingPeriodId] [int] IDENTITY(1,1) NOT NULL,
	[BillingPeriodName] [varchar](250) NOT NULL,
	[BillingPeriodValue] [smallint] NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMBillingPeriod] PRIMARY KEY CLUSTERED 
(
	[BillingPeriodId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMAccountManager]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMAccountManager]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMAccountManager](
	[AccountManagerId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[EmpId] [int] NOT NULL,
	[SortName] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMAccountManager] PRIMARY KEY CLUSTERED 
(
	[AccountManagerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SecurityUserInformation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityUserInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityUserInformation](
	[UserInfoId] [int] IDENTITY(1,1) NOT NULL,
	[UserGroupId] [int] NOT NULL,
	[UserName] [varchar](256) NOT NULL,
	[UserId] [varchar](50) NOT NULL,
	[UserPassword] [varchar](20) NOT NULL,
	[UserEmail] [varchar](100) NULL,
	[UserPhone] [varchar](50) NULL,
	[UserDesignation] [varchar](500) NULL,
	[ActiveStat] [bit] NOT NULL,
	[WorkingCostCenterId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[EmpId] [int] NULL,
	[IsAdminUser] [bit] NULL,
 CONSTRAINT [PK_SecurityUserInformation] PRIMARY KEY CLUSTERED 
(
	[UserInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityUserGroupCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityUserGroupCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityUserGroupCostCenterMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[UserGroupId] [int] NULL,
 CONSTRAINT [PK_SecurityUserGroupCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SecurityUserGroup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityUserGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityUserGroup](
	[UserGroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](100) NULL,
	[DefaultHomePageId] [int] NULL,
	[IsGroupApplicable] [bit] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SecurityUserGroup] PRIMARY KEY CLUSTERED 
(
	[UserGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityUserGroup' AND column_name = 'Email')
BEGIN
	ALTER TABLE dbo.SecurityUserGroup
	ADD Email VARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityUserGroup' AND column_name = 'UserGroupType')
BEGIN
	ALTER TABLE dbo.SecurityUserGroup
	ADD UserGroupType VARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityUserGroup' AND column_name = 'IsPaymentBillInfoHideInCompanyBillReceive')
BEGIN
	ALTER TABLE dbo.SecurityUserGroup
	ADD IsPaymentBillInfoHideInCompanyBillReceive INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityUserGroup' AND column_name = 'IsReceiveBillInfoHideInSupplierBillPayment')
BEGIN
	ALTER TABLE dbo.SecurityUserGroup
	ADD IsReceiveBillInfoHideInSupplierBillPayment INT NULL
END
GO
/****** Object:  Table [dbo].[SecurityUserCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityUserCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityUserCostCenterMapping](
	[MappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserInfoId] [int] NULL,
	[CostCenterId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SecurityUserCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SecurityObjectTab]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityObjectTab]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityObjectTab](
	[ObjectTabId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NULL,
	[ObjectGroupHead] [varchar](50) NOT NULL,
	[ObjectHead] [varchar](256) NOT NULL,
	[MenuHead] [varchar](256) NOT NULL,
	[ObjectType] [varchar](20) NOT NULL,
	[FormName] [varchar](256) NOT NULL,
	[ActiveStat] [bit] NOT NULL,
 CONSTRAINT [PK_ObjectTab] PRIMARY KEY CLUSTERED 
(
	[ObjectTabId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityObjectPermission]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityObjectPermission]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityObjectPermission](
	[ObjectPermissionId] [int] IDENTITY(1,1) NOT NULL,
	[ObjectTabId] [int] NOT NULL,
	[UserGroupId] [int] NOT NULL,
	[IsSavePermission] [bit] NULL,
	[IsUpdatePermission] [bit] NULL,
	[IsDeletePermission] [bit] NULL,
	[IsViewPermission] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SecurityObjectPermission] PRIMARY KEY CLUSTERED 
(
	[ObjectPermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SecurityMenuWiseLinks]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityMenuWiseLinks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityMenuWiseLinks](
	[MenuWiseLinksId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserGroupId] [int] NOT NULL,
	[MenuGroupId] [bigint] NOT NULL,
	[MenuLinksId] [bigint] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[IsSavePermission] [bit] NOT NULL,
	[IsUpdatePermission] [bit] NULL,
	[IsDeletePermission] [bit] NOT NULL,
	[IsViewPermission] [bit] NOT NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SecurityMenuWiseLinks] PRIMARY KEY CLUSTERED 
(
	[MenuWiseLinksId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityMenuWiseLinks' AND column_name = 'UserId')
BEGIN
	ALTER TABLE dbo.SecurityMenuWiseLinks
		ADD UserId	INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityMenuWiseLinks' AND column_name = 'UserGroupId')
BEGIN
	ALTER TABLE dbo.SecurityMenuWiseLinks
		ALTER COLUMN UserGroupId	INT NULL
END
GO

/****** Object:  Table [dbo].[SecurityMenuWiseLinksByUserInfoId]    Script Date: 05/02/2022 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityMenuWiseLinksByUserInfoId]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityMenuWiseLinksByUserInfoId](
	[MenuWiseLinksByUserInfoId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[UserGroupId] [int] NULL,
	[TransactionType] [varchar](255) NOT NULL,
	[MenuGroupId] [bigint] NOT NULL,
	[MenuLinksId] [bigint] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[IsSavePermission] [bit] NOT NULL,
	[IsUpdatePermission] [bit] NULL,
	[IsDeletePermission] [bit] NOT NULL,
	[IsViewPermission] [bit] NOT NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SecurityMenuWiseLinksByUserInfoId] PRIMARY KEY CLUSTERED 
(
	[MenuWiseLinksByUserInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[SecurityMenuLinks]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityMenuLinks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityMenuLinks](
	[MenuLinksId] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[PageId] [varchar](250) NOT NULL,
	[PageName] [varchar](50) NULL,
	[PageDisplayCaption] [varchar](50) NULL,
	[PageExtension] [varchar](25) NOT NULL,
	[PagePath] [varchar](200) NOT NULL,
	[PageType] [varchar](6) NULL,
	[LinkIconClass] [nvarchar](25) NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SecurityMenuLinks] PRIMARY KEY CLUSTERED 
(
	[MenuLinksId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityMenuGroup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityMenuGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityMenuGroup](
	[MenuGroupId] [bigint] IDENTITY(1,1) NOT NULL,
	[MenuGroupName] [nvarchar](50) NOT NULL,
	[GroupDisplayCaption] [nvarchar](50) NULL,
	[DisplaySequence] [int] NOT NULL,
	[GroupIconClass] [varchar](25) NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SecurityMenuGroup] PRIMARY KEY CLUSTERED 
(
	[MenuGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityLogFile]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityLogFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityLogFile](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NOT NULL,
	[InpuTMode] [varchar](25) NOT NULL,
	[LogMemo] [varchar](900) NOT NULL,
	[LogMode] [char](1) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityLogError]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityLogError]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityLogError](
	[ErrorLogId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ErrorDetails] [varchar](500) NULL,
	[ErrorStatus] [varchar](20) NULL,
 CONSTRAINT [PK_LogError] PRIMARY KEY CLUSTERED 
(
	[ErrorLogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityActivityLogs]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityActivityLogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityActivityLogs](
	[ActivityId] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivityType] [varchar](256) NOT NULL,
	[EntityType] [varchar](256) NOT NULL,
	[EntityId] [bigint] NULL,
	[Remarks] [varchar](256) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[Module] [varchar](100) NULL,
 CONSTRAINT [PK_ActivityLogs] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesServiceBundleDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesServiceBundleDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesServiceBundleDetails](
	[DetailsId] [int] IDENTITY(1,1) NOT NULL,
	[BundleId] [int] NULL,
	[IsProductOrService] [varchar](50) NULL,
	[ProductId] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 0) NULL,
 CONSTRAINT [PK_SalesServiceBundleDetails] PRIMARY KEY CLUSTERED 
(
	[DetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesServiceBundle]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesServiceBundle]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesServiceBundle](
	[BundleId] [int] IDENTITY(1,1) NOT NULL,
	[BundleName] [varchar](100) NULL,
	[BundleCode] [varchar](20) NULL,
	[Frequency] [varchar](20) NULL,
	[SellingPriceLocal] [int] NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[SellingPriceUsd] [int] NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SalesServiceBundle] PRIMARY KEY CLUSTERED 
(
	[BundleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesService]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesService]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesService](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](250) NULL,
	[CategoryId] [int] NOT NULL,
	[PurchasePrice] [decimal](18, 2) NULL,
	[SellingLocalCurrencyId] [int] NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[SellingUsdCurrencyId] [int] NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
	[Frequency] [varchar](20) NULL,
	[BandwidthType] [int] NULL,
	[Bandwidth] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesCustomer]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesCustomer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesCustomer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerType] [varchar](50) NULL,
	[Name] [varchar](200) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Address] [varchar](250) NULL,
	[Phone] [varchar](50) NULL,
	[Email] [varchar](100) NULL,
	[WebAddress] [varchar](100) NULL,
	[ContactPerson] [varchar](200) NULL,
	[ContactDesignation] [varchar](100) NULL,
	[Department] [varchar](50) NULL,
	[ContactEmail] [varchar](100) NULL,
	[ContactPhone] [varchar](50) NULL,
	[ContactFax] [varchar](50) NULL,
	[ContactPerson2] [varchar](200) NULL,
	[ContactDesignation2] [varchar](100) NULL,
	[Department2] [varchar](50) NULL,
	[ContactEmail2] [varchar](100) NULL,
	[ContactPhone2] [varchar](50) NULL,
	[ContactFax2] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SalesCustomer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesContractDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesContractDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesContractDetails](
	[ContractDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[SigningDate] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[DocumentName] [varchar](300) NULL,
	[DocumentPath] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_SalesContractDetails] PRIMARY KEY CLUSTERED 
(
	[ContractDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesBandwidthInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesBandwidthInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesBandwidthInfo](
	[BandwidthInfoId] [int] IDENTITY(1,1) NOT NULL,
	[BandwidthType] [varchar](50) NULL,
	[BandwidthName] [varchar](250) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMBandwithInfo] PRIMARY KEY CLUSTERED 
(
	[BandwidthInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantToken]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantToken]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantToken](
	[TokenId] [bigint] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[BearerId] [int] NOT NULL,
	[TokenDate] [date] NOT NULL,
	[Token] [int] NOT NULL,
	[TokenNumber] [nvarchar](15) NOT NULL,
	[KotId] [int] NULL,
	[BillId] [int] NULL,
	[IsBillHoldup] [bit] NULL,
	[TokenStatus] [nvarchar](25) NULL,
 CONSTRAINT [PK_RestaurantToken] PRIMARY KEY CLUSTERED 
(
	[TokenId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantTableStatus]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantTableStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantTableStatus](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
 CONSTRAINT [PK_RestaurantTableStatus] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantTableReservationDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantTableReservationDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantTableReservationDetail](
	[ReservationDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[CostCenterId] [int] NULL,
	[TableId] [int] NULL,
	[DiscountType] [varchar](20) NULL,
	[Amount] [decimal](18, 2) NULL,
	[IsRegistered] [bit] NULL,
 CONSTRAINT [PK_RestaurantTableReservationDetail] PRIMARY KEY CLUSTERED 
(
	[ReservationDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantTableManagement]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantTableManagement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantTableManagement](
	[TableManagementId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[TableId] [int] NOT NULL,
	[XCoordinate] [float] NULL,
	[YCoordinate] [float] NULL,
	[TableWidth] [int] NULL,
	[TableHeight] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantTableManagement] PRIMARY KEY CLUSTERED 
(
	[TableManagementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantTableManagement' AND column_name = 'DivTransition')
BEGIN
	ALTER TABLE dbo.RestaurantTableManagement
		ADD DivTransition [varchar](500) NOT NULL CONSTRAINT DF_RestaurantTableManagement_DivTransition DEFAULT ('matrix(1, 0, 0, 1, 0, 0)') WITH VALUES
END
GO

/****** Object:  Table [dbo].[RestaurantTable]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantTable]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantTable](
	[TableId] [int] IDENTITY(1,1) NOT NULL,
	[TableNumber] [varchar](50) NULL,
	[TableCapacity] [varchar](20) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantTable] PRIMARY KEY CLUSTERED 
(
	[TableId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantReservationTableDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantReservationTableDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantReservationTableDetail](
	[ReservationDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[CostCenterId] [int] NULL,
	[TableId] [int] NULL,
	[DiscountType] [varchar](20) NULL,
	[Amount] [decimal](18, 2) NULL,
	[IsRegistered] [bit] NULL,
 CONSTRAINT [PK_RestaurantReservationTableDetail] PRIMARY KEY CLUSTERED 
(
	[ReservationDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantReservationItemDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantReservationItemDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantReservationItemDetail](
	[ItemDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[ItemTypeId] [int] NULL,
	[ItemType] [varchar](50) NULL,
	[ItemId] [int] NULL,
	[ItemName] [varchar](300) NULL,
	[ItemUnit] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[TotalPrice] [decimal](18, 2) NULL,
	[IsComplementary] [bit] NULL,
 CONSTRAINT [PK_RestaurantReservationItemDetail] PRIMARY KEY CLUSTERED 
(
	[ItemDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantReservation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantReservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantReservation](
	[ReservationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservationNumber] [varchar](50) NULL,
	[ReservationDate] [datetime] NULL,
	[DateIn] [datetime] NULL,
	[DateOut] [datetime] NULL,
	[ConfirmationDate] [datetime] NULL,
	[ReservedCompany] [varchar](100) NULL,
	[GuestId] [bigint] NULL,
	[ContactAddress] [varchar](300) NULL,
	[ContactPerson] [varchar](200) NULL,
	[ContactNumber] [varchar](100) NULL,
	[MobileNumber] [varchar](20) NULL,
	[FaxNumber] [varchar](50) NULL,
	[ContactEmail] [varchar](100) NULL,
	[TotalTableNumber] [int] NULL,
	[ReservationMode] [varchar](20) NULL,
	[ReservationType] [varchar](20) NULL,
	[ReservationStatus] [varchar](20) NULL,
	[PendingDeadline] [datetime] NULL,
	[IsListedCompany] [bit] NULL,
	[CompanyId] [int] NULL,
	[BusinessPromotionId] [int] NULL,
	[ReferenceId] [int] NULL,
	[PaymentMode] [varchar](20) NULL,
	[PayFor] [int] NULL,
	[CurrencyType] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[Reason] [varchar](500) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantReservation] PRIMARY KEY CLUSTERED 
(
	[ReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantRecipeDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantRecipeDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantRecipeDetail](
	[RecipeId] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NULL,
	[RecipeItemId] [int] NULL,
	[RecipeItemName] [varchar](200) NULL,
	[UnitHeadId] [int] NULL,
	[ItemUnit] [decimal](18, 5) NULL,
	[ItemCost] [decimal](18, 2) NULL,
	[IsRecipe] [bit] NULL,
 CONSTRAINT [PK_RestaurantRecipeDetail] PRIMARY KEY CLUSTERED 
(
	[RecipeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantRecipeDetail' AND column_name = 'IsGradientCanChange')
BEGIN
	ALTER TABLE dbo.RestaurantRecipeDetail
		ADD IsGradientCanChange BIT NULL
END
GO 

/****** Object:  Table [dbo].[RestaurantKotSpecialRemarksDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantKotSpecialRemarksDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantKotSpecialRemarksDetail](
	[RemarksDetailId] [int] IDENTITY(1,1) NOT NULL,
	[KotId] [int] NULL,
	[ItemId] [int] NULL,
	[SpecialRemarksId] [int] NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantKotSpecialRemarksDetail] PRIMARY KEY CLUSTERED 
(
	[RemarksDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantKotPendingList]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantKotPendingList]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantKotPendingList](
	[KotPendingListId] [bigint] IDENTITY(1,1) NOT NULL,
	[TableId] [int] NULL,
	[KotId] [bigint] NOT NULL,
	[KotDate] [datetime] NOT NULL,
 CONSTRAINT [PK_RestaurantKotPendingList] PRIMARY KEY CLUSTERED 
(
	[KotPendingListId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantKotBillMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantKotBillMaster](
	[KotId] [int] IDENTITY(1,1) NOT NULL,
	[KotDate] [datetime] NULL,
	[BearerId] [int] NULL,
	[CostCenterId] [int] NULL,
	[SourceName] [varchar](100) NULL,
	[SourceId] [int] NULL,
	[PaxQuantity] [int] NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[IsBillProcessed] [bit] NULL,
	[KotStatus] [varchar](50) NULL,
	[Remarks] [varchar](300) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[TokenNumber] [nvarchar](25) NULL,
	[IsBillHoldup] [bit] NULL,
 CONSTRAINT [PK_KotBillMaster] PRIMARY KEY CLUSTERED 
(
	[KotId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillMaster' AND column_name = 'IsKotReturn')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillMaster
	  ADD IsKotReturn BIT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillMaster' AND column_name = 'ReferenceKotId')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillMaster
	  ADD ReferenceKotId INT NULL
END
GO


SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantKotBillDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantKotBillDetail](
	[KotDetailId] [int] IDENTITY(1,1) NOT NULL,
	[KotId] [int] NULL,
	[ItemType] [varchar](50) NULL,
	[ItemId] [int] NULL,
	[ItemName] [varchar](300) NULL,
	[ItemUnit] [decimal](18, 2) NULL,
	[UnitRate] [decimal](18, 2) NULL,
	[Amount] [decimal](18, 2) NULL,
	[ItemTotalAmount] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 5) NULL,
	[DiscountedAmount] [decimal](18, 5) NULL,
	[ServiceRate] [decimal](18, 5) NULL,
	[ServiceCharge] [decimal](18, 5) NULL,
	[VatAmount] [decimal](18, 5) NULL,
	[CitySDCharge] [decimal](18, 5) NULL,
	[AdditionalCharge] [decimal](18, 5) NULL,
	[PrintFlag] [bit] NULL,
	[IsChanged] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[IsDispatch] [bit] NULL,
	[ItemCost] [decimal](18, 2) NULL,
	[EmpId] [int] NULL,
	[DeliveryStatus] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_KotBillDetail] PRIMARY KEY CLUSTERED 
(
	[KotDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'IsItemReturn')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
	  ADD IsItemReturn BIT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'ReturnQuantity')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
	  ADD ReturnQuantity DECIMAL(18,2) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantSalesReturnItem' AND column_name = 'InvoiceDiscount')
BEGIN
	ALTER TABLE dbo.RestaurantSalesReturnItem
	  ADD InvoiceDiscount DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
		ADD ColorId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
		ADD SizeId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
		ADD StyleId INT NULL
END
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantKitchenCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantKitchenCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantKitchenCostCenterMapping](
	[MappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[KitchenId] [int] NOT NULL,
 CONSTRAINT [PK_RestaurantKitchenCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantKitchen]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantKitchen]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantKitchen](
	[KitchenId] [int] IDENTITY(1,1) NOT NULL,
	[KitchenName] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantKitchen] PRIMARY KEY CLUSTERED 
(
	[KitchenId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantEmpKotBillDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantEmpKotBillDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantEmpKotBillDetail](
	[DetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[BillNumber] [varchar](50) NULL,
	[KotId] [int] NULL,
	[KotDetailId] [int] NULL,
	[JobStartDate] [datetime] NULL,
	[JobEndDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[JobStatus] [varchar](50) NULL,
	[DeliveryDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantEmpKotBillDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantDailySalesStatementConfiguration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantDailySalesStatementConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantDailySalesStatementConfiguration](
	[DailySalesStatementId] [int] IDENTITY(1,1) NOT NULL,
	[DateFrom] [date] NOT NULL,
	[DateTo] [date] NOT NULL,
	[PercentageAmount] [decimal](18, 2) NOT NULL,
	[AmountInPercentage] [decimal](18, 2) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantDailySalesStatementConfiguration] PRIMARY KEY CLUSTERED 
(
	[DailySalesStatementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantCostCenterTableMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantCostCenterTableMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantCostCenterTableMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[TableId] [int] NULL,
	[StatusId] [int] NULL,
 CONSTRAINT [PK_RestaurantCostCenterTableMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantComboDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantComboDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantComboDetail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[ComboId] [int] NULL,
	[ProductId] [int] NULL,
	[ProductUnit] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RestaurantComboDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantComboCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantComboCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantComboCostCenterMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[ComboId] [int] NULL,
	[MinimumStockLevel] [decimal](18, 2) NULL,
	[StockQuantity] [decimal](18, 2) NULL,
	[SellingLocalCurrencyId] [int] NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[SellingUsdCurrencyId] [int] NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RestaurantComboCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantCombo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantCombo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantCombo](
	[ComboId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NULL,
	[ComboName] [varchar](250) NULL,
	[ComboPrice] [decimal](18, 2) NULL,
	[Code] [varchar](20) NULL,
	[ImageName] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantCombo] PRIMARY KEY CLUSTERED 
(
	[ComboId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantBuffetDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBuffetDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBuffetDetail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[BuffetId] [int] NULL,
	[ProductId] [int] NULL,
 CONSTRAINT [PK_RestaurantBuffetDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantBuffetCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBuffetCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBuffetCostCenterMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[BuffetId] [int] NULL,
	[MinimumStockLevel] [decimal](18, 2) NULL,
	[StockQuantity] [decimal](18, 2) NULL,
	[SellingLocalCurrencyId] [int] NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[SellingUsdCurrencyId] [int] NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RestaurantBuffetCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantBuffet]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBuffet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBuffet](
	[BuffetId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NULL,
	[BuffetName] [varchar](250) NULL,
	[BuffetPrice] [decimal](18, 2) NULL,
	[Code] [varchar](20) NULL,
	[ImageName] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantBuffet] PRIMARY KEY CLUSTERED 
(
	[BuffetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantBillPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBillPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBillPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[PaymentType] [varchar](50) NULL,
	[BillId] [int] NULL,
	[RegistrationId] [int] NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentMode] [varchar](20) NULL,
	[FieldId] [int] NULL,
	[CurrencyAmount] [decimal](18, 2) NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[BankId] [int] NULL,
	[BranchName] [varchar](250) NULL,
	[ChecqueNumber] [varchar](50) NULL,
	[ChecqueDate] [datetime] NULL,
	[CardType] [varchar](20) NULL,
	[CardNumber] [varchar](50) NULL,
	[ExpireDate] [datetime] NULL,
	[CardHolderName] [varchar](256) NULL,
	[CardReference] [varchar](256) NULL,
	[RefundAccountHead] [int] NULL,
	[DealId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantBillPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantBillDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBillDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBillDetail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[BillId] [int] NULL,
	[KotId] [int] NULL,
 CONSTRAINT [PK_RestaurantBillDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantBillClassificationDiscount]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBillClassificationDiscount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBillClassificationDiscount](
	[DiscountId] [int] IDENTITY(1,1) NOT NULL,
	[BillId] [int] NULL,
	[ClassificationId] [int] NULL,
	[DiscountAmount] [money] NULL,
 CONSTRAINT [PK_RestaurantBillClassificationDiscount] PRIMARY KEY CLUSTERED 
(
	[DiscountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantBill]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBill]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBill](
	[BillId] [int] IDENTITY(1,1) NOT NULL,
	[BillDate] [datetime] NULL,
	[BillNumber] [varchar](50) NULL,
	[IsBillSettlement] [bit] NULL,
	[IsComplementary] [bit] NULL,
	[IsNonChargeable] [bit] NULL,
	[SourceName] [varchar](100) NULL,
	[BillPaidBySourceId] [int] NULL,
	[CostCenterId] [int] NULL,
	[PaxQuantity] [int] NULL,
	[CustomerName] [varchar](200) NULL,
	[PayMode] [varchar](100) NULL,
	[PayModeSourceId] [int] NULL,
	[PaySourceCurrentBalance] [decimal](18, 2) NULL,
	[BankId] [int] NULL,
	[CardType] [varchar](20) NULL,
	[CardNumber] [varchar](50) NULL,
	[ExpireDate] [datetime] NULL,
	[CardHolderName] [varchar](256) NULL,
	[RegistrationId] [int] NULL,
	[SalesAmount] [decimal](18, 2) NULL,
	[DiscountType] [varchar](50) NULL,
	[DiscountTransactionId] [int] NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[CalculatedDiscountAmount] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[AdditionalChargeType] [varchar](15) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[InvoiceServiceRate] [decimal](18, 2) NULL,
	[IsInvoiceServiceChargeEnable] [bit] NULL,
	[InvoiceServiceCharge] [decimal](18, 2) NULL,
	[IsInvoiceCitySDChargeEnable] [bit] NULL,
	[InvoiceCitySDCharge] [decimal](18, 2) NULL,
	[IsInvoiceVatAmountEnable] [bit] NULL,
	[InvoiceVatAmount] [decimal](18, 2) NULL,
	[IsInvoiceAdditionalChargeEnable] [bit] NULL,
	[InvoiceAdditionalCharge] [decimal](18, 2) NULL,
	[GrandTotal] [decimal](18, 2) NULL,
	[RoundedAmount] [decimal](18, 2) NULL,
	[RoundedGrandTotal] [decimal](18, 2) NULL,
	[BillStatus] [varchar](50) NULL,
	[BillVoidBy] [int] NULL,
	[Remarks] [varchar](300) NULL,
	[Reference] [varchar](500) NULL,
	[DealId] [int] NULL,
	[UserType] [varchar](50) NULL,
	[ApprovedStatus] [bit] NULL,
	[ApprovedDate] [datetime] NULL,
	[IsBillReSettlement] [bit] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[IsLocked] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[TransactionType] [nvarchar](50) NULL,
	[TransactionId] [bigint] NULL,
	[IsBillPreviewButtonEnable] [bit] NULL,
	[ServiceChargeConfig] [decimal](18, 2) NULL,
	[CitySDChargeConfig] [decimal](18, 2) NULL,
	[VatAmountConfig] [decimal](18, 2) NULL,
	[AdditionalChargeConfig] [decimal](18, 2) NULL,
	[CurrencyExchangeRate] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RestaurantBill] PRIMARY KEY CLUSTERED 
(
	[BillId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'BillingType')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD BillingType NVARCHAR(200) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_RestaurantBill_GuidId DEFAULT (newid()) WITH VALUES
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ExchangeItemVatAmount')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD ExchangeItemVatAmount DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ExchangeItemTotal')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD ExchangeItemTotal DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'DeliveredBy')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD DeliveredBy INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'RefundId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD RefundId int NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'RefundRemarks')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD RefundRemarks VARCHAR(250) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ReferenceBillId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD ReferenceBillId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ReturnBillId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD ReturnBillId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ReturnDate')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD ReturnDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'IsReturnToStoreCompleted')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD IsReturnToStoreCompleted INT NOT NULL CONSTRAINT DF_RestaurantBill_IsReturnToStoreCompleted DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ServiceRate')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD ServiceRate DECIMAL(18, 5) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'DiscountedAmount')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD DiscountedAmount DECIMAL(18, 5) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ReferenceBillNumber')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD ReferenceBillNumber VARCHAR(50) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'IsSynced')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD IsSynced INT NOT NULL CONSTRAINT DF_RestaurantBill_IsSynced DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'GLCompanyId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD GLCompanyId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
	  ADD ProjectId INT NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'Remarks' )
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ALTER COLUMN Remarks NVARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'BillDeclaration')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD BillDeclaration NVARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'CustomerMobile')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD CustomerMobile NVARCHAR(200) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'CustomerAddress')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD CustomerAddress NVARCHAR(MAX) NULL
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RestaurantBearer]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBearer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBearer](
	[BearerId] [int] IDENTITY(1,1) NOT NULL,
	[UserInfoId] [int] NULL,
	[BearerPassword] [varchar](50) NULL,
	[CostCenterId] [int] NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[IsBearer] [bit] NULL,
	[IsRestaurantBillCanSettle] [bit] NOT NULL,
	[IsItemCanEditDelete] [bit] NULL,
	[IsItemSearchEnable] [bit] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantBearer] PRIMARY KEY CLUSTERED 
(
	[BearerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PRPOUserPermission]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PRPOUserPermission]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PRPOUserPermission](
	[MappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[UserInfoId] [int] NULL,
	[CostCenterId] [int] NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[ActiveStat] [bit] NULL,
	[IsPRAllow] [bit] NULL,
	[IsPOAllow] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PRPOUserPermission] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PPumpMachineTest]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PPumpMachineTest]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PPumpMachineTest](
	[TestId] [int] IDENTITY(1,1) NOT NULL,
	[TestDate] [datetime] NULL,
	[MachineId] [int] NULL,
	[BeforeMachineReadNumber] [varchar](50) NULL,
	[TestQuantity] [decimal](18, 2) NULL,
	[AfterMachineReadNumber] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [varchar](20) NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [varchar](20) NULL,
 CONSTRAINT [PK_PPumpMachineTest] PRIMARY KEY CLUSTERED 
(
	[TestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
/****** Object:  Table [dbo].[PMSupplierProductReturn]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierProductReturn]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierProductReturn](
	[ReturnId] [int] IDENTITY(1,1) NOT NULL,
	[ReturnDate] [datetime] NULL,
	[ReceivedId] [int] NOT NULL,
	[POrderId] [int] NULL,
	[Status] [nvarchar](15) NULL,
	[Remarks] [varchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSupplierProductReturn] PRIMARY KEY CLUSTERED 
(
	[ReturnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
------------ PMSupplierProductReturn
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'ReturnId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierProductReturn]') AND name = N'PK_PMSupplierProductReturn')
 ALTER TABLE [dbo].[PMSupplierProductReturn] DROP CONSTRAINT [PK_PMSupplierProductReturn]
 
 ALTER TABLE dbo.PMSupplierProductReturn
    ALTER COLUMN ReturnId bigint NOT NULL;
 
 ALTER TABLE [dbo].[PMSupplierProductReturn] ADD  CONSTRAINT [PK_PMSupplierProductReturn] PRIMARY KEY CLUSTERED 
 (
  [ReturnId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'PMSupplierProductReturn.ReturnId' , 'ReturnId', 'COLUMN'
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'ReturnDate')
BEGIN
ALTER TABLE dbo.PMSupplierProductReturn
	   ALTER COLUMN ReturnDate DATETIME NOT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'ReturnNumber')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD ReturnNumber NVARCHAR(25) NOT NULL CONSTRAINT DF_PMSupplierProductReturn_ReturnNumber DEFAULT '' WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'CostCenterId')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD CostCenterId INT NOT NULL CONSTRAINT DF_PMSupplierProductReturn_CostCenterId DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'LocationId')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD LocationId INT NOT NULL CONSTRAINT DF_PMSupplierProductReturn_LocationId DEFAULT 0 WITH VALUES;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'Status' )
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ALTER COLUMN Status NVARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'AchievePoint')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
	  ADD AchievePoint DECIMAL(18,2) NOT NULL CONSTRAINT DF_MemMemberBasics_AchievePoint DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD CheckedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD CheckedDate DATETIME NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD ApprovedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD ApprovedDate DATETIME NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'CreatedBy')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD CreatedBy INT NOT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'ParentTaskId')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD ParentTaskId INT NOT NULL CONSTRAINT DF_SMTask_ParentTaskId DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'DependentTaskId')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD DependentTaskId INT NOT NULL CONSTRAINT DF_SMTask_DependentTaskId DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'HasChild')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD HasChild INT NOT NULL CONSTRAINT DF_SMTask_HasChild DEFAULT 0 WITH VALUES
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'AssignType')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD AssignType NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'EmpDepartment')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD EmpDepartment INT NULL
END
GO
IF (SELECT IS_NULLABLE  FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'EndTime' ) = 'NO'
BEGIN
	ALTER TABLE dbo.SMTask
	ALTER COLUMN EndTime time(7) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'TaskFor')
BEGIN
	EXEC sp_RENAME 'SMTask.TaskType' , 'TaskFor', 'COLUMN'
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'TaskType')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD TaskType NVARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'AccountManagerId')
BEGIN
	ALTER TABLE SMTask  
		ADD AccountManagerId INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'TaskPriority')
BEGIN
	ALTER TABLE SMTask  
		ADD TaskPriority INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE SMTask  
		ADD CompanyId INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'ContactId')
BEGIN
	ALTER TABLE SMTask  
		ADD ContactId INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'DealId')
BEGIN
	ALTER TABLE SMTask  
		ADD DealId INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'ReminderDateFrom')
BEGIN
	ALTER TABLE SMTask  
		ADD ReminderDateFrom DATETIME NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'ReminderDateTo')
BEGIN
	ALTER TABLE SMTask  
		ADD ReminderDateTo DATETIME NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturn' AND column_name = 'CreatedDate')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturn
		ADD CreatedDate DATETIME NOT NULL;
END
GO
/****** Object:  Table [dbo].[PMSupplierProductReturnDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierProductReturnDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierProductReturnDetails](
	[ReturnDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[ReturnId] [int] NOT NULL,
	[CostCenterIdFrom] [int] NULL,
	[LocationIdFrom] [int] NULL,
	[CostCenterIdTo] [int] NULL,
	[LocationIdTo] [int] NULL,
	[StockById] [int] NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PMSupplierProductReturnDetails] PRIMARY KEY CLUSTERED 
(
	[ReturnDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
----PMSupplierProductReturnDetails
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'ReturnDetailsId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierProductReturnDetails]') AND name = N'PK_PMSupplierProductReturnDetails')
 ALTER TABLE [dbo].PMSupplierProductReturnDetails DROP CONSTRAINT [PK_PMSupplierProductReturnDetails]
 
 ALTER TABLE dbo.PMSupplierProductReturnDetails
    ALTER COLUMN ReturnId bigint NOT NULL;
 
 ALTER TABLE [dbo].PMSupplierProductReturnDetails ADD  CONSTRAINT [PK_PMSupplierProductReturnDetails] PRIMARY KEY CLUSTERED 
 (
  [ReturnDetailsId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'PMSupplierProductReturnDetails.ReturnDetailsId' , 'ReturnDetailsId', 'COLUMN'
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'ReturnId')
BEGIN
ALTER TABLE dbo.PMSupplierProductReturnDetails
	   ALTER COLUMN ReturnId BIGINT NOT NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'CostCenterIdFrom')
BEGIN
ALTER TABLE dbo.PMSupplierProductReturnDetails
	   DROP COLUMN CostCenterIdFrom;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'LocationIdFrom')
BEGIN
ALTER TABLE dbo.PMSupplierProductReturnDetails
	   DROP COLUMN LocationIdFrom;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'CostCenterIdTo')
BEGIN
ALTER TABLE dbo.PMSupplierProductReturnDetails
	   DROP COLUMN CostCenterIdTo;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'LocationIdTo')
BEGIN
ALTER TABLE dbo.PMSupplierProductReturnDetails
	   DROP COLUMN LocationIdTo;
END
GO
IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'StockById')
BEGIN
	IF NOT EXISTS(SELECT *   FROM sys.all_columns c
					  JOIN sys.tables t on t.object_id = c.object_id
					  JOIN sys.schemas s on s.schema_id = t.schema_id
					  JOIN sys.default_constraints d on c.default_object_id = d.object_id
					WHERE t.name = 'PMSupplierProductReturnDetails'
					  AND c.name = 'StockById'
					  AND s.name = 'dbo')
	  BEGIN
		ALTER TABLE dbo.PMSupplierProductReturnDetails
			ADD CONSTRAINT DF_PMSupplierProductReturn_StockById DEFAULT 0 FOR StockById;
		ALTER TABLE dbo.PMSupplierProductReturnDetails
				ALTER COLUMN StockById INT NOT NULL;
		END
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'OrderQuantity')
BEGIN	
		
	ALTER TABLE dbo.PMSupplierProductReturnDetails
		ADD OrderQuantity DECIMAL(18, 2) NOT NULL DEFAULT 0 WITH VALUES;	
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'OrderQuantity')
BEGIN
	IF NOT EXISTS(SELECT *   FROM sys.all_columns c
					  JOIN sys.tables t on t.object_id = c.object_id
					  JOIN sys.schemas s on s.schema_id = t.schema_id
					  JOIN sys.default_constraints d on c.default_object_id = d.object_id
					WHERE t.name = 'PMSupplierProductReturnDetails'
					  AND c.name = 'OrderQuantity'
					  AND s.name = 'dbo')
	BEGIN
		
		ALTER TABLE dbo.PMSupplierProductReturnDetails
			ADD  CONSTRAINT DF_PMSupplierProductReturnDetails_OrderQuantity DEFAULT 0 FOR OrderQuantity;
		ALTER TABLE dbo.PMSupplierProductReturnDetails
			ALTER COLUMN OrderQuantity DECIMAL(18, 2) NOT NULL;
	END
	
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'ProductId')
BEGIN
	EXEC sp_RENAME 'PMSupplierProductReturnDetails.ProductId' , 'ItemId', 'COLUMN'
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'ReturnDetailsId' AND DATA_TYPE = 'int')
BEGIN
 IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierProductReturnDetails]') AND name = N'PK_PMSupplierProductReturnDetails')
	BEGIN
		ALTER TABLE [dbo].[PMSupplierProductReturnDetails] DROP CONSTRAINT [PK_PMSupplierProductReturnDetails]
	END
 
 ALTER TABLE dbo.PMSupplierProductReturnDetails
    ALTER COLUMN ReturnDetailsId BIGINT NOT NULL;
 
 ALTER TABLE [dbo].[PMSupplierProductReturnDetails] ADD  CONSTRAINT [PK_PMSupplierProductReturnDetails] PRIMARY KEY CLUSTERED 
 (
  [ReturnDetailsId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturnDetails
		ADD ColorId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturnDetails
		ADD SizeId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierProductReturnDetails' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.PMSupplierProductReturnDetails
		ADD StyleId INT NULL
END
GO

----New Table Creation

/****** Object:  Table [dbo].[PMProductReturnSerial]    Script Date: 11/29/2018 05:12:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierProductReturnSerial]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierProductReturnSerial](
	[ReturnSerialId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReturnId] [bigint] NOT NULL,
	[ItemId] [int] NOT NULL,
	[SerialNumber] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PMSupplierProductReturnSerial] PRIMARY KEY CLUSTERED 
(
	[ReturnSerialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMSupplierPaymentLedgerClosingMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierPaymentLedgerClosingMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierPaymentLedgerClosingMaster](
	[YearClosingId] [bigint] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[ProfitLossClosing] [money] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSupplierPaymentLedgerClosingMaster] PRIMARY KEY CLUSTERED 
(
	[YearClosingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PMSupplierPaymentLedgerClosingDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierPaymentLedgerClosingDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierPaymentLedgerClosingDetails](
	[ClosingBalanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[YearClosingId] [bigint] NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[NodeId] [bigint] NOT NULL,
	[NodeHead] [nvarchar](250) NOT NULL,
	[ClosingDRAmount] [money] NOT NULL,
	[ClosingCRAmount] [money] NOT NULL,
	[ClosingBalance] [money] NOT NULL,
 CONSTRAINT [PK_PMSupplierPaymentLedgerClosingDetails] PRIMARY KEY CLUSTERED 
(
	[ClosingBalanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PMSupplierPaymentLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierPaymentLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierPaymentLedger](
	[SupplierPaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentType] [nvarchar](15) NOT NULL,
	[LedgerNumber] [varchar](25) NULL,
	[BillId] [int] NULL,
	[BillNumber] [varchar](25) NULL,
	[RefSupplierPaymentId] [bigint] NULL,
	[PaymentDate] [date] NOT NULL,
	[SupplierId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[CurrencyAmount] [money] NULL,
	[AccountsPostingHeadId] [bigint] NULL,
	[DueAmount] [money] NULL,
	[AdvanceAmount] [money] NULL,
	[AdvanceAmountRemaining] [money] NULL,
	[IsBillGenerated] [bit] NULL,
	[Remarks] [varchar](500) NULL,
	[ChequeNumber] [varchar](100) NULL,
	[PaymentStatus] [varchar](20) NULL,
	[RefPaymentId] [bigint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSupplierLedgerMaster] PRIMARY KEY CLUSTERED 
(
	[SupplierPaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierPaymentLedger' AND column_name = 'ItemReturnTotalAmount')
BEGIN
	ALTER TABLE dbo.PMSupplierPaymentLedger
		ADD ItemReturnTotalAmount MONEY NULL  DEFAULT 0 WITH VALUES
END
GO
/****** Object:  Table [dbo].[PMSupplierPaymentDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierPaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierPaymentDetails](
	[PaymentDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentId] [bigint] NOT NULL,
	[SupplierPaymentId] [bigint] NOT NULL,
	[BillId] [bigint] NOT NULL,
	[PaymentAmount] [money] NOT NULL,
 CONSTRAINT [PK_PMSupplierPaymentDetails] PRIMARY KEY CLUSTERED 
(
	[PaymentDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PMSupplierPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierPayment](
	[PaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentFor] [nvarchar](50) NULL,
	[AdjustmentType] [nvarchar](25) NULL,
	[SupplierPaymentAdvanceId] [bigint] NULL,
	[LedgerNumber] [nvarchar](50) NOT NULL,
	[PaymentDate] [date] NOT NULL,
	[SupplierId] [int] NOT NULL,
	[AdvanceAmount] [money] NULL,
	[Remarks] [nvarchar](250) NULL,
	[PaymentType] [nvarchar](50) NULL,
	[AccountingPostingHeadId] [int] NULL,
	[ChequeNumber] [nvarchar](50) NULL,
	[CurrencyId] [int] NULL,
	[ConvertionRate] [decimal](18, 2) NULL,
	[ApprovedStatus] [nvarchar](50) NULL,
	[AdjustmentAmount] [money] NULL,
	[AdjustmentAccountHeadId] [int] NULL,
	[PaymentAdjustmentAmount] [money] NULL,
 CONSTRAINT [PK_PMSupplierPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierPayment' AND column_name = 'ChecqueDate')
BEGIN
	ALTER TABLE dbo.PMSupplierPayment
		ADD ChecqueDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierPayment' AND column_name = 'CreatedBy')
BEGIN
	ALTER TABLE dbo.PMSupplierPayment
		ADD CreatedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierPayment' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.PMSupplierPayment
		ADD CheckedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierPayment' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.PMSupplierPayment
		ADD ApprovedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierPayment' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PMSupplierPayment
		ADD CheckedByUsers VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplierPayment' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PMSupplierPayment
		ADD ApprovedByUsers VARCHAR(100) NULL
END
GO
/****** Object:  Table [dbo].[PMSupplier]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplier]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplier](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
	[Name] [varchar](50) NOT NULL,
	[Code] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[WebAddress] [varchar](100) NULL,
	[ContactPerson] [varchar](100) NULL,
	[ContactEmail] [varchar](100) NULL,
	[ContactPhone] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[Balance] [decimal](18, 2) NULL,
	[IsAdhocSupplier] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSupplier] PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'NAME' )
BEGIN
	ALTER TABLE dbo.PMSupplier
		ALTER COLUMN NAME VARCHAR(MAX) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'Address' )
BEGIN
	ALTER TABLE dbo.PMSupplier
		ALTER COLUMN Address VARCHAR(MAX) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'Email' )
BEGIN
	ALTER TABLE dbo.PMSupplier
		ALTER COLUMN Email VARCHAR(MAX) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'WebAddress' )
BEGIN
	ALTER TABLE dbo.PMSupplier
		ALTER COLUMN WebAddress VARCHAR(500) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'ContactPerson' )
BEGIN
	ALTER TABLE dbo.PMSupplier
		ALTER COLUMN ContactPerson VARCHAR(MAX) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'ContactEmail' )
BEGIN
	ALTER TABLE dbo.PMSupplier
		ALTER COLUMN ContactEmail VARCHAR(MAX) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'Remarks' )
BEGIN
	ALTER TABLE dbo.PMSupplier
		ALTER COLUMN Remarks VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMSupplier' AND column_name = 'SupplierTypeId')
BEGIN
	ALTER TABLE dbo.PMSupplier
		ADD SupplierTypeId VARCHAR(50) NULL
END
GO
/****** Object:  Table [dbo].[PMSalesTechnicalInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSalesTechnicalInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSalesTechnicalInfo](
	[TechnicalInfoId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[TechnicalContactPerson] [varchar](100) NULL,
	[TechnicalPersonDepartment] [varchar](100) NULL,
	[TechnicalPersonDesignation] [varchar](100) NULL,
	[TechnicalPersonPhone] [varchar](100) NULL,
	[TechnicalPersonEmail] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSalesTechnicalInfo] PRIMARY KEY CLUSTERED 
(
	[TechnicalInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMSalesSiteInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSalesSiteInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSalesSiteInfo](
	[SiteInfoId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[SiteId] [varchar](20) NULL,
	[SiteName] [varchar](100) NULL,
	[SiteAddress] [varchar](500) NULL,
	[SiteContactPerson] [varchar](100) NULL,
	[SitePhoneNumber] [varchar](20) NULL,
	[SiteEmail] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSalesSiteInfo] PRIMARY KEY CLUSTERED 
(
	[SiteInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMSalesInvoice]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSalesInvoice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSalesInvoice](
	[InvoiceId] [bigint] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [varchar](20) NULL,
	[BillFromDate] [datetime] NULL,
	[BillToDate] [datetime] NULL,
	[SalesId] [bigint] NULL,
	[InvoiceAmount] [decimal](18, 2) NULL,
	[DueOrAdvanceAmount] [decimal](18, 2) NULL,
	[BillDueDate] [datetime] NULL,
	[InvoiceDetailId] [varchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSalesInvoice] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMSalesDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSalesDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSalesDetail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[SalesId] [int] NULL,
	[ServiceType] [varchar](50) NULL,
	[ItemId] [int] NULL,
	[ItemUnit] [decimal](18, 2) NULL,
	[SellingLocalCurrencyId] [int] NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[SellingUsdCurrencyId] [int] NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PMSalesDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMSalesBillPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSalesBillPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSalesBillPayment](
	[PaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[PaymentType] [varchar](50) NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentLocalAmount] [decimal](18, 2) NULL,
	[FieldId] [int] NULL,
	[NodeId] [int] NULL,
	[CurrencyAmount] [decimal](18, 2) NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[BankId] [int] NULL,
	[BranchName] [varchar](250) NULL,
	[ChecqueNumber] [varchar](50) NULL,
	[ChecqueDate] [datetime] NULL,
	[CardType] [varchar](20) NULL,
	[CardNumber] [varchar](50) NULL,
	[ExpireDate] [datetime] NULL,
	[CardHolderName] [varchar](256) NULL,
	[CardReference] [varchar](256) NULL,
	[DealId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[PaymentMode] [varchar](20) NULL,
 CONSTRAINT [PK_PMSalesBillPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMSalesBillingInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSalesBillingInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSalesBillingInfo](
	[BillingInfoId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[BillingContactPerson] [varchar](100) NULL,
	[BillingPersonDepartment] [varchar](100) NULL,
	[BillingPersonDesignation] [varchar](100) NULL,
	[BillingPersonPhone] [varchar](100) NULL,
	[BillingPersonEmail] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMSalesBillingInfo] PRIMARY KEY CLUSTERED 
(
	[BillingInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMSales]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSales]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSales](
	[SalesId] [int] IDENTITY(1,1) NOT NULL,
	[BillNumber] [varchar](20) NULL,
	[SalesDate] [datetime] NULL,
	[CustomerId] [int] NULL,
	[SiteInfoId] [int] NULL,
	[BillingInfoId] [int] NULL,
	[TechnicalInfoId] [int] NULL,
	[SalesAmount] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[GrandTotal] [decimal](18, 2) NULL,
	[FieldId] [int] NULL,
	[Frequency] [varchar](20) NULL,
	[DueAmount] [decimal](18, 2) NULL,
	[BillExpireDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
 CONSTRAINT [PK_PMSales] PRIMARY KEY CLUSTERED 
(
	[SalesId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMRequisition]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMRequisition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMRequisition](
	[RequisitionId] [int] IDENTITY(1,1) NOT NULL,
	[PRNumber] [varchar](20) NULL,
	[CostCenterId] [int] NOT NULL,
	[ReceivedByDate] [datetime] NOT NULL,
	[RequisitionBy] [varchar](50) NULL,
	[ApprovedStatus] [varchar](20) NULL,
	[DelivaredStatus] [varchar](50) NULL,
	[DelivarOutStatus] [varchar](50) NULL,
	[Remarks] [varchar](300) NULL,
	[CheckedBy] [int] NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMRequisition] PRIMARY KEY CLUSTERED 
(
	[RequisitionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

---renamed column name CostCenterId to FromCostCenterId
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'CostCenterId')
BEGIN
	EXEC sp_RENAME 'PMRequisition.CostCenterId' , 'FromCostCenterId', 'COLUMN'
END
GO

---add column ToCostCenterId
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'ToCostCenterId')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD ToCostCenterId INT NOT NULL CONSTRAINT DF_PMRequisition_ToCostCenterId DEFAULT 0 WITH VALUES;
END
GO
---add column POStatus
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'POStatus')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD POStatus nvarchar(25) NULL;
END
GO
---add column ReceiveStatus
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'ReceiveStatus')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD ReceiveStatus nvarchar(25) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'TransferStatus')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD TransferStatus nvarchar(25) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD CompanyId BIGINT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD ProjectId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'ApprovedStatus' )
BEGIN
	ALTER TABLE dbo.PMRequisition
		ALTER COLUMN ApprovedStatus NVARCHAR(50) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'LocationFrom')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD LocationFrom INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'LocationTo')
BEGIN
	ALTER TABLE dbo.PMRequisition
		ADD LocationTo INT NULL
END
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMRequisitionDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMRequisitionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMRequisitionDetails](
	[RequisitionDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[RequisitionId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[StockById] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[ApprovedQuantity] [decimal](18, 2) NULL,
	[DeliveredQuantity] [decimal](18, 2) NULL,
	[DelivarOutQuantity] [decimal](18, 2) NOT NULL,
	[ApprovedStatus] [varchar](20) NULL,
 CONSTRAINT [PK_PMRequisitionDetails] PRIMARY KEY CLUSTERED 
(
	[RequisitionDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
--Remarks column added 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD Remarks NVARCHAR(150) NULL;
END
GO

---- Add Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'ApprovedPOQuantity')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD ApprovedPOQuantity DECIMAL(18,2) null;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'RemainingPOQuantity')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD RemainingPOQuantity DECIMAL(18,2) null;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'ApprovedReceiveQuantity')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD ApprovedReceiveQuantity DECIMAL(18,2) null;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'RemainingReceiveQuantity')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD RemainingReceiveQuantity DECIMAL(18,2) null;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'ApprovedTransferQuantity')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD ApprovedTransferQuantity DECIMAL(18,2) null;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'AveragePrice')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD AveragePrice decimal(18, 2) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'RemainingTransferQuantity')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD RemainingTransferQuantity DECIMAL(18,2) null;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD ColorId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD SizeId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.PMRequisitionDetails
		ADD StyleId INT NULL
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'RemainingPOQuantity')
BEGIN
	IF EXISTS (SELECT TOP(1) RequisitionId FROM PMRequisitionDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				update PMRequisitionDetails
				set RemainingPOQuantity = ApprovedQuantity,
					RemainingReceiveQuantity = ApprovedQuantity,
					RemainingTransferQuantity = ApprovedQuantity
				'
		END
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'RemainingPOQuantity')
BEGIN
	IF EXISTS (SELECT TOP(1) RequisitionId FROM PMRequisitionDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				update rd 
				set 
					rd.RemainingPOQuantity = (rd.RemainingPOQuantity - pod.Quantity),  
					rd.ApprovedPOQuantity = pod.Quantity 
				from
				(
					select RequisitionId, ItemId, sum(Quantity) Quantity,
						   ROW_NUMBER() OVER(ORDER BY RequisitionId, ItemId)Rnk
					from PMPurchaseOrderDetails
					where RequisitionId > 0
					group by RequisitionId, ItemId
				)pod inner join PMRequisitionDetails rd ON pod.RequisitionId = rd.RequisitionId and pod.ItemId = rd.ItemId
				'
		END
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'RemainingPOQuantity')
BEGIN
	IF EXISTS (SELECT TOP(1) RequisitionId FROM PMRequisitionDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE r
				SET r.POStatus = CASE WHEN TotalNotPurchaseItemQuantity = 0 THEN ''Full'' ELSE ''Partial'' END
				FROM PMRequisition r 
				INNER JOIN
				(
					SELECT req.RequisitionId, (req.ApprovedQuantity - req.ApprovedPOQuantity) TotalNotPurchaseItemQuantity
					FROM
					(
						SELECT rd.RequisitionId, SUM(rd.ApprovedQuantity)ApprovedQuantity, SUM(ISNULL(rd.ApprovedPOQuantity, 0)) ApprovedPOQuantity
						FROM 
						PMRequisitionDetails rd INNER JOIN 
						(
							select RequisitionId, ItemId, sum(Quantity) Quantity
							from PMPurchaseOrderDetails
							where RequisitionId > 0
							group by RequisitionId, ItemId
						) po ON rd.RequisitionId = po.RequisitionId AND rd.ItemId = po.ItemId
						GROUP BY rd.RequisitionId
					)req

				)rr ON r.RequisitionId = rr.RequisitionId
				'
		END
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisitionDetails' AND column_name = 'RemainingTransferQuantity')
BEGIN
	IF EXISTS (SELECT TOP(1) RequisitionId FROM PMRequisitionDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				update rd 
				set 
					rd.RemainingTransferQuantity = (rd.RemainingTransferQuantity - pod.Quantity),  
					rd.ApprovedTransferQuantity = pod.Quantity 
				from
				(
					SELECT po.RequisitionOrSalesId, pod.ItemId, SUM(pod.Quantity)Quantity
					FROM 
					PMProductOut po INNER JOIN PMProductOutDetails pod ON po.OutId = pod.OutId
					WHERE po.RequisitionOrSalesId > 0
					GROUP BY po.RequisitionOrSalesId, pod.ItemId
				)pod inner join PMRequisitionDetails rd ON pod.RequisitionOrSalesId = rd.RequisitionId and pod.ItemId = rd.ItemId				
				'
		END
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMRequisition' AND column_name = 'TransferStatus')
BEGIN
	IF EXISTS (SELECT TOP(1) RequisitionId FROM PMRequisitionDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
			UPDATE r
			SET r.TransferStatus = CASE WHEN TotalNotPurchaseItemQuantity = 0 THEN ''Full'' ELSE ''Partial'' END
			FROM PMRequisition r 
			INNER JOIN 
			(
				SELECT req.RequisitionId, (req.ApprovedQuantity - req.ApprovedTransferQuantity) TotalNotPurchaseItemQuantity
				FROM
				(
					SELECT rd.RequisitionId, SUM(rd.ApprovedQuantity)ApprovedQuantity, SUM(ISNULL(rd.ApprovedTransferQuantity, 0)) ApprovedTransferQuantity
					FROM 
					PMRequisitionDetails rd INNER JOIN 
					(
						SELECT po.RequisitionOrSalesId, pod.ItemId, SUM(pod.Quantity)Quantity
						FROM 
						PMProductOut po INNER JOIN PMProductOutDetails pod ON po.OutId = pod.OutId
						WHERE po.RequisitionOrSalesId > 0
						GROUP BY po.RequisitionOrSalesId, pod.ItemId

					) po ON rd.RequisitionId = po.RequisitionOrSalesId AND rd.ItemId = po.ItemId
					GROUP BY rd.RequisitionId
				)req

			)rr ON r.RequisitionId = rr.RequisitionId								
			'
		END
END
GO

--- Remove Existing Table Column CostCenterId and Migrate Data From Details Table To Purchase Order Table
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'CostCenterId')
BEGIN
	IF EXISTS (SELECT TOP(1) POrderId FROM PMPurchaseOrderDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE po
				SET po.CostCenterId = pod.CostCenterId
				FROM PMPurchaseOrder po
				INNER JOIN
				(	SELECT POrderId, MAX(CostCenterId)  CostCenterId
					FROM PMPurchaseOrderDetails
					GROUP BY POrderId
				)pod ON po.POrderId = pod.POrderId
				'
		END

	 ALTER TABLE PMPurchaseOrderDetails DROP COLUMN CostCenterId
END
GO


SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMPurchaseOrder]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMPurchaseOrder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMPurchaseOrder](
	[POrderId] [int] IDENTITY(1,1) NOT NULL,
	[PODate] [datetime] NULL,
	[ReceivedByDate] [datetime] NULL,
	[PONumber] [varchar](50) NULL,
	[POType] [varchar](20) NULL,
	[IsLocalOrForeignPO] [varchar](50) NULL,
	[SupplierId] [int] NULL,
	[ApprovedStatus] [varchar](20) NULL,
	[ReceivedStatus] [varchar](50) NULL,
	[Remarks] [varchar](250) NULL,
	[CheckedBy] [int] NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMPurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[POrderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
--- Add Alter Purchase Order Table CostCenterId, CurrencyId, ConvertionRate
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'CostCenterId')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ADD CostCenterId INT NOT NULL CONSTRAINT DF_PMPurchaseOrder_CostCenterId DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'CurrencyId')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ADD CurrencyId BIGINT NOT NULL CONSTRAINT DF_PMPurchaseOrder_CurrencyId DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'ConvertionRate')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ADD ConvertionRate MONEY NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'ReceiveStatus')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ADD ReceiveStatus NVARCHAR(25) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'OrderType')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ADD OrderType VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'DeliveryAddress')
BEGIN	
		
	ALTER TABLE dbo.PMPurchaseOrder
		ADD DeliveryAddress VARCHAR(MAX) NULL	
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'CompanyId')
BEGIN	
	ALTER TABLE dbo.PMPurchaseOrder
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'PODescription')
BEGIN	
		
	ALTER TABLE dbo.PMPurchaseOrder
		ADD PODescription VARCHAR(MAX) NULL	
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'Remarks')
BEGIN
ALTER TABLE dbo.PMPurchaseOrder
   ALTER COLUMN Remarks VARCHAR(MAX) NULL
END 
GO
/****** Object:  Table [dbo].[PMPurchaseOrderDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMPurchaseOrderDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMPurchaseOrderDetails](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[POrderId] [int] NULL,
	[RequisitionId] [int] NULL,
	[CostCenterId] [int] NULL,
	[StockById] [int] NULL,
	[ProductId] [int] NULL,
	[PurchasePrice] [decimal](18, 2) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[QuantityReceived] [decimal](18, 2) NULL,
	[MessureUnit] [varchar](20) NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[ActualReceivedQuantity] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PMPurchaseOrderDetails] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

-- Rename column name ProductId to ItemId, Add Column Remarks
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'ProductId')
BEGIN
	EXEC sp_RENAME 'PMPurchaseOrderDetails.ProductId' , 'ItemId', 'COLUMN'
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrderDetails
		ADD Remarks NVARCHAR(250) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'RemainingReceiveQuantity')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrderDetails
		ADD RemainingReceiveQuantity DECIMAL(18,5) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'PurchasePrice')
BEGIN
ALTER TABLE dbo.PMPurchaseOrderDetails
   ALTER COLUMN PurchasePrice DECIMAL(18,5) NULL; 
END 
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'Remarks')
BEGIN
ALTER TABLE dbo.PMPurchaseOrderDetails
   ALTER COLUMN Remarks VARCHAR(MAX) NULL
END 
GO
--- Remove Existing Table Column CostCenterId and Migrate Data From Details Table To Purchase Order Table
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'CostCenterId')
BEGIN
	IF EXISTS (SELECT TOP(1) POrderId FROM PMPurchaseOrderDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE po
				SET po.CostCenterId = pod.CostCenterId
				FROM PMPurchaseOrder po
				INNER JOIN
				(	SELECT POrderId, MAX(CostCenterId)  CostCenterId
					FROM PMPurchaseOrderDetails
					GROUP BY POrderId
				)pod ON po.POrderId = pod.POrderId
				'
		END

	 ALTER TABLE PMPurchaseOrderDetails DROP COLUMN CostCenterId
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'RequisitionId')
BEGIN
	IF EXISTS (SELECT TOP(1) POrderId FROM PMPurchaseOrderDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE po
				SET po.POType = CASE WHEN RequisitionId > 0 THEN ''Requisition'' ELSE ''AdHoc'' END 
				FROM PMPurchaseOrder po
				INNER JOIN
				(	SELECT POrderId, MAX(RequisitionId)  RequisitionId
					FROM PMPurchaseOrderDetails
					GROUP BY POrderId
				)pod ON po.POrderId = pod.POrderId
				'
		END
END
GO
---------Receive Status By Purchase Order Wise Receive
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'RemainingReceiveQuantity')
BEGIN
	IF EXISTS (SELECT TOP(1) POrderId FROM PMPurchaseOrderDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'				
				update PMPurchaseOrderDetails
				set RemainingReceiveQuantity = Quantity
				'
		END
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'RemainingReceiveQuantity')
BEGIN
	IF EXISTS (SELECT TOP(1) POrderId FROM PMPurchaseOrderDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				update rd 
				set 
					rd.RemainingReceiveQuantity = (rd.RemainingReceiveQuantity - pod.Quantity),  
					rd.QuantityReceived = pod.Quantity 
				from
				(
					SELECT po.POrderId, pod.ItemId, SUM(pod.Quantity)Quantity
					FROM 
					PMProductReceived po INNER JOIN PMProductReceivedDetails pod ON po.ReceivedId = pod.ReceivedId
					WHERE po.POrderId > 0
					GROUP BY po.POrderId, pod.ItemId
				)pod inner join PMPurchaseOrderDetails rd ON pod.POrderId = rd.POrderId and pod.ItemId = rd.ItemId
				'
		END
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'ReceiveStatus')
BEGIN
	IF EXISTS (SELECT TOP(1) POrderId FROM PMPurchaseOrderDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE r
				SET r.ReceiveStatus = CASE WHEN TotalNotPurchaseItemQuantity = 0 THEN ''Full'' ELSE ''Partial'' END
				FROM PMPurchaseOrder r 
				INNER JOIN 
				(
					SELECT req.POrderId, (req.ApprovedQuantity - req.ApprovedTransferQuantity) TotalNotPurchaseItemQuantity
					FROM
					(
						SELECT rd.POrderId, SUM(rd.Quantity)ApprovedQuantity, SUM(ISNULL(rd.QuantityReceived, 0)) ApprovedTransferQuantity
						FROM 
						PMPurchaseOrderDetails rd INNER JOIN 
						(
							SELECT po.POrderId, pod.ItemId, SUM(pod.Quantity)Quantity
							FROM 
							PMProductReceived po INNER JOIN PMProductReceivedDetails pod ON po.ReceivedId = pod.ReceivedId
							WHERE po.POrderId > 0
							GROUP BY po.POrderId, pod.ItemId

						) po ON rd.POrderId = po.POrderId AND rd.ItemId = po.ItemId
						GROUP BY rd.POrderId
					)req

				)rr ON r.POrderId = rr.POrderId
				'
		END
END
GO
/****** Object:  Table [dbo].[PMProductSerialInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductSerialInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductSerialInfo](
	[SerialId] [int] IDENTITY(1,1) NOT NULL,
	[ReceivedId] [int] NULL,
	[ReceiveDetailsId] [int] NULL,
	[POrderId] [int] NULL,
	[ProductId] [int] NULL,
	[SerialNumber] [varchar](50) NULL,
	[SalesId] [int] NULL,
	[SerialStatus] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_PMProductSerialInfo] PRIMARY KEY CLUSTERED 
(
	[SerialId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

-----Rename Table Column PMProductSerialInfo
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductSerialInfo' AND column_name = 'ProductId')
BEGIN
	EXEC sp_RENAME 'PMProductSerialInfo.ProductId' , 'ItemId', 'COLUMN'
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductSerialInfo' AND column_name = 'ReturnStatus')
BEGIN
	ALTER TABLE dbo.PMProductSerialInfo
		ADD ReturnStatus NVARCHAR(25) NULL;
END
GO
/****** Object:  Table [dbo].[PMProductReturn]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReturn]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReturn](
	[ReturnId] [int] IDENTITY(1,1) NOT NULL,
	[ReturnDate] [datetime] NULL,
	[ReturnType] [varchar](20) NULL,
	[TransactionId] [int] NULL,
	[ProductId] [int] NULL,
	[SerialNumber] [varchar](50) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_PMProductReturn] PRIMARY KEY CLUSTERED 
(
	[ReturnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Added Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'ReturnNumber')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD ReturnNumber NVARCHAR(25) NOT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'FromCostCenterId')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD FromCostCenterId INT NOT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'FromLocationId')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD FromLocationId INT NOT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'Status')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD [Status] NVARCHAR(25) NOT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD CheckedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD CheckedDate DATETIME NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD ApprovedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD ApprovedDate DATETIME NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD LastModifiedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'LastModifiedDate')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD LastModifiedDate DATETIME NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'Status' )
BEGIN
	ALTER TABLE dbo.PMProductReturn
		ALTER COLUMN Status NVARCHAR(50) NULL;
END
GO


---- ALter Column Data Type

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'ReturnId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReturn]') AND name = N'PK_PMProductReturn')
 ALTER TABLE [dbo].[PMProductReturn] DROP CONSTRAINT [PK_PMProductReturn]
 
 ALTER TABLE dbo.PMProductReturn
    ALTER COLUMN ReturnId bigint NOT NULL;
 
 ALTER TABLE [dbo].[PMProductReturn] ADD  CONSTRAINT [PK_PMProductReturn] PRIMARY KEY CLUSTERED 
 (
  [ReturnId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'PMProductReturn.ReturnId' , 'ReturnId', 'COLUMN'
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'TransactionId' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.PMProductReturn
	   ALTER COLUMN TransactionId bigint NOT NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'ReturnDate')
BEGIN
ALTER TABLE dbo.PMProductReturn
	   ALTER COLUMN ReturnDate DATETIME NOT NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'ReturnType')
BEGIN
ALTER TABLE dbo.PMProductReturn
	   ALTER COLUMN ReturnType NVARCHAR(25) NOT NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'SerialNumber')
BEGIN
ALTER TABLE dbo.PMProductReturn
	   DROP COLUMN SerialNumber;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturn' AND column_name = 'Quantity')
BEGIN
ALTER TABLE dbo.PMProductReturn
	   DROP COLUMN Quantity;
END
GO

/****** Object:  Table [dbo].[PMProductReturnDetails]    Script Date: 05/12/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReturnDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReturnDetails](
	[ReturnDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReturnId] [bigint] NOT NULL,
	[StockById] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[OrderQuantity] [decimal](18, 2) NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PMProductReturnDetails] PRIMARY KEY CLUSTERED 
(
	[ReturnDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturnDetails' AND column_name = 'AverageCost')
BEGIN
ALTER TABLE dbo.PMProductReturnDetails
	   ALTER COLUMN AverageCost DECIMAL(18,5) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturnDetails' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.PMProductReturnDetails
		ADD ColorId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturnDetails' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.PMProductReturnDetails
		ADD SizeId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReturnDetails' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.PMProductReturnDetails
		ADD StyleId INT NULL
END
GO
/****** Object:  Table [dbo].[PMProductReturnSerial]    Script Date: 11/29/2018 05:12:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReturnSerial]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReturnSerial](
	[ReturnSerialId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReturnId] [bigint] NOT NULL,
	[ItemId] [int] NOT NULL,
	[SerialNumber] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PMProductReturnSerial] PRIMARY KEY CLUSTERED 
(
	[ReturnSerialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PMProductReceivedBillPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReceivedBillPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReceivedBillPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[BillNumber] [varchar](50) NULL,
	[PaymentType] [varchar](50) NULL,
	[ReceivedId] [int] NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentMode] [varchar](20) NULL,
	[FieldId] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[CurrencyAmount] [decimal](18, 2) NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[BankId] [int] NULL,
	[BranchName] [varchar](250) NULL,
	[ChecqueNumber] [varchar](50) NULL,
	[ChecqueDate] [datetime] NULL,
	[CardType] [varchar](20) NULL,
	[CardNumber] [varchar](50) NULL,
	[ExpireDate] [datetime] NULL,
	[CardHolderName] [varchar](256) NULL,
	[CardReference] [varchar](256) NULL,
	[AccountsPostingHeadId] [int] NULL,
	[RefundAccountHead] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[DealId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMProductReceivedBillPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedBillPayment' AND column_name = 'ConversionRate')
BEGIN
ALTER TABLE dbo.PMProductReceivedBillPayment
	   ALTER COLUMN ConversionRate DECIMAL(18,5) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedBillPayment' AND column_name = 'CurrencyAmount')
BEGIN
ALTER TABLE dbo.PMProductReceivedBillPayment
	   ALTER COLUMN CurrencyAmount DECIMAL(18,5) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedBillPayment' AND column_name = 'PaymentAmount')
BEGIN
ALTER TABLE dbo.PMProductReceivedBillPayment
	   ALTER COLUMN PaymentAmount DECIMAL(18,5) NULL;
END
GO
/****** Object:  Table [dbo].[PMProductReceived]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReceived]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReceived](
	[ReceivedId] [int] IDENTITY(1,1) NOT NULL,
	[ReceiveNumber] [varchar](20) NULL,
	[ReceivedDate] [datetime] NULL,
	[POrderId] [int] NOT NULL,
	[SupplierId] [int] NULL,
	[Status] [varchar](50) NULL,
	[Reason] [varchar](150) NULL,
	[ReferenceNumber] [varchar](100) NULL,
	[PurchaseBy] [varchar](250) NULL,
	[IsApprovedForPosting] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMProductReceived] PRIMARY KEY CLUSTERED 
(
	[ReceivedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

-----Alter Table Column ReceivedDate Null to Not Null
ALTER TABLE PMProductReceived ALTER COLUMN ReceivedDate DATETIME NOT NULL

-----Alter Table Column ReceivedDate Null to Not Null
ALTER TABLE PMProductReceived ALTER COLUMN CreatedBy INT NOT NULL

---- Add Table Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ReceiveType')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD ReceiveType VARCHAR(50) NOT NULL CONSTRAINT DF_PMProductReceived_ReceiveType DEFAULT '' WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'CostCenterId')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD CostCenterId INT NOT NULL CONSTRAINT DF_PMProductReceived_CostCenterId DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'LocationId')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD LocationId INT NOT NULL CONSTRAINT DF_PMProductReceived_LocationId DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'SupplierId')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD SupplierId INT NOT NULL CONSTRAINT DF_PMProductReceived_SupplierId DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'CurrencyId')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD CurrencyId INT NOT NULL CONSTRAINT DF_PMProductReceived_CurrencyId DEFAULT 0 WITH VALUES;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ReferenceBillDate')
BEGIN
	ALTER TABLE dbo.PMProductReceived ADD ReferenceBillDate DateTime NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'CurrencyId' AND DATA_TYPE = 'bigint')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		DROP CONSTRAINT DF_PMProductReceived_CurrencyId;

	ALTER TABLE dbo.PMProductReceived
	  ALTER COLUMN CurrencyId INT NOT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ConvertionRate')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD ConvertionRate MONEY NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD CheckedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD CheckedDate DATE NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD ApprovedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD ApprovedDate DATE NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD Remarks NVARCHAR(250) NULL;
END
GO
--- Migrate Data
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ReceiveType')
BEGIN
	IF EXISTS (SELECT TOP(1) ReceivedId FROM PMProductReceived)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'				
				UPDATE po
				SET po.ReceiveType = CASE WHEN POrderId = 0 THEN ''AdHoc'' ELSE ''Purchase'' END
				FROM PMProductReceived po
				'
		END
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ReturnStatus')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD ReturnStatus NVARCHAR(25) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'Status' )
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ALTER COLUMN Status NVARCHAR(50) NULL;
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'IsVoucherProcessed')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD IsVoucherProcessed BIT NULL;
END
GO
SET ANSI_PADDING OFF
GO
---- Add Table PaymentType Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'PaymentType')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD PaymentType VARCHAR(50) NOT NULL DEFAULT 'Credit' WITH VALUES;
END
/****** Object:  Table [dbo].[PMProductReceivedDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReceivedDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReceivedDetails](
	[ReceiveDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[ReceivedId] [int] NOT NULL,
	[SupplierId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[StockById] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[PurchasePrice] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PMProductReceivedDetails] PRIMARY KEY CLUSTERED 
(
	[ReceiveDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
-----Rename Table Column
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ProductId')
BEGIN
	EXEC sp_RENAME 'PMProductReceivedDetails.ProductId' , 'ItemId', 'COLUMN'
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
		ADD ColorId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
		ADD SizeId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
		ADD StyleId INT NULL
END
GO
--- Remove Existing Table Column and Migrate Data
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'SupplierId')
BEGIN
	IF EXISTS (SELECT TOP(1) ReceivedId FROM PMProductReceivedDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE po
				SET po.SupplierId = pod.SupplierId
				FROM PMProductReceived po
				INNER JOIN
				(	SELECT ReceivedId, MAX(SupplierId)  SupplierId
					FROM PMProductReceivedDetails
					GROUP BY ReceivedId
				)pod ON po.ReceivedId = pod.ReceivedId
				'
		END

	 ALTER TABLE PMProductReceivedDetails DROP COLUMN SupplierId
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'CostCenterId')
BEGIN
	IF EXISTS (SELECT TOP(1) ReceivedId FROM PMProductReceivedDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE po
				SET po.CostCenterId = pod.CostCenterId
				FROM PMProductReceived po
				INNER JOIN
				(	SELECT ReceivedId, MAX(CostCenterId)  CostCenterId
					FROM PMProductReceivedDetails
					GROUP BY ReceivedId
				)pod ON po.ReceivedId = pod.ReceivedId
				'
		END

	 ALTER TABLE PMProductReceivedDetails DROP COLUMN CostCenterId
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'LocationId')
BEGIN
	IF EXISTS (SELECT TOP(1) ReceivedId FROM PMProductReceivedDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE po
				SET po.LocationId = pod.LocationId
				FROM PMProductReceived po
				INNER JOIN
				(	SELECT ReceivedId, MAX(LocationId)  LocationId
					FROM PMProductReceivedDetails
					GROUP BY ReceivedId
				)pod ON po.ReceivedId = pod.ReceivedId
				'
		END

	 ALTER TABLE PMProductReceivedDetails DROP COLUMN LocationId
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ApprovedReturnQuantity')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
		ADD ApprovedReturnQuantity DECIMAL(18,2) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ReturnQuantity')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
		ADD ReturnQuantity DECIMAL(18,2) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'PurchasePrice')
BEGIN
ALTER TABLE dbo.PMProductReceivedDetails
	   ALTER COLUMN PurchasePrice DECIMAL(18,5) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'AverageCost')
BEGIN
ALTER TABLE dbo.PMProductReceivedDetails
	   ALTER COLUMN AverageCost DECIMAL(18,5) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'BagQuantity')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
		ADD BagQuantity INT NULL
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'BonusAmount')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
	   ADD BonusAmount DECIMAL(18,5) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ReceivedRatio')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
	ADD ReceivedRatio DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'TotalReceived')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
	ADD TotalReceived DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ReceivedWiseFGCost')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
	ADD ReceivedWiseFGCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ReceivedWiseCOGSCost')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
	ADD ReceivedWiseCOGSCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'TotalFGCost')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
	ADD TotalFGCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceivedDetails' AND column_name = 'ItemAverageCost')
BEGIN
	ALTER TABLE dbo.PMProductReceivedDetails
	ADD ItemAverageCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
/****** Object:  Table [dbo].[PMProductOut]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductOut]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductOut](
	[OutId] [int] IDENTITY(1,1) NOT NULL,
	[ProductOutFor] [varchar](50) NULL,
	[OutDate] [datetime] NULL,
	[RequisitionOrSalesId] [int] NOT NULL,
	[OutFor] [int] NULL,
	[IssueType] [varchar](25) NULL,
	[IssueForId] [bigint] NULL,
	[Status] [nvarchar](15) NULL,
	[Remarks] [varchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMProductOut] PRIMARY KEY CLUSTERED 
(
	[OutId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'IssueNumber')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD IssueNumber NVARCHAR(25) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'AccountPostingHeadId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD AccountPostingHeadId BIGINT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD CheckedBy INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD CheckedDate DATETIME NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD ApprovedDate DATETIME NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD ApprovedBy INT NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ToLocationId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD ToLocationId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'FromCostCenterId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD FromCostCenterId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'FromLocationId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD FromLocationId BIGINT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'DeliveryStatus')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD DeliveryStatus VARCHAR(30) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ReturnStatus')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD ReturnStatus VARCHAR(30) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'Status' )
BEGIN
	ALTER TABLE dbo.PMProductOut
		ALTER COLUMN Status NVARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'TransferFor')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD TransferFor NVARCHAR(50) NULL;
END
GO

--migration script for update costcenter and location from PMProductOutDetails to PMProductOut
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'FromCostCenterId')
BEGIN
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'IssueForId')
	BEGIN
		IF EXISTS (SELECT OutDetailsId FROM PMProductOutDetails)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'
				UPDATE po
				SET FromCostCenterId = pod.CostCenterIdFrom,
					FromLocationId = pod.LocationIdFrom,
					IssueForId = pod.CostCenterIdTo,
					ToLocationId = pod.LocationIdTo

				FROM PMProductOut po 
					INNER JOIN PMProductOutDetails pod
					ON pod.OutId = po.OutId'
		END
	END
END
GO


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'IssueForId')
BEGIN
	EXEC sp_RENAME 'PMProductOut.IssueForId' , 'ToCostCenterId', 'COLUMN'
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'FromCostCenterId' AND DATA_TYPE = 'bigint')
BEGIN
ALTER TABLE dbo.PMProductOut
	   ALTER COLUMN FromCostCenterId int NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'FromLocationId' AND DATA_TYPE = 'bigint')
BEGIN
ALTER TABLE dbo.PMProductOut
	   ALTER COLUMN FromLocationId int NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ToCostCenterId' AND DATA_TYPE = 'bigint')
BEGIN
ALTER TABLE dbo.PMProductOut
	   ALTER COLUMN ToCostCenterId int NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ToLocationId' AND DATA_TYPE = 'bigint')
BEGIN
ALTER TABLE dbo.PMProductOut
	   ALTER COLUMN ToLocationId int NULL;
END
GO

--- Migrate Data
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'IssueType')
BEGIN
	IF EXISTS (SELECT TOP(1) ReceivedId FROM PMProductReceived)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'				
				UPDATE po
				SET po.IssueType = CASE WHEN ProductOutFor = ''Requisition'' THEN ''Requisition Transfer'' ELSE po.IssueType END
				FROM PMProductOut po
				'
		END
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'IssueType')
BEGIN
	IF EXISTS (SELECT TOP(1) ReceivedId FROM PMProductReceived)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'				
				UPDATE po
				SET po.IssueType = CASE WHEN ProductOutFor = ''Internal'' THEN ''Stock Transfer'' ELSE po.IssueType END
				FROM PMProductOut po
				'
		END
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'IssueType')
BEGIN
	IF EXISTS (SELECT TOP(1) ReceivedId FROM PMProductReceived)
		BEGIN
			EXEC dbo.sp_executesql @statement = N'				
				UPDATE po
				SET po.ProductOutFor = CASE WHEN ProductOutFor = ''Internal'' THEN ''StockTransfer'' ELSE po.ProductOutFor END
				FROM PMProductOut po
				'
		END
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'GLCompanyId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD GLCompanyId INT NOT NULL CONSTRAINT DF_PMProductOut_GLCompanyId DEFAULT 1 WITH VALUES;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'GLProjectId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD GLProjectId INT NOT NULL CONSTRAINT DF_PMProductOut_GLProjectId DEFAULT 1 WITH VALUES;

	EXEC dbo.sp_executesql @statement = N'UPDATE PMProductOut
	SET GLCompanyId = tblCompanyProject.GLCompanyId,
		GLProjectId = tblCompanyProject.GLProjectId
	FROM 
	(SELECT TOP 1 
			GLCompanyId = GP.CompanyId,
			GLProjectId = ProjectId 

	FROM GLProject GP
	INNER JOIN GLCompany GC ON GP.CompanyId = GC.CompanyId
	)tblCompanyProject
	WHERE ProductOutFor = ''DirectOut'''
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ToGLCompanyId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD ToGLCompanyId INT NOT NULL DEFAULT 1 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOut' AND column_name = 'ToGLProjectId')
BEGIN
	ALTER TABLE dbo.PMProductOut
		ADD ToGLProjectId INT NOT NULL DEFAULT 1 WITH VALUES;
END
GO
/****** Object:  Table [dbo].[PMProductOutDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductOutDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductOutDetails](
	[OutDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[OutId] [int] NOT NULL,
	[CostCenterIdFrom] [int] NULL,
	[LocationIdFrom] [int] NULL,
	[CostCenterIdTo] [int] NULL,
	[LocationIdTo] [int] NULL,
	[StockById] [int] NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[SerialId] [int] NULL,
	[SerialNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_PMProductOutDetails] PRIMARY KEY CLUSTERED 
(
	[OutDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'AdjustmentQuantity')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD AdjustmentQuantity DECIMAL(18, 5) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'AdjustmentStockById')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD AdjustmentStockById INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'ApprovalStatus')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD ApprovalStatus NVARCHAR(15) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'ApprovedQuantity')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		DROP COLUMN ApprovedQuantity;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'ApprovedReturnQuantity')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD ApprovedReturnQuantity DECIMAL(18, 2) NULL;
END
GO
-----Rename Table Column PMProductOutDetails
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'ProductId')
BEGIN
	EXEC sp_RENAME 'PMProductOutDetails.ProductId' , 'ItemId', 'COLUMN'
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'SerialId')
BEGIN
	 ALTER TABLE PMProductOutDetails DROP COLUMN SerialId
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'SerialNumber')
BEGIN
	 ALTER TABLE PMProductOutDetails DROP COLUMN SerialNumber
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'CostCenterIdFrom')
BEGIN
	 ALTER TABLE PMProductOutDetails DROP COLUMN CostCenterIdFrom
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'LocationIdFrom')
BEGIN
	 ALTER TABLE PMProductOutDetails DROP COLUMN LocationIdFrom
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'CostCenterIdTo')
BEGIN
	 ALTER TABLE PMProductOutDetails DROP COLUMN CostCenterIdTo
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'LocationIdTo')
BEGIN
	 ALTER TABLE PMProductOutDetails DROP COLUMN LocationIdTo
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'ReturnQuantity')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD ReturnQuantity DECIMAL(18,2) NULL;

	--Migration Script for ReturnQuantity
	IF EXISTS (SELECT TOP(1) OutDetailsId FROM PMProductOutDetails)
	BEGIN
		EXEC dbo.sp_executesql @statement = N'
			UPDATE PMProductOutDetails SET ReturnQuantity = Quantity
		'
	END
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'AverageCost')
BEGIN
ALTER TABLE dbo.PMProductOutDetails
	   ALTER COLUMN AverageCost DECIMAL(18,5) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD ColorId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD SizeId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutDetails' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.PMProductOutDetails
		ADD StyleId INT NULL
END
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMMemberPaymentLedgerClosingMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMMemberPaymentLedgerClosingMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMMemberPaymentLedgerClosingMaster](
	[YearClosingId] [bigint] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[ProfitLossClosing] [money] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMMemberPaymentLedgerClosingMaster] PRIMARY KEY CLUSTERED 
(
	[YearClosingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PMMemberPaymentLedgerClosingDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMMemberPaymentLedgerClosingDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMMemberPaymentLedgerClosingDetails](
	[ClosingBalanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[YearClosingId] [bigint] NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[NodeId] [bigint] NOT NULL,
	[NodeHead] [nvarchar](250) NOT NULL,
	[ClosingDRAmount] [money] NOT NULL,
	[ClosingCRAmount] [money] NOT NULL,
	[ClosingBalance] [money] NOT NULL,
 CONSTRAINT [PK_PMMemberPaymentLedgerClosingDetails] PRIMARY KEY CLUSTERED 
(
	[ClosingBalanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PMMemberPaymentLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMMemberPaymentLedger' AND column_name = 'LedgerNumber')
BEGIN
	DROP TABLE dbo.PMMemberPaymentLedger
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMMemberPaymentLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMMemberPaymentLedger](
	[MemberPaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](50) NULL,
	[PaymentType] [nvarchar](15) NOT NULL,
	[PaymentId] [bigint] NULL,
	[LedgerNumber] [varchar](25) NULL,
	[BillId] [int] NULL,
	[BillNumber] [varchar](50) NULL,
	[PaymentDate] [date] NOT NULL,
	[MemberId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[CurrencyAmount] [money] NULL,
	[PaidAmount] [money] NULL,
	[PaidAmountCurrent] [money] NULL,
	[DueAmount] [money] NULL,
	[AdvanceAmount] [money] NULL,
	[AdvanceAmountRemaining] [money] NULL,
	[DayConvertionRate] [money] NULL,
	[AccountsPostingHeadId] [bigint] NULL,
	[GainOrLossAmount] [money] NULL,
	[RoundedAmount] [money] NULL,
	[ChequeNumber] [nvarchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[PaymentStatus] [varchar](20) NULL,
	[BillGenerationId] [bigint] NULL,
	[RefMemberPaymentId] [bigint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMMemberPaymentLedger_1] PRIMARY KEY CLUSTERED 
(
	[MemberPaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PMFinishedProductDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMFinishedProductDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMFinishedProductDetails](
	[FinishedProductDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[FinishProductId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[StockById] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_PMFinishedProductDetails] PRIMARY KEY CLUSTERED 
(
	[FinishedProductDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PMFinishedProduct]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMFinishedProduct]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMFinishedProduct](
	[FinishProductId] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[ApprovedStatus] [nvarchar](15) NULL,
	[Remarks] [varchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMFinishedProduct] PRIMARY KEY CLUSTERED 
(
	[FinishProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollWorkingDay]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollWorkingDay]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollWorkingDay](
	[WorkingDayId] [int] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NULL,
	[WorkingPlan] [varchar](50) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[DayOffOne] [varchar](50) NULL,
	[DayOffTwo] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollWorkingDay] PRIMARY KEY CLUSTERED 
(
	[WorkingDayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollTimeSlabHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollTimeSlabHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollTimeSlabHead](
	[TimeSlabId] [int] IDENTITY(1,1) NOT NULL,
	[TimeSlabHead] [nvarchar](50) NULL,
	[SlabStartTime] [datetime] NULL,
	[SlabEndTime] [datetime] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_TimeSlabHead] PRIMARY KEY CLUSTERED 
(
	[TimeSlabId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollTaxSetting]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollTaxSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollTaxSetting](
	[TaxSettingId] [int] IDENTITY(1,1) NOT NULL,
	[TaxBandForMale] [decimal](18, 0) NOT NULL,
	[TaxBandForFemale] [decimal](18, 0) NOT NULL,
	[IsTaxPaidByCompany] [bit] NULL,
	[CompanyContributionType] [varchar](50) NULL,
	[CompanyContributionAmount] [decimal](18, 0) NULL,
	[IsTaxDeductFromSalary] [bit] NULL,
	[EmpContributionType] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollTaxSetting] PRIMARY KEY CLUSTERED 
(
	[TaxSettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollTaxSetting' AND column_name = 'TaxBandForMale' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollTaxSetting] 
		ALTER COLUMN TaxBandForMale decimal(18, 2) NULL;
END
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollTaxSetting' AND column_name = 'TaxBandForFemale' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollTaxSetting] 
		ALTER COLUMN TaxBandForFemale decimal(18, 2) NULL;
END
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollTaxSetting' AND column_name = 'CompanyContributionAmount' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollTaxSetting] 
		ALTER COLUMN CompanyContributionAmount decimal(18, 2) NULL;
END
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollStaffRequisitionDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollStaffRequisitionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollStaffRequisitionDetails](
	[StaffRequisitionDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[StaffRequisitionId] [bigint] NOT NULL,
	[JobType] [int] NOT NULL,
	[JobLevel] [nvarchar](25) NOT NULL,
	[RequisitionQuantity] [smallint] NOT NULL,
	[SalaryAmount] [decimal](18, 2) NOT NULL,
	[DemandDate] [datetime] NULL,
 CONSTRAINT [PK_StaffRequisitionDetails] PRIMARY KEY CLUSTERED 
(
	[StaffRequisitionDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollStaffRequisitionDetails' AND column_name = 'FiscalYear' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.PayrollStaffRequisitionDetails
	   ADD FiscalYear INT NULL;
END
/****** Object:  Table [dbo].[PayrollStaffRequisition]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollStaffRequisition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollStaffRequisition](
	[StaffRequisitionId] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[ApprovedStatus] [nvarchar](25) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollStaffRequisition] PRIMARY KEY CLUSTERED 
(
	[StaffRequisitionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollStaffingBudgetDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollStaffingBudgetDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollStaffingBudgetDetails](
	[StaffingBudgetDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[StaffingBudgetId] [bigint] NOT NULL,
	[JobType] [int] NOT NULL,
	[JobLevel] [nvarchar](25) NOT NULL,
	[NoOfStaff] [smallint] NOT NULL,
	[BudgetAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_PayrollStaffingBudgeDetails] PRIMARY KEY CLUSTERED 
(
	[StaffingBudgetDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollStaffingBudgetDetails' AND column_name = 'FiscalYear')
BEGIN
	ALTER TABLE dbo.PayrollStaffingBudgetDetails
		ADD FiscalYear INT NULL
END
GO
/****** Object:  Table [dbo].[PayrollStaffingBudget]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollStaffingBudget]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollStaffingBudget](
	[StaffingBudgetId] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[ApprovedStatus] [nvarchar](25) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollStaffingBudget] PRIMARY KEY CLUSTERED 
(
	[StaffingBudgetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollServiceChargeDistributionDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollServiceChargeDistributionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollServiceChargeDistributionDetails](
	[ServiceProcessDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceProcessId] [bigint] NOT NULL,
	[EmpId] [int] NOT NULL,
	[TotalAttendance] [tinyint] NOT NULL,
	[ServiceDays] [tinyint] NOT NULL,
	[DistributionPercentage] [decimal](18, 5) NULL,
	[ServiceAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_PayrollServiceChargeDistributionDetails] PRIMARY KEY CLUSTERED 
(
	[ServiceProcessDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollServiceChargeDistribution]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollServiceChargeDistribution]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollServiceChargeDistribution](
	[ServiceProcessId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProcessDateFrom] [datetime] NULL,
	[ProcessDateTo] [datetime] NULL,
	[ProcessYear] [int] NULL,
	[DistributionPercentage] [decimal](18, 5) NULL,
	[ServiceAmount] [decimal](18, 2) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollServiceChargeDistribution] PRIMARY KEY CLUSTERED 
(
	[ServiceProcessId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollServiceChargeConfigurationDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollServiceChargeConfigurationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollServiceChargeConfigurationDetails](
	[ServiceChargeConfigurationDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceChargeConfigurationId] [bigint] NOT NULL,
	[EmpId] [int] NOT NULL,
 CONSTRAINT [PK_PayrollServiceChargeConfigurationDetails] PRIMARY KEY CLUSTERED 
(
	[ServiceChargeConfigurationDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollServiceChargeConfiguration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollServiceChargeConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollServiceChargeConfiguration](
	[ServiceChargeConfigurationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceAmount] [decimal](18, 2) NULL,
	[TotalEmployee] [smallint] NULL,
	[DepartmentId] [int] NULL,
	[EmpTypeId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollServiceChargeConfiguration] PRIMARY KEY CLUSTERED 
(
	[ServiceChargeConfigurationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollSalaryHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollSalaryHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollSalaryHead](
	[SalaryHeadId] [int] IDENTITY(1,1) NOT NULL,
	[SalaryHead] [varchar](200) NULL,
	[SalaryType] [varchar](50) NULL,
	[TransactionType] [varchar](50) NULL,
	[EffectedMonth] [datetime] NULL,
	[IsShowOnlyAllownaceDeductionPage] [bit] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SalaryHead] PRIMARY KEY CLUSTERED 
(
	[SalaryHeadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollSalaryHead' AND column_name = 'SalaryHead')
BEGIN
   ALTER TABLE dbo.PayrollSalaryHead
	   ALTER COLUMN SalaryHead NVARCHAR(MAX) NOT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollSalaryHead' AND column_name = 'NodeId')
BEGIN
	ALTER TABLE PayrollSalaryHead  
		ADD NodeId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollSalaryHead' AND column_name = 'ContributionType')
BEGIN
	ALTER TABLE PayrollSalaryHead  
		ADD ContributionType NVARCHAR(100) NULL;
END
GO
/****** Object:  Table [dbo].[PayrollSalaryFormula]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollSalaryFormula]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollSalaryFormula](
	[FormulaId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionType] [varchar](50) NULL,
	[GradeIdOrEmployeeId] [int] NULL,
	[SalaryHeadId] [int] NULL,
	[DependsOn] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
	[AmountType] [varchar](20) NULL,
	[IsBasicOrGross] [bit] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SalaryFormula] PRIMARY KEY CLUSTERED 
(
	[FormulaId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollSalaryFormula' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE PayrollSalaryFormula  
		ADD CompanyId INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollSalaryFormula' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE PayrollSalaryFormula  
		ADD ProjectId INT NULL;
END
GO
/****** Object:  Table [dbo].[PayrollSalaryFormulaForBothContribution]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollSalaryFormulaForBothContribution]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollSalaryFormulaForBothContribution](
	[FormulaId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionType] [varchar](50) NULL,
	[GradeIdOrEmployeeId] [int] NULL,
	[SalaryHeadId] [int] NULL,
	[DependsOn] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
	[AmountType] [varchar](20) NULL,
	[IsBasicOrGross] [bit] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollSalaryFormulaForBothContribution] PRIMARY KEY CLUSTERED 
(
	[FormulaId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollRosterHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollRosterHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollRosterHead](
	[RosterId] [int] IDENTITY(1,1) NOT NULL,
	[RosterName] [varchar](300) NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RosterHead] PRIMARY KEY CLUSTERED 
(
	[RosterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollPFSetting]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollPFSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollPFSetting](
	[PFSettingId] [int] IDENTITY(1,1) NOT NULL,
	[PFDependsOn] [nvarchar](50) NULL,
	[AmountType] [nvarchar](50) NULL,
	[EmpContributionInPercentage] [decimal](18, 2) NOT NULL,
	[CompanyContributionInPercentange] [decimal](18, 2) NOT NULL,
	[EmpCanContributeMaxOfBasicSalary] [decimal](18, 2) NOT NULL,
	[InterestDistributionRate] [decimal](18, 0) NULL,
	[CompanyContributionEligibilityYear] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollPFSetting] PRIMARY KEY CLUSTERED 
(
	[PFSettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollPFSetting' AND column_name = 'InterestDistributionRate' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollPFSetting] 
		ALTER COLUMN InterestDistributionRate decimal(18, 2) NULL;
END
GO
/****** Object:  Table [dbo].[PayrollLoanSetting]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollLoanSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollLoanSetting](
	[LoanSettingId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyLoanInterestRate] [decimal](18, 0) NOT NULL,
	[PFLoanInterestRate] [decimal](18, 0) NOT NULL,
	[MaxAmountCanWithdrawFromPFInPercentage] [int] NOT NULL,
	[MinPFMustAvailableToAllowLoan] [decimal](18, 0) NOT NULL,
	[MinJobLengthToAllowCompanyLoan] [decimal](18, 0) NOT NULL,
	[DurationToAllowNextLoanAfterCompletetionTakenLoan] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollLoanSetting] PRIMARY KEY CLUSTERED 
(
	[LoanSettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollLoanSetting' AND column_name = 'CompanyLoanInterestRate' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollLoanSetting] 
		ALTER COLUMN CompanyLoanInterestRate decimal(18, 2) NOT NULL;
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollLoanSetting' AND column_name = 'PFLoanInterestRate' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollLoanSetting] 
		ALTER COLUMN PFLoanInterestRate decimal(18, 2) NOT NULL;
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollLoanSetting' AND column_name = 'MinPFMustAvailableToAllowLoan' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollLoanSetting] 
		ALTER COLUMN MinPFMustAvailableToAllowLoan decimal(18, 2) NOT NULL;
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollLoanSetting' AND column_name = 'MinJobLengthToAllowCompanyLoan' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollLoanSetting] 
		ALTER COLUMN MinJobLengthToAllowCompanyLoan decimal(18, 2) NOT NULL;
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollLoanSetting' AND column_name = 'MaxAmountCanWithdrawFromPFInPercentage' AND DATA_TYPE = '[decimal](18, 2)')
BEGIN
	ALTER TABLE [dbo].[PayrollLoanSetting] 
		ALTER COLUMN MaxAmountCanWithdrawFromPFInPercentage decimal(18, 2) NOT NULL;
END
GO
/****** Object:  Table [dbo].[PayrollLoanHoldup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollLoanHoldup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollLoanHoldup](
	[LoanHoldupId] [int] IDENTITY(1,1) NOT NULL,
	[LoanId] [int] NOT NULL,
	[EmpId] [int] NOT NULL,
	[LoanHoldupDateFrom] [date] NOT NULL,
	[LoanHoldupDateTo] [date] NULL,
	[InstallmentNumberWhenLoanHoldup] [int] NOT NULL,
	[DueAmount] [decimal](18, 2) NOT NULL,
	[OverDueAmount] [decimal](18, 18) NOT NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [date] NULL,
	[Remarks] [varchar](200) NULL,
	[HoldupStatus] [nvarchar](10) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollLoanHoldup] PRIMARY KEY CLUSTERED 
(
	[LoanHoldupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollLoanCollection]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollLoanCollection]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollLoanCollection](
	[CollectionId] [int] IDENTITY(1,1) NOT NULL,
	[LoanId] [int] NOT NULL,
	[EmpId] [int] NOT NULL,
	[InstallmentNumber] [int] NOT NULL,
	[CollectionDate] [date] NOT NULL,
	[CollectionAmount] [decimal](18, 2) NOT NULL,
	[InterestCollectionAmount] [decimal](18, 2) NOT NULL,
	[LoanBalance] [decimal](18, 2) NULL,
	[InterestBalance] [decimal](18, 2) NULL,
	[ApprovedStatus] [varchar](15) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollLoanCollection] PRIMARY KEY CLUSTERED 
(
	[CollectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollLeaveType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollLeaveType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollLeaveType](
	[LeaveTypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](100) NULL,
	[YearlyLeave] [int] NULL,
	[LeaveModeId] [int] NULL,
	[CanCarryForward] [bit] NULL,
	[MaxDayCanCarryForwardYearly] [tinyint] NULL,
	[MaxDayCanKeepAsCarryForwardLeave] [tinyint] NULL,
	[CanEncash] [bit] NULL,
	[MaxDayCanEncash] [tinyint] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LeaveType] PRIMARY KEY CLUSTERED 
(
	[LeaveTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollLeaveBalanceClosing]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollLeaveBalanceClosing]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollLeaveBalanceClosing](
	[LeaveClosingId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [bigint] NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[LeaveTypeId] [int] NOT NULL,
	[OpeningLeave] [decimal](18, 2) NOT NULL,
	[TakenLeave] [decimal](18, 2) NOT NULL,
	[RemainingLeave] [decimal](18, 2) NOT NULL,
	[MaxDayCanCarryForwardYearly] [tinyint] NOT NULL,
	[CarryForwardedLeave] [tinyint] NOT NULL,
	[MaxDayCanKeepAsCarryForwardLeave] [tinyint] NOT NULL,
	[TotalCarryforwardLeave] [tinyint] NOT NULL,
	[MaxDayCanEncash] [tinyint] NOT NULL,
	[EncashableLeave] [tinyint] NOT NULL,
	[ApprovedStatus] [nvarchar](25) NULL,
	[Status] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollLeaveBalanceClosing] PRIMARY KEY CLUSTERED 
(
	[LeaveClosingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollJobCircularApplicantMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollJobCircularApplicantMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollJobCircularApplicantMapping](
	[JobCircularApplicantMappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[JobCircularId] [bigint] NOT NULL,
	[ApplicantId] [bigint] NOT NULL,
	[ApplicantType] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_PayrollJobCircularApplicantMapping] PRIMARY KEY CLUSTERED 
(
	[JobCircularApplicantMappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollJobCircular]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollJobCircular]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollJobCircular](
	[JobCircularId] [bigint] IDENTITY(1,1) NOT NULL,
	[StaffRequisitionDetailsId] [int] NULL,
	[JobTitle] [nvarchar](50) NOT NULL,
	[CircularDate] [datetime] NOT NULL,
	[JobType] [int] NOT NULL,
	[JobLevel] [nvarchar](25) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[NoOfVancancie] [smallint] NOT NULL,
	[DemandedTime] [datetime] NOT NULL,
	[OpenFrom] [datetime] NULL,
	[OpenTo] [datetime] NULL,
	[AgeRangeFrom] [tinyint] NOT NULL,
	[AgeRangeTo] [tinyint] NOT NULL,
	[Gender] [nvarchar](6) NOT NULL,
	[YearOfExperiance] [tinyint] NOT NULL,
	[JobDescription] [text] NULL,
	[EducationalQualification] [text] NULL,
	[AdditionalJobRequirement] [text] NULL,
	[ApprovedStatus] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollJobCircular] PRIMARY KEY CLUSTERED 
(
	[JobCircularId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollInterviewType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollInterviewType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollInterviewType](
	[InterviewTypeId] [smallint] IDENTITY(1,1) NOT NULL,
	[InterviewName] [nvarchar](150) NOT NULL,
	[Marks] [decimal](18, 2) NOT NULL,
	[Remarks] [text] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollInterviewType] PRIMARY KEY CLUSTERED 
(
	[InterviewTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollHoliday]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollHoliday]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollHoliday](
	[HolidayId] [int] IDENTITY(1,1) NOT NULL,
	[HolidayName] [varchar](300) NULL,
	[HolidayDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Description] [varchar](300) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollHoliday] PRIMARY KEY CLUSTERED 
(
	[HolidayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollGratutitySettings]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollGratutitySettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollGratutitySettings](
	[GratuityId] [int] IDENTITY(1,1) NOT NULL,
	[GratuityWillAffectAfterJobLengthInYear] [int] NOT NULL,
	[IsGratuityBasedOnBasic] [bit] NULL,
	[IsGratutityBasedOnGross] [bit] NULL,
	[GratutiyPercentage] [decimal](18, 0) NULL,
	[NumberOfGratuityAdded] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollGratutitySettings] PRIMARY KEY CLUSTERED 
(
	[GratuityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpYearlyLeave]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpYearlyLeave]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpYearlyLeave](
	[YearlyLeaveId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[LeaveTypeId] [int] NULL,
	[LeaveQuantity] [int] NULL,
 CONSTRAINT [PK_EmpYearlyLeave] PRIMARY KEY CLUSTERED 
(
	[YearlyLeaveId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpWorkStation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpWorkStation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpWorkStation](
	[WorkStationId] [int] IDENTITY(1,1) NOT NULL,
	[WorkStationName] [varchar](250) NULL,
 CONSTRAINT [PK_PayrollEmpWorkStation] PRIMARY KEY CLUSTERED 
(
	[WorkStationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpWorkStation' AND column_name = 'Description')
BEGIN
	ALTER TABLE dbo.PayrollEmpWorkStation
		ADD [Description] NVARCHAR(MAX)  NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpWorkStation' AND column_name = 'Status')
BEGIN
	ALTER TABLE dbo.PayrollEmpWorkStation
	ADD [Status] BIT  NOT NULL
	CONSTRAINT [DF_PayrollEmpWorkStation_Status] DEFAULT ((1)) WITH VALUES

END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpWorkStation' AND column_name = 'IsDeleted')
BEGIN
	ALTER TABLE dbo.PayrollEmpWorkStation
	ADD [IsDeleted] BIT  NOT NULL
	CONSTRAINT [DF_PayrollEmpWorkStation_IsDeleted] DEFAULT ((0)) WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpWorkStation' AND column_name = 'CreatedBy')
BEGIN
	ALTER TABLE dbo.PayrollEmpWorkStation
		ADD [CreatedBy] INT  NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpWorkStation' AND column_name = 'CreatedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpWorkStation
		ADD [CreatedDate] DATETIME  NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpWorkStation' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.PayrollEmpWorkStation
		ADD [LastModifiedBy] INT  NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpWorkStation' AND column_name = 'LastModifiedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpWorkStation
		ADD [LastModifiedDate] DATETIME  NULL;
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpType](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Code] [varchar](20) NULL,
	[YearlyLeave] [int] NULL,
	[IsContractualType] [bit] NULL,
	[Remarks] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsServiceChargeApplicable] [bit] NULL,
 CONSTRAINT [PK_EmpCategory] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpType' AND column_name = 'TypeCategory')
BEGIN
	ALTER TABLE dbo.PayrollEmpType
	  ADD TypeCategory VARCHAR(100) NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpTransfer]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTransfer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTransfer](
	[TransferId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransferDate] [datetime] NOT NULL,
	[EmpId] [bigint] NOT NULL,
	[PreviousDepartmentId] [int] NOT NULL,
	[PreviousDesignationId] [int] NULL,
	[CurrentDepartmentId] [int] NOT NULL,
	[CurrentDesignationId] [int] NULL,
	[PreviousLocation] [int] NULL,
	[CurrentLocation] [int] NULL,
	[ReportingDate] [date] NOT NULL,
	[JoinedDate] [datetime] NOT NULL,
	[ReportingToId] [bigint] NOT NULL,
	[ApprovedStatus] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpTransfer] PRIMARY KEY CLUSTERED 
(
	[TransferId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'ReportingTo2Id')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD ReportingTo2Id BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'PreviousReportingTo2Id')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD PreviousReportingTo2Id BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'PreviousReportingToId')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD PreviousReportingToId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'PreviousCompanyId')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD PreviousCompanyId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'CurrentCompanyId')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD CurrentCompanyId BIGINT NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'ReportingToId' AND IS_NULLABLE='NO')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ALTER COLUMN ReportingToId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'Description')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD Description NVARCHAR(MAX) NULL
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'PreviousProjectId')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD PreviousProjectId BIGINT NULL
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTransfer' AND column_name = 'CurrentProjectId')
BEGIN
	ALTER TABLE dbo.PayrollEmpTransfer
		ADD CurrentProjectId BIGINT NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpTrainingType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTrainingType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTrainingType](
	[TrainingTypeId] [int] IDENTITY(1,1) NOT NULL,
	[TrainingName] [varchar](200) NOT NULL,
	[Remarks] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpTrainingType] PRIMARY KEY CLUSTERED 
(
	[TrainingTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpTrainingOrganizer]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTrainingOrganizer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTrainingOrganizer](
	[OrganizerId] [int] IDENTITY(1,1) NOT NULL,
	[TrainingType] [varchar](50) NULL,
	[OrganizerName] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](250) NULL,
	[Email] [nvarchar](100) NULL,
	[ContactNo] [varchar](100) NULL,
 CONSTRAINT [PK_PayrollEmpTrainingOrganizer] PRIMARY KEY CLUSTERED 
(
	[OrganizerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpTrainingDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTrainingDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTrainingDetail](
	[TrainingDetailId] [int] IDENTITY(1,1) NOT NULL,
	[TrainingId] [int] NULL,
	[EmpId] [int] NULL,
	[EmpName] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpTrainingDetail] PRIMARY KEY CLUSTERED 
(
	[TrainingDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpTraining]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTraining]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTraining](
	[TrainingId] [int] IDENTITY(1,1) NOT NULL,
	[TrainingTypeId] [int] NOT NULL,
	[Trainer] [nvarchar](100) NULL,
	[OrganizerId] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[AttendeeList] [nvarchar](max) NULL,
	[Location] [nvarchar](100) NULL,
	[Remarks] [nvarchar](250) NULL,
	[Reminder] [bit] NULL,
	[ReminderHour] [int] NULL,
	[Note] [nvarchar](20) NULL,
	[EndDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpTraining] PRIMARY KEY CLUSTERED 
(
	[TrainingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTraining' AND column_name = 'Location')
BEGIN
	ALTER TABLE PayrollEmpTraining  
		ALTER COLUMN Location NVARCHAR(MAX) NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTraining' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE PayrollEmpTraining  
		ALTER COLUMN Remarks NVARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTraining' AND column_name = 'Discussed')
BEGIN
	ALTER TABLE dbo.PayrollEmpTraining
	ADD Discussed VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTraining' AND column_name = 'CallToAction')
BEGIN
	ALTER TABLE dbo.PayrollEmpTraining
	ADD CallToAction VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTraining' AND column_name = 'Conclusion')
BEGIN
	ALTER TABLE dbo.PayrollEmpTraining
	ADD Conclusion VARCHAR(MAX) NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpTimeSlabRoster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTimeSlabRoster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTimeSlabRoster](
	[RosterId] [int] IDENTITY(1,1) NOT NULL,
	[EmpTimeSlabId] [int] NULL,
	[DayName] [varchar](20) NULL,
	[TimeSlabId] [int] NULL,
 CONSTRAINT [PK_EmpTimeSlabRoster] PRIMARY KEY CLUSTERED 
(
	[RosterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpTimeSlab]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTimeSlab]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTimeSlab](
	[EmpTimeSlabId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[SlabEffectDate] [datetime] NULL,
	[TimeSlabId] [int] NOT NULL,
	[WeekEndMode] [varchar](20) NULL,
	[WeekEndFirst] [varchar](20) NULL,
	[WeekEndSecond] [varchar](20) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpTimeSlab] PRIMARY KEY CLUSTERED 
(
	[EmpTimeSlabId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpThana]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpThana]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpThana](
	[ThanaId] [int] IDENTITY(1,1) NOT NULL,
	[DistrictId] [int] NOT NULL,
	[ThanaName] [nvarchar](50) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpThana] PRIMARY KEY CLUSTERED 
(
	[ThanaId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpTermination]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTermination]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTermination](
	[TerminationId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[DecisionDate] [date] NULL,
	[TerminationDate] [date] NULL,
	[EmployeeStatusId] [int] NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpTermination] PRIMARY KEY CLUSTERED 
(
	[TerminationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpTaxDeductionSetting]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTaxDeductionSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTaxDeductionSetting](
	[TaxDeductionId] [int] IDENTITY(1,1) NOT NULL,
	[RangeFrom] [decimal](18, 0) NOT NULL,
	[RangeTo] [decimal](18, 0) NOT NULL,
	[DeductionPercentage] [decimal](18, 0) NOT NULL,
	[Remarks] [varchar](50) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpTaxDeductionSetting] PRIMARY KEY CLUSTERED 
(
	[TaxDeductionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpTaxDeductionSetting' AND column_name = 'Gender')
BEGIN
	ALTER TABLE dbo.PayrollEmpTaxDeductionSetting
		ADD Gender VARCHAR(100) NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpTaxDeduction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTaxDeduction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTaxDeduction](
	[TaxCollectionId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[BasicAmount] [decimal](18, 2) NOT NULL,
	[DeductionPercentage] [decimal](18, 2) NOT NULL,
	[RangeFrom] [decimal](18, 2) NULL,
	[RangeTo] [decimal](18, 2) NULL,
	[TaxAmount] [decimal](18, 2) NOT NULL,
	[TaxDateFrom] [datetime] NOT NULL,
	[TaxDateTo] [datetime] NOT NULL,
	[TaxYear] [smallint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmployeeTaxDeduction] PRIMARY KEY CLUSTERED 
(
	[TaxCollectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpTax]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpTax]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpTax](
	[TaxCollectionId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[EmpTaxContribution] [decimal](18, 0) NOT NULL,
	[CompanyTaxContribution] [decimal](18, 0) NOT NULL,
	[TaxDate] [datetime] NOT NULL,
	[Remarks] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpTax] PRIMARY KEY CLUSTERED 
(
	[TaxCollectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpSalaryProcessTemp]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpSalaryProcessTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpSalaryProcessTemp](
	[ProcessId] [int] IDENTITY(1,1) NOT NULL,
	[ProcessDate] [datetime] NULL,
	[SalaryDateFrom] [datetime] NULL,
	[SalaryDateTo] [datetime] NULL,
	[SalaryYear] [smallint] NULL,
	[ProcessSequence] [tinyint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsBonusPaid] [bit] NULL,
 CONSTRAINT [PK_PayrollEmpSalaryProcessTemp] PRIMARY KEY CLUSTERED 
(
	[ProcessId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpSalaryProcessTemp' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.PayrollEmpSalaryProcessTemp
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpSalaryProcessTemp' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.PayrollEmpSalaryProcessTemp
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO
/****** Object:  Table [dbo].[PayrollEmpSalaryProcessDetailTemp]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpSalaryProcessDetailTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpSalaryProcessDetailTemp](
	[ProcessDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ProcessId] [int] NULL,
	[EmpId] [int] NULL,
	[PayrollCurrencyId] [int] NULL,
	[ConvertionRate] [money] NULL,
	[BasicAmountInEmployeeCurrency] [decimal](18, 2) NULL,
	[BasicAmount] [decimal](18, 2) NULL,
	[SalaryHeadId] [int] NULL,
	[SalaryHead] [nvarchar](100) NULL,
	[TransactionType] [varchar](100) NULL,
	[SalaryHeadNote] [varchar](100) NULL,
	[DependsOn] [int] NULL,
	[AddDeductDependsOn] [nvarchar](25) NULL,
	[SalaryType] [varchar](100) NULL,
	[SalaryEffectiveness] [varchar](25) NULL,
	[AmountType] [varchar](100) NULL,
	[Amount] [decimal](18, 2) NULL,
	[SalaryAmount] [decimal](18, 2) NULL,
	[GrossAmount] [decimal](18, 2) NULL,
	[TotalAllowance] [decimal](18, 2) NULL,
	[TotalDeduction] [decimal](18, 2) NULL,
	[TotalAllowanceNotEffective] [decimal](18, 2) NULL,
	[TotalDeductionNotEffective] [decimal](18, 2) NULL,
	[HomeTakenAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PayrollEmpSalaryProcessDetailTemp] PRIMARY KEY CLUSTERED 
(
	[ProcessDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpSalaryProcessDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpSalaryProcessDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpSalaryProcessDetail](
	[ProcessDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ProcessId] [int] NULL,
	[EmpId] [int] NULL,
	[PayrollCurrencyId] [int] NULL,
	[ConvertionRate] [money] NULL,
	[BasicAmountInEmployeeCurrency] [decimal](18, 2) NULL,
	[BasicAmount] [decimal](18, 2) NULL,
	[SalaryHeadId] [int] NULL,
	[SalaryHead] [nvarchar](100) NULL,
	[TransactionType] [varchar](100) NULL,
	[SalaryHeadNote] [varchar](250) NULL,
	[DependsOn] [int] NULL,
	[AddDeductDependsOn] [nvarchar](25) NULL,
	[SalaryType] [varchar](100) NULL,
	[SalaryEffectiveness] [varchar](25) NULL,
	[AmountType] [varchar](100) NULL,
	[Amount] [decimal](18, 2) NULL,
	[SalaryAmount] [decimal](18, 2) NULL,
	[GrossAmount] [decimal](18, 2) NULL,
	[TotalAllowance] [decimal](18, 2) NULL,
	[TotalDeduction] [decimal](18, 2) NULL,
	[TotalAllowanceNotEffective] [decimal](18, 2) NULL,
	[TotalDeductionNotEffective] [decimal](18, 2) NULL,
	[HomeTakenAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_EmpSalaryProcessDetail] PRIMARY KEY CLUSTERED 
(
	[ProcessDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpSalaryProcess]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpSalaryProcess]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpSalaryProcess](
	[ProcessId] [int] IDENTITY(1,1) NOT NULL,
	[ProcessDate] [datetime] NULL,
	[SalaryDateFrom] [datetime] NULL,
	[SalaryDateTo] [datetime] NULL,
	[SalaryYear] [smallint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsBonusPaid] [bit] NULL,
 CONSTRAINT [PK_EmpSalaryProcess] PRIMARY KEY CLUSTERED 
(
	[ProcessId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpSalaryProcess' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.PayrollEmpSalaryProcess
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpSalaryProcess' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.PayrollEmpSalaryProcess
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpSalaryProcess' AND column_name = 'CurrencyExchangeRate')
BEGIN
	ALTER TABLE dbo.PayrollEmpSalaryProcess
		ADD CurrencyExchangeRate DECIMAL(18,2)  NULL DEFAULT 0.00 WITH VALUES
END
GO
/****** Object:  Table [dbo].[PayrollEmpRoster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpRoster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpRoster](
	[EmpRosterId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[RosterId] [int] NULL,
	[RosterDate] [datetime] NULL,
	[TimeSlabId] [int] NULL,
	[SecondTimeSlabId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpRoster] PRIMARY KEY CLUSTERED 
(
	[EmpRosterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpReference]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpReference]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpReference](
	[ReferenceId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[Name] [nvarchar](200) NULL,
	[Organization] [nvarchar](200) NULL,
	[Designation] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[Relation] [nvarchar](50) NULL,
 CONSTRAINT [PK_PayrollEmpReference] PRIMARY KEY CLUSTERED 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpPromotion]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpPromotion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpPromotion](
	[PromotionId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[PromotionDate] [datetime] NOT NULL,
	[PreviousDesignationId] [int] NOT NULL,
	[PreviousGradeId] [int] NOT NULL,
	[CurrentDesignationId] [int] NOT NULL,
	[CurrentGradeId] [int] NOT NULL,
	[ApprovalStatus] [nvarchar](25) NOT NULL,
	[Remarks] [text] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpPromotion] PRIMARY KEY CLUSTERED 
(
	[PromotionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpPF]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpPF]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpPF](
	[PFCollectionId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[PFType] [varchar](25) NOT NULL,
	[EmpContribution] [decimal](18, 2) NOT NULL,
	[CompanyContribution] [decimal](18, 2) NOT NULL,
	[ProvidentFundInterest] [decimal](18, 2) NOT NULL,
	[CommulativeEmpContribution] [decimal](18, 2) NOT NULL,
	[CommulativeCompanyContribution] [decimal](18, 2) NOT NULL,
	[CommulativeInterestAmount] [decimal](18, 2) NOT NULL,
	[CommulativePFAmountCurrentYear] [decimal](18, 2) NOT NULL,
	[CommulativePFAmount] [decimal](18, 2) NOT NULL,
	[PFDateFrom] [datetime] NOT NULL,
	[PFDateTo] [datetime] NOT NULL,
	[PFYear] [smallint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpPF] PRIMARY KEY CLUSTERED 
(
	[PFCollectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpPayScale]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpPayScale]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpPayScale](
	[PayScaleId] [int] IDENTITY(1,1) NOT NULL,
	[ScaleDate] [datetime] NULL,
	[GradeId] [int] NULL,
	[BasicAmount] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpPayScale] PRIMARY KEY CLUSTERED 
(
	[PayScaleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpOverTimeSetup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpOverTimeSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpOverTimeSetup](
	[OverTimeSetupId] [int] IDENTITY(1,1) NOT NULL,
	[SalaryHeadId] [int] NULL,
	[MonthlyTotalHour] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpOverTimeSetup] PRIMARY KEY CLUSTERED 
(
	[OverTimeSetupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PayrollEmpOverTime]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpOverTime]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpOverTime](
	[OverTimeId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[OverTimeDate] [datetime] NULL,
	[EntryTime] [datetime] NULL,
	[ExitTime] [datetime] NULL,
	[TotalHour] [int] NULL,
	[OTHour] [int] NULL,
	[ApprovedOTHour] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpOverTime] PRIMARY KEY CLUSTERED 
(
	[OverTimeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpOverTime' AND column_name = 'OTHour' AND DATA_TYPE = 'int')
BEGIN
	 ALTER TABLE dbo.PayrollEmpOverTime
     ALTER COLUMN OTHour DECIMAL NULL;
END
/****** Object:  Table [dbo].[PayrollEmpNomineeInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpNomineeInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpNomineeInfo](
	[NomineeId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[NomineeName] [varchar](200) NULL,
	[Relationship] [varchar](100) NULL,
	[DateOfBirth] [datetime] NULL,
	[Age] [varchar](100) NULL,
	[Percentage] [decimal](18, 2) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmployeeStatus]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeeStatus](
	[EmployeeStatusId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeStatus] [varchar](50) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmployeeStatus] PRIMARY KEY CLUSTERED 
(
	[EmployeeStatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmployeePaymentLedgerClosingMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeePaymentLedgerClosingMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeePaymentLedgerClosingMaster](
	[YearClosingId] [bigint] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[EmployeeId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[ProfitLossClosing] [money] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmployeePaymentLedgerClosingMaster] PRIMARY KEY CLUSTERED 
(
	[YearClosingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmployeePaymentLedgerClosingDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeePaymentLedgerClosingDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeePaymentLedgerClosingDetails](
	[ClosingBalanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[YearClosingId] [bigint] NULL,
	[FiscalYearId] [int] NOT NULL,
	[EmployeeId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[NodeId] [bigint] NOT NULL,
	[NodeHead] [nvarchar](250) NOT NULL,
	[ClosingDRAmount] [money] NOT NULL,
	[ClosingCRAmount] [money] NOT NULL,
	[ClosingBalance] [money] NOT NULL,
 CONSTRAINT [PK_PayrollEmployeePaymentLedgerClosingDetails] PRIMARY KEY CLUSTERED 
(
	[ClosingBalanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmployeePaymentLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeePaymentLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeePaymentLedger](
	[EmployeePaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](50) NULL,
	[PaymentType] [nvarchar](15) NOT NULL,
	[PaymentId] [bigint] NULL,
	[LedgerNumber] [varchar](25) NULL,
	[BillId] [int] NULL,
	[BillNumber] [varchar](50) NULL,
	[PaymentDate] [date] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[CurrencyAmount] [money] NULL,
	[PaidAmount] [money] NULL,
	[PaidAmountCurrent] [money] NULL,
	[DueAmount] [money] NULL,
	[AdvanceAmount] [money] NULL,
	[AdvanceAmountRemaining] [money] NULL,
	[DayConvertionRate] [money] NULL,
	[AccountsPostingHeadId] [bigint] NULL,
	[ChequeNumber] [nvarchar](50) NULL,
	[GainOrLossAmount] [money] NULL,
	[RoundedAmount] [money] NULL,
	[Remarks] [varchar](500) NULL,
	[PaymentStatus] [varchar](20) NULL,
	[BillGenerationId] [bigint] NULL,
	[RefEmployeePaymentId] [bigint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmployeePaymentLedger] PRIMARY KEY CLUSTERED 
(
	[EmployeePaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmployeePaymentDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeePaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeePaymentDetails](
	[PaymentDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentId] [bigint] NOT NULL,
	[EmployeeBillDetailsId] [bigint] NULL,
	[EmployeePaymentId] [bigint] NOT NULL,
	[BillId] [bigint] NOT NULL,
	[PaymentAmount] [money] NOT NULL,
 CONSTRAINT [PK_PayrollEmployeePaymentDetails] PRIMARY KEY CLUSTERED 
(
	[PaymentDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmployeePayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeePayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeePayment](
	[PaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeBillId] [bigint] NULL,
	[PaymentFor] [nvarchar](50) NULL,
	[LedgerNumber] [nvarchar](50) NULL,
	[PaymentDate] [date] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[AdvanceAmount] [money] NULL,
	[Remarks] [nvarchar](250) NULL,
	[PaymentType] [nvarchar](50) NULL,
	[AccountingPostingHeadId] [int] NULL,
	[ChequeNumber] [nvarchar](50) NULL,
	[CurrencyId] [int] NULL,
	[ConvertionRate] [decimal](18, 2) NULL,
	[AdjustmentType] [nvarchar](50) NULL,
	[EmployeePaymentAdvanceId] [bigint] NULL,
	[AdjustmentAmount] [money] NULL,
	[ApprovedStatus] [nvarchar](50) NULL,
	[PaymentAdjustmentAmount] [money] NULL,
	[AdjustmentAccountHeadId] [int] NULL,
 CONSTRAINT [PK_PayrollEmployeePayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmployeeBillGenerationDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeBillGenerationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeeBillGenerationDetails](
	[EmployeeBillDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeBillId] [bigint] NOT NULL,
	[EmployeePaymentId] [bigint] NOT NULL,
	[BillId] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[PaymentAmount] [money] NULL,
	[DueAmount] [money] NULL,
 CONSTRAINT [PK_PayrollEmployeeBillGenerationDetails] PRIMARY KEY CLUSTERED 
(
	[EmployeeBillDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmployeeBillGeneration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeBillGeneration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeeBillGeneration](
	[EmployeeBillId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[BillDate] [date] NOT NULL,
	[EmployeeBillNumber] [nvarchar](50) NOT NULL,
	[ApprovedStatus] [nvarchar](50) NULL,
	[BillStatus] [nvarchar](25) NULL,
	[BillCurrencyId] [int] NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmployeeBillGeneration] PRIMARY KEY CLUSTERED 
(
	[EmployeeBillId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmployee]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployee](
	[EmpId] [int] IDENTITY(1,1) NOT NULL,
	[AttendanceDeviceEmpId] [int] NULL,
	[EmpCode] [varchar](20) NULL,
	[EmpPassword] [varchar](50) NULL,
	[Title] [varchar](20) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[DisplayName] [varchar](50) NULL,
	[EmpTypeId] [int] NULL,
	[JoinDate] [datetime] NULL,
	[ProbablePFEligibilityDate] [datetime] NULL,
	[PFEligibilityDate] [datetime] NULL,
	[PFTerminateDate] [datetime] NULL,
	[ProbableGratuityEligibilityDate] [datetime] NULL,
	[GratuityEligibilityDate] [datetime] NULL,
	[ProvisionPeriod] [datetime] NULL,
	[InitialContractEndDate] [datetime] NULL,
	[ResignationDate] [datetime] NULL,
	[DepartmentId] [int] NULL,
	[DesignationId] [int] NULL,
	[GradeId] [int] NULL,
	[BasicAmount] [decimal](18, 2) NULL,
	[RepotingTo] [int] NULL,
	[OfficialEmail] [varchar](100) NULL,
	[ReferenceBy] [varchar](100) NULL,
	[Remarks] [varchar](500) NULL,
	[FathersName] [varchar](100) NULL,
	[MothersName] [varchar](100) NULL,
	[EmpDateOfBirth] [datetime] NULL,
	[Gender] [varchar](100) NULL,
	[BloodGroup] [varchar](100) NULL,
	[Religion] [varchar](30) NULL,
	[Height] [varchar](10) NULL,
	[MaritalStatus] [varchar](20) NULL,
	[CountryId] [int] NULL,
	[DivisionId] [int] NULL,
	[DistrictId] [int] NULL,
	[ThanaId] [int] NULL,
	[NationalId] [varchar](50) NULL,
	[PassportNumber] [varchar](50) NULL,
	[PIssuePlace] [varchar](50) NULL,
	[PIssueDate] [datetime] NULL,
	[PExpireDate] [datetime] NULL,
	[PresentAddress] [varchar](150) NULL,
	[PresentCity] [varchar](50) NULL,
	[PresentZipCode] [varchar](20) NULL,
	[PresentCountry] [varchar](50) NULL,
	[PresentPhone] [varchar](50) NULL,
	[PermanentAddress] [varchar](150) NULL,
	[PermanentCity] [varchar](50) NULL,
	[PermanentZipCode] [varchar](20) NULL,
	[PermanentCountry] [varchar](50) NULL,
	[PermanentPhone] [varchar](50) NULL,
	[PersonalEmail] [varchar](100) NULL,
	[NodeId] [int] NULL,
	[WorkingCostCenterId] [int] NULL,
	[WorkStationId] [int] NULL,
	[EmergencyContactName] [varchar](250) NULL,
	[EmergencyContactRelationship] [varchar](50) NULL,
	[EmergencyContactNumber] [varchar](50) NULL,
	[EmergencyContactEmail] [varchar](50) NULL,
	[DonorId] [int] NULL,
	[ActivityCode] [varchar](150) NULL,
	[Nationality] [varchar](50) NULL,
	[CurrentLocationId] [int] NULL,
	[AlternativeEmail] [varchar](50) NULL,
	[IsApplicant] [bit] NULL,
	[IsApplicantRecruitment] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsProbitionary] [bit] NULL,
	[IsContacttual] [bit] NULL,
	[CostCenterId] [int] NULL,
	[EmployeeStatusId] [int] NULL,
	[PayrollCurrencyId] [int] NULL,
	[Balance] [money] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmpId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_EmpCode] UNIQUE NONCLUSTERED 
(
	[EmpCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'PresentCountryId')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
	  ADD PresentCountryId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'PermanentCountryId')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
	  ADD PermanentCountryId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'RepotingTo2')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD RepotingTo2 INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'GlCompanyId')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD GlCompanyId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'AppoinmentLetter')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD AppoinmentLetter VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'JoiningAgreement')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD JoiningAgreement VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'ServiceBond')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD ServiceBond VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'DSOAC')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD DSOAC VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'ConfirmationLetter')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD ConfirmationLetter VARCHAR(MAX) NULL
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'IsProvidentFundDeduct')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
	  ADD IsProvidentFundDeduct BIT NOT NULL CONSTRAINT DF_PayrollEmployee_IsProvidentFundDeduct DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'MarriageDate')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
	  ADD MarriageDate DateTime NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'GLProjectId')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
	  ADD GLProjectId INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'LateBufferingTime')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
	  ADD LateBufferingTime INT  NULL DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'NotEffectOnHead')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD NotEffectOnHead INT NULL DEFAULT 0 WITH VALUES
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'EmpCompanyId')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
	  ADD EmpCompanyId INT  NOT NULL DEFAULT 1 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'PABXNumber')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD PABXNumber VARCHAR(100) NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpLoan]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpLoan]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpLoan](
	[LoanId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[LoanNumber] [varchar](50) NOT NULL,
	[LoanType] [varchar](25) NOT NULL,
	[LoanAmount] [decimal](18, 2) NOT NULL,
	[InterestRate] [decimal](18, 2) NOT NULL,
	[InterestAmount] [decimal](18, 2) NOT NULL,
	[DueAmount] [decimal](18, 2) NOT NULL,
	[DueInterestAmount] [decimal](18, 2) NOT NULL,
	[LoanTakenForPeriod] [int] NOT NULL,
	[LoanTakenForMonthOrYear] [varchar](20) NOT NULL,
	[PerInstallLoanAmount] [decimal](18, 2) NOT NULL,
	[PerInstallInterestAmount] [decimal](18, 2) NOT NULL,
	[LoanDate] [datetime] NOT NULL,
	[LoanStatus] [varchar](20) NULL,
	[ApprovedStatus] [varchar](15) NULL,
	[CheckedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmployeeLoan] PRIMARY KEY CLUSTERED 
(
	[LoanId] ASC,
	[EmpId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLoan' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpLoan
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLoan' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpLoan
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLoan' AND column_name = 'ApprovedStatus' AND CHARACTER_MAXIMUM_LENGTH = 100 )
BEGIN
	ALTER TABLE dbo.PayrollEmpLoan
		ALTER COLUMN ApprovedStatus NVARCHAR(100) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLoan' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpLoan
		ADD CheckedDate DATETIME NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLoan' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpLoan
		ADD ApprovedDate DATETIME NULL;
END
GO

/****** Object:  Table [dbo].[PayrollEmpLeaveInformation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpLeaveInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpLeaveInformation](
	[LeaveId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[LeaveMode] [varchar](20) NULL,
	[LeaveTypeId] [int] NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[TransactionType] [varchar](50) NULL,
	[NoOfDays] [int] NULL,
	[ExpireDate] [datetime] NULL,
	[LeaveStatus] [varchar](50) NULL,
	[Reason] [varchar](500) NULL,
	[ReportingTo] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpLeaveInformation] PRIMARY KEY CLUSTERED 
(
	[LeaveId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'Reason')
BEGIN
	ALTER TABLE PayrollEmpLeaveInformation  
		ALTER COLUMN Reason NVARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'WorkHandover')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
	ADD WorkHandover INT NULL DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'WorkHandoverStatus')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
	ADD WorkHandoverStatus VARCHAR(100) NULL DEFAULT 'Pending' WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
	ADD CheckedBy INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
	ADD CheckedDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
	ADD ApprovedBy INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
	ADD ApprovedDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'CancelReason')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
	ADD CancelReason VARCHAR(MAX) NULL
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpLastMonthBenifitsPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpLastMonthBenifitsPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpLastMonthBenifitsPayment](
	[BenifitId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[AfterServiceBenefit] [decimal](18, 2) NULL,
	[EmployeePFContribution] [decimal](18, 2) NULL,
	[CompanyPFContribution] [decimal](18, 2) NULL,
	[LeaveBalanceDays] [decimal](18, 0) NULL,
	[LeaveBalanceAmount] [decimal](18, 2) NULL,
	[ProcessYear] [int] NULL,
	[ProcessDateFrom] [datetime] NULL,
	[ProcessDateTo] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpLastMonthBenifitsPayment] PRIMARY KEY CLUSTERED 
(
	[BenifitId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpLanguage]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpLanguage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpLanguage](
	[LanguageId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[Language] [nvarchar](50) NULL,
	[Reading] [nvarchar](20) NULL,
	[Writing] [nvarchar](20) NULL,
	[Speaking] [nvarchar](20) NULL,
 CONSTRAINT [PK_PayrollEmpLanguage] PRIMARY KEY CLUSTERED 
(
	[LanguageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpIncrement]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpIncrement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpIncrement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IncrementDate] [datetime] NULL,
	[EmpId] [int] NULL,
	[BasicSalary] [decimal](18, 2) NULL,
	[IncrementMode] [varchar](50) NULL,
	[EffectiveDate] [datetime] NULL,
	[Amount] [decimal](18, 2) NULL,
	[Remarks] [varchar](50) NULL,
	[ApprovedStatus] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpIncrement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpIncrement' AND column_name = 'DepartmentId')
BEGIN
	ALTER TABLE dbo.PayrollEmpIncrement
		ADD DepartmentId INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpIncrement' AND column_name = 'DesignationId')
BEGIN
	ALTER TABLE dbo.PayrollEmpIncrement
		ADD DesignationId INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpIncrement' AND column_name = 'GradeId')
BEGIN
	ALTER TABLE dbo.PayrollEmpIncrement
		ADD GradeId INT NULL;
END
GO
/****** Object:  Table [dbo].[PayrollEmpGratuity]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpGratuity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpGratuity](
	[GratuityId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[BasicAmount] [decimal](18, 2) NULL,
	[GratuityAmount] [decimal](18, 2) NOT NULL,
	[NumberOfGratuity] [int] NOT NULL,
	[GratuityDate] [date] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpGratuity] PRIMARY KEY CLUSTERED 
(
	[GratuityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpGrade]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpGrade]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpGrade](
	[GradeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[ProvisionPeriodId] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[IsManagement] [bit] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[BasicAmount] [money] NULL,
 CONSTRAINT [PK_Grade] PRIMARY KEY CLUSTERED 
(
	[GradeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpExperience]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpExperience]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpExperience](
	[ExperienceId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[CompanyName] [varchar](200) NULL,
	[CompanyUrl] [varchar](200) NULL,
	[JoinDate] [datetime] NOT NULL,
	[JoinDesignation] [varchar](200) NULL,
	[LeaveDate] [datetime] NULL,
	[LeaveDesignation] [varchar](200) NULL,
	[Achievements] [varchar](500) NULL,
 CONSTRAINT [PK_EmpExperience] PRIMARY KEY CLUSTERED 
(
	[ExperienceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpEducation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpEducation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpEducation](
	[EducationId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[LevelId] [int] NULL,
	[ExamName] [varchar](200) NULL,
	[InstituteName] [varchar](200) NULL,
	[PassYear] [varchar](20) NULL,
	[SubjectName] [varchar](200) NULL,
	[PassClass] [varchar](50) NULL,
 CONSTRAINT [PK_EmpEducation] PRIMARY KEY CLUSTERED 
(
	[EducationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpDivision]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpDivision]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpDivision](
	[DivisionId] [int] IDENTITY(1,1) NOT NULL,
	[DivisionName] [nvarchar](50) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpDivision] PRIMARY KEY CLUSTERED 
(
	[DivisionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpDivision' AND column_name = 'CountryId')
BEGIN
	ALTER TABLE dbo.PayrollEmpDivision
		ADD CountryId INT NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpDistrict]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpDistrict]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpDistrict](
	[DistrictId] [int] IDENTITY(1,1) NOT NULL,
	[DivisionId] [int] NOT NULL,
	[DistrictName] [nvarchar](50) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpDistrict] PRIMARY KEY CLUSTERED 
(
	[DistrictId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpDependent]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpDependent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpDependent](
	[DependentId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[DependentName] [varchar](200) NULL,
	[Relationship] [varchar](100) NULL,
	[DateOfBirth] [datetime] NULL,
	[Age] [varchar](100) NULL,
 CONSTRAINT [PK_EmpDependent] PRIMARY KEY CLUSTERED 
(
	[DependentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpDependent' AND column_name = 'BloodGroupId')
BEGIN
	ALTER TABLE dbo.PayrollEmpDependent
		ADD BloodGroupId INT NULL;
END
GO
/****** Object:  Table [dbo].[PayrollEmpCareerTraining]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpCareerTraining]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpCareerTraining](
	[CareerTrainingId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[TrainingTitle] [nvarchar](200) NULL,
	[Topic] [nvarchar](500) NULL,
	[Institute] [nvarchar](200) NULL,
	[Country] [int] NULL,
	[Location] [nvarchar](100) NULL,
	[TrainingYear] [nvarchar](50) NULL,
	[Duration] [int] NULL,
	[DurationType] [varchar](20) NULL,
 CONSTRAINT [PK_PayrollEmpCareerTraining] PRIMARY KEY CLUSTERED 
(
	[CareerTrainingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpCareerInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpCareerInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpCareerInfo](
	[CareerInfoId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[Objective] [nvarchar](500) NULL,
	[PresentSalary] [decimal](18, 0) NULL,
	[ExpectedSalary] [decimal](18, 0) NULL,
	[Currency] [int] NULL,
	[JobLevel] [nvarchar](50) NULL,
	[AvailableType] [nvarchar](50) NULL,
	[PreferedJobType] [int] NULL,
	[PreferedOrganizationType] [int] NULL,
	[CareerSummary] [nvarchar](500) NULL,
	[PreferedJobLocationId] [int] NULL,
	[Language] [nvarchar](50) NULL,
	[ExtraCurriculmActivities] [nvarchar](500) NULL,
 CONSTRAINT [PK_PayrollEmpCareerInfo] PRIMARY KEY CLUSTERED 
(
	[CareerInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpCareerInfo' AND column_name = 'Currency' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE PayrollEmpCareerInfo
ALTER COLUMN Currency INT NULL
END
/****** Object:  Table [dbo].[PayrollEmpBenefit]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpBenefit]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpBenefit](
	[EmpBenefitMappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[BenefitHeadId] [bigint] NOT NULL,
	[EffectiveDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpBenefit] PRIMARY KEY CLUSTERED 
(
	[EmpBenefitMappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpBankInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpBankInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpBankInfo](
	[BankInfoId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[BankId] [int] NOT NULL,
	[BranchName] [varchar](250) NULL,
	[AccountName] [varchar](250) NULL,
	[AccountNumber] [nvarchar](50) NOT NULL,
	[AccountType] [nvarchar](25) NOT NULL,
	[Remarks] [nvarchar](100) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns	WHERE TABLE_NAME = 'PayrollEmpBankInfo' AND COLUMN_NAME = 'CardNumber')
BEGIN
	ALTER TABLE dbo.PayrollEmpBankInfo
		ADD CardNumber NVARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns	WHERE TABLE_NAME = 'PayrollEmpBankInfo' AND COLUMN_NAME = 'RouteNumber')
BEGIN
	ALTER TABLE dbo.PayrollEmpBankInfo
		ADD RouteNumber VARCHAR(100) NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpAttendanceLogSuprima]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAttendanceLogSuprima]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpAttendanceLogSuprima](
	[EventLogId] [int] NOT NULL,
	[AttendanceDate] [datetime] NULL,
	[DateTime] [int] NOT NULL,
	[ReaderId] [varchar](50) NOT NULL,
	[EventId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[IsLog] [smallint] NOT NULL,
	[TNAEvent] [smallint] NOT NULL,
	[IsUseTA] [smallint] NOT NULL,
	[Type] [smallint] NOT NULL,
	[IsProcessed] [bit] NULL
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendanceLogSuprima' AND column_name = 'ReaderId')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendanceLogSuprima
	ALTER COLUMN ReaderId VARCHAR(50) NULL
END
GO

/****** Object:  Table [dbo].[PayrollAttendanceLogZKTeco]    Script Date: 5/31/2021 2:13:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAttendanceLogZKTeco]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAttendanceLogZKTeco](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SerialNumber] [varchar](50) NULL,
	[UserId] [int] NULL,
	[AttendanceTime] [datetime] NULL,
	[DateTime] [int] NULL,
	[IsProcessed] [bit] NULL,
 CONSTRAINT [PK_PayrollAttendanceLogZKTeco] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PayrollEmpAttendanceLogUniview]    Script Date: 5/31/2021 2:16:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAttendanceLogUniview]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpAttendanceLogUniview](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Att_DateTime] [int] NULL,
	[DateTime] [datetime] NULL,
	[DeviceId] [varchar](50) NULL,
	[IsProcessed] [int] NULL,
	[RecordDateTime] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpAttendanceLogUniview] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpAttendance]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAttendance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpAttendance](
	[AttendanceId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[AttendanceDate] [datetime] NOT NULL,
	[EntryTime] [datetime] NULL,
	[ExitTime] [datetime] NULL,
	[Remark] [varchar](150) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpAttendance] PRIMARY KEY CLUSTERED 
(
	[AttendanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'AttendenceStatus')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD AttendenceStatus NVARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD CheckedBy INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD CheckedDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD ApprovedBy INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD ApprovedDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'CancelReason')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD CancelReason VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'AttendanceHistory')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD AttendanceHistory VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'LateApplicationDate')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
	ADD LateApplicationDate DATETIME NULL
END
GO
/****** Object:  Table [dbo].[PayrollEmpAppraisalEvalution]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAppraisalEvalution]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpAppraisalEvalution](
	[EmpAppraisalEvalutionId] [int] NOT NULL,
	[EmpId] [int] NOT NULL,
	[MarksObtains] [decimal](18, 2) NOT NULL,
	[Remarks] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpAppraisalEvalution] PRIMARY KEY CLUSTERED 
(
	[EmpAppraisalEvalutionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpAllowanceDeduction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAllowanceDeduction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpAllowanceDeduction](
	[EmpAllowDeductId] [int] IDENTITY(1,1) NOT NULL,
	[AllowDeductType] [nvarchar](20) NOT NULL,
	[DepartmentId] [int] NULL,
	[EmpId] [int] NULL,
	[SalaryHeadId] [int] NOT NULL,
	[AmountType] [nvarchar](20) NULL,
	[DependsOn] [nvarchar](25) NULL,
	[AllowDeductAmount] [decimal](18, 2) NULL,
	[EffectFrom] [datetime] NULL,
	[EffectTo] [datetime] NULL,
	[EffectiveYear] [int] NULL,
	[Remarks] [text] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmpAllowanceDeduction] PRIMARY KEY CLUSTERED 
(
	[EmpAllowDeductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollEmpAdvanceTaken]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAdvanceTaken]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpAdvanceTaken](
	[AdvanceId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[AdvanceDate] [datetime] NOT NULL,
	[AdvanceAmount] [decimal](18, 2) NOT NULL,
	[PayMonth] [varchar](20) NOT NULL,
	[IsDeductFromSalary] [bit] NOT NULL,
	[ApprovedBy] [int] NULL,
	[Remarks] [varchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CheckedBy] [int] NULL,
	[ApprovedStatus] [varchar](15) NULL,
 CONSTRAINT [PK_PayrollEmpAdvanceTaken] PRIMARY KEY CLUSTERED 
(
	[AdvanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAdvanceTaken' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE PayrollEmpAdvanceTaken  
		ALTER COLUMN Remarks NVARCHAR(MAX) NULL;
END
GO
/****** Object:  Table [dbo].[PayrollEducationLevel]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEducationLevel]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEducationLevel](
	[LevelId] [int] IDENTITY(1,1) NOT NULL,
	[LevelName] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEducationLevel] PRIMARY KEY CLUSTERED 
(
	[LevelId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollDonor]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollDonor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollDonor](
	[DonorId] [int] IDENTITY(1,1) NOT NULL,
	[DonorCode] [varchar](50) NULL,
	[DonorName] [varchar](150) NOT NULL,
 CONSTRAINT [PK_PayrollDonor] PRIMARY KEY CLUSTERED 
(
	[DonorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollDisciplinaryActionType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollDisciplinaryActionType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollDisciplinaryActionType](
	[DisciplinaryActionTypeId] [smallint] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollDisciplinaryActionType] PRIMARY KEY CLUSTERED 
(
	[DisciplinaryActionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollDisciplinaryActionReason]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollDisciplinaryActionReason]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollDisciplinaryActionReason](
	[DisciplinaryActionReasonId] [int] IDENTITY(1,1) NOT NULL,
	[ActionReason] [nvarchar](50) NOT NULL,
	[Remarks] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollDisciplinaryActionReason] PRIMARY KEY CLUSTERED 
(
	[DisciplinaryActionReasonId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollDisciplinaryAction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollDisciplinaryAction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollDisciplinaryAction](
	[DisciplinaryActionId] [bigint] IDENTITY(1,1) NOT NULL,
	[DisciplinaryActionReasonId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[ActionTypeId] [smallint] NOT NULL,
	[ActionBody] [nvarchar](550) NULL,
	[ProposedActionId] [int] NULL,
	[ApplicableDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollDisciplinaryAction] PRIMARY KEY CLUSTERED 
(
	[DisciplinaryActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollDesignation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollDesignation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollDesignation](
	[DesignationId] [int] IDENTITY(1,1) NOT NULL,
	[GradeId] [int] NULL,
	[Name] [varchar](100) NULL,
	[Remarks] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Designation] PRIMARY KEY CLUSTERED 
(
	[DesignationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollDepartment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollDepartment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollDepartment](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Remarks] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollDepartment] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollBonusSetting]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollBonusSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollBonusSetting](
	[BonusSettingId] [int] IDENTITY(1,1) NOT NULL,
	[BonusType] [varchar](20) NOT NULL,
	[EffectivePeriod] [tinyint] NULL,
	[BonusDate] [datetime] NULL,
	[BonusAmount] [decimal](18, 2) NOT NULL,
	[AmountType] [varchar](20) NOT NULL,
	[DependsOnHead] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollBonusSetting] PRIMARY KEY CLUSTERED 
(
	[BonusSettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollBestEmployeeNominationDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollBestEmployeeNominationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollBestEmployeeNominationDetails](
	[BestEmpNomineeDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[BestEmpNomineeId] [bigint] NOT NULL,
	[EmpId] [int] NOT NULL,
	[IsSelectedAsMonthlyBestEmployee] [bit] NULL,
	[IsSelectedForYearlyBestEmployee] [bit] NULL,
	[IsSelectedAsYearlyBestEmployee] [bit] NULL,
 CONSTRAINT [PK_PayrollBestEmployeeNominationDetails] PRIMARY KEY CLUSTERED 
(
	[BestEmpNomineeDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollBestEmployeeNomination]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollBestEmployeeNomination]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollBestEmployeeNomination](
	[BestEmpNomineeId] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Years] [smallint] NOT NULL,
	[Months] [tinyint] NOT NULL,
	[ApprovedStatus] [nvarchar](25) NOT NULL,
	[Status] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollBestEmployeeNomination] PRIMARY KEY CLUSTERED 
(
	[BestEmpNomineeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollBenefitHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollBenefitHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollBenefitHead](
	[BenefitHeadId] [bigint] IDENTITY(1,1) NOT NULL,
	[BenefitName] [nvarchar](500) NULL,
 CONSTRAINT [PK_PayrollBenefitHead] PRIMARY KEY CLUSTERED 
(
	[BenefitHeadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollAttendanceEventHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAttendanceEventHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAttendanceEventHead](
	[EventId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PayrollAttendanceEventHead] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



/****** Object:  Table [dbo].[PayrollAttendanceDevice]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAttendanceDevice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAttendanceDevice](
	[ReaderId] [varchar](250) NOT NULL,
	[ReaderType] [varchar](50) NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Type] [int] NOT NULL,
	[DeptIdn] [int] NOT NULL,
	[IP] [varchar](32) NOT NULL,
	[MacAddress] [varchar](32) NOT NULL,
	[ConnType] [int] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ReaderId] PRIMARY KEY CLUSTERED 
(
	[ReaderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollAttendanceDeviceLastLogRecordTime]    Script Date: 6/17/2021 12:21:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAttendanceDeviceLastLogRecordTime]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAttendanceDeviceLastLogRecordTime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceSerial] [varchar](50) NOT NULL,
	[LastLogRecordTime] [int] NULL,
 CONSTRAINT [PK_PayrollAttendanceDeviceLastLogRecordTime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PayrollAttendanceSystemConfig]    Script Date: 6/17/2021 12:21:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAttendanceSystemConfig]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAttendanceSystemConfig](
	[config_name] [varchar](50) NOT NULL,
	[value] [varchar](50) NULL,
 CONSTRAINT [PK_PayrollAttendanceSystemConfig] PRIMARY KEY CLUSTERED 
(
	[config_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO 

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollAttendanceDevice' AND column_name = 'ReaderId')
BEGIN
	IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'PK_TB_READER')
	BEGIN
		ALTER TABLE dbo.PayrollAttendanceDevice DROP CONSTRAINT PK_TB_READER;
	END
	IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'PK_ReaderId')
	BEGIN
		ALTER TABLE dbo.PayrollAttendanceDevice DROP CONSTRAINT PK_ReaderId;
	END
	ALTER TABLE dbo.PayrollAttendanceDevice ALTER COLUMN ReaderId	VARCHAR(250) NOT NULL;

	ALTER TABLE dbo.PayrollAttendanceDevice ADD CONSTRAINT PK_ReaderId PRIMARY KEY (ReaderId);
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollAttendanceDevice' AND column_name = 'RecordAttendanceOnPunchLive')
BEGIN
	ALTER TABLE dbo.PayrollAttendanceDevice
		ADD RecordAttendanceOnPunchLive	INT
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollAttendanceDevice' AND column_name = 'DeviceType')
BEGIN
	ALTER TABLE dbo.PayrollAttendanceDevice
		ADD DeviceType	VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollAttendanceDevice' AND column_name = 'PortNumber')
BEGIN
	ALTER TABLE dbo.PayrollAttendanceDevice
		ADD PortNumber	INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollAttendanceDevice' AND column_name = 'Password')
BEGIN
	ALTER TABLE dbo.PayrollAttendanceDevice
		ADD [Password] NVARCHAR(225) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollAttendanceDevice' AND column_name = 'UserName')
BEGIN
	ALTER TABLE dbo.PayrollAttendanceDevice
		ADD UserName NVARCHAR(225) NULL
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollAttendanceDevice' AND column_name = 'AdjustmentTime')
BEGIN
	ALTER TABLE dbo.PayrollAttendanceDevice
		ADD AdjustmentTime INT NULL
END
GO

/****** Object:  Table [dbo].[PayrollAppraisalRatingScale]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAppraisalRatingScale]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAppraisalRatingScale](
	[RatingScaleId] [int] IDENTITY(1,1) NOT NULL,
	[RatingScaleName] [varchar](50) NOT NULL,
	[IsRemarksMandatory] [bit] NOT NULL,
	[RatingValue] [decimal](18, 0) NULL,
	[Remarks] [varchar](150) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollAppraisalRatingScale] PRIMARY KEY CLUSTERED 
(
	[RatingScaleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollAppraisalRatingFactor]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAppraisalRatingFactor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAppraisalRatingFactor](
	[RatingFactorId] [int] IDENTITY(1,1) NOT NULL,
	[AppraisalIndicatorId] [int] NOT NULL,
	[RatingFactorName] [varchar](250) NOT NULL,
	[RatingWeight] [decimal](18, 0) NOT NULL,
	[Remarks] [varchar](150) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollAppraisalRatingFactor] PRIMARY KEY CLUSTERED 
(
	[RatingFactorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollAppraisalMarksIndicator]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAppraisalMarksIndicator]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAppraisalMarksIndicator](
	[MarksIndicatorId] [int] IDENTITY(1,1) NOT NULL,
	[AppraisalIndicatorName] [varchar](150) NOT NULL,
	[AppraisalWeight] [decimal](18, 2) NOT NULL,
	[Remarks] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollAppraisalMarksIndicator] PRIMARY KEY CLUSTERED 
(
	[MarksIndicatorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollAppraisalEvalutionRatingFactorDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAppraisalEvalutionRatingFactorDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAppraisalEvalutionRatingFactorDetails](
	[RatingFacotrDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[AppraisalEvalutionById] [int] NOT NULL,
	[MarksIndicatorId] [int] NOT NULL,
	[AppraisalRatingFactorId] [int] NOT NULL,
	[AppraisalWeight] [decimal](18, 2) NOT NULL,
	[RatingWeight] [decimal](18, 2) NOT NULL,
	[RatingValue] [decimal](18, 2) NULL,
	[Marks] [decimal](18, 2) NOT NULL,
	[Remarks] [varchar](200) NULL,
 CONSTRAINT [PK_PayrollAppraisalEvalutionRatingFactorDetails] PRIMARY KEY CLUSTERED 
(
	[RatingFacotrDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollAppraisalEvalutionBy]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAppraisalEvalutionBy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAppraisalEvalutionBy](
	[AppraisalEvalutionById] [int] IDENTITY(1,1) NOT NULL,
	[AppraisalConfigType] [nvarchar](15) NOT NULL,
	[EvalutiorId] [int] NOT NULL,
	[EmpId] [int] NULL,
	[EvalutionCompletionBy] [datetime] NULL,
	[AppraisalType] [varchar](50) NOT NULL,
	[EvaluationFromDate] [date] NOT NULL,
	[EvaluationToDate] [date] NOT NULL,
	[ApprovalStatus] [nvarchar](25) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollAppraisalEvalutionBy] PRIMARY KEY CLUSTERED 
(
	[AppraisalEvalutionById] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollApplicantResult]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollApplicantResult]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollApplicantResult](
	[ApplicantResultId] [bigint] IDENTITY(1,1) NOT NULL,
	[JobCircularId] [bigint] NOT NULL,
	[ApplicantId] [bigint] NOT NULL,
	[InterviewTypeId] [smallint] NOT NULL,
	[MarksObtain] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollApplicantResult] PRIMARY KEY CLUSTERED 
(
	[ApplicantResultId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PayrollAllowanceDeductionHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAllowanceDeductionHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAllowanceDeductionHead](
	[AllowDeductId] [int] IDENTITY(1,1) NOT NULL,
	[AllowDeductName] [varchar](200) NOT NULL,
	[AllowDeductType] [varchar](50) NOT NULL,
	[TransactionType] [varchar](50) NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_AllowanceDeductionHead] PRIMARY KEY CLUSTERED 
(
	[AllowDeductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MenuGroupNLinkIcon]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MenuGroupNLinkIcon]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MenuGroupNLinkIcon](
	[IconId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Class] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_MenuGroupNLinkIcon] PRIMARY KEY CLUSTERED 
(
	[IconId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MemMemberType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemMemberType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemMemberType](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[SubscriptionFee] [decimal](18, 2) NULL,
	[DiscountPercent] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_MemMemberType] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MemMemberReference]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemMemberReference]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemMemberReference](
	[ReferenceId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NULL,
	[Arbitrator] [varchar](200) NULL,
	[ArbitratorMode] [varchar](50) NULL,
	[Relationship] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_MemMemberReference] PRIMARY KEY CLUSTERED 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MemMemberFamilyMember]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemMemberFamilyMember]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemMemberFamilyMember](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NULL,
	[MemberName] [varchar](200) NULL,
	[MemberDOB] [datetime] NULL,
	[Occupation] [varchar](100) NULL,
	[Relationship] [varchar](50) NULL,
	[UsageMode] [varchar](20) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_MemMemberFamilyMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MemMemberBasics]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemMemberBasics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemMemberBasics](
	[MemberId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[NodeId] [int] NULL,
	[TypeId] [int] NULL,
	[MembershipNumber] [varchar](50) NULL,
	[NameTitle] [varchar](50) NULL,
	[FullName] [varchar](256) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](50) NOT NULL,
	[MemberGender] [int] NOT NULL,
	[FatherName] [varchar](256) NULL,
	[MotherName] [varchar](256) NULL,
	[BirthDate] [datetime] NOT NULL,
	[MemberAddress] [varchar](500) NOT NULL,
	[ResidencePhone] [varchar](256) NULL,
	[OfficePhone] [varchar](256) NULL,
	[MobileNumber] [varchar](256) NULL,
	[PersonalEmail] [varchar](256) NULL,
	[OfficeEmail] [varchar](256) NULL,
	[HomeFax] [varchar](256) NULL,
	[OfficeFax] [varchar](256) NULL,
	[MaritalStatus] [int] NOT NULL,
	[BloodGroup] [int] NOT NULL,
	[RegistrationDate] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[MailAddress] [varchar](500) NULL,
	[Nationality] [int] NULL,
	[PassportNumber] [varchar](256) NULL,
	[Organization] [varchar](256) NULL,
	[Occupation] [varchar](256) NULL,
	[Designation] [varchar](256) NULL,
	[AnnualTurnover] [decimal](18, 2) NULL,
	[MonthlyIncome] [decimal](18, 2) NULL,
	[SecurityDeposit] [decimal](18, 2) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[Balance] [money] NULL,
 CONSTRAINT [PKMemMemberBasics] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'AttendanceDeviceMemberId')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD AttendanceDeviceMemberId INT NULL
END
GO 

/****** Object:  Table [dbo].[MemOnlineMemEducation]    Script Date: 03/05/2019 2:04:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemOnlineMemEducation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemOnlineMemEducation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NULL,
	[Degree] [nvarchar](50) NULL,
	[Institution] [nvarchar](50) NULL,
	[PassingYear] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_MemOnlineMemEducation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[MemberPaymentConfiguration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberPaymentConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemberPaymentConfiguration](
	[MemPaymentConfigId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionType] [nvarchar](50) NULL,
	[MemberTypeOrMemberId] [bigint] NOT NULL,
	[BillingPeriod] [nvarchar](50) NULL,
	[BillingAmount] [decimal](18, 2) NULL,
	[BillingStartDate] [datetime] NULL,
	[DoorAccessDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_MemberPaymentConfiguration] PRIMARY KEY CLUSTERED 
(
	[MemPaymentConfigId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[LCPaymentLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCPaymentLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LCPaymentLedger](
	[LCPaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentType] [nvarchar](15) NOT NULL,
	[BillNumber] [varchar](25) NULL,
	[PaymentDate] [date] NOT NULL,
	[LCId] [int] NOT NULL,
	[LCBankAccountHeadId] [int] NULL,
	[AccountHeadId] [int] NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[CurrencyAmount] [money] NULL,
	[Remarks] [varchar](500) NULL,
	[PaymentStatus] [varchar](20) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LCPaymentLedger] PRIMARY KEY CLUSTERED 
(
	[LCPaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LCPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LCPayment](
	[PaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[LCId] [bigint] NOT NULL,
	[AccountHeadId] [int] NULL,
	[CurrencyId] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_LCPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[LCOverHeadName]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCOverHeadName]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LCOverHeadName](
	[OverHeadId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
	[OverHeadName] [varchar](200) NULL,
	[Description] [varchar](300) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LCOverHeadName] PRIMARY KEY CLUSTERED 
(
	[OverHeadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LCOverHeadExpense]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCOverHeadExpense]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LCOverHeadExpense](
	[ExpenseId] [int] IDENTITY(1,1) NOT NULL,
	[LCId] [int] NULL,
	[OverHeadId] [int] NULL,
	[ExpenseDate] [datetime] NULL,
	[ExpenseAmount] [decimal](18, 2) NULL,
	[Description] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LCOverHeadExpense] PRIMARY KEY CLUSTERED 
(
	[ExpenseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'Status')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD [Status] [nvarchar](15) NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'Status')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ALTER COLUMN [Status] [nvarchar](50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD [LastModifiedBy] INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'LastModifiedDate')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD [LastModifiedDate] DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD [CheckedBy] INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'CheckedDate')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD [CheckedDate] DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD [ApprovedBy] INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'ApprovedDate')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD [ApprovedDate] DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'TransactionAccountHeadId')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD TransactionAccountHeadId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'TransactionNo')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD TransactionNo NVARCHAR(25) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'PaymentMode')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD PaymentMode	NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'ChequeNumber')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD ChequeNumber NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'BankId')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD BankId	INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'CurrencyId')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD CurrencyId	INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'ConversionRate')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD ConversionRate	DECIMAL(18, 2) NULL
END
GO
/****** Object:  Table [dbo].[LCInformationDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCInformationDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LCInformationDetail](
	[LCDetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[LCId] [bigint] NOT NULL,
	[POrderId] [int] NULL,
	[CostCenterId] [int] NULL,
	[StockById] [int] NULL,
	[ProductId] [int] NULL,
	[PurchasePrice] [decimal](18, 0) NULL,
	[Quantity] [decimal](18, 0) NULL,
 CONSTRAINT [PK_LCInformationDetail] PRIMARY KEY CLUSTERED 
(
	[LCDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'QuantityReceived')
BEGIN
	ALTER TABLE dbo.LCInformationDetail
		ADD QuantityReceived DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'RemainingReceiveQuantity')
BEGIN
	ALTER TABLE dbo.LCInformationDetail
		ADD RemainingReceiveQuantity DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'ActualReceivedQuantity')
BEGIN
	ALTER TABLE dbo.LCInformationDetail
		ADD ActualReceivedQuantity DECIMAL(18,2) NULL
END
GO

/****** Object:  Table [dbo].[LCInformation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LCInformation](
	[LCId] [bigint] IDENTITY(1,1) NOT NULL,
	[LCNumber] [nvarchar](50) NULL,
	[PINumber] [nvarchar](50) NULL,
	[LCValue] [nvarchar](50) NULL,
	[LCOpenDate] [datetime] NULL,
	[LCMatureDate] [datetime] NULL,
	[SupplierId] [int] NULL,
	[ApprovedStatus] [varchar](20) NULL,
	[CheckedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsLCBankSettlement] [bit] NULL,
	[BankSettlementBy] [int] NULL,
	[BankSettlementDate] [datetime] NULL,
	[IsLCSettlement] [bit] NULL,
	[SettlementBy] [int] NULL,
	[SettlementDate] [datetime] NULL,
	[SettlementDescription] [nvarchar](500) NULL,
 CONSTRAINT [PK_LCInformation] PRIMARY KEY CLUSTERED 
(
	[LCId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'ReceivedStatus')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD ReceivedStatus VARCHAR(50) NULL
END
GO
/****** Object:  Table [dbo].[InvUserGroupCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvUserGroupCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvUserGroupCostCenterMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[UserGroupId] [int] NULL,
 CONSTRAINT [PK_InvUserGroupCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvUnitHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvUnitHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvUnitHead](
	[UnitHeadId] [int] IDENTITY(1,1) NOT NULL,
	[HeadName] [nvarchar](100) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_InvUnitHead] PRIMARY KEY CLUSTERED 
(
	[UnitHeadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvUnitConversion]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvUnitConversion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvUnitConversion](
	[ConversionId] [int] IDENTITY(1,1) NOT NULL,
	[FromUnitHeadId] [int] NULL,
	[ToUnitHeadId] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvUnitConversion] PRIMARY KEY CLUSTERED 
(
	[ConversionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvTransactionMode]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvTransactionMode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvTransactionMode](
	[TModeId] [int] IDENTITY(1,1) NOT NULL,
	[HeadName] [varchar](300) NULL,
	[CalculationType] [varchar](20) NOT NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_InvTransactionMode] PRIMARY KEY CLUSTERED 
(
	[TModeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvServicePriceMatrix]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvServicePriceMatrix]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvServicePriceMatrix](
	[ServicePriceMatrixId] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[ServicePackageId] [int] NOT NULL,
	[ServiceBandWidthId] [int] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvServicePriceMatrix] PRIMARY KEY CLUSTERED 
(
	[ServicePriceMatrixId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'ActiveStat')
BEGIN
	EXEC sp_RENAME 'InvServicePriceMatrix.ActiveStat' , 'IsActive', 'COLUMN'
END 
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'Description')
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
		ADD Description NVARCHAR(MAX) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'ShareRatio')
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
		ADD ShareRatio NVARCHAR(250) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'UplinkFrequencyId')
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
		ADD UplinkFrequencyId INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'DownlinkFrequencyId')
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
		ADD DownlinkFrequencyId INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'UplinkFrequencyUnit')
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
		ADD UplinkFrequencyUnit NVARCHAR(25) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'DownlinkFrequencyUnit')
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
		ADD DownlinkFrequencyUnit NVARCHAR(25) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePriceMatrix' AND column_name = 'PackageName')
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
		ADD PackageName NVARCHAR(350) NULL
END
GO 
IF(SELECT COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'InvServicePriceMatrix' AND COLUMN_NAME = 'ServicePackageId') IS NULL
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
			ADD CONSTRAINT DF_InvServicePriceMatrix_ServicePackageId DEFAULT 0 FOR ServicePackageId;
END
GO 
IF(SELECT COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'InvServicePriceMatrix' AND COLUMN_NAME = 'ServiceBandWidthId') IS NULL
BEGIN
	ALTER TABLE dbo.InvServicePriceMatrix
			ADD CONSTRAINT DF_InvServicePriceMatrix_ServiceBandWidthId DEFAULT 0 FOR ServiceBandWidthId;
END
GO 
/****** Object:  Table [dbo].[InvServicePackage]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvServicePackage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvServicePackage](
	[ServicePackageId] [int] IDENTITY(1,1) NOT NULL,
	[PackageName] [varchar](250) NOT NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvServicePackage] PRIMARY KEY CLUSTERED 
(
	[ServicePackageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServicePackage' AND column_name = 'ActiveStat')
BEGIN
	EXEC sp_RENAME 'InvServicePackage.ActiveStat' , 'IsActive', 'COLUMN'
END 
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvServiceBandwidth]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvServiceBandwidth]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvServiceBandwidth](
	[ServiceBandWidthId] [int] IDENTITY(1,1) NOT NULL,
	[BandWidthName] [varchar](250) NOT NULL,
	[BandWidthValue] [int] NULL,
	[BandWidth] [nvarchar](25) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvServiceBandwidth] PRIMARY KEY CLUSTERED 
(
	[ServiceBandWidthId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
--Rename Col
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServiceBandwidth' AND column_name = 'BandWidthValue')
	EXEC sp_RENAME 'InvServiceBandwidth.BandWidthValue' , 'Uplink', 'COLUMN'
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServiceBandwidth' AND column_name = 'BandWidth')
	EXEC sp_RENAME 'InvServiceBandwidth.BandWidth' , 'UplinkFrequency', 'COLUMN'
GO

-- Add col
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServiceBandwidth' AND column_name = 'Downlink')
BEGIN
	ALTER TABLE dbo.InvServiceBandwidth
		ADD Downlink int NULL
END
GO  
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServiceBandwidth' AND column_name = 'DownlinkFrequency')
BEGIN
	ALTER TABLE dbo.InvServiceBandwidth
		ADD DownlinkFrequency NVARCHAR(500) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvServiceBandwidth' AND column_name = 'Description')
BEGIN
	ALTER TABLE dbo.InvServiceBandwidth
		ADD Description NVARCHAR(500) NULL
END
GO  

/****** Object:  Table [dbo].[InvRecipeDeductionDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvRecipeDeductionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvRecipeDeductionDetails](
	[ReceipeDeductionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[FinishProductRBillId] [int] NULL,
	[ItemIdMain] [int] NULL,
	[ItemMainName] [nvarchar](250) NULL,
	[ItemId] [int] NULL,
	[RowIndex] [int] NULL,
	[RecipeItemId] [int] NULL,
	[RecipeItemName] [nvarchar](250) NULL,
	[UnitHeadId] [int] NULL,
	[ItemUnit] [decimal](18, 5) NULL,
	[ConvertionUnit] [decimal](18, 5) NULL,
	[TotalUnitWillDeduct] [decimal](18, 5) NULL,
	[UnitDeduction] [decimal](18, 5) NULL,
	[RecipeDeduction] [decimal](18, 5) NULL,
	[UnitDiffernce] [decimal](18, 5) NULL,
	[QuantityMain] [decimal](18, 5) NULL,
	[ParentQuantity] [decimal](18, 5) NULL,
	[StockQuantity] [decimal](18, 5) NULL,
	[IsRecipeExist] [bit] NULL,
	[IsRecipe] [bit] NULL,
	[DeductionDate] [datetime] NULL,
	[AverageCost] [decimal](18, 5) NULL,
	[DeductionQuantity] [decimal](18, 5) NULL,
	[TotalCost] [decimal](18, 5) NULL,
	[LocationId] [int] NOT NULL,
 CONSTRAINT [PK_InvRecipeDeductionDetails] PRIMARY KEY CLUSTERED 
(
	[ReceipeDeductionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- -- ItemMainName Column Size Update(250 to MAX)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvRecipeDeductionDetails' AND column_name = 'ItemMainName')
BEGIN
ALTER TABLE dbo.InvRecipeDeductionDetails
   ALTER COLUMN ItemMainName nvarchar(MAX) NULL; 
END 
GO

-- -- RecipeItemName Column Size Update(250 to MAX)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvRecipeDeductionDetails' AND column_name = 'RecipeItemName')
BEGIN
ALTER TABLE dbo.InvRecipeDeductionDetails
   ALTER COLUMN RecipeItemName nvarchar(MAX) NULL; 
END 
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvRecipeDeductionDetails' AND column_name = 'KotId')
BEGIN
	ALTER TABLE dbo.InvRecipeDeductionDetails
		ADD KotId INT NULL;
END
GO

/****** Object:  Table [dbo].[InvManufacturer]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvManufacturer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvManufacturer](
	[ManufacturerId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](256) NULL,
	[Code] [varchar](20) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PMManufacturer] PRIMARY KEY CLUSTERED 
(
	[ManufacturerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvLocationCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvLocationCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvLocationCostCenterMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[LocationId] [int] NULL,
 CONSTRAINT [PK_InvLocationCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvLocation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvLocation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvLocation](
	[LocationId] [int] IDENTITY(1,1) NOT NULL,
	[AncestorId] [int] NULL,
	[Lvl] [int] NOT NULL,
	[Hierarchy] [varchar](900) NULL,
	[HierarchyIndex] [varchar](900) NULL,
	[Name] [varchar](50) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[IsStoreLocation] [bit] NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvLocation] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvItemTransactionPaymentDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemTransactionPaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemTransactionPaymentDetails](
	[TransactionPaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[TransactionDate] [date] NOT NULL,
	[LocationId] [int] NULL,
	[PaymentType] [nvarchar](25) NULL,
	[CardType] [nvarchar](350) NULL,
	[PaymentAmount] [money] NULL,
 CONSTRAINT [PK_InvItemTransactionPaymentDetails] PRIMARY KEY CLUSTERED 
(
	[TransactionPaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvItemTransactionLogSummary]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemTransactionLogSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemTransactionLogSummary](
	[ItemTransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[DayOpeningQuantity] [decimal](18, 2) NOT NULL,
	[TransactionalOpeningQuantity] [decimal](18, 5) NOT NULL,
	[ReceiveQuantity] [decimal](18, 5) NULL,
	[OutItemQuantity] [decimal](18, 5) NULL,
	[WastageQuantity] [decimal](18, 5) NULL,
	[AdjustmentQuantity] [decimal](18, 5) NULL,
	[SalesQuantity] [decimal](18, 5) NULL,
	[ClosingQuantity] [decimal](18, 5) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_InvItemTransactionLogSummary] PRIMARY KEY CLUSTERED 
(
	[ItemTransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvItemTransactionLog]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemTransactionLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemTransactionLog](
	[ItemTransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionType] [nvarchar](25) NOT NULL,
	[TransactionForId] [bigint] NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[DayOpeningQuantity] [decimal](18, 2) NOT NULL,
	[TransactionalOpeningQuantity] [decimal](18, 5) NOT NULL,
	[TransactionQuantity] [decimal](18, 5) NOT NULL,
	[ClosingQuantity] [decimal](18, 5) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_InvItemTransactionSummary] PRIMARY KEY CLUSTERED 
(
	[ItemTransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionLog' AND column_name = 'TransactionType' AND DATA_TYPE = 'NVARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='25')
BEGIN
   ALTER TABLE dbo.InvItemTransactionLog
	   ALTER COLUMN TransactionType NVARCHAR(MAX) NOT NULL;
END
GO

/****** Object:  Table [dbo].[InvItemTransactionDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemTransactionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemTransactionDetails](
	[TransactionDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[LocationId] [int] NULL,
	[CategoryId] [int] NULL,
	[ItemCode] [nvarchar](50) NULL,
	[ItemId] [int] NULL,
	[ItemName] [nvarchar](500) NULL,
	[IsCustomerItem] [bit] NULL,
	[StockById] [int] NULL,
	[StockBy] [nvarchar](150) NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[BeginingStockQuantity] [decimal](18, 5) NULL,
	[PurchaseQuantity] [decimal](18, 5) NULL,
	[PurchasePrice] [money] NULL,
	[UsageQuantity] [decimal](18, 5) NULL,
	[AverageUsageCost] [decimal](18, 2) NULL,
	[WastageQuantity] [decimal](18, 5) NULL,
	[WastageCost] [money] NULL,
	[WastageAllowance] [nvarchar](300) NULL,
	[WastageReason] [nvarchar](300) NULL,
	[AdjustmentStockQuantity] [decimal](18, 5) NULL,
	[DayEndStockQuantity] [decimal](18, 5) NULL,
	[StockCountDifference] [decimal](18, 5) NULL,
	[StockCountDifferenceAmount] [money] NULL,
	[PriceToday] [money] NULL,
	[PriceYestarday] [money] NULL,
	[PriceFluctuation] [money] NULL,
	[SalesQuantity] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[Amount] [decimal](18, 2) NULL,
	[DiscountedAmount] [decimal](18, 2) NULL,
	[ServiceRate] [decimal](18, 2) NULL,
	[Discount] [decimal](18, 2) NULL,
	[Vat] [money] NULL,
	[ServiceCharge] [money] NULL,
	[PerGuestUsageQuantity] [decimal](18, 5) NULL,
	[PerGuestUsageAmount] [money] NULL,
 CONSTRAINT [PK_InvItemTransactionDetails] PRIMARY KEY CLUSTERED 
(
	[TransactionDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionDetails' AND column_name = 'ItemName' AND DATA_TYPE = 'NVARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='500')
BEGIN
   ALTER TABLE dbo.InvItemTransactionDetails
	   ALTER COLUMN ItemName VARCHAR(MAX) NULL;
END
GO

/****** Object:  Table [dbo].[InvItemTransaction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemTransaction](
	[TransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[StartBillNumber] [nvarchar](150) NULL,
	[EndingBillNumber] [nvarchar](150) NULL,
	[TotalBillCount] [smallint] NULL,
	[TotalSalesQuantity] [decimal](18, 2) NULL,
	[TotalSalesAmount] [money] NULL,
	[TotalDiscountAmount] [money] NULL,
	[TotalNetSalesAmount] [money] NULL,
	[TotalServiceChargeAmount] [money] NULL,
	[TotalVatAmount] [money] NULL,
	[GrossSalesAmount] [money] NULL,
	[TotalVoidQuantity] [smallint] NULL,
	[TotalVoidAmount] [money] NULL,
	[TotalError] [smallint] NULL,
	[TotalErrorAmount] [money] NULL,
	[TotalPax] [smallint] NULL,
	[TotalVariance] [decimal](18, 5) NULL,
	[TotalVarianceAmount] [money] NULL,
	[WastageEntryById] [int] NULL,
	[WastageEntryByName] [nvarchar](450) NULL,
	[TotalStockCountDeifference] [decimal](18, 5) NULL,
	[TotalStockCountDeifferenceAmount] [money] NULL,
	[AdjustmentEntryById] [int] NULL,
	[AdjustmentEntryByName] [nvarchar](450) NULL,
	[TotalReceivedQuantity] [decimal](18, 5) NULL,
	[TotalReceivedAmount] [money] NULL,
	[TotalUsageQuantity] [decimal](18, 5) NULL,
	[TotalUsageCost] [money] NULL,
	[TotalCashPayment] [money] NULL,
	[TotalCardPayment] [money] NULL,
	[TotalPayment] [money] NULL,
	[TotalRefundAmount] [money] NULL,
	[TotalRevenue] [money] NULL,
	[TotalPerGuestUsageQuantity] [decimal](18, 5) NULL,
	[TotalPerGuestUsageAmount] [money] NULL,
 CONSTRAINT [PK_InvItemTransaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvItemSupplierMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemSupplierMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemSupplierMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NULL,
	[ItemId] [int] NULL,
 CONSTRAINT [PK_InvItemSupplierMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvItemStockVarianceDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockVarianceDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemStockVarianceDetails](
	[StockVarianceDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[StockVarianceId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[StockById] [int] NOT NULL,
	[TModeId] [int] NOT NULL,
	[UsageQuantity] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[UsageCost] [decimal](18, 2) NULL,
	[VarianceQuantity] [decimal](18, 2) NOT NULL,
	[VarianceCost] [decimal](18, 2) NOT NULL,
	[Reason] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_InvItemStockVarianceDetails] PRIMARY KEY CLUSTERED 
(
	[StockVarianceDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvItemStockVariance]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockVariance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemStockVariance](
	[StockVarianceId] [int] IDENTITY(1,1) NOT NULL,
	[StockVarianceDate] [datetime] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[ApprovedStatus] [nvarchar](15) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvItemStockVariance] PRIMARY KEY CLUSTERED 
(
	[StockVarianceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockVariance' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.InvItemStockVariance
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockVariance' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.InvItemStockVariance
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO
/****** Object:  Table [dbo].[InvItemStockInformation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemStockInformation](
	[StockId] [int] IDENTITY(1,1) NOT NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NULL,
	[StockQuantity] [decimal](18, 5) NULL,
 CONSTRAINT [PK_InvRack] PRIMARY KEY CLUSTERED 
(
	[StockId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformation' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformation
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformation' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformation
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformation' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformation
		ADD ColorId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformation' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformation
		ADD SizeId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformation' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformation
		ADD StyleId INT NULL
END
GO

/****** Object:  Table [dbo].[InvItemStockAdjustmentDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockAdjustmentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemStockAdjustmentDetails](
	[StockAdjustmentDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[StockAdjustmentId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[StockById] [int] NOT NULL,
	[OpeningQuantity] [decimal](18, 2) NULL,
	[ReceiveQuantity] [decimal](18, 2) NULL,
	[ActualUsage] [decimal](18, 2) NULL,
	[WastageQuantity] [decimal](18, 2) NULL,
	[StockQuantity] [decimal](18, 2) NULL,
	[ActualQuantity] [decimal](18, 2) NULL,
	[VarianceQuantity] [decimal](18, 2) NULL,
	[AverageCost] [decimal](18, 5) NULL,
 CONSTRAINT [PK_InvItemStockAdjustmentDetails] PRIMARY KEY CLUSTERED 
(
	[StockAdjustmentDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockAdjustmentDetails' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.InvItemStockAdjustmentDetails
		ADD ColorId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockAdjustmentDetails' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.InvItemStockAdjustmentDetails
		ADD SizeId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockAdjustmentDetails' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.InvItemStockAdjustmentDetails
		ADD StyleId INT NULL
END
GO

/****** Object:  Table [dbo].[InvItemStockAdjustment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockAdjustment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemStockAdjustment](
	[StockAdjustmentId] [int] IDENTITY(1,1) NOT NULL,
	[AdjustmentDate] [datetime] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[AdjustmentFrequency] [varchar](50) NULL,
	[ApprovedStatus] [nvarchar](15) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LocationId] [int] NULL,
 CONSTRAINT [PK_InvStockAdjustment] PRIMARY KEY CLUSTERED 
(
	[StockAdjustmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockAdjustment' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.InvItemStockAdjustment
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockAdjustment' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.InvItemStockAdjustment
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO
/****** Object:  Table [dbo].[InvItemSpecialRemarks]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemSpecialRemarks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemSpecialRemarks](
	[SpecialRemarksId] [int] IDENTITY(1,1) NOT NULL,
	[SpecialRemarks] [varchar](200) NULL,
	[Description] [varchar](300) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantSpecialRemarks] PRIMARY KEY CLUSTERED 
(
	[SpecialRemarksId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvItemDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemDetails](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NULL,
	[ItemDetailId] [int] NULL,
	[ItemUnit] [decimal](18, 2) NULL,
 CONSTRAINT [PK_InvItemDetails] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvItemCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemCostCenterMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[ItemId] [int] NULL,
	[KitchenId] [int] NULL,
	[MinimumStockLevel] [decimal](18, 2) NULL,
	[SellingLocalCurrencyId] [int] NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[SellingUsdCurrencyId] [int] NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[SDCharge] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[DiscountType] [varchar](50) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[StockQuantity] [decimal](18, 2) NULL,
	[AdjustmentLastDate] [datetime] NULL,
	[AverageCostDelete] [decimal](18, 2) NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_PMItemCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvItemClassification]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemClassification]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemClassification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](300) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [bigint] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvItemClassification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[InvItemClassification] ADD  CONSTRAINT [DF_InvItemClassification_IsActive]  DEFAULT ((1)) FOR [IsActive]

END
GO
--Update InvItem table for new InvItemClassification table creation 
IF NOT EXISTS (SELECT Id FROM InvItemClassification)
BEGIN
	INSERT INTO InvItemClassification(Name,IsActive,CreatedDate)
	SELECT FieldValue,ActiveStat,GETDATE() FROM CommonCustomFieldData WHERE FieldType = 'InvItemClassification'

	UPDATE InvItem
	SET ClassificationId = iic.Id
	FROM CommonCustomFieldData ccfd INNER JOIN InvItemClassification iic
	ON ccfd.FieldType = 'InvItemClassification' AND iic.Name = ccfd.FieldValue
	WHERE ClassificationId = ccfd.FieldId
END
GO
/****** Object:  Table [dbo].[InvItemClassificationCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemClassificationCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemClassificationCostCenterMapping](
	[MappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[ClassificationId] [int] NULL,
 CONSTRAINT [PK_InvItemClassificationCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemClassificationCostCenterMapping' AND column_name = 'AccountsPostingHeadId')
BEGIN
	ALTER TABLE dbo.InvItemClassificationCostCenterMapping
		ADD AccountsPostingHeadId BIGINT  NULL;
END
GO

-- -- -- This is Only for First Time Mapping Data
--IF EXISTS (SELECT MappingId FROM InvItemClassificationCostCenterMapping)
--BEGIN
--	TRUNCATE TABLE InvItemClassificationCostCenterMapping
--END

--Update InvItemClassificationCostCenterMapping table for Classification Mapping
IF NOT EXISTS (SELECT MappingId FROM InvItemClassificationCostCenterMapping)
BEGIN	
	INSERT [dbo].[InvItemClassificationCostCenterMapping] ([CostCenterId], [ClassificationId], [AccountsPostingHeadId])
	
	SELECT cc.CostCenterId, mappingdata.ClassificationId, mappingdata.AccountsPostingHeadId
	FROM
	(
		SELECT iic.Id AS ClassificationId, CAST(CAST(ccfd.Description AS VARCHAR(50)) AS INT) AS AccountsPostingHeadId 
		FROM CommonCustomFieldData ccfd INNER JOIN InvItemClassification iic
		ON ccfd.FieldType = 'InvItemClassification' AND iic.Name = ccfd.FieldValue
	)mappingdata, CommonCostCenter cc
END
GO	

-- Primary key MappindId type changed from INT to BIGINT

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemClassificationCostCenterMapping' AND column_name = 'MappingId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[InvItemClassificationCostCenterMapping]') AND name = N'PK_InvItemClassificationCostCenterMapping')
 ALTER TABLE [dbo].[InvItemClassificationCostCenterMapping] DROP CONSTRAINT [PK_InvItemClassificationCostCenterMapping]
 
 ALTER TABLE dbo.InvItemClassificationCostCenterMapping
    ALTER COLUMN MappingId bigint NOT NULL;
 
 ALTER TABLE [dbo].[InvItemClassificationCostCenterMapping] ADD  CONSTRAINT [PK_InvItemClassificationCostCenterMapping] PRIMARY KEY CLUSTERED 
 (
  [MappingId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   
END 

--- CostCenterId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemClassificationCostCenterMapping' AND column_name = 'CostCenterId')
BEGIN
ALTER TABLE dbo.InvItemClassificationCostCenterMapping
   ALTER COLUMN CostCenterId BIGINT NULL; 
END 
GO

--- ClassificationId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemClassificationCostCenterMapping' AND column_name = 'ClassificationId')
BEGIN
ALTER TABLE dbo.InvItemClassificationCostCenterMapping
   ALTER COLUMN ClassificationId BIGINT NULL; 
END 
GO

-- -- ADD Foreign Key FK_InvItemClassificationCostCenterMapping_InvItemClassification (primary key of InvItemClassification)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_InvItemClassificationCostCenterMapping_InvItemClassification')
   AND parent_object_id = OBJECT_ID(N'dbo.InvItemClassificationCostCenterMapping'))
BEGIN
	ALTER TABLE [dbo].[InvItemClassificationCostCenterMapping]  WITH CHECK ADD  CONSTRAINT [FK_InvItemClassificationCostCenterMapping_InvItemClassification] FOREIGN KEY([ClassificationId])
	REFERENCES [dbo].[InvItemClassification] ([Id])
	
	ALTER TABLE [dbo].[InvItemClassificationCostCenterMapping] CHECK CONSTRAINT [FK_InvItemClassificationCostCenterMapping_InvItemClassification]
END
GO

/****** Object:  Table [dbo].[InvItem]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItem](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ItemType] [varchar](100) NULL,
	[Name] [varchar](max) NULL,
	[DisplayName] [varchar](max) NULL,
	[Code] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[CategoryId] [int] NULL,
	[StockType] [varchar](50) NULL,
	[StockBy] [int] NULL,
	[SalesStockBy] [int] NULL,
	[ClassificationId] [int] NULL,
	[IsCustomerItem] [bit] NULL,
	[IsSupplierItem] [bit] NULL,
	[ManufacturerId] [int] NULL,
	[ProductType] [varchar](20) NULL,
	[PurchasePrice] [decimal](18, 2) NULL,
	[SellingLocalCurrencyId] [int] NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[SellingUsdCurrencyId] [int] NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
	[ServiceWarranty] [int] NULL,
	[ImageName] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[AverageCost] [decimal](18, 5) NULL,
	[IsRecipe] [bit] NULL,
	[SupplierId] [int] NULL,
	[AdjustmentFrequency] [varchar](50) NULL,
	[AdjustmentLastDate] [datetime] NULL,
	[IsItemEditable] [bit] NULL,
 CONSTRAINT [PK_PMProduct] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvInventoryAccountVsItemCategoryMappping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvInventoryAccountVsItemCategoryMappping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvInventoryAccountVsItemCategoryMappping](
	[InvAccountMapId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvInventoryAccountVsItemCategoryMappping] PRIMARY KEY CLUSTERED 
(
	[InvAccountMapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvDineTimeWisePaymentDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvDineTimeWisePaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvDineTimeWisePaymentDetails](
	[TransactionPaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[TransactionDate] [date] NOT NULL,
	[DineTimeFrom] [time](3) NOT NULL,
	[DineTimeTo] [time](3) NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[PaymentType] [nvarchar](25) NOT NULL,
	[CardType] [nvarchar](250) NULL,
	[PaymentAmount] [money] NOT NULL,
	[PaymentNo] [int] NOT NULL,
 CONSTRAINT [PK_InvDineTimeWisePaymentDetails] PRIMARY KEY CLUSTERED 
(
	[TransactionPaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvDineTimeWiseItemTransactionDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvDineTimeWiseItemTransactionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvDineTimeWiseItemTransactionDetails](
	[TransactionDetailsId] [bigint] NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[DineTime] [time](7) NOT NULL,
	[TotalSales] [money] NOT NULL,
	[DiscountAmount] [money] NOT NULL,
	[ServiceCharge] [money] NOT NULL,
	[VatAmount] [money] NOT NULL,
	[GrnadTotal] [money] NOT NULL,
	[NetSales] [money] NOT NULL,
	[TTLNetSales] [money] NOT NULL,
	[Pax] [smallint] NOT NULL,
	[TTLPax] [decimal](18, 2) NOT NULL,
	[AverageGuest] [decimal](18, 2) NOT NULL,
	[Checks] [decimal](18, 2) NOT NULL,
	[TTLChecks] [decimal](18, 2) NOT NULL,
	[AverageChecks] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_InvDineTimeWiseItemTransactionDetails] PRIMARY KEY CLUSTERED 
(
	[TransactionDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvDineTimeWiseItemTransaction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvDineTimeWiseItemTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvDineTimeWiseItemTransaction](
	[TransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [date] NOT NULL,
	[TotalSalesQuantity] [decimal](18, 5) NOT NULL,
	[TotalSales] [money] NOT NULL,
	[DiscountAmount] [money] NOT NULL,
	[Netsales] [money] NOT NULL,
	[ServiceCharge] [money] NOT NULL,
	[VatAmount] [money] NOT NULL,
	[GrandTotal] [money] NOT NULL,
	[TotalPax] [int] NOT NULL,
	[TotalRevenue] [money] NOT NULL,
	[TotalVoid] [int] NOT NULL,
	[TotalVoidAmount] [money] NOT NULL,
	[ErrorCorrects] [smallint] NOT NULL,
	[ErrorCorrectsAmount] [money] NOT NULL,
	[Checks] [smallint] NOT NULL,
	[ChecksAmount] [money] NOT NULL,
	[ChecksPaid] [smallint] NOT NULL,
	[ChecksPaidAmount] [money] NOT NULL,
	[Outstanding] [smallint] NOT NULL,
	[OutstandingAmount] [money] NOT NULL,
	[TotalCashPayment] [money] NOT NULL,
	[TotalCardPayment] [money] NOT NULL,
	[TotalRefund] [money] NOT NULL,
 CONSTRAINT [PK_InvDineTimeWiseItemTransaction_1] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvDefaultClassificationConfiguration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvDefaultClassificationConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvDefaultClassificationConfiguration](
	[ConfigurationId] [int] IDENTITY(1,1) NOT NULL,
	[ClassificationId] [int] NULL,
 CONSTRAINT [PK_InvDefaultClassificationConfiguration] PRIMARY KEY CLUSTERED 
(
	[ConfigurationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvCostCenterNDineTimeWiseItemTransaction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvCostCenterNDineTimeWiseItemTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvCostCenterNDineTimeWiseItemTransaction](
	[CostCenterWiseTransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[TransactionDate] [date] NOT NULL,
	[DineTimeFrom] [time](3) NOT NULL,
	[DineTimeTo] [time](3) NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[CostCenter] [nvarchar](250) NOT NULL,
	[LocationId] [int] NOT NULL,
	[TotalSalesQuantity] [decimal](18, 5) NOT NULL,
	[TotalSales] [money] NOT NULL,
	[DiscountAmount] [money] NOT NULL,
	[Netsales] [money] NOT NULL,
	[ServiceCharge] [money] NOT NULL,
	[VatAmount] [money] NOT NULL,
	[GrandTotal] [money] NOT NULL,
	[TotalPax] [int] NOT NULL,
	[TotalRevenue] [money] NOT NULL,
	[TotalVoid] [int] NOT NULL,
	[TotalVoidAmount] [money] NOT NULL,
	[ErrorCorrects] [smallint] NOT NULL,
	[ErrorCorrectsAmount] [money] NOT NULL,
	[Checks] [smallint] NOT NULL,
	[ChecksAmount] [money] NOT NULL,
	[ChecksPaid] [smallint] NOT NULL,
	[ChecksPaidAmount] [money] NOT NULL,
	[Outstanding] [smallint] NOT NULL,
	[OutstandingAmount] [money] NOT NULL,
	[TotalCashPayment] [money] NOT NULL,
	[TotalCardPayment] [money] NOT NULL,
	[TotalRefund] [money] NOT NULL,
 CONSTRAINT [PK_InvCostCenterNDineTimeWiseItemTransaction] PRIMARY KEY CLUSTERED 
(
	[CostCenterWiseTransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvCogsClosing]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvCogsClosing]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvCogsClosing](
	[CogsClosingId] [bigint] IDENTITY(1,1) NOT NULL,
	[CogsClosingDate] [date] NOT NULL,
	[LocationId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[NodeId] [int] NOT NULL,
	[CogsAmount] [money] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvCogsClosing] PRIMARY KEY CLUSTERED 
(
	[CogsClosingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvCogsAccountVsItemCategoryMappping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvCogsAccountVsItemCategoryMappping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvCogsAccountVsItemCategoryMappping](
	[CogsAccountMapId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvCogsAccountVsItemCategoryMappping] PRIMARY KEY CLUSTERED 
(
	[CogsAccountMapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvCategoryCostCenterMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvCategoryCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvCategoryCostCenterMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_InvCategoryCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvCategory]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[AncestorId] [int] NULL,
	[Lvl] [int] NOT NULL,
	[Hierarchy] [varchar](900) NULL,
	[HierarchyIndex] [varchar](900) NULL,
	[Name] [varchar](50) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[ServiceType] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[ShowInInvoice] [bit] NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelTaskAssignmentToEmployee]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelTaskAssignmentToEmployee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelTaskAssignmentToEmployee](
	[EmpTaskId] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskId] [bigint] NOT NULL,
	[EmpId] [int] NOT NULL,
 CONSTRAINT [PK_HotelTaskAssignmentToEmployee] PRIMARY KEY CLUSTERED 
(
	[EmpTaskId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelTaskAssignmentRoomWise]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelTaskAssignmentRoomWise]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelTaskAssignmentRoomWise](
	[RoomTaskId] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskId] [bigint] NOT NULL,
	[RoomId] [int] NOT NULL,
	[TaskDetails] [nvarchar](500) NULL,
	[TaskStatus] [nvarchar](20) NULL,
	[OldHKRoomStatusId] [bigint] NULL,
	[OldHKRoomStatus] [nvarchar](100) NULL,
	[HKRoomStatusId] [bigint] NULL,
	[FeedbackTime] [datetime] NULL,
	[Feedbacks] [nvarchar](500) NULL,
	[InTime] [datetime] NULL,
	[OutTime] [datetime] NULL,
	[EmpId] [int] NULL,
 CONSTRAINT [PK_HotelTaskAssignmentRoomWise] PRIMARY KEY CLUSTERED 
(
	[RoomTaskId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelStock]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelStock]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelStock](
	[StockId] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NULL,
	[Units] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HMStock_1] PRIMARY KEY CLUSTERED 
(
	[StockId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelServiceBillTransfered]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelServiceBillTransfered]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelServiceBillTransfered](
	[TransferedId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](50) NULL,
	[TransferedDate] [datetime] NULL,
	[FromRegistrationId] [int] NULL,
	[ToRegistrationId] [int] NULL,
	[ServiceBillId] [int] NULL,
	[Remarks] [nvarchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_HotelServiceBillTransfered] PRIMARY KEY CLUSTERED 
(
	[TransferedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelSegmentRateChart]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelSegmentRateChart]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelSegmentRateChart](
	[RateChartId] [int] IDENTITY(1,1) NOT NULL,
	[SegmentId] [int] NULL,
	[RoomTypeId] [int] NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[RoomRateUSD] [decimal](18, 2) NULL,
	[EffectiveFromDate] [datetime] NULL,
	[EffectiveToDate] [datetime] NULL,
 CONSTRAINT [PK_HotelSegmentRateChart] PRIMARY KEY CLUSTERED 
(
	[RateChartId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelSegmentHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelSegmentHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelSegmentHead](
	[SegmentId] [int] IDENTITY(1,1) NOT NULL,
	[SegmentName] [varchar](200) NULL,
	[SegmentType] [varchar](50) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelSegmentHead] PRIMARY KEY CLUSTERED 
(
	[SegmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelSalesSummary]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelSalesSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelSalesSummary](
	[SummaryId] [bigint] IDENTITY(1,1) NOT NULL,
	[SummaryDate] [datetime] NULL,
	[ServiceType] [nvarchar](200) NULL,
	[ServiceName] [nvarchar](200) NULL,
	[Covers] [int] NULL,
	[TotalSales] [decimal](18, 2) NULL,
	[TotalVat] [decimal](18, 2) NULL,
	[TotalServiceCharge] [decimal](18, 2) NULL,
	[TotalCitySDCharge] [decimal](18, 2) NULL,
	[TotalAdditionalCharge] [decimal](18, 2) NULL,
	[TotalRoomSale] [decimal](18, 2) NULL,
	[TotalRoomVat] [decimal](18, 2) NULL,
	[TotalRoomServiceCharge] [decimal](18, 2) NULL,
	[TotalRoomCitySDCharge] [decimal](18, 2) NULL,
	[TotalRoomAdditionalCharge] [decimal](18, 2) NULL,
	[RoomOccupied] [decimal](18, 2) NULL,
	[OccupencyPercent] [decimal](18, 2) NULL,
	[DoubleOccupency] [decimal](18, 2) NULL,
	[NoOfGuest] [decimal](18, 2) NULL,
 CONSTRAINT [PK_HotelSalesSummary] PRIMARY KEY CLUSTERED 
(
	[SummaryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelSalesSummary' AND column_name = 'PaxQuantity')
BEGIN
	ALTER TABLE dbo.HotelSalesSummary
		ADD PaxQuantity INT NOT NULL CONSTRAINT DF_HotelSalesSummary_PaxQuantity DEFAULT 1 WITH VALUES
END
GO
/****** Object:  Table [dbo].[DashboardManagement]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DashboardManagement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DashboardManagement](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[Panel] [int] NULL,
	[DivName] [varchar](50) NULL,
 CONSTRAINT [PK_DashboardManagement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DashboardItem]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DashboardItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DashboardItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NULL,
	[ItemName] [nvarchar](200) NULL,
 CONSTRAINT [PK_DashboardItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CommonSetup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonSetup](
	[SetupId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](100) NULL,
	[SetupName] [varchar](200) NULL,
	[SetupValue] [text] NULL,
	[Description] [text] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonSetup] PRIMARY KEY CLUSTERED 
(
	[SetupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonProfession]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonProfession]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonProfession](
	[ProfessionId] [int] IDENTITY(1,1) NOT NULL,
	[ProfessionName] [varchar](200) NULL,
	[ProfessionCode] [varchar](20) NULL,
 CONSTRAINT [PK_CommonProfession] PRIMARY KEY CLUSTERED 
(
	[ProfessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonPrinterInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonPrinterInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonPrinterInfo](
	[PrinterInfoId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[StockType] [varchar](100) NULL,
	[KitchenId] [int] NULL,
	[PrinterName] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantItemType] PRIMARY KEY CLUSTERED 
(
	[PrinterInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonPaymentMode]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonPaymentMode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonPaymentMode](
	[PaymentModeId] [int] IDENTITY(1,1) NOT NULL,
	[AncestorId] [int] NULL,
	[PaymentMode] [nvarchar](100) NULL,
	[DisplayName] [nvarchar](200) NOT NULL,
	[PaymentCode] [nvarchar](5) NULL,
	[Hierarchy] [varchar](900) NULL,
	[Lvl] [int] NOT NULL,
	[HierarchyIndex] [varchar](900) NULL,
	[PaymentAccountsPostingId] [int] NULL,
	[ReceiveAccountsPostingId] [int] NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_CommonPaymentMode] PRIMARY KEY CLUSTERED 
(
	[PaymentModeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonModuleType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonModuleType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonModuleType](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleType] [nvarchar](300) NULL,
 CONSTRAINT [PK_CommonModuleType] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CommonModuleName]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonModuleName]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonModuleName](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NULL,
	[ModuleName] [varchar](300) NULL,
	[GroupName] [varchar](300) NULL,
	[ModulePath] [varchar](300) NULL,
	[IsReportType] [bit] NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_ModuleName] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonMessageDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonMessageDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonMessageDetails](
	[MessageDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[MessageId] [bigint] NOT NULL,
	[MessageTo] [int] NOT NULL,
	[UserId] [varchar](50) NULL,
	[IsReaden] [bit] NULL,
 CONSTRAINT [PK_CommonMessageDetails] PRIMARY KEY CLUSTERED 
(
	[MessageDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonMessage]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonMessage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonMessage](
	[MessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[MessageFrom] [int] NOT NULL,
	[MessageDate] [datetime] NOT NULL,
	[Importance] [varchar](25) NULL,
	[Subjects] [varchar](350) NULL,
	[MessageBody] [varchar](1000) NULL,
 CONSTRAINT [PK_CommonMessage] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonLocation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonLocation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonLocation](
	[LocationId] [int] IDENTITY(1,1) NOT NULL,
	[CityId] [int] NULL,
	[LocationName] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonLocation] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonLocation' AND column_name = 'CountryId')
BEGIN
	ALTER TABLE dbo.CommonLocation
		ADD CountryId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonLocation' AND column_name = 'StateId')
BEGIN
	ALTER TABLE dbo.CommonLocation
		ADD StateId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonLocation' AND column_name = 'CityId')
BEGIN
	ALTER TABLE dbo.CommonLocation
		ADD CityId BIGINT NULL;
END
GO
/****** Object:  Table [dbo].[CommonIndustry]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonIndustry]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonIndustry](
	[IndustryId] [int] IDENTITY(1,1) NOT NULL,
	[IndustryName] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonIndustry] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonDocuments]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonDocuments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonDocuments](
	[DocumentId] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnerId] [bigint] NULL,
	[DocumentCategory] [varchar](100) NULL,
	[DocumentType] [varchar](100) NULL,
	[Extention] [varchar](50) NULL,
	[Name] [varchar](100) NULL,
	[Path] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonCustomFieldData]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCustomFieldData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCustomFieldData](
	[FieldId] [int] IDENTITY(1,1) NOT NULL,
	[FieldType] [varchar](max) NULL,
	[FieldValue] [varchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_CommonCustomFieldData] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonCurrencyTransaction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCurrencyTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCurrencyTransaction](
	[CurrencyConversionId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionNumber] [varchar](20) NULL,
	[FromConversionHeadId] [int] NULL,
	[ToConversionHeadId] [int] NULL,
	[ConversionAmount] [decimal](18, 2) NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[ConvertedAmount] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonCurrencyConversion] PRIMARY KEY CLUSTERED 
(
	[CurrencyConversionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonCurrencyConversion]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCurrencyConversion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCurrencyConversion](
	[ConversionId] [int] IDENTITY(1,1) NOT NULL,
	[FromCurrencyId] [int] NULL,
	[ToCurrencyId] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_CommonCurrencyConversionHead] PRIMARY KEY CLUSTERED 
(
	[ConversionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CommonCurrency]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCurrency]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCurrency](
	[CurrencyId] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyName] [varchar](100) NULL,
	[CurrencyType] [varchar](100) NULL,
	[OrderByIndex] [int] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_CommonConversionHead] PRIMARY KEY CLUSTERED 
(
	[CurrencyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonCountries]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCountries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCountries](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NULL,
	[Nationality] [nvarchar](50) NULL,
	[Code2Digit] [char](2) NOT NULL,
	[Code3Digit] [char](3) NULL,
	[CodeNumeric] [char](3) NULL,
	[SBCode] [varchar](50) NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonCostCenter]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCostCenter](
	[CostCenterId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[AccountsPostingHeadId] [int] NULL,
	[CostCenter] [varchar](256) NOT NULL,
	[CostCenterLogo] [varchar](256) NULL,
	[BillNumberPrefix] [varchar](2) NULL,
	[IsServiceChargeEnable] [bit] NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[IsCitySDChargeEnable] [bit] NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[IsVatEnable] [bit] NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[IsVatSChargeInclusive] [int] NULL,
	[IsRatePlusPlus] [int] NULL,
	[IsAdditionalChargeEnable] [bit] NULL,
	[AdditionalChargeType] [varchar](50) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[IsVatOnSDCharge] [bit] NULL,
	[CostCenterType] [varchar](50) NULL,
	[IsRestaurant] [bit] NULL,
	[ReadingNumber] [decimal](18, 2) NULL,
	[OutletType] [int] NULL,
	[DefaultView] [varchar](50) NULL,
	[DefaultStockLocationId] [int] NULL,
	[IsDefaultCostCenter] [bit] NULL,
	[InvoiceTemplate] [int] NULL,
	[BillingStartTime] [int] NULL,
	[IsDiscountApplicableOnRackRate] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonCostCentreTab] PRIMARY KEY CLUSTERED 
(
	[CostCenterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO
-- add column IsDepartureChargable
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'PayrollDeptId')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD PayrollDeptId INT NULL DEFAULT 0 WITH VALUES
END
SET ANSI_PADDING OFF
GO
-- add column GLCompanyId
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'GLCompanyId')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD GLCompanyId INT NULL
END
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'IsCostCenterNameShowOnInvoice')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD IsCostCenterNameShowOnInvoice BIT NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'IsCustomerDetailsEnable')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD IsCustomerDetailsEnable BIT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCostCenter' AND column_name = 'IsDeliveredByEnable')
BEGIN
	ALTER TABLE dbo.CommonCostCenter
		ADD IsDeliveredByEnable BIT NULL
END
/****** Object:  Table [dbo].[CommonCompanyProfile]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCompanyProfile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCompanyProfile](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](20) NULL,
	[CompanyName] [varchar](300) NOT NULL,
	[CompanyAddress] [varchar](max) NOT NULL,
	[EmailAddress] [varchar](300) NULL,
	[WebAddress] [varchar](300) NULL,
	[ContactNumber] [varchar](300) NOT NULL,
	[ContactPerson] [varchar](300) NULL,
	[VatRegistrationNo] [varchar](50) NULL,
	[Remarks] [varchar](max) NULL,
	[ImageName] [varchar](max) NULL,
	[ImagePath] [varchar](max) NULL,
	[CompanyType] [varchar](50) NULL,
 CONSTRAINT [PK_CompanyProfile] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
-- add column TinNumber
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCompanyProfile' AND column_name = 'TinNumber')
BEGIN
	ALTER TABLE dbo.CommonCompanyProfile
		ADD TinNumber NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
-- add column GroupCompanyName
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCompanyProfile' AND column_name = 'GroupCompanyName')
BEGIN
	ALTER TABLE dbo.CommonCompanyProfile
		ADD GroupCompanyName NVARCHAR(MAX) NULL
END
SET ANSI_PADDING OFF
GO
-- add column Telephone
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCompanyProfile' AND column_name = 'Telephone')
BEGIN
	ALTER TABLE dbo.CommonCompanyProfile
		ADD Telephone NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
-- add column HotLineNumber
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCompanyProfile' AND column_name = 'HotLineNumber')
BEGIN
	ALTER TABLE dbo.CommonCompanyProfile
		ADD HotLineNumber NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
-- add column Fax
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCompanyProfile' AND column_name = 'Fax')
BEGIN
	ALTER TABLE dbo.CommonCompanyProfile
		ADD Fax NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonCompanyBank]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCompanyBank]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCompanyBank](
	[BankId] [int] IDENTITY(1,1) NOT NULL,
	[BankName] [varchar](150) NULL,
	[BranchName] [varchar](150) NULL,
	[SwiftCode] [varchar](20) NULL,
	[AccountName] [varchar](100) NULL,
	[AccountNo1] [varchar](20) NULL,
	[AccountNo2] [varchar](20) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CompanyBank] PRIMARY KEY CLUSTERED 
(
	[BankId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonCity]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonCity](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[CityName] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonCity] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCity' AND column_name = 'CountryId')
BEGIN
	ALTER TABLE dbo.CommonCity
		ADD CountryId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonCity' AND column_name = 'StateId')
BEGIN
	ALTER TABLE dbo.CommonCity
		ADD StateId BIGINT NULL;
END
GO
/****** Object:  Table [dbo].[CommonBusinessPromotionDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonBusinessPromotionDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonBusinessPromotionDetail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPromotionId] [int] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransactionId] [int] NULL,
 CONSTRAINT [PK_CommonBusinessPromotionDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonBusinessPromotion]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonBusinessPromotion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonBusinessPromotion](
	[BusinessPromotionId] [int] IDENTITY(1,1) NOT NULL,
	[BPHead] [varchar](250) NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[TransactionType] [varchar](50) NULL,
	[IsBPPublic] [bit] NULL,
	[PercentAmount] [decimal](18, 0) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_BusinessPromotion] PRIMARY KEY CLUSTERED 
(
	[BusinessPromotionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommonBank]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonBank]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonBank](
	[BankId] [int] IDENTITY(1,1) NOT NULL,
	[BankName] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Bank] PRIMARY KEY CLUSTERED 
(
	[BankId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CgsTransactionHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CgsTransactionHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CgsTransactionHead](
	[TransactionHeadId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Type] [nvarchar](100) NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [varchar](20) NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [varchar](20) NULL,
 CONSTRAINT [PK_CgsTransactionHead] PRIMARY KEY CLUSTERED 
(
	[TransactionHeadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CgsMonthlyTransaction]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CgsMonthlyTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CgsMonthlyTransaction](
	[MonthlyTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NULL,
	[EmpType] [nvarchar](100) NULL,
	[InputDate] [datetime] NULL,
	[Amount] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [varchar](20) NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [varchar](20) NULL,
 CONSTRAINT [PK_InvMonthlyTransaction] PRIMARY KEY CLUSTERED 
(
	[MonthlyTransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BanquetSeatingPlan]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetSeatingPlan]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetSeatingPlan](
	[SeatingId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Code] [varchar](20) NULL,
	[ImageName] [varchar](500) NULL,
	[Description] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_BanquetSeatingPlan] PRIMARY KEY CLUSTERED 
(
	[SeatingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/*=================================================================== 
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  17-07-2018		FA		Data type and column name change of SeatingId, CreatedBy, LastModifiedBy
  ===================================================================*/

  -- Change SeatingId to Id and type int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetSeatingPlan' AND column_name = 'SeatingId' AND DATA_TYPE = 'int')
BEGIN
	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetSeatingPlan]') AND name = N'PK_BanquetSeatingPlan')
	ALTER TABLE [dbo].[BanquetSeatingPlan] DROP CONSTRAINT [PK_BanquetSeatingPlan]
	
	ALTER TABLE dbo.BanquetSeatingPlan
	   ALTER COLUMN SeatingId bigint NOT NULL;
 
	ALTER TABLE [dbo].[BanquetSeatingPlan] ADD  CONSTRAINT [PK_BanquetSeatingPlan] PRIMARY KEY CLUSTERED 
	(
		[SeatingId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	 
	 EXEC sp_rename 'BanquetSeatingPlan.SeatingId', 'Id', 'COLUMN';    
END
-- Change CreatedBy data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetSeatingPlan' AND column_name = 'CreatedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetSeatingPlan
	   ALTER COLUMN CreatedBy bigint NULL;
END

-- Change LastModifiedBy data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetSeatingPlan' AND column_name = 'LastModifiedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetSeatingPlan
	   ALTER COLUMN LastModifiedBy bigint NULL;
END

/****** Object:  Table [dbo].[BanquetRequisites]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetRequisites]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetRequisites](
	[RequisitesId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Code] [varchar](20) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[Description] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_BanquetRequisites] PRIMARY KEY CLUSTERED 
(
	[RequisitesId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRequisites' AND column_name = 'AccountsPostingHeadId')
BEGIN
	ALTER TABLE dbo.BanquetRequisites
		ADD AccountsPostingHeadId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRequisites' AND column_name = 'ExpenseAccountsPostingHeadId')
BEGIN
	ALTER TABLE dbo.BanquetRequisites
		ADD ExpenseAccountsPostingHeadId BIGINT NULL;
END
GO
/*=================================================================== 
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  17-07-2018		FA		Data type and column name change of RequisitesId, CreatedBy, LastModifiedBy
  ===================================================================*/
  -- Change RequisitesId to Id and type int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRequisites' AND column_name = 'RequisitesId' AND DATA_TYPE = 'int')
BEGIN
	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetRequisites]') AND name = N'PK_BanquetRequisites')
	ALTER TABLE [dbo].[BanquetRequisites] DROP CONSTRAINT [PK_BanquetRequisites]
	
	ALTER TABLE dbo.BanquetRequisites
	   ALTER COLUMN RequisitesId bigint NOT NULL;
 
	ALTER TABLE [dbo].[BanquetRequisites] ADD  CONSTRAINT [PK_BanquetRequisites] PRIMARY KEY CLUSTERED 
	(
		[RequisitesId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	 
	 EXEC sp_rename 'BanquetRequisites.RequisitesId', 'Id', 'COLUMN';    
END

-- Change CreatedByDate to CreatedDate
 IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRequisites' AND column_name = 'CreatedByDate')
BEGIN
ALTER TABLE dbo.BanquetRequisites
  ALTER COLUMN CreatedByDate datetime NULL;
   
EXEC sp_rename 'BanquetRequisites.CreatedByDate', 'CreatedDate', 'COLUMN';     
END

-- Change LastModifiedByDate to LastModifiedDate
 IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRequisites' AND column_name = 'LastModifiedByDate')
BEGIN
ALTER TABLE dbo.BanquetRequisites
  ALTER COLUMN LastModifiedByDate datetime NULL;
   
EXEC sp_rename 'BanquetRequisites.LastModifiedByDate', 'LastModifiedDate', 'COLUMN';     
END

-- Change CreatedBy data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRequisites' AND column_name = 'CreatedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetRequisites
	   ALTER COLUMN CreatedBy bigint NULL;
END

-- Change LastModifiedBy data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRequisites' AND column_name = 'LastModifiedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetRequisites
	   ALTER COLUMN LastModifiedBy bigint NULL;
END

-- Foreign key integration of BanquetRequisites with GLNodeMatrix
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetRequisites_GLNodeMatrix')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetRequisites'))
BEGIN
ALTER TABLE [dbo].[BanquetRequisites]  WITH CHECK ADD  CONSTRAINT [FK_BanquetRequisites_GLNodeMatrix] FOREIGN KEY([AccountsPostingHeadId])
REFERENCES [dbo].[GLNodeMatrix] ([NodeId])

ALTER TABLE [dbo].[BanquetRequisites] CHECK CONSTRAINT [FK_BanquetRequisites_GLNodeMatrix]

END
GO

/****** Object:  Table [dbo].[BanquetRefference]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetRefference]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetRefference](
	[RefferenceId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[SalesCommission] [decimal](18, 2) NULL,
	[Description] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_BanquetRefference] PRIMARY KEY CLUSTERED 
(
	[RefferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

-- Primary key RefferenceId type changed from INT to BIGINT and rename to Id

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRefference' AND column_name = 'RefferenceId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetRefference]') AND name = N'PK_BanquetRefference')
 ALTER TABLE [dbo].[BanquetRefference] DROP CONSTRAINT [PK_BanquetRefference]
 
 ALTER TABLE dbo.BanquetRefference
    ALTER COLUMN RefferenceId bigint NOT NULL;
 
 ALTER TABLE [dbo].[BanquetRefference] ADD  CONSTRAINT [PK_BanquetRefference] PRIMARY KEY CLUSTERED 
 (
  [RefferenceId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'BanquetRefference.RefferenceId' , 'Id', 'COLUMN'
END


--- CreatedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRefference' AND column_name = 'CreatedBy')
BEGIN
ALTER TABLE dbo.BanquetRefference
   ALTER COLUMN CreatedBy BIGINT NULL; 
END 
GO

--- CreatedByDate column renamed to CreatedDate
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRefference' AND column_name = 'CreatedByDate')
	EXEC sp_RENAME 'BanquetRefference.CreatedByDate' , 'CreatedDate', 'COLUMN'
GO
--- LastModifiedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRefference' AND column_name = 'LastModifiedBy')
BEGIN
ALTER TABLE dbo.BanquetRefference
   ALTER COLUMN LastModifiedBy BIGINT NULL; 
END 
GO

--- CreatedByDate column renamed to CreatedDate
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetRefference' AND column_name = 'LastModifiedByDate')
	EXEC sp_RENAME 'BanquetRefference.LastModifiedByDate' , 'LastModifiedDate', 'COLUMN'
GO


GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BanquetOccessionType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetOccessionType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetOccessionType](
	[OccessionTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[Description] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_BanquetOccessionType] PRIMARY KEY CLUSTERED 
(
	[OccessionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Primary key OccessionTypeId type changed from INT to BIGINT and rename to Id

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetOccessionType' AND column_name = 'OccessionTypeId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetOccessionType]') AND name = N'PK_BanquetOccessionType')
 ALTER TABLE [dbo].[BanquetOccessionType] DROP CONSTRAINT [PK_BanquetOccessionType]
 
 ALTER TABLE dbo.BanquetOccessionType
    ALTER COLUMN OccessionTypeId bigint NOT NULL;
 
 ALTER TABLE [dbo].[BanquetOccessionType] ADD  CONSTRAINT [PK_BanquetOccessionType] PRIMARY KEY CLUSTERED 
 (
  [OccessionTypeId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'BanquetOccessionType.OccessionTypeId' , 'Id', 'COLUMN'
END


--- CreatedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetOccessionType' AND column_name = 'CreatedBy')
BEGIN
ALTER TABLE dbo.BanquetOccessionType
   ALTER COLUMN CreatedBy BIGINT NULL; 
END 
GO

--- CreatedByDate column renamed to CreatedDate
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetOccessionType' AND column_name = 'CreatedByDate')
	EXEC sp_RENAME 'BanquetOccessionType.CreatedByDate' , 'CreatedDate', 'COLUMN'
GO
--- LastModifiedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetOccessionType' AND column_name = 'LastModifiedBy')
BEGIN
ALTER TABLE dbo.BanquetOccessionType
   ALTER COLUMN LastModifiedBy BIGINT NULL; 
END 
GO

--- CreatedByDate column renamed to CreatedDate
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetOccessionType' AND column_name = 'LastModifiedByDate')
	EXEC sp_RENAME 'BanquetOccessionType.LastModifiedByDate' , 'LastModifiedDate', 'COLUMN'
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BanquetInformation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetInformation](
	[BanquetId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Capacity] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[Description] [varchar](500) NULL,
	[CostCenterId] [int] NULL,
	[Status] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_BanquetInformation] PRIMARY KEY CLUSTERED 
(
	[BanquetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetInformation' AND column_name = 'AccountsPostingHeadId')
BEGIN
	ALTER TABLE dbo.BanquetInformation
		ADD AccountsPostingHeadId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetInformation' AND column_name = 'ExpenseAccountsPostingHeadId')
BEGIN
	ALTER TABLE dbo.BanquetInformation
		ADD ExpenseAccountsPostingHeadId BIGINT NULL
END
GO 
/*=================================================================== 
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  17-07-2018		FA		Data type and column name change of BanquetId, CreatedBy, LastModifiedBy
  ===================================================================*/
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetInformation' AND column_name = 'BanquetId' AND DATA_TYPE = 'int')
BEGIN
	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetInformation]') AND name = N'PK_BanquetInformation')
	ALTER TABLE [dbo].[BanquetInformation] DROP CONSTRAINT [PK_BanquetInformation]
	
	ALTER TABLE dbo.BanquetInformation
	   ALTER COLUMN BanquetId bigint NOT NULL;
 
	ALTER TABLE [dbo].[BanquetInformation] ADD  CONSTRAINT [PK_BanquetInformation] PRIMARY KEY CLUSTERED 
	(
		[BanquetId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	 
	 EXEC sp_rename 'BanquetInformation.BanquetId', 'Id', 'COLUMN';    
END

 IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetInformation' AND column_name = 'CreatedByDate')
BEGIN
ALTER TABLE dbo.BanquetInformation
  ALTER COLUMN CreatedByDate datetime NULL;
   
EXEC sp_rename 'BanquetInformation.CreatedByDate', 'CreatedDate', 'COLUMN';     
END

 IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetInformation' AND column_name = 'LastModifiedByDate')
BEGIN
ALTER TABLE dbo.BanquetInformation
  ALTER COLUMN LastModifiedByDate datetime NULL;
   
EXEC sp_rename 'BanquetInformation.LastModifiedByDate', 'LastModifiedDate', 'COLUMN';     
END

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetInformation' AND column_name = 'CreatedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetInformation
	   ALTER COLUMN CreatedBy bigint NULL;
END

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetInformation' AND column_name = 'LastModifiedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetInformation
	   ALTER COLUMN LastModifiedBy bigint NULL;
END

--Forign Key relation of BanquetInformation with GLNodeMatrix
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetInformation_GLNodeMatrix')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetInformation'))
BEGIN
ALTER TABLE [dbo].[BanquetInformation]  WITH CHECK ADD  CONSTRAINT [FK_BanquetInformation_GLNodeMatrix] 
FOREIGN KEY([AccountsPostingHeadId])
REFERENCES [dbo].[GLNodeMatrix] ([NodeId])

ALTER TABLE [dbo].[BanquetInformation] CHECK CONSTRAINT [FK_BanquetInformation_GLNodeMatrix]
END
GO

/****** Object:  Table [dbo].[BanquetReservation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetReservation](
	[ReservationId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationNumber] [varchar](50) NULL,
	[ReservationMode] [varchar](50) NULL,
	[BanquetId] [int] NULL,
	[CostCenterId] [int] NULL,
	[IsListedCompany] [bit] NULL,
	[CompanyId] [int] NULL,
	[Name] [varchar](100) NULL,
	[Address] [varchar](500) NULL,
	[CityName] [varchar](100) NULL,
	[ZipCode] [varchar](20) NULL,
	[CountryId] [int] NULL,
	[PhoneNumber] [varchar](50) NULL,
	[EmailAddress] [varchar](100) NULL,
	[BookingFor] [varchar](50) NULL,
	[ContactPerson] [varchar](100) NULL,
	[ContactEmail] [varchar](50) NULL,
	[ContactPhone] [varchar](50) NULL,
	[ArriveDate] [datetime] NULL,
	[DepartureDate] [datetime] NULL,
	[OccessionTypeId] [int] NULL,
	[SeatingId] [int] NULL,
	[BanquetRate] [decimal](18, 2) NULL,
	[BanquetDiscount] [decimal](18, 2) NULL,
	[BanquetDiscountedAmount] [decimal](18, 2) NULL,
	[BanquetRackRate] [decimal](18, 2) NULL,
	[BanquetServiceCharge] [decimal](18, 2) NULL,
	[BanquetCitySDCharge] [decimal](18, 2) NULL,
	[BanquetVatAmount] [decimal](18, 2) NULL,
	[BanquetAdditionalCharge] [decimal](18, 2) NULL,
	[InvoiceServiceRate] [decimal](18, 2) NULL,
	[IsInvoiceServiceChargeEnable] [bit] NULL,
	[InvoiceServiceCharge] [decimal](18, 0) NULL,
	[IsInvoiceCitySDChargeEnable] [bit] NULL,
	[InvoiceCitySDCharge] [decimal](18, 2) NULL,
	[IsInvoiceVatAmountEnable] [bit] NULL,
	[InvoiceVatAmount] [decimal](18, 2) NULL,
	[IsInvoiceAdditionalChargeEnable] [bit] NULL,
	[AdditionalChargeType] [varchar](25) NULL,
	[InvoiceAdditionalCharge] [decimal](18, 2) NULL,
	[NumberOfPersonAdult] [int] NULL,
	[NumberOfPersonChild] [int] NULL,
	[CancellationReason] [varchar](max) NULL,
	[SpecialInstructions] [varchar](max) NULL,
	[RefferenceId] [int] NULL,
	[IsReturnedClient] [bit] NULL,
	[Comments] [varchar](max) NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[DiscountType] [varchar](20) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[CalculatedDiscountAmount] [decimal](18, 2) NULL,
	[DiscountedAmount] [decimal](18, 2) NULL,
	[ServiceRate] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[GrandTotal] [decimal](18, 2) NULL,
	[RoundedAmount] [decimal](18, 2) NULL,
	[RoundedGrandTotal] [decimal](18, 2) NULL,
	[RebateRemarks] [varchar](500) NULL,
	[IsBillSettlement] [bit] NULL,
	[RegistrationId] [int] NULL,
	[ActiveStatus] [int] NULL,
	[BillStatus] [varchar](50) NULL,
	[BillVoidBy] [int] NULL,
	[CurrencyExchangeRate] [decimal](18, 2) NULL,
	[Remarks] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_BanquetReservation] PRIMARY KEY CLUSTERED 
(
	[ReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

-- add column SettlementDate
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'SettlementDate')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD SettlementDate DATETIME NULL
END
GO 

-- Add Null value in RefferenceId-------
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'RefferenceId')
BEGIN
update BanquetReservation
set RefferenceId = NULL
where RefferenceId = 0
END

-- add column ReservationDiscountType
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'ReservationDiscountType')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD ReservationDiscountType VARCHAR(20) NULL
END
GO 

-- add column ReservationDiscountAmount
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'ReservationDiscountAmount')
BEGIN
	ALTER TABLE dbo.BanquetReservation
	  ADD ReservationDiscountAmount DECIMAL(18, 2) NOT NULL CONSTRAINT DF_BanquetReservation_ReservationDiscountAmount DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'GLCompanyId')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD GLCompanyId INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'GLProjectId')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD GLProjectId INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'EventType')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD EventType NVARCHAR(20) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'EventTitle')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD EventTitle NVARCHAR(200) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'MeetingAgenda')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD MeetingAgenda NVARCHAR(MAX) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'MeetingDiscussion')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD MeetingDiscussion NVARCHAR(MAX) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'CallToAction')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD CallToAction NVARCHAR(MAX) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'IsUnderCompany')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD IsUnderCompany NVARCHAR(MAX) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationDetail' AND column_name = 'ItemDescription')
BEGIN
	ALTER TABLE dbo.BanquetReservationDetail
		ADD ItemDescription NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationDetail' AND column_name = 'ItemArrivalTime')
BEGIN
	ALTER TABLE dbo.BanquetReservationDetail
		ADD ItemArrivalTime DATETIME NULL
END
GO

-- Primary key ReservationId type changed from INT to BIGINT and rename to Id

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'ReservationId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservation]') AND name = N'PK_BanquetReservation')
 ALTER TABLE [dbo].[BanquetReservation] DROP CONSTRAINT [PK_BanquetReservation]
 
 ALTER TABLE dbo.BanquetReservation
    ALTER COLUMN ReservationId bigint NOT NULL;
 
 ALTER TABLE [dbo].[BanquetReservation] ADD  CONSTRAINT [PK_BanquetReservation] PRIMARY KEY CLUSTERED 
 (
  [ReservationId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'BanquetReservation.ReservationId' , 'Id', 'COLUMN'
END

--- BanquetId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'BanquetId')
BEGIN
ALTER TABLE dbo.BanquetReservation
   ALTER COLUMN BanquetId BIGINT NULL; 
END 
GO

--- OccessionTypeId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'OccessionTypeId')
BEGIN
ALTER TABLE dbo.BanquetReservation
   ALTER COLUMN OccessionTypeId BIGINT NULL; 
END 
GO

--- SeatingId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'SeatingId')
BEGIN
ALTER TABLE dbo.BanquetReservation
   ALTER COLUMN SeatingId BIGINT NULL; 
END 
GO

--- SeatingId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'RefferenceId')
BEGIN
ALTER TABLE dbo.BanquetReservation
   ALTER COLUMN RefferenceId BIGINT NULL; 
END 
GO


--- CreatedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'CreatedBy')
BEGIN
ALTER TABLE dbo.BanquetReservation
   ALTER COLUMN CreatedBy BIGINT NULL; 
END 
GO

 
--- CreatedByDate column renamed to CreatedDate
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'CreatedByDate')
	EXEC sp_RENAME 'BanquetReservation.CreatedByDate' , 'CreatedDate', 'COLUMN'
GO

--- LastModifiedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'LastModifiedBy')
BEGIN
ALTER TABLE dbo.BanquetReservation
   ALTER COLUMN LastModifiedBy BIGINT NULL; 
END

--- CreatedByDate column renamed to CreatedDate
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'LastModifiedByDate')
	EXEC sp_RENAME 'BanquetReservation.LastModifiedByDate' , 'LastModifiedDate', 'COLUMN'
GO
SET ANSI_PADDING OFF
GO

-- Foreign Key relation -------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservation_BanquetInformation')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservation'))
BEGIN

ALTER TABLE [dbo].[BanquetReservation]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservation_BanquetInformation] 
FOREIGN KEY([BanquetId])
REFERENCES [dbo].[BanquetInformation] ([Id])

ALTER TABLE [dbo].[BanquetReservation] CHECK CONSTRAINT [FK_BanquetReservation_BanquetInformation]
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservation_BanquetOccessionType')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservation'))
BEGIN
ALTER TABLE [dbo].[BanquetReservation]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservation_BanquetOccessionType] 
FOREIGN KEY([OccessionTypeId])
REFERENCES [dbo].[BanquetOccessionType] ([Id])

ALTER TABLE [dbo].[BanquetReservation] CHECK CONSTRAINT [FK_BanquetReservation_BanquetOccessionType]
END

--IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservation_BanquetRefference')
--   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservation'))
--BEGIN
--ALTER TABLE [dbo].[BanquetReservation]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservation_BanquetRefference] 
--FOREIGN KEY([RefferenceId])
--REFERENCES [dbo].[BanquetRefference] ([Id])

--ALTER TABLE [dbo].[BanquetReservation] CHECK CONSTRAINT [FK_BanquetReservation_BanquetRefference]
--END

--IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservation_BanquetSeatingPlan')
--   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservation'))
--BEGIN
--ALTER TABLE [dbo].[BanquetReservation]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservation_BanquetSeatingPlan] 
--FOREIGN KEY([SeatingId])
--REFERENCES [dbo].[BanquetSeatingPlan] ([Id])

--ALTER TABLE [dbo].[BanquetReservation] CHECK CONSTRAINT [FK_BanquetReservation_BanquetSeatingPlan]
--END
--GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservation_BanquetRefference')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservation'))
BEGIN
    ALTER TABLE [dbo].[BanquetReservation] 
    DROP  CONSTRAINT [FK_BanquetReservation_BanquetRefference] 
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservation_BanquetSeatingPlan')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservation'))
BEGIN
    ALTER TABLE [dbo].[BanquetReservation] 
    DROP  CONSTRAINT [FK_BanquetReservation_BanquetSeatingPlan] 
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'MarketSegmentId')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD MarketSegmentId INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'GuestSourceId')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD GuestSourceId INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'BookersName')
BEGIN
	ALTER TABLE dbo.BanquetReservation
		ADD BookersName NVARCHAR(MAX) NULL;
END
GO

---added GuidId column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservation' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.BanquetReservation
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_BanquetReservation_GuidId DEFAULT (newid()) WITH VALUES
END
GO



/****** Object:  Table [dbo].[BanquetReservationDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetReservationDetail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[ItemTypeId] [int] NULL,
	[ItemType] [varchar](50) NULL,
	[ItemId] [int] NULL,
	[ItemName] [varchar](300) NULL,
	[IsComplementary] [bit] NULL,
	[ItemUnit] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[TotalPrice] [decimal](18, 2) NULL,
	[DiscountType] [varchar](20) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[DiscountedAmount] [decimal](18, 2) NULL,
	[ServiceRate] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
 CONSTRAINT [PK_BanquetReservationDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[BanquetReservationOfficeParticipants]    Script Date: 09/08/2021 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationOfficeParticipants]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetReservationOfficeParticipants](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BanquetReservationId] [int] NULL,
	[EmployeeId] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO



-- Primary key DetailId type changed from INT to BIGINT and rename to Id

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationDetail' AND column_name = 'DetailId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationDetail]') AND name = N'PK_BanquetReservationDetail')
 ALTER TABLE [dbo].[BanquetReservationDetail] DROP CONSTRAINT [PK_BanquetReservationDetail]
 
 ALTER TABLE dbo.BanquetReservationDetail
    ALTER COLUMN DetailId bigint NOT NULL;
 
 ALTER TABLE [dbo].[BanquetReservationDetail] ADD  CONSTRAINT [PK_BanquetReservationDetail] PRIMARY KEY CLUSTERED 
 (
  [DetailId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'BanquetReservationDetail.DetailId' , 'Id', 'COLUMN'
END

--- ReservationId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationDetail' AND column_name = 'ReservationId')
BEGIN
ALTER TABLE dbo.BanquetReservationDetail
   ALTER COLUMN ReservationId BIGINT NULL; 
END 
GO

--- ItemTypeId column renamed to CreatedDate
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationDetail' AND column_name = 'ItemTypeId')
	BEGIN
ALTER TABLE dbo.BanquetReservationDetail
   ALTER COLUMN ItemTypeId BIGINT NULL; 
END
GO
--- ItemId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationDetail' AND column_name = 'ItemId')
BEGIN
ALTER TABLE dbo.BanquetReservationDetail
   ALTER COLUMN ItemId BIGINT NULL; 
END 
GO
-- Foreign Key integration -----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservationDetail_BanquetReservation') 
AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservationDetail'))
BEGIN
ALTER TABLE [dbo].[BanquetReservationDetail]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservationDetail_BanquetReservation] FOREIGN KEY([ReservationId])
REFERENCES [dbo].[BanquetReservation] ([Id])

ALTER TABLE [dbo].[BanquetReservationDetail] CHECK CONSTRAINT [FK_BanquetReservationDetail_BanquetReservation]
END
GO

--- SeatingId column type changed varchar(50) to varchar(MAX)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationDetail' AND column_name = 'ItemName')
BEGIN
ALTER TABLE dbo.BanquetReservationDetail
   ALTER COLUMN ItemName VARCHAR(MAX) NULL; 
END 
GO

/****** Object:  Table [dbo].[BanquetReservationClassificationDiscount]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationClassificationDiscount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetReservationClassificationDiscount](
	[DiscountId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_BanquetReservationClassificationDiscount] PRIMARY KEY CLUSTERED 
(
	[DiscountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/*=================================================================== 
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  17-07-2018		FA		Data type and column name change of DiscountId, ReservationId
  ===================================================================*/

  -- Change DiscountId to Id and type int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationClassificationDiscount' AND column_name = 'DiscountId' AND DATA_TYPE = 'int')
BEGIN
	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationClassificationDiscount]') AND name = N'PK_BanquetReservationClassificationDiscount')
	ALTER TABLE [dbo].[BanquetReservationClassificationDiscount] DROP CONSTRAINT [PK_BanquetReservationClassificationDiscount]
	
	ALTER TABLE dbo.BanquetReservationClassificationDiscount
	   ALTER COLUMN DiscountId bigint NOT NULL;
 
	ALTER TABLE [dbo].[BanquetReservationClassificationDiscount] ADD  CONSTRAINT [PK_BanquetReservationClassificationDiscount] PRIMARY KEY CLUSTERED 
	(
		[DiscountId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	 
	 EXEC sp_rename 'BanquetReservationClassificationDiscount.DiscountId', 'Id', 'COLUMN';    
END


-- Change ReservationId data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationClassificationDiscount' AND column_name = 'ReservationId' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationClassificationDiscount
	   ALTER COLUMN ReservationId bigint NULL;
END
-- Foreign Key Integration ------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservationClassificationDiscount_BanquetReservation') AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservationClassificationDiscount'))
BEGIN
ALTER TABLE [dbo].[BanquetReservationClassificationDiscount]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservationClassificationDiscount_BanquetReservation] FOREIGN KEY([ReservationId])
REFERENCES [dbo].[BanquetReservation] ([Id])

ALTER TABLE [dbo].[BanquetReservationClassificationDiscount] CHECK CONSTRAINT [FK_BanquetReservationClassificationDiscount_BanquetReservation]
END
GO


/****** Object:  Table [dbo].[BanquetReservationBillPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationBillPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetReservationBillPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[BillNumber] [varchar](50) NULL,
	[PaymentType] [varchar](100) NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentMode] [varchar](20) NULL,
	[FieldId] [int] NULL,
	[CurrencyAmount] [decimal](18, 2) NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[AccountsPostingHeadId] [int] NULL,
	[IsPaymentTransfered] [int] NULL,
	[BankId] [int] NULL,
	[BranchName] [varchar](250) NULL,
	[ChecqueNumber] [varchar](50) NULL,
	[ChecqueDate] [datetime] NULL,
	[CardNumber] [varchar](50) NULL,
	[CardReference] [varchar](50) NULL,
	[DealId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[ExpireDate] [datetime] NULL,
	[CardType] [varchar](20) NULL,
	[CardHolderName] [varchar](256) NULL,
 CONSTRAINT [PK_BanquetReservationBillPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/*=================================================================== 
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  23-07-2018		FA		Data type and column name change of PaymentId, CreatedBy, LastModifiedBy
  ===================================================================*/
  -- Change PaymentId to Id and type int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'PaymentId' AND DATA_TYPE = 'int')
BEGIN
	IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetReservationBillPayment]') AND name = N'PK_BanquetReservationBillPayment')
	ALTER TABLE [dbo].[BanquetReservationBillPayment] DROP CONSTRAINT [PK_BanquetReservationBillPayment]
	
	ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN PaymentId bigint NOT NULL;
 
	ALTER TABLE [dbo].[BanquetReservationBillPayment] ADD  CONSTRAINT [PK_BanquetReservationBillPayment] PRIMARY KEY CLUSTERED 
	(
		[PaymentId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	 
	 EXEC sp_rename 'BanquetReservationBillPayment.PaymentId', 'Id', 'COLUMN';    
END

-- Change CreatedByDate to CreatedDate
 IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'CreatedByDate')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
  ALTER COLUMN CreatedByDate datetime NULL;
   
EXEC sp_rename 'BanquetReservationBillPayment.CreatedByDate', 'CreatedDate', 'COLUMN';     
END

-- Change LastModifiedByDate to LastModifiedDate
 IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'LastModifiedByDate')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
  ALTER COLUMN LastModifiedByDate datetime NULL;
   
EXEC sp_rename 'BanquetReservationBillPayment.LastModifiedByDate', 'LastModifiedDate', 'COLUMN';     
END

-- Change AccountsPostingHeadId data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'AccountsPostingHeadId' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN AccountsPostingHeadId bigint NULL;
END

-- Change ReservationId data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'ReservationId' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN ReservationId bigint NULL;
END

-- Change BankId data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'BankId' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN BankId bigint NULL;
END
-- Change FieldId data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'FieldId' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN FieldId bigint NULL;
END

-- Change DealId data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'DealId' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN DealId bigint NULL;
END

-- Change CreatedBy data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'CreatedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN CreatedBy bigint NULL;
END

-- Change LastModifiedBy data type from int to bigint
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetReservationBillPayment' AND column_name = 'LastModifiedBy' AND DATA_TYPE = 'int')
BEGIN
ALTER TABLE dbo.BanquetReservationBillPayment
	   ALTER COLUMN LastModifiedBy bigint NULL;
END

--Forign Key relation of BanquetReservationBillPayment with GLNodeMatrix and BanquetReservation
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservationBillPayment_BanquetReservation')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservationBillPayment'))
BEGIN

ALTER TABLE [dbo].[BanquetReservationBillPayment]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservationBillPayment_BanquetReservation] 
FOREIGN KEY([ReservationId])
REFERENCES [dbo].[BanquetReservation] ([Id])

ALTER TABLE [dbo].[BanquetReservationBillPayment] CHECK CONSTRAINT [FK_BanquetReservationBillPayment_BanquetReservation]

END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetReservationBillPayment_GLNodeMatrix')
   AND parent_object_id = OBJECT_ID(N'dbo.BanquetReservationBillPayment'))
BEGIN
ALTER TABLE [dbo].[BanquetReservationBillPayment]  WITH CHECK ADD  CONSTRAINT [FK_BanquetReservationBillPayment_GLNodeMatrix] FOREIGN KEY([AccountsPostingHeadId])
REFERENCES [dbo].[GLNodeMatrix] ([NodeId])

ALTER TABLE [dbo].[BanquetReservationBillPayment] CHECK CONSTRAINT [FK_BanquetReservationBillPayment_GLNodeMatrix]
END
GO

/****** Object:  Table [dbo].[BanquetBillPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetBillPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetBillPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentAmount] [decimal](18, 0) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[DealId] [int] NULL,
 CONSTRAINT [PK_BanquetBillPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

-- Primary key PaymentId type changed from INT to BIGINT and rename to Id

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetBillPayment' AND column_name = 'PaymentId' AND DATA_TYPE = 'int')
BEGIN
 IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BanquetBillPayment]') AND name = N'PK_BanquetBillPayment')
 ALTER TABLE [dbo].[BanquetBillPayment] DROP CONSTRAINT [PK_BanquetBillPayment]
 
 ALTER TABLE dbo.BanquetBillPayment
    ALTER COLUMN PaymentId bigint NOT NULL;
 
 ALTER TABLE [dbo].[BanquetBillPayment] ADD  CONSTRAINT [PK_BanquetBillPayment] PRIMARY KEY CLUSTERED 
 (
  [PaymentId] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   exec sp_RENAME 'BanquetBillPayment.PaymentId' , 'Id', 'COLUMN'
END

--- ReservationId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetBillPayment' AND column_name = 'ReservationId')
BEGIN
ALTER TABLE dbo.BanquetBillPayment
   ALTER COLUMN ReservationId BIGINT NULL; 
END 
GO

--- DealId column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetBillPayment' AND column_name = 'DealId')
BEGIN
ALTER TABLE dbo.BanquetBillPayment
   ALTER COLUMN DealId BIGINT NULL; 
END 
GO

--- CreatedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetBillPayment' AND column_name = 'CreatedBy')
BEGIN
ALTER TABLE dbo.BanquetBillPayment
   ALTER COLUMN CreatedBy BIGINT NULL; 
END 
GO

--- LastModifiedBy column type changed INT to BIGINT
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'BanquetBillPayment' AND column_name = 'LastModifiedBy')
BEGIN
ALTER TABLE dbo.BanquetBillPayment
   ALTER COLUMN LastModifiedBy BIGINT NULL; 
END 
GO
---- Foreign Key integration ------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_BanquetBillPayment_BanquetReservation') 
AND parent_object_id = OBJECT_ID(N'dbo.BanquetBillPayment'))
BEGIN
ALTER TABLE [dbo].[BanquetBillPayment]  WITH CHECK ADD  CONSTRAINT [FK_BanquetBillPayment_BanquetReservation] 
FOREIGN KEY([ReservationId])
REFERENCES [dbo].[BanquetReservation] ([Id])

ALTER TABLE [dbo].[BanquetBillPayment] CHECK CONSTRAINT [FK_BanquetBillPayment_BanquetReservation]
END
GO


GO
/****** Object:  UserDefinedFunction [dbo].[FnTens]    Script Date: 06/29/2018 12:43:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FnTens]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE Function [dbo].[FnTens](@iTens int)
	Returns varchar(25)
As
Begin
	Declare @TenStr varchar(25)
	Declare @lOnes varchar(5)
	Declare @lTens varchar(7)

	If Floor(@iTens/10)=0
			Set @TenStr=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End	if Floor(@iTens/10)=1 
		Begin
			if @iTens=11 set @TenStr=''Eleven''
			if @iTens=12 set @TenStr=''Twelve''
			if @iTens=13 set @TenStr=''Thirteen''
			if @iTens=14 set @TenStr=''Fourteen''
			if @iTens=15 set @TenStr=''Fifteen''
			if @iTens=16 set @TenStr=''Sixteen''
			if @iTens=17 set @TenStr=''Seventeen''
			if @iTens=18 set @TenStr=''Eighteen''
			if @iTens=19 set @TenStr=''Nineteen''
		End

	If Floor(@iTens/10)=2 
		Begin
			set @lTens=''Twenty''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End

			set @TenStr=@lTens+space(1)+@lOnes
		End

	If Floor(@iTens/10)=3 
		Begin
			set @lTens=''Thirty''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End

			set @TenStr=@lTens+space(1)+@lOnes
		End

	If Floor(@iTens/10)=4 
		Begin
			set @lTens=''Forty''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End

			set @TenStr=@lTens+space(1)+@lOnes
		End

	If Floor(@iTens/10)=5 
		Begin
			set @lTens=''Fifty''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End

			set @TenStr=@lTens+space(1)+@lOnes
		End

	If Floor(@iTens/10)=6 
		Begin
			set @lTens=''Sixty''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End

			set @TenStr=@lTens+space(1)+@lOnes
		End

	If Floor(@iTens/10)=7 
		Begin
			set @lTens=''Seventy''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End

			set @TenStr=@lTens+space(1)+@lOnes
		End

	If Floor(@iTens/10)=8 
		Begin
			set @lTens=''Eighty''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End
			
			set @TenStr=@lTens+space(1)+@lOnes
		End

	If Floor(@iTens/10)=9 
		Begin
			set @lTens=''Ninety''
			Set @lOnes=Case Cast(Right(Cast(@iTens as varchar(2)),1) as int) 
			When 0 Then '''' When 1 Then ''One'' When 2 Then ''Two'' 
			When 3 Then ''Three'' When 4 Then ''Four'' When 5 Then ''Five'' 
			When 6 Then ''Six'' When 7 Then ''Seven'' When 8 Then ''Eight'' 
			When 9 Then ''Nine'' End

			set @TenStr=@lTens+space(1)+@lOnes
		End

	Return @TenStr
End

' 
END
GO
/****** Object:  Table [dbo].[HotelRoomStopChargePosting]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomStopChargePosting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomStopChargePosting](
	[StopChargePostingId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[CostCenterId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelRoomStopChargePosting] PRIMARY KEY CLUSTERED 
(
	[StopChargePostingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRoomStatusPossiblePathHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomStatusPossiblePathHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomStatusPossiblePathHead](
	[PathId] [int] IDENTITY(1,1) NOT NULL,
	[PossiblePath] [nvarchar](300) NULL,
	[DisplayText] [nvarchar](200) NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_HotelRoomStatusPossiblePathHead] PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRoomStatusPossiblePath]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomStatusPossiblePath]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomStatusPossiblePath](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[UserGroupId] [int] NULL,
	[PossiblePathType] [nvarchar](200) NULL,
	[PathId] [int] NULL,
	[DisplayText] [nvarchar](200) NULL,
	[DisplayOrder] [int] NULL,
 CONSTRAINT [PK_HotelRoomStatusPossiblePath] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRoomStatusHistory]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomStatusHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomStatusHistory](
	[HistoryDate] [datetime] NULL,
	[RoomType] [varchar](50) NULL,
	[TotalRoom] [int] NULL,
	[TotalAvailable] [int] NULL,
	[TotalOutOfService] [int] NULL,
	[TotalReserved] [int] NULL,
	[TotalOccupied] [int] NULL,
	[TotalGuest] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomStatus]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomStatus](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
 CONSTRAINT [PK_RoomStatus] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomReservationOnline]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomReservationOnline]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomReservationOnline](
	[ReservationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservationNumber] [varchar](50) NULL,
	[ReservationDate] [datetime] NULL,
	[DateIn] [datetime] NULL,
	[DateOut] [datetime] NULL,
	[ConfirmationDate] [datetime] NULL,
	[ReservedCompany] [varchar](100) NULL,
	[GuestId] [bigint] NULL,
	[ContactAddress] [varchar](300) NULL,
	[ContactPerson] [varchar](200) NULL,
	[ContactNumber] [varchar](100) NULL,
	[MobileNumber] [varchar](20) NULL,
	[FaxNumber] [varchar](50) NULL,
	[ContactEmail] [varchar](100) NULL,
	[TotalRoomNumber] [int] NULL,
	[ReservedMode] [varchar](20) NULL,
	[ReservationType] [varchar](20) NULL,
	[ReservationMode] [varchar](20) NULL,
	[PendingDeadline] [datetime] NULL,
	[IsListedCompany] [bit] NULL,
	[CompanyId] [int] NULL,
	[BusinessPromotionId] [int] NULL,
	[ReferenceId] [int] NULL,
	[PaymentMode] [varchar](20) NULL,
	[PayFor] [int] NULL,
	[CurrencyType] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[Reason] [varchar](500) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[NumberOfPersonAdult] [int] NULL,
	[NumberOfPersonChild] [int] NULL,
	[IsFamilyOrCouple] [bit] NULL,
	[AirportPickUp] [varchar](50) NULL,
	[AirportDrop] [varchar](50) NULL,
 CONSTRAINT [PK_RoomReservationOnline] PRIMARY KEY CLUSTERED 
(
	[ReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO



/****** Object:  Table [dbo].[HotelRoomReservationDetailOnline]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomReservationDetailOnline]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomReservationDetailOnline](
	[ReservationDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[RoomTypeId] [int] NULL,
	[RoomId] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[DiscountType] [varchar](20) NULL,
	[Amount] [decimal](18, 2) NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[NoShowCharge] [decimal](18, 2) NULL,
	[IsRegistered] [bit] NULL,
	[Status] [nvarchar](20) NULL,
 CONSTRAINT [PK_ReservationDetailOnline] PRIMARY KEY CLUSTERED 
(
	[ReservationDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomReservationDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomReservationDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomReservationDetail](
	[ReservationDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[RoomTypeId] [int] NULL,
	[RoomId] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[DiscountType] [varchar](20) NULL,
	[Amount] [decimal](18, 2) NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[NoShowCharge] [decimal](18, 2) NULL,
	[IsRegistered] [bit] NULL,
	[Status] [nvarchar](20) NULL,
	[RoomTypeWisePaxQuantity] [int] NULL,
 CONSTRAINT [PK_ReservationDetail] PRIMARY KEY CLUSTERED 
(
	[ReservationDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
-- add new column IsServiceChargeEnable 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservationDetail' AND column_name = 'IsServiceChargeEnable')
BEGIN
	ALTER TABLE dbo.HotelRoomReservationDetail
		ADD IsServiceChargeEnable INT NOT NULL CONSTRAINT DF_HotelRoomReservationDetail_IsServiceChargeEnable DEFAULT 0 WITH VALUES	
END

GO

-- add new column IsCityChargeEnable 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservationDetail' AND column_name = 'IsCityChargeEnable')
BEGIN
	ALTER TABLE dbo.HotelRoomReservationDetail
	  ADD IsCityChargeEnable INT NOT NULL CONSTRAINT DF_HotelRoomReservationDetail_IsCityChargeEnable DEFAULT 0 WITH VALUES
END
GO

-- add new column IsVatAmountEnable 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservationDetail' AND column_name = 'IsVatAmountEnable')
BEGIN
	ALTER TABLE dbo.HotelRoomReservationDetail
		ADD IsVatAmountEnable INT NOT NULL CONSTRAINT DF_HotelRoomReservationDetail_IsVatAmountEnable DEFAULT 0 WITH VALUES
END
GO

-- add new column IsAdditionalChargeEnable
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservationDetail' AND column_name = 'IsAdditionalChargeEnable')
BEGIN
	ALTER TABLE dbo.HotelRoomReservationDetail
		ADD IsAdditionalChargeEnable INT NOT NULL CONSTRAINT DF_HotelRoomReservationDetail_IsAdditionalChargeEnable DEFAULT 0 WITH VALUES
END
GO
-- add new column TotalRoomRate
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservationDetail' AND column_name = 'TotalRoomRate')
BEGIN
	ALTER TABLE dbo.HotelRoomReservationDetail
		ADD TotalRoomRate DECIMAL(18, 2) NULL;
END


SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomReservation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomReservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomReservation](
	[ReservationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservationNumber] [varchar](50) NULL,
	[ReservationDate] [datetime] NULL,
	[DateIn] [datetime] NULL,
	[DateOut] [datetime] NULL,
	[ConfirmationDate] [datetime] NULL,
	[ReservedCompany] [varchar](100) NULL,
	[GuestId] [bigint] NULL,
	[ContactAddress] [varchar](300) NULL,
	[ContactPerson] [varchar](200) NULL,
	[ContactNumber] [varchar](100) NULL,
	[MobileNumber] [varchar](20) NULL,
	[FaxNumber] [varchar](50) NULL,
	[ContactEmail] [varchar](100) NULL,
	[TotalRoomNumber] [int] NULL,
	[ReservedMode] [varchar](20) NULL,
	[ReservationType] [varchar](20) NULL,
	[ReservationMode] [varchar](20) NULL,
	[PendingDeadline] [datetime] NULL,
	[IsListedCompany] [bit] NULL,
	[CompanyId] [int] NULL,
	[BusinessPromotionId] [int] NULL,
	[ReferenceId] [int] NULL,
	[PaymentMode] [varchar](20) NULL,
	[PayFor] [int] NULL,
	[CurrencyType] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[Reason] [varchar](500) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[NumberOfPersonAdult] [int] NULL,
	[NumberOfPersonChild] [int] NULL,
	[IsFamilyOrCouple] [bit] NULL,
	[AirportPickUp] [varchar](50) NULL,
	[AirportDrop] [varchar](50) NULL,
	[RoomInfo] [varchar](1000) NULL,
	[MarketSegmentId] [int] NULL,
	[GuestSourceId] [int] NULL,
	[IsRoomRateShowInPreRegistrationCard] [bit] NULL,
 CONSTRAINT [PK_RoomReservation] PRIMARY KEY CLUSTERED 
(
	[ReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'MealPlanId')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD MealPlanId INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'ClassificationId')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD ClassificationId INT NOT NULL CONSTRAINT DF_HotelRoomReservation_ClassificationId DEFAULT 452 WITH VALUES	
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'IsVIPGuest')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD IsVIPGuest BIT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'VipGuestTypeId')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD VipGuestTypeId INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'BookersName')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD BookersName NVARCHAR(MAX) NULL;
END
GO 
--Add column GuestRemarks----
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'GuestRemarks')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD GuestRemarks VARCHAR(500) NULL
END
GO  
-- add column POS remarks
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'POSRemarks')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD POSRemarks VARCHAR(500) NULL
END
GO 
-- add column IsListedContact
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'IsListedContact')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD IsListedContact BIT NULL
END
GO 
-- add column ContactId
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'ContactId')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD ContactId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomReservation' AND column_name = 'IsComplementaryGuest')
BEGIN
	ALTER TABLE dbo.HotelRoomReservation
		ADD IsComplementaryGuest BIT NOT NULL DEFAULT 0 WITH VALUES
END
GO
/****** Object:  Table [dbo].[HotelRoomRegistrationDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomRegistrationDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomRegistrationDetail](
	[RegistrationDetailId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[GuestName] [varchar](100) NULL,
	[GuestDOB] [varchar](20) NULL,
	[GuestSex] [varchar](20) NULL,
	[GuestEmail] [varchar](50) NULL,
	[GuestPhone] [varchar](50) NULL,
	[GuestAddress1] [varchar](250) NULL,
	[GuestAddress2] [varchar](250) NULL,
	[GuestCity] [varchar](50) NULL,
	[GuestZipCode] [varchar](10) NULL,
	[GuestCountryId] [int] NULL,
	[GuestNationality] [varchar](50) NULL,
	[GuestDrivinlgLicense] [varchar](50) NULL,
	[GuestAuthentication] [varchar](50) NULL,
	[NationalId] [varchar](50) NULL,
	[PassportNumber] [varchar](50) NULL,
	[PIssueDate] [varchar](10) NULL,
	[PIssuePlace] [varchar](50) NULL,
	[PExpireDate] [varchar](10) NULL,
	[VisaNumber] [varchar](50) NULL,
	[VIssueDate] [varchar](10) NULL,
	[VExpireDate] [varchar](10) NULL,
 CONSTRAINT [PK_RegistrationDetails] PRIMARY KEY CLUSTERED 
(
	[RegistrationDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomRegistration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomRegistration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomRegistration](
	[RegistrationId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationNumber] [varchar](50) NULL,
	[ArriveDate] [datetime] NULL,
	[BillingStartDate] [datetime] NULL,
	[ExpectedCheckOutDate] [datetime] NULL,
	[BillHoldUpDate] [datetime] NULL,
	[CheckOutDate] [datetime] NULL,
	[ActualCheckOutDate] [datetime] NULL,
	[RoomId] [int] NULL,
	[EntitleRoomType] [int] NULL,
	[CurrencyType] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[DiscountType] [varchar](20) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[NoShowCharge] [decimal](18, 2) NULL,
	[IsServiceChargeEnable] [bit] NULL,
	[IsCityChargeEnable] [bit] NULL,
	[IsVatAmountEnable] [bit] NULL,
	[IsAdditionalChargeEnable] [bit] NULL,
	[TotalRoomRate] [decimal](18, 2) NULL,
	[IsCompanyGuest] [bit] NULL,
	[IsHouseUseRoom] [bit] NULL,
	[CommingFrom] [varchar](200) NULL,
	[NextDestination] [varchar](200) NULL,
	[VisitPurpose] [varchar](500) NULL,
	[IsFromReservation] [bit] NULL,
	[ReservationId] [int] NULL,
	[IsFamilyOrCouple] [bit] NULL,
	[NumberOfPersonAdult] [int] NULL,
	[NumberOfPersonChild] [int] NULL,
	[IsListedCompany] [bit] NULL,
	[ReservedCompany] [varchar](100) NULL,
	[CompanyId] [int] NULL,
	[ContactPerson] [varchar](200) NULL,
	[ContactNumber] [varchar](100) NULL,
	[PaymentMode] [varchar](20) NULL,
	[PayFor] [int] NULL,
	[BusinessPromotionId] [int] NULL,
	[IsRoomOwner] [int] NULL,
	[GuestSourceId] [int] NULL,
	[MealPlanId] [int] NULL,
	[ReferenceId] [int] NULL,
	[IsReturnedGuest] [bit] NULL,
	[IsVIPGuest] [bit] NULL,
	[VipGuestTypeId] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[AirportPickUp] [varchar](50) NULL,
	[AirportDrop] [varchar](50) NULL,
	[CardType] [varchar](20) NULL,
	[CardNumber] [varchar](50) NULL,
	[CardExpireDate] [datetime] NULL,
	[CardHolderName] [varchar](256) NULL,
	[CardReference] [varchar](256) NULL,
	[IsStopChargePosting] [bit] NULL,
	[IsBlankRegistrationCard] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RoomRegistration] PRIMARY KEY CLUSTERED 
(
	[RegistrationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
--Add IsEarlyCheckInChargeEnable Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'IsEarlyCheckInChargeEnable')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
	  ADD IsEarlyCheckInChargeEnable BIT NOT NULL CONSTRAINT DF_HotelRoomRegistration_IsEarlyCheckInChargeEnable DEFAULT 1 WITH VALUES
END
GO
-- add column POS remarks
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'POSRemarks')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
		ADD POSRemarks VARCHAR(500) NULL
END
GO

--Add HoldUpAmount Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'HoldUpAmount')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
	  ADD HoldUpAmount DECIMAL(18, 2) NOT NULL CONSTRAINT DF_HotelRoomRegistration_HoldUpAmount DEFAULT 0 WITH VALUES
END
GO

--Added GuidId column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_HotelRoomRegistration_GuidId DEFAULT (newid()) WITH VALUES
END
GO

 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'MarketSegmentId')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
		ADD MarketSegmentId INT NULL
END
GO
-- add column IsListedContact
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'IsListedContact')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
		ADD IsListedContact BIT NULL
END
GO 
-- add column ContactId
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'ContactId')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
		ADD ContactId BIGINT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'PackageId')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
		ADD PackageId BIGINT NULL
END
GO
-- add column IsExpectedCheckOutTimeEnable
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomRegistration' AND column_name = 'IsExpectedCheckOutTimeEnable')
BEGIN
	ALTER TABLE dbo.HotelRoomRegistration
		ADD IsExpectedCheckOutTimeEnable BIT NULL DEFAULT 0 WITH VALUES;
END
GO 
/****** Object:  Table [dbo].[HotelRoomOwnerDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomOwnerDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomOwnerDetail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NULL,
	[RoomId] [int] NULL,
	[CommissionValue] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OwnerDetail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRoomOwner]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomOwner]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomOwner](
	[OwnerId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[Description] [varchar](500) NULL,
	[Address] [varchar](500) NULL,
	[CityName] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[StateName] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[Email] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RoomOwner] PRIMARY KEY CLUSTERED 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomNumber]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomNumber]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomNumber](
	[RoomId] [int] IDENTITY(1,1) NOT NULL,
	[RoomTypeId] [int] NULL,
	[RoomNumber] [varchar](50) NULL,
	[RoomName] [varchar](300) NULL,
	[IsSmokingRoom] [bit] NULL,
	[StatusId] [int] NULL,
	[HKRoomStatusId] [bigint] NULL,
	[CleanupStatus] [varchar](50) NULL,
	[CleanDate] [datetime] NULL,
	[LastCleanDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RoomNumber] PRIMARY KEY CLUSTERED 
(
	[RoomId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomLogFile]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomLogFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomLogFile](
	[RoomLogFileId] [bigint] IDENTITY(1,1) NOT NULL,
	[RoomId] [int] NULL,
	[StatusId] [int] NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
 CONSTRAINT [PK_RoomLogFile] PRIMARY KEY CLUSTERED 
(
	[RoomLogFileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomInventoryDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomInventoryDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomInventoryDetails](
	[OutDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[InventoryOutId] [int] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[StockById] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
 CONSTRAINT [PK_HotelRoomInventoryDetails] PRIMARY KEY CLUSTERED 
(
	[OutDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRoomInventory]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomInventory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomInventory](
	[InventoryOutId] [int] IDENTITY(1,1) NOT NULL,
	[OutDate] [datetime] NOT NULL,
	[RoomTypeId] [int] NOT NULL,
	[RoomId] [int] NOT NULL,
	[Remarks] [varchar](500) NULL,
	[Status] [nvarchar](15) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HMRoomInventory] PRIMARY KEY CLUSTERED 
(
	[InventoryOutId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRoomDiscrepancy]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomDiscrepancy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomDiscrepancy](
	[RoomDiscrepancyId] [bigint] IDENTITY(1,1) NOT NULL,
	[RoomId] [int] NOT NULL,
	[TaskId] [bigint] NULL,
	[HKRoomStatusId] [bigint] NOT NULL,
	[FOPersons] [int] NULL,
	[HKPersons] [int] NULL,
	[DiscrepanciesDetails] [nvarchar](500) NULL,
	[AssignDate] [datetime] NULL,
	[Reason] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelRoomDiscrepancy] PRIMARY KEY CLUSTERED 
(
	[RoomDiscrepancyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRoomCondition]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomCondition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomCondition](
	[RoomConditionId] [bigint] IDENTITY(1,1) NOT NULL,
	[ConditionName] [nvarchar](100) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelRoomCondition] PRIMARY KEY CLUSTERED 
(
	[RoomConditionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelReservationServiceInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelReservationServiceInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelReservationServiceInfo](
	[DetailServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[ServiceId] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[IsAchieved] [bit] NULL,
 CONSTRAINT [PK_HotelReservationPaidService] PRIMARY KEY CLUSTERED 
(
	[DetailServiceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelReservationNoShowProcess]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelReservationNoShowProcess]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelReservationNoShowProcess](
	[ProcessId] [int] IDENTITY(1,1) NOT NULL,
	[ProcessDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelReservationNoShowProcess] PRIMARY KEY CLUSTERED 
(
	[ProcessId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelReservationComplementaryItem]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelReservationComplementaryItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelReservationComplementaryItem](
	[RCItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[ComplementaryItemId] [int] NULL,
 CONSTRAINT [PK_ReservationComplementaryItem] PRIMARY KEY CLUSTERED 
(
	[RCItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelReservationBillPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelReservationBillPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelReservationBillPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[PaymentType] [varchar](100) NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentMode] [varchar](20) NULL,
	[FieldId] [int] NULL,
	[CurrencyAmount] [decimal](18, 2) NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[AccountsPostingHeadId] [int] NULL,
	[IsPaymentTransfered] [int] NULL,
	[BankId] [int] NULL,
	[BranchName] [varchar](250) NULL,
	[ChecqueNumber] [varchar](50) NULL,
	[ChecqueDate] [datetime] NULL,
	[CardNumber] [varchar](50) NULL,
	[CardReference] [varchar](50) NULL,
	[DealId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[ExpireDate] [datetime] NULL,
	[CardType] [varchar](20) NULL,
	[CardHolderName] [varchar](256) NULL,
	[Remarks] [varchar](max) NULL,
 CONSTRAINT [PK_GuestReservationBillPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelReservationBillPayment' AND column_name = 'BillNumber')
BEGIN
	ALTER TABLE dbo.HotelReservationBillPayment
		ADD BillNumber NVARCHAR(50) NULL;
END
GO
/****** Object:  Table [dbo].[HotelReservationAireportPickupDrop]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelReservationAireportPickupDrop]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelReservationAireportPickupDrop](
	[APDId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[GuestId] [int] NULL,
	[PickupDropType] [varchar](20) NULL,
	[ArrivalAirlineId] [int] NULL,
	[ArrivalFlightName] [varchar](200) NULL,
	[ArrivalFlightNumber] [varchar](200) NULL,
	[ArrivalTime] [time](7) NULL,
	[IsArrivalChargable] [bit] NULL,
	[DepartureAirlineId] [int] NULL,
	[DepartureFlightName] [varchar](200) NULL,
	[DepartureFlightNumber] [varchar](200) NULL,
	[DepartureTime] [time](7) NULL,
	[IsDepartureChargable] [bit] NULL,
 CONSTRAINT [PK_ReservationAireportPickupDrop] PRIMARY KEY CLUSTERED 
(
	[APDId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelRegistrationServiceInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRegistrationServiceInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRegistrationServiceInfo](
	[DetailServiceId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[ServiceId] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[IsAchieved] [bit] NULL,
 CONSTRAINT [PK_HotelRegistrationPaidService] PRIMARY KEY CLUSTERED 
(
	[DetailServiceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRegistrationComplementaryItem]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRegistrationComplementaryItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRegistrationComplementaryItem](
	[RCItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[ComplementaryItemId] [int] NULL,
 CONSTRAINT [PK_RegistrationComplementaryItem] PRIMARY KEY CLUSTERED 
(
	[RCItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRegistrationAireportPickupDrop]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRegistrationAireportPickupDrop]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRegistrationAireportPickupDrop](
	[APDId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[ArrivalFlightName] [varchar](200) NULL,
	[ArrivalFlightNumber] [varchar](50) NULL,
	[ArrivalTime] [time](7) NULL,
	[DepartureAirlineId] [int] NULL,
	[DepartureFlightName] [varchar](200) NULL,
	[DepartureFlightNumber] [varchar](50) NULL,
	[DepartureTime] [time](7) NULL,
 CONSTRAINT [PK_RegistrationAireportPickupDrop] PRIMARY KEY CLUSTERED 
(
	[APDId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
-- add column IsDepartureChargable
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRegistrationAireportPickupDrop' AND column_name = 'IsDepartureChargable')
BEGIN
	ALTER TABLE dbo.HotelRegistrationAireportPickupDrop
		ADD IsDepartureChargable BIT NULL
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelPaymentSummary]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelPaymentSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelPaymentSummary](
	[SummaryId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [datetime] NULL,
	[ServiceDate] [varchar](100) NULL,
	[RoomNumber] [varchar](100) NULL,
	[BillNumber] [varchar](100) NULL,
	[PaymentDescription] [varchar](300) NULL,
	[PaymentMode] [varchar](200) NULL,
	[POSTerminalBank] [varchar](300) NULL,
	[ReceivedAmount] [decimal](18, 2) NULL,
	[PaidAmount] [decimal](18, 2) NULL,
	[OperatedBy] [varchar](300) NULL,
	[ReportType] [varchar](100) NULL,
 CONSTRAINT [PK_HotelPaymentSummary] PRIMARY KEY CLUSTERED 
(
	[SummaryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelOnlineRoomReservationDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelOnlineRoomReservationDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelOnlineRoomReservationDetail](
	[ReservationDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationId] [int] NULL,
	[RoomTypeId] [int] NULL,
	[RoomId] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[DiscountType] [varchar](20) NULL,
	[Amount] [decimal](18, 2) NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[IsRegistered] [bit] NULL,
 CONSTRAINT [PK_OnlineReservationDetail] PRIMARY KEY CLUSTERED 
(
	[ReservationDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelOnlineRoomReservation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelOnlineRoomReservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelOnlineRoomReservation](
	[ReservationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservationNumber] [varchar](50) NULL,
	[ReservationDate] [datetime] NULL,
	[DateIn] [datetime] NULL,
	[DateOut] [datetime] NULL,
	[ConfirmationDate] [datetime] NULL,
	[ReservedCompany] [varchar](100) NULL,
	[GuestId] [bigint] NULL,
	[ContactAddress] [varchar](300) NULL,
	[ContactPerson] [varchar](200) NULL,
	[ContactNumber] [varchar](100) NULL,
	[MobileNumber] [varchar](20) NULL,
	[FaxNumber] [varchar](50) NULL,
	[ContactEmail] [varchar](100) NULL,
	[TotalRoomNumber] [int] NULL,
	[ReservedMode] [varchar](20) NULL,
	[ReservationType] [varchar](20) NULL,
	[ReservationMode] [varchar](20) NULL,
	[PendingDeadline] [datetime] NULL,
	[IsListedCompany] [bit] NULL,
	[CompanyId] [int] NULL,
	[BusinessPromotionId] [int] NULL,
	[ReferenceId] [int] NULL,
	[PaymentMode] [varchar](20) NULL,
	[PayFor] [int] NULL,
	[CurrencyType] [int] NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[Reason] [varchar](500) NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_OnlineRoomReservation] PRIMARY KEY CLUSTERED 
(
	[ReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelMonthToDateInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelMonthToDateInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelMonthToDateInfo](
	[MTDID] [bigint] IDENTITY(1,1) NOT NULL,
	[MTDDate] [date] NOT NULL,
	[ActualRoomsOccupied] [decimal](18, 2) NULL,
	[Occupency] [decimal](18, 2) NULL,
	[ActualRoomsRevenue] [decimal](18, 2) NULL,
	[AverageRate] [decimal](18, 2) NULL,
	[RevenuePerRoom] [decimal](18, 2) NULL,
	[MTDAVGRoomsOccupancy] [decimal](18, 2) NULL,
	[MTDRoomsAverageRevenue] [decimal](18, 2) NULL,
	[MTDAverageRate] [decimal](18, 2) NULL,
	[MTDRevenuePerRoom] [decimal](18, 2) NULL,
	[Remarks] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelMonthToDateInfo_1] PRIMARY KEY CLUSTERED 
(
	[MTDID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelInHouseGuestLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelInHouseGuestLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelInHouseGuestLedger](
	[RegistrationId] [int] NULL,
	[TransactionDate] [datetime] NULL,
	[TransactionType] [varchar](100) NULL,
	[NightAuditStatus] [bit] NULL,
	[TransactionId] [int] NULL,
	[TransactionHead] [varchar](300) NULL,
	[TransactionAmount] [decimal](18, 2) NULL,
	[CalculationType] [varchar](50) NULL,
	[IsTransactionVoid] [bit] NULL,
	[IsGuestCheckOut] [bit] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelHKRoomStatus]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelHKRoomStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelHKRoomStatus](
	[HKRoomStatusId] [bigint] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[OrderByIndex] [int] NULL,
 CONSTRAINT [PK_HotelHKRoomStatus] PRIMARY KEY CLUSTERED 
(
	[HKRoomStatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelGuestServiceInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestServiceInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestServiceInfo](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
	[CostCenterId] [int] NULL,
	[ServiceName] [varchar](200) NULL,
	[Description] [varchar](300) NULL,
	[ServiceType] [varchar](100) NULL,
	[UnitPriceLocal] [decimal](18, 2) NULL,
	[UnitPriceUsd] [decimal](18, 2) NULL,
	[IsVatEnable] [bit] NULL,
	[IsServiceChargeEnable] [bit] NULL,
	[IsSDChargeEnable] [bit] NULL,
	[IsAdditionalChargeEnable] [bit] NULL,
	[IsGeneralService] [bit] NULL,
	[IsPaidService] [bit] NULL,
	[IsNextDayAchievement] [bit] NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelPaidService] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestServiceBillApproved]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestServiceBillApproved]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestServiceBillApproved](
	[ApprovedId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[RoomNumber] [varchar](50) NULL,
	[ApprovedDate] [datetime] NULL,
	[ServiceBillId] [int] NOT NULL,
	[ServiceDate] [datetime] NULL,
	[ServiceType] [varchar](100) NULL,
	[ServiceId] [int] NULL,
	[ServiceName] [varchar](100) NULL,
	[ServiceQuantity] [decimal](18, 2) NULL,
	[ServiceRate] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[InvoiceServiceRate] [decimal](18, 2) NULL,
	[IsServiceChargeEnable] [bit] NULL,
	[InvoiceServiceCharge] [decimal](18, 2) NULL,
	[IsVatAmountEnable] [bit] NULL,
	[InvoiceVatAmount] [decimal](18, 2) NULL,
	[ApprovedStatus] [varchar](50) NULL,
	[PaymentMode] [varchar](50) NULL,
	[IsPaidService] [bit] NULL,
	[IsPaidServiceAchieved] [bit] NULL,
	[IsDayClosed] [bit] NULL,
	[RestaurantBillId] [int] NULL,
	[PaidServiceAchievementStatus] [varchar](300) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GuestServiceBillApproved] PRIMARY KEY CLUSTERED 
(
	[ApprovedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestServiceBill]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestServiceBill]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestServiceBill](
	[ServiceBillId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceDate] [datetime] NULL,
	[BillNumber] [varchar](50) NULL,
	[RegistrationId] [int] NULL,
	[GuestName] [varchar](250) NULL,
	[ServiceId] [int] NULL,
	[ServiceRate] [decimal](18, 0) NULL,
	[ServiceQuantity] [int] NULL,
	[DiscountAmount] [decimal](18, 0) NULL,
	[IsComplementary] [bit] NULL,
	[IsPaidService] [bit] NULL,
	[IsPaidServiceAchieved] [bit] NULL,
	[Remarks] [varchar](500) NULL,
	[PaymentMode] [varchar](20) NULL,
	[EmpId] [int] NULL,
	[CompanyId] [int] NULL,
	[RackRate] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[InvoiceRackRate] [decimal](18, 2) NULL,
	[IsServiceChargeEnable] [bit] NULL,
	[InvoiceServiceCharge] [decimal](18, 2) NULL,
	[IsCitySDChargeEnable] [bit] NULL,
	[InvoiceCitySDCharge] [decimal](18, 2) NULL,
	[IsVatAmountEnable] [bit] NULL,
	[InvoiceVatAmount] [decimal](18, 2) NULL,
	[IsAdditionalChargeEnable] [bit] NULL,
	[InvoiceAdditionalCharge] [decimal](18, 2) NULL,
	[TotalCalculatedAmount] [decimal](18, 2) NULL,
	[CurrencyExchangeRate] [decimal](18, 2) NULL,
	[InvoiceUsdRackRate] [decimal](18, 2) NULL,
	[InvoiceUsdServiceCharge] [decimal](18, 2) NULL,
	[InvoiceUsdVatAmount] [decimal](18, 2) NULL,
	[ReferenceServiceBillId] [int] NULL,
	[ApprovedStatus] [bit] NULL,
	[ApprovedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GHServiceBill] PRIMARY KEY CLUSTERED 
(
	[ServiceBillId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestServiceBill' AND column_name = 'ReferenceBillNumber')
BEGIN
	ALTER TABLE dbo.HotelGuestServiceBill
		ADD ReferenceBillNumber NVARCHAR(50) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestServiceBill' AND column_name = 'GuidId')
BEGIN
	ALTER TABLE dbo.HotelGuestServiceBill
	  ADD GuidId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_HotelGuestServiceBill_GuidId DEFAULT (newid()) WITH VALUES
END
GO

/****** Object:  Table [dbo].[HotelGuestRoomShiftInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestRoomShiftInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestRoomShiftInfo](
	[RoomShiftId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[PreviousRoomId] [int] NULL,
	[ShiftedRoomId] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelGuestRoomShiftInfo] PRIMARY KEY CLUSTERED 
(
	[RoomShiftId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestReservationOnline]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestReservationOnline]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestReservationOnline](
	[GuestReservationId] [bigint] IDENTITY(1,1) NOT NULL,
	[GuestId] [bigint] NOT NULL,
	[ReservationId] [bigint] NOT NULL,
	[RoomId] [int] NULL,
 CONSTRAINT [PK_HotelGuestReservationOnline] PRIMARY KEY CLUSTERED 
(
	[GuestReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelGuestReservation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestReservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestReservation](
	[GuestReservationId] [bigint] IDENTITY(1,1) NOT NULL,
	[GuestId] [bigint] NOT NULL,
	[ReservationId] [bigint] NOT NULL,
	[RoomId] [int] NULL,
 CONSTRAINT [PK_GuestReservation] PRIMARY KEY CLUSTERED 
(
	[GuestReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelGuestRegistration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestRegistration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestRegistration](
	[GuestRegistrationId] [bigint] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [bigint] NULL,
	[GuestId] [bigint] NULL,
	[CheckInDate] [datetime] NULL,
	[CheckOutDate] [datetime] NULL,
	[PaxInRate] [decimal](18, 2) NULL,
 CONSTRAINT [PK_GuestRegistrationMapping] PRIMARY KEY CLUSTERED 
(
	[GuestRegistrationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelGuestReference]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestReference]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestReference](
	[ReferenceId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Email] [varchar](50) NULL,
	[Organization] [varchar](150) NULL,
	[Designation] [varchar](150) NULL,
	[TelephoneNumber] [varchar](50) NULL,
	[CellNumber] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[SalesCommission] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_GuestReference] PRIMARY KEY CLUSTERED 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestPreferenceMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestPreferenceMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestPreferenceMapping](
	[MappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[GuestId] [int] NOT NULL,
	[PreferenceId] [bigint] NOT NULL,
 CONSTRAINT [PK_HotelGuestPreferenceMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelGuestPreference]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestPreference]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestPreference](
	[PreferenceId] [bigint] IDENTITY(1,1) NOT NULL,
	[PreferenceName] [nvarchar](300) NULL,
	[Description] [nvarchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelGuestPreference] PRIMARY KEY CLUSTERED 
(
	[PreferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelGuestInformationOnline]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestInformationOnline]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestInformationOnline](
	[GuestId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](20) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[GuestName] [varchar](100) NULL,
	[GuestDOB] [datetime] NULL,
	[GuestSex] [varchar](20) NULL,
	[GuestEmail] [varchar](50) NULL,
	[ProfessionId] [int] NULL,
	[GuestPhone] [varchar](50) NULL,
	[GuestAddress1] [varchar](250) NULL,
	[GuestAddress2] [varchar](250) NULL,
	[GuestCity] [varchar](50) NULL,
	[GuestZipCode] [varchar](10) NULL,
	[GuestCountryId] [int] NULL,
	[GuestNationality] [varchar](50) NULL,
	[GuestDrivinlgLicense] [varchar](50) NULL,
	[GuestAuthentication] [varchar](50) NULL,
	[NationalId] [varchar](50) NULL,
	[PassportNumber] [varchar](50) NULL,
	[PIssueDate] [datetime] NULL,
	[PIssuePlace] [varchar](50) NULL,
	[PExpireDate] [datetime] NULL,
	[VisaNumber] [varchar](50) NULL,
	[VIssueDate] [datetime] NULL,
	[VExpireDate] [datetime] NULL,
	[GuestPreferences] [text] NULL,
 CONSTRAINT [PK_HotelGuestInformationOnline] PRIMARY KEY CLUSTERED 
(
	[GuestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestInformation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestInformation]') AND type in (N'U'))
CREATE TABLE [dbo].[HotelGuestInformation](
	[GuestId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](20) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[GuestName] [varchar](100) NULL,
	[GuestDOB] [datetime] NULL,
	[GuestSex] [varchar](20) NULL,
	[GuestEmail] [varchar](50) NULL,
	[ProfessionId] [int] NULL,
	[GuestPhone] [varchar](50) NULL,
	[GuestAddress1] [varchar](250) NULL,
	[GuestAddress2] [varchar](250) NULL,
	[GuestCity] [varchar](50) NULL,
	[GuestZipCode] [varchar](10) NULL,
	[GuestCountryId] [int] NULL,
	[GuestNationality] [varchar](50) NULL,
	[GuestDrivinlgLicense] [varchar](50) NULL,
	[GuestAuthentication] [varchar](50) NULL,
	[NationalId] [varchar](50) NULL,
	[PassportNumber] [varchar](50) NULL,
	[PIssueDate] [datetime] NULL,
	[PIssuePlace] [varchar](50) NULL,
	[PExpireDate] [datetime] NULL,
	[VisaNumber] [varchar](50) NULL,
	[VIssueDate] [datetime] NULL,
	[VExpireDate] [datetime] NULL,
	[GuestPreferences] [text] NULL,
	[ClassificationId] [int] NULL
 CONSTRAINT [PK_RegistrationDetail] PRIMARY KEY CLUSTERED 
(
	[GuestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--guest block and description column added 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestInformation' AND column_name = 'GuestBlock')
BEGIN
	ALTER TABLE dbo.HotelGuestInformation
		ADD GuestBlock bit NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestInformation' AND column_name = 'Description')
BEGIN
	ALTER TABLE dbo.HotelGuestInformation
		ADD Description NVARCHAR(300) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestInformation' AND column_name = 'AdditionalRemarks')
BEGIN
	ALTER TABLE dbo.HotelGuestInformation
		ADD AdditionalRemarks NVARCHAR(MAX) NULL;
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestHouseCheckOut]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestHouseCheckOut]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestHouseCheckOut](
	[CheckOutId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[BillNumber] [varchar](50) NULL,
	[CheckOutDate] [datetime] NULL,
	[PayMode] [varchar](20) NULL,
	[BankId] [int] NULL,
	[BranchName] [varchar](250) NULL,
	[ChecqueNumber] [varchar](50) NULL,
	[CardNumber] [varchar](50) NULL,
	[CardType] [varchar](20) NULL,
	[ExpireDate] [datetime] NULL,
	[CardHolderName] [varchar](256) NULL,
	[CardReference] [varchar](50) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[RebateRemarks] [varchar](500) NULL,
	[BillPaidBy] [int] NULL,
	[DealId] [int] NULL,
	[IsDayClosed] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GuestHouseBill] PRIMARY KEY CLUSTERED 
(
	[CheckOutId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestExtraServiceBillApproved]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestExtraServiceBillApproved]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestExtraServiceBillApproved](
	[ApprovedId] [int] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NULL,
	[RegistrationId] [int] NULL,
	[RoomNumber] [varchar](50) NULL,
	[ApprovedDate] [datetime] NULL,
	[ServiceBillId] [int] NOT NULL,
	[ServiceDate] [datetime] NULL,
	[ServiceType] [varchar](100) NULL,
	[ServiceId] [int] NULL,
	[ServiceName] [varchar](100) NULL,
	[ServiceQuantity] [decimal](18, 2) NULL,
	[ServiceRate] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[InvoiceServiceRate] [decimal](18, 2) NULL,
	[IsServiceChargeEnable] [bit] NULL,
	[InvoiceServiceCharge] [decimal](18, 2) NULL,
	[IsCitySDChargeEnable] [bit] NULL,
	[InvoiceCitySDCharge] [decimal](18, 2) NULL,
	[IsVatAmountEnable] [bit] NULL,
	[InvoiceVatAmount] [decimal](18, 2) NULL,
	[IsAdditionalChargeEnable] [bit] NULL,
	[InvoiceAdditionalCharge] [decimal](18, 2) NULL,
	[CurrencyExchangeRate] [decimal](18, 2) NULL,
	[InvoiceUsdRackRate] [decimal](18, 2) NULL,
	[InvoiceUsdServiceCharge] [decimal](18, 2) NULL,
	[InvoiceUsdVatAmount] [decimal](18, 2) NULL,
	[CalculatedTotalAmount] [decimal](18, 2) NULL,
	[ApprovedStatus] [varchar](50) NULL,
	[PaymentMode] [varchar](50) NULL,
	[IsPaidService] [bit] NULL,
	[IsPaidServiceAchieved] [bit] NULL,
	[IsDayClosed] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelGuestRestaurantExtraBillApproved] PRIMARY KEY CLUSTERED 
(
	[ApprovedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestDocuments]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestDocuments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestDocuments](
	[DocumentsId] [int] IDENTITY(1,1) NOT NULL,
	[GuestId] [int] NULL,
	[Name] [varchar](50) NULL,
	[Path] [varchar](500) NULL,
 CONSTRAINT [PK_GuestDocuments] PRIMARY KEY CLUSTERED 
(
	[DocumentsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestDayLetCheckOut]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestDayLetCheckOut]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestDayLetCheckOut](
	[DayLetId] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[DayLetDiscountType] [varchar](50) NULL,
	[DayLetDiscount] [decimal](18, 2) NULL,
	[DayLetDiscountAmount] [decimal](18, 2) NULL,
	[DayLetStatus] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelGuestDayLetCheckOut] PRIMARY KEY CLUSTERED 
(
	[DayLetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMCompanySignupStatus]    Script Date: 3/5/2019 11:00:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCompanySignupStatus]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMCompanySignupStatus](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Status] [varchar](150) NULL,
		[IsActive] [bit] NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_SMCompanySignupStatus] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[SMCompanySignupStatus] ADD  CONSTRAINT [DF_SMCompanySignupStatus_IsActive]  DEFAULT ((1)) FOR [IsActive]
END
GO
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_HotelGuestCompany_DealStageWiseCompanyStatusId') AND parent_object_id = OBJECT_ID(N'dbo.HotelGuestCompany'))
BEGIN	
	ALTER TABLE [dbo].[HotelGuestCompany]  DROP CONSTRAINT [FK_HotelGuestCompany_DealStageWiseCompanyStatusId]
END
GO
/****** Object:  Table [dbo].[SMDealStageWiseCompanyStatus]    Script Date: 4/26/2019 12:56:08 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMDealStageWiseCompanyStatus]') AND type in (N'U'))
DROP TABLE [dbo].[SMDealStageWiseCompanyStatus]
GO

/****** Object:  Table [dbo].[SMDealStageWiseCompanyStatus]    Script Date: 4/26/2019 12:56:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMLifeCycleStage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMLifeCycleStage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LifeCycleStage] [varchar](200) NULL,
	[DisplaySequence] [int] NULL,
	[Description] [varchar](MAX) NULL,
	[IsRelatedToDeal] [bit] NOT NULL,
	[DealType] [varchar](20) NULL,
	[ForcastType] [varchar](20) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SMLifeCycleStage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[SMLifeCycleStage] ADD  CONSTRAINT [DF_LifeCycleStage_IsActive]  DEFAULT ((1)) FOR [IsActive]

ALTER TABLE [dbo].[SMLifeCycleStage] ADD  CONSTRAINT [DF_LifeCycleStage_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO

/****** Object:  Table [dbo].[HotelGuestCompany]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestCompany]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestCompany](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](150) NULL,
	[CompanyAddress] [varchar](250) NULL,
	[EmailAddress] [varchar](50) NULL,
	[WebAddress] [varchar](50) NULL,
	[ContactPerson] [varchar](100) NULL,
	[ContactNumber] [varchar](50) NULL,
	[ContactDesignation] [varchar](250) NULL,
	[TelephoneNumber] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[DiscountPercent] [decimal](18, 0) NULL,
	[ReferenceId] [int] NULL,
	[IndustryId] [int] NULL,
	[LocationId] [int] NULL,
	[NodeId] [int] NULL,
	[SignupStatus] [varchar](50) NULL,
	[AffiliatedDate] [datetime] NULL,
	[CreditLimit] [decimal](18, 2) NULL,
	[IsMember] [bit] NULL,
	[Balance] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GuestCompany] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'NumberOfEmployee')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD NumberOfEmployee INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BillingLocationId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BillingLocationId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ShippingLocationId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD ShippingLocationId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BillingAddress')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BillingAddress VARCHAR(MAX) NULL 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ShippingAddress')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD ShippingAddress VARCHAR(MAX) NULL 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'AnnualRevenue')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD AnnualRevenue DECIMAL(18,2) NULL
END
GO
--- Migration script for update signup status with SignupStatusId and Foreign key relation given
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'SignupStatus')
BEGIN
	INSERT INTO SMCompanySignupStatus
	(		[Status],
			IsActive,
			CreatedBy,
			CreatedDate
	)
	(SELECT 
	
		'Affiliated',
		1,
		1,
		GETDATE()
	
	UNION ALL
	SELECT 
	
		'Prospective',
		1,
		1,
		GETDATE()
	)
	EXECUTE dbo.sp_executesql @statement = N'
		UPDATE  dbo.HotelGuestCompany
				SET SignupStatus = CASE WHEN SignupStatus = ''Affiliated'' THEN (SELECT Id FROM SMCompanySignupStatus WHERE [Status] = ''Affiliated'')
									WHEN SignupStatus = ''Prospective'' THEN (SELECT Id FROM SMCompanySignupStatus WHERE [Status] = ''Prospective'')
									END'
	
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'SignupStatus')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ALTER COLUMN SignupStatus INT NULL

	EXEC sp_RENAME 'HotelGuestCompany.SignupStatus' , 'SignupStatusId', 'COLUMN'
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ReferenceId')
BEGIN
	EXEC sp_RENAME 'HotelGuestCompany.ReferenceId' , 'CompanyOwnerId', 'COLUMN'
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_HotelGuestCompany_SignupStatusId')
		AND parent_object_id = OBJECT_ID(N'dbo.HotelGuestCompany'))
BEGIN
	ALTER TABLE [dbo].[HotelGuestCompany]  WITH CHECK ADD  CONSTRAINT [FK_HotelGuestCompany_SignupStatusId] 
	FOREIGN KEY([SignupStatusId])
	REFERENCES [dbo].[SMCompanySignupStatus] ([Id])

	ALTER TABLE [dbo].[HotelGuestCompany] CHECK CONSTRAINT [FK_HotelGuestCompany_SignupStatusId]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'DealStageWiseCompanyStatusId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD DealStageWiseCompanyStatusId BIGINT NULL

	ALTER TABLE [dbo].[HotelGuestCompany]  WITH CHECK ADD  CONSTRAINT [FK_HotelGuestCompany_DealStageWiseCompanyStatusId] FOREIGN KEY([DealStageWiseCompanyStatusId])
	REFERENCES [dbo].[SMDealStageWiseCompanyStatus] ([Id])
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'IsClient')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD IsClient BIT NOT NULL CONSTRAINT DF_HotelGuestCompany_IsClient DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'CompanyNumber')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD CompanyNumber VARCHAR(50) NULL 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BranchCode')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BranchCode VARCHAR(50) NULL 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'RemainingBalance')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD RemainingBalance DECIMAL(18,2) NULL
END
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestBillPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestBillPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestBillPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[BillNumber] [varchar](50) NULL,
	[ModuleName] [varchar](100) NULL,
	[PaymentType] [varchar](50) NULL,
	[ServiceBillId] [int] NULL,
	[RegistrationId] [int] NULL,
	[RoomNumber] [varchar](50) NULL,
	[PaymentDate] [datetime] NULL,
	[TransactionDate] [datetime] NULL,
	[PaymentMode] [varchar](20) NULL,
	[PaymentModeId] [int] NULL,
	[FieldId] [int] NULL,
	[CurrencyAmount] [decimal](18, 2) NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[PaymentDescription] [varchar](500) NULL,
	[BankId] [int] NULL,
	[BranchName] [varchar](250) NULL,
	[ChecqueNumber] [varchar](50) NULL,
	[ChecqueDate] [datetime] NULL,
	[CardType] [varchar](20) NULL,
	[CardNumber] [varchar](50) NULL,
	[ExpireDate] [datetime] NULL,
	[CardHolderName] [varchar](256) NULL,
	[CardReference] [varchar](256) NULL,
	[AccountsPostingHeadId] [int] NULL,
	[RefundAccountHead] [int] NULL,
	[Remarks] [varchar](500) NULL,
	[DealId] [int] NULL,
	[ServiceRate] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[CurrencyExchangeRate] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GuestBillPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestBillPayment' AND column_name = 'IsAdvancePayment')
BEGIN
	ALTER TABLE dbo.HotelGuestBillPayment
		ADD IsAdvancePayment BIT NOT NULL CONSTRAINT DF_HotelGuestBillPayment_IsAdvancePayment DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestBillPayment' AND column_name = 'ReservationId')
BEGIN
	ALTER TABLE dbo.HotelGuestBillPayment
		ADD ReservationId BIGINT NOT NULL CONSTRAINT DF_HotelGuestBillPayment_ReservationId DEFAULT 0 WITH VALUES;
END
GO
/****** Object:  Table [dbo].[HotelGuestBillApproved]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestBillApproved]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestBillApproved](
	[ApprovedId] [bigint] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [bigint] NULL,
	[ServiceDate] [date] NULL,
	[ApprovedDate] [date] NULL,
	[RoomType] [varchar](50) NULL,
	[RoomId] [int] NULL,
	[RoomNumber] [varchar](50) NULL,
	[ServiceName] [varchar](300) NULL,
	[TotalRoomCharge] [decimal](18, 2) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[BPPercentAmount] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[CitySDCharge] [decimal](18, 2) NULL,
	[AdditionalCharge] [decimal](18, 2) NULL,
	[InvoiceRackRate] [decimal](18, 2) NULL,
	[IsServiceChargeEnable] [bit] NULL,
	[InvoiceServiceCharge] [decimal](18, 2) NULL,
	[IsCitySDChargeEnable] [bit] NULL,
	[InvoiceCitySDCharge] [decimal](18, 2) NULL,
	[IsVatAmountEnable] [bit] NULL,
	[InvoiceVatAmount] [decimal](18, 2) NULL,
	[IsAdditionalChargeEnable] [bit] NULL,
	[InvoiceAdditionalCharge] [decimal](18, 2) NULL,
	[CurrencyExchangeRate] [decimal](18, 2) NULL,
	[ReferenceSalesCommission] [decimal](18, 2) NULL,
	[IsBillHoldUp] [bit] NULL,
	[ApprovedStatus] [varchar](50) NULL,
	[TotalCalculatedAmount] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GuestBillApproved] PRIMARY KEY CLUSTERED 
(
	[ApprovedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

--added IsExtraBedCharge column 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestBillApproved' AND column_name = 'IsExtraBedCharge')
BEGIN
	ALTER TABLE dbo.HotelGuestBillApproved
	  ADD IsExtraBedCharge BIT NOT NULL CONSTRAINT DF_HotelGuestBillApproved_IsExtraBedCharge DEFAULT 0 WITH VALUES
END
GO

/****** Object:  Table [dbo].[HotelFloorManagement]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelFloorManagement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelFloorManagement](
	[FloorManagementId] [int] IDENTITY(1,1) NOT NULL,
	[FloorId] [int] NOT NULL,
	[BlockId] [int] NOT NULL,
	[RoomId] [int] NOT NULL,
	[XCoordinate] [float] NULL,
	[YCoordinate] [float] NULL,
	[RoomWidth] [int] NULL,
	[RoomHeight] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HMFloorManagement_1] PRIMARY KEY CLUSTERED 
(
	[FloorManagementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelFloorBlock]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelFloorBlock]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelFloorBlock](
	[BlockId] [int] IDENTITY(1,1) NOT NULL,
	[BlockName] [varchar](100) NULL,
	[BlockDescription] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HMFloorBlock] PRIMARY KEY CLUSTERED 
(
	[BlockId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelFloor]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelFloor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelFloor](
	[FloorId] [int] IDENTITY(1,1) NOT NULL,
	[FloorName] [varchar](100) NULL,
	[FloorDescription] [varchar](500) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HMFloor] PRIMARY KEY CLUSTERED 
(
	[FloorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelEmpTaskAssignment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelEmpTaskAssignment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelEmpTaskAssignment](
	[TaskId] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskSequence] [int] NOT NULL,
	[AssignDate] [date] NULL,
	[Shift] [nvarchar](10) NULL,
	[RoomNumber] [varchar](50) NULL,
	[FloorId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelEmpTaskAssignment] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelDayClose]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelDayClose]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelDayClose](
	[DayCloseId] [int] IDENTITY(1,1) NOT NULL,
	[DayCloseDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelDayClose] PRIMARY KEY CLUSTERED 
(
	[DayCloseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelDailyRoomCondition]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelDailyRoomCondition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelDailyRoomCondition](
	[DailyRoomConditionId] [bigint] IDENTITY(1,1) NOT NULL,
	[RoomId] [int] NOT NULL,
	[RoomConditionId] [bigint] NOT NULL,
	[AssignDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelDailyRoomCondition] PRIMARY KEY CLUSTERED 
(
	[DailyRoomConditionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelCurrencyConversion]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCurrencyConversion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCurrencyConversion](
	[ConversionId] [int] IDENTITY(1,1) NOT NULL,
	[ConversionRateId] [int] NULL,
	[ConversionAmount] [decimal](18, 2) NULL,
	[ConversionRate] [decimal](18, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedByDate] [datetime] NULL,
 CONSTRAINT [PK_HotelCurrencyConversion] PRIMARY KEY CLUSTERED 
(
	[ConversionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelComplementaryItem]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelComplementaryItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelComplementaryItem](
	[ComplementaryItemId] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [varchar](200) NULL,
	[Description] [varchar](300) NULL,
	[ActiveStat] [bit] NULL,
	[IsDefaultItem] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GHComplementaryItem] PRIMARY KEY CLUSTERED 
(
	[ComplementaryItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelCompanyWiseDiscountPolicy]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyWiseDiscountPolicy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyWiseDiscountPolicy](
	[CompanyWiseDiscountId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[RoomTypeId] [int] NOT NULL,
	[DiscountType] [nvarchar](15) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[ActiveStat] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelCompanyWiseDiscountPolicy] PRIMARY KEY CLUSTERED 
(
	[CompanyWiseDiscountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelCompanyPaymentLedgerClosingMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyPaymentLedgerClosingMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyPaymentLedgerClosingMaster](
	[YearClosingId] [bigint] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[ProfitLossClosing] [money] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelCompanyPaymentLedgerClosingMaster] PRIMARY KEY CLUSTERED 
(
	[YearClosingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelCompanyPaymentLedgerClosingDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyPaymentLedgerClosingDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyPaymentLedgerClosingDetails](
	[ClosingBalanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[YearClosingId] [bigint] NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[NodeId] [bigint] NOT NULL,
	[NodeHead] [nvarchar](250) NOT NULL,
	[ClosingDRAmount] [money] NOT NULL,
	[ClosingCRAmount] [money] NOT NULL,
	[ClosingBalance] [money] NOT NULL,
 CONSTRAINT [PK_HotelCompanyPaymentLedgerClosingDetails] PRIMARY KEY CLUSTERED 
(
	[ClosingBalanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelCompanyPaymentLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyPaymentLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyPaymentLedger](
	[CompanyPaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](50) NULL,
	[PaymentType] [nvarchar](15) NOT NULL,
	[PaymentId] [bigint] NULL,
	[LedgerNumber] [varchar](25) NULL,
	[BillId] [int] NULL,
	[BillNumber] [varchar](50) NULL,
	[PaymentDate] [date] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[CurrencyAmount] [money] NULL,
	[PaidAmount] [money] NULL,
	[PaidAmountCurrent] [money] NULL,
	[DueAmount] [money] NULL,
	[AdvanceAmount] [money] NULL,
	[AdvanceAmountRemaining] [money] NULL,
	[DayConvertionRate] [money] NULL,
	[AccountsPostingHeadId] [bigint] NULL,
	[GainOrLossAmount] [money] NULL,
	[RoundedAmount] [money] NULL,
	[ChequeNumber] [nvarchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[PaymentStatus] [varchar](20) NULL,
	[BillGenerationId] [bigint] NULL,
	[RefCompanyPaymentId] [bigint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelCompanyPaymentLedger] PRIMARY KEY CLUSTERED 
(
	[CompanyPaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPaymentLedger' AND column_name = 'ChequeDate')
BEGIN
	ALTER TABLE dbo.HotelCompanyPaymentLedger
		ADD ChequeDate DATETIME NULL
END
GO
/****** Object:  Table [dbo].[HotelCompanyPaymentDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyPaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyPaymentDetails](
	[PaymentDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentId] [bigint] NOT NULL,
	[CompanyBillDetailsId] [bigint] NULL,
	[CompanyPaymentId] [bigint] NOT NULL,
	[BillId] [bigint] NOT NULL,
	[PaymentAmount] [money] NOT NULL,
 CONSTRAINT [PK_HotelCompanyPaymentDetails] PRIMARY KEY CLUSTERED 
(
	[PaymentDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelCompanyPayment]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyPayment](
	[PaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyBillId] [bigint] NULL,
	[PaymentFor] [nvarchar](50) NULL,
	[LedgerNumber] [nvarchar](50) NULL,
	[PaymentDate] [date] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[AdvanceAmount] [money] NULL,
	[Remarks] [nvarchar](250) NULL,
	[PaymentType] [nvarchar](50) NULL,
	[AccountingPostingHeadId] [int] NULL,
	[ChequeNumber] [nvarchar](50) NULL,
	[CurrencyId] [int] NULL,
	[ConvertionRate] [decimal](18, 2) NULL,
	[ApprovedStatus] [nvarchar](50) NULL,
	[AdjustmentType] [nvarchar](50) NULL,
	[CompanyPaymentAdvanceId] [bigint] NULL,
	[AdjustmentAmount] [money] NULL,
	[AdjustmentAccountHeadId] [int] NULL,
	[PaymentAdjustmentAmount] [money] NULL,
 CONSTRAINT [PK_HotelCompanyPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'CreatedBy')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD CreatedBy INT
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'CreatedDate')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD CreatedDate DATETIME
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD LastModifiedBy INT
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'LastModifiedDate')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD LastModifiedDate DATETIME
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'ChequeDate')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD ChequeDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD CheckedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD ApprovedBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD CheckedByUsers VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyPayment' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.HotelCompanyPayment
		ADD ApprovedByUsers VARCHAR(100) NULL
END
GO
/****** Object:  Table [dbo].[HotelCompanyBillGenerationDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyBillGenerationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyBillGenerationDetails](
	[CompanyBillDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyBillId] [bigint] NOT NULL,
	[CompanyPaymentId] [bigint] NOT NULL,
	[BillId] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[PaymentAmount] [money] NULL,
	[DueAmount] [money] NULL,
 CONSTRAINT [PK_HotelCompnayBillGenerationDetails] PRIMARY KEY CLUSTERED 
(
	[CompanyBillDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelCompanyBillGenerationDetails' AND column_name = 'ModuleName')
BEGIN     
   ALTER TABLE dbo.HotelCompanyBillGenerationDetails
	ADD ModuleName VARCHAR(MAX) NULL;
END
GO
/****** Object:  Table [dbo].[HotelCompanyBillGeneration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyBillGeneration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyBillGeneration](
	[CompanyBillId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[BillDate] [date] NOT NULL,
	[CompanyBillNumber] [nvarchar](50) NOT NULL,
	[ApprovedStatus] [nvarchar](50) NULL,
	[BillStatus] [nvarchar](25) NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[BillCurrencyId] [int] NULL,
 CONSTRAINT [PK_HotelCompnayBillGeneration] PRIMARY KEY CLUSTERED 
(
	[CompanyBillId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelAirlineInformation]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelAirlineInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelAirlineInformation](
	[AirlineId] [int] IDENTITY(1,1) NOT NULL,
	[AirlineName] [varchar](200) NULL,
	[FlightNumber] [varchar](50) NULL,
	[AirlineTime] [time](7) NULL,
	[ActiveStat] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelAirlineInformation] PRIMARY KEY CLUSTERED 
(
	[AirlineId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLVoucherApprovedInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLVoucherApprovedInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLVoucherApprovedInfo](
	[ApprovedId] [int] IDENTITY(1,1) NOT NULL,
	[DealId] [int] NULL,
	[ApprovedType] [varchar](50) NULL,
	[UserInfoId] [int] NULL,
 CONSTRAINT [PK_GLVoucherApprovedInfo] PRIMARY KEY CLUSTERED 
(
	[ApprovedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLVoucherApprovedInfo' AND column_name = 'DealId' AND DATA_TYPE = 'int')
BEGIN
	ALTER TABLE dbo.GLVoucherApprovedInfo
    ALTER COLUMN DealId bigint NULL;
END
/****** Object:  Table [dbo].[GLRuleBreak]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLRuleBreak]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLRuleBreak](
	[RuleBreakId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
 CONSTRAINT [PK_GLRuleBreak] PRIMARY KEY CLUSTERED 
(
	[RuleBreakId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GLReportConfigurationDetail]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLReportConfigurationDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLReportConfigurationDetail](
	[RCDetailId] [int] IDENTITY(1,1) NOT NULL,
	[RCId] [int] NULL,
	[NodeId] [int] NULL,
 CONSTRAINT [PK_GLReportConfigurationDetail] PRIMARY KEY CLUSTERED 
(
	[RCDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GLReportConfiguration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLReportConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLReportConfiguration](
	[RCId] [bigint] IDENTITY(1,1) NOT NULL,
	[AncestorId] [bigint] NULL,
	[IsAccountHead] [bit] NULL,
	[NodeId] [int] NULL,
	[NodeNumber] [varchar](50) NULL,
	[NodeHead] [varchar](256) NULL,
	[Lvl] [int] NULL,
	[GroupName] [varchar](100) NULL,
	[ReportType] [varchar](100) NULL,
	[AccountType] [varchar](100) NULL,
	[CalculationType] [varchar](100) NULL,
	[IsActiveLinkUrl] [bit] NULL,
 CONSTRAINT [PK_GLReportConfiguration] PRIMARY KEY CLUSTERED 
(
	[RCId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[GLProjectWiseCostCenterMapping]    Script Date: 7/8/2019 3:34:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLProjectWiseCostCenterMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLProjectWiseCostCenterMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[CostCenterId] [int] NULL,
 CONSTRAINT [PK_GLProjectWiseCostCenterMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[GLProject]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLProject]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLProject](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](150) NULL,
	[ShortName] [varchar](50) NULL,
	[Description] [text] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLProject] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLProject' AND column_name = 'StartDate')
BEGIN
	ALTER TABLE dbo.GLProject
	ADD StartDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLProject' AND column_name = 'EndDate')
BEGIN
	ALTER TABLE dbo.GLProject
	ADD EndDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLProject' AND column_name = 'StageId')
BEGIN
	ALTER TABLE dbo.GLProject
	ADD StageId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLProject' AND column_name = 'ProjectCompanyId')
BEGIN
	ALTER TABLE dbo.GLProject
	ADD ProjectCompanyId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLProject' AND column_name = 'ProjectAmount')
BEGIN
	ALTER TABLE dbo.GLProject
	ADD ProjectAmount DECIMAL(18, 2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLProject' AND column_name = 'IsDefaultProject')
BEGIN
	ALTER TABLE dbo.GLProject
	ADD IsDefaultProject BIT NULL	
END
GO
/****** Object:  Table [dbo].[GLProfitLossSetup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLProfitLossSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLProfitLossSetup](
	[PLSetupId] [int] IDENTITY(1,1) NOT NULL,
	[PLHeadId] [int] NULL,
	[NodeId] [int] NULL,
 CONSTRAINT [PK_GLProfitLossSetup] PRIMARY KEY CLUSTERED 
(
	[PLSetupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GLProfitLossHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLProfitLossHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLProfitLossHead](
	[PLHeadId] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NULL,
	[PLHead] [varchar](250) NULL,
	[NotesNumber] [varchar](20) NULL,
	[CalculationMode] [varchar](20) NULL,
	[DisplayMode] [varchar](20) NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_GLProfitLossHead] PRIMARY KEY CLUSTERED 
(
	[PLHeadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLProfitLossGroupHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLProfitLossGroupHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLProfitLossGroupHead](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupHead] [varchar](250) NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_GLProfitLossGroupHead] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLNotesConfiguration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLNotesConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLNotesConfiguration](
	[ConfigurationId] [int] IDENTITY(1,1) NOT NULL,
	[ConfigurationType] [varchar](50) NULL,
	[NotesNumber] [varchar](20) NULL,
	[NodeId] [int] NULL,
 CONSTRAINT [PK_GLNotesConfiguration] PRIMARY KEY CLUSTERED 
(
	[ConfigurationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

-- =================================================================================================================
/****** Object:  Table [dbo].[GLLedgerMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLLedgerMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLLedgerMaster](
	[LedgerMasterId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[DonorId] [int] NULL,
	[VoucherType] [nvarchar](15) NOT NULL,
	[IsBankExist] [bit] NULL,
	[VoucherNo] [varchar](20) NULL,
	[VoucherDate] [date] NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[Narration] [varchar](500) NULL,
	[PayerOrPayee] [varchar](256) NULL,
	[GLStatus] [varchar](20) NULL,
	[CheckedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[ReferenceNumber] [varchar](50) NULL,
	[FiscalYearId] [int] NULL,
 CONSTRAINT [PK_GLLedgerMaster] PRIMARY KEY CLUSTERED 
(
	[LedgerMasterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'IsOpeningLedger')
BEGIN
	ALTER TABLE dbo.GLLedgerMaster
		ADD IsOpeningLedger BIT NULL CONSTRAINT DF_GLLedgerMaster_IsOpeningLedger DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'ReferenceVoucherNumber')
BEGIN
	ALTER TABLE dbo.GLLedgerMaster
		ADD ReferenceVoucherNumber VARCHAR(20) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'IsModulesTransaction')
BEGIN
	ALTER TABLE dbo.GLLedgerMaster
		ADD IsModulesTransaction BIT NOT NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'IsSynced')
BEGIN
	ALTER TABLE dbo.GLLedgerMaster
		ADD IsSynced BIT NOT NULL CONSTRAINT DF_GLLedgerMaster_IsSynced DEFAULT 0 WITH VALUES;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'IsSynced' AND DATA_TYPE = 'int')
BEGIN
	ALTER TABLE [dbo].[GLLedgerMaster] DROP CONSTRAINT [DF_GLLedgerMaster_IsSynced]
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'IsSynced' AND DATA_TYPE = 'int')
BEGIN
	ALTER TABLE dbo.GLLedgerMaster
		ALTER COLUMN IsSynced BIT NOT NULL 
END
GO
IF NOT EXISTS(SELECT *   FROM sys.all_columns c
					  JOIN sys.tables t on t.object_id = c.object_id
					  JOIN sys.schemas s on s.schema_id = t.schema_id
					  JOIN sys.default_constraints d on c.default_object_id = d.object_id
					WHERE t.name = 'GLLedgerMaster'
					  AND c.name = 'IsSynced'
					  AND s.name = 'dbo')
BEGIN
	ALTER TABLE [dbo].[GLLedgerMaster] ADD CONSTRAINT [DF_GLLedgerMaster_IsSynced] DEFAULT 0 FOR IsSynced;
END
GO

UPDATE GLLedgerMaster
SET GLStatus = 'Pending'
WHERE GLStatus = 'Submit' 

/****** Object:  Table [dbo].[GLLedgerDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLLedgerDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLLedgerDetails](
	[LedgerDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[LedgerMasterId] [bigint] NOT NULL,
	[NodeId] [bigint] NOT NULL,
	[BankAccountId] [int] NULL,
	[ChequeNumber] [varchar](256) NULL,
	[ChequeDate] [date] NULL,
	[LedgerMode] [tinyint] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[NodeNarration] [varchar](500) NULL,
	[CostCenterId] [int] NOT NULL,
	[CurrencyAmount] [money] NULL,
	[NodeType] [nvarchar](25) NULL,
	[ParentId] [bigint] NULL,
	[ParentLedgerId] [bigint] NULL,
 CONSTRAINT [PK_GLLedgerDetails] PRIMARY KEY CLUSTERED 
(
	[LedgerDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLLedger](
	[LedgerId] [int] IDENTITY(1,1) NOT NULL,
	[DealId] [int] NOT NULL,
	[NodeId] [bigint] NOT NULL,
	[LedgerMode] [tinyint] NOT NULL,
	[BankAccountId] [int] NULL,
	[ChequeNumber] [varchar](256) NULL,
	[LedgerAmount] [decimal](18, 2) NOT NULL,
	[NodeNarration] [varchar](500) NULL,
	[CostCenterId] [int] NOT NULL,
	[FieldId] [int] NULL,
	[CurrencyAmount] [decimal](18, 2) NULL,
	[NodeType] [nvarchar](25) NULL,
	[ParentId] [bigint] NULL,
	[ParentLedgerId] [bigint] NULL,
 CONSTRAINT [PK_GLLedger] PRIMARY KEY CLUSTERED 
(
	[LedgerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLGeneralLedger]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLGeneralLedger]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLGeneralLedger](
	[GLId] [bigint] IDENTITY(1,1) NOT NULL,
	[CostCentreId] [int] NULL,
	[CostCentreHead] [varchar](256) NULL,
	[NodeId] [bigint] NULL,
	[NodeHead] [varchar](256) NULL,
	[Lvl] [int] NULL,
	[Hierarchy] [varchar](900) NULL,
	[HierarchyIndex] [varchar](900) NULL,
	[NodeMode] [tinyint] NULL,
	[DealId] [int] NULL,
	[LedgerId] [int] NULL,
	[VoucherDate] [datetime] NULL,
	[VoucherNo] [varchar](12) NULL,
	[LedgerMode] [tinyint] NULL,
	[PriorBalance] [numeric](15, 2) NULL,
	[ReceivedAmount] [numeric](15, 2) NULL,
	[PaidAmount] [numeric](15, 2) NULL,
	[NodeNarration] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PKGLGeneralLedger] PRIMARY KEY CLUSTERED 
(
	[GLId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLFixedAssets]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLFixedAssets]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLFixedAssets](
	[FixedAssetsId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
	[BlockB] [decimal](18, 2) NULL,
	[BlockC] [decimal](18, 2) NULL,
	[BlockD] [decimal](18, 2) NULL,
	[BlockE] [decimal](18, 2) NULL,
	[BlockF] [decimal](18, 2) NULL,
	[BlockG] [decimal](18, 2) NULL,
	[BlockH] [decimal](18, 2) NULL,
	[BlockI] [decimal](18, 2) NULL,
 CONSTRAINT [PK_GLFixedAssets] PRIMARY KEY CLUSTERED 
(
	[FixedAssetsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GLFiscalYearClosingMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLFiscalYearClosingMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLFiscalYearClosingMaster](
	[YearClosingId] [bigint] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[DonorId] [int] NULL,
	[ProfitLossClosing] [money] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLFiscalYearClosingMaster] PRIMARY KEY CLUSTERED 
(
	[YearClosingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GLFiscalYearClosingDetails]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLFiscalYearClosingDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLFiscalYearClosingDetails](
	[ClosingBalanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[YearClosingId] [bigint] NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[DonorId] [int] NULL,
	[NodeId] [bigint] NOT NULL,
	[NodeHead] [nvarchar](250) NOT NULL,
	[ClosingDRAmount] [money] NOT NULL,
	[ClosingCRAmount] [money] NOT NULL,
	[ClosingBalance] [money] NOT NULL,
 CONSTRAINT [PK_GLFiscalYearClosingBalance] PRIMARY KEY CLUSTERED 
(
	[ClosingBalanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[GLFiscalYearProjectMapping]    Script Date: 7/1/2019 5:19:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLFiscalYearProjectMapping]') AND type in (N'U'))
CREATE TABLE [dbo].[GLFiscalYearProjectMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_GLFiscalYearProjectMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[GLFiscalYear]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLFiscalYear]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLFiscalYear](
	[FiscalYearId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[FiscalYearName] [varchar](200) NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IncomeTaxPercentage] [money] NULL,
	[IsFiscalYearClosed] [bit] NULL,
 CONSTRAINT [PK_GLFiscalYear] PRIMARY KEY CLUSTERED 
(
	[FiscalYearId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

--- Migrate ProjectId from GLFiscalYear to GLFiscalYearProjectMapping then drop
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLFiscalYear' AND column_name = 'ProjectId')
BEGIN
	EXEC dbo.sp_executesql @statement = N'INSERT INTO GLFiscalYearProjectMapping
	SELECT  FiscalYearId,
			ProjectId 
	FROM GLFiscalYear

	ALTER TABLE dbo.GLFiscalYear
		DROP COLUMN ProjectId'
END

/****** Object:  Table [dbo].[GLDonor]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLDonor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLDonor](
	[DonorId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](50) NULL,
	[Name] [varchar](150) NOT NULL,
	[ShortName] [varchar](50) NULL,
	[Description] [text] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLDonor] PRIMARY KEY CLUSTERED 
(
	[DonorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLDealMaster]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLDealMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLDealMaster](
	[DealId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[VoucherMode] [tinyint] NULL,
	[BankExistOrNot] [tinyint] NULL,
	[CashChequeMode] [tinyint] NULL,
	[VoucherNo] [varchar](20) NULL,
	[VoucherDate] [datetime] NULL,
	[Narration] [varchar](500) NULL,
	[PayerOrPayee] [varchar](256) NULL,
	[GLStatus] [varchar](20) NULL,
	[CheckedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLMaster] PRIMARY KEY CLUSTERED 
(
	[DealId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLCompany]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLCompany]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLCompany](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](150) NULL,
	[ShortName] [varchar](50) NULL,
	[Description] [text] NULL,
	[IsProfitableOrganization] [bit] NOT NULL,
	[IsManufacturarOrganization] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLCompany] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'InterCompanyTransactionHeadId')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD InterCompanyTransactionHeadId INT NULL

END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'ImageName')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD ImageName [VARCHAR](250) NULL

END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'CompanyAddress')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD CompanyAddress [VARCHAR](MAX) NULL

END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'WebAddress')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD WebAddress [VARCHAR](250) NULL

END
GO
-- add column BinNumber
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'BinNumber')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD BinNumber NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
-- add column TinNumber
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'TinNumber')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD TinNumber NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
-- add column CompanyType
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'CompanyType')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD CompanyType NVARCHAR(250) NULL
END
SET ANSI_PADDING OFF
GO
-- add column BudgetType
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'BudgetType')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD BudgetType NVARCHAR(50) NULL DEFAULT 'Yearly' WITH VALUES
END
SET ANSI_PADDING OFF
GO
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'IsByProductCalculationEnable')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD IsByProductCalculationEnable INT NULL DEFAULT 0 WITH VALUES
END
GO
-- add column Telephone
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'Telephone')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD Telephone NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
-- add column HotLineNumber
 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLCompany' AND column_name = 'HotLineNumber')
BEGIN
	ALTER TABLE dbo.GLCompany
		ADD HotLineNumber NVARCHAR(100) NULL
END
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLCommonSetup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLCommonSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLCommonSetup](
	[SetupId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[TypeName] [varchar](100) NULL,
	[SetupName] [varchar](200) NULL,
	[SetupValue] [text] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLCommonSetup] PRIMARY KEY CLUSTERED 
(
	[SetupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLCashFlowSetup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLCashFlowSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLCashFlowSetup](
	[CFSetupId] [int] IDENTITY(1,1) NOT NULL,
	[HeadId] [int] NULL,
	[NodeId] [int] NULL,
 CONSTRAINT [PK_GLCashFlowSetup] PRIMARY KEY CLUSTERED 
(
	[CFSetupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GLCashFlowHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLCashFlowHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLCashFlowHead](
	[HeadId] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NULL,
	[CashFlowHead] [varchar](250) NULL,
	[NotesNumber] [varchar](20) NULL,
	[VoucherMode] [int] NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_GLCashFlowHead] PRIMARY KEY CLUSTERED 
(
	[HeadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLCashFlowGroupHead]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLCashFlowGroupHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLCashFlowGroupHead](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupHead] [varchar](250) NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_GLCashFlowGroupHead] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLAccountTypeSetup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLAccountTypeSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLAccountTypeSetup](
	[AccountTypeId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
	[AccountType] [varchar](5) NULL,
 CONSTRAINT [PK_GLAccountTypeSetup] PRIMARY KEY CLUSTERED 
(
	[AccountTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLAccountsMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLAccountsMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLAccountsMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[MappingKey] [varchar](500) NULL,
	[MappingValue] [varchar](200) NULL,
	[ActiveStat] [bit] NULL,
 CONSTRAINT [PK_GLAccountsMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GLAccountConfiguration]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLAccountConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLAccountConfiguration](
	[ConfigurationId] [int] IDENTITY(1,1) NOT NULL,
	[AccountType] [varchar](10) NULL,
	[ProjectId] [int] NULL,
	[NodeId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLAccountConfiguration] PRIMARY KEY CLUSTERED 
(
	[ConfigurationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[GetRoomTypeWiseRoomCount]    Script Date: 06/29/2018 12:43:07 ******/
/****** Object:  UserDefinedFunction [dbo].[Get

WiseRoomCount]    Script Date: 06/29/2018 12:43:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRoomTypeWiseRoomCount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		MR
-- Create date: 27-06-2018
-- Name:		GetRoomTypeWiseRoomCount
-- Description:	This function returns total room count for the specific room type
-- =============================================
CREATE FUNCTION [dbo].[GetRoomTypeWiseRoomCount](
@RoomTypeId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @RoomCount INT
	
	SELECT @RoomCount = COUNT(*) FROM HotelRoomNumber WHERE RoomTypeId = @RoomTypeId

	RETURN  @RoomCount
END
' 
END
GO
/****** Object:  View [dbo].[viewComEmployeeInfo]    Script Date: 06/29/2018 12:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewComEmployeeInfo]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[viewComEmployeeInfo]
AS
SELECT     dbo.PayrollEmployee.EmpId, dbo.PayrollEmployee.EmpCode, dbo.PayrollEmployee.DisplayName, dbo.PayrollEmployee.FirstName, dbo.PayrollEmployee.LastName, 
                      dbo.PayrollEmployee.JoinDate, dbo.PayrollEmployee.EmpTypeId, Cat.Name AS EmpType, Cat.Code AS CategoryCode, dbo.PayrollEmployee.DesignationId, Desg.Name AS Designation, 
                      dbo.PayrollEmployee.DepartmentId, Dept.Name AS Department
FROM         dbo.PayrollEmployee LEFT OUTER JOIN
                      dbo.PayrollEmpType AS Cat ON dbo.PayrollEmployee.EmpTypeId = Cat.TypeId LEFT OUTER JOIN
                      dbo.PayrollDesignation AS Desg ON dbo.PayrollEmployee.DesignationId = Desg.DesignationId LEFT OUTER JOIN
                      dbo.PayrollDepartment AS Dept ON dbo.PayrollEmployee.DepartmentId = Dept.DepartmentId
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'viewComEmployeeInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[43] 4[14] 2[24] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PayrollEmployee"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 206
               Right = 218
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cat"
            Begin Extent = 
               Top = 6
               Left = 466
               Bottom = 126
               Right = 644
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Desg"
            Begin Extent = 
               Top = 127
               Left = 600
               Bottom = 247
               Right = 772
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Dept"
            Begin Extent = 
               Top = 6
               Left = 256
               Bottom = 126
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewComEmployeeInfo'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane2' , N'SCHEMA',N'dbo', N'VIEW',N'viewComEmployeeInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewComEmployeeInfo'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'viewComEmployeeInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewComEmployeeInfo'
GO
/****** Object:  View [dbo].[viewChkFromLdg]    Script Date: 06/29/2018 12:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewChkFromLdg]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[viewChkFromLdg]
AS
SELECT DISTINCT LedgerMasterId, ChequeNumber AS VcheqNo
FROM         dbo.GLLedgerDetails
WHERE     (LedgerDetailsId IN
                          (SELECT     LedgerDetailsId
                            FROM          dbo.GLLedgerDetails AS GLLedger_1
                            WHERE      (ChequeNumber IS NOT NULL) AND (ChequeNumber <> '''')))
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'viewChkFromLdg', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "GLLedgerDetails"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 126
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewChkFromLdg'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'viewChkFromLdg', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewChkFromLdg'
GO
/****** Object:  UserDefinedFunction [dbo].[FnNumberToString]    Script Date: 06/29/2018 12:43:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FnNumberToString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE Function [dbo].[FnNumberToString](@iNumber as Decimal(13,3))
	Returns varchar(256)
As
Begin
	Declare @PeriodPosition int
	Declare @FullNumber varchar(10)
	Declare @lclScale varchar(3)
	Declare @rNumToST varchar(256)

	set @PeriodPosition=charindex(char(46),@iNumber)

	set @FullNumber=substring(cast(@iNumber as varchar(25)),1,@PeriodPosition-1)
	set @lclScale=substring(cast(@iNumber as varchar(25)),@PeriodPosition+1,2)

	If len(@FullNumber) between 1 and 2
		set @rNumToST=''Taka ''+ dbo.FnTens(@FullNumber)+case cast(@lclScale as int) 
		when 0 then '''' else '' Paisa ''+ dbo.FnTens(@lclScale) End+'' Only''
	If len(@FullNumber)=3
		set @rNumToST=''Taka ''+case substring(@FullNumber,1,1) when 0 then '''' 
		when 1 then ''One Hundred '' when 2 then ''Two Hundred '' 
		when 3 then ''Three Hundred '' when 4 then ''Four Hundred '' 
		when 5 then ''Five Hundred '' when 6 then ''Six Hundred '' 
		when 7 then ''Seven Hundred '' when 8 then ''Eight Hundred '' 
		when 9 then ''Nine Hundred '' End+dbo.FnTens(right(@FullNumber,2))+
		case cast(@lclScale as int) when 0 then '''' else '' Paisa ''+
		dbo.FnTens(@lclScale) End+'' Only'' 
	If len(@FullNumber)= 4
		set @rNumToST=''Taka ''+dbo.FnTens(substring(@FullNumber,1,1))+
		space(1)+''Thousand ''+case substring(@FullNumber,2,1) 
		when 0 then '''' when 1 then ''One Hundred '' when 2 then ''Two Hundred '' 
		when 3 then ''Three Hundred '' when 4 then ''Four Hundred '' when 5 then 
		''Five Hundred '' when 6 then ''Six Hundred '' when 7 then ''Seven Hundred '' 
		when 8 then ''Eight Hundred '' when 9 then ''Nine Hundred '' End+
		dbo.FnTens(right(@FullNumber,2))+case cast(@lclScale as int) 
		when 0 then '''' else '' Paisa ''+dbo.FnTens(@lclScale) End+'' Only'' 
	If len(@FullNumber)= 5
		set @rNumToST=''Taka ''+dbo.FnTens(substring(@FullNumber,1,2))+
		space(1)+''Thousand ''+case substring(@FullNumber,3,1) when 0 then ''''
		when 1 then ''One Hundred '' when 2 then ''Two Hundred '' 
		when 3 then ''Three Hundred '' when 4 then ''Four Hundred '' 
		when 5 then ''Five Hundred '' when 6 then ''Six Hundred '' 
		when 7 then ''Seven Hundred '' when 8 then ''Eight Hundred ''
		when 9 then ''Nine Hundred '' End+dbo.FnTens(right(@FullNumber,2)) +
		case cast(@lclScale as int) when 0 then '''' else '' Paisa ''+dbo.FnTens(@lclScale) End+
		'' Only''
	If len(@FullNumber)=6 
		set @rNumToST=''Taka ''+dbo.FnTens(substring(@FullNumber,1,1))+
		space(1)+''Lac ''+dbo.FnTens(substring(@FullNumber,2,2))+space(1)+''Thousand ''+
		case substring(@FullNumber,4,1) when 0 then '''' when 1 then ''One Hundred '' 
		when 2 then ''Two Hundred '' when 3 then ''Three Hundred '' when 4 then 
		''Four Hundred '' when 5 then ''Five Hundred '' when 6 then ''Six Hundred '' 
		when 7 then ''Seven Hundred '' when 8 then ''Eight Hundred '' when 9 then 
		''Nine Hundred '' End+dbo.FnTens(right(@FullNumber,2))+case cast(@lclScale as int) 
		when 0 then '''' else '' Paisa ''+dbo.FnTens(@lclScale) End+'' Only'' 
	If len(@FullNumber)=7 
		set @rNumToST=''Taka ''+dbo.FnTens(substring(@FullNumber,1,2))+space(1)+''Lac ''+
		dbo.FnTens(substring(@FullNumber,3,2))+space(1)+''Thousand ''+
		case substring(@FullNumber,5,1) when 0 then '''' when 1 then ''One Hundred '' 
		when 2 then ''Two Hundred '' when 3 then ''Three Hundred '' 
		when 4 then ''Four Hundred '' when 5 then ''Five Hundred ''
		when 6 then ''Six Hundred '' when 7 then ''Seven Hundred '' 
		when 8 then ''Eight Hundred '' when 9 then ''Nine Hundred '' End+
		dbo.FnTens(right(@FullNumber,2))+case cast(@lclScale as int) 
		when 0 then '''' else '' Paisa ''+dbo.FnTens(@lclScale) End+'' Only''
	If len(@FullNumber)=8 
		set @rNumToST=''Taka ''+dbo.FnTens(substring(@FullNumber,1,1))+space(1)+
		''Crore ''+dbo.FnTens(substring(@FullNumber,2,2))+space(1)+''Lac ''+
		dbo.FnTens(substring(@FullNumber,4,2))+space(1)+''Thousand ''+
		case substring(@FullNumber,6,1) when 0 then '''' when 1 then ''One Hundred '' 
		when 2 then ''Two Hundred '' when 3 then ''Three Hundred '' 
		when 4 then ''Four Hundred '' when 5 then ''Five Hundred ''
		when 6 then ''Six Hundred '' when 7 then ''Seven Hundred '' 
		when 8 then ''Eight Hundred '' when 9 then ''Nine Hundred '' End+
		dbo.FnTens(right(@FullNumber,2))+case cast(@lclScale as int) 
		when 0 then '''' else '' Paisa ''+dbo.FnTens(@lclScale) End+'' Only''
	If len(@FullNumber)=9 
		set @rNumToST=''Taka ''+dbo.FnTens(substring(@FullNumber,1,2))+space(1)+
		''Crore ''+	dbo.FnTens(substring(@FullNumber,3,2))+space(1)+''Lac ''+
		dbo.FnTens(substring(@FullNumber,5,2))+space(1)+''Thousand ''+
		case substring(@FullNumber,7,1) when 0 then '''' when 1 then ''One Hundred '' 
		when 2 then ''Two Hundred '' when 3 then ''Three Hundred '' when 4 then ''Four 
		Hundred '' when 5 then ''Five Hundred '' when 6 then ''Six Hundred '' 
		when 7 then ''Seven Hundred '' when 8 then ''Eight Hundred ''
		when 9 then ''Nine Hundred '' End+dbo.FnTens(right(@FullNumber,2))+
		case cast(@lclScale as int) when 0 then '''' else '' Paisa ''+dbo.FnTens(@lclScale) End
		+'' Only''
	If len(@FullNumber)=10 
		set @rNumToST=''Taka ''+case substring(@FullNumber,1,1) 
		when 1 then ''One Hundred '' when 2 then ''Two Hundred '' 
		when 3 then ''Three Hundred '' when 4 then ''Four Hundred '' 
		when 5 then ''Five Hundred '' when 6 then ''Six Hundred '' 
		when 7 then ''Seven Hundred '' when 8 then ''Eight Hundred '' 
		when 9 then ''Nine Hundred '' End+dbo.FnTens(substring(@FullNumber,2,2))
		+space(1)+''Crore ''+	dbo.FnTens(substring(@FullNumber,4,2))+space(1)+
		''Lac ''+dbo.FnTens(substring(@FullNumber,6,2))+space(1)+''Thousand ''+
		case substring(@FullNumber,8,1) when 0 then '''' when 1 then ''One Hundred '' 
		when 2 then ''Two Hundred '' when 3 then ''Three Hundred ''
		when 4 then ''Four Hundred '' when 5 then ''Five Hundred '' 
		when 6 then ''Six Hundred '' when 7 then ''Seven Hundred '' 
		when 8 then ''Eight Hundred '' when 9 then ''Nine Hundred '' End+
		dbo.FnTens(right(@FullNumber,2))+case cast(@lclScale as int) 
		when 0 then '''' else '' Paisa ''+dbo.FnTens(@lclScale) End+'' Only''

	Return @rNumToST
End

' 
END
GO
/****** Object:  Table [dbo].[HotelRoomType]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomType](
	[RoomTypeId] [int] IDENTITY(1,1) NOT NULL,
	[RoomType] [varchar](100) NULL,
	[TypeCode] [varchar](10) NULL,
	[RoomRate] [decimal](18, 2) NULL,
	[RoomRateUSD] [decimal](18, 2) NULL,
	[ActiveStat] [bit] NULL,
	[SuiteType] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[PaxQuantity] [int] NULL,
	[TotalRooms]  AS ([dbo].[GetRoomTypeWiseRoomCount]([RoomTypeId])),
 CONSTRAINT [PK_RoomType] PRIMARY KEY CLUSTERED 
(
	[RoomTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomType' AND column_name = 'AccountsPostingHeadId')
BEGIN
	ALTER TABLE dbo.HotelRoomType
		ADD AccountsPostingHeadId BIGINT NULL;
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomType' AND column_name = 'AccountsPostingHeadId' AND DATA_TYPE = 'int')
BEGIN
	ALTER TABLE dbo.HotelRoomType
		ALTER COLUMN AccountsPostingHeadId BIGINT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomType' AND column_name = 'DisplaySequence')
BEGIN
	ALTER TABLE dbo.HotelRoomType
		ADD DisplaySequence INT  NOT NULL DEFAULT 1 WITH VALUES;	
END
GO
---- Foreign key integration of HotelRoomType with GLNodeMatrix
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_HotelRoomType_GLNodeMatrix')
   AND parent_object_id = OBJECT_ID(N'dbo.HotelRoomType'))
BEGIN
ALTER TABLE [dbo].[HotelRoomType]  WITH CHECK ADD  CONSTRAINT [FK_HotelRoomType_GLNodeMatrix] FOREIGN KEY([AccountsPostingHeadId])
REFERENCES [dbo].[GLNodeMatrix] ([NodeId])

ALTER TABLE [dbo].[HotelRoomType] CHECK CONSTRAINT [FK_HotelRoomType_GLNodeMatrix]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomType' AND column_name = 'MinimumRoomRate')
BEGIN
	ALTER TABLE dbo.HotelRoomType
		ADD MinimumRoomRate DECIMAL(18,2) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelRoomType' AND column_name = 'MinimumRoomRateUSD')
BEGIN
	ALTER TABLE dbo.HotelRoomType
		ADD MinimumRoomRateUSD DECIMAL(18,2) NULL;
END
GO

/****** Object:  View [dbo].[ViewLedgerDetailAmountSum]    Script Date: 06/29/2018 12:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewLedgerDetailAmountSum]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[ViewLedgerDetailAmountSum]
AS
SELECT     TOP (100) PERCENT LedgerMasterId, SUM(DRAmount) AS Amount, dbo.FnNumberToString(SUM(DRAmount)) AS InWordAmount
FROM         dbo.GLLedgerDetails
GROUP BY LedgerMasterId
ORDER BY LedgerMasterId
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'ViewLedgerDetailAmountSum', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "GLLedgerDetails"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 126
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLedgerDetailAmountSum'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'ViewLedgerDetailAmountSum', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLedgerDetailAmountSum'
GO
/****** Object:  View [dbo].[viewCommonVoucher]    Script Date: 06/29/2018 12:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewCommonVoucher]'))
EXEC dbo.sp_executesql @statement = N'/************************************************************
 * Code formatted by SoftTree SQL Assistant © v4.8.24
 * Time: 11/29/2016 5:25:03 PM
 ************************************************************/
CREATE VIEW [dbo].[viewCommonVoucher]
AS
SELECT     gm.LedgerMasterId, glp.CompanyId, glc.Code AS CompanyCode, glc.Name AS CompanyName, gm.ProjectId, glp.Code AS ProjectCode, glp.Name AS ProjectName, gm.VoucherNo, gm.VoucherDate, 
                      gm.Narration, gl.LedgerDetailsId, gl.NodeNarration, gl.NodeId, nm.Hierarchy, nm.Lvl, nm.NodeHead, nm.NodeNumber, gl.ChequeNumber, nm.NodeMode, gl.DRAmount, gl.CRAmount, cfl.VcheqNo, 
                      gm.GLStatus, gm.PayerOrPayee, gl.LedgerMode, gm.DonorId
FROM         dbo.GLProject AS glp LEFT OUTER JOIN
                      dbo.GLCompany AS glc ON glc.CompanyId = glp.CompanyId RIGHT OUTER JOIN
                      dbo.ViewLedgerDetailAmountSum AS ldas RIGHT OUTER JOIN
                      dbo.GLLedgerMaster AS gm LEFT OUTER JOIN
                      dbo.viewChkFromLdg AS cfl ON gm.LedgerMasterId = cfl.LedgerMasterId ON ldas.LedgerMasterId = gm.LedgerMasterId ON glp.ProjectId = gm.ProjectId RIGHT OUTER JOIN
                      dbo.GLNodeMatrix AS nm RIGHT OUTER JOIN
                      dbo.GLLedgerDetails AS gl ON nm.NodeId = gl.NodeId ON gm.LedgerMasterId = gl.LedgerMasterId
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'viewCommonVoucher', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[68] 4[5] 2[22] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "glp"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "glc"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 125
               Right = 834
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ldas"
            Begin Extent = 
               Top = 6
               Left = 248
               Bottom = 125
               Right = 410
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "gm"
            Begin Extent = 
               Top = 6
               Left = 448
               Bottom = 126
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cfl"
            Begin Extent = 
               Top = 126
               Left = 236
               Bottom = 215
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "nm"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "gl"
            Begin Extent = 
               Top = 126
               Left = 434
               Bottom = 356
               Right = 604
            End
            DisplayFlags = 280
            TopColumn = 4
      ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewCommonVoucher'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane2' , N'SCHEMA',N'dbo', N'VIEW',N'viewCommonVoucher', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 23
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewCommonVoucher'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'viewCommonVoucher', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewCommonVoucher'
GO
/****** Object:  Default [DF_BanquetReservation_CompanyId]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_BanquetReservation_CompanyId]') AND parent_object_id = OBJECT_ID(N'[dbo].[BanquetReservation]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BanquetReservation_CompanyId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BanquetReservation] ADD  CONSTRAINT [DF_BanquetReservation_CompanyId]  DEFAULT ((0)) FOR [CompanyId]
END


End
GO
/****** Object:  Default [DF_BanquetReservation_CalculatedDiscountAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_BanquetReservation_CalculatedDiscountAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[BanquetReservation]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BanquetReservation_CalculatedDiscountAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BanquetReservation] ADD  CONSTRAINT [DF_BanquetReservation_CalculatedDiscountAmount]  DEFAULT ((0)) FOR [CalculatedDiscountAmount]
END


End
GO
/****** Object:  Default [DF_BanquetReservation_RegistrationId]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_BanquetReservation_RegistrationId]') AND parent_object_id = OBJECT_ID(N'[dbo].[BanquetReservation]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BanquetReservation_RegistrationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BanquetReservation] ADD  CONSTRAINT [DF_BanquetReservation_RegistrationId]  DEFAULT ((0)) FOR [RegistrationId]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_IsServiceChargeEnable]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_IsServiceChargeEnable]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_IsServiceChargeEnable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_IsServiceChargeEnable]  DEFAULT ((1)) FOR [IsServiceChargeEnable]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_ServiceCharge]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_ServiceCharge]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_ServiceCharge]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_ServiceCharge]  DEFAULT ((0)) FOR [ServiceCharge]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_IsVatEnable1]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_IsVatEnable1]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_IsVatEnable1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_IsVatEnable1]  DEFAULT ((1)) FOR [IsCitySDChargeEnable]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_IsVatEnable]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_IsVatEnable]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_IsVatEnable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_IsVatEnable]  DEFAULT ((1)) FOR [IsVatEnable]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_VatAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_VatAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_VatAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_VatAmount]  DEFAULT ((0)) FOR [VatAmount]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_IsVatSChargeInclusive]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_IsVatSChargeInclusive]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_IsVatSChargeInclusive]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_IsVatSChargeInclusive]  DEFAULT ((1)) FOR [IsVatSChargeInclusive]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_IsVatSChargeInclusive1]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_IsVatSChargeInclusive1]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_IsVatSChargeInclusive1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_IsVatSChargeInclusive1]  DEFAULT ((1)) FOR [IsRatePlusPlus]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_IsAdditionalChargeEnable]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_IsAdditionalChargeEnable]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_IsAdditionalChargeEnable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_IsAdditionalChargeEnable]  DEFAULT ((0)) FOR [IsAdditionalChargeEnable]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_AdditionalChargeAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_AdditionalChargeAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_AdditionalChargeAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_AdditionalChargeAmount]  DEFAULT ((0)) FOR [AdditionalCharge]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_InvoiceTemplate]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_InvoiceTemplate]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_InvoiceTemplate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_InvoiceTemplate]  DEFAULT ((1)) FOR [InvoiceTemplate]
END


End
GO
/****** Object:  Default [DF_CommonCostCenter_BillingStartTime]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCostCenter_BillingStartTime]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCostCenter]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCostCenter_BillingStartTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCostCenter] ADD  CONSTRAINT [DF_CommonCostCenter_BillingStartTime]  DEFAULT ((-1)) FOR [BillingStartTime]
END


End
GO
/****** Object:  Default [DF_CommonCurrency_CurrencyType]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CommonCurrency_CurrencyType]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommonCurrency]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CommonCurrency_CurrencyType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CommonCurrency] ADD  CONSTRAINT [DF_CommonCurrency_CurrencyType]  DEFAULT ('Other') FOR [CurrencyType]
END


End
GO
/****** Object:  Default [DF_GLCompany_IsProfitableOrganization]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_GLCompany_IsProfitableOrganization]') AND parent_object_id = OBJECT_ID(N'[dbo].[GLCompany]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GLCompany_IsProfitableOrganization]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GLCompany] ADD  CONSTRAINT [DF_GLCompany_IsProfitableOrganization]  DEFAULT ((0)) FOR [IsProfitableOrganization]
END


End
GO
/****** Object:  Default [DF__NodeMatri__NodeM__05D8E0BE]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__NodeMatri__NodeM__05D8E0BE]') AND parent_object_id = OBJECT_ID(N'[dbo].[GLNodeMatrix]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__NodeMatri__NodeM__05D8E0BE]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GLNodeMatrix] ADD  CONSTRAINT [DF__NodeMatri__NodeM__05D8E0BE]  DEFAULT ((1)) FOR [NodeMode]
END


End
GO
/****** Object:  Default [DF_GLNodeMatrix_IsTransactionalHead]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_GLNodeMatrix_IsTransactionalHead]') AND parent_object_id = OBJECT_ID(N'[dbo].[GLNodeMatrix]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GLNodeMatrix_IsTransactionalHead]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GLNodeMatrix] ADD  CONSTRAINT [DF_GLNodeMatrix_IsTransactionalHead]  DEFAULT ((1)) FOR [IsTransactionalHead]
END


End
GO
/****** Object:  Default [DF_HotelGuestBillApproved_UnitPrice]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestBillApproved_UnitPrice]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestBillApproved]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestBillApproved_UnitPrice]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestBillApproved] ADD  CONSTRAINT [DF_HotelGuestBillApproved_UnitPrice]  DEFAULT ((0)) FOR [TotalRoomCharge]
END


End
GO
/****** Object:  Default [DF_HotelGuestBillApproved_IsBillHoldUp]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestBillApproved_IsBillHoldUp]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestBillApproved]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestBillApproved_IsBillHoldUp]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestBillApproved] ADD  CONSTRAINT [DF_HotelGuestBillApproved_IsBillHoldUp]  DEFAULT ((0)) FOR [IsBillHoldUp]
END


End
GO
/****** Object:  Default [DF_HotelGuestBillApproved_TotalCalculatedAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestBillApproved_TotalCalculatedAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestBillApproved]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestBillApproved_TotalCalculatedAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestBillApproved] ADD  CONSTRAINT [DF_HotelGuestBillApproved_TotalCalculatedAmount]  DEFAULT ((0)) FOR [TotalCalculatedAmount]
END


End
GO
/****** Object:  Default [DF_GuestCompany_DiscountPercent]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_GuestCompany_DiscountPercent]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestCompany]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GuestCompany_DiscountPercent]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestCompany] ADD  CONSTRAINT [DF_GuestCompany_DiscountPercent]  DEFAULT ((0)) FOR [DiscountPercent]
END


End
GO
/****** Object:  Default [DF_HotelGuestCompany_CreditLimit]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestCompany_CreditLimit]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestCompany]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestCompany_CreditLimit]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestCompany] ADD  CONSTRAINT [DF_HotelGuestCompany_CreditLimit]  DEFAULT ((0)) FOR [CreditLimit]
END


End
GO
/****** Object:  Default [DF_HotelGuestCompany_Balance]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestCompany_Balance]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestCompany]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestCompany_Balance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestCompany] ADD  CONSTRAINT [DF_HotelGuestCompany_Balance]  DEFAULT ((0)) FOR [Balance]
END


End
GO
/****** Object:  Default [DF_GuestHouseCheckOut_TotalAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_GuestHouseCheckOut_TotalAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestHouseCheckOut]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GuestHouseCheckOut_TotalAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestHouseCheckOut] ADD  CONSTRAINT [DF_GuestHouseCheckOut_TotalAmount]  DEFAULT ((0)) FOR [TotalAmount]
END


End
GO
/****** Object:  Default [DF_HotelGuestReservation_RoomId]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestReservation_RoomId]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestReservation]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestReservation_RoomId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestReservation] ADD  CONSTRAINT [DF_HotelGuestReservation_RoomId]  DEFAULT ((0)) FOR [RoomId]
END


End
GO
/****** Object:  Default [DF_HotelGuestReservationOnline_RoomId]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestReservationOnline_RoomId]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestReservationOnline]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestReservationOnline_RoomId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestReservationOnline] ADD  CONSTRAINT [DF_HotelGuestReservationOnline_RoomId]  DEFAULT ((0)) FOR [RoomId]
END


End
GO
/****** Object:  Default [DF_HotelGuestServiceBill_TotalCalculatedAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelGuestServiceBill_TotalCalculatedAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelGuestServiceBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelGuestServiceBill_TotalCalculatedAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelGuestServiceBill] ADD  CONSTRAINT [DF_HotelGuestServiceBill_TotalCalculatedAmount]  DEFAULT ((0)) FOR [TotalCalculatedAmount]
END


End
GO
/****** Object:  Default [DF_OnlineReservationDetail_IsRegistered]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_OnlineReservationDetail_IsRegistered]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelOnlineRoomReservationDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OnlineReservationDetail_IsRegistered]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelOnlineRoomReservationDetail] ADD  CONSTRAINT [DF_OnlineReservationDetail_IsRegistered]  DEFAULT ((0)) FOR [IsRegistered]
END


End
GO
/****** Object:  Default [DF_HotelRoomNumber_IsSmokingRoom]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelRoomNumber_IsSmokingRoom]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelRoomNumber]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelRoomNumber_IsSmokingRoom]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelRoomNumber] ADD  CONSTRAINT [DF_HotelRoomNumber_IsSmokingRoom]  DEFAULT ((0)) FOR [IsSmokingRoom]
END


End
GO
/****** Object:  Default [DF_HotelRoomRegistration_NoShowCharge]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelRoomRegistration_NoShowCharge]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelRoomRegistration]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelRoomRegistration_NoShowCharge]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelRoomRegistration] ADD  CONSTRAINT [DF_HotelRoomRegistration_NoShowCharge]  DEFAULT ((0)) FOR [NoShowCharge]
END


End
GO
/****** Object:  Default [DF_HotelRoomRegistration_IsBlankRegistrationCard]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelRoomRegistration_IsBlankRegistrationCard]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelRoomRegistration]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelRoomRegistration_IsBlankRegistrationCard]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelRoomRegistration] ADD  CONSTRAINT [DF_HotelRoomRegistration_IsBlankRegistrationCard]  DEFAULT ((0)) FOR [IsBlankRegistrationCard]
END


End
GO
/****** Object:  Default [DF_ReservationDetail_IsRegistered]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ReservationDetail_IsRegistered]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelRoomReservationDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ReservationDetail_IsRegistered]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelRoomReservationDetail] ADD  CONSTRAINT [DF_ReservationDetail_IsRegistered]  DEFAULT ((0)) FOR [IsRegistered]
END


End
GO
/****** Object:  Default [DF_ReservationDetailOnline_IsRegistered]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ReservationDetailOnline_IsRegistered]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelRoomReservationDetailOnline]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ReservationDetailOnline_IsRegistered]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelRoomReservationDetailOnline] ADD  CONSTRAINT [DF_ReservationDetailOnline_IsRegistered]  DEFAULT ((0)) FOR [IsRegistered]
END


End
GO
/****** Object:  Default [DF_HotelRoomStatusPossiblePathHead_ActiveStat]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_HotelRoomStatusPossiblePathHead_ActiveStat]') AND parent_object_id = OBJECT_ID(N'[dbo].[HotelRoomStatusPossiblePathHead]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_HotelRoomStatusPossiblePathHead_ActiveStat]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HotelRoomStatusPossiblePathHead] ADD  CONSTRAINT [DF_HotelRoomStatusPossiblePathHead_ActiveStat]  DEFAULT ((1)) FOR [ActiveStat]
END


End
GO
/****** Object:  Default [DF_InvCategory_ShowInInvoice]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_InvCategory_ShowInInvoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvCategory]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_InvCategory_ShowInInvoice]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InvCategory] ADD  CONSTRAINT [DF_InvCategory_ShowInInvoice]  DEFAULT ((1)) FOR [ShowInInvoice]
END


End
GO
/****** Object:  Default [DF_PMProductCategory_NodeMode]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_PMProductCategory_NodeMode]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvCategory]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PMProductCategory_NodeMode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InvCategory] ADD  CONSTRAINT [DF_PMProductCategory_NodeMode]  DEFAULT ((0)) FOR [ActiveStat]
END


End
GO
/****** Object:  Default [DF_InvItem_AdjustmentFrequency]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_InvItem_AdjustmentFrequency]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvItem]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_InvItem_AdjustmentFrequency]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InvItem] ADD  CONSTRAINT [DF_InvItem_AdjustmentFrequency]  DEFAULT ('Daily') FOR [AdjustmentFrequency]
END


End
GO
/****** Object:  Default [DF_InvItemTransactionDetails_IsCustomerItem]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_InvItemTransactionDetails_IsCustomerItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvItemTransactionDetails]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_InvItemTransactionDetails_IsCustomerItem]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InvItemTransactionDetails] ADD  CONSTRAINT [DF_InvItemTransactionDetails_IsCustomerItem]  DEFAULT ((0)) FOR [IsCustomerItem]
END


End
GO
/****** Object:  Default [DF_InvLocation_NodeMode]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_InvLocation_NodeMode]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvLocation]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_InvLocation_NodeMode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InvLocation] ADD  CONSTRAINT [DF_InvLocation_NodeMode]  DEFAULT ((0)) FOR [ActiveStat]
END


End
GO
/****** Object:  Default [DF_MemMemberType_DiscountPercent]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_MemMemberType_DiscountPercent]') AND parent_object_id = OBJECT_ID(N'[dbo].[MemMemberType]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_MemMemberType_DiscountPercent]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[MemMemberType] ADD  CONSTRAINT [DF_MemMemberType_DiscountPercent]  DEFAULT ((0)) FOR [DiscountPercent]
END


End
GO
/****** Object:  Default [DF_PayrollLeaveType_CarryForward]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_PayrollLeaveType_CarryForward]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayrollLeaveType]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PayrollLeaveType_CarryForward]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PayrollLeaveType] ADD  CONSTRAINT [DF_PayrollLeaveType_CarryForward]  DEFAULT ((0)) FOR [MaxDayCanCarryForwardYearly]
END


End
GO
/****** Object:  Default [DF__PayrollSa__IsBas__57FF697A]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__PayrollSa__IsBas__57FF697A]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayrollSalaryFormula]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__PayrollSa__IsBas__57FF697A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PayrollSalaryFormula] ADD  CONSTRAINT [DF__PayrollSa__IsBas__57FF697A]  DEFAULT ((0)) FOR [IsBasicOrGross]
END


End
GO
/****** Object:  Default [DF_PMProductReceived_AdhocSupplierId]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_PMProductReceived_AdhocSupplierId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMProductReceived]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PMProductReceived_AdhocSupplierId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PMProductReceived] ADD  CONSTRAINT [DF_PMProductReceived_AdhocSupplierId]  DEFAULT ((0)) FOR [SupplierId]
END


End
GO
/****** Object:  Default [DF_PMPurchaseOrder_IsLocalOrForeignPO]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_PMPurchaseOrder_IsLocalOrForeignPO]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMPurchaseOrder]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PMPurchaseOrder_IsLocalOrForeignPO]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PMPurchaseOrder] ADD  CONSTRAINT [DF_PMPurchaseOrder_IsLocalOrForeignPO]  DEFAULT ('Local') FOR [IsLocalOrForeignPO]
END


End
GO
/****** Object:  Default [DF_PMRequisitionDetails_DelivarOutQuantity]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_PMRequisitionDetails_DelivarOutQuantity]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMRequisitionDetails]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PMRequisitionDetails_DelivarOutQuantity]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PMRequisitionDetails] ADD  CONSTRAINT [DF_PMRequisitionDetails_DelivarOutQuantity]  DEFAULT ((0)) FOR [DelivarOutQuantity]
END

End
GO
/****** Object:  Default [DF_PMSupplier_Balance]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_PMSupplier_Balance]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMSupplier]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PMSupplier_Balance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PMSupplier] ADD  CONSTRAINT [DF_PMSupplier_Balance]  DEFAULT ((0)) FOR [Balance]
END


End
GO
/****** Object:  Default [DF_PMSupplierPaymentLedger_IsBillGenerated]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_PMSupplierPaymentLedger_IsBillGenerated]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMSupplierPaymentLedger]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PMSupplierPaymentLedger_IsBillGenerated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PMSupplierPaymentLedger] ADD  CONSTRAINT [DF_PMSupplierPaymentLedger_IsBillGenerated]  DEFAULT ((0)) FOR [IsBillGenerated]
END


End
GO
/****** Object:  Default [DF_RestaurantBearer_IsRestaurantBillCanSettle]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBearer_IsRestaurantBillCanSettle]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBearer]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBearer_IsRestaurantBillCanSettle]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBearer] ADD  CONSTRAINT [DF_RestaurantBearer_IsRestaurantBillCanSettle]  DEFAULT ((1)) FOR [IsRestaurantBillCanSettle]
END


End
GO
/****** Object:  Default [DF_RestaurantBearer_IsItemCanEditDelete]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBearer_IsItemCanEditDelete]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBearer]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBearer_IsItemCanEditDelete]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBearer] ADD  CONSTRAINT [DF_RestaurantBearer_IsItemCanEditDelete]  DEFAULT ((0)) FOR [IsItemCanEditDelete]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_PayModeCurrentBalance]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_PayModeCurrentBalance]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_PayModeCurrentBalance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_PayModeCurrentBalance]  DEFAULT ((0)) FOR [PaySourceCurrentBalance]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_DiscountAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_DiscountAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_DiscountAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_DiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_CalculatedDiscountAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_CalculatedDiscountAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_CalculatedDiscountAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_CalculatedDiscountAmount]  DEFAULT ((0)) FOR [CalculatedDiscountAmount]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_ServiceCharge]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_ServiceCharge]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_ServiceCharge]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_ServiceCharge]  DEFAULT ((0)) FOR [ServiceCharge]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_VatAmount2]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_VatAmount2]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_VatAmount2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_VatAmount2]  DEFAULT ((0)) FOR [CitySDCharge]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_VatAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_VatAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_VatAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_VatAmount]  DEFAULT ((0)) FOR [VatAmount]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_VatAmount1]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_VatAmount1]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_VatAmount1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_VatAmount1]  DEFAULT ((0)) FOR [AdditionalCharge]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_GrandTotal]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_GrandTotal]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_GrandTotal]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_GrandTotal]  DEFAULT ((0)) FOR [GrandTotal]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_IsActive]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_IsActive]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_IsActive]  DEFAULT ((1)) FOR [IsActive]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_IsDeleted]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  Default [DF_RestaurantBill_IsLocked]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantBill_IsLocked]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantBill]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantBill_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantBill] ADD  CONSTRAINT [DF_RestaurantBill_IsLocked]  DEFAULT ((0)) FOR [IsLocked]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_ItemUnit]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_ItemUnit]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_ItemUnit]  DEFAULT ((0)) FOR [ItemUnit]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_UnitRate]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_UnitRate]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_UnitRate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_UnitRate]  DEFAULT ((0)) FOR [UnitRate]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_Amount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_Amount]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_Amount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_Amount]  DEFAULT ((0)) FOR [Amount]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_DiscountedAmount1]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_DiscountedAmount1]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_DiscountedAmount1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_DiscountedAmount1]  DEFAULT ((0)) FOR [DiscountAmount]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_DiscountedAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_DiscountedAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_DiscountedAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_DiscountedAmount]  DEFAULT ((0)) FOR [DiscountedAmount]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_ServiceRate]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_ServiceRate]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_ServiceRate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_ServiceRate]  DEFAULT ((0)) FOR [ServiceRate]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_ServiceCharge]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_ServiceCharge]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_ServiceCharge]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_ServiceCharge]  DEFAULT ((0)) FOR [ServiceCharge]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_VatAmount]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_VatAmount]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_VatAmount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_VatAmount]  DEFAULT ((0)) FOR [VatAmount]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_CitySDCharge]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_CitySDCharge]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_CitySDCharge]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_CitySDCharge]  DEFAULT ((0)) FOR [CitySDCharge]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_AdditionalCharge]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_AdditionalCharge]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_AdditionalCharge]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_AdditionalCharge]  DEFAULT ((0)) FOR [AdditionalCharge]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_PrintFlag]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_PrintFlag]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_PrintFlag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_PrintFlag]  DEFAULT ((0)) FOR [PrintFlag]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_IsChanged]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_IsChanged]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_IsChanged]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_IsChanged]  DEFAULT ((0)) FOR [IsChanged]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_IsDeleted]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  Default [DF_RestaurantKotBillDetail_IsDispatch]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillDetail_IsDispatch]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillDetail]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillDetail_IsDispatch]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillDetail] ADD  CONSTRAINT [DF_RestaurantKotBillDetail_IsDispatch]  DEFAULT ((0)) FOR [IsDispatch]
END
End
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'InvoiceDiscount')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
		ADD InvoiceDiscount [decimal](18, 2) NULL;
END
GO




/****** Object:  Default [DF_RestaurantKotBillMaster_PaxQuantity]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantKotBillMaster_PaxQuantity]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantKotBillMaster]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantKotBillMaster_PaxQuantity]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantKotBillMaster] ADD  CONSTRAINT [DF_RestaurantKotBillMaster_PaxQuantity]  DEFAULT ((1)) FOR [PaxQuantity]
END


End
GO
/****** Object:  Default [DF_RestaurantToken_IsBillHoldup]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_RestaurantToken_IsBillHoldup]') AND parent_object_id = OBJECT_ID(N'[dbo].[RestaurantToken]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RestaurantToken_IsBillHoldup]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RestaurantToken] ADD  CONSTRAINT [DF_RestaurantToken_IsBillHoldup]  DEFAULT ((0)) FOR [IsBillHoldup]
END


End
GO
/****** Object:  Default [DF_SecurityMenuGroup_ActiveStat]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SecurityMenuGroup_ActiveStat]') AND parent_object_id = OBJECT_ID(N'[dbo].[SecurityMenuGroup]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SecurityMenuGroup_ActiveStat]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SecurityMenuGroup] ADD  CONSTRAINT [DF_SecurityMenuGroup_ActiveStat]  DEFAULT ((0)) FOR [ActiveStat]
END


End
GO
/****** Object:  Default [DF_SecurityMenuLinks_ActiveStat]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SecurityMenuLinks_ActiveStat]') AND parent_object_id = OBJECT_ID(N'[dbo].[SecurityMenuLinks]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SecurityMenuLinks_ActiveStat]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SecurityMenuLinks] ADD  CONSTRAINT [DF_SecurityMenuLinks_ActiveStat]  DEFAULT ((0)) FOR [ActiveStat]
END


End
GO
/****** Object:  Default [DF_SecurityMenuWiseLinks_IsSavePermission]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SecurityMenuWiseLinks_IsSavePermission]') AND parent_object_id = OBJECT_ID(N'[dbo].[SecurityMenuWiseLinks]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SecurityMenuWiseLinks_IsSavePermission]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SecurityMenuWiseLinks] ADD  CONSTRAINT [DF_SecurityMenuWiseLinks_IsSavePermission]  DEFAULT ((0)) FOR [IsSavePermission]
END


End
GO
/****** Object:  Default [DF_SecurityMenuWiseLinks_IsDeletePermission]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SecurityMenuWiseLinks_IsDeletePermission]') AND parent_object_id = OBJECT_ID(N'[dbo].[SecurityMenuWiseLinks]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SecurityMenuWiseLinks_IsDeletePermission]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SecurityMenuWiseLinks] ADD  CONSTRAINT [DF_SecurityMenuWiseLinks_IsDeletePermission]  DEFAULT ((0)) FOR [IsDeletePermission]
END


End
GO
/****** Object:  Default [DF_SecurityMenuWiseLinks_IsViewPermission]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SecurityMenuWiseLinks_IsViewPermission]') AND parent_object_id = OBJECT_ID(N'[dbo].[SecurityMenuWiseLinks]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SecurityMenuWiseLinks_IsViewPermission]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SecurityMenuWiseLinks] ADD  CONSTRAINT [DF_SecurityMenuWiseLinks_IsViewPermission]  DEFAULT ((0)) FOR [IsViewPermission]
END


End
GO
/****** Object:  Default [DF_SecurityMenuWiseLinks_ActiveStat]    Script Date: 06/29/2018 12:43:03 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SecurityMenuWiseLinks_ActiveStat]') AND parent_object_id = OBJECT_ID(N'[dbo].[SecurityMenuWiseLinks]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SecurityMenuWiseLinks_ActiveStat]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SecurityMenuWiseLinks] ADD  CONSTRAINT [DF_SecurityMenuWiseLinks_ActiveStat]  DEFAULT ((0)) FOR [ActiveStat]
END

End
GO

/****** Object:  Table [dbo].[HotelDayProcessing]    Script Date: 07/02/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelDayProcessing]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelDayProcessing](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProcessType] [varchar](100) NULL,
	[ProcessDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelDayProcessing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[GLBudget]    Script Date: 07/05/2018 14:47:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLBudget]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLBudget](
	[BudgetId] [bigint] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [bigint] NOT NULL,
	[CheckedBy] [bigint] NULL,
	[ApprovedBy] [bigint] NULL,
	[ApprovedStatus] [nvarchar](25) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLBudget] PRIMARY KEY CLUSTERED 
(
	[BudgetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLBudget' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.GLBudget
		ADD CompanyId INT NOT NULL CONSTRAINT DF_GLBudget_CompanyId DEFAULT 0 WITH VALUES;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLBudget' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.GLBudget
		ADD ProjectId INT NOT NULL CONSTRAINT DF_GLBudget_ProjectId DEFAULT 0 WITH VALUES;
END
GO

/****** Object:  Table [dbo].[GLBudgetDetails]    Script Date: 07/05/2018 14:48:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLBudgetDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GLBudgetDetails](
	[BudgetDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[BudgetId] [bigint] NOT NULL,
	[MonthId] [smallint] NOT NULL,
	[NodeId] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
 CONSTRAINT [PK_GLBudgetDetails] PRIMARY KEY CLUSTERED 
(
	[BudgetDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[CommonReportConfigMaster]    Script Date: 7/17/2018 8:02:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonReportConfigMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonReportConfigMaster](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReportTypeId] [int] NOT NULL,
	[AncestorId] [bigint] NULL,
	[Caption] [nvarchar](350) NOT NULL,
	[SortingOrder] [smallint] NOT NULL,
	[Lvl] [bigint] NULL,
	[Hierarchy] [varchar](max) NULL,
	[HierarchyIndex] [bigint] NULL,
	[IsParent] [bit] NULL,
	[NodeType] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GLReprtConfigMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[CommonReportConfigDetails]    Script Date: 7/17/2018 8:04:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonReportConfigDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonReportConfigDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReportConfigId] [bigint] NOT NULL,
	[NodeId] [bigint] NOT NULL,
	[NodeName] [nvarchar](450) NULL,
	[SortingOrder] [smallint] NOT NULL,
 CONSTRAINT [PK_GLReportConfigDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'PaymentRemarks')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD PaymentRemarks NVARCHAR(500) NULL;
END
GO

/*=================================================================== 
  Author				: FA
  Create date			: 03/09/2018 
  Name					: CommonCheckedByApprovedBy
  Description			: 
 ===================================================================*/

/****** Object:  Table [dbo].[CommonCheckedByApprovedBy]    Script Date: 03/09/2018 7:18:08 PM ******/



/****** Object:  Table [dbo].[CommonCheckedByApprovedBy]    Script Date: 03/09/2018 7:18:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonCheckedByApprovedBy]') AND type in (N'U'))
CREATE TABLE [dbo].[CommonCheckedByApprovedBy](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FeaturesId] [bigint] NULL,
	[UserInfoId] [bigint] NULL,
	[IsCheckedBy] [bit] NULL,
	[IsApprovedBy] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonCheckedByApprovedBy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItem' AND column_name = 'LastPurchaseDate')
BEGIN
	ALTER TABLE dbo.InvItem
		ADD LastPurchaseDate DATE NULL;
END
GO

/*=================================================================== 
  Author				: FA
  Create date			: 14/09/2018 
  Name					: HotelRoomFeatures
  Description			: Create table
 ===================================================================*/

/****** Object:  Table [dbo].[HotelRoomFeatures]    Script Date: 06/09/2018 3:57:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomFeatures]') AND type in (N'U'))
CREATE TABLE [dbo].[HotelRoomFeatures](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Features] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ActiveStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelRoomFeatures] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/*=================================================================== 
  Author				: FA
  Create date			: 14/09/2018 
  Name					: HotelRoomFeaturesInfo
  Description			: Create table
 ===================================================================*/

/****** Object:  Table [dbo].[HotelRoomFeaturesInfo]    Script Date: 14/09/2018 5:33:07 PM ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomFeaturesInfo]') AND type in (N'U'))
CREATE TABLE [dbo].[HotelRoomFeaturesInfo](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoomId] [bigint] NOT NULL,
	[FeaturesId] [bigint] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelRoomFeaturesInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[HotelLinkedRoomMaster]    Script Date: 9/18/2018 3:47:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelLinkedRoomMaster]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[HotelLinkedRoomMaster](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [bigint] NOT NULL,
	[LinkName] [nvarchar](50) NULL,
	[Description] [nvarchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelLinkedRoomMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[HotelLinkedRoomDetails]    Script Date: 9/18/2018 3:54:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelLinkedRoomDetails]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[HotelLinkedRoomDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MasterId] [bigint] NOT NULL,
	[RegistrationId] [bigint] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelLinkedRoomDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[HotelLinkedRoomDetails]  WITH CHECK ADD  CONSTRAINT [FK_HotelLinkedRoomDetails_HotelLinkedRoomDetails] FOREIGN KEY([MasterId])
REFERENCES [dbo].[HotelLinkedRoomMaster] ([Id])

ALTER TABLE [dbo].[HotelLinkedRoomDetails] CHECK CONSTRAINT [FK_HotelLinkedRoomDetails_HotelLinkedRoomDetails]

END
GO

/****** Object:  Table [dbo].[GatePass]    Script Date: 9/20/2018 4:17:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatePass]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatePass](
	[GatePassId] [bigint] IDENTITY(1,1) NOT NULL,
	[GatePassNumber] [varchar](20) NULL,
	[GatePassDate] [datetime] NOT NULL,
	[SupplierId] [int] NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[ResponsiblePersonId] [int] NULL,
	[ApprovedBy] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CheckedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[CheckedDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_GatePass] PRIMARY KEY CLUSTERED 
(
	[GatePassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[GatePassDetails]    Script Date: 9/20/2018 4:18:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatePassDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatePassDetails](
	[GatePassItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[GatePassId] [bigint] NULL,
	[CostCenterId] [int] NULL,
	[ItemId] [int] NULL,
	[StockById] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Description] [nvarchar](250) NULL,
	[ReturnType] [int] NULL,
	[ReturnDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[ApprovedQuantity] [decimal](18, 0) NULL,
 CONSTRAINT [PK_GatePassDetails] PRIMARY KEY CLUSTERED 
(
	[GatePassItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[HotelGuestBirthdayNotification]    Script Date: 9/30/2018 3:45:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestBirthdayNotification]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelGuestBirthdayNotification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GuestId] [bigint] NOT NULL,
	[Date] [date] NOT NULL,
	[IsEmailSent] [bit] NOT NULL,
	[IsSmsSent] [bit] NOT NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [bigint] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelGuestBirthdayNotification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[HotelGuestBirthdayNotification] ADD  CONSTRAINT [DF_HotelGuestBirthdayNotification_IsEmailSent]  DEFAULT ((0)) FOR [IsEmailSent]

ALTER TABLE [dbo].[HotelGuestBirthdayNotification] ADD  CONSTRAINT [DF_HotelGuestBirthdayNotification_IsSmsSent]  DEFAULT ((0)) FOR [IsSmsSent]
END
GO


/****** Object:  Table [dbo].[PMProductOutSerialInfo]    Script Date: 10/10/2018 6:40:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductOutSerialInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductOutSerialInfo](
	[OutSerialId] [bigint] IDENTITY(1,1) NOT NULL,
	[OutId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[SerialNumber] [varchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutSerialInfo' AND column_name = 'ReturnOutStatus')
BEGIN
	ALTER TABLE dbo.PMProductOutSerialInfo
		ADD ReturnOutStatus NVARCHAR(25) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductOutSerialInfo' AND column_name = 'IsFAItemReturn')
BEGIN
	ALTER TABLE dbo.PMProductOutSerialInfo
		ADD IsFAItemReturn BIT NOT NULL CONSTRAINT DF_PMProductOutSerialInfo_IsFAItemReturn DEFAULT 0 WITH VALUES
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvItemStockSerialInformation]    Script Date: 11/10/2018 10:30:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockSerialInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemStockSerialInformation](
	[SerialStockId] [bigint] IDENTITY(1,1) NOT NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NULL,
	[SerialNumber] [varchar](50) NULL,
	[SerialStatus] [varchar](25) NULL

 CONSTRAINT [PK_InvItemStockSerialInformation] PRIMARY KEY CLUSTERED 
(
	[SerialStockId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockSerialInformation' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.InvItemStockSerialInformation
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockSerialInformation' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.InvItemStockSerialInformation
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO
/****** Object:  Table [dbo].[HotelManagerReportInfo]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelManagerReportInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelManagerReportInfo](
	[SummaryId] [bigint] IDENTITY(1,1) NOT NULL,
	[SummaryDate] [datetime] NULL,
	[OrderByNumber] [int] NULL,
	[ServiceType] [nvarchar](MAX) NULL,
	[ServiceName] [nvarchar](MAX) NULL,
	[Covers] DECIMAL(18, 2),
 CONSTRAINT [PK_HotelManagerReportInfo] PRIMARY KEY CLUSTERED 
(
	[SummaryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantResettlementLog]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantResettlementLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantResettlementLog](
	[ResettlementHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[BillId] [int] NOT NULL,
	[KotId] [int] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[BillNumber] [varchar](50) NOT NULL,
	[ResettlementDate] [datetime] NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemName] [varchar](300) NOT NULL,
	[ItemUnit] [decimal](18, 5) NOT NULL,
	[UnitRate] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CalculatedDiscountAmount] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_RestaurantResettlementLog] PRIMARY KEY CLUSTERED 
(
	[ResettlementHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanquetResettlementLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BanquetResettlementLog](
	[ResettlementHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservationId] [bigint] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[ReservationNumber] [varchar](50) NOT NULL,
	[ResettlementDate] [datetime] NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemName] [varchar](300) NOT NULL,
	[ItemUnit] [decimal](18, 5) NOT NULL,
	[UnitRate] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CalculatedDiscountAmount] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_BanquetResettlementLog] PRIMARY KEY CLUSTERED 
(
	[ResettlementHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


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

/****** Object:  Table [dbo].[HotelRoomReservationSummary]    Script Date: 12/05/2018 15:35:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomReservationSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRoomReservationSummary](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservationId] [bigint] NULL,
	[RoomTypeId] [int] NULL,
	[RoomQuantity] [int] NULL,
	[PaxQuantity] [int] NULL,
 CONSTRAINT [PK_HotelRoomReservationSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HotelRoomReservationRoomDetail]    Script Date: 01/11/2019 13:09:13 ******/
IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRoomReservationRoomDetail]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[HotelRoomReservationRoomDetail](
		[ReservationId] [bigint] NULL,
		[RoomTypeId] [int] NULL,
		[RoomQuantity] [int] NULL
	)
END
GO
/****** Object:  Table [dbo].[CommonFormWiseFieldSetup]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonFormWiseFieldSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonFormWiseFieldSetup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PageId] [int] NOT NULL,
	[FieldId] [varchar](200) NOT NULL,
	[FieldName] [varchar](400) NOT NULL,
	[IsMandatory] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonFormWiseFieldSetup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[CommonFormWiseFieldSetup] ADD  CONSTRAINT [DF_CommonFormWiseFieldSetup_IsMandatory]  DEFAULT ((0)) FOR [IsMandatory]

END
GO

/****** Object:  Table [dbo].[PayrollEmpStatusHistory]    Script Date: 1/16/2019 11:54:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpStatusHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpStatusHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [bigint] NOT NULL,
	[EmpStatusId] [int] NOT NULL,
	[ActionDate] [datetime] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[Reason] [varchar](1000) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpStatusHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RestaurantSalesReturnItem]    Script Date: 1/18/2019 5:13:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantSalesReturnItem]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[RestaurantSalesReturnItem](
		[ReturnId] [bigint] IDENTITY(1,1) NOT NULL,
		[BillId] [int] NOT NULL,
		[KotId] [int] NOT NULL,
		[CostCenterId] [int] NOT NULL,
		[ItemId] [int] NOT NULL,
		[ItemName] [varchar](300) NOT NULL,
		[ItemUnit] [decimal](18, 5) NOT NULL,
		[UnitRate] [decimal](18, 2) NOT NULL,
		[Amount] [decimal](18, 2) NOT NULL,
		[AverageCost] [decimal](18, 2) NOT NULL,
		[ReturnedUnit] [decimal](18, 2) NOT NULL,
		[LastReturnedUnit] [decimal](18, 2) NOT NULL,
		[LastReturnDate]   [datetime] NULL,
		[ItemTotalAmount] [decimal](18, 2) NULL,
		[DiscountAmount] [decimal](18, 5) NULL,
		[DiscountedAmount] [decimal](18, 5) NULL,
		[ServiceRate] [decimal](18, 5) NULL,
		[ServiceCharge] [decimal](18, 5) NULL,
		[VatAmount] [decimal](18, 5) NULL,
		[CitySDCharge] [decimal](18, 5) NULL,
		[AdditionalCharge] [decimal](18, 5) NULL,
		[ItemCost] [decimal](18, 2) NULL,
	 CONSTRAINT [PK_RestaurantSalesReturnItem] PRIMARY KEY CLUSTERED 
	(
		[ReturnId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_ReturnedUnit]  DEFAULT ((0)) FOR [ReturnedUnit]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_LastReturnedUnit]  DEFAULT ((0)) FOR [LastReturnedUnit]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_DiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_DiscountedAmount]  DEFAULT ((0)) FOR [DiscountedAmount]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_ServiceRate]  DEFAULT ((0)) FOR [ServiceRate]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_ServiceCharge]  DEFAULT ((0)) FOR [ServiceCharge]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_VatAmount]  DEFAULT ((0)) FOR [VatAmount]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_CitySDCharge]  DEFAULT ((0)) FOR [CitySDCharge]
ALTER TABLE [dbo].[RestaurantSalesReturnItem] ADD  CONSTRAINT [DF_RestaurantSalesReturnItem_AdditionalCharge]  DEFAULT ((0)) FOR [AdditionalCharge]

END
GO

/****** Object:  Table [dbo].[HotelMarketSegmentWiseSalesSummary]    Script Date: 1/30/2019 11:23:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelMarketSegmentWiseSalesSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelMarketSegmentWiseSalesSummary](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[MarketSegmentId] [int] NOT NULL,
	[ReferenceId] [int] NOT NULL,
	[Room] [int] NOT NULL,
	[RoomRate] [decimal](18, 2) NOT NULL,
	[TotalRoom] [int] NOT NULL,
	[Pax] [int] NOT NULL,
	[MaxPax] [int] NOT NULL,
 CONSTRAINT [PK_HotelMarketSegmentWiseSalesSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonDeleteLedgerLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonDeleteLedgerLog](
	[DeleteLedgerId] [bigint] IDENTITY(1,1) NOT NULL,
	[DeleteLedgerDate] [date] NULL,
	[ModuleName] [varchar](50) NULL,
	[LedgerNumber] [varchar](25) NULL,
	[BillId] [int] NULL,
	[BillNumber] [varchar](50) NULL,
	[PaymentDate] [date] NOT NULL,
	[TransactionType] [nvarchar](15) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[CurrencyAmount] [money] NULL,
 CONSTRAINT [PK_CommonDeleteLedgerLog] PRIMARY KEY CLUSTERED 
(
	[DeleteLedgerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemTransactionHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemTransactionHistory](
	[ItemTransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionType] [nvarchar](25) NOT NULL,
	[TransactionForId] [bigint] NULL,
	[TransactionForType] [nvarchar](25) NULL,
	[TransactionFor] [nvarchar](25) NULL,
	[CostCenterId] [int] NULL,
	[LocationId] [int] NULL,
	[ToCostCenterId] [int] NULL,
	[ToLocationId] [int] NULL,
	[CostCenter] [varchar](250) NULL,
	[Location] [varchar](250) NULL,
	[ToCostCenter] [varchar](250) NULL,
	[ToLocation] [varchar](250) NULL,
	[ItemId] [int] NOT NULL,
	[ItemName] [varchar](500) NOT NULL,
	[Quantity] [decimal](18, 2) NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[TransactionTotalCost] [decimal](18, 2) NULL,
	[RefDocNo] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_InvItemTransactionHistory] PRIMARY KEY CLUSTERED 
(
	[ItemTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'TransactionType' AND DATA_TYPE = 'NVARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='25')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN TransactionType NVARCHAR(MAX) NOT NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'TransactionForType' AND DATA_TYPE = 'NVARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='25')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN TransactionForType NVARCHAR(MAX) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'TransactionFor' AND DATA_TYPE = 'NVARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='25')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN TransactionFor NVARCHAR(MAX) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'CostCenter' AND DATA_TYPE = 'VARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='250')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN CostCenter VARCHAR(MAX) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'Location' AND DATA_TYPE = 'VARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='250')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN Location VARCHAR(MAX) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'ToCostCenter' AND DATA_TYPE = 'VARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='250')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN ToCostCenter VARCHAR(MAX) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'ToLocation' AND DATA_TYPE = 'VARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='250')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN ToLocation VARCHAR(MAX) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemTransactionHistory' AND column_name = 'RefDocNo' AND DATA_TYPE = 'NVARCHAR' AND CHARACTER_MAXIMUM_LENGTH ='50')
BEGIN
   ALTER TABLE dbo.InvItemTransactionHistory
	   ALTER COLUMN RefDocNo NVARCHAR(MAX) NULL;
END
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemTransactionSerialHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemTransactionSerialHistory](
	[ItemSerialTransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemTransactionId] [bigint] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[CostCenterId] [int] NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NOT NULL,
	[SerialNumber] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_InvItemTransactionSerialHistory] PRIMARY KEY CLUSTERED 
(
	[ItemSerialTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO
GO
GO
/****** Object:  Table [dbo].[DiscountConfigSetup]    Script Date: 02/13/2019 6:51:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiscountConfigSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DiscountConfigSetup](
	[ConfigurationId] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDiscountApplicableIndividually] [bit] NULL,
	[IsDiscountApplicableMaxOneWhenMultiple] [bit] NULL,
	[IsDiscountOptionShowsWhenMultiple] [bit] NULL,
	[IsDiscountAndMembershipDiscountApplicableTogether] [bit] NULL,
	[IsBankDiscount] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_DiscountConfigSetup] PRIMARY KEY CLUSTERED 
(
	[ConfigurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MembershipPointDetails]    Script Date: 2/18/19 3:58:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MembershipPointDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MembershipPointDetails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NULL,
	[PaymentAmount] [money] NULL,
	[PointWiseAmount] [money] NULL,
	[PointType] [varchar](50) NULL,
	[TransactionDate] [datetime] NULL,
	[BillId] [bigint] NULL,
 CONSTRAINT [PK_MembershipPointDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[DiscountMaster]    Script Date: 2/19/2019 5:22:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiscountMaster]') AND type in (N'U'))
CREATE TABLE [dbo].[DiscountMaster](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[Todate] [datetime] NOT NULL,
	[DiscountFor] [varchar](50) NULL,
	[Remarks] [varchar](200) NULL,
	[DiscountName] [varchar](150) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CostCenterId] [int] NOT NULL,
 CONSTRAINT [PK_DiscountMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DiscountDetail]    Script Date: 2/19/2019 5:23:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiscountDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DiscountDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DiscountMasterId] [bigint] NOT NULL,
	[DiscountForId] [bigint] NOT NULL,
	[DiscountType] [varchar](25) NULL,
	[Discount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_DiscountDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[DiscountDetail] ADD  CONSTRAINT [DF_DiscountDetail_Discount]  DEFAULT ((0)) FOR [Discount]

ALTER TABLE [dbo].[DiscountDetail]  WITH CHECK ADD  CONSTRAINT [FK_DiscountDetail_DiscountMaster] FOREIGN KEY([DiscountMasterId])
REFERENCES [dbo].[DiscountMaster] ([Id])

ALTER TABLE [dbo].[DiscountDetail] CHECK CONSTRAINT [FK_DiscountDetail_DiscountMaster]

END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMLogKeeping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMLogKeeping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](MAX) NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[CompanyId] [int] NULL,
	[ContactId] [bigint] NULL,
	[DealId] [bigint] NULL,
	[SalesCallEntryId] [bigint] NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_SMLogKeeping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMLogKeeping' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.SMLogKeeping
		ADD LastModifiedBy BIGINT NULL 
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMStage')
BEGIN
	DROP TABLE [dbo].[SMStage]
END
GO
/****** Object:  Table [dbo].[SMDealStage]    Script Date: 4/25/2019 10:14:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMDealStage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMDealStage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DealStage] [varchar](300) NULL,
	[Complete] [DECIMAL](18,2) NULL,
	[ForcastType] [varchar](20) NULL,
	[ForcastCategory] [varchar](20) NULL,
	[DisplaySequence] [int] NULL,
	[Description] [varchar](MAX) NULL,	
	[IsSiteSurvey] [bit] NOT NULL,
	[IsQuotationReveiw] [bit] NOT NULL,
	[IsCloseWon] [bit] NOT NULL,
	[IsCloseLost] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMDealStage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF


ALTER TABLE [dbo].[SMDealStage] ADD  CONSTRAINT [DF_SMDealStage_IsActive]  DEFAULT ((1)) FOR [IsActive]

ALTER TABLE [dbo].[SMDealStage] ADD  CONSTRAINT [DF_SMDealStage_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

ALTER TABLE [dbo].[SMDealStage] ADD  CONSTRAINT [DF_SMDealStage_IsSiteSurvey]  DEFAULT ((0)) FOR [IsSiteSurvey]

ALTER TABLE [dbo].[SMDealStage] ADD  CONSTRAINT [DF_SMDealStage_IsQuotationReveiw]  DEFAULT ((0)) FOR [IsQuotationReveiw]

ALTER TABLE [dbo].[SMDealStage] ADD  CONSTRAINT [DF_SMDealStage_IsCloseWon]  DEFAULT ((0)) FOR [IsCloseWon]

ALTER TABLE [dbo].[SMDealStage] ADD  CONSTRAINT [DF_SMDealStage_IsCloseLost]  DEFAULT ((0)) FOR [IsCloseLost]

END
GO

/****** Object:  Table [dbo].[SMContactInformation]    Script Date: 3/7/2019 10:54:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMContactInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMContactInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[ContactNo] [varchar](50) NOT NULL,
	[ContactOwnerId] [int] NULL,
	[CompanyId] [int] NULL,
	[JobTitle] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[LastContactDateTime] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ContactInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'IsDeleted')
BEGIN
	ALTER TABLE dbo.SMContactInformation
		ADD [IsDeleted] [bit] NOT NULL
	ALTER TABLE [dbo].[SMContactInformation] ADD  CONSTRAINT [DF_ContactInformation_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'LifeCycleId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD LifeCycleId BIGINT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'SourceId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD SourceId BIGINT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'DOB')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD DOB DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'DateAnniversary')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD DateAnniversary DATETIME NULL
END
GO

--            
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PersonalAddress')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PersonalAddress NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'ContactType')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD ContactType NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'Department')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD Department NVARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'TicketNo')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD TicketNo NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'MobilePersonal')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD MobilePersonal NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'MobileWork')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD MobileWork NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PhonePersonal')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PhonePersonal NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PhoneWork')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PhoneWork NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'Facebook')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD Facebook NVARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'Skype')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD Skype NVARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'Whatsapp')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD Whatsapp NVARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'Twitter')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD Twitter NVARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'EmailWork')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD EmailWork NVARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'ContactNumber')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD ContactNumber VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'WorkAddress')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD WorkAddress VARCHAR(200) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'WorkCountryId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD WorkCountryId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'WorkStateId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD WorkStateId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'WorkCityId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD WorkCityId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'WorkLocationId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD WorkLocationId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'WorkStreet')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD WorkStreet VARCHAR(MAX) NULL 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'WorkPostCode')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD WorkPostCode VARCHAR(MAX) NULL 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PersonalCountryId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PersonalCountryId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PersonalStateId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PersonalStateId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PersonalCityId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PersonalCityId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PersonalLocationId')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PersonalLocationId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PersonalStreet')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PersonalStreet VARCHAR(MAX) NULL 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'PersonalPostCode')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ADD PersonalPostCode VARCHAR(MAX) NULL 
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'Name')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ALTER COLUMN Name VARCHAR(MAX) NOT NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'Email')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ALTER COLUMN Email VARCHAR(MAX) NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'ContactNo')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ALTER COLUMN ContactNo VARCHAR(MAX) NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactInformation' AND column_name = 'JobTitle')
BEGIN
	ALTER TABLE dbo.SMContactInformation
	ALTER COLUMN JobTitle VARCHAR(MAX) NULL
END
GO
/****** Object:  Table [dbo].[SMSalesCallEntry]    Script Date: 3/12/2019 3:20:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSalesCallEntry]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSalesCallEntry](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LogType] [nvarchar](50) NOT NULL,
	[MeetingDate] [datetime] NULL,
	[MeetingLocation] [nvarchar](150) NULL,
	[ParticipantFromParty] [nvarchar](350) NULL,
	[MeetingAgenda] [nvarchar](150) NULL,
	[Decission] [nvarchar](500) NULL,
	[MeetingAfterAction] [nvarchar](150) NULL,
	[EmailType] [nvarchar](50) NULL,
	[CallStatus] [nvarchar](50) NULL,
	[LogBody] [nvarchar](550) NULL,
	[CompanyId] [int] NULL,
	[DealId] [bigint] NULL,
	[ContactId] [bigint] NULL,
	[LogDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMSalesCallEntry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMSalesCallEntry' AND column_name = 'MeetingType')
BEGIN
	ALTER TABLE dbo.SMSalesCallEntry
		ADD MeetingType VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMSalesCallEntry' AND column_name = 'SocialMediaId')
BEGIN
	ALTER TABLE dbo.SMSalesCallEntry
		ADD SocialMediaId INT
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMSalesCallEntry' AND column_name = 'AccountManagerId')
BEGIN
	ALTER TABLE dbo.SMSalesCallEntry
		ADD AccountManagerId INT
END
GO
/****** Object:  Table [dbo].[SMSalesCallEntry]    Script Date: 3/12/2019 3:20:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSalesCallParticipant]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSalesCallParticipant](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SalesCallEntryId] [bigint] NOT NULL,
	[PrticipantType] [nvarchar](50) NOT NULL,
	[ContactId] [bigint] NOT NULL,
 CONSTRAINT [PK_SMSalesCallParticipant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[SMTask]    Script Date: 3/13/2019 1:44:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMTask]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMTask](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskName] [varchar](150) NULL,
	[DueDate] [date] NOT NULL,
	[DueTime] [time](7) NOT NULL,
	[Description] [varchar](1000) NULL,
	[TaskType] [varchar](50) NULL,
	[AssignToId] [int] NULL,
	[EmailReminderType] [varchar](50) NULL,
	[IsEmailReminderSent] [bit] NOT NULL,
	[EmailReminderDate] [date] NULL,
	[EmailReminderTime] [time](7) NULL,
	[IsCompleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMTaskAssignment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[SMTask] ADD  CONSTRAINT [DF_SMTask_IsEmailReminderCompletedToday]  DEFAULT ((0)) FOR [IsEmailReminderSent]

ALTER TABLE [dbo].[SMTask] ADD  CONSTRAINT [DF_SMTaskAssignment_IsCompleted]  DEFAULT ((0)) FOR [IsCompleted]

ALTER TABLE [dbo].[SMTask]  WITH CHECK ADD  CONSTRAINT [FK_SMTaskAssignment_SMAccountManager] FOREIGN KEY([AssignToId])
REFERENCES [dbo].[SMAccountManager] ([AccountManagerId])

ALTER TABLE [dbo].[SMTask] CHECK CONSTRAINT [FK_SMTaskAssignment_SMAccountManager]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'TaskStage')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD TaskStage INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'SourceNameId')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD SourceNameId BIGINT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'TaskDate')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD TaskDate DATE NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'StartTime')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD StartTime TIME(7) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'TaskStatus')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD TaskStatus NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'CallToAction')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD CallToAction NVARCHAR(MAX) NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'DueDate')
BEGIN
	EXEC sp_RENAME 'SMTask.DueDate' , 'EstimatedDoneDate', 'COLUMN'
END
GO 
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'DueTime')
BEGIN
	EXEC sp_RENAME 'SMTask.DueTime' , 'EndTime', 'COLUMN'
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMTask' AND column_name = 'EstimatedDoneHour')
BEGIN
	ALTER TABLE dbo.SMTask
		ADD EstimatedDoneHour DECIMAL(18,2) NULL
END
GO

/****** Object:  Table [dbo].[SMSalesTransfer]    Script Date: 3/27/2019 3:25:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSalesTransfer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSalesTransfer](
	[SalesTransferId] [bigint] IDENTITY(1,1) NOT NULL,
	[DealId] [bigint] NULL,
	[QuotationId] [bigint] NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[DeliverStatus] [nvarchar](50) NULL,
	[Date] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[IsApproved] [bit] NOT NULL,
	[Remarks] [nvarchar](100) NULL,
	[TransferNumber] [varchar](15) NULL,
 CONSTRAINT [PK_SMSalesTransfer] PRIMARY KEY CLUSTERED 
(
	[SalesTransferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

ALTER TABLE [dbo].[SMSalesTransfer] ADD  CONSTRAINT [DF_SMSalesTransfer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

ALTER TABLE [dbo].[SMSalesTransfer] ADD  CONSTRAINT [DF_SMSalesTransfer_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
END
GO



/****** Object:  Table [dbo].[SMSalesTransferDetails]    Script Date: 3/27/2019 3:27:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSalesTransferDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSalesTransferDetails](
	[SalesTransferDetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[SalesTransferId] [bigint] NOT NULL,
	[ItemId] [int] NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[StockById] [int] NULL,
 CONSTRAINT [PK_SMSalesTransferDetails] PRIMARY KEY CLUSTERED 
(
	[SalesTransferDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[SMSalesTransferDetails] ADD  CONSTRAINT [DF_SMSalesTransferDetails_Quantity]  DEFAULT ((0.00)) FOR [Quantity]

END
GO

/****** Object:  Table [dbo].[SMSalesItemSerialTransfer]    Script Date: 3/27/2019 3:21:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSalesItemSerialTransfer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSalesItemSerialTransfer](
	[SalesItemSerialTransferId] [bigint] IDENTITY(1,1) NOT NULL,
	[SalesTransferId] [bigint] NOT NULL,
	[ItemId] [int] NOT NULL,
	[SerialNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_SMSalesItemSerialTransfer] PRIMARY KEY CLUSTERED 
(
	[SalesItemSerialTransferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- TABLE UPDATE WITH COLUMN


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NameBangla')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NameBangla NVARCHAR(200) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NickName')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NickName VARCHAR(200) NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'CountryId')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD CountryId INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'ReligionId')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD ReligionId INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'ProfessionId')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD ProfessionId INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Introducer_1_id')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Introducer_1_id INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Introducer_2_id')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Introducer_2_id INT NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Hobbies')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Hobbies VARCHAR(200) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Awards')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Awards VARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'OfficeAddress')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD OfficeAddress VARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Remarks VARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Remarks1')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Remarks1 VARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Remarks2')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Remarks2 VARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NationalitySt')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NationalitySt VARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsAccepted')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsAccepted BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsRejected')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsRejected BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsDeferred')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsDeferred BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'BirthPlace')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD BirthPlace NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Height')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Height FLOAT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'Weight')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD Weight FLOAT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NationalID')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NationalID NVARCHAR(100) NULL
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'FirstName' AND DATA_TYPE = 'VARCHAR')
BEGIN
   ALTER TABLE dbo.MemMemberBasics
	   ALTER COLUMN FirstName VARCHAR(200) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'LastName' AND DATA_TYPE = 'VARCHAR')
BEGIN
   ALTER TABLE dbo.MemMemberBasics
	   ALTER COLUMN LastName VARCHAR(200) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsAccepted1')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsAccepted1 BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsRejected1')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsRejected1 BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsDeferred1')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsDeferred1 BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsAccepted2')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsAccepted2 BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsRejected2')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsRejected2 BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'IsDeferred2')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD IsDeferred2 BIT NULL
END
GO

--Nominee 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NomineeName')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NomineeName NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NomineeFather')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NomineeFather NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NomineeMother')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NomineeMother NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NomineeDOB')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NomineeDOB DATETIME NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'NomineeRelationId')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD NomineeRelationId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'PathPersonalImg')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD PathPersonalImg NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'PathNIdImage')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD PathNIdImage NVARCHAR(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'MeetingDate')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD MeetingDate DATETIME NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'MeetingDateEC')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD MeetingDateEC DATETIME NULL
END
GO
 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'MeetingDecision')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD MeetingDecision nvarchar(200) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberBasics' AND column_name = 'MeetingDecisionEC')
BEGIN
	ALTER TABLE dbo.MemMemberBasics
		ADD MeetingDecisionEC nvarchar(200) NULL
END
GO

--MemMemberFamilyMember  
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberFamilyMember' AND column_name = 'FamMemBloodGroupId')
BEGIN
	ALTER TABLE dbo.MemMemberFamilyMember
		ADD FamMemBloodGroupId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'MemMemberFamilyMember' AND column_name = 'FamMemMarriageDate')
BEGIN
	ALTER TABLE dbo.MemMemberFamilyMember
		ADD FamMemMarriageDate DATETIME NULL
END
GO
-- Online membership ends

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMQuotation' AND column_name = 'DeliverStatus')
BEGIN
	ALTER TABLE dbo.SMQuotation
		ADD DeliverStatus nvarchar(50) NULL
 END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMSalesTransfer' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.SMSalesTransfer
		ADD ApprovedBy int NULL
 END
GO
-- TABLE UPDATE WITH COLUMN End

/****** Object:  Table [dbo].[TemporarySync]    Script Date: 4/16/2019 4:29:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemporarySync]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TemporarySync](
		[BillId] [int] NULL,
		[BillType] [varchar](50) NULL
	) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ReceipeModifierType]    Script Date: 04/18/2019 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReceipeModifierType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ReceipeModifierType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NULL,
	[RecipeItemId] [int] NULL,
	[UnitHead] [nvarchar](50) NULL,
	[UnitQuantity] [decimal](18, 2) NULL,
	[AdditionalCost] [decimal](18, 2) NULL,
	[TotalCost] [decimal](18, 2) NULL,
	[UnitHeadId] [int] NULL,
 CONSTRAINT [PK_ReceipeModifierType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[RestaurantKotRecipeDetail]    Script Date: 5/3/19 10:57:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantKotRecipeDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantKotRecipeDetail](
	[RecipeId] [int] IDENTITY(1,1) NOT NULL,
	[KotId] [int] NULL,
	[ItemId] [int] NULL,
	[RecipeItemId] [int] NULL,
	[RecipeItemName] [varchar](200) NULL,
	[UnitHeadId] [int] NULL,
	[ItemUnit] [decimal](18, 2) NULL,
	[ItemCost] [decimal](18, 2) NULL,
	[HeadName] [varchar](50) NULL,
	[TypeId] [int] NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_RestaurantKotRecipeDetail] PRIMARY KEY CLUSTERED 
(
	[RecipeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDealInfo')
BEGIN
	DROP TABLE [dbo].[SMDealInfo]
END
GO

/****** Object:  Table [dbo].[SMDeal]    Script Date: 4/26/2019 3:41:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMDeal]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMDeal](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NULL,
	[CompanyId] [int] NULL,
	[Name] [varchar](50) NULL,
	[Amount] [decimal](18, 2) NULL,
	[ExpectedRevenue] [decimal](18, 2) NULL,
	[Type] [varchar](50) NULL,
	[StageId] [int] NULL,
	[StartDate] [datetime] NULL,
	[CloseDate] [datetime] NULL,
	[ProbabilityStageId] [int] NULL,
	[SegmentId] [int] NULL,
	[Description] [varchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	CONSTRAINT [PK_SMDeal] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


	ALTER TABLE [dbo].[SMDeal] ADD  CONSTRAINT [DF_SMDeal_IsActive]  DEFAULT ((1)) FOR [IsActive]


	ALTER TABLE [dbo].[SMDeal] ADD  CONSTRAINT [DF_SMDeal_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO

GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDeal' AND column_name = 'ImplementationDate')
BEGIN
	ALTER TABLE dbo.SMDeal
	ADD ImplementationDate DATETIME NULL
END
GO

GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDeal' AND column_name = 'ImplementationFeedback')
BEGIN
	ALTER TABLE dbo.SMDeal
	ADD ImplementationFeedback NVARCHAR(200) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDeal' AND column_name = 'DealNumber')
BEGIN
	ALTER TABLE dbo.SMDeal
	ADD DealNumber VARCHAR(50) NULL
END
GO 
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDeal' AND column_name = 'IsCanDelete')
BEGIN
	ALTER TABLE dbo.SMDeal 
	ADD IsCanDelete BIT NOT NULL 
	CONSTRAINT [DF_SMDeal_IsCanDelete]  DEFAULT ((1))
	WITH VALUES
END
GO
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_SMDeal_HotelGuestCompany') AND parent_object_id = OBJECT_ID(N'dbo.SMDeal'))
BEGIN	
	ALTER TABLE [dbo].[SMDeal]  DROP CONSTRAINT [FK_SMDeal_HotelGuestCompany]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDeal' AND column_name = 'ImplementationStatus')
BEGIN
	ALTER TABLE dbo.SMDeal
		ADD ImplementationStatus VARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDeal' AND column_name = 'GLCompanyId')
BEGIN
	ALTER TABLE dbo.SMDeal 
	ADD GLCompanyId INT NOT NULL 
	CONSTRAINT [DF_SMDeal_GLCompanyId]  DEFAULT ((1))
	WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMDeal' AND column_name = 'GLProjectId')
BEGIN
	ALTER TABLE dbo.SMDeal 
	ADD GLProjectId INT NOT NULL 
	CONSTRAINT [DF_SMDeal_GLProjectId]  DEFAULT ((1))
	WITH VALUES
END
GO
/****** Object:  Table [dbo].[SMDealWiseContactMap]    Script Date: 4/26/2019 3:43:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMDealWiseContactMap]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMDealWiseContactMap](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[DealId] [bigint] NULL,
		[ContactId] [bigint] NULL,
	 CONSTRAINT [PK_SMDealWiseContactMap] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[SMSourceInformation]    Script Date: 4/29/2019 6:33:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSourceInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSourceInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SourceName] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMSourceInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[SMSourceInformation] ADD  CONSTRAINT [DF_SMSourceInformation_Status]  DEFAULT ((1)) FOR [Status]

ALTER TABLE [dbo].[SMSourceInformation] ADD  CONSTRAINT [DF_SMSourceInformation_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO


/****** Object:  Table [dbo].[SMCompanyTypeInformation]    Script Date: 4/30/2019 6:33:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCompanyTypeInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMCompanyTypeInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMCompanyTypeInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[SMCompanyTypeInformation] ADD  CONSTRAINT [DF_SMCompanyTypeInformation_Status]  DEFAULT ((1)) FOR [Status]

ALTER TABLE [dbo].[SMCompanyTypeInformation] ADD  CONSTRAINT [DF_SMCompanyTypeInformation_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMCompanyTypeInformation' AND column_name = 'IsLocalOrForeign')
BEGIN
	ALTER TABLE dbo.SMCompanyTypeInformation
		ADD IsLocalOrForeign INT NULL DEFAULT 1 WITH VALUES
	
	TRUNCATE TABLE SMCompanyTypeInformation	
END
GO

/****** Object:  Table [dbo].[HotelCompanyContactDetails]    Script Date: 04/29/2019 5:49:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyContactDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelCompanyContactDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionType] [varchar](200) NULL,
	[TransactionId] [bigint] NULL,
	[FieldName] [varchar](200) NULL,
	[FieldValue] [varchar](200) NULL,
 CONSTRAINT [PK_HotelCompanyContactDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- Update HotelGuestCompany -- start
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'CompanyOwnerId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
		ADD CompanyOwnerId	INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'IndustryId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD IndustryId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'AncestorId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD AncestorId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'Lvl')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD Lvl INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'Hierarchy')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD Hierarchy VARCHAR(900) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'HierarchyIndex')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD HierarchyIndex VARCHAR(900) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'CompanyType')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD CompanyType INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'IndustryId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD IndustryId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'OwnershipId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD OwnershipId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'TicketNumber')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD TicketNumber VARCHAR(20) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'LifeCycleStageId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD LifeCycleStageId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BillingStreet')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BillingStreet VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BillingCityId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BillingCityId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BillingStateId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BillingStateId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BillingCountryId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BillingCountryId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'BillingPostCode')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD BillingPostCode VARCHAR(10) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ShippingStreet')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD ShippingStreet VARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ShippingCityId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD ShippingCityId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ShippingStateId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD ShippingStateId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ShippingCountryId')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD ShippingCountryId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'ShippingPostCode')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD ShippingPostCode VARCHAR(10) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = 'Fax')
BEGIN
	ALTER TABLE dbo.HotelGuestCompany
	ADD Fax VARCHAR(50) NULL
END
GO
--IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'HotelGuestCompany' AND column_name = '')
--BEGIN
--	ALTER TABLE dbo.HotelGuestCompany
--	ADD NULL
--END
--GO

-- Update HotelGuestCompany -- END

/****** Object:  Table [dbo].[SMSegmentInformation]    Script Date: 4/29/2019 6:33:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSegmentInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSegmentInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SegmentName] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMSegmentInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[SMSegmentInformation] ADD  CONSTRAINT [DF_SMSegmentInformation_Status]  DEFAULT ((1)) FOR [Status]

ALTER TABLE [dbo].[SMSegmentInformation] ADD  CONSTRAINT [DF_SMSegmentInformation_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

END
GO

/****** Object:  Table [dbo].[SMDealProbabilityStageInformation]    Script Date: 4/29/2019 6:33:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMDealProbabilityStageInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMDealProbabilityStageInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProbabilityStage] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMDealProbabilityStageInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[SMDealProbabilityStageInformation] ADD  CONSTRAINT [DF_SMDealProbabilityStageInformation_Status]  DEFAULT ((1)) FOR [Status]

ALTER TABLE [dbo].[SMDealProbabilityStageInformation] ADD  CONSTRAINT [DF_SMDealProbabilityStageInformation_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

END
GO

/****** Object:  Table [dbo].[SMOwnershipInformation]    Script Date: 4/29/2019 6:33:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMOwnershipInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMOwnershipInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnershipName] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMOwnershipInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[SMOwnershipInformation] ADD  CONSTRAINT [DF_SMOwnershipInformation_Status]  DEFAULT ((1)) FOR [Status]

ALTER TABLE [dbo].[SMOwnershipInformation] ADD  CONSTRAINT [DF_SMOwnershipInformation_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

END
GO


/****** Object:  Table [dbo].[SMContactTransfer]    Script Date: 5/16/2019 12:34:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMContactTransfer]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMContactTransfer](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[ContactId] [bigint] NOT NULL,
		[PreviousCompanyId] [int] NOT NULL,
		[TransferredCompanyId] [int] NOT NULL,
		[Title] [nvarchar](50) NULL,
		[Department] [nvarchar](100) NULL,
		[Mobile] [nvarchar](50) NULL,
		[Phone] [nvarchar](50) NULL,
		[Email] [nvarchar](50) NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_SMContactTransfer] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSiteSurveyNote]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMSiteSurveyNote](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IsSiteSurveyUnderCompany] [bit] NOT NULL,
	[CompanyId] [int] NULL,
	[ContactId] [int] NULL,
	[Address] [nvarchar](max) NULL,
	[DealId] [bigint] NULL,
	[IsDealNeedSiteSurvey] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SegmentId] [bigint] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[Status] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_SMSiteSurveyNote] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[SMSiteSurveyNote] ADD  CONSTRAINT [DF_SMSiteSurveyNote_IsDealNeedSiteSurvey]  DEFAULT ((0)) FOR [IsDealNeedSiteSurvey]

ALTER TABLE [dbo].[SMSiteSurveyNote] ADD  CONSTRAINT [DF_SMSiteSurveyNote_IsActive]  DEFAULT ((1)) FOR [IsActive]

ALTER TABLE [dbo].[SMSiteSurveyNote] ADD  CONSTRAINT [DF_SMSiteSurveyNote_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

ALTER TABLE [dbo].[SMSiteSurveyNote] ADD  CONSTRAINT [DF_SMSiteSurveyNote_Status]  DEFAULT ('Pending') FOR [Status]

ALTER TABLE [dbo].[SMSiteSurveyNote] ADD  CONSTRAINT [DF_SMSiteSurveyNote_IsSiteSurveyUnderCompany]  DEFAULT ((1)) FOR [IsSiteSurveyUnderCompany]

END
GO

/****** Object:  Table [dbo].[SMSiteSurveyFeedback]    Script Date: 5/20/2019 4:43:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSiteSurveyFeedback]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSiteSurveyFeedback](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SiteSurveyNoteId] [bigint] NULL,
	[SurveyFeedback] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMSiteSurveyFeedback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[SMSiteSurveyFeedback] ADD  CONSTRAINT [DF_SMSiteSurveyFeedback_IsActive]  DEFAULT ((1)) FOR [IsActive]

ALTER TABLE [dbo].[SMSiteSurveyFeedback] ADD  CONSTRAINT [DF_SMSiteSurveyFeedback_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

END
GO

/****** Object:  Table [dbo].[SMSiteSurveyFeedbackDetails]    Script Date: 5/20/2019 4:43:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSiteSurveyFeedbackDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSiteSurveyFeedbackDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SiteSurveyFeedbackId] [bigint] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_SMSiteSurveyFeedbackDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

/****** Object:  Table [dbo].[SMSiteSurveyEngineer]    Script Date: 5/20/2019 4:42:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMSiteSurveyEngineer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMSiteSurveyEngineer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SiteSurveyFeedbackId] [bigint] NULL,
	[EmpId] [int] NULL,
 CONSTRAINT [PK_SMSiteSurveyEngineer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO
GO
/****** Object:  Table [dbo].[DealImpFeedback]    Script Date: 05/20/2019 4:39:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DealImpFeedback]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DealImpFeedback](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ImpEngineerId] [bigint] NULL,
	[DealId] [bigint] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_DealImpFeedback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[TaskStage]    Script Date: 4/25/2019 10:14:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskStage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TaskStage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaskStage] [varchar](300) NULL,
	[Complete] [DECIMAL](18,2) NULL,	
	[DisplaySequence] [int] NULL,
	[Description] [varchar](MAX) NULL,	
	[IsFinalStage] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_TaskStage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[TaskStage] ADD  CONSTRAINT [DF_TaskStage_IsActive]  DEFAULT ((1)) FOR [IsActive]

ALTER TABLE [dbo].[TaskStage] ADD  CONSTRAINT [DF_TaskStage_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

ALTER TABLE [dbo].[TaskStage] ADD  CONSTRAINT [DF_TaskStage_IsFinalStage]  DEFAULT ((0)) FOR [IsFinalStage]

END
GO







/****** Object:  Table [dbo].[GLProjectStage]    Script Date: 4/25/2019 10:14:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLProjectStage]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GLProjectStage](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ProjectStage] [varchar](300) NULL,
		[Complete] [decimal](18, 2) NULL,
		[DisplaySequence] [int] NULL,
		[Description] [varchar](max) NULL,
		[IsFinalStage] [bit] NOT NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_ProjectStage] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	SET ANSI_PADDING OFF
	ALTER TABLE [dbo].[GLProjectStage] ADD  CONSTRAINT [DF_ProjectStage_IsFinalStage]  DEFAULT ((0)) FOR [IsFinalStage]
	ALTER TABLE [dbo].[GLProjectStage] ADD  CONSTRAINT [DF_ProjectStage_IsActive]  DEFAULT ((1)) FOR [IsActive]
	ALTER TABLE [dbo].[GLProjectStage] ADD  CONSTRAINT [DF_ProjectStage_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TMTaskAssignedEmployee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TMTaskAssignedEmployee](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskId] [bigint] NULL,
	[EmployeeId] [bigint] NULL,
 CONSTRAINT [PK_TMTaskAssignedEmployee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TMTaskAssignedEmployee' AND column_name = 'TaskFeedback')
BEGIN
	ALTER TABLE dbo.TMTaskAssignedEmployee
		ADD TaskFeedback NVARCHAR(MAX) NULL
END
GO

/****** Object:  Table [dbo].[SMCostAnalysisDetail]    Script Date: 7/25/2019 1:12:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCostAnalysisDetail]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMCostAnalysisDetail](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[SMCostAnalysisId] [bigint] NOT NULL,
		[ItemType] [varchar](25) NULL,
		[CategoryId] [int] NULL,
		[ServicePackageId] [int] NULL,
		[ServiceBandWidthId] [int] NULL,
		[ServiceTypeId] [int] NULL,
		[UpLink] [int] NOT NULL,
		[ItemId] [int] NULL,
		[StockBy] [int] NULL,
		[Quantity] [decimal](18, 2) NOT NULL,
		[UnitPrice] [decimal](18, 2) NOT NULL,
		[TotalOfferedPrice] [decimal](18, 2) NOT NULL,
		[AverageCost] [decimal](18, 2) NOT NULL,
		[TotalCost] [decimal](18, 2) NOT NULL,
		[AdditionalCost] [decimal](18, 2) NOT NULL,
		[TotalProjetcedCost] [decimal](18, 2) NOT NULL,
	 CONSTRAINT [PK_SMCostAnalysisDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_UpLink]  DEFAULT ((0)) FOR [UpLink]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_Quantity]  DEFAULT ((0)) FOR [Quantity]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_OfferedPrice]  DEFAULT ((0)) FOR [TotalOfferedPrice]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_AverageCost]  DEFAULT ((0)) FOR [AverageCost]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_TotalCost]  DEFAULT ((0)) FOR [TotalCost]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_AddtionalCost]  DEFAULT ((0)) FOR [AdditionalCost]

	ALTER TABLE [dbo].[SMCostAnalysisDetail] ADD  CONSTRAINT [DF_SMCostAnalysisDetail_TotalPrice]  DEFAULT ((0)) FOR [TotalProjetcedCost]
END
GO


/****** Object:  Table [dbo].[SMCostAnalysis]    Script Date: 7/25/2019 1:14:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMCostAnalysis]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMCostAnalysis](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Name] [varchar](500) NULL,
		[Remarks] [varchar](500) NULL,
		[TotalCost] [decimal](18, 2) NOT NULL,
		[GrandTotal] [decimal](18, 2) NOT NULL,
		[DiscountType] [varchar](50) NULL,
		[DiscountAmount] [decimal](18, 2) NOT NULL,
		[CalculatedDiscountAmount] [decimal](18, 2) NOT NULL,
		[DiscountedAmount] [decimal](18, 2) NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_SMCostAnalysis] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	

	ALTER TABLE [dbo].[SMCostAnalysis] ADD  CONSTRAINT [DF_SMCostAnalysis_TotalCost]  DEFAULT ((0)) FOR [TotalCost]

	ALTER TABLE [dbo].[SMCostAnalysis] ADD  CONSTRAINT [DF_SMCostAnalysis_GrandTotal]  DEFAULT ((0)) FOR [GrandTotal]	

	ALTER TABLE [dbo].[SMCostAnalysis] ADD  CONSTRAINT [DF_SMCostAnalysis_DiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]	

	ALTER TABLE [dbo].[SMCostAnalysis] ADD  CONSTRAINT [DF_SMCostAnalysis_CalculatedDiscountAmount]  DEFAULT ((0)) FOR [CalculatedDiscountAmount]	

	ALTER TABLE [dbo].[SMCostAnalysis] ADD  CONSTRAINT [DF_SMCostAnalysis_DiscountedAmount]  DEFAULT ((0)) FOR [DiscountedAmount]
END
GO
/****** Object:  Table [dbo].[CustomNotice]    Script Date: 08/06/2019 1:08:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomNotice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CustomNotice](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NoticeName] [nvarchar](250) NULL,
	[Content] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CustomNotice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CustomNotice' AND column_name = 'CloseDate')
BEGIN
	ALTER TABLE dbo.CustomNotice
		ADD CloseDate Datetime NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CustomNotice' AND column_name = 'AssignType')
BEGIN
	ALTER TABLE dbo.CustomNotice
		ADD AssignType NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CustomNotice' AND column_name = 'EmpDepartment')
BEGIN
	ALTER TABLE dbo.CustomNotice
		ADD EmpDepartment INT NULL
END
GO
/****** Object:  Table [dbo].[CustomNoticeEmployeeMapping]    Script Date: 08/06/2019 1:15:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomNoticeEmployeeMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CustomNoticeEmployeeMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NoticeId] [bigint] NULL,
	[EmpId] [int] NULL,
 CONSTRAINT [PK_CustomNoticeEmployeeMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[GLOpeningBalance]    Script Date: 8/20/2019 5:31:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GLOpeningBalance](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[TransactionType] [varchar](50) NULL,
		[CompanyId] [int] NOT NULL,
		[ProjectId] [int] NOT NULL,
		[FiscalYearId] [int] NOT NULL,
		[IsApproved] [bit] NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_GLOpeningBalance] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[GLOpeningBalance] ADD  CONSTRAINT [DF_GLOpeningBalance_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalance' AND column_name = 'OpeningDate')
BEGIN
	ALTER TABLE GLOpeningBalance
		DROP COLUMN OpeningDate;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalance' AND column_name = 'OpeningBalanceDate')
BEGIN
	ALTER TABLE dbo.GLOpeningBalance
		ADD OpeningBalanceDate DATETIME NOT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalance' AND column_name = 'OpeningBalanceEquity')
BEGIN
	ALTER TABLE dbo.GLOpeningBalance
		ADD OpeningBalanceEquity MONEY NULL
END
GO
/****** Object:  Table [dbo].[GLOpeningBalanceDetail]    Script Date: 8/20/2019 5:32:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLOpeningBalanceDetail]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GLOpeningBalanceDetail](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[GLOpeningBalanceId] [bigint] NOT NULL,
		[TransactionNodeId] [bigint] NOT NULL,
		[DebitAmount] [decimal](18, 2) NULL,
		[CreditAmount] [decimal](18, 2) NULL,
	 CONSTRAINT [PK_GLOpeningBalanceDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalanceDetail' AND column_name = 'TransactionNodeId')
BEGIN
	ALTER TABLE GLOpeningBalanceDetail
		DROP COLUMN TransactionNodeId;
END
--GO
--IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalanceDetail' AND column_name = 'DebitAmount')
--BEGIN
--	ALTER TABLE GLOpeningBalanceDetail
--		DROP COLUMN DebitAmount;
--END
--GO
--IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalanceDetail' AND column_name = 'CreditAmount')
--BEGIN
--	ALTER TABLE GLOpeningBalanceDetail
--		DROP COLUMN CreditAmount;
--END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalanceDetail' AND column_name = 'AccountNodeId')
BEGIN
	ALTER TABLE dbo.GLOpeningBalanceDetail
		ADD AccountNodeId BIGINT NOT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalanceDetail' AND column_name = 'AccountType')
BEGIN
	ALTER TABLE dbo.GLOpeningBalanceDetail
		ADD AccountType VARCHAR(50) NOT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalanceDetail' AND column_name = 'AccountName')
BEGIN
	ALTER TABLE dbo.GLOpeningBalanceDetail
		ADD AccountName VARCHAR(MAX) NOT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLOpeningBalanceDetail' AND column_name = 'OpeningBalance')
BEGIN
	ALTER TABLE dbo.GLOpeningBalanceDetail
		ADD OpeningBalance DECIMAL(18, 3) NOT NULL
END
GO
/****** Object:  Table [dbo].[InvOpeningBalance]    Script Date: 8/20/2019 5:33:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[InvOpeningBalance](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[CompanyId] [int] NOT NULL,
		[ProjectId] [int] NOT NULL,
		[FiscalYearId] [int] NOT NULL,
		[StoreId] [int] NOT NULL,
		[LocationId] [int] NOT NULL,
		[IsApproved] [bit] NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_InvOpeningBalance] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[InvOpeningBalance] ADD  CONSTRAINT [DF_InvOpeningBalance_StoreId]  DEFAULT ((0)) FOR [StoreId]

	ALTER TABLE [dbo].[InvOpeningBalance] ADD  CONSTRAINT [DF_InvOpeningBalance_LocationId]  DEFAULT ((0)) FOR [LocationId]

	ALTER TABLE [dbo].[InvOpeningBalance] ADD  CONSTRAINT [DF_InvOpeningBalance_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvOpeningBalance' AND column_name = 'OpeningDate')
BEGIN
	ALTER TABLE dbo.InvOpeningBalance
		ADD OpeningDate DATETIME NULL
END
GO

/****** Object:  Table [dbo].[InvOpeningBalanceDetail]    Script Date: 8/20/2019 5:34:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvOpeningBalanceDetail]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[InvOpeningBalanceDetail](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[InvOpeningBalanceId] [bigint] NOT NULL,
		[TransactionNodeId] [bigint] NOT NULL,
		[UnitCost] [decimal](18, 2) NULL,
		[StockQuantity] [decimal](18, 2) NULL,
		[Total] [decimal](18, 2) NULL,
	 CONSTRAINT [PK_InvOpeningBalanceDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvOpeningBalanceDetail' AND column_name = 'UnitHead')
BEGIN
	ALTER TABLE dbo.InvOpeningBalanceDetail
		ADD UnitHead VARCHAR(300) NULL
END
GO
/****** Object:  Table [dbo].[FADepreciation]    Script Date: 8/27/2019 6:07:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FADepreciation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FADepreciation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[FiscalYearId] [int] NOT NULL,
	[AccountHeadId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_FADepreciation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[FADepreciationDetails]    Script Date: 8/27/2019 6:09:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FADepreciationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FADepreciationDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DepreciationId] [bigint] NOT NULL,
	[TransactionNodeId] [bigint] NOT NULL,
	[DepreciationPercentage] [decimal](18, 2) NULL,
 CONSTRAINT [PK_FADepreciationDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
GO

/****** Object:  Table [dbo].[ContributionAnalysis]    Script Date: 08/30/2019 6:42:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContributionAnalysis]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContributionAnalysis](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SummaryDate] [datetime] NULL,
	[ContributionType] [nvarchar](100) NULL,
	[ContributionTypeWiseId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[TotalRoomSale] [decimal](18, 2) NULL,
	[AverageRoomRate] [decimal](18, 2) NULL,
	[OccupencyPercent] [decimal](18, 2) NULL,
	[Pax] [decimal](18, 2) NULL,
	[NoOfNight] [decimal](18, 2) NULL,
 CONSTRAINT [PK_ContributionAnalysis] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[ActivityLogDetails]    Script Date: 08/21/2019 7:06:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityLogDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityLogDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivityId] [bigint] NULL,
	[FieldName] [nvarchar](max) NULL,
	[PreviousData] [nvarchar](max) NULL,
	[CurrentData] [nvarchar](max) NULL,
	[DetailDescription] [nvarchar](max) NULL
 CONSTRAINT [PK_ActivityLogDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
GO
/****** Object:  Table [dbo].[ActivityLogDetailsSetup]    Script Date: 09/06/2019 12:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityLogDetailsSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityLogDetailsSetup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PageId] [nvarchar](250) NULL,
	[FieldName] [nvarchar](100) NULL,
	[IsSaveActivity] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ActivityLogDetailsSetup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[HotelCompanyPaymentTransectionDetails]    Script Date: 9/18/2019 5:22:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyPaymentTransectionDetails]') AND type in (N'U'))
BEGIN

	
CREATE TABLE [dbo].[HotelCompanyPaymentTransectionDetails](
	[PaymentTransectionId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentMode] [nvarchar](50) NULL,
	[PaymentHeadId] [int] NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[PaymentDate] [datetime] NULL,
	[CurrencyTypeId] [int] NULL,
	[ConvertionRate] [decimal](18, 2) NULL,
	[ChequeDate] [datetime] NULL,
	[ChequeNumber] [nvarchar](max) NULL,
	[HotelCompanyPaymentId] [bigint] NULL,
 CONSTRAINT [PK_HotelCompanyPaymentTransectionDetails] PRIMARY KEY CLUSTERED 
(
	[PaymentTransectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	
END
GO

/****** Object:  Table [dbo].[CashRequisition]    Script Date: 9/18/2019 5:22:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CashRequisition]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[CashRequisition](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[CompanyId] [int] NULL,
		[ProjectId] [int] NULL,
		[EmployeeId] [int] NULL,
		[Amount] [decimal](18, 2) NULL,
		[Remarks] [nvarchar](500) NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
		[TransactionType] [nvarchar](100) NULL,
		[ApproveStatus] [nvarchar](100) NULL,
		[CheckedBy] [int] NULL,
		[CheckedDate] [datetime] NULL,
		[ApproveBy] [int] NULL,
		[ApproveDate] [datetime] NULL,
		[RequsitionBy] [nvarchar](100) NULL,
		[RefId] [int] NULL,
		[RemainingBalance] [decimal](18, 2) NULL,
		[HaveCashRequisitionAdjustment] [bit] NULL,
	 CONSTRAINT [PK_CashRequisition] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisition' AND column_name = 'TransactionNo')
BEGIN
	ALTER TABLE dbo.CashRequisition
		ADD TransactionNo NVARCHAR(50) NULL 
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisition' AND column_name = 'TransactionFromAccountHeadId')
BEGIN
	ALTER TABLE dbo.CashRequisition
		ADD TransactionFromAccountHeadId INT NULL 
END
GO

/****** Object:  Table [dbo].[CashRequisitionDetails]    Script Date: 9/18/2019 5:26:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CashRequisitionDetails]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[CashRequisitionDetails](
		[CRDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
		[CashRequisitionId] [bigint] NULL,
		[RequisitionForHeadId] [int] NULL,
		[RequisitionAmount] [decimal](18, 2) NULL,
		[Remarks] [nvarchar](250) NULL,
		[RequisitionForHeadName] [nvarchar](250) NULL,
	 CONSTRAINT [PK_CashRequisitionDetails] PRIMARY KEY CLUSTERED 
	(
		[CRDetailsId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisitionDetails' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.CashRequisitionDetails
		ADD CompanyId INT NOT NULL CONSTRAINT DF_CashRequisitionDetails_CompanyId DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisitionDetails' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.CashRequisitionDetails
		ADD ProjectId INT NOT NULL CONSTRAINT DF_CashRequisitionDetails_ProjectId DEFAULT 0 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisitionDetails' AND column_name = 'SupplierId')
BEGIN
	ALTER TABLE dbo.CashRequisitionDetails
		ADD SupplierId INT NULL
END
GO


/****** Object:  Table [dbo].[PMMemberPayment]    Script Date: 09/19/2019 2:59:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMMemberPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMMemberPayment](
	[PaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[MemberBillId] [bigint] NULL,
	[PaymentFor] [nvarchar](50) NULL,
	[LedgerNumber] [nvarchar](50) NULL,
	[PaymentDate] [date] NOT NULL,
	[MemberId] [int] NOT NULL,
	[AdvanceAmount] [money] NULL,
	[Remarks] [nvarchar](250) NULL,
	[PaymentType] [nvarchar](50) NULL,
	[AccountingPostingHeadId] [int] NULL,
	[ChequeNumber] [nvarchar](50) NULL,
	[CurrencyId] [int] NULL,
	[ConvertionRate] [decimal](18, 2) NULL,
	[ApprovedStatus] [nvarchar](50) NULL,
	[AdjustmentType] [nvarchar](50) NULL,
	[MemberPaymentAdvanceId] [bigint] NULL,
	[AdjustmentAmount] [money] NULL,
	[AdjustmentAccountHeadId] [int] NULL,
	[PaymentAdjustmentAmount] [money] NULL,
 CONSTRAINT [PK_PMMemberPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

--PMMemberPaymentDetails
GO

/****** Object:  Table [dbo].[PMMemberPaymentDetails]    Script Date: 09/19/2019 4:39:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMMemberPaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMMemberPaymentDetails](
	[PaymentDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentId] [bigint] NOT NULL,
	[MemberBillDetailsId] [bigint] NULL,
	[MemberPaymentId] [bigint] NOT NULL,
	[BillId] [bigint] NOT NULL,
	[PaymentAmount] [money] NOT NULL,
 CONSTRAINT [PK_PMMemberPaymentDetails] PRIMARY KEY CLUSTERED 
(
	[PaymentDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

GO

/****** Object:  Table [dbo].[PMMemberBillGeneration]    Script Date: 09/17/2019 12:49:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMMemberBillGeneration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMMemberBillGeneration](
	[MemberBillId] [bigint] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[BillDate] [date] NOT NULL,
	[MemberBillNumber] [nvarchar](50) NOT NULL,
	[ApprovedStatus] [nvarchar](50) NULL,
	[BillStatus] [nvarchar](25) NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[BillCurrencyId] [int] NULL,
 CONSTRAINT [PK_PMMemberBillGeneration] PRIMARY KEY CLUSTERED 
(
	[MemberBillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
GO

/****** Object:  Table [dbo].[PMMemberBillGenerationDetails]    Script Date: 09/17/2019 12:53:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMMemberBillGenerationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMMemberBillGenerationDetails](
	[MemberBillDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[MemberBillId] [bigint] NOT NULL,
	[MemberPaymentId] [bigint] NOT NULL,
	[BillId] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[PaymentAmount] [money] NULL,
	[DueAmount] [money] NULL,
 CONSTRAINT [PK_PMMemberBillGenerationDetails] PRIMARY KEY CLUSTERED 
(
	[MemberBillDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[InvServiceFrequency]    Script Date: 9/26/2019 3:34:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvServiceFrequency]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[InvServiceFrequency](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Frequency] [int] NOT NULL,
	 CONSTRAINT [PK_InvServiceFrequency] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[DMDocumentMaster]    Script Date: 9/25/2019 3:09:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMDocumentMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DMDocumentMaster](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentName] [varchar](150) NULL,
	[Description] [varchar](MAX) NULL,
	[EmailReminderType] [varchar](50) NULL,
	[IsEmailReminderSent] [bit] NOT NULL,
	[EmailReminderDate] [date] NULL,
	[EmailReminderTime] [time](7) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CallToAction] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_DMDocumentMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[DMDocumentMaster] ADD  CONSTRAINT [DF_DMDocumentMaster_IsEmailReminderSent]  DEFAULT ((0)) FOR [IsEmailReminderSent]

SET ANSI_PADDING OFF
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'DMDocumentMaster' AND column_name = 'AssignType')
BEGIN
	ALTER TABLE dbo.DMDocumentMaster
		ADD AssignType NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'DMDocumentMaster' AND column_name = 'EmpDepartment')
BEGIN
	ALTER TABLE dbo.DMDocumentMaster
		ADD EmpDepartment INT NULL
END
GO

/****** Object:  Table [dbo].[DMDocumentAssignedEmployee]    Script Date: 9/26/2019 11:02:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMDocumentAssignedEmployee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DMDocumentAssignedEmployee](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentId] [bigint] NULL,
	[EmployeeId] [bigint] NULL,
 CONSTRAINT [PK_DMDocumentAssignedEmployee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[DocumentsDetalisForDocManagement]    Script Date: 9/24/2019 12:39:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentsDetalisForDocManagement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DocumentsDetalisForDocManagement](
	[DocumentId] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnerId] [bigint] NULL,
	[DocumentCategory] [varchar](100) NULL,
	[DocumentType] [varchar](100) NULL,
	[Extention] [varchar](50) NULL,
	[Name] [varchar](100) NULL,
	[DocumentName] [varchar](100) NULL,
	[Path] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[CreatedByDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentsDetalisForDocManagement] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'LCType')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD LCType VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'BankAccountId')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD BankAccountId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'Incoterms')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD Incoterms VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD CompanyId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD ProjectId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCPayment' AND column_name = 'AccountHeadName')
BEGIN
	ALTER TABLE dbo.LCPayment
		ADD AccountHeadName VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCPayment' AND column_name = 'ConvertionRate')
BEGIN
	ALTER TABLE dbo.LCPayment
		ADD ConvertionRate DECIMAL(18,2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCPayment' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE dbo.LCPayment
		ADD Remarks VARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCPayment' AND column_name = 'PaymentDate')
BEGIN
	ALTER TABLE dbo.LCPayment
		ADD PaymentDate DATETIME NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'StockBy')
BEGIN
	ALTER TABLE dbo.LCInformationDetail
		ADD StockBy VARCHAR(50) NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'LCValue')
BEGIN
	ALTER TABLE dbo.LCInformation
		DROP COLUMN LCValue 
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'LCValue')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD LCValue DECIMAL(18,2) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'PODId')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD PODId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'CNFId')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD CNFId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'TransactionType')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD TransactionType VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadName' AND column_name = 'IsCNFHead')
BEGIN
	ALTER TABLE dbo.LCOverHeadName
		ADD IsCNFHead BIT NOT NULL CONSTRAINT DF_LCOverHeadName_IsCNFHead DEFAULT 0 WITH VALUES
END
GO

GO
/****** Object:  Table [dbo].[LCCnfInfo]    Script Date: 10/10/2019 15:49:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCCnfInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LCCnfInfo](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
	[Name] [varchar](50) NOT NULL,
	[Code] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[WebAddress] [varchar](100) NULL,
	[ContactPerson] [varchar](100) NULL,
	[ContactEmail] [varchar](100) NULL,
	[ContactPhone] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[Balance] [decimal](18, 2) NULL CONSTRAINT [DF_LCCnfInfo_Balance]  DEFAULT ((0)),
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LCCnfInfo] PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
SET ANSI_PADDING OFF
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpReference' AND column_name = 'Description')
BEGIN
	ALTER TABLE dbo.PayrollEmpReference
		ADD Description VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'TinNumber')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD TinNumber VARCHAR(100) NULL
END
GO

/****** Object:  Table [dbo].[GLNodeMatrix]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNFTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE CNFTransaction
(
	Id	INT IDENTITY(1,1) NOT NULL,
	CNFId INT NOT NULL,
	TransactionType NVARCHAR(50) NOT NULL,
	PaymentMode	NVARCHAR(50) NOT NULL,
	PaymentDate	DATETIME NULL,
	ChequeNumber NVARCHAR(50) NULL,
	BankId	INT NULL,
	CurrencyId	INT NULL,
	PaymentAmount	DECIMAL(18, 2) NOT NULL,
	ConversionRate	DECIMAL(18, 2) NULL,
	TransactionNo NVARCHAR(50),
	Remarks	NVARCHAR(MAX)	NULL,
	[Status] [nvarchar](15) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CheckedBy] [int] NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
CONSTRAINT [pk_CNFTransaction] PRIMARY KEY CLUSTERED
(
	ID ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)ON [PRIMARY]
SET ANSI_PADDING OFF
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CNFTransaction' AND column_name = 'TransactionAccountHeadId')
BEGIN
	ALTER TABLE dbo.CNFTransaction
		ADD TransactionAccountHeadId INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItem' AND column_name = 'Model')
BEGIN
	ALTER TABLE dbo.InvItem
		ADD Model VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItem' AND column_name = 'CountryId')
BEGIN
	ALTER TABLE dbo.InvItem
		ADD CountryId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItem' AND column_name = 'IsAttributeItem')
BEGIN
	ALTER TABLE dbo.InvItem
	ADD IsAttributeItem BIT  NOT NULL
	CONSTRAINT [DF_PayrollEmpWorkStation_IsAttributeItem] DEFAULT ((0)) WITH VALUES

END
GO
/****** Object:  Table [dbo].[RateChartMaster]    Script Date: 04-Nov-19 12:41:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RateChartMaster]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RateChartMaster](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[PromotionName] [varchar](150) NULL,
		[CompanyId] [int] NULL,
		[EffectFrom] [datetime] NOT NULL,
		[EffectTo] [datetime] NOT NULL,
		[RateChartFor] [varchar](50) NULL,
		[Remarks] [varchar](200) NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_RateChartMaster] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RateChartMaster' AND column_name = 'TotalPrice')
BEGIN
	ALTER TABLE dbo.RateChartMaster
		ADD TotalPrice DECIMAL(18,2) NULL;
END
GO
/****** Object:  Table [dbo].[RateChartDetail]    Script Date: 04-Nov-19 12:40:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RateChartDetail]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RateChartDetail](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[RateChartMasterId] [bigint] NOT NULL,
		[ServiceType] [nvarchar](25) NOT NULL,
		[CategoryId] [int] NOT NULL,
		[ServicePackageId] [int] NULL,
		[ItemId] [int] NOT NULL,
		[StockBy] [int] NOT NULL,
		[Quantity] [decimal](18, 2) NOT NULL,
		[UnitPrice] [decimal](18, 2) NOT NULL,
		[TotalPrice] [decimal](18, 2) NOT NULL,
		[DiscountType] [varchar](20) NULL,
		[DiscountAmount] [decimal](18, 2) NULL,
		[DiscountAmountUSD] [decimal](18, 2) NULL,
		[IsDiscountForAll] [bit] NOT NULL,
		[ServiceTypeId] [int] NULL,
	 CONSTRAINT [PK_RateChartDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RateChartDetail] ADD  CONSTRAINT [DF_RateChartDetail_IsDiscountForAll]  DEFAULT ((1)) FOR [IsDiscountForAll]
END
GO

/****** Object:  Table [dbo].[RateChartDiscountDetail]    Script Date: 04-Nov-19 12:41:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RateChartDiscountDetail]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RateChartDiscountDetail](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[RateChartDetailId] [bigint] NULL,
		[OutLetId] [bigint] NULL,
		[Type] [varchar](50) NULL,
		[TypeId] [int] NULL,
		[DiscountType] [varchar](20) NULL,
		[DiscountAmount] [decimal](18, 2) NULL,
		[DiscountAmountUSD] [decimal](18, 2) NULL,
	 CONSTRAINT [PK_RateChartDiscountDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RateChartDiscountDetail' AND column_name = 'OfferredPrice')
BEGIN
	ALTER TABLE dbo.RateChartDiscountDetail
		ADD OfferredPrice DECIMAL(18,2) NULL;
END
GO
/****** Object:  Table [dbo].[InvItemStockInformationLog]    Script Date: 11/07/2019 11:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockInformationLog]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[InvItemStockInformationLog](
	[TransactionDate] [datetime] NOT NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NULL,
	[StockQuantity] [decimal](18, 5) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformationLog' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformationLog
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformationLog' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformationLog
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformationLog' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformationLog
		ADD ColorId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformationLog' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformationLog
		ADD SizeId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvItemStockInformationLog' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.InvItemStockInformationLog
		ADD StyleId INT NULL
END
GO
/****** Object:  Table [dbo].[HotelLostNFound]    Script Date: 10/30/2019 5:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelLostNFound]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelLostNFound](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TransectionType] [nvarchar](100) NULL,
	[TransectionId] [int] NULL,
	[OtherArea] [nvarchar](100) NULL,
	[ItemName] [nvarchar](200) NULL,
	[Description] [nvarchar](200) NULL,
	[ItemType] [nvarchar](50) NULL,
	[FoundDateTime] [datetime] NULL,
	[WhoFoundIt] [int] NULL,
	[WhoFoundItName] [nvarchar](200) NULL,
	[ReturnDate] [datetime] NULL,
	[WhomToReturn] [nvarchar](200) NULL,
	[ReturnDescription] [nvarchar](200) NULL,
	[HasItemReturned] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_HotelLostNFound] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[LCCnfPaymentLedger]    Script Date: 11/07/2019 11:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCCnfPaymentLedger]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LCCnfPaymentLedger](
	[SupplierPaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentType] [nvarchar](15) NOT NULL,
	[BillNumber] [varchar](25) NULL,
	[PaymentDate] [date] NOT NULL,
	[AccountsPostingHeadId] [int] NULL,
	[SupplierId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ConvertionRate] [money] NULL,
	[DRAmount] [money] NOT NULL,
	[CRAmount] [money] NOT NULL,
	[CurrencyAmount] [money] NULL,
	[Remarks] [varchar](500) NULL,
	[PaymentStatus] [varchar](20) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LCCnfPaymentLedger] PRIMARY KEY CLUSTERED 
(
	[SupplierPaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LCCnfPaymentLedgerClosingDetails]    Script Date: 11/07/2019 11:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCCnfPaymentLedgerClosingDetails]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LCCnfPaymentLedgerClosingDetails](
	[ClosingBalanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[YearClosingId] [bigint] NULL,
	[FiscalYearId] [int] NOT NULL,
	[CompanyId] [int] NULL,
	[ProjectId] [int] NULL,
	[DonorId] [int] NULL,
	[NodeId] [bigint] NOT NULL,
	[NodeHead] [nvarchar](500) NOT NULL,
	[ClosingDRAmount] [money] NOT NULL,
	[ClosingCRAmount] [money] NOT NULL,
	[ClosingBalance] [money] NOT NULL,
 CONSTRAINT [PK_LCCNFPaymentLedgerClosingDetails] PRIMARY KEY CLUSTERED 
(
	[ClosingBalanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[HotelRepairNMaintenance]    Script Date: 11/7/2019 5:35:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRepairNMaintenance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HotelRepairNMaintenance](
	[Id] [bigint]  IDENTITY(1,1) NOT NULL,
	[MaintenanceType] [nvarchar](25) NOT NULL,
	[ItemName] [nvarchar](max) NULL,
	[ItemId] [int] NULL,
	[Details] [nvarchar](max) NULL,
	[MaintenanceArea] [nvarchar](50) NOT NULL,
	[TransectionId] [int] NULL,
	[IsEmergency] [bit] NOT NULL,
	[ExpectedDate] [date] NULL,
	[ExpectedTime] [time](7) NULL,
	[RequestedById] [int] NULL,
	[RequestedByName] [nvarchar](max) NULL,
	[RepairNMaintenanceNo] [nvarchar](25) NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL
	CONSTRAINT [PK_HotelRepairNMaintenance] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[VMOverheadExpense]    Script Date: 11/27/2019 6:19:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VMOverheadExpense]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VMOverheadExpense](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[VehicleId] [bigint] NULL,
	[OverheadId] [bigint] NULL,
	[ExpenseDate] [DateTime] NULL,
	[ExpenseAmount] [Decimal](18,2) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [nvarchar](15) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CheckedBy] [int] NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL
 CONSTRAINT [PK_VMOverheadExpense] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'TransactionType')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD TransactionType varchar(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'TransactionNo')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
	ADD TransactionNo nvarchar(25) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'TransactionAccountHeadId')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense	
	ADD TransactionAccountHeadId int NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'PaymentMode')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD PaymentMode	NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'ChequeNumber')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD ChequeNumber NVARCHAR(50) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'BankId')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD BankId	INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'CurrencyId')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD CurrencyId	INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'ConversionRate')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD ConversionRate	DECIMAL(18, 2) NULL
END
GO

/****** Object:  Table [dbo].[VMVehicleType]    Script Date: 11/27/2019 6:19:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VMVehicleType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VMVehicleType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL
 CONSTRAINT [PK_VMVehicleType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO


/****** Object:  Table [dbo].[VMOverHead]    Script Date: 11/27/2019 6:19:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VMOverHead]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VMOverHead](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[VehicleId] [bigint] NULL,
	[OverheadName] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL
 CONSTRAINT [PK_VMOverHead] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverHead' AND column_name = 'VehicleId')
BEGIN
	EXEC sp_RENAME 'VMOverHead.VehicleId' , 'AccountHeadId', 'COLUMN'
END
GO
/****** Object:  Table [dbo].[VMManufacturer]    Script Date: 11/27/2019 6:19:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VMManufacturer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VMManufacturer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BrandName] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL
 CONSTRAINT [PK_VMManufacturer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[PayrollEmpAttendanceLogKjTech]    Script Date: 02-Dec-19 11:32:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAttendanceLogKjTech]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PayrollEmpAttendanceLogKjTech](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[EnrollNumber] [int] NULL,
		[Granted] [int] NULL,
		[Method] [int] NULL,
		[DoorMode] [int] NULL,
		[FunNumber] [int] NULL,
		[Sensor] [int] NULL,
		[EntryTime] [datetime] NULL,
	 CONSTRAINT [PK_PayrollEmpAttendanceLogKjTech] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[VMDriverInformation]    Script Date: 12/01/2019 10:53:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VMDriverInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VMDriverInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DriverName] [nvarchar](200) NULL,
	[DrivingLicenceNumber] [nvarchar](50) NULL,
	[DateOfBirth] [datetime] NULL,
	[NID] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[EmergancyContactPerson] [nvarchar](200) NULL,
	[EmergancyContactNumber] [nvarchar](50) NULL,
	[EmployeeId] [bigint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_VMDriverInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PMSupplierDetails]    Script Date: 12/8/2019 1:54:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMSupplierDetails](
	[SupplierDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NULL,
	[ContactPerson] [nvarchar](100) NULL,
	[ContactEmail] [nvarchar](50) NULL,
	[ContactPhone] [nvarchar](50) NULL,
	[ContactType] [nvarchar](50) NULL,
	[ContactAddress] [nvarchar](500) NULL,
 CONSTRAINT [PK_PMSupplierDetails] PRIMARY KEY CLUSTERED 
(
	[SupplierDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RFQuotation]    Script Date: 12/8/2019 2:21:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQuotation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RFQuotation](
	[RFQId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](300) NULL,
	[IndentName] [nvarchar](300) NULL,
	[PaymentTerm] [nvarchar](300) NULL,
	[CreditDays] [decimal](18, 2) NULL,
	[DeliveryTerms] [nvarchar](300) NULL,
	[SiteAddress] [nvarchar](300) NULL,
	[ExpireDateTime] [datetime] NULL,
	[VAT] [decimal](18, 2) NULL,
	[AIT] [decimal](18, 2) NULL,
	[IndentPurpose] [nvarchar](300) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[StoreID] [int] NULL,
 CONSTRAINT [PK_RFQuotation] PRIMARY KEY CLUSTERED 
(
	[RFQId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityUserInformation' AND column_name = 'SupplierId')
BEGIN
	ALTER TABLE dbo.SecurityUserInformation
		ADD SupplierId INT NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SecurityUserInformation' AND column_name = 'UserSignature')
BEGIN
	ALTER TABLE dbo.SecurityUserInformation
		ADD UserSignature NVARCHAR(MAX) NULL;
END
GO
/****** Object:  Table [dbo].[RFQuotationFeedback]    Script Date: 12/8/2019 2:24:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQuotationFeedback]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RFQuotationFeedback](
	[RFQSupplierId] [int] IDENTITY(1,1) NOT NULL,
	[TotalItemQuoted] [int] NULL,
	[QuotedAmount] [decimal](18, 2) NULL,
	[ApplicableVatAit] [decimal](18, 2) NULL,
	[DeliveryCost] [decimal](18, 2) NULL,
	[TotalBillingAmount] [decimal](18, 2) NULL,
	[AdditionalInformation] [nvarchar](300) NULL,
	[QuotedBy] [int] NULL,
	[QuotedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[SupplierId] [int] NULL,
	[RFQId] [int] NULL,
 CONSTRAINT [PK_RFQuotationFeedback] PRIMARY KEY CLUSTERED 
(
	[RFQSupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RFQuotationItemDetails]    Script Date: 12/8/2019 2:26:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQuotationItemDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RFQuotationItemDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](300) NULL,
	[Value] [nvarchar](300) NULL,
	[RFQItemId] [int] NULL,
 CONSTRAINT [PK_RFQuotationItemDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RFQuotationItemDetailsFeedback]    Script Date: 12/8/2019 2:28:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQuotationItemDetailsFeedback]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RFQuotationItemDetailsFeedback](
	[FeedbackId] [int] IDENTITY(1,1) NOT NULL,
	[RFQSupplierItemId] [int] NULL,
	[RFQuotationItemDetailsId] [int] NULL,
	[Feedback] [nvarchar](300) NULL,
 CONSTRAINT [PK_RFQuotationItemDetailsFeedback] PRIMARY KEY CLUSTERED 
(
	[FeedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RFQuotationItems]    Script Date: 12/8/2019 2:30:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQuotationItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RFQuotationItems](
	[RFQItemId] [int] IDENTITY(1,1) NOT NULL,
	[RFQId] [int] NULL,
	[ItemId] [int] NULL,
	[StockUnit] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RFQuotationItems] PRIMARY KEY CLUSTERED 
(
	[RFQItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RFQuotationItemsFeedback]    Script Date: 12/8/2019 2:32:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQuotationItemsFeedback]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RFQuotationItemsFeedback](
	[RFQSupplierItemId] [int] IDENTITY(1,1) NOT NULL,
	[RFQSupplierId] [int] NULL,
	[ItemId] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[Discount] [decimal](18, 2) NULL,
	[OfferedUnitPrice] [decimal](18, 2) NULL,
	[OfferedUnitPriceWithVatAit] [decimal](18, 2) NULL,
	[BillingAmount] [decimal](18, 2) NULL,
	[AdvanceAmount] [decimal](18, 2) NULL,
	[OfferValidation] [int] NULL,
	[DeliveryDuration] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[StockUnit] [int] NULL,
	[RFQItemId] [int] NULL,
	[ItemRemarks] [nvarchar](300) NULL,
 CONSTRAINT [PK_RFQuotationItemsFeedback] PRIMARY KEY CLUSTERED 
(
	[RFQSupplierItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[RFQuotationSuppliers]    Script Date: 12/8/2019 2:33:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQuotationSuppliers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RFQuotationSuppliers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NULL,
	[RFQId] [int] NULL,
 CONSTRAINT [PK_RFQuotationSuppliers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[VMVehicleInformation]    Script Date: 12/01/2019 10:53:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VMVehicleInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VMVehicleInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[VehicleName] [nvarchar](100) NULL,
	[AccountHeadId] [bigint] NULL,
	[ManufacturerId] [bigint] NULL,
	[ModelName] [nvarchar](100) NULL,
	[ModelYear] [int] NULL,
	[TaxValidationYear] [int] NULL,
	[Fare] [decimal](18, 2) NULL,
	[PassengerCapacity] [int] NULL,
	[Status] [bit] NULL,
	[VehicleTypeId] [int] NULL,
	[NumberPlate] [nvarchar](100) NULL,
	[IsABSEnable] [bit] NULL,
	[IsAirBagAvailable] [bit] NULL,
	[TaxNumber] [nvarchar](50) NULL,
	[InsuranceNumber] [nvarchar](50) NULL,
	[AirConditioningType] [nvarchar](50) NULL,
	[Mileage] [decimal](18, 2) NULL,
	[FuelType] [nvarchar](50) NULL,
	[FuelTankCapacity] [decimal](18, 2) NULL,
	[EngineCapacity] [decimal](18, 2) NULL,
	[EngineType] [nvarchar](50) NULL,
	[BodyType] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_VMVehicleInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[PayrollEmpAttendanceLogActatec]    Script Date: 11-Dec-19 2:43:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAttendanceLogActatek]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PayrollEmpAttendanceLogActatek](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[logIDField] [bigint] NULL,
		[userIDField] [varchar](50) NULL,
		[userNameField] [nvarchar](250) NULL,
		[departmentNameField] [varchar](250) NULL,
		[timestampField] [datetime] NULL,
		[terminalSNField] [varchar](250) NULL,
		[accessMethodField] [varchar](250) NULL,
		[triggerField] [varchar](50) NULL,
	 CONSTRAINT [PK_PayrollEmpAttendanceLogActatek] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

---add column POStatus
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisition' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.CashRequisition
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisition' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.CashRequisition
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpAttendance' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpAttendance
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmpLeaveInformation' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PayrollEmpLeaveInformation
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.LCInformation
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCOverHeadExpense' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.LCOverHeadExpense
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CNFTransaction' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.CNFTransaction
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CNFTransaction' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.CNFTransaction
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.GLLedgerMaster
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GLLedgerMaster' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.GLLedgerMaster
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'Remarks' AND DATA_TYPE = 'varchar')
BEGIN
       ALTER TABLE dbo.PayrollEmployee
	   ALTER COLUMN Remarks VARCHAR(MAX);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployee' AND column_name = 'EmergencyContactNumberHome')
BEGIN
	ALTER TABLE dbo.PayrollEmployee
		ADD EmergencyContactNumberHome NVARCHAR(50) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GatePass' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.GatePass
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'GatePass' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.GatePass
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO


/****** Object:  Table [dbo].[PayrollEmpInformationForAttendanceDevice]    Script Date: 19-Dec-19 1:25:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpInformationForAttendanceDevice]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PayrollEmpInformationForAttendanceDevice](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[DeviceId] [int] NULL,
		[EmpId] [int] NULL,
		[EmpCode] [varchar](500) NULL,
		[EmployeeName] [varchar](500) NULL,
	 CONSTRAINT [PK_PayrollEmpInformationForAttendanceDevice] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PayrollEmployeeAttendanceDeviceMapping]    Script Date: 19-Dec-19 1:31:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeAttendanceDeviceMapping]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PayrollEmployeeAttendanceDeviceMapping](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[DeviceId] [varchar](50) NULL,
		[EmployeeId] [int] NULL,
		[MappingEmployeeCode] [varchar](250) NULL,
	 CONSTRAINT [PK_PayrollEmployeeAttendanceDeviceMapping] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollEmployeeAttendanceDeviceMapping' AND column_name = 'DeviceId' )
BEGIN
	ALTER TABLE dbo.PayrollEmployeeAttendanceDeviceMapping
		ALTER COLUMN DeviceId VARCHAR(50) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CNFTransaction' AND column_name = 'Status' )
BEGIN
	ALTER TABLE dbo.CNFTransaction
		ALTER COLUMN Status NVARCHAR(100) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'ApprovedStatus' )
BEGIN
	ALTER TABLE dbo.LCInformation
		ALTER COLUMN ApprovedStatus NVARCHAR(100) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'VMOverheadExpense' AND column_name = 'Status' )
BEGIN
	ALTER TABLE dbo.VMOverheadExpense
		ALTER COLUMN Status NVARCHAR(100) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ADD CheckedByUsers VARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ADD ApprovedByUsers VARCHAR(MAX) NULL;
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrder' AND column_name = 'ApprovedStatus' )
BEGIN
	ALTER TABLE dbo.PMPurchaseOrder
		ALTER COLUMN ApprovedStatus NVARCHAR(50) NULL;
END
GO

/****** Object:  Table [dbo].[TermsNConditionsMaster]    Script Date: 1/7/2020 7:30:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TermsNConditionsMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TermsNConditionsMaster](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[DisplaySequence] [int] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_TermsNConditionsMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[TermsNConditionsDetails]    Script Date: 1/7/2020 7:29:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TermsNConditionsDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TermsNConditionsDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TermsNConditionsId] [bigint] NOT NULL,
	[ConditionForID] [int] NOT NULL,
 CONSTRAINT [PK_TermsNConditionsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PMPurchaseOrderTermsNConditions]    Script Date: 1/9/2020 4:53:23 PM ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMPurchaseOrderTermsNConditions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMPurchaseOrderTermsNConditions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TermsNConditionsId] [bigint] NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[Title] [nvarchar](200) NULL,
	[DisplaySequence] [int] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_PMPurchaseOrderTermsNConditions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

GO

/****** Object:  Table [dbo].[SMContactDetails]    Script Date: 01/09/2020 3:42:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMContactDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMContactDetails](
	[DetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentType] [nvarchar](100) NULL,
	[ParentId] [bigint] NULL,
	[TransectionType] [nvarchar](100) NULL,
	[Title] [nvarchar](150) NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_SMContactDetails] PRIMARY KEY CLUSTERED 
(
	[DetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactDetails' AND column_name = 'ContactTitleId')
BEGIN
	ALTER TABLE dbo.SMContactDetails
		ADD ContactTitleId INT
END
GO

/****** Object:  Table [dbo].[SMContactDetailsTitle]    Script Date: 01/09/2020 3:42:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMContactDetailsTitle]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMContactDetailsTitle](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TransectionType] [nvarchar](100) NULL,
	[Title] [nvarchar](max) NULL,
	[Status] bit
 CONSTRAINT [PK_SMContactDetailsTitle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactDetailsTitle' AND column_name = 'CreatedBy')
BEGIN
	ALTER TABLE dbo.SMContactDetailsTitle
		ADD CreatedBy INT
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactDetailsTitle' AND column_name = 'CreatedDate')
BEGIN
	ALTER TABLE dbo.SMContactDetailsTitle
		ADD CreatedDate DATETIME
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactDetailsTitle' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.SMContactDetailsTitle
		ADD LastModifiedBy INT
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SMContactDetailsTitle' AND column_name = 'LastModifiedDate')
BEGIN
	ALTER TABLE dbo.SMContactDetailsTitle
		ADD LastModifiedDate DATETIME
END
GO
/****** Object:  Table [dbo].[PayrollEmployeeBalanceTransfer]    Script Date: 1/15/2020 3:08:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeBalanceTransfer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmployeeBalanceTransfer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TransferFrom] [bigint] NULL,
	[TransferTo] [bigint] NULL,
	[TransferAmount] [decimal](18, 2) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [nvarchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CheckedBy] [int] NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,	
	[CheckedByUsers] [varchar](max) NULL,
	[ApprovedByUsers] [varchar](max) NULL,
 CONSTRAINT [PK_PayrollEmployeeBalanceTransfer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisition' AND column_name = 'RequireDate')
BEGIN
	ALTER TABLE dbo.CashRequisition
		ADD RequireDate DATETIME NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CashRequisition' AND column_name = 'LastAdjustmentDate')
BEGIN
	ALTER TABLE dbo.CashRequisition
		ADD LastAdjustmentDate DATETIME NULL
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PayrollLoanHoldup' AND column_name = 'OverDueAmount' )
BEGIN
	ALTER TABLE dbo.PayrollLoanHoldup
		ALTER COLUMN OverDueAmount decimal(18, 2) NOT NULL
END
GO


/****** Object:  Table [dbo].[PayrollLeaveDeductionPolicyDetails]    Script Date: 2/24/2020 4:00:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollLeaveDeductionPolicyDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollLeaveDeductionPolicyDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MasterId] [bigint] NOT NULL,
	[LeaveId] [int] NOT NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [PK_PayrollLeaveDeductionPolicyDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[PayrollLeaveDeductionPolicyMaster]    Script Date: 2/24/2020 3:58:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollLeaveDeductionPolicyMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollLeaveDeductionPolicyMaster](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NoOfLate] [bigint] NOT NULL,
	[NoOfLeave] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollLeaveDeductionPolicyMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[PayrollLeaveDeductionPolicyMaster] ADD  CONSTRAINT [DF_PayrollLeaveDeductionPolicyMaster_IsActive]  DEFAULT ((1)) FOR [IsActive]
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'Remarks')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
		ADD Remarks VARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'RemainingDeliveredQuantity')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
		ADD RemainingDeliveredQuantity Decimal(18,2) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantKotBillDetail' AND column_name = 'DeliveredQuantity')
BEGIN
	ALTER TABLE dbo.RestaurantKotBillDetail
		ADD DeliveredQuantity Decimal(18,2) NULL
END
GO

GO
/****** Object:  Table [dbo].[TemplateInformation]    Script Date: 02/26/20 4:30:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemplateInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TemplateInformation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NULL,
	[TemplateForId] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Subject] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
 CONSTRAINT [PK_TemplateInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
GO
/****** Object:  Table [dbo].[TemplateInformationDetails]    Script Date: 02/26/20 5:25:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemplateInformationDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TemplateInformationDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TemplateId] [bigint] NULL,
	[BodyText] [nvarchar](max) NULL,
	[ReplacedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_TemplateInformationDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
GO

/****** Object:  Table [dbo].[TemplateEmail]    Script Date: 03/08/20 11:31:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemplateEmail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TemplateEmail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TemplateType] [nvarchar](max) NULL,
	[TemplateBody] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_TemplateEmail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[TemplateEmailDetails]    Script Date: 03/08/20 11:31:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemplateEmailDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TemplateEmailDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TemplateEmailId] [bigint] NULL,
	[EmployeeId] [int] NULL,
 CONSTRAINT [PK_TemplateEmailDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateEmailDetails' AND column_name = 'TemplateType')
BEGIN
	ALTER TABLE dbo.TemplateEmailDetails
	ADD TemplateType NVARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateInformation' AND column_name = 'TemplateFor')
BEGIN
	ALTER TABLE dbo.TemplateInformation
		ADD [TemplateFor] NVARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateInformation' AND column_name = 'Type')
BEGIN
	ALTER TABLE dbo.TemplateInformation
		ADD [Type] NVARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateInformation' AND column_name = 'CreatedBy')
BEGIN
	ALTER TABLE dbo.TemplateInformation
		ADD [CreatedBy] INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateInformation' AND column_name = 'CreatedDate')
BEGIN
	ALTER TABLE dbo.TemplateInformation
		ADD CreatedDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateInformation' AND column_name = 'LastModifiedBy')
BEGIN
	ALTER TABLE dbo.TemplateInformation
		ADD [LastModifiedBy] INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateInformation' AND column_name = 'LastModifiedDate')
BEGIN
	ALTER TABLE dbo.TemplateInformation
		ADD LastModifiedDate DATETIME NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateInformationDetails' AND column_name = 'TableName')
BEGIN
	ALTER TABLE dbo.TemplateInformationDetails
	ADD TableName NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateEmail' AND column_name = 'AssignType')
BEGIN
	ALTER TABLE dbo.TemplateEmail
	ADD AssignType NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TemplateEmail' AND column_name = 'TemplateId')
BEGIN
	ALTER TABLE dbo.TemplateEmail
	ADD TemplateId BIGINT NULL
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'PurchasePrice')
BEGIN
ALTER TABLE dbo.LCInformationDetail
	   ALTER COLUMN PurchasePrice DECIMAL(18,5) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'Quantity')
BEGIN
ALTER TABLE dbo.LCInformationDetail
	   ALTER COLUMN Quantity DECIMAL(18,5) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'QuantityReceived')
BEGIN
ALTER TABLE dbo.LCInformationDetail
	   ALTER COLUMN QuantityReceived DECIMAL(18,5) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'RemainingReceiveQuantity')
BEGIN
ALTER TABLE dbo.LCInformationDetail
	   ALTER COLUMN RemainingReceiveQuantity DECIMAL(18,5) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformationDetail' AND column_name = 'ActualReceivedQuantity')
BEGIN
ALTER TABLE dbo.LCInformationDetail
	   ALTER COLUMN ActualReceivedQuantity DECIMAL(18,5) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCInformation' AND column_name = 'LCValue')
BEGIN
ALTER TABLE dbo.LCInformation
	   ALTER COLUMN LCValue DECIMAL(18,5) NULL;
END
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCPayment' AND column_name = 'Amount')
BEGIN
ALTER TABLE dbo.LCPayment
	   ALTER COLUMN Amount DECIMAL(18,5) NULL;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'LCPayment' AND column_name = 'ConvertionRate')
BEGIN
ALTER TABLE dbo.LCPayment
	   ALTER COLUMN ConvertionRate DECIMAL(18,5) NULL;
END
GO


/****** Object:  Table [dbo].[InvItemStockAdjustmentSerialInfo]    Script Date: 3/29/2020 4:06:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemStockAdjustmentSerialInfo]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[InvItemStockAdjustmentSerialInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StockAdjustmentId] [int] NOT NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NULL,
	[SerialNumber] [nvarchar](250) NULL,
 CONSTRAINT [PK_InvItemStockAdjustmentSerialInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonBank' AND column_name = 'AccountName')
BEGIN
	ALTER TABLE dbo.CommonBank
		ADD AccountName NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonBank' AND column_name = 'BranchName')
BEGIN
	ALTER TABLE dbo.CommonBank
		ADD BranchName NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonBank' AND column_name = 'AccountNumber')
BEGIN
	ALTER TABLE dbo.CommonBank
		ADD AccountNumber NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonBank' AND column_name = 'AccountType')
BEGIN
	ALTER TABLE dbo.CommonBank
		ADD AccountType NVARCHAR(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonBank' AND column_name = 'Description')
BEGIN
	ALTER TABLE dbo.CommonBank
		ADD Description NVARCHAR(MAX) NULL
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CommonBank' AND column_name = 'BankHeadId')
BEGIN
	ALTER TABLE dbo.CommonBank
		ADD BankHeadId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'PaymentInstructionId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD PaymentInstructionId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'ContactId')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD ContactId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'Subject')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD Subject NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'RestaurantBill' AND column_name = 'BillType')
BEGIN
	ALTER TABLE dbo.RestaurantBill
		ADD BillType nvarchar(50) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD CompanyId INT  NULL DEFAULT 1 WITH VALUES
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMProductReceived' AND column_name = 'ProjectId')
BEGIN
	ALTER TABLE dbo.PMProductReceived
		ADD ProjectId INT  NULL DEFAULT 1 WITH VALUES
END
GO
/****** Object:  Table [dbo].[STSupportNCaseSetupInfo]    Script Date: 13/05/2020 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[STSupportNCaseSetupInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[STSupportNCaseSetupInfo](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SetupType] [nvarchar](100) NULL,
	[Status] [bit] NOT NULL,	
	[PriorityLabel] [int] NULL,
	[IsDeclineStage] [bit] NULL,
	[IsCloseStage] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_STSupportNCaseSetupInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[STSupportNCaseSetupInfo] ADD  CONSTRAINT [DF_STSupportNCaseSetupInfo_Status]  DEFAULT ((1)) FOR [Status]
END
GO
/****** Object:  Table [dbo].[SecurityUserAdminAuthorization]    Script Date: 3/29/2020 4:06:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityUserAdminAuthorization]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SecurityUserAdminAuthorization](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[UserInfoId] [int] NULL,
		[ModuleId] [int] NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL
	 CONSTRAINT [PK_SecurityUserAdminAuthorization] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
/****** Object:  Table [dbo].[CommonState]    Script Date: 6/2/2020 10:58:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonState]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonState](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StateName] [nvarchar](max) NULL,
	[CountryId] [int] NULL,
 CONSTRAINT [PK_CommonState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[STSupportDetails]    Script Date: 9/18/2019 5:22:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[STSupportDetails]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[STSupportDetails](
		[STSupportDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
		[ItemId] [int] NULL,
		[CategoryId] [int] NULL,
		[StockBy] [int] NULL,
		[Type] [nvarchar](max) NULL,
		[STSupportId] [int] NULL,
		[UnitPrice] [decimal](18, 2) NULL,
	 CONSTRAINT [PK_STSupportDetails] PRIMARY KEY CLUSTERED 
	(
		[STSupportDetailsId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupportDetails' AND column_name = 'UnitQuantity')
BEGIN	
		
	ALTER TABLE dbo.STSupportDetails
		ADD UnitQuantity DECIMAL(18, 2) NOT NULL DEFAULT 1 WITH VALUES;	
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupportDetails' AND column_name = 'VatRate')
BEGIN
	ALTER TABLE dbo.STSupportDetails
	ADD VatRate DECIMAL(18, 2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupportDetails' AND column_name = 'VatAmount')
BEGIN
	ALTER TABLE dbo.STSupportDetails
	ADD VatAmount DECIMAL(18, 2) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupportDetails' AND column_name = 'TotalPrice')
BEGIN	
		
	ALTER TABLE dbo.STSupportDetails
		ADD TotalPrice DECIMAL(18, 2) NOT NULL ;	
END
GO
/****** Object:  Table [dbo].[STSupport]    Script Date: 9/18/2019 5:22:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[STSupport]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[STSupport](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[CaseNumber] [nvarchar](50) NULL,
		[CaseOwnerId] [int] NULL,
		[ClientId] [int] NULL,
		[SupportCategoryId] [int] NULL,
		[SupportSource] [nvarchar](100) NULL,
		[SupportSourceOtherDetails] [nvarchar](max) NULL,
		[CaseId] [int] NULL,
		[CaseDetails] [nvarchar](max) NULL,
		[ItemOrServiceDetails] [nvarchar](max) NULL,
		[SupportStageId] [int] NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
		[InternalNotesDetails] [nvarchar](max) NULL,
		[SupportTypeId] [int] NULL,
		[SupportPriorityId] [int] NULL,
		[SupportForwardToId] [int] NULL,
		[SupportDeadline] [datetime] NULL,
		[BillConfirmation] [nvarchar](max) NULL,
	 CONSTRAINT [PK_STSupport] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'BillStatus')
BEGIN
	ALTER TABLE dbo.STSupport
	ADD BillStatus NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'FeedbackDate')
BEGIN
	ALTER TABLE dbo.STSupport
		ADD FeedbackDate DATETIME NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'SupportStatus')
BEGIN
	ALTER TABLE dbo.STSupport
		ADD SupportStatus [nvarchar](100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'Feedback')
BEGIN
	ALTER TABLE dbo.STSupport
		ADD Feedback [nvarchar](MAX) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TMTaskAssignedEmployee' AND column_name = 'ImplementStatus')
BEGIN
	ALTER TABLE dbo.TMTaskAssignedEmployee
		ADD ImplementStatus BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TMTaskAssignedEmployee' AND column_name = 'ImplementionTime')
BEGIN
	ALTER TABLE dbo.TMTaskAssignedEmployee
		ADD ImplementionTime TIME(7) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'TMTaskAssignedEmployee' AND column_name = 'ImplementionDate')
BEGIN
	ALTER TABLE dbo.TMTaskAssignedEmployee
		ADD ImplementionDate DATETIME NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'BillGenerateBy')
BEGIN
	ALTER TABLE dbo.STSupport
		ADD BillGenerateBy INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'BillGenerateDate')
BEGIN
	ALTER TABLE dbo.STSupport
		ADD BillGenerateDate DATETIME NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'PaymentInstructionId')
BEGIN
	ALTER TABLE STSupport  
		ADD PaymentInstructionId INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'STSupport' AND column_name = 'ContactId')
BEGIN
	ALTER TABLE STSupport  
		ADD ContactId INT NULL;
END
GO
/****** Object:  Table [dbo].[STSupportPriceMatrixSetup]    Script Date: 5/13/2020 5:08:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[STSupportPriceMatrixSetup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[STSupportPriceMatrixSetup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[ItemId] [int] NOT NULL,
	[Price] [decimal](18, 2) NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_STSupportPriceMatrixSetup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[STSupportPriceMatrixSetup] ADD  CONSTRAINT [DF_STSupportPriceMatrixSetup_Status]  DEFAULT ((1)) FOR [Status]
END
GO

/****** Object:  Table [dbo].[AccountManager]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountManager]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountManager](
	[AccountManagerId] [int] IDENTITY(1,1) NOT NULL,
	[EmpId] [int] NOT NULL,
	[Lvl] [int] NOT NULL,
	[AncestorId] [int] NULL,
	[Hierarchy] [varchar](900) NULL,
	[HierarchyIndex] [varchar](900) NULL,
	[Type] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_AccountManager] PRIMARY KEY CLUSTERED 
(
	[AccountManagerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollMonthlyAttendanceProcessLog]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollMonthlyAttendanceProcessLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollMonthlyAttendanceProcessLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProcessFromDate] [datetime] NULL,
	[ProcessToDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_PayrollMonthlyAttendanceProcessLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[SMLogKeepingBackup]    Script Date: 9/20/2020 1:03:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMLogKeepingBackup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMLogKeepingBackup](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[LogKeepingId] [bigint] NULL,
		[Type] [nvarchar](50) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[LogDateTime] [datetime] NOT NULL,
		[CompanyId] [int] NULL,
		[ContactId] [bigint] NULL,
		[DealId] [bigint] NULL,
		[SalesCallEntryId] [bigint] NULL,
		[CreatedBy] [int] NOT NULL,
		[LastModifiedBy] [bigint] NULL,
	 CONSTRAINT [PK_SMLogKeepingBackup] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO



/****** Object:  Table [dbo].[SMLogKeepingContact]    Script Date: 9/20/2020 3:10:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMLogKeepingContact]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMLogKeepingContact](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LogKeepingId] [bigint] NULL,
	[ContactId] [bigint] NULL,
 CONSTRAINT [PK_SMLogKeepingContact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[CallToAction]    Script Date: 3/12/2019 3:20:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CallToAction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CallToAction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MasterId] [bigint] NULL,
	[FromCallToAction] [nvarchar](100) NULL,
	[ContactId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CallToAction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[CallToAction] ADD  CONSTRAINT [DF_CallToAction_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

END
GO

/****** Object:  Table [dbo].[CallToActionDetails]    Script Date: 3/12/2019 3:20:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CallToActionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CallToActionDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CallToActionId] [bigint] NULL,
	[Type] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[Time] [time](7) NULL,
	[OtherActivities] [nvarchar](MAX) NULL,
	[Description] [nvarchar](MAX) NULL,

 CONSTRAINT [PK_CallToActionDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CallToActionDetails' AND column_name = 'ContactId')
BEGIN
	ALTER TABLE dbo.CallToActionDetails
		ADD ContactId BIGINT NULL
END
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'CallToActionDetails' AND column_name = 'CompanyId')
BEGIN
	ALTER TABLE dbo.CallToActionDetails
		ADD CompanyId BIGINT NULL
END
GO 
/****** Object:  Table [dbo].[CallToActionParticipant]    Script Date: 3/12/2019 3:20:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CallToActionParticipant]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CallToActionParticipant](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CallToActionDetailsId] [bigint] NOT NULL,
	[PrticipantType] [nvarchar](50) NOT NULL,
	[ContactId] [bigint] NOT NULL,
 CONSTRAINT [PK_CallToActionParticipant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[CallToActionReminder]    Script Date: 3/12/2019 3:20:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CallToActionReminder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CallToActionReminder](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CallToActionDetailsId] [bigint] NULL,
	[Reminder] [bigint] NULL,

 CONSTRAINT [PK_CallToActionReminder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/***** Object:  Table [dbo].[InvItemAttribute]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemAttribute]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemAttribute](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[SetupType] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvItemAttribute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


/***** Object:  Table [dbo].[InvItemAttributeMapping]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemAttributeMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvItemAttributeMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[AttributeId] [int] NULL,
	[ItemId] [int] NULL,
 CONSTRAINT [PK_InvItemAttributeMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'ColorId')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrderDetails
		ADD ColorId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'SizeId')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrderDetails
		ADD SizeId INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'PMPurchaseOrderDetails' AND column_name = 'StyleId')
BEGIN
	ALTER TABLE dbo.PMPurchaseOrderDetails
		ADD StyleId INT NULL
END
GO
/***** Object:  Table [dbo].[RestaurantSalesReturn]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantSalesReturn]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantSalesReturn](
	[ReturnId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReturnDate] [datetime] NOT NULL,
	[ReturnType] [nvarchar](25) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[Remarks] [varchar](MAX) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ReturnNumber] [nvarchar](25) NOT NULL,
	[FromCostCenterId] [int] NOT NULL,
	[FromLocationId] [int] NOT NULL,
	[Status] [nvarchar](25) NOT NULL,
	[CheckedBy] [int] NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RestaurantSalesReturn] PRIMARY KEY CLUSTERED 
(
	[ReturnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/***** Object:  Table [dbo].[RestaurantSalesReturnDetails]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantSalesReturnDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantSalesReturnDetails](
	[ReturnDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReturnId] [bigint] NOT NULL,
	[StockById] [int] NOT NULL,
	[KotDetailId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[OrderQuantity] [decimal](18, 2) NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[AverageCost] [decimal](18, 2) NULL,
	[ServiceRate] [decimal](18, 2) NULL,
	[ServiceCharge] [decimal](18, 2) NULL,
	[VatAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RestaurantSalesReturnDetails] PRIMARY KEY CLUSTERED 
(
	[ReturnDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[SDCInvoiceInformation]    Script Date: 12/28/2020 5:19:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SDCInvoiceInformation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SDCInvoiceInformation](
	[SDCInvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[BillId] [int] NULL,
	[SDCInvoiceNumber] [varchar](50) NULL,
	[QRCode] [varchar](900) NULL,
 CONSTRAINT [PK_SDCInvoiceInformation] PRIMARY KEY CLUSTERED 
(
	[SDCInvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'SDCInvoiceInformation' AND column_name = 'BillType')
BEGIN
	ALTER TABLE dbo.SDCInvoiceInformation
		ADD BillType VARCHAR(255) NULL
END
GO


/****** Object:  Table [dbo].[TaskParticipant]    Script Date: 1/15/2021 4:17:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskParticipant]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TaskParticipant](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskId] [bigint] NOT NULL,
	[ParticipantType] [nvarchar](50) NOT NULL,
	[ContactId] [bigint] NOT NULL,
 CONSTRAINT [PK_TaskParticipant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO

/***** Object:  Table [dbo].[SMTaskFeedback]    Script Date: 1/17/2021 6:42:32 PM *****/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMTaskFeedback]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMTaskFeedback](
		[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
		[TaskId] [BIGINT] NOT NULL,
		[EmployeeId] [INT] NOT NULL,
		[ImplementationStatus] [NVARCHAR](100) NOT NULL,
		[TaskStage] [INT] NOT NULL,
		[TaskFeedback] [NVARCHAR](MAX) NULL,
		[StartDate] [DATETIME] NOT NULL,
		[StartTime] [TIME](7) NOT NULL,
		[FinishDate] [DATETIME] NOT NULL,
		[FinishTime] [TIME](7) NOT NULL,
		[MeetingAgenda] [NVARCHAR](MAX) NULL,
		[MeetingLocation] [NVARCHAR](MAX) NULL,
		[MeetingDiscussion] [NVARCHAR](MAX) NULL,
		[CallToAction] [NVARCHAR](MAX) NULL,
	 CONSTRAINT [PK_SMTaskFeedback] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

/***** Object:  Table [dbo].[SMTaskFeedbackParticipant]    Script Date: 1/17/2021 6:30:32 PM *****/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMTaskFeedbackParticipant]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SMTaskFeedbackParticipant](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[FeedbackId] [bigint] NOT NULL,
		[ParticipantId] [int] NOT NULL,
		[ParticipantType] [varchar](100) NOT NULL,
	 CONSTRAINT [PK_SMTaskFeedbackParticipant] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO
/***** Object:  Table [dbo].[InvProduction]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvProduction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvProduction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductionDate] [datetime] NOT NULL,
	[CostCenterId] [int] NULL,
	[ApprovedStatus] [nvarchar](25) NOT NULL,
	[Remarks] [nvarchar](MAX) NOT NULL,	
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvProduction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProduction' AND column_name = 'CheckedBy')
BEGIN
	ALTER TABLE dbo.InvProduction
		ADD CheckedBy INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProduction' AND column_name = 'ApprovedBy')
BEGIN
	ALTER TABLE dbo.InvProduction
		ADD ApprovedBy INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProduction' AND column_name = 'CheckedByUsers')
BEGIN
	ALTER TABLE dbo.InvProduction
		ADD CheckedByUsers VARCHAR(100) NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProduction' AND column_name = 'ApprovedByUsers')
BEGIN
	ALTER TABLE dbo.InvProduction
		ADD ApprovedByUsers VARCHAR(100) NULL
END
GO
/***** Object:  Table [dbo].[InvProductionRMDetails]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvProductionRMDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvProductionRMDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductionId] [int] NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NULL,	
	[ColorId] [int] NULL,
	[SizeId] [int] NULL,
	[StyleId] [int] NULL,	
	[StockById] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[AverageCost] [decimal](18, 2) NULL,
 CONSTRAINT [PK_InvProductionRMDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/***** Object:  Table [dbo].[InvProductionFGDetails]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvProductionFGDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvProductionFGDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductionId] [int] NULL,
	[LocationId] [int] NULL,
	[ItemId] [int] NULL,	
	[ColorId] [int] NULL,
	[SizeId] [int] NULL,
	[StyleId] [int] NULL,	
	[StockById] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[AverageCost] [decimal](18, 2) NULL,
 CONSTRAINT [PK_InvProductionFGDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'BagQuantity')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
		ADD BagQuantity INT NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'ProductionRatio')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD ProductionRatio DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'TotalProduction')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD TotalProduction DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'ProductionWiseFGCost')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD ProductionWiseFGCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'ProductionWiseCOGSCost')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD ProductionWiseCOGSCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'TotalFGCost')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD TotalFGCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'ProfitAndLoss')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD ProfitAndLoss DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'ProfitRatio')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD ProfitRatio DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'InvProductionFGDetails' AND column_name = 'ItemAverageCost')
BEGIN
	ALTER TABLE dbo.InvProductionFGDetails
	ADD ItemAverageCost DECIMAL(18,5) NULL DEFAULT 0 WITH VALUES;
END
GO
/****** Object:  Table [dbo].[SecurityUserGroupProjectMapping]    Script Date: 06/29/2018 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityUserGroupProjectMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityUserGroupProjectMapping](
	[MappingId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[UserGroupId] [int] NULL,
 CONSTRAINT [PK_SecurityUserGroupProjectMapping] PRIMARY KEY CLUSTERED 
(
	[MappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RestaurantBillItemSerialInfo]    Script Date: 10/10/2018 6:40:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestaurantBillItemSerialInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RestaurantBillItemSerialInfo](
	[SerialId] [bigint] IDENTITY(1,1) NOT NULL,
	[BillId] [bigint] NOT NULL,
	[ItemId] [int] NOT NULL,
	[SerialNumber] [varchar](200) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMGLCompanyAndGuestCompanyMapping]    Script Date: 8/1/2021 6:40:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMGLCompanyAndGuestCompanyMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMGLCompanyAndGuestCompanyMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[GLCompanyId] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMAccountManagerSalesTarget]    Script Date: 07/05/2018 14:47:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMAccountManagerSalesTarget]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMAccountManagerSalesTarget](
	[TargetId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[FiscalYearId] [bigint] NOT NULL,
	[CheckedBy] [bigint] NULL,
	[ApprovedBy] [bigint] NULL,
	[ApprovedStatus] [nvarchar](25) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SMAccountManagerSalesTarget] PRIMARY KEY CLUSTERED 
(
	[TargetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

/****** Object:  Table [dbo].[SMAccountManagerSalesTargetDetails]    Script Date: 07/05/2018 14:48:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SMAccountManagerSalesTargetDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SMAccountManagerSalesTargetDetails](
	[TargetDetailsId] [bigint] IDENTITY(1,1) NOT NULL,
	[TargetId] [bigint] NOT NULL,
	[MonthId] [smallint] NOT NULL,
	[AccountManagerId] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
 CONSTRAINT [PK_SMAccountManagerSalesTargetDetails] PRIMARY KEY CLUSTERED 
(
	[TargetDetailsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonPaymentInstruction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommonPaymentInstruction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Instruction] [varchar](MAX) NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedBy] [bigint] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CommonPaymentInstruction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PayrollEmpAttendanceLogUniviewRawData]    Script Date: 10/3/2021 12:57:01 PM ******/
IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAttendanceLogUniviewRawData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollEmpAttendanceLogUniviewRawData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RawLogData] [text] NULL,
	[RawLogTime] [datetime] NULL,
 CONSTRAINT [PK_PayrollEmpAttendanceLogUniviewRawData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[PayrollAttendanceDeviceLogUniviewSubscriptionData]    Script Date: 10/3/2021 12:57:01 PM ******/
IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollAttendanceDeviceLogUniviewSubscriptionData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayrollAttendanceDeviceLogUniviewSubscriptionData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [varchar](255) NULL,
	[ResponseURL] [varchar](max) NULL,
	[CreatedId] [int] NULL,
	[ResponseCode] [int] NULL,
	[SubResponseCode] [int] NULL,
	[ResponseString] [varchar](max) NULL,
	[StatusCode] [int] NULL,
	[StatusString] [varchar](max) NULL,
 CONSTRAINT [PK_PayrollAttendanceDeviceLogUniviewSubscriptionData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PMSupplierCompany]    Script Date: 10/3/2021 12:57:01 PM ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierCompany]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PMSupplierCompany](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[SupplierId] [int] NOT NULL,
		[CompanyId] [int] NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[LastModifiedBy] [int] NULL,
		[LastModifiedDate] [datetime] NULL,
	 CONSTRAINT [PK_PMSupplierCompany] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[HotelRegistrationGuestBreakfast]    Script Date: 10/3/2021 12:57:01 PM ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelRegistrationGuestBreakfast]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[HotelRegistrationGuestBreakfast](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[GuestId] [int] NOT NULL,
		[RegistrationId] [int] NOT NULL,
		[BreakfastDate] [date] NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL
	 CONSTRAINT [PK_HotelRegistrationGuestBreakfast] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_PADDING OFF
GO
/***** Object:  Table [dbo].[STCaseInternalNotesHistoryDetails]    Script Date: 06/29/2018 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[STCaseInternalNotesHistoryDetails]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[STCaseInternalNotesHistoryDetails](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[CaseId] [bigint] NOT NULL,
		[InternalNotesDetails] [nvarchar](MAX) NULL,		
	 CONSTRAINT [PK_STCaseInternalNotesHistoryDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotelGuestPaymentTransferInfo]    Script Date: 10/3/2021 12:57:01 PM ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelGuestPaymentTransferInfo]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[HotelGuestPaymentTransferInfo](
		[FromRegistrationId] [bigint] NOT NULL,
		[ToRegistrationId] [bigint] NOT NULL,
		[TransferDate] [datetime] NOT NULL,
		[PreviousBillNumber] [nvarchar](50) NOT NULL,
		[CurrentBillNumber] [nvarchar](50) NOT NULL,
		[PreviousAmount] [money] NOT NULL,
		[TransferAmount] [money] NOT NULL,
		[TransferDescription] [nvarchar](MAX) NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL	 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_PADDING OFF
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollEmpAttendanceLogMobileApp]    Script Date: 21/8/2022 12:57:01 PM ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmpAttendanceLogMobileApp]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PayrollEmpAttendanceLogMobileApp](
        [EmpId] [int] NOT NULL,
        [Latitude] [float] NOT NULL,
        [Longitude] [float] NOT NULL,
        [AttDateTime] [datetime] NOT NULL,
        [Image] [nvarchar](200) NOT NULL,
        [GoogleMapUrl] [nvarchar](100) NOT NULL
    ) ON [PRIMARY]
END
GO
/***** Object:  Table [dbo].[InvProductionOEDetails]    Script Date: 08/25/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvProductionOEDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvProductionOEDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductionId] [bigint] NULL,
	[NodeId] [int] NULL,
	[OEAmount] [decimal](18,5) NULL,	
	[OERemarks] [varchar](max) NULL,
 CONSTRAINT [PK_InvProductionOEDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/***** Object:  Table [dbo].[PMProductReceivedOEDetails]    Script Date: 08/25/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReceivedOEDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReceivedOEDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReceivedId] [bigint] NULL,
	[NodeId] [int] NULL,
	[OEAmount] [decimal](18,5) NULL,	
	[OERemarks] [varchar](max) NULL,
 CONSTRAINT [PK_PMProductReceivedOEDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/***** Object:  Table [dbo].[PMProductReceivedPMDetails]    Script Date: 09/07/2022 11:27:55 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMProductReceivedPMDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PMProductReceivedPMDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReceivedId] [bigint] NULL,
	[NodeId] [int] NULL,
	[Amount] [decimal](18,5) NULL,	
	[Remarks] [varchar](max) NULL,
 CONSTRAINT [PK_PMProductReceivedPMDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/***** Object:  Table [dbo].[HotelPaymentReservationTransferInfo]    Script Date: 09/18/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/***** Object:  Table [dbo].[HotelPaymentReservationTransferInfo]    Script Date: 09/18/2022 12:43:03 *****/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelPaymentReservationTransferInfo]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[HotelPaymentReservationTransferInfo](
		[FromReservationId] [bigint] NOT NULL,
		[ToReservationId] [bigint] NOT NULL,
		[TransferDate] [datetime] NOT NULL,
		[PreviousBillNumber] [nvarchar](50) NOT NULL,
		[CurrentBillNumber] [nvarchar](50) NOT NULL,
		[PreviousAmount] [money] NOT NULL,
		[TransferAmount] [money] NOT NULL,
		[TransferDescription] [nvarchar](MAX) NOT NULL,
		[CreatedBy] [int] NULL,
		[CreatedDate] [datetime] NULL	 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/***** Object:  Table [dbo].[LCCNFOpeningBalance]    Script Date: 10/16/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/***** Object:  Table [dbo].[LCCNFOpeningBalance]    Script Date: 10/16/2022 12:43:03 *****/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCCNFOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LCCNFOpeningBalance](
		[SupplierId] [int] NOT NULL,
		[DrAmount] [decimal](18,5) NULL,
		[CrAmount] [decimal](18,5) NULL 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/***** Object:  Table [dbo].[HotelCompanyOpeningBalance]    Script Date: 10/11/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/***** Object:  Table [dbo].[HotelCompanyOpeningBalance]    Script Date: 10/11/2022 12:43:03 *****/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[HotelCompanyOpeningBalance](
		[CompanyId] [int] NOT NULL,
		[DrAmount] [decimal](18,5) NULL,
		[CrAmount] [decimal](18,5) NULL 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/***** Object:  Table [dbo].[PayrollEmployeeOpeningBalance]    Script Date: 10/12/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/***** Object:  Table [dbo].[PayrollEmployeeOpeningBalance]    Script Date: 10/12/2022 12:43:03 *****/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PayrollEmployeeOpeningBalance](
		[EmployeeId] [int] NOT NULL,
		[DrAmount] [decimal](18,5) NULL,
		[CrAmount] [decimal](18,5) NULL 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/***** Object:  Table [dbo].[InvItemOpeningBalance]    Script Date: 10/13/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/***** Object:  Table [dbo].[InvItemOpeningBalance]    Script Date: 10/11/2022 12:43:03 *****/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvItemOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[InvItemOpeningBalance](
		[ItemId] [int] NOT NULL,
		[StockQuantity] [decimal](18,5) NULL,
		[UnitHead] [varchar](100) NULL 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/***** Object:  Table [dbo].[PMSupplierOpeningBalance]    Script Date: 10/12/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/***** Object:  Table [dbo].[PMSupplierOpeningBalance]    Script Date: 10/12/2022 12:43:03 *****/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PMSupplierOpeningBalance](
		[SupplierId] [int] NOT NULL,
		[DrAmount] [decimal](18,5) NULL,
		[CrAmount] [decimal](18,5) NULL 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/***** Object:  Table [dbo].[MemberOpeningBalance]    Script Date: 10/12/2022 12:43:03 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/***** Object:  Table [dbo].[MemberOpeningBalance]    Script Date: 10/12/2022 12:43:03 *****/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberOpeningBalance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MemberOpeningBalance](
		[MemberId] [int] NOT NULL,
		[DrAmount] [decimal](18,5) NULL,
		[CrAmount] [decimal](18,5) NULL 
	) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ATTicketMaster]    Script Date: 11/20/2022 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTicketMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ATTicketMaster](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CostCenterId] [int] NOT NULL,
	[BillNumber] [nvarchar](50) NOT	NULL,
	[TransactionType] [nvarchar](100) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[CompanyName] [nvarchar](max) NULL,
	[ReferenceId] [bigint] NOT NULL,
	[ReferenceName] [nvarchar](max) NOT NULL,
	[RegistrationNumber] [nvarchar](100) NULL,
	[Status] [nvarchar](100) NULL,
	[CheckedBy] [int] NULL,
	[CheckedByUsers] [nvarchar](100) NULL,
	[CheckedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedByUsers] [nvarchar](100) NULL,
	[ApprovedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	PRIMARY KEY (Id)
	)
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'ATTicketMaster' AND column_name = 'InvoiceAmount')
BEGIN
	ALTER TABLE dbo.ATTicketMaster
		ADD InvoiceAmount DECIMAL(18,2) NULL
END
GO
/****** Object:  Table [dbo].[ATTicketDetails]    Script Date: 11/20/2022 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTicketDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ATTicketDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketMasterId] [bigint] NOT NULL,
	[ClientName] [nvarchar](max) NULL,
	[MobileNo] [nvarchar](300) NULL,
	[Email] [nvarchar](300) NULL,
	[Address] [nvarchar](max) NULL,
	[IssueDate] [datetime] NOT NULL,
	[TicketType] [nvarchar](100) NOT NULL,
	[AirlineId] [int] NULL,
	[AirlineName] [nvarchar](max) NULL,
	[FlightDate] [datetime] NOT NULL,
	[ReturnDate] [datetime] NULL,
	[TicketNumber] [nvarchar](50) NOT NULL,
	[PNRNumber] [nvarchar](100) NOT NULL,
	[InvoiceAmount] [decimal](18, 2) NOT NULL,
	[AirlineAmount] [decimal](18, 2) NULL,
	[RoutePath] [nvarchar](max) NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	PRIMARY KEY (Id)
	)
END
/****** Object:  Table [dbo].[ATTicketPaymentDetails]    Script Date: 11/24/2022 12:43:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTicketPaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ATTicketPaymentDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketMasterId] [bigint] NOT NULL,
	[PaymentMode] [nvarchar](100) NOT NULL,
	[BankName] [nvarchar](max) NULL,
	[ReceiveAmount] [decimal] NOT NULL,
	[PaymentModeId] [int] NOT NULL,
	[CurrencyTypeId] [int] NOT NULL,
	[CurrencyType] [nvarchar](100) NOT NULL, 
	[CardType] [nvarchar](100) NULL,
	[CardTypeId] [int] NULL,
	[CardNumber] [nvarchar](max) NULL,
	[BankId] [int] NULL,
	[ChequeNumber] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	PRIMARY KEY (Id)
	)
END
GO