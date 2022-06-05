using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpSalaryProcessDetailBO
    {  
        public int ProcessDetailId { get; set; }
        public int ProcessId { get; set; }
        public int EmpId { get; set; }
        public decimal BasicAmount { get; set; }
        public int SalaryHeadId { get; set; }
        public string SalaryHead { get; set; }

        public string TransactionType { get; set; }
        public string DependsOn { get; set; }

        public string SalaryType { get; set; }
        public string AmountType { get; set; }
        public decimal Amount { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal HomeTakenAmount { get; set; }
        public string SalaryHeadNote { get; set; }

        public bool IsBonusPaid { get; set; }
    }
}
