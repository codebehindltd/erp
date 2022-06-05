UPDATE hcs SET  hcs.SETupValue = '0' 
		FROM   CommonSETup hcs
		WHERE  hcs.TypeName = 'PayrollPFEmployeeContributionId'
			   AND hcs.SETupName = 'PayrollPFEmployeeContributionId'
GO			   
UPDATE hcs SET  hcs.SETupValue = '0' 
		FROM   CommonSETup hcs
		WHERE  hcs.TypeName = 'PayrollPFCompanyContributionId'
			   AND hcs.SETupName = 'PayrollPFCompanyContributionId'
GO			   
UPDATE hcs SET  hcs.SETupValue = '0' 
		FROM   CommonSETup hcs
		WHERE  hcs.TypeName = 'PayrollTaxDeductionId'
			   AND hcs.SETupName = 'PayrollTaxDeductionId'
GO
UPDATE hcs SET  hcs.SETupValue = '0' 
		FROM   CommonSETup hcs
		WHERE  hcs.TypeName = 'PayrollAdvanceHeadId'
			   AND hcs.SETupName = 'PayrollAdvanceHeadId'
GO			   
UPDATE hcs SET  hcs.SETupValue = '0' 
		FROM   CommonSETup hcs
		WHERE  hcs.TypeName = 'WithoutSalaryLeaveHeadId'
			   AND hcs.SETupName = 'WithoutSalaryLeaveHeadId'