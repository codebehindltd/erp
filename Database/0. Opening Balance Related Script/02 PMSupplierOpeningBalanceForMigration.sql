--GO
--/****** Object:  Table [dbo].[PMSupplierOpeningBalance]    Script Date: 1/26/2023 5:33:01 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierOpeningBalance]') AND type in (N'U'))
--BEGIN
--	CREATE TABLE [dbo].[PMSupplierOpeningBalance](
--		[Id] [bigint] IDENTITY(1,1) NOT NULL,
--		[CompanyId] [int] NOT NULL,
--		[ProjectId] [int] NOT NULL,
--		[FiscalYearId] [int] NULL,
--		[OpeningDate] [datetime] NULL,
--		[IsApproved] [bit] NULL,
--		[CreatedBy] [int] NULL,
--		[CreatedDate] [datetime] NULL,
--		[LastModifiedBy] [int] NULL,
--		[LastModifiedDate] [datetime] NULL,
--	 CONSTRAINT [PK_PMSupplierOpeningBalance] PRIMARY KEY CLUSTERED 
--	(
--		[Id] ASC
--	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]
--END
--ELSE
--BEGIN
	IF EXISTS(SELECT * FROM PMSupplierOpeningBalance)
	BEGIN
		DECLARE @TempSupplierOpeningBalanceInfo AS TABLE
		(
			SupplierId	INT,
			DrAmount		DECIMAL(18,5),
			CrAmount		DECIMAL(18,5)
		)

		INSERT INTO @TempSupplierOpeningBalanceInfo
		SELECT	SupplierId,
				DrAmount,
				CrAmount
		FROM PMSupplierOpeningBalance

		DROP TABLE PMSupplierOpeningBalance

		IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierOpeningBalance]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[PMSupplierOpeningBalance](
				[Id] [bigint] IDENTITY(1,1) NOT NULL,
				[CompanyId] [int] NOT NULL,
				[ProjectId] [int] NOT NULL,
				[FiscalYearId] [int] NULL,
				[OpeningDate] [datetime] NULL,
				[IsApproved] [bit] NULL,
				[CreatedBy] [int] NULL,
				[CreatedDate] [datetime] NULL,
				[LastModifiedBy] [int] NULL,
				[LastModifiedDate] [datetime] NULL,
			 CONSTRAINT [PK_PMSupplierOpeningBalance] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		--INSERT INTO PMSupplierOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
		--SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

		IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PMSupplierOpeningBalanceDetail]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[PMSupplierOpeningBalanceDetail](
				[Id] [bigint] IDENTITY(1,1) NOT NULL,
				[SupplierMasterId] [bigint] NOT NULL,
				[SupplierId] [int] NOT NULL,
				[DrAmount] [decimal](18,5) NULL,
				[CrAmount] [decimal](18,5) NULL
			 CONSTRAINT [PK_PMSupplierOpeningBalanceDetail] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		INSERT INTO PMSupplierOpeningBalanceDetail
		SELECT 1, SupplierId, DrAmount, CrAmount FROM @TempSupplierOpeningBalanceInfo

		SELECT * FROM PMSupplierOpeningBalanceDetail
	END
--END
--GO