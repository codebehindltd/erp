namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpGrade")]
    public partial class PayrollEmpGrade
    {
        [Key]
        public int GradeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int? ProvisionPeriodId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public bool? IsManagement { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? BasicAmount { get; set; }
    }
}
