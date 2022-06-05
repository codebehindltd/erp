
using System;
using System.Collections.Generic;

namespace HotelManagement.Entity.HMCommon
{

    public class RateChartDetail
    {
        public long Id { get; set; }
        public long RateChartMasterId { get; set; }
        public string ServiceType { get; set; }
        public int CategoryId { get; set; }
        public int? ServicePackageId { get; set; }
        public string PackageName { get; set; }
        public int Uplink { get; set; }
        public int Downlink { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int StockBy { get; set; }
        public string UnitHead { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountAmountUSD { get; set; }
        public bool IsDiscountForAll { get; set; }
        public int? ServiceTypeId { get; set; }
        public List<RateChartDiscountDetail> RateChartDiscountDetails { get; set; }

        public RateChartDetail()
        {
            RateChartDiscountDetails = new List<RateChartDiscountDetail>();
        }
    }
}
