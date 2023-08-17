GO
/****** Object:  StoredProcedure [dbo].[VatRelatedDataDelete]    Script Date: 08/11/2018 13:16:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VatRelatedDataDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[VatRelatedDataDelete]
GO

/****** Object:  StoredProcedure [dbo].[VatRelatedDataDelete]    Script Date: 08/11/2018 13:16:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[VatRelatedDataDelete]	
	@TransactionType	VARCHAR(100),
	@TransactionNumber VARCHAR(100)
AS
BEGIN
	--DECLARE @RegistrationIdList VARCHAR(MAX)
	--TRUNCATE TABLE TempRegistration
	--SET @RegistrationIdList= 'RR00007254,RR00007259,RR00007263,RR00007266,RR00007267'
	--INSERT INTO TempRegistration(RegistrationNumber)
	--SELECT splitdata FROM   dbo.fnSplitString(@RegistrationIdList, ',')

	--SELECT * FROM TempRegistration
	
	----SELECT * FROM HotelRoomRegistration WHERE RegistrationNumber = 'RR00007254'
	
	



	--DECLARE @TransactionType	VARCHAR(100),
	--		@TransactionNumber VARCHAR(100)
			
	--SET @TransactionType = 'BNumber'
	--SET @TransactionNumber = 'TB00002828'
	
	 ---- RegistrationNumber
	 ---- BillNumber
	 ---- BillNumber
	 
	-- select * from HotelRoomRegistration where RegistrationId in (
	--select RegistrationId from HotelGuestServiceBill where RegistrationId in (
	--SELECT RegistrationId FROM HotelRoomRegistration WHERE CheckOutDate > '2018-09-01' AND CheckOutDate < '2018-10-01'))
	
	--EXEC VatRelatedDataDelete 'RNumber', 'RR00000001'

	--EXEC VatRelatedDataDelete 'RNumber', 'RR00000001'

	----Reservation Base Data Remove Process-----------------Start
	----GO
	----DECLARE @RegistrationId INT
	----DECLARE @getRegistrationId CURSOR
	----SET @getRegistrationId = CURSOR FOR

	----SELECT RegistrationId FROM HotelRoomRegistration 
	----WHERE ReservationId IN(
	----	SELECT TOP 1 ReservationId FROM HotelRoomReservation WHERE ReservationNumber LIKE '%GR00000%'
	----)

	----OPEN @getRegistrationId
	----FETCH NEXT
	----FROM @getRegistrationId INTO @RegistrationId
	----WHILE @@FETCH_STATUS = 0
	----BEGIN
	----	 PRINT @RegistrationId
	----	DECLARE @RegistrationNumber VARCHAR(100)
	----	SELECT @RegistrationNumber = RegistrationNumber 
	----	FROM HotelRoomRegistration 
	----	WHERE RegistrationId = @RegistrationId
	----	AND CheckOutDate IS NOT NULL
	
	----	EXEC VatRelatedDataDelete 'RNumber', @RegistrationNumber

	----FETCH NEXT
	----FROM @getRegistrationId INTO @RegistrationId
	----END
	----CLOSE @getRegistrationId
	----DEALLOCATE @getRegistrationId
	----GO
	----Reservation Base Data Remove Process-----------------End
	
	DECLARE @DataCount INT	
	DECLARE @RegistrationId BIGINT
	SET @RegistrationId = 0
	
	IF @TransactionType = 'RNumber'
	BEGIN
		SELECT @RegistrationId = RegistrationId 
		FROM HotelRoomRegistration 
		WHERE RegistrationNumber = @TransactionNumber
		AND CheckOutDate IS NOT NULL
	END
	ELSE IF @TransactionType = 'BNumber'
	BEGIN
		SELECT @RegistrationId = ISNULL(RegistrationId, 0)
		FROM RestaurantBill 
		WHERE BillNumber = @TransactionNumber
	END
	ELSE IF @TransactionType = 'SNumber'
	BEGIN
		--DECLARE @ServiceBillId INT
		--SELECT @ServiceBillId = ServiceBillId FROM HotelGuestServiceBill WHERE BillNumber = 'SB00003365'

		------SELECT * FROM HotelGuestServiceBill WHERE ServiceBillId = @ServiceBillId
		------SELECT * FROM HotelGuestBillPayment WHERE ModuleName = 'FrontOffice' AND ServiceBillId = @ServiceBillId

		--DELETE FROM HotelGuestServiceBill WHERE ServiceBillId = @ServiceBillId
		--DELETE FROM HotelGuestBillPayment WHERE ModuleName = 'FrontOffice' AND ServiceBillId = @ServiceBillId

		SET @DataCount = 0
		SET @RegistrationId = 0
	END

	SET @DataCount = 0	
	BEGIN
		SELECT @DataCount = COUNT(RegistrationId) FROM HotelRoomRegistration 
		WHERE RegistrationId = @RegistrationId
		AND CheckOutDate IS NOT NULL
		
		SELECT @RegistrationId = RegistrationId 
		FROM HotelRoomRegistration 
		WHERE RegistrationId = @RegistrationId
		AND CheckOutDate IS NOT NULL
	END	
	
	IF @DataCount = 0
	BEGIN
		SET @RegistrationId = 0
	END

	--SELECT * FROM HotelRoomRegistration WHERE RegistrationId = @RegistrationId
	IF @RegistrationId > 0
	BEGIN
		--PRINT '11111111111111111111111'
		DELETE FROM HotelRoomRegistration WHERE RegistrationId = @RegistrationId
		DELETE FROM HotelGuestServiceBill WHERE RegistrationId = @RegistrationId
		DELETE FROM HotelGuestBillApproved WHERE RegistrationId = @RegistrationId
		DELETE FROM RestaurantBill WHERE RegistrationId = @RegistrationId
		DELETE FROM HotelGuestBillPayment WHERE RegistrationId = @RegistrationId
		DELETE FROM HotelGuestExtraServiceBillApproved WHERE RegistrationId = @RegistrationId
	END
	ELSE
	BEGIN
		IF @TransactionType = 'BNumber'
		BEGIN
			DECLARE @BillId BIGINT 
			DECLARE @KotId BIGINT
			SELECT @BillId = BillId, @RegistrationId = ISNULL(RegistrationId, 0) FROM RestaurantBill WHERE BillNumber = @TransactionNumber
			SELECT @KotId = KotId FROM RestaurantBillDetail WHERE BillId = @BillId
			IF @RegistrationId = 0
			BEGIN
				--PRINT '22222222222222222222'
				DELETE FROM RestaurantBill where BillId = @BillId
				DELETE FROM RestaurantBillDetail where BillId = @BillId
				DELETE FROM RestaurantKotBillMaster where KotId = @KotId
				DELETE FROM RestaurantKotBillDetail where KotId = @KotId
				DELETE FROM HotelGuestExtraServiceBillApproved where ServiceBillId = @BillId
				DELETE FROM HotelGuestBillPayment where ServiceBillId = @BillId and ModuleName = 'Restaurant'
			END
		END
		ELSE IF @TransactionType = 'SNumber'
		BEGIN
			DECLARE @ServiceBillId INT
			SELECT @ServiceBillId = ServiceBillId FROM HotelGuestServiceBill WHERE BillNumber = @TransactionNumber

			----SELECT * FROM HotelGuestServiceBill WHERE ServiceBillId = @ServiceBillId
			----SELECT * FROM HotelGuestBillPayment WHERE ModuleName = 'FrontOffice' AND ServiceBillId = @ServiceBillId

			DELETE FROM HotelGuestServiceBill WHERE ServiceBillId = @ServiceBillId
			DELETE FROM HotelGuestBillPayment WHERE ModuleName = 'FrontOffice' AND ServiceBillId = @ServiceBillId

			SET @DataCount = 0
			SET @RegistrationId = 0
		END
	END
END
GO