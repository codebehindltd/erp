INSERT INTO GLNodeMatrix
(
	-- NodeId -- this column value is auto-generated,
	AncestorId,
	NodeNumber,
	NodeHead,
	Lvl,
	Hierarchy,
	HierarchyIndex,
	NodeMode,
	CreatedBy,
	CreatedDate,
	NodeType
)
(SELECT
	1,
	F1,
	LTRIM(RTRIM(F2)),
	0,
	'',
	'',
	0,
	1,
	GETDATE(),
	F3
FROM dbo.rptChartOfAccounts$
WHERE f1 IS NOT NULL)
--SELECT * FROM GLNodeMatrix ORDER BY AncestorId ASC

--UPDATE GLNodeMatrix
--SET NodeNumber = REPLACE(NodeNumber, 'M', '')


--- Ancestor Id Generate
--UPDATE nmt
--SET nmt.AncestorId = nod.AncestorNodeId
--FROM GLNodeMatrix nmt
--INNER JOIN 
--(
--    SELECT nm.NodeId,
--           nm.NodeNumber,
--           LEFT(nm.NodeNumber, LEN(nm.NodeNumber) - 2) AS AncestorNodeNumber,
--           AncestorNodeId = (
--               SELECT nmf.NodeId
--               FROM   GLNodeMatrix nmf
--               WHERE  nmf.NodeNumber = LEFT(nm.NodeNumber, LEN(nm.NodeNumber) - 2)
--           )
--    FROM   GLNodeMatrix nm
--    WHERE  LEN(NodeNumber) > 1
--)nod ON nmt.NodeId = nod.NodeId


DECLARE @NodeId INT = 1

;WITH CteCOA (NodeId, AncestorId, NodeNumber, NodeHead, NodeType, LevelNo, CHierarchy, CHierarchyIndex)
AS
(
	Select NodeId, AncestorId, NodeNumber, NodeHead, NodeType, 0 LevelNo, 
	CONVERT(VARCHAR(1000), NodeId) CHierarchy,
	CONVERT(VARCHAR(1000), dbo.FnHieIndex(NodeId)) CHierarchyIndex 
	From GLNodeMatrix 
	Where NodeId = @NodeId

	UNION ALL

	SELECT B.NodeId, B.AncestorId, B.NodeNumber, B.NodeHead, B.NodeType, A.LevelNo + 1, 
	
		 (CONVERT(VARCHAR(1000),(A.CHierarchy + '.' + CONVERT(VARCHAR(50),B.NodeId)))),
		   
		 (CONVERT(VARCHAR(1000),  CHierarchyIndex + '.' + dbo.FnHieIndex(B.NodeId)))
		  
	FROM GLNodeMatrix B
	INNER JOIN CteCOA A ON A.NodeId = B.AncestorId
	Where B.NodeId != @NodeId
)
  --SELECT * 
  --FROM CteCOA
    
  UPDATE nm 
  SET nm.Hierarchy = '.'+ ct.CHierarchy + '.', 
  nm.HierarchyIndex =  '.' + ct.CHierarchyIndex + '.',
  nm.Lvl = ct.LevelNo
  FROM GLNodeMatrix nm 
  INNER JOIN CteCOA ct ON nm.NodeId = ct.NodeId
 
 
--UPDATE GLNodeMatrix
--SET    IsTransactionalHead = NULL

--UPDATE nmm
--SET    nmm.IsTransactionalHead = 1
--FROM   GLNodeMatrix nmm
--WHERE  nmm.NodeId NOT IN (SELECT nm.AncestorId
--                          FROM   GLNodeMatrix nm
--                          WHERE  nm.AncestorId IS NOT NULL)
                          
--UPDATE GLNodeMatrix
--SET    NodeType = CASE 
--                       WHEN CHARINDEX('.1.', Hierarchy) = 1 THEN 'Assets'
--                       ELSE CASE 
--                                 WHEN CHARINDEX('.2.', Hierarchy) = 1 THEN 
--                                      'Liabilities'
--                                 ELSE CASE 
--                                           WHEN CHARINDEX('.3.', Hierarchy) = 1 THEN 
--                                                'Income'
--                                           ELSE CASE 
--                                                     WHEN CHARINDEX('.4.', Hierarchy) 
--                                                          = 1 THEN 'Expenditure'
--                                                END
--                                      END
--                            END
--                  END


--/****** Object:  UserDefinedFunction [dbo].[fnGetHierarchy]    Script Date: 11/10/2016 18:04:47 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--ALTER FUNCTION [dbo].[fnGetHierarchy]  
--(
-- @Id bigint
--)
--RETURNS nvarchar(500)
--AS

--BEGIN

-- DECLARE @parent bigint
-- DECLARE @path nvarchar(1000)
-- DECLARE @result nvarchar(4000) 
-- SET @result=''

-- WHILE @Id  IS NOT NULL 
-- BEGIN
--  SELECT @path=CAST( Id AS nvarchar), 
--        @parent = parentid 
--    FROM  MSTChartAccount 
--   WHERE id=@Id

--  SET @result = '.'+@path + @result
--  --SET @result = @path+ '.' + @result
--  SET @Id = @parent
-- END

-- --Remove the leading slash
-- IF (SUBSTRING(@result,1,1) = '.')
-- BEGIN
--  SET @result = SUBSTRING(@result,2,LEN(@result))
-- END 

-- IF(LEN(@result)=0)
-- BEGIN
--  SET @result= NULL
-- END
 
-- RETURN( @result)
--END


--/****** Object:  UserDefinedFunction [dbo].[fnGetHierarchyIndex]    Script Date: 11/10/2016 18:04:53 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--ALTER FUNCTION [dbo].[fnGetHierarchyIndex]  
--(
-- @Id bigint
--)
--RETURNS nvarchar(500)
--AS

--BEGIN

-- DECLARE @parent bigint
-- DECLARE @path nvarchar(1000)
-- DECLARE @result nvarchar(4000) 
-- SET @result=''

-- WHILE @Id  IS NOT NULL 
-- BEGIN
--  SELECT @path=CAST( Id AS nvarchar), 
--        @parent = parentid 
--    FROM  MSTChartAccount 
--   WHERE id=@Id

--  SET @result = '.'+ RIGHT('0000'+ISNULL(@path,''),4) + @result
--  --SET @result = @path+ '.' + @result
--  SET @Id = @parent
-- END

-- --Remove the leading slash
-- IF (SUBSTRING(@result,1,1) = '.')
-- BEGIN
--  SET @result = SUBSTRING(@result,2,LEN(@result))
-- END 

-- IF(LEN(@result)=0)
-- BEGIN
--  SET @result= NULL
-- END
 
-- RETURN( @result)
--END

--/****** Object:  UserDefinedFunction [dbo].[fnGetLevel]    Script Date: 11/10/2016 18:05:45 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--ALTER FUNCTION [dbo].[fnGetLevel]  
--(
-- @Id bigint
--)
--RETURNS INT
--AS

--BEGIN

-- DECLARE @result bigint
-- set @result = (SELECT    LEN(Hierarchy) - LEN(REPLACE(Hierarchy, '.', ''))
--  FROM          dbo.MSTChartAccount AS t WHERE Id= @Id)
 
-- RETURN( @result)
--END

