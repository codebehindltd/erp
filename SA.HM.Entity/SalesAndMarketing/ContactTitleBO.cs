using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public  class ContactTitleBO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string TransectionType { get; set; }
        public bool Status { get; set; }
        public string ActiveStatus { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
