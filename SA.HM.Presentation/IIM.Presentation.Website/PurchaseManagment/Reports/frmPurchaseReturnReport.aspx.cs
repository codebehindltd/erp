using HotelManagement.Data.HMCommon;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
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

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmPurchaseReturnReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _RoomStatusInfoByDate = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            LoadSupplierInfo();
        }

        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            ddlSupplier.DataSource = entityDA.GetPMSupplierInfo();
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSupplier.Items.Insert(0, item);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
           
            //inputs 
           
            string fromDate = string.Empty, toDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            int supplierId = 0;
            string status = null;
            DateTime FromDate = DateTime.UtcNow;
            DateTime ToDate = DateTime.UtcNow;
            DateTime dtValue = DateTime.UtcNow;

            string returnNumber = txtReturnNumber.Text;

            if (!string.IsNullOrEmpty(txtReturnNumber.Text))
            {
                returnNumber = txtReturnNumber.Text;
                fromDate = string.Empty;
                toDate = string.Empty;
                supplierId = 0;

                txtStartDate.Text = fromDate;
                txtEndDate.Text = toDate;
            }
            else
            {
                if (string.IsNullOrEmpty(txtStartDate.Text)) 
                {
                    fromDate = string.Empty;
                    //txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                     //FromDate = hmUtility.GetDateTimeFromString("", hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }
                else
                {
                    FromDate = hmUtility.GetDateTimeFromString(txtStartDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    fromDate = Convert.ToString(FromDate);
                    txtStartDate.Text = FromDate.ToShortDateString();
                    //FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }
                if (string.IsNullOrEmpty(txtEndDate.Text))
                {
                    toDate = string.Empty;
                    //endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                    //txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                     //ToDate = hmUtility.GetDateTimeFromString("", hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }
                else
                {
                    ToDate = hmUtility.GetDateTimeFromString(txtEndDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    toDate = Convert.ToString(ToDate);
                    //toDate = dtValue.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    txtEndDate.Text = ToDate.ToShortDateString();
                    //ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    // DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

                }
                if (ddlSupplier.SelectedValue != "0")
                {
                    supplierId = Convert.ToInt32(ddlSupplier.SelectedValue);
                    ddlSupplier.SelectedValue = supplierId.ToString();
                }
                else
                {
                    supplierId = Convert.ToInt32(ddlSupplier.SelectedValue);
                }

                if ((!string.IsNullOrEmpty(fromDate)) && (string.IsNullOrEmpty(toDate)))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please provide To Date", AlertType.Warning);
                    txtEndDate.Focus();
                    return;
                }
                else if ((!string.IsNullOrEmpty(toDate)) && (string.IsNullOrEmpty(fromDate)))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please provide From Date", AlertType.Warning);
                    txtEndDate.Focus();
                    return;
                }
            }
           
            _RoomStatusInfoByDate = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptPMPurchaseReturnReport.rdlc");

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

            List<PMSupplierProductReturnDetailsBO> proRet = new List<PMSupplierProductReturnDetailsBO>();
            PMProductReturnDA proDA = new PMProductReturnDA();
            proRet = proDA.GetPurchaseReturnForReportBySearchCriteria(fromDate, toDate, returnNumber, supplierId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], proRet));
            
            rvTransaction.LocalReport.Refresh();

        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}