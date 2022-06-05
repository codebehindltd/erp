using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Common
{
    public partial class HMReport : System.Web.UI.MasterPage
    {
        protected string isMenuCollupse = string.Empty;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        public string innBoardDateFormat = "", isOldMenuEnable = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            hmUtility.GetCurrentApplicationUserInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SiteTitle.InnerText = userInformationBO.SiteTitle;

            this.lblCurrentUser.Text = userInformationBO.UserId;
            this.CheckObjectPermission(userInformationBO.UserInfoId);

            MainMenu.InnerHtml = userInformationBO.UserMenu;
            ReportMenu.InnerHtml = userInformationBO.UserReportMenu;

            this.MenuCollupseOption();

            if (Session["ReportMenuShowHide"] != null)
                hfReportIsHidden.Value = (string)Session["ReportMenuShowHide"];
            else
                hfReportIsHidden.Value = "1";

            lblDayOpenDate.Text = userInformationBO.DayOpenDate.ToString(userInformationBO.ServerDateFormat);

            innBoardDateFormat = userInformationBO.ClientDateFormat;
            isOldMenuEnable = userInformationBO.IsOldMenuEnable;

            MessageCount();
        }
        private void MenuCollupseOption()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string currentFormName = ((url.Split('/').Last()).Split('?')[0]).Split('.')[0];

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userInformationBO.UserInfoId, currentFormName.ToUpper());
            if (objectPermissionBO.ObjectPermissionId > 0)
            {
                isMenuCollupse = (objectPermissionBO.ObjectGroupHead).Replace("grp", "menu");
            }
        }

        private void CheckObjectPermission(int userId)
        {
            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            List<ObjectPermissionBO> objectPermissionBOList = new List<ObjectPermissionBO>();
            objectPermissionBOList = objectPermissionDA.GetFormPermissionByUserId(userId, "Report");
            if (objectPermissionBOList.Count > 0)
            {
                foreach (ObjectPermissionBO row in objectPermissionBOList)
                {
                    string menuName = row.ObjectHead;
                    string groupName = row.ObjectGroupHead;
                    isViewPermission = row.IsViewPermission;

                    if (groupName.Equals("grpReportHotelManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportHotelManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportGeneralLedger"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportGeneralLedger.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportPayrollManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportPayrollManagement.Visible = row.IsViewPermission;
                        }
                    }

                    else if (groupName.Equals("grpReportRestaurant"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportRestaurant.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportPurchaseManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportPurchaseManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportInventoryManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportInventoryManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportBanquetManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportBanquetManagement.Visible = row.IsViewPermission;
                        }
                    }

                    else if (groupName.Equals("grpReportSalesManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportSalesManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportSalesNMarketing"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportSalesNMarketing.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportMembershipManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportMembershipManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("menuReportHMCommont"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportSalesManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpReportRecruitmentManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            this.grpReportRecruitmentManagement.Visible = row.IsViewPermission;
                        }
                    }


                    switch (menuName)
                    {

                        //HM Common
                        case "lifrmReportActivitiLogs":
                            this.lifrmReportActivitiLogs.Visible = isViewPermission;
                            break;
                        //Hotel Management---------------------------------------------------------------
                        case "liReportRoomStatusHistory":
                            this.liReportRoomStatusHistory.Visible = isViewPermission;
                            break;
                        case "liFrmReportRoomStatus":
                            this.liFrmReportRoomStatus.Visible = isViewPermission;
                            break;
                        case "liFrmReportRoomWiseStatus":
                            this.liFrmReportRoomWiseStatus.Visible = isViewPermission;
                            break;
                        case "liFrmReportRoomReservation":
                            this.liFrmReportRoomReservation.Visible = isViewPermission;
                            break;
                        case "liFrmCorporateGuestInfo":
                            this.liFrmCorporateGuestInfo.Visible = isViewPermission;
                            break;
                        case "liFrmPromotionalGuestInfo":
                            this.liFrmPromotionalGuestInfo.Visible = isViewPermission;
                            break;

                        case "liFrmGuestCountry":
                            this.liFrmGuestCountry.Visible = isViewPermission;
                            break;
                        case "liFrmOccupencyInfoByRoopType":
                            this.liFrmOccupencyInfoByRoopType.Visible = isViewPermission;
                            break;

                        case "liFrmReturnedGuest":
                            this.liFrmReturnedGuest.Visible = isViewPermission;
                            break;
                        case "liFrmGuestSource":
                            this.liFrmGuestSource.Visible = isViewPermission;
                            break;
                        case "liFrmReportRoomAvailableStatus":
                            this.liFrmReportRoomAvailableStatus.Visible = isViewPermission;
                            break;

                        case "liFrmReportDivisionRevenue":
                            this.liFrmReportDivisionRevenue.Visible = isViewPermission;
                            break;   //
                        case "liFrmReportRoomSalesRevenue":
                            this.liFrmReportRoomSalesRevenue.Visible = isViewPermission;
                            break;
                        case "liFrmReportBCDivisionRevenue":
                            this.liFrmReportBCDivisionRevenue.Visible = isViewPermission;
                            break;
                        case "liFrmReportBCRoomSalesRevenue":
                            this.liFrmReportBCRoomSalesRevenue.Visible = isViewPermission;
                            break;
                        case "liFrmReportDiscountVsActualSales":
                            this.liFrmReportDiscountVsActualSales.Visible = isViewPermission;
                            break;
                        case "liFrmReportRoomOccupencyAnalysis":
                            this.liFrmReportRoomOccupencyAnalysis.Visible = isViewPermission;
                            break;
                        case "liFrmServiceAndRoomBillByDate":
                            this.liFrmServiceAndRoomBillByDate.Visible = isViewPermission;
                            break;
                        case "liFrmReportTimeSlabInfo":
                            this.liFrmReportTimeSlabInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestHouseInfo":
                            this.liFrmReportGuestHouseInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportCanceledReservation":
                            this.liFrmReportCanceledReservation.Visible = isViewPermission;
                            break;
                        case "liFrmReportInHouseGuestLedger":
                            this.liFrmReportInHouseGuestLedger.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestLedgerTranscript":
                            this.liFrmReportGuestLedgerTranscript.Visible = isViewPermission;
                            break;
                        case "liFrmReportSalesInfo":
                            this.liFrmReportSalesInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportFrontOfficeCash":
                            this.liFrmReportFrontOfficeCash.Visible = isViewPermission;
                            break;
                        case "liFrmReportHouseKeeping":
                            this.liFrmReportHouseKeeping.Visible = isViewPermission;
                            break;
                        case "liFrmReportRoomShiftInfo":
                            this.liFrmReportRoomShiftInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportSalesAudit":
                            this.liFrmReportSalesAudit.Visible = isViewPermission;
                            break;
                        case "liFrmReportSalesSummary":
                            this.liFrmReportSalesSummary.Visible = isViewPermission;
                            break;
                        case "liFrmReportDayUse":
                            this.liFrmReportDayUse.Visible = isViewPermission;
                            break;
                        case "liFrmReportLateCheckOut":
                            this.liFrmReportLateCheckOut.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestPaymentInfo":
                            this.liFrmReportGuestPaymentInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestInformationDetails":
                            this.liFrmReportGuestInformationDetails.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestAirportPickupDropInfo":
                            this.liFrmReportGuestAirportPickupDropInfo.Visible = isViewPermission;
                            break;
                        case "lifrmReportRoomReservationNoShow":
                            this.lifrmReportRoomReservationNoShow.Visible = isViewPermission;
                            break;
                        case "lifrmReservationBillTransaction":
                            this.lifrmReservationBillTransaction.Visible = isViewPermission;
                            break;
                        case "liFrmReportMonthToDateInfo":
                            this.liFrmReportMonthToDateInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportServiceBillTransfer":
                            this.liFrmReportServiceBillTransfer.Visible = isViewPermission;
                            break;
                        case "liFrmCurrencyTransaction":
                            this.liFrmCurrencyTransaction.Visible = isViewPermission;
                            break;
                        case "liFrmComplimentaryGuest":
                            this.liFrmComplimentaryGuest.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestTotalStayovers":
                            this.liFrmReportGuestTotalStayovers.Visible = isViewPermission;
                            break;
                        case "liFrmVIPGuest":
                            this.liFrmVIPGuest.Visible = isViewPermission;
                            break;
                        case "liFrmGuestBirthday":
                            this.liFrmGuestBirthday.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestCreditCard":
                            this.liFrmReportGuestCreditCard.Visible = isViewPermission;
                            break;
                        case "liFrmReportAdvReservationForecast":
                            this.liFrmReportAdvReservationForecast.Visible = isViewPermission;
                            break;
                        //General Ledger----------------------------------------------------------------- 
                        case "liFrmReportChartOfAccounts":
                            this.liFrmReportChartOfAccounts.Visible = isViewPermission;
                            break;
                        case "liFrmReportGeneralLedger":
                            this.liFrmReportGeneralLedger.Visible = isViewPermission;
                            break;
                        case "liFrmReportChequeList":
                            this.liFrmReportChequeList.Visible = isViewPermission;
                            break;
                        case "liFrmReportJournalRegister":
                            this.liFrmReportJournalRegister.Visible = isViewPermission;
                            break;
                        case "liFrmReportReceiveNPayment":
                            this.liFrmReportReceiveNPayment.Visible = isViewPermission;
                            break;
                        case "liFrmReportCashFlowStatement":
                            this.liFrmReportCashFlowStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportProfitNLossStatement":
                            this.liFrmReportProfitNLossStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportCashBookStatement":
                            this.liFrmReportCashBookStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportBankBookStatement":
                            this.liFrmReportBankBookStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportTrailBalanceStatement":
                            this.liFrmReportTrailBalanceStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportBalanceSheetStatement":
                            this.liFrmReportBalanceSheetStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportReceivableNPayable":
                            this.liFrmReportReceivableNPayable.Visible = isViewPermission;
                            break;
                        case "liFrmReportBreakDownStatement":
                            this.liFrmReportBreakDownStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportFixedAssetsStatement":
                            this.liFrmReportFixedAssetsStatement.Visible = isViewPermission;
                            break;

                        //Payroll Management----------------------------------------------------------------- 
                        case "liFrmReportEmployeeInformation":
                            this.liFrmReportEmployeeInformation.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmployeeList":
                            this.liFrmReportEmployeeList.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmpRoster":
                            this.liFrmReportEmpRoster.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmpAttendance":
                            this.liFrmReportEmpAttendance.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmpSalarySheet":
                            this.liFrmReportEmpSalarySheet.Visible = isViewPermission;
                            break;
                        case "liFrmReportPayslip":
                            this.liFrmReportPayslip.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmpCV":
                            this.liFrmReportEmpCV.Visible = isViewPermission;
                            break;
                        case "liFrmTerminationLetter":
                            this.liFrmTerminationLetter.Visible = isViewPermission;
                            break;
                        case "liFrmRelieveLetter":
                            this.liFrmRelieveLetter.Visible = isViewPermission;
                            break;
                        case "liFrmReportDisciplinaryAction":
                            this.liFrmReportDisciplinaryAction.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmpTransfer":
                            this.liFrmReportEmpTransfer.Visible = isViewPermission;
                            break;
                        case "liFrmReportAppraisalEvaluation":
                            this.liFrmReportAppraisalEvaluation.Visible = isViewPermission;
                            break;
                        case "liFrmReportAppraisalEvaluationDetails":
                            this.liFrmReportAppraisalEvaluationDetails.Visible = isViewPermission;
                            break;
                        case "liFrmReportPF":
                            this.liFrmReportPF.Visible = isViewPermission;
                            break;
                        case "liFrmEmpLeaveReport":
                            this.liFrmEmpLeaveReport.Visible = isViewPermission;
                            break;
                        case "liFrmEmpLeaveInformation":
                            this.liFrmEmpLeaveInformation.Visible = isViewPermission;
                            break;
                        case "liFrmEmployeeOfTheYearMonth":
                            this.liFrmEmployeeOfTheYearMonth.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmpTraining":
                            this.liFrmReportEmpTraining.Visible = isViewPermission;
                            break;
                        case "liFrmReportStaffingBudget":
                            this.liFrmReportStaffingBudget.Visible = isViewPermission;
                            break;
                        case "liFrmReportStaffRequisition":
                            this.liFrmReportStaffRequisition.Visible = isViewPermission;
                            break;
                        case "liFrmEmployeeServiceChargeDistributionReport":
                            this.liFrmEmployeeServiceChargeDistributionReport.Visible = isViewPermission;
                            break;
                        case "liFrmEmpOvertime":
                            this.liFrmEmpOvertime.Visible = isViewPermission;
                            break;
                        case "liFrmReportGratuityApproval":
                            this.liFrmReportGratuityApproval.Visible = isViewPermission;
                            break;
                        case "liFrmEmpPromotion":
                            this.liFrmEmpPromotion.Visible = isViewPermission;
                            break;
                        case "liFrmReportPFMemberList":
                            this.liFrmReportPFMemberList.Visible = isViewPermission;
                            break;
                        case "liFrmReportPFStatement":
                            this.liFrmReportPFStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportPFMonthlyBalance":
                            this.liFrmReportPFMonthlyBalance.Visible = isViewPermission;
                            break;
                        case "liFrmReportPFLoanCollection":
                            this.liFrmReportPFLoanCollection.Visible = isViewPermission;
                            break;
                        case "liFrmReportPFReports":
                            this.liFrmReportPFReports.Visible = isViewPermission;
                            break;
                        case "liFrmReportGratuityEliEmpList":
                            this.liFrmReportGratuityEliEmpList.Visible = isViewPermission;
                            break;
                        case "liFrmReportGratuityMonthlyBalance":
                            this.liFrmReportGratuityMonthlyBalance.Visible = isViewPermission;
                            break;
                        case "liFrmReportGratuityStatement":
                            this.liFrmReportGratuityStatement.Visible = isViewPermission;
                            break;
                        case "liFrmBankSalaryInstruction":
                            this.liFrmBankSalaryInstruction.Visible = isViewPermission;
                            break;
                        case "liFrmEmployeeServiceChargeBankAdvice":
                            this.liFrmEmployeeServiceChargeBankAdvice.Visible = isViewPermission;
                            break;
                        case "liFrmOvertimeAnalysis":
                            this.liFrmOvertimeAnalysis.Visible = isViewPermission;
                            break;
                        case "liFrmEmpTaxDeduction":
                            this.liFrmEmpTaxDeduction.Visible = isViewPermission;
                            break;
                        case "liFrmReportEmpReconsilation":
                            this.liFrmReportEmpReconsilation.Visible = isViewPermission;
                            break;
                        case "liFrmReportReconcilationSummary":
                            this.liFrmReportReconcilationSummary.Visible = isViewPermission;
                            break;

                        //Restaurant Management-----------------------------------------------------------------
                        case "liFrmReportRestaurantSalesInfo":
                            this.liFrmReportRestaurantSalesInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportRestaurantTransaction":
                            this.liFrmReportRestaurantTransaction.Visible = isViewPermission;
                            break;
                        case "liFrmReportRestaurantCancelBill":
                            this.liFrmReportRestaurantCancelBill.Visible = isViewPermission;
                            break;
                        case "liFrmReportDailySalesStatement":
                            this.liFrmReportDailySalesStatement.Visible = isViewPermission;
                            break;
                        case "liFrmReportInvWiseRestaurantSalesInfo":
                            this.liFrmReportInvWiseRestaurantSalesInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportDateWiseSalesSummary":
                            this.liFrmReportDateWiseSalesSummary.Visible = isViewPermission;
                            break;
                        //Banquet Management-----------------------------------------------------------------
                        case "liFrmReportBanquetClientRevenue":
                            this.liFrmReportBanquetClientRevenue.Visible = isViewPermission;
                            break;
                        case "liFrmReportBanquetHalRevenue":
                            this.liFrmReportBanquetHalRevenue.Visible = isViewPermission;
                            break;
                        case "liFrmReportBanquetOccessionTypeRevenue":
                            this.liFrmReportBanquetOccessionTypeRevenue.Visible = isViewPermission;
                            break;
                        case "liFrmReportBanquetReferenceRevenue":
                            this.liFrmReportBanquetReferenceRevenue.Visible = isViewPermission;
                            break;  //
                        case "liFrmReportReservationBillInfo":
                            this.liFrmReportReservationBillInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportBanquetRevenueInfo":
                            this.liFrmReportBanquetRevenueInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportBanquetReservationInfo":
                            this.liFrmReportBanquetReservationInfo.Visible = isViewPermission;
                            break;                        
                        case "liFrmBanquetSalesInfo":
                            this.liFrmBanquetSalesInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportBanquetTransaction":
                            this.liFrmReportBanquetTransaction.Visible = isViewPermission;
                            break;
                        case "liFrmCanceledReservationInfo":
                            this.liFrmCanceledReservationInfo.Visible = isViewPermission;
                            break;

                        // Purchase Management
                        case "liFrmReportSupplierInformation":
                            this.liFrmReportSupplierInformation.Visible = isViewPermission;
                            break;
                        case "liFrmReportDateWisePurchaseInformation":
                            this.liFrmReportDateWisePurchaseInformation.Visible = isViewPermission;
                            break;
                        case "liFrmReportItemWisePurchaseInformation":
                            this.liFrmReportItemWisePurchaseInformation.Visible = isViewPermission;
                            break;
                        case "liFrmReportSupplierWisePurchaseInformation":
                            this.liFrmReportSupplierWisePurchaseInformation.Visible = isViewPermission;
                            break;
                        case "liFrmReportPurchaseOrderInformation":
                            this.liFrmReportPurchaseOrderInformation.Visible = isViewPermission;
                            break;
                        case "liFrmReportPurchaseReturn":
                            this.liFrmReportPurchaseReturn.Visible = isViewPermission;
                            break;
                        case "liFrmReportSupplierStatement":
                            this.liFrmReportSupplierStatement.Visible = isViewPermission;
                            break;

                        // Inventory Management
                        case "liFrmReportItemWiseStock":
                            this.liFrmReportItemWiseStock.Visible = isViewPermission;
                            break;
                        case "liFrmReportProductReceiveInfo":
                            this.liFrmReportProductReceiveInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportRoomInventory":
                            this.liFrmReportRoomInventory.Visible = isViewPermission;
                            break;
                        case "lifrmReportInvItemInfo":
                            this.lifrmReportInvItemInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportItemRecipe":
                            this.liFrmReportItemRecipe.Visible = isViewPermission;
                            break;
                        case "liFrmReportInvStockVariance":
                            this.liFrmReportInvStockVariance.Visible = isViewPermission;
                            break;
                        case "liFrmReportInvCOSDetails":
                            this.liFrmReportInvCOSDetails.Visible = isViewPermission;
                            break;
                        //case "liFrmReportInvItemUsage":
                        //    this.liFrmReportInvItemUsage.Visible = isViewPermission;
                        //    break;
                        case "liFrmReportInvVarianceInfo":
                            this.liFrmReportInvVarianceInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportConsolidatedMenuItemSalesDetail":
                            this.liFrmReportConsolidatedMenuItemSalesDetail.Visible = isViewPermission;
                            break;
                        case "liFrmReportrptConsolidatedMenuItemSalesSummary":
                            this.liFrmReportrptConsolidatedMenuItemSalesSummary.Visible = isViewPermission;
                            break;
                        case "liFrmInventoryItemUsageAnalysis":
                            this.liFrmInventoryItemUsageAnalysis.Visible = isViewPermission;
                            break;
                        case "liFrmDailyConsolidatedRevenueCenterSalesDetail":
                            this.liFrmDailyConsolidatedRevenueCenterSalesDetail.Visible = isViewPermission;
                            break;
                        case "liFrmDailyConsolidatedRevenueCostCenterSalesDetailByDineTime":
                            this.liFrmDailyConsolidatedRevenueCostCenterSalesDetailByDineTime.Visible = isViewPermission;
                            break;
                        case "liFrmReportDatewisePurchaseComparison":
                            this.liFrmReportDatewisePurchaseComparison.Visible = isViewPermission;
                            break;
                        // Sales Management    
                        case "liFrmReportSalesInformation":
                            this.liFrmReportSalesInformation.Visible = isViewPermission;
                            break;
                        case "liFrmReportItemWiseSalesInfo":
                            this.liFrmReportItemWiseSalesInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportCustomerWiseSalesInfo":
                            this.liFrmReportCustomerWiseSalesInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportSalesReturn":
                            this.liFrmReportSalesReturn.Visible = isViewPermission;
                            break;
                        case "liFrmReportCustomerInfo":
                            this.liFrmReportCustomerInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportServiceWiseSales":
                            this.liFrmReportServiceWiseSales.Visible = isViewPermission;
                            break;
                        // Sales & marketing
                        case "liFrmReportGuestCompanyInfo":
                            this.liFrmReportGuestCompanyInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestReferenceInfo":
                            this.liFrmReportGuestReferenceInfo.Visible = isViewPermission;
                            break;
                        case "liFrmReportSalesNMarketing":
                            this.liFrmReportSalesNMarketing.Visible = isViewPermission;
                            break;
                        case "liFrmReportGuestInformation":
                            this.liFrmReportGuestInformation.Visible = isViewPermission;
                            break;
                        // Membership    
                        case "liFrmReportMemberList":
                            this.liFrmReportMemberList.Visible = isViewPermission;
                            break;
                        //Recruitment management------------------------------------------------------------
                        case "liFrmReportApplicantList":
                            this.liFrmReportApplicantList.Visible = isViewPermission;
                            break;
                        case "liFrmReportApplicantResume":
                            this.liFrmReportApplicantResume.Visible = isViewPermission;
                            break;
                    }
                }
            }
            else
            {
                Response.Redirect("/HMCommon/frmHMHome.aspx");
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Redirect("/HotelManagement/frmSearchGuest.aspx?SearchText=" + this.SearchTextBox.Text);
        }
        public void MessageCount()
        {
            Int16 TotalUnreadMessage = 0;

            CommonMessageDA messageDa = new CommonMessageDA();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            messageDetails = messageDa.GetMessageDetailsByUserId(userInformationBO.UserId, null, 10, out TotalUnreadMessage);

            lblMessageCount.Text = TotalUnreadMessage.ToString();

            if (TotalUnreadMessage > 0)
            {
                MessageCountBadge.Attributes.Add("style", " background-color:#18cde6;");
            }

            int rowCount = 0;
            string messageBrief = string.Empty;
            string time = string.Empty, mDate = string.Empty;
            string readenMessageColor = "#E5E5E5";

            foreach (CommonMessageDetailsBO md in messageDetails)
            {
                time = md.MessageDate.ToString("hh:mm tt");
                mDate = md.MessageDate.ToString("MMM dd");

                if (rowCount > 0)
                {
                    messageBrief += "<li class='divider' style='margin: 2px 1px;'></li>";
                }

                readenMessageColor = md.IsReaden ? "#F5F5F5" : string.Empty;

                //EncryptionHelper encryptionHelper = new EncryptionHelper();
                //string encryptData = encryptionHelper.Encrypt(md.MessageId.ToString() + "," + md.MessageDetailsId.ToString());

                messageBrief += "<li style='background-color:" + readenMessageColor + ";'>" +
                                "<a tabindex='-1' style = 'margin: 3px 5px 5px 1px; padding:0;' href='/HMCommon/frmCommonMessageDetails.aspx?mid=" + md.MessageId.ToString() + "," + md.MessageDetailsId.ToString() + "'>" +
                                 "<div class='row-fluid'>" +
                                 "<div class='span3' style= 'min-height:19px; margin-left: 1%;'>" + md.UserName + "</div>" +
                                 "<div class='span7' style= 'min-height:19px; margin-left: 1%;'>" + md.Subjects +
                                 " - " + (md.MessageBody.Length > 10 ? (md.MessageBody.Substring(0, 10) + " ...") : md.MessageBody) +
                                 "</div>" +
                                 "<div class='span2' style= 'min-height:19px; margin-left: 1%;'>" + mDate + " " + time + "</div>" +
                                 "</div>" +
                                "</a></li>";

                rowCount++;
            }

            if (string.IsNullOrEmpty(messageBrief))
            {
                messageBrief += "<li><a tabindex='-1' style = 'margin: 3px 5px 5px 1px; padding:0;' href='javascript:void()'>" +
                                    "<div class='row-fluid'>" +
                                    "<div class='span12'>No Message in Message Box</div>" +
                                    "</div>" +
                                   "</a></li>";
            }

            MessageBriefDescription.InnerHtml = messageBrief;
        }
    }
}