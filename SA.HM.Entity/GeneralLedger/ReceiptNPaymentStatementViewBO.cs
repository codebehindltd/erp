using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class ReceiptNPaymentStatementViewBO
    {
        public List<ReceiptNPaymentStatementBO> ReceiptStatement = new List<ReceiptNPaymentStatementBO>();
        public List<ReceiptNPaymentStatementBO> PaymentStatement = new List<ReceiptNPaymentStatementBO>();
    }
}
