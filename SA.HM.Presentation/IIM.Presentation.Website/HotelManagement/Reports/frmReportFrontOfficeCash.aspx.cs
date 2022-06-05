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
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportFrontOfficeCash : System.Web.UI.Page
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
                this.LoadUserInformation();
                LoadServiceInfo();
            }
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
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptFrontOfficeCash.rdlc");

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

            string reportName = "Payment Transaction Report";

            if (!string.IsNullOrEmpty(hfServiceIdList.Value))
                serviceIdList = hfServiceIdList.Value;
            if (string.IsNullOrWhiteSpace(serviceIdList))
            {
                serviceIdList = "-1";
                reportName = "Payment Transaction Report: All Service";
            }
            else
            {
                reportName = "Payment Transaction Report: " + hfServiceNameList.Value + "";
            }

            serviceNameList = hfServiceNameList.Value;

            if (this.ddlPaymentMode.SelectedValue == "All")
            {
                reportName = reportName + " (All Payment Type)";
            }
            else
            {
                reportName = reportName + " (" + this.ddlPaymentMode.SelectedValue + ")";
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);
            string paymentMode = this.ddlPaymentMode.SelectedValue;
            DateTime serviceFromDate = hmUtility.GetDateTimeFromString(txtServiceFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime serviceToDate = hmUtility.GetDateTimeFromString(txtServiceToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            int receivedBy = Convert.ToInt32(ddlReceivedBy.SelectedValue);

            string transactionType = ddlTransactionType.SelectedValue;

            AllReportDA reportDA = new AllReportDA();
            List<SalesTransactionReportViewBO> salesTransactionBO = new List<SalesTransactionReportViewBO>();
            salesTransactionBO = reportDA.GetSalesTransactionInfo(transactionType, paymentMode, serviceIdList, serviceFromDate, serviceToDate, receivedBy);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesTransactionBO));
            
            rvTransaction.LocalReport.DisplayName = "Payment Transaction";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("/HotelManagement/Reports/frmReportFrontOfficeCash.aspx");
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());
            frmPrint.Attributes["src"] = reportSource;
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
        private void LoadUserInformation()
        {
            UserInformationDA entityDA = new UserInformationDA();
            this.ddlReceivedBy.DataSource = entityDA.GetUserInformation();
            this.ddlReceivedBy.DataTextField = "UserName";
            this.ddlReceivedBy.DataValueField = "UserInfoId";
            this.ddlReceivedBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "--- All ---";
            this.ddlReceivedBy.Items.Insert(0, item);
        }
        public static string GetHTMLServiceGridView(List<HotelGuestServiceInfoBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='text-align:center' scope='col' style='width:80px;' >Select <input id = 'chkAllCostCenter' type = 'checkbox' class='' value = 'chkAllCostCenter' onclick='CheckAllCostCenter()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0; outline:none;' /></th>";
            strTable += "<th align='left' scope='col'>Service</th></tr>";
            //strTable += "<tr style='background-color:#E3EAEB;'><td align='center'> All <input id = 'chkAllCostCenter' type = 'checkbox' class='' value = 'chkAllCostCenter' onclick='CheckAllCostCenter()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td> </tr>";

            int counter = 0;
            foreach (HotelGuestServiceInfoBO dr in List)
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
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.ServiceId + "' name='" + dr.ServiceName.Replace("'", "&#39;") + "' value='" + dr.ServiceId + "' >";
                strTable += "</td><td align='left'>" + dr.ServiceName + "</td></tr>";
            }

            strTable += "</td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        private void LoadServiceInfo()
        {
            HotelGuestServiceInfoDA serviceDA = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> list = new List<HotelGuestServiceInfoBO>();
            list = serviceDA.GetAllGuestServiceInfo();

            foreach (HotelGuestServiceInfoBO row in list)
            {
                if (row.ServiceName == "Room Rent")
                {
                    row.ServiceName = "Room Transaction";
                }
            }

            string grid = GetHTMLServiceGridView(list);
            ltServiceInformation.Text = grid;
        }
    }
}