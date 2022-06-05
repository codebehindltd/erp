namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSalesBillPayment")]
    public partial class PMSalesBillPayment
    {
        [Key]
        public long PaymentId { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal? PaymentLocalAmount { get; set; }

        public int? FieldId { get; set; }

        public int? NodeId { get; set; }

        public decimal? CurrencyAmount { get; set; }

        public decimal? PaymentAmount { get; set; }

        public int? BankId { get; set; }

        [StringLength(250)]
        public string BranchName { get; set; }

        [StringLength(50)]
        public string ChecqueNumber { get; set; }

        public DateTime? ChecqueDate { get; set; }

        [StringLength(20)]
        public string CardType { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        public DateTime? ExpireDate { get; set; }

        [StringLength(256)]
        public string CardHolderName { get; set; }

        [StringLength(256)]
        public string CardReference { get; set; }

        public int? DealId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(20)]
        public string PaymentMode { get; set; }
    }
}
