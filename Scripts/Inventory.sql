TRUNCATE TABLE InvCategory 
TRUNCATE TABLE InvCategoryCostCenterMapping 
TRUNCATE TABLE InvDefaultClassificationConfiguration 
TRUNCATE TABLE InvItem 
TRUNCATE TABLE InvItemClassificationCostCenterMapping 
TRUNCATE TABLE InvItemCostCenterMapping 
TRUNCATE TABLE InvItemDetails 
TRUNCATE TABLE InvItemSpecialRemarks 
TRUNCATE TABLE InvItemStockAdjustment 
TRUNCATE TABLE InvItemSupplierMapping 
TRUNCATE TABLE InvLocation 
TRUNCATE TABLE InvLocationCostCenterMapping 
TRUNCATE TABLE RestaurantRecipeDetail
TRUNCATE TABLE InvCogsClosing
TRUNCATE TABLE InvCogsAccountVsItemCategoryMappping
TRUNCATE TABLE InvInventoryAccountVsItemCategoryMappping
TRUNCATE TABLE InvItemTransactionLog
TRUNCATE TABLE InvItemTransactionLogSummary
TRUNCATE TABLE InvItemTransactionPaymentDetails
TRUNCATE TABLE ItemEntry
TRUNCATE TABLE InvItemStockInformationLog
TRUNCATE TABLE InvItemTransactionHistory


-- -------------------------*All Tables*----------------------
SELECT * FROM InvCategory
SELECT * FROM InvCategoryCostCenterMapping
SELECT * FROM InvCostCenterNDineTimeWiseItemTransaction
SELECT * FROM InvDefaultClassificationConfiguration
SELECT * FROM InvDineTimeWiseItemTransaction
SELECT * FROM InvDineTimeWiseItemTransactionDetails
SELECT * FROM InvDineTimeWisePaymentDetails
SELECT * FROM InvItem
SELECT * FROM InvItemClassificationCostCenterMapping
SELECT * FROM InvItemCostCenterMapping
SELECT * FROM InvItemDetails
SELECT * FROM InvItemStockAdjustment
SELECT * FROM InvItemStockAdjustmentDetails
SELECT * FROM InvItemStockInformation
SELECT * FROM InvItemStockVariance
SELECT * FROM InvItemStockVarianceDetails
SELECT * FROM InvItemSupplierMapping
SELECT * FROM InvItemTransaction
SELECT * FROM InvItemTransactionDetails
SELECT * FROM InvItemTransactionPaymentDetails
SELECT * FROM InvRecipeDeductionDetails
SELECT * FROM RestaurantBill
SELECT * FROM RestaurantBillClassificationDiscount
SELECT * FROM RestaurantBillDetail
SELECT * FROM RestaurantBillPayment
SELECT * FROM RestaurantBuffet
SELECT * FROM RestaurantBuffetCostCenterMapping
SELECT * FROM RestaurantCombo
SELECT * FROM RestaurantComboCostCenterMapping
SELECT * FROM RestaurantKotBillDetail
SELECT * FROM RestaurantKotBillMaster
SELECT * FROM RestaurantKotSpecialRemarksDetail
SELECT * FROM RestaurantRecipeDetail
SELECT * FROM RestaurantReservation
SELECT * FROM RestaurantReservationItemDetail
SELECT * FROM RestaurantReservationTableDetail
SELECT * FROM RestaurantTableReservationDetail

SELECT * FROM PMFinishedProduct
SELECT * FROM PMFinishedProductDetails
SELECT * FROM PMProductOut
SELECT * FROM PMProductOutDetails
SELECT * FROM PMProductReceived
SELECT * FROM PMProductReceivedBillPayment
SELECT * FROM PMProductReceivedDetails
SELECT * FROM PMProductReturn
SELECT * FROM PMProductSerialInfo
SELECT * FROM PMPurchaseOrdeR
SELECT * FROM PMPurchaseOrderDetails
SELECT * FROM PMRequisition
SELECT * FROM PMRequisitionDetails
