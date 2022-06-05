using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using System.Web.Services;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmUserInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        UserInformationBO userInformationBO = new UserInformationBO();
        Boolean isDynamicBestRegardsForConfirmationLetterEnable = false;
        //**************************** Handlers ****************************//
        private void LoadModuleInformation()
        {
            List<ModuleNameBO> allModuleInfoList = new List<ModuleNameBO>();
            ModuleNameDA moduleNameDA = new ModuleNameDA();
            allModuleInfoList = moduleNameDA.GetAllActiveModuleInfo().Where(x => x.IsReportType != true).ToList();
            this.gvAdminAuthorization.DataSource = allModuleInfoList;
            this.gvAdminAuthorization.DataBind();
        }

        private void CheckPermission()
        {
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                LoadModuleInformation();
                IsAdminUser();
                LoadUserGroup();
                txtUserName.Focus();
                IsDynamicBestRegardsForConfirmationLetter();
                LoadEmployeeInfo();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
            SetTab("SearchTab");
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            ClearSearch();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            if (txtUserPassword.Text.Equals(txtUserConfirmPassword.Text))
            {
                UserInformationBO currentUserInformationBO = new UserInformationBO();
                currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                UserInformationBO userInformationBO = new UserInformationBO();
                UserInformationDA userInformationDA = new UserInformationDA();

                userInformationBO.UserName = txtUserName.Text;
                userInformationBO.UserId = txtUserId.Text.Trim();
                userInformationBO.UserPassword = txtUserPassword.Text.Trim();
                userInformationBO.UserGroupId = Convert.ToInt32(ddlUserGroupId.SelectedValue);
                userInformationBO.UserEmail = txtUserEmail.Text;
                userInformationBO.UserPhone = txtUserPhone.Text;
                userInformationBO.UserDesignation = txtUserDesignation.Text;
                userInformationBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                userInformationBO.IsAdminUser = ddlIsAdminUser.SelectedIndex == 0 ? false : true;
                userInformationBO.EmpId = Convert.ToInt32(ddlEmployee.SelectedValue);

                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();

                int rowsAuthorizationList = gvAdminAuthorization.Rows.Count;
                for (int i = 0; i < rowsAuthorizationList; i++)
                {
                    CheckBox cb = (CheckBox)gvAdminAuthorization.Rows[i].FindControl("chkIsSavePermission");
                    if (cb.Checked == true)
                    {
                        SecurityUserAdminAuthorizationBO adminAuthorization = new SecurityUserAdminAuthorizationBO();
                        Label lbl = (Label)gvAdminAuthorization.Rows[i].FindControl("lblModuleId");

                        adminAuthorization.ModuleId = Convert.ToInt32(lbl.Text);
                        if (!string.IsNullOrEmpty(txtUserInfoId.Value))
                        {
                            adminAuthorization.UserInfoId = Int32.Parse(txtUserInfoId.Value);
                        }
                        else
                        {
                            adminAuthorization.UserInfoId = 0;
                        }
                        adminAuthorizationList.Add(adminAuthorization);
                    }
                }

                if (string.IsNullOrWhiteSpace(txtUserInfoId.Value))
                {
                    if (!CheckingMaximumUserPermission())
                    {
                        return;
                    }

                    if (DuplicateCheckDynamicaly("UserId", txtUserId.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "User Id Already Exist.", AlertType.Warning);
                        txtUserId.Focus();
                        return;
                    }
                    else if (userInformationBO.UserGroupId == 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "User Group.", AlertType.Warning);
                        txtUserId.Focus();
                        return;
                    }

                    int tmpUserInfoId = 0;
                    userInformationBO.CreatedBy = currentUserInformationBO.UserInfoId;
                    Boolean status = userInformationDA.SaveUserInformation(userInformationBO, adminAuthorizationList, out tmpUserInfoId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.UserInformation.ToString(), tmpUserInfoId,
                    ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserInformation));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        LoadGridView();
                        Cancel();
                    }
                }
                else
                {
                    if (DuplicateCheckDynamicaly("UserId", txtUserId.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "User Id Already Exist.", AlertType.Warning);
                        txtUserId.Focus();
                        return;
                    }
                    else if (userInformationBO.UserGroupId == 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "User Group.", AlertType.Warning);
                        txtUserId.Focus();
                        return;
                    }
                    userInformationBO.UserInfoId = Convert.ToInt32(txtUserInfoId.Value);
                    userInformationBO.LastModifiedBy = currentUserInformationBO.UserInfoId;
                    Boolean status = userInformationDA.UpdateUserInformation(userInformationBO, adminAuthorizationList);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.UserInformation.ToString(), userInformationBO.UserInfoId,
                    ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserInformation));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        LoadGridView();
                        Cancel();
                    }
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Password and Confirm Password is not same.", AlertType.Warning);
                txtUserPassword.Focus();
            }
        }
        protected void gvUserInformation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserInformation.PageIndex = e.NewPageIndex;
            LoadGridView();
        }
        protected void gvUserInformation_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvUserInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userInfoId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(userInfoId);
                btnSave.Text = "Update";
                SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                UserInformationDA userInformationDA = new UserInformationDA();

                Boolean status = userInformationDA.DeleteUserInformationById(userInfoId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.UserInformation.ToString(), userInfoId,
                    ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserInformation));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
                LoadGridView();
                SetTab("SearchTab");
            }
        }
        //*********************** User Defined Function *******************//
        private void IsAdminUser()
        {
            IsAdminUserLabelDiv.Visible = false;
            IsAdminUserDropDownDiv.Visible = false;
            AdminAuthorizationInformationDiv.Visible = false;
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.UserGroupId == 1)
            {
                IsAdminUserLabelDiv.Visible = true;
                IsAdminUserDropDownDiv.Visible = true;
                AdminAuthorizationInformationDiv.Visible = true;
            }
        }
        private void IsDynamicBestRegardsForConfirmationLetter()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isDynamicBestRegardsForConfirmationLetterBO = new HMCommonSetupBO();
            isDynamicBestRegardsForConfirmationLetterBO = commonSetupDA.GetCommonConfigurationInfo("IsDynamicBestRegardsForConfirmationLetter", "IsDynamicBestRegardsForConfirmationLetter");

            if (isDynamicBestRegardsForConfirmationLetterBO != null)
            {
                if (isDynamicBestRegardsForConfirmationLetterBO.SetupId > 0)
                {
                    if (Convert.ToInt32(isDynamicBestRegardsForConfirmationLetterBO.SetupValue) > 0)
                    {
                        isDynamicBestRegardsForConfirmationLetterEnable = true;
                        pnlDesignationInformation.Visible = true;
                    }
                }
            }
        }
        private void CheckObjectPermission()
        {
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmUserInformation.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(txtUserId.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "User Id.", AlertType.Warning);
                txtUserId.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtUserPassword.Text))
            {
                if (string.IsNullOrWhiteSpace(txtUserInfoId.Value))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Password.", AlertType.Warning);
                    txtUserPassword.Focus();
                    flag = false;
                }
            }
            else if (ddlUserGroupId.SelectedIndex == -1)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "User Group.", AlertType.Warning);
                ddlUserGroupId.Focus();
                flag = false;
            }
            return flag;
        }
        private void LoadUserGroup()
        {
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            UserGroupDA userGroupDA = new UserGroupDA();
            List<UserGroupBO> GetActiveUserGroupInfoList = userGroupDA.GetActiveUserGroupInfo(userInformationBO.UserGroupId);

            if (userInformationBO.UserInfoId == 1)
            {
                ddlUserGroupId.DataSource = GetActiveUserGroupInfoList;
            }
            else
            {
                ddlUserGroupId.DataSource = GetActiveUserGroupInfoList.Where(x => x.UserGroupId != 1).ToList();
            }

            ddlUserGroupId.DataTextField = "GroupName";
            ddlUserGroupId.DataValueField = "UserGroupId";
            ddlUserGroupId.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlUserGroupId.Items.Insert(0, FirstItem);

            if (userInformationBO.UserInfoId == 1)
            {
                ddlSUserGroup.DataSource = GetActiveUserGroupInfoList;
            }
            else
            {
                ddlSUserGroup.DataSource = GetActiveUserGroupInfoList.Where(x => x.UserGroupId != 1).ToList();
            }
            ddlSUserGroup.DataTextField = "GroupName";
            ddlSUserGroup.DataValueField = "UserGroupId";
            ddlSUserGroup.DataBind();

            ListItem srcFirstItem = new ListItem();
            srcFirstItem.Value = "0";
            srcFirstItem.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSUserGroup.Items.Insert(0, srcFirstItem);
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddlEmployee.DataSource = empList;
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployee.Items.Insert(0, FirstItem);
        }
        private void LoadGridView()
        {
            CheckObjectPermission();
            string UserName = txtSName.Text;
            string Email = txtSUserEMail.Text;
            string UserId = txtSUserId.Text;
            string PhoneNo = txtSUserPhone.Text;
            int GroupId = Int32.Parse(ddlSUserGroup.SelectedValue);
            bool ActiveState = ddlSActiveStat.SelectedIndex == 0 ? true : false;

            UserInformationDA userInformationDA = new UserInformationDA();
            List<UserInformationBO> files = userInformationDA.GetUserInformationBySearchCriteria(UserName, Email, UserId, PhoneNo, GroupId, ActiveState);

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (currentUserInformationBO.UserInfoId == 1)
            {
                gvUserInformation.DataSource = files.Where(x => x.SupplierId == 0).ToList(); ;
                gvUserInformation.DataBind();
            }
            else
            {
                gvUserInformation.DataSource = files.Where(x => x.UserInfoId != 1 && x.SupplierId == 0).ToList();
                gvUserInformation.DataBind();
            }
        }
        private void Cancel()
        {
            txtUserName.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtUserPassword.Text = string.Empty;
            txtUserPhone.Text = string.Empty;
            txtUserEmail.Text = string.Empty;
            txtUserDesignation.Text = string.Empty;
            ddlUserGroupId.SelectedIndex = 0;
            ddlActiveStat.SelectedIndex = 0;
            ddlIsAdminUser.SelectedIndex = 0;
            txtUserInfoId.Value = string.Empty;
            btnSave.Text = "Save";
            txtUserName.Focus();
            //pnlPasswordPanel.Visible = true;

            int rowsStockItem = gvAdminAuthorization.Rows.Count;
            for (int i = 0; i < rowsStockItem; i++)
            {
                CheckBox cb = (CheckBox)gvAdminAuthorization.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }
        }
        private void ClearSearch()
        {
            txtSName.Text = string.Empty;
            txtSUserEMail.Text = string.Empty; ;
            txtSUserId.Text = string.Empty; ;
            txtSUserPhone.Text = string.Empty;
            ddlSUserGroup.SelectedIndex = 0;
            ddlSActiveStat.SelectedIndex = 0;
        }
        private void FillForm(int userId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();
            userInformationBO = userInformationDA.GetUserInformationById(userId);
            ddlUserGroupId.SelectedValue = userInformationBO.UserGroupId.ToString();
            txtUserName.Text = userInformationBO.UserName;
            txtUserId.Text = userInformationBO.UserId;
            txtUserPassword.Text = string.Empty;
            txtUserEmail.Text = userInformationBO.UserEmail;
            txtUserPhone.Text = userInformationBO.UserPhone;
            txtUserDesignation.Text = userInformationBO.UserDesignation;
            ddlActiveStat.SelectedValue = (userInformationBO.ActiveStat == true ? 0 : 1).ToString();
            ddlIsAdminUser.SelectedValue = (userInformationBO.IsAdminUser == true ? 1 : 0).ToString();
            txtUserInfoId.Value = userInformationBO.UserInfoId.ToString();
            ddlEmployee.SelectedValue = userInformationBO.EmpId.ToString();

            List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
            adminAuthorizationList = userInformationDA.GetSecurityUserAdminAuthorizationByUserInfoId(userId);
            if (adminAuthorizationList != null)
            {
                if (adminAuthorizationList.Count > 0)
                {
                    int rowsStockItem = gvAdminAuthorization.Rows.Count;

                    List<UserGroupCostCenterMappingBO> listStockItem = new List<UserGroupCostCenterMappingBO>();
                    for (int i = 0; i < rowsStockItem; i++)
                    {
                        UserGroupCostCenterMappingBO costCenterStockItem = new UserGroupCostCenterMappingBO();
                        Label lbl = (Label)gvAdminAuthorization.Rows[i].FindControl("lblModuleId");
                        costCenterStockItem.CostCenterId = Int32.Parse(lbl.Text);
                        listStockItem.Add(costCenterStockItem);
                    }

                    for (int i = 0; i < listStockItem.Count; i++)
                    {
                        for (int j = 0; j < adminAuthorizationList.Count; j++)
                        {
                            if (listStockItem[i].CostCenterId == adminAuthorizationList[j].ModuleId)
                            {
                                CheckBox cb = (CheckBox)gvAdminAuthorization.Rows[i].FindControl("chkIsSavePermission");
                                cb.Checked = true;
                            }
                        }
                    }
                }
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
        private bool CheckingMaximumUserPermission()
        {
            bool status = true;
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            string encryptFieldType = encryptionHelper.Encrypt(hmUtility.GetExpireInformation("FieldType"));

            string decryptValue = string.Empty;
            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customFieldForCash = new CustomFieldBO();
            customFieldForCash = hmCommonDA.GetCustomFieldByFieldName(encryptFieldType);
            if (customFieldForCash != null)
            {
                if (customFieldForCash.FieldId > 0)
                {
                    try
                    {
                        decryptValue = encryptionHelper.Decrypt(customFieldForCash.FieldValue);

                        string[] separators = { "Innb0@rd" };
                        string[] decryptWordList = decryptValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        int maxUser = Convert.ToInt32(decryptWordList[0]);
                        int alreadydadedUser = 0;

                        UserInformationDA userEntityDA = new UserInformationDA();
                        List<UserInformationBO> userEntityBOList = new List<UserInformationBO>();
                        userEntityBOList = userEntityDA.GetAllUserInformation();

                        if (userEntityBOList != null)
                        {
                            alreadydadedUser = userEntityBOList.Count;

                            if (maxUser < alreadydadedUser)
                            {
                                CommonHelper.AlertInfo(innboardMessage, "The user information you entered exceeded your user limitation.", AlertType.Warning);
                                txtUserId.Focus();
                                status = false;
                            }
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "The user information you entered exceeded your user limitation.", AlertType.Warning);
                            txtUserId.Focus();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "The user information you entered exceeded your user limitation.", AlertType.Warning);
                        txtUserId.Focus();
                        status = false;
                        throw ex;
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "The user information you entered exceeded your user limitation.", AlertType.Warning);
                    txtUserId.Focus();
                    status = false;
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "The user information you entered exceeded your user limitation.", AlertType.Warning);
                txtUserId.Focus();
                status = false;
            }

            return status;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "SecurityUserInformation";
            string pkFieldName = "UserInfoId";
            string pkFieldValue = txtUserInfoId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}