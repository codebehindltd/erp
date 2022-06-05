namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMProductOutSerialInfo")]
    public partial class PMProductOutSerialInfo
    {
        [Key]
        [Column(Order = 0)]
        public long OutSerialId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OutId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemId { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string SerialNumber { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CreatedBy { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime CreatedDate { get; set; }

        [StringLength(25)]
        public string ReturnOutStatus { get; set; }
    }
}
