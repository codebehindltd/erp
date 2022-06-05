using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class TermsNConditionsMasterBO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int? DisplaySequence { get; set; }
        public string Description { get; set; }
        public List<long> Details { get; set; }
    }
}
