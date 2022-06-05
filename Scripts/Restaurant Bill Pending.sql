/****** Object:  StoredProcedure [dbo].[UpdateRestaurantBillPending]    Script Date: 08/02/2018 10:25:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRestaurantBillPending]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateRestaurantBillPending]
GO

/****** Object:  StoredProcedure [dbo].[UpdateRestaurantBillPending]    Script Date: 08/02/2018 10:25:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateRestaurantBillPending]
	@RegistrationNumber VARCHAR(100),
	@SearchType			VARCHAR(100),
	@TransactionType	VARCHAR(100)
AS
BEGIN
	-- -- EXEC UpdateRestaurantBillPending 'RR00000002', 'RoomNumber', 'UPDATE' -- -- SELECT/UPDATE
	DECLARE @RegistrationId INT,
	@RoomNumber VARCHAR(100)
	
	SELECT @RegistrationId = hrr.RegistrationId,
		   @RoomNumber = hrn.RoomNumber
	FROM HotelRoomRegistration hrr
	INNER JOIN HotelRoomNumber hrn ON hrn.RoomId = hrr.RoomId
	WHERE hrr.RegistrationNumber = @RegistrationNumber
	
	DECLARE @BillId       INT,
	        @CheckInDate  DATETIME
	
	SELECT @CheckInDate = hrr.BillingStartDate
	FROM   HotelRoomRegistration hrr
	WHERE  hrr.RegistrationId = @RegistrationId
	
	DECLARE @TemporaryBillInfo AS TABLE
	        (
	            BillId INT,
	            BillDate DATETIME,
	            BillNumber VARCHAR(100),
	            CostCenterId INT,
	            CostCenter VARCHAR(300)
	        )	
	
	IF @SearchType = 'RegistrationId'
	BEGIN
	    INSERT INTO @TemporaryBillInfo
	    SELECT TOP 1 rb.BillId,
	           rb.BillDate,
	           rb.BillNumber,
	           rb.CostCenterId,
	           ccc.CostCenter
	    FROM   RestaurantBill rb
	           INNER JOIN CommonCostCenter ccc
	                ON  ccc.CostCenterId = rb.CostCenterId
	    WHERE  rb.RegistrationId = @RegistrationId
	           AND ISNULL(rb.IsBillSettlement, 0) <> 1
	           AND dbo.FnDate(rb.BillDate) >= dbo.FnDate(@CheckInDate)
	END
	ELSE 
	IF @SearchType = 'RoomNumber'
	BEGIN
	    INSERT INTO @TemporaryBillInfo
	    SELECT TOP 1 rb.BillId,
	           rb.BillDate,
	           rb.BillNumber,
	           rb.CostCenterId,
	           ccc.CostCenter
	    FROM   RestaurantBill rb
	           INNER JOIN CommonCostCenter ccc
	                ON  ccc.CostCenterId = rb.CostCenterId
	    WHERE  rb.SourceName = 'GuestRoom'
	           AND rb.BillPaidBySourceId = @RoomNumber
	           AND ISNULL(rb.IsBillSettlement, 0) <> 1
	           AND dbo.FnDate(rb.BillDate) >= dbo.FnDate(@CheckInDate)
	END
	
	DECLARE @mCostCenterId  INT,
	        @mSourceName    VARCHAR(100),
	        @mSourceId      INT,
	        @mKotId         INT
	
	SELECT @BillId = BillId
	FROM   @TemporaryBillInfo
	
	SELECT @mCostCenterId = rb.CostCenterId,
	       @mSourceName = rkbm.SourceName,
	       @mSourceId = rkbm.SourceId,
	       @mKotId = rkbm.KotId
	FROM   RestaurantBill rb
	       INNER JOIN RestaurantBillDetail rbd
	            ON  rb.BillId = rbd.BillId
	       INNER JOIN RestaurantKotBillMaster rkbm
	            ON  rbd.KotId = rkbm.KotId
	WHERE  rb.BillId = @BillId
	
	DECLARE @TemporaryKotBillDetailInfo AS TABLE
	        (
	            Id BIGINT not null,
	            [KotDetailId] [int] NOT NULL,
	            [KotId] [int] NULL,
	            [ItemType] [varchar](50) NULL,
	            [ItemId] [int] NULL,
	            [ItemName] [varchar](100) NULL,
	            [ItemCode] [varchar](20) NULL,
	            [Category] [varchar](100) NULL,
	            [ItemUnit] [decimal](18, 2) NULL,
	            [UnitRate] [decimal](18, 2) NULL,
	            [Amount] [decimal](18, 2) NULL,
	            [VatAmount] [decimal](18, 2) NULL,
	            [ServiceCharge] [decimal](18, 2) NULL,
	            [PrintFlag] [bit] NULL,
	            [IsChanged] [bit] NULL,
	            IsItemEditable bit
	        )
	
	INSERT INTO @TemporaryKotBillDetailInfo
	EXEC dbo.GetKotBillDetailInfoByTableNBearerId_SP
	     @CostCenterId = @mCostCenterId,
	     @SourceName = @mSourceName,
	     @SourceId = @mSourceId,
	     @KotId = @mKotId
	
	DECLARE @TotalCount INT
	SET @TotalCount = 0
	SELECT @TotalCount = COUNT(Id)
	FROM   @TemporaryKotBillDetailInfo
	
	DECLARE @PendingBillId BIGINT
	
	IF @TotalCount > 0
	BEGIN
	    SELECT @PendingBillId = BillId
	    FROM   @TemporaryBillInfo
	END
	ELSE
	BEGIN
	    DECLARE @TemporaryBillInfo2 AS TABLE
	            (
	                BillId INT,
	                BillDate DATETIME,
	                BillNumber VARCHAR(100),
	                CostCenterId INT,
	                CostCenter VARCHAR(300)
	            )    
	    
	    SELECT @PendingBillId = BillId
	    FROM   @TemporaryBillInfo2
	END
	
	IF UPPER(@TransactionType) = 'SELECT'
	BEGIN		
		SELECT * FROM RestaurantBill
		WHERE BillId = @PendingBillId
	END
	ELSE IF UPPER(@TransactionType) = 'UPDATE'
	BEGIN
		UPDATE RestaurantBill
		SET BillPaidBySourceId = 0
		WHERE BillId = @PendingBillId
	END	
END
GO