using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class SupplierPaymentDetailsBO
    {
        public Int64 PaymentDetailsId { get; set; }
        public Int64 PaymentId { get; set; }
        public int BillId { get; set; }
        public decimal PaymentAmount { get; set; }
        public Int64 SupplierPaymentId { get; set; }
    }
}
