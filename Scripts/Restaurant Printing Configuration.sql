UPDATE CommonSetup
SET    SetupValue = '0'
WHERE  SetupName = 'IsRestaurantBillDirectPrint'
       AND TypeName = 'IsRestaurantBillDirectPrint'
GO

UPDATE CommonSetup
SET    SetupValue = '0'
WHERE  SetupName = 'IsRestaurantBillShowNDirectPrint'
       AND TypeName = 'IsRestaurantBillShowNDirectPrint'