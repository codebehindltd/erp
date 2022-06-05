using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Text.RegularExpressions;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesManagment;
using Newtonsoft.Json;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmPayrollConfiguration : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCheckBox();
                LoadCompanyContributionOn();
                LoadAttendanceDevice();
                LoadSalaryHead();
                LoadOverTimeMode();
                LoadSalaryMonthStartDate();
                LoadBonusMonthlyEffectedPeriod();
                LoadBasicHeadSetupInfo();
                LoadSalaryProcessSystem();
                LoadLeaveType();
                LoadBonusHeadSetupInfo();
                LoadMonthlyWorkingDay();
                LoadWorkingHour();
                LoadDeductionAmount();
                LoadTotalWorkingDayForOvertime();
                LoadEmpLateBufferingTime();

                LoadMinimumOvertimeHour();
                LoadInsteadLeaveForOneHoliday();
                LoadMonthlyWorkingDayForAbsentree();
                LoadInsteadLeaveHead();
                LoadLoan();
                LoadPF();
                LoadTax();
                LoadGratuity();
                LoadBonus();
                LoadAmountType();
                GetDependsOnHeadList();
            }
        }
        private bool IsValidMail(string Email)
        {
            bool status = true;
            string expression = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        private void LoadCheckBox()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithInventory", "IsPayrollIntegrateWithInventory");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollIntegrateWithInventory.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollIntegrateWithInventory.Checked = false;
                else
                    IsPayrollIntegrateWithInventory.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithAccounts", "IsPayrollIntegrateWithAccounts");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollIntegrateWithAccounts.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollIntegrateWithAccounts.Checked = false;
                else
                    IsPayrollIntegrateWithAccounts.Checked = true;

            }

            //
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollWorkStationHide", "IsPayrollWorkStationHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollWorkStationHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollWorkStationHide.Checked = false;
                else
                    IsPayrollWorkStationHide.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDonorNameAndActivityCodeHide", "IsPayrollDonorNameAndActivityCodeHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollDonorNameAndActivityCodeHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollDonorNameAndActivityCodeHide.Checked = false;
                else
                    IsPayrollDonorNameAndActivityCodeHide.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDependentHide", "IsPayrollDependentHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollDependentHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollDependentHide.Checked = false;
                else
                    IsPayrollDependentHide.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBeneficiaryHide", "IsPayrollBeneficiaryHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollBeneficiaryHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollBeneficiaryHide.Checked = false;
                else
                    IsPayrollBeneficiaryHide.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollReferenceHide", "IsPayrollReferenceHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollReferenceHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollReferenceHide.Checked = false;
                else
                    IsPayrollReferenceHide.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBenefitsHide", "IsPayrollBenefitsHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollBenefitsHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollBenefitsHide.Checked = false;
                else
                    IsPayrollBenefitsHide.Checked = true;

            }

            //

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSalaryProcessBasedOnAttendance", "IsSalaryProcessBasedOnAttendance");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSalaryProcessBasedOnAttendance.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsSalaryProcessBasedOnAttendance.Checked = false;
                else
                    IsSalaryProcessBasedOnAttendance.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsServiceChargeShowsInSalarySheetAndPaySlip", "IsServiceChargeShowsInSalarySheetAndPaySlip");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsServiceChargeShowsInSalarySheetAndPaySlip.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsServiceChargeShowsInSalarySheetAndPaySlip.Checked = false;
                else
                    IsServiceChargeShowsInSalarySheetAndPaySlip.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsEmployeeBasicSetUpOnly", "IsEmployeeBasicSetUpOnly");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsEmployeeBasicSetUpOnly.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsEmployeeBasicSetUpOnly.Checked = false;
                else
                    IsEmployeeBasicSetUpOnly.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollProvidentFundDeductHide", "IsPayrollProvidentFundDeductHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollProvidentFundDeductHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollProvidentFundDeductHide.Checked = false;
                else
                    IsPayrollProvidentFundDeductHide.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollCostCenterDivHide", "IsPayrollCostCenterDivHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollCostCenterDivHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPayrollCostCenterDivHide.Checked = false;
                else
                    IsPayrollCostCenterDivHide.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects", "IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Checked = false;
                else
                    IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Checked = true;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsEmployeeCanEditDetailsInfo", "IsEmployeeCanEditDetailsInfo");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsEmployeeCanEditDetailsInfo.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsEmployeeCanEditDetailsInfo.Checked = false;
                else
                    IsEmployeeCanEditDetailsInfo.Checked = true;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAutoLoanCollectionProcessEnable", "IsAutoLoanCollectionProcessEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsAutoLoanCollectionProcessEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsAutoLoanCollectionProcessEnable.Checked = false;
                else
                    IsAutoLoanCollectionProcessEnable.Checked = true;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsEmployeeCodeAutoGenerate", "IsEmployeeCodeAutoGenerate");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsEmployeeCodeAutoGenerate.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsEmployeeCodeAutoGenerate.Checked = false;
                else
                    IsEmployeeCodeAutoGenerate.Checked = true;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsEmpSearchFromDashboardEnable", "IsEmpSearchFromDashboardEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsEmpSearchFromDashboardEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsEmpSearchFromDashboardEnable.Checked = false;
                else
                    IsEmpSearchFromDashboardEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsEmpSearchDetailsFromDashboardEnable", "IsEmpSearchDetailsFromDashboardEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsEmpSearchDetailsFromDashboardEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsEmpSearchDetailsFromDashboardEnable.Checked = false;
                else
                    IsEmpSearchDetailsFromDashboardEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCashRequisitionEnable", "IsCashRequisitionEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCashRequisitionEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCashRequisitionEnable.Checked = false;
                else
                    IsCashRequisitionEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBillVoucherEnable", "IsBillVoucherEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBillVoucherEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBillVoucherEnable.Checked = false;
                else
                    IsBillVoucherEnable.Checked = true;
            }
        }
        private void LoadAttendanceDevice()
        {
            List<AttendanceDeviceBO> attendanceDeviceBOList = new List<AttendanceDeviceBO>();
            AttendanceDeviceDA attendanceDeviceDA = new AttendanceDeviceDA();
            attendanceDeviceBOList = attendanceDeviceDA.GetAllAttendanceDeviceInfo();

            if (attendanceDeviceBOList != null)
            {
                ddlAttendanceDevice.DataSource = attendanceDeviceBOList;
                ddlAttendanceDevice.DataTextField = "ReaderId";
                ddlAttendanceDevice.DataValueField = "ReaderId";
                ddlAttendanceDevice.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                //item.Value = "---None---";
                item.Text = hmUtility.GetDropDownFirstValue();
                ddlAttendanceDevice.Items.Insert(0, item);
            }
        }
        //--Overtime Information----------------------------------------------        
        private void LoadOverTimeMode()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Overtime", "Overtime Setup");
            string mainString = commonSetupBO.SetupValue;
            if (!string.IsNullOrEmpty(mainString))
            {

                txtOverTimeSetupId.Value = commonSetupBO.SetupId.ToString();
                string[] dataArray = mainString.Split('~');
                ddlSalaryHeadId.SelectedValue = dataArray[0];
                //txtMonthlyTotalHour.Text = dataArray[1];
            }
        }
        private void ClearOverTimeInfo()
        {
            txtOverTimeSetupId.Value = string.Empty;
        }
        //--Salary Month Start Date Information----------------------------------------------
        private void LoadSalaryMonthStartDate()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Salary Start Date", "Salary Start Date Setup");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                ddlStartDate.SelectedValue = commonSetupBO.SetupValue;
                txtStartDateId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadBonusMonthlyEffectedPeriod()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollBonusMonthlyEffectedPeriod", "PayrollBonusMonthlyEffectedPeriod");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                ddlBonusMonthlyPeriod.SelectedValue = commonSetupBO.SetupValue;
                hfBonusMonthlyPeriodId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadSalaryProcessSystem()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Salary Process System", "Salary Process System Setup");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                ddlSalaryProcessSystem.SelectedValue = commonSetupBO.SetupValue;
                txtSalaryProcessId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        protected void btnUpdateSettings_Click(object sender, EventArgs e)
        {
            //if (!IsFrmBasicHeadSetupInfoValid())
            //{
            //    return;
            //}

            if (!IsFrmBasicHeadSetupInfoValid())
            {
                return;
            }

            //if (ddlPayrollAfterServiceBenefitId.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Payroll After Service Benefit.", AlertType.Warning);
            //    ddlPayrollAfterServiceBenefitId.Focus();
            //    return;
            //}
            //if (ddlPayrollAdvanceHeadId.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Payroll Advance Head.", AlertType.Warning);
            //    ddlPayrollAdvanceHeadId.Focus();
            //    return;
            //}

            //if (ddlPayrollBonusMonthlyEffectedPeriod.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Payroll Bonus Monthly Effected Period.", AlertType.Warning);
            //    ddlPayrollBonusMonthlyEffectedPeriod.Focus();
            //    return;
            //}

            //if (ddlLastPayHeadIdLeaveBalance.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Last Pay Head Leave Balance.", AlertType.Warning);
            //    ddlLastPayHeadIdLeaveBalance.Focus();
            //    return;
            //}

            //if (ddlWithoutSalaryLeaveHeadId.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Without Salary Leave Head.", AlertType.Warning);
            //    ddlWithoutSalaryLeaveHeadId.Focus();
            //    return;
            //}

            //if (ddlServiceChargeAllowanceHeadId.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Service Charge Allowance Head.", AlertType.Warning);
            //    ddlServiceChargeAllowanceHeadId.Focus();
            //    return;
            //}

            //if (ddlOvertimeAllowanceHeadId.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Overtime Allowance Head.", AlertType.Warning);
            //    ddlOvertimeAllowanceHeadId.Focus();
            //    return;
            //}

            //if (ddlMedicalAllowance.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Medical Allowance.", AlertType.Warning);
            //    ddlMedicalAllowance.Focus();
            //    return;
            //}


            if (ddlPayrollSalaryExecutionProcessType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Payroll Salary Execution Process Type.", AlertType.Warning);
                ddlPayrollSalaryExecutionProcessType.Focus();
                return;
            }

            if (ddlPayrollReportingTo.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Payroll Reporting To.", AlertType.Warning);
                ddlPayrollReportingTo.Focus();
                return;
            }

            //

            try
            {
                int tmpSetupId = 0;
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                if (!string.IsNullOrEmpty(txtStartDateId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtStartDateId.Value);
                }

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "Salary Start Date Setup";
                commonSetupBO.TypeName = "Salary Start Date";
                commonSetupBO.SetupValue = ddlStartDate.SelectedValue;
                Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
                //arpon

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtBasicSetupId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtBasicSetupId.Value);
                }


                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupBO commonSetupDirectSalaryProcessDependsOn = new HMCommonSetupBO();
                commonSetupDirectSalaryProcessDependsOn = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "Basic Salary Setup";
                commonSetupBO.TypeName = "Basic Salary Head";
                commonSetupBO.SetupValue = ddlBasicSalaryHeadId.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                if ((commonSetupDirectSalaryProcessDependsOn.SetupValue).ToString() == HMConstants.SalaryProcessDependsOn.Gross.ToString())
                {
                    commonSetupBO = new HMCommonSetupBO();
                    if (!string.IsNullOrEmpty(txtGrossSetupId.Value))
                    {
                        commonSetupBO.SetupId = Int32.Parse(txtGrossSetupId.Value);
                    }

                    commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                    commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                    commonSetupBO.SetupName = "Gross Salary Setup";
                    commonSetupBO.TypeName = "Gross Salary Head";
                    commonSetupBO.SetupValue = ddlGrossSalaryHeadId.SelectedValue;
                    status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
                }

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtOverTimeSetupId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtOverTimeSetupId.Value);
                }

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "Overtime Setup";
                commonSetupBO.TypeName = "Overtime";
                commonSetupBO.SetupValue = ddlSalaryHeadId.SelectedValue + '~' + "0";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtMinimumOvertimeHourId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtMinimumOvertimeHourId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollMinimumOvertimeHour";
                commonSetupBO.TypeName = "MinimumOvertimeHour";
                commonSetupBO.SetupValue = txtMinimumOvertimeHour.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);


                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtSalaryProcessId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtSalaryProcessId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "Salary Process System Setup";
                commonSetupBO.TypeName = "Salary Process System";
                commonSetupBO.SetupValue = ddlSalaryProcessSystem.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtMonthlyWorkingDayForAbsentreeId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtMonthlyWorkingDayForAbsentreeId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollInsteadLeaveForOneHoliday";
                commonSetupBO.TypeName = "InsteadLeaveForOneHoliday";
                commonSetupBO.SetupValue = txtMonthlyWorkingDayForAbsentree.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtMonthlyWorkingDayId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtMonthlyWorkingDayId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "MonthlyWorkingDay";
                commonSetupBO.TypeName = "PayrollMonthlyWorkingDay";
                commonSetupBO.SetupValue = txtMonthlyWorkingDay.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtWorkingHourId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtWorkingHourId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "DailyWorkingHour";
                commonSetupBO.TypeName = "PayrollDailyWorkingHour";
                commonSetupBO.SetupValue = txtWorkingHour.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtInsteadLeaveHeadId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtInsteadLeaveHeadId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollInsteadLeaveHeadId";
                commonSetupBO.TypeName = "InsteadLeaveHeadId";
                commonSetupBO.SetupValue = ddlInsteadLeaveHeadId.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtInsteadLeaveForOneHolidayId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtInsteadLeaveForOneHolidayId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollInsteadLeaveForOneHoliday";
                commonSetupBO.TypeName = "InsteadLeaveForOneHoliday";
                commonSetupBO.SetupValue = txtInsteadLeaveForOneHoliday.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtDeductionAmountForEachEmployeeFromSalaryId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtDeductionAmountForEachEmployeeFromSalaryId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "DeductionAmountForEachEmployeeFromSalary";
                commonSetupBO.TypeName = "DeductionAmountForEachEmployeeFromSalary";
                commonSetupBO.SetupValue = txtDeductionAmountForEachEmployeeFromSalary.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtTotalWorkingDayForOvertimeId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtTotalWorkingDayForOvertimeId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "TotalWorkingDayForOvertime";
                commonSetupBO.TypeName = "TotalWorkingDayForOvertime";
                commonSetupBO.SetupValue = txtTotalWorkingDayForOvertime.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //txtEmpLateBufferingTime
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtEmpLateBufferingTimeId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtEmpLateBufferingTimeId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "LateBufferingTime";
                commonSetupBO.TypeName = "LateBufferingTime";
                commonSetupBO.SetupValue = txtEmpLateBufferingTime.Text;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtPayrollAfterServiceBenefitId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtPayrollAfterServiceBenefitId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollAfterServiceBenefitId";
                commonSetupBO.TypeName = "PayrollAfterServiceBenefitId";
                commonSetupBO.SetupValue = ddlPayrollAfterServiceBenefitId.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtPayrollAdvanceHeadId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtPayrollAdvanceHeadId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollAdvanceHeadId";
                commonSetupBO.TypeName = "PayrollAdvanceHeadId";
                commonSetupBO.SetupValue = ddlPayrollAdvanceHeadId.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtPayrollBonusMonthlyEffectedPeriod.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtPayrollBonusMonthlyEffectedPeriod.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollBonusMonthlyEffectedPeriod";
                commonSetupBO.TypeName = "PayrollBonusMonthlyEffectedPeriod";
                commonSetupBO.SetupValue = ddlPayrollBonusMonthlyEffectedPeriod.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtLastPayHeadIdLeaveBalance.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtLastPayHeadIdLeaveBalance.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "LastPayHeadIdLeaveBalance";
                commonSetupBO.TypeName = "LastPayHeadIdLeaveBalance";
                commonSetupBO.SetupValue = ddlLastPayHeadIdLeaveBalance.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtWithoutSalaryLeaveHeadId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtWithoutSalaryLeaveHeadId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "WithoutSalaryLeaveHeadId";
                commonSetupBO.TypeName = "WithoutSalaryLeaveHeadId";
                commonSetupBO.SetupValue = ddlWithoutSalaryLeaveHeadId.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtServiceChargeAllowanceHeadId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtServiceChargeAllowanceHeadId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "ServiceChargeAllowanceHeadId";
                commonSetupBO.TypeName = "ServiceChargeAllowanceHeadId";
                commonSetupBO.SetupValue = ddlServiceChargeAllowanceHeadId.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtOvertimeAllowanceHeadId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtOvertimeAllowanceHeadId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "OvertimeAllowanceHeadId";
                commonSetupBO.TypeName = "OvertimeAllowanceHeadId";
                commonSetupBO.SetupValue = ddlOvertimeAllowanceHeadId.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtMedicalAllowance.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtMedicalAllowance.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "Medical Allowance";
                commonSetupBO.TypeName = "Medical Allowance";
                commonSetupBO.SetupValue = ddlMedicalAllowance.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtPayrollSalaryExecutionProcessType.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtPayrollSalaryExecutionProcessType.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollSalaryExecutionProcessType";
                commonSetupBO.TypeName = "PayrollSalaryExecutionProcessType";
                commonSetupBO.SetupValue = ddlPayrollSalaryExecutionProcessType.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);


                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(txtPayrollReportingTo.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtPayrollReportingTo.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollReportingTo";
                commonSetupBO.TypeName = "PayrollReportingTo";
                commonSetupBO.SetupValue = ddlPayrollReportingTo.SelectedValue;
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollIntegrateWithInventory.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollIntegrateWithInventory.Value);
                }
                if (IsPayrollIntegrateWithInventory.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollIntegrateWithInventory";
                commonSetupBO.TypeName = "IsPayrollIntegrateWithInventory";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollIntegrateWithAccounts.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollIntegrateWithAccounts.Value);
                }
                if (IsPayrollIntegrateWithAccounts.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollIntegrateWithAccounts";
                commonSetupBO.TypeName = "IsPayrollIntegrateWithAccounts";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollWorkStationHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollWorkStationHide.Value);
                }
                if (IsPayrollWorkStationHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollWorkStationHide";
                commonSetupBO.TypeName = "IsPayrollWorkStationHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollDonorNameAndActivityCodeHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollDonorNameAndActivityCodeHide.Value);
                }
                if (IsPayrollDonorNameAndActivityCodeHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollDonorNameAndActivityCodeHide";
                commonSetupBO.TypeName = "IsPayrollDonorNameAndActivityCodeHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollDependentHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollDependentHide.Value);
                }
                if (IsPayrollDependentHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollDependentHide";
                commonSetupBO.TypeName = "IsPayrollDependentHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollBeneficiaryHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollBeneficiaryHide.Value);
                }
                if (IsPayrollBeneficiaryHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollBeneficiaryHide";
                commonSetupBO.TypeName = "IsPayrollBeneficiaryHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollReferenceHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollReferenceHide.Value);
                }
                if (IsPayrollReferenceHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollReferenceHide";
                commonSetupBO.TypeName = "IsPayrollReferenceHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollBenefitsHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollBenefitsHide.Value);
                }
                if (IsPayrollBenefitsHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollBenefitsHide";
                commonSetupBO.TypeName = "IsPayrollBenefitsHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsSalaryProcessBasedOnAttendance.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsSalaryProcessBasedOnAttendance.Value);
                }
                if (IsSalaryProcessBasedOnAttendance.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsSalaryProcessBasedOnAttendance";
                commonSetupBO.TypeName = "IsSalaryProcessBasedOnAttendance";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsServiceChargeShowsInSalarySheetAndPaySlip.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsServiceChargeShowsInSalarySheetAndPaySlip.Value);
                }
                if (IsServiceChargeShowsInSalarySheetAndPaySlip.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsServiceChargeShowsInSalarySheetAndPaySlip";
                commonSetupBO.TypeName = "IsServiceChargeShowsInSalarySheetAndPaySlip";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsEmployeeBasicSetUpOnly.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsEmployeeBasicSetUpOnly.Value);
                }
                if (IsEmployeeBasicSetUpOnly.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollProvidentFundDeductHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollProvidentFundDeductHide.Value);
                }
                if (IsPayrollProvidentFundDeductHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollProvidentFundDeductHide";
                commonSetupBO.TypeName = "IsPayrollProvidentFundDeductHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);


                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsPayrollCostCenterDivHide.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsPayrollCostCenterDivHide.Value);
                }
                if (IsPayrollCostCenterDivHide.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsPayrollCostCenterDivHide";
                commonSetupBO.TypeName = "IsPayrollCostCenterDivHide";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);


                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Value);
                }
                if (IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects";
                commonSetupBO.TypeName = "IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //IsEmployeeCanEditDetailsInfo
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsEmployeeCanEditDetailsInfo.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsEmployeeCanEditDetailsInfo.Value);
                }
                if (IsEmployeeCanEditDetailsInfo.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsEmployeeCanEditDetailsInfo";
                commonSetupBO.TypeName = "IsEmployeeCanEditDetailsInfo";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //IsAutoLoanCollectionProcessEnable
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsAutoLoanCollectionProcessEnable.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsAutoLoanCollectionProcessEnable.Value);
                }
                if (IsAutoLoanCollectionProcessEnable.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsAutoLoanCollectionProcessEnable";
                commonSetupBO.TypeName = "IsAutoLoanCollectionProcessEnable";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //IsEmployeeCodeAutoGenerate
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsEmployeeCodeAutoGenerate.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsEmployeeCodeAutoGenerate.Value);
                }
                if (IsEmployeeCodeAutoGenerate.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsEmployeeCodeAutoGenerate";
                commonSetupBO.TypeName = "IsEmployeeCodeAutoGenerate";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //IsEmpSearchFromDashboardEnable
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsEmpSearchFromDashboardEnable.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsEmpSearchFromDashboardEnable.Value);
                }
                if (IsEmpSearchFromDashboardEnable.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsEmpSearchFromDashboardEnable";
                commonSetupBO.TypeName = "IsEmpSearchFromDashboardEnable";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //IsEmpSearchDetailsFromDashboardEnable
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsEmpSearchDetailsFromDashboardEnable.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsEmpSearchDetailsFromDashboardEnable.Value);
                }
                if (IsEmpSearchDetailsFromDashboardEnable.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsEmpSearchDetailsFromDashboardEnable";
                commonSetupBO.TypeName = "IsEmpSearchDetailsFromDashboardEnable";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //IsCashRequisitionEnable
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsCashRequisitionEnable.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsCashRequisitionEnable.Value);
                }
                if (IsCashRequisitionEnable.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsCashRequisitionEnable";
                commonSetupBO.TypeName = "IsCashRequisitionEnable";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                //IsBillVoucherEnable
                commonSetupBO = new HMCommonSetupBO();
                if (!string.IsNullOrEmpty(hfIsBillVoucherEnable.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(hfIsBillVoucherEnable.Value);
                }
                if (IsBillVoucherEnable.Checked)
                    commonSetupBO.SetupValue = "1";
                else
                    commonSetupBO.SetupValue = "0";

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "IsBillVoucherEnable";
                commonSetupBO.TypeName = "IsBillVoucherEnable";
                status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                if (status)
                {

                    CommonHelper.AlertInfo(innboardMessage, "Payroll Configuration Settings are updated Successfully.", AlertType.Success);

                }
                else
                {

                    CommonHelper.AlertInfo(innboardMessage, "Payroll Configuration Settings are not updated Successfully.", AlertType.Error);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void btnBonusMonthlyPeriodInfoSave_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(hfBonusMonthlyPeriodId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(hfBonusMonthlyPeriodId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollBonusMonthlyEffectedPeriod";
            commonSetupBO.TypeName = "PayrollBonusMonthlyEffectedPeriod";
            commonSetupBO.SetupValue = ddlBonusMonthlyPeriod.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Monthly Period Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Monthly Period Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.BonusConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BonusConfiguration));
            }
            else
            {
                //isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Monthly Period Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Monthly Period Saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.BonusConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BonusConfiguration));
            }


        }
        private void ClearSalaryMonthStartDateInfo()
        {
            txtStartDateId.Value = string.Empty;
        }
        //--Salary Head Information---------------------------------------------- 
        public void LoadSalaryHead()
        {
            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            ddlSalaryHeadId.Items.Insert(0, item1);

            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> fields = new List<SalaryHeadBO>();
            List<SalaryHeadBO> grossFields = new List<SalaryHeadBO>();
            List<SalaryHeadBO> deductionFields = new List<SalaryHeadBO>();
            fields = salaryHeadDA.GetSalaryHeadInfo();
            grossFields = salaryHeadDA.GetSalaryHeadInfo();
            deductionFields = salaryHeadDA.GetSalaryHeadInfoForDeduction();

            ddlSalaryHeadId.DataSource = fields;
            ddlSalaryHeadId.DataTextField = "SalaryHead";
            ddlSalaryHeadId.DataValueField = "SalaryHeadId";
            ddlSalaryHeadId.DataBind();
            ddlSalaryHeadId.Items.Insert(0, item1);

            ddlPayrollAdvanceHeadId.DataSource = fields;
            ddlPayrollAdvanceHeadId.DataTextField = "SalaryHead";
            ddlPayrollAdvanceHeadId.DataValueField = "SalaryHeadId";
            ddlPayrollAdvanceHeadId.DataBind();
            ddlPayrollAdvanceHeadId.Items.Insert(0, item1);

            ddlPayrollAfterServiceBenefitId.DataSource = fields;
            ddlPayrollAfterServiceBenefitId.DataTextField = "SalaryHead";
            ddlPayrollAfterServiceBenefitId.DataValueField = "SalaryHeadId";
            ddlPayrollAfterServiceBenefitId.DataBind();
            ddlPayrollAfterServiceBenefitId.Items.Insert(0, item1);

            ddlPayrollBonusMonthlyEffectedPeriod.DataSource = fields;
            ddlPayrollBonusMonthlyEffectedPeriod.DataTextField = "SalaryHead";
            ddlPayrollBonusMonthlyEffectedPeriod.DataValueField = "SalaryHeadId";
            ddlPayrollBonusMonthlyEffectedPeriod.DataBind();
            ddlPayrollBonusMonthlyEffectedPeriod.Items.Insert(0, item1);

            ddlLastPayHeadIdLeaveBalance.DataSource = fields;
            ddlLastPayHeadIdLeaveBalance.DataTextField = "SalaryHead";
            ddlLastPayHeadIdLeaveBalance.DataValueField = "SalaryHeadId";
            ddlLastPayHeadIdLeaveBalance.DataBind();
            ddlLastPayHeadIdLeaveBalance.Items.Insert(0, item1);

            ddlWithoutSalaryLeaveHeadId.DataSource = fields;
            ddlWithoutSalaryLeaveHeadId.DataTextField = "SalaryHead";
            ddlWithoutSalaryLeaveHeadId.DataValueField = "SalaryHeadId";
            ddlWithoutSalaryLeaveHeadId.DataBind();
            ddlWithoutSalaryLeaveHeadId.Items.Insert(0, item1);

            ddlPFLoanHeadId.DataSource = deductionFields;
            ddlPFLoanHeadId.DataTextField = "SalaryHead";
            ddlPFLoanHeadId.DataValueField = "SalaryHeadId";
            ddlPFLoanHeadId.DataBind();
            ddlPFLoanHeadId.Items.Insert(0, item1);

            ddlServiceChargeAllowanceHeadId.DataSource = fields;
            ddlServiceChargeAllowanceHeadId.DataTextField = "SalaryHead";
            ddlServiceChargeAllowanceHeadId.DataValueField = "SalaryHeadId";
            ddlServiceChargeAllowanceHeadId.DataBind();
            ddlServiceChargeAllowanceHeadId.Items.Insert(0, item1);

            ddlOvertimeAllowanceHeadId.DataSource = fields;
            ddlOvertimeAllowanceHeadId.DataTextField = "SalaryHead";
            ddlOvertimeAllowanceHeadId.DataValueField = "SalaryHeadId";
            ddlOvertimeAllowanceHeadId.DataBind();
            ddlOvertimeAllowanceHeadId.Items.Insert(0, item1);

            ddlMedicalAllowance.DataSource = fields;
            ddlMedicalAllowance.DataTextField = "SalaryHead";
            ddlMedicalAllowance.DataValueField = "SalaryHeadId";
            ddlMedicalAllowance.DataBind();
            ddlMedicalAllowance.Items.Insert(0, item1);

            ddlBasicSalaryHeadId.DataSource = fields;
            ddlBasicSalaryHeadId.DataTextField = "SalaryHead";
            ddlBasicSalaryHeadId.DataValueField = "SalaryHeadId";
            ddlBasicSalaryHeadId.DataBind();

            ddlBasicSalaryHeadId.Items.Insert(0, item1);


            ddlPayrollTaxDeduction.DataSource = deductionFields;
            ddlPayrollTaxDeduction.DataTextField = "SalaryHead";
            ddlPayrollTaxDeduction.DataValueField = "SalaryHeadId";
            ddlPayrollTaxDeduction.DataBind();

            ddlPayrollTaxDeduction.Items.Insert(0, item1);

            ddlGrossSalaryHeadId.DataSource = grossFields;
            ddlGrossSalaryHeadId.DataTextField = "SalaryHead";
            ddlGrossSalaryHeadId.DataValueField = "SalaryHeadId";
            ddlGrossSalaryHeadId.DataBind();
            ListItem item2 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();

            ddlGrossSalaryHeadId.Items.Insert(0, item2);

            ddlBonusHeadId.DataSource = fields;
            ddlBonusHeadId.DataTextField = "SalaryHead";
            ddlBonusHeadId.DataValueField = "SalaryHeadId";
            ddlBonusHeadId.DataBind();

            ddlDependsOn.DataSource = fields;
            ddlDependsOn.DataTextField = "SalaryHead";
            ddlDependsOn.DataValueField = "SalaryHeadId";
            ddlDependsOn.DataBind();

            ddlEmployeeContributionHeadId.DataSource = fields.Where(x => x.ContributionType == "Both").ToList();
            ddlEmployeeContributionHeadId.DataTextField = "SalaryHead";
            ddlEmployeeContributionHeadId.DataValueField = "SalaryHeadId";
            ddlEmployeeContributionHeadId.DataBind();

            ddlEmployeeContributionHeadId.Items.Insert(0, item1);

            ddlCompanyContributionHeadId.DataSource = fields.Where(x => x.ContributionType == "Both").ToList();
            ddlCompanyContributionHeadId.DataTextField = "SalaryHead";
            ddlCompanyContributionHeadId.DataValueField = "SalaryHeadId";
            ddlCompanyContributionHeadId.DataBind();

            ddlCompanyContributionHeadId.Items.Insert(0, item1);
        }
        protected void btnBonusHead_Click(object sender, EventArgs e)
        {
            if (!IsFrmBasicHeadSetupInfoValid())
            {
                return;
            }
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(hfBonusHeadId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(hfBonusHeadId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollBonusHeadId";
            commonSetupBO.TypeName = "PayrollBonusHeadId";
            commonSetupBO.SetupValue = ddlBonusHeadId.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Head Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Head Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.BonusHead.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BonusHead));
                SetTab("BonusTab");
            }
            else
            {
                //isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Head Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Head Saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.BonusHead.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BonusHead));
                SetTab("BonusTab");
            }
        }
        public void LoadLeaveType()
        {
            LeaveTypeDA leaveDA = new LeaveTypeDA();
            List<LeaveTypeBO> list = new List<LeaveTypeBO>();
            list = leaveDA.GetLeaveTypeInfo();

            ddlInsteadLeaveHeadId.DataSource = list;
            ddlInsteadLeaveHeadId.DataTextField = "TypeName";
            ddlInsteadLeaveHeadId.DataValueField = "LeaveTypeId";
            ddlInsteadLeaveHeadId.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            ddlInsteadLeaveHeadId.Items.Insert(0, item1);
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupDA.GetCommonConfigurationInfo("InsteadLeaveHeadId", "PayrollInsteadLeaveHeadId");

            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtInsteadLeaveHeadId.Value = commonSetupBO.SetupId.ToString();
                ddlInsteadLeaveHeadId.SelectedValue = commonSetupBO.SetupValue;
            }


        }
        private void LoadBasicHeadSetupInfo()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");

            HMCommonSetupBO commonSetupDirectSalaryProcessDependsOn = new HMCommonSetupBO();
            commonSetupDirectSalaryProcessDependsOn = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

            if ((commonSetupDirectSalaryProcessDependsOn.SetupValue).ToString() != HMConstants.SalaryProcessDependsOn.Gross.ToString())
            {
                GrossSalarySetup.Style.Add("display", "none ");
            }

            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtBasicSetupId.Value = commonSetupBO.SetupId.ToString();
                ddlBasicSalaryHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Gross Salary Head", "Gross Salary Setup");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtGrossSetupId.Value = commonSetupBO.SetupId.ToString();
                ddlGrossSalaryHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollAfterServiceBenefitId", "PayrollAfterServiceBenefitId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtPayrollAfterServiceBenefitId.Value = commonSetupBO.SetupId.ToString();
                ddlPayrollAfterServiceBenefitId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollAdvanceHeadId", "PayrollAdvanceHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtPayrollAdvanceHeadId.Value = commonSetupBO.SetupId.ToString();
                ddlPayrollAdvanceHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollBonusMonthlyEffectedPeriod", "PayrollBonusMonthlyEffectedPeriod");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtPayrollBonusMonthlyEffectedPeriod.Value = commonSetupBO.SetupId.ToString();
                ddlPayrollBonusMonthlyEffectedPeriod.SelectedValue = commonSetupBO.SetupValue;
            }
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("LastPayHeadIdLeaveBalance", "LastPayHeadIdLeaveBalance");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtLastPayHeadIdLeaveBalance.Value = commonSetupBO.SetupId.ToString();
                ddlLastPayHeadIdLeaveBalance.SelectedValue = commonSetupBO.SetupValue;
            }
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("WithoutSalaryLeaveHeadId", "WithoutSalaryLeaveHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtWithoutSalaryLeaveHeadId.Value = commonSetupBO.SetupId.ToString();
                ddlWithoutSalaryLeaveHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PFLoanHeadId", "PFLoanHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtPFLoanHeadId.Value = commonSetupBO.SetupId.ToString();
                ddlPFLoanHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("ServiceChargeAllowanceHeadId", "ServiceChargeAllowanceHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtServiceChargeAllowanceHeadId.Value = commonSetupBO.SetupId.ToString();
                ddlServiceChargeAllowanceHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("OvertimeAllowanceHeadId", "OvertimeAllowanceHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtOvertimeAllowanceHeadId.Value = commonSetupBO.SetupId.ToString();
                ddlOvertimeAllowanceHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Medical Allowance", "Medical Allowance");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtMedicalAllowance.Value = commonSetupBO.SetupId.ToString();
                ddlMedicalAllowance.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollSalaryExecutionProcessType", "PayrollSalaryExecutionProcessType");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtPayrollSalaryExecutionProcessType.Value = commonSetupBO.SetupId.ToString();
                ddlPayrollSalaryExecutionProcessType.SelectedValue = commonSetupBO.SetupValue;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollReportingTo", "PayrollReportingTo");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtPayrollReportingTo.Value = commonSetupBO.SetupId.ToString();
                ddlPayrollReportingTo.SelectedValue = commonSetupBO.SetupValue;
            }



            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollTaxDeductionId", "PayrollTaxDeductionId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                ddlPayrollTaxDeductionId.Value = commonSetupBO.SetupId.ToString();
                ddlPayrollTaxDeduction.SelectedValue = commonSetupBO.SetupValue;
            }
        }
        private void LoadBonusHeadSetupInfo()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollBonusHeadId", "PayrollBonusHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                hfBonusHeadId.Value = commonSetupBO.SetupId.ToString();
                ddlBonusHeadId.SelectedValue = commonSetupBO.SetupValue;
                btnBonusHead.Text = "Update";

            }
        }
        private bool IsFrmBasicHeadSetupInfoValid()
        {
            bool flag = true;
            if (ddlBasicSalaryHeadId.SelectedIndex == 0)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Select Valid Basic Salary Head";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Valid Basic Salary Head.", AlertType.Warning);
                ddlBasicSalaryHeadId.Focus();
                flag = false;
            }
            //else
            //{
            //    isMessageBoxEnable = -1;
            //    lblMessage.Text = string.Empty;
            //}
            return flag;
        }
        private void LoadMonthlyWorkingDay()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollMonthlyWorkingDay", "MonthlyWorkingDay");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtMonthlyWorkingDay.Text = commonSetupBO.SetupValue;
                txtMonthlyWorkingDayId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadDeductionAmount()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("DeductionAmountForEachEmployeeFromSalary", "DeductionAmountForEachEmployeeFromSalary");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtDeductionAmountForEachEmployeeFromSalary.Text = commonSetupBO.SetupValue;
                txtDeductionAmountForEachEmployeeFromSalaryId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadTotalWorkingDayForOvertime()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("TotalWorkingDayForOvertime", "TotalWorkingDayForOvertime");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtTotalWorkingDayForOvertime.Text = commonSetupBO.SetupValue;
                txtTotalWorkingDayForOvertimeId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadEmpLateBufferingTime()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("LateBufferingTime", "LateBufferingTime");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtEmpLateBufferingTime.Text = commonSetupBO.SetupValue;
                txtEmpLateBufferingTimeId.Value = commonSetupBO.SetupId.ToString();
            }
        }

        private void LoadWorkingHour()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollDailyWorkingHour", "DailyWorkingHour");

            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtWorkingHour.Text = commonSetupBO.SetupValue;
                txtWorkingHourId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadMinimumOvertimeHour()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("MinimumOvertimeHour", "PayrollMinimumOvertimeHour");


            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtMinimumOvertimeHour.Text = commonSetupBO.SetupValue;
                txtMinimumOvertimeHourId.Value = commonSetupBO.SetupId.ToString();

            }
        }
        private void LoadInsteadLeaveForOneHoliday()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InsteadLeaveForOneHoliday", "PayrollInsteadLeaveForOneHoliday");

            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtInsteadLeaveForOneHoliday.Text = commonSetupBO.SetupValue;
                txtInsteadLeaveForOneHolidayId.Value = commonSetupBO.SetupId.ToString();

            }
        }
        private void LoadMonthlyWorkingDayForAbsentree()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InsteadLeaveForOneHoliday", "PayrollInsteadLeaveForOneHoliday");

            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtMonthlyWorkingDayForAbsentree.Text = commonSetupBO.SetupValue;
                txtMonthlyWorkingDayForAbsentreeId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadInsteadLeaveHead()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InsteadLeaveHeadId", "PayrollInsteadLeaveHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                ddlInsteadLeaveHeadId.SelectedValue = commonSetupBO.SetupValue;
                txtInsteadLeaveHeadId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        protected void btnEmpTaxSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsTaxFrmValid())
                {
                    return;
                }

                TaxSettingBO taxBO = new TaxSettingBO();
                EmpTaxDA taxDA = new EmpTaxDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                int tmpSetupId = 0;
                if (!string.IsNullOrEmpty(ddlPayrollTaxDeductionId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(ddlPayrollTaxDeductionId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PayrollTaxDeductionId";
                commonSetupBO.TypeName = "PayrollTaxDeductionId";
                commonSetupBO.SetupValue = ddlPayrollTaxDeduction.SelectedValue;
                Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                userInformationBO = new UserInformationBO();

                taxBO.TaxBandForMale = Convert.ToDecimal(0); //Convert.ToDecimal(txtTaxBandM.Text.Trim());
                taxBO.TaxBandForFemale = Convert.ToDecimal(0); //Convert.ToDecimal(txtTaxBandF.Text.Trim());
                taxBO.IsTaxPaidByCompany = false; //chkIsTaxPaidbyCmp.Checked ? true : false;
                taxBO.CompanyContributionType = "Basic"; // ddlCmpContType.SelectedItem.Text;
                //if (!string.IsNullOrEmpty(txtCmpContAmount.Text))
                //    taxBO.CompanyContributionAmount = Convert.ToDecimal(txtCmpContAmount.Text.Trim());
                taxBO.CompanyContributionAmount = Convert.ToDecimal(0);
                taxBO.IsTaxDeductFromSalary = chkIsTaxDdctFrmSlry.Checked ? true : false;
                taxBO.EmployeeContributionType = ddlEmpContType.SelectedValue;
                taxBO.Remarks = txtRemarks.Text;

                int hiddenId = Convert.ToInt32(txtTaxSettingId.Value);
                int tmpId;
                if (hiddenId > 0)
                {
                    taxBO.TaxSettingId = hiddenId;
                    taxBO.LastModifiedBy = userInformationBO.UserInfoId;
                    taxBO.LastModifiedDate = DateTime.Now;

                    status = taxDA.UpdateEmpTaxInfo(taxBO);
                    if (status)
                    {
                        //isMessageBoxEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.TaxSetting.ToString(), taxBO.TaxSettingId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TaxSetting));
                    }
                }
                else
                {
                    taxBO.CreatedBy = userInformationBO.UserInfoId;
                    taxBO.CreatedDate = DateTime.Now;

                    status = taxDA.SaveEmpTaxInformation(taxBO, out tmpId);
                    if (status)
                    {
                        //isMessageBoxEnable = 2;
                        //lblMessage.Text = "Save Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.TaxSetting.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TaxSetting));
                        LoadTax();
                        //btnEmpTaxSave.Text = "Update";
                    }
                }
                SetTab("TaxTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnEmpTaxClear_Click(object sender, EventArgs e)
        {
            txtTaxBandM.Text = string.Empty;
            txtTaxBandF.Text = string.Empty;
            chkIsTaxPaidbyCmp.Checked = false;
            ddlCmpContType.SelectedIndex = 0;
            txtCmpContAmount.Text = string.Empty;
            chkIsTaxDdctFrmSlry.Checked = false;
            ddlEmpContType.SelectedIndex = 0;
            txtRemarks.Text = string.Empty;
            SetTab("TaxTab");
        }
        protected void btnEmpPFSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsPFFrmValid())
                {
                    return;
                }

                PFSettingBO pfBO = new PFSettingBO();
                EmpPFDA pfDA = new EmpPFDA();

                string pfEmployeeContId = ddlEmployeeContributionHeadId.Text;
                string pfCompanyContId = ddlCompanyContributionHeadId.Text;
                string pfCompanyContributionOn = ddlCompanyContributionOn.SelectedValue;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                pfBO.EmployeeContributionInPercentage = Convert.ToDecimal(txtEmpCont.Text.Trim());
                pfBO.CompanyContributionInPercentange = Convert.ToDecimal(txtCmpCont.Text.Trim());
                pfBO.EmployeeCanContributeMaxOfBasicSalary = Convert.ToDecimal(txtEmpMaxCont.Text.Trim());
                pfBO.InterestDistributionRate = Convert.ToDecimal(txtIntDisRt.Text.Trim());
                pfBO.CompanyContributionEligibilityYear = Convert.ToInt32(txtCmpContElegYear.Text);

                int hiddenId = Convert.ToInt32(txtPFSettingId.Value);
                int tmpId;

                if (hiddenId > 0)
                {
                    pfBO.PFSettingId = hiddenId;
                    pfBO.LastModifiedBy = userInformationBO.UserInfoId;
                    pfBO.LastModifiedDate = DateTime.Now;

                    Boolean status = pfDA.UpdateEmpPFInfo(pfBO, pfEmployeeContId, pfCompanyContId, pfCompanyContributionOn);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.PFSetting.ToString(), pfBO.PFSettingId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PFSetting));
                    }
                }
                else
                {
                    pfBO.CreatedBy = userInformationBO.UserInfoId;
                    pfBO.CreatedDate = DateTime.Now;

                    Boolean status = pfDA.SaveEmpPFInformation(pfBO, pfEmployeeContId, pfCompanyContId, pfCompanyContributionOn, out tmpId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.PFSetting.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PFSetting));
                        LoadPF();
                        //btnEmpPFSave.Text = "Update";
                    }
                }
                SetTab("PFTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
        }
        protected void btnEmpPFClear_Click(object sender, EventArgs e)
        {
            txtEmpCont.Text = string.Empty;
            txtCmpCont.Text = string.Empty;
            txtEmpMaxCont.Text = string.Empty;
            txtIntDisRt.Text = string.Empty;
            txtCmpContElegYear.Text = string.Empty;
            SetTab("PFTab");
        }
        protected void btnEmpLoanSave_Click(object sender, EventArgs e)
        {
            int tmpId;
            try
            {
                if (!IsLoanFrmValid())
                {
                    return;
                }
                if (ddlPFLoanHeadId.SelectedIndex == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "PF Loan Head.", AlertType.Warning);
                    ddlPFLoanHeadId.Focus();
                    return;
                }
                UserInformationBO userInformationBO = new UserInformationBO();
                int tmpSetupId = 0;
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                if (!string.IsNullOrEmpty(txtPFLoanHeadId.Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(txtPFLoanHeadId.Value);
                }
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
                commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
                commonSetupBO.SetupName = "PFLoanHeadId";
                commonSetupBO.TypeName = "PFLoanHeadId";
                commonSetupBO.SetupValue = ddlPFLoanHeadId.SelectedValue;
                bool status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                LoanSettingBO loanBO = new LoanSettingBO();
                EmpLoanDA loanDA = new EmpLoanDA();


                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                loanBO.CompanyLoanInterestRate = Convert.ToDecimal(txtCmpLnIntRate.Text.Trim());
                loanBO.PFLoanInterestRate = Convert.ToDecimal(txtPFlnIntRate.Text.Trim());
                loanBO.MaxAmountCanWithdrawFromPFInPercentage = Convert.ToDecimal(txtMaxAmtWdrwfmPF.Text.Trim());
                loanBO.MinPFMustAvailableToAllowLoan = Convert.ToDecimal(txtMinPFavlfrLn.Text.Trim());
                loanBO.MinJobLengthToAllowCompanyLoan = Convert.ToDecimal(txtMinJobLnthfrCmpLn.Text.Trim());
                loanBO.DurationToAllowNextLoanAfterCompletetionTakenLoan = Convert.ToInt32(txtDrtnfrNxtLn.Text.Trim());

                int hiddenId = Convert.ToInt32(txtLoanSettingId.Value);
                if (hiddenId > 0)
                {
                    loanBO.LoanSettingId = hiddenId;
                    loanBO.LastModifiedBy = userInformationBO.UserInfoId;
                    loanBO.LastModifiedDate = DateTime.Now;

                    status = loanDA.UpdateEmpLoanInfo(loanBO);
                    if (status)
                    {
                        //isMessageBoxEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpLoan.ToString(), loanBO.LoanSettingId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLoan));
                    }
                }
                else
                {
                    loanBO.CreatedBy = userInformationBO.UserInfoId;
                    loanBO.CreatedDate = DateTime.Now;

                    status = loanDA.SaveEmpLoanInformation(loanBO, out tmpId);
                    if (status)
                    {
                        //isMessageBoxEnable = 2;
                        //lblMessage.Text = "Save Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpLoan.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLoan));
                        LoadLoan();
                        //btnEmpLoanSave.Text = "Update";
                    }
                }
                SetTab("LoanTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
        }
        protected void btnEmpLoanClear_Click(object sender, EventArgs e)
        {
            txtCmpLnIntRate.Text = string.Empty;
            txtPFlnIntRate.Text = string.Empty;
            txtMaxAmtWdrwfmPF.Text = string.Empty;
            txtMinPFavlfrLn.Text = string.Empty;
            txtMinJobLnthfrCmpLn.Text = string.Empty;
            txtDrtnfrNxtLn.Text = string.Empty;
            SetTab("LoanTab");
        }
        protected void btnEmpGratutitySave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsGratuityFrmValid())
                {
                    return;
                }

                GratutitySettingsBO gratuityBO = new GratutitySettingsBO();
                EmpGratuityDA gratuityDA = new EmpGratuityDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                gratuityBO.GratuityWillAffectAfterJobLengthInYear = Convert.ToInt32(txtNoofJobYearfrGrty.Text.Trim());
                gratuityBO.IsGratuityBasedOnBasic = chkIsGrtybsdonBasic.Checked ? true : false;
                gratuityBO.GratutiyPercentage = Convert.ToDecimal(txtGrtyPercntge.Text.Trim());
                gratuityBO.NumberOfGratuityAdded = Convert.ToInt32(txtGrtyNoAdded.Text.Trim());

                int hiddenId = Convert.ToInt32(txtGratuityId.Value);
                int tmpId;
                if (hiddenId > 0)
                {
                    gratuityBO.GratuityId = hiddenId;
                    gratuityBO.LastModifiedBy = userInformationBO.UserInfoId;
                    gratuityBO.LastModifiedDate = DateTime.Now;

                    Boolean status = gratuityDA.UpdateEmpGratuityInfo(gratuityBO);
                    if (status)
                    {
                        //isMessageBoxEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.GratutitySettings.ToString(), gratuityBO.GratuityId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GratutitySettings));
                    }
                }
                else
                {
                    gratuityBO.CreatedBy = userInformationBO.UserInfoId;
                    gratuityBO.CreatedDate = DateTime.Now;

                    Boolean status = gratuityDA.SaveEmpGratuityInfo(gratuityBO, out tmpId);
                    if (status)
                    {
                        //isMessageBoxEnable = 2;
                        //lblMessage.Text = "Save Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.GratutitySettings.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GratutitySettings));
                        LoadGratuity();
                        //btnEmpLoanSave.Text = "Update";
                    }
                }
                SetTab("GratuityTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
        }
        protected void btnEmpGratutityClear_Click(object sender, EventArgs e)
        {
            txtNoofJobYearfrGrty.Text = string.Empty;
            chkIsGrtybsdonBasic.Checked = false;
            txtGrtyPercntge.Text = string.Empty;
            txtGrtyNoAdded.Text = string.Empty;
            SetTab("GratuityTab");
        }
        protected void btnEmpBonusSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<EmpBonusBO> addList = new List<EmpBonusBO>();
                List<EmpBonusBO> deleteList = new List<EmpBonusBO>();

                addList = JsonConvert.DeserializeObject<List<EmpBonusBO>>(hfSaveObj.Value);
                deleteList = JsonConvert.DeserializeObject<List<EmpBonusBO>>(hfDeleteObj.Value);

                if (deleteList.Count == 0)
                {
                    if (!IsBonusFrmValid())
                    {
                        return;
                    }
                }

                EmpBonusBO bonusBO = new EmpBonusBO();
                EmpBonusDA bonusDA = new EmpBonusDA();
                int tmpId;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                bonusBO.BonusType = ddlBonusType.SelectedValue;
                if (bonusBO.BonusType == "PeriodicBonus")
                {
                    bonusBO.BonusAmount = Convert.ToDecimal(txtBonusAmount.Text);
                    bonusBO.AmountType = ddlAmountType.SelectedValue;
                    bonusBO.DependsOnHead = Convert.ToInt32(ddlDependsOn.SelectedValue);
                    bonusBO.EffectivePeriod = Convert.ToByte(ddlBonusMonthlyPeriod.SelectedValue);
                    bonusBO.BonusDate = null;
                }


                for (int i = 0; i < addList.Count; i++)
                {
                    addList[i].EffectivePeriod = null;
                    addList[i].CreatedBy = userInformationBO.UserInfoId;
                }

                if (bonusBO.BonusType == "FestivalBonus")
                {
                    if (addList.Count == 0 && deleteList.Count == 0)
                    {
                        //if (deleteList.Count == 0)
                        //{
                        CommonHelper.AlertInfo(innboardMessage, "Please add or delete at least one Bonus.", AlertType.Warning);
                        ddlBonusType.SelectedIndex = 0;
                        return;
                        //}
                    }
                    if (addList.Count > 0 || deleteList.Count > 0)
                    {
                        Boolean status = bonusDA.SaveOrDeleteEmpFestivalBonusInfo(deleteList, addList);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Operation Successfull.", AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpBonus.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee Fastival Bonus Info");
                        }
                    }
                    //else
                    //{
                    //    Boolean status = bonusDA.UpdateEmpFestivalBonusInfo(addList);
                    //    if (status)
                    //    {
                    //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    //    }
                    //}
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(hfBonusId.Value))
                    {
                        bonusBO.BonusSettingId = Convert.ToInt32(hfBonusId.Value);
                        bonusBO.LastModifiedBy = userInformationBO.UserInfoId;
                        //bonusBO.LastModifiedDate = DateTime.Now;

                        Boolean status = bonusDA.UpdateEmpPeriodicBonusInfo(bonusBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpBonus.ToString(), bonusBO.BonusSettingId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpBonus));
                        }
                    }
                    else
                    {
                        bonusBO.CreatedBy = userInformationBO.UserInfoId;
                        //bonusBO.CreatedDate = DateTime.Now;

                        Boolean status = bonusDA.SaveEmpPeriodicBonusInfo(bonusBO, out tmpId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpBonus.ToString(), bonusBO.BonusSettingId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpBonus));
                        }
                    }
                }
                LoadBonus();
                SetTab("BonusTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
        }
        protected void btnEmpBonusClear_Click(object sender, EventArgs e)
        {
            ddlBonusType.SelectedIndex = 0;
            txtBonusAmount.Text = string.Empty;
            ddlAmountType.SelectedIndex = 0;
            ddlDependsOn.SelectedIndex = 0;
            txtBonusDate.Text = string.Empty;
            ddlBonusMonthlyPeriod.SelectedIndex = 7;
        }
        protected void btnAttendanceDeviceUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsAttendanceDeviceFrmValid())
                {
                    return;
                }

                AttendanceDeviceBO attendanceDeviceBO = new AttendanceDeviceBO();
                AttendanceDeviceDA attendanceDeviceDA = new AttendanceDeviceDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                attendanceDeviceBO.ReaderId = ddlAttendanceDevice.SelectedValue;
                attendanceDeviceBO.ReaderType = ddlDeviceType.SelectedValue;
                attendanceDeviceBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);

                Boolean status = attendanceDeviceDA.UpdateAttendanceDeviceInfo(attendanceDeviceBO);
                if (status)
                {
                    long deviceId = long.Parse(attendanceDeviceBO.ReaderId);
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.AttendanceDevice.ToString(), deviceId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AttendanceDevice));
                }
                SetTab("AttendanceDevice");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }

        }
        private void LoadCompanyContributionOn()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> featureList = new List<CustomFieldBO>();
            featureList = commonDA.GetCustomField("CompanyContributionOn");

            ddlCompanyContributionOn.DataSource = featureList;
            ddlCompanyContributionOn.DataTextField = "Description";
            ddlCompanyContributionOn.DataValueField = "FieldValue";
            ddlCompanyContributionOn.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyContributionOn.Items.Insert(0, item);
        }
        private bool IsLoanFrmValid()
        {
            bool flag = true;
            decimal checkNumber;

            if (txtCmpLnIntRate.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Company Loan Interest Rate";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Loan Interest Rate.", AlertType.Warning);
                flag = false;
                txtCmpLnIntRate.Focus();
            }
            else if (decimal.TryParse(txtCmpLnIntRate.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtCmpLnIntRate.Focus();
            }
            else if (txtPFlnIntRate.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Provident Fund Loan Interest Rate";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Provident Fund Loan Interest Rate.", AlertType.Warning);
                flag = false;
                txtPFlnIntRate.Focus();
            }
            else if (decimal.TryParse(txtPFlnIntRate.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtPFlnIntRate.Focus();
            }
            else if (txtMaxAmtWdrwfmPF.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Max Amount Can Withdraw From Provident Fund";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Max Amount Can Withdraw From Provident Fund.", AlertType.Warning);
                flag = false;
                txtMaxAmtWdrwfmPF.Focus();
            }
            else if (decimal.TryParse(txtMaxAmtWdrwfmPF.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtMaxAmtWdrwfmPF.Focus();
            }
            else if (txtMinPFavlfrLn.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Min Provident Fund Must Available To Allow Loan";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Min Provident Fund Must Available To Allow Loan.", AlertType.Warning);
                flag = false;
                txtMinPFavlfrLn.Focus();
            }
            else if (decimal.TryParse(txtMinPFavlfrLn.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtMinPFavlfrLn.Focus();
            }
            else if (txtMinJobLnthfrCmpLn.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Min Job Length To Allow Company Loan";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Min Job Length To Allow Company Loan.", AlertType.Warning);
                flag = false;
                txtMinJobLnthfrCmpLn.Focus();
            }
            else if (decimal.TryParse(txtMinJobLnthfrCmpLn.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtMinJobLnthfrCmpLn.Focus();
            }
            else if (txtDrtnfrNxtLn.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Duration For Next Loan After Completion Taken Loan";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Duration For Next Loan After Completion Taken Loan.", AlertType.Warning);
                flag = false;
                txtDrtnfrNxtLn.Focus();
            }
            else if (decimal.TryParse(txtDrtnfrNxtLn.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtDrtnfrNxtLn.Focus();
            }
            SetTab("LoanTab");
            return flag;
        }
        private bool IsPFFrmValid()
        {
            bool flag = true;
            decimal checkNumber;

            if (txtEmpCont.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Please Provide Employee Contribution.", AlertType.Warning);
                flag = false;
                txtEmpCont.Focus();
            }
            else if (decimal.TryParse(txtEmpCont.Text, out checkNumber) == false)
            {
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtEmpCont.Focus();
            }
            else if (txtCmpCont.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Contribution.", AlertType.Warning);
                flag = false;
                txtCmpCont.Focus();
            }
            else if (decimal.TryParse(txtCmpCont.Text, out checkNumber) == false)
            {
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtCmpCont.Focus();
            }
            else if (txtEmpMaxCont.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Can Contribute Max Of Basic Salary.", AlertType.Warning);
                flag = false;
                txtEmpMaxCont.Focus();
            }
            else if (decimal.TryParse(txtEmpMaxCont.Text, out checkNumber) == false)
            {
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtEmpMaxCont.Focus();
            }
            else if (decimal.TryParse(txtIntDisRt.Text, out checkNumber) == false)
            {
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtIntDisRt.Focus();
            }
            else if (string.IsNullOrEmpty(txtCmpContElegYear.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Contribution Eligibility Year.", AlertType.Warning);
                flag = false;
                txtCmpContElegYear.Focus();
            }
            else if (decimal.TryParse(txtCmpContElegYear.Text, out checkNumber) == false)
            {
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtCmpContElegYear.Focus();
            }
            SetTab("PFTab");
            return flag;
        }
        private bool IsTaxFrmValid()
        {
            bool flag = true;
            //float checkNumber;

            //if (txtTaxBandM.Text == "")
            //{
            //    //isMessageBoxEnable = 1;
            //    //lblMessage.Text = "Please Provide Tax Band for Male";
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Tax Band for Male.", AlertType.Warning);
            //    flag = false;
            //    txtTaxBandM.Focus();
            //}
            //else if (float.TryParse(txtTaxBandM.Text, out checkNumber) == false)
            //{
            //    //isMessageBoxEnable = 1;
            //    //lblMessage.Text = "You have not entered a valid number";
            //    CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
            //    flag = false;
            //    txtTaxBandM.Focus();
            //}
            //else if (txtTaxBandF.Text == "")
            //{
            //    //isMessageBoxEnable = 1;
            //    //lblMessage.Text = "Please Provide Tax Band for Female";
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Tax Band for Female.", AlertType.Warning);
            //    flag = false;
            //    txtTaxBandF.Focus();
            //}
            //else 
            if (ddlPayrollTaxDeduction.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Payroll Tax Deduction.", AlertType.Warning);
                ddlPayrollTaxDeduction.Focus();
                flag = false;
            }
            //else if (float.TryParse(txtTaxBandF.Text, out checkNumber) == false)
            //{
            //    //isMessageBoxEnable = 1;
            //    //lblMessage.Text = "You have not entered a valid number";
            //    CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
            //    flag = false;
            //    txtTaxBandF.Focus();
            //}
            //else if (int.TryParse(txtCmpContAmount.Text, out checkNumber) == false)
            //{
            //    isMessageBoxEnable = 1;
            //    lblMessage.Text = "You have not entered a valid number";
            //    flag = false;
            //    txtCmpContAmount.Focus();
            //}
            SetTab("TaxTab");
            return flag;
        }
        private bool IsGratuityFrmValid()
        {
            bool flag = true;
            int checkNumber;
            float checkNumberFloat;

            if (txtNoofJobYearfrGrty.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide No of Job Year For Gratutity";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "No of Job Year For Gratutity.", AlertType.Warning);
                flag = false;
                txtNoofJobYearfrGrty.Focus();
            }
            else if (int.TryParse(txtNoofJobYearfrGrty.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtNoofJobYearfrGrty.Focus();
            }
            else if (txtGrtyNoAdded.Text == "")
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide No. of Gratutity Added";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "No. of Gratutity Added.", AlertType.Warning);
                flag = false;
                txtGrtyNoAdded.Focus();
            }
            else if (int.TryParse(txtGrtyNoAdded.Text, out checkNumber) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtGrtyNoAdded.Focus();
            }
            else if (float.TryParse(txtGrtyPercntge.Text, out checkNumberFloat) == false)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtGrtyPercntge.Focus();
            }
            SetTab("GratuityTab");
            return flag;
        }
        private bool IsBonusFrmValid()
        {
            bool flag = true;
            if (ddlBonusType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Bonus Type.", AlertType.Warning);
                ddlBonusType.SelectedIndex = 0;
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtBonusAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bonus Amount.", AlertType.Warning);
                ddlBonusType.SelectedIndex = 0;
                flag = false;
            }
            else if (ddlAmountType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Amount Type.", AlertType.Warning);
                ddlBonusType.SelectedIndex = 0;
                flag = false;
            }
            //else if (string.IsNullOrWhiteSpace(txtBonusDate.Text))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bonus Date.", AlertType.Warning);
            //    flag = false;
            //}
            if (ddlAmountType.SelectedValue == "Percent(%)")
            {
                if (txtBonusAmount.Text.Length > 3)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Percentage cann't be more than 100%.", AlertType.Warning);
                    flag = false;
                }
            }
            SetTab("BonusTab");
            return flag;
        }
        private bool IsAttendanceDeviceFrmValid()
        {
            bool flag = true;
            if (ddlAttendanceDevice.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Attendance Device.", AlertType.Warning);
                ddlAttendanceDevice.SelectedIndex = 0;
                flag = false;
            }
            else if (ddlDeviceType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Device Type.", AlertType.Warning);
                ddlDeviceType.SelectedIndex = 0;
                flag = false;
            }
            SetTab("AttendanceDevice");
            return flag;
        }
        private void LoadLoan()
        {
            LoanSettingBO loanBO = new LoanSettingBO();
            EmpLoanDA loanDA = new EmpLoanDA();
            loanBO = loanDA.GetEmpLoanInformation();

            txtCmpLnIntRate.Text = loanBO.CompanyLoanInterestRate.ToString();
            txtPFlnIntRate.Text = loanBO.PFLoanInterestRate.ToString();
            txtMaxAmtWdrwfmPF.Text = loanBO.MaxAmountCanWithdrawFromPFInPercentage.ToString();
            txtMinPFavlfrLn.Text = loanBO.MinPFMustAvailableToAllowLoan.ToString();
            txtMinJobLnthfrCmpLn.Text = loanBO.MinJobLengthToAllowCompanyLoan.ToString();
            txtDrtnfrNxtLn.Text = loanBO.DurationToAllowNextLoanAfterCompletetionTakenLoan.ToString();
            txtLoanSettingId.Value = loanBO.LoanSettingId.ToString();

            int hiddenId = Convert.ToInt32(txtLoanSettingId.Value);
            if (hiddenId > 0)
            {
                btnEmpLoanSave.Text = "Update";
            }

        }
        private void LoadPF()
        {
            PFSettingBO pfBO = new PFSettingBO();
            EmpPFDA pfDA = new EmpPFDA();
            pfBO = pfDA.GetEmpPFInformation();

            txtEmpCont.Text = pfBO.EmployeeContributionInPercentage.ToString();
            txtCmpCont.Text = pfBO.CompanyContributionInPercentange.ToString();
            txtEmpMaxCont.Text = pfBO.EmployeeCanContributeMaxOfBasicSalary.ToString();
            txtIntDisRt.Text = pfBO.InterestDistributionRate.ToString();
            txtPFSettingId.Value = pfBO.PFSettingId.ToString();
            txtCmpContElegYear.Text = pfBO.CompanyContributionEligibilityYear.ToString();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollPFEmployeeContributionId", "PayrollPFEmployeeContributionId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                ddlEmployeeContributionHeadId.SelectedValue = commonSetupBO.SetupValue;

            }
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollPFCompanyContributionId", "PayrollPFCompanyContributionId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                ddlCompanyContributionHeadId.SelectedValue = commonSetupBO.SetupValue;
            }

            HMCommonSetupBO commonSetupPFCompanyContributionOnBO = new HMCommonSetupBO();
            commonSetupPFCompanyContributionOnBO = commonSetupDA.GetCommonConfigurationInfo("PFCompanyContributionOn", "PFCompanyContributionOn");
            if (!string.IsNullOrEmpty(commonSetupPFCompanyContributionOnBO.SetupValue))
            {
                ddlCompanyContributionOn.SelectedValue = commonSetupPFCompanyContributionOnBO.SetupValue;
            }

            int hiddenId = Convert.ToInt32(txtPFSettingId.Value);
            if (hiddenId > 0)
            {
                btnEmpPFSave.Text = "Update";
            }
        }
        private void LoadTax()
        {
            TaxSettingBO taxBO = new TaxSettingBO();
            EmpTaxDA taxDA = new EmpTaxDA();
            taxBO = taxDA.GetEmpTaxInformation();

            txtTaxBandM.Text = taxBO.TaxBandForMale.ToString();
            txtTaxBandF.Text = taxBO.TaxBandForFemale.ToString();
            if (taxBO.IsTaxPaidByCompany.Equals(true))
            {
                chkIsTaxPaidbyCmp.Checked = true;
            }
            if (!String.IsNullOrWhiteSpace(taxBO.CompanyContributionType))
            {
                ddlCmpContType.SelectedValue = taxBO.CompanyContributionType.ToString();
            }
            txtCmpContAmount.Text = taxBO.CompanyContributionAmount.ToString();
            if (taxBO.IsTaxDeductFromSalary.Equals(true))
            {
                chkIsTaxDdctFrmSlry.Checked = true;
            }
            if (!string.IsNullOrWhiteSpace(taxBO.EmployeeContributionType))
            {
                ddlEmpContType.SelectedValue = taxBO.EmployeeContributionType.ToString();
            }
            if (!string.IsNullOrWhiteSpace(taxBO.Remarks))
            {
                txtRemarks.Text = taxBO.Remarks.ToString();
            }
            txtTaxSettingId.Value = taxBO.TaxSettingId.ToString();

            int hiddenId = Convert.ToInt32(txtTaxSettingId.Value);
            if (hiddenId > 0)
            {
                btnEmpTaxSave.Text = "Update";
            }
        }
        private void LoadGratuity()
        {
            GratutitySettingsBO gratuityBO = new GratutitySettingsBO();
            EmpGratuityDA gratuityDA = new EmpGratuityDA();
            gratuityBO = gratuityDA.GetEmpGratuityInfo();

            txtNoofJobYearfrGrty.Text = gratuityBO.GratuityWillAffectAfterJobLengthInYear.ToString();
            if (gratuityBO.IsGratuityBasedOnBasic.Equals(true))
            {
                chkIsGrtybsdonBasic.Checked = true;
            }
            txtGrtyPercntge.Text = gratuityBO.GratutiyPercentage.ToString();
            txtGrtyNoAdded.Text = gratuityBO.NumberOfGratuityAdded.ToString();
            txtGratuityId.Value = gratuityBO.GratuityId.ToString();

            int hiddenId = Convert.ToInt32(txtGratuityId.Value);
            if (hiddenId > 0)
            {
                btnEmpGratutitySave.Text = "Update";
            }
        }
        private void LoadBonus()
        {
            List<EmpBonusBO> perBonusList = new List<EmpBonusBO>();
            List<EmpBonusBO> fesBonusList = new List<EmpBonusBO>();
            EmpBonusDA bonusDA = new EmpBonusDA();
            perBonusList = bonusDA.GetBonusList("PeriodicBonus");
            fesBonusList = bonusDA.GetBonusList("FestivalBonus");

            if (perBonusList.Count != 0)
            {
                hfBonusId.Value = perBonusList[0].BonusSettingId.ToString();
                ddlBonusType.Text = perBonusList[0].BonusType.ToString();
                txtBonusAmount.Text = perBonusList[0].BonusAmount.ToString();
                ddlAmountType.Text = perBonusList[0].AmountType.ToString();
                ddlDependsOn.Text = perBonusList[0].DependsOnHead.ToString();
            }
            if (fesBonusList.Count != 0)
            {
                ltlTableWiseBonusAdd.InnerHtml = GenerateBonusTable(fesBonusList);
                //txtBonusAmount.Text = fesBonusList[0].BonusAmount.ToString();
                //txtBonusDate.Text = fesBonusList[0].ViewBonusDate.ToString();
                //ddlAmountType.SelectedValue = fesBonusList[0].AmountType.ToString();
                //ddlDependsOn.SelectedValue = fesBonusList[0].DependsOnHead.ToString();
            }
        }
        public void LoadAmountType()
        {
            SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = salaryFormulaDA.GetCustomFields("AmountType", hmUtility.GetDropDownFirstValue());
            ddlAmountType.DataSource = fields;
            ddlAmountType.DataTextField = "FieldValue";
            ddlAmountType.DataValueField = "FieldValue";
            ddlAmountType.DataBind();
        }
        public void GetDependsOnHeadList()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");

            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            var entity = salaryHeadDA.GetSalaryHeadInfoById(Int32.Parse(commonSetupBO.SetupValue));

            List<ItemViewBO> List = new List<ItemViewBO>();
            ItemViewBO itemViewBO = new ItemViewBO();
            itemViewBO.ItemId = entity.SalaryHeadId;
            itemViewBO.ItemName = entity.SalaryHead;
            List.Add(itemViewBO);

            ddlDependsOn.DataSource = List;
            ddlDependsOn.DataValueField = "ItemId";
            ddlDependsOn.DataTextField = "ItemName";
            ddlDependsOn.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SettingsTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "TaxTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");

            }
            else if (TabName == "PFTab")
            {
                C.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "LoanTab")
            {
                D.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "GratuityTab")
            {
                E.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "BonusTab")
            {
                F.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "AttendanceDevice")
            {
                G.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
            }

        }
        public string GenerateBonusTable(List<EmpBonusBO> bonusList)
        {
            string strTable = "";
            var deleteLink = "";

            strTable += "<table cellspacing='0' cellpadding='4' id='BonusInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'>Bonus Id</th><th style='display:none'>Bonus Type</th><th style='display:none'>Depends On</th><th align='left' scope='col' style='width: 30%;'>Bonus Amount</th><th align='left' scope='col' style='width: 30%;'>Amount Type</th><th align='left' scope='col' style='width: 30%;'>Bonus Date</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";
            strTable += "<tbody>";
            int counter = 0;
            if (bonusList != null)
            {
                foreach (EmpBonusBO dr in bonusList)
                {
                    deleteLink = "<a href=\"#\" onclick= 'DeleteBonus(this)' ><img alt=\"Edit\" src=\"../Images/delete.png\" /></a>";
                    counter++;

                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:White;'>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }

                    strTable += "<td align='left' style=\"display:none;\">" + dr.BonusSettingId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.BonusType + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.DependsOnHead + "</td>";
                    strTable += "<td align='left' style=\"width:30%; text-align:Left;\">" + dr.BonusAmount + "</td>";
                    strTable += "<td align='left' style=\"width:30%; text-align:Left;\">" + dr.AmountType + "</td>";
                    strTable += "<td align='left' style=\"width:30%; text-align:Left;\">" + dr.ViewBonusDate + "</td>";

                    strTable += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
                    strTable += "</tr>";

                }
            }
            strTable += "</tbody>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        protected void ddlAttendanceDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlAttendanceDevice.SelectedValue) > 0)
            {
                AttendanceDeviceBO attendanceDeviceBO = new AttendanceDeviceBO();
                AttendanceDeviceDA attendanceDeviceDA = new AttendanceDeviceDA();
                attendanceDeviceBO = attendanceDeviceDA.GetAttendanceDeviceInfoById(ddlAttendanceDevice.SelectedValue);
                if (attendanceDeviceBO != null)
                {
                    if (!attendanceDeviceBO.ReaderId.Equals(""))
                    {
                        ddlDeviceType.SelectedValue = attendanceDeviceBO.ReaderType;
                        SetTab("AttendanceDevice");
                    }
                }
            }
        }

        [WebMethod]
        public static List<PayrollLeaveDeductionPolicyMasterBO> SearchLeaveDeductionPolicy()
        {
            PayrollConfigurationDA DA = new PayrollConfigurationDA();
            List<PayrollLeaveDeductionPolicyMasterBO> files = DA.LeaveDeductionPolicyMaster();
            return files;
        }
        [WebMethod]
        public static List<LeaveTypeBO> LoadLeaveTable()
        {
            LeaveTypeDA da = new LeaveTypeDA();
            List<LeaveTypeBO> files = da.GetLeaveTypeInfo();
            return files;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateLeaveDeductionConfig(PayrollLeaveDeductionPolicyMasterBO PayrollLeaveDeductionPolicyMasterBO, List<PayrollLeaveDeductionPolicyDetailBO> DeductionPolicyDetailList, List<PayrollLeaveDeductionPolicyDetailBO> deletedDeductionPolicyDetailList)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (PayrollLeaveDeductionPolicyMasterBO.Id == 0)
            {
                PayrollLeaveDeductionPolicyMasterBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                PayrollLeaveDeductionPolicyMasterBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            long OutId;
            PayrollConfigurationDA DA = new PayrollConfigurationDA();

            status = DA.SaveOrUpdateLeaveDeductionConfiguration(PayrollLeaveDeductionPolicyMasterBO, DeductionPolicyDetailList, deletedDeductionPolicyDetailList, out OutId);
            if (status)
            {
                if (PayrollLeaveDeductionPolicyMasterBO.Id == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LeaveDeduction.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveDeduction));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LeaveDeduction.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveDeduction));
                }


            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return rtninfo;
        }
        [WebMethod]
        public static PayrollLeaveDeductionPolicyMasterBO FillForm(int Id)
        {
            PayrollConfigurationDA DA = new PayrollConfigurationDA();
            PayrollLeaveDeductionPolicyMasterBO Master = new PayrollLeaveDeductionPolicyMasterBO();
            Master = DA.GetLeaveDeductionPolicyMasterById(Id);
            Master.DetailList = DA.GetLeaveDeductionPolicyDetailsByMasterId(Id);
            return Master;
        }

        [WebMethod]
        public static ReturnInfo DeleteLeaveDeductionPolicy(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            PayrollConfigurationDA DA = new PayrollConfigurationDA();
            status = DA.DeleteLeaveDeductionPolicy(Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LeaveDeduction.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveDeduction));
            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Error);
            }
            return rtninfo;
        }
    }
}
