using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpLeaveApplication : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        protected int _LeaveId;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadLeaveMode();
                LoadLeaveType();
                LoadLeaveStatus();
                GetLeaveTakenNBalanceByEmployee();
                LoadReportingTo();
            }
            CheckObjectPermission();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
            SearchPanel.Visible = true;
        }
        protected void gvEmployeeLeave_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (LeaveInformationBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCancel = (ImageButton)e.Row.FindControl("ImgCancel");

                if (item.LeaveStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.LeaveStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgCancel.Visible = isDeletePermission;
                    imgApproved.Visible = isSavePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgCancel.Visible = false;
                    imgApproved.Visible = false;
                }
            }
        }
        protected void gvEmployeeLeave_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool status = false;
            int leaveId = Convert.ToInt32(e.CommandArgument.ToString());
            LeaveInformationBO leaveInformationBO = new LeaveInformationBO();
            LeaveInformationDA leaveInformationDA = new LeaveInformationDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (e.CommandName == "CmdEdit")
            {
                FillForm(leaveId);
            }
            else if (e.CommandName == "CmdApproved")
            {
                leaveInformationBO.LeaveId = leaveId;
                leaveInformationBO.LeaveStatus = HMConstants.ApprovalStatus.Approved.ToString();
                leaveInformationBO.LastModifiedBy = userInformationBO.UserInfoId;

                status = leaveInformationDA.UpdateEmpLeaveStatus(leaveInformationBO);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), leaveId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee Leave Status Updated");
                    LoadGrid();
                    GetLeaveTakenNBalanceByEmployee();
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            else if (e.CommandName == "CmdCancel")
            {
                leaveInformationBO.LeaveId = leaveId;
                leaveInformationBO.LeaveStatus = HMConstants.ApprovalStatus.Cancel.ToString();
                leaveInformationBO.LastModifiedBy = userInformationBO.UserInfoId;

                status = leaveInformationDA.UpdateEmpLeaveStatus(leaveInformationBO);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), leaveId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Employee Leave Status Cancelled");
                    LoadGrid();
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            else if (e.CommandName == "CmdApplicationPreview")
            {
                _LeaveId = -1;
                _LeaveId = Convert.ToInt32(e.CommandArgument.ToString());
                string url = "/Payroll/Reports/frmReportLeaveApplication.aspx?LeaveId=" + _LeaveId;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(GetType(), "script", sPopUp, true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveLeaveInformation();
        }
        //************************ User Defined Function ********************//
        private void LoadLeaveType()
        {
            LeaveTypeDA entityDA = new LeaveTypeDA();
            ddlLeaveTypeId.DataSource = entityDA.GetActiveLeaveTypeInfo();
            ddlLeaveTypeId.DataTextField = "TypeName";
            ddlLeaveTypeId.DataValueField = "LeaveTypeId";
            ddlLeaveTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLeaveTypeId.Items.Insert(0, item);

            ddlLeaveTypeIdSearch.DataSource = entityDA.GetActiveLeaveTypeInfo();
            ddlLeaveTypeIdSearch.DataTextField = "TypeName";
            ddlLeaveTypeIdSearch.DataValueField = "LeaveTypeId";
            ddlLeaveTypeIdSearch.DataBind();
            ddlLeaveTypeIdSearch.Items.Insert(0, item);
        }
        private void LoadLeaveMode()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("LeaveMode", hmUtility.GetDropDownFirstValue());

            fields.RemoveAt(0);

            ddlLeaveMode.DataSource = fields;
            ddlLeaveMode.DataTextField = "Description";
            ddlLeaveMode.DataValueField = "FieldValue";
            ddlLeaveMode.DataBind();
            ddlLeaveMode.Items.Insert(0, new ListItem("---Please Select---", "0"));

            ddlLeaveModeSearch.DataSource = fields;
            ddlLeaveModeSearch.DataTextField = "Description";
            ddlLeaveModeSearch.DataValueField = "FieldValue";
            ddlLeaveModeSearch.DataBind();
            ddlLeaveModeSearch.Items.Insert(0, new ListItem("---Please Select---", "0"));
        }
        private void LoadLeaveStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("LeaveStatus", hmUtility.GetDropDownFirstValue());

            fields.RemoveAt(0);

            this.ddlLeaveStatusSearch.DataSource = fields;
            this.ddlLeaveStatusSearch.DataTextField = "FieldValue";
            this.ddlLeaveStatusSearch.DataValueField = "FieldValue";
            this.ddlLeaveStatusSearch.DataBind();

            ddlLeaveStatusSearch.Items.Insert(0, new ListItem("---Please Select---", "0"));
        }
        private void LoadReportingTo()
        {
            DesignationDA designationDA = new DesignationDA();
            List<DesignationBO> fields = new List<DesignationBO>();
            fields = designationDA.GetDesignationInfo();

            this.ddlReportingTo.DataSource = fields;
            this.ddlReportingTo.DataTextField = "Name";
            this.ddlReportingTo.DataValueField = "DesignationId";
            this.ddlReportingTo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlReportingTo.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {            
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        public void SaveLeaveInformation()
        {
            try
            {
                string message = string.Empty;
                LeaveInformationBO leaveInformationBO = new LeaveInformationBO();
                LeaveInformationDA leaveInformationDA = new LeaveInformationDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                leaveInformationBO.LeaveMode = ddlLeaveMode.SelectedValue;
                leaveInformationBO.LeaveTypeId = Convert.ToInt32(ddlLeaveTypeId.SelectedValue);
                leaveInformationBO.FromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat);
                leaveInformationBO.ToDate = hmUtility.GetDateTimeFromString(txtToDate.Text, userInformationBO.ServerDateFormat);
                leaveInformationBO.NoOfDays = ((leaveInformationBO.ToDate - leaveInformationBO.FromDate).Days) + 1;
                leaveInformationBO.LeaveStatus = HMConstants.ApprovalStatus.Submit.ToString();
                if (ddlReportingTo.SelectedIndex != 0)
                    leaveInformationBO.ReportingTo = Convert.ToInt32(ddlReportingTo.SelectedValue);
                leaveInformationBO.Reason = txtRemarks.Text;

                if (string.IsNullOrWhiteSpace(hfLeaveId.Value))
                {
                    int tmpUserInfoId = 0;
                    leaveInformationBO.EmpId = userInformationBO.EmpId;
                    leaveInformationBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = leaveInformationDA.SaveEmpLeaveInformation(leaveInformationBO, out tmpUserInfoId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), tmpUserInfoId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLeaveInfo));

                        Clear();
                        GetLeaveTakenNBalanceByEmployee();
                        SaveMessageForLeaveNotification(leaveInformationBO.ReportingTo);
                    }
                }
                else
                {
                    leaveInformationBO.EmpId = Convert.ToInt32(hfEmpId.Value);
                    leaveInformationBO.LeaveId = Convert.ToInt32(hfLeaveId.Value);
                    leaveInformationBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = leaveInformationDA.UpdateUserInformation(leaveInformationBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), leaveInformationBO.LeaveId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLeaveInfo));
                        Clear();
                        GetLeaveTakenNBalanceByEmployee();
                        SaveMessageForLeaveNotification(leaveInformationBO.ReportingTo);// no effect
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        private void SaveMessageForLeaveNotification(int designationId)
        {
            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationBO reportingtoBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            reportingtoBO = userInformationDA.GetUserInformationForLeaveNotification(designationId);

            if (reportingtoBO != null)
            {
                if (userInformationBO.EmpId > 0)
                {
                    EmployeeDA empDA = new EmployeeDA();
                    EmployeeBO empBO = new EmployeeBO();
                    empBO = empDA.GetEmployeeInfoById(userInformationBO.EmpId);
                    string msgBody = string.Empty;
                    msgBody = empBO.DisplayName;

                    bool IsMessageSendAllGroupUser = false;

                    CommonMessageDA messageDa = new CommonMessageDA();
                    CommonMessageBO messageBO = new CommonMessageBO();
                    CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
                    List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

                    messageBO.Subjects = "Leave Application";
                    messageBO.MessageBody = "You have leave application approval pending of ." + msgBody;
                    messageBO.MessageFrom = userInformationBO.UserInfoId;
                    messageBO.MessageFromUserId = userInformationBO.UserId;
                    messageBO.MessageDate = DateTime.Now;
                    messageBO.Importance = "Normal";

                    detailBO.MessageTo = reportingtoBO.UserInfoId;
                    detailBO.UserId = reportingtoBO.UserId;
                    messageDetails.Add(detailBO);

                    messageDa.SaveMessage(messageBO, messageDetails, IsMessageSendAllGroupUser);
                }
            }
        }
        public void FillForm(int EditId)
        {
            LeaveInformationBO leave = new LeaveInformationBO();
            LeaveInformationDA da = new LeaveInformationDA();

            leave = da.GetEmpLeaveInformationById(EditId);

            hfLeaveId.Value = leave.LeaveId.ToString();
            hfEmpId.Value = leave.EmpId.ToString();
            ddlLeaveMode.SelectedValue = leave.LeaveMode;
            ddlLeaveTypeId.SelectedValue = leave.LeaveTypeId.ToString();
            ddlReportingTo.SelectedValue = leave.ReportingTo.ToString();
            txtFromDate.Text = hmUtility.GetStringFromDateTime(leave.FromDate);
            txtToDate.Text = hmUtility.GetStringFromDateTime(leave.ToDate);
            txtNoOfDays.Text = leave.NoOfDays.ToString();
            txtRemarks.Text = leave.Reason;
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";
            SetTab("entry");
        }
        private void Clear()
        {
            txtRemarks.Text = string.Empty;
            txtNoOfDays.Text = "0";
            ddlLeaveTypeId.SelectedValue = "0";
            ddlLeaveTypeIdSearch.SelectedValue = "0";
            ddlLeaveMode.SelectedValue = "0";
            ddlLeaveModeSearch.SelectedValue = "0";
            ddlLeaveStatusSearch.SelectedValue = "0";
            ddlReportingTo.SelectedValue = "0";
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtFromDateSearch.Text = string.Empty;
            txtToDateSearch.Text = string.Empty;

            gvEmployeeLeave.DataSource = null;
            gvEmployeeLeave.DataBind();
            SearchPanel.Visible = false;

            hfLeaveId.Value = "";

            btnSave.Text = "Save";
            SetTab("entry");
        }
        private void SetTab(string TabName)
        {
            if (TabName == "entry")
            {
                entry.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                search.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "search")
            {
                search.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                entry.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        public void GetLeaveTakenNBalanceByEmployee()
        {
            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            List<LeaveTakenNBalanceBO> lists = new List<LeaveTakenNBalanceBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            lists = leaveDa.GetLeaveTakenNBalanceByEmployee(userInformationBO.EmpId, DateTime.Now);
            gvLeaveBalance.DataSource = lists;
            gvLeaveBalance.DataBind();
        }
        private void LoadGrid()
        {
            string leaveType = string.Empty, leaveMode = string.Empty, fromDate = string.Empty, toDate = string.Empty, leaveStatus = string.Empty;
            int totalRecords = 0;

            if (ddlLeaveTypeIdSearch.SelectedValue != "0")
                leaveType = ddlLeaveTypeIdSearch.SelectedValue;

            if (ddlLeaveModeSearch.SelectedValue != "0")
                leaveMode = ddlLeaveModeSearch.SelectedValue;

            fromDate = txtFromDateSearch.Text;
            toDate = txtToDateSearch.Text;

            if (ddlLeaveStatusSearch.SelectedValue != "0")
                leaveStatus = ddlLeaveStatusSearch.SelectedValue;

            UserInformationBO userInformationBO = new UserInformationBO();

            LeaveInformationDA entityDA = new LeaveInformationDA();
            List<LeaveInformationBO> employeeLeave = new List<LeaveInformationBO>();

            HMUtility hmUtility = new HMUtility();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DateTime leaveFromDate = DateTime.Now;
            DateTime leaveToDate = DateTime.Now;

            if (String.IsNullOrEmpty(fromDate))
            {
                leaveFromDate = DateTime.Now.AddDays((DateTime.Now.Day) * -1).Date;
            }
            else
            {
                leaveFromDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
            }

            if (String.IsNullOrEmpty(toDate))
            {
                leaveToDate = DateTime.Now.Date;
            }
            else
            {
                leaveToDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
            }

            //UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int IsAdminUser = 0;
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                    {
                        IsAdminUser = 1;
                    }
                }
            }
            #endregion

            employeeLeave = entityDA.GetEmpLeaveInformationByEmpIdForGridPaging(IsAdminUser, userInformationBO.UserInfoId, userInformationBO.EmpId, leaveStatus, leaveType, leaveMode, leaveFromDate, leaveToDate, userInformationBO.GridViewPageSize, 1, out totalRecords);

            gvEmployeeLeave.DataSource = employeeLeave;
            gvEmployeeLeave.DataBind();            

            SetTab("search");
        }
    }
}