using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.CostofGoodsSold
{
    public class CgsMonthlyTransactionBO
    {
        public int MonthlyTransactionId { get; set; }
        public int EmpId { get; set; }
        public string EmpType { get; set; }
        public DateTime InputDate { get; set; }
        public decimal Amount { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
