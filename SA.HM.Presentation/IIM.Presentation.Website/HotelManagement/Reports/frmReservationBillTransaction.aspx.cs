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
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReservationBillTransaction : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                this.LoadUserInformation();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            string paymentMode = string.Empty, paymentType = string.Empty, receivedBy = string.Empty;
            string reportName = "Reservation Transaction";

            if (this.ddlPaymentMode.SelectedValue == "All" && ddlPaymentType.SelectedValue == "All")
            {
                reportName = reportName + " (All Payment Type)";
            }
            else
            {
                reportName = reportName + " (Payment Mode: " + this.ddlPaymentMode.SelectedValue + ", Payment Type: " + ddlPaymentType.SelectedValue + ")";
            }

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            if (ddlPaymentMode.SelectedValue != "All")
            {
                paymentMode = ddlPaymentMode.SelectedValue;
            }

            if (ddlPaymentType.SelectedValue != "All")
            {
                paymentType = ddlPaymentType.SelectedValue;
            }

            if (ddlReceivedBy.SelectedValue != "0")
            {
                receivedBy = ddlReceivedBy.SelectedValue;
            }

            string reservationNumber = txtReservationNumber.Text;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/ReservationBillTransaction.rdlc");

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
            reportParam.Add(new ReportParameter("SearchDate", hmUtility.GetFromDateAndToDate(FromDate, ToDate)));
            reportParam.Add(new ReportParameter("ReportName", reportName));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(reportParam);

            RoomReservationDA reservationDa = new RoomReservationDA();
            List<ReservationBillTransactionReportBO> reservationBill = new List<ReservationBillTransactionReportBO>();
            reservationBill = reservationDa.GetReservationBillTransaction(reservationNumber, paymentMode, paymentType, FromDate, ToDate, receivedBy);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], reservationBill));

            rvTransaction.LocalReport.DisplayName = "Reservation Transaction";
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

    }
}