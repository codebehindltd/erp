DECLARE @TransactionDate DATETIME
SET @TransactionDate = dbo.FnDate('2019-12-01')

-- -- -- -- Dynamically Processing -----------------------------Need to Update
--UPDATE HotelRoomRegistration
--SET CheckOutDate = NULL,
--ActualCheckOutDate = NULL
--WHERE RegistrationId = 120

UPDATE HotelRoomRegistration
SET CheckOutDate = NULL,
ActualCheckOutDate = NULL
WHERE GuidId IN('06BA92DB-F75C-4715-AF64-FC2FC25D901B')

DELETE FROM HotelGuestHouseCheckOut 
WHERE dbo.FnDate(CheckOutDate) = dbo.FnDate(@TransactionDate) -- Room Check Out

UPDATE HotelRoomNumber
SET StatusId = 1,
HKRoomStatusId = 5,
CleanupStatus = 'Cleaned'

--DELETE FROM HotelRoomNumber WHERE RoomId = 114
UPDATE HotelRoomNumber
SET StatusId = 2
WHERE RoomId = 114
--return
-- -- -- -- Dynamically Processing -----------------------------End

--DELETE FROM HotelGuestInformation WHERE 
-- -- Registration
DELETE FROM HotelRegistrationAireportPickupDrop WHERE RegistrationId IN (SELECT RegistrationId FROM HotelRoomRegistration WHERE CreatedDate >= @TransactionDate )
DELETE FROM HotelRoomRegistration WHERE CreatedDate >= @TransactionDate  -- Detail Registration Information
DELETE FROM HotelGuestCompany WHERE CreatedDate >= @TransactionDate --If Company Guest Check In then Registration table update by CompanyId
DELETE FROM HotelGuestRegistration WHERE CheckInDate >= @TransactionDate --Registration and Guest Mapping
DELETE FROM HotelGuestBillPayment WHERE PaymentDate >= @TransactionDate  -- All Type of Payment(Module Wise) without Room Reseervation Advance Payment

DELETE FROM HotelGuestBillApproved WHERE CreatedDate >= @TransactionDate --Room Charge
DELETE FROM HotelGuestServiceBill WHERE ServiceDate >= @TransactionDate -- Service Bill
DELETE FROM HotelGuestHouseCheckOut WHERE CheckOutDate >= @TransactionDate -- Room Check Out
DELETE FROM HotelGuestDayLetCheckOut WHERE CreatedDate >= @TransactionDate -- If any Discount when check out
DELETE FROM HotelSalesSummary WHERE SummaryDate >= @TransactionDate -- All Module Summary Posted for Report when Day Close
DELETE FROM HotelCompanyPaymentLedger WHERE CreatedDate >= @TransactionDate

-----------Setup Data--------for Registration
--DELETE FROM HotelRoomNumber
--DELETE FROM HotelRoomType
--DELETE FROM CommonCurrency
--DELETE FROM CommonCustomFieldData
--DELETE FROM HotelGuestReference
--DELETE FROM HotelAirlineInformation
--DELETE FROM HotelGuestServiceInfo
--DELETE FROM HotelRoomStatus
--DELETE FROM HotelHKRoomStatus
--DELETE FROM HotelGuestServiceInfo
--DELETE FROM HotelGuestCompany



-- -- Restaurant
DELETE FROM RestaurantBillClassificationDiscount WHERE BillId IN (select BillId from RestaurantBill WHERE CreatedDate >= @TransactionDate) -- KOT and Bill Mapping
DELETE FROM RestaurantBillDetail WHERE BillId IN (select BillId from RestaurantBill WHERE CreatedDate >= @TransactionDate) -- KOT and Bill Mapping
DELETE FROM RestaurantBill WHERE CreatedDate >= @TransactionDate --Restaurant Bill when Bill Preview, Bill Generate and Settlement
DELETE FROM RestaurantKotBillDetail WHERE KotId IN (select KotId from RestaurantKotBillMaster WHERE CreatedDate >= @TransactionDate) --Restaurant Item Details
 -- Food/Beverage wise Discount
DELETE FROM RestaurantKotBillMaster WHERE CreatedDate >= @TransactionDate -- Room/Table/Take Away wise KOT
DELETE FROM RestaurantKotSpecialRemarksDetail WHERE CreatedDate >= @TransactionDate -- Special Instruction for Item wise
DELETE FROM HotelGuestExtraServiceBillApproved WHERE CreatedDate >= @TransactionDate --if Restaurant Bill Settlement in Room (for Other(Cash/Card/Company/Employee) payment posted in HotelGuestBillPayment table)

--DELETE FROM RestaurantTable
--DELETE FROM RestaurantTableManagement
--DELETE FROM RestaurantCostCenterTableMapping
--DELETE FROM InvCategory
--DELETE FROM InvCategoryCostCenterMapping
--DELETE FROM InvCogsAccountVsItemCategoryMappping
--DELETE FROM InvInventoryAccountVsItemCategoryMappping
--DELETE FROM InvItem
--DELETE FROM InvItemCostCenterMapping
--DELETE FROM InvItemClassification
--DELETE FROM InvItemClassificationCostCenterMapping

-- -- Banquet
DELETE FROM BanquetReservationDetail WHERE ReservationId IN (select ID from BanquetReservation WHERE CreatedDate >= @TransactionDate)
DELETE FROM BanquetReservationClassificationDiscount WHERE ReservationId IN (select ID from BanquetReservation WHERE CreatedDate >= @TransactionDate)
DELETE FROM BanquetReservation WHERE CreatedDate >= @TransactionDate


--DELETE FROM BanquetInformation
--DELETE FROM BanquetOccessionType
--DELETE FROM BanquetRequisites
--DELETE FROM BanquetSeatingPlan

DELETE FROM SecurityActivityLogs WHERE CreatedByDate >= @TransactionDate
DELETE FROM HotelRoomLogFile WHERE FromDate >= @TransactionDate

DELETE FROM HotelDayClose WHERE dbo.FnDate(DayCloseDate) >= dbo.FnDate(@TransactionDate)

