using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SupportAndTicket
{
    public class STSupportPriceMatrixSetupBO
    {
        public long Id { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string Company { get; set; }
        public int ItemId { get; set; }
        public string Item { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string UnitHead { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
