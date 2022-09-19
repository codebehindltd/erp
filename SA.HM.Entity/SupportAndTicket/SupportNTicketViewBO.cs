using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SupportAndTicket
{
    public class SupportNTicketViewBO
    {
        public long Id { get; set; }
        public string SupportCategory { get; set; }
        public string BranchCode { get; set; }
        public string CompanyName { get; set; }
        public string BillingAddress { get; set; }
        public string CityName { get; set; }
        public string CaseName { get; set; }
        public string WarrantyType { get; set; }        
        public string CaseNumber { get; set; }
        public string CaseDeltails { get; set; }
        public string ItemCategory { get; set; }
        public string ItemName { get; set; }
        public decimal UnitQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal VatAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal TotalAmount { get; set; }
        public string SupportType { get; set; }
        public string SupportStatus { get; set; }
        public string BillStatus { get; set; }
        public string FeedbackStatus { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string AssignedTo { get; set; }
        public string CaseCloseByName { get; set; }
        public string CaseCloseDateDisplay { get; set; }
        public int PassDay { get; set; }
    }
}
