namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSupplierProductReturn")]
    public partial class PMSupplierProductReturn
    {
        [Key]
        public long ReturnId { get; set; }

        public DateTime ReturnDate { get; set; }

        public int ReceivedId { get; set; }

        public int? POrderId { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Required]
        [StringLength(25)]
        public string ReturnNumber { get; set; }

        public int CostCenterId { get; set; }

        public int LocationId { get; set; }

        public int? CheckedBy { get; set; }

        public DateTime? CheckedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }
    }
}
