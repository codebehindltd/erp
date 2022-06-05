using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollEmpStatusHistoryBO
    {
        public long Id { get; set; }
        public long EmpId { get; set; }
        public int EmpStatusId { get; set; }
        public DateTime ActionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Reason { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
