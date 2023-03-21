DECLARE @PageId VARCHAR(500)
SET @PageId = 'frmSalesNMarketingSchedule'
DECLARE @MenuLinksId INT
--SELECT * FROM SecurityMenuLinks WHERE PageId = @PageId

SELECT @MenuLinksId = MenuLinksId FROM SecurityMenuLinks WHERE PageId = @PageId
DELETE FROM SecurityMenuWiseLinks WHERE MenuLinksId = @MenuLinksId --AND UserGroupId <> 1
UPDATE SecurityMenuLinks SET ActiveStat = 0 WHERE PageId = @PageId
