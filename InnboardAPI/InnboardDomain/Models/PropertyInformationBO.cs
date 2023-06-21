using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models
{
    public class PropertyInformationBO
    {
        public long Id { get; set; }
        public string PropertyName { get; set; }
        public string PropertyAddress { get; set; }
        public string WebAddress { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string EndPointIp { get; set; }
        public string LogoURL { get; set; }
        public string RankNumber { get; set; }
        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public string LineThree { get; set; }
        public string LineFour { get; set; }
        public string LineFive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
