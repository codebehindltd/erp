using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SupportAndTicket
{
    public class STSupportNCaseSetupInfoBO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SetupType { get; set; }
        public string SetupTypeDisplay { get; set; }
        public bool Status { get; set; }
        public int? PriorityLabel { get; set; }
        public bool? IsDeclineStage { get; set; }
        public bool? IsCloseStage { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
