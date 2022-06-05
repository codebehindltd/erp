UPDATE GLNodeMatrix
SET    IsTransactionalHead = NULL

UPDATE nmm
SET    nmm.IsTransactionalHead = 1
FROM   GLNodeMatrix nmm
WHERE  nmm.NodeId NOT IN (SELECT nm.AncestorId
                          FROM   GLNodeMatrix nm
                          WHERE  nm.AncestorId IS NOT NULL)
                          
UPDATE GLNodeMatrix
SET    NodeType = CASE 
                       WHEN CHARINDEX('.1.', Hierarchy) = 1 THEN 'Assets'
                       ELSE CASE 
                                 WHEN CHARINDEX('.2.', Hierarchy) = 1 THEN 
                                      'Liabilities'
                                 ELSE CASE 
                                           WHEN CHARINDEX('.3.', Hierarchy) = 1 THEN 
                                                'Income'
                                           ELSE CASE 
                                                     WHEN CHARINDEX('.4.', Hierarchy) 
                                                          = 1 THEN 'Expenditure'
                                                END
                                      END
                            END
                  END