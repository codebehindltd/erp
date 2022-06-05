namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomReservationSummary")]
    public partial class HotelRoomReservationSummary
    {
        public long Id { get; set; }

        public long? ReservationId { get; set; }

        public int? RoomTypeId { get; set; }

        public int? RoomQuantity { get; set; }

        public int? PaxQuantity { get; set; }
    }
}
