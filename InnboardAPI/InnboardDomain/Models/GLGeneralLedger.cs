namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLGeneralLedger")]
    public partial class GLGeneralLedger
    {
        [Key]
        public long GLId { get; set; }

        public int? CostCentreId { get; set; }

        [StringLength(256)]
        public string CostCentreHead { get; set; }

        public long? NodeId { get; set; }

        [StringLength(256)]
        public string NodeHead { get; set; }

        public int? Lvl { get; set; }

        [StringLength(900)]
        public string Hierarchy { get; set; }

        [StringLength(900)]
        public string HierarchyIndex { get; set; }

        public byte? NodeMode { get; set; }

        public int? DealId { get; set; }

        public int? LedgerId { get; set; }

        public DateTime? VoucherDate { get; set; }

        [StringLength(12)]
        public string VoucherNo { get; set; }

        public byte? LedgerMode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PriorBalance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ReceivedAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidAmount { get; set; }

        [StringLength(500)]
        public string NodeNarration { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
