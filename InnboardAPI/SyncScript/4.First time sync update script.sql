
IF NOT EXISTS(SELECT 1 FROM BanquetSync)
BEGIN
	INSERT INTO BanquetSync(GuidId,IsSyncCompleted ) 
	SELECT br.GuidId , IsSyncCompleted = 1 FROM  BanquetReservation br
END
IF NOT EXISTS(SELECT 1 FROM RegistrationSync)
BEGIN
	INSERT INTO RegistrationSync(GuidId,IsSyncCompleted ) 
	SELECT rr.GuidId , IsSyncCompleted = CASE WHEN rr.CheckOutDate IS NULL THEN 0
										ELSE 1 END
	  FROM HotelRoomRegistration rr
		
END
IF NOT EXISTS(SELECT 1 FROM RestaurantBillSync)
BEGIN
	INSERT INTO RestaurantBillSync(GuidId,IsSyncCompleted ) 
	SELECT rb.GuidId , IsSyncCompleted=1 FROM RestaurantBill rb
END
IF NOT EXISTS(SELECT 1 FROM ServiceBillSync)
BEGIN
	INSERT INTO ServiceBillSync(GuidId,IsSyncCompleted ) 
	SELECT hgsb.GuidId , IsSyncCompleted=1 FROM HotelGuestServiceBill hgsb
END


	