using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMQuotationDetailsBO
    {
        public Int64 QuotationDetailsId { get; set; }
        public Int64 QuotationId { get; set; }
        public string ItemType { get; set; }
        public int CategoryId { get; set; }

        public int ServicePackageId { get; set; }
        public int ServiceBandWidthId { get; set; }
        public int ServiceTypeId { get; set; }

        public int ItemId { get; set; }
        public int StockBy { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }
        
        public string ItemName { get; set; }
        public string HeadName { get; set; }
        public decimal SalesQuantity { get; set; }
        public string Category { get; set; }

        public string PackageName { get; set; }
        public string BandWidthName { get; set; }
        //public int BandWidthValue { get; set; }
        public string ServiceType { get; set; }
        public long SalesTransferDetailId { get; set; }
        public int Uplink { get; set; }
        public int Downlink { get; set; }
        public string SalesNote { get; set; }

        public string CostCenterList { get; set; }

        public decimal? RemainingDeliveryQuantity { get; set; }
        public string ProductType { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal TransferedQuantity { get; set; }
        public decimal AverageCost { get; set; }
        public bool IsDiscountForAll { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountAmountUSD { get; set; }
        public List<SMQuotationDiscountDetails> QuotationDiscountDetails { get; set; }
    }
}
