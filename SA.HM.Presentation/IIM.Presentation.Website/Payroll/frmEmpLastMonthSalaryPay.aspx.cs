using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpLastMonthSalaryPay : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSalaryProcessMonth();
                LoadYearList();
            }
        }

        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            ddlEffectedMonth.DataSource = entityDA.GetSalaryProcessMonth();
            ddlEffectedMonth.DataTextField = "MonthHead";
            ddlEffectedMonth.DataValueField = "MonthValue";
            ddlEffectedMonth.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEffectedMonth.Items.Insert(0, item);
        }

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

        //public void LoadGridView(DateTime processDateFrom, DateTime processDateTo)
        //{

        //}

        protected void btnLastMonthSalaryEmployeeLoad_Click(object sender, EventArgs e)
        {
            string salaryYear = ddlYear.SelectedValue;
            string selectedMonthRange = ddlEffectedMonth.SelectedValue.ToString();

            DateTime SalaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, salaryYear));
            DateTime SalaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, salaryYear));

            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeLastSalaryPayBO> salary = new List<EmployeeLastSalaryPayBO>();
            salary = empDa.GetLastMonthSalaryEmployee(Convert.ToInt16(salaryYear), SalaryDateFrom, SalaryDateTo);

            gvLastMonthSalaryEmployee.DataSource = salary;
            gvLastMonthSalaryEmployee.DataBind();
        }

        [WebMethod]
        public static PayrollEmpLastMonthBenifitsPaymentBO GetLastMonthSalaryEmployeeBenifits(int empId, string salaryYear, string selectedMonthRange)
        {
            DateTime SalaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, salaryYear));
            DateTime SalaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, salaryYear));

            PayrollEmpLastMonthBenifitsPaymentBO empBenefits = new PayrollEmpLastMonthBenifitsPaymentBO();
            EmployeeDA empDa = new EmployeeDA();

            empBenefits = empDa.GetLastMonthSalaryEmployeeBenifits(empId, Convert.ToInt16(salaryYear), SalaryDateFrom, SalaryDateTo);

            return empBenefits;
        }

        [WebMethod]
        public static LastMonthSalaryEmployeeBenifitsBO GetLeaveBalanceAmount(int empId, string salaryYear, string selectedMonthRange, string leaveDay)
        {
            DateTime SalaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, salaryYear));
            DateTime SalaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, salaryYear));
            
            LastMonthSalaryEmployeeBenifitsBO empBenefits = new LastMonthSalaryEmployeeBenifitsBO();
            EmployeeDA empDa = new EmployeeDA();

            empBenefits = empDa.GetLeaveBalanceAmount(empId, SalaryDateFrom, SalaryDateTo, Convert.ToInt32(leaveDay));

            return empBenefits;
        }

        [WebMethod]
        public static bool SaveUpdateEmpBenefit(PayrollEmpLastMonthBenifitsPaymentBO benefitBo, string salaryYear, string selectedMonthRange)
        {
            DateTime SalaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, salaryYear));
            DateTime SalaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, salaryYear));

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            
            benefitBo.CreatedBy = userInformationBO.UserInfoId;
            benefitBo.ProcessYear = Convert.ToInt16(salaryYear);
            benefitBo.ProcessDateFrom = SalaryDateFrom;
            benefitBo.ProcessDateTo = SalaryDateTo;
            int tmpId;

            EmployeeDA empDa = new EmployeeDA();

            if (benefitBo.BenifitId == 0)
            {
                bool status = empDa.SavePayrollEmpLastMonthBenifitsPayment(benefitBo, out tmpId);
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.PayrollEmpLastMonthBenifitsPayment.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PayrollEmpLastMonthBenifitsPayment));
                }
                return status;
             }
            else
            {
                bool status = empDa.UpdatePayrollEmpLastMonthBenifitsPayment(benefitBo);
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.PayrollEmpLastMonthBenifitsPayment.ToString(), benefitBo.BenifitId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PayrollEmpLastMonthBenifitsPayment));
                }
                
                return status;
            }
                
        }
    }
}