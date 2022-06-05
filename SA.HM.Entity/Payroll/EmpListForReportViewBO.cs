using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpListForReportViewBO
    {
        public int    EmpId { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string BloodGroup { get; set; }
        public string EmpCode { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string PersonalEmail { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string Height { get; set; }
        public string EmpDateOfBirth { get; set; }
        public int GLCompanyId { get; set; }
        public int GLProjectId { get; set; }
        public int    CountryId { get; set; }
        public string NationalId { get; set; }
        public string PresentPhone { get; set; }
        public string Religion { get; set; }
        public string PresentAddress { get; set; }
        public string PresentCity { get; set; }
        public string PresentCountry { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentCountry { get; set; }
        public int departmentId { get; set; }
        public string DepartmentName { get; set; }
        public int designationId { get; set; }
        public string DesignationName { get; set; }
        public int workStationId { get; set; }
        public string WorkStationName { get; set; }
        public string JoinDateString { get; set; }
        public string NationalIdOrOtherCertificate { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeTypeExtension { get; set; }
        public string PromotionDateDisplay { get; set; }
        public string ReferenceName { get; set; }
        public string PreviousDesignation { get; set; }
        public string CurrentDesignation { get; set; }
        public string PreviousGrade { get; set; }
        public string CurrentGrade { get; set; }
        public string Department { get; set; }
        public string CompanyName { get; set; }
        public string SalaryProcessType { get; set; }
        public string LetterBody { get; set; }
        public decimal BasicAmount { get; set; }
        public string BestWishes { get; set; }
        public string BestRegards { get; set; }
        public string LetterNote { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string CreatedBy { get; set; }
        public string IncrementDateDisplay { get; set; }
        public string CurrencyName { get; set; }
        public decimal BasicSalary { get; set; }
        public string IncrementMode { get; set; }
        public string EffectiveDateDisplay { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal IncrementAmount { get; set; }
        public string ApprovedStatus { get; set; }
    }
}
