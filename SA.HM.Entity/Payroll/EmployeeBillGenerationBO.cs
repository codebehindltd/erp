using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeBillGenerationBO
    {
        public long EmployeeBillId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime BillDate { get; set; }
        public string EmployeeBillNumber { get; set; }
        public int BillCurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string Remarks { get; set; }
        public string ApprovedStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public string EmployeeName { get; set; }
    }
}
