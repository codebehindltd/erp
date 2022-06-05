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
    public partial class AttendenceInformation : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
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
                CheckObjectPermission();
            }
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
        [WebMethod]
        public static ReturnInfo MakeApproval(int id, string action, string cencelReason)
        {
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();
            rtninf.IsSuccess = false;

            EmpAttendanceBO attendanceBO = new EmpAttendanceBO();
            EmpAttendanceBO prevBO = new EmpAttendanceBO();
            EmpAttendanceDA attendenceDA = new EmpAttendanceDA();

            prevBO = attendenceDA.GetEmpAttendenceInfoById(id);
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (action == HMConstants.ApprovalStatus.Checked.ToString() && (prevBO.AttendenceStatus == "Pending" || prevBO.AttendenceStatus == "Partially Checked"))
            {
                attendanceBO.AttendenceStatus = HMConstants.ApprovalStatus.Checked.ToString();
                attendanceBO.CheckedBy = userInformationBO.UserInfoId;

                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
            }
            else if (action == HMConstants.ApprovalStatus.Approved.ToString() && (prevBO.AttendenceStatus == "Checked" || prevBO.AttendenceStatus == "Partially Approved"))
            {
                attendanceBO.AttendenceStatus = HMConstants.ApprovalStatus.Approved.ToString();
                attendanceBO.ApprovedBy = userInformationBO.UserInfoId;

                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
            }
            else if (action == HMConstants.ApprovalStatus.Cancel.ToString())
            {
                attendanceBO.AttendenceStatus = HMConstants.ApprovalStatus.Cancel.ToString();
                attendanceBO.CancelReason = cencelReason;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo("You Dont Have Permission for This", AlertType.Error);
                rtninf.IsSuccess = false;
                return rtninf;
            }

            attendanceBO.LastModifiedBy = userInformationBO.UserInfoId;
            attendanceBO.AttendanceId = id;
            status = attendenceDA.UpdateAttendanceStatus(attendanceBO);
            if (status)
            {
                rtninf.IsSuccess = true;
                bool logStatus = hmUtility.CreateActivityLogEntity(action,
                            EntityTypeEnum.EntityType.EmpAttendance.ToString(), id,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee Attendence Status Updated");
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                rtninf.IsSuccess = false;
            }
            return rtninf;
        }
        [WebMethod]
        public static EmpAttendanceBO FillForm(int EditId)
        {
            HMUtility hmUtility = new HMUtility();
            EmpAttendanceBO attendenceBO = new EmpAttendanceBO();
            EmpAttendanceDA attendenceDA = new EmpAttendanceDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            attendenceBO = attendenceDA.GetEmpAttendenceInfoById(EditId);
            attendenceBO.stringAttendenceDate = attendenceBO.AttendanceDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            attendenceBO.StartHour = attendenceBO.EntryTime.ToString(userInformationBO.TimeFormat);
            if (attendenceBO.ExitTime != null)
            {
                attendenceBO.EndHour = Convert.ToDateTime(attendenceBO.ExitTime).ToString(userInformationBO.TimeFormat);
            }
            else
            {
                attendenceBO.EndHour = string.Empty;
            }

            return attendenceBO;
        }
        [WebMethod]
        public static GridViewDataNPaging<EmpAttendanceReportBO, GridPaging> LoadAttendenceInformation(int isAdminUser, string fromDate, string toDate, int empId, string searchStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            GridViewDataNPaging<EmpAttendanceReportBO, GridPaging> myGridData = new GridViewDataNPaging<EmpAttendanceReportBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);

            int totalRecords = 0;
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            EmpAttendanceDA attendanceDA = new EmpAttendanceDA();
            DateTime rosterDateFrom = DateTime.Now, rosterDateTo = DateTime.Now;
            if (!string.IsNullOrEmpty(fromDate))
            {
                rosterDateFrom = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                rosterDateFrom = rosterDateFrom.AddDays(-1);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                rosterDateTo = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                rosterDateTo = rosterDateTo.AddDays(1).AddSeconds(-1);
            }
            List<EmpAttendanceReportBO> serviceList = new List<EmpAttendanceReportBO>();
            
            serviceList = attendanceDA.GetEmpAttendanceInfoByDateRange(isAdminUser, rosterDateFrom, rosterDateTo, userInformationBO.UserInfoId, empId, searchStatus, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            foreach (EmpAttendanceReportBO row in serviceList)
            {
                if (row.IsCanEdit == true)
                {
                    if (userInformationBO.EmpId != row.EmpId)
                    {
                        row.IsCanEdit = false;
                    }
                }
            }

            myGridData.GridPagingProcessing(serviceList, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo SaveLeaveInformation(EmpAttendanceBO attendanceBO)
        {
            ReturnInfo rtninf = new ReturnInfo();
            rtninf.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            try
            {
                EmpAttendanceDA entityDA = new EmpAttendanceDA();
                EmpAttendanceBO prevBO = new EmpAttendanceBO();


                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                if (attendanceBO.AttendanceId <= 0)//save
                {
                    int tmpUserInfoId = 0;
                    attendanceBO.EmpId = userInformationBO.EmpId;
                    attendanceBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.SaveEmpAttendenceInfo(attendanceBO, out tmpUserInfoId);
                    if (status)
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        rtninf.IsSuccess = true;
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpAttendance.ToString(), tmpUserInfoId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAttendance));
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        rtninf.IsSuccess = false;
                    }
                }
                else
                {
                    prevBO = entityDA.GetEmpAttendenceInfoById(attendanceBO.AttendanceId);
                    prevBO.LastModifiedBy = userInformationBO.UserInfoId;
                    prevBO.AttendenceStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    prevBO.Remark = attendanceBO.Remark;
                    Boolean status = entityDA.UpdateEmpAttendenceInfo(prevBO);
                    if (status)
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Application Submitted", AlertType.Success);
                        rtninf.IsSuccess = true;
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpAttendance.ToString(), attendanceBO.AttendanceId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAttendance));
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtninf;
        }
        [WebMethod]
        public static EmpAttendanceBO AttendanceApplicationFillForm(int employeeId, string attendenceDate)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DateTime attendDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(attendenceDate))
            {
                attendDate = CommonHelper.DateTimeToMMDDYYYY(attendenceDate);
            }

            EmpAttendanceBO attendenceBO = new EmpAttendanceBO();
            EmpAttendanceDA attendanceDA = new EmpAttendanceDA();
            attendenceBO = attendanceDA.GetAttendenceByEmpIdAndDate(employeeId, attendDate);
            if (attendenceBO.AttendanceId > 0)
            {
                attendenceBO.stringAttendenceDate = attendenceBO.AttendanceDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                attendenceBO.StartHour = attendenceBO.EntryTime.ToString(userInformationBO.TimeFormat);
                if (attendenceBO.ExitTime != null)
                {
                    attendenceBO.EndHour = Convert.ToDateTime(attendenceBO.ExitTime).ToString(userInformationBO.TimeFormat);
                }
                else
                {
                    attendenceBO.EndHour = string.Empty;
                }
            }

            return attendenceBO;
        }
        [WebMethod]
        public static ReturnInfo PerformAttendanceSaveAction(int employeeId, string attendanceDate, string entryHour, string exitHour, string remarks)
        {
            Int64 attendanceId = 0;
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO != null)
            {
                DateTime attendDate = CommonHelper.DateTimeToMMDDYYYY(attendanceDate);
                EmpAttendanceBO attendenceBO = new EmpAttendanceBO();
                EmpAttendanceDA attendanceDA = new EmpAttendanceDA();
                attendenceBO = attendanceDA.GetAttendenceByEmpIdAndDate(employeeId, attendDate);
                if (attendenceBO.AttendanceId > 0)
                {
                    attendanceId = attendenceBO.AttendanceId;
                }

                if (entryHour == "0")
                {
                    entryHour = string.Empty;
                }

                if (exitHour == "0")
                {
                    exitHour = string.Empty;
                }

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                string Today = hmUtility.GetStringFromDateTime(attendDate);

                attendenceBO.AttendanceDate = hmUtility.GetDateTimeFromString(attendanceDate, userInformationBO.ServerDateFormat);
                attendenceBO.EmpId = employeeId;
                attendenceBO.Remark = remarks;
                attendenceBO.AttendenceStatus = "Pending";

                if (!string.IsNullOrWhiteSpace(entryHour))
                {
                    attendenceBO.EntryTime = Convert.ToDateTime((attendenceBO.AttendanceDate.Date.ToString("MM/dd/yyyy") + " " + entryHour));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(exitHour))
                    {
                        attendenceBO.EntryTime = Convert.ToDateTime((attendenceBO.AttendanceDate.Date.ToString("MM/dd/yyyy") + " " + exitHour));
                    }
                    else
                    {
                        attendenceBO.EntryTime = DateTime.Now;
                    }
                }

                if (!string.IsNullOrWhiteSpace(exitHour))
                {
                    attendenceBO.ExitTime = Convert.ToDateTime((attendenceBO.AttendanceDate.Date.ToString("MM/dd/yyyy") + " " + exitHour));
                }
                else
                {
                    attendenceBO.ExitTime = null;
                }

                // // // ------User Admin Authorization BO Session Information --------------------------------
                #region User Admin Authorization
                if (userInformationBO.UserInfoId == 1)
                {
                    if (!string.IsNullOrWhiteSpace(entryHour) && !string.IsNullOrWhiteSpace(exitHour))
                    {
                        if (attendenceBO.EntryTime != attendenceBO.ExitTime)
                        {
                            attendenceBO.AttendenceStatus = "Approved";
                        }
                    }
                }
                else
                {
                    List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                    adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                    if (adminAuthorizationList != null)
                    {
                        if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(entryHour) && !string.IsNullOrWhiteSpace(exitHour))
                            {
                                if (attendenceBO.EntryTime != attendenceBO.ExitTime)
                                {
                                    attendenceBO.AttendenceStatus = "Approved";
                                }
                            }
                        }
                    }
                }
                #endregion

                if (attendanceId == 0)
                {
                    int tmpUserInfoId = 0;
                    attendenceBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = attendanceDA.SaveEmpAttendenceInfo(attendenceBO, out tmpUserInfoId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.EmpAttendance.ToString(), tmpUserInfoId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAttendance));
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
                    }
                }
                else
                {
                    attendenceBO.AttendanceId = Convert.ToInt32(attendanceId);
                    attendenceBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = attendanceDA.UpdateEmpAttendenceInfo(attendenceBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.EmpAttendance.ToString(), attendenceBO.AttendanceId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAttendance));
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
            }

            return rtninf;
        }
    }
}