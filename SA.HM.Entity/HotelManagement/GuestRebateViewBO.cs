using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestRebateViewBO
    {
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ServiceDateString { get; set; }
        public string Reference { get; set; }
        public decimal? RebateAmount { get; set; }
        public string RebateBy { get; set; }
        public string RebateRemarks { get; set; }
    }
}
