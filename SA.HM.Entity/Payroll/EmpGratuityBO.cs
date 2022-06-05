using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpGratuityBO
    {
        public int GratuityId { get; set; }
        public int EmpId { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal? GratuityAmount { get; set; }
        public int NumberOfGratuity { get; set; }
        public DateTime GratuityDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string EmpCode { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? GratuityEligibilityDate { get; set; }
        public int? ServiceYear { get; set; }

        public string ShowGratuityEligibilityDate { get; set; }               

    }
}
