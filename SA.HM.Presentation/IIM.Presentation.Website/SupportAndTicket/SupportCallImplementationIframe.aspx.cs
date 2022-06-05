using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SupportAndTicket;
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

namespace HotelManagement.Presentation.Website.SupportAndTicket
{
    public partial class SupportCallImplementationIframe : System.Web.UI.Page
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

                LoadDepartment();
                LoadSupportDropDown();
                LoadCaseOwner();
            }

        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void LoadDepartment()
        {
            DepartmentDA DA = new DepartmentDA();
            List<DepartmentBO> depBO = new List<DepartmentBO>();
            depBO = DA.GetDepartmentInfo();

            ddlEmpDepartment.DataSource = depBO;
            ddlEmpDepartment.DataTextField = "Name";
            ddlEmpDepartment.DataValueField = "DepartmentId";
            ddlEmpDepartment.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmpDepartment.Items.Insert(0, item);

        }
        private void LoadSupportDropDown()
        {
            List<STSupportNCaseSetupInfoBO> SupportNCaseSetupList = new List<STSupportNCaseSetupInfoBO>();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();
            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportStage");

            ddlSupportStage.DataSource = SupportNCaseSetupList;
            ddlSupportStage.DataTextField = "Name";
            ddlSupportStage.DataValueField = "Id";
            ddlSupportStage.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupportStage.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportCategory");

            ddlSupportCategory.DataSource = SupportNCaseSetupList;
            ddlSupportCategory.DataTextField = "Name";
            ddlSupportCategory.DataValueField = "Id";
            ddlSupportCategory.DataBind();

            ddlSupportCategory.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("Case");

            ddlCase.DataSource = SupportNCaseSetupList;
            ddlCase.DataTextField = "Name";
            ddlCase.DataValueField = "Id";
            ddlCase.DataBind();


            ddlCase.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportType");

            ddlSupportType.DataSource = SupportNCaseSetupList;
            ddlSupportType.DataTextField = "Name";
            ddlSupportType.DataValueField = "Id";
            ddlSupportType.DataBind();


            ddlSupportType.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportPriority");

            ddlSupportPriority.DataSource = SupportNCaseSetupList;
            ddlSupportPriority.DataTextField = "Name";
            ddlSupportPriority.DataValueField = "Id";
            ddlSupportPriority.DataBind();


            ddlSupportPriority.Items.Insert(0, item);




        }

        private void LoadCaseOwner()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> CaseOwnerBO = new List<EmployeeBO>();
            CaseOwnerBO = EmpDA.GetEmployeeInfo();

            ddlCaseOwner.DataSource = CaseOwnerBO;
            ddlCaseOwner.DataTextField = "DisplayName";
            ddlCaseOwner.DataValueField = "EmpId";
            ddlCaseOwner.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCaseOwner.Items.Insert(0, item);

            DepartmentDA entityDA = new DepartmentDA();
            ddlSupportForwardTo.DataSource = entityDA.GetDepartmentInfo();
            ddlSupportForwardTo.DataTextField = "Name";
            ddlSupportForwardTo.DataValueField = "DepartmentId";
            ddlSupportForwardTo.DataBind();

            ddlSupportForwardTo.Items.Insert(0, item);


        }
        [WebMethod]
        public static List<EmployeeBO> LoadEmployeeByGroup(int groupId)
        {
            EmployeeDA DA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = DA.GetEmployeeByDepartment(groupId);
            return empList;
        }
        [WebMethod]
        public static List<EmployeeBO> LoadAllEmployee()
        {
            EmployeeDA DA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = DA.GetEmployeeInfo();
            return empList;
        }
        [WebMethod]
        public static List<EmployeeBO> GetEmployeeInformationAutoSearch(string searchString)
        {
            EmployeeDA DA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = DA.GetEmpInformationForAutoSearch(searchString);
            return empList;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateTask(SMTask task, string EmpList, int randomDocId, string deletedDoc)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            int OwnerIdForDocuments = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            AssignTaskDA taskDA = new AssignTaskDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            task.TaskDate = DateTime.Now;
            task.StartTime = DateTime.Now;
            task.CreatedBy = userInformationBO.UserInfoId;

            long id = 0;

            try
            {
                status = taskDA.SaveOrUpdateTask(task, EmpList, out id);

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

                if (status)
                {
                    info.IsSuccess = true;
                    if (task.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), task.Id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                Random rd = new Random();
                int randomId = rd.Next(100000, 999999);
                info.Data = randomId;

            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return info;
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
        public static STSupportBO GetSupportById(long id)
        {
            STSupportBO BO = new STSupportBO();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();

            BO = supportDA.GetSupportCallInformationById(id);

            return BO;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskAssignDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskAssignDocuments", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            //docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        [WebMethod]
        public static List<DocumentsBO> GetCallCenterUploadedDocByWebMethod(int id, int supportCallId)
        {
            List<int> delete = new List<int>();
            
            List<DocumentsBO> docList = new List<DocumentsBO>();

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskAssignDocuments", (int)id));

            if (supportCallId > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupportAndTicketDoc", (int)supportCallId));

            
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
        public static List<DocumentsBO> GetFeedbackUploadedDocByWebMethod(int id, int supportCallId)
        {
            List<int> delete = new List<int>();

            List<DocumentsBO> docList = new List<DocumentsBO>();

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskFeedbackDocuments", (int)id));

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