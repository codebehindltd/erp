using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Entity.HMCommon
{
    public class GridData
    {
        public List<EmployeeYearlyLeaveBO> EmployeeLeave { get; set; }
        public GridPaging Pages { get; set; }
    }
}
