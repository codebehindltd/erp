namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomReservation")]
    public partial class HotelRoomReservation
    {
        [Key]
        public long ReservationId { get; set; }

        [StringLength(50)]
        public string ReservationNumber { get; set; }

        public DateTime? ReservationDate { get; set; }
        public string ReservationDateDisplay { get; set; }
        public DateTime? DateIn { get; set; }
        public string DateInDisplay { get; set; }

        public DateTime? DateOut { get; set; }

        public string DateOutDisplay { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        [StringLength(100)]
        public string ReservedCompany { get; set; }

        public long? GuestId { get; set; }
        [StringLength(500)]
        public string GuestName { get; set; }

        [StringLength(300)]
        public string ContactAddress { get; set; }

        [StringLength(200)]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        public string ContactNumber { get; set; }

        [StringLength(20)]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        public string FaxNumber { get; set; }

        [StringLength(100)]
        public string ContactEmail { get; set; }

        public int? TotalRoomNumber { get; set; }

        [StringLength(20)]
        public string ReservedMode { get; set; }

        [StringLength(20)]
        public string ReservationType { get; set; }

        [StringLength(20)]
        public string ReservationMode { get; set; }

        public DateTime? PendingDeadline { get; set; }

        public bool? IsListedCompany { get; set; }

        public int? CompanyId { get; set; }

        public int? BusinessPromotionId { get; set; }

        public int? ReferenceId { get; set; }

        [StringLength(20)]
        public string PaymentMode { get; set; }

        public int? PayFor { get; set; }

        public int? CurrencyType { get; set; }

        public decimal? ConversionRate { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? NumberOfPersonAdult { get; set; }

        public int? NumberOfPersonChild { get; set; }

        public bool? IsFamilyOrCouple { get; set; }

        [StringLength(50)]
        public string AirportPickUp { get; set; }

        [StringLength(50)]
        public string AirportDrop { get; set; }

        [StringLength(1000)]
        public string RoomInfo { get; set; }

        public int? MarketSegmentId { get; set; }

        public int? GuestSourceId { get; set; }

        public bool? IsRoomRateShowInPreRegistrationCard { get; set; }

        public int? MealPlanId { get; set; }

        public int? ClassificationId { get; set; }

        public bool? IsVIPGuest { get; set; }

        public int? VipGuestTypeId { get; set; }

        public string BookersName { get; set; }

        [StringLength(500)]
        public string GuestRemarks { get; set; }

        [StringLength(500)]
        public string POSRemarks { get; set; }
    }
}
