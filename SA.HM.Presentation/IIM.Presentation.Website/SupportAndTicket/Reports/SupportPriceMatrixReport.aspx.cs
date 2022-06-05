using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SupportAndTicket;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SupportAndTicket.Reports
{
    public partial class SupportPriceMatrixReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

        }
        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyByAutoSearch(string searchString)
        {

            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> CompanyList = guestCompanyDA.GetCompanyInfoBySearchCriteria(searchString);

            return CompanyList;
        }
        [WebMethod]
        public static List<InvCategoryBO> GetCategoryByAutoSearch(string searchString)
        {

            InvCategoryDA DA = new InvCategoryDA();
            List<InvCategoryBO> CategoryList = DA.GetInvCategoryByAutoSearch(searchString);

            return CategoryList;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> GetItemByAutoSearch(string searchString, int categoryId)
        {

            InvItemDA DA = new InvItemDA();
            List<InvItemAutoSearchBO> ItemList = DA.GetItemByCategoryAutoSearch(searchString, categoryId, true);

            return ItemList;
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            int companyId = Convert.ToInt32(hfCompanyId.Value);
            int categoryId = Convert.ToInt32(hfCategoryId.Value);
            int itemId = Convert.ToInt32(hfItemId.Value);
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            var reportPath = "";

            reportPath = Server.MapPath(@"~/SupportAndTicket/Reports/Rdlc/rptSupportPriceMatrixReport.rdlc");


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
            List<STSupportPriceMatrixSetupBO> SupportList = new List<STSupportPriceMatrixSetupBO>();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();
            SupportList = DA.GetPriceMatrixSetupForReport(companyId, categoryId, itemId);
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], SupportList));
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
        }
    }
}