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
        public decimal CreditLimit { get; set; }
        public DateTime CreditLimitExpire { get; set; }
        public decimal ShortCreditLimit { get; set; }
        public DateTime ShortCreditLimitExpire { get; set; }
        public decimal TransportFareFactory { get; set; }
        public decimal TransportFareDepo { get; set; }
        public decimal SalesCommission { get; set; }
        public bool LegalAction { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DetailDescription { get; set; }
        public string CallToAction { get; set; }
    }
}
