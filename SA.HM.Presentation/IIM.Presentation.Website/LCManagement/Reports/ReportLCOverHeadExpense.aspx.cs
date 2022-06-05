using HotelManagement.Data.HMCommon;
using HotelManagement.Data.LCManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.LCManagement;
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

namespace HotelManagement.Presentation.Website.LCManagement.Reports
{
    public partial class ReportLCOverHeadExpense : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOverHeadName();
                LoadLCNumber();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {

            _RoomStatusInfoByDate = 1;

            string reportType = ddlReportType.SelectedValue;
            int expenseHead = Convert.ToInt32(ddlOverHeadId.SelectedValue);
            int lcNumber = Convert.ToInt32(ddlLCId.SelectedValue);
            string transactionType = ddlTransactionType.SelectedValue;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

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
            if (reportType == "Date Wise")
                reportPath = Server.MapPath(@"~/LCManagement/Reports/Rdlc/rptDateWiseLCOverHeadExpense.rdlc");
            else if (reportType == "OverHead Wise")
                reportPath = Server.MapPath(@"~/LCManagement/Reports/Rdlc/rptOverHeadWiseLCOverHeadExpense.rdlc");
            else if (reportType == "LC Wise")
                reportPath = Server.MapPath(@"~/LCManagement/Reports/Rdlc/rptLCNumberWiseLCOverHeadExpense.rdlc");


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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
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

            OverHeadExpenseDA DA = new OverHeadExpenseDA();
            List<OverHeadExpenseBO> expenseList = new List<OverHeadExpenseBO>();

            expenseList = DA.GetOverHeadExpenseInfoForReport(FromDate, ToDate, expenseHead, lcNumber);
            expenseList = expenseList.Where(p => p.TransactionType == transactionType || transactionType == "0").ToList();

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], expenseList));
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        private void LoadOverHeadName()
        {
            OverHeadNameDA bankDA = new OverHeadNameDA();
            List<OverHeadNameBO> entityBOList = new List<OverHeadNameBO>();
            entityBOList = bankDA.GetActiveLCOverHeadNameInfo();

            this.ddlOverHeadId.DataSource = entityBOList;
            this.ddlOverHeadId.DataTextField = "OverHeadName";
            this.ddlOverHeadId.DataValueField = "OverHeadId";
            this.ddlOverHeadId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlOverHeadId.Items.Insert(0, itemBank);
        }

        private void LoadLCNumber()
        {
            LCInformationDA bankDA = new LCInformationDA();
            List<LCInformationBO> entityBOList = new List<LCInformationBO>();
            entityBOList = bankDA.GetApprovedLCInformation();

            this.ddlLCId.DataSource = entityBOList;
            this.ddlLCId.DataTextField = "LCNumber";
            this.ddlLCId.DataValueField = "LCId";
            this.ddlLCId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlLCId.Items.Insert(0, itemBank);
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