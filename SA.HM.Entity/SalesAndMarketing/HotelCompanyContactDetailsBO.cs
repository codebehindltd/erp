using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class HotelCompanyContactDetailsBO
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public Int64? TransactionId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }

        
    }
}
