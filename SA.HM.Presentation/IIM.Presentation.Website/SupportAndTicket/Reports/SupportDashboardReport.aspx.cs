using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.SupportAndTicket;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SupportAndTicket.Reports
{
    public partial class SupportDashboardReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSupportDropDown();
                LoadCategory();
                LoadServiceWarranty();
                LoadCity();
                LoadCompany();

            }

        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            DateTime FromDate = DateTime.MinValue;
            DateTime ToDate = DateTime.MinValue;

            /*if (string.IsNullOrEmpty(txtSearchFromDate.Text))
            {
                //startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                //txtSearchFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }*/

            if (!string.IsNullOrEmpty(txtSearchFromDate.Text) && string.IsNullOrEmpty(txtSearchToDate.Text))
            {
                return;
            }
            if (string.IsNullOrEmpty(txtSearchFromDate.Text) && !string.IsNullOrEmpty(txtSearchToDate.Text))
            {
                return;
            }

            if (!string.IsNullOrEmpty(txtSearchFromDate.Text))
            {
                startDate = txtSearchFromDate.Text;
                FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            /*if (string.IsNullOrEmpty(txtSearchToDate.Text))
            {
                //endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                //txtSearchToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }*/

            if (!string.IsNullOrEmpty(txtSearchToDate.Text))
            {
                endDate = txtSearchToDate.Text;
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            }

            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            var reportPath = "";
            if (ddlReportType.SelectedValue == "Summary")
            {
                reportPath = Server.MapPath(@"~/SupportAndTicket/Reports/Rdlc/rptSupportDashboardReport.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/SupportAndTicket/Reports/Rdlc/rptSupportDetailsReport.rdlc");
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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            SupportNTicketReportDA DA = new SupportNTicketReportDA();
            if (ddlReportType.SelectedValue == "Summary")
            {
                List<BillClaimedNTotalBillClaimedBO> BillClaimedNTotalBillClaimedList = new List<BillClaimedNTotalBillClaimedBO>();
                List<CaseNameAndCountNumberBO> CaseNameAndCountNumberList = new List<CaseNameAndCountNumberBO>();
                List<CityNCompanyNameAndCountNumberBO> CompanyNameAndCountNumberList = new List<CityNCompanyNameAndCountNumberBO>();
                List<CityNCompanyNameAndCountNumberBO> CityNameAndCountNumberList = new List<CityNCompanyNameAndCountNumberBO>();
                List<WarrantyAndCountNumberBO> ItemWarrantyAndCountNumberList = new List<WarrantyAndCountNumberBO>();
                List<SupportCategoryAndCountNumberBO> SupportCategoryAndCountNumberList = new List<SupportCategoryAndCountNumberBO>();
                List<ItemCategoryAndCountNumberBO> ItemCategoryAndCountNumberList = new List<ItemCategoryAndCountNumberBO>();
                List<SupportTypeAndCountNumberBO> SupportTypeAndCountNumberList = new List<SupportTypeAndCountNumberBO>();

                BillClaimedNTotalBillClaimedList = DA.GetBillClaimedNTotalBillClaimeForReport(FromDate, ToDate);
                CaseNameAndCountNumberList = DA.GetCaseNameAndCountNumberForReport(FromDate, ToDate);
                CompanyNameAndCountNumberList = DA.GetCityNCompanyNameAndCountNumberForReport("Company", FromDate, ToDate);
                CityNameAndCountNumberList = DA.GetCityNCompanyNameAndCountNumberForReport("State", FromDate, ToDate);
                ItemWarrantyAndCountNumberList = DA.GetWarrantyAndCountNumberForReport(FromDate, ToDate);
                SupportCategoryAndCountNumberList = DA.GetSupportCategoryAndCountNumberForReport(FromDate, ToDate);
                ItemCategoryAndCountNumberList = DA.GetSupportItemCategoryAndCountNumberForReport(FromDate, ToDate);
                SupportTypeAndCountNumberList = DA.GetSupportTypeAndCountNumberForReport(FromDate, ToDate);

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], BillClaimedNTotalBillClaimedList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], CaseNameAndCountNumberList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], CityNameAndCountNumberList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[3], ItemWarrantyAndCountNumberList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[4], SupportCategoryAndCountNumberList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[5], ItemCategoryAndCountNumberList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[6], SupportTypeAndCountNumberList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[7], CompanyNameAndCountNumberList));

                ddlReportType.SelectedValue = "Summary";
            }
            else
            {
                int caseId = 0, itemCategory = 0, supportCategory = 0, supportType = 0, warentyType = 0, companyId = 0, city = 0;
                string ticketNumber = "", status = "";
                decimal amountFrom = 0, amountTo = 0;
                caseId = Convert.ToInt32(ddlCase.SelectedValue);
                itemCategory = Convert.ToInt32(ddlItemCategory.SelectedValue);
                supportCategory = Convert.ToInt32(ddlSupportCategory.SelectedValue);
                supportType = Convert.ToInt32(ddlSupportType.SelectedValue);
                warentyType = Convert.ToInt32(ddlWarrantyType.SelectedValue);
                companyId = Convert.ToInt32(ddlCompany.SelectedValue);
                
                int countryId = 0;
                int stateId = 0;

                if (!string.IsNullOrWhiteSpace(txtBillingCountry.Text))
                {
                    countryId = Convert.ToInt32(hfBillingCountryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(txtBillingState.Text))
                {
                    stateId = Convert.ToInt32(hfBillingStateId.Value);
                }

                ticketNumber = txtTicketNumber.Text;
                status = ddlStatus.SelectedValue;
                if (!string.IsNullOrEmpty(txtSearchFromAmount.Text))
                    amountFrom = Convert.ToDecimal(txtSearchFromAmount.Text);
                if (!string.IsNullOrEmpty(txtSearchToAmount.Text))
                    amountTo = Convert.ToDecimal(txtSearchToAmount.Text);

                List<SupportNTicketViewBO> SupportNTicketList = new List<SupportNTicketViewBO>();

                if (
                    FromDate == DateTime.MinValue
                    && ToDate == DateTime.MinValue
                    && string.IsNullOrEmpty(ticketNumber)
                    && caseId == 0
                    && supportType == 0
                    && supportCategory == 0
                    && warentyType == 0
                    && companyId == 0
                    && countryId == 0
                    && stateId == 0
                    && string.IsNullOrEmpty(status)
                    && amountFrom == 0
                    && amountTo == 0
                    )
                {
                    FromDate = DateTime.Now;
                    ToDate = DateTime.Now;
                }

                SupportNTicketList = DA.GetSupportNTicketDetailsForReport(FromDate, ToDate, ticketNumber, caseId, itemCategory, supportCategory, supportType, warentyType, companyId, countryId, stateId, status, amountFrom, amountTo);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], SupportNTicketList));
                ddlReportType.SelectedValue = "Details";
            }
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
        }
        private void LoadSupportDropDown()
        {
            List<STSupportNCaseSetupInfoBO> SupportNCaseSetupList = new List<STSupportNCaseSetupInfoBO>();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportCategory");

            ddlSupportCategory.DataSource = SupportNCaseSetupList;
            ddlSupportCategory.DataTextField = "Name";
            ddlSupportCategory.DataValueField = "Id";
            ddlSupportCategory.DataBind();

            ddlSupportCategory.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("Case");

            ddlCase.DataSource = SupportNCaseSetupList;
            ddlCase.DataTextField = "Name";
            ddlCase.DataValueField = "Id";
            ddlCase.DataBind();
            ddlCase.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportType");

            ddlSupportType.DataSource = SupportNCaseSetupList;
            ddlSupportType.DataTextField = "Name";
            ddlSupportType.DataValueField = "Id";
            ddlSupportType.DataBind();

            ddlSupportType.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("All");
            ddlItemCategory.DataSource = List;
            ddlItemCategory.DataTextField = "MatrixInfo";
            ddlItemCategory.DataValueField = "CategoryId";
            ddlItemCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlItemCategory.Items.Insert(0, item);
        }
        private void LoadServiceWarranty()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("InventoryProductServiceWarranty");

            ddlWarrantyType.DataSource = fields;
            ddlWarrantyType.DataTextField = "FieldValue";
            ddlWarrantyType.DataValueField = "FieldId";
            ddlWarrantyType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlWarrantyType.Items.Insert(0, item);
        }
        private void LoadCity()
        {
            /*CityDA cityDA = new CityDA();
            List<CityBO> cityBO = new List<CityBO>();
            cityBO = cityDA.GetCityInfo();

            ddlCity.DataSource = cityBO;
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityId";
            ddlCity.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCity.Items.Insert(0, item);*/
            //ddlCity.Items.Insert(0, item);
        }
        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();

            ddlCompany.DataSource = files;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCompany.Items.Insert(0, item);
        }
    }
}