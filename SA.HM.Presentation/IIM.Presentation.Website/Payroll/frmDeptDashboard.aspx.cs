using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmDeptDashboard : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTrainingList();
                LoadLeaveBalance();
            }
        }

        private void LoadTrainingList()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpTrainingDA empTrainingDA = new EmpTrainingDA();
            List<EmpTrainingBO> viewList = new List<EmpTrainingBO>();
            viewList = empTrainingDA.GetTrainingListForDepartmentHead(userInformationBO.EmpId);

            gvTrainingList.DataSource = viewList;
            gvTrainingList.DataBind();
        }

        private void LoadLeaveBalance()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();


            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            List<LeaveTakenNBalanceBO> leaveInformationList = new List<LeaveTakenNBalanceBO>();
            leaveInformationList = leaveDa.GetDepartmentWiseTotalLeaveBalance(userInformationBO.EmpId);

            gvLeaveBalance.DataSource = leaveInformationList;
            gvLeaveBalance.DataBind();
        }

        [WebMethod]
        public static List<EmpTypeWiseEmpNoViewBO> LoadEmpTypeWiseEmpNo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            EmpTypeDA emptypeDA = new EmpTypeDA();
            List<EmpTypeWiseEmpNoViewBO> viewList = new List<EmpTypeWiseEmpNoViewBO>();
            viewList = emptypeDA.GetEmpTypeWiseEmp(userInformationBO.EmpId);

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
            leaveInformationList = leaveDa.GetDepartmentWiseTotalLeaveBalance(userInformationBO.EmpId);

            return leaveInformationList;
        }
    }
}