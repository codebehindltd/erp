namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpIncrement")]
    public partial class PayrollEmpIncrement
    {
        public int Id { get; set; }

        public DateTime? IncrementDate { get; set; }

        public int? EmpId { get; set; }

        public decimal? BasicSalary { get; set; }

        [StringLength(50)]
        public string IncrementMode { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        [StringLength(25)]
        public string ApprovedStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
