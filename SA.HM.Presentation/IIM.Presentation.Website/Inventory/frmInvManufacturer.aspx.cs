using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmInvManufacturer : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }
            this.CheckObjectPermission();
        }
        protected void gvManufacturer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _ManufacturerId;
            if (e.CommandName == "CmdEdit")
            {
                try
                {
                    _ManufacturerId = Convert.ToInt32(e.CommandArgument.ToString());
                    FillForm(_ManufacturerId);
                    SetTab("EntryTab");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _ManufacturerId = Convert.ToInt32(e.CommandArgument.ToString());
                DeleteData(_ManufacturerId);
                SetTab("SearchTab");

            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            lblMessage.Text = string.Empty;

            InvManufacturerBO PMManufacturerBO = new InvManufacturerBO();
            InvManufacturerDA PMManufacturerDA = new InvManufacturerDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PMManufacturerBO.Remarks = this.txtRemarks.Text.Trim().ToString();
            PMManufacturerBO.Name = this.txtName.Text.Trim().ToString();
            PMManufacturerBO.Code = this.txtCode.Text.Trim().ToString();

            if (string.IsNullOrWhiteSpace(txtManufacturerId.Value))
            {
                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                {
                    CommonHelper.AlertInfo("Manufacturer Name already exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) == 1)
                {
                    CommonHelper.AlertInfo("Manufacturer Code already exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }


                int tmpUserInfoId = 0;
                PMManufacturerBO.CreatedBy = userInformationBO.UserInfoId;
                bool status = PMManufacturerDA.SaveManufacturerInfo(PMManufacturerBO, out tmpUserInfoId);

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.InvManufacturer.ToString(), tmpUserInfoId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvManufacturer));
                    //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                    //EntityTypeEnum.EntityType.ChartOfAccount.ToString(), NodeId,
                    //ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                    //hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ChartOfAccount));
                    this.LoadGridView();
                    this.Cancel();
                }
                else
                {
                    CommonHelper.AlertInfo("You Can't Save. Manufacture  " + txtName.Text + "  Already Inserted.", AlertType.Warning);
                    this.LoadGridView();
                    this.Cancel();
                }
            }
            else
            {

                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo("Manufacturer Name already exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 1) == 1)
                {
                    CommonHelper.AlertInfo("Manufacturer Code already exist.", AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                PMManufacturerBO.ManufacturerId = Convert.ToInt32(txtManufacturerId.Value);
                PMManufacturerBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = PMManufacturerDA.UpdateManufactureInfo(PMManufacturerBO);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.InvManufacturer.ToString(), PMManufacturerBO.ManufacturerId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvManufacturer));
                    this.LoadGridView();
                    this.Cancel();
                    txtManufacturerId.Value = "";
                }
            }
            SetTab("EntryTab");
        }
        protected void gvManufacturer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvManufacturer.PageIndex = e.NewPageIndex;
        }
        protected void gvManufacturer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtRemarks.Text = string.Empty;
            this.txtName.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(this.txtName.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Manufacturer Name.", AlertType.Warning);
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtCode.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Manufacturer Code.", AlertType.Warning);
                flag = false;
            }
            else
            {
                this.isMessageBoxEnable = -1;
                this.lblMessage.Text = string.Empty;

            }
            return flag;
        }
        private void ClearSearch()
        {
            txtSearchCode.Text = "";
            txtSearchName.Text = "";

        }
        public void LoadGridView()
        {
            this.CheckObjectPermission();
            List<InvManufacturerBO> manufacturerList = new List<InvManufacturerBO>();
            InvManufacturerDA manufacturerDA = new InvManufacturerDA();
            manufacturerList = manufacturerDA.GetManufacturerInfoBySearchCriteria(txtSearchCode.Text, txtSearchName.Text);
            gvManufacturer.DataSource = manufacturerList;
            gvManufacturer.DataBind();
            SetTab("SearchTab");
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
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void DeleteData(int sManId)
        {
            string result = string.Empty;
            Boolean status = false;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                InvManufacturerDA DA = new InvManufacturerDA();
                status = DA.CheckItemReference(sManId);
                if (status)
                {
                    status = hmCommonDA.DeleteInfoById("InvManufacturer", "ManufacturerId", sManId);
                }
                if (status)
                {
                    result = "success";
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.InvManufacturer.ToString(), sManId,
                            ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvManufacturer));
                        this.LoadGridView();
                        this.Cancel();
                    }
                }
                else
                {
                    result = "Fail";

                    CommonHelper.AlertInfo(innboardMessage, "This Manufacturer has a reference in Item Information", AlertType.Error);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                        EntityTypeEnum.EntityType.InvManufacturer.ToString(), sManId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvManufacturer));
                    this.LoadGridView();
                    this.Cancel();

                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }
        }
        public void FillForm(int EditId)
        {
            InvManufacturerBO manufacturerBO = new InvManufacturerBO();
            InvManufacturerDA manufacturerDA = new InvManufacturerDA();
            manufacturerBO = manufacturerDA.GetManufacturerInfoById(EditId);
            txtCode.Text = manufacturerBO.Code;
            txtName.Text = manufacturerBO.Name;
            txtRemarks.Text = manufacturerBO.Remarks;
            txtManufacturerId.Value = manufacturerBO.ManufacturerId.ToString();
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "InvManufacturer";
            string pkFieldName = "ManufacturerId";
            string pkFieldValue = this.txtManufacturerId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}