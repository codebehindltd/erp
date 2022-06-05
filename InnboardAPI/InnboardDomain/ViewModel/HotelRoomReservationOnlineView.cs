using InnboardDomain.Models;
using System.Collections.Generic;

namespace InnboardDomain.ViewModel
{
    public class HotelRoomReservationOnlineView
    {
        public HotelRoomReservationOnline HotelRoomReservationOnline { get; set; }
        public HotelGuestInformationOnline HotelGuestInformationOnline { get; set; }
        public List<HotelRoomReservationDetailOnline> HotelRoomReservationDetailOnlines { get; set; }
    }
}
