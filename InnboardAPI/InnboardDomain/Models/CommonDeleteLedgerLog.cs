namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonDeleteLedgerLog")]
    public partial class CommonDeleteLedgerLog
    {
        [Key]
        public long DeleteLedgerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeleteLedgerDate { get; set; }

        [StringLength(50)]
        public string ModuleName { get; set; }

        [StringLength(25)]
        public string LedgerNumber { get; set; }

        public int? BillId { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(15)]
        public string TransactionType { get; set; }

        public int TransactionId { get; set; }

        public int CurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConvertionRate { get; set; }

        [Column(TypeName = "money")]
        public decimal DRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? CurrencyAmount { get; set; }
    }
}
