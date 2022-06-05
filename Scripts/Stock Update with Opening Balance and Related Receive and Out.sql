--select * from CommonCostcenter where CostCenterId = 5

--select * from InvLocation

--select * from BarOpening
--select * from OpeningStock

--SELECT * FROM PMProductReceivedDetails
--SELECT * FROM PMProductOutDetails

--select * from InvItemStockVarianceDetails

--SELECT * FROM InvRecipeDeductionDetails

DECLARE @CurrentStockBar TABLE
(
	ItemId					INT,
	ItemName				VARCHAR(250),
	OpeningQuantity			DECIMAL(18,5),
	ReceiveQuantity			DECIMAL(18,5),
	OutQuantity				DECIMAL(18,5),
	WastageQuantity			DECIMAL(18,5),
	UsageQuantity			DECIMAL(18,5),
	ClosingQuantity			DECIMAL(18,5)
)

DECLARE @CurrentStockMain TABLE
(
	ItemId					INT,
	ItemName				VARCHAR(250),
	OpeningQuantity			DECIMAL(18,5),
	ReceiveQuantity			DECIMAL(18,5),
	OutQuantity				DECIMAL(18,5),
	WastageQuantity			DECIMAL(18,5),
	UsageQuantity			DECIMAL(18,5),
	ClosingQuantity			DECIMAL(18,5)
)

------ Bar Stock

INSERT INTO @CurrentStockBar
SELECT itm.ItemId, itm.Name, bop.OpeningQuantity, 0, 0, 0, 0, 0
FROM BarOpening bop INNER JOIN InvItem itm ON bop.ItemName like itm.Name

--SELECT * FROM @CurrentStockBar

UPDATE csb
SET csb.ReceiveQuantity = rcv.ReceivedQuantity
FROM @CurrentStockBar csb INNER JOIN
(
	SELECT stk.ItemId, SUM(pr.Quantity) ReceivedQuantity 
	FROM PMProductReceivedDetails pr 
	INNER JOIN 
	@CurrentStockBar stk ON pr.ProductId = stk.ItemId
	WHERE pr.LocationId = 3 AND CostCenterId = 5
	GROUP BY stk.ItemId
)rcv ON csb.ItemId = rcv.ItemId

UPDATE csb
SET csb.OutQuantity = ISNULL(rcv.OutQuantity, 0)
FROM @CurrentStockBar csb INNER JOIN
(
	SELECT stk.ItemId, SUM(pr.Quantity) OutQuantity 
	FROM PMProductOutDetails pr 
	INNER JOIN 
	@CurrentStockBar stk ON pr.ProductId = stk.ItemId
	WHERE pr.LocationIdFrom = 3 AND CostCenterIdFrom = 5
	GROUP BY stk.ItemId
)rcv ON csb.ItemId = rcv.ItemId


UPDATE csb
SET csb.WastageQuantity = ISNULL(rcv.VarianceQuantity, 0)
FROM @CurrentStockBar csb INNER JOIN
(
	SELECT stk.ItemId, SUM(pr.VarianceQuantity) VarianceQuantity 
	FROM InvItemStockVarianceDetails pr 
	INNER JOIN 
	@CurrentStockBar stk ON pr.ItemId = stk.ItemId
	WHERE pr.LocationId = 3
	GROUP BY stk.ItemId

)rcv ON csb.ItemId = rcv.ItemId


DECLARE @UsageItem TABLE(
		ItemId INT,
		UsageQuantity DECIMAL(18, 5)
	)

IF OBJECT_ID('tempdb.dbo.##UsageItems') IS NOT NULL
BEGIN
	DROP TABLE ##UsageItems
END

SELECT dd.* 
INTO ##UsageItems
FROM InvRecipeDeductionDetails dd INNER JOIN @CurrentStockBar bs ON ISNULL(dd.ItemIdMain,0) = bs.ItemId
WHERE LocationId = 3 AND
 dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate('2018-1-1')) AND dbo.FnDate(dbo.FnDate(GETDATE()))

INSERT INTO @UsageItem	
SELECT ItemId, SUM(ItemUnit) ItemUnit
FROM
(
	SELECT * FROM
	(
		SELECT RecipeItemId ItemId, SUM(TotalUnitWillDeduct) ItemUnit, LocationId
		FROM ##UsageItems ius
		WHERE  RecipeItemId IS NOT NULL AND
			   (TotalUnitWillDeduct > 0 AND RecipeDeduction = 0) AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))
		GROUP BY RecipeItemId, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(TotalUnitWillDeduct)ItemUnit, LocationId
		FROM ##UsageItems ius
		WHERE  ItemIdMain IS NOT NULL  AND ius.ItemId IS NULL AND ius.IsRecipe = 0	 AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	   	
		GROUP BY ItemIdMain, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(UnitDeduction)ItemUnit, LocationId
		FROM ##UsageItems ius
		WHERE  ItemIdMain IS NOT NULL  AND ius.ItemId IS NULL AND ius.IsRecipe = 1	 AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	   	
		GROUP BY ItemIdMain, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(TotalUnitWillDeduct)ItemUnit, LocationId
		FROM ##UsageItems ius
		WHERE  ItemIdMain IS NOT NULL  AND ius.ItemId IS NULL AND ius.RecipeItemId IS NULL AND ius.IsRecipe = 1	 AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	   	
		GROUP BY ItemIdMain, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(UnitDeduction)ItemUnit, LocationId
		FROM ##UsageItems ius
		WHERE  ItemIdMain IS NOT NULL AND
			   TotalUnitWillDeduct = RecipeDeduction  AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	
		GROUP BY ItemIdMain, LocationId
	)usg
)usage
GROUP BY ItemId

UPDATE csb
SET csb.UsageQuantity = ISNULL(rcv.UsageQuantity, 0)
FROM @CurrentStockBar csb INNER JOIN
@UsageItem rcv ON csb.ItemId = rcv.ItemId

UPDATE @CurrentStockBar
SET ClosingQuantity = OpeningQuantity + ReceiveQuantity - OutQuantity - WastageQuantity - UsageQuantity

--SELECT * 
--FROM @CurrentStockBar

--------------------Bar Stock Ended-----------------------

--- Main Stock
INSERT INTO @CurrentStockMain
SELECT itm.ItemId, itm.Name, bop.OpeningQuantity, 0, 0, 0, 0, 0 
FROM OpeningStock bop INNER JOIN InvItem itm ON bop.ItemName like itm.Name

UPDATE csb
SET csb.ReceiveQuantity = rcv.ReceivedQuantity
FROM @CurrentStockMain csb INNER JOIN
(
	SELECT stk.ItemId, SUM(pr.Quantity) ReceivedQuantity 
	FROM PMProductReceivedDetails pr 
	INNER JOIN 
	@CurrentStockMain stk ON pr.ProductId = stk.ItemId
	WHERE pr.LocationId = 1 AND CostCenterId = 5
	GROUP BY stk.ItemId
)rcv ON csb.ItemId = rcv.ItemId

UPDATE csb
SET csb.OutQuantity = ISNULL(rcv.OutQuantity, 0)
FROM @CurrentStockMain csb INNER JOIN
(
	SELECT stk.ItemId, SUM(pr.Quantity) OutQuantity 
	FROM PMProductOutDetails pr 
	INNER JOIN 
	@CurrentStockMain stk ON pr.ProductId = stk.ItemId
	WHERE pr.LocationIdFrom = 1 AND CostCenterIdFrom = 5
	GROUP BY stk.ItemId
)rcv ON csb.ItemId = rcv.ItemId


UPDATE csb
SET csb.WastageQuantity = ISNULL(rcv.VarianceQuantity, 0)
FROM @CurrentStockMain csb INNER JOIN
(
	SELECT stk.ItemId, SUM(pr.VarianceQuantity) VarianceQuantity 
	FROM InvItemStockVarianceDetails pr 
	INNER JOIN 
	@CurrentStockMain stk ON pr.ItemId = stk.ItemId
	WHERE pr.LocationId = 1
	GROUP BY stk.ItemId

)rcv ON csb.ItemId = rcv.ItemId


DECLARE @UsageItemMain TABLE(
		ItemId INT,
		UsageQuantity DECIMAL(18, 5)
	)

IF OBJECT_ID('tempdb.dbo.##UsageItemsMain') IS NOT NULL
BEGIN
	DROP TABLE ##UsageItemsMain
END

SELECT dd.* 
INTO ##UsageItemsMain
FROM InvRecipeDeductionDetails dd INNER JOIN @CurrentStockMain bs ON ISNULL(dd.ItemIdMain,0) = bs.ItemId
WHERE LocationId = 3 AND
 dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate('2018-1-1')) AND dbo.FnDate(dbo.FnDate(GETDATE()))

INSERT INTO @UsageItemMain	
SELECT ItemId, SUM(ItemUnit) ItemUnit
FROM
(
	SELECT * FROM
	(
		SELECT RecipeItemId ItemId, SUM(TotalUnitWillDeduct) ItemUnit, LocationId
		FROM ##UsageItemsMain ius
		WHERE  RecipeItemId IS NOT NULL AND
			   (TotalUnitWillDeduct > 0 AND RecipeDeduction = 0) AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))
		GROUP BY RecipeItemId, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(TotalUnitWillDeduct)ItemUnit, LocationId
		FROM ##UsageItemsMain ius
		WHERE  ItemIdMain IS NOT NULL  AND ius.ItemId IS NULL AND ius.IsRecipe = 0	 AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	   	
		GROUP BY ItemIdMain, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(UnitDeduction)ItemUnit, LocationId
		FROM ##UsageItemsMain ius
		WHERE  ItemIdMain IS NOT NULL  AND ius.ItemId IS NULL AND ius.IsRecipe = 1	 AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	   	
		GROUP BY ItemIdMain, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(TotalUnitWillDeduct)ItemUnit, LocationId
		FROM ##UsageItemsMain ius
		WHERE  ItemIdMain IS NOT NULL  AND ius.ItemId IS NULL AND ius.RecipeItemId IS NULL AND ius.IsRecipe = 1	 AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	   	
		GROUP BY ItemIdMain, LocationId
		
		UNION
		
		SELECT ItemIdMain ItemId, SUM(UnitDeduction)ItemUnit, LocationId
		FROM ##UsageItemsMain ius
		WHERE  ItemIdMain IS NOT NULL AND
			   TotalUnitWillDeduct = RecipeDeduction  AND
			   dbo.FnDate(DeductionDate) BETWEEN dbo.FnDate(dbo.FnDate(GETDATE())) AND dbo.FnDate(dbo.FnDate(GETDATE()))	
		GROUP BY ItemIdMain, LocationId
	)usg
)usage
GROUP BY ItemId

UPDATE csb
SET csb.UsageQuantity = ISNULL(rcv.UsageQuantity, 0)
FROM @CurrentStockMain csb INNER JOIN
@UsageItemMain rcv ON csb.ItemId = rcv.ItemId

UPDATE @CurrentStockMain
SET ClosingQuantity = OpeningQuantity + ReceiveQuantity - OutQuantity - WastageQuantity - UsageQuantity

--SELECT * FROM @CurrentStockMain

-----------Ended Stock Main----------------------

--SELECT * FROM @CurrentStockBar
--SELECT * FROM @CurrentStockMain

-----------Stock Quantity Updation----------------------

UPDATE stk
SET stk.StockQuantity = bar.ClosingQuantity
FROM InvItemStockInformation stk INNER JOIN @CurrentStockBar bar ON stk.ItemId = bar.ItemId

UPDATE stk
SET stk.StockQuantity = bar.ClosingQuantity
FROM InvItemStockInformation stk INNER JOIN @CurrentStockMain bar ON stk.ItemId = bar.ItemId


--UPDATE stk
--SET stk.StockQuantity = 0
--FROM InvItemStockInformation stk 
--WHERE StockQuantity < 0


