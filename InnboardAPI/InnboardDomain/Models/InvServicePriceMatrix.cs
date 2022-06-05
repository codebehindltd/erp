namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvServicePriceMatrix")]
    public partial class InvServicePriceMatrix
    {
        [Key]
        public int ServicePriceMatrixId { get; set; }

        public int ItemId { get; set; }

        public int ServicePackageId { get; set; }

        public int ServiceBandWidthId { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
