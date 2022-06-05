namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("viewCommonVoucher")]
    public partial class viewCommonVoucher
    {
        public long? LedgerMasterId { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(20)]
        public string CompanyCode { get; set; }

        [StringLength(150)]
        public string CompanyName { get; set; }

        public int? ProjectId { get; set; }

        [StringLength(20)]
        public string ProjectCode { get; set; }

        [StringLength(150)]
        public string ProjectName { get; set; }

        [StringLength(20)]
        public string VoucherNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? VoucherDate { get; set; }

        [StringLength(500)]
        public string Narration { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LedgerDetailsId { get; set; }

        [StringLength(500)]
        public string NodeNarration { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long NodeId { get; set; }

        [StringLength(900)]
        public string Hierarchy { get; set; }

        public int? Lvl { get; set; }

        [StringLength(256)]
        public string NodeHead { get; set; }

        [StringLength(50)]
        public string NodeNumber { get; set; }

        [StringLength(256)]
        public string ChequeNumber { get; set; }

        public bool? NodeMode { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "money")]
        public decimal DRAmount { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "money")]
        public decimal CRAmount { get; set; }

        [StringLength(256)]
        public string VcheqNo { get; set; }

        [StringLength(20)]
        public string GLStatus { get; set; }

        [StringLength(256)]
        public string PayerOrPayee { get; set; }

        public byte? LedgerMode { get; set; }

        public int? DonorId { get; set; }
    }
}
