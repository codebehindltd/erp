using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBillReSettlementLogReportBO
    {
        public long ResettlementHistoryId { get; set; }
        public int BillId { get; set; }
        public string BillNumber { get; set; }
        public int KotId { get; set; }
        public System.DateTime ResettlementDate { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public int CreatedBy { get; set; }
        public string UserName { get; set; }
        public decimal CalculatedDiscountAmount { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}
