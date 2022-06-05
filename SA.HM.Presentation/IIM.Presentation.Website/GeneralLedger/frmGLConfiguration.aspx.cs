using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using System.Collections;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmGLConfiguration : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlVoucherNumberPanel.Visible = false;
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadAccountsConfiguration();
                LoadAccountsSavedInformation();
                LoadCompany();
                LoadAccountHead();
                LoadDropDown(ddlAccMonthProject);
                LoadDropDown(ddlVProject);
                LoadDropDown(ddlProject);
                LoadConfiguration();
            }
        }
        protected void btnVoucherSave_Click(object sender, EventArgs e)
        {
            if (ddlVCompany.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Company.", AlertType.Warning);
                ddlVCompany.Focus();
                return;
            }

            int tmpSetupId = -1;
            GLCommonSetupBO setupBO = new GLCommonSetupBO();
            setupBO.ProjectId = Int32.Parse(ddlVProject.SelectedValue);
            setupBO.TypeName = "VoucherNumber";
            setupBO.SetupName = "VoucherNumberGenerationMethod";
            setupBO.SetupValue = ddlVoucherNumber.SelectedValue;
            if (!String.IsNullOrEmpty(txtVoucherSetupId.Value.ToString()))
            {
                setupBO.SetupId = Int32.Parse(txtVoucherSetupId.Value);
            }

            GLCommonSetupDA setupDA = new GLCommonSetupDA();
            Boolean status = setupDA.SaveOrUpdateGLCommonSetup(setupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Voucher Number Configuration Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.VoucherSetUp.ToString(), setupBO.SetupId,
                ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.VoucherSetUp));

            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Voucher Number Configuration saved Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.VoucherSetUp.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.VoucherSetUp));
                btnVoucherSave.Text = "Update";
            }

        }
        protected void btnMonthSave_Click(object sender, EventArgs e)
        {

            if (ddlAccMonthProject.SelectedIndex == -1)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Project Name.", AlertType.Warning);
                return;
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int tmpFiscalYearId = -1;
            GLFiscalYearBO fiscalYear = new GLFiscalYearBO();
            fiscalYear.ProjectId = Int32.Parse(ddlAccMonthProject.SelectedValue);
            fiscalYear.FiscalYearName = txtFiscalYearName.Text;
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                //fiscalYear.FromDate = Convert.ToDateTime(txtStartDate.Text);
                fiscalYear.FromDate = CommonHelper.DateTimeToMMDDYYYY(txtStartDate.Text);
                //fiscalYear.FromDate = hmUtility.GetDateTimeFromString(this.txtStartDate.Text, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                //fiscalYear.ToDate = Convert.ToDateTime(txtEndDate.Text);
                fiscalYear.ToDate = CommonHelper.DateTimeToMMDDYYYY(txtEndDate.Text);
                //fiscalYear.ToDate = hmUtility.GetDateTimeFromString(this.txtEndDate.Text, userInformationBO.ServerDateFormat);
            }
            if (!String.IsNullOrEmpty(txtMonthSetupId.Value.ToString()))
            {
                fiscalYear.FiscalYearId = Int32.Parse(txtMonthSetupId.Value);
            }

            string projectIdList = hfProjectIdList.Value;
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            Boolean status = fiscalYearDA.SaveOrUpdateGLFiscalYear(fiscalYear, projectIdList, out tmpFiscalYearId);
            if (tmpFiscalYearId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.LedgerMonthSetup.ToString(), fiscalYear.FiscalYearId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LedgerMonthSetup));
                ClearFiscalYearInfo();
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.LedgerMonthSetup.ToString(), tmpFiscalYearId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LedgerMonthSetup));
                btnMonthSave.Text = "Update";
                ClearFiscalYearInfo();
            }
        }
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedValue = Int32.Parse(ddlCompany.SelectedValue);
            ddlProject.DataSource = GetProjectList(SelectedValue);
            ddlProject.DataTextField = "Name";
            ddlProject.DataValueField = "ProjectId";
            ddlProject.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            //itemNodeId.Text = "---Select---";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlProject.Items.Insert(0, itemNodeId);
        }
        protected void ddlAccMonthCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

            int SelectedValue = Int32.Parse(ddlAccMonthCompany.SelectedValue);
            ddlAccMonthProject.DataSource = GetProjectList(SelectedValue);
            ddlAccMonthProject.DataTextField = "Name";
            ddlAccMonthProject.DataValueField = "ProjectId";
            ddlAccMonthProject.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            //itemNodeId.Text = "---Select---";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlAccMonthProject.Items.Insert(0, itemNodeId);
        }
        protected void ddlAccMonthProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            int projectID = Int32.Parse(ddlAccMonthProject.SelectedValue);
            string SetupType = "AccountFirstMonth";
            GLCommonSetupBO setupBO = GetCommonSetupInfoById(projectID, SetupType);
            if (setupBO.SetupId > 0)
            {
                //ddlAccMonth.SelectedValue = setupBO.SetupValue;
                txtMonthSetupId.Value = setupBO.SetupId.ToString();
                btnMonthSave.Text = "Update";
            }
            else
            {
                txtMonthSetupId.Value = "";
                //ddlAccMonth.SelectedIndex = 0;
                btnMonthSave.Text = "Save";
            }

        }
        protected void ddlVCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

            int SelectedValue = Int32.Parse(ddlVCompany.SelectedValue);
            ddlVProject.DataSource = GetProjectList(SelectedValue);
            ddlVProject.DataTextField = "Name";
            ddlVProject.DataValueField = "ProjectId";
            ddlVProject.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            //itemNodeId.Text = "---Select---";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlVProject.Items.Insert(0, itemNodeId);
        }
        protected void ddlVProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            int projectID = Int32.Parse(ddlVProject.SelectedValue);
            string SetupType = "VoucherNumberGenerationMethod";
            GLCommonSetupBO setupBO = GetCommonSetupInfoById(projectID, SetupType);
            if (setupBO.SetupId > 0)
            {

                ddlVoucherNumber.SelectedValue = setupBO.SetupValue;
                txtVoucherSetupId.Value = setupBO.SetupId.ToString();
                btnVoucherSave.Text = "Update";
            }
            else
            {
                txtVoucherSetupId.Value = "";
                ddlVoucherNumber.SelectedIndex = 0;
                btnVoucherSave.Text = "Save";
            }
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            int projectId = Int32.Parse(ddlProject.SelectedValue);
            // LoadGridView(projectId);
            SetTab("AccountMapping");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                SetTab("AccountMapping");
                return;
            }
            if (IsFrmServerValid())
            {
                CommonHelper.AlertInfo(innboardMessage, "Duplicate Strongly Prohabited.", AlertType.Warning);
                SetTab("AccountMapping");
                return;
            }

            int tmpConfigurationId;
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            GLAccountConfigurationBO configurationBO = new GLAccountConfigurationBO();
            GLAccountConfigurationDA configurationDA = new GLAccountConfigurationDA();
            configurationBO.ProjectId = Int32.Parse(ddlProject.SelectedValue);
            configurationBO.AccountType = ddlAccountType.SelectedValue;
            configurationBO.NodeId = Convert.ToInt32(ddlHeadId.SelectedValue);


            configurationBO.CreatedBy = userInformationBO.UserInfoId;
            Boolean status = configurationDA.SaveAccountConfigurationInfo(configurationBO, out tmpConfigurationId);
            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.LedgerAccountConfiguration.ToString(), tmpConfigurationId,
                ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LedgerAccountConfiguration));
                Cancel();
            }
            SetTab("AccountMapping");
        }
        protected void btnGLApproval_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBOCheckedBy = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOCheckedBy.SetupId = 13;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBOCheckedBy.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBOCheckedBy.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBOCheckedBy.SetupName = "CheckedBySetup";
            commonSetupBOCheckedBy.TypeName = "VoucherApproveSystem";
            commonSetupBOCheckedBy.SetupValue = this.ddlAccountsCheckedBy.SelectedValue;
            commonSetupBOCheckedBy.Description = string.Empty;
            Boolean statusCheckedBy = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBOCheckedBy, out tmpSetupId);

            HMCommonSetupBO commonSetupBOApprovedBy = new HMCommonSetupBO();
            commonSetupBOApprovedBy.SetupId = 14;
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBOApprovedBy.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBOApprovedBy.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBOApprovedBy.SetupName = "ApprovedBySetup";
            commonSetupBOApprovedBy.TypeName = "VoucherApproveSystem";
            commonSetupBOApprovedBy.SetupValue = this.ddlAccountsApprovedBy.SelectedValue;
            commonSetupBOApprovedBy.Description = string.Empty;
            Boolean statusApprovedBy = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBOApprovedBy, out tmpSetupId);

            if (statusCheckedBy && statusApprovedBy)
            {
                CommonHelper.AlertInfo(innboardMessage, "Accounts Approval Configuration Setup Successfully.", AlertType.Success);
            }
        }
        //************************ User Defined Function ********************//
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadAccountsConfiguration()
        {
            UserInformationDA entityDA = new UserInformationDA();
            List<UserInformationBO> GetUserInformationBOList = entityDA.GetUserInformation();
            this.ddlAccountsCheckedBy.DataSource = GetUserInformationBOList;
            this.ddlAccountsCheckedBy.DataTextField = "UserName";
            this.ddlAccountsCheckedBy.DataValueField = "UserInfoId";
            this.ddlAccountsCheckedBy.DataBind();

            this.ddlAccountsApprovedBy.DataSource = GetUserInformationBOList;
            this.ddlAccountsApprovedBy.DataTextField = "UserName";
            this.ddlAccountsApprovedBy.DataValueField = "UserInfoId";
            this.ddlAccountsApprovedBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlAccountsCheckedBy.Items.Insert(0, item);
            this.ddlAccountsApprovedBy.Items.Insert(0, item);
        }
        private void LoadAccountsSavedInformation()
        {
            ddlAccountsCheckedBy.SelectedValue = "0";
            HMCommonSetupBO commonSetupBORequisition = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBORequisition = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "CheckedBySetup");

            if (commonSetupBORequisition != null)
            {
                if (commonSetupBORequisition.SetupId > 0)
                {
                    ddlAccountsCheckedBy.SelectedValue = commonSetupBORequisition.SetupValue.ToString();
                }
            }

            ddlAccountsApprovedBy.SelectedValue = "0";
            HMCommonSetupBO commonSetupBOPO = new HMCommonSetupBO();
            commonSetupBOPO = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "ApprovedBySetup");

            if (commonSetupBOPO != null)
            {
                if (commonSetupBOPO.SetupId > 0)
                {
                    ddlAccountsApprovedBy.SelectedValue = commonSetupBOPO.SetupValue.ToString();
                }
            }
        }
        private void LoadDropDown(DropDownList dropdown)
        {
            List<GLProjectBO> List = new List<GLProjectBO>();
            GLProjectDA projectDA = new GLProjectDA();
            List = projectDA.GetAllGLProjectInfo();
            dropdown.DataSource = List;
            dropdown.DataTextField = "Name";
            dropdown.DataValueField = "ProjectId";
            dropdown.DataBind();
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (ddlCompany.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Company.", AlertType.Warning);
                flag = false;
                ddlCompany.Focus();
                SetTab("AccountMapping");
            }


            return flag;
        }
        private Boolean IsFrmServerValid()
        {
            GLAccountConfigurationDA confDA = new GLAccountConfigurationDA();
            int count = confDA.GetAccountConfigurationInfoByProjectAndAccountType(Int32.Parse(ddlProject.SelectedValue), ddlAccountType.SelectedValue, Int32.Parse(ddlHeadId.SelectedValue));
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void LoadAccountHead()
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            ddlHeadId.DataSource = nodeMatrixDA.GetNodeMatrixInfo();
            ddlHeadId.DataTextField = "NodeHead";
            ddlHeadId.DataValueField = "NodeId";
            ddlHeadId.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            //itemNodeId.Text = "---Select---";
            ddlHeadId.Items.Insert(0, itemNodeId);
        }
        private void Cancel()
        {
            // throw new NotImplementedException();
            ddlAccountType.SelectedIndex = 0;
            ddlCompany.SelectedIndex = 0;
            btnMonthSave.Text = "Save";
            txtAccountHead.Text = "";
            txtConfigurationId.Value = string.Empty;
            txtSaveValidation.Value = string.Empty;

        }
        private void ClearFiscalYearInfo()
        {
            txtMonthSetupId.Value = "0";
            ddlAccMonthCompany.SelectedIndex = 0;
            txtFiscalYearName.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGLAccountConfiguration.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> List = new List<GLCompanyBO>();
            List = companyDA.GetAllGLCompanyInfo();
            ddlCompany.DataSource = List;
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ddlVCompany.DataSource = List;
            ddlVCompany.DataTextField = "Name";
            ddlVCompany.DataValueField = "CompanyId";
            ddlVCompany.DataBind();

            ddlAccMonthCompany.DataSource = List;
            ddlAccMonthCompany.DataTextField = "Name";
            ddlAccMonthCompany.DataValueField = "CompanyId";
            ddlAccMonthCompany.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            //itemNodeId.Text = "---Select---";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, itemNodeId);
            ddlVCompany.Items.Insert(0, itemNodeId);
            ddlAccMonthCompany.Items.Insert(0, itemNodeId);
        }
        private List<GLProjectBO> GetProjectList(int SelectedValue)
        {
            GLProjectDA projectDA = new GLProjectDA();
            return projectDA.GetProjectByCompanyId(SelectedValue);
        }
        private GLCommonSetupBO GetCommonSetupInfoById(int projectId, string SetupName)
        {
            GLCommonSetupDA setupDA = new GLCommonSetupDA();
            return setupDA.GetCommonSetupInfoById(projectId, SetupName);
        }
        private void SetTab(string TabName)
        {
            //if (TabName == "AccountMapping")
            //{
            //    //B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            //    A.Attributes.Add("class", "ui-state-default ui-corner-top");
            //}
            //else if (TabName == "Setting")
            //{
            //    A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            //    //B.Attributes.Add("class", "ui-state-default ui-corner-top");
            //}
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string DeleteData(int sEmpId)
        {
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("GLAccountConfiguration", "ConfigurationId", sEmpId);
                if (status)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
        [WebMethod]
        public static GLCommonSetupBO PopulateMonth(int projectId)
        {
            string SetupType = "AccountFirstMonth";

            GLCommonSetupDA setupDA = new GLCommonSetupDA();
            GLCommonSetupBO setupBO = setupDA.GetCommonSetupInfoById(projectId, SetupType);
            return setupBO;
        }
        [WebMethod]
        public static GLFiscalYearBO PopulateGLFiscalYearInfo(int projectId)
        {
            //string SetupType = "AccountFirstMonth";
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            GLFiscalYearBO fiscalYearBO = fiscalYearDA.GetFiscalYearInfoByProjectId(projectId);
            return fiscalYearBO;
        }
        [WebMethod]
        public static List<GLFiscalYearBO> GetAllGLFiscalYearInfo()
        {
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            List<GLFiscalYearBO> fiscalYearBO = new List<GLFiscalYearBO>();
            fiscalYearBO = fiscalYearDA.GetAllFiscalYear();
            return fiscalYearBO;
        }
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectInfoByGLCompany(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();
            projectList = entityDA.GetGLProjectInfoByGLCompany(companyId);

            return projectList;
        }
        [WebMethod]
        public static GLFiscalYearViewBO GetFiscalYearInfoByFiscalYearId(int fiscalYearId)
        {
            GLFiscalYearViewBO GLFiscalYear = new GLFiscalYearViewBO();
            GLProjectDA entityDA = new GLProjectDA();
            GLFiscalYear = entityDA.GetFiscalYearViewInfoByFiscalYearId(fiscalYearId);

            return GLFiscalYear;
        }
        [WebMethod]
        public static string CheckingFiscalYaerExist(int projectId)
        {
            string strTable = "0";

            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            GLFiscalYearBO fiscalYearBO = fiscalYearDA.GetFiscalYearInfoByProjectId(projectId);
            if (fiscalYearBO != null)
            {
                if (fiscalYearBO.FiscalYearId > 0)
                {
                    strTable = "1";
                }
            }

            return strTable;
        }
        [WebMethod]
        public static string GenerateGridForMapping(int projectId)
        {
            GLAccountConfigurationDA configDA = new GLAccountConfigurationDA();
            List<GLAccountConfigurationBO> List = new List<GLAccountConfigurationBO>();
            List = configDA.GetAccountConfigurationByProjectID(projectId);

            string strTable = "";
            strTable += "<table id='TableWiseItemInformation' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Node Head</th><th align='left' scope='col'>Project Name</th><th align='left' scope='col'>Account Type</th><th align='center' scope='col'>Action</th></tr>";
            int counter = 0;
            foreach (GLAccountConfigurationBO dr in List)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.NodeHead + "</td>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.NodeHead + "</td>";
                }

                strTable += "<td align='left' style='width: 15%;'>" + dr.ProjectName + "</td>";
                strTable += "<td align='left' style='width: 15%;'>" + dr.AccountTypeName + "</td>";
                strTable += "<td align='center' style='width: 15%;'>";
                if (dr.ConfigurationId > 0)
                {
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformDeleteAction(" + dr.ConfigurationId + ")' alt='Delete Information' border='0' />";
                }

                strTable += "</td></tr>";



            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;

        }
        [WebMethod]
        public static GLCommonSetupBO PopulateVoucher(int projectId)
        {
            string SetupType = "VoucherNumberGenerationMethod";
            GLCommonSetupDA setupDA = new GLCommonSetupDA();
            GLCommonSetupBO setupBO = setupDA.GetCommonSetupInfoById(projectId, SetupType);
            return setupBO;

        }
        [WebMethod]
        public static List<string> GetAutoCompleteData(string searchText)
        {
            List<string> nodeMatrixBOList = new List<string>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAccountHead1(searchText, 0);
            return nodeMatrixBOList;
        }
        [WebMethod]
        public static string FillForm(string searchText)
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            string nodeMatrixBO = nodeMatrixDA.GetNodeMatrixInfoByAccountHead2(searchText);
            return nodeMatrixBO;
        }
        [WebMethod]
        public static GLAccountConfigurationBO GetSaveValidation(string projectId, string accountType)
        {
            GLAccountConfigurationBO entityBO = new GLAccountConfigurationBO();
            GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
            entityBO = entityDA.GetAccountConfigurationInfoByProjectIdNAccountType(Convert.ToInt32(projectId), accountType);

            return entityBO;
        }
        private void UpdateConfiguration(Control control, int UserInfoId)
        {
            int tmpSetupId = 0;
            string hiddenField = "hf" + control.ID;

            Boolean status = false;
            Control hiddenControl = Div2.FindControl(hiddenField);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            if (control != null && hiddenControl != null)
            {
                commonSetupBO.SetupId = 0;
                if (control is TextBox)
                {
                    commonSetupBO.SetupValue = ((TextBox)control).Text;
                }
                else if (control is CheckBox)
                {
                    if (((CheckBox)control).Checked)
                        commonSetupBO.SetupValue = "1";
                    else
                        commonSetupBO.SetupValue = "0";
                }
                else if (control is DropDownList)
                {
                    commonSetupBO.SetupValue = ((DropDownList)control).SelectedValue;
                }
                if (!string.IsNullOrEmpty(((HiddenField)hiddenControl).Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(((HiddenField)hiddenControl).Value);
                }

                commonSetupBO.CreatedBy = UserInfoId;
                commonSetupBO.LastModifiedBy = UserInfoId;
                commonSetupBO.SetupName = control.ID;
                commonSetupBO.TypeName = control.ID;

                try
                {
                    status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                    if (status)
                    {
                        if (commonSetupBO.SetupId > 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.GeneralLedgerVoucher.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GeneralLedgerVoucher));
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.GeneralLedgerVoucher.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GeneralLedgerVoucher));
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void LoadConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsShowReferenceVoucherNumber", "IsShowReferenceVoucherNumber");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsShowReferenceVoucherNumber.Value = setUpBO.SetupId.ToString();
                IsShowReferenceVoucherNumber.Checked = setUpBO.SetupValue == "1";
            }
            btnSaveGLConfig.Text = "Update";
        }
        protected void btnSaveGLConfig_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (Control item in Div2.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            LoadConfiguration();
        }

    }
}