using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public static class ConstantHelper
    {
        public static readonly string CompanyEmail = @"admin@innboardbd.com";
        public static readonly string WebsiteName = @"www.innboard.com";
        public static readonly string SMTPClientUserId = @"innboard@gmail.com";
        public static readonly string SMTPClientUserPassword = @"innboard";
        public static readonly string SMTPClientHost = @"smtp.gmail.com";
        public static readonly int SMTPPort = 587;

        public enum RestaurantBillSource 
        {
            RestaurantTable,
            RestaurantToken,
            GuestRoom
        }
        public enum CustomerSupplierAutoSearch
        {
            CustomerItem,
            SupplierItem,
            CustomerNSupplierItem
        }
        public enum KotStatus
        {
            pending,
            settled,
            cleaned
        }
        public enum SalesandMarketingLogType {
            ContactCreated,
            ContactActivity,
            CompanyCreated,
            CompanyActivity,
            DealCreated,
            DealActivity,
            LoggedCall,
            LoggedMessage,
            LoggedEmail,
            LoggedMeeting,

        }
        public enum FrontOfficeLogActivityType
        {
            ReservationCreated,
            ReservationActivity,
            RegistrationCreated,
            RegistrationActivity,
            ServiceBillActivity,
            RoomChange,
            GuestManagement,
            NightAudit
        }
        public enum FOActivityLogFormName
        {
            frmRoomRegistrationNew,
            frmRoomReservationNew,
            frmGHServiceBill,
            frmRoomShift
        }
        public enum UserGroupType
        {
            Admin,
            FrontOffice,
            Inventory,
            Accounts,
            Technical
        }

        public enum GLTransactionType
        {
            [Description("Accounts")]
            Accounts,
            [Description("Company (CRM)")]
            Company,
            [Description("Supplier")]
            Supplier,
            [Description("Employee")]
            Employee,
            [Description("Member")]
            Member,
            [Description("CNF")]
            CNF,
            [Description("Inventory")]
            Inventory
        }
    }
}