using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.Restaurant;
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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.RetailPOS;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmRetailPos : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        public static object RestaurantKotBillMaster { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                getPOSRefundConfiguration();
                getIsMembershipPaymentEnable();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithGuestCompany", "IsRestaurantIntegrateWithGuestCompany");
                if (invoiceTemplateBO.SetupId > 0) { hfIsRestaurantIntegrateWithCompany.Value = invoiceTemplateBO.SetupValue; }



                HMCommonSetupBO isRestaurantPaxConfirmationEnableBO = new HMCommonSetupBO();
                isRestaurantPaxConfirmationEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsCostCenterWiseBillNumberGenerate", "IsCostCenterWiseBillNumberGenerate");
                hfIsCostCenterWiseBillNumberGenerate.Value = isRestaurantPaxConfirmationEnableBO.SetupValue;

                if (Request.QueryString["cid"] != null)
                {
                    int costCenterId = Convert.ToInt32(Request.QueryString["cid"].ToString());
                    LoadNSetBasicInfo(costCenterId);
                }
                else
                {
                    LoadNSetBasicInfo(0);
                }
            }
        }


        private void getPOSRefundConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("POSRefundConfiguration", "POSRefundConfiguration");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfPOSRefundConfiguration.Value = setUpBO.SetupValue;
            }

        }

        private void getIsMembershipPaymentEnable()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsMembershipPaymentEnable", "IsMembershipPaymentEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsMembershipPaymentEnable.Value = setUpBO.SetupValue;


            }

        }

        private void LoadNSetBasicInfo(int costCenterId)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            if (costCenterId == 0)
            {
                List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
                costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByType("RetailPos");

                if (costCentreTabBOList.Count > 0)
                {
                    var vc = costCentreTabBOList.Where(c => c.CostCenterType == "RetailPos").ToList();

                    if (vc.Count > 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Cost Center Is Not Setup Properly. Please Setup Cost Center and Try Again.", AlertType.Error);
                        return;
                    }

                    costCenterId = costCentreTabBOList[0].CostCenterId;
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Cost Center Is Not Setup Properly. Please Setup Cost Center and Try Again.", AlertType.Error);
                    return;
                }
            }

            hfCostcenterId.Value = costCenterId.ToString();

            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(costCenterId);
            if (costCentreTabBO.CostCenterId > 0)
            {
                hfBillPrefixCostcentrwise.Value = costCentreTabBO.BillNumberPrefix;
                if (costCentreTabBO.IsVatEnable == true)
                {
                    hfIsVatEnable.Value = "1";
                    hfRestaurantVatAmount.Value = costCentreTabBO.VatAmount.ToString();
                }
                else
                {
                    hfIsVatEnable.Value = "0";
                    hfRestaurantVatAmount.Value = "0";
                }

                if (costCentreTabBO.IsVatSChargeInclusive == 1)
                    hfIsRestaurantBillInclusive.Value = "1";
                else
                    hfIsRestaurantBillInclusive.Value = "0";

                HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
                isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");

                hfIsRestaurantBillAmountWillRound.Value = isRestaurantBillAmountWillRoundBO.SetupValue.ToString();
            }
        }

        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            string reportName = "rptRetailPosBill";

            string queryStringId = hfBillId.Value;
            int billID = Int32.Parse(queryStringId);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                rvTransactionShow.LocalReport.DataSources.Clear();

                var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransactionShow.LocalReport.ReportPath = reportPath;

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
                    reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                }

                rvTransactionShow.LocalReport.EnableExternalImages = true;
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

                rvTransactionShow.LocalReport.SetParameters(reportParam);

                //RestaurentBillDA rda = new RestaurentBillDA();
                //List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                //restaurantBill = rda.GetRestaurantBillReport(billID);

                RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
                RestaurentPosDA rda = new RestaurentPosDA();
                restaurantBill = rda.RetailPosBill(billID);

                var dataSet = rvTransactionShow.LocalReport.GetDataSourceNames();
                rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill.PosBillWithSalesReturn));
                rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], restaurantBill.PosSalesReturnPayment));

                rvTransactionShow.LocalReport.DisplayName = "Bill Invoice";
                rvTransactionShow.LocalReport.Refresh();

                string url = "$('#reportContainer').dialog({ " +
                       " autoOpen: true, " +
                       " modal: true, " +
                       " minWidth: 500, " +
                       " minHeight: 555, " +
                       " width: 'auto', " +
                       " closeOnEscape: false, " +
                       " resizable: false, " +
                       " height: 'auto', " +
                       " fluid: true, " +
                       " title: 'Invoice Preview', " +
                       " show: 'slide', " +
                       " close: ClosePrintDialog " +
                       "});" + "$('.ui-dialog-titlebar-close').css({ " +
                        " 'top': '27%', " +
                        " 'width': '40', " +
                        " 'height': '40', " +
                        " 'background-repeat': 'no-repeat', " +
                        " 'background-position': 'center center' " +
                        " }); " +
                        " $('.ui-dialog-titlebar').css('padding','0.8em 1em'); " +
                        " setTimeout(function () { ScrollToDown(); }, 1000); ";

                ClientScript.RegisterStartupScript(this.GetType(), "script", url, true);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvTransactionShow.LocalReport.Render("PDF", null, out mimeType,
                               out encoding, out extension, out streamids, out warnings);

                string fileName = string.Empty, fileNamePrint = string.Empty;
                DateTime dateTime = DateTime.Now;

                fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";
                fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";

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

                IframeReportPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

                //rrp.PrintForPos();

                //string url = "/Restaurant/Reports/frmReportTPRestaurantBillInfo.aspx?billID=" + this.hfBillId.Value;
                //string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes'); window.onunload = CloseWindow();";
                ////Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
                //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
        }
        protected void btnPrinReturnBillPreview_Click(object sender, EventArgs e)
        {
            string reportName = "rptRetailPosReturnBill";

            string queryStringId = hfBillId.Value;
            int billID = Int32.Parse(queryStringId);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                rvTransactionShow.LocalReport.DataSources.Clear();

                var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransactionShow.LocalReport.ReportPath = reportPath;

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
                    reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                }

                rvTransactionShow.LocalReport.EnableExternalImages = true;
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

                rvTransactionShow.LocalReport.SetParameters(reportParam);

                RestaurentPosDA rda = new RestaurentPosDA();
                RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
                restaurantBill = rda.GetRetailPosBillWithSalesReturn(billID);

                var dataSet = rvTransactionShow.LocalReport.GetDataSourceNames();
                rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill.PosBillWithSalesReturn));
                rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], restaurantBill.PosSalesReturnItem));
                rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[2], restaurantBill.PosSalesReturnPayment));

                rvTransactionShow.LocalReport.DisplayName = "Bill Invoice";
                rvTransactionShow.LocalReport.Refresh();

                string url = "$('#reportContainer').dialog({ " +
                       " autoOpen: true, " +
                       " modal: true, " +
                       " minWidth: 500, " +
                       " minHeight: 555, " +
                       " width: 'auto', " +
                       " closeOnEscape: false, " +
                       " resizable: false, " +
                       " height: 'auto', " +
                       " fluid: true, " +
                       " title: 'Invoice Preview', " +
                       " show: 'slide', " +
                       " close: ClosePrintDialog " +
                       "});" + "$('.ui-dialog-titlebar-close').css({ " +
                        " 'top': '27%', " +
                        " 'width': '40', " +
                        " 'height': '40', " +
                        " 'background-repeat': 'no-repeat', " +
                        " 'background-position': 'center center' " +
                        " }); " +
                        " $('.ui-dialog-titlebar').css('padding','0.8em 1em'); " +
                        " setTimeout(function () { ScrollToDown(); }, 1000); ";

                ClientScript.RegisterStartupScript(this.GetType(), "script", url, true);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvTransactionShow.LocalReport.Render("PDF", null, out mimeType,
                               out encoding, out extension, out streamids, out warnings);

                string fileName = string.Empty, fileNamePrint = string.Empty;
                DateTime dateTime = DateTime.Now;

                fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";
                fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";

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

                IframeReportPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

                //rrp.PrintForPos();

                //string url = "/Restaurant/Reports/frmReportTPRestaurantBillInfo.aspx?billID=" + this.hfBillId.Value;
                //string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes'); window.onunload = CloseWindow();";
                ////Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
                //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }

        private bool IsRestaurantTokenInfoDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            bool IsRestaurantTokenInfoDisable = false;

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantTokenInfoDisable", "IsRestaurantTokenInfoDisable");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        IsRestaurantTokenInfoDisable = false;
                    }
                    else
                    {
                        IsRestaurantTokenInfoDisable = true;
                    }
                }
            }

            return IsRestaurantTokenInfoDisable;
        }
        private bool IsRestaurantOrderSubmitDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            bool IsRestaurantOrderSubmitDisable = false;

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

            return IsRestaurantOrderSubmitDisable;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch(string itemCode, string itemName, string categoryName)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            CostCentreTabDA costCenterDa = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            InvItemDA itemDa = new InvItemDA();

            costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("RetailPos");

            itemInfo = itemDa.GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch(itemCode, itemName, categoryName, costCentreTabBOList[0].CostCenterId);

            return itemInfo;
        }
        [WebMethod]
        public static List<GetDiscountDetailsBO> GetAllDiscount(int costcenter)
        {
            List<GetDiscountDetailsBO> GetDiscountDetailsBOs = new List<GetDiscountDetailsBO>();
            DiscountDA discountDa = new DiscountDA();

            GetDiscountDetailsBOs = discountDa.GetAllDiscountByCostcenterId(costcenter);

            return GetDiscountDetailsBOs;
        }


        [WebMethod]
        public static ReturnInfo FullBillRefundSettlement( int memberId, RestaurantBillBO RestaurantBill, GuestBillPaymentBO BillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {
                RestaurentPosDA posda = new RestaurentPosDA();
                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                if (RestaurantBill.IsBillReSettlement && RestaurantBill.RefundId == 1)
                {
                    posda.UpdateBillForFullRefund(memberId, RestaurantBill, BillPayment);
                    //RestaurantBill.BillId
                }

                //rtninf.Pk = billId;
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillRefund, AlertType.Success);

            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo BillSettlement(int kotId, int memberId, RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> BillPayment,
                                                List<KotBillDetailBO> BillDetails, List<KotBillDetailBO> EditeDetails,
                                                List<KotBillDetailBO> DeletedDetails, List<RestaurantSalesReturnItemBO> SalesReturnItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            KotBillMasterBO billmaster = new KotBillMasterBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            CostCentreTabDA costCenterDa = new CostCentreTabDA();
            RestaurentPosDA posda = new RestaurentPosDA();
            HMUtility hmUtility = new HMUtility();

            RestaurantBill.ProjectId = 1;

            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            int billId = 0;

            try
            {
                costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("RetailPos");

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                billmaster.KotId = kotId;

                RestaurantBill.BearerId = userInformationBO.UserInfoId;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;
                RestaurantBill.SOrderId = 0;

                billmaster.CostCenterId = costCentreTabBOList[0].CostCenterId;

                if (RestaurantBill.IsBillReSettlement)
                {
                    billmaster.ReferenceKotId = kotId;
                    billmaster.IsKotReturn = true;
                    kotId = 0;
                }

                RestaurantBill.CostCenterId = billmaster.CostCenterId;


                if (kotId == 0)
                {
                    billmaster.SourceId = 1;
                    billmaster.PaxQuantity = 1;
                    billmaster.SourceName = "RestaurantToken";
                    billmaster.BearerId = userInformationBO.UserInfoId;
                    billmaster.KotStatus = ConstantHelper.KotStatus.settled.ToString();
                    billmaster.CreatedBy = userInformationBO.UserInfoId;
                    billmaster.IsBillHoldup = false;

                    posda.SaveRestaurantBillForPos("RetailPOS", billmaster, BillDetails, RestaurantBill, BillPayment, SalesReturnItem, null, true, true, out billId, memberId);
                }
                else if (kotId > 0 && RestaurantBill.BillId == 0)
                {
                    billmaster.IsBillHoldup = false;
                    billmaster.IsBillProcessed = true;
                    billmaster.KotStatus = "settled";

                    RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                    RestaurantBill.BillPaidBySourceId = kotId;
                    RestaurantBill.CreatedBy = userInformationBO.UserInfoId;

                    posda.UpdateRestaurantBillForPos("RetailPOS", kotId, billmaster, BillDetails, EditeDetails, DeletedDetails, RestaurantBill, BillPayment, null, true, true, out billId);
                }
                else if (kotId > 0 && RestaurantBill.BillId > 0)
                {
                    billId = RestaurantBill.BillId;
                    RestaurantBill.BillPaidBySourceId = kotId;

                    posda.UpdateForRestauranBillReSettlement("RetailPOS", kotId, RestaurantBill, BillDetails, EditeDetails, DeletedDetails, BillPayment);
                }

                posda.SaveMembershipPointDetails(RestaurantBill, memberId, billId);

                rtninf.Pk = billId;
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillSettlement, AlertType.Success);

            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo BillHoldup(int kotId, List<KotBillDetailBO> BillDetails, List<KotBillDetailBO> EditeDetails, List<KotBillDetailBO> DeletedDetails)
        {
            ReturnInfo rtninf = new ReturnInfo();
            KotBillMasterBO billmaster = new KotBillMasterBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            CostCentreTabDA costCenterDa = new CostCentreTabDA();
            RestaurentPosDA posda = new RestaurentPosDA();

            HMUtility hmUtility = new HMUtility();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            foreach (KotBillDetailBO row in BillDetails)
            {
                row.CreatedBy = userInformationBO.UserInfoId;
            }

            int newKotId = 0;

            try
            {
                costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("RetailPos");
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (kotId == 0)
                {
                    billmaster.SourceId = 1;
                    billmaster.PaxQuantity = 1;
                    billmaster.SourceName = "RestaurantToken";
                    billmaster.BearerId = userInformationBO.UserInfoId;
                    billmaster.CostCenterId = costCentreTabBOList[0].CostCenterId;
                    billmaster.CreatedBy = userInformationBO.UserInfoId;
                    billmaster.IsBillHoldup = true;
                    billmaster.KotStatus = ConstantHelper.KotStatus.pending.ToString();
                    posda.SaveRestaurantBillHoldUpForPos(billmaster, BillDetails, out newKotId);
                }
                else
                {
                    posda.UpdateRestaurantBillHoldUpForPos(kotId, BillDetails, EditeDetails, DeletedDetails);
                }

                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.HoldUp, AlertType.Success);

            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static List<RestaurantTokenBO> GetHoldUpPosInfo()
        {
            RestaurentPosDA tokenDa = new RestaurentPosDA();
            List<RestaurantTokenBO> tokenList = new List<RestaurantTokenBO>();

            tokenList = tokenDa.GetHoldUpPosInfo();

            return tokenList;
        }

        [WebMethod]
        public static MemMemberBasicsBO GetPointsByCustomerCode(string customerCode)
        {
            RestaurentPosDA posDA = new RestaurentPosDA();
            MemMemberBasicsBO memberBO = posDA.GetPointsByCustomerCode(customerCode);

            return memberBO;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> GetOrderedItemByKotId(int kotId)
        {
            RestaurentPosDA posda = new RestaurentPosDA();
            CostCentreTabDA costCenterDa = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("RetailPos");

            itemInfo = posda.GetOrderedItemByKotId(kotId, costCentreTabBOList[0].CostCenterId);

            return itemInfo;
        }
        [WebMethod]
        public static RestaurantBillPaymentResume BillEdit(string billNumberOrId)
        {

            int billId = 0;
            RestaurentBillDA rda = new RestaurentBillDA();

            if (billNumberOrId.Take(2).All(char.IsDigit))
            {
                billId = Int32.Parse(billNumberOrId);
            }
            else
            {
                billId = rda.GetBillPaymentByBillId(billNumberOrId);
            }

            RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();

            if (billId == 0)
            {
                paymentResume.IsSuccess = false;
                return paymentResume;
            }

            KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
            RestaurentPosDA posda = new RestaurentPosDA();
            KotBillMasterDA kotDa = new KotBillMasterDA();
            RestaurentBillDA billDa = new RestaurentBillDA();
            RestaurentPosDA posDa = new RestaurentPosDA();
            KotBillDetailDA kotDetailsDA = new KotBillDetailDA();
            InvCategoryDA catDa = new InvCategoryDA();

            KotBillMasterBO kotBillMaster = new KotBillMasterBO();
            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();

            RestaurantBillBO kotBill = new RestaurantBillBO();
            List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();
            List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();
            List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();
            GuestCompanyDA bpDA = new GuestCompanyDA();
            GuestCompanyBO guestCompany = new GuestCompanyBO();

            string kotIdList = string.Empty, tableIdList = string.Empty;

            billDetailList = billDa.GetRestaurantBillDetailsByBillId(billId);
            kotBillMaster = kotBillMasterDA.GetKotBillMasterInfoKotId(billDetailList[0].KotId);
            kotBill = posda.GetRetailsPosBillByKotId(kotBillMaster.KotId, kotBillMaster.SourceName);

            if (kotBill.IsBillReSettlement)
            {
                paymentResume.IsSuccess = false;
                return paymentResume;
            }

            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            itemInfo = posda.GetOrderedItemByKotId(billDetailList[0].KotId, kotBillMaster.CostCenterId);

            if (kotBill != null)
            {
                kotBillPayment = billDa.GetBillPaymentByBillId(kotBill.BillId, "RetailPos");
                billDetailList = billDa.GetRestaurantBillDetailsByBillId(kotBill.BillId);
                classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(kotBill.BillId);
            }

            billDetailList = billDetailList.Where(b => b.KotId != kotBillMaster.KotId).ToList();
            kotDetails = kotDetailsDA.GetRestaurantOrderItemByMultipleKotId(kotBillMaster.CostCenterId.ToString(), kotIdList, kotBillMaster.SourceName);

            MembershipPointDetailsBO membershipPointDetails = new MembershipPointDetailsBO();
            membershipPointDetails = posda.GetMembershipPointDetails(billId);

            paymentResume.KotBillMaster = kotBillMaster;
            paymentResume.OrderItem = itemInfo;
            paymentResume.KotBillDetails = kotDetails;
            paymentResume.RestaurantKotBill = kotBill;
            paymentResume.RestaurantKotBillPayment = kotBillPayment;
            paymentResume.membershipPointDetails = membershipPointDetails;
            if (kotBill.TransactionType == "Company")
            {
                paymentResume.guestCompanyBO = bpDA.GetGuestCompanyInfoById((int)(kotBill.TransactionId));

            }

            paymentResume.IsSuccess = true;

            HttpContext.Current.Session["RestaurantKotBillResumeForPos"] = paymentResume;

            return paymentResume;
        }

    }
}