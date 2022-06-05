using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpCareerTrainingBO
    {
        public int CareerTrainingId { get; set; }
        public int? EmpId { get; set; }
        public string TrainingTitle { get; set; }
        public string Topic { get; set; }
        public string Institute { get; set; }
        public int Country { get; set; }
        public string Location { get; set; }
        public string TrainingYear { get; set; }
        public int? Duration { get; set; }
        public string DurationType { get; set; }
        public string StartDate { get; set; }
    }
}
