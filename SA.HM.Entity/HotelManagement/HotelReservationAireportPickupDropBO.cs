using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelReservationAireportPickupDropBO
    {
        public int APDId { get; set; }
        public int ReservationId { get; set; }
        public int? GuestId { get; set; }
        public int ArrivalAirlineId { get; set; }
        public string ArrivalFlightName { get; set; }
        public string ArrivalFlightNumber { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public int DepartureAirlineId { get; set; }
        public string DepartureFlightName { get; set; }
        public string DepartureFlightNumber { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string PickupDropType { get; set; }
        public string GuestName { get; set; }
        public string AirportPickUp { get; set; }
        public string AirportDrop { get; set; }
        public string ArrivalTimeShow { get; set; }
        public string DepartureTimeShow { get; set; }
        public Boolean IsArrivalChargable { get; set; }
        public decimal ArrivalChargableAmount { get; set; }
        public Boolean IsDepartureChargable { get; set; }
        public decimal DepartureChargableAmount { get; set; }
    }
}
