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
    public partial class ReportPMPurchaseReturn : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string returnId = Request.QueryString["ReturnId"];
                if (!string.IsNullOrEmpty(returnId))
                {
                    this.ReportProcessing(Convert.ToInt64(returnId));
                }
            }
        }
        private void ReportProcessing(long returnId)
        {
            _RoomStatusInfoByDate = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/PurchaseReturnForChalan.rdlc");

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
                paramReport.Add(new ReportParameter("EmailAddress", files[0].EmailAddress));
                paramReport.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));

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
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            rvTransaction.LocalReport.SetParameters(paramReport);

            

            //PMSupplierProductReturnDetailsBO //PMProductReturnDA
            List<PMSupplierProductReturnDetailsBO> proRet = new List<PMSupplierProductReturnDetailsBO>();
            PMProductReturnDA proDA = new PMProductReturnDA();

            proRet = proDA.GetPurchaseReturnForChalanByReturnId(returnId); 

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], proRet));
            rvTransaction.LocalReport.DisplayName = "Product Return";
            rvTransaction.LocalReport.Refresh();

        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            _RoomStatusInfoByDate = 1;
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}