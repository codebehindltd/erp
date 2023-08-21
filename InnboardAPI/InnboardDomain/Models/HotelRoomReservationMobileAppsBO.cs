namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomReservation")]
    public class HotelRoomReservationMobileAppsBO
    {
        public long ReservationId { get; set; }
        public int GuestSourceId { get; set; }
        public int MemberId { get; set; }
        public string TransactionType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public string GuestRemarks { get; set; }
        public string TransactionId { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionDetails { get; set; }
        public int CreatedBy { get; set; }
        public List<HotelRoomReservationDetailsMobileAppsBO> HotelRoomReservationDetails { get; set; }
    }
}
