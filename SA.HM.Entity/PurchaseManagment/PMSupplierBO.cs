using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMSupplierBO
    {

        public int SupplierId { get; set; }
        public int SupplierDetailsId { get; set; }
        public int NodeId { get; set; }
        public string Name { get; set; }
        public string SupplierType { get; set; }
        public string SupplierTypeId { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactAddress { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactType { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public int IsAdhocSupplier { get; set; }

        public decimal Balance { get; set; }
        public string NameWithCode { get; set; }

        public string CompanyCommaSeperatedIds { get; set; }
    }
}
