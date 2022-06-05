using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using Microsoft.Reporting.WebForms;
using System.IO;
using Newtonsoft.Json;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmCompanyPaymentCollection : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadUserInformation();
                LoadAdjustmentEquivalantHead();
            }
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            list = entityDA.GetCostCentreTabInfoByType("Restaurant,RetailPos,Billing");

            this.ddlCostcenter.DataSource = list;
            this.ddlCostcenter.DataTextField = "CostCenter";
            this.ddlCostcenter.DataValueField = "CostCenterId";
            this.ddlCostcenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "--- All ---";
            this.ddlCostcenter.Items.Insert(0, item);
        }
        private void LoadUserInformation()
        {
            UserInformationDA entityDA = new UserInformationDA();
            this.ddlReceivedBy.DataSource = entityDA.GetUserInformation().OrderBy(x => x.UserIdAndUserName).ToList();
            this.ddlReceivedBy.DataTextField = "UserIdAndUserName";
            this.ddlReceivedBy.DataValueField = "UserInfoId";
            this.ddlReceivedBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "--- All ---";
            this.ddlReceivedBy.Items.Insert(0, item);
        }
        private void LoadAdjustmentEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();

            // // //-----Tax Deducted at Source by Customer (AIT)
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO taxDeductedAtSourceByCustomerAccountHeadBO = new HMCommonSetupBO();
            taxDeductedAtSourceByCustomerAccountHeadBO = commonSetupDA.GetCommonConfigurationInfo("TaxDeductedAtSourceByCustomerAccountHeadId", "TaxDeductedAtSourceByCustomerAccountHeadId");
            if (taxDeductedAtSourceByCustomerAccountHeadBO != null)
            {
                List<NodeMatrixBO> entityBOAditionalList = new List<NodeMatrixBO>();
                entityBOAditionalList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList(taxDeductedAtSourceByCustomerAccountHeadBO.SetupValue).Where(x => x.IsTransactionalHead == true).ToList();

                entityBOList.AddRange(entityBOAditionalList);
            }

            List<NodeMatrixBO> entityExpenditureBOList = new List<NodeMatrixBO>();
            entityExpenditureBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityExpenditureBOList != null)
            {
                entityBOList.AddRange(entityExpenditureBOList);
            }

            ddlAdjustmentNodeHead.DataSource = entityBOList;
            ddlAdjustmentNodeHead.DataTextField = "HeadWithCode";
            ddlAdjustmentNodeHead.DataValueField = "NodeId";
            ddlAdjustmentNodeHead.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstAllValue();
            ddlAdjustmentNodeHead.Items.Insert(0, itemBank);
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";

            if (ddlReportFormat.SelectedValue == "Company")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/RptCompanyPaymentCollection.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/RptCompanyPaymentCollectionDateWise.rdlc");
            }


            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            int receivedBy = 0, costcenterId = 0;
            string startDate = string.Empty, endDate = string.Empty, reportType = string.Empty, collectionType = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtDateFrom.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtDateFrom.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtDateFrom.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDateTo.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDateTo.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDateTo.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            Int32 companyId = Convert.ToInt32(hfCompanyId.Value);

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

            costcenterId = Convert.ToInt32(ddlCostcenter.SelectedValue);
            receivedBy = Convert.ToInt32(ddlReceivedBy.SelectedValue);
            reportType = ddlReportType.SelectedValue;
            collectionType = ddlCollectionType.SelectedValue;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string reportTitle = "Company Payment Collection";
            if (ddlCostcenter.SelectedValue != "0")
            {
                reportTitle = reportTitle + " for the Cost Center: " + ddlCostcenter.SelectedItem.Text;
            }

            reportParam.Add(new ReportParameter("ReportTitle", reportTitle));
            reportParam.Add(new ReportParameter("FromDate", startDate));
            reportParam.Add(new ReportParameter("ToDate", endDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(reportParam);

            GuestCompanyDA commonReportDa = new GuestCompanyDA();
            List<CompanyPaymentLedgerReportVwBo> companyLedger = new List<CompanyPaymentLedgerReportVwBo>();

            companyLedger = commonReportDa.GetCompanyPaymentInfoForReport(costcenterId, reportType, companyId, FromDate, ToDate, receivedBy, collectionType, ddlAdjustmentNodeHead.SelectedItem.Text);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], companyLedger));

            rvTransaction.LocalReport.DisplayName = reportTitle;
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

        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyData(string searchText)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            GuestCompanyDA suppDa = new GuestCompanyDA();

            companyList = suppDa.GetCompanyInfoBySearchCriteria(searchText);
            return companyList;
        }

    }
}