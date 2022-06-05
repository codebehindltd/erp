namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomRegistration")]
    public partial class HotelRoomRegistration
    {
        [Key]
        public int RegistrationId { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        public DateTime? ArriveDate { get; set; }

        public DateTime? BillingStartDate { get; set; }

        public DateTime? ExpectedCheckOutDate { get; set; }

        public DateTime? BillHoldUpDate { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public DateTime? ActualCheckOutDate { get; set; }

        public int? RoomId { get; set; }

        public int? EntitleRoomType { get; set; }

        public int? CurrencyType { get; set; }

        public decimal? ConversionRate { get; set; }

        public decimal? UnitPrice { get; set; }

        [StringLength(20)]
        public string DiscountType { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? RoomRate { get; set; }

        public decimal? NoShowCharge { get; set; }

        public bool? IsServiceChargeEnable { get; set; }

        public bool? IsCityChargeEnable { get; set; }

        public bool? IsVatAmountEnable { get; set; }

        public bool? IsAdditionalChargeEnable { get; set; }

        public decimal? TotalRoomRate { get; set; }

        public bool? IsCompanyGuest { get; set; }

        public bool? IsHouseUseRoom { get; set; }

        [StringLength(200)]
        public string CommingFrom { get; set; }

        [StringLength(200)]
        public string NextDestination { get; set; }

        [StringLength(500)]
        public string VisitPurpose { get; set; }

        public bool? IsFromReservation { get; set; }

        public int? ReservationId { get; set; }

        public bool? IsFamilyOrCouple { get; set; }

        public int? NumberOfPersonAdult { get; set; }

        public int? NumberOfPersonChild { get; set; }

        public bool? IsListedCompany { get; set; }

        [StringLength(100)]
        public string ReservedCompany { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(200)]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        public string ContactNumber { get; set; }

        [StringLength(20)]
        public string PaymentMode { get; set; }

        public int? PayFor { get; set; }

        public int? BusinessPromotionId { get; set; }

        public int? IsRoomOwner { get; set; }

        public int? GuestSourceId { get; set; }

        public int? MealPlanId { get; set; }

        public int? ReferenceId { get; set; }

        public bool? IsReturnedGuest { get; set; }

        public bool? IsVIPGuest { get; set; }

        public int? VipGuestTypeId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(50)]
        public string AirportPickUp { get; set; }

        [StringLength(50)]
        public string AirportDrop { get; set; }

        [StringLength(20)]
        public string CardType { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        public DateTime? CardExpireDate { get; set; }

        [StringLength(256)]
        public string CardHolderName { get; set; }

        [StringLength(256)]
        public string CardReference { get; set; }

        public bool? IsStopChargePosting { get; set; }

        public bool? IsBlankRegistrationCard { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsEarlyCheckInChargeEnable { get; set; }

        public Guid GuidId { get; set; }

        [StringLength(500)]
        public string POSRemarks { get; set; }

        public decimal HoldUpAmount { get; set; }

        public int? MarketSegmentId { get; set; }

        [NotMapped]
        public DateTime? CheckOutDateForAPI { get; set; }
    }
}
