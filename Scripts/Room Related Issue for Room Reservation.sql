--SELECT * FROM HotelRoomNumber WHERE RoomNumber = 702
SELECT * 
FROM HotelRoomReservationDetail
WHERE ReservationId IN (
	SELECT ReservationId
	FROM HotelRoomReservation
	WHERE dbo.FnDate(DateIn) > dbo.FnDate('2022-05-23')
)
AND RoomId = 21

--UPDATE HotelRoomReservationDetail SET RoomId = 0 WHERE ReservationDetailId IN (
--4424,
--4427,
--4430,
--6680)