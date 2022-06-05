using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class LCInformationDetailBO
    {
        public long LCDetailId { get; set; }
        public long LCId { get; set; }
        public int POrderId { get; set; }
        public int CostCenterId { get; set; }
        public int StockById { get; set; }
        public int ProductId { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Quantity { get; set; }
        public string StockBy { get; set; }
        public string ItemName { get; set; }
        public decimal ItemTotal { get; set; }
    }
}
