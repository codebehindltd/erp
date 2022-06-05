DECLARE @TransactionDate DATETIME
SET @TransactionDate = dbo.FnDate('2019-12-01')
truncate table RegistrationSynctruncate table BanquetSynctruncate table RestaurantBillSynctruncate table ServiceBillSync

IF NOT EXISTS(SELECT 1 FROM BanquetSync)
BEGIN
	INSERT INTO BanquetSync(GuidId,IsSyncCompleted ) 
	SELECT br.GuidId , IsSyncCompleted = 1 FROM  BanquetReservation br
END
IF NOT EXISTS(SELECT 1 FROM RegistrationSync)
BEGIN
	INSERT INTO RegistrationSync(GuidId,IsSyncCompleted ) 
	SELECT rr.GuidId , IsSyncCompleted = 1
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


UPDATE RegistrationSync
SET IsSyncCompleted = 0
WHERE GuidId IN (SELECT GuidId FROM HotelRoomRegistration WHERE GuidId IN('06BA92DB-F75C-4715-AF64-FC2FC25D901B'))

DELETE FROM RegistrationSync WHERE GuidId IN(SELECT GuidId FROM HotelRoomRegistration WHERE CreatedDate > @TransactionDate)
DELETE FROM ServiceBillSync WHERE GuidId IN(SELECT GuidId FROM HotelGuestServiceBill WHERE ServiceDate > @TransactionDate)
DELETE FROM RestaurantBillSync WHERE GuidId IN (SELECT GuidId FROM RestaurantBill WHERE CreatedDate > @TransactionDate)
DELETE FROM BanquetSync WHERE GuidId IN  (SELECT GuidId FROM BanquetReservation WHERE CreatedDate > @TransactionDate)


UPDATE CommonSetup 
	SET SetupValue = convert(varchar, @TransactionDate, 127)

 WHERE TypeName = 'LastSyncDateTime' AND SetupName = 'LastSyncDateTime' 

