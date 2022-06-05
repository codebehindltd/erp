using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.LCManagement;
using HotelManagement.Entity.LCManagement;
using HotelManagement.Data.PurchaseManagment;

namespace HotelManagement.Presentation.Website.LCManagement.Reports
{
    public partial class frmReportDateWiseLCInformation : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadSupplierInfo();
            }
        }
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            this.ddlSupplier.DataSource = entityDA.GetPMSupplierInfo();
            this.ddlSupplier.DataTextField = "Name";
            this.ddlSupplier.DataValueField = "SupplierId";
            this.ddlSupplier.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlSupplier.Items.Insert(0, item);
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            int? lCId = null;
            string lCType = string.Empty;
            int supplierId = 0;

            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }

            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);           

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            if (ddlReportType.SelectedValue == "LCNumber")
            {
                reportPath = Server.MapPath(@"~/LCManagement/Reports/Rdlc/rptLCDetailsInfo.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/LCManagement/Reports/Rdlc/rptDateWiseLCInfo.rdlc");
            }            

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
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

            string reportName = string.Empty;
            lCType = this.ddlReportType.SelectedValue.ToString();
            if (ddlReportType.SelectedValue == "Open")
            {
                reportName = "Date Wise LC";
            }
            else if (ddlReportType.SelectedValue == "Mature")
            {
                reportName = "Date Wise LC Mature";
            }
            else if (ddlReportType.SelectedValue == "Settle")
            {
                reportName = "Date Wise LC Settlement";
            }
            else if (ddlReportType.SelectedValue == "Supplier")
            {
                supplierId = Convert.ToInt32(this.ddlSupplier.SelectedValue.ToString());
                reportName = "Supplier Wise LC";
            }
            else if (ddlReportType.SelectedValue == "LCNumber")
            {
                reportName = "LC Details Information";
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetStringFromDateTime(currentDate) + " " + currentDate.ToString("hh:mm:ss tt");
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            LCInformationDA reqDA = new LCInformationDA();            

            if (ddlReportType.SelectedValue == "LCNumber")
            {
                List<LCReportViewBO> lcInfo = new List<LCReportViewBO>();
                lcInfo = reqDA.GetLCDetailsReportInfo("LCInformation", txtLCNumber.Text);

                List<LCReportViewBO> LCInformationDetail = new List<LCReportViewBO>();
                LCInformationDetail = reqDA.GetLCDetailsReportInfo("LCInformationDetail", txtLCNumber.Text);
                
                List<LCReportViewBO> LCPayment = new List<LCReportViewBO>();
                LCPayment = reqDA.GetLCDetailsReportInfo("LCPayment", txtLCNumber.Text);

                List<LCReportViewBO> PMProductReceived = new List<LCReportViewBO>();
                PMProductReceived = reqDA.GetLCDetailsReportInfo("PMProductReceived", txtLCNumber.Text);

                List<LCReportViewBO> LCOverHeadExpense = new List<LCReportViewBO>();
                LCOverHeadExpense = reqDA.GetLCDetailsReportInfo("LCOverHeadExpense", txtLCNumber.Text);
                                
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], LCInformationDetail));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], PMProductReceived));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], LCPayment));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[3], LCOverHeadExpense));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[4], lcInfo));
            }
            else
            {
                List<LCReportViewBO> purchaseInfo = new List<LCReportViewBO>();
                purchaseInfo = reqDA.GetLCReportInfo(FromDate, ToDate, lCId, lCType, supplierId);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], purchaseInfo));
            }
                
            rvTransaction.LocalReport.DisplayName = "" + reportName + " Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}