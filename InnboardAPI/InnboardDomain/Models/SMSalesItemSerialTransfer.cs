namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSalesItemSerialTransfer")]
    public partial class SMSalesItemSerialTransfer
    {
        [Key]
        public long SalesItemSerialTransferId { get; set; }

        public long SalesTransferId { get; set; }

        public int ItemId { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; }
    }
}
