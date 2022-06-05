----select ProductId, (PurchasePrice/Quantity) AverageCost
----from
----(
----	select ProductId, SUM(Quantity)Quantity, SUM(Quantity * PurchasePrice)PurchasePrice
----	from PMProductReceivedDetails
----	GROUP BY ProductId
----)rcv

----update itm
----set itm.AverageCost = avgc.AverageCost
----from InvItem itm
----inner join
----(
----	select ProductId, (PurchasePrice/Quantity) AverageCost
----	from
----	(
----		select ProductId, SUM(Quantity)Quantity, SUM(Quantity * PurchasePrice)PurchasePrice
----		from PMProductReceivedDetails
----		GROUP BY ProductId
----	)rcv
----)avgc on avgc.ProductId = itm.ItemId


--0-- Update Average Cost By Purchase Price
UPDATE InvItem
SET    AverageCost = PurchasePrice

--1-- Update Recipe Item Cost From Item Table using Latest Average Cost

---- Show
SELECT rcd.ItemId,
       rcd.RecipeItemId,
       rcd.RecipeItemName,
       rcd.ItemUnit,
       rcd.UnitHeadId,
       rcd.ItemCost,
       [dbo].[FnGetInvItemConvertedStockQuantity]
       (
           0,
           0,
           rcd.RecipeItemId,
           rcd.UnitHeadId,
           rcd.ItemUnit,
           'ItemCost'
       ) CurrentRate
FROM   InvItem rd
       INNER JOIN RestaurantRecipeDetail rcd
            ON  rd.ItemId = rcd.RecipeItemId  

----Update
UPDATE rcpitm
SET    ItemCost = recpcrntcst.CurrentRate
FROM   RestaurantRecipeDetail rcpitm
       INNER JOIN (
                SELECT rcd.ItemId,
                       rcd.RecipeItemId,
                       rcd.ItemUnit,
                       rcd.UnitHeadId,
                       [dbo].[FnGetInvItemConvertedStockQuantity]
                       (
                           0,
                           0,
                           rcd.RecipeItemId,
                           rcd.UnitHeadId,
                           rcd.ItemUnit,
                           'ItemCost'
                       ) CurrentRate
                FROM   InvItem rd
                       INNER JOIN RestaurantRecipeDetail rcd
                            ON  rd.ItemId = rcd.RecipeItemId
            )recpcrntcst
            ON  rcpitm.ItemId = recpcrntcst.ItemId
            AND rcpitm.RecipeItemId = recpcrntcst.RecipeItemId
 
 
--2-- Update Recipe Item Avearge Cost OF Item Table From RestaurantRecipeDetail Table using Latest Recipe Cost

--Show
SELECT ri.ItemId,
       i.Name,
       ri.ItemCost
FROM   (
           SELECT ItemId,
                  SUM(ItemCost) ItemCost
           FROM   RestaurantRecipeDetail
           GROUP BY
                  ItemId
       )ri
       INNER JOIN InvItem i
            ON  ri.ItemId = i.ItemId
ORDER BY
       ri.ItemId
 
 --Update
UPDATE i
SET    i.AverageCost = ri.ItemCost
FROM   (
           SELECT ItemId,
                  SUM(ItemCost) ItemCost
           FROM   RestaurantRecipeDetail
           GROUP BY
                  ItemId
       )ri
       INNER JOIN InvItem i
            ON  ri.ItemId = i.ItemId

--3-- Delete Recipe Item From RestaurantRecipeDetail Table Which is not In Item Table
SELECT *
FROM   RestaurantRecipeDetail
WHERE  ItemId NOT IN (SELECT ItemId
                      FROM   InvItem)

DELETE 
FROM   RestaurantRecipeDetail
WHERE  ItemId NOT IN (SELECT ItemId
                      FROM   InvItem)


UPDATE rd
SET    rd.AverageCost = i.AverageCost,
       rd.TotalCost = (i.AverageCost * rd.DeductionQuantity)
FROM   InvRecipeDeductionDetails rd
       INNER JOIN InvItem i
            ON  rd.RecipeItemId = i.ItemId