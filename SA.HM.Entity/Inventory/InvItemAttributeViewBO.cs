using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemAttributeViewBO
    {
        public List<InvItemAttributeBO> InvItemAttributeBOList { get; set; }
        public List<string> InvItemAttributeSetupTypeList { get; set; }
        
    }
}
