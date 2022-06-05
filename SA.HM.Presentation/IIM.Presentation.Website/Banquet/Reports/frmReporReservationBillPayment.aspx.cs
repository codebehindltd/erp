using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.IO;

namespace HotelManagement.Presentation.Website.Banquet.Reports
{
    public partial class frmReporReservationBillPayment : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _RoomStatusInfoByDate = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryStringIdList = Request.QueryString["PaymentIdList"];

                if (!string.IsNullOrEmpty(queryStringIdList))
                {
                    this.Session["GuestBillPaymentIdList"] = string.Empty;
                    this.Session["GuestBillPaymentIdList"] = Request.QueryString["PaymentIdList"];
                    Response.Redirect("/Banquet/Reports/frmReporReservationBillPayment.aspx");
                }
                
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("GuestPaymentStatementTemplate", "GuestPaymentStatementTemplate");

                if (this.Session["GuestBillPaymentIdList"] != null)
                {
                    if (invoiceTemplateBO != null)
                    {
                        if (invoiceTemplateBO.SetupId > 0)
                        {
                            if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 1)
                            {
                                this.ReportProcessing("rptBanquetTransactionInfo");
                            }
                            else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 2)
                            {
                                this.ReportProcessing("rptBanquetTransactionInfoTwoRow");
                            }
                        }
                    }
                    //this.ReportProcessing();
                }
            }
        }
        //************************ User Defined Function ********************//
        private void ReportProcessing(string reportName)
        {
            if (this.Session["GuestBillPaymentIdList"] != null)
            {
                this.txtPaymentIdList.Text = string.Empty;
                this.txtCompanyName.Text = string.Empty;
                this.txtCompanyAddress.Text = string.Empty;
                this.txtCompanyWeb.Text = string.Empty;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.txtPrintedBy.Text = userInformationBO.UserName;
                HMCommonDA hmCommonDA = new HMCommonDA();
                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                if (files[0].CompanyId > 0)
                {
                    this.txtCompanyName.Text = files[0].CompanyName;
                    this.txtCompanyAddress.Text = files[0].CompanyAddress;
                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        this.txtCompanyWeb.Text = files[0].WebAddress;
                    }
                    else
                    {
                        this.txtCompanyWeb.Text = files[0].ContactNumber;
                    }

                }

                this.txtPaymentIdList.Text = this.Session["GuestBillPaymentIdList"].ToString();
                //this.txtGuestBillFromDate.Text = this.Session["GuestBillFromDate"].ToString();
                //this.txtGuestBillToDate.Text = this.Session["GuestBillToDate"].ToString();   

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/" + reportName + ".rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                _RoomStatusInfoByDate = 1;

                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;

                List<ReportParameter> reportParam = new List<ReportParameter>();

                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);  //hmUtility.GetDateTimeStringFromDateTime(currentDate);
                string footerPoweredByInfo = string.Empty;
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                //paramImagePath.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                reportParam.Add(new ReportParameter("HMCompanyProfile", this.txtCompanyName.Text));
                reportParam.Add(new ReportParameter("HMCompanyAddress", this.txtCompanyAddress.Text));
                reportParam.Add(new ReportParameter("HMCompanyWeb", this.txtCompanyWeb.Text));
                reportParam.Add(new ReportParameter("PrintedBy", this.txtPrintedBy.Text));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                rvTransaction.LocalReport.SetParameters(reportParam);

                //rvTransaction.LocalReport.SetParameters(paramHMCompanyProfile);
                //rvTransaction.LocalReport.SetParameters(paramHMCompanyAddress);
                //rvTransaction.LocalReport.SetParameters(paramHMCompanyWeb);
                //rvTransaction.LocalReport.SetParameters(paramPrintedBy);
                //TransactionDataSource.SelectParameters[0].DefaultValue = this.txtPaymentIdList.Text;
                //TransactionDataSource.SelectParameters[1].DefaultValue = this.txtCompanyName.Text;
                //TransactionDataSource.SelectParameters[2].DefaultValue = this.txtCompanyAddress.Text;
                //TransactionDataSource.SelectParameters[3].DefaultValue = this.txtCompanyWeb.Text;
                //TransactionDataSource.SelectParameters[1].DefaultValue = this.txtPrintedBy.Text;
                string searchCriteria = this.txtPaymentIdList.Text;

                GuestHouseCheckOutDA entityDA = new GuestHouseCheckOutDA();
                List<GuestBillPaymentInvoiceReportViewBO> serviceBillBO = entityDA.GetBanquetTransactionInvoiceInfo(searchCriteria);

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], serviceBillBO));

                rvTransaction.LocalReport.DisplayName = "Guest Payment Information";
                rvTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
                rvTransaction.LocalReport.DisplayName = "Guest Payment Invoice";
                rvTransaction.LocalReport.Refresh();
            }
        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            string searchCriteria = this.txtPaymentIdList.Text;
            GuestHouseCheckOutDA entityDA = new GuestHouseCheckOutDA();

            List<GuestBillPaymentInvoiceReportViewBO> serviceBillBO = entityDA.GetGuestPaymentInvoiceInfo(searchCriteria);
            e.DataSources.Add(new ReportDataSource("GuestTransactionInfo", serviceBillBO));
        }
    }
}