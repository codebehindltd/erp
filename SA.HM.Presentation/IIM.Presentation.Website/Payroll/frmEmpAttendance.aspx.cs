using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpAttendance : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                //this.ddlExitAMPM.SelectedValue = "PM";
                LoadDummyGridData();
                SetDefaulTime();
            }
            CheckObjectPermission();
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
            List<EmpAttendanceBO> attendanceList = new List<EmpAttendanceBO>();
            EmpAttendanceBO obj = new EmpAttendanceBO();

            obj.Remark = string.Empty;
            obj.AttendanceDate = DateTime.Now;
            obj.EntryTime = DateTime.Now;
            obj.ExitTime = DateTime.Now;
            obj.AttendanceId = 0;
            obj.EmpId = 0;
            obj.EmployeeName = string.Empty;

            attendanceList.Add(obj);

            this.gvEmpAttendance.DataSource = attendanceList;
            this.gvEmpAttendance.DataBind();
        }

        private void SetDefaulTime()
        {
            this.txtEntryHour.Text = "10:00";            
            this.txtExitHour.Text = "06:00";           
        }

        [WebMethod]
        public static ReturnInfo PerformAttendanceSaveAction(int employeeId, string attendanceId, string attendanceDate, string entryHour, string exitHour, string remarks)
        {
            string message = "";
            ReturnInfo rtninf = new ReturnInfo();

            if (entryHour == "0")
            {
                entryHour = string.Empty;
            }

            if (exitHour == "0")
            {
                exitHour = string.Empty;
            }

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpAttendanceBO attendenceBO = new EmpAttendanceBO();
            EmpAttendanceDA attendenceDA = new EmpAttendanceDA();
            //DateTime attendDate = Convert.ToDateTime(attendanceDate);
            DateTime attendDate = CommonHelper.DateTimeToMMDDYYYY(attendanceDate);
            string Today = hmUtility.GetStringFromDateTime(attendDate);

            attendenceBO.AttendanceDate = hmUtility.GetDateTimeFromString(attendanceDate, userInformationBO.ServerDateFormat);
            attendenceBO.EmpId = employeeId;
            attendenceBO.Remark = remarks;

            if (!string.IsNullOrWhiteSpace(entryHour))
            {
                attendenceBO.EntryTime = Convert.ToDateTime((attendenceBO.AttendanceDate.Date.ToString("MM/dd/yyyy") + " " + entryHour));
            }
            else
            {
                attendenceBO.EntryTime = DateTime.Now;
            }

            if (!string.IsNullOrWhiteSpace(exitHour))
            {
                attendenceBO.ExitTime = Convert.ToDateTime((attendenceBO.AttendanceDate.Date.ToString("MM/dd/yyyy") + " " + exitHour));
            }
            else
            {
                attendenceBO.ExitTime = null;
            }

            if (string.IsNullOrWhiteSpace(attendanceId))
            {
                int tmpUserInfoId = 0;
                attendenceBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = attendenceDA.SaveEmpAttendenceInfo(attendenceBO, out tmpUserInfoId);

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
                Boolean status = attendenceDA.UpdateEmpAttendenceInfo(attendenceBO);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.EmpAttendance.ToString(), attendenceBO.AttendanceId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAttendance));
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
            }

            return rtninf;
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpAttendanceBO, GridPaging> LoadEmployeeAttendance(int employeeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<EmpAttendanceBO, GridPaging> myGridData = new GridViewDataNPaging<EmpAttendanceBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            EmpAttendanceDA attendenceDA = new EmpAttendanceDA();
            List<EmpAttendanceBO> dwList = attendenceDA.GetAllAttendenceInfoByEmployeeIDForGridPaging(employeeId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            foreach (EmpAttendanceBO atn in dwList)
            {
                if (atn.EntryTime != null)
                {
                    atn.stringEntryTime = atn.stringEntryTime;
                }
                else
                {
                    atn.stringExitTime = string.Empty;
                }

                if (atn.ExitTime != null)
                {
                    atn.stringExitTime = atn.stringExitTime;
                }
                else
                {
                    atn.stringExitTime = string.Empty;
                }
            }

            myGridData.GridPagingProcessing(dwList, totalRecords);

            return myGridData;
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
                //attendenceBO.EndHour = attendenceBO.ExitTime.ToString(userInformationBO.TimeFormat);
            }
            else
            {
                attendenceBO.EndHour = string.Empty;
            }

            return attendenceBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string message = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpAttendance", "AttendanceId", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.EmpAttendance.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAttendance));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        [WebMethod]
        public static int CheckJoinDate(int empId, DateTime attendanceDate)
        {
            EmployeeBO bo = new EmployeeBO();
            EmployeeDA da = new EmployeeDA();
            bo = da.GetEmployeeInfoById(empId);
            int isInvalid = 0;
            if (attendanceDate < bo.JoinDate)
            {
                isInvalid = 1;
            }
            return isInvalid;
        }
    }
}