using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CallToActionDetailBO
    {
        public long Id { get; set; }
        public long? CallToActionId { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public string OtherActivities { get; set; }
        public string Description { get; set; }
        public string TaskAssignedEmployee { get; set; }
        public string PerticipentFromOffice { get; set; }
        public string PerticipentFromClient { get; set; }
        public string TaskAssignedEmployeeName { get; set; }
        public string PerticipentFromOfficeName { get; set; }
        public string PerticipentFromClientName { get; set; }
        public string ReminderDayList { get; set; }
        public int TempId { get; set; }
        public long? ContactId { get; set; }
        public long? CompanyId { get; set; }
        public long? TaskId { get; set; }
        public string TaskName { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
    }
}
