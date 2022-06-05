/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncRegistrationInformation_SP]    Script Date: 10/24/2018 5:31:49 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveOrUpdateSyncRegistrationInformation_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveOrUpdateSyncRegistrationInformation_SP]
GO

/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncRegistrationInformation_SP]    Script Date: 10/24/2018 5:31:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 24/10/2018
  Name					: SaveOrUpdateSyncRegistrationInformation_SP
  Description			: This procedure Save Or Update Registration Sync Information 
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

/*exec SyncRegistration_SP 2*/
CREATE PROCEDURE [dbo].[SaveOrUpdateSyncRegistrationInformation_SP] 
	(
		@GuidId		UNIQUEIDENTIFIER,
		@IsSyncCompleted BIT
	)
AS
BEGIN
IF EXISTS (SELECT * FROM RegistrationSync WHERE GuidId = @GuidId)
BEGIN
	UPDATE RegistrationSync
	SET IsSyncCompleted = @IsSyncCompleted
	WHERE GuidId = @GuidId
END
ELSE
	BEGIN
	INSERT INTO RegistrationSync
				(
					GuidId,
					IsSyncCompleted
				)
				VALUES(
					@GuidId,
					@IsSyncCompleted
				)
	END

END
GO


/****** Object:  StoredProcedure [dbo].[GetSyncedRegistrationByRegistrationId_SP]    Script Date: 10/24/2018 2:33:56 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSyncedRegistrationByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSyncedRegistrationByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetSyncedRegistrationByRegistrationId_SP]    Script Date: 10/24/2018 2:33:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 24/10/2018
  Name					: GetSyncedRegistrationByRegistrationId_SP
  Description			: This procedure returns Synced Registration By RegistrationId for sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/
CREATE PROCEDURE [dbo].[GetSyncedRegistrationByRegistrationId_SP]
(
	@RegistrationId		INT
)
AS
BEGIN
	
	SELECT rr.RegistrationId,
		   rr.RegistrationNumber,
		   rs.GuidId,
		   rs.IsSyncCompleted

	FROM HotelRoomRegistration rr
	INNER JOIN RegistrationSync rs
	ON rr.GuidId = rs.GuidId
	WHERE RR.RegistrationId = @RegistrationId

END
GO

/****** Object:  StoredProcedure [dbo].[GetRegistrationDetailsForDataSync_SP]    Script Date: 10/24/2018 1:58:53 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRegistrationDetailsForDataSync_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRegistrationDetailsForDataSync_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetRegistrationDetailsForDataSync_SP]    Script Date: 10/24/2018 1:58:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 24/10/2018
  Name					: GetRegistrationDetailsForDataSync_SP
  Description			: This procedure returns Registration Details For Data Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetRegistrationDetailsForDataSync_SP]
	
AS
BEGIN

	DECLARE   @DayRange INT = 0

	SELECT @DayRange  = (-1)*(CONVERT(INT, CONVERT(VARCHAR(50), SetupValue))) FROM CommonSetup WHERE SetupName = 'SyncDayRange' AND TypeName = 'SyncDayRange'
	
	DECLARE  @ToDate DATETIME = GETDATE(), @FromDate DATETIME = DATEADD( DAY , @DayRange, GETDATE())
	
	SELECT  rr.RegistrationId,
			rr.RegistrationNumber,
			rr.RoomRate,
	        dbo.FnGuestInformationListWithCommaSeperator(rr.RegistrationId, 'Registration') AS 
	        GuestName,
	        dbo.FnGenerateDateFormat(rr.ArriveDate, 'DateStamp') AS 
	        Arrival,
	        dbo.FnGenerateDateFormat(rr.ExpectedCheckOutDate, 'DateStamp') AS 
	        Departure,
			rr.ArriveDate ArriveDate,
			rr.CheckOutDate CheckOutDate,
			rs.GuidId,
			ISNULL(rs.IsSyncCompleted,0) IsSyncCompleted
			INTO #TEMP
	FROM   HotelRoomRegistration rr
	LEFT JOIN RegistrationSync rs
	ON rs.GuidId = rr.GuidId
	ORDER BY rr.RegistrationId DESC
	
	SELECT * FROM #TEMP
	WHERE (IsSyncCompleted = 0 AND GuidId IS NOT NULL)
	OR (GuidId IS NULL AND ((ArriveDate BETWEEN @FromDate AND @ToDate) OR (CheckOutDate BETWEEN @FromDate AND @ToDate)))

	DROP TABLE #TEMP
END
GO


/****** Object:  StoredProcedure [dbo].[GetGuestHouseCheckOutInfoByRegistrationId_SP]    Script Date: 10/23/2018 1:32:24 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGuestHouseCheckOutInfoByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetGuestHouseCheckOutInfoByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestHouseCheckOutInfoByRegistrationId_SP]    Script Date: 10/23/2018 1:32:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 23/10/2018
  Name					: GetGuestHouseCheckOutInfoByRegistrationId_SP
  Description			: This procedure returns Guest House CheckOut Info By RegistrationId
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  24/7/2018			NKD
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetGuestHouseCheckOutInfoByRegistrationId_SP]
(
	@registrationId		INT
)
AS
BEGIN
	SELECT * FROM HotelGuestHouseCheckOut
	WHERE RegistrationId = @registrationId
END
GO



/****** Object:  StoredProcedure [dbo].[GetRoomRegistrationInfoById_SP]    Script Date: 10/26/2018 12:57:42 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRoomRegistrationInfoById_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRoomRegistrationInfoById_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetRoomRegistrationInfoById_SP]    Script Date: 10/26/2018 12:57:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/*=================================================================== 
  Author				: 
  Create date			: 
  Name					: GetRoomRegistrationInfoById_SP
  Description			: This procedure returns Registration Information
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  
  17-10-18			FA		POSRemarks col added
  23-10-2018		NKD		Added BillHoldUpDate,ActualCheckOutDate,CheckOutDate,TotalRoomRate,HoldUpAmount and GuidId column 
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetRoomRegistrationInfoById_SP]
	@RegistrationId INT
AS
BEGIN
	/*
	EXEC GetRoomRegistrationInfoById_SP 3
	*/	
	
	DECLARE @LocalCurrencyId INT
	SET @LocalCurrencyId = 1
	SELECT @LocalCurrencyId = cc.CurrencyId
	FROM   CommonCurrency cc
	WHERE  cc.CurrencyType = 'Local'
	
	SELECT rr.RegistrationId,
	       rr.RegistrationNumber,
	       rr.ArriveDate,
		   rr.BillingStartDate,
	       rr.ExpectedCheckOutDate,
		   rr.BillHoldUpDate,
		   rr.ActualCheckOutDate,
		   rr.CheckOutDate,
	       rr.RoomId,
	       ISNULL(rn.RoomNumber, 0) AS RoomNumber,
	       ISNULL(rr.EntitleRoomType, 0) AS EntitleRoomType,
	       rr.CurrencyType,
	       dbo.FnGetCurrencyHeadById(rr.CurrencyType) Currency,
	       ISNULL(rr.ConversionRate, 0) AS ConversionRate,
	       ISNULL(rr.UnitPrice, 0) AS UnitPrice,
	       rr.DiscountType,
	       ISNULL(rr.DiscountAmount, 0) AS DiscountAmount,
	       ISNULL(rr.RoomRate, 0) AS RoomRate,
	       ISNULL(rr.IsServiceChargeEnable, 0) AS IsServiceChargeEnable,
	       ISNULL(rr.IsCityChargeEnable, 0) AS IsCityChargeEnable,
	       ISNULL(rr.IsVatAmountEnable, 0) AS IsVatAmountEnable,
	       ISNULL(rr.IsAdditionalChargeEnable, 0) AS IsAdditionalChargeEnable,
		   rr.TotalRoomRate,

	       ISNULL(rr.IsCompanyGuest, 0) AS IsCompanyGuest,
	       ISNULL(rr.IsHouseUseRoom, 0) AS IsHouseUseRoom,
	       rr.CommingFrom,
	       rr.NextDestination,
	       rr.VisitPurpose,
	       rr.IsFromReservation,
	       rr.ReservationId,
	       rr2.ReservationNumber + ' (' + dbo.FnGetFirstGuestNameForReservation(rr2.ReservationId) 
	       + ')' AS ReservationInfo,
	       ISNULL(rr.IsFamilyOrCouple, 0) AS IsFamilyOrCouple,
	       rr.NumberOfPersonAdult,
	       rr.NumberOfPersonChild,
	       rr.IsListedCompany,
	       rr.ReservedCompany,
	       rr.CompanyId,
	       CASE 
	            WHEN rr.CompanyId = 0 THEN rr.ReservedCompany
	            ELSE hgc.CompanyName
	       END CompanyName,
	       rr.ContactPerson,
	       rr.ContactNumber,
	       rr.PaymentMode,
	       rr.PayFor,
	       rr.BusinessPromotionId,
	       ISNULL(rr.IsRoomOwner, 0) AS IsRoomOwner,
	       rr.GuestSourceId,
	       rr.ReferenceId,
	       rr.IsReturnedGuest,
	       ISNULL(rr.IsVIPGuest, 0) AS IsVIPGuest,
	       ISNULL(rr.VIPGuestTypeId, 0) AS VIPGuestTypeId,
	       rr.Remarks,
		   rr.POSRemarks,
	       rr.AirportPickUp,
	       rr.AirportDrop,
	       ISNULL(rapd.APDId, 0) AS APDId,
	       rapd.ArrivalFlightName,
	       rapd.ArrivalFlightNumber,
	       rapd.ArrivalTime,
	       ISNULL(rapd.DepartureAirlineId, 0) AS DepartureAirlineId,
	       rapd.DepartureFlightName,
	       rapd.DepartureFlightNumber,
	       rapd.DepartureTime,
	       CASE 
	            WHEN CheckOutDate IS NULL THEN 0
	            ELSE 1
	       END IsGuestCheckedOut,
	       CASE 
	            WHEN dbo.FnIsPaidServiceExistByRegistrationId(rr.RegistrationId) 
	                 = 0 THEN 0
	            ELSE 1
	       END IsPaidServiceExist,
	       dbo.FnGetCurrencyHeadById(@LocalCurrencyId) AS LocalCurrencyHead,
	       rr.CardType,
	       rr.CardNumber,
	       rr.CardHolderName,
	       dbo.FnGenerateDateFormat(rr.CardExpireDate, 'DateStamp') AS 
	       CardExpireDate,
	       rr.CardReference,
	       CASE 
	            WHEN ISNULL(rr.BillHoldUpDate, 0) = 0 THEN 0
	            ELSE 1
	       END AS IsBillHoldUp,
	       ISNULL(rr.MealPlanId, 0) AS MealPlanId,
		   ISNULL(rr.IsStopChargePosting, 0)IsStopChargePosting,
		   rr.IsBlankRegistrationCard,
		   rr.CreatedBy,
		   rr.CreatedDate,
		   rr.IsEarlyCheckInChargeEnable,
		   ISNULL(rr.POSRemarks,'') POSRemarks,
		   rr.HoldUpAmount,
		   rr.GuidId
		   
	FROM   HotelRoomRegistration rr
	       LEFT JOIN HotelRoomNumber rn
	            ON  rn.RoomId = rr.RoomId
	       LEFT JOIN HotelRegistrationAireportPickupDrop rapd
	            ON  rapd.RegistrationId = rr.RegistrationId
	       LEFT JOIN HotelRoomReservation rr2
	            ON  rr.ReservationId = rr2.ReservationId
	       LEFT OUTER JOIN HotelGuestCompany hgc
	            ON  hgc.CompanyId = rr.CompanyId
	WHERE  rr.RegistrationId = @RegistrationId
END
GO


/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncRestaurantInformation_SP]    Script Date: 10/26/2018 7:21:52 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveOrUpdateSyncRestaurantInformation_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveOrUpdateSyncRestaurantInformation_SP]
GO

/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncRestaurantInformation_SP]    Script Date: 10/26/2018 7:21:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: NKD
  Create date			: 26/10/2018
  Name					: SaveOrUpdateSyncRestaurantInformation_SP
  Description			: This procedure Save Or Update Restaurant Sync Information
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

/*exec SyncRegistration_SP 2*/
CREATE PROCEDURE [dbo].[SaveOrUpdateSyncRestaurantInformation_SP] 
	(
		@GuidId		UNIQUEIDENTIFIER,
		@IsSyncCompleted BIT
	)
AS
BEGIN
IF EXISTS (SELECT * FROM RestaurantBillSync WHERE GuidId = @GuidId)
BEGIN
	UPDATE RestaurantBillSync
	SET IsSyncCompleted = @IsSyncCompleted
	WHERE GuidId = @GuidId
END
ELSE
	BEGIN
	INSERT INTO RestaurantBillSync
				(
					GuidId,
					IsSyncCompleted
				)
				VALUES(
					@GuidId,
					@IsSyncCompleted
				)
	END

END
GO


/****** Object:  StoredProcedure [dbo].[GetRestaurantBillsForSync_SP]    Script Date: 10/26/2018 10:12:48 AM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRestaurantBillsForSync_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRestaurantBillsForSync_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetRestaurantBillsForSync_SP]    Script Date: 10/26/2018 10:12:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: GetRestaurantBillsForSync_SP
  Description			: This procedure returns RestaurantBills For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/
CREATE PROCEDURE [dbo].[GetRestaurantBillsForSync_SP] 
	
AS
BEGIN

/*
exec GetRestaurantBillsForSync_SP

*/
	DECLARE   @DayRange INT = 0

	SELECT @DayRange  = (-1)*(CONVERT(INT, CONVERT(VARCHAR(50), SetupValue))) FROM CommonSetup WHERE SetupName = 'SyncDayRange' AND TypeName = 'SyncDayRange'
	
	DECLARE  @ToDate DATETIME = GETDATE(), @FromDate DATETIME =DATEADD(DAY,@DayRange, GETDATE())

	(SELECT  rb.BillId,
			rb.BillNumber,
			hgesba.RegistrationId,
			cc.CostCenter,
			rb.RoundedGrandTotal,
			ISNULL(hgbp.PaymentMode,'Guest Room') PaymentMode,
			rb.BillDate BillDate,
			rbs.GuidId,
			ISNULL( rbs.IsSyncCompleted,0) IsSyncCompleted
			INTO ##a
			FROM   RestaurantBill rb 
			INNER JOIN HotelGuestBillPayment hgbp
			ON rb.BillId = hgbp.ServiceBillId AND hgbp.ModuleName ='Restaurant'
			LEFT JOIN HotelGuestExtraServiceBillApproved hgesba
			ON  rb.BillId = hgesba.ServiceBillId 
			
			INNER JOIN CommonCostCenter cc
			ON rb.CostCenterId = cc.CostCenterId
			
			LEFT JOIN RestaurantBillSync rbs
			ON rbs.GuidId = rb.GuidId
			WHERE rb.IsBillSettlement = 1
		)
		UNION
		(
		SELECT  rb.BillId,
			rb.BillNumber,
			hgesba.RegistrationId,
			cc.CostCenter,
			rb.RoundedGrandTotal,
			PaymentMode = 'Room',
			rb.BillDate BillDate,
			rbs.GuidId,
			ISNULL( rbs.IsSyncCompleted,0) IsSyncCompleted

			FROM   RestaurantBill rb 
			INNER JOIN HotelGuestExtraServiceBillApproved hgesba
			ON  rb.BillId = hgesba.ServiceBillId 
			
			INNER JOIN CommonCostCenter cc
			ON rb.CostCenterId = cc.CostCenterId
			
			LEFT JOIN RestaurantBillSync rbs
			ON rbs.GuidId = rb.GuidId
			WHERE rb.IsBillSettlement = 1)


	SELECT BillId,	BillNumber,	RegistrationId,	CostCenter,	RoundedGrandTotal,BillDate,
		   GuidId, IsSyncCompleted,
		   PaymentMode = STUFF((
			SELECT N', ' + PaymentMode FROM ##a
			WHERE BillId = x.BillId 
			ORDER BY BillId DESC
			FOR XML PATH(''), TYPE).value(N'.[1]', N'nvarchar(max)'), 1, 2, N'')
	FROM ##a AS x
	WHERE (x.IsSyncCompleted = 0 AND x.GuidId IS NOT NULL) OR
		(x.GuidId IS NULL AND (x.BillDate BETWEEN @FromDate AND @TODATE))
	GROUP BY BillId,	BillNumber,	RegistrationId,	CostCenter,	RoundedGrandTotal,BillDate,
			 GuidId, IsSyncCompleted
	ORDER BY BillId DESC
	DROP TABLE ##a		 
	
END
GO


/****** Object:  StoredProcedure [dbo].[GetSyncedRestaurantBillByRegistrationId_SP]    Script Date: 10/31/2018 10:47:10 AM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSyncedRestaurantBillByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSyncedRestaurantBillByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetSyncedRestaurantBillByRegistrationId_SP]    Script Date: 10/31/2018 10:47:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: GetServiceBillsForSync_SP
  Description			: This procedure returns Synced RestaurantBill By RegistrationId For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetSyncedRestaurantBillByRegistrationId_SP]
(
	@BillId		INT
)
AS
BEGIN
	SELECT rb.BillId,
		   rb.RegistrationId,
		   ccc.CostCenter,
		   rs.GuidId,
		   rs.IsSyncCompleted

	FROM RestaurantBill rb
	INNER JOIN RestaurantBillSync rs
	ON rb.GuidId = rs.GuidId
	INNER JOIN CommonCostCenter ccc
	ON rb.CostCenterId = ccc.CostCenterId
	WHERE rb.BillId = @BillId
END
GO

/****** Object:  StoredProcedure [dbo].[GetServiceBillsForSync_SP]    Script Date: 10/31/2018 1:15:37 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceBillsForSync_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceBillsForSync_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetServiceBillsForSync_SP]    Script Date: 10/31/2018 1:15:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: GetServiceBillsForSync_SP
  Description			: This procedure returns ServiceBills For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/
CREATE PROCEDURE [dbo].[GetServiceBillsForSync_SP] 
	
AS
BEGIN

/*
exec GetServiceBillsForSync_SP

*/
	DECLARE   @DayRange INT = 0

	SELECT @DayRange  = (-1)*(CONVERT(INT, CONVERT(VARCHAR(50), SetupValue))) FROM CommonSetup WHERE SetupName = 'SyncDayRange' AND TypeName = 'SyncDayRange'
	
	DECLARE  @ToDate DATETIME = GETDATE(), @FromDate DATETIME =DATEADD(DAY,@DayRange, GETDATE())

	(SELECT  hgsb.ServiceBillId,
		hgsb.BillNumber,
		hgsb.RegistrationId,
		cc.CostCenter,
		hgsb.TotalCalculatedAmount,
		hgbp.PaymentMode,
		hgsb.ServiceDate,
		sbs.GuidId,
		ISNULL( sbs.IsSyncCompleted,0) IsSyncCompleted
		INTO ##a
		FROM HotelGuestServiceBill hgsb 

		INNER JOIN HotelGuestServiceInfo hgsi
		ON hgsb.ServiceId = hgsi.ServiceId
		 
		INNER JOIN CommonCostCenter cc
		ON hgsi.CostCenterId = cc.CostCenterId
			
		INNER JOIN HotelGuestBillPayment hgbp
		ON hgbp.ServiceBillId = hgsb.ServiceBillId 
		 
		LEFT JOIN ServiceBillSync sbs
		ON sbs.GuidId = hgsb.GuidId
		WHERE hgbp.ModuleName = 'FrontOffice'
	)
		UNION 

	(
		SELECT  hgsb.ServiceBillId,
		hgsb.BillNumber,
		hgsb.RegistrationId,
		cc.CostCenter,
		hgsb.TotalCalculatedAmount,
		PaymentMode = 'Room',
		hgsb.ServiceDate,
		sbs.GuidId,
		ISNULL( sbs.IsSyncCompleted,0) IsSyncCompleted
		--INTO ##a
		FROM HotelGuestServiceBill hgsb 

		INNER JOIN HotelGuestServiceInfo hgsi
		ON hgsb.ServiceId = hgsi.ServiceId
		AND hgsb.RegistrationId > 0
		 
		INNER JOIN CommonCostCenter cc
		ON hgsi.CostCenterId = cc.CostCenterId
		
		LEFT JOIN ServiceBillSync sbs
		ON sbs.GuidId = hgsb.GuidId
	)


	SELECT ServiceBillId,	BillNumber,	RegistrationId,	CostCenter,	TotalCalculatedAmount, ServiceDate,
		   GuidId, IsSyncCompleted,
		   PaymentMode = STUFF((
			SELECT N', ' + PaymentMode FROM ##a
			WHERE ServiceBillId = x.ServiceBillId
			ORDER BY ServiceBillId DESC
			FOR XML PATH(''), TYPE).value(N'.[1]', N'nvarchar(max)'), 1, 2, N'')
	FROM ##a AS x
	WHERE (x.IsSyncCompleted = 0 AND x.GuidId IS NOT NULL) OR
		(x.GuidId IS NULL AND (x.ServiceDate BETWEEN @FromDate AND @TODATE))

	GROUP BY ServiceBillId,	BillNumber,	RegistrationId,	CostCenter,	TotalCalculatedAmount, ServiceDate,
			 GuidId, IsSyncCompleted
	ORDER BY ServiceBillId DESC
	DROP TABLE ##a		 
	
END
GO



/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncServiceBillInformation_SP]    Script Date: 10/31/2018 2:52:41 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveOrUpdateSyncServiceBillInformation_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveOrUpdateSyncServiceBillInformation_SP]
GO

/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncServiceBillInformation_SP]    Script Date: 10/31/2018 2:52:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: SaveOrUpdateSyncServiceBillInformation_SP
  Description			: This procedure Save Or Update ServiceBill Sync Information
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

/*exec SyncRegistration_SP 2*/
CREATE PROCEDURE [dbo].[SaveOrUpdateSyncServiceBillInformation_SP] 
	(
		@GuidId		UNIQUEIDENTIFIER,
		@IsSyncCompleted BIT
	)
AS
BEGIN
	IF EXISTS (SELECT * FROM ServiceBillSync WHERE GuidId = @GuidId)
	BEGIN
		UPDATE ServiceBillSync
		SET IsSyncCompleted = @IsSyncCompleted
		WHERE GuidId = @GuidId
	END
	ELSE
		BEGIN
		INSERT INTO ServiceBillSync
					(
						GuidId,
						IsSyncCompleted
					)
					VALUES(
						@GuidId,
						@IsSyncCompleted
					)
		END
	END
GO


/****** Object:  StoredProcedure [dbo].[GetGuestBillPaymentsByModuleNameAndServiceBillId_SP]    Script Date: 10/31/2018 3:17:27 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGuestBillPaymentsByModuleNameAndServiceBillId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetGuestBillPaymentsByModuleNameAndServiceBillId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestBillPaymentsByModuleNameAndServiceBillId_SP]    Script Date: 10/31/2018 3:17:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: GetGuestBillPaymentsByModuleNameAndServiceBillId_SP
  Description			: This procedure returns Guest Bill Payments By Module Name And ServiceBillId For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetGuestBillPaymentsByModuleNameAndServiceBillId_SP]
	(
		@ModuleName		varchar(100),
		@ServiceBillId	INT
	)
AS
BEGIN
	SELECT hbp.*,
		   hrr.GuidId GuidId
		   FROM HotelGuestBillPayment hbp
		   LEFT JOIN HotelRoomRegistration hrr
		   ON hbp.RegistrationId = hrr.RegistrationId

		   WHERE hbp.ModuleName = @ModuleName 
		   AND hbp.ServiceBillId = @ServiceBillId
END
GO

/****** Object:  StoredProcedure [dbo].[GetSyncedServiceBillByRegistrationId_SP]    Script Date: 10/31/2018 3:45:11 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSyncedServiceBillByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSyncedServiceBillByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetSyncedServiceBillByRegistrationId_SP]    Script Date: 10/31/2018 3:45:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: GetSyncedServiceBillByRegistrationId_SP
  Description			: This procedure returns Synced ServiceBill By RegistrationId_SP For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/
CREATE PROCEDURE [dbo].[GetSyncedServiceBillByRegistrationId_SP]
	@BillId		INT
AS
BEGIN
	SELECT hgsb.ServiceBillId,
		   hgsb.RegistrationId,
		   rs.GuidId,
		   rs.IsSyncCompleted

	FROM HotelGuestServiceBill hgsb
	INNER JOIN ServiceBillSync rs
	ON hgsb.GuidId = rs.GuidId
	WHERE hgsb.ServiceBillId = @BillId
END
GO


/****** Object:  StoredProcedure [dbo].[GetBanquetBillsForSync_SP]    Script Date: 11/6/2018 2:10:12 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBanquetBillsForSync_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBanquetBillsForSync_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetBanquetBillsForSync_SP]    Script Date: 11/6/2018 2:10:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: GetBanquetBillsForSync_SP
  Description			: This procedure returns BanquetBill For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetBanquetBillsForSync_SP]
	
AS
BEGIN
	DECLARE   @DayRange INT = 0

	SELECT @DayRange  = (-1)*(CONVERT(INT, CONVERT(VARCHAR(50), SetupValue))) FROM CommonSetup WHERE SetupName = 'SyncDayRange' AND TypeName = 'SyncDayRange'
	
	DECLARE  @ToDate DATETIME = GETDATE(), @FromDate DATETIME =DATEADD(DAY,@DayRange, GETDATE())

	SELECT  br.Id,
			br.ReservationNumber,
			cc.CostCenter,
			br.RoundedGrandTotal,
			br.SettlementDate,
			sbs.GuidId,
			ISNULL( sbs.IsSyncCompleted,0) IsSyncCompleted
			INTO ##a
			FROM BanquetReservation br 

			INNER JOIN CommonCostCenter cc
			ON br.CostCenterId = cc.CostCenterId
						
			LEFT JOIN BanquetSync sbs
			ON sbs.GuidId = br.GuidId
			

			SELECT a.Id, hgbp.RegistrationId, hgbp.PaymentMode
			INTO ##Payment
			FROM ##a a
			INNER JOIN HotelGuestBillPayment hgbp
			ON hgbp.ServiceBillId = a.Id 
			WHERE hgbp.ModuleName = 'Banquet'

		
			SELECT a.Id, ReservationNumber,	r.RegistrationId,	CostCenter,	RoundedGrandTotal,SettlementDate,
				   GuidId, IsSyncCompleted, p.PaymentMode
			FROM
			(
				SELECT Id,			
				PaymentMode = STUFF((
				SELECT N', ' + PaymentMode FROM ##Payment
				WHERE Id = x.Id
				ORDER BY Id DESC
				FOR XML PATH(''), TYPE).value(N'.[1]', N'nvarchar(max)'), 1, 2, N'')
				FROM ##Payment AS x				
				GROUP BY Id 
			)p INNER JOIN ##a a ON p.Id = a.Id
			INNER JOIN
			(
				SELECT Id, MAX(RegistrationId) RegistrationId FROM ##Payment GROUP BY Id
			)r ON p.Id = r.Id
			WHERE (a.IsSyncCompleted = 0 AND a.GuidId IS NOT NULL) OR
				  (a.GuidId IS NULL AND (a.SettlementDate BETWEEN @FromDate AND @TODATE))		
			ORDER BY a.Id DESC


			DROP TABLE ##a, ##Payment	 
	
END
GO


/****** Object:  StoredProcedure [dbo].[GetSyncedBanquetBillByRegistrationId_SP]    Script Date: 10/31/2018 6:00:01 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSyncedBanquetBillByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSyncedBanquetBillByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetSyncedBanquetBillByRegistrationId_SP]    Script Date: 10/31/2018 6:00:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: GetSyncedBanquetBillByRegistrationId_SP
  Description			: This procedure returns Synced BanquetBill By RegistrationId
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetSyncedBanquetBillByRegistrationId_SP]
	(
	@Id		BIGINT
)
AS
BEGIN
	SELECT br.Id,
		   br.RegistrationId,
		   ccc.CostCenter,
		   rs.GuidId,
		   rs.IsSyncCompleted

	FROM BanquetReservation br
	INNER JOIN RestaurantBillSync rs
	ON br.GuidId = rs.GuidId
	INNER JOIN CommonCostCenter ccc
	ON br.CostCenterId = ccc.CostCenterId
	WHERE br.Id = @Id
END
GO

/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncBanquetInformation_SP]    Script Date: 10/31/2018 6:38:53 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveOrUpdateSyncBanquetInformation_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveOrUpdateSyncBanquetInformation_SP]
GO

/****** Object:  StoredProcedure [dbo].[SaveOrUpdateSyncBanquetInformation_SP]    Script Date: 10/31/2018 6:38:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*=================================================================== 
  Author				: 31/10/2018
  Create date			: NKD
  Name					: SaveOrUpdateSyncBanquetInformation_SP
  Description			: This procedure Save Or Update Banquet Sync Information
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[SaveOrUpdateSyncBanquetInformation_SP] 
	(
		@GuidId		UNIQUEIDENTIFIER,
		@IsSyncCompleted BIT
	)
AS
BEGIN
IF EXISTS (SELECT * FROM BanquetSync WHERE GuidId = @GuidId)
BEGIN
	UPDATE BanquetSync
	SET IsSyncCompleted = @IsSyncCompleted
	WHERE GuidId = @GuidId
END
ELSE
	BEGIN
	INSERT INTO BanquetSync
				(
					GuidId,
					IsSyncCompleted
				)
				VALUES(
					@GuidId,
					@IsSyncCompleted
				)
	END

END
GO

/****** Object:  StoredProcedure [dbo].[GetGuestBillByRegistrationId_SP]    Script Date: 11/1/2018 12:04:42 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGuestBillByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetGuestBillByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestBillByRegistrationId_SP]    Script Date: 11/1/2018 12:04:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*=================================================================== 
  Author				: NKD
  Create date			: 23-10-2018
  Name					: GetGuestBillByRegistrationId_SP
  Description			: This procedure get GuestBill by RegistrationId for sync
 ===================================================================
  CHANGE HISTORY
 ===================================================================
  Date				Name	Comments  
 ===================================================================*/

CREATE PROCEDURE [dbo].[GetGuestBillByRegistrationId_SP]
	(
		@RegistrationId		INT	
	)
AS
BEGIN
	SELECT hgb.*,
		   hrr.GuidId GuidId
		   FROM HotelGuestBillPayment hgb
		   LEFT JOIN HotelRoomRegistration hrr
		   ON hgb.RegistrationId = hrr.RegistrationId
		WHERE hgb.RegistrationId = @RegistrationId
END
GO

/****** Object:  StoredProcedure [dbo].[GetHotelGuestServiceBillById_SP]    Script Date: 11/1/2018 1:15:03 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHotelGuestServiceBillById_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHotelGuestServiceBillById_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetHotelGuestServiceBillById_SP]    Script Date: 11/1/2018 1:15:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 1/11/2018
  Name					: GetHotelGuestServiceBillById_SP
  Description			: This procedure returns Hotel Guest ServiceBill By RegistrationId For Sync
  ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetHotelGuestServiceBillById_SP] 
	(
		@ServiceBillId		INT
	)
AS
BEGIN
	SELECT hgs.*,
		   hrr.GuidId RegistrationGuidId
		   FROM HotelGuestServiceBill hgs
		   LEFT JOIN HotelRoomRegistration hrr
		   ON hgs.RegistrationId = hrr.RegistrationId 
		   WHERE ServiceBillId = @ServiceBillId
END
GO


/****** Object:  StoredProcedure [dbo].[GetHotelGuestServiceBillByIdForSync_SP]    Script Date: 11/1/2018 1:38:15 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHotelGuestServiceBillByIdForSync_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHotelGuestServiceBillByIdForSync_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetHotelGuestServiceBillByIdForSync_SP]    Script Date: 11/1/2018 1:38:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 1/11/2018
  Name					: GetHotelGuestServiceBillByIdForSync_SP
  Description			: This procedure returns Banquet Reservation By RegistrationId For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/
CREATE PROCEDURE [dbo].[GetHotelGuestServiceBillByIdForSync_SP] 
	(
		@Id		INT
	)
AS
BEGIN
	SELECT br.*,
		   hrr.GuidId RegistrationGuidId
		   FROM BanquetReservation br
		   LEFT JOIN HotelRoomRegistration hrr
		   ON br.RegistrationId = hrr.RegistrationId 
		   WHERE br.Id = @Id
END
GO

/****** Object:  StoredProcedure [dbo].[GetGuestExtraServiceBillApprovedByServiceBillId_SP]    Script Date: 11/1/2018 2:16:38 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGuestExtraServiceBillApprovedByServiceBillId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetGuestExtraServiceBillApprovedByServiceBillId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestExtraServiceBillApprovedByServiceBillId_SP]    Script Date: 11/1/2018 2:16:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 1/11/2018
  Name					: GetGuestExtraServiceBillApprovedByServiceBillId_SP
  Description			: This procedure returns Restaurant Bill By RegistrationId For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/
CREATE PROCEDURE [dbo].[GetGuestExtraServiceBillApprovedByServiceBillId_SP]
	(
		@ServiceBillId	INT
	)
AS
BEGIN
	SELECT hge.*,
		   hrr.GuidId GuidId
		   FROM HotelGuestExtraServiceBillApproved hge
		   LEFT JOIN HotelRoomRegistration hrr
		   ON hge.RegistrationId = hrr.RegistrationId

		   WHERE hge.ServiceBillId = @ServiceBillId
END
GO

/****** Object:  StoredProcedure [dbo].[GetBillInfoByBillIdForAPI_SP]    Script Date: 11/1/2018 2:27:48 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBillInfoByBillIdForAPI_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBillInfoByBillIdForAPI_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetBillInfoByBillIdForAPI_SP]    Script Date: 11/1/2018 2:27:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*=================================================================== 
  Author				: NKD
  Create date			: 1/11/2018
  Name					: GetBillInfoByBillIdForAPI_SP
  Description			: This procedure returns Restaurant Bill By RegistrationId For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

CREATE PROCEDURE [dbo].[GetBillInfoByBillIdForAPI_SP]
	(
		@BillId	INT
	)
AS
BEGIN
	SELECT hb.*,
		   hrr.GuidId RegistrationGuidId
		   FROM RestaurantBill hb
		   LEFT JOIN HotelRoomRegistration hrr
		   ON hb.RegistrationId = hrr.RegistrationId

		   WHERE hb.BillId = @BillId
END
GO

/****** Object:  StoredProcedure [dbo].[GetGuestByRegistrationIdForSync_SP]    Script Date: 11/5/2018 3:00:00 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGuestByRegistrationIdForSync_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetGuestByRegistrationIdForSync_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestByRegistrationIdForSync_SP]    Script Date: 11/5/2018 3:00:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*=================================================================== 
  Author				: NKD
  Create date			: 5/11/2018
  Name					: GetGuestBillByRegistrationIdForSync_SP
  Description			: This procedure returns Guest By RegistrationId For Sync
 ===================================================================
  CHANGE HISTORY
  ===================================================================
  Date				Name	Comments
  ===================================================================*/

/* EXEC GetGuestByRegistrationIdForSync_SP 22*/
CREATE PROCEDURE [dbo].[GetGuestByRegistrationIdForSync_SP] 
	(
		@RegistrationId		INT
	)
AS
BEGIN
	SELECT hgi.* 
		FROM HotelRoomRegistration rr
		INNER JOIN HotelGuestRegistration hgr
		ON rr.RegistrationId = hgr.RegistrationId
		INNER JOIN HotelGuestInformation hgi
		ON hgi.GuestId = hgr.GuestId
		WHERE hgr.RegistrationId = @RegistrationId
END
GO


/****** Object:  StoredProcedure [dbo].[GetGuestRegistrationMappingByRegistrationId_SP]    Script Date: 10/16/2018 4:30:26 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGuestRegistrationMappingByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetGuestRegistrationMappingByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestRegistrationMappingByRegistrationId_SP]    Script Date: 10/16/2018 4:30:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: NKD
  Create date			: 23-10-2018
  Name					: GetGuestRegistrationMappingByRegistrationId_SP
  Description			: This procedure get GuestRegistrationMapping information by RegistrationId for sync
 ===================================================================
  CHANGE HISTORY
 ===================================================================
  Date				Name	Comments  
 ===================================================================*/

CREATE PROCEDURE [dbo].[GetGuestRegistrationMappingByRegistrationId_SP] 
	(
		@RegistrationId		BIGINT
	)
AS
BEGIN
	SELECT * FROM HotelGuestRegistration
	WHERE RegistrationId = @RegistrationId

END
GO



/****** Object:  StoredProcedure [dbo].[GetHotelGuestApprovedBillByRegistrationId_SP]    Script Date: 10/22/2018 6:29:20 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHotelGuestApprovedBillByRegistrationId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHotelGuestApprovedBillByRegistrationId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetHotelGuestApprovedBillByRegistrationId_SP]    Script Date: 10/22/2018 6:29:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*=================================================================== 
  Author				: NKD
  Create date			: 23-10-2018
  Name					: GetHotelGuestApprovedBillByRegistrationId_SP
  Description			: This procedure get HotelGuestApprovedBill for sync
 ===================================================================
  CHANGE HISTORY
 ===================================================================
  Date				Name	Comments  
 ===================================================================*/

CREATE PROCEDURE [dbo].[GetHotelGuestApprovedBillByRegistrationId_SP]
	(
		@RegistrationId		INT
	
	)
AS
BEGIN
	SELECT * FROM HotelGuestBillApproved
	WHERE RegistrationId = @RegistrationId
END
GO

/****** Object:  StoredProcedure [dbo].[GetCompanyPaymentLedgerByRegistrationIdAndModuleName_SP]    Script Date: 11/7/2018 7:14:23 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCompanyPaymentLedgerByRegistrationIdAndModuleName_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCompanyPaymentLedgerByRegistrationIdAndModuleName_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetCompanyPaymentLedgerByRegistrationIdAndModuleName_SP]    Script Date: 11/7/2018 7:14:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*=================================================================== 
  Author				: NKD
  Create date			: 23-10-2018
  Name					: GetCompanyPaymentLedgerByRegistrationIdAndModuleName_SP
  Description			: This procedure get CompanyPaymentLedger by RegistrationId And ModuleName for sync
 ===================================================================
  CHANGE HISTORY
 ===================================================================
  Date				Name	Comments  
 ===================================================================*/
CREATE PROCEDURE [dbo].[GetCompanyPaymentLedgerByRegistrationIdAndModuleName_SP]
	(
		@RegistrationId		INT
	)
AS
BEGIN
	SELECT hcpl.*
		   FROM HotelCompanyPaymentLedger hcpl
		   INNER JOIN HotelGuestBillPayment hgbp 
		   ON hcpl.PaymentId = hgbp.PaymentId

		   WHERE hgbp.RegistrationId = @RegistrationId 
				 AND hgbp.ModuleName = 'FrontOffice'

END
GO

/****** Object:  StoredProcedure [dbo].[GetCompanyPaymentLedgerByModuleNameAndBillId_SP]    Script Date: 11/7/2018 5:43:26 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCompanyPaymentLedgerByModuleNameAndBillId_SP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCompanyPaymentLedgerByModuleNameAndBillId_SP]
GO

/****** Object:  StoredProcedure [dbo].[GetCompanyPaymentLedgerByModuleNameAndBillId_SP]    Script Date: 11/7/2018 5:43:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetCompanyPaymentLedgerByModuleNameAndBillId_SP]
	(
		@BillId		INT,
		@ModuleName	VARCHAR(100)
	
	)
AS
BEGIN
	SELECT * 
	FROM HotelCompanyPaymentLedger 
	WHERE BillId = @BillId AND ModuleName = @ModuleName
END
GO