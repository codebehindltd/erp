using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLVoucherApprovedInfoBO
    {
        public int ApprovedId { get; set; }
        public long? DealId { get; set; }
        public string ApprovedType { get; set; }
        public int? UserInfoId { get; set; }
    }
}
