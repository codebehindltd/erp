--TRUNCATE TABLE RestaurantBearer
TRUNCATE TABLE RestaurantBill
TRUNCATE TABLE RestaurantBillDetail
TRUNCATE TABLE RestaurantBillPayment
--TRUNCATE TABLE RestaurantBuffet
--TRUNCATE TABLE RestaurantBuffetDetail
--TRUNCATE TABLE RestaurantCombo
--TRUNCATE TABLE RestaurantComboDetail
--TRUNCATE TABLE RestaurantCostCenterTableMapping
TRUNCATE TABLE RestaurantKotRecipeDetail
TRUNCATE TABLE RestaurantKotBillDetail
TRUNCATE TABLE RestaurantKotBillMaster
TRUNCATE TABLE RestaurantKotSpecialRemarksDetail
TRUNCATE TABLE InvItemStockInformation
TRUNCATE TABLE InvRecipeDeductionDetails
TRUNCATE TABLE InvCogsClosing
TRUNCATE TABLE InvItemStockAdjustment
TRUNCATE TABLE InvItemStockAdjustmentDetails
TRUNCATE TABLE InvItemStockVariance
TRUNCATE TABLE InvItemStockVarianceDetails
TRUNCATE TABLE PMSupplierPayment
TRUNCATE TABLE PMSupplierPaymentDetails
TRUNCATE TABLE RestaurantKotPendingList
TRUNCATE TABLE RestaurantBillClassificationDiscount
TRUNCATE TABLE RestaurantSalesReturn
TRUNCATE TABLE RestaurantSalesReturnDetails
TRUNCATE TABLE RestaurantSalesReturnItem

TRUNCATE TABLE InvProduction
TRUNCATE TABLE InvProductionRMDetails
TRUNCATE TABLE InvProductionFGDetails

UPDATE RestaurantCostCenterTableMapping SET StatusId = 1
UPDATE InvItem SET PurchasePrice = 0, AverageCost = 0

--TRUNCATE TABLE HotelGuestBillPayment

--TRUNCATE TABLE PMManufacturer
--TRUNCATE TABLE PMProduct
--TRUNCATE TABLE PMProductCategory
--TRUNCATE TABLE PMProductCostCenterMapping
--TRUNCATE TABLE PMProductOut
--TRUNCATE TABLE PMProductReceived
--TRUNCATE TABLE PMProductReturn
--TRUNCATE TABLE PMProductSerialInfo
--TRUNCATE TABLE PMPurchaseOrder
--TRUNCATE TABLE PMPurchaseOrderDetails
--TRUNCATE TABLE PMRequisition
--TRUNCATE TABLE PMSales
--TRUNCATE TABLE PMSalesBillingInfo
--TRUNCATE TABLE PMSalesBillPayment
--TRUNCATE TABLE PMSalesDetail
--TRUNCATE TABLE PMSalesInvoice
--TRUNCATE TABLE PMSalesSiteInfo
--TRUNCATE TABLE PMSalesTechnicalInfo