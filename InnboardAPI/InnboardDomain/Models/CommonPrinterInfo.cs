namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonPrinterInfo")]
    public partial class CommonPrinterInfo
    {
        [Key]
        public int PrinterInfoId { get; set; }

        public int? CostCenterId { get; set; }

        [StringLength(100)]
        public string StockType { get; set; }

        public int? KitchenId { get; set; }

        [StringLength(500)]
        public string PrinterName { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
