namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollSalaryHead")]
    public partial class PayrollSalaryHead
    {
        [Key]
        public int SalaryHeadId { get; set; }

        [StringLength(200)]
        public string SalaryHead { get; set; }

        [StringLength(50)]
        public string SalaryType { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; }

        public DateTime? EffectedMonth { get; set; }

        public bool? IsShowOnlyAllownaceDeductionPage { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
