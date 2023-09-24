using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class SalaryHeadBO
    {
        public int SalaryHeadId { get; set; }
        public string SalaryHead { get; set; }        
        public string SalaryType { get; set; }
        public string ContributionType { get; set; }
        public string VoucherMode { get; set; }
        public long NodeId { get; set; }
        public string TransactionType { get; set; }
        public DateTime? EffectedMonth { get; set; }
        public bool IsShowOnlyAllownaceDeductionPage { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
