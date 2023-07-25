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
        public string TransactionType { get; set; }
        public int RegistrationId { get; set; }
        public string RoomNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string CountryName { get; set; }
        public string PassportNumber { get; set; }
        public string TransactionDetails { get; set; }
        public string UserName { get; set; }
        public string CreatedDateString { get; set; }
    }
}
