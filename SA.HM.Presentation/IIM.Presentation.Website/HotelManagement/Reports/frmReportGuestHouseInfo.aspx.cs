using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.IO;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestHouseInfo : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        protected int _IsDateInformationShow = 1;
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCurrentDate();
                this.LoadGuestCompany();
                this.LoadGuestGroupName();
                this.OthersWithoutArrivalInfoDiv.Visible = true;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string reportDisplayDate = string.Empty;

            DateTime currentDate = DateTime.Now;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (this.ddlReportType.SelectedValue == "0")
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please provide a Report Name.";
                this.ddlReportType.Focus();
                return;
            }
            DateTime reportDate = hmUtility.GetDateTimeFromString(startDate, userInformationBO.ServerDateFormat);
            string reportType = ddlReportType.SelectedValue;
            string guestCompany = ddlGuestCompany.SelectedValue;

            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat);
                startDate = txtFromDate.Text;
            }
            else
            {
                fromDate = reportDate;
                txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                startDate = txtFromDate.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, userInformationBO.ServerDateFormat);
                endDate = txtToDate.Text;
            }
            else
            {
                toDate = reportDate;
                txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                endDate = txtToDate.Text;
            }

            if (this.ddlReportType.SelectedValue.ToString() == "InHouseGustList")
            {
                _IsDateInformationShow = -1;
            }
            else if (ddlReportType.SelectedValue == "CheckInList")
            {
                _IsDateInformationShow = 2;
            }
            else if (ddlReportType.SelectedValue == "CheckOutList")
            {
                _IsDateInformationShow = 3;
            }
            else if (ddlReportType.SelectedValue == "InHouseGroupList")
            {
                _IsDateInformationShow = 4;
                guestCompany = ddlGroupName.SelectedItem.Text.Replace("--- ALL ---", "0");
            }
            else if (ddlReportType.SelectedValue == "ArrivalList" || ddlReportType.SelectedValue == "ProbablyVacantList")
            {
                _IsDateInformationShow = 5;
            }
            else
            {
                _IsDateInformationShow = 1;
            }

            _RoomStatusInfoByDate = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = string.Empty;

            if (this.ddlReportType.SelectedValue.ToString() == "ArrivalList")
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseArrivalInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue.ToString() == "CheckOutList")
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseCheckOutInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue.ToString() == "ProbablyVacantList")
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseExpectedCheckOutInfo.rdlc");
            }
            else if (ddlReportType.SelectedValue.ToString() == "InHouseGroupList")
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                guestCompany = ddlGroupName.SelectedItem.Text.Replace("--- ALL ---", "0");
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGroupGuestHouseInfo.rdlc");
            }
            else if (ddlReportType.SelectedValue.ToString() == "InHouseGustList" && ddlOrderBy.SelectedValue == "Room")
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseInfoOrderByRoom.rdlc");
            }
            else if (ddlReportType.SelectedValue.ToString() == "InHouseGustList" && ddlOrderBy.SelectedValue == "Company")
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptInHouseCompanyGuestInfo.rdlc");
            }
            else
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseInfo.rdlc");
            }

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

            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("ReportDisplayDate", reportDisplayDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            if (ddlReportType.SelectedValue == "InHouseGustList")
            {
                if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
                {
                    fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat);
                    toDate = fromDate;
                }
                else
                {
                    fromDate = reportDate;
                    toDate = reportDate;
                    txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                    txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                }

                if (ddlGuestCompany.SelectedValue == "0")
                {
                    if (ddlOrderBy.SelectedValue == "Company")
                    {
                        guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany).OrderBy(x => x.GuestCompany).ToList();
                    }
                    else
                    {
                        guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany).OrderBy(x => x.RoomNumber).ToList();
                    }
                }
                else
                {
                    guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany);
                }

                if (!string.IsNullOrWhiteSpace(txtReferenceNumber.Text))
                {
                    guestHouseInfo = guestHouseInfo.Where(x => x.RRNumber.Trim().Contains(this.txtReferenceNumber.Text.Trim())).ToList();
                }
            }
            else
            {
                guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany);
            }

            if (ddlReportType.SelectedValue == "CheckInList")
            {
                if (this.ddlGuestCompany.SelectedValue != "0")
                {
                    guestHouseInfo = guestHouseInfo.Where(x => x.GuestCompany.Trim().Contains(this.ddlGuestCompany.SelectedItem.Text.Trim())).ToList();
                }
            }
            else if (ddlReportType.SelectedValue == "ArrivalList")
            {
                if (this.ddlGuestCompany.SelectedValue != "0")
                {
                    guestHouseInfo = guestHouseInfo.Where(x => x.GuestCompany.Trim().Contains(this.ddlGuestCompany.SelectedItem.Text.Trim())).ToList();
                }

                if (!string.IsNullOrWhiteSpace(txtReferenceNumber.Text))
                {
                    guestHouseInfo = guestHouseInfo.Where(x => x.RRNumber.Trim().Contains(this.txtReferenceNumber.Text.Trim())).ToList();
                }
            }

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], guestHouseInfo));

            if (this.ddlReportType.SelectedValue.ToString() == "ArrivalList")
            {
                rvTransaction.LocalReport.DisplayName = "Expected Arrival Guest List";
            }
            else if (this.ddlReportType.SelectedValue.ToString() == "CheckOutList")
            {
                rvTransaction.LocalReport.DisplayName = "Checked-Out Guest List";
            }
            else if (this.ddlReportType.SelectedValue.ToString() == "ProbablyVacantList")
            {
                rvTransaction.LocalReport.DisplayName = "Expected Departure Guest List";
            }
            else if (this.ddlReportType.SelectedValue.ToString() == "InHouseGroupList")
            {
                rvTransaction.LocalReport.DisplayName = "In-House Group Guest List";
            }
            else
            {
                rvTransaction.LocalReport.DisplayName = "In-House Guest List";
            }

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
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
        }
        public void LoadGuestCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetGuestCompanyInfo();
            ddlGuestCompany.DataSource = files;
            ddlGuestCompany.DataTextField = "CompanyName";
            ddlGuestCompany.DataValueField = "CompanyId";
            ddlGuestCompany.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlGuestCompany.Items.Insert(0, itemReference);
        }
        public void LoadGuestGroupName()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetGuestGroupNameInfo();
            ddlGroupName.DataSource = files;
            ddlGroupName.DataTextField = "GroupName";
            ddlGroupName.DataValueField = "GroupName";
            ddlGroupName.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlGroupName.Items.Insert(0, itemReference);
        }
    }
}