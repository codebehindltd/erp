using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.TaskManagement
{
    public class SMTaskFeedbackBO
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public int EmployeeId { get; set; }
        public string ImplementationStatus { get; set; }
        public int TaskStage { get; set; }
        public string TaskFeedback { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime FinishTime { get; set; }
        public string MeetingAgenda { get; set; }
        public string MeetingLocation { get; set; }
        public string MeetingDiscussion { get; set; }
        public string CallToAction { get; set; }
    }
}
