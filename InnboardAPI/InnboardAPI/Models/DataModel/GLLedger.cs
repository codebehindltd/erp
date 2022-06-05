namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLLedger")]
    public partial class GLLedger
    {
        [Key]
        public int LedgerId { get; set; }

        public int DealId { get; set; }

        public long NodeId { get; set; }

        public byte LedgerMode { get; set; }

        public int? BankAccountId { get; set; }

        [StringLength(256)]
        public string ChequeNumber { get; set; }

        public decimal LedgerAmount { get; set; }

        [StringLength(500)]
        public string NodeNarration { get; set; }

        public int CostCenterId { get; set; }

        public int? FieldId { get; set; }

        public decimal? CurrencyAmount { get; set; }

        [StringLength(25)]
        public string NodeType { get; set; }

        public long? ParentId { get; set; }

        public long? ParentLedgerId { get; set; }
    }
}
