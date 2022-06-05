using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpTaxDeductionBO
    {
        public long TaxCollectionId { get; set; }
        public int EmpId { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal DeductionPercentage { get; set; }
        public decimal? RangeFrom { get; set; }
        public decimal? RangeTo { get; set; }
        public decimal TaxAmount { get; set; }
        public DateTime TaxDateFrom { get; set; }
        public DateTime TaxDateTo { get; set; }
        public Int16? TaxYear { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string Department { get; set; }
    }
}
