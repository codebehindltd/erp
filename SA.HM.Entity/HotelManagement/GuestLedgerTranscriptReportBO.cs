using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestLedgerTranscriptReportBO
    {
        //RegistrationId	RoomNumber	GuestName	CompanyName	Pay	Araival	Departure	ServiceId	ServiceName	ServiceAmount

        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public int RoomNumber { get; set; }
        public string GuestName { get; set; }
        public string CompanyName { get; set; }
        public string Pay { get; set; }
        public DateTime Araival { get; set; }
        public DateTime Departure { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServiceAmount { get; set; }
        public DateTime ServiceDate { get; set; }
        public int BillPaidBy { get; set; }
        public decimal RoomTransferedTotalAmount { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}
