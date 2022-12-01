using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.AirTicketing
{
    public class AirlineTicketInfoBO
    {
        public string AirlineName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime FlightDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string ClientName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketType { get; set; }
        public int AirlineId { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string TicketNumber { get; set; }
        public string PnrNumber { get; set; }
        public decimal AirlineAmount { get; set; }
        public string RoutePath { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
