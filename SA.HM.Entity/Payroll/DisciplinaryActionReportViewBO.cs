using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class DisciplinaryActionReportViewBO
    {
        public int? ProposedActionId { get; set; }
        public DateTime ApplicableDate { get; set; }
        public string ApplicableDateForReport { get; set; }
        public string DisplayName { get; set; }
        public string ActionName { get; set; }
        public string ActionReason { get; set; }
        public string ProposedAction { get; set; }
        public string ActionBody { get; set; }
    }
}
