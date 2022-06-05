namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestServiceBill")]
    public partial class HotelGuestServiceBill
    {
        [Key]
        public int ServiceBillId { get; set; }

        public DateTime? ServiceDate { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }

        public int? RegistrationId { get; set; }

        [StringLength(250)]
        public string GuestName { get; set; }

        public int? ServiceId { get; set; }

        public decimal? ServiceRate { get; set; }

        public int? ServiceQuantity { get; set; }

        public decimal? DiscountAmount { get; set; }

        public bool? IsComplementary { get; set; }

        public bool? IsPaidService { get; set; }

        public bool? IsPaidServiceAchieved { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(20)]
        public string PaymentMode { get; set; }

        public int? EmpId { get; set; }

        public int? CompanyId { get; set; }

        public decimal? RackRate { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? InvoiceRackRate { get; set; }

        public bool? IsServiceChargeEnable { get; set; }

        public decimal? InvoiceServiceCharge { get; set; }

        public bool? IsCitySDChargeEnable { get; set; }

        public decimal? InvoiceCitySDCharge { get; set; }

        public bool? IsVatAmountEnable { get; set; }

        public decimal? InvoiceVatAmount { get; set; }

        public bool? IsAdditionalChargeEnable { get; set; }

        public decimal? InvoiceAdditionalCharge { get; set; }

        public decimal? TotalCalculatedAmount { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }

        public decimal? InvoiceUsdRackRate { get; set; }

        public decimal? InvoiceUsdServiceCharge { get; set; }

        public decimal? InvoiceUsdVatAmount { get; set; }

        public int? ReferenceServiceBillId { get; set; }

        public bool? ApprovedStatus { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
