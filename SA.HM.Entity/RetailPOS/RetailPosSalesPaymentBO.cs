using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.RetailPOS
{
    public class RetailPosSalesPaymentBO
    {
        public int PaymentId { get; set; }
        public Nullable<int> BillId { get; set; }
        public string PayMode { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
    }
}
