using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmloyeeGratuityBO
    {
        public int GratuityId { get; set; }
        public int EmpId { get; set; }
        public decimal GratuityAmount { get; set; }
        public int NumberOfGratuity { get; set; }
        public System.DateTime GratuityDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
