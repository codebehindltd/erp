using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InnboardDomain.Models;

namespace InnboardDomain.ViewModel
{
    public class RestaurantDataSync
    {
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public RestaurantBill Bill { get; set; }
        public List<RestaurantBillDetail> BillDetails { get; set; }
        public List<RestaurantBillClassificationDiscount> BillClassificationDiscounts { get; set; }
        public List<RestaurantKotBillMaster> KotBillMasters { get; set; }
        public List<RestaurantKotBillDetail> KotBillDetails { get; set; }
        public List<RestaurantKotSpecialRemarksDetail> KotSpecialRemarksDetails { get; set; }
        public List<HotelGuestExtraServiceBillApproved> GuestExtraServiceApprovedBills { get; set; }
        public List<HotelGuestBillPayment> GuestBillPayments { get; set; }

        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }
    }
}
