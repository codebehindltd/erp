namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BanquetReservationBillPayment")]
    public partial class BanquetReservationBillPayment
    {
        public long Id { get; set; }

        public long? ReservationId { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }

        [StringLength(100)]
        public string PaymentType { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(20)]
        public string PaymentMode { get; set; }

        public long? FieldId { get; set; }

        public decimal? CurrencyAmount { get; set; }

        public decimal? PaymentAmount { get; set; }

        public long? AccountsPostingHeadId { get; set; }

        public int? IsPaymentTransfered { get; set; }

        public long? BankId { get; set; }

        [StringLength(250)]
        public string BranchName { get; set; }

        [StringLength(50)]
        public string ChecqueNumber { get; set; }

        public DateTime? ChecqueDate { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(50)]
        public string CardReference { get; set; }

        public long? DealId { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [StringLength(20)]
        public string CardType { get; set; }

        [StringLength(256)]
        public string CardHolderName { get; set; }

        public virtual BanquetReservation BanquetReservation { get; set; }

        public virtual GLNodeMatrix GLNodeMatrix { get; set; }
    }
}
