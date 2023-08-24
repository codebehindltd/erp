using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace Mamun.Presentation.Website.Common
{
    public partial class HM : System.Web.UI.MasterPage
    {
        protected string isMenuCollupse = string.Empty;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        public string innBoardDateFormat = string.Empty, isOldMenuEnable = string.Empty;
        protected int isSoftwareLicenseExpiredNotificationMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SoftwareLicenseExpiredNotification"] != null)
            {
                isSoftwareLicenseExpiredNotificationMessageBoxEnable = 1;
                //lblSoftwareLicenseExpiredNotificationMessage.Text = "Your License Will Expire Soon, Please Contact with Data Grid Limited.";
                lblSoftwareLicenseExpiredNotificationMessage.Text = "Your License Will Expire Soon, Please Contact with Administrator.";
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO != null)
            {
                SiteTitle.Text = userInformationBO.SiteTitle;

                lblCurrentUser.Text = userInformationBO.UserId;
                lblDayOpenDate.Text = userInformationBO.DayOpenDate.ToString(userInformationBO.ServerDateFormat);
                CheckObjectPermission(userInformationBO.UserInfoId);

                MainMenu.InnerHtml = userInformationBO.UserMenu;
                ReportMenu.InnerHtml = userInformationBO.UserReportMenu;

                MenuCollupseOption();

                hfMessageHideTime.Value = userInformationBO.MessageHideTimer.ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if (Session["HomeMenuShowHide"] != null)
                hfHomeIsHidden.Value = (string)Session["HomeMenuShowHide"];
            else
                hfHomeIsHidden.Value = "1";
            innBoardDateFormat = userInformationBO.ClientDateFormat;
            isOldMenuEnable = userInformationBO.IsOldMenuEnable;

            MessageCount();
        }
        private void MenuCollupseOption()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string currentFormName = ((url.Split('/').Last()).Split('?')[0]).Split('.')[0];
            if (!string.IsNullOrWhiteSpace(currentFormName))
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
                ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
                objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userInformationBO.UserInfoId, currentFormName.ToUpper());
                if (objectPermissionBO.ObjectPermissionId > 0)
                {
                    //class="nav nav-list collapse in"
                    isMenuCollupse = (objectPermissionBO.ObjectGroupHead).Replace("grp", "menu");
                }
            }
        }
        private void CheckObjectPermission(int userId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            List<ObjectPermissionBO> objectPermissionBOList = new List<ObjectPermissionBO>();
            objectPermissionBOList = objectPermissionDA.GetFormPermissionByUserId(userId, "Form");
            if (objectPermissionBOList.Count > 0)
            {
                foreach (ObjectPermissionBO row in objectPermissionBOList)
                {
                    string menuName = row.ObjectHead;
                    string groupName = row.ObjectGroupHead;
                    isViewPermission = row.IsViewPermission;

                    if (groupName.Equals("grpDashboard"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpDashboard.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpHotelManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpHotelManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpRestaurantManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpRestaurantManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpGeneralLedger"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpGeneralLedger.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpPayrollManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpPayrollManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpInventoryManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpInventoryManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpPurchaseManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpPurchaseManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpSalesManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpSalesManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpUserPanel"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpUserPanel.Visible = row.IsViewPermission;
                        }
                    }

                    else if (groupName.Equals("grpBanquetManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpBanquetManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpCostofGoodsSold"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpCostofGoodsSold.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpSalesMarketingManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpSalesMarketingManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpRecruitmentManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpRecruitmentManagement.Visible = row.IsViewPermission;
                        }
                    }
                    else if (groupName.Equals("grpMembershipManagement"))
                    {
                        if (row.IsViewPermission)
                        {
                            grpMembershipManagement.Visible = row.IsViewPermission;
                        }
                    }

                    switch (menuName)
                    {
                        //grpDashboard: Company Information----------------------------------------------
                        case "liFrmCompany":
                            liFrmCompany.Visible = isViewPermission;
                            break;
                        case "liFrmCostCentre":
                            liFrmCostCentre.Visible = isViewPermission;
                            break;
                        case "liFrmPrinterInfo":
                            liFrmPrinterInfo.Visible = isViewPermission;
                            break;
                        case "liFrmBank":
                            liFrmBank.Visible = isViewPermission;
                            break;
                        case "liFrmEmpOverTime":
                            liFrmEmpOverTime.Visible = isViewPermission;
                            break;
                        case "liFrmBusinessPromotion":
                            liFrmBusinessPromotion.Visible = isViewPermission;
                            break;
                        case "liFrmHMCommonSetup":
                            liFrmHMCommonSetup.Visible = isViewPermission;
                            break;
                        case "liFrmCommonCurrency":
                            liFrmCommonCurrency.Visible = isViewPermission;
                            break;
                        case "liFrmCurrencyConversion":
                            liFrmCurrencyConversion.Visible = isViewPermission;
                            break;
                        case "liFrmCurrencyTransaction":
                            liFrmCurrencyTransaction.Visible = isViewPermission;
                            break;
                        case "liFrmMessageMail":
                            liFrmMessageMail.Visible = isViewPermission;
                            break;
                        case "liFrmDatabaseBackup":
                            liFrmDatabaseBackup.Visible = isViewPermission;
                            break;
                        case "liFrmDashboard":
                            liFrmDashboard.Visible = isViewPermission;
                            break;
                        case "liFrmVatAdjustment":
                            liFrmVatAdjustment.Visible = isViewPermission;
                            break;
                        case "liFrmDashboardPermission":
                            liFrmDashboardPermission.Visible = isViewPermission;
                            break;

                        //Hotel Management---------------------------------------------------------------   
                        case "liFrmHotelConfiguration":
                            liFrmHotelConfiguration.Visible = isViewPermission;
                            break;
                        case "liFrmSearchGuest":
                            liFrmSearchGuest.Visible = isViewPermission;
                            break;
                        case "liFrmGuestServiceInfo":
                            liFrmGuestServiceInfo.Visible = isViewPermission;
                            break;
                        case "liFrmHMFloor":
                            liFrmHMFloor.Visible = isViewPermission;
                            break;
                        case "liFrmRoomType":
                            liFrmRoomType.Visible = isViewPermission;
                            break;
                        case "liFrmRoomNumber":
                            liFrmRoomNumber.Visible = isViewPermission;
                            break;
                        case "liFrmRoomOwner":
                            liFrmRoomOwner.Visible = false; //isViewPermission;
                            break;
                        case "liFrmRoomStatusInfo":
                            liFrmRoomStatusInfo.Visible = isViewPermission;
                            break;
                        case "liFrmRoomAllocation":
                            liFrmRoomAllocation.Visible = false;
                            break;
                        case "liFrmFloorWiseRoomAllocation":
                            liFrmFloorWiseRoomAllocation.Visible = false;
                            break;
                        case "liFrmManageOnlineRoomReservation":
                            liFrmManageOnlineRoomReservation.Visible = isViewPermission;
                            break;
                        case "liFrmRoomReservation":
                            liFrmRoomReservation.Visible = isViewPermission;
                            break;
                        case "liFrmReservationBillPayment":
                            liFrmReservationBillPayment.Visible = isViewPermission;
                            break;
                        case "liFrmRoomRegistration":
                            liFrmRoomRegistration.Visible = isViewPermission;
                            break;
                        case "liFrmRoomShift":
                            liFrmRoomShift.Visible = isViewPermission;
                            break;
                        case "liFrmGHServiceBill":
                            liFrmGHServiceBill.Visible = isViewPermission;
                            break;
                        case "liFrmServiceBillTransfer":
                            liFrmServiceBillTransfer.Visible = isViewPermission;
                            break;
                        case "liFrmBillAdjustment":
                            liFrmBillAdjustment.Visible = isViewPermission;
                            break;
                        case "liFrmNightAudit":
                            liFrmNightAudit.Visible = isViewPermission;
                            break;
                        case "liFrmGuestBillPayment":
                            liFrmGuestBillPayment.Visible = isViewPermission;
                            break;
                        case "liFrmRoomCheckOut":
                            liFrmRoomCheckOut.Visible = isViewPermission;
                            break;
                        case "liFrmHMFloorManagement":
                            liFrmHMFloorManagement.Visible = isViewPermission;
                            break;
                        case "liFrmRoomCleanup":
                            liFrmRoomCleanup.Visible = isViewPermission;
                            break;
                        case "liFrmRoomCalender":
                            liFrmRoomCalender.Visible = isViewPermission;
                            break;
                        case "liFrmHMComplementaryItem":
                            liFrmHMComplementaryItem.Visible = isViewPermission;
                            break;
                        case "liFrmHMDayClose":
                            liFrmHMDayClose.Visible = isViewPermission;
                            break;
                        case "liFrmGuestCheckIn":
                            liFrmGuestCheckIn.Visible = isViewPermission;
                            break;
                        case "liFrmGuestCheckOut":
                            liFrmGuestCheckOut.Visible = isViewPermission;
                            break;
                        case "liFrmApproveRoomReservationNoShow":
                            liFrmApproveRoomReservationNoShow.Visible = isViewPermission;
                            break;
                        case "liFrmDiscountPolicy":
                            liFrmDiscountPolicy.Visible = isViewPermission;
                            break;
                        case "liFrmGuestPreference":
                            liFrmGuestPreference.Visible = isViewPermission;
                            break;
                        //case "liFrmFrontOfficePS":
                        //    liFrmFrontOfficePS.Visible = isViewPermission;
                        //    break;

                        //Restaurant Management---------------------------------------------------------------
                        case "liFrmRestaurantConfiguration":
                            liFrmRestaurantConfiguration.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantKitchen":
                            liFrmRestaurantKitchen.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantTable":
                            liFrmRestaurantTable.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantTableManagement":
                            liFrmRestaurantTableManagement.Visible = isViewPermission;
                            break;
                        case "liFrmFrontOfficeUser":
                            liFrmFrontOfficeUser.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantBearer":
                            liFrmRestaurantBearer.Visible = isViewPermission;
                            break;
                        //case "liFrmRestaurantItem":
                        //    liFrmRestaurantItem.Visible = isViewPermission;
                        //    break;
                        case "liFrmRestaurantCombo":
                            liFrmRestaurantCombo.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantBuffet":
                            liFrmRestaurantBuffet.Visible = isViewPermission;
                            break;
                        case "liFrmCostCenterChooseForKot":
                            liFrmCostCenterChooseForKot.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantBill":
                            liFrmRestaurantBill.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantBillSearch":
                            liFrmRestaurantBillSearch.Visible = isViewPermission;
                            break;
                        case "liFrmRestaurantReservation":
                            liFrmRestaurantReservation.Visible = isViewPermission;
                            break;
                        case "liFrmFoodDisPatch":
                            liFrmFoodDisPatch.Visible = isViewPermission;
                            break;
                        case "liFrmPayFirstRestaurantBill":
                            liFrmPayFirstRestaurantBill.Visible = isViewPermission;
                            break;
                        //case "liFrmRestaurantPS":
                        //    liFrmRestaurantPS.Visible = isViewPermission;
                        //    break;
                        case "liFrmRestaurantTableCalendar":
                            liFrmRestaurantTableCalendar.Visible = isViewPermission;
                            break;

                        //Banquet Management------------------------------------
                        case "liFrmBanquetTheme":
                            liFrmBanquetTheme.Visible = isViewPermission;
                            break;
                        case "liFrmSeatingPlan":
                            liFrmSeatingPlan.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetRequisites":
                            liFrmBanquetRequisites.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetInformation":
                            liFrmBanquetInformation.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetReservation":
                            liFrmBanquetReservation.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetBillPayment":
                            liFrmBanquetBillPayment.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetRefference":
                            liFrmBanquetRefference.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetReservationCancel":
                            liFrmBanquetReservationCancel.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetCalendar":
                            liFrmBanquetCalendar.Visible = isViewPermission;
                            break;
                        case "liFrmBanquetBillSettlement":
                            liFrmBanquetBillSettlement.Visible = isViewPermission;
                            break;
                        case "liBanquetBillSearch":
                            liBanquetBillSearch.Visible = isViewPermission;
                            break;
                        //General Ledger---------------------------------------------------------------
                        case "liFrmNodeMatrix":
                            liFrmNodeMatrix.Visible = isViewPermission;
                            break;
                        case "liFrmVoucher":
                            liFrmVoucher.Visible = isViewPermission;
                            break;
                        case "liFrmGLCompany":
                            liFrmGLCompany.Visible = isViewPermission;
                            break;
                        case "liFrmGLProject":
                            liFrmGLProject.Visible = isViewPermission;
                            break;
                        case "liFrmGLConfiguration":
                            liFrmGLConfiguration.Visible = isViewPermission;
                            break;
                        case "liFrmVoucherSearch":
                            liFrmVoucherSearch.Visible = isViewPermission;
                            break;
                        case "liFrmGLFixedAssets":
                            liFrmGLFixedAssets.Visible = isViewPermission;
                            break;
                        //Inventory Managment---------------------------------------------------------------
                        case "liFrmInvLocation":
                            liFrmInvLocation.Visible = isViewPermission;
                            break;
                        case "liFrmInvCategory":
                            liFrmInvCategory.Visible = isViewPermission;
                            break;
                        case "liFrmInvItem":
                            liFrmInvItem.Visible = isViewPermission;
                            break;
                        case "liFrmInvItemSpecialRemarks":
                            liFrmInvItemSpecialRemarks.Visible = isViewPermission;
                            break;
                        case "liFrmInvFinishedProduct":
                            liFrmInvFinishedProduct.Visible = isViewPermission;
                            break;
                        case "liFrmInvFinishedProductApproval":
                            liFrmInvFinishedProductApproval.Visible = isViewPermission;
                            break;
                        case "liFrmInvStockVariance":
                            liFrmInvStockVariance.Visible = isViewPermission;
                            break;
                        case "liFrmInvStockVarianceApproval":
                            liFrmInvStockVarianceApproval.Visible = isViewPermission;
                            break;
                        case "liFrmPMProductReceive":
                            liFrmPMProductReceive.Visible = isViewPermission;
                            break;
                        case "liFrmProductReceiveApproval":
                            liFrmProductReceiveApproval.Visible = isViewPermission;
                            break;
                        case "liFrmPMProductOut":
                            liFrmPMProductOut.Visible = isViewPermission;
                            break;
                        case "liFrmProductOut":
                            liFrmProductOut.Visible = isViewPermission;
                            break;
                        case "liFrmProductOutForRoom":
                            liFrmProductOutForRoom.Visible = isViewPermission;
                            break;
                        case "liFrmInvStockAdjustment":
                            liFrmInvStockAdjustment.Visible = isViewPermission;
                            break;

                        case "liFrmInvManufacturer":
                            liFrmInvManufacturer.Visible = isViewPermission;
                            break;
                        case "liFrmUnitHead":
                            liFrmUnitHead.Visible = isViewPermission;
                            break;
                        case "liFrmUnitConversionRate":
                            liFrmUnitConversionRate.Visible = isViewPermission;
                            break;
                        //Purchase Managment---------------------------------------------------------------                      
                        case "liFrmPMSupplier":
                            liFrmPMSupplier.Visible = isViewPermission;
                            break;
                        case "liFrmPMRequisition":
                            liFrmPMRequisition.Visible = isViewPermission;
                            break;
                        case "liFrmPMRequisitionApproval":
                            liFrmPMRequisitionApproval.Visible = isViewPermission;
                            break;
                        case "liFrmPMProductPO":
                            liFrmPMProductPO.Visible = isViewPermission;
                            break;
                        case "liFrmPMProductPOApproval":
                            liFrmPMProductPOApproval.Visible = isViewPermission;
                            break;
                        case "liFrmPMPurchaseReturn":
                            liFrmPMPurchaseReturn.Visible = isViewPermission;
                            break;
                        case "liFrmPMServicePO":
                            liFrmPMServicePO.Visible = isViewPermission;
                            break;
                        case "liFrmPMServicePOApproval":
                            liFrmPMServicePOApproval.Visible = isViewPermission;
                            break;
                        //Sales Managment---------------------------------------------------------------
                        case "liFrmSalesService":
                            liFrmSalesService.Visible = isViewPermission;
                            break;
                        case "liFrmServiceBundle":
                            liFrmServiceBundle.Visible = isViewPermission;
                            break;
                        case "liFrmSalesCustomer":
                            liFrmSalesCustomer.Visible = isViewPermission;
                            break;

                        case "liFrmSalesBandwidthInfo":
                            liFrmSalesBandwidthInfo.Visible = isViewPermission;
                            break;

                        case "liFrmPMSales":
                            liFrmPMSales.Visible = isViewPermission;
                            break;
                        case "liFrmPMSalesReturn":
                            liFrmPMSalesReturn.Visible = isViewPermission;
                            break;
                        case "liFrmPMSalesInvoice":
                            liFrmPMSalesInvoice.Visible = isViewPermission;
                            break;
                        case "liFrmPMSalesBillPayment":
                            liFrmPMSalesBillPayment.Visible = isViewPermission;
                            break;
                        case "liFrmSearchInvoice":
                            liFrmSearchInvoice.Visible = isViewPermission;
                            break;
                        case "liFrmSalesContractDetails":
                            liFrmSalesContractDetails.Visible = isViewPermission;
                            break;
                        //Payroll Management---------------------------------------------------------------    
                        case "liFrmPayrollConfiguration":
                            liFrmPayrollConfiguration.Visible = isViewPermission;
                            break;
                        case "liFrmDepartment":
                            liFrmDepartment.Visible = isViewPermission;
                            break;
                        case "liFrmEmpGrade":
                            liFrmEmpGrade.Visible = isViewPermission;
                            break;
                        case "liFrmSalaryHead":
                            liFrmSalaryHead.Visible = isViewPermission;
                            break;
                        case "liFrmEmpType":
                            liFrmEmpType.Visible = isViewPermission;
                            break;
                        case "liFrmDesignation":
                            liFrmDesignation.Visible = isViewPermission;
                            break;
                        case "liFrmEmployee":
                            liFrmEmployee.Visible = isViewPermission;
                            break;
                        case "liFrmEmpChangePassword":
                            liFrmEmpChangePassword.Visible = isViewPermission;
                            break;
                        case "liFrmLeaveType":
                            liFrmLeaveType.Visible = isViewPermission;
                            break;
                        case "liFrmEmpYearlyLeave":
                            liFrmEmpYearlyLeave.Visible = isViewPermission;
                            break;
                        case "liFrmLeaveInformation":
                            liFrmLeaveInformation.Visible = isViewPermission;
                            break;
                        case "liFrmSalaryFormula":
                            liFrmSalaryFormula.Visible = isViewPermission;
                            break;
                        case "liFrmEmpIncrement":
                            liFrmEmpIncrement.Visible = isViewPermission;
                            break;
                        case "liFrmWorkingPlan":
                            liFrmWorkingPlan.Visible = isViewPermission;
                            break;
                        case "liFrmEmpResignationInfo":
                            liFrmEmpResignationInfo.Visible = isViewPermission;
                            break;
                        //case "liFrmEmpPayScale":
                        //    liFrmEmpPayScale.Visible = isViewPermission;
                        //    break;
                        //case "liFrmAllowanceDeductionHead":
                        //    liFrmAllowanceDeductionHead.Visible = isViewPermission;
                        //    break;
                        case "liFrmEmpAllowanceDeduction":
                            liFrmEmpAllowanceDeduction.Visible = isViewPermission;
                            break;
                        case "liFrmDeviceAttendanceSynchronization":
                            liFrmDeviceAttendanceSynchronization.Visible = isViewPermission;
                            break;
                        case "liFrmRosterHead":
                            liFrmRosterHead.Visible = isViewPermission;
                            break;
                        case "liFrmTimeSlabHead":
                            liFrmTimeSlabHead.Visible = isViewPermission;
                            break;
                        case "liFrmEmpRoster":
                            liFrmEmpRoster.Visible = isViewPermission;
                            break;
                        case "liFrmEmpAttendance":
                            liFrmEmpAttendance.Visible = isViewPermission;
                            break;
                        case "liFrmAttendanceSynchronization":
                            liFrmAttendanceSynchronization.Visible = isViewPermission;
                            break;
                        case "liFrmEmpLeaveAdjustment":
                            liFrmEmpLeaveAdjustment.Visible = isViewPermission;
                            break;
                        case "liFrmEmpSalaryProcess":
                            liFrmEmpSalaryProcess.Visible = isViewPermission;
                            break;
                        case "liFrmPayrollHoliday":
                            liFrmPayrollHoliday.Visible = isViewPermission;
                            break;
                        case "liFrmEmpAdvanceTaken":
                            liFrmEmpAdvanceTaken.Visible = isViewPermission;
                            break;
                        case "liFrmLoan":
                            liFrmLoan.Visible = isViewPermission;
                            break;
                        case "liFrmLoanCollectionSearch":
                            liFrmLoanCollectionSearch.Visible = isViewPermission;
                            break;
                        case "liFrmEmpLoanSearch":
                            liFrmEmpLoanSearch.Visible = isViewPermission;
                            break;
                        case "liFrmApprMarksIndicator":
                            liFrmApprMarksIndicator.Visible = isViewPermission;
                            break;
                        case "liFrmApprRatingFactor":
                            liFrmApprRatingFactor.Visible = isViewPermission;
                            break;

                        case "liFrmEmpAppraisalCriteriaSetup":
                            liFrmEmpAppraisalCriteriaSetup.Visible = isViewPermission;
                            break;

                        case "liFrmApprEvatuation":
                            liFrmApprEvatuation.Visible = isViewPermission;
                            break;
                        case "liFrmEmpTaxDeduction":
                            liFrmEmpTaxDeduction.Visible = isViewPermission;
                            break;
                        case "liFrmOrganizer":
                            liFrmOrganizer.Visible = isViewPermission;
                            break;
                        case "liFrmEmptrainingType":
                            liFrmEmptrainingType.Visible = isViewPermission;
                            break;
                        case "liFrmEmpTraining":
                            liFrmEmpTraining.Visible = isViewPermission;
                            break;
                        case "liEmpLastMonthBenifit":
                            liEmpLastMonthBenifit.Visible = isViewPermission;
                            break;
                        case "liFrmDiscipinaryAction":
                            liFrmDiscipinaryAction.Visible = isViewPermission;
                            break;
                        case "liFrmDisciplinaryActionType":
                            liFrmDisciplinaryActionType.Visible = isViewPermission;
                            break;
                        case "liFrmDisciplinaryActionReason":
                            liFrmDisciplinaryActionReason.Visible = isViewPermission;
                            break;
                        case "liFrmEmpLeaveApplication":
                            liFrmEmpLeaveApplication.Visible = isViewPermission;
                            break;
                        case "liFrmEmpTransfer":
                            liFrmEmpTransfer.Visible = isViewPermission;
                            break;
                        case "liFrmLeaveOpening":
                            liFrmLeaveOpening.Visible = isViewPermission;
                            break;
                        case "liFrmLeaveBalanceClosing":
                            liFrmLeaveBalanceClosing.Visible = isViewPermission;
                            break;
                        case "liFrmServiceChargeDistribution":
                            liFrmServiceChargeDistribution.Visible = isViewPermission;
                            break;
                        case "liFrmServiceChargeConfiguration":
                            liFrmServiceChargeConfiguration.Visible = isViewPermission;
                            break;
                        case "liFrmBestEmployeeNomination":
                            liFrmBestEmployeeNomination.Visible = isViewPermission;
                            break;
                        case "liFrmBestEmployeeSelection":
                            liFrmBestEmployeeSelection.Visible = isViewPermission;
                            break;
                        case "liFrmEmpSearch":
                            liFrmEmpSearch.Visible = isViewPermission;
                            break;
                        case "liFrmDivisionList":
                            liFrmDivisionList.Visible = isViewPermission;
                            break;
                        case "liFrmStaffingBudget":
                            liFrmStaffingBudget.Visible = isViewPermission;
                            break;
                        case "liFrmStaffRequisition":
                            liFrmStaffRequisition.Visible = isViewPermission;
                            break;
                        case "liFrmOvertimeHourOfEmployee":
                            liFrmOvertimeHourOfEmployee.Visible = isViewPermission;
                            break;
                        case "liFrmEmpPFApproval":
                            liFrmEmpPFApproval.Visible = isViewPermission;
                            break;
                        case "liFrmPFopeningBalance":
                            liFrmPFopeningBalance.Visible = isViewPermission;
                            break;
                        case "liFrmEmpGratuityApproval":
                            liFrmEmpGratuityApproval.Visible = isViewPermission;
                            break;
                        case "liFrmEmpPromotion":
                            liFrmEmpPromotion.Visible = isViewPermission;
                            break;
                        case "liFrmRelieveLetter":
                            liFrmRelieveLetter.Visible = isViewPermission;
                            break;

                        //User Information---------------------------------------------------------------
                        case "liFrmUserGroup":
                            liFrmUserGroup.Visible = isViewPermission;
                            break;
                        case "liFrmUserInformation":
                            liFrmUserInformation.Visible = isViewPermission;
                            break;
                        case "liFrmObjectPermission":
                            liFrmObjectPermission.Visible = isViewPermission;
                            break;
                        //Cost of Goods SOld---------------------------------------------------------------
                        case "liFrmMonthlyTransaction":
                            liFrmMonthlyTransaction.Visible = isViewPermission;
                            break;
                        case "liFrmTransactionHead":
                            liFrmTransactionHead.Visible = isViewPermission;
                            break;
                        //Sales & Marketing management------------------------------------------------------
                        case "liFrmGuestReference":
                            liFrmGuestReference.Visible = isViewPermission;
                            break;
                        case "liFrmGuestCompany":
                            liFrmGuestCompany.Visible = isViewPermission;
                            break;
                        case "liFrmLocation":
                            liFrmLocation.Visible = isViewPermission;
                            break;
                        case "liFrmCity":
                            liFrmCity.Visible = isViewPermission;
                            break;
                        case "liFrmIndustry":
                            liFrmIndustry.Visible = isViewPermission;
                            break;
                        case "liFrmSalesCall":
                            liFrmSalesCall.Visible = isViewPermission;
                            break;
                        //Recruitment management------------------------------------------------------------
                        case "liFrmApplicantInfo":
                            liFrmApplicantInfo.Visible = isViewPermission;
                            break;
                        case "liFrmResumeSearch":
                            liFrmResumeSearch.Visible = isViewPermission;
                            break;
                        case "liFrmEmpJobCircular":
                            liFrmEmpJobCircular.Visible = isViewPermission;
                            break;
                        case "liFrmInterviewType":
                            liFrmInterviewType.Visible = isViewPermission;
                            break;

                        case "liFrmAppicantMarksDetails":
                            liFrmAppicantMarksDetails.Visible = isViewPermission;
                            break;
                        case "liFrmInterviewEvaluation":
                            liFrmInterviewEvaluation.Visible = isViewPermission;
                            break;

                        //Membership management------------------------------------------------------------
                        case "liFrmMemberBasic":
                            liFrmMemberBasic.Visible = isViewPermission;
                            break;
                        case "liFrmMemberType":
                            liFrmMemberType.Visible = isViewPermission;
                            break;

                    }
                }
            }
            //else
            //{
            //    Response.Redirect("/HMCommon/frmHMHome.aspx");
            //}
        }
        public void MessageCount()
        {
            Int64 TotalUnreadMessage = 0;

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
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Redirect("/HotelManagement/frmSearchGuest.aspx?SearchText=" + SearchTextBox.Text);
        }
    }
}