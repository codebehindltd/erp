using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CountriesBO
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Nationality { get; set; }
        public string Code2Digit { get; set; }
        public string Code3Digit { get; set; }
        public string CodeNumeric { get; set; }
    }
}
