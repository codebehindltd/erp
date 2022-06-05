using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomFeaturesBO
    {
        public long Id { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public bool ActiveStatus { get; set; }
        public string ActiveStatusSt { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
