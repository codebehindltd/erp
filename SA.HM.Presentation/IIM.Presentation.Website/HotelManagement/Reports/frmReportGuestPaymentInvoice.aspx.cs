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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestPaymentInvoice : System.Web.UI.Page
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
                    Response.Redirect("/HotelManagement/Reports/frmReportGuestPaymentInvoice.aspx");
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
                                this.ReportProcessing("rptGuestTransactionInfo");
                            }
                            else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 2)
                            {
                                this.ReportProcessing("rptGuestTransactionInfoTwoRow");
                            }
                        }
                    }
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

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
                isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
                if (isOnlyPdfEnableWhenReportExportBO != null)
                {
                    if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
                    {
                        if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
                        {
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());
                        }
                    }
                }

                this.txtPaymentIdList.Text = this.Session["GuestBillPaymentIdList"].ToString();
                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/" + reportName + ".rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                _RoomStatusInfoByDate = 1;

                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;

                List<ReportParameter> reportParam = new List<ReportParameter>();

                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                string footerPoweredByInfo = string.Empty;
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                reportParam.Add(new ReportParameter("HMCompanyProfile", this.txtCompanyName.Text));
                reportParam.Add(new ReportParameter("HMCompanyAddress", this.txtCompanyAddress.Text));
                reportParam.Add(new ReportParameter("HMCompanyWeb", this.txtCompanyWeb.Text));
                reportParam.Add(new ReportParameter("PrintedBy", this.txtPrintedBy.Text));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                rvTransaction.LocalReport.SetParameters(reportParam);
                string searchCriteria = this.txtPaymentIdList.Text;

                GuestHouseCheckOutDA entityDA = new GuestHouseCheckOutDA();
                List<GuestBillPaymentInvoiceReportViewBO> serviceBillBO = entityDA.GetGuestPaymentInvoiceInformation(searchCriteria);

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

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvTransaction.LocalReport.Render("PDF", null, out mimeType,
                               out encoding, out extension, out streamids, out warnings);

                string fileName = string.Empty, fileNamePrint = string.Empty;
                DateTime dateTime = DateTime.Now;
                fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
                fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Open exsisting pdf
                Document document = new Document(PageSize.LETTER);
                PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
                //Getting a instance of new pdf wrtiter
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
                   HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                int i = 0;
                int p = 0;
                int n = reader.NumberOfPages;
                Rectangle psize = reader.GetPageSize(1);

                float width = psize.Width;
                float height = psize.Height;

                //Add Page to new document
                while (i < n)
                {
                    document.NewPage();
                    p++;
                    i++;

                    PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page1, 0, 0);
                }

                //Attach javascript to the document
                PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
                //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
                writer.AddJavaScript(jAction);

                document.Close();

                frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}