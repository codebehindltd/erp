using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.UserInformation;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportDivisionRevenue : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                txtReportYear.Text = "2013";
                string CurrentYear = DateTime.Today.Year.ToString();
                txtReportDurationName.Text = "Yearly";
                txtReportFor.Text = "DivisionRevenue";
                LoadFiscalYear();
                LoadRevenueDivisionInfo();
            }
        }
        private void LoadFiscalYear()
        {
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            fiscalYearList = fiscalYearDA.GetAllFiscalYear();

            ddlFiscalYear.DataSource = fiscalYearList;
            ddlFiscalYear.DataTextField = "FiscalYearName";
            ddlFiscalYear.DataValueField = "FiscalYearId";
            ddlFiscalYear.DataBind();

            System.Web.UI.WebControls.ListItem itemProject = new System.Web.UI.WebControls.ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            ddlFiscalYear.Items.Insert(0, itemProject);
        }
        private void LoadRevenueDivisionInfo()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetRevenueDivisionInformation();
            ddlServiceName.DataSource = fields;
            ddlServiceName.DataTextField = "ServiceName";
            ddlServiceName.DataValueField = "ServiceName";
            ddlServiceName.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            _RoomStatusInfoByDate = 1;
            int fiscalYearId = Convert.ToInt32(ddlFiscalYear.SelectedValue);
            string monthName = ddlMonth.SelectedValue.ToString();
            string reportType = ddlReportType.SelectedValue.ToString();
            string ReportFor = "DivisionRevenue";
            string reportDurationName = "";
            if (reportType == "Yearly")
            {
                reportDurationName = "Yearly";
            }
            else
            {
                reportDurationName = monthName;
            }
            //txtReportDurationName.Text = ReportDurationName;
            //txtReportYear.Text = ReportYear;
            txtReportFor.Text = ReportFor;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            string serviceName = ddlServiceName.SelectedValue.ToString();

            var reportPath = "";
            if (serviceName == "--- All ---")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptDivisionRevenueMonthWise.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptDivisionRevenue.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramReport = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            paramReport.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
          
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);
            string reportFor = ReportFor;            

            AllReportDA reportDA = new AllReportDA();
            List<RoomSalesBCReportViewBO> RoomSalesBCBO = new List<RoomSalesBCReportViewBO>();
            RoomSalesBCBO = reportDA.GetRoomSalesBCInfo(fiscalYearId, reportDurationName, reportFor, serviceName);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], RoomSalesBCBO));

            rvTransaction.LocalReport.DisplayName = "Division Revenue (Bar Chart)";
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