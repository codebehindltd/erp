GO
/****** Object:  StoredProcedure [dbo].[UpdateRestaurantBillDate]    Script Date: 08/02/2018 10:44:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRestaurantBillDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateRestaurantBillDate]
GO

/****** Object:  StoredProcedure [dbo].[UpdateRestaurantBillDate]    Script Date: 08/02/2018 10:44:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateRestaurantBillDate]
	@BillNumber VARCHAR(50), 
	@UpdatedTransactionDate DATETIME
AS
BEGIN
	--EXEC UpdateRestaurantBillDate 'RS00000569', '2018-07-12' -- -- YYYY-MM-DD
	DECLARE @BillId INT
	DECLARE @PaymentId INT
	DECLARE @TransactionType VARCHAR(100)
	DECLARE @TransactionId INT

	SELECT @BillId = BillId, @TransactionType = TransactionType, @TransactionId = TransactionId FROM RestaurantBill 
	WHERE BillNumber = @BillNumber

	SELECT @PaymentId = PaymentId FROM HotelGuestBillPayment 
	WHERE ServiceBillId = @BillId AND PaymentType = 'Advance' AND ModuleName = 'Restaurant'

	-- -- -- RestaurantBill
	UPDATE RestaurantBill 
	SET BillDate = DATEADD( day, DATEDIFF( day, BillDate, @UpdatedTransactionDate ), BillDate ),
		CreatedDate = DATEADD( day, DATEDIFF( day, CreatedDate, @UpdatedTransactionDate ), CreatedDate )
	WHERE BillNumber = @BillNumber

	-- -- -- HotelGuestBillPayment
	UPDATE HotelGuestBillPayment 
	SET PaymentDate = DATEADD( day, DATEDIFF( day, PaymentDate, @UpdatedTransactionDate ), PaymentDate ),
		TransactionDate = DATEADD( day, DATEDIFF( day, TransactionDate, @UpdatedTransactionDate ), TransactionDate ),
		ChecqueDate = DATEADD( day, DATEDIFF( day, ChecqueDate, @UpdatedTransactionDate ), ChecqueDate ),
		CreatedDate = DATEADD( day, DATEDIFF( day, CreatedDate, @UpdatedTransactionDate ), CreatedDate )
	WHERE ServiceBillId = @BillId AND ModuleName = 'Restaurant'

	-- -- -- HotelGuestExtraServiceBillApproved
	UPDATE HotelGuestExtraServiceBillApproved 
	SET ApprovedDate = DATEADD( day, DATEDIFF( day, ApprovedDate, @UpdatedTransactionDate ), ApprovedDate ), 
		ServiceDate = DATEADD( day, DATEDIFF( day, ServiceDate, @UpdatedTransactionDate ), ServiceDate ),
		CreatedDate = DATEADD( day, DATEDIFF( day, CreatedDate, @UpdatedTransactionDate ), CreatedDate )
	WHERE ServiceBillId = @BillId

	-- -- -- RestaurantKotBillMaster
	UPDATE RestaurantKotBillMaster 
	SET KotDate = DATEADD( day, DATEDIFF( day, KotDate, @UpdatedTransactionDate ), KotDate ),
		CreatedDate = DATEADD( day, DATEDIFF( day, CreatedDate, @UpdatedTransactionDate ), CreatedDate )
	WHERE KotId IN (SELECT KotId FROM RestaurantBillDetail WHERE BillId in (@BillId))

	IF @TransactionType = 'Company'
	BEGIN
		UPDATE HotelCompanyPaymentLedger
		SET PaymentDate = DATEADD( day, DATEDIFF( day, PaymentDate, @UpdatedTransactionDate ), PaymentDate ),
			CreatedDate = DATEADD( day, DATEDIFF( day, CreatedDate, @UpdatedTransactionDate ), CreatedDate )
		WHERE PaymentId = @PaymentId AND CompanyId = @TransactionId
	END
	ELSE IF @TransactionType = 'Employee'
	BEGIN
		UPDATE PayrollEmployeePaymentLedger
		SET PaymentDate = DATEADD( day, DATEDIFF( day, PaymentDate, @UpdatedTransactionDate ), PaymentDate ),
		CreatedDate = DATEADD( day, DATEDIFF( day, CreatedDate, @UpdatedTransactionDate ), CreatedDate )
		WHERE PaymentId = @PaymentId AND EmployeeId = @TransactionId
	END
	--ELSE IF @TransactionType = 'Member'
	--BEGIN
	--	UPDATE PMMemberPaymentLedger
	--	SET PaymentDate = PaymentDate - 1
	--	WHERE PaymentId = @PaymentId AND MemberId = @TransactionId
	--END
END
GO