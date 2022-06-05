using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeStatusBO
    {
        public int EmployeeStatusId { get; set; }
        public string EmployeeStatus { get; set; }       
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
