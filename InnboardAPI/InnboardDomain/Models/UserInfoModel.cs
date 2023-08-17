using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models
{
    public class UserInfoModel
    {
        public int UserInfoId { get; set; }
        public string UserId { get; set; }
        public int UserGroupId { get; set; }
        public string GroupName { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public string UserDesignation { get; set; }        
        public int GuestOrMemberId { get; set; }
        public int EmpId { get; set; }
        public int SupplierId { get; set; }        
        public System.DateTime CreatedDate { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public bool IsAdminUser { get; set; }
        public string InnboardHomePage { get; set; }
        public string UserGroupType { get; set; }
        public string UserSignature { get; set; }
        public int IsPaymentBillInfoHideInCompanyBillReceive { get; set; }
        public int IsReceiveBillInfoHideInSupplierBillPayment { get; set; }
    }
}
