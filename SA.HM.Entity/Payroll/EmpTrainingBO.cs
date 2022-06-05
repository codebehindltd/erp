using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpTrainingBO
    {
        public int TrainingId { get; set; }
        public string Trainer { get; set; }
        public int TrainingTypeId { get; set; }
        public string TrainingName { get; set; }
        public int OrganizerId { get; set; }
        public string Organizer { get; set; }
        public DateTime StartDate { get; set; }
        public string AttendeeList { get; set; }
        public string EmpEmail { get; set; }
        public string Location { get; set; }        
        public bool? Reminder { get; set; }
        public int? ReminderHour { get; set; }
        public string Note { get; set; }
        public DateTime EndDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }
        public string Discussed { get; set; }
        public string CallToAction { get; set; }
        public string Conclusion { get; set; }
    }
}
