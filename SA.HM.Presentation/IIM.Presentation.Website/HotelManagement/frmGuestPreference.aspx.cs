using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGuestPreference : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            CheckPermission();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtPreferenceName.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Preference Name.", AlertType.Warning);
                    this.txtPreferenceName.Focus();
                }
                else
                {
                    GuestPreferenceBO preferenceBO = new GuestPreferenceBO();
                    GuestPreferenceDA preferenceDA = new GuestPreferenceDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    preferenceBO.PreferenceName = txtPreferenceName.Text;
                    preferenceBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                    preferenceBO.Description = txtDescription.Text;
                    if (string.IsNullOrWhiteSpace(hfPreferenceId.Value))
                    {
                        int tmpPrefId = 0;
                        preferenceBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = preferenceDA.SaveGuestPreferenceInfo(preferenceBO, out tmpPrefId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.GuestPreference.ToString(), tmpPrefId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreference));
                            this.Cancel();
                        }
                    }
                    else
                    {
                        preferenceBO.PreferenceId = Convert.ToInt32(hfPreferenceId.Value);
                        preferenceBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = preferenceDA.UpdateGuestPreferenceInfo(preferenceBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.GuestPreference.ToString(), preferenceBO.PreferenceId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreference));
                            this.Cancel();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }

            this.SetTab("EntryTab");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void gvPreference_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvPreference.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvPreference_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int prefId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(prefId);
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("HotelGuestPreference", "PreferenceId", prefId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.GuestPreference.ToString(), prefId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreference));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        protected void gvPreference_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = isUpdatePermission;
                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = isDeletePermission;
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtPreferenceName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;            
            this.txtDescription.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.hfPreferenceId.Value = string.Empty;     
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
        private void LoadGridView()
        {
            string prefName = txtSPreferenceName.Text;
            Boolean activeStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            GuestPreferenceDA comItemDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> files = comItemDA.GetGuestPreferenceInfo(prefName, activeStat);
            this.gvPreference.DataSource = files;
            this.gvPreference.DataBind();
            SetTab("SearchTab");
        }
        public void FillForm(int editId)
        {
            GuestPreferenceBO prefBO = new GuestPreferenceBO();
            GuestPreferenceDA prefDA = new GuestPreferenceDA();
            prefBO = prefDA.GetGuestPreferenceInfoById(editId);
            ddlActiveStat.SelectedValue = (prefBO.ActiveStat == true ? 0 : 1).ToString();
            hfPreferenceId.Value = prefBO.PreferenceId.ToString();
            txtPreferenceName.Text = prefBO.PreferenceName.ToString();
            txtDescription.Text = prefBO.Description;
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }        
    }
}