using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
   public class AllowanceDeductionHeadBO
    {
        public int AllowDeductId { get; set; }
        public string AllowDeductName { get; set; }
        public string AllowDeductType { get; set; }
        public string TransactionType { get; set; }
        public bool ActiveStat { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string ActiveStatus { get; set; }
    }
}
