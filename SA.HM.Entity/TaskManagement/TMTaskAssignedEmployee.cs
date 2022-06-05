using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.TaskManagement
{
    public class TMTaskAssignedEmployee
    {
        public long Id { get; set; }
        public Nullable<long> TaskId { get; set; }
        public long EmployeeId { get; set; }
    }
}
