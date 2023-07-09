using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GenerateGuestBillReportBO
    {
        public string HMCompanyProfile { get; set; }
        public string HMCompanyAddress { get; set; }
        public string HMCompanyWeb { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public string ReservationNumber { get; set; }
        public string BillNumber { get; set; }
        public string GuestName { get; set; }
        public int? TotalPerson { get; set; }
        public string GuestAddress { get; set; }
        public string ArriveDate { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int IsDiscountApplicableOnRackRate { get; set; }
        public string LocalCurrencyHead { get; set; }
        public decimal? LocalCurrencyUnitPrice { get; set; }
        public decimal? LocalCurrencyRoomRate { get; set; }
        public string CurrencyHead { get; set; }        
        public decimal? UnitPrice { get; set; }
        public decimal? RoomRate { get; set; }
        public string ExpectedCheckOutDate { get; set; }
        public string CheckOutDate { get; set; }
        public Nullable<int> IsBillSplited { get; set; }
        public string PrintDate { get; set; }
        public string PrintedBy { get; set; }
        public int Night { get; set; }

        public bool IsServiceChargeEnable { get; set; }
        public bool IsCityChargeEnable { get; set; }
        public bool IsVatAmountEnable { get; set; }
    }
}
