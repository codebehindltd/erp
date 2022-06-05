using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
   public class HMDayCloseBO
    {

        public DateTime DayClossingDate { get; set; }
        public int DayClossingModuleId { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
