using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportChartOfAccounts : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _GeneralLedgerInfo = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GenerateReport();
            }
        }
        private void GenerateReport()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA hmGeneralLedgerDA = new NodeMatrixDA();

            _GeneralLedgerInfo = 1;
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            string companyWeb = string.Empty;


            if (files != null)
            {
                if (files.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        companyWeb = files[0].WebAddress;
                    }
                    else
                    {
                        companyWeb = files[0].ContactNumber;
                    }

                    rvTransaction.LocalReport.DataSources.Clear();
                    rvTransaction.ProcessingMode = ProcessingMode.Local;

                    var reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptChartOfAccounts.rdlc");

                    if (!File.Exists(reportPath))
                        return;

                    rvTransaction.LocalReport.ReportPath = reportPath;

                    //-- Company Logo -------------------------------
                    string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                    rvTransaction.LocalReport.EnableExternalImages = true;

                    DateTime currentDate = DateTime.Now;
                    string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                    string footerPoweredByInfo = string.Empty;
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;                   

                    List<ReportParameter> paramLogo = new List<ReportParameter>();
                    paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                    paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                    paramLogo.Add(new ReportParameter("PrintDateTime", printDate));

                    rvTransaction.LocalReport.SetParameters(paramLogo);
                    //-- Company Logo ------------------End----------

                    List<NodeMatrixBO> nodeMatrixList = new List<NodeMatrixBO>();
                    nodeMatrixList = hmGeneralLedgerDA.GetGetChartOfAccounts(files[0].CompanyName, files[0].CompanyAddress, companyWeb);

                    var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], nodeMatrixList));


                    //TransactionDataSource.SelectParameters[0].DefaultValue = this.txtCompanyName.Text;
                    //TransactionDataSource.SelectParameters[1].DefaultValue = this.txtCompanyAddress.Text;
                    //TransactionDataSource.SelectParameters[2].DefaultValue = this.txtCompanyWeb.Text;

                    rvTransaction.LocalReport.DisplayName = "Chart Of Accounts";

                    rvTransaction.LocalReport.Refresh();
                }
            }
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