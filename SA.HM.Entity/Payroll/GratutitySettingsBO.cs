using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class GratutitySettingsBO
    {
        public int GratuityId { get; set; }
        public int GratuityWillAffectAfterJobLengthInYear { get; set; }
        public Nullable<bool> IsGratuityBasedOnBasic { get; set; }
        public Nullable<decimal> GratutiyPercentage { get; set; }
        public int NumberOfGratuityAdded { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
