using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class SalesServiceBundleBO
    {
        public int BundleId { get; set; }
        public string BundleName { get; set; }
        public string BundleCode { get; set; }
        public string Frequency { get; set; }

        public int SellingPriceLocal { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public int SellingPriceUsd { get; set; }
        public decimal UnitPriceUsd { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
