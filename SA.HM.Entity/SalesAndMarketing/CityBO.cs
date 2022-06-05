using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CityBO
    {
        public long CityId { get; set; }
        public long CountryId { get; set; }
        public string Country { get; set; }
        public long StateId { get; set; }
        public string State { get; set; }
        public string CityName { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
