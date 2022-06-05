using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmObjectPermission : BasePage
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        UserInformationBO userInformationBO = new UserInformationBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadModuleInfo();
                this.LoadUserGroup();
                if (this.ddlUserGroupId.SelectedIndex != -1)
                {
                    this.LoadGridView();
                }
                if (Session["UserGroupHeadSelectedValue"] != null)
                {
                    this.ddlUserGroupId.SelectedValue = Session["UserGroupHeadSelectedValue"].ToString();
                }
                if (Session["GroupHeadSelectedValue"] != null)
                {
                    this.ddlGroupHead.SelectedValue = Session["GroupHeadSelectedValue"].ToString();
                    this.LoadGridView();
                }

                if (Session["SaveUpdateMessage"] != null)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Session.Remove("SaveUpdateMessage");
                }
                CheckPermission();
            }

        }
        private void CheckPermission()
        {
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadModuleInfo()
        {
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ModuleNameDA entityDA = new ModuleNameDA();
            this.ddlGroupHead.DataSource = entityDA.GetModuleInfo(userInformationBO.UserGroupId);
            this.ddlGroupHead.DataTextField = "ModuleName";
            this.ddlGroupHead.DataValueField = "GroupName";
            this.ddlGroupHead.DataBind();
        }
        private void LoadUserGroup()
        {
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            UserGroupDA userGroupDA = new UserGroupDA();
            this.ddlUserGroupId.DataSource = userGroupDA.GetActiveUserGroupInfo(userInformationBO.UserGroupId);
            this.ddlUserGroupId.DataTextField = "GroupName";
            this.ddlUserGroupId.DataValueField = "UserGroupId";
            this.ddlUserGroupId.DataBind();
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            if (this.ddlUserGroupId.SelectedIndex != -1)
            {
                if (this.ddlUserGroupId.SelectedIndex != -1)
                {
                    ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
                    List<ObjectPermissionBO> files = objectPermissionDA.GetObjectPermission(Convert.ToInt32(this.ddlUserGroupId.SelectedValue), this.ddlGroupHead.SelectedValue);

                    this.gvObjectPermission.DataSource = files;
                    this.gvObjectPermission.DataBind();
                }
            }
            else
            {
                this.gvObjectPermission.DataSource = null;
                this.gvObjectPermission.DataBind();
            }
        }
        private void CheckObjectPermission()
        {

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmObjectPermission.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSaveAll.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        protected void gvObjectPermission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvObjectPermission.PageIndex = e.NewPageIndex;
            if (this.ddlUserGroupId.SelectedIndex != -1)
            {
                this.LoadGridView();
            }
        }
        protected void gvObjectPermission_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblIsSaveValue = (Label)e.Row.FindControl("lblchkIsSavePermission");
                if (lblIsSaveValue.Text == "False")
                {
                    ((CheckBox)e.Row.FindControl("chkIsSavePermission")).Checked = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkIsSavePermission")).Checked = true;
                }

                Label lblIsDeleteValue = (Label)e.Row.FindControl("lblchkIsDeletePermission");
                if (lblIsDeleteValue.Text == "False")
                {
                    ((CheckBox)e.Row.FindControl("chkIsDeletePermission")).Checked = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkIsDeletePermission")).Checked = true;
                }

                Label lblIsViewValue = (Label)e.Row.FindControl("lblchkIsViewPermission");
                if (lblIsViewValue.Text == "False")
                {
                    ((CheckBox)e.Row.FindControl("chkIsViewPermission")).Checked = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkIsViewPermission")).Checked = true;
                }

                Label lblgvObjectTypeValue = (Label)e.Row.FindControl("lblgvObjectType");
                if (lblgvObjectTypeValue.Text == "Report" || lblgvObjectTypeValue.Text == "Search")
                {
                    ((CheckBox)e.Row.FindControl("chkIsSavePermission")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkIsDeletePermission")).Visible = false;
                }

            }
        }
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (this.ddlUserGroupId.SelectedIndex != -1)
            {
                int counter = 0;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                foreach (GridViewRow row in gvObjectPermission.Rows)
                {
                    counter = counter + 1;
                    bool isSave = ((CheckBox)row.FindControl("chkIsSavePermission")).Checked;
                    bool isDelete = ((CheckBox)row.FindControl("chkIsDeletePermission")).Checked;
                    bool isView = ((CheckBox)row.FindControl("chkIsViewPermission")).Checked;
                    if (isSave || isDelete)
                    {
                        isView = true;
                    }

                    //if (isSave || isDelete || isView)
                    //{
                    ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
                    ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
                    objectPermissionBO.IsDeletePermission = isDelete;
                    objectPermissionBO.IsSavePermission = isSave;
                    objectPermissionBO.IsViewPermission = isView;
                    Label lblObjectTabIdValue = (Label)row.FindControl("lblObjectTabId");
                    objectPermissionBO.ObjectTabId = Convert.ToInt32(lblObjectTabIdValue.Text);
                    objectPermissionBO.UserGroupId = Convert.ToInt32(this.ddlUserGroupId.SelectedValue);

                    int tmpObjectPermissionId;

                    Label lblObjectPermissionIdValue = (Label)row.FindControl("lblObjectPermissionId");
                    if (lblObjectPermissionIdValue == null || lblObjectPermissionIdValue.Text == "0")
                    {
                        objectPermissionBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean statusSave = objectPermissionDA.SaveObjectPermissionInfo(objectPermissionBO, out tmpObjectPermissionId);
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        if (statusSave && (isSave || isDelete || isView))
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.UserPermission.ToString(), tmpObjectPermissionId,
                            ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserPermission));
                        }
                    }
                    else
                    {
                        objectPermissionBO.LastModifiedBy = userInformationBO.UserInfoId;
                        objectPermissionBO.ObjectPermissionId = Convert.ToInt32(lblObjectPermissionIdValue.Text);
                        Boolean statusUpdate = objectPermissionDA.UpdateObjectPermissionInfo(objectPermissionBO);
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        if (statusUpdate && (isSave || isDelete || isView))
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.UserPermission.ToString(), objectPermissionBO.ObjectPermissionId,
                            ProjectModuleEnum.ProjectModule.UserManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.UserPermission));
                        }
                    }

                    //}
                }

                if (gvObjectPermission.Rows.Count == counter)
                {
                    Session["UserGroupHeadSelectedValue"] = this.ddlUserGroupId.SelectedValue;
                    Session["GroupHeadSelectedValue"] = this.ddlGroupHead.SelectedValue;
                    //Session["SaveUpdateMessage"] = lblMessage.Text;
                    Response.Redirect("/UserInformation/frmObjectPermission.aspx");
                }
            }

            if (this.ddlUserGroupId.SelectedIndex != -1)
            {
                this.LoadGridView();
            }
        }

        protected void ddlUserGroupId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlUserGroupId.SelectedIndex != -1)
            {
                this.LoadGridView();
            }
        }


        protected void ddlGroupHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["GroupHeadSelectedValue"] = null;
            this.LoadGridView();
        }
    }
}