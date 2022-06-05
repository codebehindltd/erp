using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
   public class DayDA
    {

       public List<DayBO> GetDayList()
       {

           List<DayBO> dayList = new List<DayBO>();
           DayBO sun = new DayBO();
           sun.Id = 7;
           sun.Name = "Sunday";
           DayBO mon = new DayBO();
           mon.Id = 1;
           mon.Name = "Monday";
           DayBO tue = new DayBO();
           tue.Id = 2;
           tue.Name = "Tuesday";
           DayBO wed = new DayBO();
           wed.Id = 3;
           wed.Name = "Wednesday";
           DayBO thu = new DayBO();
           thu.Id = 4;
           thu.Name = "Thursday";
           DayBO fri = new DayBO();
           fri.Id = 5;
           fri.Name = "Friday";
           DayBO sat = new DayBO();
           sat.Id = 6;
           sat.Name = "Saturday";

           dayList.Add(sun);
           dayList.Add(mon);
           dayList.Add(tue);
           dayList.Add(wed);
           dayList.Add(thu);
           dayList.Add(fri);
           dayList.Add(sat);
           return dayList;
       }

    }
}
