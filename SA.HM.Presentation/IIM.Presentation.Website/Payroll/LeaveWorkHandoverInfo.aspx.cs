using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class LeaveWorkHandoverInfo : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                if (!isValidEmployee())
                {
                    return;
                }
                LoadEmployeeInfo();
                LoadLeaveType();
                LoadLeaveMode();
                CheckObjectPermission();
            }
        }
        public bool isValidEmployee()
        {
            bool status = true;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.EmpId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Your Employee is not mapped with software user, Please contact with admin.", AlertType.Warning);
                status = false;
            }
            return status;
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDa.GetEmployeeInfo();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstAllValue();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            Boolean IsAdminUser = false;
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            if (IsAdminUser)
            {
                hfIsAdminUser.Value = "1";
                ddlEmployee.DataSource = empList;
                ddlEmployee.DataTextField = "DisplayName";
                ddlEmployee.DataValueField = "EmpId";
                ddlEmployee.DataBind();

                ddlApplicationEmployee.Enabled = true;
                hfIsEmpListVisible.Value = "1";
                ddlApplicationEmployee.DataSource = empList;
                ddlApplicationEmployee.DataTextField = "DisplayName";
                ddlApplicationEmployee.DataValueField = "EmpId";
                ddlApplicationEmployee.DataBind();
                ddlApplicationEmployee.Items.Insert(0, FirstItem);
            }
            else
            {
                hfIsAdminUser.Value = "0";
                ddlApplicationEmployee.Enabled = false;
                if (userInformationBO.EmpId > 0)
                {
                    hfIsEmpListVisible.Value = "0";

                    List<EmployeeBO> empListInfo = new List<EmployeeBO>();

                    empListInfo.AddRange(empList.Where(x => x.EmpId == userInformationBO.EmpId).ToList());
                    empListInfo.AddRange(empList.Where(x => x.RepotingTo == userInformationBO.EmpId).ToList());
                    empListInfo.AddRange(empList.Where(x => x.RepotingTo2 == userInformationBO.EmpId).ToList());

                    empList = empListInfo;
                    ddlEmployee.DataSource = empList;
                    ddlEmployee.DataTextField = "DisplayName";
                    ddlEmployee.DataValueField = "EmpId";
                    ddlEmployee.DataBind();

                    ddlApplicationEmployee.DataSource = empList;
                    ddlApplicationEmployee.DataTextField = "DisplayName";
                    ddlApplicationEmployee.DataValueField = "EmpId";
                    ddlApplicationEmployee.DataBind();
                }
            }

            if (empList.Count > 1)
            {
                ddlEmployee.Items.Insert(0, FirstItem);
            }

            ddlWorkHandover.DataSource = empList;
            ddlWorkHandover.DataTextField = "DisplayName";
            ddlWorkHandover.DataValueField = "EmpId";
            ddlWorkHandover.DataBind();
            ListItem FirstItemTaskHandoverTo = new ListItem();
            FirstItemTaskHandoverTo.Value = "0";
            FirstItemTaskHandoverTo.Text = hmUtility.GetDropDownFirstValue();
            ddlWorkHandover.Items.Insert(0, FirstItemTaskHandoverTo);
        }
        private void LoadLeaveType()
        {
            LeaveTypeDA entityDA = new LeaveTypeDA();
            List<LeaveTypeBO> ActiveLeaveTypeInfoListBO = new List<LeaveTypeBO>();
            ActiveLeaveTypeInfoListBO = entityDA.GetActiveLeaveTypeInfo();

            EmployeeDA empDA = new EmployeeDA();
            EmployeeBO empBO = new EmployeeBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.EmpId > 0)
            {
                empBO = empDA.GetEmployeeInfoById(userInformationBO.EmpId);
                if (empBO != null)
                {
                    if (empBO.EmpId > 0)
                    {
                        List<LeaveTakenNBalanceBO> lists = new List<LeaveTakenNBalanceBO>();

                        EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
                        lists = leaveDa.GetLeaveTakenNBalanceByEmployee(userInformationBO.EmpId, DateTime.Now);

                        string subContent = string.Empty;

                        for (int i = 0; i < lists.Count; i++)
                        {
                            if ((lists[i].TotalTakenLeave + lists[i].RemainingLeave) == 0)
                            {
                                ActiveLeaveTypeInfoListBO = ActiveLeaveTypeInfoListBO.Where(x => x.LeaveTypeId != lists[i].LeaveTypeID).ToList();
                            }
                        }

                        if (empBO.Gender == "Male")
                        {
                            ActiveLeaveTypeInfoListBO = ActiveLeaveTypeInfoListBO.Where(x => x.LeaveTypeId != 4).ToList();
                        }
                        else if (empBO.Gender == "Female")
                        {
                            ActiveLeaveTypeInfoListBO = ActiveLeaveTypeInfoListBO.Where(x => x.LeaveTypeId != 5).ToList();
                        }

                    }
                }
            }

            ddlLeaveTypeId.DataSource = ActiveLeaveTypeInfoListBO;
            ddlLeaveTypeId.DataTextField = "TypeName";
            ddlLeaveTypeId.DataValueField = "LeaveTypeId";
            ddlLeaveTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLeaveTypeId.Items.Insert(0, item);
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
        [WebMethod]
        public static LeaveInformationBO FillForm(int EditId)
        {
            LeaveInformationBO leave = new LeaveInformationBO();
            LeaveInformationDA da = new LeaveInformationDA();

            leave = da.GetEmpLeaveInformationById(EditId);
            return leave;
        }
        [WebMethod]
        public static ReturnInfo GetLeaveApproval(int id, string action, string cencelReason)
        {
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();
            rtninf.IsSuccess = false;

            LeaveInformationBO prevBO = new LeaveInformationBO();
            LeaveInformationBO leaveInformationBO = new LeaveInformationBO();
            LeaveInformationDA leaveInformationDA = new LeaveInformationDA();
            prevBO = leaveInformationDA.GetEmpLeaveInformationById(id);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            leaveInformationBO.WorkHandoverStatus = action;
            rtninf.AlertMessage = CommonHelper.AlertInfo("Leave Work Handover Status Updated.", AlertType.Success);
            leaveInformationBO.LastModifiedBy = userInformationBO.UserInfoId;
            leaveInformationBO.LeaveId = id;
            status = leaveInformationDA.UpdateEmpLeaveWorkHandoverStatus(leaveInformationBO);
            if (status)
            {
                rtninf.IsSuccess = true;
                bool logStatus = hmUtility.CreateActivityLogEntity(action,
                            EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), id,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee Leave Work Handover Status Updated");
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                rtninf.IsSuccess = false;
            }
            return rtninf;
        }
        [WebMethod]
        public static GridViewDataNPaging<LeaveInformationBO, GridPaging> LoadLeaveInformation(int isAdminUser, string fromDate, string toDate, int empId, string searchStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<LeaveInformationBO, GridPaging> myGridData = new GridViewDataNPaging<LeaveInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            if (userInformationBO.EmpId != 0)
            {
                int totalRecords = 0;
                DateTime leaveFromDate = DateTime.Now;
                DateTime leaveToDate = DateTime.Now;
                userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

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
                    leaveToDate = leaveToDate.AddDays(30).AddSeconds(-1);
                }

                int searchEmployeeId = 0;
                if (userInformationBO.EmpId == empId)
                {
                    searchEmployeeId = userInformationBO.EmpId;
                }
                else
                {
                    searchEmployeeId = empId;
                }

                pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
                string leaveType = string.Empty, leaveMode = "0";
                LeaveInformationDA entityDA = new LeaveInformationDA();
                
                List<LeaveInformationBO> employeeLeave = new List<LeaveInformationBO>();

                employeeLeave = entityDA.GetEmpLeaveWorkHandoverInformationByEmpIdForGridPaging(isAdminUser, userInformationBO.UserInfoId, searchEmployeeId, searchStatus, leaveType, leaveMode, leaveFromDate, leaveToDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

                //List<LeaveInformationBO> employeeLeave = entityDA.GetEmpLeaveInformationByEmpIdForGridPaging(isAdminUser, userInformationBO.UserInfoId, searchEmployeeId, searchStatus, leaveType, leaveMode, leaveFromDate, leaveToDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

                myGridData.GridPagingProcessing(employeeLeave, totalRecords);
            }

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo SaveLeaveInformation(LeaveInformationBO leaveInformationBO)
        {
            ReturnInfo rtninf = new ReturnInfo();
            rtninf.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            try
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                LeaveInformationDA leaveInformationDA = new LeaveInformationDA();

                leaveInformationBO.LeaveStatus = HMConstants.ApprovalStatus.Pending.ToString();

                // // // ------User Admin Authorization BO Session Information --------------------------------
                #region User Admin Authorization
                if (userInformationBO.UserInfoId == 1)
                {
                    leaveInformationBO.LeaveStatus = HMConstants.ApprovalStatus.Approved.ToString();
                }
                else
                {
                    List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                    adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                    if (adminAuthorizationList != null)
                    {
                        if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                        {
                            leaveInformationBO.LeaveStatus = HMConstants.ApprovalStatus.Approved.ToString();
                        }
                    }
                }
                #endregion

                if (leaveInformationBO.LeaveId <= 0)//save
                {
                    int tmpUserInfoId = 0;
                    leaveInformationBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = leaveInformationDA.SaveEmpLeaveInformation(leaveInformationBO, out tmpUserInfoId);
                    if (status)
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        rtninf.IsSuccess = true;
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), tmpUserInfoId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLeaveInfo));
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        rtninf.IsSuccess = false;
                    }
                }
                else
                {
                    leaveInformationBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = leaveInformationDA.UpdateUserInformation(leaveInformationBO);
                    if (status)
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        rtninf.IsSuccess = true;
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), leaveInformationBO.LeaveId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLeaveInfo));
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtninf;
        }
    }
}