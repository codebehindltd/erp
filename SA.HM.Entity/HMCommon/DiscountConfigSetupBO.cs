using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class DiscountConfigSetupBO
    {
        public long ConfigurationId { get; set; }
        public bool? IsDiscountApplicableIndividually { get; set; }
        public bool? IsDiscountApplicableMaxOneWhenMultiple { get; set; }
        public bool? IsDiscountOptionShowsWhenMultiple { get; set; }
        public bool? IsDiscountAndMembershipDiscountApplicableTogether { get; set; }
        public bool? IsBankDiscount { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
