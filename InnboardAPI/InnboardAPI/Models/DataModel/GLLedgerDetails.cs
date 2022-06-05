namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GLLedgerDetails
    {
        [Key]
        public long LedgerDetailsId { get; set; }

        public long LedgerMasterId { get; set; }

        public long NodeId { get; set; }

        public int? BankAccountId { get; set; }

        [StringLength(256)]
        public string ChequeNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChequeDate { get; set; }

        public byte? LedgerMode { get; set; }

        [Column(TypeName = "money")]
        public decimal DRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CRAmount { get; set; }

        [StringLength(500)]
        public string NodeNarration { get; set; }

        public int CostCenterId { get; set; }

        [Column(TypeName = "money")]
        public decimal? CurrencyAmount { get; set; }

        [StringLength(25)]
        public string NodeType { get; set; }

        public long? ParentId { get; set; }

        public long? ParentLedgerId { get; set; }
    }
}
