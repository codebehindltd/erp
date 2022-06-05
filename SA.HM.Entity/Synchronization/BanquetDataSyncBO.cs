using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{
    public class BanquetDataSyncBO
    {
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public BanquetReservation BanquetReservation { get; set; }
        public List<BanquetReservationDetailBO> BanquetReservationDetails { get; set; }
        public List<BanquetReservationClassificationDiscountBO> ClassificationDiscounts { get; set; }
        public List<HotelGuestBillPaymentBO> GuestBillPayments { get; set; }

        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }
        public BanquetDataSyncBO()
        {
            BanquetReservationDetails = new List<BanquetReservationDetailBO>();
            ClassificationDiscounts = new List<BanquetReservationClassificationDiscountBO>();
            GuestBillPayments = new List<HotelGuestBillPaymentBO>();
            CompanyPayments = new List<HotelCompanyPaymentLedger>();
        }
    }
}
