using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestHouseInfoForReportBO
    {
        public string ReportName { get; set; }
        public string RRNumber { get; set; }
        public string GuestName { get; set; }
        public string GuestNationality { get; set; }
        public string GuestPassportNumber { get; set; }
        public string GuestCompany { get; set; }
        public long? CompanyId { get; set; }
        public int? TotalPerson { get; set; }
        public string ProbableArrivalTime { get; set; }
        public string DateIn { get; set; }
        public string DateOut { get; set; }
        public string RoomType { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomNumberOrPickInfo { get; set; }
        public string ReportTable { get; set; }
        public string ReportType { get; set; }
        public string ReservedBy { get; set; }
        public string PaymentMode { get; set; }
        public string GuestReferance { get; set; }
        public int? Quantity { get; set; }
        public string RoomNumberList { get; set; }
        public int CurrencyType { get; set; }
        public string CurrencyHead { get; set; }
        public decimal RoomRate { get; set; }        
        public string ArrivalFlightName { get; set; }
        public string ArrivalFlightNumber { get; set; }
        public string ArrivalTimeString { get; set; }
        public string UserName { get; set; }
        public string CheckOutBy { get; set; }
        public string Remarks { get; set; }
        public string AirportPickUp { get; set; }
        public string AirportDrop { get; set; }
        public string GroupName { get; set; }
        public int? VipGuestTypeId { get; set; }
        public string BlockCode { get; set; }
        public string MktCode { get; set; }
        public int? NumberOfPersonAdult { get; set; }
        public int? NumberOfPersonChild { get; set; }
        public string MealPlan { get; set; }
        public int IsStopChargePosting { get; set; }
        public string Email { get; set; }
        public string BillNo { get; set; }
        public string LocalCurrencyHead { get; set; }
        public decimal TotalRoomRateUSD { get; set; }
        public decimal TotalRoomRateLocalCurrency { get; set; }
    }
}
