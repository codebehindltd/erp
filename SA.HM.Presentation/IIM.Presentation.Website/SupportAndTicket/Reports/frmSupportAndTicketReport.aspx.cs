using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Entity.SupportAndTicket;

namespace HotelManagement.Presentation.Website.SupportAndTicket.Reports
{
    public partial class frmSupportAndTicketReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["TId"];
                string[] param1 = param.Split(',');
                string strTransactionId = param1[0];
                string reportType = param1[1];
                
                if (!string.IsNullOrWhiteSpace(strTransactionId) && !string.IsNullOrEmpty(reportType))
                {
                    int transactionId = Convert.ToInt32(strTransactionId);
                    if (transactionId > 0)
                    {
                        GenerateReport(transactionId, reportType);
                    }
                }
            }
        }

        private void GenerateReport(int transactionId, string reportType)
        {
            dispalyReport = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = "";
            if (reportType == "STicket")
            {
                reportPath = Server.MapPath(@"~/SupportAndTicket/Reports/Rdlc/rptSupportTicket.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();
            List<STSupportBO> supportList = new List<STSupportBO>();

            if (reportType == "STicket")
            {
                supportList = supportDA.GetSupportTicket(transactionId);
            }

            List<ReportParameter> reportParam = new List<ReportParameter>();

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    webAddress = files[0].ContactNumber;
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
            
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            reportParam.Add(new ReportParameter("CompanyProfile", companyName));
            reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
            reportParam.Add(new ReportParameter("CompanyWeb", webAddress));

            rvTransaction.LocalReport.SetParameters(reportParam);            

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], supportList));

            if (reportType == "STicket")
            {
                rvTransaction.LocalReport.DisplayName = "Support Ticket.";
            }

            rvTransaction.LocalReport.Refresh();
        }
    }
}