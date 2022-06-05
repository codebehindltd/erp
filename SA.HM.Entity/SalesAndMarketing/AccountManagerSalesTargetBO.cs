using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class AccountManagerSalesTargetBO
    {
        public Int64 TargetId { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public Int64 FiscalYearId { get; set; }
        public Int64? CheckedBy { get; set; }       
        public Int64? ApprovedBy { get; set; }
        public string ApprovedStatus { get; set; }        
        public Int64 CreatedBy { get; set; }
        public Int64? LastModifiedBy { get; set; }
        public string CostCenter { get; set; }
        public string FiscalYearName { get; set; }
    }
}
