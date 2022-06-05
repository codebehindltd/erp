using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpGratuityApproval : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
            if (!IsPostBack)
            {

            }
        }
        
        protected void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        //************************ User Defined WebMethod ********************//       
        [WebMethod]
        public static EmployeeBO GetEmpForGratuityApproval(string employeeId)
        {
            int empId = 0;

            if (!string.IsNullOrEmpty(employeeId))
            {
                empId = Convert.ToInt32(employeeId);
            }

            EmployeeBO empBO = new EmployeeBO();
            EmployeeDA empDA = new EmployeeDA();
            EmpGratuityDA grtDa = new EmpGratuityDA();
            GratutitySettingsBO gratuityInfo = new GratutitySettingsBO();

            if (empId > 0)
            {
                empBO = empDA.GetEmployeeInfoById(empId);
                gratuityInfo = grtDa.GetEmpGratuityInfo();

                empBO.ProbableGratuityEligibilityDate = empBO.JoinDate.Value.AddYears(gratuityInfo.GratuityWillAffectAfterJobLengthInYear);
            }
            
            return empBO;
        }

        [WebMethod]
        public static ReturnInfo ApproveGratuity(string employeeId, string graEligibleDate)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            int empId = 0; DateTime? gratuityEligibilityDate = null;
            if (!string.IsNullOrEmpty(employeeId))
            {
                empId = Convert.ToInt32(employeeId);
            }
            if (!string.IsNullOrEmpty(graEligibleDate))
            {
                //gratuityEligibilityDate = Convert.ToDateTime(graEligibleDate);
                gratuityEligibilityDate = CommonHelper.DateTimeToMMDDYYYY(graEligibleDate);
            }
            bool status = false;
            if (empId > 0 && gratuityEligibilityDate != null)
            {
                EmpGratuityDA gratuityDA = new EmpGratuityDA();
                status = gratuityDA.ApproveGratuity(empId, gratuityEligibilityDate);
            }
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.EmpGratuity.ToString(), empId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpGratuity));
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