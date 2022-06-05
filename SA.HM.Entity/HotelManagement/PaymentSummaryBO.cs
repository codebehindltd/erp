using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class PaymentSummaryBO
    {
        public int RegistrationId { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal TotalPayment { get; set; }
    }
}
