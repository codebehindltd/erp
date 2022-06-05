namespace HotelManagement.Entity.HMCommon
{
    using System;
    using System.Collections.Generic;
    
    public partial class RateChartMaster
    {
        public long Id { get; set; }
        public string PromotionName { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime EffectFrom { get; set; }
        public DateTime EffectTo { get; set; }
        public string RateChartFor { get; set; }
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
