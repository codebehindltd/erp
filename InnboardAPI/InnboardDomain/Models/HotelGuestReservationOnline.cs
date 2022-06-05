namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestReservationOnline")]
    public partial class HotelGuestReservationOnline
    {
        [Key]
        public long GuestReservationId { get; set; }

        public long GuestId { get; set; }

        public long ReservationId { get; set; }

        public int? RoomId { get; set; }
    }
}
