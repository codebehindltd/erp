using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CommonCurrencyTransactionBO
    {
        public int CurrencyConversionId { get; set; }
        public string TransactionNumber { get; set; }
        public int FromConversionHeadId { get; set; }
        public string FromConversionHead { get; set; }
        public int ToConversionHeadId { get; set; }
        public string ToConversionHead { get; set; }
        public decimal ConversionAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal ConvertedAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
