using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMLogKeepingBO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime LogDateTime { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<long> ContactId { get; set; }
        public Nullable<long> DealId { get; set; }
        public Nullable<long> SalesCallEntryId { get; set; }
        public int CreatedBy { get; set; }
    }
}
