using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class AirlineBO
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? AirlineTime { get; set; }
        public string AirlineTimeString { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
