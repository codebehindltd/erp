using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpPayScaleBO   
    {
        public int PayScaleId { get; set; }
        public DateTime ScaleDate { get; set; }
        public string Date { get; set;}
        public int GradeId { get; set; }
        public string Grade { get; set; }
        public decimal BasicAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
