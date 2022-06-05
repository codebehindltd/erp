using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmAccountManager : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadDepartment();
            }
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            ddlDepartmentId.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = "0" });
        }
        private void Cancel()
        {
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        [WebMethod]
        public static string GetEmployeeByDepartmentWise(int departmentId)
        {
            string grid = string.Empty, alternateColor = "", isChecked = "checked= 'checked'";
            int rowCounter = 0, acountManagerId = 0;

            EmployeeDA gradeDa = new EmployeeDA();
            AccountManagerDA managerDa = new AccountManagerDA();

            List<EmployeeBO> employee = new List<EmployeeBO>();
            List<AccountManagerBO> managerList = new List<AccountManagerBO>();

            employee = gradeDa.GetEmployeeByDepartment(departmentId);
            managerList = managerDa.GetAccountManager(departmentId);

            grid = "<table id='EmployeeWiseList' style=\"width: 100%;\" class=\"table table-bordered table-condensed table-responsive\">" +
                        "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold; text-align:left; \">" +
                                "<th style=\"width: 10%;\">" +
                                    "Select" +
                                "</th>" +
                                "<th style=\"width: 20%;\">" +
                                    "Employee Code" +
                                "</th>" +
                                "<th style=\"width: 45%;\">" +
                                    "Employee Name" +
                                "</th>" +
                                 "<th style=\"width: 25%;\">" +
                                    "Designation" +
                                "</th>" +
                            "</tr>" +
                        "</thead>" +
                        "<tbody>";

            foreach (EmployeeBO sh in employee)
            {
                var am = (from m in managerList where m.EmpId == sh.EmpId select m).FirstOrDefault();

                if (am != null)
                {
                    acountManagerId = am.AccountManagerId;
                }

                rowCounter++;
                if (rowCounter % 2 == 0)
                {
                    alternateColor = "style=\"background-color:#E3EAEB;\"";
                }
                else
                    alternateColor = "style=\"background-color:#FFFFFF;\"";

                grid += "<tr " + alternateColor + ">" +
                             "<td style=\"display:none;\">" + sh.EmpId + "</td>" +
                             "<td style=\"display:none;\">" + acountManagerId + "</td>" +
                             "<td style=\"width: 10%;\">" +
                                "<input type='checkbox' id='emp" + sh.EmpId + "' " + (acountManagerId != 0 ? isChecked : string.Empty) + " />" +
                            "</td>" +
                            "<td style=\"width: 20%;\">" +
                                sh.EmpCode +
                            "</td>" +
                             "<td style=\"width: 45%;\">" +
                                sh.FullName +
                            "</td>" +
                             "<td style=\"width: 25%;\">" +
                                sh.Designation +
                            "</td>" +
                        "</tr>";

                acountManagerId = 0;
            }
            grid += " </tbody> </table>";

            return grid;

        }

        [WebMethod]
        public static ReturnInfo SaveUpdateAccountManager(List<AccountManagerBO> addedManager, List<AccountManagerBO> deletedManager)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                AccountManagerDA salaryDa = new AccountManagerDA();
                status = salaryDa.SaveUpdateAccountManager(addedManager, deletedManager, userInformationBO.UserInfoId);

                if (status == true)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + ", " + ex.InnerException.ToString(), AlertType.Error);

            }

            return rtninf;
        }
    }
}