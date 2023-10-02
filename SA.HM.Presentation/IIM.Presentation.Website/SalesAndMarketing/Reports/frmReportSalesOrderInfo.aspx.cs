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
using HotelManagement.Entity.RetailPOS;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportSalesOrderInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BillinReportProcessing("rptSalesOrderInvoiceForA4Page");
                //string queryStringId = Request.QueryString["soId"];
                //string queryStringChallan = Request.QueryString["Challan"];
                ////if (!string.IsNullOrEmpty(queryStringId))
                ////{
                //    this.ForwordReportProcessing();
                ////}
                ////else
                ////{
                ////    this.ReportProcessingForChallan("rptRestaurentBillForChallan");
                ////}
            }
        }
        //private void ForwordReportProcessing()
        //{
        //    string queryStringId = Request.QueryString["soId"];
        //    int billID = Int32.Parse(queryStringId);

        //    if (!string.IsNullOrEmpty(queryStringId))
        //    {
        //        CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
        //        CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
        //        costCentreTabBO = costCentreTabDA.GetCostCenterDetailInformation("Restaurant", billID);

        //        if (costCentreTabBO != null)
        //        {
        //            if (costCentreTabBO.InvoiceTemplate > 0)
        //            {
        //                if (costCentreTabBO.CompanyType == "RiceMill")
        //                {
        //                    hfBillTemplate.Value = "5";
        //                    this.BillinReportProcessing("rptRiceMillBillForA4Page");
        //                }
        //                else
        //                {
        //                    if (costCentreTabBO.InvoiceTemplate == 1)
        //                    {
        //                        hfBillTemplate.Value = "1";
        //                        this.ReportProcessing("rptRestaurentBillForPosWithoutSDC");
        //                    }
        //                    else if (costCentreTabBO.InvoiceTemplate == 2)
        //                    {
        //                        hfBillTemplate.Value = "2";
        //                        this.ReportProcessing("rptRestaurentBillForDotMatrix");
        //                    }
        //                    else if (costCentreTabBO.InvoiceTemplate == 3)
        //                    {
        //                        hfBillTemplate.Value = "3";
        //                        this.ReportProcessing("rptRestaurentBillTwoColumn");
        //                    }
        //                    else if (costCentreTabBO.InvoiceTemplate == 4)
        //                    {
        //                        hfBillTemplate.Value = "4";
        //                        this.ReportProcessingForPosToken("rptRestaurentBillForPosToken");
        //                    }
        //                    else if (costCentreTabBO.InvoiceTemplate == 5)
        //                    {
        //                        hfBillTemplate.Value = "5";
        //                        this.BillinReportProcessing("rptRestaurentBillForA4Page");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //// Template 1 -- POS, Template 2---- Dot Matrix, Template 3 -- Double Column
        private void BillinReportProcessing(string reportName)
        {
            string queryStringUS = Request.QueryString["US"];
            List<ReportParameter> reportParam = new List<ReportParameter>();
            string queryStringId = Request.QueryString["soId"];
            int billID = Int32.Parse(queryStringId);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.DataSources.Clear();

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();

                string companyName = string.Empty;
                string companyAddress = string.Empty;
                string binNumber = string.Empty;
                string tinNumber = string.Empty;
                string projectName = string.Empty;
                if (files[0].CompanyId > 0)
                {
                    //reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                    companyName = files[0].CompanyName;
                    companyAddress = files[0].CompanyAddress;
                    binNumber = files[0].VatRegistrationNo;
                    tinNumber = files[0].TinNumber;

                    //reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                    //reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
                    reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                }

                //CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                //HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                //Boolean isVatAmountEnable = true;
                //costCentreTabBO = costCentreTabDA.GetCostCenterDetailInformation("Restaurant", billID);

                //if (costCentreTabBO != null)
                //{
                //    if (costCentreTabBO.InvoiceTemplate > 0)
                //    {
                //        isVatAmountEnable = costCentreTabBO.IsVatEnable;
                //        if (costCentreTabBO.InvoiceTemplate == 5)
                //        {
                //            if (reportName == "rptRiceMillBillForA4Page")
                //            {
                //                reportName = "rptRiceMillBillForA4Page";
                //            }
                //            else
                //            {
                //                reportName = "rptRestaurentBillForA4Page";
                //            }

                //            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBillingInvoiceTemplateWithoutHeader", "IsBillingInvoiceTemplateWithoutHeader");

                //            if (commonSetupBO != null)
                //            {
                //                if (commonSetupBO.SetupValue != "0")
                //                {
                //                    if (reportName == "rptRiceMillBillForA4Page")
                //                    {
                //                        reportName = "rptRiceMillBillForA4PageWithoutHeader";
                //                    }
                //                    else
                //                    {
                //                        reportName = "rptRestaurentBillForA4PageWithoutHeader";
                //                    }
                //                }
                //            }
                //        }

                //        if (costCentreTabBO.IsCostCenterNameShowOnInvoice)
                //        {
                //            companyName = costCentreTabBO.CostCenter;
                //            if (!string.IsNullOrWhiteSpace(costCentreTabBO.CompanyAddress))
                //            {
                //                companyAddress = costCentreTabBO.CompanyAddress;
                //            }
                //        }
                //    }
                //}

                //RestaurantBillBO billBO = new RestaurantBillBO();
                //RestaurentBillDA billDA = new RestaurentBillDA();
                //billBO = billDA.GetBillInfoByBillId(billID);

                string billRemarks = string.Empty;
                string billDeclaration = string.Empty;
                string imagePreparedBySignature = string.Empty;

                //if (billBO != null)
                //{
                //    if (billBO.BillId > 0)
                //    {
                //        billRemarks = billBO.Remarks;
                //        binNumber = billBO.BinNumber;
                //        tinNumber = billBO.TinNumber;
                //        projectName = billBO.ProjectName;
                //        billDeclaration = billBO.BillDeclaration;

                //        if (!string.IsNullOrEmpty(queryStringUS))
                //        {
                //            imagePreparedBySignature = billBO.UserSignature;
                //        }
                //        if (billBO.IsInvoiceVatAmountEnable == false)
                //        {
                //            if (reportName == "rptRestaurentBillForA4Page")
                //            {
                //                if (reportName == "rptRiceMillBillForA4Page")
                //                {
                //                    reportName = "rptRiceMillBillForA4Page";
                //                }
                //                else
                //                {
                //                    if (!isVatAmountEnable)
                //                    {
                //                        reportName = "rptRestaurentBillForA402Page";
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (reportName == "rptRiceMillBillForA4Page")
                //                {
                //                    reportName = "rptRiceMillBillForA4PageWithoutHeader";
                //                }
                //                else
                //                {
                //                    reportName = "rptRestaurentBillForA402PageWithoutHeader";
                //                }
                //            }
                //        }
                //    }
                //}

                var reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                reportParam.Add(new ReportParameter("CompanyProfile", companyName));
                reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
                reportParam.Add(new ReportParameter("VatRegistrationNo", binNumber));
                reportParam.Add(new ReportParameter("TinNumber", tinNumber));
                reportParam.Add(new ReportParameter("ProjectName", projectName));

                reportParam.Add(new ReportParameter("BillDeclaration", billDeclaration));

                rvTransaction.LocalReport.EnableExternalImages = true;
                HMCommonDA hmCommonDA = new HMCommonDA();
                //string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
                //if (!string.IsNullOrWhiteSpace(imageName))
                //{
                //    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                //}
                //else
                //{
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                //}

                //if (!string.IsNullOrWhiteSpace(imagePreparedBySignature))
                //{
                //    reportParam.Add(new ReportParameter("BillingDefaultPreparedBySignature", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/UserSignature/" + imagePreparedBySignature)));
                //}
                //else
                //{
                    reportParam.Add(new ReportParameter("BillingDefaultPreparedBySignature", "Hide"));
                //}

                HMCommonSetupBO setUpBOApprovedBySignature = new HMCommonSetupBO();
                string imageApprovedBySignature = "0";
                //if (!string.IsNullOrEmpty(queryStringUS))
                //{
                //    setUpBOApprovedBySignature = commonSetupDA.GetCommonConfigurationInfo("BillingDefaultApprovedBySignature", "BillingDefaultApprovedBySignature");
                //    if (!string.IsNullOrWhiteSpace(setUpBOApprovedBySignature.SetupValue))
                //    {
                //        imageApprovedBySignature = setUpBOApprovedBySignature.SetupValue;
                //    }
                //}

                //if (imageApprovedBySignature != "0")
                //{
                //    reportParam.Add(new ReportParameter("BillingDefaultApprovedBySignature", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/UserSignature/" + imageApprovedBySignature)));
                //}
                //else
                //{
                    reportParam.Add(new ReportParameter("BillingDefaultApprovedBySignature", "Hide"));
                //}

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

                //HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
                //isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
                //if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                //{
                //    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
                //}
                //else
                //{
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
                //}


                //isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
                //if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                //{
                //    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
                //}
                //else
                //{
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
                //}

                //isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsBillingInvoiceDueSectionEnable", "IsBillingInvoiceDueSectionEnable");
                //if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                //{
                //    reportParam.Add(new ReportParameter("IsBillingInvoiceDueSectionEnable", "1"));
                //}
                //else
                //{
                    reportParam.Add(new ReportParameter("IsBillingInvoiceDueSectionEnable", "0"));
                //}

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));


                RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
                RestaurentPosDA rda = new RestaurentPosDA();
                restaurantBill = rda.GetSalesOrderInfoBySOIdForReport(billID);

                string discountTitle = string.Empty;
                string billDescription = string.Empty;
                if (billID > 0)
                {
                    if (restaurantBill.PosBillWithSalesReturn != null)
                    {
                        if (restaurantBill.PosBillWithSalesReturn.Count > 0)
                        {
                            billDescription = restaurantBill.PosBillWithSalesReturn[0].BillDescription;
                            billRemarks = restaurantBill.PosBillWithSalesReturn[0].BillDescription;

                            if (restaurantBill.PosBillWithSalesReturn[0].DiscountType == "Percentage")
                            {
                                discountTitle = "Discount (" + restaurantBill.PosBillWithSalesReturn[0].DiscountAmount + "%)";
                            }
                            else
                            {
                                discountTitle = "Discount";
                            }
                        }
                    }
                }

                //Decimal billCompanyDueTotal = 0.00;
                //billCompanyDueTotal = 1000.00;
                //reportParam.Add(new ReportParameter("BillCompanyDueTotal", billCompanyDueTotal));


                reportParam.Add(new ReportParameter("DiscountTitle", discountTitle));
                reportParam.Add(new ReportParameter("BillRemarks", billRemarks));

                rvTransaction.LocalReport.SetParameters(reportParam);
                var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill.PosBillWithSalesReturn));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], restaurantBill.PosSalesReturnPayment));

                rvTransaction.LocalReport.DisplayName = "Sales Order";
                rvTransaction.LocalReport.Refresh();
            }
        }
        //private void ReportProcessing(string reportName)
        //{
        //    string queryStringId = Request.QueryString["soId"];

        //    if (!string.IsNullOrEmpty(queryStringId))
        //    {
        //        int billID = Int32.Parse(queryStringId);
        //        rvTransaction.ProcessingMode = ProcessingMode.Local;
        //        rvTransaction.LocalReport.DataSources.Clear();

        //        var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
        //        if (!File.Exists(reportPath))
        //            return;

        //        rvTransaction.LocalReport.ReportPath = reportPath;

        //        RestaurantBillBO billBO = new RestaurantBillBO();
        //        RestaurentBillDA billDA = new RestaurentBillDA();
        //        billBO = billDA.GetBillInfoByBillId(billID);

        //        CompanyDA companyDA = new CompanyDA();
        //        List<CompanyBO> files = companyDA.GetCompanyInfo();
        //        List<ReportParameter> reportParam = new List<ReportParameter>();

        //        string strVatRegistrationNo = string.Empty;
        //        string strContactNumber = string.Empty;

        //        if (files[0].CompanyId > 0)
        //        {
        //            reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
        //            reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
        //            //reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
        //            strVatRegistrationNo = files[0].VatRegistrationNo;
        //            reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

        //            if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
        //            {
        //                reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
        //            }

        //            //reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
        //            strContactNumber = files[0].ContactNumber;
        //        }

        //        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //        HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
        //        isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
        //        if (isOnlyPdfEnableWhenReportExportBO != null)
        //        {
        //            if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
        //            {
        //                if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
        //                {
        //                    CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
        //                    CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());
        //                }
        //            }
        //        }

        //        rvTransaction.LocalReport.EnableExternalImages = true;
        //        HMCommonDA hmCommonDA = new HMCommonDA();
        //        HMCommonSetupBO waterMarkBo = new HMCommonSetupBO();
        //        waterMarkBo = commonSetupDA.GetCommonConfigurationInfo("IsWaterMarkImageDisplayEnableInRestaurant", "IsWaterMarkImageDisplayEnableInRestaurant");

        //        string strBillDisplayText = "";

        //        if (Convert.ToInt32(waterMarkBo.SetupValue) == 1)
        //        {
        //            strBillDisplayText = "*** Duplicate Bill ***";
        //            reportParam.Add(new ReportParameter("WaterMarkImagePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/Bill-Duplicate-Water-Mark-Restaurant.png")));
        //        }
        //        else
        //            reportParam.Add(new ReportParameter("WaterMarkImagePath", string.Empty));

        //        string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
        //        if (!string.IsNullOrWhiteSpace(imageName))
        //        {
        //            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("Path", "Hide"));
        //        }

        //        reportParam.Add(new ReportParameter("BillDisplayText", strBillDisplayText));
        //        reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessegeForRestaurantBill")));
        //        reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
        //        reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
        //        reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

        //        HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
        //        isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
        //        if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
        //        {
        //            reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
        //        }

        //        this.IsRestaurantOrderSubmitDisableInfo();

        //        if (IsRestaurantOrderSubmitDisable)
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
        //        }

        //        this.IsRestaurantTokenInfoDisableInfo();

        //        if (IsRestaurantTokenInfoDisable)
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
        //        }

        //        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));

        //        DateTime currentDate = DateTime.Now;
        //        HMCommonDA printDateDA = new HMCommonDA();

        //        string printDate = string.Empty;
        //        printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

        //        //RestaurantBillBO billInfoBO = new RestaurantBillBO();
        //        //billInfoBO = billDA.GetBillInfoByBillId(billID);
        //        //if (billInfoBO != null)
        //        //{
        //        //    printDate = hmUtility.GetDateTimeStringFromDateTime(billInfoBO.BillDate); //hmUtility.GetStringFromDateTime(billInfoBO.BillDate) + " " + billInfoBO.BillDate.ToString("hh:mm:ss tt");
        //        //}
        //        //else
        //        //{
        //        //    printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
        //        //}

        //        RestaurentBillDA rda = new RestaurentBillDA();
        //        List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
        //        restaurantBill = rda.GetRestaurantBillReport(billID);

        //        int isRestaurantCreditSaleEnable = 0;
        //        HMCommonSetupBO isRestaurantCreditSaleEnableBO = new HMCommonSetupBO();
        //        isRestaurantCreditSaleEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantCreditSaleEnable", "IsRestaurantCreditSaleEnable");

        //        if (isRestaurantCreditSaleEnableBO != null)
        //        {
        //            if (isRestaurantCreditSaleEnableBO.SetupValue == "1")
        //            {
        //                isRestaurantCreditSaleEnable = 1;
        //            }
        //        }

        //        decimal paySourceCurrentBalance = 0;
        //        RestaurantBillBO restaurantBillInfoBO = new RestaurantBillBO();
        //        if (isRestaurantCreditSaleEnable == 1)
        //        {
        //            restaurantBillInfoBO = rda.GetRestaurantBillInfoForCompanyBalance(billID);
        //            if (restaurantBillInfoBO != null)
        //            {
        //                paySourceCurrentBalance = restaurantBillInfoBO.PaySourceCurrentBalance;
        //            }
        //        }

        //        reportParam.Add(new ReportParameter("PaySourceCurrentBalance", paySourceCurrentBalance.ToString()));
        //        reportParam.Add(new ReportParameter("PrintDateTime", printDate));

        //        // //----------------- Show Hide Related Information -------------------------------------------------------
        //        string IsServiceChargeEnableConfig = "1";
        //        string IsCitySDChargeEnableConfig = "1";
        //        string IsVatAmountEnableConfig = "1";
        //        string IsAdditionalChargeEnableConfig = "1";
        //        string IsCostCenterNameShowOnInvoice = "1";

        //        CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
        //        CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
        //        if (restaurantBill != null)
        //        {
        //            if (restaurantBill.Count > 0)
        //            {
        //                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(restaurantBill[0].CostCenterId);

        //                if (!string.IsNullOrWhiteSpace(costCentreTabBO.VatRegistrationNo))
        //                {
        //                    strVatRegistrationNo = costCentreTabBO.VatRegistrationNo;
        //                }

        //                if (!string.IsNullOrWhiteSpace(costCentreTabBO.ContactNumber))
        //                {
        //                    strContactNumber = costCentreTabBO.ContactNumber;
        //                }

        //                IsServiceChargeEnableConfig = costCentreTabBO.IsServiceChargeEnable ? "1" : "0";
        //                IsCitySDChargeEnableConfig = costCentreTabBO.IsCitySDChargeEnable ? "1" : "0";
        //                IsVatAmountEnableConfig = costCentreTabBO.IsVatEnable ? "1" : "0";
        //                IsAdditionalChargeEnableConfig = costCentreTabBO.IsAdditionalChargeEnable ? "1" : "0";
        //                IsCostCenterNameShowOnInvoice = costCentreTabBO.IsCostCenterNameShowOnInvoice ? "1" : "0";
        //            }
        //        }

        //        reportParam.Add(new ReportParameter("VatRegistrationNo", strVatRegistrationNo));
        //        reportParam.Add(new ReportParameter("ContactNumber", strContactNumber));

        //        reportParam.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
        //        reportParam.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
        //        reportParam.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
        //        reportParam.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));
        //        reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

        //        rvTransaction.LocalReport.SetParameters(reportParam);
        //        var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
        //        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

        //        rvTransaction.LocalReport.DisplayName = "Bill Information";
        //        rvTransaction.LocalReport.Refresh();
        //    }
        //}
        //private void ReportProcessingForChallan(string reportName)
        //{
        //    string queryStringId = Request.QueryString["Challan"];

        //    if (!string.IsNullOrEmpty(queryStringId))
        //    {
        //        int billID = Int32.Parse(queryStringId);
        //        rvTransaction.ProcessingMode = ProcessingMode.Local;
        //        rvTransaction.LocalReport.DataSources.Clear();

        //        var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
        //        if (!File.Exists(reportPath))
        //            return;

        //        rvTransaction.LocalReport.ReportPath = reportPath;

        //        RestaurantBillBO billBO = new RestaurantBillBO();
        //        RestaurentBillDA billDA = new RestaurentBillDA();
        //        billBO = billDA.GetBillInfoByBillId(billID);

        //        CompanyDA companyDA = new CompanyDA();
        //        List<CompanyBO> files = companyDA.GetCompanyInfo();
        //        List<ReportParameter> reportParam = new List<ReportParameter>();

        //        if (files[0].CompanyId > 0)
        //        {
        //            reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
        //            reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
        //            reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
        //            reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

        //            if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
        //            {
        //                reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
        //            }

        //            reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
        //        }

        //        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //        HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
        //        isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
        //        if (isOnlyPdfEnableWhenReportExportBO != null)
        //        {
        //            if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
        //            {
        //                if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
        //                {
        //                    CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
        //                    CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());
        //                }
        //            }
        //        }

        //        rvTransaction.LocalReport.EnableExternalImages = true;
        //        HMCommonDA hmCommonDA = new HMCommonDA();

        //        string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
        //        if (!string.IsNullOrWhiteSpace(imageName))
        //        {
        //            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("Path", "Hide"));
        //        }

        //        reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
        //        reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
        //        reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
        //        reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));
        //        this.IsRestaurantOrderSubmitDisableInfo();

        //        if (IsRestaurantOrderSubmitDisable)
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
        //        }

        //        this.IsRestaurantTokenInfoDisableInfo();

        //        if (IsRestaurantTokenInfoDisable)
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
        //        }

        //        HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
        //        isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
        //        if (isRestaurantIntegrateWithFrontOfficeBO != null)
        //        {
        //            if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
        //            {
        //                reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
        //            }
        //            else
        //            {
        //                reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
        //            }
        //        }

        //        DateTime currentDate = DateTime.Now;
        //        HMCommonDA printDateDA = new HMCommonDA();

        //        string printDate = string.Empty;
        //        RestaurantBillBO billInfoBO = new RestaurantBillBO();
        //        billInfoBO = billDA.GetBillInfoByBillId(billID);
        //        if (billInfoBO != null)
        //        {
        //            printDate = hmUtility.GetDateTimeStringFromDateTime(billInfoBO.BillDate);
        //        }
        //        else
        //        {
        //            printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
        //        }

        //        reportParam.Add(new ReportParameter("PrintDateTime", printDate));
        //        rvTransaction.LocalReport.SetParameters(reportParam);

        //        RestaurentBillDA rda = new RestaurentBillDA();
        //        List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
        //        restaurantBill = rda.GetRestaurantBillReport(billID);

        //        var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
        //        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

        //        rvTransaction.LocalReport.DisplayName = "Bill Information";
        //        rvTransaction.LocalReport.Refresh();
        //    }
        //}
        //private void ReportProcessingForPosToken(string reportName)
        //{
        //    string queryStringId = Request.QueryString["soId"];
        //    int billID = Int32.Parse(queryStringId);

        //    if (!string.IsNullOrEmpty(queryStringId))
        //    {
        //        rvTransaction.ProcessingMode = ProcessingMode.Local;
        //        rvTransaction.LocalReport.DataSources.Clear();

        //        var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
        //        if (!File.Exists(reportPath))
        //            return;

        //        rvTransaction.LocalReport.ReportPath = reportPath;

        //        RestaurantBillBO billBO = new RestaurantBillBO();
        //        RestaurentBillDA billDA = new RestaurentBillDA();
        //        billBO = billDA.GetBillInfoByBillId(billID);

        //        CompanyDA companyDA = new CompanyDA();
        //        List<CompanyBO> files = companyDA.GetCompanyInfo();
        //        List<ReportParameter> reportParam = new List<ReportParameter>();

        //        if (files[0].CompanyId > 0)
        //        {
        //            reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
        //            reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
        //            reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

        //            if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
        //            {
        //                reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
        //            }

        //            reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
        //        }

        //        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //        HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
        //        isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
        //        if (isOnlyPdfEnableWhenReportExportBO != null)
        //        {
        //            if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
        //            {
        //                if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
        //                {
        //                    CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
        //                    CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());
        //                }
        //            }
        //        }

        //        rvTransaction.LocalReport.EnableExternalImages = true;
        //        HMCommonDA hmCommonDA = new HMCommonDA();

        //        string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
        //        if (!string.IsNullOrWhiteSpace(imageName))
        //        {
        //            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("Path", "Hide"));
        //        }

        //        reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
        //        reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
        //        reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
        //        reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));
        //        reportParam.Add(new ReportParameter("ReportTitle", "KOT"));
        //        reportParam.Add(new ReportParameter("CostCenter", ""));
        //        reportParam.Add(new ReportParameter("SourceName", ""));
        //        reportParam.Add(new ReportParameter("TableNo", ""));
        //        reportParam.Add(new ReportParameter("KotNo", ""));
        //        reportParam.Add(new ReportParameter("KotDate", DateTime.Now.ToString()));
        //        reportParam.Add(new ReportParameter("WaiterName", ""));
        //        reportParam.Add(new ReportParameter("RestaurantName", files[0].CompanyName));

        //        this.IsRestaurantOrderSubmitDisableInfo();

        //        if (IsRestaurantOrderSubmitDisable)
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
        //        }

        //        this.IsRestaurantTokenInfoDisableInfo();

        //        if (IsRestaurantTokenInfoDisable)
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
        //        }
        //        else
        //        {
        //            reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
        //        }

        //        HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
        //        isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
        //        if (isRestaurantIntegrateWithFrontOfficeBO != null)
        //        {
        //            if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
        //            {
        //                reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
        //            }
        //            else
        //            {
        //                reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
        //            }
        //        }

        //        DateTime currentDate = DateTime.Now;
        //        HMCommonDA printDateDA = new HMCommonDA();
        //        string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

        //        reportParam.Add(new ReportParameter("PrintDateTime", printDate));

        //        RestaurentBillDA rda = new RestaurentBillDA();
        //        KotBillDetailDA entityDA = new KotBillDetailDA();

        //        List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
        //        List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

        //        restaurantBill = rda.GetRestaurantBillReport(billID);
        //        reportParam.Add(new ReportParameter("KotNo", restaurantBill[0].KotId.ToString()));

        //        entityBOList = entityDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "AllItem", false);

        //        reportParam.Add(new ReportParameter("SpecialRemarks", entityBOList[0].Remarks));
        //        rvTransaction.LocalReport.SetParameters(reportParam);

        //        List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
        //        List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
        //        List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

        //        var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
        //        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));
        //        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], kotOrderSubmitEntityBOList));
        //        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[2], changedOrEditedEntityBOList));
        //        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[3], voidOrDeletedItemEntityBOList));

        //        rvTransaction.LocalReport.DisplayName = "Invoice";
        //        rvTransaction.LocalReport.Refresh();
        //    }
        //}
        //private void IsRestaurantOrderSubmitDisableInfo()
        //{
        //    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
        //    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantOrderSubmitDisable", "IsRestaurantOrderSubmitDisable");
        //    if (commonSetupBO != null)
        //    {
        //        if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
        //        {
        //            if (commonSetupBO.SetupValue == "0")
        //            {
        //                IsRestaurantOrderSubmitDisable = false;
        //            }
        //            else
        //            {
        //                IsRestaurantOrderSubmitDisable = true;
        //            }
        //        }
        //    }
        //}
        //private void IsRestaurantTokenInfoDisableInfo()
        //{
        //    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
        //    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantTokenInfoDisable", "IsRestaurantTokenInfoDisable");
        //    if (commonSetupBO != null)
        //    {
        //        if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
        //        {
        //            if (commonSetupBO.SetupValue == "0")
        //            {
        //                IsRestaurantTokenInfoDisable = false;
        //            }
        //            else
        //            {
        //                IsRestaurantTokenInfoDisable = true;
        //            }
        //        }
        //    }
        //}
        ////Pos Printing
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
        //protected void btnPrintReportTemplate2_Click(object sender, EventArgs e)
        //{
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;
        //    string deviceInfo =
        //     @"<DeviceInfo>
        //        <OutputFormat>PDF</OutputFormat>
        //        <PageWidth>5.5in</PageWidth>
        //        <PageHeight>8.5in</PageHeight>
        //        <MarginTop>0.0in</MarginTop>
        //        <MarginLeft>0.0in</MarginLeft>
        //        <MarginRight>0.0in</MarginRight>
        //        <MarginBottom>0.0in</MarginBottom>
        //    </DeviceInfo>";

        //    byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
        //                   out encoding, out extension, out streamids, out warnings);

        //    string fileName = string.Empty, fileNamePrint = string.Empty;
        //    DateTime dateTime = DateTime.Now;
        //    fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
        //    fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

        //    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();

        //    var pgSize = new Rectangle(396.0f, 612.0f);
        //    //var doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);

        //    Document document = new Document(pgSize, 0f, 0f, 0f, 0f); //PageSize.A5.Rotate()
        //    PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));

        //    //Getting a instance of new pdf wrtiter
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
        //       HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
        //    document.Open();
        //    PdfContentByte cb = writer.DirectContentUnder;

        //    int i = 0;
        //    int p = 0;
        //    int n = reader.NumberOfPages;

        //    //Rectangle psize = reader.GetPageSizeWithRotation(1);

        //    //float width = Utilities.InchesToPoints(3.5f);
        //    //float height = Utilities.InchesToPoints(8.5f);

        //    //iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(width, height);
        //    //document.SetMargins(0f, 0f, 0f, 0f);
        //    //document.SetPageSize(psize);
        //    //Add Page to new document

        //    while (i < n)
        //    {
        //        document.NewPage();
        //        p++;
        //        i++;

        //        PdfImportedPage page1 = writer.GetImportedPage(reader, i);
        //        //cb.AddTemplate(page1, 0, 1, -1, 0, page1.Width, 0); //270
        //        //cb.AddTemplate(page1, -1f, 0, 0, -1f, page1.Width, page1.Height); //180
        //        //cb.AddTemplate(page1, 0, -1f, 1f, 0, 0, page1.Height);

        //        cb.AddTemplate(page1, 0, 0);
        //    }

        //    //Attach javascript to the document
        //    PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
        //    //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
        //    writer.AddJavaScript(jAction);

        //    document.Close();

        //    frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

        //    //writer.Close();
        //    //document.Close();
        //    //reader.Close();
        //    //bytes = null;
        //    //cb = null;
        //}
        ////Double Column
        //protected void btnPrintReportTemplate3_Click(object sender, EventArgs e)
        //{
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;
        //    string deviceInfo =
        //     @"<DeviceInfo>
        //        <OutputFormat>PDF</OutputFormat>
        //        <PageWidth>11in</PageWidth>
        //        <PageHeight>8.5in</PageHeight>
        //        <MarginTop>0.0in</MarginTop>
        //        <MarginLeft>0.0in</MarginLeft>
        //        <MarginRight>0.0in</MarginRight>
        //        <MarginBottom>0.0in</MarginBottom>
        //    </DeviceInfo>";

        //    byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
        //                   out encoding, out extension, out streamids, out warnings);

        //    string fileName = string.Empty, fileNamePrint = string.Empty;
        //    DateTime dateTime = DateTime.Now;
        //    fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
        //    fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

        //    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();

        //    //Open exsisting pdf
        //    var pgSize = new Rectangle(396.0f, 612.0f);
        //    //var doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);

        //    Document document = new Document(PageSize.LETTER.Rotate(), 0f, 0f, 0f, 0f); //PageSize.A5.Rotate()
        //    PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
        //    //Getting a instance of new pdf wrtiter
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
        //       HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
        //    document.Open();
        //    PdfContentByte cb = writer.DirectContentUnder;

        //    int i = 0;
        //    int p = 0;
        //    int n = reader.NumberOfPages;

        //    //Rectangle psize = reader.GetPageSizeWithRotation(1);

        //    //float width = Utilities.InchesToPoints(3.5f);
        //    //float height = Utilities.InchesToPoints(8.5f);

        //    //iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(width, height);
        //    //document.SetMargins(0f, 0f, 0f, 0f);
        //    //document.SetPageSize(psize);
        //    //Add Page to new document

        //    while (i < n)
        //    {
        //        document.NewPage();
        //        p++;
        //        i++;

        //        PdfImportedPage page1 = writer.GetImportedPage(reader, i);
        //        //cb.AddTemplate(page1, 0, 1, -1, 0, page1.Width, 0); //270
        //        //cb.AddTemplate(page1, -1f, 0, 0, -1f, page1.Width, page1.Height); //180
        //        //cb.AddTemplate(page1, 0, -1f, 1f, 0, 0, page1.Height);

        //        cb.AddTemplate(page1, 0, 0);
        //    }

        //    //Attach javascript to the document
        //    PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
        //    //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
        //    writer.AddJavaScript(jAction);

        //    document.Close();

        //    frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

        //    //writer.Close();
        //    //document.Close();
        //    //reader.Close();
        //    //bytes = null;
        //    //cb = null;
        //}
    }
}