using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{

    public class RegistrationDataSyncBO 
    {
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public HotelRoomRegistration RoomRegistration { get; set; }
        public List<GuestRegistrationMappingBO> GuestRegistrationMappings { get; set; }
        public List<HotelGuestInformation> Guests { get; set; }
        public List<HotelGuestBillPaymentBO> GuestBillPayments { get; set; }
        public List<HotelGuestBillApproved> ApprovedHotelGuestBills { get; set; }
        public List<HotelRegistrationAireportPickupDropViewBO> AirportPickupDrops { get; set; }
        public List<HotelGuestServiceBillBO> GuestServiceBills { get; set; }
        public HotelGuestHouseCheckOutBO GuestHouseCheckOut { get; set; }
        public HotelGuestDayLetCheckOut HotelGuestDayLateCheckOut { get; set; }
        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }

        public List<Guid> BillPaidForGuidId { get; set; }
        public RegistrationDataSyncBO()
        {
            GuestRegistrationMappings = new List<GuestRegistrationMappingBO>();
            Guests = new List<HotelGuestInformation>();
            GuestBillPayments = new List<HotelGuestBillPaymentBO>();
            ApprovedHotelGuestBills = new List<HotelGuestBillApproved>();
            AirportPickupDrops = new List<HotelRegistrationAireportPickupDropViewBO>();
            GuestServiceBills = new List<HotelGuestServiceBillBO>();
            CompanyPayments = new List<HotelCompanyPaymentLedger>();
            BillPaidForGuidId = new List<Guid>();
        }
    }
}
