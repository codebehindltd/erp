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
using System.Collections;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportSplitBillInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ForwordReportProcessing();
            }
        }
        private void ForwordReportProcessing()
        {
            string queryStringId = Request.QueryString["billId"];
            if ((queryStringId.Split('~')[0]).Length > 0)
            {
                int billID = Int32.Parse(queryStringId.Split('~')[0]);
                string selectItemIdList = queryStringId.Split('~')[1];

                if (!string.IsNullOrEmpty(queryStringId))
                {
                    if (!string.IsNullOrWhiteSpace(selectItemIdList))
                    {
                        CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                        CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                        costCentreTabBO = costCentreTabDA.GetCostCenterDetailInformation("Restaurant", billID);

                        if (costCentreTabBO != null)
                        {
                            if (costCentreTabBO.InvoiceTemplate > 0)
                            {
                                if (costCentreTabBO.InvoiceTemplate == 1)
                                {
                                    hfBillTemplate.Value = "1";
                                    this.ReportProcessing("rptRestaurentBillForPos");
                                }
                                else if (costCentreTabBO.InvoiceTemplate == 2)
                                {
                                    hfBillTemplate.Value = "2";
                                    this.ReportProcessing("rptRestaurentBillForDotMatrix");
                                }
                                else if (costCentreTabBO.InvoiceTemplate == 3)
                                {
                                    hfBillTemplate.Value = "3";
                                    this.ReportProcessing("rptRestaurentBillTwoColumn");
                                }
                                else if (costCentreTabBO.InvoiceTemplate == 4)
                                {
                                    hfBillTemplate.Value = "4";
                                    this.ReportProcessingForPosToken("rptRestaurentBillForPosToken");
                                }
                            }
                        }
                    }
                }
            }
        }
        // Template 1 -- POS, Template 2---- Dot Matrix, Template 3 -- Double Column
        private void ReportProcessing(string reportName)
        {
            //string queryStringId = Request.QueryString["billId"];
            //int billID = Int32.Parse(queryStringId);

            string queryStringId = Request.QueryString["billId"];
            if ((queryStringId.Split('~')[0]).Length > 0)
            {
                int billID = Int32.Parse(queryStringId.Split('~')[0]);
                string selectItemIdList = queryStringId.Split('~')[1];


                if (!string.IsNullOrEmpty(queryStringId))
                {
                    rvTransaction.ProcessingMode = ProcessingMode.Local;
                    rvTransaction.LocalReport.DataSources.Clear();

                    var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                    if (!File.Exists(reportPath))
                        return;

                    rvTransaction.LocalReport.ReportPath = reportPath;

                    RestaurantBillBO billBO = new RestaurantBillBO();
                    RestaurentBillDA billDA = new RestaurentBillDA();
                    billBO = billDA.GetBillInfoByBillId(billID);

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

                    this.IsRestaurantOrderSubmitDisableInfo();

                    if (IsRestaurantOrderSubmitDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                    }

                    this.IsRestaurantTokenInfoDisableInfo();

                    if (IsRestaurantTokenInfoDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                    }

                    string IsCostCenterNameShowOnInvoice = "1";
                    reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

                    HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
                    isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
                    if (isRestaurantIntegrateWithFrontOfficeBO != null)
                    {
                        if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
                        {
                            reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
                        }
                        else
                        {
                            reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
                        }
                    }

                    DateTime currentDate = DateTime.Now;
                    HMCommonDA printDateDA = new HMCommonDA();

                    string printDate = string.Empty;
                    RestaurantBillBO billInfoBO = new RestaurantBillBO();
                    billInfoBO = billDA.GetBillInfoByBillId(billID);
                    if (billInfoBO != null)
                    {
                        printDate = hmUtility.GetDateTimeStringFromDateTime(billInfoBO.BillDate);
                            //hmUtility.GetStringFromDateTime(billInfoBO.BillDate) + " " + billInfoBO.BillDate.ToString("hh:mm:ss tt");
                    }
                    else
                    {
                        printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                    }

                    reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                    rvTransaction.LocalReport.SetParameters(reportParam);

                    RestaurentBillDA rda = new RestaurentBillDA();
                    List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                    restaurantBill = rda.GetRestaurantBillReport(billID);

                    decimal netSales = 0;
                    decimal discount = 0;
                    //decimal afterDiscount = 0;
                    //decimal serviceCharge = 0;
                    //decimal vatAmount = 0;
                    //decimal grandTotal = 0;

                    List<RestaurantBillReportBO> restaurantBillInvoice = new List<RestaurantBillReportBO>();
                    string[] itemIdList = selectItemIdList.Split(',');
                    foreach (string row in itemIdList)
                    {
                        if (row.Length > 0)
                        {
                            RestaurantBillReportBO restaurantInvoice = new RestaurantBillReportBO();
                            restaurantInvoice = restaurantBill.Where(x => x.ItemId.ToString() == row).FirstOrDefault();
                            if (restaurantInvoice.ItemUnit > 1)
                            {
                                restaurantInvoice.NetAmount = (restaurantInvoice.NetAmount / restaurantInvoice.ItemUnit);
                                restaurantInvoice.Amount = (restaurantInvoice.Amount / restaurantInvoice.ItemUnit);
                                restaurantInvoice.ItemUnit = 1;
                            }
                            else
                            {
                                restaurantInvoice.ItemUnit = 1;
                            }

                            netSales += !string.IsNullOrWhiteSpace(restaurantInvoice.Amount.ToString()) ? Convert.ToDecimal(restaurantInvoice.Amount) : 0;

                            restaurantBillInvoice.Add(restaurantInvoice);
                        }
                    }

                    foreach (RestaurantBillReportBO invoiceBO in restaurantBillInvoice)
                    {
                        invoiceBO.DiscountTitle = "Discount";
                        invoiceBO.DiscountAmount = discount;
                        invoiceBO.DiscountedAmount = netSales;

                        int isInclusiveBill = !string.IsNullOrWhiteSpace(invoiceBO.IsInclusiveBill.ToString()) ? Convert.ToInt32(invoiceBO.IsInclusiveBill) : 0;
                        int isVatServiceChargeEnable = !string.IsNullOrWhiteSpace(invoiceBO.IsVatServiceChargeEnable.ToString()) ? Convert.ToInt32(invoiceBO.IsVatServiceChargeEnable) : 0;

                        invoiceBO.ServiceCharge = VatOrServiceChargeCalculation("SCharge", netSales, invoiceBO.RestaurantVatString, invoiceBO.RestaurantServiceChargeString, isInclusiveBill, isVatServiceChargeEnable);
                        invoiceBO.VatAmount = VatOrServiceChargeCalculation("Vat", netSales, invoiceBO.RestaurantVatString, invoiceBO.RestaurantServiceChargeString, isInclusiveBill, isVatServiceChargeEnable);


                        if (isInclusiveBill == 0)
                        {
                            invoiceBO.NetAmount = netSales;
                            invoiceBO.TotalSales = invoiceBO.NetAmount;
                            invoiceBO.GrandTotal = invoiceBO.DiscountedAmount + invoiceBO.ServiceCharge + invoiceBO.VatAmount;
                        }
                        else
                        {
                            invoiceBO.NetAmount = (netSales - invoiceBO.ServiceCharge - invoiceBO.VatAmount);
                            invoiceBO.TotalSales = invoiceBO.NetAmount;
                            invoiceBO.GrandTotal = invoiceBO.DiscountedAmount;
                        }
                    }

                    var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBillInvoice));

                    rvTransaction.LocalReport.DisplayName = "Bill Information";
                    rvTransaction.LocalReport.Refresh();
                }
            }
        }
        private decimal VatOrServiceChargeCalculation(string calculationType, decimal netAmount, string restaurantVatString, string restaurantServiceChargeString, int IsInclusiveBill, int IsVatServiceChargeEnable)
        {
            decimal returnAmount = 0;
            decimal calculatedPercentAmount = 0;
            decimal paramServiceCharge = 0;
            decimal paramVatAmount = 0;

            paramVatAmount = !string.IsNullOrWhiteSpace(restaurantVatString) ? Convert.ToDecimal(restaurantVatString) : 0;
            paramServiceCharge = !string.IsNullOrWhiteSpace(restaurantServiceChargeString) ? Convert.ToDecimal(restaurantServiceChargeString) : 0;

            paramVatAmount = ((100 + paramServiceCharge) * paramVatAmount / 100);

            //-- Calculated Total Percent Amount----------
            if (IsInclusiveBill == 0)
            {
                calculatedPercentAmount = 100;
            }
            else
            {
                calculatedPercentAmount = (100 + paramVatAmount + paramServiceCharge);
            }

            if (calculationType == "Vat")
            {
                returnAmount = ((netAmount * paramVatAmount) / calculatedPercentAmount);
            }
            else
            {
                returnAmount = ((netAmount * paramServiceCharge) / calculatedPercentAmount);
            }

            return returnAmount;
        }
        private void ReportProcessingForPosToken(string reportName)
        {
            //string queryStringId = Request.QueryString["billId"];
            //int billID = Int32.Parse(queryStringId);

            string queryStringId = Request.QueryString["billId"];
            if ((queryStringId.Split('~')[0]).Length > 0)
            {
                int billID = Int32.Parse(queryStringId.Split('~')[0]);
                string selectItemIdList = queryStringId.Split('~')[1];

                if (!string.IsNullOrEmpty(queryStringId))
                {
                    rvTransaction.ProcessingMode = ProcessingMode.Local;
                    rvTransaction.LocalReport.DataSources.Clear();

                    var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                    if (!File.Exists(reportPath))
                        return;

                    rvTransaction.LocalReport.ReportPath = reportPath;

                    RestaurantBillBO billBO = new RestaurantBillBO();
                    RestaurentBillDA billDA = new RestaurentBillDA();
                    billBO = billDA.GetBillInfoByBillId(billID);

                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();
                    List<ReportParameter> reportParam = new List<ReportParameter>();

                    if (files[0].CompanyId > 0)
                    {
                        reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                        reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

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

                    reportParam.Add(new ReportParameter("ReportTitle", "KOT"));
                    reportParam.Add(new ReportParameter("CostCenter", ""));
                    reportParam.Add(new ReportParameter("SourceName", ""));
                    reportParam.Add(new ReportParameter("TableNo", ""));
                    reportParam.Add(new ReportParameter("KotNo", ""));
                    reportParam.Add(new ReportParameter("KotDate", DateTime.Now.ToString()));
                    reportParam.Add(new ReportParameter("WaiterName", ""));
                    reportParam.Add(new ReportParameter("RestaurantName", files[0].CompanyName));

                    this.IsRestaurantOrderSubmitDisableInfo();

                    if (IsRestaurantOrderSubmitDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                    }

                    this.IsRestaurantTokenInfoDisableInfo();


                    if (IsRestaurantTokenInfoDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                    }

                    string IsCostCenterNameShowOnInvoice = "1";
                    reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

                    HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
                    isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
                    if (isRestaurantIntegrateWithFrontOfficeBO != null)
                    {
                        if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
                        {
                            reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
                        }
                        else
                        {
                            reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
                        }
                    }

                    DateTime currentDate = DateTime.Now;
                    HMCommonDA printDateDA = new HMCommonDA();
                    string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                    reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                    RestaurentBillDA rda = new RestaurentBillDA();
                    KotBillDetailDA entityDA = new KotBillDetailDA();

                    List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                    List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

                    restaurantBill = rda.GetRestaurantBillReport(billID);

                    reportParam.Add(new ReportParameter("KotNo", restaurantBill[0].KotId.ToString()));

                    entityBOList = entityDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "AllItem", false);

                    reportParam.Add(new ReportParameter("SpecialRemarks", entityBOList[0].Remarks));
                    rvTransaction.LocalReport.SetParameters(reportParam);

                    List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                    List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                    List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                    var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], kotOrderSubmitEntityBOList));
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[2], changedOrEditedEntityBOList));
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[3], voidOrDeletedItemEntityBOList));

                    rvTransaction.LocalReport.DisplayName = "Restaurant Bill";
                    rvTransaction.LocalReport.Refresh();
                }
            }
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