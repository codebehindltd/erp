using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Maintenance;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.Maintenance;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Maintenance.Reports
{
    public partial class frmReportGatePassInvoice : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            string gatePassId = Request.QueryString["GpId"];
            string supplierId = Request.QueryString["SupId"];

            if (!String.IsNullOrEmpty(gatePassId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int64.Parse(gatePassId), Int32.Parse(supplierId));
                }
            }
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

        private void LoadReport(long gatePassId,int supplierId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Maintenance/Reports/Rdlc/RptGatePassInvoice.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            HMCommonDA hmCommonDA = new HMCommonDA();

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            
            PMPurchaseOrderDA purchaseDA = new PMPurchaseOrderDA();
            GatePassDA gatePassDA = new GatePassDA();
            
            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            List<SupplierNCompanyInfoForGatePassInvoiceBO> gatePassInfoInfo = new List<SupplierNCompanyInfoForGatePassInvoiceBO>();
            List<PurchaseOrderDetailsForInvoiceBO> supplierInfo = new List<PurchaseOrderDetailsForInvoiceBO>();

            gatePassInfoInfo = gatePassDA.GetGatePassInfoForInvoice(gatePassId, supplierId);
            supplierInfo = purchaseDA.GetSupplierInformationForInvoice(supplierId);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], supplierInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], gatePassInfoInfo));
            rvTransaction.LocalReport.DisplayName = "Gate Pass";
            rvTransaction.LocalReport.Refresh();
        }
    }
}