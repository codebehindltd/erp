using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmployeeTermination : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadEmployeeStatus();
            }
        }

        public void LoadEmployeeStatus()
        {
            EmployeeDA costCentreTabDA = new EmployeeDA();
            var List = costCentreTabDA.GetEmployeeStatus();
            ddlEmployeeStatus.DataSource = List;
            ddlEmployeeStatus.DataTextField = "EmployeeStatus";
            ddlEmployeeStatus.DataValueField = "EmployeeStatusId";
            ddlEmployeeStatus.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployeeStatus.Items.Insert(0, item);
        }

        [WebMethod]
        public static ReturnInfo SaveTermination(PayrollEmpTerminationBO termination)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //termination.DecisionDate = Convert.ToDateTime(txtDecisionDate.Text);
                //termination.TerminationDate = Convert.ToDateTime(txtTerminationDate.Text);
                //termination.EmpId = Convert.ToInt32(hfEmployeeId.Value);
                //termination.Remarks = txtRemarks.Text;

                termination.CreatedBy = userInformationBO.UserInfoId;

                EmployeeDA empda = new EmployeeDA();
                int tmpId;
                rtninf.IsSuccess = empda.SaveEmployeeTermination(termination, out tmpId);

                if (rtninf.IsSuccess)
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.PayrollEmpTerminatio.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PayrollEmpTerminatio));
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
    }
}