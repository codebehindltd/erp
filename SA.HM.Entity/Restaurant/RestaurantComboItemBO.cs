using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantComboItemBO
    {
        public int ComboId { get; set; }
        public int RandomComboId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CostCenterId { get; set; }
        public string ComboName { get; set; }
        public decimal ComboPrice { get; set; }
        public string Code { get; set; }
        public string ImageName { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
