using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmQuotationWiseSalesReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuotation();
            }
        }

        private void LoadQuotation()
        {
            SalesQuotationNBillingDA salesQuotationDA = new SalesQuotationNBillingDA();
            ddlQuotationNumber.DataSource = salesQuotationDA.GetAllAcceptedQuotation();
            ddlQuotationNumber.DataTextField = "QuotationNo";
            ddlQuotationNumber.DataValueField = "QuotationId";
            ddlQuotationNumber.DataBind();

            ListItem Quotation = new ListItem();
            Quotation.Value = "0";
            Quotation.Text = hmUtility.GetDropDownFirstAllValue();
            ddlQuotationNumber.Items.Insert(0, Quotation);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string  reportName = "Quotation Wise Sales Report";
            
            int quotationNumber = 0;

            if (ddlQuotationNumber.SelectedValue != string.Empty)
                quotationNumber = Convert.ToInt32(ddlQuotationNumber.SelectedValue);

            rvQuotationSR.LocalReport.DataSources.Clear();
            rvQuotationSR.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptQuotationWiseSalesReport.rdlc");
            

            if (!File.Exists(reportPath))
                return;

            rvQuotationSR.LocalReport.ReportPath = reportPath;

            QuationWiseSalesListBO salesBO = new QuationWiseSalesListBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();
            salesBO = salesDA.GetQuotationWiseSalesList_SP(quotationNumber);

            List<ReportParameter> paramReport = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            

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

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvQuotationSR.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            rvQuotationSR.LocalReport.SetParameters(paramReport);
            var dataSet = rvQuotationSR.LocalReport.GetDataSourceNames();
            rvQuotationSR.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], salesBO.quotationWiseSales));
            rvQuotationSR.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], salesBO.itemDetails));
            rvQuotationSR.LocalReport.DataSources.Add(new ReportDataSource(dataSet[2], salesBO.seviceDetails));

            rvQuotationSR.LocalReport.DisplayName = reportName;
            rvQuotationSR.LocalReport.Refresh();
        }
    }
}