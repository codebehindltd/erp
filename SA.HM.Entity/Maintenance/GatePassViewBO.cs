using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Maintenance
{
    public class GatePassViewBO
    {
        public GatePassBO GatePass { get; set; }
        public List<GatePassItemBO> GatePassDetails { get; set; }
    }
}
