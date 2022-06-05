using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class HotelRoomInventoryDetailsBO
    {
        public int OutDetailsId { get; set; }
        public int InventoryOutId { get; set; }
        public int CostCenterId { get; set; }
        public int LocationId { get; set; }
        public int StockById { get; set; }        
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public int CategoryId { get; set; }
        
        public string ProductName { get; set; }
        public string CostCenter { get; set; }
        public string Location { get; set; }
        public string StockBy { get; set; }
    }
}
