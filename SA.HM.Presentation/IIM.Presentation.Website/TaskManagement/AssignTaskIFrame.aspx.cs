using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Presentation.Website.TaskManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class AssignTaskIFrame : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        public AssignTaskIFrame() : base("AssignTask")
        {

        }
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                int companyId = Convert.ToInt32(Request.QueryString["cid"]);
                int contactId = Convert.ToInt32(Request.QueryString["cntid"]);
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                //hfId.Value = "0";
                hfParentDoc.Value = "0";
                FileUpload();
                CheckPermission();
                LoadDepartment();
                LoadTaskStage();
                LoadProjectName();
                LoadQuotationNo();
                LoadTaskTypeFor();
                LoadParticipantFromClient(companyId);
                LoadGetAssignTo();
                LoadAccountManager();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void LoadGetAssignTo()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> EmpBO = new List<EmployeeBO>();
            EmpBO = EmpDA.GetEmployeeInfo();

            ddlAssignTo.DataSource = EmpBO;
            ddlAssignTo.DataTextField = "DisplayName";
            ddlAssignTo.DataValueField = "EmpId";
            ddlAssignTo.DataBind();

        }
        private void LoadParticipantFromClient(int companyId)
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();
            contactInformation = DA.GetContactInformationByCompanyId(companyId);

            ddltxtParticipantFromClient.DataSource = contactInformation;
            ddltxtParticipantFromClient.DataTextField = "Name";
            ddltxtParticipantFromClient.DataValueField = "Id";
            ddltxtParticipantFromClient.DataBind();

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

            ddlAssignDepartment.DataSource = depBO;
            ddlAssignDepartment.DataTextField = "Name";
            ddlAssignDepartment.DataValueField = "DepartmentId";
            ddlAssignDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmpDepartment.Items.Insert(0, item);

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
        private void LoadProjectName()
        {
            GLProjectDA DA = new GLProjectDA();
            List<GLProjectBO> SourceBO = new List<GLProjectBO>();
            SourceBO = DA.GetAllGLProjectInfo();

            ddlProjectName.DataSource = SourceBO;
            ddlProjectName.DataTextField = "Name";
            ddlProjectName.DataValueField = "ProjectId";
            ddlProjectName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlProjectName.Items.Insert(0, item);

        }
        private void LoadQuotationNo()
        {
            AssignTaskDA DA = new AssignTaskDA();
            List<SMQuotationBO> SourceBO = new List<SMQuotationBO>();
            SourceBO = DA.GetQuotationInformationForTaskType();

            ddlSaleNo.DataSource = SourceBO;
            ddlSaleNo.DataTextField = "QuotationNo";
            ddlSaleNo.DataValueField = "QuotationId";
            ddlSaleNo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSaleNo.Items.Insert(0, item);

        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlAccountManager.Enabled = true;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                isAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        isAdminUser = 1;
                    }
                }
            }
            #endregion

            accountManagerBOList = accountManagerDA.GetAccountManager(isAdminUser, "CRM", userInformationBO.UserInfoId);

            ddlAccountManager.DataSource = accountManagerBOList;
            ddlAccountManager.DataTextField = "DisplayName";
            ddlAccountManager.DataValueField = "UserInfoId";
            ddlAccountManager.DataBind();

            if (accountManagerBOList != null)
            {
                if (accountManagerBOList.Count > 1)
                {
                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstValue();
                    ddlAccountManager.Items.Insert(0, item);
                }
                else
                {
                    ddlAccountManager.Enabled = false;
                }
            }

            ddlAccountManager.SelectedValue = userInformationBO.UserInfoId.ToString();
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateTask(SMTask task, string EmpList, string ClientLIst, int randomDocId, string deletedDoc)
        {
            ReturnInfo info = new ReturnInfo();
            int OwnerIdForDocuments = 0;
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            AssignTaskDA taskDA = new AssignTaskDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            task.CreatedBy = userInformationBO.UserInfoId;

            long id = 0;

            try
            {
                status = taskDA.SaveOrUpdateTask(task, EmpList, out id);
                if (status && !string.IsNullOrEmpty(ClientLIst))
                {
                    status = taskDA.SaveOrUpdateTaskParticipantClient(id, ClientLIst, "Client");
                }

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
            if (stage.Task.TaskFor == "CRM")
            {
                stage.ContactLIst = taskDA.GetTaskAssignedContactsById(id);
            }
            return stage;
        }
        private void LoadTaskTypeFor()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("TaskType");

            ddlTaskFor.DataSource = fields;
            ddlTaskFor.DataTextField = "Description";
            ddlTaskFor.DataValueField = "FieldValue";
            ddlTaskFor.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTaskFor.Items.Insert(0, item);
            ddlTaskFor.SelectedValue = "0";
        }

        [WebMethod]
        public static List<RestaurantBillBO> GetBillNoByText(string searchTerm)
        {
            HMCommonDA commonDA = new HMCommonDA();
            RestaurentBillDA rrDA = new RestaurentBillDA();
            List<RestaurantBillBO> reservationInfoList = new List<RestaurantBillBO>();
            reservationInfoList = rrDA.GetBillNoByText(searchTerm);

            return reservationInfoList;
        }
        [WebMethod]
        public static ReturnInfo UpdateTaskIsCompleted(long id, bool isCompleted)
        {
            SMTask stage = new SMTask();
            AssignTaskDA taskDA = new AssignTaskDA();
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            bool status = false;

            try
            {

                status = taskDA.UpdateTaskIsCompleted(id, userInformationBO.UserInfoId, isCompleted);
                if (status)
                {
                    info.IsSuccess = true;
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), id,
                        ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return info;
        }

        [WebMethod]
        public static List<SMTask> LoadTaskByTaskTypeNTaskId(string taskType, int taskId)
        {
            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();

            taskList = taskDA.GetAllTaskByTaskTypeNTaskId(taskType, taskId);
            return taskList;
        }

        [WebMethod]
        public static ReturnInfo DeleteTask(long id)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            AssignTaskDA taskDA = new AssignTaskDA();

            try
            {
                status = hmCommonDA.DeleteInfoById("SMTask", "Id", id);
                if (status)
                {
                    info.IsSuccess = true;
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
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
        public static bool HasEmailAddress(int id)
        {
            EmployeeDA managerDA = new EmployeeDA();
            EmployeeBO managerBO = new EmployeeBO();

            managerBO = managerDA.GetEmployeeInfoById(id);

            if (managerBO != null && !string.IsNullOrEmpty(managerBO.OfficialEmail))
                return true;
            else
                return false;
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
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }

        [WebMethod]
        public static HotelRepairNMaintenanceBO GetRepairNMaintenanceById(int id)
        {
            HotelRepairNMaintenanceBO RepairNMaintenance = new HotelRepairNMaintenanceBO();
            HotelRepairNMaintenanceDA DA = new HotelRepairNMaintenanceDA();
            RepairNMaintenance = DA.GetHotelRepairNMaintenanceById(id);

            return RepairNMaintenance;
        }
        //[WebMethod]
        //public static string LoadTaskDocument(long id)
        //{
        //    List<DocumentsBO> docList = new List<DocumentsBO>();

        //    docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskAssignDocuments", (int)id);
        //    docList = new HMCommonDA().GetDocumentListWithIcon(docList);

        //    string strTable = "";
        //    strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
        //    int counter = 0;
        //    foreach (DocumentsBO dr in docList)
        //    {
        //        if (dr.Extention == ".jpg")
        //        {
        //            string ImgSource = dr.Path + dr.Name;
        //            counter++;
        //            strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
        //            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
        //            strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
        //            strTable += "<span>'" + dr.Name + "'</span>";
        //            strTable += "</a>";
        //            strTable += "</div>";
        //        }
        //        else
        //        {
        //            string ImgSource = dr.Path + dr.Name;
        //            counter++;
        //            strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
        //            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
        //            strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
        //            strTable += "<span>'" + dr.Name + "'</span>";
        //            strTable += "</a>";
        //            strTable += "</div>";
        //        }
        //    }
        //    strTable += "</div>";
        //    if (strTable == "")
        //    {
        //        strTable = "<tr><td align='center'>No Record Available!</td></tr>";
        //    }
        //    return strTable;

        //}

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
        public static List<ContactInformationBO> GetContactInformationByCompanyId(int id)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> contactInfo = new List<ContactInformationBO>();
            contactInfo = DA.GetContactInformationByCompanyId(id);
            return contactInfo;
        }

        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyByAutoSearchAndAccountManagerId(string searchTerm, int accountManagerId)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfoByCompanyNameAccountManagerIdAndIndustryId(searchTerm, accountManagerId, 0);

            return companyInfo;
        }
    }
}