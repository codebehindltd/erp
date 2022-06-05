using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Membership;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBillPaymentResume
    {
        public bool IsSuccess { get; set; }
        public KotBillMasterBO KotBillMaster = new KotBillMasterBO();
        public List<KotBillDetailBO> KotBillDetails = new List<KotBillDetailBO>();
        public RestaurantBillBO RestaurantKotBill = new RestaurantBillBO();
        public List<GuestBillPaymentBO> RestaurantKotBillPayment = new List<GuestBillPaymentBO>();
        public GuestExtraServiceBillApprovedBO RoomWiseBillPayment = new GuestExtraServiceBillApprovedBO();
        public List<InvItemAutoSearchBO> OrderItem = new List<InvItemAutoSearchBO>();
        public MembershipPointDetailsBO membershipPointDetails = new MembershipPointDetailsBO();
        public GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
    }
}
