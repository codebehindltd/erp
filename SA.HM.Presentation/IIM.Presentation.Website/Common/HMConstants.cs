using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public class HMConstants
    {
        public enum ApplicationFormName
        {
            frmCompany,
            frmSearchGuest,
            frmBank,
            frmLocation,
            frmCity,
            frmIndustry,
            frmCostCentre,
            frmGuestHouseService,
            frmHMFloor,
            frmGuestCompany,
            frmRoomType,
            frmRoomNumber,
            frmRoomAllocation,
            frmRoomReservation,
            frmRoomReservationNew,
            frmReservationBillPayment,
            frmRoomRegistration,
            frmRoomRegistrationNew,
            frmRoomShift,
            frmGHServiceBill,
            frmNightAudit,
            frmAvailableGuestList,
            frmGuestBillPayment,
            frmExpectedDeparture,
            frmHMConvertCurrency,
            frmReportRoomStatusHistory,
            frmReportRoomStatus,
            frmReportRoomWiseStatus,
            frmReportRoomReservation,
            frmReportGuestAirportPickupDropInfo,
            frmUserGroup,
            frmUserInformation,
            frmObjectPermission,
            frmHMFloorManagement,
            frmRoomCleanup,
            frmSupplier,
            frmStockCategory,
            frmStockItem,
            frmHMStockPurchase,
            frmHMRoomInventory,
            frmNodeMatrix,
            frmVoucher,
            frmReportChartOfAccounts,
            frmManageOnlineRoomReservation,
            frmDepartment,
            frmEmpType,
            frmDesignation,
            frmLeaveType,
            frmLeaveInformation,
            frmKitchenInformation,
            frmPrinterInfo,
            frmRestaurantItemCategory,
            frmRestaurantItem,
            frmRestaurantCombo,
            frmRestaurantBuffet,
            frmCostCenterChooseForKot,
            frmGuestRoomKotBill,
            frmRestaurantBill,
            FrmChooseTableForBill,
            frmCostCenterChooseForRB,
            frmFrontOfficeUser,
            frmPRPOUserPermission,
            frmCashierInformation,
            frmEmpGrade,
            frmSalaryHead,
            frmTableInformation,
            frmTableManagement,
            frmSalaryFormula,
            frmEmpIncrement,
            frmHMCommonSetup,
            frmEmpOverTime,
            frmEmpPayScale,
            frmAllowanceDeductionHead,
            frmEmpAllowanceDeduction,
            frmTimeSlabHead,
            frmEmpTimeSlab,
            frmEmpLeaveAdjustment,
            frmEmpSalaryProcess,
            frmEmpAttendance,
            GLProject,
            frmGLProject,
            frmGLDonor,
            frmGLCompany,
            frmGLAccountConfiguration,
            frmVoucherSearch,
            frmInvManufacturer,
            frmInvCategory,
            frmInvItem,
            frmInvFinishedProduct,
            frmInvProduction,
            frmInvStockVariance,
            frmPMConfiguration,
            frmPMRequisition,
            frmPMProductPO,
            frmPMSupplier,
            frmPMRequisitionApproval,
            frmPMPurchaseOrderDetails,
            frmMachineTest,
            frmSalesCustomer,
            frmSalesService,
            frmServiceBundle,
            frmCompanyBank,
            frmPMSales,
            frmPMSalesBillPayment,
            frmPMProductPOApproval,
            frmPMServicePOApproval,
            frmBanquetOccessionType,
            frmBanquetSeatingPlan,
            frmBanquetRequisites,
            frmBanquetInformation,
            frmBanquetReservation,
            frmBanquetBillPayment,
            frmReportDivisionRevenue,
            frmReportBCDivisionRevenue,
            frmReportBCRoomSalesRevenue,
            frmReportRoomSalesRevenue,
            frmReportDiscountVsActualSales,
            frmReportRoomOccupencyAnalysis,
            frmBanquetRefference,
            frmReportBanquetReferenceRevenue,
            frmReportBanquetHalRevenue,
            frmReportBanquetOccessionTypeRevenue,
            frmReportBanquetClientRevenue,
            frmReportBanquetRevenueInfo,
            frmLCBankSettlement,
            frmGuestReference,
            frmBanquetReservationCancel,
            frmPayrollConfiguration,
            frmLCSettlement,
            frmHotelConfiguration,
            frmPOSConfiguration,
            frmRoomCalender,
            frmRosterHead,
            frmHMComplementaryItem,
            frmInvItemSpecialRemarks,
            frmReportGuestCompanyInfo,
            frmReportGuestReferenceInfo,
            frmReportRoomAvailableStatus,
            frmReportCanceledReservation,
            frmReportInHouseGuestLedger,
            frmBanquetBillSettlement,
            frmGLFixedAssets,
            frmReportHouseKeeping,
            frmReportGuestLedgerTranscript,
            frmSalesBandwidthInfo,
            frmReportRoomShiftInfo,
            frmHMDayClose,
            frmWorkingPlan,
            frmPayrollHoliday,
            frmEmpChangePassword,
            frmServiceBillTransfer,
            frmBillAdjustment,
            frmBillSearch,
            frmVatAdjustment,
            frmEmpLoan,
            frmGuestCheckIn,
            frmGuestCheckOut,
            frmInvLocation,
            frmInvRack,
            frmCommonMessage,
            frmReportHKRoomDetail,
            frmAirlineInformation,
            frmGuestManagement,
            frmProductReceiveAccountsPostingApproval,
            frmBanquetBillSearch,
            frmInvStockAdjustment,
            frmItemClassification,
            ItemRequisitionInformation,
            frmProjectInformation
        }

        public enum SalaryType
        {
            Allowance,
            Deduction,
            NotEffective,
            LastMonthPay
        }

        public enum SalaryProcessDependsOn
        {
            Basic,
            Gross
        }

        public enum AmountType
        {
            Percent,
            Amount,
            Days
        }

        public enum ApprovalStatus
        {
            Pending,
            Approved,
            PartiallyApproved,
            Checked,
            PartiallyChecked,
            Cancel,
            Submit
        }

        public enum LoanStatus
        {
            Regular,
            Due,
            Overdue,
            Holdup
        }

        public enum ComapreString
        {
            EQ,
            GT,
            LT,
            GE,
            LE
        }

        public enum PrintTemplate
        {
            Template1 = 1,
            Template2 = 2,
            Template3 = 3,
            Template4 = 4
        }

        public enum RestaurantBillPrintAndPreview
        {
            NoPrintAndPreview = 0,
            BillPreviewOnly = 1,
            DirectPrint = 2,
            PrintAndPreview = 3
        }

        public enum KotPrintAndPreview
        {
            KotNotPrint = 0,
            KotPrint = 1
        }

        public enum PurchaseOrderTemplate
        {
            Template1 = 1,
            Template2 = 2,
            ApprovedEnable = 1,
            ApprovedDisable = 0,
        }

        public enum ProductReceiveTemplate
        {
            Template1 = 1,
            Template2 = 2,
            ApprovalEnable = 1,
            ApprovalDisable = 0,
        }

        public enum ViewTemplate
        {
            Template1 = 1,
            Template2 = 2,
            ApprovedEnable = 1,
            ApprovedDisable = 0,
        }

        public enum DiscountType
        {
            Fixed,
            Percentage
        }

        public enum PaymentMode
        {
            Cash,
            Card,
            Refund,
            Rounded,
            Employee,
            Company,
            Member,
            GuestRoom
        }

        public enum CardType
        {
            AmexCard = 'a',
            MasterCard = 'm',
            VisaCard = 'v',
            DiscoverCard = 'd'
        }

        public enum PrintPageType
        {
            Portrait,
            Landscape
        }

        public enum CostcenterFilteringForPOPR
        {
            Requisition,
            Purchase,
            Receive
        }

        public enum PayrollSalaryExecutionProcessType
        {
            Regular = 1,
            RedCross = 2,
            IPTech = 3,
            SouthSudan = 4
        }

        public enum PermissionType
        {
            AdminPanel = 1,
            POS = 2,
            Both = 3
        }

        
        public static class EmailTemplates
        {
            public const string ReservationConfirmation = "ReservationConfirmationTemplate.html";
            public const string CheckOut = "CheckOutTemplate.html";
            public const string BirthdayWish = "BirthdayTemplate.html";
            public const string Membership = "MembershipConfirmTemplate.html"; 
            public const string MembershipRejected = "MembershipRejectTemplate.html";
            public const string MembershipRegistration = "MembershipRegistration.html";
            public const string MembershipIntroducer1 = "MembershipIntroducer.html";
            public const string MembershipIntroducer2 = "MembershipIntroducer2.html";
            public const string SalesTaskReminderTemplate = "SalesTaskReminderTemplate.html";
            public const string RequestForQuotationTemplete = "RequestForQuotationTemplete.html";
            public const string TemplateCustomEmail = "TemplateCustomEmail.html";
        }

        public static class SMSTemplates
        {
            public const string ReservationConfirmation = "ReservationConfirmationSms.txt";
            public const string ReservationCancellation = "ReservationCancelSms.txt";
            public const string ReservationAmedmentSmsTemplate = "ReservationAmedmentSms.txt";
        }
    }
}