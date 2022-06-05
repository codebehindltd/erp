namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCPaymentLedger")]
    public partial class LCPaymentLedger
    {
        [Key]
        public long LCPaymentId { get; set; }

        [Required]
        [StringLength(15)]
        public string PaymentType { get; set; }

        [StringLength(25)]
        public string BillNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        public int LCId { get; set; }

        public int? LCBankAccountHeadId { get; set; }

        public int? AccountHeadId { get; set; }

        public int CurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConvertionRate { get; set; }

        [Column(TypeName = "money")]
        public decimal DRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? CurrencyAmount { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
