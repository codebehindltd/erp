SELECT * FROM RestaurantBill WHERE BillNumber = 'RB00002296'
SELECT * FROM RestaurantBillDetail WHERE BillId = 2296
SELECT * FROM RestaurantKotBillDetail WHERE KotId = 2697


SELECT rkbd.* FROM RestaurantBill rb
LEFT JOIN RestaurantBillDetail rbd ON rb.BillId = rbd.BillId
LEFT JOIN RestaurantKotBillMaster rkbm ON rbd.KotId = rkbm.KotId
LEFT JOIN RestaurantKotBillDetail rkbd ON rkbd.KotId = rkbm.KotId
WHERE rb.BillNumber = 'RB00021843'