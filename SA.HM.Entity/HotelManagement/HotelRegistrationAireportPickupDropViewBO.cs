using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelRegistrationAireportPickupDropViewBO
    {
        public int APDId { get; set; }

        public int? RegistrationId { get; set; }

        [StringLength(200)]
        public string ArrivalFlightName { get; set; }

        [StringLength(50)]
        public string ArrivalFlightNumber { get; set; }

        public DateTime? ArrivalTime { get; set; }

        public int? DepartureAirlineId { get; set; }

        [StringLength(200)]
        public string DepartureFlightName { get; set; }

        [StringLength(50)]
        public string DepartureFlightNumber { get; set; }

        public DateTime? DepartureTime { get; set; }
    }
}
