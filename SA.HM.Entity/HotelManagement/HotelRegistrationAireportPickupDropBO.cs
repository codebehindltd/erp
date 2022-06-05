using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelRegistrationAireportPickupDropBO
    {
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

        public bool? IsDepartureChargable { get; set; }
    }
}
