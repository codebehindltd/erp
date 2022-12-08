using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvNutrientInfoBO
    {
        public long NutrientId { get; set; }
        public long NutritionTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public Boolean ActiveStat { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int SetupTypeId { get; set; }
        public Boolean IsEdit { get; set; }
    }
}
