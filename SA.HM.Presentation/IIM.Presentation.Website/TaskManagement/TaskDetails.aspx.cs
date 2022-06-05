using HotelManagement.Data.HMCommon;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.TaskManagement
{
    public partial class TaskDetails : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                //hfId.Value = "0";
                hfParentDoc.Value = "0";
                FileUpload();
                LoadTaskTypeFor();
                LoadTaskStage();
            }
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void LoadTaskStage()
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
        private void LoadTaskTypeFor()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("TaskType");

            ddlTaskType.DataSource = fields;
            ddlTaskType.DataTextField = "Description";
            ddlTaskType.DataValueField = "FieldValue";
            ddlTaskType.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTaskType.Items.Insert(0, item);
            ddlTaskType.SelectedValue = "0";
        }
        [WebMethod]
        public static SMTaskViewBO GetTaskId(long id)
        {
            SMTaskViewBO stage = new SMTaskViewBO();
            AssignTaskDA taskDA = new AssignTaskDA();

            stage.Task = taskDA.GetTaskById(id);
            stage.EmployeeList = taskDA.GetTaskAssignedEmployeeById(id);

            return stage;
        }

        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int id, int SupportId)
        {

            List<DocumentsBO> docList = new List<DocumentsBO>();

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskAssignDocuments", (int)id));

            if (SupportId > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupportAndTicketDoc", (int)SupportId));

            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }

        [WebMethod]
        public static ReturnInfo SaveTaskFeedback(int stage, string feedback, int taskId, int empId, bool ImpStatus, DateTime date, DateTime time, int randomDocId, string deletedDoc)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            int OwnerIdForDocuments = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            AssignTaskDA taskDA = new AssignTaskDA();

            long id = 0;

            try
            {
                status = taskDA.SaveTaskFeedback(stage, feedback, taskId, empId, ImpStatus, date, time, out id);

                if (status)
                {
                    if (status)
                        OwnerIdForDocuments = Convert.ToInt32(id);

                    DocumentsDA documentsDA = new DocumentsDA();
                    string s = deletedDoc;
                    string[] DeletedDocList = s.Split(',');
                    for (int i = 0; i < DeletedDocList.Length; i++)
                    {
                        DeletedDocList[i] = DeletedDocList[i].Trim();
                        Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

                    info.IsSuccess = true;
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Random rd = new Random();
                    int randomId = rd.Next(100000, 999999);
                    info.Data = randomId;

                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return info;
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocForFeedbackByWebMethod(int randomId, int id, string deletedDoc)
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