using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpBonusBO
    {
        public int BonusSettingId { get; set; }
        public string BonusType { get; set; }
        public byte? EffectivePeriod { get; set; }
        public DateTime? BonusDate { get; set; }
        public decimal BonusAmount { get; set; }
        public string AmountType { get; set; }
        public int DependsOnHead { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string ViewBonusDate { get; set; }
    }
}
