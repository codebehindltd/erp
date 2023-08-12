using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models
{
    public class GuestOrMemberProfileBO
    {
        public int ProfileId { get; set; }
        public string ProfileNumber { get; set; }
        public string GuestName { get; set; }
        public string GuestPhone { get; set; }
        public string GuestNationality { get; set; }
        public string NationalId { get; set; }
        public string PassportNumber { get; set; }
        public string GuestEmail { get; set; }
        public string GuestAddress { get; set; }
        public string CountryName { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
    }
}
