using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class DealImpFeedbackBO
    {
        
        public string ImpEngineerName { get; set; }
        public long? ImpEngineerId { get; set; }
        public long? DealId { get; set; }
        public long? Id { get; set; }

        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
