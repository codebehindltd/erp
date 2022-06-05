using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollEmployeeBalanceTransferBO
    {
        public long Id { get; set; }
        public Nullable<long> TransferFrom { get; set; }
        public string TransferFromEmp { get; set; }
        public Nullable<long> TransferTo { get; set; }
        public string TransferToEmp { get; set; }
        public Nullable<decimal> TransferAmount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> CheckedBy { get; set; }
        public Nullable<System.DateTime> CheckedDate { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public string CheckedByUsers { get; set; }
        public string ApprovedByUsers { get; set; }

        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
    }
}
