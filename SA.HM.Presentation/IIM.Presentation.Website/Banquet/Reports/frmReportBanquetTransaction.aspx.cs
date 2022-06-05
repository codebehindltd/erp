using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Banquet.Reports
{
    public partial class frmReportBanquetTransaction : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                this.LoadCurrentDate();
                //this.LoadServiceId();
                this.LoadUserInformation();
                this.LoadServiceInfo();
            }
        }

        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtServiceFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtServiceToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }

        private bool isFormValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(txtServiceFromDate.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide From Date.";
                txtServiceFromDate.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtServiceToDate.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide To Date.";
                txtServiceToDate.Focus();
                status = false;
            }
            return status;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            if (!isFormValid())
            {
                return;
            }
            string serviceIdList = string.Empty, serviceNameList = string.Empty;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/rptBanquetTransaction.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            List<ReportParameter> paramReport = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            paramReport.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            string searchDate = "Date From: " + txtServiceFromDate.Text + " To: " + txtServiceToDate.Text;
            paramReport.Add(new ReportParameter("SearchDate", searchDate));
            //paramReport.Add(new ReportParameter("SearchDate", hmUtility.GetSearchDate(hmUtility.GetDateTimeFromString(txtServiceDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat))));

            string reportName = "Banquet Transaction Report";
            if (!string.IsNullOrEmpty(hfServiceIdList.Value))
                serviceIdList = hfServiceIdList.Value;

            if (string.IsNullOrWhiteSpace(serviceIdList))
            {
                serviceIdList = "-1";
                reportName = "Banquet Transaction Report: All Service";
            }
            else
            {
                reportName = "Banquet Transaction Report: " + hfServiceNameList.Value + "";
            }

            serviceNameList = hfServiceNameList.Value;

            if (this.ddlPaymentMode.SelectedValue == "All")
            {
                reportName = reportName + " (All Payment Type)";
            }
            else
            {
                if (this.ddlPaymentMode.SelectedValue == "Other Room")
                {
                    reportName = reportName + " (Guest Room)";
                }
                else
                {
                    reportName = reportName + " (" + this.ddlPaymentMode.SelectedValue + ")";
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            DateTime serviceFromDate = hmUtility.GetDateTimeFromString(txtServiceFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime serviceToDate = hmUtility.GetDateTimeFromString(txtServiceToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);


            hfIndividualModuleName.Value = "BanquetModule";
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);

            
            string paymentMode = ddlPaymentMode.SelectedValue;
            //int serviceId = Convert.ToInt32(ddlServiceId.SelectedValue);
           
            int receivedBy = Convert.ToInt32(ddlReceivedBy.SelectedValue);
            string moduleName = hfIndividualModuleName.Value;

            RestaurantSalesTransactionDA salesTransactionDA = new RestaurantSalesTransactionDA();
            List<Entity.Banquet.SalesTransactionReportViewBO> salesList = new List<Entity.Banquet.SalesTransactionReportViewBO>();
            salesList = salesTransactionDA.GetBanquetTransactionInfoForReport( paymentMode, serviceIdList, serviceFromDate, serviceToDate, receivedBy, moduleName);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesList));
            
            rvTransaction.LocalReport.DisplayName = "Sales Transaction";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        
        
        
        private void LoadUserInformation()
        {
            UserInformationDA entityDA = new UserInformationDA();
            this.ddlReceivedBy.DataSource = entityDA.GetCashierOrWaiterInformation();
            this.ddlReceivedBy.DataTextField = "UserName";
            this.ddlReceivedBy.DataValueField = "UserInfoId";
            this.ddlReceivedBy.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = "--- All ---";
            this.ddlReceivedBy.Items.Insert(0, item);
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());
            frmPrint.Attributes["src"] = reportSource;
        }

        private void LoadServiceInfo()
        {

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            list = costCentreTabDA.GetCostCentreTabInfoByType("Banquet");

            string grid = GetHTMLServiceGridView(list);
            ltServiceInformation.Text = grid;
        }
        public static string GetHTMLServiceGridView(List<CostCentreTabBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='text-align:center' scope='col' style='width:80px;' >Select</th><th align='left' scope='col'>Service</th></tr>";

            int counter = 0;
            foreach (CostCentreTabBO dr in List)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='center'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.CostCenterId + "' name='" + dr.CostCenter.Replace("'", "&#39;") + "' value='" + dr.CostCenterId + "' >";
                strTable += "</td><td align='left'>" + dr.CostCenter + "</td></tr>";
            }

            strTable += "</td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmReportBanquetTransaction.aspx");
        }
    }
}