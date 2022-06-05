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
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmGLCompany : System.Web.UI.Page
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
                LoadddlBudgetType();
                LoadInterCompany();
            }
        }
        protected void gvGLCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvGLCompany.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvGLCompany_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvGLCompany_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int companyId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(companyId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("GLCompany", "CompanyId", companyId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GLCompany.ToString(), companyId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLCompany));
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

            GLCompanyBO companyBO = new GLCompanyBO();
            GLCompanyDA companyDA = new GLCompanyDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            companyBO.Code = this.txtCode.Text;
            companyBO.Name = this.txtName.Text;
            companyBO.ShortName = this.txtShortName.Text;
            companyBO.Description = this.txtDescription.Text;
            companyBO.CompanyAddress = this.txtCompanyAddress.Text;
            companyBO.WebAddress = this.txtWebAddress.Text;
            companyBO.BinNumber = this.txtBinNumber.Text;
            companyBO.TinNumber = this.txtTinNumber.Text;
            companyBO.BudgetType = this.ddlBudgetType.SelectedValue;
            companyBO.InterCompanyTransactionHeadId = Convert.ToInt32(ddlInterCompanyName.SelectedValue);

            if (string.IsNullOrWhiteSpace(txtCompanyId.Value))
            {
                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                {
                    CommonHelper.AlertInfo("Project Name Already Exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) == 1)
                {
                    CommonHelper.AlertInfo("Project Code Already Exist.", AlertType.Warning);
                    txtCode.Focus();
                    return;
                }

                int tmpCmpId = 0;
                companyBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = companyDA.SaveGLCompanyInfo(companyBO, out tmpCmpId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GLCompany.ToString(), tmpCmpId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLCompany));
                    this.Cancel();
                }
            }
            else
            {
                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo("Project Name Already Exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo("Project Code Already Exist.", AlertType.Warning);
                    txtCode.Focus();
                    return;
                }
                companyBO.CompanyId = Convert.ToInt32(txtCompanyId.Value);
                companyBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = companyDA.UpdateGLCompanyInfo(companyBO);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GLCompany.ToString(), companyBO.CompanyId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLCompany));
                    this.Cancel();
                }
            }

        }
        //************************ User Defined Function ********************//
        private void LoadddlBudgetType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BudgetType", hmUtility.GetDropDownFirstValue());

            ddlBudgetType.DataSource = fields;
            ddlBudgetType.DataTextField = "FieldValue";
            ddlBudgetType.DataValueField = "FieldValue";
            ddlBudgetType.DataBind();
        }
        private void LoadInterCompany()
        {
            hfIsInterCompanyExist.Value = "0";
            ListItem itemCompanyForAll = new ListItem();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> interCompanyList = new List<NodeMatrixBO>();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InterCompanyTransactionHeadId", "InterCompanyTransactionHeadId");
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    NodeMatrixBO matrixBO = new NodeMatrixBO();
                    NodeMatrixDA matrixDA = new NodeMatrixDA();
                    if (string.IsNullOrWhiteSpace(commonSetupBO.SetupValue))
                    {
                        commonSetupBO.SetupValue = "0";
                    }

                    matrixBO = matrixDA.GetNodeMatrixInfoById(Convert.ToInt32(commonSetupBO.SetupValue));
                    if (matrixBO != null)
                    {
                        if (matrixBO.NodeId > 0)
                        {
                            interCompanyList = entityDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('" + matrixBO.Hierarchy + "', Hierarchy) = 1 AND IsTransactionalHead = 1");

                            if (interCompanyList.Count == 1)
                            {
                                ddlInterCompanyName.DataSource = interCompanyList;
                                ddlInterCompanyName.DataTextField = "NodeHead";
                                ddlInterCompanyName.DataValueField = "NodeId";
                                ddlInterCompanyName.DataBind();
                                hfIsInterCompanyExist.Value = "1";
                            }
                            else
                            {
                                hfIsInterCompanyExist.Value = "1";
                                ddlInterCompanyName.DataSource = interCompanyList;
                                ddlInterCompanyName.DataTextField = "NodeHead";
                                ddlInterCompanyName.DataValueField = "NodeId";
                                ddlInterCompanyName.DataBind();
                                hfIsInterCompanyExist.Value = "0";
                                ListItem itemCompany = new ListItem();
                                itemCompany.Value = "0";
                                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                                ddlInterCompanyName.Items.Insert(0, itemCompany);
                            }
                        }
                        else
                        {
                            itemCompanyForAll.Value = "0";
                            itemCompanyForAll.Text = hmUtility.GetDropDownFirstAllValue();
                            ddlInterCompanyName.Items.Insert(0, itemCompanyForAll);
                        }
                    }
                    else
                    {                        
                        itemCompanyForAll.Value = "0";
                        itemCompanyForAll.Text = hmUtility.GetDropDownFirstAllValue();
                        ddlInterCompanyName.Items.Insert(0, itemCompanyForAll);
                    }
                }
                else
                {
                    itemCompanyForAll.Value = "0";
                    itemCompanyForAll.Text = hmUtility.GetDropDownFirstAllValue();
                    ddlInterCompanyName.Items.Insert(0, itemCompanyForAll);
                }
            }
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGLCompany.ToString());

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

            string Name = txtSName.Text;
            string Code = txtSCode.Text;
            string ShortName = txtSShortName.Text;

            this.CheckObjectPermission();
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> list = new List<GLCompanyBO>();
            list = companyDA.GetAllGLCompanyInfoBySearchCriteria(Name, Code, ShortName);
            this.gvGLCompany.DataSource = list;
            this.gvGLCompany.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.txtCompanyId.Value = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtShortName.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.txtCompanyAddress.Text = string.Empty;
            this.txtWebAddress.Text = string.Empty;
            this.txtBinNumber.Text = string.Empty;
            this.txtTinNumber.Text = string.Empty;
            this.ddlBudgetType.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
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
            string tableName = "GLCompany";
            string pkFieldName = "CompanyId";
            string pkFieldValue = this.txtCompanyId.Value;
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
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Project Code.", AlertType.Warning);
                txtCode.Focus();
            }

            return status;

        }
        public void FillForm(int EditId)
        {
            GLCompanyBO companyBO = new GLCompanyBO();
            GLCompanyDA companyDA = new GLCompanyDA();
            companyBO = companyDA.GetGLCompanyInfoById(EditId);
            txtName.Text = companyBO.Name;
            txtShortName.Text = companyBO.ShortName;
            txtCode.Text = companyBO.Code;
            txtDescription.Text = companyBO.Description;
            txtCompanyAddress.Text = companyBO.CompanyAddress;
            txtWebAddress.Text = companyBO.WebAddress;
            txtBinNumber.Text = companyBO.BinNumber;
            txtTinNumber.Text = companyBO.TinNumber;
            ddlBudgetType.SelectedValue = companyBO.BudgetType;
            txtCompanyId.Value = companyBO.CompanyId.ToString();
            ddlInterCompanyName.SelectedValue = companyBO.InterCompanyTransactionHeadId.ToString();
        }
    }
}