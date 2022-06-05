using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class BestEmployeeNominationBO
    {
        public long BestEmpNomineeId { get; set; }
        public int DepartmentId { get; set; }
        public short Years { get; set; }
        public byte Months { get; set; }
        public string ApprovedStatus { get; set; }
        public string Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
