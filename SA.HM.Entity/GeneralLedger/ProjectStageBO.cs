using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class ProjectStageBO
    {
        public int Id { get; set; }
        public string ProjectStage { get; set; }
        public Nullable<decimal> Complete { get; set; }
        public Nullable<int> DisplaySequence { get; set; }
        public string Description { get; set; }
        public bool IsFinalStage { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
