using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmHMComplementaryItem : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            CheckPermission();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtItemName.Text))
                {                    
                    this.isNewAddButtonEnable = 2;                                        
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Item Name.", AlertType.Warning);
                    this.txtItemName.Focus();
                }
                else
                {

                    HMComplementaryItemBO comItemBO = new HMComplementaryItemBO();
                    HMComplementaryItemDA comItemDA = new HMComplementaryItemDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    comItemBO.ItemName = this.txtItemName.Text;
                    comItemBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                    comItemBO.Description = txtDescription.Text;
                    comItemBO.IsDefaultItem = ddlIsDefaultItem.SelectedIndex == 0 ? true : false;
                    if (string.IsNullOrWhiteSpace(txtComplementaryItemId.Value))
                    {
                        int tmpCompId = 0;
                        comItemBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = comItemDA.SaveHMComplementaryItemInfo(comItemBO, out tmpCompId);
                        if (status)
                        {                                                        
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.ComplementaryItem.ToString(), tmpCompId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ComplementaryItem));
                            this.Cancel();
                        }
                    }
                    else
                    {
                        comItemBO.ComplementaryItemId = Convert.ToInt32(txtComplementaryItemId.Value);
                        comItemBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = comItemDA.UpdateHMComplementaryItemInfo(comItemBO);
                        if (status)
                        {                                                        
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.ComplementaryItem.ToString(), comItemBO.ComplementaryItemId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ComplementaryItem));
                            this.Cancel();
                        }
                    }
                    if (gvComplementaryItem.Rows.Count > 0)
                    {
                        this.LoadGridView();
                    }
                    this.SetTab("EntryTab");
                }
            }
            catch (Exception ex)
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void gvComplementaryItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvComplementaryItem.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvComplementaryItem_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvComplementaryItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int comItemId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(comItemId);
                this.btnSave.Text = "Update";
                btnSave.Visible = isUpdatePermission;
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("HotelComplementaryItem", "ComplementaryItemId", comItemId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.ComplementaryItem.ToString(), comItemId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ComplementaryItem));                     
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
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtItemName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtComplementaryItemId.Value = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtItemName.Focus();
            this.ddlIsDefaultItem.SelectedIndex = 0;
        }
        private void LoadGridView()
        {
            string ItemName = txtSItemName.Text; 
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            int isDefault = -1;
            isDefault = Int32.Parse(ddlSIsDefaultItem.SelectedValue);
            HMComplementaryItemDA comItemDA = new HMComplementaryItemDA();
            List<HMComplementaryItemBO> files = comItemDA.GetComplementaryItemInfoBySearchCriteria(ItemName, ActiveStat, isDefault);
            this.gvComplementaryItem.DataSource = files;
            this.gvComplementaryItem.DataBind();
            
            SetTab("SearchTab");
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
            
        }
        public void FillForm(int EditId)
        {
            
            HMComplementaryItemBO comItemBO = new HMComplementaryItemBO();
            HMComplementaryItemDA comItemDA = new HMComplementaryItemDA();
            comItemBO = comItemDA.GetComplementaryItemInfoById(EditId); 
            ddlActiveStat.SelectedValue = (comItemBO.ActiveStat == true ? 0 : 1).ToString();
            ddlIsDefaultItem.SelectedValue = (comItemBO.IsDefaultItem == true ? 1 : 0).ToString();
            txtComplementaryItemId.Value = comItemBO.ComplementaryItemId.ToString();
            txtItemName.Text = comItemBO.ItemName.ToString();
            txtDescription.Text = comItemBO.Description;
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
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("HotelComplementaryItem", "ComplementaryItemId", sEmpId);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ComplementaryItem.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ComplementaryItem));              
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch(Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/HMCommon/frmBank.aspx'>Bank</a><span class='divider'></span>";
            return breadCrumbs;
        }
    }
}