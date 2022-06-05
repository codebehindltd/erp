using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{
    public class ServiceBillDataSyncBO
    {
        public ServiceBillDataSyncBO()
        {
            GuestBillPayments = new List<HotelGuestBillPaymentBO>();
        }
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public HotelGuestServiceBillBO ServiceBill { get; set; }
        public List<HotelGuestBillPaymentBO> GuestBillPayments { get; set; }

        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }
    }
}
