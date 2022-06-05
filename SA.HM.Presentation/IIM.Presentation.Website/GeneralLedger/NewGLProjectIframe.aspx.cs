using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
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
    public partial class NewGLProjectIframe : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        public Boolean isViewPermission = false;
        public Boolean isSavePermission = false;
        public Boolean isDeletePermission = false;
        public Boolean isUpdatePermission = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                hfParentDoc.Value = "0";
                FileUpload();
                LoadCompany();
                LoadProjectStage();
                LoadSMCompany();
                LoadCostCenter();
                OnInit();
            }
        }
        private void OnInit()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, "GLProject");

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            isUpdatePermission = objectPermissionBO.IsUpdatePermission;
            isViewPermission = objectPermissionBO.IsViewPermission;

            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            var List = companyDA.GetAllGLCompanyInfo();
            this.ddlCompanyId.DataSource = List;

            this.ddlCompanyId.DataTextField = "Name";
            this.ddlCompanyId.DataValueField = "CompanyId";
            this.ddlCompanyId.DataBind();

            ListItem itemCompany = new ListItem();
            itemCompany.Value = "0";
            itemCompany.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyId.Items.Insert(0, itemCompany);

        }
        private void LoadProjectStage()
        {
            ProjectStageDA companyDA = new ProjectStageDA();
            var List = companyDA.GetAllProjectStages();
            this.ddlProjectStage.DataSource = List;

            this.ddlProjectStage.DataTextField = "ProjectStage";
            this.ddlProjectStage.DataValueField = "Id";
            this.ddlProjectStage.DataBind();

            ListItem itemCompany = new ListItem();
            itemCompany.Value = "0";
            itemCompany.Text = hmUtility.GetDropDownFirstValue();
            ddlProjectStage.Items.Insert(0, itemCompany);
        }
        private void LoadSMCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();

            ddlCompanyName.DataSource = files;
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataValueField = "CompanyId";
            ddlCompanyName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (files.Count > 1)
                ddlCompanyName.Items.Insert(0, item);
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "ProjectId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();
            List<GLProjectWiseCostCenterMappingBO> projects = (from c in files
                                                               select new GLProjectWiseCostCenterMappingBO()
                                                               {
                                                                   CostCenterId = c.CostCenterId,
                                                                   CostCenter = c.CostCenter
                                                               }).ToList();
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableCostCenterInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' style='width:10%; text-align: center' scope='col' ><input id='chkAll' type='checkbox'></th><th align='left' style='width:90%;' scope='col'>Cost Center</th></tr>";
            //strTable += "<tr> <td colspan='2'>";
            //strTable += "<div style=\"height: 375px; overflow-y: scroll; text-align: left;\">";
            //strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableRoomInformation'>";
            int counter = 0;
            foreach (GLProjectWiseCostCenterMappingBO dr in projects)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='center' style='width:10%;'>";
                // strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformDeleteAction(" + dr.RoomId + ")' alt='Delete Information' border='0' />";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.CostCenterId + "' name='" + dr.CostCenter + "' value='" + dr.CostCenterId + "' >";
                strTable += "</td><td align='left' style='width:90%;'>" + dr.CostCenter + "</td>";
                strTable += "<td align='left' style='display:none'>" + dr.CostCenterId + "</td></tr>";

            }

            //strTable += "</table> </div> </td> </tr> </table>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }
            literalCostCenter.Text = strTable;
        }
        public static int DuplicateCheckDynamicaly(string pkFieldValue, string fieldName, string fieldValue, int isUpdate, string customCondition)
        {
            string tableName = "GLProject";
            string pkFieldName = "ProjectId";
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                if (pkFieldValue != "0")
                {
                    isUpdate = 1;
                }
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateDataCheckDynamicalyWithCustomCondition(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue, customCondition);
            return IsDuplicate;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateProject(GLProjectBO GLProjectBO, string CostCenterList, int randomDocId)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;

            if (DuplicateCheckDynamicaly(GLProjectBO.ProjectId.ToString(), "Code", GLProjectBO.Code, 0, "CompanyId = " + GLProjectBO.CompanyId.ToString()) > 0)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo("Code" + AlertMessage.DuplicateValidation, AlertType.Warning);
            }
            else
            {
                int OwnerIdForDocuments = 0;
                HMUtility hmUtility = new HMUtility();
                HMCommonDA hmCommonDA = new HMCommonDA();

                GLProjectBO projectBO = new GLProjectBO();
                GLProjectDA projectDA = new GLProjectDA();

                long id = 0;
                try
                {
                    status = projectDA.SaveGLProjectInfo(GLProjectBO, CostCenterList, out id);
                    if (status)
                        OwnerIdForDocuments = Convert.ToInt32(id);
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));
                    if (status)
                    {
                        info.IsSuccess = true;
                        if (GLProjectBO.ProjectId == 0)
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                            info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                            info.Data = 0;
                        }
                        else
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), GLProjectBO.ProjectId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
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
            }
            return info;
        }
        [WebMethod]
        public static GLProjectBO FillForm(int Id)
        {
            GLProjectBO projectBO = new GLProjectBO();
            GLProjectDA projectDA = new GLProjectDA();
            projectBO = projectDA.GetGLProjectInfoById(Id);
            return projectBO;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLProjectDocument", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLProjectDocument", (int)id));

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