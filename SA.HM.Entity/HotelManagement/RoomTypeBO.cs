using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity
{
    public class RoomTypeBO
    {
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public string TypeCode { get; set; }
        public string LocalCurrencyHead { get; set; }
        public decimal RoomRate { get; set; }
        public decimal RoomRateUSD { get; set; }
        public decimal? MinimumRoomRate { get; set; }
        public decimal? MinimumRoomRateUSD { get; set; }
        public Boolean ActiveStat { get; set; }
        public Boolean SuiteType { get; set; }
        public string ActiveStatus { get; set; }
        public int PaxQuantity { get; set; }
        public int ChildQuantity { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int TotalRoom { get; set; }
        public int OccupiedRoomCount { get; set; }
        public int AvailableRoomCount { get; set; }
        public Int64 AccountsPostingHeadId { get; set; }
    }
}
