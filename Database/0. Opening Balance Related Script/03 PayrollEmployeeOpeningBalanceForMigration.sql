--GO
--/****** Object:  Table [dbo].[PayrollEmployeeOpeningBalance]    Script Date: 1/26/2023 5:33:01 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeOpeningBalance]') AND type in (N'U'))
--BEGIN
--	CREATE TABLE [dbo].[PayrollEmployeeOpeningBalance](
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
--	 CONSTRAINT [PK_PayrollEmployeeOpeningBalance] PRIMARY KEY CLUSTERED 
--	(
--		[Id] ASC
--	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]
--END
--ELSE
--BEGIN
	IF EXISTS(SELECT * FROM PayrollEmployeeOpeningBalance)
	BEGIN
		DECLARE @TempEmployeeOpeningBalanceInfo AS TABLE
		(
			EmpId	INT,
			DrAmount	DECIMAL(18,5),
			CrAmount	DECIMAL(18,5)
		)

		INSERT INTO @TempEmployeeOpeningBalanceInfo
		SELECT	EmployeeId,
				DrAmount,
				CrAmount
		FROM PayrollEmployeeOpeningBalance

		DROP TABLE PayrollEmployeeOpeningBalance

		IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeOpeningBalance]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[PayrollEmployeeOpeningBalance](
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
				CONSTRAINT [PK_PayrollEmployeeOpeningBalance] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		--INSERT INTO PayrollEmployeeOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
		--SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

		DROP TABLE PayrollEmployeeOpeningBalanceDetail
		IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayrollEmployeeOpeningBalanceDetail]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[PayrollEmployeeOpeningBalanceDetail](
				[Id] [bigint] IDENTITY(1,1) NOT NULL,
				[EmployeeMasterId] [bigint] NOT NULL,
				[EmpId] [int] NOT NULL,
				[DrAmount] [decimal](18,5) NULL,
				[CrAmount] [decimal](18,5) NULL
			 CONSTRAINT [PK_PayrollEmployeeOpeningBalanceDetail] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		INSERT INTO PayrollEmployeeOpeningBalanceDetail
		SELECT 1, EmpId, DrAmount, CrAmount FROM @TempEmployeeOpeningBalanceInfo

		SELECT * FROM PayrollEmployeeOpeningBalanceDetail
	END
--END
--GO