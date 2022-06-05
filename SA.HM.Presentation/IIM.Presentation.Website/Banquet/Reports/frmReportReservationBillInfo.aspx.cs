using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Presentation.Website.Common.SDCTool;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;

namespace HotelManagement.Presentation.Website.Banquet.Reports
{
    public partial class frmReportReservationBillInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryStringId = Request.QueryString["Id"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    GenerateReport(Convert.ToInt32(queryStringId));
                }
            }
        }
        private void GenerateReport(int reservationId)
        {
            try
            {
                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.EnableExternalImages = true;

                var reportPath = "";
                string queryIsSdcEnable = Request.QueryString["sdc"];
                if (!string.IsNullOrEmpty(queryIsSdcEnable) && queryIsSdcEnable == "1")
                {
                    reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/RptReportReservationBillInfoWithQRCode.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/RptReportReservationBillInfo.rdlc");
                }

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
                    reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
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

                string footerPoweredByInfo = string.Empty;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                HMCommonDA hmCommonDA = new HMCommonDA();
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                reportParam.Add(new ReportParameter("AppreciationMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLAppreciationMessege")));
                reportParam.Add(new ReportParameter("FooterImagePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramFooterMiddleImagePath"))));
                reportParam.Add(new ReportParameter("CancellationPolicyMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLCancellationPolicyMessege")));
                reportParam.Add(new ReportParameter("AdditionalServiceMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLAdditionalServiceMessege")));
                reportParam.Add(new ReportParameter("RegardsMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLRegardsMessege")));
                reportParam.Add(new ReportParameter("FooterAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramCLFooterAddress")));
                reportParam.Add(new ReportParameter("IsBigSizeCompanyLogo", hmCommonDA.GetCustomFieldValueByFieldName("IsBigSizeCompanyLogo")));
                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                string banquetTermsAndConditions = hmCommonDA.GetCustomFieldValueByFieldName("BanquetTermsAndConditions");
                reportParam.Add(new ReportParameter("BanquetTermsAndConditions", banquetTermsAndConditions));

                BanquetReservationBO reservationBO = new BanquetReservationBO();
                BanquetReservationDA reservationDA = new BanquetReservationDA();
                reservationBO = reservationDA.GetBanquetReservationInfoById(reservationId);
                if (reservationBO != null)
                {
                    reportParam.Add(new ReportParameter("ReservationMode", reservationBO.ReservationMode));
                }
                BanquetBillPaymentDA banquetDa = new BanquetBillPaymentDA();
                List<BanquetReservationBillGenerateReportBO> BanquetReservationBill = new List<BanquetReservationBillGenerateReportBO>();
                BanquetReservationBill = banquetDa.GetBanquetReservationBillGenerateReport(reservationId);

                if (!string.IsNullOrEmpty(queryIsSdcEnable) && queryIsSdcEnable == "1")
                {
                    List<BanquetReservationBillGenerateReportBO> BanquetBillInfoBO = new List<BanquetReservationBillGenerateReportBO>();
                    BanquetBillInfoBO = BanquetReservationBill.Where(x => x.ItemType != "Payments").ToList();
                    SdcInvoiceHandler sdcInvHandler = new SdcInvoiceHandler("BanquetBill");

                    List<SdcBillReportBO> billReportsBo = new List<SdcBillReportBO>();
                    foreach (BanquetReservationBillGenerateReportBO reportBo in BanquetBillInfoBO)
                    {
                        SdcBillReportBO bo = new SdcBillReportBO();
                        bo.ItemId = (int)reportBo.ItemId;
                        bo.ItemCode = "" + reportBo.ItemUnit;
                        bo.HsCode = "";
                        bo.ItemName = reportBo.ItemName;
                        bo.UnitRate = reportBo.UnitPrice;
                        bo.PaxQuantity = reportBo.NumberOfPersonAdult;
                        
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

                    sdcInvHandler.SdcInvoiceProcess(userInformationBO, reservationId, billReportsBo);

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
                                RestaurantBillBOSDCInfo = sdc_rda.GetSDCInfoviceInformation(reservationId);
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

                foreach (BanquetReservationBillGenerateReportBO row in BanquetReservationBill)
                {
                    row.IsFNBPanelVisible = 1;
                    row.IsRequisitesPanelVisible = 1;
                    reportParam.Add(new ReportParameter("InnboardVatAmount", row.InnboardVatAmount));
                    reportParam.Add(new ReportParameter("InnboardServiceChargeAmount", row.InnboardServiceChargeAmount));
                }
                rvTransaction.LocalReport.SetParameters(reportParam);
                var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], BanquetReservationBill));
                rvTransaction.LocalReport.DisplayName = "Banquet Invoice";
                rvTransaction.LocalReport.Refresh();

                SendMail(reservationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SendMail(int reservationId)
        {
            bool status = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            RoomReservationBO roomReservationBO = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();

            roomReservationBO = roomReservationDA.GetRoomReservationInfoByIdNew(reservationId);

            string ReceivingMail = roomReservationBO.ContactEmail;
            if (string.IsNullOrWhiteSpace(ReceivingMail))
                return status;
            
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
            fileName = "BR" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            filePath = HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName);
            if (!File.Exists(filePath))
                return false;
            //Send Mail            

            string MailBody = "Please find the attachment.";

            Email email;
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;

            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                email = new Email()
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = ReceivingMail,
                    Subject = "Banquet Settlement Invoice",
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