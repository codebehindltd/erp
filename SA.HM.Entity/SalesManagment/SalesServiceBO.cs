using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class SalesServiceBO
    {

        public int ServiceId { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public decimal PurchasePrice { get; set; }
        public int SellingLocalCurrencyId { get; set; }
        public int SellingUsdCurrencyId { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }

        public string Frequency { get; set; }

        public int BandwidthType { get; set; }
        public int Bandwidth { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
