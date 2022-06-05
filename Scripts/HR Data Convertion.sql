
/*
	truncate table dbo.PayrollDepartment
	truncate table dbo.PayrollDesignation
	truncate table dbo.PayrollEmpGrade
	truncate table dbo.PayrollEmployee
	truncate table dbo.PayrollEmpType
*/

--update EmployeeInformation
--set Designation = 76
--where  Designation = 112

INSERT INTO PayrollEmployee
(EmpCode, Title, FirstName, LastName, DisplayName, EmpTypeId, InitialContractEndDate,
		DepartmentId, JoinDate, GradeId, DesignationId, Gender, Religion, MaritalStatus, Nationality)
SELECT EmployeeCode, Title, FirstName, LastName, FullName, EmployeeType, ContractEndDate,
		Department, Joindate, EmployeeGrade, Designation, Gender, Religion, MaritalStatus, Nationality
from EmployeeInformation
where EmployeeCode is not null
order by EmployeeCode

select EmployeeCode, ROW_NUMBER() OVER(PARTITION BY EmployeeCode ORDER BY EmployeeCode) rnk
from EmployeeInformation
where EmployeeCode is not null



select * from dbo.Designation
select * from dbo.EmployeeInformation

select * from dbo.Department
select * from dbo.PayrollDepartment


select * from dbo.Designation
select * from dbo.PayrollDesignation

select * from PayrollEmployee


--insert into dbo.PayrollDesignation
--(Name, ActiveStat, CreatedBy, CreatedDate)
--	SELECT Designation, 1, 1, GETDATE()
--	from Designation 
--	ORDER BY Serial
	



--insert into dbo.PayrollDepartment
--(Name, ActiveStat, CreatedBy, CreatedDate)
--	SELECT Department, 1, 1, GETDATE()
--	from Department 
--	ORDER BY Serial
	


-------------------------------------------------
--select * from Manager

--delete from Manager where 
--EmployeeCode is null

-- dd-MM-yyyy to  yyyy/MM/dd
--select JoinDate, SUBSTRING(JoinDate,7, 4) + '/' + SUBSTRING(JoinDate,1, 2) + '/' + SUBSTRING(JoinDate,4, 2)
--from Manager

-- UPDATE Manager SET JoinDate = '20' + JoinDate

---- MM-dd-yyyy to  yyyy/MM/dd
--select JoinDate, SUBSTRING(JoinDate,7, 4) + '/' + SUBSTRING(JoinDate,4, 2) + '/' + SUBSTRING(JoinDate,1, 2)
--from Manager

--select JoinDate, '20' + SUBSTRING(JoinDate,1, 2) + '/' + SUBSTRING(JoinDate,7, 4) + '/' + SUBSTRING(JoinDate,4, 2) 
--from Manager

/*
	ALTER TABLE Manager ADD 
	EmpTypeId		INT NULL,
	DepartmentId	INT NULL,
	GradeId			INT NULL,
	DesignationId	INT NULL
*/

/*	
	update Manager
	set JoinDate =  SUBSTRING(JoinDate,7, 4) + '/' + SUBSTRING(JoinDate,1, 2) + '/' + SUBSTRING(JoinDate,4, 2)
	where JoinDate IS NOT NULL
	
	UPDATE Manager 
	SET JoinDate = NULL WHERE JoinDate = '.201/01/.1'
	
	update Manager
	set ContractEndDate =  SUBSTRING(ContractEndDate,7, 4) + '/' + SUBSTRING(ContractEndDate,1, 2) + '/' + SUBSTRING(ContractEndDate,4, 2)
	where ContractEndDate IS NOT NULL

	UPDATE Manager 
	SET ContractEndDate = NULL WHERE ContractEndDate = '.201/01/.1'

	update Manager
	set EmployeeType = RTRIM(LTRIM(EmployeeType))

	update Manager
	set Department = RTRIM(LTRIM(Department))

	update Manager
	set EmployeeGrade = RTRIM(LTRIM(EmployeeGrade))

	update Manager
	set Designation = RTRIM(LTRIM(Designation))


-------------------
	UPDATE Manager SET Degination = REPLACE(Degination, CHAR(255), '')
	UPDATE Manager SET JoinDate = REPLACE(JoinDate, '.', '/')

	UPDATE Manager SET JoinDate = '2016/01/15'
	WHERE JoinDate = '2016/15/01'
	
	UPDATE Manager SET Designation = 'A/C Technician'
	WHERE JoinDate = 'A/C Technichian'

	SELECT JoinDate, REPLACE(JoinDate, '.', '/')
	from Manager 
*/


/*
	INSERT INTO PayrollEmpType
	(Name, ActiveStat, CreatedBy, CreatedDate)
	SELECT DISTINCT EmployeeType, 1, 1, GETDATE()
	from Manager WHERE EmployeeType IS NOT NULL AND
	EmployeeType NOT IN (SELECT Name FROm PayrollEmpType)

	INSERT INTO PayrollDepartment
	(Name, ActiveStat, CreatedBy, CreatedDate)
	SELECT DISTINCT Department, 1, 1, GETDATE()
	from Manager 
	WHERE Department IS NOT NULL AND
	Department NOT IN (SELECT Name FROm PayrollDepartment)

	INSERT INTO PayrollEmpGrade
	(Name, ActiveStat, CreatedBy, CreatedDate)
	SELECT DISTINCT EmployeeGrade, 1, 1, GETDATE()
	from Manager 
	WHERE EmployeeGrade IS NOT NULL AND
	Department NOT IN (SELECT Name FROm PayrollEmpGrade)

	INSERT INTO PayrollDesignation
	(Name, ActiveStat, CreatedBy, CreatedDate)
	SELECT DISTINCT Designation, 1, 1, GETDATE()
	from Manager 
	WHERE Designation IS NOT NULL AND
	Designation NOT IN (SELECT Name FROm PayrollDesignation)
*/

/*
	UPDATE fbp 
	SET fbp.EmpTypeId = etyp.TypeId
	FROM Manager fbp inner join PayrollEmpType etyp ON fbp.EmployeeType = etyp.Name

	UPDATE fbp 
	SET fbp.DepartmentId = etyp.DepartmentId
	FROM Manager fbp inner join PayrollDepartment etyp ON fbp.Department = etyp.Name

	UPDATE fbp 
	SET fbp.DesignationId = etyp.DesignationId
	FROM Manager fbp inner join PayrollDesignation etyp ON fbp.Designation = etyp.Name
	
	SELECT  
	fbp.EmployeeType, etyp.TypeId
	FROM Manager fbp inner join PayrollEmpType etyp ON fbp.EmployeeType = etyp.Name

	SELECT  
	fbp.Department, etyp.Name, etyp.DepartmentId
	FROM Manager fbp inner join PayrollDepartment etyp ON fbp.Department = etyp.Name

	SELECT  
	fbp.Designation, etyp.Name, etyp.DesignationId
	FROM Manager fbp inner join PayrollDesignation etyp ON fbp.Designation = etyp.Name
	
*/


INSERT INTO PayrollEmployee
(EmpCode, Title, FirstName, LastName, EmpTypeId, InitialContractEndDate,
		DepartmentId, JoinDate, GradeId, DesignationId, Gender, Religion, MaritalStatus, Nationality)
SELECT EmployeeCode,	Title,	FirstName,	LastName,	EmpTypeId,	
	   ContractEndDate,	DepartmentId,	JoinDate,	GradeId,	DesignationId,	Gender,	Religion,	
	   MaritalStatus,	Nationality
from Manager
WHERE EmployeeCode IS NOT NULL


select  EmpCode, Title, FirstName, LastName, EmpTypeId, InitialContractEndDate,
		DepartmentId, JoinDate, GradeId, DesignationId, Gender, Religion, MaritalStatus, Nationality
FROm PayrollEmployee

--update emp set emp.EmpTypeId = emk.EmpTypeId
--From PayrollEmployee emp inner join Manager emk on emp.EmpCode = emk.EmployeeCode
--where emk.[FirstName] is not null



/*

	DECLARE @ecode int = 58

	UPDATE pem
	SET pem.EmployeeCode = CONVERT(varchar(50), @ecode), @ecode = @ecode + 1
	FROM Manager pem
	WHERE EmployeeCode IS NOT NULL
*/

