using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CallToActionDetailViewBO: CallToActionDetailBO
    {
        public List<long> PerticipentFromOfficeList { get; set; }
        public List<long> PerticipentFromClientList { get; set; }
        public List<long> TaskAssignedEmployeetList { get; set; }
        public List<long> ReminderList { get; set; }
    }
}
