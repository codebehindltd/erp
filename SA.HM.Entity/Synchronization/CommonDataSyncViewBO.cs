using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{
    public class CommonDataSyncViewBO
    {
        public int BillId { get; set; }
        public int RegistrationId { get; set; }
        public string BillNumber { get; set; }
        public string CostCenter { get; set; }
        public decimal BillAmount { get; set; }
        public decimal VatAmount { get; set; }
        public string PaymentDescription { get; set; }

        public virtual Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
    }
}
