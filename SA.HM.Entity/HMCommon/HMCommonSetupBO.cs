using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
   public class HMCommonSetupBO
    {
        public int SetupId { get; set; }
        public string TypeName { get; set; }
        public string SetupName { get; set; }
        public string SetupValue { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public static String DatabaseName { get; set; }
        public static String DBServerName { get; set; }
        public string PrintDate { get; set; }
        public string PayByDetails { get; set; }
        public int PaymentModeId { get; set; }
        public string PaymentMode { get; set; }
        public string DisplayName { get; set; }
        public int PaymentAccountsPostingId { get; set; }
        public string PaymentAccountsPostingHead { get; set; }
        public int ReceiveAccountsPostingId { get; set; }
        public string ReceiveAccountsPostingHead { get; set; }
        public int FieldId { get; set; }
        public string FieldType { get; set; }
        public string FieldValue { get; set; }
        public Boolean ActiveStat { get; set; }
        public int IsCanCheck { get; set; }
        public int IsCanApprove { get; set; }
    }
}
