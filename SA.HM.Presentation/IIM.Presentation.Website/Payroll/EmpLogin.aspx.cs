using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.UserInformation;
using System.Web.Services;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class EmpLogin : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                pnlLogin.Visible = true;
                pnlEmpAttendance.Visible = false;
                pLogIn.InnerText = "Log In";
            }

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            EmployeeBO employeeBO = new EmployeeBO();



            if (!IsValid())
            {
                return;
            }

            employeeBO = new EmployeeDA().GetEmpInformationByEmpCodeNPwd(txtEmpCode.Text, txtEmpPassword.Text);
            if (employeeBO.EmpId > 0)
            {
                Session["EmpLogin_EmployeeId"] = employeeBO.EmpId;
                pnlLogin.Visible = false;
                pnlEmpAttendance.Visible = true;

                ShowHide();


            }
            else
            {
                return;
            }


        }



        private void ShowHide()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            pLogIn.InnerText = "Attendance";
            string entryExitTime = "";
            EmpAttendanceBO empAttendanceBO = new EmpAttendanceDA().GetAttendenceByEmpCodeAndDate(txtEmpCode.Text, DateTime.Now);
            //lblAttendanceDate.Text = "Attendance Date: " + DateTime.Now.ToString("dd/MM/yyyy");
            lblAttendanceDate.Text = "Attendance Date: " + DateTime.Now.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            if (empAttendanceBO.EntryTime == DateTime.MinValue)
            {
                btnStart.Visible = true;
                btnEnd.Visible = false;
            }
            else
            {
                btnStart.Visible = false;
                lblEntryTime.Text = "Entry Time:" + Convert.ToDateTime(empAttendanceBO.EntryTime).ToString(userInformationBO.TimeFormat);

                if (empAttendanceBO.ExitTime == empAttendanceBO.EntryTime)
                    btnEnd.Visible = true;
                else
                {
                    btnEnd.Visible = false;
                    lblExitTime.Text = "Exit Time: " + empAttendanceBO.ExitTime == null ? DateTime.Now.ToString(userInformationBO.TimeFormat) : Convert.ToDateTime(empAttendanceBO.ExitTime).ToString(userInformationBO.TimeFormat);

                }
            }





            Session["EmpAttendance"] = empAttendanceBO;
        }
        //*********************** User Defined Function *******************//
        private bool IsValid()
        {
            bool status = true;
            if (txtEmpCode.Text.IndexOf('<') != -1 || txtEmpCode.Text.IndexOf('>') != -1)
            {
                isMessageBoxEnable = 1;
                this.lblMessage.Text = "potentially dangerous user name.";
                this.txtEmpCode.Focus();
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                status = false;
            }
            if (txtEmpCode.Text.Length < 0 || txtEmpCode.Text.Length > 20 || txtEmpPassword.Text.Length < 0 || txtEmpPassword.Text.Length > 20)
            {
                isMessageBoxEnable = 1;
                this.lblMessage.Text = "user name or password you entered is invalid.";
                this.txtEmpCode.Focus();
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                status = false;
            }

            return status;

        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            int tmpUserInfoId = 0;


            EmpAttendanceBO empAttendanceBO = Session["EmpAttendance"] as EmpAttendanceBO;
            int empId = (int)Session["EmpLogin_EmployeeId"];

            empAttendanceBO.EmpId = empId;
            empAttendanceBO.AttendanceDate = DateTime.Now;
            empAttendanceBO.EntryTime = DateTime.Now;
            empAttendanceBO.CreatedBy = 1;
            empAttendanceBO.ExitTime = DateTime.Now;

            Boolean status = new EmpAttendanceDA().SaveEmpAttendenceInfo(empAttendanceBO, out tmpUserInfoId);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Attendance saved Successfully.";
                ShowHide();
            }
            else
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Attendance has not saved.";
            }
        }

        protected void btnEnd_Click(object sender, EventArgs e)
        {

            EmpAttendanceBO empAttendanceBO = Session["EmpAttendance"] as EmpAttendanceBO;

            empAttendanceBO.ExitTime = DateTime.Now;
            empAttendanceBO.LastModifiedBy = 1;
            empAttendanceBO.AttendanceDate = DateTime.Now;

            Boolean status = new EmpAttendanceDA().UpdateEmpAttendenceInfo(empAttendanceBO);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Attendance saved Successfully.";
                ShowHide();
            }
            else
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Attendance has not saved.";
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmpLogin.aspx");
        }




        /*
                [WebMethod]
                public static MessageUtility GetEmployeeAttendance(string empCode)
                {
                    string message = "";

                    HMUtility hmUtility = new HMUtility();
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    EmpOverTimeBO overTimeBO = new EmpOverTimeBO();
                    EmpOverTimeDA overTimeDA = new EmpOverTimeDA();

                    overTimeBO.EmpId = employeeId;
                    overTimeBO.OverTimeHour = overTimeHour;
                    overTimeBO.OverTimeDate = hmUtility.GetDateTimeFromString(overTimeDate);

                    if (string.IsNullOrWhiteSpace(overTimeId))
                    {
                        int tmpUserInfoId = 0;
                        overTimeBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = overTimeDA.SaveOverTimeInfo(overTimeBO, out tmpUserInfoId);
                        if (status)
                        {
                            message = CommonHelper.SaveMessage;
                        }
                    }
                    else
                    {
                        overTimeBO.OverTimeId = Convert.ToInt32(overTimeId);
                        overTimeBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = overTimeDA.UpdateOverTimeInfo(overTimeBO);
                        if (status)
                        {
                            message = CommonHelper.UpdateMessage;
                        }
                    }

                    return CommonHelper.MessageInfo(CommonHelper.MessageTpe.Success.ToString(), message);
                }
         * */

        /*
              public ActivityLogsBO GetActivityLog()
              {
                  UserInformationBO userInformationBO = new UserInformationBO();
                  userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                  ActivityLogsBO logBO = new ActivityLogsBO();
                  logBO.CreatedBy = userInformationBO.UserInfoId;
                  logBO.ActivityType = ActivityTypeEnum.ActivityType.Login.ToString();
                  logBO.EntityId = userInformationBO.UserInfoId;
                  logBO.EntityType = "UserInformation";
                  logBO.Remarks = "First Test";
                  return logBO;
              }*/

    }
}