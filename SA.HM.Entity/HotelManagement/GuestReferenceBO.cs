using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestReferenceBO
    {
        public int ReferenceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public decimal SalesCommission { get; set; }

        public string Email { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string CellNumber { get; set; }
        public string TelephoneNumber { get; set; }
    }

}
