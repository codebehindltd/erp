using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportGuestInformation : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int IsSuccess = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                this.IsAdminUserInfo();
                this.LoadCompany();
                this.LoadReference();
                this.LoadCountry();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsSuccess = 1;

            string reportType = string.Empty;

            int? company, reference, country;

            if (ddlReportType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Report Type.", AlertType.Warning);
                return;
            }

            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtToDate.Text))
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


            if (ddlCompany.SelectedIndex == 0)
            {
                company = null;
            }
            else
            {
                company = Convert.ToInt32(ddlCompany.SelectedValue);
            }
            if (ddlRefernece.SelectedIndex == 0)
            {
                reference = null;
            }
            else
            {
                reference = Convert.ToInt32(ddlRefernece.SelectedValue);
            }
            if (ddlCountry.SelectedIndex == 0)
            {
                country = null;
            }
            else
            {
                country = Convert.ToInt32(ddlCountry.SelectedValue);
            }
            //string year = ddlYear.SelectedValue.ToString();
            //string month = ddlMonth.SelectedValue.ToString();


            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            if (this.ddlReportType.SelectedValue == "GuestInformation")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptGuestInformation.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "CompanyWise")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptCompanyWiseGuestInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "CountryWise")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptCountryWiseGuestInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "ReferenceWise")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptReferenceWiseGuestInfo.rdlc");
            }

            if (ddlReportTypeSD.SelectedValue == "Summary")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptStayedNightsSummaryInfo.rdlc");
            }
            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
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

            string reportName = string.Empty;
            if (this.ddlReportType.SelectedValue == "GuestInformation")
            {
                reportType = "GuestInformation";
                reportName = "Guest Information";
            }
            else if (this.ddlReportType.SelectedValue == "CompanyWise")
            {
                reportType = "CompanyWise";
                reportName = "Company Wise Guest Information";
            }
            else if (this.ddlReportType.SelectedValue == "CountryWise")
            {
                reportType = "CountryWise";
                reportName = "Country Wise Guest Information";
            }
            else if (this.ddlReportType.SelectedValue == "ReferenceWise")
            {
                reportType = "ReferenceWise";
                reportName = "Reference Wise Guest Information";
            }

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("ReportDateFrom", startDate));
            paramReport.Add(new ReportParameter("ReportDateTo", endDate));

            rvTransaction.LocalReport.SetParameters(paramReport);
            string filterType = this.ddlFilterType.SelectedValue;

            SalesCallDA salesDA = new SalesCallDA();
            List<CompanyWiseGuestInfoViewBO> list = new List<CompanyWiseGuestInfoViewBO>();
            List<SummaryGuestInfoViewBO> summaryList = new List<SummaryGuestInfoViewBO>();

            if (this.ddlReportType.SelectedValue == "GuestInformation")
            {
                list = salesDA.GetGuestInformationForReport();
            }
            else
            {
                if (ddlReportTypeSD.SelectedValue == "Details")
                    list = salesDA.GetGuestInformationBySearchCriteriaForReport(reportType, filterType, company, reference, country, FromDate, ToDate);
                else if (ddlReportTypeSD.SelectedValue == "Summary")
                    summaryList = salesDA.GetGuestInformationBySearchCriteriaForSummaryReport(reportType, company, reference, country, FromDate, ToDate);
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            if (ddlReportTypeSD.SelectedValue == "Summary")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], summaryList));
            }
            else if (ddlReportTypeSD.SelectedValue == "Details")
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], list));

            if (this.ddlReportType.SelectedValue == "GuestInformation")
            {
                this.ddlCompany.SelectedValue = "0";
                this.ddlCountry.SelectedValue = "0";
                this.ddlRefernece.SelectedValue = "0";
                rvTransaction.LocalReport.DisplayName = "Guest Information";
            }
            else if (this.ddlReportType.SelectedValue == "CompanyWise")
            {
                //this.ddlCompany.SelectedValue = "0";
                this.ddlCountry.SelectedValue = "0";
                this.ddlRefernece.SelectedValue = "0";
                rvTransaction.LocalReport.DisplayName = "Company Wise Guest Information";
            }
            else if (this.ddlReportType.SelectedValue == "CountryWise")
            {
                this.ddlCompany.SelectedValue = "0";
                //this.ddlCountry.SelectedValue = "0";
                this.ddlRefernece.SelectedValue = "0";
                rvTransaction.LocalReport.DisplayName = "Country Wise Guest Information";
            }
            else if (this.ddlReportType.SelectedValue == "ReferenceWise")
            {
                this.ddlCompany.SelectedValue = "0";
                this.ddlCountry.SelectedValue = "0";
                //this.ddlRefernece.SelectedValue = "0";
                rvTransaction.LocalReport.DisplayName = "Reference Wise Guest Information";
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
        //************************ User Defined Function ********************//
        private void IsAdminUserInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            Boolean IsAdminUser = false;
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            if (!IsAdminUser)
            {
                this.ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("GuestInformation"));
            }
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
        private void LoadReference()
        {
            GuestReferenceDA entityDA = new GuestReferenceDA();
            List<GuestReferenceBO> files = entityDA.GetAllGuestRefference();
            ddlRefernece.DataSource = files;
            ddlRefernece.DataTextField = "Name";
            ddlRefernece.DataValueField = "ReferenceId";
            ddlRefernece.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlRefernece.Items.Insert(0, itemReference);
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();

            this.ddlCountry.DataSource = List;
            this.ddlCountry.DataTextField = "CountryName";
            this.ddlCountry.DataValueField = "CountryId";
            this.ddlCountry.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCountry.Items.Insert(0, item);
        }
    }
}