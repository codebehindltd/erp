using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
   public class PayrollWorkingDayBO
   {
       public int WorkingDayId { get; set; }


       public int TypeId { get; set; }
       public string WorkingPlan { get; set; }

       public DateTime StartTime { get; set; }
       public DateTime EndTime { get; set; }


       public string DayOffOne { get; set; }
       public string DayOffTwo { get; set; }
       public string TypeName { get; set; }


       public int CreatedBy { get; set; }
       public DateTime CreatedDate { get; set; }
       public int LastModifiedBy { get; set; }
       public DateTime LastModifiedDate { get; set; }

   }
}
