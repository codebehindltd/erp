using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
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

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class ItemConsumptionInformationReport : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadEmployee();
                LoadCategory();
                LoadProductInfo();
            }
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            int? itemId = null;
            int _costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);

            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }

            if (string.IsNullOrEmpty(txtEndDate.Text))
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
            string _consumer;
            int _consumptionFor = Convert.ToInt32(ddlConsumptionFor.SelectedValue);
            if(_consumptionFor==1)
            {
                _consumer = ddlCostCenter.SelectedValue;
            }
            else
            {
                _consumer = ddlEmployeeName.SelectedValue;
            }
            string _item = ddlItem.SelectedValue;
            string _category = ddlCategory.SelectedValue;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemConsumptionInformation.rdlc");

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
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            PMProductOutDA DA = new PMProductOutDA();
            List<ItemConsumptionInformationReportBO> itemConsumptionInfoBO = new List<ItemConsumptionInformationReportBO>();
            itemConsumptionInfoBO = DA.GetItemConsumptionInformationForReport(FromDate, ToDate, _consumptionFor,_consumer, _category,_item);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], itemConsumptionInfoBO));
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
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo();
            this.ddlCostCenter.DataSource = List;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCostCenter.Items.Insert(0, item);
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeeBO> employeeList = new List<EmployeeBO>();
            employeeList = employeeDa.GetEmployeeInfo();

            ddlEmployeeName.DataSource = employeeList;
            ddlEmployeeName.DataTextField = "DisplayName";
            ddlEmployeeName.DataValueField = "EmpId";
            ddlEmployeeName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlEmployeeName.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }
        private void LoadProductInfo()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            productList = productDA.GetInvItemInfoByCategoryId(0, Convert.ToInt32(ddlCategory.SelectedValue));

            ddlItem.DataSource = productList;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";

            ddlItem.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlItem.Items.Insert(0, item);

        }
    }
}