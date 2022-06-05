namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMPurchaseOrder")]
    public partial class PMPurchaseOrder
    {
        [Key]
        public int POrderId { get; set; }

        public DateTime? PODate { get; set; }

        public DateTime? ReceivedByDate { get; set; }

        [StringLength(50)]
        public string PONumber { get; set; }

        [StringLength(20)]
        public string POType { get; set; }

        [StringLength(50)]
        public string IsLocalOrForeignPO { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(20)]
        public string ApprovedStatus { get; set; }

        [StringLength(50)]
        public string ReceivedStatus { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int? CheckedBy { get; set; }

        public DateTime? CheckedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int CostCenterId { get; set; }

        public long CurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConvertionRate { get; set; }

        [StringLength(25)]
        public string ReceiveStatus { get; set; }
    }
}
