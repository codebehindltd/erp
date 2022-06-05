DECLARE @CompanyID INT
DECLARE @getCompanyID CURSOR
SET @getCompanyID = CURSOR FOR
SELECT CompanyId
FROM HotelGuestCompany
OPEN @getCompanyID
FETCH NEXT
FROM @getCompanyID INTO @CompanyID
WHILE @@FETCH_STATUS = 0
BEGIN
	--PRINT @CompanyID
	INSERT INTO SMContactInformation
	(
		Name,
		ContactNo,
		MobilePersonal,
		ContactOwnerId,
		CompanyId,
		JobTitle,
		Email,
		PersonalAddress,
		LastContactDateTime,
		CreatedBy,
		CreatedDate,
		IsDeleted,
		LifeCycleId
	)

	SELECT
		ContactPerson,
		ContactNumber,
		ContactNumber,
		CompanyOwnerId,
		@CompanyID,
		ContactDesignation,
		EmailAddress,
		CompanyAddress,
		CreatedDate,
		CreatedBy,
		CreatedDate,
		0,
		LifeCycleStageId
	FROM HotelGuestCompany
	WHERE CompanyId = @CompanyID
	AND ContactPerson IS NOT NULL
	
FETCH NEXT
FROM @getCompanyID INTO @CompanyID
END
CLOSE @getCompanyID
DEALLOCATE @getCompanyID