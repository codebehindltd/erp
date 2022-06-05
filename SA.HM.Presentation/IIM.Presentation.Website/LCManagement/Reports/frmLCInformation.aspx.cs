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
    public partial class frmLCInformation : Page
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string pLCId = Request.QueryString["LCId"];

            if (!String.IsNullOrEmpty(pLCId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(pLCId));
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
        //protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        //{
        //    HMUtility hmUtility = new HMUtility();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //    try
        //    {
        //        Warning[] warnings;
        //        string[] streamids;
        //        string mimeType;
        //        string encoding;
        //        string extension;

        //        byte[] bytes = rvTransaction.LocalReport.Render("PDF", null, out mimeType,
        //                       out encoding, out extension, out streamids, out warnings);

        //        string fileName = string.Empty, fileNamePrint = string.Empty;
        //        DateTime dateTime = DateTime.Now;
        //        fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
        //        fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

        //        FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
        //        fs.Write(bytes, 0, bytes.Length);
        //        fs.Close();

        //        //Open exsisting pdf
        //        Document document = new Document(PageSize.LETTER);
        //        PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
        //        //Getting a instance of new pdf wrtiter
        //        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
        //           HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
        //        document.Open();
        //        PdfContentByte cb = writer.DirectContent;

        //        int i = 0;
        //        int p = 0;
        //        int n = reader.NumberOfPages;
        //        Rectangle psize = reader.GetPageSize(1);

        //        float width = psize.Width;
        //        float height = psize.Height;

        //        //Add Page to new document
        //        while (i < n)
        //        {
        //            document.NewPage();
        //            p++;
        //            i++;

        //            PdfImportedPage page1 = writer.GetImportedPage(reader, i);
        //            cb.AddTemplate(page1, 0, 0);
        //        }

        //        //Attach javascript to the document
        //        PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
        //        //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
        //        writer.AddJavaScript(jAction);

        //        document.Close();

        //        frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void LoadReport(int lcId)
        {
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            string lCType = string.Empty;

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/LCManagement/Reports/Rdlc/rptLCDetailsInfo.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            List<ReportParameter> reportParam = new List<ReportParameter>();

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

            string reportName = "LC Details Information";

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetStringFromDateTime(currentDate) + " " + currentDate.ToString("hh:mm:ss tt");
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            LCInformationDA reqDA = new LCInformationDA();
            LCInformationViewBO ViewBO = new LCInformationViewBO();
            ViewBO = reqDA.GetLCInformationALlDetailsByLCNumber(lcId);

            if (ViewBO != null)
            {
                if (ViewBO.LCInformation.LCId > 0)
                {
                    List<LCReportViewBO> lcInfo = new List<LCReportViewBO>();
                    lcInfo = reqDA.GetLCDetailsReportInfo("LCInformation", ViewBO.LCInformation.LCNumber);

                    List<LCReportViewBO> LCInformationDetail = new List<LCReportViewBO>();
                    LCInformationDetail = reqDA.GetLCDetailsReportInfo("LCInformationDetail", ViewBO.LCInformation.LCNumber);

                    List<LCReportViewBO> LCPayment = new List<LCReportViewBO>();
                    LCPayment = reqDA.GetLCDetailsReportInfo("LCPayment", ViewBO.LCInformation.LCNumber);

                    List<LCReportViewBO> PMProductReceived = new List<LCReportViewBO>();
                    PMProductReceived = reqDA.GetLCDetailsReportInfo("PMProductReceived", ViewBO.LCInformation.LCNumber);

                    List<LCReportViewBO> LCOverHeadExpense = new List<LCReportViewBO>();
                    LCOverHeadExpense = reqDA.GetLCDetailsReportInfo("LCOverHeadExpense", ViewBO.LCInformation.LCNumber);

                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], LCInformationDetail));
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], PMProductReceived));
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], LCPayment));
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[3], LCOverHeadExpense));
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[4], lcInfo));
                }
            }

            rvTransaction.LocalReport.DisplayName = "" + reportName + " Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
    }
}