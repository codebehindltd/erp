using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestHouseServiceBO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int IncomeNodeId { get; set; }
        public string IncomeNodeHead { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public Boolean IsVatEnable { get; set; }
        public Boolean IsServiceChargeEnable { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
