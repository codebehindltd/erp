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
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmCurrencyTransaction : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        public int isMessageBoxEnable = -1;
        protected int _EntryPanelVisibleFalse = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMessageBoxEnable = -1;
                LoadReceivedBy();
                LoadCurrency();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _EntryPanelVisibleFalse = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            int? currencyType = null, receivedBy = null;
            string costCenter = string.Empty, paymentMode = string.Empty;

            if (string.IsNullOrWhiteSpace(txtPaymentDateFrom.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtPaymentDateFrom.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtPaymentDateFrom.Text;
            }
            if (string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtPaymentDateTo.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtPaymentDateTo.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            if (ddlCurrency.SelectedValue != string.Empty)
            {
                currencyType = Convert.ToInt32(ddlCurrency.SelectedValue);
            }

            if (ddlReceivedBy.SelectedValue != string.Empty)
            {
                receivedBy = Convert.ToInt32(ddlReceivedBy.SelectedValue);
            }

            costCenter = ddlCostCenter.SelectedValue;
            paymentMode = ddlPaymentMode.SelectedValue;

            var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptCurrencyTransaction.rdlc");

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
            rvTransaction.LocalReport.EnableExternalImages = true;

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(reportParam);

            string tansactionNumber = txtTransactionNumber.Text;

            GuestPaymentDA guestPaymentDA = new GuestPaymentDA();
            List<CurrencyTransactionBO> currencyTranscation = new List<CurrencyTransactionBO>();
            currencyTranscation = guestPaymentDA.GetCurrencyTransaction(FromDate, ToDate, tansactionNumber, currencyType, paymentMode, receivedBy, costCenter);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], currencyTranscation));

            rvTransaction.LocalReport.DisplayName = "Currency Transaction";
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
        //************************ User Defined Function ********************//
        private void LoadReceivedBy()
        {
            UserInformationDA userDa = new UserInformationDA();
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();

            userInformationList = userDa.GetUserInformation();

            this.ddlReceivedBy.DataSource = userInformationList;
            this.ddlReceivedBy.DataTextField = "UserName";
            this.ddlReceivedBy.DataValueField = "UserInfoId";
            this.ddlReceivedBy.DataBind();

            this.ddlReceivedBy.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = string.Empty });
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCurrency.Items.Insert(0, item);
        }
    }
}