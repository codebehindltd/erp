namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpLeaveInformation")]
    public partial class PayrollEmpLeaveInformation
    {
        [Key]
        public int LeaveId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(20)]
        public string LeaveMode { get; set; }

        public int? LeaveTypeId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; }

        public int? NoOfDays { get; set; }

        public DateTime? ExpireDate { get; set; }

        [StringLength(50)]
        public string LeaveStatus { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        public int? ReportingTo { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
