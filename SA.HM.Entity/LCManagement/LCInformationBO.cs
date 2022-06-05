using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class LCInformationBO : PermissionViewBO
    {
        public long LCId { get; set; }
        public long RandomProductId { get; set; }
        public string LCNumber { get; set; }
        public string PINumber { get; set; }
        public DateTime LCOpenDate { get; set; }
        public DateTime? LCMatureDate { get; set; }
        public int BankAccountId { get; set; }
        public decimal? LCValue { get; set; }
        public int SupplierId { get; set; }
        public int CompanyId { get; set; }
        public int LCManageAccountId { get; set; }
        public int ProjectId { get; set; }
        public int POorderId { get; set; }
        public string LCTypes { get; set; }
        public string Incoterms { get; set; }
        
        public string Supplier { get; set; }
        public int CheckedBy { get; set; }
        public int ApprovedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ApprovedStatus { get; set; }
        public string Remarks { get; set; }

        public Boolean IsLCBankSettlement { get; set; }
        public int BankSettlementBy { get; set; }
        public DateTime BankSettlementDate { get; set; }

        public Boolean IsLCSettlement { get; set; }
        public string SettlementDescription { get; set; }
        public int SettlementBy { get; set; }
        public DateTime SettlementDate { get; set; }
        public int CostCenterId { get; set; }
    }
}
