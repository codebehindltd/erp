INSERT INTO HotelCompanyOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

INSERT INTO PMSupplierOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

INSERT INTO PayrollEmployeeOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

INSERT INTO LCCNFOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

INSERT INTO MemberOpeningBalance(CompanyId, ProjectId, FiscalYearId, OpeningDate, IsApproved, CreatedBy, CreatedDate)
SELECT TOP 1 CompanyId, ProjectId, FiscalYearId, OpeningBalanceDate, IsApproved, CreatedBy, CreatedDate FROM GLOpeningBalance

SELECT * FROM GLOpeningBalance
SELECT * FROM HotelCompanyOpeningBalance
SELECT * FROM PMSupplierOpeningBalance
SELECT * FROM PayrollEmployeeOpeningBalance
SELECT * FROM LCCNFOpeningBalance
SELECT * FROM MemberOpeningBalance