using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.Membership;

namespace HotelManagement.Presentation.Website.Membership.Reports
{
    public partial class frmReportMemberPayment : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryStringIdList = Request.QueryString["PaymentIdList"];
                if (!string.IsNullOrEmpty(queryStringIdList))
                {
                    this.Session["MemberBillPaymentIdList"] = string.Empty;
                    this.Session["MemberBillPaymentIdList"] = Request.QueryString["PaymentIdList"];
                    Response.Redirect("/Membership/Reports/frmReportMemberPayment.aspx");
                }

                if (this.Session["MemberBillPaymentIdList"] != null)
                {
                    ReportProcessing();
                }
            }
        }

        private void ReportProcessing()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Membership/Reports/Rdlc/rptMemberPayment.rdlc");

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
            string printedby = userInformationBO.UserName;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(paramReport);

            List<ReportParameter> param1 = new List<ReportParameter>();
            param1.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            param1.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            param1.Add(new ReportParameter("PrintedBy", printedby));
            rvTransaction.LocalReport.SetParameters(param1);

            List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            paramPrintDateTime.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramPrintDateTime);

            string paymentIdList = Session["MemberBillPaymentIdList"].ToString();
            MemberPaymentDA memberDA = new MemberPaymentDA();
            List<PMMemberPaymentLedgerBO> paymentList = new List<PMMemberPaymentLedgerBO>();
            paymentList = memberDA.GetMemberPaymentLedger(null, null, paymentIdList, true);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], paymentList));

            rvTransaction.LocalReport.DisplayName = "Member Payment Invoice";
            rvTransaction.LocalReport.Refresh();
        }
    }
}