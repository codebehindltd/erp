SELECT     LedgerDetailsId, LedgerMasterId, NodeId, DRAmount, CRAmount, BankAccountId, ChequeNumber, ChequeDate, LedgerMode, NodeNarration, CostCenterId, CurrencyAmount, NodeType, ParentId, 
                      ParentLedgerId
FROM         GLLedgerDetails
WHERE     (LedgerMasterId IN
                          (SELECT     LedgerMasterId
                            FROM          GLLedgerMaster
                            WHERE      (VoucherNo = 'JV0000001266')))