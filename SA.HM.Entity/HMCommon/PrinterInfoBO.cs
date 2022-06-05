using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class PrinterInfoBO
    {
        public int PrinterInfoId { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public string StockType { get; set; }
        public int KitchenId { get; set; }
        public string KitchenOrStockName { get; set; }
        public string PrinterName { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public bool PrintFlag { get; set; }
        public bool IsChanged { get; set; }
        public string DefaultView { get; set; }
    }
}
