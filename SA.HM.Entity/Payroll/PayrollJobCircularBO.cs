using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollJobCircularBO
    {
        public long JobCircularId { get; set; }
        public int StaffRequisitionDetailsId { get; set; }
        public string JobTitle { get; set; }
        public DateTime CircularDate { get; set; }
        public int JobType { get; set; }
        public string JobLevel { get; set; }
        public int DepartmentId { get; set; }
        public short NoOfVancancie { get; set; }
        public DateTime DemandedTime { get; set; }
        public DateTime? OpenFrom { get; set; }
        public DateTime? OpenTo { get; set; }
        public byte AgeRangeFrom { get; set; }
        public byte AgeRangeTo { get; set; }
        public string Gender { get; set; }
        public byte YearOfExperiance { get; set; }
        public string JobDescription { get; set; }
        public string EducationalQualification { get; set; }
        public string AdditionalJobRequirement { get; set; }
        public string ApprovedStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public Int64 ApplicantId { get; set; }
        public string DepartmentName { get; set; }
        public string JobTypeName { get; set; }
        public decimal MarksObtain { get; set; }
        public string EmployeeName { get; set; }
        public string PresentPhone { get; set; }
    }
}
