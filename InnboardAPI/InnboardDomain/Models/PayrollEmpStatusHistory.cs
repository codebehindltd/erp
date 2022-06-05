namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpStatusHistory")]
    public partial class PayrollEmpStatusHistory
    {
        public long Id { get; set; }

        public long EmpId { get; set; }

        public int EmpStatusId { get; set; }

        public DateTime ActionDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        [Required]
        [StringLength(1000)]
        public string Reason { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
