using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class SetupSearchBO
    {
        public int? DivisionId { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public string DivisionName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? ThanaId { get; set; }
        public string ThanaName { get; set; }
    }
}
