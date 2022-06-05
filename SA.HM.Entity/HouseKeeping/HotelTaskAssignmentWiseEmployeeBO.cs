using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HotelTaskAssignmentWiseEmployee
    {
        public long TaskAssignEmployeeListId { get; set; }
        public long TaskId { get; set; }
        public int EmpId { get; set; }       
    }
}
