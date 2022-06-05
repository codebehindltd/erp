namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMProductReturn")]
    public partial class PMProductReturn
    {
        [Key]
        public int ReturnId { get; set; }

        public DateTime? ReturnDate { get; set; }

        [StringLength(20)]
        public string ReturnType { get; set; }

        public int? TransactionId { get; set; }

        public int? ProductId { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; }

        public decimal? Quantity { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
