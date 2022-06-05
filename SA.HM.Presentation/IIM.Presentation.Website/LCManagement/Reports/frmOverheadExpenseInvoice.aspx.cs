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
using HotelManagement.Data.LCManagement;
using HotelManagement.Entity.LCManagement;

namespace HotelManagement.Presentation.Website.LCManagement.Reports
{
    public partial class frmOverheadExpenseInvoice : Page
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string pId = Request.QueryString["PId"];

            if (!String.IsNullOrEmpty(pId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(pId));
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
                //Document document = new Document(PageSize.LETTER);
                Document document = new Document(PageSize.A4);
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
        private void LoadReport(int expenseId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/LCManagement/Reports/Rdlc/RptOverheadExpenseInvoice.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            HMCommonDA hmCommonDA = new HMCommonDA();

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            OverHeadExpenseDA overHeadExpenseDA = new OverHeadExpenseDA();

            List<OverHeadExpenseBO> overHeadExpenseBO = new List<OverHeadExpenseBO>();
            overHeadExpenseBO = overHeadExpenseDA.GetLCOverheadExpenseInfoByExpenseId(expenseId);

            List<PMSupplierPaymentLedgerBO> SupplierBillInfoBOList = new List<PMSupplierPaymentLedgerBO>();
            LCInformationViewBO lcInformationBO = new LCInformationViewBO();

            string TransactionFrom = string.Empty;
            string TransactionTo = string.Empty;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            if (files[0].CompanyId > 0)
            {
                TransactionFrom = TransactionFrom + files[0].CompanyName;
                if (!string.IsNullOrWhiteSpace(files[0].CompanyAddress))
                {
                    TransactionFrom = TransactionFrom + "~" + files[0].CompanyAddress;
                }

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    TransactionFrom = TransactionFrom + "~" + files[0].WebAddress;
                }
                else
                {
                    TransactionFrom = TransactionFrom + "~" + files[0].ContactNumber;
                }
            }

            if (overHeadExpenseBO != null)
            {
                if (overHeadExpenseBO.Count > 0)
                {
                    LCInformationDA lcInformationDA = new LCInformationDA();
                    lcInformationBO = lcInformationDA.GetLCInformationALlDetailsByLCNumber(overHeadExpenseBO[0].LCId);
                    if (lcInformationBO.LCInformation.LCId > 0)
                    {
                        TransactionTo = "LC# " + lcInformationBO.LCInformation.LCNumber;
                    }
                }
            }

            List<CommonCheckByApproveByUserBO> EmpList = new List<CommonCheckByApproveByUserBO>();
            EmpList = hmCommonDA.GetCommonCheckByApproveByUserByPrimaryKey("LCOverHeadExpense", "ExpenseId", expenseId.ToString());
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], overHeadExpenseBO));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], EmpList));

            rvTransaction.LocalReport.DisplayName = "Supplier Pameny Invoice";

            reportParam.Add(new ReportParameter("TransactionFrom", TransactionFrom));
            reportParam.Add(new ReportParameter("TransactionTo", TransactionTo));
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.SetParameters(reportParam);

            rvTransaction.LocalReport.Refresh();
        }
        //private bool SendMail(string queryStringId)
        //{
        //    bool status = false;
        //    //Get Supplier Email
        //    int supplierId = 0;
        //    string suppId = Request.QueryString["SupId"];
        //    if (!String.IsNullOrEmpty(suppId))
        //    {
        //        supplierId = Convert.ToInt32(suppId);
        //    }
        //    PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
        //    List<PurchaseOrderDetailsForInvoiceBO> supplierInfo = new List<PurchaseOrderDetailsForInvoiceBO>();
        //    supplierInfo = purchaseDa.GetSupplierInformationForInvoice(supplierId);

        //    //Get pdf file
        //    HMUtility hmUtility = new HMUtility();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;

        //    string deviceInfo = string.Empty;

        //    byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
        //                   out encoding, out extension, out streamids, out warnings);

        //    string fileName = string.Empty, filePath = string.Empty;
        //    DateTime dateTime = DateTime.Now;
        //    fileName = "PO" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

        //    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();

        //    filePath = HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName);

        //    //Send Mail            
        //    string ReceivingMail = supplierInfo[0].SupplierEmail;
        //    string MailBody = "Please find the attachment.";


        //    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
        //    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
        //    string mainString = commonSetupBO.SetupValue;
        //    Email email;


        //    if (!string.IsNullOrEmpty(mainString))
        //    {
        //        string[] dataArray = mainString.Split('~');
        //        email = new Email()
        //        {
        //            From = dataArray[0],
        //            Password = dataArray[1],
        //            To = ReceivingMail,
        //            Subject = "Purchase Order.",
        //            Body = MailBody,
        //            AttachmentSavedPath = filePath,
        //            Host = dataArray[2],
        //            Port = dataArray[3]
        //        };

        //        try
        //        {
        //            status = EmailHelper.SendEmail(email, null);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return status;
        //}
    }
}