namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomReservationOnline")]
    public partial class HotelRoomReservationOnline
    {
        [Key]
        public long ReservationId { get; set; }

        [StringLength(50)]
        public string ReservationNumber { get; set; }

        public DateTime? ReservationDate { get; set; }

        public DateTime? DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        [StringLength(100)]
        public string ReservedCompany { get; set; }

        public long? GuestId { get; set; }

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
    }
}
