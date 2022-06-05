namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTermination")]
    public partial class PayrollEmpTermination
    {
        [Key]
        public int TerminationId { get; set; }

        public int? EmpId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DecisionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TerminationDate { get; set; }

        public int? EmployeeStatusId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
