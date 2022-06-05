namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestBillPayment")]
    public partial class HotelGuestBillPayment
    {
        [Key]
        public int PaymentId { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }

        [StringLength(100)]
        public string ModuleName { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        public int? ServiceBillId { get; set; }

        public int? RegistrationId { get; set; }

        [StringLength(50)]
        public string RoomNumber { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? TransactionDate { get; set; }

        [StringLength(20)]
        public string PaymentMode { get; set; }

        public int? PaymentModeId { get; set; }

        public int? FieldId { get; set; }

        public decimal? CurrencyAmount { get; set; }

        public decimal? PaymentAmount { get; set; }

        [StringLength(500)]
        public string PaymentDescription { get; set; }

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

        public int? AccountsPostingHeadId { get; set; }

        public int? RefundAccountHead { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? DealId { get; set; }

        public decimal? ServiceRate { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsAdvancePayment { get; set; }
    }
}
