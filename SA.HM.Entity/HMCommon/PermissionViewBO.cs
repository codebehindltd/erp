using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class PermissionViewBO
    {
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanCheck { get; set; }
        public bool IsCanApprove { get; set; }
    }
}
