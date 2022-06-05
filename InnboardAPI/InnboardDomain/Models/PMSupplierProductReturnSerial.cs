namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSupplierProductReturnSerial")]
    public partial class PMSupplierProductReturnSerial
    {
        [Key]
        public long ReturnSerialId { get; set; }

        public long ReturnId { get; set; }

        public int ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; }
    }
}
