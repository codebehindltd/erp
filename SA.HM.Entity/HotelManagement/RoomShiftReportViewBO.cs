using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomShiftReportViewBO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ShiftDate { get; set; }
        public string Remarks { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string PreviousRoom { get; set; }
        public string ShiftedRoom { get; set; }
        public string ShiftedBy { get; set; }
    }
}
