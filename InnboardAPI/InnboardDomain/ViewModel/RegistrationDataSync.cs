using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.ViewModel
{
    public class RegistrationDataSync
    {
        
        public Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public HotelRoomRegistration RoomRegistration { get; set; }
        public List<HotelGuestRegistration> GuestRegistrationMappings { get; set; }
        public List<HotelGuestInformation> Guests { get; set; }
        public List<HotelGuestBillPayment> GuestBillPayments { get; set; }
        public List<HotelRegistrationAireportPickupDropView> AirportPickupDrops { get; set; }
        public List<HotelGuestBillApproved> ApprovedHotelGuestBills { get; set; }
        public List<HotelGuestServiceBill> GuestServiceBills { get; set; }
        public HotelGuestHouseCheckOut GuestHouseCheckOut { get; set; }
        public HotelGuestDayLetCheckOut HotelGuestDayLateCheckOut { get; set; }

        public List<HotelCompanyPaymentLedger> CompanyPayments { get; set; }
        
        public List<Guid> BillPaidForGuidId { get; set; }

        public RegistrationDataSync()
        {
            GuestRegistrationMappings = new List<HotelGuestRegistration>();
            Guests = new List<HotelGuestInformation>();
            GuestBillPayments = new List<HotelGuestBillPayment>();
            AirportPickupDrops = new List<HotelRegistrationAireportPickupDropView>();
            ApprovedHotelGuestBills = new List<HotelGuestBillApproved>();
            GuestServiceBills = new List<HotelGuestServiceBill>();
            CompanyPayments = new List<HotelCompanyPaymentLedger>();
            BillPaidForGuidId = new List<Guid>();
        }
    }
}
