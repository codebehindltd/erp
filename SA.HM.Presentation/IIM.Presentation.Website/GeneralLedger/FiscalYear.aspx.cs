using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class FiscalYear : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
        }
        protected void btnMonthSave_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int tmpFiscalYearId = -1;
            GLFiscalYearBO fiscalYear = new GLFiscalYearBO();
            fiscalYear.FiscalYearName = txtFiscalYearName.Text;
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                //fiscalYear.FromDate = Convert.ToDateTime(txtStartDate.Text);
                fiscalYear.FromDate = CommonHelper.DateTimeToMMDDYYYY(txtStartDate.Text);
                //fiscalYear.FromDate = hmUtility.GetDateTimeFromString(this.txtStartDate.Text, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                //fiscalYear.ToDate = Convert.ToDateTime(txtEndDate.Text);
                fiscalYear.ToDate = CommonHelper.DateTimeToMMDDYYYY(txtEndDate.Text);
                //fiscalYear.ToDate = hmUtility.GetDateTimeFromString(this.txtEndDate.Text, userInformationBO.ServerDateFormat);
            }
            fiscalYear.CreatedBy = userInformationBO.UserInfoId;
            fiscalYear.FiscalYearId = Convert.ToInt32(txtMonthSetupId.Value);
            string projectIdList = hfProjectIdList.Value;
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            Boolean status = fiscalYearDA.SaveOrUpdateGLFiscalYear(fiscalYear, projectIdList, out tmpFiscalYearId);
            if (tmpFiscalYearId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.LedgerMonthSetup.ToString(), fiscalYear.FiscalYearId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LedgerMonthSetup));
                ClearFiscalYearInfo();
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.LedgerMonthSetup.ToString(), tmpFiscalYearId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LedgerMonthSetup));
                btnMonthSave.Text = "Update";
                ClearFiscalYearInfo();
            }
        }
        private void ClearFiscalYearInfo()
        {
            txtFiscalYearName.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
        }
        [WebMethod]
        public static List<GLProjectBO> GetAllGLProjects()
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();
            projectList = entityDA.GetAllGLProjectInfo();

            return projectList;
        }
        [WebMethod]
        public static List<GLFiscalYearBO> GetAllGLFiscalYearInfo()
        {
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            List<GLFiscalYearBO> fiscalYearBO = new List<GLFiscalYearBO>();
            fiscalYearBO = fiscalYearDA.GetAllFiscalYear();
            return fiscalYearBO;
        }

        [WebMethod]
        public static GLFiscalYearViewBO GetFiscalYearInfoByFiscalYearId(int fiscalYearId)
        {
            GLFiscalYearViewBO GLFiscalYear = new GLFiscalYearViewBO();
            GLProjectDA entityDA = new GLProjectDA();
            GLFiscalYear = entityDA.GetFiscalYearViewInfoByFiscalYearId(fiscalYearId);

            return GLFiscalYear;
        }
    }
}