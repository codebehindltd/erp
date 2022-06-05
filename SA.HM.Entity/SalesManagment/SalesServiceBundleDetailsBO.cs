using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class SalesServiceBundleDetailsBO
    {
        public int DetailsId { get; set; }
        public int BundleId { get; set; }
        public string IsProductOrService { get; set; }
        public string ItemName { get; set; }
        public int ProductId { get; set; }
        public int ServiceId { get; set; }
        public string ProductOrServiceName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int SellingPriceLocal { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public int SellingPriceUsd { get; set; }
        public decimal UnitPriceUsd { get; set; }

        public decimal TotalUnitPriceLocal { get; set; }
        public decimal TotalUnitPriceUsd { get; set; }
       


    }
}
