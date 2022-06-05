using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmRosterHead : BasePage
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
                this.LoadCurrentDate();
                CheckObjectPermission();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime resultDate;
            if (string.IsNullOrWhiteSpace(this.txtBPHead.Text))
            {
                this.isNewAddButtonEnable = 2;               
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Name.", AlertType.Warning);
                this.txtBPHead.Focus();
            }
            else if (String.IsNullOrEmpty(txtPeriodFrom.Text))
            {
                this.isNewAddButtonEnable = 2;                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "From Date.", AlertType.Warning);
                this.txtPeriodFrom.Focus();
            
            }
            //else if (String.IsNullOrEmpty(txtPeriodTo.Text) || !DateTime.TryParse(txtPeriodTo.Text, out resultDate))
            //{
                
            //    this.isNewAddButtonEnable = 2;
            //    this.isMessageBoxEnable = 1;
            //    lblMessage.Text = "Please enter To Date";
            //    this.txtPeriodTo.Focus();

            //}
            else
            {
                try
                {
                    RosterHeadBO entityBO = new RosterHeadBO();
                    RosterHeadDA entityDA = new RosterHeadDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    entityBO.RosterName = this.txtBPHead.Text;
                    entityBO.FromDate = hmUtility.GetDateTimeFromString(this.txtPeriodFrom.Text, userInformationBO.ServerDateFormat);
                    //entityBO.ToDate = hmUtility.ParseDateTime(this.txtPeriodTo.Text);
                    entityBO.ToDate = entityBO.FromDate.AddDays(6);
                    entityBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                    if (string.IsNullOrWhiteSpace(txtBusinessPromotionId.Value))
                    {
                        int tmpRosterId = 0;
                        entityBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = entityDA.SaveRosterHeadInfo(entityBO, out tmpRosterId);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.RosterHead.ToString(), tmpRosterId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RosterHead));
                            this.Cancel();
                        }
                    }
                    else
                    {
                        entityBO.RosterId = Convert.ToInt32(txtBusinessPromotionId.Value);
                        entityBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = entityDA.UpdateRosterHeadInfo(entityBO);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.RosterHead.ToString(), entityBO.RosterId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RosterHead));
                            this.Cancel();
                        }
                    }
                    if (gvGuestHouseService.Rows.Count > 0)
                    {
                        this.LoadGridView();
                    }
                    this.SetTab("EntryTab");
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    
                }
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rosterId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(rosterId);
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollRosterHead", "RosterId", rosterId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.RosterHead.ToString(), rosterId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RosterHead));
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
        private void CheckObjectPermission()
        {
            
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView()
        {
            string rosterHead = txtSName.Text;
            this.CheckObjectPermission();

            RosterHeadDA da = new RosterHeadDA();
            List<RosterHeadBO> files = da.GetRosterHeadInfoBySearchCriteria(rosterHead);

            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
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
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtPeriodFrom.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtPeriodTo.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(7));
        }
        private void Cancel()
        {
            this.LoadCurrentDate();
            this.txtBPHead.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtBusinessPromotionId.Value = string.Empty;
            this.txtBPHead.Focus();
        }
        public void FillForm(int EditId)
        {
            RosterHeadBO entityBO = new RosterHeadBO();
            RosterHeadDA entityDA = new RosterHeadDA();
            entityBO = entityDA.GetRosterHeadInfoById(EditId);
            this.txtBusinessPromotionId.Value = entityBO.RosterId.ToString();
            ddlActiveStat.SelectedValue = (entityBO.ActiveStat == true ? 0 : 1).ToString();
            txtBPHead.Text = entityBO.RosterName;
            txtPeriodFrom.Text = hmUtility.GetStringFromDateTime(entityBO.FromDate);
            txtPeriodTo.Text = hmUtility.GetStringFromDateTime(entityBO.ToDate);
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}