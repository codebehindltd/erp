DECLARE @RegistrationId  INT
DECLARE @RoomId          INT
DECLARE @CheckOutId      INT
DECLARE @PaymentId       INT

SET @RegistrationId = 0
SET @RoomId = 0
SET @CheckOutId = 0
SET @PaymentId = 0


select * from HotelRoomNumber where RoomId = @RoomId
select * from HotelRoomRegistration where RegistrationId = @RegistrationId
select * from HotelGuestBillPayment where RegistrationId = @RegistrationId-- AND PaymentId = @PaymentId
select * from HotelGuestHouseCheckOut where RegistrationId = @RegistrationId

--UPDATE HotelRoomRegistration
--SET    CheckOutDate = NULL
--WHERE  RegistrationId = @RegistrationId

--UPDATE HotelRoomNumber
--SET    StatusId = 2,
--       HKRoomStatusId = 2,
--       CleanupStatus = 'Cleaned'
--WHERE  RoomId = @RoomId
 
--DELETE 
--FROM   HotelGuestBillPayment
--WHERE  RegistrationId = @RegistrationId
--       AND PaymentId = @PaymentId

--DELETE 
--FROM   HotelGuestHouseCheckOut
--WHERE  RegistrationId = @RegistrationId
--       AND CheckOutId = @CheckOutId
 
