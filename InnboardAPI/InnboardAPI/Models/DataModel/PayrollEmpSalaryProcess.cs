namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpSalaryProcess")]
    public partial class PayrollEmpSalaryProcess
    {
        [Key]
        public int ProcessId { get; set; }

        public DateTime? ProcessDate { get; set; }

        public DateTime? SalaryDateFrom { get; set; }

        public DateTime? SalaryDateTo { get; set; }

        public short? SalaryYear { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool? IsBonusPaid { get; set; }
    }
}
