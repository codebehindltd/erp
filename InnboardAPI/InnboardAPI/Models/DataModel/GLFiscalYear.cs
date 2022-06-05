namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLFiscalYear")]
    public partial class GLFiscalYear
    {
        [Key]
        public int FiscalYearId { get; set; }

        public int? ProjectId { get; set; }

        [StringLength(200)]
        public string FiscalYearName { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? IncomeTaxPercentage { get; set; }

        public bool? IsFiscalYearClosed { get; set; }
    }
}
