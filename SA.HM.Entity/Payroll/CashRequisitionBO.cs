using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class CashRequisitionBO : PermissionViewBO
    {
        public Int64 Id { get; set; }
        public Int64 DetailId { get; set; }
        public Int64 SerialNumber { get; set; }
        public Int64? RefId { get; set; }
        public int CostCenterId { get; set; }
        public int RequisitionForHeadId { get; set; }
        public string RequisitionForHeadName { get; set; }
        public string AdjustmentIdList { get; set; }
        public string AuthorizedByList { get; set; }        
        public int? CompanyId { get; set; }
        public int? ProjectId { get; set; }
        public int? IndividualCompanyId { get; set; }
        public int? IndividualProjectId { get; set; }
        public int? EmployeeId { get; set; }
        public int? TransactionFromAccountHeadId { get; set; }
        public string TransactionFromAccountHead { get; set; }
        public decimal? Amount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public decimal? RemainingBalance { get; set; }
        public decimal? RequsitionAmount { get; set; }
        public string Remarks { get; set; }
        public string IndividualRemarks { get; set; }
        public string IndividualCompanyName { get; set; }
        public string IndividualProjectName { get; set; }
        public int RemainingAdjustmentDay { get; set; }
        public bool HasPermissionForChild { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? LastAdjustmentDate { get; set; }
        public DateTime? RequireDate { get; set; }
        public string ApprovedStatus { get; set; }
        public string CostCenterName { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionType { get; set; }
        public string RequsitionBy { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public bool HaveCashRequisitionAdjustment { get; set; }
        public string RequsitionDateString { get; set; }
        public string RequireDateString { get; set; }
        public string ApproveDateString { get; set; }
        public string OfficialEmail { get; set; }
        public string OfficialPhone { get; set; }
        public int? SupplierId { get; set; }

        
        public DateTime? RequisitionDate { get; set; }
        public string Employee { get; set; }
        //public decimal? Amount { get; set; }
        public string Company { get; set; }
        public string Project { get; set; }
        public string RequisitionFor { get; set; }
        //public string Remarks { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherNarration { get; set; }
        //public decimal? RemainingBalance { get; set; }
        public string AdjustmentNo { get; set; }
        public DateTime? AdjustmentDate { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public string AdjustmentCompany { get; set; }
        public string AdjustmentProject { get; set; }
        public string AdjustmentPurpose { get; set; }
        public string RequisitionDateDisplay { get; set; }
        public string VoucherDateDisplay { get; set; }
        public string AdjustmentDateDisplay { get; set; }
    }
}
