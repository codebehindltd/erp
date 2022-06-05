using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CompanyBO
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string GroupCompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string VatRegistrationNo { get; set; }
        public string TinNumber { get; set; }
        public string Remarks { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public string CompanyType { get; set; }        
    }
}
