using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpTrainingDetailViewBO
    {
        public int TrainingDetailId { get; set; }
        public int TrainingId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpDepartment { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpBranch { get; set; }
        public string EmpEmail { get; set; }
    }
}
