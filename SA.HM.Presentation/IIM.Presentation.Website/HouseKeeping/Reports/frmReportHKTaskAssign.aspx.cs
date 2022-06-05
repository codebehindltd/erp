using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Entity.Payroll;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.HouseKeeping.Reports
{
    public partial class frmReportHKTaskAssign : System.Web.UI.Page
    {
        protected int IsSuccess = -1;
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadCurrentDate();
                LoadCommonDropDownHiddenField();
            }
        }
        //protected void ddlDepartment_Change(object sender, EventArgs e)
        //{
        //    int departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
        //    LoadEmployee(departmentId);
        //}
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSuccess = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            DateTime searchDate = DateTime.Now;
            string dateTime = string.Empty;
            string shift = string.Empty;

            Int64 taskId = Convert.ToInt64(ddlTaskSequence.SelectedValue);
            shift = ddlShift.SelectedValue;

            if (!string.IsNullOrEmpty(txtSearchDate.Text))
            {
                dateTime = txtSearchDate.Text;
                searchDate = hmUtility.GetDateTimeFromString(dateTime, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                txtSearchDate.Text = hmUtility.GetStringFromDateTime(searchDate);
                dateTime = txtSearchDate.Text;
                searchDate = hmUtility.GetDateTimeFromString(dateTime, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HouseKeeping/Reports/Rdlc/rptHKTaskAssign.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string reportDate = hmUtility.GetStringFromDateTime(searchDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));

            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            paramReport.Add(new ReportParameter("EmpName", string.Empty));

            paramReport.Add(new ReportParameter("Shift", shift));
            paramReport.Add(new ReportParameter("ReportDate", reportDate));

            rvTransaction.LocalReport.SetParameters(paramReport);

            HKRoomStatusDA hkStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> viewList = new List<HKRoomStatusViewBO>();

            viewList = hkStatusDA.GetHKTaskAssignForReport(taskId, searchDate, shift);

            foreach (HKRoomStatusViewBO bo in viewList)
            {
                if (bo.FORoomStatus != "Occupied")
                {
                    bo.GuestName = "";
                }
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Task Assignment";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HotelEmpTaskAssignmentBO> taskList = new List<HotelEmpTaskAssignmentBO>();

            DateTime taskDate = DateTime.Now;

            if (txtSearchDate.Text.Trim() != string.Empty)
                taskDate = Convert.ToDateTime(CommonHelper.DateTimeToMMDDYYYY(txtSearchDate.Text));

            taskList = hkRoomStatusDA.GetTaskAssignment(ddlShift.SelectedValue, taskDate);

            ddlTaskSequence.DataSource = taskList;
            ddlTaskSequence.DataTextField = "TaskSequence";
            ddlTaskSequence.DataValueField = "TaskId";
            ddlTaskSequence.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTaskSequence.Items.Insert(0, item);
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;

            if (hfDateTime.Value != "")
                txtSearchDate.Text = hfDateTime.Value;
            else
                txtSearchDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        //private void LoadEmployee(int departmentId)
        //{
        //    EmployeeDA roomTypeDA = new EmployeeDA();
        //    var List = roomTypeDA.GetEmployeeByDepartment(departmentId);
        //    ddlEmployee.DataSource = List;
        //    ddlEmployee.DataTextField = "DisplayName";
        //    ddlEmployee.DataValueField = "EmpId";
        //    ddlEmployee.DataBind();

        //    ListItem emp = new ListItem();
        //    emp.Value = "0";
        //    emp.Text = hmUtility.GetDropDownFirstValue();
        //    ddlEmployee.Items.Insert(0, emp);
        //}
        private void LoadDepartment()
        {
            //DepartmentDA entityDA = new DepartmentDA();
            //ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            //ddlDepartment.DataTextField = "Name";
            //ddlDepartment.DataValueField = "DepartmentId";
            //ddlDepartment.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlDepartment.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        [WebMethod]
        public static List<EmployeeBO> LoadEmployee(int depId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeByDepartment(depId);

            return empList;
        }
    }
}