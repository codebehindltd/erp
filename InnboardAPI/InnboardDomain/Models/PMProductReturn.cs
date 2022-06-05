namespace InnboardDomain.Models
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
        public long ReturnId { get; set; }

        public DateTime ReturnDate { get; set; }

        [Required]
        [StringLength(25)]
        public string ReturnType { get; set; }

        public long TransactionId { get; set; }

        public int? ProductId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Required]
        [StringLength(25)]
        public string ReturnNumber { get; set; }

        public int FromCostCenterId { get; set; }

        public int FromLocationId { get; set; }

        [Required]
        [StringLength(25)]
        public string Status { get; set; }

        public int? CheckedBy { get; set; }

        public DateTime? CheckedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
