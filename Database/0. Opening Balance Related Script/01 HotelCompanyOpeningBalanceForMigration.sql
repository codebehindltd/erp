--GO
--/****** Object:  Table [dbo].[HotelCompanyOpeningBalance]    Script Date: 1/26/2023 5:33:01 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyOpeningBalance]') AND type in (N'U'))
--BEGIN
--	CREATE TABLE [dbo].[HotelCompanyOpeningBalance](
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
--	 CONSTRAINT [PK_HotelCompanyOpeningBalance] PRIMARY KEY CLUSTERED 
--	(
--		[Id] ASC
--	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]
--END
--ELSE
--BEGIN
	IF EXISTS(SELECT * FROM HotelCompanyOpeningBalance)
	BEGIN
		DECLARE @TempCompanyOpeningBalanceInfo AS TABLE
		(
			CompanyId	INT,
			DrAmount		DECIMAL(18,5),
			CrAmount		DECIMAL(18,5)
		)

		INSERT INTO @TempCompanyOpeningBalanceInfo
		SELECT	CompanyId,
				DrAmount,
				CrAmount
		FROM HotelCompanyOpeningBalance

		DROP TABLE HotelCompanyOpeningBalance

		IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyOpeningBalance]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[HotelCompanyOpeningBalance](
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
			 CONSTRAINT [PK_HotelCompanyOpeningBalance] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END
		
		--INSERT INTO HotelCompanyOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
		--SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance
		
		IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HotelCompanyOpeningBalanceDetail]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[HotelCompanyOpeningBalanceDetail](
				[Id] [bigint] IDENTITY(1,1) NOT NULL,
				[CompanyMasterId] [bigint] NOT NULL,
				[CompanyId] [int] NOT NULL,
				[DrAmount] [decimal](18,5) NULL,
				[CrAmount] [decimal](18,5) NULL
			 CONSTRAINT [PK_HotelCompanyOpeningBalanceDetail] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		INSERT INTO HotelCompanyOpeningBalanceDetail
		SELECT 1, CompanyId, DrAmount, CrAmount FROM @TempCompanyOpeningBalanceInfo

		SELECT * FROM HotelCompanyOpeningBalanceDetail
	END
--END
--GO