DECLARE @PassportNumber   VARCHAR(100),
        @Deleted_GuestId  INT,
        @Count            INT
        
SET @PassportNumber = ''

DECLARE @ProductID     INT
DECLARE @getProductID  CURSOR    
SET @getProductID =     CURSOR FOR

SELECT hgi.GuestId
FROM   HotelGuestInformation hgi
ORDER BY
       hgi.GuestId DESC

OPEN @getProductID
FETCH NEXT
FROM @getProductID INTO @ProductID
WHILE @@FETCH_STATUS = 0
BEGIN
    --PRINT @ProductID
    
    SELECT @PassportNumber = LTRIM(RTRIM(hgi.PassportNumber))
    FROM   HotelGuestInformation hgi
    WHERE  hgi.GuestId = @ProductID
    
    IF (@PassportNumber <> '' AND @PassportNumber IS NOT NULL)
    BEGIN
        PRINT @PassportNumber
        
        SELECT @Count = COUNT(hgi.GuestId)
        FROM   HotelGuestInformation hgi
        WHERE  LTRIM(RTRIM(hgi.PassportNumber)) = @PassportNumber
               AND @ProductID <> hgi.GuestId
        
        WHILE (@Count > 0)
        BEGIN
            PRINT '1111111111111111111111111111'
            SELECT TOP 1 @Deleted_GuestId = hgi.GuestId
            FROM   HotelGuestInformation hgi
            WHERE  LTRIM(RTRIM(hgi.PassportNumber)) = @PassportNumber
                   AND @ProductID <> hgi.GuestId
            ORDER BY
                   hgi.GuestId DESC
            
            UPDATE HotelGuestRegistration
            SET    GuestId = @ProductID
            WHERE  GuestId = @Deleted_GuestId
            
            UPDATE HotelGuestReservation
            SET    GuestId = @ProductID
            WHERE  GuestId = @Deleted_GuestId
            
            DELETE 
            FROM   HotelGuestInformation
            WHERE  GuestId = @Deleted_GuestId
            
            SELECT @Count = COUNT(hgi.GuestId)
            FROM   HotelGuestInformation hgi
            WHERE  LTRIM(RTRIM(hgi.PassportNumber)) = @PassportNumber
                   AND @ProductID <> hgi.GuestId
        END
    END
    
    SET @Count = 0
    SET @PassportNumber = ''
    FETCH NEXT
    FROM @getProductID INTO @ProductID
END
CLOSE @getProductID
DEALLOCATE @getProductID


SELECT GuestId,
       GuestName,
       GuestDOB,
       GuestEmail,
       GuestPhone,
       GuestCountryId,
       NationalId,
       PassportNumber,
       ROW_NUMBER() OVER(
           PARTITION BY PassportNumber ORDER BY PassportNumber,
           GuestId DESC
       )GuestRank
FROM   HotelGuestInformation
WHERE  PassportNumber IS NOT NULL
       AND LTRIM(RTRIM(PassportNumber)) != ''
       AND LTRIM(RTRIM(PassportNumber)) != '.'
