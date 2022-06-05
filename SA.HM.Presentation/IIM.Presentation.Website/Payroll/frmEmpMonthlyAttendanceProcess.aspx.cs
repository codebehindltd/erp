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

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpMonthlyAttendanceProcess : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
                LoadSalaryProcessMonth();
                LoadYearList();
            }
        }
        protected void btnAttendanceProcess_Click(object sender, EventArgs e)
        {
            Boolean status = false;

            try
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO salaryProcessType = new HMCommonSetupBO();
                salaryProcessType = commonSetupDA.GetCommonConfigurationInfo("PayrollSalaryExecutionProcessType", "PayrollSalaryExecutionProcessType");

                List<AttendanceAndLeaveApprovalBO> empSalaryProcessBOList = new List<AttendanceAndLeaveApprovalBO>();
                EmpSalaryProcessDA empSalaryProcessDA = new EmpSalaryProcessDA();

                string selectedMonthRange = ddlEffectedMonth.SelectedValue.ToString();

                DateTime salaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                DateTime salaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));

                HMCommonDA hmCoomnoDA = new HMCommonDA();
                DayCloseBO dayCloseBO = new DayCloseBO();

                dayCloseBO = hmCoomnoDA.GetPayrollMonthlyAttendanceProcessLog(salaryDateFrom, salaryDateTo);
                if (dayCloseBO != null)
                {
                    if (dayCloseBO.Id > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Monthly Attendance Already Processed, Please Try With Another Month.", AlertType.Warning);
                        this.ddlEffectedMonth.Focus();
                        return;
                    }
                }

                status = empSalaryProcessDA.EmployeeLateAttendanceDeductionProcess("Deduction", -1, salaryDateFrom, salaryDateTo, userInformationBO.UserInfoId);
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpSalaryProcess.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Employee's Monthly Late Attendance Deduction Process");
                    CommonHelper.AlertInfo(innboardMessage, "Employee's Monthly Late Attendance Deduction Process Operation Successfull.", AlertType.Success);
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Employee's Monthly Late Attendance Deduction Process Operation Failed.", AlertType.Error);
                }

                //empSalaryProcessBOList = empSalaryProcessDA.GetEmployeeLateAttendanceDeductionProcess("Selection", -1, salaryDateFrom, salaryDateTo, userInformationBO.UserInfoId);
                //gvAttendance.DataSource = empSalaryProcessBOList;
                //gvAttendance.DataBind();
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
        }
        private void CheckObjectPermission()
        {
            btnAttendanceProcess.Visible = isSavePermission;
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
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}