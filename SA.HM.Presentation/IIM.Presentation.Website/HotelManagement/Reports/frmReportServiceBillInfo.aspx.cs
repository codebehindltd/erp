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
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Presentation.Website.Common.SDCTool;

namespace HotelManagement.Presentation.Website.Restaurant.Reports
{
    public partial class frmReportServiceBillInfo : System.Web.UI.Page
    {
        //**************************** Handlers ****************************//
        //**************************** Handlers ****************************//
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int invoiceTemplate = 1;
                List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("ServiceBill");
                if (costCentreTabBO != null)
                {
                    if (costCentreTabBO.Count > 0)
                    {
                        invoiceTemplate = costCentreTabBO[0].InvoiceTemplate;
                    }
                }
                
                if (invoiceTemplate == 1)
                {
                    hfBillTemplate.Value = "1";
                    this.ReportProcessing("rptGuestHouseServiceBillForPos");
                }
                else if (invoiceTemplate == 2)
                {
                    hfBillTemplate.Value = "2";
                    this.ReportProcessing("rptGuestHouseServiceBillForDotMatrix");
                }
                else if (invoiceTemplate == 3)
                {
                    hfBillTemplate.Value = "3";
                    this.ReportProcessing("rptGuestHouseServiceBillTwoColumn");
                }
                else if (invoiceTemplate == 4)
                {
                    hfBillTemplate.Value = "4";
                    this.ReportProcessing("rptGuestHouseServiceBillForA4");
                }


                //HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                //HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                //invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("ServiceBillInvoiceTemplate", "ServiceBillInvoiceTemplate");

                //if (invoiceTemplateBO != null)
                //{
                //    if (invoiceTemplateBO.SetupId > 0)
                //    {

                //    }
                //}                
            }
        }

        // Template 1 -- POS, Template 2---- Dot Matrix, Template 3 -- Double Column
        private void ReportProcessing(string reportName)
        {
            string queryStringId = Request.QueryString["billId"];
            string queryIsSdcEnable = Request.QueryString["sdc"];
            int billID = Int32.Parse(queryStringId);

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.DataSources.Clear();
                //queryIsSdcEnable = "0";
                var reportPath = "";
                if(queryIsSdcEnable == "1")
                {
                    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/" + reportName + "WithQRCode.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/" + reportName + ".rdlc");
                }
                //var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (files[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
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

                rvTransaction.LocalReport.EnableExternalImages = true;
                HMCommonDA hmCommonDA = new HMCommonDA();
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                
                // //----------------- Show Hide Related Information -------------------------------------------------------
                string IsServiceChargeEnableConfig = "1";
                string IsCitySDChargeEnableConfig = "1";
                string IsVatAmountEnableConfig = "1";
                string IsAdditionalChargeEnableConfig = "1";

                List <CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("ServiceBill");

                if (costCentreTabBO.Count > 0)
                {
                    IsServiceChargeEnableConfig = costCentreTabBO[0].IsServiceChargeEnable ? "1" : "0";
                    IsCitySDChargeEnableConfig = costCentreTabBO[0].IsCitySDChargeEnable ? "1" : "0";
                    IsVatAmountEnableConfig = costCentreTabBO[0].IsVatEnable ? "1" : "0";
                    IsAdditionalChargeEnableConfig = costCentreTabBO[0].IsAdditionalChargeEnable ? "1" : "0";
                }

                reportParam.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
                reportParam.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
                reportParam.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
                reportParam.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));
                
                
                GHServiceBillDA rda = new GHServiceBillDA();
                List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                restaurantBill = rda.GetGuestHouseServiceBillInfoByServiceBillIdForReport(billID);

                if (queryIsSdcEnable == "1")
                {
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    //
                    SdcInvoiceHandler sdcInvHandler = new SdcInvoiceHandler("ServiceBill");

                    List<SdcBillReportBO> billReportsBo = new List<SdcBillReportBO>();
                    foreach(RestaurantBillReportBO reportBo in restaurantBill)
                    {
                        SdcBillReportBO bo = new SdcBillReportBO();
                        bo.ItemId = reportBo.ItemId;
                        bo.ItemCode = reportBo.ItemCode;
                        bo.HsCode = "";
                        bo.ItemName = reportBo.ItemName;
                        bo.UnitRate = reportBo.UnitRate;
                        bo.PaxQuantity = reportBo.PaxQuantity;
                        
                        if (reportBo.CitySDCharge > 0)
                        {
                            bo.SdCategory = "13701";
                        }
                        else
                        {
                            bo.SdCategory = "16051";
                        }

                        bo.VatCategory = "13651";

                        billReportsBo.Add(bo);
                    }

                    sdcInvHandler.SdcInvoiceProcess(userInformationBO, billID, billReportsBo);

                    while (!sdcInvHandler.IsInvoiceReceived)
                    {
                        //Wait Until the invoice received from the NBR Server through the SDC Integrated device. This is just a blank while loop because i have to wait 
                        //for the response of SDC Device Event what i have fired inside the SdcInvoiceProcess above.
                        //After receiving the response I will call the ProcessReport method bellow.
                    }

                    if (sdcInvHandler.IsDeviceConnected)
                    {
                        //this.ProcessReport(userInformationBO);
                        String SDCInvoiceNumber = string.Empty;
                        String SDCQRCode = string.Empty;
                        HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                        if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
                        {

                            if (queryIsSdcEnable == "1")
                            {
                                RestaurentBillDA sdc_rda = new RestaurentBillDA();
                                RestaurantBillBO RestaurantBillBOSDCInfo = new RestaurantBillBO();
                                RestaurantBillBOSDCInfo = sdc_rda.GetSDCInfoviceInformation(billID);
                                if (RestaurantBillBOSDCInfo.BillId > 0)
                                {
                                    SDCInvoiceNumber = RestaurantBillBOSDCInfo.SDCInvoiceNumber;
                                    SDCQRCode = RestaurantBillBOSDCInfo.QRCode;

                                    byte[] QrImage;
                                    HMCommonDA DA = new HMCommonDA();
                                    QrImage = DA.GenerateQrCode(SDCQRCode);
                                    reportParam.Add(new ReportParameter("SDCInvoiceNumber", SDCInvoiceNumber));
                                    reportParam.Add(new ReportParameter("QRCode", Convert.ToBase64String(QrImage)));
                                }
                                //SDCQRCode = @"http://efdnbr.gov.bd/verify?param=Z01200200046.002020XWBQGEY034.TAr5odmwlLh9C%2F%2BLGDYXbFtcSeqGSfUugBeIYKTPTDuScyaNZojLOLFUsnRiPJdU";
                            }
                        }
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("SDCInvoiceNumber", ""));
                        reportParam.Add(new ReportParameter("QRCode", ""));
                        //this.ProcessReportWithoutSDCIntegration(userInformationBO);
                    }
                }

                rvTransaction.LocalReport.SetParameters(reportParam);

                var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                rvTransaction.LocalReport.DisplayName = "Service Bill";
                rvTransaction.LocalReport.Refresh();
            }
        }
        
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rvTransaction.LocalReport.Render("PDF", null, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/output.pdf"), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            //Open exsisting pdf
            Document document = new Document(PageSize.LETTER);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/output.pdf"));
            //Getting a instance of new pdf wrtiter
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath("~/PrintDocument/Print.pdf"), FileMode.Create));
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

            frmPrint.Attributes["src"] = "../../PrintDocument/Print.pdf";
        }

        //Pos Printing
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

            var pgSize = new Rectangle(396.0f, 612.0f);
            Document document = new Document(PageSize.LETTER); //PageSize.A5.Rotate()

            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));

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

        //Dot Matrix
        protected void btnPrintReportTemplate2_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>PDF</OutputFormat>
                <PageWidth>5.5in</PageWidth>
                <PageHeight>8.5in</PageHeight>
                <MarginTop>0.0in</MarginTop>
                <MarginLeft>0.0in</MarginLeft>
                <MarginRight>0.0in</MarginRight>
                <MarginBottom>0.0in</MarginBottom>
            </DeviceInfo>";

            byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileName = string.Empty, fileNamePrint = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            var pgSize = new Rectangle(396.0f, 612.0f);
            Document document = new Document(pgSize, 0f, 0f, 0f, 0f); //PageSize.A5.Rotate()

            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));

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

        //Double Column
        protected void btnPrintReportTemplate3_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>PDF</OutputFormat>
                <PageWidth>11in</PageWidth>
                <PageHeight>8.5in</PageHeight>
                <MarginTop>0.0in</MarginTop>
                <MarginLeft>0.0in</MarginLeft>
                <MarginRight>0.0in</MarginRight>
                <MarginBottom>0.0in</MarginBottom>
            </DeviceInfo>";

            byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileName = string.Empty, fileNamePrint = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            var pgSize = new Rectangle(396.0f, 612.0f);
            Document document = new Document(PageSize.LETTER.Rotate(), 0f, 0f, 0f, 0f); //PageSize.A5.Rotate()

            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));

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

        //A4 Size
        protected void btnPrintReportTemplate4_Click(object sender, EventArgs e)
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