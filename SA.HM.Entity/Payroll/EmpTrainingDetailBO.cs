using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpTrainingDetailBO
    {
        public int TrainingDetailId { get; set; }
        public int TrainingId { get; set; }
        public int EmpId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string EmpName { get; set; }
    }
}
