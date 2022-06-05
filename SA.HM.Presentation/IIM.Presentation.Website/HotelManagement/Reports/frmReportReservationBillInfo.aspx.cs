using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
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
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportReservationBillInfo : System.Web.UI.Page
    {
        //**************************** Handlers ****************************//
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadReport();
            }

        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            int ReservationId = Convert.ToInt32(this.txtReservationNumber.Text);
            //--Service Bill Information----------------
            HMComplementaryItemDA entityDA = new HMComplementaryItemDA();
            List<HMComplementaryItemBO> entityBOList = entityDA.GetComplementaryItemInfoByReservationId(ReservationId);
            e.DataSources.Add(new ReportDataSource("ReservationComplementaryItem", entityBOList));


            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> PaymentInstuctionInfoList = new List<DocumentsBO>();
            PaymentInstuctionInfoList = docDA.GetPaymentInstuctionInfo();

            foreach (DocumentsBO row in PaymentInstuctionInfoList)
            {
                row.Path = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + row.Path + row.Name).Replace("?GuestBillInfo=2&isPreview=True", ""); 
            }
            e.DataSources.Add(new ReportDataSource("PaymentInstruction", PaymentInstuctionInfoList));
        }
        private void LoadReport()
        {
            Boolean IsPaymentInstuctionInfoList = false;
            this.Session["CurrentRegistrationId"] = null;
            string queryStringId = Request.QueryString["GuestBillInfo"];
            string APDInfoId = Request.QueryString["APDInfo"];
            bool isPreview = Convert.ToBoolean(Request.QueryString["isPreview"]);

            if (string.IsNullOrEmpty(queryStringId))
                return;

            long APDId = 0;
            if (!string.IsNullOrEmpty(APDInfoId))
            {
                APDId = Convert.ToInt64(APDInfoId);
            }

            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> PaymentInstuctionInfoList = new List<DocumentsBO>();
            PaymentInstuctionInfoList = docDA.GetPaymentInstuctionInfo();

            if (PaymentInstuctionInfoList != null)
            {
                if (PaymentInstuctionInfoList.Count > 0)
                {
                    IsPaymentInstuctionInfoList = true;
                }
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

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

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGenerateReservationBill.rdlc");

            HMCommonSetupBO isReservationConfimationOldCopyEnableBO = new HMCommonSetupBO();
            isReservationConfimationOldCopyEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsReservationConfimationOldCopyEnable", "IsReservationConfimationOldCopyEnable");
            if (isReservationConfimationOldCopyEnableBO != null)
            {
                if (isReservationConfimationOldCopyEnableBO.SetupValue == "1")
                {
                    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGenerateReservationBillOld.rdlc");
                }
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            int billID = Int32.Parse(queryStringId);
            this.txtReservationNumber.Text = queryStringId;

            HMCommonDA hmCommonDA = new HMCommonDA();
            List<ReportParameter> reportParam = new List<ReportParameter>();

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            int ReservationId = Convert.ToInt32(this.txtReservationNumber.Text);
            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO rUserInformationBO = userInformationDA.GetUserInformationByReservationId(ReservationId);

            //paramCLRegardsMessege information ---------------------------------
            string strParamCLRegardsMessege = string.Empty;
            strParamCLRegardsMessege = hmCommonDA.GetCustomFieldValueByFieldName("paramCLRegardsMessege");
            HMCommonSetupBO isDynamicBestRegardsForConfirmationLetterBO = new HMCommonSetupBO();
            isDynamicBestRegardsForConfirmationLetterBO = commonSetupDA.GetCommonConfigurationInfo("IsDynamicBestRegardsForConfirmationLetter", "IsDynamicBestRegardsForConfirmationLetter");

            if (isDynamicBestRegardsForConfirmationLetterBO != null)
            {
                if (isDynamicBestRegardsForConfirmationLetterBO.SetupId > 0)
                {
                    if (Convert.ToInt32(isDynamicBestRegardsForConfirmationLetterBO.SetupValue) > 0)
                    {
                        strParamCLRegardsMessege = "<div style='font-size: 12px '><b>With Warmest Regards </b></div>";
                        strParamCLRegardsMessege = strParamCLRegardsMessege + "<br><br>";
                        strParamCLRegardsMessege = strParamCLRegardsMessege + "--------------------------------";
                        strParamCLRegardsMessege = strParamCLRegardsMessege + "<div style='font-size: 12px '><b>" + rUserInformationBO.UserName + " </b> </div> ";
                        strParamCLRegardsMessege = strParamCLRegardsMessege + "<div style='font-size: 12px '><b>" + rUserInformationBO.UserDesignation + "</b></div>";
                        if (!string.IsNullOrWhiteSpace(rUserInformationBO.UserPhone))
                        {
                            if (!string.IsNullOrWhiteSpace(rUserInformationBO.UserEmail))
                            {
                                strParamCLRegardsMessege = strParamCLRegardsMessege + "<div style='font-size: 12px '><b> Phone: " + rUserInformationBO.UserPhone + ", Email: " + rUserInformationBO.UserEmail + " </b></div>";
                            }
                            else
                            {
                                strParamCLRegardsMessege = strParamCLRegardsMessege + "<div style='font-size: 12px '><b> Phone: " + rUserInformationBO.UserPhone + " </b></div>";
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(rUserInformationBO.UserEmail))
                        {
                            strParamCLRegardsMessege = strParamCLRegardsMessege + "<div style='font-size: 12px '><b> Email: " + rUserInformationBO.UserEmail + " </b></div>";
                        }

                        strParamCLRegardsMessege = strParamCLRegardsMessege + "<br>";
                    }
                }
            }

            //paramCLRegardsMessege information ---------------------------------
            string IsInclusiveHotelManagementBill = string.Empty;
            string strServiceChargeCitySDChargeVatAmount = string.Empty;
            string IsInclusiveHotelManagementBillString = string.Empty;

            List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");
            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.Count > 0)
                {
                    if (costCentreTabBO[0].IsVatSChargeInclusive.ToString() == "0")
                    {
                        IsInclusiveHotelManagementBill = "0";
                        IsInclusiveHotelManagementBillString = "exclusive";
                    }
                    else
                    {
                        IsInclusiveHotelManagementBill = "1";
                        IsInclusiveHotelManagementBillString = "inclusive";
                    }

                    if (costCentreTabBO[0].ServiceCharge > 0)
                    {
                        strServiceChargeCitySDChargeVatAmount = "Room Rates are Based on US Dollar and Above Price(s) is/are " + IsInclusiveHotelManagementBillString + " of ";
                        if (string.IsNullOrWhiteSpace(strServiceChargeCitySDChargeVatAmount))
                        {
                            strServiceChargeCitySDChargeVatAmount = strServiceChargeCitySDChargeVatAmount + costCentreTabBO[0].ServiceCharge.ToString() + "% Service Charge";
                        }
                        else
                        {
                            strServiceChargeCitySDChargeVatAmount = strServiceChargeCitySDChargeVatAmount + costCentreTabBO[0].ServiceCharge.ToString() + "% Service Charge";
                        }
                    }

                    if (costCentreTabBO[0].CitySDCharge > 0)
                    {
                        if (string.IsNullOrWhiteSpace(strServiceChargeCitySDChargeVatAmount))
                        {
                            strServiceChargeCitySDChargeVatAmount = "Room Rates are Based on US Dollar and Above Price(s) is/are " + IsInclusiveHotelManagementBillString + " of ";
                            strServiceChargeCitySDChargeVatAmount = strServiceChargeCitySDChargeVatAmount + costCentreTabBO[0].CitySDCharge.ToString() + "% City Charge";
                        }
                        else
                        {
                            strServiceChargeCitySDChargeVatAmount = strServiceChargeCitySDChargeVatAmount + " & " + costCentreTabBO[0].CitySDCharge.ToString() + "% City Charge";
                        }
                    }

                    if (costCentreTabBO[0].VatAmount > 0)
                    {
                        if (string.IsNullOrWhiteSpace(strServiceChargeCitySDChargeVatAmount))
                        {
                            strServiceChargeCitySDChargeVatAmount = "Room Rates are Based on US Dollar and Above Price(s) is/are " + IsInclusiveHotelManagementBillString + " of ";
                            strServiceChargeCitySDChargeVatAmount = strServiceChargeCitySDChargeVatAmount + costCentreTabBO[0].VatAmount.ToString() + "% VAT";
                        }
                        else
                        {
                            strServiceChargeCitySDChargeVatAmount = strServiceChargeCitySDChargeVatAmount + " & " + costCentreTabBO[0].VatAmount.ToString() + "% VAT";
                        }
                    }
                }
            }

            string strDefaultUsdToLocalCurrencyConversionRate = hmCommonDA.GetCustomFieldValueByFieldName("DefaultUsdToLocalCurrencyConversionRate");

            string isWeAcceptPanelHide = "0";
            HMCommonSetupBO commonSetupCountryIdBO = new HMCommonSetupBO();
            commonSetupCountryIdBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");
            if (commonSetupCountryIdBO.SetupValue != "19")
            {
                isWeAcceptPanelHide = "1";
            }

            // // IsRoomReservationConfirmationLetterWeAcceptHide Configuration
            string isRoomReservationConfirmationLetterWeAcceptHide = "0";
            HMCommonSetupBO IsRoomReservationConfirmationLetterWeAcceptHideBO = new HMCommonSetupBO();
            IsRoomReservationConfirmationLetterWeAcceptHideBO = commonSetupDA.GetCommonConfigurationInfo("IsRoomReservationConfirmationLetterWeAcceptHide", "IsRoomReservationConfirmationLetterWeAcceptHide");

            if (IsRoomReservationConfirmationLetterWeAcceptHideBO != null)
            {
                if (IsRoomReservationConfirmationLetterWeAcceptHideBO.SetupId > 0)
                {
                    isRoomReservationConfirmationLetterWeAcceptHide = IsRoomReservationConfirmationLetterWeAcceptHideBO.SetupValue;
                }
            }

            // // ReservationConfirmationLetterCheckInHours Configuration
            string ReservationConfirmationLetterCheckInHours = "13:00";
            HMCommonSetupBO reservationConfirmationLetterCheckInHours = new HMCommonSetupBO();
            reservationConfirmationLetterCheckInHours = commonSetupDA.GetCommonConfigurationInfo("ReservationConfirmationLetterCheckInHours", "ReservationConfirmationLetterCheckInHours");

            if (reservationConfirmationLetterCheckInHours != null)
            {
                if (reservationConfirmationLetterCheckInHours.SetupId > 0)
                {
                    ReservationConfirmationLetterCheckInHours = reservationConfirmationLetterCheckInHours.SetupValue;
                }
            }

            // // ReservationConfirmationLetterCheckOutHours Configuration
            string ReservationConfirmationLetterCheckOutHours = "12:00";
            HMCommonSetupBO reservationConfirmationLetterCheckOutHours = new HMCommonSetupBO();
            reservationConfirmationLetterCheckOutHours = commonSetupDA.GetCommonConfigurationInfo("ReservationConfirmationLetterCheckOutHours", "ReservationConfirmationLetterCheckOutHours");

            if (reservationConfirmationLetterCheckOutHours != null)
            {
                if (reservationConfirmationLetterCheckOutHours.SetupId > 0)
                {
                    ReservationConfirmationLetterCheckOutHours = reservationConfirmationLetterCheckOutHours.SetupValue;
                }
            }

            if (IsPaymentInstuctionInfoList)
            {
                isRoomReservationConfirmationLetterWeAcceptHide = "1";
            }

            // // Advance Amount Information
            RoomReservationDA reservationDA = new RoomReservationDA();
            ReservationBillPaymentBO paymentBO = new ReservationBillPaymentBO();
            paymentBO = reservationDA.GetReservationAdvanceTotalAmountInfoById(billID);
            reportParam.Add(new ReportParameter("ReservationAdvanceAmount", paymentBO.PaymentAmount.ToString()));
            reportParam.Add(new ReportParameter("IsWeAcceptPanelHide", isWeAcceptPanelHide));
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            string outletPathImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftOutletPathImageName");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("OutletImageInfo", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + outletPathImageName)));
            reportParam.Add(new ReportParameter("AppreciationMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLAppreciationMessege")));
            reportParam.Add(new ReportParameter("AdditionalServiceMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLAdditionalServiceMessege")));
            reportParam.Add(new ReportParameter("RoomReservationTermsAndConditions", hmCommonDA.GetCustomFieldValueByFieldName("RoomReservationTermsAndConditions")));
            reportParam.Add(new ReportParameter("CancellationPolicyMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLCancellationPolicyMessege")));
            reportParam.Add(new ReportParameter("RegardsMessege", strParamCLRegardsMessege));
            reportParam.Add(new ReportParameter("FooterImagePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramFooterMiddleImagePath"))));
            reportParam.Add(new ReportParameter("FooterAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramCLFooterAddress")));
            reportParam.Add(new ReportParameter("IsBigSizeCompanyLogo", hmCommonDA.GetCustomFieldValueByFieldName("IsBigSizeCompanyLogo")));
            reportParam.Add(new ReportParameter("IsInclusiveHotelManagementBill", IsInclusiveHotelManagementBill));
            reportParam.Add(new ReportParameter("RoomRateInclusiveOrExclusiveInfo", strServiceChargeCitySDChargeVatAmount));
            reportParam.Add(new ReportParameter("DefaultUsdToLocalCurrencyConversionRate", strDefaultUsdToLocalCurrencyConversionRate));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            reportParam.Add(new ReportParameter("ReservationConfirmationLetterCheckInHours", ReservationConfirmationLetterCheckInHours));
            reportParam.Add(new ReportParameter("ReservationConfirmationLetterCheckOutHours", ReservationConfirmationLetterCheckOutHours));
            reportParam.Add(new ReportParameter("IsRoomReservationConfirmationLetterWeAcceptHide", isRoomReservationConfirmationLetterWeAcceptHide));

            int isReservationConfirmationLetterOutletImageDisplay = 0;
            HMCommonSetupBO IsReservationConfirmationLetterOutletImageDisplayBO = new HMCommonSetupBO();
            IsReservationConfirmationLetterOutletImageDisplayBO = commonSetupDA.GetCommonConfigurationInfo("IsReservationConfirmationLetterOutletImageDisplay", "IsReservationConfirmationLetterOutletImageDisplay");
            if (IsReservationConfirmationLetterOutletImageDisplayBO != null)
            {
                if (IsReservationConfirmationLetterOutletImageDisplayBO.SetupValue == "1")
                {
                    isReservationConfirmationLetterOutletImageDisplay = 1;
                }
            }
            reportParam.Add(new ReportParameter("IsReservationConfirmationLetterOutletImageDisplay", isReservationConfirmationLetterOutletImageDisplay.ToString()));

            int isDetailsRoomInfoShowInReservationConfirmationLetter = 0;
            HMCommonSetupBO IsDetailsRoomInfoShowInReservationConfirmationLetterBO = new HMCommonSetupBO();
            IsDetailsRoomInfoShowInReservationConfirmationLetterBO = commonSetupDA.GetCommonConfigurationInfo("IsDetailsRoomInfoShowInReservationConfirmationLetter", "IsDetailsRoomInfoShowInReservationConfirmationLetter");
            if (IsDetailsRoomInfoShowInReservationConfirmationLetterBO != null)
            {
                if (IsDetailsRoomInfoShowInReservationConfirmationLetterBO.SetupValue == "1")
                {
                    isDetailsRoomInfoShowInReservationConfirmationLetter = 1;
                }
            }
            reportParam.Add(new ReportParameter("IsDetailsRoomInfoShowInReservationConfirmationLetter", isDetailsRoomInfoShowInReservationConfirmationLetter.ToString()));

            rvTransaction.LocalReport.SetParameters(reportParam);

            RoomReservationDA reservationDa = new RoomReservationDA();
            List<RoomReservationBillBO> reservationBill = new List<RoomReservationBillBO>();
            if (APDId == 0)
            {
                reservationBill = reservationDa.GetRoomReservationBill(billID);
            }
            else
            {
                reservationBill = reservationDa.GetRoomReservationBill(billID).Where(x => x.APDId == APDId).ToList();
            }     

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], reservationBill));

            rvTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);

            rvTransaction.LocalReport.DisplayName = "Reservation Confirmation Letter";
            rvTransaction.LocalReport.Refresh();

            //Send mail
            if (!isPreview)
                SendMail(queryStringId);
            
        }


        /// <summary>
        /// Sms Send
        /// </summary>
        
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
        protected void btnEmailSend_Click(object sender, EventArgs e)
        {
            string queryStringId = Request.QueryString["GuestBillInfo"];
            SendMail(queryStringId);
        }

        private bool SendMail(string queryStringId)
        {
            bool status = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsRoomReservationEmailAutoPostingEnable");
            string IsEnable = commonSetupBO.SetupValue;
            if (IsEnable == "0")
                return status;

            int reservationId = 0;

            if (!String.IsNullOrEmpty(queryStringId))
            {
                reservationId = Convert.ToInt32(queryStringId);
            }
            List<RoomReservationBillBO> roomReservationBO = new List<RoomReservationBillBO>();
            RoomReservationDA roomReservationDA = new RoomReservationDA();

            roomReservationBO = roomReservationDA.GetRoomReservationBill(reservationId);

            string ReceivingMail = roomReservationBO[0].ContactEmail;
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
                string totalRoomType = string.Empty;
                int numberOfNight = 0;
                foreach (var item in roomReservationBO)
                {
                    totalRoomType += HMUtility.ConvertNumberToString((int)item.TypeWiseTotalRooms) + " " + "(" + item.TypeWiseTotalRooms + ")" + " " + item.RoomType + ",";
                    numberOfNight += (int)item.RoomOfNights * (int)item.TypeWiseTotalRooms;
                }

                totalRoomType = totalRoomType.Remove(totalRoomType.Length - 1);
                string[] dataArray = mainString.Split('~');

                HMCommonDA hmCommonDA = new HMCommonDA();

                var tokens = new Dictionary<string, string>
                         {
                             {"NAME", roomReservationBO[0].GuestList},
                             {"COMPANYNAME",roomReservationBO[0].CompanyName },
                             {"RESERVATIONNUMBER",roomReservationBO[0].ReservationNumber },
                             {"NUMBEROFROOMSWITHROOMTYPE",totalRoomType},
                             {"PAX",roomReservationBO[0].TotalGuest.ToString()},
                             {"NIGHTSTAYNO",numberOfNight.ToString()},
                             {"DATE",roomReservationBO[0].StringArrivalDate},
                             {"RegardsMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLRegardsMessege")}
                         };
                email = new Email()
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = ReceivingMail,
                    Subject = "Room Reservation Letter",
                    AttachmentSavedPath = filePath,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    TempleteName = HMConstants.EmailTemplates.ReservationConfirmation,
                    FromDisplayName = roomReservationBO[0].CompanyAddress
                };



                try
                {
                    status = EmailHelper.SendEmail(email, tokens);
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
