using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Restaurant;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportMachineTestInfo : System.Web.UI.Page
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCostCenter();
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;

            int costCenterId = Convert.ToInt32(this.ddlCostCenter.SelectedValue.ToString());

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";

            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptMachineTestInformation.rdlc");

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
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            paramReport.Add(new ReportParameter("ReportDateFrom", FromDate.ToString("dd-MMM-yyyy")));
            paramReport.Add(new ReportParameter("ReportDateTo", ToDate.ToString("dd-MMM-yyyy")));

            rvTransaction.LocalReport.SetParameters(paramReport);

            RestaurantSalesInfoDA salesDA = new RestaurantSalesInfoDA();
            List<MachineTestBO> transactionList = new List<MachineTestBO>();

            transactionList = salesDA.GetMachineTestInformation(costCenterId, FromDate, ToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], transactionList));

            rvTransaction.LocalReport.DisplayName = "Machine Test Information";
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
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo().Where(x => x.CostCenterType == "PetrolPump").ToList();
            this.ddlCostCenter.DataSource = List;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCostCenter.Items.Insert(0, item);
        }
    }
}