UPDATE HotelRoomLogFile
	SET ToDate = GETDATE()-1
WHERE RoomId IN (
					SELECT DISTINCT RoomId 
					FROM HotelRoomReservation rr
					INNER JOIN HotelRoomReservationDetail rd
						ON rr.ReservationId = rd.ReservationId
					WHERE rr.DateIn > GETDATE()-1
					AND rd.RoomId IN (
										SELECT RoomId 
										FROM HotelRoomLogFile
										WHERE FromDate > GETDATE()-1
									  )
				 )
AND ToDate > GETDATE()-1
