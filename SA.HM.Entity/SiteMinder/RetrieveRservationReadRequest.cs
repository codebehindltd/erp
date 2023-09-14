using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SiteMinder
{
    public class RetrieveRservationReadRequest
    {
        public static readonly string SelectionType = "Undelivered";
        public enum ReservationStatus
        {
            Book,
            Cancel,
            Modify,
            All
        }
    }
}
