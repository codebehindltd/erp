namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class HotelRoomReservationDetailsMobileAppsBO
    {
        public long ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomQuantity { get; set; }
        public int PaxQuantity { get; set; }
        public int ChildQuantity { get; set; }
        public int ExtraBedQuantity { get; set; }
        public string GuestNotes { get; set; }
    }
}
