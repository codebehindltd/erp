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
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.RetailPOS;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportBillForRetailPos : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillInvoiceTemplate", "RestaurantBillInvoiceTemplate");
                if (invoiceTemplateBO != null)
                {
                    if (invoiceTemplateBO.SetupId > 0)
                    {
                        if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 1)
                        {
                            hfBillTemplate.Value = "1";
                            this.ReportProcessing("rptRetailPosBill");
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 2)
                        {
                            hfBillTemplate.Value = "2";
                            this.ReportProcessing("rptRestaurentBillForDotMatrix");
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 3)
                        {
                            hfBillTemplate.Value = "3";
                            this.ReportProcessing("rptRestaurentBillTwoColumn");
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 4)
                        {
                            hfBillTemplate.Value = "4";
                            this.ReportProcessingForPosToken("rptRestaurentBillForPosToken");
                        }
                    }
                }
            }
        }

        // Template 1 -- POS, Template 2---- Dot Matrix, Template 3 -- Double Column

        private void ReportProcessing(string reportName)
        {
            int billID = 0;
            string queryStringId = string.Empty, kotId = string.Empty;
            queryStringId = Request.QueryString["billId"];

            if (Request.QueryString["kotId"] != null)
                kotId = Request.QueryString["kotId"];

            string queryRePrint = "0";
            if (Request.QueryString["RePrint"] != null)
                queryRePrint = Request.QueryString["RePrint"];

            RestaurentBillDA rda = new RestaurentBillDA();

            if (!string.IsNullOrEmpty(kotId))
            {
                KotBillDetailDA entityDA = new KotBillDetailDA();
                Boolean status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(Convert.ToInt32(kotId));
            }

            if (queryStringId.Take(2).All(char.IsDigit))
            {
                billID = Int32.Parse(queryStringId);
            }
            else
            {
                billID = rda.GetBillPaymentByBillId(queryStringId);
            }

            RestaurentPosDA posda = new RestaurentPosDA();
            RestaurantBillBO billBO = new RestaurantBillBO();
            billBO = posda.GetBillInfoForRetailPosByBillId(billID);

            if (billBO.BillId == 0)
            {
                lblReportMessage.Enabled = true;
                lblReportMessage.Text = "Invalid Bill Number.";
                return;
            }
            else
            {
                lblReportMessage.Enabled = false;
            }

            if (billBO.ReturnBillId > 0)
            {
                BillReturnPreview(billID, billBO, "rptRetailPosReturnBill");
            }
            else
            {
                BillPreview(billID, billBO, reportName, queryRePrint);
            }

            rvTransaction.LocalReport.DisplayName = "Restaurant Bill";
            rvTransaction.LocalReport.Refresh();
        }

        private void ReportProcessingForPosToken(string reportName)
        {
            string queryStringId = Request.QueryString["billId"];
            int billID = Int32.Parse(queryStringId);

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.DataSources.Clear();

                var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                RestaurentPosDA posda = new RestaurentPosDA();
                RestaurantBillBO billBO = new RestaurantBillBO();
                billBO = posda.GetBillInfoForRetailPosByBillId(billID);

                if (billBO.IsBillReSettlement)
                {

                }
                else
                {
                    BillPreview(billID, billBO, reportName, string.Empty);
                }

                rvTransaction.LocalReport.DisplayName = "Restaurant Bill";
                rvTransaction.LocalReport.Refresh();
            }
        }

        private void BillPreview(int billID, RestaurantBillBO billBO, string reportName, string queryRePrint)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.DataSources.Clear();

            var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
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
                reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }

                reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
            }

            rvTransaction.LocalReport.EnableExternalImages = true;
            HMCommonDA hmCommonDA = new HMCommonDA();
            //reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));

            string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
            if (!string.IsNullOrWhiteSpace(imageName))
            {
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            }
            else
            {
                reportParam.Add(new ReportParameter("Path", "Hide"));
            }

            reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
            reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
            reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

            reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
            isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
            if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
            {
                reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
            }
            else
            {
                reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
            }

            string IsCostCenterNameShowOnInvoice = "1";
            reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

            //reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
            //reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
            //reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));

            DateTime currentDate = DateTime.Now;
            HMCommonDA printDateDA = new HMCommonDA();
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            RestaurentPosDA rda = new RestaurentPosDA();
            RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
            restaurantBill = rda.RetailPosBill(billID);

            var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill.PosBillWithSalesReturn));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], restaurantBill.PosSalesReturnPayment));
        }
        public void BillReturnPreview(int billID, RestaurantBillBO billBO, string reportName)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.DataSources.Clear();

            var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
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
                reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }

                reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
            }

            rvTransaction.LocalReport.EnableExternalImages = true;
            HMCommonDA hmCommonDA = new HMCommonDA();
            //reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));

            string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
            if (!string.IsNullOrWhiteSpace(imageName))
            {
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            }
            else
            {
                reportParam.Add(new ReportParameter("Path", "Hide"));
            }

            reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
            reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
            reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

            reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
            isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
            if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
            {
                reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
            }
            else
            {
                reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
            }

            //reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
            //reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
            //reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));

            DateTime currentDate = DateTime.Now;
            HMCommonDA printDateDA = new HMCommonDA();
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            RestaurentPosDA rda = new RestaurentPosDA();
            RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
            restaurantBill = rda.GetRetailPosBillWithSalesReturn(billID);

            var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill.PosBillWithSalesReturn));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], restaurantBill.PosSalesReturnItem));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[2], restaurantBill.PosSalesReturnPayment));
        }
        private void IsRestaurantOrderSubmitDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantOrderSubmitDisable", "IsRestaurantOrderSubmitDisable");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        //this.btnOrderSubmit.Visible = false;
                        IsRestaurantOrderSubmitDisable = false;
                    }
                    else
                    {
                        //this.btnOrderSubmit.Visible = true;
                        IsRestaurantOrderSubmitDisable = true;
                    }
                }
            }

            //this.imgBtnRoomWiseKotBill.Visible = false;
        }
        private void IsRestaurantTokenInfoDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantTokenInfoDisable", "IsRestaurantTokenInfoDisable");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        //this.btnOrderSubmit.Visible = false;
                        IsRestaurantTokenInfoDisable = false;
                    }
                    else
                    {
                        //this.btnOrderSubmit.Visible = true;
                        IsRestaurantTokenInfoDisable = true;
                    }
                }
            }

            //this.imgBtnRoomWiseKotBill.Visible = false;
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
            //var doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);

            Document document = new Document(pgSize, 0f, 0f, 0f, 0f); //PageSize.A5.Rotate()
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));

            //Getting a instance of new pdf wrtiter
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContentUnder;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;

            //Rectangle psize = reader.GetPageSizeWithRotation(1);

            //float width = Utilities.InchesToPoints(3.5f);
            //float height = Utilities.InchesToPoints(8.5f);

            //iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(width, height);
            //document.SetMargins(0f, 0f, 0f, 0f);
            //document.SetPageSize(psize);
            //Add Page to new document

            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                //cb.AddTemplate(page1, 0, 1, -1, 0, page1.Width, 0); //270
                //cb.AddTemplate(page1, -1f, 0, 0, -1f, page1.Width, page1.Height); //180
                //cb.AddTemplate(page1, 0, -1f, 1f, 0, 0, page1.Height);

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

            //Open exsisting pdf
            var pgSize = new Rectangle(396.0f, 612.0f);
            //var doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);

            Document document = new Document(PageSize.LETTER.Rotate(), 0f, 0f, 0f, 0f); //PageSize.A5.Rotate()
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
            //Getting a instance of new pdf wrtiter
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContentUnder;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;

            //Rectangle psize = reader.GetPageSizeWithRotation(1);

            //float width = Utilities.InchesToPoints(3.5f);
            //float height = Utilities.InchesToPoints(8.5f);

            //iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(width, height);
            //document.SetMargins(0f, 0f, 0f, 0f);
            //document.SetPageSize(psize);
            //Add Page to new document

            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                //cb.AddTemplate(page1, 0, 1, -1, 0, page1.Width, 0); //270
                //cb.AddTemplate(page1, -1f, 0, 0, -1f, page1.Width, page1.Height); //180
                //cb.AddTemplate(page1, 0, -1f, 1f, 0, 0, page1.Height);

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