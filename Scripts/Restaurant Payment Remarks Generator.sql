--SELECT *, ROW_NUMBER() OVER(ORDER BY BillId) RowNo
--INTO #Bill
--FROM RestaurantBill

--SELECT * FROM RestaurantBill

--select * from HotelGuestBillPayment

--select ServiceBillId, RoomNumber, PaymentMode, CalculatedTotalAmount 
--from HotelGuestExtraServiceBillApproved where ServiceType = 'RestaurantService'

DECLARE @PaymentRemarks TABLE(  
	BillId			BIGINT,
	PaymentRemarks	NVARCHAR(150)
)

INSERT INTO @PaymentRemarks
SELECT BillId, PaymentRemarks

FROM
(
	SELECT BillId, CASE WHEN PaymentAmount > 0 THEN
				 CASE WHEN LEN(PaymentDescription) > 0 THEN PaymentMode + '(' + PaymentDescription + ')' + ':' + CONVERT(VARCHAR(20), PaymentAmount)
				 ELSE PaymentMode + ':' + CONVERT(VARCHAR(20), PaymentAmount) END
			  ELSE 
				CASE WHEN  IsNonChargeable = 1 THEN 'Non Chargeable(NC)' ELSE 
				CASE WHEN  IsComplementary = 1 THEN 'Complementary' ELSE
				'' END 
			  END END PaymentRemarks
	FROM
	(
		SELECT rb.BillId, rb.BillDate, bp.PaymentMode, bp.PaymentDescription, bp.PaymentAmount, rb.IsComplementary, rb.IsNonChargeable
		FROM 
		RestaurantBill rb INNER JOIN
		HotelGuestBillPayment bp ON rb.BillId = bp.ServiceBillId AND ModuleName = 'Restaurant'		
		WHERE ISNULL(rb.IsBillSettlement, 0) = 1 AND bp.ModuleName = 'Restaurant'

		UNION

		SELECT rb.BillId, rb.BillDate, ('Room#' + gb.RoomNumber) PaymentMode, '' PaymentDescription, CalculatedTotalAmount PaymentAmount, 0 IsComplementary, 0 IsNonChargeable
		FROM 
		RestaurantBill rb INNER JOIN
		HotelGuestExtraServiceBillApproved gb ON rb.BillId = gb.ServiceBillId AND gb.ServiceType = 'RestaurantService'
		WHERE ISNULL(rb.IsBillSettlement, 0) = 1 AND gb.ServiceType = 'RestaurantService'
	 
	)bpd
)pr
GROUP BY BillId, PaymentRemarks

UPDATE rb 
SET rb.PaymentRemarks = r.Remarks
FROM RestaurantBill rb 
INNER JOIN
(
	SELECT Main.BillId,
		   LEFT(Main.Remarks,Len(Main.Remarks)-1) As Remarks
	FROM
		(
			SELECT DISTINCT ST2.BillId, 
				(
					SELECT ST1.PaymentRemarks + ',' AS [text()]
					FROM @PaymentRemarks ST1
					WHERE ST1.BillId = ST2.BillId
					ORDER BY ST1.BillId
					FOR XML PATH ('')
				) [Remarks]
			FROM @PaymentRemarks ST2
		) [Main]
)r ON r.BillId = rb.BillId

--SELECT Main.BillId,
--       LEFT(Main.Remarks,Len(Main.Remarks)-1) As Remarks
--FROM
--    (
--        SELECT DISTINCT ST2.BillId, 
--            (
--                SELECT ST1.PaymentRemarks + ',' AS [text()]
--                FROM @PaymentRemarks ST1
--                WHERE ST1.BillId = ST2.BillId
--                ORDER BY ST1.BillId
--                FOR XML PATH ('')
--            ) [Remarks]
--        FROM @PaymentRemarks ST2
--    ) [Main]

--SELECT * FROM @PaymentRemarks