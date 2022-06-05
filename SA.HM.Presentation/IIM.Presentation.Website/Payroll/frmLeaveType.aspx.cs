using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmLeaveType : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.LoadLeaveMode();
                this.LoadGridView();
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
        private void LoadLeaveMode()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("LeaveMode");

            this.ddlLeaveMode.DataSource = fields;
            this.ddlLeaveMode.DataTextField = "Description";
            this.ddlLeaveMode.DataValueField = "FieldId";
            this.ddlLeaveMode.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlLeaveMode.Items.Insert(0, item);
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();

            LeaveTypeDA da = new LeaveTypeDA();
            List<LeaveTypeBO> files = da.GetLeaveTypeInfo();
            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
        }
        private void Cancel()
        {
            this.txtLeaveTypeName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtBankId.Value = string.Empty;
            this.txtLeaveTypeName.Focus();
            this.txtYearlyLeave.Text = string.Empty;
            this.ddlLeaveMode.SelectedIndex = 0;
            ckbCanCarryForward.Checked = false;
            txtMaxDayCanCarryForwardYearly.Text = string.Empty;
            txtMaxDayCanKeepAsCarryForwardLeave.Text = string.Empty;

            txtMaxDayCanCarryForwardYearly.Enabled = false;
            txtMaxDayCanKeepAsCarryForwardLeave.Enabled = false;

            ckbCanCash.Checked = false;
            txtMaxDayCanEncash.Text = string.Empty;
            txtMaxDayCanEncash.Enabled = false;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LeaveTypeBO leaveTypeBO = new LeaveTypeBO();
                LeaveTypeDA leaveTypeDA = new LeaveTypeDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                leaveTypeBO.TypeName = txtLeaveTypeName.Text;
                leaveTypeBO.YearlyLeave = Int32.Parse(txtYearlyLeave.Text);
                leaveTypeBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                leaveTypeBO.LeaveModeId = Convert.ToInt32(ddlLeaveMode.SelectedValue);

                if (ckbCanCarryForward.Checked)
                {
                    leaveTypeBO.CanCarryForward = true;
                    leaveTypeBO.MaxDayCanCarryForwardYearly = Convert.ToByte(txtMaxDayCanCarryForwardYearly.Text);
                    leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave = Convert.ToByte(txtMaxDayCanKeepAsCarryForwardLeave.Text);
                }
                else
                {
                    leaveTypeBO.CanCarryForward = false;
                    leaveTypeBO.MaxDayCanCarryForwardYearly = 0;
                    leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave = 0;
                }

                if (ckbCanCash.Checked)
                {
                    leaveTypeBO.CanEncash = true;
                    leaveTypeBO.MaxDayCanEncash = Convert.ToByte(txtMaxDayCanEncash.Text);
                }
                else
                {
                    leaveTypeBO.CanEncash = false;
                    leaveTypeBO.MaxDayCanEncash = 0;
                }

                if (string.IsNullOrWhiteSpace(txtBankId.Value))
                {
                    int tmpLeaveTypeId = 0;
                    leaveTypeBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = leaveTypeDA.SaveLeaveTypeInfo(leaveTypeBO, out tmpLeaveTypeId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.LeaveType.ToString(), tmpLeaveTypeId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveType));
                        this.LoadGridView();
                        this.Cancel();
                    }
                }
                else
                {
                    leaveTypeBO.LeaveTypeId = Convert.ToInt32(txtBankId.Value);
                    leaveTypeBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = leaveTypeDA.UpdateLeaveTypeInfo(leaveTypeBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LeaveType.ToString(), leaveTypeBO.LeaveTypeId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveType));
                        this.LoadGridView();
                        this.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
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
            int leaveTypeId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdEdit")
            {
                btnSave.Visible = isUpdatePermission;
                FillForm(leaveTypeId);
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollLeaveType", "LeaveTypeId", leaveTypeId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.LeaveInformation.ToString(), leaveTypeId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveInformation));
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
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
        public void FillForm(int EditId)
        {
            LeaveTypeBO leaveTypeBO = new LeaveTypeBO();
            LeaveTypeDA leaveTypeDA = new LeaveTypeDA();
            leaveTypeBO = leaveTypeDA.GetLeaveTypeInfoById(EditId);

            txtLeaveTypeName.Text = leaveTypeBO.TypeName;
            ddlActiveStat.SelectedIndex = leaveTypeBO.ActiveStat == true ? 0 : 1;
            txtBankId.Value = leaveTypeBO.LeaveTypeId.ToString();
            txtYearlyLeave.Text = leaveTypeBO.YearlyLeave.ToString();
            ddlLeaveMode.SelectedValue = leaveTypeBO.LeaveModeId.ToString();

            if (leaveTypeBO.CanCarryForward == true)
            {
                ckbCanCarryForward.Checked = true;
                txtMaxDayCanCarryForwardYearly.Text = Convert.ToString(leaveTypeBO.MaxDayCanCarryForwardYearly);
                txtMaxDayCanKeepAsCarryForwardLeave.Text = Convert.ToString(leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave);

                txtMaxDayCanCarryForwardYearly.Enabled = true;
                txtMaxDayCanKeepAsCarryForwardLeave.Enabled = true;
            }
            else
            {
                ckbCanCarryForward.Checked = false;
                txtMaxDayCanCarryForwardYearly.Text = string.Empty;
                txtMaxDayCanKeepAsCarryForwardLeave.Text = string.Empty;

                txtMaxDayCanCarryForwardYearly.Enabled = false;
                txtMaxDayCanKeepAsCarryForwardLeave.Enabled = false;
            }

            if (leaveTypeBO.CanEncash == true)
            {
                ckbCanCash.Checked = true;
                txtMaxDayCanEncash.Text = Convert.ToString(leaveTypeBO.MaxDayCanEncash);
                txtMaxDayCanEncash.Enabled = true;
            }
            else
            {
                ckbCanCash.Checked = false;
                txtMaxDayCanEncash.Text = string.Empty;
                txtMaxDayCanEncash.Enabled = false;
            }

            btnSave.Text = "Update";
            this.SetTab("EntryTab");
        }

    }
}