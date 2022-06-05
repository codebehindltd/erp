using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpPFApproval : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
            }
        }
        [WebMethod]
        public static EmployeeBO GetEmpForPFApproval(string employeeId)
        {
            int empId = 0;

            if (!string.IsNullOrEmpty(employeeId))
            {
                empId = Convert.ToInt32(employeeId);
            }

            EmployeeBO empBO = new EmployeeBO();
            EmployeeDA empDA = new EmployeeDA();
            empBO = empDA.GetEmployeeInfoById(empId);

            if (empBO.PFEligibilityDate == null)
            {
                PFSettingBO pfSettingBO = new PFSettingBO();
                EmpPFDA pfDA = new EmpPFDA();
                pfSettingBO = pfDA.GetEmpPFInformation();

                empBO.ProbablePFEligibilityDate = empBO.JoinDate.Value.AddYears(pfSettingBO.CompanyContributionEligibilityYear);
            }
            else
            {
                empBO.ProbablePFEligibilityDate = empBO.PFEligibilityDate.Value;
            }

            return empBO;
        }
        [WebMethod]
        public static ReturnInfo ApprovePF(string employeeId, string pfEligibleDate)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            int empId = 0;
            DateTime? pfEligibilityDate = null;
            if (!string.IsNullOrEmpty(employeeId))
            {
                empId = Convert.ToInt32(employeeId);
            }
            if (!string.IsNullOrEmpty(pfEligibleDate))
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                pfEligibilityDate = hmUtility.GetDateTimeFromString(pfEligibleDate, userInformationBO.ServerDateFormat); //Convert.ToDateTime(pfEligibleDate);
            }
            bool status = false;
            if (empId > 0 && pfEligibilityDate != null)
            {
                EmpPFDA pfDA = new EmpPFDA();
                status = pfDA.ApprovePF(empId, pfEligibilityDate);

            }
            if (status)
            {
                rtninf.IsSuccess = true;

                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.PFSetting.ToString(), empId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PFSetting));

                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo TerminatePF(string EmployeeId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            int empId = 0;
            HMUtility hmUtility = new HMUtility();
            if (!string.IsNullOrEmpty(EmployeeId))
            {
                empId = Convert.ToInt32(EmployeeId);
            }
            bool status = false;
            EmpPFDA pfDA = new EmpPFDA();
            status = pfDA.UpdateEmpForPFTermination(empId);

            if (status)
            {
                rtninf.IsSuccess = true;
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.PFSetting.ToString(), empId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Provident Fund Termination Approved");

                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;

        }
    }
}