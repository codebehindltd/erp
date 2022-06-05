using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class LateChkOutReportViewBO
    {
        public string DayLetDate { get; set; }
        public string RoomNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public int? Pax { get; set; }
        public decimal? Tariff { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountedTariff { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? Vat { get; set; }
        public decimal? BasicRoomRent { get; set; }
    }
}
