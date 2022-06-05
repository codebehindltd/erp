using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class ItemViewBO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Code { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public string ProductType { get; set; }       
    }
}
