ALTER TABLE InvItem
  ALTER COLUMN AverageCost DECIMAL(18,5) NOT NULL;

insert into InvItemStockInformation
select  prd.LocationId, prd.ProductId, 
	    SUM(prd.Quantity)Quantity
from PMProductReceived pr inner join 
PMProductReceivedDetails prd on pr.ReceivedId = prd.ReceivedId
where pr.[Status] = 'Approved'
GROUP BY  prd.LocationId, prd.ProductId

update itm
set itm.AverageCost = avc.AverageCost
from InvItem itm
inner join
(
	select  prd.ProductId, 
			SUM(prd.Quantity)Quantity, SUM((prd.PurchasePrice * prd.Quantity))/SUM(Quantity)AverageCost
	from PMProductReceived pr inner join 
	PMProductReceivedDetails prd on pr.ReceivedId = prd.ReceivedId
	where pr.[Status] = 'Approved'
	GROUP BY  prd.ProductId
)avc on itm.ItemId = avc.ProductId