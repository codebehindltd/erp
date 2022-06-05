using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class BusinessPromotionBO
    {
        public int BusinessPromotionId { get; set; }
        public string BPHead { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public string TransactionType { get; set; }
        public int TransactionId { get; set; }
        public Boolean IsBPPublic { get; set; }
        public decimal PercentAmount { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string BusinessPromotionIdNPercentAmount { get; set; }
    }
}
