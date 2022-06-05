/*
select EmpCode --,max(receiveddate) as MaxDate
FROM PayrollEmployee pe
group by EmpCode 
having count(EmpCode) > 1

SELECT * FROM PayrollEmployee pe WHERE pe.EmpCode = 1196
SELECT * FROM PayrollEmployee pe WHERE pe.EmpCode = 4310
SELECT * FROM PayrollEmployee pe WHERE pe.EmpCode = 8076

DELETE FROM PayrollEmployee WHERE EmpId IN (119,213,83,91)
*/
DECLARE @HeadAncestorId INT
SET @HeadAncestorId = 801

DECLARE @Lvl INT
SELECT @Lvl = Lvl + 1
FROM   GLNodeMatrix gm
WHERE  gm.NodeId = @HeadAncestorId

DECLARE @ProductID     INT
DECLARE @getProductID  CURSOR 
SET @getProductID =  CURSOR FOR
SELECT pe.EmpId
FROM   PayrollEmployee pe

OPEN @getProductID
FETCH NEXT
FROM @getProductID INTO @ProductID
WHILE @@FETCH_STATUS = 0
BEGIN
    --PRINT @ProductID
    DECLARE @NodeNumber  VARCHAR(50)
    DECLARE @NodeHead    VARCHAR(300)
    SELECT @NodeNumber = 'EMP' + EmpCode,
           @NodeHead = pe.DisplayName
    FROM   PayrollEmployee pe
    WHERE  pe.EmpId = @ProductID
    
    DECLARE @return_value  INT,
            @NodeId        INT,
            @Err           INT
    
    EXEC @return_value = [dbo].[SaveNodeMatrixInfo_SP]
         @AncestorId = @HeadAncestorId,
         @NodeNumber = @NodeNumber,
         @NodeHead = @NodeHead,
         @NodeMode = 1,
         @CFHeadId = 0,
         @PLHeadId = 0,
         @CreatedBy = 1,
         @NodeId = @NodeId OUTPUT,
         @Err = @Err OUTPUT
    
    FETCH NEXT
    FROM @getProductID INTO @ProductID
END
CLOSE @getProductID
DEALLOCATE @getProductID
