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
    public partial class frmFloorBlock : BasePage
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
                
            }
            CheckPermission();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtFloorName.Text))
                {
                    this.isNewAddButtonEnable = 2;                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Block Name.", AlertType.Warning);
                    this.txtFloorName.Focus();
                }
                else
                {
                    FloorBlockBO floorBO = new FloorBlockBO();
                    FloorBlockDA floorDA = new FloorBlockDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    floorBO.BlockName = this.txtFloorName.Text;
                    floorBO.BlockDescription = this.txtFloorDescription.Text;
                    floorBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                    if (string.IsNullOrWhiteSpace(txtFloorId.Value))
                    {
                        if (DuplicateCheckDynamicaly("BlockName", txtFloorName.Text.ToString(), 0) == 1)
                        {
                            this.isNewAddButtonEnable = 2;                            
                            CommonHelper.AlertInfo(innboardMessage, "Block Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            this.txtFloorName.Focus();
                            return;
                        }

                        int tmpFloorId = 0;
                        floorBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = floorDA.SaveFloorBlockInfo(floorBO, out tmpFloorId);
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
                        if (DuplicateCheckDynamicaly("BlockName", txtFloorName.Text.ToString(), 1) == 1)
                        {
                            this.isNewAddButtonEnable = 2;                            
                            CommonHelper.AlertInfo(innboardMessage, "Block Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            this.txtFloorName.Focus();
                            return;
                        }

                        floorBO.BlockId = Convert.ToInt32(txtFloorId.Value);
                        floorBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = floorDA.UpdateFloorBlockInfo(floorBO);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.FloorManagement.ToString(), floorBO.BlockId,
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
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("HotelFloorBlock", "BlockId", floorId);
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
            string tableName = "HotelFloorBlock";
            string pkFieldName = "BlockId";
            string pkFieldValue = this.txtFloorId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        
        private void LoadGridView()
        {
            bool ActiveState = this.ddlSActiveStat.SelectedIndex == 0 ? true : false;
            string FloorName = this.txtSFloorName.Text;

            FloorBlockDA da = new FloorBlockDA();
            List<FloorBlockBO> files = da.GetFloorBlockInfoBySearchCriteria(FloorName, ActiveState);

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
        }
        public void FillForm(int floorId)
        {
            FloorBlockBO floorBO = new FloorBlockBO();
            FloorBlockDA floorDA = new FloorBlockDA();
            floorBO = floorDA.GetFloorBlockInfoById(floorId);
            txtFloorName.Text = floorBO.BlockName;
            txtFloorId.Value = floorBO.BlockId.ToString();
            ddlActiveStat.SelectedValue = (floorBO.ActiveStat == true ? 0 : 1).ToString();
            txtFloorDescription.Text = floorBO.BlockDescription;

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
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }
    }
}