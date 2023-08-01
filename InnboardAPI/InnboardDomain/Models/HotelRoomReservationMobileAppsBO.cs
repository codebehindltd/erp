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
        public string TransactionType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int RoomTypeId { get; set; }
        public decimal PaxQuantity { get; set; }
        public decimal ChildQuantity { get; set; }
        public decimal ExtraBedQuantity { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public string GuestNotes { get; set; }
    }
}
