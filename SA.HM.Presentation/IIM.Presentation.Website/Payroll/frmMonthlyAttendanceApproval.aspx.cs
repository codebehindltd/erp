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
    public partial class frmMonthlyAttendanceApproval : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadMonth();
                LoadYearList();
            }
        }

        private void LoadMonth()
        {
            List<MonthYearBO> monthyear = new List<MonthYearBO>();
            monthyear = CommonHelper.MonthGeneration("MMMM");

            ddlReportMonth.DataSource = monthyear;
            ddlReportMonth.DataTextField = "MonthName";
            ddlReportMonth.DataValueField = "MonthId";
            ddlReportMonth.DataBind();

            ListItem itemDonor = new ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstValue();
            ddlReportMonth.Items.Insert(0, itemDonor);

        }
        private void LoadYearList()
        {
            List<string> fields = new List<string>();
            fields = hmUtility.GetReportYearList();

            ddlYear.DataSource = fields;
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlYear.Items.Insert(0, item);
        }

        [WebMethod]
        public static ReturnInfo SaveLeaveAndAttendanceInformation(List<EmpAttendanceBO> AttendanceEntry, List<EmpAttendanceBO> AttendanceUpdate, List<LeaveInformationBO> LeaveInformation)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            
            try
            {
                string message = string.Empty;

                HMUtility hmUtility = new HMUtility();
                LeaveInformationBO leaveInformationBO = new LeaveInformationBO();
                LeaveInformationDA leaveInformationDA = new LeaveInformationDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                
                status = leaveInformationDA.SaveLeaveAndAttendance(AttendanceEntry, AttendanceUpdate, LeaveInformation, userInformationBO.UserInfoId);

                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else {
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

                    attendanceDate = dateFrom.AddDays(d - 1);

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

        [WebMethod]
        public static AttendanceAndLeaveApprovalViewBO GetAttendanceAndLeaveApproval(int empId, int monthId, int yearId)
        {
            AttendanceAndLeaveApprovalViewBO bo = new AttendanceAndLeaveApprovalViewBO();
            LeaveTypeDA leaveDa = new LeaveTypeDA();
            AttendanceDeviceDA attainDa = new AttendanceDeviceDA();

            HMCommonDA commonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime time = DateTime.Now;
            int monthStartIndex = 0, monthEndedIndex = 0;
            DateTime firstDayOfMonth = DateTime.Now, lastDayOfMonth = DateTime.Now;

            firstDayOfMonth = new DateTime(yearId, monthId, 1);
            lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            monthStartIndex = firstDayOfMonth.Day;
            monthEndedIndex = lastDayOfMonth.Day;

            bo.AttendanceAndLeaveApproval = attainDa.GetAttendanceAndLeaveApproval(empId, monthId, monthStartIndex, monthEndedIndex, firstDayOfMonth);
            bo.LeaveType = leaveDa.GetLeaveTypeInfo();
            bo.LeaveMode = commonDA.GetCustomField("LeaveMode");

            foreach (AttendanceAndLeaveApprovalBO al in bo.AttendanceAndLeaveApproval)
            {
                al.AttendanceDateStr = Convert.ToDateTime(al.AttendanceDate).ToString("MM/dd/yyyy");

                if (al.InTime != null)
                {
                    time = DateTime.Today.Add((TimeSpan)al.InTime);
                    al.InTimeStr = time.ToString(userInformationBO.TimeFormat);
                }

                if (al.OutTime != null)
                {
                    time = DateTime.Today.Add((TimeSpan)al.OutTime);
                    al.OutTimeStr = time.ToString(userInformationBO.TimeFormat);
                }
            }

            return bo;
        }        
    }
}