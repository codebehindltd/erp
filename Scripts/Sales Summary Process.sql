GO

/****** Object:  StoredProcedure [dbo].[SalesSummaryProcessForSupport]    Script Date: 08/04/2018 11:53:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesSummaryProcessForSupport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SalesSummaryProcessForSupport]
GO

/****** Object:  StoredProcedure [dbo].[SalesSummaryProcessForSupport]    Script Date: 08/04/2018 11:53:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SalesSummaryProcessForSupport]
	@FromDate            DATETIME,
    @ToDate              DATETIME
AS
BEGIN
	--EXEC SalesSummaryProcessForSupport '2018-10-29', '2018-10-29' -- -- YYYY-MM-DD
	
	-- -- Declaration  ----------------
	DECLARE @BillingStartDate    DATETIME,
			@counter			 INT

	SET @counter = 0
	SELECT @counter = DATEDIFF(DAY, @FromDate, @ToDate) + 1 

	IF @counter > 0
	BEGIN
		DECLARE @StartFrom INT
		SET @StartFrom = 0
	    
		WHILE (@StartFrom <= @counter -1)
		BEGIN
			PRINT @FromDate
			DECLARE @TransactionDate VARCHAR(100)
			SET @TransactionDate = @FromDate
			DELETE FROM HotelSalesSummary WHERE dbo.FnDate(SummaryDate) = dbo.FnDate(@TransactionDate)
			DELETE FROM HotelPaymentSummary WHERE dbo.FnDate(TransactionDate) = dbo.FnDate(@TransactionDate)
			EXEC SalesAuditProcessingInfo_SP
			@AuditDate = @TransactionDate

			-- -- Counter Increment  ----------------
			SET @FromDate = DATEADD(DAY, 1, @FromDate)
			SET @StartFrom = @StartFrom + 1
		END
	END
END
GO