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
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportProductionInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string iFGId = Request.QueryString["iFGId"];

            if (!String.IsNullOrEmpty(iFGId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(iFGId));
                }
            }
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {

        }
        protected void btnEmailSend_Click(object sender, EventArgs e)
        {
            SendMail();
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

            }
        }
        private void LoadReport(int iFGId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptProductionInfoInfo.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            HMCommonDA hmCommonDA = new HMCommonDA();
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

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetStringFromDateTime(currentDate) + " " + currentDate.ToString("hh:mm:ss tt");
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            PMFinishProductDA purchaseDa = new PMFinishProductDA();
            List<FinishedProductDetailsBO> finishedGoodsInfo = new List<FinishedProductDetailsBO>();
            List<FinishedProductDetailsBO> rmInfo = new List<FinishedProductDetailsBO>();
            List<FinishedProductDetailsBO> fgInfo = new List<FinishedProductDetailsBO>();
            List<OverheadExpensesBO> oeInfo = new List<OverheadExpensesBO>();

            PMFinishProductDA goodsDA = new PMFinishProductDA();
            bool status = goodsDA.ByProductProductionCalculation(iFGId);

            rmInfo = purchaseDa.GetInvProductionRMInformation(iFGId);
            oeInfo = purchaseDa.GetInvProductionOEInformation(iFGId);
            fgInfo = purchaseDa.GetInvProductionInformation(iFGId);

            if (fgInfo != null)
            {
                if (fgInfo.Count > 0)
                {
                    reportParam.Add(new ReportParameter("CostcenterInfo", rmInfo[0].CostCenter));
                    reportParam.Add(new ReportParameter("FinishedGoodsDate", rmInfo[0].ProductionDateString));
                }
                else
                {
                    reportParam.Add(new ReportParameter("CostcenterInfo", ""));
                    reportParam.Add(new ReportParameter("FinishedGoodsDate", ""));
                }
            }
            else
            {
                reportParam.Add(new ReportParameter("CostcenterInfo", ""));
                reportParam.Add(new ReportParameter("FinishedGoodsDate", ""));
            }

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], rmInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], fgInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], oeInfo));
            rvTransaction.LocalReport.DisplayName = "Production Information";
            rvTransaction.LocalReport.Refresh();
        }
        private void SendMail()
        {
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
            string Mail = "", Password = "", SmtpHost = "", SmtpPort = "";

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;
            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                Mail = dataArray[0];
                Password = dataArray[1];
                SmtpHost = dataArray[2];
                SmtpPort = dataArray[3];
            }

            if (!string.IsNullOrWhiteSpace(ReceivingMail))
            {

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SmtpHost); //smtp.gmail.com
                mail.From = new MailAddress(Mail);
                mail.To.Add(ReceivingMail);
                mail.Subject = "Purchase Order.";
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                mail.Attachments.Add(new Attachment(@"" + filePath + ""));
                SmtpServer.Port = Int32.Parse(SmtpPort);// 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Mail, Password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
        }
    }
}