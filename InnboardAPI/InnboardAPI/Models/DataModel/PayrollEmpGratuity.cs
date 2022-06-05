namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpGratuity")]
    public partial class PayrollEmpGratuity
    {
        [Key]
        public int GratuityId { get; set; }

        public int EmpId { get; set; }

        public decimal? BasicAmount { get; set; }

        public decimal GratuityAmount { get; set; }

        public int NumberOfGratuity { get; set; }

        [Column(TypeName = "date")]
        public DateTime GratuityDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
