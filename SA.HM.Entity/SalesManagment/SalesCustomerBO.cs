using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class SalesCustomerBO
    {
        public int CustomerId { get; set; }
        public string CustomerType { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }

        public string ContactPerson { get; set; }
        public string ContactDesignation { get; set; }
        public string Department { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFax { get; set; }

        public string ContactPerson2 { get; set; }
        public string ContactDesignation2 { get; set; }
        public string Department2 { get; set; }
        public string ContactEmail2 { get; set; }
        public string ContactPhone2 { get; set; }
        public string ContactFax2 { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string DisplayName { get; set; }
    }
}
