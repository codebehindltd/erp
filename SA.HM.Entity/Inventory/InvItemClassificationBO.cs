using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemClassificationBO
    {
        public Int64 ClassificationId { get; set; }
        public String ClassificationName { get; set; }
        public Boolean IsActive { get; set; }
        public string ActiveStatus { get; set; }


        public Int64 CreatedBy { get; set; }
        public Int64 LastModifiedBy { get; set; }

    }
}
