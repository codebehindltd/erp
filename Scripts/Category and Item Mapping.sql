INSERT INTO RestaurantKitchen
SELECT --KitchenId
       9,
       KitchenName,
       ActiveStat,
       CreatedBy,
       CreatedDate,
       LastModifiedBy,
       LastModifiedDate
FROM   RestaurantKitchen rk
WHERE  rk.CostCenterId = 4



INSERT INTO InvCategoryCostCenterMapping
SELECT DISTINCT
       9,
       CategoryId
FROM   InvCategoryCostCenterMapping icccm




INSERT INTO InvItemCostCenterMapping
SELECT DISTINCT 
       --MappingId,
       9,
       ItemId,
       2,
       MinimumStockLevel,
       SellingLocalCurrencyId,
       UnitPriceLocal,
       SellingUsdCurrencyId,
       UnitPriceUsd,
       DiscountType,
       DiscountAmount,
       StockQuantity,
       AdjustmentLastDate,
       AverageCostDelete
FROM   InvItemCostCenterMapping
WHERE  CostCenterId = 4

INSERT INTO InvItemCostCenterMapping
SELECT DISTINCT 
       --MappingId,
       9,
       ItemId,
       2,
       MinimumStockLevel,
       SellingLocalCurrencyId,
       UnitPriceLocal,
       SellingUsdCurrencyId,
       UnitPriceUsd,
       DiscountType,
       DiscountAmount,
       StockQuantity,
       AdjustmentLastDate,
       AverageCostDelete
FROM   InvItemCostCenterMapping
WHERE  ItemId NOT IN (SELECT ItemId
                      FROM   InvItemCostCenterMapping
                      WHERE  CostCenterId = 4)