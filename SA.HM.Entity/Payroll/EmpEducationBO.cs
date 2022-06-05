using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Payroll
{
    public class EmpEducationBO
    {
        public int EducationId { get; set; }
        public int EmpId { get; set; }
        public int LevelId { get; set; }
        public string EmployeeName { get; set; }
        public string ExamName { get; set; }
        public string InstituteName { get; set; }
        public string PassYear { get; set; }
        public string SubjectName { get; set; }
        public string PassClass { get; set; }
    }
}
