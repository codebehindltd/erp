using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmGLProject : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }
            LoadFormConfiguration();
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProjectId.Value = seatingId.ToString();
                this.IsProjectCodeAutoGenerate();
                this.LoadCompany();
                this.LoadProjectStage();
                this.LoadCostCenterInfoGridView();
                this.LoadSMCompany();
                this.FileUpload();
            }
        }
        protected void gvUserGroupCostCenterInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                GLProjectWiseCostCenterMappingBO data = (GLProjectWiseCostCenterMappingBO)e.Row.DataItem;

                CheckBox checkBox = (CheckBox)e.Row.FindControl("chkIsSavePermission");
                checkBox.Checked = data.Id > 0;
            }
        }
        protected void gvGLProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGLProject.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvGLProject_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGLProject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int projectId = 0, companyId = 0;
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

            if (commandArgs.Length > 1)
                companyId = Convert.ToInt32(commandArgs[1]);
            if (commandArgs.Length > 0)
                projectId = Convert.ToInt32(commandArgs[0]);
            if (e.CommandName == "CmdEdit")
            {
                if (projectId > 0)
                    FillForm(projectId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("GLProject", "ProjectId", projectId);
                if (status)
                    hmCommonDA.DeleteInfoById("GLProjectWiseCostCenterMapping", "ProjectId", projectId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GLProject.ToString(), projectId,
                      ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLProject));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
            else if (e.CommandName == "CmdDetails")
            {
                Response.Redirect("/GeneralLedger/ProjectInformation.aspx?pid=" + projectId.ToString() + "&cid=" + companyId.ToString());
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            GLProjectBO projectBO = new GLProjectBO();
            GLProjectDA projectDA = new GLProjectDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            projectBO.Code = this.txtCode.Text;
            projectBO.Name = this.txtName.Text;
            projectBO.ShortName = this.txtShortName.Text;
            projectBO.Description = this.txtDescription.Text;
            projectBO.CompanyId = Int32.Parse(this.ddlCompanyId.SelectedValue);

            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                projectBO.StartDate = hmUtility.GetDateTimeFromString(this.txtStartDate.Text, userInformationBO.ServerDateFormat);
            }
            else
            {
                projectBO.StartDate = null;
            }

            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                projectBO.EndDate = hmUtility.GetDateTimeFromString(this.txtEndDate.Text, userInformationBO.ServerDateFormat);
            }
            else
            {
                projectBO.EndDate = null;
            }

            projectBO.StageId = Int32.Parse(this.ddlProjectStage.SelectedValue);
            projectBO.ProjectAmount = !string.IsNullOrWhiteSpace(this.txtProjectAmount.Text) ? Convert.ToDecimal(this.txtProjectAmount.Text) : 0;

            if (cbCompanyProject.Checked)
                projectBO.ProjectCompanyId = Convert.ToInt64(ddlCompanyName.SelectedValue);

            List<GLProjectWiseCostCenterMappingBO> newCostCenterList = new List<GLProjectWiseCostCenterMappingBO>();
            List<GLProjectWiseCostCenterMappingBO> deletedCostCenterList = new List<GLProjectWiseCostCenterMappingBO>();

            int rowsKitchenItem = gvUserGroupCostCenterInfo.Rows.Count;
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                CheckBox cb = (CheckBox)gvUserGroupCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                int Id = Convert.ToInt32(((Label)gvUserGroupCostCenterInfo.Rows[i].FindControl("lblId")).Text);

                GLProjectWiseCostCenterMappingBO costCenter = new GLProjectWiseCostCenterMappingBO();
                Label lbl = (Label)gvUserGroupCostCenterInfo.Rows[i].FindControl("lblCostCentreId");

                if (cb.Checked && Id == 0)
                {
                    costCenter.CostCenterId = Convert.ToInt32(lbl.Text);
                    newCostCenterList.Add(costCenter);
                }
                else if (!cb.Checked && Id > 0)
                {
                    costCenter.Id = Id;
                    deletedCostCenterList.Add(costCenter);
                }
            }

            string deletedDocuments = hfDeletedDoc.Value;

            if (string.IsNullOrWhiteSpace(txtProjectId.Value))
            {
                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Project Name Already Exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) == 1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Project Code Already Exist.", AlertType.Warning);
                    txtCode.Focus();
                    return;
                }

                int tmpProjectId = 0;
                projectBO.CreatedBy = userInformationBO.UserInfoId;

                long randomDocumentOwnerId = Convert.ToInt64(RandomProjectId.Value);

                //Boolean status = projectDA.SaveGLProjectInfo(projectBO, newCostCenterList, randomDocumentOwnerId, deletedDocuments, out tmpProjectId);
                //if (status)
                //{
                //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                //    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GLProject.ToString(), tmpProjectId,
                //    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLProject));
                //    this.Cancel();
                //}
            }
            else
            {
                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Project Name Already Exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Project Code Already Exist.", AlertType.Warning);
                    txtCode.Focus();
                    return;
                }

                projectBO.ProjectId = Convert.ToInt32(txtProjectId.Value);
                projectBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = projectDA.UpdateGLProjectInfo(projectBO, newCostCenterList, deletedCostCenterList, deletedDocuments);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GLProject.ToString(), projectBO.ProjectId,
                      ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLProject));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    this.Cancel();
                }
            }

        }
        //************************ User Defined Function ********************//
        private void IsProjectCodeAutoGenerate()
        {
            CodeModelLabel.Visible = true;
            CodeModelControl.Visible = true;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsProjectCodeAutoGenerate", "IsProjectCodeAutoGenerate");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsProjectCodeAutoGenerate.Value = homePageSetupBO.SetupValue;
                    if (homePageSetupBO.SetupValue == "1")
                    {
                        CodeModelLabel.Visible = false;
                        CodeModelControl.Visible = false;
                    }
                }
            }
        }
        private void LoadFormConfiguration()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                pnlAdminUser.Visible = true;
                pnlNormalUser.Visible = false;
            }
            else if (userInformationBO.IsAdminUser)
            {
                pnlAdminUser.Visible = true;
                pnlNormalUser.Visible = false;
            }
            else
            {
                pnlAdminUser.Visible = false;
                pnlNormalUser.Visible = true;
            }
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGLProject.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();

            string CompanyCode = txtSCompanyCode.Text;
            string ShortName = txtSShortName.Text;
            string ProjectName = txtSProjectName.Text;
            int CompanyId = Int32.Parse(ddlSCompanyName.SelectedValue);
            GLProjectDA projectDA = new GLProjectDA();
            List<GLProjectBO> list = new List<GLProjectBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                list = projectDA.GetAllGLProjectInfoBySearchCriteria(ProjectName, ShortName, CompanyCode, CompanyId);
                this.gvGLProject.DataSource = list;
                this.gvGLProject.DataBind();
                this.SetTab("SearchTab");
            }
            else if (userInformationBO.IsAdminUser)
            {
                this.gvGLProjectNU.DataSource = null;
                this.gvGLProjectNU.DataBind();
                list = projectDA.GetAllGLProjectInfoBySearchCriteria(ProjectName, ShortName, CompanyCode, CompanyId);
                this.gvGLProject.DataSource = list;
                this.gvGLProject.DataBind();
                this.SetTab("SearchTab");
            }
            else
            {
                list = projectDA.GetUserGroupWiseAllGLProjectInfoBySearchCriteria(userInformationBO.UserGroupId, ProjectName, ShortName, CompanyCode, CompanyId);
                this.gvGLProjectNU.DataSource = list;
                this.gvGLProjectNU.DataBind();
            }
        }
        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            var List = companyDA.GetAllGLCompanyInfo();
            this.ddlCompanyId.DataSource = List;

            this.ddlCompanyId.DataTextField = "Name";
            this.ddlCompanyId.DataValueField = "CompanyId";
            this.ddlCompanyId.DataBind();

            ddlSCompanyName.DataSource = List;
            this.ddlSCompanyName.DataTextField = "Name";
            this.ddlSCompanyName.DataValueField = "CompanyId";
            this.ddlSCompanyName.DataBind();
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
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.ddlCompanyId.SelectedIndex = 0;
            this.txtProjectId.Value = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtShortName.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
            cbCompanyProject.Checked = false;
            //ddlCompanyName.SelectedIndex = 0;
            int rowsStockItem = gvUserGroupCostCenterInfo.Rows.Count;
            for (int i = 0; i < rowsStockItem; i++)
            {
                CheckBox cb = (CheckBox)gvUserGroupCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }
            ProjectDocumentInfo.InnerHtml = "";
        }
        public void FillForm(int EditId)
        {
            Cancel();
            GLProjectBO projectBO = new GLProjectBO();
            GLProjectDA projectDA = new GLProjectDA();
            projectBO = projectDA.GetGLProjectInfoById(EditId);
            txtName.Text = projectBO.Name;
            txtShortName.Text = projectBO.ShortName;
            txtCode.Text = projectBO.Code;
            txtDescription.Text = projectBO.Description;
            txtProjectId.Value = projectBO.ProjectId.ToString();
            ddlCompanyId.SelectedValue = projectBO.CompanyId.ToString();
            ddlProjectStage.SelectedValue = projectBO.StageId.ToString();
            if (projectBO.ProjectCompanyId > 0)
            {
                cbCompanyProject.Checked = true;
                ddlCompanyName.SelectedValue = projectBO.ProjectCompanyId.ToString();
            }

            if (projectBO.StartDate != null)
            {
                txtStartDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(projectBO.StartDate));
            }
            else
            {
                txtStartDate.Text = string.Empty;
            }

            if (projectBO.EndDate != null)
            {
                txtEndDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(projectBO.EndDate));
            }
            else
            {
                txtEndDate.Text = string.Empty;
            }

            txtProjectAmount.Text = projectBO.ProjectAmount.ToString();
            RandomProjectId.Value = projectBO.ProjectId.ToString();
            FileUpload();

            List<DocumentsBO> documents = LoadProjectDocument(EditId, EditId);
            GenerateProjectDocumentList(documents);

            gvUserGroupCostCenterInfo.DataSource = projectBO.CostCenters;
            gvUserGroupCostCenterInfo.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "GLProject";
            string pkFieldName = "ProjectId";
            string pkFieldValue = this.txtProjectId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        public bool isValidForm()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Project Name.", AlertType.Warning);
                this.txtName.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtCode.Text))
            {
                if (hfIsProjectCodeAutoGenerate.Value == "0")
                {
                    status = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Project Code.", AlertType.Warning);
                    txtCode.Focus();
                }
            }
            else if (this.ddlProjectStage.SelectedValue == "0")
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Project Stage.", AlertType.Warning);
                ddlProjectStage.Focus();
            }

            return status;
        }
        private void LoadCostCenterInfoGridView()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();
            List<GLProjectWiseCostCenterMappingBO> projects = (from c in files
                                                               select new GLProjectWiseCostCenterMappingBO()
                                                               {
                                                                   CostCenterId = c.CostCenterId,
                                                                   CostCenter = c.CostCenter
                                                               }).ToList();

            this.gvUserGroupCostCenterInfo.DataSource = projects;
            this.gvUserGroupCostCenterInfo.DataBind();
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
            flashUpload.QueryParameters = "ProjectId=" + Server.UrlEncode(RandomProjectId.Value);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ArrayList PopulateProjects(int companyId)
        {
            ArrayList list = new ArrayList();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();
            projectList = entityDA.GetGLProjectInfoByGLCompany(Convert.ToInt32(companyId));
            int count = projectList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        projectList[i].Name.ToString(),
                                        projectList[i].ProjectId.ToString()
                                         ));
            }
            return list;
        }

        [WebMethod]
        public static ArrayList PopulateFiscalYear(int projectId)
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            fiscalYearList = entityDA.GetFiscalYearListByProjectId(Convert.ToInt32(projectId));
            int count = fiscalYearList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        fiscalYearList[i].FiscalYearName.ToString(),
                                        fiscalYearList[i].FiscalYearId.ToString()
                                         ));
            }
            return list;
        }

        [WebMethod]
        public static ArrayList PopulateAllFiscalYear()
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            fiscalYearList = entityDA.GetAllFiscalYear();
            int count = fiscalYearList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        fiscalYearList[i].FiscalYearName.ToString(),
                                        fiscalYearList[i].FiscalYearId.ToString()
                                         ));
            }
            return list;
        }
        [WebMethod]
        public static List<DocumentsBO> LoadProjectDocument(long id, int randomId = 0, string deletedDoc = "")
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            if (id > 0)
            {
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLProjectDocument", id));
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", id));
            }
            if (randomId != id)
                docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLProjectDocument", randomId);

            if (delete.Count > 0)
                docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        private void GenerateProjectDocumentList(List<DocumentsBO> documents)
        {
            List<DocumentsBO> guestDoc = documents;
            int totalDoc = documents.Count;
            int row = 0;
            string imagePath = "";
            string guestDocumentTable = "";
            if (totalDoc > 0)
            {
                guestDocumentTable += "<table id='dealDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

                for (row = 0; row < totalDoc; row++)
                {
                    if (row % 2 == 0)
                    {
                        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                    }
                    else
                    {
                        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                    }

                    guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                    if (guestDoc[row].Path != "")
                    {
                        if (guestDoc[row].Extention == ".jpg")
                            imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                        else
                            imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    }
                    else
                        imagePath = "";

                    guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                    guestDocumentTable += "<td align='left' style='width: 20%'>";
                    guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                    guestDocumentTable += "</td>";
                    guestDocumentTable += "</tr>";
                }
                guestDocumentTable += "</table>";

                ProjectDocumentInfo.InnerHtml = guestDocumentTable;
            }
        }
    }
}