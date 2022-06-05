using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
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

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class CashRequsitionInvoice : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Id = Request.QueryString["Id"];
                if (!string.IsNullOrEmpty(Id))
                {
                    this.ReportProcessing(Id);
                }
            }
        }
        private void ReportProcessing(string id)
        {
            CashRequisitionDA DA = new CashRequisitionDA();
            List<CashRequisitionBO> CashRequisition = new List<CashRequisitionBO>();
            List<CashRequisitionBO> CashRequisitionDetails = new List<CashRequisitionBO>();
            CashRequisition = DA.GetCashRequisitionByIdForInvoice(Convert.ToInt32(id));
            CashRequisitionDetails = DA.GetCashRequisitionDetailsByIdForInvoice(Convert.ToInt32(id));

            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            string cashRequisitionRemarks = string.Empty;

            if (CashRequisition[0].TransactionType == "Bill Voucher")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptBillvoucherInvoice.rdlc");

            }
            else if(CashRequisition[0].TransactionType == "Cash Requisition")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/CashRequsitionInvoice.rdlc");
            }
            else if (CashRequisition[0].TransactionType == "Cash Requisition Adjustment")
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptCashRequsitionAdjustment.rdlc");
            }

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

            if (CashRequisition != null)
            {
                if (CashRequisition[0].Id > 0)
                {
                    cashRequisitionRemarks = CashRequisition[0].Remarks;
                }
            }

            paramReport.Add(new ReportParameter("CashRequisitionRemarks", cashRequisitionRemarks));

            string outDate = hmUtility.GetStringFromDateTime(currentDate);
            List<CommonCheckByApproveByUserBO> EmpList = new List<CommonCheckByApproveByUserBO>();
            EmpList = hmCommonDA.GetCommonCheckByApproveByUserByPrimaryKey("CashRequisition", "Id", id);
            
            rvTransaction.LocalReport.SetParameters(paramReport);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], EmpList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], CashRequisitionDetails));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[2], CashRequisition));            

            rvTransaction.LocalReport.DisplayName = "Cash Requisition Invoice";
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