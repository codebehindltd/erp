namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpPayScale")]
    public partial class PayrollEmpPayScale
    {
        [Key]
        public int PayScaleId { get; set; }

        public DateTime? ScaleDate { get; set; }

        public int? GradeId { get; set; }

        public decimal? BasicAmount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
