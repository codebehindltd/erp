using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data;
using System.IO;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportRoomReservation : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                this.LoadUserInformation();
                this.LoadRoomNumber();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.LoadCompanyInformation();
                this.LoadReferenceInformation();
                this.LoadCurrentDate();
                //((HMReport)Master).innBoardDateFormat = userInformationBO.DateFormat;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;

            string startDate = string.Empty, endDate = string.Empty, reservationNumber = string.Empty;
            string company = string.Empty, reference = string.Empty, reservationMode = string.Empty, roomNumber = string.Empty, filterBy = string.Empty;

            if (ddlCompany.SelectedValue != "0")
                company = ddlCompany.SelectedValue;

            if (ddlReference.SelectedValue != "0")
                reference = ddlReference.SelectedValue;

            if (ddlReservationMode.SelectedValue != "All")
                reservationMode = ddlReservationMode.SelectedValue;

            if (ddlRoomNumber.SelectedValue != "0")
                roomNumber = ddlRoomNumber.SelectedItem.Text;

            filterBy = ddlFilterBy.SelectedValue;
            int reservedBy = Convert.ToInt32(ddlReceivedBy.SelectedValue);

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetFromDate();
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetToDate();
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            if (ddlReportType.SelectedValue == "All")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptRoomReservation.rdlc");
            }
            else if (ddlReportType.SelectedValue == "WithAdvance")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptRoomReservationReport.rdlc");
            }
            else if (ddlReportType.SelectedValue == "WithoutAdvance")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptRoomReservationReport.rdlc");
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

            string filterByText = string.Empty;
            filterByText = ddlFilterBy.SelectedItem.Text;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FilterByText", filterByText));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            reservationNumber = txtReservationNumber.Text;
            RoomReservationDA purchaseDa = new RoomReservationDA();
            List<RoomReservationInfoByDateRangeReportBO> rommStatus = new List<RoomReservationInfoByDateRangeReportBO>();
            rommStatus = purchaseDa.GetRoomReservationInfoByDateRange(FromDate, ToDate, reservationNumber, company, reference, reservationMode, roomNumber, filterBy, reservedBy);
            
            if (ddlReportType.SelectedValue == "All")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], rommStatus));
            }
            else if (ddlReportType.SelectedValue == "WithAdvance")
            {
                rommStatus = rommStatus.Where(x => x.ReservationAdvanceTotal > 0).ToList();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], rommStatus));
            }
            else if (ddlReportType.SelectedValue == "WithoutAdvance")
            {
                rommStatus = rommStatus.Where(x => x.ReservationAdvanceTotal == 0).ToList();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], rommStatus));
            }
                        
            rvTransaction.LocalReport.DisplayName = "Room Reservation Information";
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
        private void LoadRoomNumber()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            this.ddlRoomNumber.DataSource = roomNumberDA.GetRoomNumberInfo();
            this.ddlRoomNumber.DataTextField = "RoomNumber";
            this.ddlRoomNumber.DataValueField = "RoomId";
            this.ddlRoomNumber.DataBind();

            ListItem itemRoom = new ListItem();
            itemRoom.Value = "0";
            itemRoom.Text = "-- All --";
            this.ddlRoomNumber.Items.Insert(0, itemRoom);
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void LoadCompanyInformation()
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            companyList = companyDA.GetGuestCompanyInfo();
            ddlCompany.DataSource = companyList;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = "-- All --";
            this.ddlCompany.Items.Insert(0, itemNodeId);
        }
        private void LoadReferenceInformation()
        {
            List<GuestReferenceBO> refernceList = new List<GuestReferenceBO>();
            GuestReferenceDA referenceDA = new GuestReferenceDA();
            refernceList = referenceDA.GetAllGuestRefference();
            ddlReference.DataSource = refernceList;
            ddlReference.DataTextField = "Name";
            ddlReference.DataValueField = "ReferenceId";
            ddlReference.DataBind();
            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = "-- All --";
            this.ddlReference.Items.Insert(0, itemNodeId);
        }
    }
}