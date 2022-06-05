using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpNomineeBO
    {
        public int NomineeId { get; set; }
        public int EmpId { get; set; }
        public string NomineeName { get; set; }
        public string Relationship { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public string ShowDateOfBirth { get; set; }

    }
}
