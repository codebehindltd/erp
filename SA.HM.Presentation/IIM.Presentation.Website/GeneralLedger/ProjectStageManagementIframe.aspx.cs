using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class ProjectStageManagementIframe : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public ProjectStageManagementIframe() : base("GLProject")
        {
             
        }
        // string ProjectName, DateTime fromDate, DateTime toDate, string assignToId
        [WebMethod]
        public static ArrayList LoadProjectAndStage()
        {
            ArrayList arr = new ArrayList();
            List<GLProjectBO> project = new List<GLProjectBO>();
            GLProjectDA projectDA = new GLProjectDA();
            project = projectDA.GetAllGLProjectInfo();

            ProjectStageDA DA = new ProjectStageDA();
            List<ProjectStageBO> StageBO = new List<ProjectStageBO>();
            StageBO = DA.GetAllProjectStages();

            arr.Add(new { Projects = project, Stages = StageBO });
            return arr;
        }
        [WebMethod]
        public static ReturnInfo UpdateProjectStage(int stageId, int Id)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            GLProjectDA projectDA = new GLProjectDA();
            GLProjectBO project = new GLProjectBO();
            //GuestCompanyBO currentBO = new GuestCompanyBO();
            GLProjectBO previousBO = new GLProjectBO();

            previousBO = projectDA.GetGLProjectInfoById(Id);

            if (previousBO.StageId == stageId)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo("No Change", AlertType.Information);
                return rtninfo;
            }
            else
            {
                rtninfo.IsSuccess = projectDA.UpdateStage(stageId, Id);
            }

            if (rtninfo.IsSuccess)
            {
                project = projectDA.GetGLProjectInfoById(Id);
                rtninfo.AlertMessage = CommonHelper.AlertInfo("Project Stage Update successful", AlertType.Success);
                rtninfo.Id = project.ProjectId;

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.GLProject.ToString(), project.ProjectId,
                            ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLProject));
            }
            else
            {
                rtninfo.AlertMessage = CommonHelper.AlertInfo("Project Stage Update Failed", AlertType.Warning);
            }

            return rtninfo;
        }
    }
}