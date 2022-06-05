using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesManagment;
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

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class ItemConsumptionReport : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "all";
                CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
                LoadCostCenter();
                LoadCommonDropDownHiddenField();
                LoadCategory();
                LoadEmployee();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            string reportType = ddlReportType.SelectedValue;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            int? itemId = null;
            int _costCenterId = Convert.ToInt32(ddlCostCenterfrom.SelectedValue);

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

            string _costCenter = ddlCostCenterfrom.SelectedValue;
            string _location = hfLocationId.Value;
            string _category = ddlCategory.SelectedValue;
            string item = hfItemId.Value;
            string consumptionType = ddlConsumptionType.SelectedValue;
            string consumer = "0";
            if (consumptionType == "Employee")
            {
                consumer = ddlEmployee.SelectedValue;
            }
            else if(consumptionType == "CostCenter")
            {
                consumer = ddlCostCenter.SelectedValue;
            }
            else if(consumptionType == "0")
            {
                consumer = "0";
            }
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            var reportPath = "";

            if (reportType == "Item Wise")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemConsumptionInformation.rdlc");
            }
            else if (reportType == "Date Wise")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemConsumption.rdlc");
            }
            else if (reportType == "Consumption Type Wise")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/RptConsumptionTypeWiseConsumption.rdlc");
            }
            else if (reportType == "Costcenter Wise")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/RptCostCenterWiseConsumption.rdlc");
            }
            else if (reportType == "Category Wise")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/RptCategoryWiseConsumption.rdlc");
            }
            else if (reportType == "Consumption Number Wise")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/RptConsumptionNoWiseConsumption.rdlc");
            }
            else
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemConsumption.rdlc");



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

            int companyId = 0;
            int projectId = 0;

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                //projectName = hfProjectId.Value != "0" ? hfProjectName.Value : "All";
            }

            PMProductOutDA DA = new PMProductOutDA();
            List<ItemConsumptionReportBO> itemConsumptionBO = new List<ItemConsumptionReportBO>();

            itemConsumptionBO = DA.GetItemConsumptionInfoForReport(companyId, projectId, FromDate, ToDate, _costCenter, _location, _category, item, consumptionType, consumer);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], itemConsumptionBO));
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
        private void LoadCostCenter()
        {
            List<CostCentreTabBO> costCenter = new List<CostCentreTabBO>();
            CostCentreTabDA DA = new CostCentreTabDA();
            costCenter = DA.GetAllCostCentreTabInfo();


            ddlCostCenterfrom.DataSource = costCenter;
            ddlCostCenterfrom.DataTextField = "CostCenter";
            ddlCostCenterfrom.DataValueField = "CostCenterId";
            ddlCostCenterfrom.DataBind();

            ddlCostCenter.DataSource = costCenter;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCostCenterfrom.Items.Insert(0, item);
            this.ddlCostCenter.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
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

        private void LoadEmployee()
        {
            EmployeeDA empDa = new EmployeeDA();
            var employee = empDa.GetEmployeeInfo();

            ddlEmployee.DataSource = employee;
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlEmployee.Items.Insert(0, item);
        }
        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }

        [WebMethod]
        public static List<ItemViewBO> LoadProductByCategoryNCostcenterId(string costCenterId, string categoryId)
        {
            InvItemDA itemda = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();

            productList = itemda.GetInvItemInfoByCategory(Convert.ToInt32(costCenterId), Convert.ToInt32(categoryId));
            List<ItemViewBO> itemViewList = new List<ItemViewBO>();

            itemViewList = (from s in productList
                            select new ItemViewBO
                            {
                                ItemId = s.ItemId,
                                ItemName = s.Name,
                                ProductType = s.ProductType

                            }).ToList();

            return itemViewList;
        }
    }
}