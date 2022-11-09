using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CustomFieldBO
    {
        public int FieldId { get; set; }
        public string FieldValue { get; set; }
        public string FieldType { get; set; }        
        public string Description { get; set; }
        public bool ActiveStat { get; set; }
        public DateTime TransactionDate { get; set; }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}
