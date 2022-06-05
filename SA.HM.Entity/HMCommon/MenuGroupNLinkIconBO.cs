using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class MenuGroupNLinkIconBO
    {
        public long IconId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Code { get; set; }        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
