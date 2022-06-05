using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Restaurant;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using HotelManagement.Presentation.Website.Common.SDCTool;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestBillInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int IsInvoiceTemplate1Visible = -1;
        string GuestBillRoomIdParameterValue = string.Empty;
        string GuestBillServiceIdParameterValue = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadGuestBillPreview();
            }
        }
        //************************ User Defined Function ********************//
        private void LoadGuestBillPreview()
        {
            if (this.Session["CheckOutRegistrationIdList"] != null)
            {
                string registrationId = this.Session["CheckOutRegistrationIdList"].ToString();
                //this.LoadRoomGridView(registrationId);
                //this.LoadServiceGridView(registrationId);
                //this.SavePendingGuestRoomInformation();
                //this.LoadBillPrintPreviewDynamicallyForReport(registrationId);
                this.ReportProcessing();

                bool isCheckOut = Convert.ToBoolean(Request.QueryString["IsCheckOut"]);
                if (isCheckOut)
                    SendMail(registrationId);
            }
        }
        private void LoadBillPrintPreviewDynamicallyForReport(string registrationId)
        {
            HMUtility hmUtility = new HMUtility();
            HttpContext.Current.Session["CheckOutRegistrationIdList"] = registrationId;
            string currencyRate = "0.00";
            string isIsplite = "0";
            string serviceType = "";
            string SelectdIndividualTransferedPaymentId = "0";
            string SelectdPaymentId = "0";
            string SelectdIndividualPaymentId = "0";
            string SelectdIndividualServiceId = "0";
            string SelectdIndividualRoomId = "0";
            string SelectdServiceId = "0";
            string SelectdRoomId = "0";
            string StartDate = hmUtility.GetFromDate();
            string EndDate = hmUtility.GetToDate();
            string ddlRegistrationId = registrationId;
            string txtSrcRegistrationIdList = registrationId;

            HttpContext.Current.Session["IsBillSplited"] = "0";
            HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
            HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();

            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);

        }
        private void ReportProcessing()
        {
            if (this.Session["CheckOutRegistrationIdList"] != null)
            {
                int frontOfficeInvoiceTemplate = 1;
                int isCashierNameShownInInvoice = 1;
                string footerPoweredByInfo = string.Empty;
                this.txtRegistrationNumber.Text = string.Empty;
                this.txtCompanyName.Text = string.Empty;
                this.txtCompanyAddress.Text = string.Empty;
                this.txtCompanyWeb.Text = string.Empty;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.txtPrintedBy.Text = userInformationBO.UserName;
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                HMCommonDA hmCommonDA = new HMCommonDA();
                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();

                if (files[0].CompanyId > 0)
                {
                    this.txtCompanyName.Text = files[0].CompanyName;
                    this.txtCompanyAddress.Text = files[0].CompanyAddress;
                    this.txtVatRegistrationNo.Text = files[0].VatRegistrationNo;
                    this.txtGroupCompanyName.Text = files[0].GroupCompanyName;
                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        this.txtCompanyWeb.Text = files[0].WebAddress;
                    }
                    else
                    {
                        this.txtCompanyWeb.Text = files[0].ContactNumber;
                    }
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

                            CommonHelper.DisableReportExportFormat(rvInvoiceTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
                            CommonHelper.DisableReportExportFormat(rvInvoiceTransaction, CommonHelper.ReportImportFormat.Word.ToString());
                        }
                    }
                }

                this.txtRegistrationNumber.Text = this.Session["CheckOutRegistrationIdList"].ToString();
                this.txtIsBillSplited.Text = this.Session["IsBillSplited"].ToString();
                this.txtGuestBillFromDate.Text = this.Session["GuestBillFromDate"].ToString();
                this.txtGuestBillToDate.Text = this.Session["GuestBillToDate"].ToString();

                HMCommonSetupBO frontOfficeInvoiceTemplateBO = new HMCommonSetupBO();
                frontOfficeInvoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeInvoiceTemplate", "FrontOfficeInvoiceTemplate");
                if (frontOfficeInvoiceTemplateBO != null)
                {
                    frontOfficeInvoiceTemplate = Convert.ToInt32(frontOfficeInvoiceTemplateBO.SetupValue);
                }

                //IsCashierNameShownInInvoice
                HMCommonSetupBO IsCashierNameShownInInvoiceBO = new HMCommonSetupBO();
                IsCashierNameShownInInvoiceBO = commonSetupDA.GetCommonConfigurationInfo("IsCashierNameShownInInvoice", "IsCashierNameShownInInvoice");
                if (IsCashierNameShownInInvoiceBO != null)
                {
                    isCashierNameShownInInvoice = Convert.ToInt32(IsCashierNameShownInInvoiceBO.SetupValue);
                }

                //FrontOfficeMasterInvoiceTemplateBO
                int frontOfficeMasterInvoiceTemplate = 1;
                HMCommonSetupBO FrontOfficeMasterInvoiceTemplateBO = new HMCommonSetupBO();
                FrontOfficeMasterInvoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeMasterInvoiceTemplate", "FrontOfficeMasterInvoiceTemplate");
                if (FrontOfficeMasterInvoiceTemplateBO != null)
                {
                    frontOfficeMasterInvoiceTemplate = Convert.ToInt32(FrontOfficeMasterInvoiceTemplateBO.SetupValue);
                }

                string strSdc = Request.QueryString["sdc"];

                //For the time being, 
                //strSdc = "1";

                var reportPath = "";
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBill.rdlc");

                HMCommonSetupBO isInvoiceGuestBillWithoutHeaderandFooterBO = new HMCommonSetupBO();
                isInvoiceGuestBillWithoutHeaderandFooterBO = commonSetupDA.GetCommonConfigurationInfo("IsInvoiceGuestBillWithoutHeaderAndFooter", "IsInvoiceGuestBillWithoutHeaderAndFooter");
                if (isInvoiceGuestBillWithoutHeaderandFooterBO != null)
                {
                    if (isInvoiceGuestBillWithoutHeaderandFooterBO.SetupValue == "0")
                    {
                        if (frontOfficeMasterInvoiceTemplate == 1)
                        {
                            if (!string.IsNullOrEmpty(strSdc) && strSdc == "1")
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillWithQRCode.rdlc");
                            }
                            else
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBill.rdlc");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(strSdc) && strSdc == "1")
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillTwoWithQRCode.rdlc");
                            }
                            else
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillTwo.rdlc");
                            }
                        }
                    }
                    else
                    {
                        if (frontOfficeMasterInvoiceTemplate == 1)
                        {
                            if (string.IsNullOrEmpty(strSdc) && strSdc == "1")
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillWithoutHeaderandFooterWithQRCode.rdlc");
                            }
                            else
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillWithoutHeaderandFooter.rdlc");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(strSdc) && strSdc == "1")
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillWithoutHeaderandFooterWithQRCode.rdlc");
                            }
                            else
                            {
                                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillWithoutHeaderandFooter.rdlc");
                            }
                        }

                    }
                }

                if (!File.Exists(reportPath))
                {
                    return;
                }

                rvInvoiceTransaction.LocalReport.ReportPath = reportPath;
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

                // // ----- Costcenter Wise Invoice Logo Information
                List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");
                if (costCentreTabBO != null)
                {
                    if (costCentreTabBO.Count > 0)
                    {
                        rvInvoiceTransaction.LocalReport.EnableExternalImages = true;
                        List<ReportParameter> paramReport = new List<ReportParameter>();

                        if (!string.IsNullOrWhiteSpace(costCentreTabBO[0].CostCenterLogo))
                        {
                            ImageName = costCentreTabBO[0].CostCenterLogo;
                            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                        }
                        else
                        {
                            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                        }

                        paramReport.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                        paramReport.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                        paramReport.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                        paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                        paramReport.Add(new ReportParameter("GroupCompanyName", files[0].GroupCompanyName));
                        paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                        paramReport.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
                        paramReport.Add(new ReportParameter("IsBigSizeCompanyLogo", hmCommonDA.GetCustomFieldValueByFieldName("IsBigSizeCompanyLogo")));
                        paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                        paramReport.Add(new ReportParameter("IsCashierNameShownInInvoice", isCashierNameShownInInvoice.ToString()));

                        HMCommonSetupBO isWaterMarkImageEnableBO = new HMCommonSetupBO();
                        isWaterMarkImageEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsWaterMarkImageDisplayEnable", "IsWaterMarkImageDisplayEnable");
                        if (isWaterMarkImageEnableBO != null)
                        {
                            if (isWaterMarkImageEnableBO.SetupValue == "1")
                            {
                                paramReport.Add(new ReportParameter("WaterMarkImagePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/WaterMarkTextA4.png")));
                            }
                            else
                            {
                                paramReport.Add(new ReportParameter("WaterMarkImagePath", ""));
                            }
                        }

                        string isDiscountApplicableOnRackRate = "0";
                        //List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                        //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                        //costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");

                        if (costCentreTabBO.Count > 0)
                        {
                            isDiscountApplicableOnRackRate = costCentreTabBO[0].IsDiscountApplicableOnRackRate == true ? "1" : "0";
                        }

                        if (frontOfficeInvoiceTemplate == 1)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateOne"));
                        }
                        else if (frontOfficeInvoiceTemplate == 2)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateTwo"));
                        }
                        else if (frontOfficeInvoiceTemplate == 3)
                        {
                            if (isDiscountApplicableOnRackRate == "0")
                            {
                                paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateThree"));
                            }
                            else
                            {
                                paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateSix"));
                            }
                        }
                        else if (frontOfficeInvoiceTemplate == 4)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateFour"));
                        }
                        else if (frontOfficeInvoiceTemplate == 5)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateFive"));
                        }

                        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                        {
                            paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                        }
                        else
                        {
                            paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                        }

                        // //----------------- Show Hide Related Information -------------------------------------------------------
                        string IsServiceChargeEnableConfig = "1";
                        string IsCitySDChargeEnableConfig = "1";
                        string IsVatAmountEnableConfig = "1";
                        string IsAdditionalChargeEnableConfig = "1";

                        //List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                        //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                        //costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");

                        if (costCentreTabBO.Count > 0)
                        {
                            IsServiceChargeEnableConfig = costCentreTabBO[0].IsServiceChargeEnable ? "1" : "0";
                            IsCitySDChargeEnableConfig = costCentreTabBO[0].IsCitySDChargeEnable ? "1" : "0";
                            IsVatAmountEnableConfig = costCentreTabBO[0].IsVatEnable ? "1" : "0";
                            IsAdditionalChargeEnableConfig = costCentreTabBO[0].IsAdditionalChargeEnable ? "1" : "0";
                        }

                        paramReport.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
                        paramReport.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
                        paramReport.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
                        paramReport.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));

                        string isBillSummaryPartWillHide = "0";
                        HMCommonSetupBO IsBillSummaryPartWillHideBO = new HMCommonSetupBO();
                        IsBillSummaryPartWillHideBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeMasterInvoiceTemplate", "IsBillSummaryPartWillHide");
                        if (IsBillSummaryPartWillHideBO != null)
                        {
                            isBillSummaryPartWillHide = IsBillSummaryPartWillHideBO.SetupValue;
                        }
                        paramReport.Add(new ReportParameter("IsBillSummaryPartWillHide", isBillSummaryPartWillHide));



                        GuestBillPaymentDA paymentDa = new GuestBillPaymentDA();
                        List<GenerateGuestBillReportBO> guestBill = new List<GenerateGuestBillReportBO>();

                        if (this.Session["CheckOutMasterRegistrationId"] != null)
                        {
                            guestBill = paymentDa.GetGenerateGuestBill(this.Session["CheckOutMasterRegistrationId"].ToString(), txtIsBillSplited.Text, txtGuestBillFromDate.Text, txtGuestBillToDate.Text, txtPrintedBy.Text);
                        }
                        else
                        {
                            guestBill = paymentDa.GetGenerateGuestBill(txtRegistrationNumber.Text, txtIsBillSplited.Text, txtGuestBillFromDate.Text, txtGuestBillToDate.Text, txtPrintedBy.Text);
                        }

                        // // ----------- Guest Stayed Night Count ---------------------
                        List<GuestServiceBillApprovedBO> reportDetailsInfo = Session["ReportGuestBillInfoDataSource"] as List<GuestServiceBillApprovedBO>;
                        int nightCount = reportDetailsInfo.Where(x => x.PaymentType == "RoomService").ToList().Count;

                        int registrationId = 0;

                        foreach (GenerateGuestBillReportBO row in guestBill)
                        {
                            row.Night = nightCount;
                            registrationId = row.RegistrationId;
                        }

                        bool isCheckOut = Convert.ToBoolean(Request.QueryString["IsCheckOut"]);

                        //For the testing purpose I have set the true value to isCheckOut variable.
                        //isCheckOut = true;

                        if (isCheckOut)
                        {
                            //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                            HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue) && setUpBO.SetupValue == "1")
                            {
                                List<GuestServiceBillApprovedBO> reportBillDetailsInfo = new List<GuestServiceBillApprovedBO>();
                                reportBillDetailsInfo = reportDetailsInfo.Where(x => x.ServiceType != "GuestPayment").ToList().ToList();

                                SdcInvoiceHandler sdcInvHandler = new SdcInvoiceHandler("GuestBill");

                                List<SdcBillReportBO> billReportsBo = new List<SdcBillReportBO>();
                                foreach (GuestServiceBillApprovedBO reportBo in reportBillDetailsInfo)
                                {
                                    SdcBillReportBO bo = new SdcBillReportBO();
                                    
                                    bo.ItemId = (reportBo.ServiceId + 1);
                                    bo.ItemCode = "" + reportBo.ServiceId.ToString();
                                    bo.HsCode = "";
                                    bo.ItemName = reportBo.GuestService;
                                    //bo.UnitRate = reportBo.TotalRoomCharge;
                                    bo.UnitRate = reportBo.ServiceRate;
                                    bo.PaxQuantity = 1;

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

                                sdcInvHandler.SdcInvoiceProcess(userInformationBO, registrationId, billReportsBo);

                                while (!sdcInvHandler.IsInvoiceReceived)
                                {
                                    //Wait Until the invoice received from the NBR Server through the SDC Integrated device. This is just a blank while loop because i have to wait 
                                    //for the response of SDC Device Event what i have fired inside the SdcInvoiceProcess above.
                                    //After receiving the response I will call the ProcessReport method bellow.
                                }

                                if (sdcInvHandler.IsDeviceConnected)
                                {
                                    String SDCInvoiceNumber = string.Empty;
                                    String SDCQRCode = string.Empty;
                                    setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                                    if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
                                    {

                                        if (strSdc == "1")
                                        {
                                            RestaurentBillDA sdc_rda = new RestaurentBillDA();
                                            RestaurantBillBO RestaurantBillBOSDCInfo = new RestaurantBillBO();
                                            RestaurantBillBOSDCInfo = sdc_rda.GetSDCInfoviceInformation(registrationId);
                                            if (RestaurantBillBOSDCInfo.BillId > 0)
                                            {
                                                SDCInvoiceNumber = RestaurantBillBOSDCInfo.SDCInvoiceNumber;
                                                SDCQRCode = RestaurantBillBOSDCInfo.QRCode;

                                                byte[] QrImage;
                                                HMCommonDA DA = new HMCommonDA();
                                                QrImage = DA.GenerateQrCode(SDCQRCode);
                                                paramReport.Add(new ReportParameter("SDCInvoiceNumber", SDCInvoiceNumber));
                                                paramReport.Add(new ReportParameter("QRCode", Convert.ToBase64String(QrImage)));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    paramReport.Add(new ReportParameter("SDCInvoiceNumber", ""));
                                    paramReport.Add(new ReportParameter("QRCode", ""));
                                }
                            }
                        }

                        rvInvoiceTransaction.LocalReport.SetParameters(paramReport);

                        var reportDataset = rvInvoiceTransaction.LocalReport.GetDataSourceNames();
                        rvInvoiceTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestBill));
                        rvInvoiceTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
                        rvInvoiceTransaction.LocalReport.Refresh();
                        InvoiceTemplate1.Visible = false;
                        InvoiceTemplate2.Visible = true;
                    }
                }
            }
        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            InvoiceTemplate1.Visible = false;
            InvoiceTemplate2.Visible = true;

            //List<ReportParameter> reportParam = new List<ReportParameter>();


            //// //----------------- Show Hide Related Information -------------------------------------------------------
            //string IsServiceChargeEnableConfig = "1";
            //string IsCitySDChargeEnableConfig = "1";
            //string IsVatAmountEnableConfig = "1";
            //string IsAdditionalChargeEnableConfig = "1";

            //List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
            //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            //costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");

            //if (costCentreTabBO.Count > 0)
            //{
            //    IsServiceChargeEnableConfig = costCentreTabBO[0].IsServiceChargeEnable ? "1" : "0";
            //    IsCitySDChargeEnableConfig = costCentreTabBO[0].IsCitySDChargeEnable ? "1" : "0";
            //    IsVatAmountEnableConfig = costCentreTabBO[0].IsVatEnable ? "1" : "0";
            //    IsAdditionalChargeEnableConfig = costCentreTabBO[0].IsAdditionalChargeEnable ? "1" : "0";
            //}

            //reportParam.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
            //reportParam.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
            //reportParam.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
            //reportParam.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));


            //rvTransaction.LocalReport.SetParameters(reportParam);
            //rvInvoiceTransaction.SubmittingParameterValues.SetParameters(reportParam);

            e.DataSources.Add(new ReportDataSource("GuestServiceBillInfo", Session["ReportGuestBillInfoDataSource"] as List<GuestServiceBillApprovedBO>));
        }

        private bool SendMail(string queryStringId)
        {
            bool status = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsCheckOutEmailAutoPostingEnable");
            string IsEnable = commonSetupBO.SetupValue;
            if (IsEnable == "0")
                return status;

            int registrationId = 0;

            if (!String.IsNullOrEmpty(queryStringId))
            {
                registrationId = Convert.ToInt32(queryStringId);
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();


            List<RegistrationCardInfoBO> registrationBO = new List<RegistrationCardInfoBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            registrationBO = roomRegistrationDA.GetRoomRegistrationDetailByRegistrationId(registrationId, string.Empty, string.Empty, string.Empty);

            string ReceivingMail = registrationBO[0].GuestEmail;
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

            byte[] bytes = rvInvoiceTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileName = string.Empty, filePath = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = "RR" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            filePath = HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName);
            if (!File.Exists(filePath))
                return false;
            //Send Mail            

            Email email;
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;


            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');

                var tokens = new Dictionary<string, string>
                         {
                             {"NAME", registrationBO[0].GuestName},
                             {"REGISTRATIONNO",registrationBO[0].RegistrationNumber },
                             {"COMPANYNAME",files[0].CompanyName }
                         };
                email = new Email()
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = ReceivingMail,
                    Subject = "Expression of Appreciation",
                    AttachmentSavedPath = filePath,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    FromDisplayName = files[0].WebAddress,
                    TempleteName = HMConstants.EmailTemplates.CheckOut
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

                byte[] bytes = rvInvoiceTransaction.LocalReport.Render("PDF", null, out mimeType,
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