using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmGuestReference : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
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

                GuestReferenceBO referenceBO = new GuestReferenceBO();
                GuestReferenceDA referenceDA = new GuestReferenceDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                referenceBO.Name = txtName.Text;
                referenceBO.Email = txtEmail.Text;
                referenceBO.Organization = txtOrganization.Text;
                referenceBO.Designation = txtDesignation.Text;
                referenceBO.CellNumber = txtCellNumber.Text;
                referenceBO.TelephoneNumber = txtTelephoneNumber.Text;
                referenceBO.Description = this.txtDescription.Text;
                referenceBO.SalesCommission = !string.IsNullOrWhiteSpace(this.txtSalesCommission.Text) ? Convert.ToDecimal(this.txtSalesCommission.Text) : 0;
                referenceBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                if (string.IsNullOrWhiteSpace(txtReferenceId.Value))
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Reference Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }


                    int tmpRefferenceId = 0;
                    referenceBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = referenceDA.SaveGuestReferenceInfo(referenceBO, out tmpRefferenceId);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.GuestReference.ToString(), tmpRefferenceId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestReference));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Reference Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }


                    referenceBO.ReferenceId = Convert.ToInt32(txtReferenceId.Value);
                    referenceBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = referenceDA.UpdateGuestReferenceInfo(referenceBO);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.GuestReference.ToString(), referenceBO.ReferenceId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestReference));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvGuestReference_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvGuestReference_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGuestReference.PageIndex = e.NewPageIndex;

        }
        protected void gvGuestReference_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int _referenceId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                GuestReferenceBO referenceBO = new GuestReferenceBO();
                GuestReferenceDA referenceDA = new GuestReferenceDA();
                referenceBO = referenceDA.GetGuestReferenceInfoById(_referenceId);
                txtDescription.Text = referenceBO.Description;
                ddlActiveStat.SelectedValue = (referenceBO.ActiveStat == true ? 0 : 1).ToString();
                txtName.Text = referenceBO.Name;
                txtEmail.Text = referenceBO.Email;
                txtOrganization.Text = referenceBO.Organization;
                txtDesignation.Text = referenceBO.Designation;
                txtCellNumber.Text = referenceBO.CellNumber;
                txtTelephoneNumber.Text = referenceBO.TelephoneNumber;
                txtSalesCommission.Text = referenceBO.SalesCommission.ToString();
                txtReferenceId.Value = referenceBO.ReferenceId.ToString();
                btnSave.Text = "Update";
                SetTab("EntryPanel");
            }
            else
            {
                DeleteData(_referenceId);
                LoadGridBySearchCriteria();
                SetTab("SearchPanel");

            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.ddlActiveStat.SelectedIndex = 0;
            txtDescription.Text = "";
            txtName.Text = "";
            txtEmail.Text = string.Empty;
            txtOrganization.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtCellNumber.Text = string.Empty;
            txtTelephoneNumber.Text = string.Empty;
            this.txtReferenceId.Value = string.Empty;
            this.txtSalesCommission.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGuestReference.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void DeleteData(int sReferenceId)
        {
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("HotelGuestReference", "ReferenceId", sReferenceId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                        EntityTypeEnum.EntityType.GuestReference.ToString(), sReferenceId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestReference));
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
            CheckObjectPermission();
            string Name = txtSearchName.Text;

            List<GuestReferenceBO> referenceList = new List<GuestReferenceBO>();
            GuestReferenceDA referenceDA = new GuestReferenceDA();
            referenceList = referenceDA.GetGuestReferenceBySearchCriteria(Name);
            gvGuestReference.DataSource = referenceList;
            gvGuestReference.DataBind();
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
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Reference Name.", AlertType.Warning);
                this.txtName.Focus();
                flag = false;

            }
            else if (!string.IsNullOrWhiteSpace(this.txtEmail.Text))
            {
                if (!(hmUtility.IsInnboardValidMail(this.txtEmail.Text)))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Email.", AlertType.Warning);
                    this.txtEmail.Focus();
                    flag = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.txtCellNumber.Text))
            {
                var match = Regex.Match(txtCellNumber.Text, @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
                if (match.Success)
                {

                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Cell Number.", AlertType.Warning);
                    txtCellNumber.Focus();
                    return flag = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.txtTelephoneNumber.Text))
            {
                var match = Regex.Match(txtTelephoneNumber.Text, @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
                if (match.Success)
                {

                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Telephone Number.", AlertType.Warning);
                    txtTelephoneNumber.Focus();
                    return flag = false;
                }
            }

            //else if (string.IsNullOrEmpty(txtSalesCommission.Text) || !Decimal.TryParse(txtSalesCommission.Text, out result))
            //{

            //    this.isMessageBoxEnable = 1;
            //    this.lblMessage.Text = "Please provide correct Discount Amount.";
            //    this.txtSalesCommission.Focus();
            //    flag = false;

            //}
            return flag;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "HotelGuestReference";
            string pkFieldName = "ReferenceId";
            string pkFieldValue = this.txtReferenceId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}