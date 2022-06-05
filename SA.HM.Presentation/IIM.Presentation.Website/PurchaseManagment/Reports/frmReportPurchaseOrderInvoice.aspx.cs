using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmReportPurchaseOrderInvoice : Page
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string pOrderId = Request.QueryString["POrderId"];
            string supplierId = Request.QueryString["SupId"];
            string currencyId = Request.QueryString["CurrencyId"];

            if (!String.IsNullOrEmpty(pOrderId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(pOrderId), Int32.Parse(supplierId), Int32.Parse(currencyId));
                }
            }
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {

        }
        protected void btnEmailSend_Click(object sender, EventArgs e)
        {
            //SendMail();
        }
        //************************ User Defined Function ********************//
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
        private void LoadReport(int POrderId, int supplierId, int currencyId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            string isStockSummaryEnableInStockReport = "0";
            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsStockSummaryEnableInStockReport", "IsStockSummaryEnableInStockReport");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    if (homePageSetupBO.SetupValue == "1")
                    {
                        isStockSummaryEnableInStockReport = "1";
                    }
                }
            }

            var reportPath = "";
            reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptProductPurchaseOrderInvoice.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            HMCommonDA hmCommonDA = new HMCommonDA();

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();

            List<SupplierNCompanyInfoForPurchaseInvoiceBO> purchaseInfo = new List<SupplierNCompanyInfoForPurchaseInvoiceBO>();
            List<PurchaseOrderDetailsForInvoiceBO> supplierInfo = new List<PurchaseOrderDetailsForInvoiceBO>();
            List<PMPurchaseOrderTermsNConditionBO> TNC = new List<PMPurchaseOrderTermsNConditionBO>();
            supplierInfo = purchaseDa.GetSupplierInformationForInvoice(supplierId);
            purchaseInfo = purchaseDa.GetPurchaseInformationForInvoice(POrderId, supplierId, currencyId);
            if (purchaseInfo != null)
            {
                if (purchaseInfo[0].POrderId > 0)
                {
                    if (purchaseInfo[0].CompanyId > 0)
                    {
                        GLCompanyBO glCompanyBO = new GLCompanyBO();
                        GLCompanyDA glCompanyDA = new GLCompanyDA();
                        glCompanyBO = glCompanyDA.GetGLCompanyInfoById(purchaseInfo[0].CompanyId);
                        if (glCompanyBO != null)
                        {
                            if (glCompanyBO.CompanyId > 0)
                            {
                                if (!string.IsNullOrWhiteSpace(glCompanyBO.Name))
                                {
                                    foreach (PurchaseOrderDetailsForInvoiceBO row in supplierInfo)
                                    {
                                        row.CompanyName = glCompanyBO.Name;
                                    }                                    
                                }
                                if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                                {
                                    foreach (PurchaseOrderDetailsForInvoiceBO row in supplierInfo)
                                    {
                                        row.CompanyAddress = glCompanyBO.CompanyAddress;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                                {
                                    foreach (PurchaseOrderDetailsForInvoiceBO row in supplierInfo)
                                    {
                                        row.WebAddress = glCompanyBO.WebAddress;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                                {
                                    ImageName = glCompanyBO.ImageName;
                                }
                            }
                        }
                    }
                }
            }
            
            TNC = purchaseDa.GetTermsNConditionsByPOId(POrderId);
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], supplierInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], purchaseInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], TNC));            
            rvTransaction.LocalReport.DisplayName = "Purchase Order Information";

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("CurrencyId", currencyId.ToString()));
            reportParam.Add(new ReportParameter("IsStockSummaryEnableInStockReport", isStockSummaryEnableInStockReport));
            rvTransaction.LocalReport.SetParameters(reportParam);
            
            rvTransaction.LocalReport.Refresh();
        }
        private bool SendMail(string queryStringId)
        {
            bool status = false;
            //Get Supplier Email
            int supplierId = 0;
            string suppId = Request.QueryString["SupId"];
            if (!String.IsNullOrEmpty(suppId))
            {
                supplierId = Convert.ToInt32(suppId);
            }
            PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
            List<PurchaseOrderDetailsForInvoiceBO> supplierInfo = new List<PurchaseOrderDetailsForInvoiceBO>();
            supplierInfo = purchaseDa.GetSupplierInformationForInvoice(supplierId);

            //Get pdf file
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string deviceInfo = string.Empty;

            byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileName = string.Empty, filePath = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = "PO" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            filePath = HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName);

            //Send Mail            
            string ReceivingMail = supplierInfo[0].SupplierEmail;
            string MailBody = "Please find the attachment.";


            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;
            Email email;


            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                email = new Email()
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = ReceivingMail,
                    Subject = "Purchase Order.",
                    Body = MailBody,
                    AttachmentSavedPath = filePath,
                    Host = dataArray[2],
                    Port = dataArray[3]
                };

                try
                {
                    status = EmailHelper.SendEmail(email, null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }
    }
}