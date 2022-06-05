using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMCostAnalysisDetail
    {

        public long Id { get; set; }
        public long SMCostAnalysisId { get; set; }
        public string ItemType { get; set; }
        public int? CategoryId { get; set; }
        public int? ServicePackageId { get; set; }
        public int? ServiceBandWidthId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? StockBy { get; set; }
        public string StockByName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalOfferedPrice { get; set; }
        public int UpLink { get; set; }
        public decimal AverageCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal TotalProjetcedCost { get; set; }

    }
}
