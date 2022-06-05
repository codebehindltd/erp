using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmHMFloor : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckPermission();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtFloorName.Text))
                {
                    this.isNewAddButtonEnable = 2;                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Floor Name.", AlertType.Warning);
                    this.txtFloorName.Focus();
                }
                else
                {
                    HMFloorBO floorBO = new HMFloorBO();
                    HMFloorDA floorDA = new HMFloorDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    floorBO.FloorName = this.txtFloorName.Text;
                    floorBO.FloorDescription = this.txtFloorDescription.Text;
                    floorBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                    if (string.IsNullOrWhiteSpace(txtFloorId.Value))
                    {
                        if (DuplicateCheckDynamicaly("FloorName", txtFloorName.Text.ToString(), 0) == 1)
                        {
                            this.isNewAddButtonEnable = 2;                            
                            CommonHelper.AlertInfo(innboardMessage, "Floor Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            this.txtFloorName.Focus();
                            return;
                        }

                        int tmpFloorId = 0;
                        floorBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = floorDA.SaveHMFloorInfo(floorBO, out tmpFloorId);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.FloorManagement.ToString(), tmpFloorId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FloorManagement));

                            this.Cancel();
                        }
                    }
                    else
                    {
                        if (DuplicateCheckDynamicaly("FloorName", txtFloorName.Text.ToString(), 1) == 1)
                        {
                            this.isNewAddButtonEnable = 2;                            
                            CommonHelper.AlertInfo(innboardMessage, "Floor Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            this.txtFloorName.Focus();
                            return;
                        }

                        floorBO.FloorId = Convert.ToInt32(txtFloorId.Value);
                        floorBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = floorDA.UpdateHMFloorInfo(floorBO);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.FloorManagement.ToString(), floorBO.FloorId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FloorManagement));
                            this.Cancel();
                        }
                    }
                }
            }
            catch(Exception ex){                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void gvHMFloor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvHMFloor.PageIndex = e.NewPageIndex;
            this.CheckPermission();
            this.LoadGridView();
        }
        protected void gvHMFloor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                // imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                // imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvHMFloor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int floorId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(floorId);
                this.btnSave.Text = "Update";
                btnSave.Visible = isUpdatePermission;
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("HotelFloor", "FloorId", floorId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.FloorManagement.ToString(), floorId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FloorManagement));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                }
                catch(Exception ex){                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        //************************ User Defined Function ********************//
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "HotelFloor";
            string pkFieldName = "FloorId";
            string pkFieldValue = this.txtFloorId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;

        }
        private void LoadGridView()
        {
            bool ActiveState = this.ddlSActiveStat.SelectedIndex == 0 ? true : false;
            string FloorName = this.txtSFloorName.Text;

            HMFloorDA da = new HMFloorDA();
            List<HMFloorBO> files = da.GetHMFloorInfoBySearchCriteria(FloorName, ActiveState);

            this.gvHMFloor.DataSource = files;
            this.gvHMFloor.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.txtFloorName.Text = string.Empty;
            this.txtFloorDescription.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtFloorId.Value = string.Empty;
            this.txtFloorName.Focus();
            this.SetTab("EntryTab");
            CheckPermission();
        }
        public void FillForm(int floorId)
        {
            HMFloorBO floorBO = new HMFloorBO();
            HMFloorDA floorDA = new HMFloorDA();
            floorBO = floorDA.GetHMFloorInfoById(floorId);
            txtFloorName.Text = floorBO.FloorName;
            txtFloorId.Value = floorBO.FloorId.ToString();
            ddlActiveStat.SelectedValue = (floorBO.ActiveStat == true ? 0 : 1).ToString();
            txtFloorDescription.Text = floorBO.FloorDescription;

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
    }
}