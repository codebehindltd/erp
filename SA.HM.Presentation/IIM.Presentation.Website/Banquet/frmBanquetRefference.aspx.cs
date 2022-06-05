using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetRefference : BasePage
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            hfSalesCommissionType.Value = "Percentage";
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            CheckPermission();
            //string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            //if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            //{
            //    this.isMessageBoxEnable = 2;
            //    this.lblMessage.Text = "Delete Operation Successfull";
            //}
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
                BanquetRefferenceBO refferenceBO = new BanquetRefferenceBO();
                BanquetRefferenceDA refferenceDA = new BanquetRefferenceDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                refferenceBO.Name = txtName.Text;
                refferenceBO.SalesCommission = Convert.ToDecimal(txtSalesCommission.Text);
                refferenceBO.Description = this.txtDescription.Text;

                if (string.IsNullOrWhiteSpace(txtRefferenceId.Value))
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                    {
                        //this.isMessageBoxEnable = 1;
                        //lblMessage.Text = "Reference  Name Already Exist";
                        CommonHelper.AlertInfo(innboardMessage, "Reference  Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }


                    long tmpRefferenceId = 0;
                    refferenceBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = refferenceDA.SaveBanquetRefferenceInfo(refferenceBO, out tmpRefferenceId);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        this.isNewAddButtonEnable = 2;
                        //lblMessage.Text = "Saved Operation Successfull";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetRefference.ToString(), tmpRefferenceId, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetRefference));

                        this.Cancel();
                    }
                }
                else
                {

                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                    {
                        //this.isMessageBoxEnable = 1;
                        //lblMessage.Text = "Refference Name Already Exist";
                        CommonHelper.AlertInfo(innboardMessage, "Refference Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }

                    refferenceBO.Id = Convert.ToInt64(txtRefferenceId.Value);
                    refferenceBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = refferenceDA.UpdateBanquetRefferenceInfo(refferenceBO);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        this.isNewAddButtonEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetRefference.ToString(), refferenceBO.Id, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetRefference));
                        this.Cancel();
                    }
                }
                SetTab("EntryPanel");
            }
            catch(Exception ex){
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvBanquetRefference_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvBanquetRefference_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvBanquetRefference.PageIndex = e.NewPageIndex;

        }
        protected void gvBanquetRefference_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int _refferenceId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {

                BanquetRefferenceBO refferenceBO = new BanquetRefferenceBO();
                BanquetRefferenceDA refferenceDA = new BanquetRefferenceDA();
                refferenceBO = refferenceDA.GetBanquetRefferenceInfoById(_refferenceId);
                txtDescription.Text = refferenceBO.Description;
                txtName.Text = refferenceBO.Name;
                txtSalesCommission.Text = refferenceBO.SalesCommission.ToString();
                txtRefferenceId.Value = refferenceBO.Id.ToString();
                btnSave.Text = "Update";
                SetTab("EntryPanel");
            }
            else
            {
                DeleteData(_refferenceId);
                LoadGridBySearchCriteria();
                SetTab("SearchPanel");

            }
        }
        //************************ User Defined Function ********************//

        private void Cancel()
        {
            txtDescription.Text = "";
            txtName.Text = "";
            txtSalesCommission.Text = string.Empty;
            this.txtRefferenceId.Value = string.Empty;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmBanquetRefference.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        public void DeleteData(int sRefferenceId)
        {
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("BanquetRefference", "Id", sRefferenceId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BanquetRefference.ToString(), sRefferenceId, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetRefference));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
            }
            catch(Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }

        }
        private void LoadGridBySearchCriteria()
        {
            CheckObjectPermission();
            string Name = txtSearchName.Text;

            List<BanquetRefferenceBO> refferenceList = new List<BanquetRefferenceBO>();
            BanquetRefferenceDA refferenceDA = new BanquetRefferenceDA();
            refferenceList = refferenceDA.GetBanquetRefferenceBySearchCriteria(Name);
            gvBanquetRefference.DataSource = refferenceList;
            gvBanquetRefference.DataBind();

            SetTab("SearchPanel");

        }
        private void SetTab(string TabName)
        {
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
            decimal result;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Refference Name.", AlertType.Warning);
                this.txtName.Focus();
                flag = false;
            }
            else if (string.IsNullOrEmpty(txtSalesCommission.Text) || !Decimal.TryParse(txtSalesCommission.Text, out result))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Sales Commission.", AlertType.Warning);
                this.txtSalesCommission.Focus();
                flag = false;
            }

            return flag;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "BanquetRefference";
            string pkFieldName = "Id";
            string pkFieldValue = this.txtRefferenceId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}