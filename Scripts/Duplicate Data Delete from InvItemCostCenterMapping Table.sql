delete a from
(
 select *, ROW_NUMBER() OVER (PARTITION BY CostcenterId, Itemid ORDER BY CostcenterId, Itemid)rnk
 from InvItemCostCenterMapping
)a where a.rnk > 1