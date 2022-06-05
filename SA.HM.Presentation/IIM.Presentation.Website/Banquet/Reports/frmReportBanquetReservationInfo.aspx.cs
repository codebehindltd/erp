using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Banquet.Reports
{
    public partial class frmReportBanquetReservationInfo : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();
        protected int IsSuccess = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadBanquetInfo();
            }
        }
        private void LoadBanquetInfo()
        {
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            var List = banquetDA.GetAllBanquetInformation();
            this.ddlBanquetName.DataSource = List;
            this.ddlBanquetName.DataTextField = "Name";
            this.ddlBanquetName.DataValueField = "Id";
            this.ddlBanquetName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlBanquetName.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            IsSuccess = 1;
            long banquetId = Convert.ToInt64(ddlBanquetName.SelectedValue);
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtArriveDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtArriveDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else {
                startDate = txtArriveDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtDepartureDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtDepartureDate.Text = hmUtility.GetStringFromDateTime(dateTime);                
            }
            else {
                endDate = txtDepartureDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/rptBanquetReservation.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate); // hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));

            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            BanquetReservationDA reservationDA = new BanquetReservationDA();
            List<BanquetReservationReportViewBO> list = new List<BanquetReservationReportViewBO>();
            list = reservationDA.GetBanquetInfoForReport(FromDate, ToDate, banquetId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], list));

            rvTransaction.LocalReport.DisplayName = "Banquet Reservation Information";
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
    }
}