using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class DailySalesStatementBO
    {
        public Nullable<int> BillNo { get; set; }
        public string BillNumber { get; set; }
        public Nullable<int> Kot { get; set; }
        public Nullable<int> Pax { get; set; }

        public Nullable<decimal> TotalSales { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> AfterDiscountAmount { get; set; }
        public Nullable<decimal> NetSalesAmount { get; set; }

        public string VatPercentage { get; set; }
        public string ServiceChargePercentage { get; set; }
        public string CitySDChargePercentage { get; set; }
        public string AdditionalChargePercentage { get; set; }

        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<decimal> RoundedAmount { get; set; }
        public Nullable<decimal> RoundedGrandTotal { get; set; }

        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> CitySDCharge { get; set; }
        public Nullable<decimal> AdditionalCharge { get; set; }
        
        public Nullable<int> CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterType { get; set; }
        public string ItemType { get; set; }
        public Nullable<decimal> ItemValue { get; set; }
        public string Remarks { get; set; }
        public string CostCenters { get; set; }
        public int ItemRank { get; set; }
        public string UserName { get; set; }

        //BillNo	Kot	Pax	TotalSales	DiscountAmount	AfterDiscountAmount	NetSalesAmount	VatPercentage	
        //ServiceChargePercentage	GrandTotal	CostCenterId	CostCenter	ItemType	ItemValue	Remarks	ItemRank

    }
}
