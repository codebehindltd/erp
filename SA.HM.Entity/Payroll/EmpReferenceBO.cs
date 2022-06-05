using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpReferenceBO
    {
        public int ReferenceId { get; set; }
        public int? EmpId { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }

        public string Relationship { get; set; }

        public string Description { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Relation { get; set; }

        public long RowRank { get; set; }       
    }
}
