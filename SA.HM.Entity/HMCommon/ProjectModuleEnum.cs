using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HotelManagement.Entity.HMCommon
{
    public class ProjectModuleEnum
    {

        public enum ProjectModule
        {
            [Description("Task Management")]
            TaskManagement,

            [Description("Common Configuration")]
            CommonConfiguration,

            [Description("Front Office")]
            FrontOffice,

            [Description("Inventory Management")]
            InventoryManagement,

            [Description("Purchase Management")]
            PurchaseManagement,

            [Description("Restaurant Management")]
            RestaurantManagement,

            [Description("Sales Management")]
            SalesManagement,

            [Description("General Ledger")]
            GeneralLedger,

            [Description("Payroll Management")]
            PayrollManagement,

            [Description("Banquet Management")]
            BanquetManagement,

            [Description("Sales & Marketing Management")]
            SalesMarketingManagement,

            [Description("User Management")]
            UserManagement,

            [Description("Membership Management")]
            MembershipManagement,

            [Description("Membership Management")]
            HouseKeeping,

            [Description("Maintenance")]
            Maintenance,

            [Description("Document Management")]
            DocumentManagement,
            [Description("LC Management")]
            LCManagement,
            [Description("HM Common")]
            HMCommon,

            [Description("Vehicle Management")]
            VehicleManagement
        }

    }
}
