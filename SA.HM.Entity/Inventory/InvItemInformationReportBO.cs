using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemInformationReportBO
    {
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        public string StockBy { get; set; }
        public string AdjustmentFrequency { get; set; }
        public int ClassificationId { get; set; }
        public string Classification { get; set; }
        public string CostCenter { get; set; }
        public int CostCenterId { get; set; }
        public decimal? UnitPriceLocal { get; set; }
        public decimal? UnitPriceUsd { get; set; }
        public string ForeignCurrencyType { get; set; }
        public string LocalCurrencyType { get; set; }
    }
}
