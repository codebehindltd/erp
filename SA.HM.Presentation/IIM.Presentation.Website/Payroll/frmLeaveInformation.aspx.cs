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
    public partial class frmLeaveInformation : BasePage
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
                LoadDummyGridData();
                this.LoadEmployee();
                this.LoadLeaveMode();
                this.LoadLeaveType();
                this.LoadLeaveStatus();
                CheckObjectPermission();
            }
        }
        private void LoadEmployee()
        {
            EmployeeDA entityDA = new EmployeeDA();
            List<EmployeeBO> boList = new List<EmployeeBO>();
            boList = entityDA.GetEmployeeInfo();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlReportingTo.DataSource = boList;
            ddlReportingTo.DataTextField = "EmployeeName";
            ddlReportingTo.DataValueField = "EmpId";
            ddlReportingTo.DataBind();
            ddlReportingTo.Items.Insert(0, item);
        }
        private void LoadLeaveType()
        {
            LeaveTypeDA entityDA = new LeaveTypeDA();
            this.ddlLeaveTypeId.DataSource = entityDA.GetLeaveTypeInfo();
            this.ddlLeaveTypeId.DataTextField = "TypeName";
            this.ddlLeaveTypeId.DataValueField = "LeaveTypeId";
            this.ddlLeaveTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlLeaveTypeId.Items.Insert(0, item);

            this.ddlLeaveTypeIdSearch.DataSource = entityDA.GetLeaveTypeInfo();
            this.ddlLeaveTypeIdSearch.DataTextField = "TypeName";
            this.ddlLeaveTypeIdSearch.DataValueField = "LeaveTypeId";
            this.ddlLeaveTypeIdSearch.DataBind();

            this.ddlLeaveTypeIdSearch.Items.Insert(0, item);
        }
        private void LoadLeaveMode()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("LeaveMode");

            this.ddlLeaveMode.DataSource = fields;
            this.ddlLeaveMode.DataTextField = "Description";
            this.ddlLeaveMode.DataValueField = "FieldValue";
            this.ddlLeaveMode.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLeaveMode.Items.Insert(0, item);

            ddlLeaveModeSearch.DataSource = fields;
            ddlLeaveModeSearch.DataTextField = "Description";
            ddlLeaveModeSearch.DataValueField = "FieldValue";
            ddlLeaveModeSearch.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstAllValue();
            ddlLeaveModeSearch.Items.Insert(0, item1);
        }
        private void LoadLeaveStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("LeaveStatus", hmUtility.GetDropDownFirstValue());

            this.ddlLeaveStatus.DataSource = fields;
            this.ddlLeaveStatus.DataTextField = "FieldValue";
            this.ddlLeaveStatus.DataValueField = "FieldValue";
            this.ddlLeaveStatus.DataBind();

            this.ddlLeaveStatusSearch.DataSource = fields;
            this.ddlLeaveStatusSearch.DataTextField = "FieldValue";
            this.ddlLeaveStatusSearch.DataValueField = "FieldValue";
            this.ddlLeaveStatusSearch.DataBind();

            this.ddlLeaveStatus.SelectedIndex = 0;
        }
        private void CheckObjectPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadDummyGridData()
        {
            List<LeaveInformationBO> leave = new List<LeaveInformationBO>();

            LeaveInformationBO obj = new LeaveInformationBO();
            obj.LeaveId = 0;
            obj.EmpId = 0;
            obj.EmployeeName = "";
            obj.LeaveMode = "";
            obj.LeaveTypeId = 0;
            obj.TypeName = "";
            obj.FromDate = DateTime.Now;
            obj.ToDate = DateTime.Now;
            obj.NoOfDays = 0;
            obj.LeaveStatus = "";
            obj.Reason = "";

            leave.Add(obj);

            this.gvUserInformation.DataSource = leave;
            this.gvUserInformation.DataBind();
        }
        //************************ User Defined Function ********************//
        protected void gvUserInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int empId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdDelete")
            {
                DeleteData(empId);
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static GridViewDataNPaging<LeaveInformationBO, GridPaging> LoadLeaveInformation(int empId, string leaveType, string leaveMode, string fromDate, string toDate, string leaveStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DateTime leaveFromDate = DateTime.Now;
            DateTime leaveToDate = DateTime.Now;

            if (!string.IsNullOrEmpty(fromDate))
            {
                leaveFromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                leaveFromDate = leaveFromDate.AddDays(-1);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                leaveToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                leaveToDate = leaveToDate.AddDays(1).AddSeconds(-1);
            }

            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<LeaveInformationBO, GridPaging> myGridData = new GridViewDataNPaging<LeaveInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

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

            LeaveInformationDA entityDA = new LeaveInformationDA();
            List<LeaveInformationBO> employeeLeave = entityDA.GetEmpLeaveInformationByEmpIdForGridPaging(IsAdminUser, userInformationBO.UserInfoId, empId, leaveStatus, leaveType, leaveMode, leaveFromDate, leaveToDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(employeeLeave, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo SaveLeaveInformation(int employeeID, string leaveId, string leaveMode, int leaveType, string fromDate, string toDate, int numberOfDays, int reportingTo, string leaveStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                string message = string.Empty;

                HMUtility hmUtility = new HMUtility();
                LeaveInformationBO leaveInformationBO = new LeaveInformationBO();
                LeaveInformationDA leaveInformationDA = new LeaveInformationDA();
                EmployeeDA employeeDA = new EmployeeDA();
                EmployeeBO employeeBO = new EmployeeBO();
                employeeBO = employeeDA.GetEmployeeInfoById(employeeID);

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                leaveInformationBO.EmpId = employeeID;
                leaveInformationBO.LeaveMode = leaveMode;
                leaveInformationBO.LeaveTypeId = leaveType;
                leaveInformationBO.FromDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
                leaveInformationBO.ToDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
                leaveInformationBO.NoOfDays = numberOfDays;
                leaveInformationBO.LeaveStatus = leaveStatus;
                leaveInformationBO.ReportingTo = 0;
                if (leaveStatus == "Pending")
                {
                    leaveInformationBO.ReportingTo = reportingTo;
                }
                else
                {
                    leaveInformationBO.ReportingTo = userInformationBO.UserInfoId;
                }

                if (employeeBO.Gender == "Male" && leaveType == 4)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("You are not eligible for Maternity Leave ", AlertType.Information);
                    return rtninf;
                }

                if (employeeBO.Gender == "Female" && leaveType == 5)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("You are not eligible for Paternity Leave ", AlertType.Information);
                    return rtninf;
                }

                List<LeaveInformationBO> employeeLeave = leaveInformationDA.GetEmpDuplicateLeaveInformation(employeeID, leaveInformationBO.FromDate, leaveInformationBO.ToDate);
                if (employeeLeave.Count > 0)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("You have leave in this date range already.", AlertType.Warning);
                }
                else
                {

                    Boolean status = false;

                    if (string.IsNullOrWhiteSpace(leaveId))
                    {
                        int tmpUserInfoId = 0;
                        leaveInformationBO.CreatedBy = userInformationBO.UserInfoId;
                        status = leaveInformationDA.SaveEmpLeaveInformation(leaveInformationBO, out tmpUserInfoId);
                        if (status)
                        {
                            rtninf.IsSuccess = true;
                            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LeaveInformation.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveInformation));
                        }
                    }
                    else
                    {
                        leaveInformationBO.LeaveId = Convert.ToInt32(leaveId);
                        leaveInformationBO.LastModifiedBy = userInformationBO.UserInfoId;
                        status = leaveInformationDA.UpdateUserInformation(leaveInformationBO);
                        if (status)
                        {
                            rtninf.IsSuccess = true;
                            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LeaveInformation.ToString(), leaveInformationBO.LeaveId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveInformation));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }


            return rtninf;
        }
        [WebMethod]
        public static LeaveInformationBO FillForm(int EditId)
        {
            LeaveInformationBO bo = new LeaveInformationBO();
            LeaveInformationDA da = new LeaveInformationDA();

            bo = da.GetEmpLeaveInformationById(EditId);

            return bo;
        }
        [WebMethod]
        public static List<LeaveTakenNBalanceBO> GetLeaveTakenNBalanceByEmployee(int empId)
        {
            List<LeaveTakenNBalanceBO> lists = new List<LeaveTakenNBalanceBO>();

            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            lists = leaveDa.GetLeaveTakenNBalanceByEmployee(empId, DateTime.Now);

            return lists;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int empId)
        {
            HMUtility hmUtility = new HMUtility();
            string message = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpLeaveInformation", "LeaveId", empId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.LeaveInformation.ToString(), empId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveInformation));
                }
            }
            catch
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo LeaveApproval(int Id, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            LeaveInformationDA DA = new LeaveInformationDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = DA.LeaveApproval(Id, approvedStatus, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    if (approvedStatus == "Checked")
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else if (approvedStatus == "Approved")
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
    }
}