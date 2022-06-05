using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HotelManagement.Entity.HMCommon
{
    public class EntityTypeEnum
    {
        public enum EntityType
        {
            [Description("Login")]
            Login,
            [Description("Logout")]
            Logout,
            [Description("ServiceBill")]
            ServiceBill,
            [Description("Company")]
            Company,
            [Description("Cost Center")]
            CostCenter,
            [Description("Printer")]
            Printer,
            [Description("Company Bank")]
            CompanyBank,
            [Description("Bank")]
            Bank,
            [Description("Location")]
            Location,
            [Description("City")]
            City,
            [Description("Industry")]
            Industry,
            [Description("BussinessPromotion")]
            BussinessPromotion,
            [Description("Payment Mode Configuration")]
            PaymentModeConfiguration,
            [Description("Send Mail")]
            SendMail,
            [Description("Conversion Head")]
            ConversionHead,
            [Description("Receive Mail")]
            ReceiveMail,
            [Description("Basic Head Setup")]
            BasicHeadSetup,
            [Description("Salary Date Setup")]
            SalaryDateSetup,
            [Description("Over Time Setup")]
            OverTimeSetup,
            [Description("Banquet Occession Type")]
            BanquetOccessionType,
            [Description("Banquet SeatingP lan")]
            BanquetSeatingPlan,
            [Description("Banquet Requisites")]
            BanquetRequisites,
            [Description("Banquet Refference")]
            BanquetRefference,
            [Description("Banquet Information")]
            BanquetInformation,
            [Description("Banquet Reservation")]
            BanquetReservation,
            [Description("Banquet Bill Payment")]
            BanquetBillPayment,
            [Description("Chart Of Account")]
            ChartOfAccount,
            [Description("Genarel Ledger Company")]
            GLCompany,
            [Description("Genarel Ledger Project")]
            GLProject,
            [Description("Genarel Ledger Donor")]
            GLDonor,
            [Description("Voucher SetUp")]
            VoucherSetUp,
            [Description("Ledger Month Setup")]
            LedgerMonthSetup,
            [Description("Ledger Account Configuration")]
            LedgerAccountConfiguration,
            [Description("General Ledger Voucher")]
            GeneralLedgerVoucher,
            [Description("Ledger Deal Status")]
            Attendance,
            [Description("Attendance Status")]
            LedgerDealStatus,
            [Description("Room CheckOut")]
            RoomCheckOut,
            [Description("Guest Bill Payment")]
            GuestBillPayment,
            [Description("Guest Bill Payment Transfer")]
            GuestBillPaymentTransfer,
            [Description("PaxIn")]
            PaxIn,
            [Description("Approve Available Guest")]
            ApproveAvailableGuest,
            [Description("Approve Guest Service")]
            ApproveGuestService,
            [Description("Guest Room And Service Approve")]
            GuestRoomAndServiceApprove,
            [Description("Room Shift")]
            RoomShift,
            [Description("Room Registration")]
            RoomRegistration,
            [Description("Reservation Bill Payment")]
            ReservationBillPayment,
            [Description("Room Type")]
            RoomType,
            [Description("Room Number")]
            RoomNumber,
            [Description("Room Clean")]
            RoomClean,
            [Description("Floor Management")]
            FloorManagement,
            [Description("Guest House Service")]
            GuestHouseService,
            [Description("Hottel Bill Configuration")]
            HottelBillConfiguration,
            [Description("Servic Bill Configuration")]
            ServicBillConfiguration,
            [Description("Check Out Time Configuration")]
            CheckOutTimeConfiguration,
            [Description("Check In Time Configuration")]
            CheckInTimeConfiguration,
            [Description("Guest Reference")]
            GuestReference,
            [Description("Guest Company")]
            GuestCompany,
            [Description("Company Site")]
            CompanySite,
            [Description("Special Remarks")]
            SpecialRemarks,
            [Description("Complementary Item")]
            ComplementaryItem,
            [Description("Hotel Service")]
            HotelService,
            [Description("Room Reservation")]
            RoomReservation,
            [Description("Online Room Reservation")]
            RoomReservationOnline,
            [Description("Sales")]
            PMSales,
            [Description("Currency Configuration")]
            CurrencyConfiguration,
            [Description("Currency Conversion")]
            CurrencyConversion,
            [Description("Sales BandwidthInfo")]
            SalesBandwidthInfo,
            [Description(" Contact Title")]
            ContactTitle,
            [Description(" Working Plan")]
            WorkingPlan,
            [Description(" Payroll Holiday")]
            PayrollHoliday,
            [Description(" Monthly Working Day Configuration")]
            MonthlyWorkingDayConfiguration,
            [Description(" Working Hour Monthly Working Day Configuration")]
            WorkingHourMonthlyWorkingDayConfiguration,
            [Description(" Working Hour Configuration")]
            WorkingHourConfiguration,
            [Description(" Minimum Overtime Hour Configuration")]
            MinimumOvertimeHourConfiguration,
            [Description(" Instead Leave For One Holiday Configuration")]
            InsteadLeaveForOneHolidayConfiguration,
            [Description(" Monthly Working Day For Absentree Configuration")]
            MonthlyWorkingDayForAbsentreeConfiguration,
            [Description(" Instead Leave Head Configuration")]
            InsteadLeaveHeadConfiguration,
            [Description("Machine Test")]
            MachineTest,
            [Description("Restaurant Kitchen")]
            RestaurantKitchen,
            [Description("Restaurant Table")]
            RestaurantTable,
            [Description("Restaurant Bearer")]
            RestaurantBearer,
            [Description("EmpKotBillDetail")]
            EmpKotBillDetail,
            [Description("Restaurant Combo")]
            RestaurantCombo,
            [Description("Restaurant Buffet")]
            RestaurantBuffet,
            [Description("Restaurant Cashier")]
            RestaurantCashier,
            [Description("Sales Call")]
            SalesCall,
            [Description("User Group")]
            UserGroup,
            [Description("User Information")]
            UserInformation,
            [Description("User Permission")]
            UserPermission,
            [Description("Membership Type")]
            MembershipType,
            [Description("Membership Info")]
            MembershipInfo,
            [Description("Applicant Info")]
            ApplicantInfo,
            [Description("Job Circular")]
            JobCircular,
            [Description("Interview Type")]
            InterviewType,
            [Description("Interview Evaluation")]
            InterviewEvaluation,
            [Description("Restaurant Bill")]
            RestaurantBill,
            [Description("Restaurant Item")]
            RestaurantItem,
            [Description("Restaurant Item Remarks")]
            RestaurantItemRemarks,
            [Description("Supplier")]
            Supplier,
            [Description("Product Requisition")]
            ProductRequisition,
            [Description("Product Purchase Order")]
            ProductPurchaseOrder,
            [Description("Purchase Return")]
            PurchaseReturn,
            [Description("Employee Department")]
            EmployeeDepartment,
            [Description("Employee Grade")]
            EmployeeGrade,
            [Description("Employee Designation")]
            EmployeeDesignation,
            [Description("Employee Type")]
            EmployeeType,
            [Description("Employee Bill Generation")]
            EmployeeBillGeneration,
            [Description("Employee Payment")]
            EmployeePayment,
            [Description("Employee Termination")]
            PayrollEmpTerminatio,
            [Description("Item Category")]
            ItemCategory,
            [Description("Inv Item")]
            InvItem,
            [Description("Inv Item Classification Name")]
            InvItemClassification,
            [Description("Inv Item Classification Mapping with Account Head")]
            InvItemClassificationMappingCostCenterWithAccountHead,
            [Description("Salary Head")]
            SalaryHead,
            [Description("Salary Formula")]
            SalaryFormula,
            [Description("Leave Type")]
            LeaveType,
            [Description("Leave Deduction Configuration")]
            LeaveDeduction,
            [Description("Roster Head")]
            RosterHead,
            [Description("TimeSlab Head")]
            TimeSlabHead,
            [Description("Yearly Leave")]
            YearlyLeave,
            [Description("Emp Leave Info")]
            EmpLeaveInfo,
            [Description("Emp Increment")]
            EmpIncrement,
            [Description("Emp Allowance/Dedeuction")]
            EmpAllowDeduct,
            [Description("Emp Roster")]
            EmpRoster,
            [Description("Emp Attendance")]
            EmpAttendance,
            [Description("Emp Advance Taken")]
            EmpAdvanceTaken,
            [Description("Emp Loan")]
            EmpLoan,
            [Description("Employee Loan Collection")]
            LoanCollection,
            [Description("Employee Loan Holdup")]
            LoanHoldup,
            [Description("Emp Promotion")]
            EmpPromotion,
            [Description("Emp Gratuity")]
            EmpGratuity,
            [Description("Staff Requisition")]
            StaffRequisition,
            [Description("Staff Budget")]
            StaffBudget,
            [Description("Best Employee Selection")]
            BestEmpSelection,
            [Description("Best Employee Nomination")]
            BestEmpNomination,
            [Description("Employee Transfer")]
            EmpTransfer,
            [Description("Bill Adjustment")]
            BillAdjustment,
            [Description("Room Reservation No Show")]
            RoomReservationNoShow,
            [Description("Company Wise Discount Policy")]
            DiscountPolicy,
            [Description("Guest Preference")]
            GuestPreference,
            [Description("Benefit")]
            Benefit,
            [Description("CNF Transaction")]
            CNFTransaction,
            [Description("LC Over Head")]
            LCOverHead,
            [Description("LC Over Head Expense")]
            LCOverHeadExpense,
            [Description("Daily Sales Statement Configuration")]
            RestaurantDailySalesStatementConfiguration,
            [Description("Sales Quotation")]
            SMQuotation,
            [Description("Sales Quotation Details")]
            SMQuotationDetails,
            [Description("Airline")]
            Airline,
            [Description("ProductReceive")]
            ProductReceive,
            [Description("Product Receive Approval")]
            ProductReceiveApproval,
            [Description("Employee")]
            Employee,
            [Description("Day Close Process")]
            DayClose,
            [Description("Auto Night Audit and Approval Process")]
            AutoNightAuditAndApprovalProcess,
            [Description("Report Config Master")]
            CommonReportConfigMaster,

            [Description("Report Config Details")]
            CommonReportConfigDetails,
            [Description("Guest House CheckOut")]
            GuestHouseCheckOut,
            [Description("Company Payment")]
            CompanyPayment,
            [Description("Company Payment Details")]
            CompanyPaymentDetails,
            [Description("Company Bill Generation Details")]
            CompanyBillGenerationDetails,
            [Description("Guest Information")]
            GuestInformation,
            [Description("HM Paid Service")]
            HMPaidService,
            [Description("RoomStatus Possible Path")]
            RoomStatusPossiblePath,
            [Description("Guest Reservation Mapping")]
            GuestReservationMapping,
            [Description("Guest Preference Mapping")]
            GuestPreferenceMapping,
            [Description("Guest Information Online")]
            GuestInformationOnline,
            [Description("Room Reservation Details")]
            RoomReservationDetails,
            [Description("Hotel Guest DayLet CheckOut")]
            HotelGuestDayLetCheckOut,
            [Description("Room Stop Charge Posting Details")]
            RoomStopChargePostingDetails,
            [Description("Room Owner")]
            RoomOwner,
            [Description("Room Owner Detail")]
            RoomOwnerDetail,
            [Description("Documents")]
            Documents,
            [Description("Hotel Reservation Aireport Pickup Drop")]
            HotelReservationAireportPickupDrop,
            [Description("Room Discrepancy")]
            HotelRoomDiscrepancy,
            [Description("Employee Task Assignment")]
            HotelEmpTaskAssignment,
            [Description("Daily Room Condition")]
            HotelDailyRoomCondition,
            [Description("Room Wise Task Assignment")]
            TaskAssignmentRoomWise,
            [Description("Inventory Manufacturer")]
            InvManufacturer,
            [Description("Inventory Item Stock Adjustment")]
            ItemStockAdjustment,
            [Description("Inventory Item Stock Variance")]
            InvItemStockVariance,
            [Description("Purchase Management Product Output")]
            PMProductOut,
            [Description("Product Out For Room")]
            ProductOutForRoom,
            [Description("Purchase Management Product Received")]
            PMProductReceived,
            [Description("Appraisal Evaluation")]
            AppraisalEvaluation,
            [Description("Disciplinary Action")]
            DisciplinaryAction,
            [Description("Disciplinary Action Reason")]
            DisciplinaryActionReason,
            [Description("Disciplinary Action Type")]
            DisciplinaryActionType,
            [Description("Allowance Deduction")]
            AllowanceDeduction,
            [Description("Employee Grade")]
            EmpGrade,
            [Description("Finished Product")]
            FinishedProduct,
            [Description("Payroll Employee Last Month Benifits Payment")]
            PayrollEmpLastMonthBenifitsPayment,
            [Description("Leave Information")]
            LeaveInformation,
            [Description("Employee OverTime")]
            EmpOverTime,
            [Description("Employee Pay Scale")]
            EmpPayScale,
            [Description("Provident Fund Setting")]
            PFSetting,
            [Description("Employee Salary Process")]
            EmpSalaryProcess,
            [Description("Employee Tax Deduction Setting")]
            EmpTaxDeductionSetting,
            [Description("Employee Time Slab")]
            EmpTimeSlab,
            [Description("Employee Training")]
            EmpTraining,
            [Description("Employee Training Organizer")]
            EmpTrainingOrganizer,
            [Description("Employee Training Type")]
            EmpTrainingType,
            [Description("Employee Type")]
            EmpType,
            [Description("Employee Yearly Leave")]
            EmployeeYearlyLeave,
            [Description("Hotel Management Common Setup")]
            HMCommonSetup,
            [Description("Bonus Configuration")]
            BonusConfiguration,
            [Description("Tax Setting")]
            TaxSetting,
            [Description("Loan Setting")]
            LoanSetting,
            [Description("Gratutity Settings")]
            GratutitySettings,
            [Description("Attendance Device")]
            AttendanceDevice,
            [Description("Bonus Head")]
            BonusHead,
            [Description("Employee Bonus")]
            EmpBonus,
            [Description("Service Charge Configuration")]
            ServiceChargeConfiguration,
            [Description("Update Approved Night Audited Data")]
            UpdateApprovedNightAuditedData,
            [Description("Purchase Management Product Output Details")]
            PMProductOutDetails,
            [Description("Hotel Room features")]
            RoomFeatures,
            [Description("Hotel Room features information")]
            RoomFeaturesInfo,
            [Description("GatePass")]
            GatePass,
            [Description("Linked Room")]
            LinkedRoom,
            [Description("Restuarant Configuration")]
            RestuarantConfiguration,
            [Description("Inventory Configuration")]
            InventoryConfiguration,
            [Description("Front Office Configuration")]
            FOConfiguration,
            [Description("Product Return")]
            PMProductReturn,
            [Description("Product Return Details")]
            PMProductReturnDetails,
            [Description("Product Return Serial Item")]
            PMProductReturnSerial,
            [Description("Restaurant Sales Return Item")]
            RestaurantSalesReturnItem,
            [Description("Discount Configuration Setup")]
            DiscountConfigSetup,
            [Description("Discount Master")]
            DiscountMaster,
            [Description("Recipe Items")]
            RecipeeItems,
            [Description("Company Signup Status")]
            SMCompanySignupStatus,
            [Description("Sales & Marketing Stage")]
            SMDealStage,
            [Description("Task Assignment")]
            SMTask,
            [Description("Sales & Marketing Deal")]
            SMDeal,
            [Description("Deal Implementation Feedback")]
            DealImpFeedback,
            [Description("Sales & Marketing Deal Wise Contact Map")]
            SMDealWiseContactMap,
            [Description("Sales & Marketing Contact Information")]
            ContactInformation,
            [Description("Sales & Marketing Contact Transfer")]
            SMContactTransfer,
            [Description("Payroll Stage")]
            TaskStage,
            [Description("Project Stage")]
            ProjectStage,
            [Description("Cost Analysis")]
            SMCostAnalysis,
            [Description("General Ledger Opening Balance")]
            GLOpeningBalance,
            [Description("Inventory Opening Balance")]
            InvOpeningBalance,
            [Description("Fixed Assets")]
            FixedAssets,
            [Description("InvServicePackage")]
            InvServicePackage,
            [Description("InvServicePriceMatrix")]
            InvServicePriceMatrix,
            [Description("Service Bandwidth")]
            ServiceBandwidth,
            [Description("Inv Service Bandwidth Frequency")]
            InvServiceFrequency,
            [Description("Document Management Documents")]
            DMDocuments,
            [Description("LC CNF Info")]
            LCCnfInfo,
            [Description("Lost and Found")]
            LostNFound,
            [Description("VM Driver Information")]
            VMDriverInformation,
            [Description("VM Vehicle Information")]
            VMVehicleInformation,
            [Description("Hotel Repair And Maintenance")]
            HotelRepairNMaintenance,
            [Description("Template Information For Email, SMS, Letter")]
            TemplateInformation,
            [Description("Call To Action")]
            CallToAction

        }
    }
}
