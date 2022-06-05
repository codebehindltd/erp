using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomStatusPossiblePathBO
    {
        public int MappingId { get; set; }
        public int UserGroupId { get; set; }
        public string PossiblePathType { get; set; }
        public int PathId { get; set; }
        public string DisplayText { get; set; }
        public int DisplayOrder { get; set; }
    }
}
