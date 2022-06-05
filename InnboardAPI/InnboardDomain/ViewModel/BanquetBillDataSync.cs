using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.ViewModel
{
    public class BanquetBillDataSync
    {
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public BanquetReservation BanquetReservation { get; set; }

        public List<BanquetReservationDetail> BanquetReservationDetails { get; set; }

        public List<BanquetReservationClassificationDiscount> ClassificationDiscounts { get; set; }

        public List<HotelGuestBillPayment> GuestBillPayments { get; set; }

        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }
    }
}
