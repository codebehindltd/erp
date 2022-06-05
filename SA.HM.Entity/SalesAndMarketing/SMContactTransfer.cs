using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMContactTransfer
    {
        public long Id { get; set; }
        public long ContactId { get; set; }
        public int? PreviousCompanyId { get; set; }
        public string PreviousCompany { get; set; }
        public int? TransferredCompanyId { get; set; }
        public string TransferredCompany { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
