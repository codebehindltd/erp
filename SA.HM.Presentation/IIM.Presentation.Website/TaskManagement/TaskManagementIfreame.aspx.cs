using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.TaskManagement
{
    public partial class TaskManagementIfreame :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public TaskManagementIfreame() : base("AssignTask")
        {

        }
        // string TaskName, DateTime fromDate, DateTime toDate, string assignToId
        [WebMethod]
        public static ArrayList LoadTaskAndStage()
        {
            ArrayList arr = new ArrayList();
            List<SMTask> tasks = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();
            tasks = taskDA.GetAllTaskInfo();

            TaskStageDA DA = new TaskStageDA();
            List<TaskStageBO> StageBO = new List<TaskStageBO>();
            StageBO = DA.GetAllTaskStages();

            arr.Add(new { Tasks = tasks, Stages = StageBO });
            return arr;
        }
        [WebMethod]
        public static ReturnInfo UpdateTaskStage(int stageId, int Id)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            AssignTaskDA taskDA = new AssignTaskDA();
            SMTask task = new SMTask();
            //GuestCompanyBO currentBO = new GuestCompanyBO();
            SMTask previousBO = new SMTask();

            previousBO = taskDA.GetTaskById(Id);

            if (previousBO.TaskStage == stageId)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo("No Change", AlertType.Information);
                return rtninfo;
            }
            else
            {
                rtninfo.IsSuccess = taskDA.UpdateStage(stageId, Id);
            }

            if (rtninfo.IsSuccess)
            {
                task = taskDA.GetTaskById(Id);
                rtninfo.AlertMessage = CommonHelper.AlertInfo("Task Stage Update successful", AlertType.Success);
                rtninfo.Id = task.Id;

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.SMTask.ToString(), task.Id,
                            ProjectModuleEnum.ProjectModule.TaskManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMTask));
            }
            else
            {
                rtninfo.AlertMessage = CommonHelper.AlertInfo("Task Stage Update Failed", AlertType.Warning);
            }

            return rtninfo;
        }
    }
}