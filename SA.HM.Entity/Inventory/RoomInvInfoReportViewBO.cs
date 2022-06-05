using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class RoomInvInfoReportViewBO
    {
        public int InventoryId { get; set; }
        public DateTime TransectionDate { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCategory { get; set; }
        public decimal Quantity { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string Remarks { get; set; }
    }
}
