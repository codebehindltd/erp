using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeTaxDeductionSettingBO
    {
        public int TaxDeductionId { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
        public decimal DeductionPercentage { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
