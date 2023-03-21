--GO
--/****** Object:  Table [dbo].[LCCNFOpeningBalance]    Script Date: 1/26/2023 5:33:01 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCCNFOpeningBalance]') AND type in (N'U'))
--BEGIN
--	CREATE TABLE [dbo].[LCCNFOpeningBalance](
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
--	 CONSTRAINT [PK_LCCNFOpeningBalance] PRIMARY KEY CLUSTERED 
--	(
--		[Id] ASC
--	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]
--END
--ELSE
--BEGIN
	--IF EXISTS(SELECT * FROM LCCNFOpeningBalance)
	BEGIN
		DECLARE @TempLCCNFOpeningBalanceInfo AS TABLE
		(
			SupplierId	INT,
			DrAmount		DECIMAL(18,5),
			CrAmount		DECIMAL(18,5)
		)

		INSERT INTO @TempLCCNFOpeningBalanceInfo
		SELECT	SupplierId,
				DrAmount,
				CrAmount
		FROM LCCNFOpeningBalance

		DROP TABLE LCCNFOpeningBalance

		IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCCNFOpeningBalance]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[LCCNFOpeningBalance](
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
			 CONSTRAINT [PK_LCCNFOpeningBalance] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		--INSERT INTO LCCNFOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
		--SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

		DROP TABLE LCCNFOpeningBalanceDetail
		IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LCCNFOpeningBalanceDetail]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[LCCNFOpeningBalanceDetail](
				[Id] [bigint] IDENTITY(1,1) NOT NULL,
				[CNFMasterId] [bigint] NOT NULL,
				[SupplierId] [int] NOT NULL,
				[DrAmount] [decimal](18,5) NULL,
				[CrAmount] [decimal](18,5) NULL
			 CONSTRAINT [PK_LCCNFOpeningBalanceDetail] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		INSERT INTO LCCNFOpeningBalanceDetail
		SELECT 1, SupplierId, DrAmount, CrAmount FROM @TempLCCNFOpeningBalanceInfo

		SELECT * FROM LCCNFOpeningBalanceDetail
	END
--END
--GO