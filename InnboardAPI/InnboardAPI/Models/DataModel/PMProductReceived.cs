namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMProductReceived")]
    public partial class PMProductReceived
    {
        [Key]
        public int ReceivedId { get; set; }

        [StringLength(20)]
        public string ReceiveNumber { get; set; }

        public DateTime ReceivedDate { get; set; }

        public int POrderId { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(150)]
        public string Reason { get; set; }

        [StringLength(100)]
        public string ReferenceNumber { get; set; }

        [StringLength(250)]
        public string PurchaseBy { get; set; }

        public bool? IsApprovedForPosting { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiveType { get; set; }

        public int CostCenterId { get; set; }

        public int LocationId { get; set; }

        public int CurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConvertionRate { get; set; }

        public int? CheckedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CheckedDate { get; set; }

        public int? ApprovedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApprovedDate { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }
    }
}
