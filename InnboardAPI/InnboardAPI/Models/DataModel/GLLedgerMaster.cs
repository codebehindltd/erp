namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLLedgerMaster")]
    public partial class GLLedgerMaster
    {
        [Key]
        public long LedgerMasterId { get; set; }

        public int CompanyId { get; set; }

        public int ProjectId { get; set; }

        public int? DonorId { get; set; }

        [Required]
        [StringLength(15)]
        public string VoucherType { get; set; }

        public bool? IsBankExist { get; set; }

        [StringLength(20)]
        public string VoucherNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? VoucherDate { get; set; }

        public int CurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConvertionRate { get; set; }

        [StringLength(500)]
        public string Narration { get; set; }

        [StringLength(256)]
        public string PayerOrPayee { get; set; }

        [StringLength(20)]
        public string GLStatus { get; set; }

        public int? CheckedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public int? FiscalYearId { get; set; }
    }
}
