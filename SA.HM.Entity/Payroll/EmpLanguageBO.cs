using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpLanguageBO
    {
        public int LanguageId { get; set; }
        public int EmpId { get; set; }
        public string Language { get; set; }
        public string Reading { get; set; }
        public string Writing { get; set; }
        public string Speaking { get; set; }

        public string ReadingLevel { get; set; }
        public string WritingLevel { get; set; }
        public string SpeakingLevel { get; set; }
    }
}
