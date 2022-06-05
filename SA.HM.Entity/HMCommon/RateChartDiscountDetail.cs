namespace HotelManagement.Entity.HMCommon
{
    using System;
    using System.Collections.Generic;
    
    public partial class RateChartDiscountDetail
    {
        public long Id { get; set; }
        public long? RateChartDetailId { get; set; }
        public long? OutLetId { get; set; }
        public string OutLetName { get; set; }
        public string Type { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountAmountUSD { get; set; }
        public decimal? OfferredPrice { get; set; }
    }
}
