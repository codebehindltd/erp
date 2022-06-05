namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BanquetReservation")]
    public partial class BanquetReservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BanquetReservation()
        {
            BanquetBillPayment = new HashSet<BanquetBillPayment>();
            BanquetReservationBillPayment = new HashSet<BanquetReservationBillPayment>();
            BanquetReservationClassificationDiscount = new HashSet<BanquetReservationClassificationDiscount>();
            BanquetReservationDetail = new HashSet<BanquetReservationDetail>();
        }

        public long Id { get; set; }

        [StringLength(50)]
        public string ReservationNumber { get; set; }

        [StringLength(50)]
        public string ReservationMode { get; set; }

        public long? BanquetId { get; set; }

        public int? CostCenterId { get; set; }

        public bool? IsListedCompany { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
        public string CityName { get; set; }

        [StringLength(20)]
        public string ZipCode { get; set; }

        public int? CountryId { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string EmailAddress { get; set; }

        [StringLength(50)]
        public string BookingFor { get; set; }

        [StringLength(100)]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        public string ContactEmail { get; set; }

        [StringLength(50)]
        public string ContactPhone { get; set; }

        public DateTime? ArriveDate { get; set; }

        public DateTime? DepartureDate { get; set; }

        public long? OccessionTypeId { get; set; }

        public long? SeatingId { get; set; }

        public decimal? BanquetRate { get; set; }

        public decimal? BanquetDiscount { get; set; }

        public decimal? BanquetDiscountedAmount { get; set; }

        public decimal? BanquetRackRate { get; set; }

        public decimal? BanquetServiceCharge { get; set; }

        public decimal? BanquetCitySDCharge { get; set; }

        public decimal? BanquetVatAmount { get; set; }

        public decimal? BanquetAdditionalCharge { get; set; }

        public decimal? InvoiceServiceRate { get; set; }

        public bool? IsInvoiceServiceChargeEnable { get; set; }

        public decimal? InvoiceServiceCharge { get; set; }

        public bool? IsInvoiceCitySDChargeEnable { get; set; }

        public decimal? InvoiceCitySDCharge { get; set; }

        public bool? IsInvoiceVatAmountEnable { get; set; }

        public decimal? InvoiceVatAmount { get; set; }

        public bool? IsInvoiceAdditionalChargeEnable { get; set; }

        [StringLength(25)]
        public string AdditionalChargeType { get; set; }

        public decimal? InvoiceAdditionalCharge { get; set; }

        public int? NumberOfPersonAdult { get; set; }

        public int? NumberOfPersonChild { get; set; }

        public string CancellationReason { get; set; }

        public string SpecialInstructions { get; set; }

        public long? RefferenceId { get; set; }

        public bool? IsReturnedClient { get; set; }

        public string Comments { get; set; }

        public decimal? TotalAmount { get; set; }

        [StringLength(20)]
        public string DiscountType { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? CalculatedDiscountAmount { get; set; }

        public decimal? DiscountedAmount { get; set; }

        public decimal? ServiceRate { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public decimal? GrandTotal { get; set; }

        public decimal? RoundedAmount { get; set; }

        public decimal? RoundedGrandTotal { get; set; }

        [StringLength(500)]
        public string RebateRemarks { get; set; }

        public bool? IsBillSettlement { get; set; }

        public int? RegistrationId { get; set; }

        public int? ActiveStatus { get; set; }

        [StringLength(50)]
        public string BillStatus { get; set; }

        public int? BillVoidBy { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }

        public string Remarks { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? MarketSegmentId { get; set; }

        public int? GuestSourceId { get; set; }

        public string BookersName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetBillPayment> BanquetBillPayment { get; set; }

        public virtual BanquetInformation BanquetInformation { get; set; }

        public virtual BanquetOccessionType BanquetOccessionType { get; set; }

        public virtual BanquetRefference BanquetRefference { get; set; }

        public virtual BanquetSeatingPlan BanquetSeatingPlan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetReservationBillPayment> BanquetReservationBillPayment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetReservationClassificationDiscount> BanquetReservationClassificationDiscount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetReservationDetail> BanquetReservationDetail { get; set; }
    }
}
