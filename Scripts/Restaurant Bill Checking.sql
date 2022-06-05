DECLARE @BillId int 
DECLARE @KotId int
select @BillId = BillId from RestaurantBill where BillNumber = 'PB00001131'
select @KotId = KotId from RestaurantBillDetail where BillId = @BillId

select * from RestaurantBill where BillId = @BillId
select * from RestaurantBillDetail where BillId = @BillId
select * from RestaurantKotBillMaster where KotId = @KotId
select * from RestaurantKotBillDetail where KotId = @KotId
select * from HotelGuestExtraServiceBillApproved where ServiceBillId = @BillId
select * from HotelGuestBillPayment where ServiceBillId = @BillId and ModuleName = 'Restaurant'