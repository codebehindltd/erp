using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.RetailPOS
{
    public class RetailPosBillReturnBO
    {
        public List<RetailPosBillWithSalesReturnBO> PosBillWithSalesReturn { get; set; }
        public List<RetailPosSalesReturnItemBO> PosSalesReturnItem { get; set; }
        public List<RetailPosSalesPaymentBO> PosSalesReturnPayment { get; set; }
    }
}




