--GO
--/****** Object:  Table [dbo].[MemberOpeningBalance]    Script Date: 1/26/2023 5:33:01 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberOpeningBalance]') AND type in (N'U'))
--BEGIN
--	CREATE TABLE [dbo].[MemberOpeningBalance](
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
--	 CONSTRAINT [PK_MemberOpeningBalance] PRIMARY KEY CLUSTERED 
--	(
--		[Id] ASC
--	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]
--END
--ELSE
--BEGIN
	--IF EXISTS(SELECT * FROM MemberOpeningBalance)
	BEGIN
		DECLARE @TempMemberOpeningBalanceInfo AS TABLE
		(
			MemberId		INT,
			DrAmount		DECIMAL(18,5),
			CrAmount		DECIMAL(18,5)
		)

		INSERT INTO @TempMemberOpeningBalanceInfo
		SELECT	MemberId,
				DrAmount,
				CrAmount
		FROM MemberOpeningBalance

		DROP TABLE MemberOpeningBalance

		IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberOpeningBalance]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[MemberOpeningBalance](
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
			 CONSTRAINT [PK_MemberOpeningBalance] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		--INSERT INTO MemberOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
		--SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

		DROP TABLE MemberOpeningBalanceDetail
		IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberOpeningBalanceDetail]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[MemberOpeningBalanceDetail](
				[Id] [bigint] IDENTITY(1,1) NOT NULL,
				[MemberMasterId] [bigint] NOT NULL,
				[MemberId] [int] NOT NULL,
				[DrAmount] [decimal](18,5) NULL,
				[CrAmount] [decimal](18,5) NULL
			 CONSTRAINT [PK_MemberOpeningBalanceDetail] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END

		INSERT INTO MemberOpeningBalanceDetail
		SELECT 1, MemberId, DrAmount, CrAmount FROM @TempMemberOpeningBalanceInfo

		SELECT * FROM MemberOpeningBalanceDetail
	END
--END
--GO