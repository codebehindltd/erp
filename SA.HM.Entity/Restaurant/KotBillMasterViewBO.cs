using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class KotBillMasterViewBO
    {
        public int KotId { get; set; }
        public Nullable<System.DateTime> KotDate { get; set; }
        public Nullable<int> BearerId { get; set; }
        public Nullable<int> CostCenterId { get; set; }
        public string SourceName { get; set; }
        public Nullable<int> SourceId { get; set; }
        public Nullable<int> PaxQuantity { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<bool> IsBillProcessed { get; set; }
        public string KotStatus { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string TokenNumber { get; set; }
        public Nullable<bool> IsBillHoldup { get; set; }

        public string CategoryList { get; set; }
    }
}
