namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelReservationServiceInfo")]
    public partial class HotelReservationServiceInfo
    {
        [Key]
        public int DetailServiceId { get; set; }

        public int? ReservationId { get; set; }

        public int? ServiceId { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool? IsAchieved { get; set; }
    }
}
