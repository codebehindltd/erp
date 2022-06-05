using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ManagerReportBO
    {
        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public decimal? Covers { get; set; }
        public decimal? MtdCovers { get; set; }
        public decimal? YtdCovers { get; set; }
        public int? OrderByNumber { get; set; }
    }
}
