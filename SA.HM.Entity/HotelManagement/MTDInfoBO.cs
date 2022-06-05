using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class MTDInfoBO
    {
        public long MTDID { get; set; }
        public System.DateTime MTDDate { get; set; }
        public Nullable<decimal> ActualRoomsOccupied { get; set; }
        public Nullable<decimal> Occupency { get; set; }
        public Nullable<decimal> ActualRoomsRevenue { get; set; }
        public Nullable<decimal> AverageRate { get; set; }
        public Nullable<decimal> RevenuePerRoom { get; set; }
        public Nullable<decimal> MTDAVGRoomsOccupancy { get; set; }
        public Nullable<decimal> MTDRoomsAverageRevenue { get; set; }
        public Nullable<decimal> MTDAverageRate { get; set; }
        public Nullable<decimal> MTDRevenuePerRoom { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
