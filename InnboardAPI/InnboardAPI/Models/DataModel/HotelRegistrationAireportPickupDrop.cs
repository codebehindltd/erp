namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRegistrationAireportPickupDrop")]
    public partial class HotelRegistrationAireportPickupDrop
    {
        [Key]
        public int APDId { get; set; }

        public int? RegistrationId { get; set; }

        [StringLength(200)]
        public string ArrivalFlightName { get; set; }

        [StringLength(50)]
        public string ArrivalFlightNumber { get; set; }

        public TimeSpan? ArrivalTime { get; set; }

        public int? DepartureAirlineId { get; set; }

        [StringLength(200)]
        public string DepartureFlightName { get; set; }

        [StringLength(50)]
        public string DepartureFlightNumber { get; set; }

        public TimeSpan? DepartureTime { get; set; }
    }
}
