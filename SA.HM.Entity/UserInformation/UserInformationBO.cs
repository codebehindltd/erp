using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.UserInformation
{
    public class UserInformationBO
    {
        public int UserInfoId { get; set; }
        public int UserGroupId { get; set; }
        public string UserGroupType { get; set; }
        public string UserName { get; set; }
        public string UserIdAndUserName { get; set; }
        public string GroupName { get; set; }
        public string EmployeeName { get; set; }
        public string UserId { get; set; }
        public string UserIdAndName { get; set; }
        public string UserPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public Boolean IsAdminUser { get; set; }
        public int EmpId { get; set; }
        public int SupplierId { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserDesignation { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string SiteTitle { get; set; }
        public int GridViewPageSize { get; set; }
        public int GridViewPageLink { get; set; }
        public int MessageHideTimer { get; set; }
        public int WorkingCostCenterId { get; set; }
        public string InnboardHomePage { get; set; }
        public string RoomStatusFilteringType { get; set; }
        public int IsInnboardVatEnable { get; set; }
        public int IsInnboardServiceChargeEnable { get; set; }
        public string ServerDateFormat { get; set; }
        public string ClientDateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string FooterPoweredByInfo { get; set; }
        public string IsOldMenuEnable { get; set; }
        public DateTime DayOpenDate { get; set; }
        public string UserMenu { get; set; }
        public string UserReportMenu { get; set; }
        public string DisplayName { get; set; }
        public bool IsItemCanEditDelete { get; set; }
        public int BearerId { get; set; }
        public bool IsBearer { get; set; }
        public bool IsRestaurantBillCanSettle { get; set; }
        public bool IsMenuSearchRoomSearchRoomStatisticsInfoEnable { get; set; }
        public bool IsItemSearchEnable { get; set; }
        public byte PermissionType { get; set; }
        public byte DefaultPanel { get; set; }
        public string UserSignature { get; set; }
        public string CompanyType { get; set; }
        public string PayrollProvidentFundTitleText { get; set; }
        public int IsPaymentBillInfoHideInCompanyBillReceive { get; set; }
        public int IsReceiveBillInfoHideInSupplierBillPayment { get; set; }
        public bool IsItemAverageCostUpdateEnable { get; set; }
        public byte[] PoweredByQrCode { get; set; }
        public byte[] CompanyQrCode { get; set; }
    }
}
