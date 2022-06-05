namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTransfer")]
    public partial class PayrollEmpTransfer
    {
        [Key]
        public long TransferId { get; set; }

        public DateTime TransferDate { get; set; }

        public long EmpId { get; set; }

        public int PreviousDepartmentId { get; set; }

        public int? PreviousDesignationId { get; set; }

        public int CurrentDepartmentId { get; set; }

        public int? CurrentDesignationId { get; set; }

        public int? PreviousLocation { get; set; }

        public int? CurrentLocation { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReportingDate { get; set; }

        public DateTime JoinedDate { get; set; }

        public long ReportingToId { get; set; }

        [StringLength(25)]
        public string ApprovedStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
