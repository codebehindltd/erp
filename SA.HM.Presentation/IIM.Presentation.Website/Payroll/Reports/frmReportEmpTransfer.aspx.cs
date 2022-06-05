using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpTransfer : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartmentFrom.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentFrom.DataTextField = "Name";
            this.ddlDepartmentFrom.DataValueField = "DepartmentId";
            this.ddlDepartmentFrom.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlDepartmentFrom.Items.Insert(0, item);

            this.ddlDepartmentTo.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentTo.DataTextField = "Name";
            this.ddlDepartmentTo.DataValueField = "DepartmentId";
            this.ddlDepartmentTo.DataBind();

            this.ddlDepartmentTo.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            int empId = 0;
            int? preDepartId = null, curDepartId = null;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrEmpty(txtTransferDateFrom.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtTransferDateFrom.Text = startDate;
            }
            else
            {
                startDate = txtTransferDateFrom.Text;
            }
            if (string.IsNullOrEmpty(txtTransferDateTo.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtTransferDateTo.Text = endDate;
            }
            else
            {
                endDate = txtTransferDateTo.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            
            HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch.FindControl("hfEmployeeId");
            empId = Convert.ToInt32(hfEmployeeId.Value);
            int empType = Convert.ToInt32(ddlEmployee.SelectedValue);

            if (empId == 0 && empType == 1)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "an Employee.", AlertType.Warning);
                return;
            }
            else if(empType == 0)
            {
                empId = 0;
            }

            if (ddlDepartmentFrom.SelectedIndex != 0)
            {
                preDepartId = Convert.ToInt32(ddlDepartmentFrom.SelectedValue);
            }
            if (ddlDepartmentTo.SelectedIndex != 0)
            {
                curDepartId = Convert.ToInt32(ddlDepartmentTo.SelectedValue);
            }          
            

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/ReportEmpTransfer.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> reportParam = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(reportParam);

            EmployeeDA empDA = new EmployeeDA();
            List<EmpTransferReportViewBO> viewBO = new List<EmpTransferReportViewBO>();
            viewBO = empDA.GetEmpTransferInfoForReport(empId, preDepartId, curDepartId, FromDate, ToDate);
            //viewBO = viewBO.Where(i=>i.CurrentDepartment = ddlDepartmentTo.se)
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewBO));

            rvTransaction.LocalReport.DisplayName = "Employee Transfer Info";

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
    }
}