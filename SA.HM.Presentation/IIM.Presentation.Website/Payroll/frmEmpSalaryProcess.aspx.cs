using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpSalaryProcess : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "select";
                CheckObjectPermission();
                LoadSalaryProcessMonth();
                LoadYearList();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void gvSalaryFormula_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool status = false;
            string currencyType = ddlCurrencyType.SelectedValue;
            int salaryProcessId = Convert.ToInt32(e.CommandArgument.ToString());
            EmpSalaryProcessDA salaryProcessDa = new EmpSalaryProcessDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (e.CommandName == "CmdApproved")
            {
                status = salaryProcessDa.ApprovedMonthlySalaryProcess(salaryProcessId, userInformationBO.UserInfoId);

                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.EmpSalaryProcess.ToString(), salaryProcessId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Employee salary process is approved");

                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                    LoadGridView();
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            else if (e.CommandName == "CmdShowStatement")
            {
                string url = "/Payroll/Reports/frmReportEmpSalarySheetTemp.aspx?pid=" + salaryProcessId.ToString() + 
                             "&mr=" + ddlSEffectedMonth.SelectedValue + "&yr=" + ddlSYear.SelectedValue +
                             "&ct=" + currencyType;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=850,height=780,left=200,top=50,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
        }
        protected void gvSalaryFormula_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void btnSalryProcess_Click(object sender, EventArgs e)
        {
            Boolean status = false;
            Boolean isCompanyProjectWiseEmployeeSalaryProcessEnable = false;
            hfIsCompanyProjectWiseEmployeeSalaryProcessEnable.Value = "0";

            try
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                HMCommonSetupBO IsCompanyProjectWiseEmployeeSalaryProcessEnable = new HMCommonSetupBO();
                IsCompanyProjectWiseEmployeeSalaryProcessEnable = commonSetupDA.GetCommonConfigurationInfo("IsCompanyProjectWiseEmployeeSalaryProcessEnable", "IsCompanyProjectWiseEmployeeSalaryProcessEnable");
                if (IsCompanyProjectWiseEmployeeSalaryProcessEnable != null)
                {
                    if (IsCompanyProjectWiseEmployeeSalaryProcessEnable.SetupValue == "1")
                    {
                        isCompanyProjectWiseEmployeeSalaryProcessEnable = true;
                        hfIsCompanyProjectWiseEmployeeSalaryProcessEnable.Value = "1";
                    }
                }

                HMCommonSetupBO salaryProcessType = new HMCommonSetupBO();
                salaryProcessType = commonSetupDA.GetCommonConfigurationInfo("PayrollSalaryExecutionProcessType", "PayrollSalaryExecutionProcessType");

                EmpSalaryProcessBO empSalaryProcessBO = new EmpSalaryProcessBO();
                EmpSalaryProcessDA empSalaryProcessDA = new EmpSalaryProcessDA();

                string selectedMonthRange = ddlEffectedMonth.SelectedValue.ToString();

                int companyId = Int32.Parse(hfCompanyId.Value);
                int projectId = Int32.Parse(hfProjectId.Value);

                empSalaryProcessBO.SalaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                empSalaryProcessBO.SalaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));
                empSalaryProcessBO.CreatedBy = userInformationBO.UserInfoId;

                status = empSalaryProcessDA.SaveMonthlySalaryProcessTemp(companyId, projectId, empSalaryProcessBO.SalaryDateFrom, empSalaryProcessBO.SalaryDateTo, Convert.ToInt16(ddlYear.SelectedValue), userInformationBO.UserInfoId, Convert.ToInt32(salaryProcessType.SetupValue));

                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpSalaryProcess.ToString(), empSalaryProcessBO.ProcessId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Employee's monthly salary process is saved");
                    CommonHelper.AlertInfo(innboardMessage, "Salary Process Operation Successfull.", AlertType.Success);
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Salary Process Operation Failed.", AlertType.Error);
                }
                SetTab("EntryTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, ex.Message.ToString(), AlertType.Error);
            }
        }
        //************************ User Defined Function ********************//
        private void LoadYearList()
        {
            List<string> fields = new List<string>();
            fields = hmUtility.GetReportYearList();

            ddlYear.DataSource = fields;
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlYear.Items.Insert(0, item);

            ddlSYear.DataSource = fields;
            ddlSYear.DataBind();
            ddlSYear.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            btnSalryProcess.Visible = isSavePermission;
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            List<SalaryProcessMonthBO> monthBOList = new List<SalaryProcessMonthBO>();
            monthBOList = entityDA.GetSalaryProcessMonth();

            ddlEffectedMonth.DataSource = monthBOList;
            ddlEffectedMonth.DataTextField = "MonthHead";
            ddlEffectedMonth.DataValueField = "MonthValue";
            ddlEffectedMonth.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEffectedMonth.Items.Insert(0, item);

            ddlSEffectedMonth.DataSource = monthBOList;
            ddlSEffectedMonth.DataTextField = "MonthHead";
            ddlSEffectedMonth.DataValueField = "MonthValue";
            ddlSEffectedMonth.DataBind();
            ddlSEffectedMonth.Items.Insert(0, item);
        }
        public static List<DateTime> GetDateArrayBetweenTwoDates(DateTime StartDate, DateTime EndDate)
        {
            var dates = new List<DateTime>();

            for (var dt = StartDate; dt <= EndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt.AddDays(1).AddSeconds(-1));
            }
            return dates;
        }
        public void LoadGridView()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<EmpSalaryProcessBO> salaryProcess = new List<EmpSalaryProcessBO>();
            EmpSalaryProcessDA processDa = new EmpSalaryProcessDA();

            int companyId = Int32.Parse(hfSrcCompanyId.Value);
            int projectId = Int32.Parse(hfSrcProjectId.Value);

            string salaryYear = ddlSYear.SelectedValue;
            string selectedMonthRange = ddlSEffectedMonth.SelectedValue.ToString();

            DateTime SalaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, salaryYear));
            DateTime SalaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, salaryYear));

            salaryProcess = processDa.GetSalaryProcessFromTempTable(companyId, projectId, Convert.ToInt16(salaryYear), SalaryDateFrom, SalaryDateTo);
            gvSalaryFormula.DataSource = salaryProcess;
            gvSalaryFormula.DataBind();

            SetTab("SearchTab");
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
    }
}