using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class QuotationAndSalesTransferDetailsViewBO
    {
        public long QuotationDetailsId { get; set; }
        public long SalesTransferId { get; set; }
        public string CompanyName { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string HeadName { get; set; }
        public decimal QuotationQuantity { get; set; }
        public decimal SalesQuantity { get; set; }
        public int ItemId { get; set; }
        public int StockBy { get; set; }
        public long SalesTransferDetailId { get; set; }
        public string CostCenterList { get; set; }
        public string ProductType { get; set; }
    }
}
