OFF INCREMENTAL FOR GLLedgerMaster
OFF INCREMENTAL FOR GLLedgerDetails

-- Data Convertion For Ledger Master
INSERT INTO GLLedgerMaster
  (
    LedgerMasterId,
    CompanyId,
    ProjectId,
    DonorId,
    VoucherType,
    IsBankExist,
    VoucherNo,
    VoucherDate,
    CurrencyId,
    ConvertionRate,
    Narration,
    PayerOrPayee,
    GLStatus,
    CheckedBy,
    ApprovedBy,
    CreatedBy,
    CreatedDate,
    LastModifiedBy,
    LastModifiedDate
  )
SELECT DealId,
       1 CompanyId,
       ProjectId,
       NULL DonorId,
       LEFT(VoucherNo, 2) VoucherType,
       BankExistOrNot IsBankExist,
       VoucherNo,
       VoucherDate,
       1 CurrencyId,
       0 ConvertionRate,
       Narration,
       PayerOrPayee,
       GLStatus,
       CheckedBy,
       ApprovedBy,
       CreatedBy,
       CreatedDate,
       LastModifiedBy,
       LastModifiedDate
FROM   GLDealMaster
-- Data Convertion For Ledger Master

-- Data Convertion For Ledger Details
INSERT INTO GLLedgerDetails
  (
    LedgerDetailsId,
    LedgerMasterId,
    NodeId,
    BankAccountId,
    ChequeNumber,
    ChequeDate,
    DRAmount,
    CRAmount,
    NodeNarration,
    CostCenterId,
    CurrencyAmount,
    NodeType,
    ParentId,
    ParentLedgerId
  )
SELECT LedgerId LedgerDetailsId,
       DealId LedgerMasterId,
       NodeId,
       BankAccountId,
       ChequeNumber,
       NULL ChequeDate,
       CASE 
            WHEN LedgerMode = 1 THEN LedgerAmount
            ELSE 0.00
       END DRAmount,
       CASE 
            WHEN LedgerMode = 2 THEN LedgerAmount
            ELSE 0.00
       END CRAmount,
       NodeNarration,
       CostCenterId,
       CurrencyAmount,
       NodeType,
       ParentId,
       ParentLedgerId
FROM   GLLedger
-- Data Convertion For Ledger Master


UPDATE dbo.GLLedgerDetails
SET    LedgerMode = CASE 
                         WHEN DRAmount > 0 THEN 1
                         ELSE 2
                    END