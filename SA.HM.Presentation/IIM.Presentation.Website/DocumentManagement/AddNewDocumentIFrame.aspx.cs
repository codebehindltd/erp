using HotelManagement.Data.DocumentManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.DocumentManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.DocumentManagement
{
    public partial class AddNewDocumentIFrame : System.Web.UI.Page
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
                FileUpload();
                LoadDepartment();
            }
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
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "DocumentDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        [WebMethod]
        public static List<DocumentsForDocManagementBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsForDocManagementBO> docList = new List<DocumentsForDocManagementBO>();
            docList = new DocumentsForDocManagementDA().GetDocumentsByUserTypeAndUserId("DocumentsDoc", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsForDocManagementDA().GetDocumentsByUserTypeAndUserId("DocumentsDoc", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsForDocManagementBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new DocumentsForDocManagementDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateDocument(DMDocumentBO document, string EmpList, int randomDocId, string deletedDoc)
        {
            ReturnInfo info = new ReturnInfo();
            int OwnerIdForDocuments = 0;
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            DocumentsForDocManagementDA documentDA = new DocumentsForDocManagementDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            document.CreatedBy = userInformationBO.UserInfoId;

            long id = 0;

            try
            {
                status = documentDA.SaveOrUpdateDocument(document, EmpList, out id);
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
                Boolean updateStatus = documentDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, document.DocumentName, Convert.ToInt32(randomDocId));

                if (status)
                {
                    info.IsSuccess = true;
                    if (document.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.DMDocuments.ToString(), id, ProjectModuleEnum.ProjectModule.DocumentManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DMDocuments));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.DMDocuments.ToString(), document.Id, ProjectModuleEnum.ProjectModule.DocumentManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DMDocuments));
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
        public static DMDocumentBO GetDocumentId(long id)
        {
            DMDocumentBO document = new DMDocumentBO();
            DocumentsForDocManagementDA documentDA = new DocumentsForDocManagementDA();

            document = documentDA.GetDocumentById(id);
            document.EmployeeList = documentDA.GetDocumentAssignedEmployeeById(id);

            return document;
        }
    }
}