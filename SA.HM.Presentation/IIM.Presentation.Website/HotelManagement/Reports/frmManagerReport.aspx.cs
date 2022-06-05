using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HotelManagement;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmManagerReport : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtCompanyName.Text = string.Empty;
            this.txtCompanyAddress.Text = string.Empty;
            this.txtCompanyWeb.Text = string.Empty;
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string approvedDate = string.Empty;
            DateTime ApprovedDate = new DateTime();
            DateTime dateTime = DateTime.Now;

            var reportType = ddlReportType.SelectedValue.ToString();

            if (string.IsNullOrWhiteSpace(this.txtApprovedDate.Text))
            {
                approvedDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtApprovedDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                approvedDate = this.txtApprovedDate.Text;                
            }
            ApprovedDate = hmUtility.GetDateTimeFromString(approvedDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            //-- Company Logo -------------------------------
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptManagerReportYtd.rdlc");

            //if(ddlReportType.SelectedValue == "YTD")
            //{
            //    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptManagerReportYtd.rdlc");
            //}
            //else if (ddlReportType.SelectedValue == "MTD")
            //{
            //    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/RptSalesAuditMtd.rdlc");
            //}
            //else if (ddlReportType.SelectedValue == "Today")
            //{
            //    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/RptSalesAudit.rdlc");
            //}

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyName", files[0].CompanyName));
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

            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("ReportDate", this.txtApprovedDate.Text));
            rvTransaction.LocalReport.SetParameters(paramReport);

            SalesAuditDA salesAuditDA = new SalesAuditDA();
            List<ManagerReportBO> salesAuditPaymentBO = new List<ManagerReportBO>();
            salesAuditPaymentBO = salesAuditDA.GetManagerReportInfo(ApprovedDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesAuditPaymentBO));
            rvTransaction.LocalReport.DisplayName = "Manager Report";
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