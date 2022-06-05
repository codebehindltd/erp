using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportKotDetailsInfo : BasePage
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                LoadCostCenter();
                //LoadKOTNumber();
                //LoadBillNumber();
                LoadSetteledBy();
            }           
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            list = costCentreTabDA.GetAllCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();

            ddlCostCenter.DataSource = list;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCostCenter.Items.Insert(0, item);

        }
        //private void LoadKOTNumber()
        //{
        //    RestaurentBillDA restaurantBillDA = new RestaurentBillDA();
        //    List<KotBillDetailBO> kotBill = new List<KotBillDetailBO>();
        //    kotBill = restaurantBillDA.GetRestaurantKOTNumberForddl();

        //    ddlKotNumber.DataSource = kotBill;
        //    ddlKotNumber.DataTextField = "KotId";
        //    ddlKotNumber.DataValueField = "KotId";
        //    ddlKotNumber.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstAllValue();
        //    ddlKotNumber.Items.Insert(0, item);
        //}
        //private void LoadBillNumber()
        //{
        //    RestaurentBillDA restaurantBillDA = new RestaurentBillDA();
        //    List<RestaurantBill> bill = new List<RestaurantBill>();

        //    bill = restaurantBillDA.GetRestaurantBillNumberForddl();

        //    ddlBillNumber.DataSource = bill;
        //    ddlBillNumber.DataTextField = "BillNumber";
        //    ddlBillNumber.DataValueField = "BillNumber";
        //    ddlBillNumber.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstAllValue();
        //    ddlBillNumber.Items.Insert(0, item);
        //}
        private void LoadSetteledBy()
        {
            RestaurentBillDA restaurantBillDA = new RestaurentBillDA();
            List<KotBillDetailBO> kotBill = new List<KotBillDetailBO>();
            kotBill = restaurantBillDA.GetRestaurantSetteledByForddl();

            ddlSettledBy.DataSource = kotBill;
            ddlSettledBy.DataTextField = "LastModifiedUser";
            ddlSettledBy.DataValueField = "LastModifiedBy";
            ddlSettledBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSettledBy.Items.Insert(0, item);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();            
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                FromDate = null;
            }
            else
            {
                startDate = txtFromDate.Text;
                FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (string.IsNullOrWhiteSpace(txtToDate.Text) && !string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            }
            else if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = txtToDate.Text;
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            }
            else
            {
                ToDate = null;
            }
            int costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);
            string billNumber = txtBillNumber.Text;
            string kotNumber = txtKotNumber.Text;
            int userInfoId = Convert.ToInt32(ddlSettledBy.SelectedValue);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantKotDetailsInfo.rdlc");

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
            
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(paramReport);

            RestaurentBillDA restaurantBillDA = new RestaurentBillDA();
            List<RestaurantBillBO> restaurantCancelBillList = new List<RestaurantBillBO>();

            restaurantCancelBillList = restaurantBillDA.GetRestaurantKotDetailsInfo(FromDate, ToDate, costCenterId, billNumber, kotNumber, userInfoId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], restaurantCancelBillList));

            rvTransaction.LocalReport.DisplayName = "Kot Details Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;             
        }
    }
}