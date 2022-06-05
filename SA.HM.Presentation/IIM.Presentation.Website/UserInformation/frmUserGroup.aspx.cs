using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using HotelManagement.Data.Security;
using HotelManagement.Entity.Security;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmUserGroup : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        UserInformationBO userInformationBO = new UserInformationBO();
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
                this.LoadDefaultModule();
                this.LoadDefaultHomePage(0);
                this.LoadCommonDropDownHiddenField();
                this.LoadCostCenterInfoGridView();
                this.LoadProjectInfoGridView();
                this.LoadUserGroupType();
                this.CheckPermission();
            }
        }
        private void CheckPermission()
        {
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void gvUserGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvUserGroup.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvUserGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = false;
            }
        }
        protected void gvUserGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userGroupId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                UserGroupBO userGroupBO = new UserGroupBO();
                UserGroupDA userGroupDA = new UserGroupDA();
                userGroupBO = userGroupDA.GetUserGroupInfoById(userGroupId);
                txtGroupName.Text = userGroupBO.GroupName;
                ddlActiveStat.SelectedValue = (userGroupBO.ActiveStat == true ? 0 : 1).ToString();
                this.LoadDefaultModule();
                ddlDefaultModule.SelectedValue = userGroupBO.DefaultModuleId.ToString();
                this.LoadDefaultHomePage(userGroupBO.DefaultModuleId);
                ddlDefaultHomePage.SelectedValue = userGroupBO.DefaultHomePageId.ToString();
                txtUserGroupId.Value = userGroupBO.UserGroupId.ToString();
                txtEmail.Text = userGroupBO.Email;
                ddlUserGroupType.SelectedValue = userGroupBO.UserGroupType;

                LoadUserGroupCostCenterMappingInfo(userGroupId);
                LoadUserGroupProjectMappingInfo(userGroupId);

                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                string result = string.Empty;
                UserGroupBO userGroupBO = new UserGroupBO();
                UserGroupDA userGroupDA = new UserGroupDA();
                Boolean status = userGroupDA.DeleteUserGroupInfoById(userGroupId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.UserGroup.ToString(), userGroupId,
                    ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserGroup));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtGroupName.Text))
            {
                this.txtGroupName.Focus();
            }
            else
            {
                UserGroupBO userGroupBO = new UserGroupBO();
                UserGroupDA userGroupDA = new UserGroupDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                userGroupBO.GroupName = this.txtGroupName.Text;
                userGroupBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                userGroupBO.DefaultModuleId = Convert.ToInt32(ddlDefaultModule.SelectedValue);
                userGroupBO.DefaultHomePageId = Convert.ToInt32(ddlDefaultHomePage.SelectedValue);
                userGroupBO.Email = txtEmail.Text;
                userGroupBO.UserGroupType = ddlUserGroupType.SelectedValue;
                List<UserGroupCostCenterMappingBO> costCenterList = new List<UserGroupCostCenterMappingBO>();

                int rowsKitchenItem = gvUserGroupCostCenterInfo.Rows.Count;
                for (int i = 0; i < rowsKitchenItem; i++)
                {
                    CheckBox cb = (CheckBox)gvUserGroupCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                    if (cb.Checked == true)
                    {
                        UserGroupCostCenterMappingBO costCenter = new UserGroupCostCenterMappingBO();
                        Label lbl = (Label)gvUserGroupCostCenterInfo.Rows[i].FindControl("lblCostCentreId");

                        costCenter.CostCenterId = Convert.ToInt32(lbl.Text);
                        if (!string.IsNullOrEmpty(txtUserGroupId.Value))
                        {
                            costCenter.UserGroupId = Int32.Parse(txtUserGroupId.Value);
                        }
                        else
                        {
                            costCenter.UserGroupId = 0;
                        }
                        costCenterList.Add(costCenter);
                    }
                }

                List<UserGroupCostCenterMappingBO> projectList = new List<UserGroupCostCenterMappingBO>();
                int rowProjects = gvProject.Rows.Count;
                for (int i = 0; i < rowProjects; i++)
                {
                    CheckBox cb = (CheckBox)gvProject.Rows[i].FindControl("chkIsSavePermission");
                    if (cb.Checked == true)
                    {
                        UserGroupCostCenterMappingBO costCenter = new UserGroupCostCenterMappingBO();
                        Label lbl = (Label)gvProject.Rows[i].FindControl("lblProjectId");

                        costCenter.ProjectId = Convert.ToInt32(lbl.Text);
                        if (!string.IsNullOrEmpty(txtUserGroupId.Value))
                        {
                            costCenter.UserGroupId = Int32.Parse(txtUserGroupId.Value);
                        }
                        else
                        {
                            costCenter.UserGroupId = 0;
                        }
                        projectList.Add(costCenter);
                    }
                }

                if (string.IsNullOrWhiteSpace(txtUserGroupId.Value))
                {
                    if (DuplicateCheckDynamicaly("GroupName", this.txtGroupName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Group Name already exist.", AlertType.Warning);
                        this.txtGroupName.Focus();
                        return;
                    }

                    int tmpUserGroupId = 0;
                    userGroupBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = userGroupDA.SaveUserGroupInfo(userGroupBO, costCenterList, projectList, out tmpUserGroupId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.UserGroup.ToString(), tmpUserGroupId,
                    ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserGroup));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.LoadGridView();
                        this.Cancel();
                    }
                }
                else
                {
                    if (DuplicateCheckDynamicaly("GroupName", this.txtGroupName.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Group Name already exist.", AlertType.Warning);
                        this.txtGroupName.Focus();
                        return;
                    }
                    userGroupBO.UserGroupId = Convert.ToInt32(txtUserGroupId.Value);
                    userGroupBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = userGroupDA.UpdateUserGroupInfo(userGroupBO, costCenterList, projectList);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.UserGroup.ToString(), userGroupBO.UserGroupId,
                    ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserGroup));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.LoadGridView();
                        SetTab("EntryTab");
                        this.Cancel();
                    }
                }
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmUserGroup.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadCostCenterInfoGridView()
        {
            this.CheckObjectPermission();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();

            this.gvUserGroupCostCenterInfo.DataSource = files;
            this.gvUserGroupCostCenterInfo.DataBind();
        }
        private void LoadProjectInfoGridView()
        {
            this.CheckObjectPermission();
            GLProjectDA costCentreTabDA = new GLProjectDA();
            List<GLProjectBO> files = costCentreTabDA.GetAllGLProjectInfo();
            this.gvProject.DataSource = files;
            this.gvProject.DataBind();
        }
        private void LoadGridView()
        {
            string GroupName = txtSGroupName.Text;
            int Status = Int32.Parse(ddlSStatus.SelectedValue);
            if (Status == 0) { Status = 1; } else { Status = 0; }

            this.CheckObjectPermission();
            UserGroupDA da = new UserGroupDA();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<UserGroupBO> files = new List<UserGroupBO>();

            if (hmUtility.GetCurrentApplicationUserInfo().UserInfoId == 1)
            {
                files = da.GetUserGroupInfoBySearchCriteria(userInformationBO.UserGroupId, GroupName, Status);
            }
            else if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser == true)
            {
                files = da.GetUserGroupInfoBySearchCriteria(2, GroupName, Status).Where(x => x.UserGroupId != 1).ToList();
            }
            else
            {
                files = da.GetUserGroupInfoBySearchCriteria(userInformationBO.UserGroupId, GroupName, Status).Where(x => x.UserGroupId != 1).ToList();
            }

            this.gvUserGroup.DataSource = files;
            this.gvUserGroup.DataBind();
            SetTab("SearchTab");
        }
        private void Cancel()
        {
            txtGroupName.Text = string.Empty;
            ddlActiveStat.SelectedIndex = 0;
            btnSave.Text = "Save";
            txtUserGroupId.Value = string.Empty;
            ddlDefaultModule.SelectedValue = "0";
            ddlDefaultHomePage.SelectedValue = "0";
            txtEmail.Text= string.Empty;
            txtGroupName.Focus();

            int rowsStockItem = gvUserGroupCostCenterInfo.Rows.Count;
            for (int i = 0; i < rowsStockItem; i++)
            {
                CheckBox cb = (CheckBox)gvUserGroupCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }

            int rowsProject = gvProject.Rows.Count;
            for (int i = 0; i < rowsProject; i++)
            {
                CheckBox cb = (CheckBox)gvProject.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }
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
        private void LoadDefaultModule()
        {
            MenuDA menuDa = new MenuDA();
            List<CommonModuleNameBO> menuGroup = new List<CommonModuleNameBO>();
            List<CommonModuleNameBO> permittedMenuGroup = new List<CommonModuleNameBO>();
            menuGroup = menuDa.GetCommonMenuModule();

            string permissionList = Session["SoftwareModulePermissionList"].ToString();
            permissionList = "0," + permissionList;
            permittedMenuGroup = menuGroup.Where(x => permissionList.Contains(x.TypeId.ToString())).ToList();

            this.ddlDefaultModule.DataSource = permittedMenuGroup;
            this.ddlDefaultModule.DataTextField = "ModuleName";
            this.ddlDefaultModule.DataValueField = "ModuleId";
            this.ddlDefaultModule.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDefaultModule.Items.Insert(0, FirstItem);
        }
        private void LoadDefaultHomePage(int moduleId)
        {
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ModuleNameDA entityDA = new ModuleNameDA();
            this.ddlDefaultHomePage.DataSource = entityDA.GetSecurityMenuLinksInfo(userInformationBO.UserGroupId, moduleId);
            this.ddlDefaultHomePage.DataTextField = "PageName";
            this.ddlDefaultHomePage.DataValueField = "MenuLinksId";
            this.ddlDefaultHomePage.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDefaultHomePage.Items.Insert(0, FirstItem);
        }
        private void LoadUserGroupCostCenterMappingInfo(int EditId)
        {
            List<UserGroupCostCenterMappingBO> costListStockItem = new List<UserGroupCostCenterMappingBO>();
            UserGroupCostCenterMappingDA costStockItemDA = new UserGroupCostCenterMappingDA();
            costListStockItem = costStockItemDA.GetUserGroupCostCenterMappingByUserGroupId(EditId);
            int rowsStockItem = gvUserGroupCostCenterInfo.Rows.Count;

            List<UserGroupCostCenterMappingBO> listStockItem = new List<UserGroupCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                UserGroupCostCenterMappingBO costCenterStockItem = new UserGroupCostCenterMappingBO();
                Label lbl = (Label)gvUserGroupCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                costCenterStockItem.CostCenterId = Int32.Parse(lbl.Text);
                listStockItem.Add(costCenterStockItem);
            }


            for (int i = 0; i < listStockItem.Count; i++)
            {
                for (int j = 0; j < costListStockItem.Count; j++)
                {
                    if (listStockItem[i].CostCenterId == costListStockItem[j].CostCenterId)
                    {
                        CheckBox cb = (CheckBox)gvUserGroupCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        cb.Checked = true;
                    }
                }
            }

        }
        private void LoadUserGroupProjectMappingInfo(int EditId)
        {
            List<UserGroupCostCenterMappingBO> costListStockItem = new List<UserGroupCostCenterMappingBO>();
            UserGroupCostCenterMappingDA costStockItemDA = new UserGroupCostCenterMappingDA();
            costListStockItem = costStockItemDA.GetUserGroupProjectMappingByUserGroupId(EditId);
            int rowsStockItem = gvProject.Rows.Count;

            List<UserGroupCostCenterMappingBO> listStockItem = new List<UserGroupCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                UserGroupCostCenterMappingBO costCenterStockItem = new UserGroupCostCenterMappingBO();
                Label lbl = (Label)gvProject.Rows[i].FindControl("lblProjectId");
                costCenterStockItem.ProjectId = Int32.Parse(lbl.Text);
                listStockItem.Add(costCenterStockItem);
            }

            for (int i = 0; i < listStockItem.Count; i++)
            {
                for (int j = 0; j < costListStockItem.Count; j++)
                {
                    if (listStockItem[i].ProjectId == costListStockItem[j].ProjectId)
                    {
                        CheckBox cb = (CheckBox)gvProject.Rows[i].FindControl("chkIsSavePermission");
                        cb.Checked = true;
                    }
                }
            }
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "SecurityUserGroup";
            string pkFieldName = "UserGroupId";
            string pkFieldValue = this.txtUserGroupId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        private void LoadUserGroupType()
        {
            CheckObjectPermission();
            HMCommonDA commonDA = new HMCommonDA();
            Array itemValues = Enum.GetValues(typeof(ConstantHelper.UserGroupType));

            List<ListItem> fields = new List<ListItem>();

            foreach (var field in itemValues)
            {
                ListItem element = new ListItem(field.ToString(), field.ToString());
                fields.Add(element);
            }
              
            ddlUserGroupType.DataSource = fields;
            ddlUserGroupType.DataTextField = "Text";
            ddlUserGroupType.DataValueField = "Value";
            ddlUserGroupType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlUserGroupType.Items.Insert(0, item);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                UserGroupBO userGroupBO = new UserGroupBO();
                UserGroupDA userGroupDA = new UserGroupDA();

                Boolean status = userGroupDA.DeleteUserGroupInfoById(sEmpId);
                if (status)
                {
                    //result = "success";
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return rtninf;
        }
        [WebMethod]
        public static UserGroupBO FillForm(int EditId)
        {
            UserGroupBO userGroupBO = new UserGroupBO();
            UserGroupDA userGroupDA = new UserGroupDA();

            userGroupBO = userGroupDA.GetUserGroupInfoById(EditId);
            return userGroupBO;
        }
        [WebMethod]
        public static ArrayList PopulateHomePage(int moduleId)
        {
            ArrayList list = new ArrayList();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<ModuleNameBO> projectList = new List<ModuleNameBO>();
            ModuleNameDA entityDA = new ModuleNameDA();
            projectList = entityDA.GetSecurityMenuLinksInfo(userInformationBO.UserGroupId, Convert.ToInt32(moduleId));
            int count = projectList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        projectList[i].PageName.ToString(),
                                        projectList[i].MenuLinksId.ToString()
                                         ));
            }
            return list;

        }
    }
}