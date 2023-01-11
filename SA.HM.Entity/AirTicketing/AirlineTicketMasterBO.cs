using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.AirTicketing
{
    public class AirlineTicketMasterBO
    {
        public long TicketId { get; set; }
        public int CostCenterId { get; set; }
        public string BillNumber { get; set; }
        public string TransactionType { get; set; }
        public long TransactionId { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int PaymentInstructionBankId { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public long ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string RegistrationNumber { get; set; }
        public Decimal? InvoiceAmount { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string Status { get; set; }
        public int? CheckedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
        public string TicketNumber { get; set; }
        public string PNRNumber { get; set; }
    }
}
