using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetBillPaymentBO
    {
        public Int64 Id { get; set; }
        public Int64 ReservationId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmout { get; set; }
        public Int64 CreatedBy { get; set; }
        public Int64 LastModifiedBy { get; set; }
        public Int64 DealId { get; set; }
    }
}
