using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportGuestEnvelope : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryStringId = Request.QueryString["GOCId"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ForwordReportProcessing();
                }
            }
        }
        private void ForwordReportProcessing()
        {
            string queryStringId = Request.QueryString["GOCId"];
            //int billID = Int32.Parse(queryStringId);

            if (!string.IsNullOrEmpty(queryStringId))
            {
                this.ReportProcessing("rptGuestEnvelope");
            }
        }
        private void ReportProcessing(string reportName)
        {
            string queryStringId = Request.QueryString["GOCId"];
            

            if (!string.IsNullOrEmpty(queryStringId))
            {
                int GOCId = Int32.Parse(queryStringId);

                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.DataSources.Clear();

                var reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                rvTransaction.LocalReport.EnableExternalImages = true;
                HMCommonDA hmCommonDA = new HMCommonDA();

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();

                string printDate = string.Empty;
                PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
                List<PurchaseOrderDetailsForInvoiceBO> supplierInfo = new List<PurchaseOrderDetailsForInvoiceBO>();
                supplierInfo = purchaseDa.GetCompanyAndGuestCompanyInfo(GOCId);
                var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], supplierInfo));

                rvTransaction.LocalReport.DisplayName = "Envelope Information";
                rvTransaction.LocalReport.Refresh();
            }
        }
        protected void btnPrintReportTemplate1_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

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

            //writer.Close();
            //document.Close();
            //reader.Close();
            //bytes = null;
            //cb = null;
        }
    }
}