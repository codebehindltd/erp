using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HotelManagement;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmServiceAndRoomBillByDate : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadServiceName();
            }
            this.txtCompanyName.Text = string.Empty;
            this.txtCompanyAddress.Text = string.Empty;
            this.txtCompanyWeb.Text = string.Empty;
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            _RoomStatusInfoByDate = 1;
            string approvedDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(this.txtApprovedDate.Text))
            {
                approvedDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtApprovedDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                approvedDate = this.txtApprovedDate.Text;
            }
            DateTime ApprovedDate = hmUtility.GetDateTimeFromString(approvedDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptServiceAndRoomBillByDate.rdlc");

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
            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> param1 = new List<ReportParameter>();
            param1.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            param1.Add(new ReportParameter("PrintDateTime", printDate));
            param1.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(param1);

            string serviceName = this.ddlServiceName.SelectedItem.Text;

            AllReportDA reportDA = new AllReportDA();
            List<NightAuditReportViewBO> nightAuditBO = new List<NightAuditReportViewBO>();
            nightAuditBO = reportDA.GetNightAuditInfo(ApprovedDate, serviceName);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], nightAuditBO));

            rvTransaction.LocalReport.DisplayName = "Night Audit Report";
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
        private void LoadServiceName()
        {
            GuestHouseCheckOutDA entityDA = new GuestHouseCheckOutDA();
            this.ddlServiceName.DataSource = entityDA.GetGuestApprovedServiceInfo();
            this.ddlServiceName.DataTextField = "ServiceName";
            this.ddlServiceName.DataValueField = "ServiceInfo";
            this.ddlServiceName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlServiceName.Items.Insert(0, item);
        }
    }
}