using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomFeaturesInfoBO
    {
        public long Id { get; set; }
        public long RoomId { get; set; }
        public long FeaturesId { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
