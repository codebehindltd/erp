using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportSalesOrderInformation : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadAllCostCentreTabInfo();
                this.LoadProductInfo();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            int costCenterId = 0, productId= 0;
            string sONumber = null;

            if (string.IsNullOrEmpty(txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtFromDate.Text;
            }
            if (string.IsNullOrEmpty(txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            sONumber = txtSPONumber.Text;
            string salesOrderStatus = ddlStatus.SelectedValue;

            costCenterId = Convert.ToInt32(ddlSrcCostCenter.SelectedValue);
            productId = Convert.ToInt32(ddlProductId.SelectedValue);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptSalesOrderInformation.rdlc");

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

            PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> allOrderList = new List<PMPurchaseOrderBO>();
            allOrderList = detalisDA.GetSMSalesOrderInformation(FromDate, ToDate, sONumber, costCenterId, productId, ddlStatus.SelectedValue);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], allOrderList));

            rvTransaction.LocalReport.DisplayName = "Sales Order Information";
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
        private void LoadAllCostCentreTabInfo()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            costCentreTabBOList = entityDA.GetAllRestaurantTypeCostCentreTabInfo();

            ListItem item2 = new ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstAllValue();

            this.ddlSrcCostCenter.DataSource = costCentreTabBOList;
            this.ddlSrcCostCenter.DataTextField = "CostCenter";
            this.ddlSrcCostCenter.DataValueField = "CostCenterId";
            this.ddlSrcCostCenter.DataBind();
            this.ddlSrcCostCenter.Items.Insert(0, item2);
        }
        private void LoadProductInfo()
        {
            InvItemDA productDA = new InvItemDA();
            ddlProductId.DataSource = productDA.GetInvItemInfo();
            ddlProductId.DataTextField = "CodeAndName";
            ddlProductId.DataValueField = "ItemId";

            ddlProductId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlProductId.Items.Insert(0, item);
        }
    }
}