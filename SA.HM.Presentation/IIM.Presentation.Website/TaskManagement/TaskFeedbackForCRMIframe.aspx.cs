using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.TaskManagement
{
    public partial class TaskFeedbackForCRMIframe : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            int taskId = Convert.ToInt32(Request.QueryString["tid"]);
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                //hfId.Value = "0";
                hfParentDoc.Value = "0";

                LoadTaskStage();
                GetClientPerticipantByTaskId(taskId);
                LoadEmployeeInfo();
            }
        }
        public void LoadTaskStage()
        {
            TaskStageDA DA = new TaskStageDA();
            List<TaskStageBO> StageBO = new List<TaskStageBO>();
            StageBO = DA.GetAllTaskStages();
            ddlTaskStage.DataSource = StageBO;
            ddlTaskStage.DataTextField = "TaskStage";
            ddlTaskStage.DataValueField = "Id";
            ddlTaskStage.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTaskStage.Items.Insert(0, item);
        }
        public void GetClientPerticipantByTaskId(int taskId)
        {
            SMTask task = new SMTask();
            AssignTaskDA taskDA = new AssignTaskDA();
            task = taskDA.GetTaskById(taskId);

            List<ContactInformationBO> contacts = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();
            contacts = DA.GetContactInformationByCompanyId(task.CompanyId);

            ddlParticipantFromClient.DataSource = contacts;
            ddlParticipantFromClient.DataTextField = "Name";
            ddlParticipantFromClient.DataValueField = "Id";
            ddlParticipantFromClient.DataBind();
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddlParticipantFromOffice.DataSource = empList;
            ddlParticipantFromOffice.DataTextField = "DisplayName";
            ddlParticipantFromOffice.DataValueField = "EmpId";
            ddlParticipantFromOffice.DataBind();


        }
        [WebMethod]
        public static SMTaskViewBO GetTaskId(long id)
        {
            SMTaskViewBO stage = new SMTaskViewBO();
            AssignTaskDA taskDA = new AssignTaskDA();

            stage.Task = taskDA.GetTaskById(id);
            stage.EmployeeList = taskDA.GetTaskAssignedEmployeeById(id);
            if (stage.Task.TaskFor == "CRM")
            {
                stage.ContactLIst = taskDA.GetTaskAssignedContactsById(id);
            }
            return stage;
        }

        [WebMethod]
        public static ReturnInfo SaveTaskFeedback(SMTaskFeedbackBO SMTaskFeedback, string participantFromCompany, string participantFromClient, int randomDocId, string deletedDoc)
        {
            bool status;
            ReturnInfo returnInfo = new ReturnInfo();
            int stage = SMTaskFeedback.TaskStage; string feedback = ""; long taskId = SMTaskFeedback.TaskId; int empId = SMTaskFeedback.EmployeeId; bool ImpStatus = true; DateTime date = SMTaskFeedback.FinishDate; DateTime time = SMTaskFeedback.FinishTime;
            long outId = 0;
            int OwnerIdForDocuments = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {
                AssignTaskDA DA = new AssignTaskDA();
                returnInfo.IsSuccess = DA.SaveTaskFeedBackForCRM(SMTaskFeedback, participantFromCompany, participantFromClient, out outId);
                if (returnInfo.IsSuccess)
                    OwnerIdForDocuments = Convert.ToInt32(outId);

                if (returnInfo.IsSuccess && SMTaskFeedback.ImplementationStatus == "Completed")
                {
                    if (SMTaskFeedback.ImplementationStatus == "Completed")
                    {
                        returnInfo.IsSuccess = DA.SaveTaskFeedback(stage, feedback, Convert.ToInt32(taskId), empId, ImpStatus, date, time, out outId);
                    }
                }                

                DocumentsDA documentsDA = new DocumentsDA();
                string s = deletedDoc;
                string[] DeletedDocList = s.Split(',');
                for (int i = 0; i < DeletedDocList.Length; i++)
                {
                    DeletedDocList[i] = DeletedDocList[i].Trim();
                    Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                }
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

                if (returnInfo.IsSuccess)
                {
                    returnInfo.IsSuccess = true;
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), outId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    returnInfo.Data = 0;
                }
                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                Random rd = new Random();
                int randomId = rd.Next(100000, 999999);
                returnInfo.Data = randomId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnInfo;
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskFeedbackDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskFeedbackDocuments", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }


    }
}