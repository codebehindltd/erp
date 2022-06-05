using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportEmpKotBillDetailsInfo : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                LoadEmployee();
                LoadCommonDropDownHiddenField();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string costcenterIdList = string.Empty;
            string serviceIdList = string.Empty, serviceNameList = string.Empty;

            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //ToDate = ToDate.AddDays(1).AddSeconds(-1);

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);


            int empId = Convert.ToInt32(this.ddlEmpId.SelectedValue);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            string paramReportType = string.Empty;
            string reportType = string.Empty;

            paramReportType = "SalesDetail";
            reportType = "Sales Detail";
            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptEmpKotBillDetailInfo.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string reportName = "Job Assignment and Delivery Information";

            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));


            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);

            string srcType = this.ddlSearchType.SelectedValue;
            string jobStatus = this.ddlDeliveryStatus.SelectedValue;
            RestaurantTableDA tableDA = new RestaurantTableDA();
            List<EmpKotBillDetailBO> jobList = tableDA.GetEmpKotBillDetailInformation(empId, FromDate, ToDate, srcType, jobStatus);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], jobList));

            rvTransaction.LocalReport.DisplayName = "" + reportType + " Information";
            rvTransaction.LocalReport.Refresh();

        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadEmployee()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeInfoByStatus("0");
            this.ddlEmpId.DataSource = empList;
            this.ddlEmpId.DataTextField = "DisplayName";
            this.ddlEmpId.DataValueField = "EmpId";
            this.ddlEmpId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmpId.Items.Insert(0, item);
        }

    }
}