using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CommonCurrencyBO
    {
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyType { get; set; }
        public bool ActivaStat { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedByDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedByDate { get; set; }
        public string ActiveStatus { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
