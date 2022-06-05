using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CallToActionBO
    {
        public long Id { get; set; }
        public long? MasterId { get; set; }
        public string FromCallToAction { get; set; }
        public long? ContactId { get; set; }
        public string CompanyName { get; set; }
        public long? CompanyId { get; set; }
        public string ContactName { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String CreatedDateString { get; set; }
        public String CreatedTimeString { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
