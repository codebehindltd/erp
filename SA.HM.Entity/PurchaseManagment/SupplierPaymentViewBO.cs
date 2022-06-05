using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class SupplierPaymentViewBO
    {
        public SupplierPaymentBO SupplierPayment { get; set; }
        public List<SupplierPaymentDetailsBO> SupplierPaymentDetails { get; set; }
        public List<PMSupplierPaymentLedgerBO> PaymentDetailsInfo = new List<PMSupplierPaymentLedgerBO>();
        public List<PMSupplierPaymentLedgerBO> SupplierBill = new List<PMSupplierPaymentLedgerBO>();
        public PMSupplierBO Supplier = new PMSupplierBO();
    }
}
