using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpSalaryProcessBO
    {
        public int ProcessId { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime SalaryDateFrom { get; set; }
        public DateTime SalaryDateTo { get; set; }
        public short SalaryYear { get; set; }
        public byte ProcessSequence { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
