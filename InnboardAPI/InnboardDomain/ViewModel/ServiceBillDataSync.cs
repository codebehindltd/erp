using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.ViewModel
{
    public class ServiceBillDataSync
    {
        public ServiceBillDataSync()
        {
            GuestBillPayments = new List<HotelGuestBillPayment>();
        }
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public HotelGuestServiceBill ServiceBill { get; set; }
        public List<HotelGuestBillPayment> GuestBillPayments { get; set; }

        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }
    }
}
