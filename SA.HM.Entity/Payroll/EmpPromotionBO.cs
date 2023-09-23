using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpPromotionBO
    {
        public Int64 PromotionId { get; set; }
        public int EmpId { get; set; }
        public DateTime PromotionDate { get; set; }
        public string PromotionDateDisplay { get; set; }
        public int PreviousDesignationId { get; set; }
        public int PreviousGradeId { get; set; }
        public int CurrentDesignationId { get; set; }
        public int CurrentGradeId { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime PromotionDateCreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string PreviousDesignation { get; set; }
        public string CurrentDesignation { get; set; }
        public string PreviousGrade { get; set; }
        public string CurrentGrade { get; set; }
        public string DepartmentName { get; set; }
        public string PromotionDateShow { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EffectiveDateDisplay { get; set; }
    }
}
