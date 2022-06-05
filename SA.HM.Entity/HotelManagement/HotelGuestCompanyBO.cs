using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelGuestCompanyBO
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string Remarks { get; set; }
        public string SignupStatus { get; set; }
        public string LifeCycleStage { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? NodeId { get; set; }
        public int? CompanyOwnerId { get; set; }
        public int CreatedBy { get; set; }
    }
}
