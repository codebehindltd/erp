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
    public partial class frmEmpMonthlyAttendance : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
            }
            CheckPermission();
        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        [WebMethod]
        public static ReturnInfo SaveLeaveInformation(int employeeID, DateTime fromDate, DateTime toDate, List<EmpAttendanceBO> Attendance, List<LeaveInformationBO> LeaveInformation)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            var monthlyAttendance = Attendance.GroupBy(x => x.AttendanceDate).Select(x => x.First()).ToList();

            var attendateWithoutLeave = (from ma in monthlyAttendance
                                         where !(from liv in LeaveInformation
                                                 select liv.FromDate).Contains(ma.AttendanceDate)
                                         select ma).ToList();

            try
            {
                string message = string.Empty;

                HMUtility hmUtility = new HMUtility();
                LeaveInformationBO leaveInformationBO = new LeaveInformationBO();
                LeaveInformationDA leaveInformationDA = new LeaveInformationDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //leaveInformationBO.EmpId = employeeID;
                //leaveInformationBO.LeaveMode = leaveMode;
                //leaveInformationBO.LeaveTypeId = leaveType;
                //leaveInformationBO.FromDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
                //leaveInformationBO.ToDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
                //leaveInformationBO.NoOfDays = numberOfDays;
                //leaveInformationBO.LeaveStatus = leaveStatus;
                //leaveInformationBO.ReportingTo = 0;

                //List<LeaveInformationBO> employeeLeave = leaveInformationDA.GetEmpDuplicateLeaveInformation(employeeID, leaveInformationBO.FromDate, leaveInformationBO.ToDate);
                //if (employeeLeave.Count > 0)
                //{
                //    rtninf.IsSuccess = false;
                //    rtninf.AlertMessage = CommonHelper.AlertInfo("You have leave in this date range already.", AlertType.Warning);
                //}
                //else
                //{

                //    Boolean status = false;

                //    if (string.IsNullOrWhiteSpace(leaveId))
                //    {
                //        int tmpUserInfoId = 0;
                //        leaveInformationBO.CreatedBy = userInformationBO.UserInfoId;
                int tmpLeaveId;
                status = leaveInformationDA.SaveMonthlyLeaveAndAttendance(attendateWithoutLeave, LeaveInformation, userInformationBO.UserInfoId, employeeID, fromDate, toDate, out tmpLeaveId);

                if (status)
                {
                    rtninf.IsSuccess = true;
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.LeaveInformation.ToString(), tmpLeaveId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LeaveInformation));

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                   
                }
                //    }
                //    else
                //    {
                //        leaveInformationBO.LeaveId = Convert.ToInt32(leaveId);
                //        leaveInformationBO.LastModifiedBy = userInformationBO.UserInfoId;
                //        status = leaveInformationDA.UpdateUserInformation(leaveInformationBO);
                //        if (status)
                //        {
                //            rtninf.IsSuccess = true;
                //            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                //        }
                //    }
                //}
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
        public static string GetLeaveTakenNBalanceByEmployee(int employeeId, DateTime dateFrom, DateTime dateTo)
        {
            HMUtility hmUtility = new HMUtility();
            EmpAttendanceDA attendanaceDa = new EmpAttendanceDA();
            LeaveInformationDA leaveDa = new LeaveInformationDA();
            List<LeaveInformationBO> leaveInformation = new List<LeaveInformationBO>();
            List<LeaveInformationBO> leave = new List<LeaveInformationBO>();
            List<EmpAttendanceBO> attendence = new List<EmpAttendanceBO>();
            DateTime attendanceDate = new DateTime();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            leave = leaveDa.GetEmployeeWiseLeaveBalance(employeeId);

            leaveInformation = leaveDa.GetLeaveInformationByEmpIdAndDateRange(employeeId, dateFrom, dateTo);
            attendence = attendanaceDa.GetAttendenceByEmpIdAndDateRange(employeeId, dateFrom, dateTo);

            string table = string.Empty, thead = string.Empty, tbody = string.Empty, check = "checked = 'checked'";
            int d = 0, row = 0, dayFrom = dateFrom.Day, dayTo = dateTo.Day, year = dateFrom.Year, month = dateFrom.Month;

            thead = @"<th style='width:125px;'>Description</th>";
            thead += @"<th style='width:18px;'>Leave Bal.</th>";

            for (d = dayFrom; d <= dayTo; d++)
            {
                thead += string.Format("<th style='width:15px;'>{0}</th>", d.ToString().PadLeft(2, '0'));
            }

            for (row = 0; row < leave.Count; row++)
            {
                tbody += "<tr>";
                tbody += string.Format("<td style='display:none;'>{0}</td>", 0); //Leave Info Id
                tbody += string.Format("<td style='display:none;'>{0}</td>", 0); //Attendance Id
                tbody += string.Format("<td style='display:none;'>{0}</td>", leave[row].LeaveTypeId);
                tbody += string.Format(@"<td style='display:none;'>{0}</td> ", (month.ToString() + "/" + (row + 1).ToString() + "/" + year.ToString()));
                tbody += string.Format("<td style='display:none;'>{0}</td>", (((dateTo - dateFrom).TotalDays) + 1));
                tbody += string.Format("<td style='display:none;'>{0}</td>", leave[row].LeaveModeId);
                tbody += string.Format("<td style='display:none;'>{0}</td>", leave[row].LeaveMode);

                tbody += string.Format("<td style='width:125px;'>{0}</td>", leave[row].TypeName);
                tbody += string.Format("<td style='width:15px;'>{0}</td>", leave[row].OpeningLeave.ToString("0"));

                for (d = dayFrom; d <= dayTo; d++)
                {
                    //attendanceDate = Convert.ToDateTime(month.ToString() + "/" + d.ToString() + "/" + year.ToString());

                    attendanceDate = dateFrom.AddDays(d-1);

                    var attendanceAlreadyGiven = (from a in attendence where a.AttendanceDate == attendanceDate select a).FirstOrDefault();
                    var leaveAlreadytaken = (from a in leaveInformation where a.FromDate == attendanceDate && a.LeaveTypeId == leave[row].LeaveTypeId select a).FirstOrDefault();

                    tbody += @"<td style='width:15px;'> <span style='display:none;'>" +
                                (month.ToString() + "/" + d.ToString() + "/" + year.ToString()) +
                                "</span>";

                    //tbody += "<td style='width:15px;'>";

                    //if (attendanceAlreadyGiven != null && leaveAlreadytaken == null)
                    //{
                    //    tbody += @"<span style='display:none;'>" +
                    //            (month.ToString() + "/" + d.ToString() + "/" + year.ToString()) +
                    //             "," + attendanceAlreadyGiven.AttendanceId + "," + 0 +
                    //            "</span>";
                    //}
                    //else if (attendanceAlreadyGiven == null && leaveAlreadytaken != null)
                    //{
                    //    tbody += @"<span style='display:none;'>" +
                    //            (month.ToString() + "/" + d.ToString() + "/" + year.ToString()) +
                    //             "," + 0 + "," + leaveAlreadytaken.LeaveId +
                    //            "</span>";
                    //}
                    //else
                    //{
                    //    tbody += @"<span style='display:none;'>" +
                    //            (month.ToString() + "/" + d.ToString() + "/" + year.ToString()) +
                    //            "," + 0 + "," + 0 +
                    //            "</span>";
                    //}

                    if (attendence.Count > 0)
                    {
                        if (attendanceAlreadyGiven != null && leaveAlreadytaken == null)
                            tbody += string.Format("<input type='checkbox' id='cb{0}' {1} />", leave[row].LeaveTypeId.ToString(), check);
                        else if (attendanceAlreadyGiven == null && leaveAlreadytaken != null)
                            tbody += string.Format("<input type='checkbox' id='cb{0}' {1} />", leave[row].LeaveTypeId.ToString(), string.Empty);
                        else
                            tbody += string.Format("<input type='checkbox' id='cb{0}' {1} />", leave[row].LeaveTypeId.ToString(), check);
                    }
                    else
                    {
                        tbody += string.Format("<input type='checkbox' id='cb{0}' {1} />", leave[row].LeaveTypeId.ToString(), check);
                    }

                    tbody += "</td>";
                }

                tbody += "</tr>";
            }

            table += string.Format(@"<table id='LeaveInfoTable' class='table table-bordered table-condensed table-hover table-responsive'>
                        <thead>
                            <tr>
                                {0}
                            </tr>
                        </thead>
                        <tbody>                           
                             {1}                          
                        </tbody>
                    </table>", thead, tbody);


            return table;
        }

    }
}