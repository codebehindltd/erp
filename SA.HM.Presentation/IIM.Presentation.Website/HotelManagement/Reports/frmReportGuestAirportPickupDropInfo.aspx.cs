using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestAirportPickupDropInfo : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.LoadCurrentDate();
                this.LoadCompany();
            }
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            //this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            companyList = companyDA.GetGuestCompanyInfo();

            this.ddlCompany.DataSource = companyList;
            this.ddlCompany.DataTextField = "CompanyName";
            this.ddlCompany.DataValueField = "CompanyId";
            this.ddlCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCompany.Items.Insert(0, item);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            int companyId = 0;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetFromDate();
                endDate = hmUtility.GetFromDate();
            }
            else
            {
                startDate = this.txtFromDate.Text;
                endDate = this.txtFromDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            if (ddlCompany.SelectedIndex != 0)
            {
                companyId = Convert.ToInt32(ddlCompany.SelectedValue);
            }

            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();
            //if (files != null)
            //{
            //    if (files.Count > 0)
            //    {
            //        this.txtCompanyName.Text = files[0].CompanyName;
            //        this.txtCompanyAddress.Text = files[0].CompanyAddress;
            //        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //        {
            //            this.txtCompanyWeb.Text = files[0].WebAddress;
            //        }
            //        else
            //        {
            //            this.txtCompanyWeb.Text = files[0].ContactNumber;
            //        }
            //    }
            //}

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (ddlType.SelectedValue == "All")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestAirportPickUpDropInfo.rdlc");                
            }
            else if (ddlType.SelectedValue == "PickUp")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestAirportPickUp.rdlc");                
            }
            else {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestAirportDrop.rdlc");                
            }

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
           
            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            //List<ReportParameter> paramReport = new List<ReportParameter>();
            //rvTransaction.LocalReport.SetParameters(paramReport);

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
           // rvTransaction.LocalReport.SetParameters(paramReport);

           // List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            //RoomReservationDataSource.SelectParameters[0].DefaultValue = FromDate.ToString();
            //RoomReservationDataSource.SelectParameters[1].DefaultValue = ToDate.AddDays(1).AddSeconds(-1).ToString();
            //RoomReservationDataSource.SelectParameters[2].DefaultValue = this.txtCompanyName.Text;
            //RoomReservationDataSource.SelectParameters[3].DefaultValue = this.txtCompanyAddress.Text;
            //RoomReservationDataSource.SelectParameters[4].DefaultValue = this.txtCompanyWeb.Text;

            GuestAirportPickUpDropDA guestAirportPDDA = new GuestAirportPickUpDropDA();
            List<GuestAirportPickUpDropReportViewBO> guestAipportPDBO = new List<GuestAirportPickUpDropReportViewBO>();
            guestAipportPDBO = guestAirportPDDA.GetGuestAirportPickupDropInfo(FromDate, ToDate, companyId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestAipportPDBO));
            if (ddlType.SelectedValue == "All")
            {
                rvTransaction.LocalReport.DisplayName = "Airport Pickup Drop Information";
            }
            else if (ddlType.SelectedValue == "PickUp")
            {
                rvTransaction.LocalReport.DisplayName = "Airport Pickup Information";
            }
            else rvTransaction.LocalReport.DisplayName = "Airport Drop Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            string reportType = string.Empty;
            if (ddlType.SelectedValue == "All")
            {                
                reportType = HMConstants.PrintPageType.Landscape.ToString();
            }
            else if (ddlType.SelectedValue == "PickUp")
            {                
                reportType = HMConstants.PrintPageType.Portrait.ToString();
            }
            else
            {                
                reportType = HMConstants.PrintPageType.Portrait.ToString();
            }

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, reportType);

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}