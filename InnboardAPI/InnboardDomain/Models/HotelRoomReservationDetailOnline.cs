namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomReservationDetailOnline")]
    public partial class HotelRoomReservationDetailOnline
    {
        [Key]
        public int ReservationDetailId { get; set; }

        public int? ReservationId { get; set; }

        public int? RoomTypeId { get; set; }

        public int? RoomId { get; set; }

        public decimal? UnitPrice { get; set; }

        [StringLength(20)]
        public string DiscountType { get; set; }

        public decimal? Amount { get; set; }

        public decimal? RoomRate { get; set; }

        public decimal? NoShowCharge { get; set; }

        public bool? IsRegistered { get; set; }

        [StringLength(20)]
        public string Status { get; set; }
    }
}
