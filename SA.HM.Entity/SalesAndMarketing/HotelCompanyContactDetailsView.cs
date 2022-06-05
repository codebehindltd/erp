using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class HotelCompanyContactDetailsView
    {
        public List<HotelCompanyContactDetailsBO> Eamils = new List<HotelCompanyContactDetailsBO>();
        public List<HotelCompanyContactDetailsBO> Phones = new List<HotelCompanyContactDetailsBO>();
        public List<HotelCompanyContactDetailsBO> Faxs = new List<HotelCompanyContactDetailsBO>();
        public List<HotelCompanyContactDetailsBO> Webs = new List<HotelCompanyContactDetailsBO>();
    }
}
