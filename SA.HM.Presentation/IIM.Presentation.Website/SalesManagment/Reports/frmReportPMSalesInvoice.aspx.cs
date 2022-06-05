using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.SalesManagment.Reports
{
    public partial class frmReportPMSalesInvoice : System.Web.UI.Page
    {
        protected int IsInvoiceTemplate1Visible = -1;
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string SalesId = Request.QueryString["SalesId"];
            string From = Request.QueryString["From"];
            string InvoiceId = Request.QueryString["InvoiceId"];
            string CustomerInfo = Request.QueryString["CustomerInfo"];
            int salesId = 0;
            string SalesType = "";

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(SalesId))
                {
                    if (From == "IndividualBill")
                    {
                        if (Session["ToBillExpireDate"] != null)
                        {
                            txtEndDate.Text = Session["ToBillExpireDate"].ToString();
                        }
                    }
                    SalesType = From;
                    salesId = Int32.Parse(SalesId);
                    LoadReport(salesId, SalesType, InvoiceId, CustomerInfo);
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadReport(int salesId, string salesType, string invoiceId, string customerInfo)
        {
            var endDate = DateTime.Now;
            string headerImageName = string.Empty;
            string headerImagePath = string.Empty;
            HMCommonDA hmCommonDA = new HMCommonDA();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            if (!string.IsNullOrEmpty(customerInfo))
            {
                customerInfo = customerInfo.Replace(":", string.Empty).Replace(",", string.Empty) + " " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.ToString("yyyy");
            }

            if (files[0].CompanyId > 0)
            {
                headerImageName = files[0].ImageName;
                headerImagePath = files[0].ImagePath;
                //this.txtCompanyName.Text = files[0].CompanyName;
                //this.txtCompanyAddress.Text = files[0].CompanyAddress;
                //if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                //{
                //    this.txtCompanyWeb.Text = files[0].WebAddress;
                //}
                //else
                //{
                //    this.txtCompanyWeb.Text = files[0].ContactNumber;
                //}
            }

            if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                endDate = hmUtility.GetDateTimeFromString(txtEndDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransactionTwo.LocalReport.EnableExternalImages = true;
            List<ReportParameter> param1 = new List<ReportParameter>();
            //param1.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/StyleSheet/images/Innboard-Logo_White.jpg")));
            param1.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + headerImagePath + headerImageName)));
            rvTransaction.LocalReport.SetParameters(param1);
            rvTransactionTwo.LocalReport.SetParameters(param1);

            this.txtSalesId.Text = salesId.ToString();
            this.txtSalesType.Text = salesType.ToString();
            this.txtInvoiceId.Text = invoiceId.ToString();
            endDate = hmUtility.GetDateTimeFromString(endDate.ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            int template = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["SalesInvoiceTemplate"].ToString());
            if (template == 1)
            {
                TransactionDataSource.SelectParameters[0].DefaultValue = this.txtSalesId.Text;
                TransactionDataSource.SelectParameters[1].DefaultValue = this.txtSalesType.Text;
                TransactionDataSource.SelectParameters[2].DefaultValue = this.txtInvoiceId.Text;
                TransactionDataSource.SelectParameters[3].DefaultValue = endDate.ToString();

                rvTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
                rvTransaction.LocalReport.DisplayName = customerInfo;
                rvTransaction.LocalReport.Refresh();
                SalesInvoiceTemplateOne.Visible = true;
                SalesInvoiceTemplateTwo.Visible = false;
            }
            else
            {
                TransactionDataSourceTwo.SelectParameters[0].DefaultValue = this.txtSalesId.Text;
                TransactionDataSourceTwo.SelectParameters[1].DefaultValue = this.txtSalesType.Text;
                TransactionDataSourceTwo.SelectParameters[2].DefaultValue = this.txtInvoiceId.Text;
                TransactionDataSourceTwo.SelectParameters[3].DefaultValue = endDate.ToString();

                rvTransactionTwo.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
                rvTransactionTwo.LocalReport.DisplayName = customerInfo;
                rvTransactionTwo.LocalReport.Refresh();
                SalesInvoiceTemplateOne.Visible = false;
                SalesInvoiceTemplateTwo.Visible = true;
            }
        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            if (this.txtSalesType.Text == "TmpInvoice")
            {
                if (this.Session["PMSalesDetailList"] != null)
                {
                    List<PMSalesDetailBO> detailListBO = Session["PMSalesDetailList"] == null ? new List<PMSalesDetailBO>() : Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
                    List<PMSalesInvoiceViewBO> salesInvoiceViewBOList = new List<PMSalesInvoiceViewBO>();
                    string strStarData = "********************";
                    foreach (PMSalesDetailBO row in detailListBO)
                    {
                        var endDate = DateTime.Now;
                        PMSalesInvoiceViewBO salesInvoiceViewBO = new PMSalesInvoiceViewBO();

                        salesInvoiceViewBO.AccountName = strStarData;
                        salesInvoiceViewBO.AccountNoLocal = strStarData;
                        salesInvoiceViewBO.AccountNoUSD = strStarData;
                        salesInvoiceViewBO.AdvanceOrDueAmount = 0;
                        salesInvoiceViewBO.BankName = strStarData;
                        salesInvoiceViewBO.BillFromDate = endDate;
                        salesInvoiceViewBO.BillToDate = endDate;
                        salesInvoiceViewBO.BranchName = strStarData;
                        salesInvoiceViewBO.CompanyAddress = strStarData;
                        salesInvoiceViewBO.CompanyName = strStarData;
                        salesInvoiceViewBO.ContactNumber = strStarData;
                        salesInvoiceViewBO.Currency = "BDT"; //-----------------------------------------Will be resolved---------------------.
                        salesInvoiceViewBO.CustomerAddress = strStarData;
                        salesInvoiceViewBO.CustomerCode = strStarData;
                        salesInvoiceViewBO.CustomerContactNumber = strStarData;
                        salesInvoiceViewBO.CustomerEmailAddress = strStarData;
                        salesInvoiceViewBO.CustomerId = 0;
                        salesInvoiceViewBO.CustomerName = strStarData;
                        salesInvoiceViewBO.CustomerWebAddress = strStarData;
                        salesInvoiceViewBO.DueDate = endDate;
                        salesInvoiceViewBO.EmailAddress = strStarData;
                        salesInvoiceViewBO.FieldId = 45;  //-----------------------------------------Will be resolved---------------------.
                        salesInvoiceViewBO.InvoiceDate = endDate;
                        salesInvoiceViewBO.InvoiceFor = strStarData;
                        salesInvoiceViewBO.InvoiceNo = strStarData;
                        salesInvoiceViewBO.SwiftCode = strStarData;
                        salesInvoiceViewBO.WebAddress = strStarData;
                        salesInvoiceViewBO.ItemId = row.ItemId;
                        salesInvoiceViewBO.ItemName = row.ItemName;
                        salesInvoiceViewBO.ItemQuantity = row.ItemUnit;
                        salesInvoiceViewBO.TotalPrice = row.TotalPrice;
                        salesInvoiceViewBO.UnitPrice = row.UnitPriceLocal;

                        salesInvoiceViewBOList.Add(salesInvoiceViewBO);
                    }

                    e.DataSources.Add(new ReportDataSource("SalesInvoiceDS", salesInvoiceViewBOList));
                }
            }
            else
            {
                var endDate = DateTime.Now;

                if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    endDate = hmUtility.GetDateTimeFromString(txtEndDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }

                PMSalesDetailsDA entityDA = new PMSalesDetailsDA();
                List<PMSalesInvoiceViewBO> salesInvoiceViewBOList = entityDA.GetPMSalesByInvoiceNumberAndCustomerId(Convert.ToInt32(this.txtSalesId.Text), this.txtSalesType.Text, Convert.ToInt32(this.txtInvoiceId.Text), endDate);
                e.DataSources.Add(new ReportDataSource("SalesInvoiceDS", salesInvoiceViewBOList));

                string month = string.Empty;
                if (salesInvoiceViewBOList[0].BillFromDate.Month == salesInvoiceViewBOList[0].BillToDate.Month)
                {
                    month = " " + salesInvoiceViewBOList[0].BillFromDate.ToString("MMMM") + " " + salesInvoiceViewBOList[0].BillFromDate.ToString("yyyy");
                }
                else if (salesInvoiceViewBOList[0].BillFromDate.Month != salesInvoiceViewBOList[0].BillToDate.Month)
                {
                    month = " " + salesInvoiceViewBOList[0].BillFromDate.ToString("MMMM") + " " + salesInvoiceViewBOList[0].BillFromDate.ToString("yyyy") + " To " + salesInvoiceViewBOList[0].BillToDate.ToString("MMMM") + " " + salesInvoiceViewBOList[0].BillToDate.ToString("yyyy");
                }

                rvTransactionTwo.LocalReport.DisplayName = (salesInvoiceViewBOList[0].CustomerName + month);
            }
        }
    }
}