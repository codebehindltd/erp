using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class TaxSettingBO
    {
        public int TaxSettingId { get; set; }
        public decimal TaxBandForMale { get; set; }
        public decimal TaxBandForFemale { get; set; }
        public Nullable<bool> IsTaxPaidByCompany { get; set; }
        public string CompanyContributionType { get; set; }
        public Nullable<decimal> CompanyContributionAmount { get; set; }
        public Nullable<bool> IsTaxDeductFromSalary { get; set; }
        public string EmployeeContributionType { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
