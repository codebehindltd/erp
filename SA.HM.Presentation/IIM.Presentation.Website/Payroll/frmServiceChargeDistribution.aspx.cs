using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmServiceChargeDistribution : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSalaryProcessMonth();
                LoadYearList();
                CheckObjectPermission();

            }
        }

        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlProcessMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlProcessMonth.DataTextField = "MonthHead";
            this.ddlProcessMonth.DataValueField = "MonthValue";
            this.ddlProcessMonth.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProcessMonth.Items.Insert(0, item);
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlYear.Items.Insert(0, item);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool status = false;
            int tmpId;

            try
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                string selectedMonthRange = string.Empty;
                decimal serviceAmount = 0;

                selectedMonthRange = ddlProcessMonth.SelectedValue;               

                string fromSalaryDate = string.Empty;
                string toSalaryDate = string.Empty;
                DateTime salaryDateFrom = DateTime.Now, salaryDateTo = DateTime.Now;

                //salaryDateFrom = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);
                //salaryDateTo = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);
                salaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                salaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));  

                serviceAmount = Convert.ToDecimal(txtServiceAmount.Text);

                EmployeeDA empDa = new EmployeeDA();
                status = empDa.SaveServiceChargeDistribution(salaryDateFrom, salaryDateTo, Convert.ToInt32( ddlYear.SelectedValue), serviceAmount, userInformationBO.UserInfoId);

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.ServiceChargeConfiguration.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Service Charge Distribution Saved");
                    Cancel();
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
               
            }

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void Cancel()
        {
            txtServiceAmount.Text = string.Empty;
            ddlProcessMonth.SelectedValue = "0";
        }

        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }
    }
}