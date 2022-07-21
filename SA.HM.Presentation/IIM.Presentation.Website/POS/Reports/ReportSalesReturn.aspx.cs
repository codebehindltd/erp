using HotelManagement.Data.HMCommon;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
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

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class ReportSalesReturn : System.Web.UI.Page
    {
        
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
            
            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptSalesReturn.rdlc");

            if (!File.Exists(reportPath))
                return;
            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

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

            List<PMSupplierProductReturnBO> proRet = new List<PMSupplierProductReturnBO>();
            PMProductReturnDA proDA = new PMProductReturnDA();

            proRet = proDA.GetSalesReturnRecipt(returnId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], proRet));
            rvTransaction.LocalReport.DisplayName = "Sales Return";
            rvTransaction.LocalReport.Refresh();

        }

    }
}



