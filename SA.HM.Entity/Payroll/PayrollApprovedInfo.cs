using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollApprovedInfo
    {
        public int ApprovedId { get; set; }
        public int DealId { get; set; }
        public string ApprovedType { get; set; }
        public int UserInfoId { get; set; }
    }
}
