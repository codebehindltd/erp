using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class PMSalesDetailBO
    {
        public int DetailId { get; set; }
        public int SalesId { get; set; }
        public string ServiceType { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal TotalPrice { get; set; }
        public int FieldId1 { get; set; }
        public decimal UnitPrice1 { get; set; }
        public int FieldId2 { get; set; }
        public decimal UnitPrice2 { get; set; }

        public int SellingLocalCurrencyId { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public int SellingUsdCurrencyId { get; set; }
        public decimal UnitPriceUsd { get; set; }

        public int IsSeparateSalesInventory { get; set; }
        public string SerialNumber { get; set; }
    }
}
