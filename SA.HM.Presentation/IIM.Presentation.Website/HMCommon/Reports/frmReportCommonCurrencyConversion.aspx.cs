using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HMCommon.Reports
{
    public partial class frmReportCommonCurrencyConversion : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                string queryStringIdList = Request.QueryString["ConversionIdList"];

                if (!string.IsNullOrEmpty(queryStringIdList))
                {
                    this.Session["CurrencyConversionIdList"] = string.Empty;
                    this.Session["CurrencyConversionIdList"] = Request.QueryString["ConversionIdList"];
                    Response.Redirect("/HMCommon/Reports/frmReportCommonCurrencyConversion.aspx");
                }

                if (this.Session["CurrencyConversionIdList"] != null)
                {
                    this.ReportProcessing();
                }
            }

        }        

        private void ReportProcessing()
        {
            if (this.Session["CurrencyConversionIdList"] != null)
            {
                this.txtConversionIdList.Text = string.Empty;
                this.txtConversionIdList.Text = this.Session["CurrencyConversionIdList"].ToString();
                string searchCriteria = this.txtConversionIdList.Text;

                HMCommonDA hmCommonDA = new HMCommonDA();

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/HMCommon/Reports/Rdlc/rptCommonCurrencyConversion.rdlc");

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
                }
                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;

                paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                paramReport.Add(new ReportParameter("PrintDate", printDate));

                rvTransaction.LocalReport.SetParameters(paramReport);

                CommonCurrencyTransactionDA cmnCurrencyConvDA = new CommonCurrencyTransactionDA();
                List<CommonCurrencyTransactionBO> cmnCurrencyConvBOList = new List<CommonCurrencyTransactionBO>();
                cmnCurrencyConvBOList = cmnCurrencyConvDA.GetCommonCurrencyConversionInfo(searchCriteria);

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], cmnCurrencyConvBOList));

                rvTransaction.LocalReport.DisplayName = "Currency Conversion Bill";
                rvTransaction.LocalReport.Refresh();
            }
        }
    }
}