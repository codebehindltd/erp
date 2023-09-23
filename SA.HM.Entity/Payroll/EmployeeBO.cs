using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeBO : EmpBankInfoBO
    {
        public int EmpId { get; set; }
        public int AttendanceDeviceEmpId { get; set; }
        public string Title { get; set; }
        public string EmpCode { get; set; }
        public string EmpDisplayNameWithCode { get; set; }
        public string EmpPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeName { get; set; }
        public string TransactionName { get; set; }

        public DateTime? JoinDate { get; set; }
        public int WorkAnniversary { get; set; }
        public DateTime? ResignationDate { get; set; }
        public DateTime? InitialContractEndDate { get; set; }
        public DateTime? ProvisionPeriod { get; set; }
        public int DepartmentId { get; set; }
        public int TransactionCount { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Department { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpType { get; set; }
        public string TypeCategory { get; set; }        
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public decimal BasicAmount { get; set; }
        public DateTime? ProbablePFEligibilityDate { get; set; }
        public DateTime? PFEligibilityDate { get; set; }
        public DateTime? PFTerminateDate { get; set; }
        public DateTime? ProbableGratuityEligibilityDate { get; set; }
        public DateTime? GratuityEligibilityDate { get; set; }
        public string OfficialEmail { get; set; }
        public string PABXNumber { get; set; }
        public string ReferenceBy { get; set; }
        public string Remarks { get; set; }
        public int RepotingTo { get; set; }
        public string RepotingToOne { get; set; }
        public int RepotingTo2 { get; set; }
        public string RepotingToTwo { get; set; }
        public int NodeId { get; set; }
        public string EmpIdNNodeId { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public DateTime? EmpDateOfBirth { get; set; }
        public DateTime? EmpDateOfMarriage { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string Height { get; set; }
        public string MaritalStatus { get; set; }
        public int CountryId { get; set; }
        public int PresentCountryId { get; set; }
        public int PermanentCountryId { get; set; }
        public string Nationality { get; set; }
        public string NationalId { get; set; }
        public string TinNumber { get; set; }
        public int DivisionId { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int CostCenterId { get; set; }
        public string PassportNumber { get; set; }
        public string PIssuePlace { get; set; }
        public DateTime? PIssueDate { get; set; }
        public DateTime? PExpireDate { get; set; }
        public string PresentAddress { get; set; }
        public string PresentCity { get; set; }
        public string PresentZipCode { get; set; }
        public string PresentCountry { get; set; }
        public string PresentPhone { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentZipCode { get; set; }
        public string PermanentCountry { get; set; }
        public string PermanentPhone { get; set; }
        public string PersonalEmail { get; set; }
        public string AlternativeEmail { get; set; }
        public bool IsApplicant { get; set; }
        public bool IsApplicantRecruitment { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DocumentsBO Image { get; set; }
        public DocumentsBO Signature { get; set; }
        public string SiteTitle { get; set; }
        public string SignaturePath { get; set; }
        public string EmployeeSignature { get; set; }
        public string ImagePath { get; set; }
        public string EmployeeImage { get; set; }
        public int RandomEmpId { get; set; }
        public int WorkingCostCenterId { get; set; }
        public string ServerDateFormat { get; set; }
        public string ClientDateFormat { get; set; }
        public int? WorkStationId { get; set; }
        public int? DonorId { get; set; }
        public string ActivityCode { get; set; }
        public string DonorName { get; set; }
        public string WorkStationName { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EmergencyContactNumberHome { get; set; }
        public string EmergencyContactEmail { get; set; }
        public string DateOfBirthForReport { get; set; }
        public string JoinDateForReport { get; set; }
        public string CurrentLocation { get; set; }
        public int BearerId { get; set; }
        public bool IsRestaurantBillCanSettle { get; set; }
        public bool IsItemCanEditDelete { get; set; }
        public bool IsProvidentFundDeduct { get; set; }
        public DateTime DayOpenDate { get; set; }
        public int EmployeeStatusId { get; set; }
        public string EmployeeStatus { get; set; }
        public string ShowPIssueDate { get; set; }
        public string ShowPExpireDate { get; set; }
        public string Grade { get; set; }
        public string ShowProvisionPeriod { get; set; }
        public string ShowContractEndDate { get; set; }
        public string ReportingTo { get; set; }
        public string ReportingTo2 { get; set; }
        public string DistrictName { get; set; }
        public string DivisionName { get; set; }
        public string ThanaName { get; set; }
        public string ShowProbableGratuityEligibilityDate { get; set; }
        public string ShowProbablePFEligibilityDate { get; set; }
        public int PayrollCurrencyId { get; set; }
        public int NotEffectOnHead { get; set; }
        public string CurrencyName { get; set; }
        public bool IsContractualType { get; set; }
        public int TotalEmployeeNumber { get; set; }
        public int EmpCompanyId { get; set; }
        public int GlCompanyId { get; set; }
        public int GlProjectId { get; set; }
        public string GLCompanyName { get; set; }
        public decimal Balance { get; set; }
        public string AttendanceDeviceEmpCode { get; set; }
        public string JoinDateDisplay { get; set; }
        public string DateOfBirthDisplay { get; set; }
        public string ProvisionPeriodDisplay { get; set; }
        public int EmployeeCount { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string HolidayName { get; set; }
        public string Description { get; set; }
        public string RowNumber { get; set; }
        public Int64 SerialNumber { get; set; }

        //new 
        public string Extention { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string IconImage { get; set; }

        //Letter Information
        public string AppoinmentLetter { get; set; }
        public string JoiningAgreement { get; set; }
        public string ServiceBond { get; set; }
        public string DSOAC { get; set; }
        public string ConfirmationLetter { get; set; }
        public decimal EmpContribution { get; set; }
        public decimal CompanyContribution { get; set; }
        public decimal ProvidentFundInterest { get; set; }
        public string SalarySheetSpecialNotes { get; set; }
    }

}
