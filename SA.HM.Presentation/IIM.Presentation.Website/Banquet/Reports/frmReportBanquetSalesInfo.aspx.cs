using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;

namespace HotelManagement.Presentation.Website.Banquet.Reports
{
    public partial class frmReportBanquetSalesInfo : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                //this.FilterByItem();
                //LoadServiceInfo();
                LoadCategory();
                this.LoadCommonDropDownHiddenField();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            int categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : -1;
            string referNo = !string.IsNullOrWhiteSpace(this.txtReferenceNo.Text) ? this.txtReferenceNo.Text : "All";
            string tranType = this.ddlPaymentType.SelectedValue;
            txtHiddenItemId.Value = string.Empty;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/rptBanquetSalesInfo.rdlc");

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

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            //paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);

            BanquetReservationDA salesDA = new BanquetReservationDA();
            List<BanquetSalesInfoReportViewBO> salesList = new List<BanquetSalesInfoReportViewBO>();
            //salesList = salesDA.GetSalesRestaurantInfoForReport(FromDate, ToDate, filterBy, categoryId, serviceId, tranctype);
            salesList = salesDA.GetBanquetSalesInfoForReport(FromDate, ToDate, categoryId, itemId, referNo, tranType);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesList));

            rvTransaction.LocalReport.DisplayName = "Banquet Sales Information";
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
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            categoryList = da.GetAllInvItemCatagoryInfoByServiceType("Product");

            List<InvCategoryBO> banquetHallList = new List<InvCategoryBO>();
            InvCategoryBO banquetHallBO = new InvCategoryBO();
            banquetHallBO.CategoryId = 100001;
            banquetHallBO.Name = "BanquetHall";
            banquetHallBO.MatrixInfo = "Banquet Hall";
            banquetHallList.Add(banquetHallBO);

            List<InvCategoryBO> requisitesList = new List<InvCategoryBO>();
            InvCategoryBO requisitesBO = new InvCategoryBO();
            requisitesBO.CategoryId = 100000;
            requisitesBO.Name = "Requisites";
            requisitesBO.MatrixInfo = "Requisites";
            requisitesList.Add(requisitesBO);

            List<InvCategoryBO> List = new List<InvCategoryBO>();
            List.AddRange(banquetHallList);
            List.AddRange(requisitesList);
            List.AddRange(categoryList);

            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);
        }
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetItemNameAndItemIdByCategoryId(0, CategoryId);
            
            return productList;
        }
    }
}