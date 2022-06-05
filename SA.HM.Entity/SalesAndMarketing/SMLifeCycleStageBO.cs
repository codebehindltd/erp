using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMLifeCycleStageBO
    {
        public long Id { get; set; }
        public string LifeCycleStage { get; set; }
        public Nullable<int> DisplaySequence { get; set; }
        public string Description { get; set; }
        public bool IsRelatedToDeal { get; set; }
        public string DealType { get; set; }
        public string ForcastType { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
