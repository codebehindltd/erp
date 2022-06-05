namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemStockSerialInformation")]
    public partial class InvItemStockSerialInformation
    {
        [Key]
        public long SerialStockId { get; set; }

        public int? LocationId { get; set; }

        public int? ItemId { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; }

        [StringLength(25)]
        public string SerialStatus { get; set; }
    }
}
