using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.IO;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.UserInformation;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmSalesNMarketingSchedule : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int Success = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadSalesperson();
                this.LoadPurpose();
            }
        }
        private void LoadPurpose()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            List<CustomFieldBO> searchFields = new List<CustomFieldBO>();
            //fields = commonDA.GetCustomField("Sales&MarketingPurpose", hmUtility.GetDropDownFirstValue());
            fields = commonDA.GetCustomField("Sales&MarketingPurpose", hmUtility.GetDropDownFirstAllValue());

            this.ddlPurpose.DataSource = fields;
            this.ddlPurpose.DataTextField = "FieldValue";
            this.ddlPurpose.DataValueField = "FieldId";
            this.ddlPurpose.DataBind();
        }
        private void LoadSalesperson()
        {
            //List<SalesPersonViewBO> List = new List<SalesPersonViewBO>();
            //SalesCallDA salesCallDA = new SalesCallDA();
            //List = salesCallDA.GetSalesPersonInfo();

            UserInformationDA userInfoDA = new UserInformationDA();
            List<UserInformationBO> list = new List<UserInformationBO>();
            list = userInfoDA.GetAllUserInformation();

            this.ddlSalesPerson.DataSource = list;
            this.ddlSalesPerson.DataTextField = "UserName";
            this.ddlSalesPerson.DataValueField = "UserInfoId";
            this.ddlSalesPerson.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSalesPerson.Items.Insert(0, item);
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            Success = 1;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            int salesPerson = Convert.ToInt32(ddlSalesPerson.SelectedValue); 
            int salesPurpose = Convert.ToInt32(ddlPurpose.SelectedValue); 
            HMCommonDA hmCommonDA = new HMCommonDA();

            rvSalesSchedule.LocalReport.DataSources.Clear();
            rvSalesSchedule.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptSalesNMarketingSchedule.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvSalesSchedule.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();           

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvSalesSchedule.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));            

            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            if (files[0].CompanyId > 0)
            {
                paramLogo.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramLogo.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }
            rvSalesSchedule.LocalReport.SetParameters(paramLogo);

            SalesCallDA salesCallDA = new SalesCallDA();
            List<SalesNMarketingScheduleViewBO> salesPersonList = new List<SalesNMarketingScheduleViewBO>();
            salesPersonList = salesCallDA.GetSalesNMarketingScheduleInfo(FromDate, ToDate, salesPerson, salesPurpose);

            var reportDataset = rvSalesSchedule.LocalReport.GetDataSourceNames();
            rvSalesSchedule.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesPersonList));

            rvSalesSchedule.LocalReport.DisplayName = "Sales & Marketing Schedule";
            rvSalesSchedule.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvSalesSchedule.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}