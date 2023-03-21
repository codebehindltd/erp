TRUNCATE TABLE PayrollAppraisalEvalutionBy 
TRUNCATE TABLE PayrollAppraisalEvalutionRatingFactorDetails 
TRUNCATE TABLE PayrollStaffingBudget
TRUNCATE TABLE PayrollStaffingBudgetDetails
TRUNCATE TABLE PayrollStaffRequisition
TRUNCATE TABLE PayrollStaffRequisitionDetails
TRUNCATE TABLE PayrollEmpTraining
TRUNCATE TABLE PayrollEmpTrainingDetail
TRUNCATE TABLE PayrollApplicantResult
TRUNCATE TABLE PayrollDisciplinaryAction
TRUNCATE TABLE PayrollEmpSalaryProcessTemp
TRUNCATE TABLE PayrollEmpSalaryProcessDetailTemp
TRUNCATE TABLE PayrollEmpSalaryProcess
TRUNCATE TABLE PayrollEmpSalaryProcessDetail
TRUNCATE TABLE CashRequisition
TRUNCATE TABLE CashRequisitionDetails

TRUNCATE TABLE PayrollEmpLoan
TRUNCATE TABLE PayrollEmpAdvanceTaken
TRUNCATE TABLE PayrollLoanCollection
TRUNCATE TABLE PayrollLoanHoldup
TRUNCATE TABLE PayrollEmployeePaymentLedger

TRUNCATE TABLE PayrollEmpAttendance
TRUNCATE TABLE PayrollEmpAttendanceLogSuprima
TRUNCATE TABLE PayrollEmpLeaveInformation
TRUNCATE TABLE PayrollEmpPF
TRUNCATE TABLE PayrollEmpSalaryProcessDetail
TRUNCATE TABLE PayrollEmpThana
TRUNCATE TABLE PayrollLeaveBalanceClosing
TRUNCATE TABLE PayrollSalaryFormula

--SELECT * FROM PayrollEmployee pe WHERE pe.EmpCode = 'E165'
UPDATE PayrollEmployee
SET	
	IsApplicant = 1
WHERE EmpId = 103
SELECT * FROM PayrollJobCircular pjc
SELECT * FROM PayrollJobCircularApplicantMapping pjcam



--TRUNCATE TABLE PayrollAllowanceDeductionHead
--TRUNCATE TABLE PayrollAppraisalEvalutionBy
--TRUNCATE TABLE PayrollAppraisalEvalutionRatingFactorDetails
--TRUNCATE TABLE PayrollAppraisalMarksIndicator
--TRUNCATE TABLE PayrollAppraisalRatingFactor
--TRUNCATE TABLE PayrollAppraisalRatingScale
--TRUNCATE TABLE PayrollDepartment
--TRUNCATE TABLE PayrollDesignation
--TRUNCATE TABLE PayrollDonor
--TRUNCATE TABLE PayrollEmpAdvanceTaken
--TRUNCATE TABLE PayrollEmpAllowanceDeduction
--TRUNCATE TABLE PayrollEmpAppraisalEvalution
--TRUNCATE TABLE PayrollEmpAttendance
--TRUNCATE TABLE PayrollEmpBankInfo
--TRUNCATE TABLE PayrollEmpType
--TRUNCATE TABLE PayrollEmpDependent
--TRUNCATE TABLE PayrollEmpEducation
--TRUNCATE TABLE PayrollEmpExperience
--TRUNCATE TABLE PayrollEmpGrade
--TRUNCATE TABLE PayrollEmpGratuity
--TRUNCATE TABLE PayrollEmpIncrement
--TRUNCATE TABLE PayrollEmpLeaveInformation
--TRUNCATE TABLE PayrollEmpLoan
--TRUNCATE TABLE PayrollEmployee
--TRUNCATE TABLE PayrollEmpNomineeInfo
--TRUNCATE TABLE PayrollEmpOverTime
--TRUNCATE TABLE PayrollEmpOverTimeSetup
--TRUNCATE TABLE PayrollEmpPayScale
--TRUNCATE TABLE PayrollEmpPF
--TRUNCATE TABLE PayrollEmpRoster
--TRUNCATE TABLE PayrollEmpSalaryProcess
--TRUNCATE TABLE PayrollEmpSalaryProcessDetail
--TRUNCATE TABLE PayrollEmpTax
--TRUNCATE TABLE PayrollEmpTaxDeductionSetting
--TRUNCATE TABLE PayrollEmpTimeSlab
--TRUNCATE TABLE PayrollEmpTimeSlabRoster
--TRUNCATE TABLE PayrollEmpTraining
--TRUNCATE TABLE PayrollEmpTrainingType
--TRUNCATE TABLE PayrollEmpWorkStation
--TRUNCATE TABLE PayrollEmpYearlyLeave
--TRUNCATE TABLE PayrollGratutitySettings
--TRUNCATE TABLE PayrollHoliday
--TRUNCATE TABLE PayrollLeaveType
--TRUNCATE TABLE PayrollLoanCollection
--TRUNCATE TABLE PayrollLoanHoldup
--TRUNCATE TABLE PayrollLoanSetting
--TRUNCATE TABLE PayrollPFSetting
--TRUNCATE TABLE PayrollRosterHead
--TRUNCATE TABLE PayrollSalaryFormula
--TRUNCATE TABLE PayrollSalaryHead
--TRUNCATE TABLE PayrollTaxSetting
--TRUNCATE TABLE PayrollTimeSlabHead
--TRUNCATE TABLE PayrollWorkingDay

--SELECT 'TRUNCATE TABLE ' + TABLE_NAME
--FROM INFORMATION_SCHEMA.TABLES
--WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='Innboard'
--ORDER BY TABLE_NAME