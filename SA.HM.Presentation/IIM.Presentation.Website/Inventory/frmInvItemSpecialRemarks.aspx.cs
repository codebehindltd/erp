using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmInvItemSpecialRemarks : BasePage
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
            CheckObjectPermission();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtItemName.Text))
            {
                this.isMessageBoxEnable = 1;
                this.isNewAddButtonEnable = 2;
                lblMessage.Text = "Please Provide Special Remarks";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bank Name.", AlertType.Warning);
                this.txtItemName.Focus();
            }
            else
            {

                InvItemSpecialRemarksBO comItemBO = new InvItemSpecialRemarksBO();
                InvItemSpecialRemarksDA comItemDA = new InvItemSpecialRemarksDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                comItemBO.SpecialRemarks = this.txtItemName.Text;
                comItemBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                comItemBO.Description = txtDescription.Text;
                if (string.IsNullOrWhiteSpace(txtSpecialRemarksId.Value))
                {
                    int tmpBankId = 0;
                    comItemBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = comItemDA.SaveInvItemSpecialRemarksInfo(comItemBO, out tmpBankId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.SpecialRemarks.ToString(), tmpBankId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SpecialRemarks));
                        this.Cancel();
                    }
                }
                else
                {
                    comItemBO.SpecialRemarksId = Convert.ToInt32(txtSpecialRemarksId.Value);
                    comItemBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = comItemDA.UpdateInvItemSpecialRemarksInfo(comItemBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.SpecialRemarks.ToString(), comItemBO.SpecialRemarksId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SpecialRemarks));
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
        protected void gvComplementaryItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvComplementaryItem.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
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
                this.btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("InvItemSpecialRemarks", "SpecialRemarksId", comItemId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
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
            this.txtSpecialRemarksId.Value = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtItemName.Focus();
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            string ItemName = txtSItemName.Text; 
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            InvItemSpecialRemarksDA comItemDA = new InvItemSpecialRemarksDA();
            List<InvItemSpecialRemarksBO> files = comItemDA.GetInvItemSpecialRemarksInfoBySearchCriteria(ItemName, ActiveStat);
            this.gvComplementaryItem.DataSource = files;
            this.gvComplementaryItem.DataBind();
            
            SetTab("SearchTab");
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void FillForm(int EditId)
        {

            InvItemSpecialRemarksBO comItemBO = new InvItemSpecialRemarksBO();
            InvItemSpecialRemarksDA comItemDA = new InvItemSpecialRemarksDA();
            comItemBO = comItemDA.GetInvItemSpecialRemarksInfoById(EditId); 
            ddlActiveStat.SelectedValue = (comItemBO.ActiveStat == true ? 0 : 1).ToString();
            txtSpecialRemarksId.Value = comItemBO.SpecialRemarksId.ToString();
            txtItemName.Text = comItemBO.SpecialRemarks.ToString();
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
        public static string DeleteData(int sEmpId)
        {
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("InvItemSpecialRemarks", "SpecialRemarksId", sEmpId);
                if (status)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/HMCommon/frmInvItemSpecialRemarks.aspx'>Bank</a><span class='divider'></span>";
            return breadCrumbs;
        }
    }
}