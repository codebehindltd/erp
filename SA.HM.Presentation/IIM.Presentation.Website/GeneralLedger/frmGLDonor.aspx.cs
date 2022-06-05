using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using System.Collections;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmGLDonor : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                this.LoadCompany();
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
            int donorId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(donorId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("GLDonor", "DonorId", donorId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GLDonor.ToString(), donorId,
                      ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLDonor));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
                LoadGridView();
                this.SetTab("SearchTab");
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

            GLDonorBO projectBO = new GLDonorBO();
            GLDonorDA projectDA = new GLDonorDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            projectBO.Code = this.txtCode.Text;
            projectBO.Name = this.txtName.Text;
            projectBO.ShortName = this.txtShortName.Text;
            projectBO.Description = this.txtDescription.Text;
            //projectBO.CompanyId = Int32.Parse(this.ddlCompanyId.SelectedValue);


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

                int tmpDonorId = 0;
                projectBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = projectDA.SaveGLDonorInfo(projectBO, out tmpDonorId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GLDonor.ToString(), tmpDonorId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLDonor));
                    this.Cancel();
                }
            }
            else
            {
                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Donor Name Already Exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Donor Code Already Exist.", AlertType.Warning);
                    txtCode.Focus();
                    return;
                }

                projectBO.DonorId = Convert.ToInt32(txtProjectId.Value);
                projectBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = projectDA.UpdateGLDonorInfo(projectBO);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GLDonor.ToString(), projectBO.DonorId,
                      ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLDonor));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    this.Cancel();
                }
            }

        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGLDonor.ToString());

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
            int CompanyId = 0;//Int32.Parse(ddlSCompanyName.SelectedValue);
            GLDonorDA projectDA = new GLDonorDA();
            List<GLDonorBO> list = new List<GLDonorBO>();
            list = projectDA.GetAllGLDonorInfoBySearchCriteria(ProjectName, ShortName, CompanyCode, CompanyId);
            this.gvGLProject.DataSource = list;
            this.gvGLProject.DataBind();
            this.SetTab("SearchTab");
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

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCompanyId.Items.Insert(0, item);
            this.ddlSCompanyName.Items.Insert(0, item);
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
        }
        public void FillForm(int EditId)
        {
            GLDonorBO projectBO = new GLDonorBO();
            GLDonorDA projectDA = new GLDonorDA();
            projectBO = projectDA.GetGLDonorInfoById(EditId);
            txtName.Text = projectBO.Name;
            txtShortName.Text = projectBO.ShortName;
            txtCode.Text = projectBO.Code;
            txtDescription.Text = projectBO.Description;
            txtProjectId.Value = projectBO.DonorId.ToString();
            //ddlCompanyId.SelectedValue = projectBO.CompanyId.ToString();
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
            string tableName = "GLDonor";
            string pkFieldName = "DonorId";
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
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Donor Name.", AlertType.Warning);
                this.txtName.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtCode.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Donor Code.", AlertType.Warning);
                txtCode.Focus();
            }

            //else if (this.ddlCompanyId.SelectedIndex == 0)
            //{
            //    status = false;
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Company.", AlertType.Warning);
            //    txtCode.Focus();
            //}

            return status;
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

    }
}