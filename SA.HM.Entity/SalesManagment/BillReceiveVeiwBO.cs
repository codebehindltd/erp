using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class BillReceiveVeiwBO
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime BillFromDate { get; set; }
        public DateTime BillToDate { get; set; }
        public Decimal InvoiceAmount { get; set; }
        public Decimal DueOrAdvanceAmount { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public int FieldId { get; set; }


    }
}
