namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelReservationAireportPickupDrop")]
    public partial class HotelReservationAireportPickupDrop
    {
        [Key]
        public int APDId { get; set; }

        public int? ReservationId { get; set; }

        public int? GuestId { get; set; }

        [StringLength(20)]
        public string PickupDropType { get; set; }

        public int? ArrivalAirlineId { get; set; }

        [StringLength(200)]
        public string ArrivalFlightName { get; set; }

        [StringLength(200)]
        public string ArrivalFlightNumber { get; set; }

        public TimeSpan? ArrivalTime { get; set; }

        public bool? IsArrivalChargable { get; set; }

        public int? DepartureAirlineId { get; set; }

        [StringLength(200)]
        public string DepartureFlightName { get; set; }

        [StringLength(200)]
        public string DepartureFlightNumber { get; set; }

        public TimeSpan? DepartureTime { get; set; }

        public bool? IsDepartureChargable { get; set; }
    }
}
