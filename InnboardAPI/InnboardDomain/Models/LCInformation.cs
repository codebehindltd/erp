namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCInformation")]
    public partial class LCInformation
    {
        [Key]
        public long LCId { get; set; }

        [StringLength(50)]
        public string LCNumber { get; set; }

        [StringLength(50)]
        public string PINumber { get; set; }

        [StringLength(50)]
        public string LCValue { get; set; }

        public DateTime? LCOpenDate { get; set; }

        public DateTime? LCMatureDate { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(20)]
        public string ApprovedStatus { get; set; }

        public int? CheckedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool? IsLCBankSettlement { get; set; }

        public int? BankSettlementBy { get; set; }

        public DateTime? BankSettlementDate { get; set; }

        public bool? IsLCSettlement { get; set; }

        public int? SettlementBy { get; set; }

        public DateTime? SettlementDate { get; set; }

        [StringLength(500)]
        public string SettlementDescription { get; set; }
    }
}
