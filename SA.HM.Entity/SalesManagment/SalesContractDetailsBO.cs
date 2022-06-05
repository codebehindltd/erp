using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class SalesContractDetailsBO
    {
        public int ContractDetailsId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SigningDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public int tempContractDetailsId { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
