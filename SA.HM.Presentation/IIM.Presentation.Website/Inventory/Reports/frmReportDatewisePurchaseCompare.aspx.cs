using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportDatewisePurchaseCompare : BasePage
    {
        HiddenField innboardMessage;
        protected int _ReportShow = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {                
                LoadItem();                
            }
        }
        private void LoadItem()
        {
            List<InvItemBO> List = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            List = productDA.GetInvItemInfo();
            ddlItem.DataSource = List;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlItem.Items.Insert(0, itemNodeId);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            if (ddlItem.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select an Item.", AlertType.Warning);
                return;
            }
            else
            {

                _ReportShow = 1;
                string startDate = string.Empty, endDate = string.Empty;
                DateTime dateTime = DateTime.Now;
                if (string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                    //txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                }
                else
                {
                    startDate = txtStartDate.Text;
                }
                if (string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                    //txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                }
                else
                {
                    endDate = txtEndDate.Text;
                }
                DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
                int itemId = 0;

                if (!string.IsNullOrWhiteSpace(ddlItem.SelectedValue))
                {
                    itemId = Convert.ToInt32(ddlItem.SelectedValue);
                }

                AllInventoryReportDA allInventoryReportDA = new AllInventoryReportDA();
                List<DatewisePurchaseCompareViewBO> viewList = new List<DatewisePurchaseCompareViewBO>();
                viewList = allInventoryReportDA.GetDatewisePurchaseCompareReportInfo(FromDate, ToDate, itemId);

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.EnableExternalImages = true;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptDatewisePurchaseCompare.rdlc");

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

                HMCommonDA hmCommonDA = new HMCommonDA();
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

                reportParam.Add(new ReportParameter("ReportDateFrom", startDate));
                reportParam.Add(new ReportParameter("ReportDateTo", endDate));
                reportParam.Add(new ReportParameter("ItemName", viewList.Count > 0 ? viewList[0].ItemName : ""));
                reportParam.Add(new ReportParameter("StockBy", viewList.Count > 0 ? viewList[0].StockBy : ""));

                rvTransaction.LocalReport.SetParameters(reportParam);

                var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], viewList));

                rvTransaction.LocalReport.DisplayName = "Date Wise Purchase Comparison";
                rvTransaction.LocalReport.Refresh();

                frmPrint.Attributes["src"] = "";
            }
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