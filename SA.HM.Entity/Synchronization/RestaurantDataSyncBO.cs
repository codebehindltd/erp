using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{
    public class RestaurantDataSyncBO
    {
        public RestaurantDataSyncBO()
        {
            BillDetails = new List<RestaurantBillDetailBO>();
            BillClassificationDiscounts = new List<RestaurantBillClassificationDiscount>();
            KotBillMasters = new List<KotBillMasterBO>();
            KotBillDetails = new List<RestaurantKotBillDetail>();
            KotSpecialRemarksDetails = new List<RestaurantKotSpecialRemarksDetailBO>();
            GuestBillPayments = new List<HotelGuestBillPaymentBO>();
            GuestExtraServiceApprovedBills = new List<GuestExtraServiceBillApprovedBO>();
        }
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public RestaurantBill Bill { get; set; }
        public List<RestaurantBillDetailBO> BillDetails { get; set; }
        public List<RestaurantBillClassificationDiscount> BillClassificationDiscounts { get; set; }
        public List<KotBillMasterBO> KotBillMasters { get; set; }
        public List<RestaurantKotBillDetail> KotBillDetails { get; set; }
        public List<RestaurantKotSpecialRemarksDetailBO> KotSpecialRemarksDetails { get; set; }
        public List<HotelGuestBillPaymentBO> GuestBillPayments { get; set; }
        public List<GuestExtraServiceBillApprovedBO> GuestExtraServiceApprovedBills { get; set; }
        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }
    }
}
