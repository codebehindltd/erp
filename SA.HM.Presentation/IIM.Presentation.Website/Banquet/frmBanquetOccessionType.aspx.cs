using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Banquet
{

    public partial class frmBanquetOccessionType : BasePage
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            CheckPermission();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridBySearchCriteria();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    this.isNewAddButtonEnable = 2;
                    return;
                }


                BanquetOccessionTypeBO themeBO = new BanquetOccessionTypeBO();
                BanquetOccessionTypeDA themeDA = new BanquetOccessionTypeDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                themeBO.Name = txtName.Text;
                themeBO.Code = this.txtCode.Text;
                themeBO.Description = this.txtDescription.Text;

                if (string.IsNullOrWhiteSpace(txtOccessionTypeId.Value))
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Occassion Type Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }
                    else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Occassion Type Code" + AlertMessage.TextTypeValidation, AlertType.Warning);
                        txtCode.Focus();
                        return;
                    }

                    long tmpThemeId = 0;
                    themeBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = themeDA.SaveBanquetThemeInfo(themeBO, out tmpThemeId);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetOccessionType.ToString(),
                        tmpThemeId, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetOccessionType));

                        this.Cancel();
                    }
                }
                else
                {

                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Occassion Type Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }
                    else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, " Occassion Type  Code" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        txtCode.Focus();
                        return;
                    }
                    themeBO.Id = Convert.ToInt64(txtOccessionTypeId.Value);
                    themeBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = themeDA.UpdateBanquetThemeInfo(themeBO);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetOccessionType.ToString(), themeBO.Id,
                            ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetOccessionType));
                        this.Cancel();
                    }
                }
                this.SetTab("EntryPanel");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }

        }
        protected void gvTheme_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                // imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvTheme_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTheme.PageIndex = e.NewPageIndex;
        }
        protected void gvTheme_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            long _themeId = Convert.ToInt64(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                BanquetOccessionTypeBO themeBO = new BanquetOccessionTypeBO();
                BanquetOccessionTypeDA themeDA = new BanquetOccessionTypeDA();
                themeBO = themeDA.GetBanquetThemeInfoById(_themeId);

                txtDescription.Text = themeBO.Description;
                txtCode.Text = themeBO.Code;
                txtName.Text = themeBO.Name;

                txtOccessionTypeId.Value = themeBO.Id.ToString();
                btnSave.Text = "Update";
                SetTab("EntryPanel");
            }
            else
            {
                DeleteData(_themeId);
                LoadGridBySearchCriteria();
                SetTab("SearchPanel");

            }
        }
        //************************ User Defined Function ********************//

        private void Cancel()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            txtName.Text = "";
            this.txtOccessionTypeId.Value = string.Empty;
            this.txtCode.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        public void DeleteData(long sEmpId)
        {
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("BanquetOccessionType", "Id", sEmpId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BanquetOccessionType.ToString(),
                          sEmpId, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetOccessionType));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }

        }
        private void LoadGridBySearchCriteria()
        {
            string Name = txtSearchName.Text;
            string Code = txtSearchCode.Text;

            List<BanquetOccessionTypeBO> themeList = new List<BanquetOccessionTypeBO>();
            BanquetOccessionTypeDA themeDA = new BanquetOccessionTypeDA();
            themeList = themeDA.GetBanquetThemeBySearchCriteria(Name, Code);

            gvTheme.DataSource = themeList;
            gvTheme.DataBind();

            SetTab("SearchPanel");

        }
        private void SetTab(string TabName)
        {
            // SetTab("SearchPanel");
            // SetTab("EntryPanel");
            if (TabName == "SearchPanel")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryPanel")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Occassion Type Name.", AlertType.Warning);
                this.txtName.Focus();
                flag = false;

            }
            else if (string.IsNullOrEmpty(txtCode.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Occassion Type Code.", AlertType.Warning);
                flag = false;
                txtCode.Focus();
            }
            return flag;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "BanquetOccessionType";
            string pkFieldName = "Id";
            string pkFieldValue = this.txtOccessionTypeId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}