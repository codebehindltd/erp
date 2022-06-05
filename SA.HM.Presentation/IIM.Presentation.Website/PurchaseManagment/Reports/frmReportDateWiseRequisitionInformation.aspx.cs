using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmReportDateWiseRequisitionInformation : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
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

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptDateWiseRequisition.rdlc");

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

            PMRequisitionDA reqDA = new PMRequisitionDA();
            List<PMRequisitionReportViewBO> purchaseInfo = new List<PMRequisitionReportViewBO>();
            
            if (ddlPOApprovalStatus.SelectedValue == "0")
            {
                purchaseInfo = reqDA.GetDateWiseRequisitionReportInfo(FromDate, ToDate, itemId, _costCenterId);
            }
            else if (ddlPOApprovalStatus.SelectedValue == "1")
            {
                purchaseInfo = reqDA.GetDateWiseRequisitionReportInfo(FromDate, ToDate, itemId, _costCenterId).Where(x => x.ApprovedStatus == "Submitted").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "2")
            {
                purchaseInfo = reqDA.GetDateWiseRequisitionReportInfo(FromDate, ToDate, itemId, _costCenterId).Where(x => x.ApprovedStatus == "Approved").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "3")
            {
                purchaseInfo = reqDA.GetDateWiseRequisitionReportInfo(FromDate, ToDate, itemId, _costCenterId).Where(x => x.ApprovedStatus == "Checked").ToList();
            }
            else if (ddlPOApprovalStatus.SelectedValue == "4")
            {
                purchaseInfo = reqDA.GetDateWiseRequisitionReportInfo(FromDate, ToDate, itemId, _costCenterId).Where(x => x.ApprovedStatus == "Cancel").ToList();
            }

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], purchaseInfo));
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
            PMRequisitionDA DA = new PMRequisitionDA();
            List<CostCentreTabBO> costCenter = new List<CostCentreTabBO>();

            costCenter = DA.GetRequisitionCostCenter();

            HMUtility hmUtility = new HMUtility();
            ddlCostCenter.DataSource = costCenter;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCostCenter.Items.Insert(0, item);
        }
    }
}