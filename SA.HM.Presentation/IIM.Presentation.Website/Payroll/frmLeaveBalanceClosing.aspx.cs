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
    public partial class frmLeaveBalanceClosing : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadGridView();
                CheckObjectPermission();
            }
        }
        private void CheckObjectPermission()
        {
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
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
            //this.txtLeaveTypeName.Text = string.Empty;
            //this.ddlActiveStat.SelectedIndex = 0;
            //this.btnSave.Text = "Save";
            //this.txtBankId.Value = string.Empty;
            //this.txtLeaveTypeName.Focus();
            //this.txtYearlyLeave.Text = string.Empty;

            //ckbCanCarryForward.Checked = false;
            //txtMaxDayCanCarryForwardYearly.Text = string.Empty;
            //txtMaxDayCanKeepAsCarryForwardLeave.Text = string.Empty;

            //txtMaxDayCanCarryForwardYearly.Enabled = false;
            //txtMaxDayCanKeepAsCarryForwardLeave.Enabled = false;

            //ckbCanCash.Checked = false;
            //txtMaxDayCanEncash.Text = string.Empty;
            //txtMaxDayCanEncash.Enabled = false;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int empId = 0;
            HiddenField hfEmployeeId = (HiddenField)this.employeeSearch.FindControl("hfEmployeeId");
            empId = Convert.ToInt32(hfEmployeeId.Value);

            LeaveInformationDA leaveDA = new LeaveInformationDA();
            List<LeaveBalanceApproveViewBO> leaveBalanceList = new List<LeaveBalanceApproveViewBO>();
            leaveBalanceList = leaveDA.GetLeaveBalanceCloseForEmployee(empId);
            this.gvLeaveBalance.DataSource = leaveBalanceList;
            this.gvLeaveBalance.DataBind();
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
                ImageButton imgApprove = (ImageButton)e.Row.FindControl("ImgDetailsApproved");
                //  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
                imgApprove.Visible = isSavePermission;
          
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int leaveTypeId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdEdit")
            {

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
                            EntityTypeEnum.EntityType.LeaveType.ToString(), leaveTypeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveType));
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

        protected void gvLeaveBalance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvLeaveBalance.PageIndex = e.NewPageIndex;
        }
        protected void gvLeaveBalance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool rtnInfo = false;

            try
            {
                int leaveTypeId = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow gvRow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                Label lblEmpId = (Label)gvRow.FindControl("lblid");

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (e.CommandName == "CmdApprove")
                {
                    LeaveInformationDA lida = new LeaveInformationDA();
                    rtnInfo = lida.SaveClosingLeave(Convert.ToInt32(lblEmpId.Text), leaveTypeId, userInformationBO.UserInfoId);
                }

                if (rtnInfo == true)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LeaveType.ToString(), leaveTypeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveType));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                }
                else
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
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
            //LeaveTypeBO leaveTypeBO = new LeaveTypeBO();
            //LeaveTypeDA leaveTypeDA = new LeaveTypeDA();
            //leaveTypeBO = leaveTypeDA.GetLeaveTypeInfoById(EditId);

            //txtLeaveTypeName.Text = leaveTypeBO.TypeName;
            //ddlActiveStat.SelectedIndex = leaveTypeBO.ActiveStat == true ? 0 : 1;
            //txtBankId.Value = leaveTypeBO.LeaveTypeId.ToString();
            //txtYearlyLeave.Text = leaveTypeBO.YearlyLeave.ToString();

            //if (leaveTypeBO.CanCarryForward == true)
            //{
            //    ckbCanCarryForward.Checked = true;
            //    txtMaxDayCanCarryForwardYearly.Text = Convert.ToString(leaveTypeBO.MaxDayCanCarryForwardYearly);
            //    txtMaxDayCanKeepAsCarryForwardLeave.Text = Convert.ToString(leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave);

            //    txtMaxDayCanCarryForwardYearly.Enabled = true;
            //    txtMaxDayCanKeepAsCarryForwardLeave.Enabled = true;
            //}
            //else
            //{
            //    ckbCanCarryForward.Checked = false;
            //    txtMaxDayCanCarryForwardYearly.Text = string.Empty;
            //    txtMaxDayCanKeepAsCarryForwardLeave.Text = string.Empty;

            //    txtMaxDayCanCarryForwardYearly.Enabled = false;
            //    txtMaxDayCanKeepAsCarryForwardLeave.Enabled = false;
            //}

            //if (leaveTypeBO.CanCarryForward == true)
            //{
            //    ckbCanCash.Checked = true;
            //    txtMaxDayCanEncash.Text = Convert.ToString(leaveTypeBO.MaxDayCanEncash);
            //    txtMaxDayCanEncash.Enabled = true;
            //}
            //else
            //{
            //    ckbCanCash.Checked = false;
            //    txtMaxDayCanEncash.Text = string.Empty;
            //    txtMaxDayCanEncash.Enabled = false;
            //}

            //btnSave.Text = "Update";
            //this.SetTab("EntryTab");
        }

    }
}