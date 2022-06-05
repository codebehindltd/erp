using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class SettlementSummaryBO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string BillNumber { get; set; }
        public string RoomNumber { get; set; }
        public int FolioNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string CurrencyName { get; set; }
        public decimal BillAmount { get; set; }
        public string PaymentMode { get; set; }
        public decimal SettlementAmount { get; set; }
        public string UserName { get; set; }


    }
}
