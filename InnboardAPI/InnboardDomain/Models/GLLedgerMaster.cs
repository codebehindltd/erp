namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLLedgerMaster")]
    public partial class GLLedgerMaster
    {
        public GLLedgerMaster()
        {
            GLLedgerDetails = new HashSet<GLLedgerDetails>();
            GLVoucherApprovedInfos = new HashSet<GLVoucherApprovedInfo>();
        }
        [Key]
        public long LedgerMasterId { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int? DonorId { get; set; }
        public string VoucherType { get; set; }
        public bool? IsBankExist { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int CurrencyId { get; set; }
        public decimal? ConvertionRate { get; set; }
        public string Narration { get; set; }
        public string PayerOrPayee { get; set; }
        public string GLStatus { get; set; }
        public int? CheckedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string ReferenceNumber { get; set; }
        public int? FiscalYearId { get; set; }
        public string ReferenceVoucherNumber { get; set; }
        public bool IsSynced { get; set; }

        public ICollection<GLLedgerDetails> GLLedgerDetails { get; set; }
        public ICollection<GLVoucherApprovedInfo> GLVoucherApprovedInfos { get; set; }
    }
}
