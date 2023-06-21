using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models
{
    public class CustomFieldBO
    {
        public int FieldId { get; set; }
        public string FieldValue { get; set; }
        public string FieldType { get; set; }
        public string Description { get; set; }
        public bool ActiveStat { get; set; }
        public DateTime TransactionDate { get; set; }



    }
}
