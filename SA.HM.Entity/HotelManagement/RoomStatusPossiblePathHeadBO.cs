using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomStatusPossiblePathHeadBO
    {
        public int PathId { get; set; }
        public string PossiblePath { get; set; }
        public string DisplayText { get; set; }
        public bool ActiveStat { get; set; }
    }
}
