using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMTaskAssignmentView
    {
        public long Id { get; set; }
        public string TaskName { get; set; }
        public DateTime DueDateTime { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public string AssigneeName { get; set; }
        public string AssignToName { get; set; }
        public string AssignToEmailAddress { get; set; }
        public bool IsCompleted { get; set; }
    }
}
