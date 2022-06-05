namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSupplierPaymentLedger")]
    public partial class PMSupplierPaymentLedger
    {
        [Key]
        public long SupplierPaymentId { get; set; }

        [Required]
        [StringLength(15)]
        public string PaymentType { get; set; }

        [StringLength(25)]
        public string LedgerNumber { get; set; }

        public int? BillId { get; set; }

        [StringLength(25)]
        public string BillNumber { get; set; }

        public long? RefSupplierPaymentId { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        public int SupplierId { get; set; }

        public int CurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConvertionRate { get; set; }

        [Column(TypeName = "money")]
        public decimal DRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? CurrencyAmount { get; set; }

        public long? AccountsPostingHeadId { get; set; }

        [Column(TypeName = "money")]
        public decimal? DueAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdvanceAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdvanceAmountRemaining { get; set; }

        public bool? IsBillGenerated { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(100)]
        public string ChequeNumber { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; }

        public long? RefPaymentId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
