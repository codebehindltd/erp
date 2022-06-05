using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class ActivityLogDetailsBO
    {
        public long Id { get; set; }
        public long ActivityId { get; set; }
        public string FormName { get; set; }
        public string FieldName { get; set; }
        public string PreviousData { get; set; }
        public string CurrentData { get; set; }
        public string DetailDescription { get; set; }
        public bool IsSaveActivity { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
