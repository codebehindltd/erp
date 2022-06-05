using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class SMContactDetailsBO
    {
        public long DetailsId { get; set; }
        public string ParentType { get; set; }
        public long? ParentId { get; set; }
        public string TransectionType { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
