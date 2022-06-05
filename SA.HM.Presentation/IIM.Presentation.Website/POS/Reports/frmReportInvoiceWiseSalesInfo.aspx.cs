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
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using HotelManagement.Data.UserInformation;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportInvoiceWiseSalesInfo : BasePage
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                FilterByItem();
                LoadServiceInfo();
                LoadCategory();
                LoadUserInformation();
                LoadCommonDropDownHiddenField();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                startDate = txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                endDate = txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            //string filterBy = ddlFilterBy.SelectedValue;
            //int categoryId = Convert.ToInt32(ddlServiceId.SelectedValue);
            //int serviceId = Convert.ToInt32(ddlServiceId.SelectedValue);
            //string tranctype = ddlTransactionType.SelectedValue;

            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : -1;
            //!string.IsNullOrWhiteSpace(ddlItem.SelectedValue) ? Convert.ToInt32(ddlItem.SelectedValue) : -1;

            //int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : -1;
            string referNo = !string.IsNullOrWhiteSpace(txtReferenceNo.Text) ? txtReferenceNo.Text : "All";
            string tranType = ddlPaymentType.SelectedValue;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptInvoiceWiseSalesInfo.rdlc");

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

            string reportName = "Invoice Wise Sales Information";
            //if (ddlFilterBy.SelectedValue == "All")
            //{
            //    reportName = "Restaurant Sales Information";
            //}
            //else if (ddlFilterBy.SelectedValue == "Room")
            //{
            //    reportName = "Restaurant Sales Information (Room Sales)";
            //}
            //else
            //{
            //    reportName = "Restaurant Sales Information" + " (" + ddlFilterBy.SelectedValue + ")";
            //}


            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            //List<ReportParameter> param1 = new List<ReportParameter>();
            //List<ReportParameter> param2 = new List<ReportParameter>();
            //List<ReportParameter> param3 = new List<ReportParameter>();

            //rvTransaction.LocalReport.SetParameters(paramReport);

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));

            //rvTransaction.LocalReport.SetParameters(param1);
            //rvTransaction.LocalReport.SetParameters(param2);
            //rvTransaction.LocalReport.SetParameters(param3);
            //List<ReportParameter> paramReportName = new List<ReportParameter>();            
            //rvTransaction.LocalReport.SetParameters(paramReportName);
            //List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");

            //hfIsRestaurantBillAmountWillRound.Value = "1";
            if (isRestaurantBillAmountWillRoundBO != null)
            {
                paramReport.Add(new ReportParameter("IsRestaurantBillAmountWillRound", isRestaurantBillAmountWillRoundBO.SetupValue));
                //hfIsRestaurantBillAmountWillRound.Value = isRestaurantBillAmountWillRoundBO.SetupValue;
            }

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramReport);


            RestaurantSalesInfoDA salesDA = new RestaurantSalesInfoDA();
            List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();
            //salesList = salesDA.GetSalesRestaurantInfoForReport(FromDate, ToDate, filterBy, categoryId, serviceId, tranctype);


            string costCenterIdList = string.Empty;
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> CostCentreTabBOList = costCentreTabDA.GetCostCentreTabInfo();

            if (CostCentreTabBOList != null)
            {
                foreach (CostCentreTabBO row in CostCentreTabBOList)
                {
                    if (!string.IsNullOrWhiteSpace(costCenterIdList))
                    {
                        costCenterIdList = costCenterIdList + "," + row.CostCenterId.ToString();
                    }
                    else
                    {
                        costCenterIdList = row.CostCenterId.ToString();
                    }
                }
            }

            int cashierId = Convert.ToInt32(ddlCashierName.SelectedValue);
            salesList = salesDA.GetInvoiceWiseSalesRestaurantInfoForReport(costCenterIdList, FromDate, ToDate, cashierId, userInformationBO.UserGroupId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesList));

            rvTransaction.LocalReport.DisplayName = "Invoice Wise Sales Information";
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
        private void FilterByItem()
        {
            ddlFilterBy.Items.Add(new ListItem("--- All ---", "All"));
            ddlFilterBy.Items.Add(new ListItem("Room Sale", "Room"));
            ddlFilterBy.Items.Add(new ListItem("Cash Sale", "Cash"));
            ddlFilterBy.Items.Add(new ListItem("Card Sale", "Card"));
            ddlFilterBy.Items.Add(new ListItem("Company Sales", "Company"));
            //ddlFilterBy.Items.Add(new ListItem("Cheque Sale", "ChequeSale"));
        }
        private void LoadServiceInfo()
        {

            HotelGuestServiceInfoDA serviceDA = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> list = new List<HotelGuestServiceInfoBO>();
            list = serviceDA.GetAllGuestServiceInfo();
            ddlServiceId.DataSource = list;
            ddlServiceId.DataValueField = "ServiceId";
            ddlServiceId.DataTextField = "ServiceName";
            ddlServiceId.DataBind();

            ListItem itemAll = new ListItem();
            itemAll.Value = "-1";
            itemAll.Text = "--- All ---";
            ddlServiceId.Items.Insert(0, itemAll);

        }
        private void LoadUserInformation()
        {
            UserInformationDA entityDA = new UserInformationDA();
            ddlCashierName.DataSource = entityDA.GetCashierWaiterInformation(0);
            ddlCashierName.DataTextField = "DisplayName";
            ddlCashierName.DataValueField = "EmpId";
            ddlCashierName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "--- All ---";
            ddlCashierName.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, CategoryId);

            return productList;
        }
    }
}