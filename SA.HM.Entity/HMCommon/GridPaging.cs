using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class GridPaging
    {
        public string NextButton { get; set; }
        public string PreviousButton { get; set; }
        public string Pagination { get; set; }
        public int CurrentPageNumber { get; set; }

        public int IsPreviousButtonVisible { get; set; }
        public int IsNextButtonVisible { get; set; }        
    }
}
