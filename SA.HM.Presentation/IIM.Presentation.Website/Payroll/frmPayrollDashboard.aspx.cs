﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmPayrollDashboard : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LeaveChart();
            }
        }

        [WebMethod]
        public static List<LeaveTakenNBalanceBO> LoadLeave()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            DateTime reportDate = DateTime.Now;

            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            List<LeaveTakenNBalanceBO> leaveInformationList = new List<LeaveTakenNBalanceBO>();
            leaveInformationList = leaveDa.GetLeaveTakenNBalanceByEmployee(userInformationBO.EmpId, reportDate);

            return leaveInformationList;
        }

        [WebMethod]
        public static List<DailyEmpStatusViewBO> LoadAttendance()
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            //DateTime reportDate = DateTime.Now;

            EmpAttendanceDA empAttendanceDA = new EmpAttendanceDA();
            List<DailyEmpStatusViewBO> viewList = new List<DailyEmpStatusViewBO>();
            viewList = empAttendanceDA.GetEmpAttendenceForDashboard();

            return viewList;
        }

        [WebMethod]
        public static List<LeaveTakenNBalanceBO> LoadLeaveForpieChart()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            DateTime reportDate = DateTime.Now;

            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            List<LeaveTakenNBalanceBO> leaveInformationList = new List<LeaveTakenNBalanceBO>();
            leaveInformationList = leaveDa.GetLeaveTakenNBalanceByEmployee(userInformationBO.EmpId, reportDate);

            return leaveInformationList;
        }

        [WebMethod]
        public static List<EmpMonthlyAttendanceViewBO> LoadEmpMonthlyAttendance()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            EmpAttendanceDA empAttendanceDA = new EmpAttendanceDA();
            List<EmpMonthlyAttendanceViewBO> viewList = new List<EmpMonthlyAttendanceViewBO>();
            viewList = empAttendanceDA.GetEmpMonthlyAttendance(userInformationBO.EmpId);

            return viewList;
        }



        [WebMethod]
        public static List<LeaveTakenNBalanceBO> LoadDepartmentWiseLeaveBalance()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            //int departmentId = userInformationBO

            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            List<LeaveTakenNBalanceBO> leaveInformationList = new List<LeaveTakenNBalanceBO>();
            leaveInformationList = leaveDa.GetDepartmentWiseTotalLeaveBalance(5);

            return leaveInformationList;
        }
    }
}