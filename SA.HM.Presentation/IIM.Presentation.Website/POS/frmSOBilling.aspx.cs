using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Inventory;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HotelManagement;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmSOBilling : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int LocalCurrencyId;
        protected int ShowReportFrame = 0;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["cid"] != null)
                {
                    int costCenterId = Convert.ToInt32(Request.QueryString["cid"].ToString());
                    LoadCostCenter(costCenterId);
                }
                else
                {
                    LoadCostCenter(0);
                }

                LoadCategory();
                LoadCurrency();
                LoadStockBy();
                LoadRegisteredGuestCompanyInfo();
                LoadLocalCurrencyId();
                IsLocalCurrencyDefaultSelected();
                LoadBank();
                LoadTemplateNo();
                LoadAllCostCentreTabInfo();
            }
        }
        protected void btnSettlement_Click(object sender, EventArgs e)
        {
            KotBillMasterBO billmasterBO = new KotBillMasterBO();
            KotBillMasterDA billmasterDA = new KotBillMasterDA();
            KotBillDetailBO billdetailBO = new KotBillDetailBO();
            KotBillDetailDA billdetailDA = new KotBillDetailDA();
            RestaurantBillBO restaurantBillBO = new RestaurantBillBO();
        }
        protected void btnInvoiceGenerate_Click(object sender, EventArgs e)
        {
            string reportName = "rptRestaurentBillForPos";
            string stnbillId = hfBillId.Value;
            hfBillId.Value = stnbillId;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrEmpty(stnbillId))
            {
                ShowReportFrame = 1;

                int billID = Int32.Parse(stnbillId);

                rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                rvTransactionShow.LocalReport.DataSources.Clear();

                var reportPath = Server.MapPath(@"~/PointOfSales/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransactionShow.LocalReport.ReportPath = reportPath;

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

                rvTransactionShow.LocalReport.EnableExternalImages = true;
                HMCommonDA hmCommonDA = new HMCommonDA();
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));
                reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetStringFromDateTime(currentDate) + " " + currentDate.ToString("hh:mm:ss tt");

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                rvTransactionShow.LocalReport.SetParameters(reportParam);

                RestaurentBillDA rda = new RestaurentBillDA();
                List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                restaurantBill = rda.GetRestaurantBillReport(billID);

                var dataSet = rvTransactionShow.LocalReport.GetDataSourceNames();
                rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                rvTransactionShow.LocalReport.DisplayName = "Invoice";
                rvTransactionShow.LocalReport.Refresh();

                string url = "$('#LoadReport').dialog({ " +
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
            }
        }
        //************************ User Defined Function ********************//
        private void LoadAllCostCentreTabInfo()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            costCentreTabBOList = entityDA.GetAllRestaurantTypeCostCentreTabInfo();

            System.Web.UI.WebControls.ListItem item2 = new System.Web.UI.WebControls.ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstAllValue();

            this.ddlSrcCostCenter.DataSource = costCentreTabBOList;
            this.ddlSrcCostCenter.DataTextField = "CostCenter";
            this.ddlSrcCostCenter.DataValueField = "CostCenterId";
            this.ddlSrcCostCenter.DataBind();
            this.ddlSrcCostCenter.Items.Insert(0, item2);
        }
        public void LoadCostCenter(int costCenterId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetUserWiseAllRestaurantTypeCostCentreTabInfo(userInformationBO.UserInfoId).Where(x => x.CostCenterId == costCenterId).ToList();

            this.ddlCostCenterId.DataSource = List;
            this.ddlCostCenterId.DataTextField = "CostCenter";
            this.ddlCostCenterId.DataValueField = "CostCenterId";
            this.ddlCostCenterId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            if (List.Count > 1)
            {
                this.ddlCostCenterId.Items.Insert(0, item);
            }

            this.ddlCostCenterId.SelectedValue = costCenterId.ToString();
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            this.ddlChequeBankId.DataSource = entityBOList;
            this.ddlChequeBankId.DataTextField = "BankName";
            this.ddlChequeBankId.DataValueField = "BankId";
            this.ddlChequeBankId.DataBind();
        }
        private void LoadCategory()
        {
            InvCategoryDA categoryDA = new InvCategoryDA();
            var category = categoryDA.GetAllInvItemCatagoryInfoByServiceType("Product");

            this.ddlCategory.DataSource = category;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            System.Web.UI.WebControls.ListItem item1 = new System.Web.UI.WebControls.ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item1);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCurrency.Items.Insert(0, item);
        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            this.ddlStockBy.DataSource = headListBO;
            this.ddlStockBy.DataTextField = "HeadName";
            this.ddlStockBy.DataValueField = "UnitHeadId";
            this.ddlStockBy.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlStockBy.Items.Insert(0, item);
        }
        private void LoadRegisteredGuestCompanyInfo()
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            ddlCompany.DataSource = companyDa.GetGuestCompanyInfo();
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();
        }
        private void IsLocalCurrencyDefaultSelected()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsLocalCurrencyDefaultSelected", "IsLocalCurrencyDefaultSelected");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        CommonCurrencyDA headDA = new CommonCurrencyDA();
                        List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
                        currencyListBO = headDA.GetConversionHeadInfoByType("All");

                        ddlCurrency.DataSource = currencyListBO;
                        ddlCurrency.DataTextField = "CurrencyName";
                        ddlCurrency.DataValueField = "CurrencyId";
                        ddlCurrency.DataBind();

                        CommonCurrencyBO currencyBO = currencyListBO.Where(x => x.CurrencyType == "Local").SingleOrDefault();
                        ddlCurrency.SelectedValue = currencyBO.CurrencyId.ToString();
                    }
                }
            }
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void LoadTemplateNo()
        {
            HMCommonSetupBO commonSetupBO1 = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBO2 = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO1 = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantCreditSaleEnable", "IsRestaurantCreditSaleEnable");
            commonSetupBO2 = commonSetupDA.GetCommonConfigurationInfo("IsPayFirstExtraButtonShow", "IsPayFirstExtraButtonShow");

            if (commonSetupBO1 != null)
            {
                if (commonSetupBO1.SetupId > 0)
                {
                    hfCreditSale.Value = "1";//commonSetupBO1.SetupValue;
                }
            }
            if (commonSetupBO2 != null)
            {
                if (commonSetupBO2.SetupId > 0)
                {
                    hfExtraButtonShowHide.Value = commonSetupBO2.SetupValue;
                    if (hfExtraButtonShowHide.Value == "1")
                    {
                        btnChallan.Visible = true;
                    }
                    else btnChallan.Visible = false;
                }
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemNCategoryAutoSearch(string itemName, int costcenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            int categoryId = 0;

            InvItemDA itemDA = new InvItemDA();
            itemInfo = itemDA.GetItemNameForAutoSearch(itemName, categoryId, costcenterId);
            return itemInfo;
        }
        [WebMethod]
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
        [WebMethod]
        public static int[] SaveSettlement(int salesOrderId, string saveType, int kotId, int restaurantBillId, KotBillMasterBO billmasterBO, List<KotBillDetailBO> billdetailList, RestaurantBillBO restaurantBill, List<GuestBillPaymentBO> guestBillPayment)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            for (int i = 0; i < guestBillPayment.Count; i++)
            {
                if (guestBillPayment[i] == null)
                {
                    guestBillPayment.Remove(guestBillPayment[i]);
                }
            }
            foreach (GuestBillPaymentBO bo in guestBillPayment)
            {
                bo.PaymentDate = DateTime.Now;
            }

            int[] returnArray = new int[10];

            KotBillMasterDA billmasterDA = new KotBillMasterDA();
            KotBillDetailDA billdetailDA = new KotBillDetailDA();
            RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
            HMCommonDA hmCommonDA = new HMCommonDA();

            billmasterBO.BearerId = userInformationBO.UserInfoId;
            billmasterBO.CreatedBy = userInformationBO.UserInfoId;
            billmasterBO.IsBillHoldup = false;
            bool status = false;
            if (kotId == 0)
            {
                status = billmasterDA.SaveKotBillMasterInfoForNewTouchScreen(billmasterBO, out kotId);
                if (status)
                {
                    foreach (KotBillDetailBO bo in billdetailList)
                    {
                        bo.KotId = kotId;
                        status = billdetailDA.SaveKotBillDetailInfo(bo);
                    }
                }
            }
            else
            {
                List<KotBillDetailBO> savedbilldetailList = new List<KotBillDetailBO>();
                List<KotBillDetailBO> editedbilldetailList = new List<KotBillDetailBO>();
                savedbilldetailList = billdetailDA.GetKotBillDetailListByKotId(kotId);
                foreach (KotBillDetailBO bo in savedbilldetailList)
                {
                    KotBillDetailBO boo = billdetailList.Where(a => a.ItemId == bo.ItemId).SingleOrDefault();
                    if (boo != null)
                    {
                        editedbilldetailList.Add(boo);
                        billdetailList.Remove(boo);
                    }
                }
                foreach (KotBillDetailBO bo in editedbilldetailList)
                {
                    KotBillDetailBO boo = savedbilldetailList.Where(a => a.ItemId == bo.ItemId).SingleOrDefault();
                    if (boo != null)
                    {
                        savedbilldetailList.Remove(boo); // These becomes the deleted list
                    }
                }
                foreach (KotBillDetailBO bo in billdetailList)
                {
                    bo.KotId = kotId;
                    status = billdetailDA.SaveKotBillDetailInfo(bo);
                }
                foreach (KotBillDetailBO bo in savedbilldetailList)
                {
                    status = hmCommonDA.DeleteInfoById("RestaurantKotBillDetail", "KotDetailId", bo.KotDetailId);
                }
            }

            restaurantBill.KotId = kotId;
            restaurantBill.BearerId = userInformationBO.UserInfoId;
            restaurantBill.CreatedBy = userInformationBO.UserInfoId;
            restaurantBill.BillDate = DateTime.Now;
            restaurantBill.BillPaymentDate = DateTime.Now;

            List<RestaurantBillDetailBO> restaurantBillDetailList = new List<RestaurantBillDetailBO>();
            RestaurantBillDetailBO restaurantBillDetailBO = new RestaurantBillDetailBO();
            restaurantBillDetailBO.KotId = kotId;
            restaurantBillDetailBO.TableId = 0;
            restaurantBillDetailList.Add(restaurantBillDetailBO);

            int billID = 0;

            List<RestaurantBillDetailBO> BillDeletedDetail = new List<RestaurantBillDetailBO>();
            List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
            List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();
            List<ItemClassificationBO> AddedClassificationList = new List<ItemClassificationBO>();
            List<ItemClassificationBO> DeletedClassificationList = new List<ItemClassificationBO>();

            int isRestaurantCreditSaleEnable = 0;
            HMCommonSetupBO isRestaurantCreditSaleEnableBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            isRestaurantCreditSaleEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantCreditSaleEnable", "IsRestaurantCreditSaleEnable");

            if (isRestaurantCreditSaleEnableBO != null)
            {
                if (isRestaurantCreditSaleEnableBO.SetupValue == "1")
                {
                    isRestaurantCreditSaleEnable = 1;
                }
            }

            if (restaurantBillId == 0)
            {
                billID = restaurentBillDA.SaveRestaurantBillForAll(restaurantBill, restaurantBillDetailList, guestBillPayment, null, string.Empty, true, true, out billID);
                restaurantBill.BillId = billID;
                Boolean mStatus = restaurentBillDA.UpdatePOSMachineReaderUnitInfo(restaurantBill);
            }
            else
            {
                restaurantBill.BillId = restaurantBillId;
                billID = restaurantBillId;
                status = restaurentBillDA.UpdateRestauranBillGenerationNewSettlement(restaurantBill, restaurantBillDetailList, BillDeletedDetail, guestBillPayment, paymentUpdate, paymentDelete, AddedClassificationList, DeletedClassificationList, string.Empty, true);
            }

            if (status)
            {
                List<SMSalesOrderBO> salesOrderBOList = new List<SMSalesOrderBO>();

                SMSalesOrderBO salesOrderBO = new SMSalesOrderBO();
                salesOrderBO.SOrderId = salesOrderId;
                salesOrderBO.BillId = billID;
                salesOrderBO.DeliveryStatus = "Full";
                salesOrderBOList.Add(salesOrderBO);
                PMPurchaseOrderDA purchaseOrderDA = new PMPurchaseOrderDA();
                if (salesOrderBOList.Count > 0)
                {
                    status = purchaseOrderDA.UpdateSalesdOrder(salesOrderBOList);
                }

                returnArray[0] = kotId;
                returnArray[1] = billID;

                if (isRestaurantCreditSaleEnable == 1)
                {
                    foreach (GuestBillPaymentBO row in guestBillPayment)
                    {
                        restaurantBill.PayMode = "Company";
                        restaurantBill.PayModeSourceId = !string.IsNullOrWhiteSpace(row.CompanyId.ToString()) ? Convert.ToInt32(row.CompanyId) : 0;
                    }
                }
                Boolean statusCompany = restaurentBillDA.UpdateRestaurantBillInfoForCompany(restaurantBill);
            }
            else
            {
                returnArray[0] = 0;
                returnArray[1] = 0;
            }
            return returnArray;
        }
        [WebMethod]
        public static int[] SavePreview(int salesOrderId, int kotId, int restaurantBillId, KotBillMasterBO billmasterBO, List<KotBillDetailBO> billdetailList, RestaurantBillBO restaurantBill)
        {
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int[] returnArray = new int[10];

            KotBillMasterDA billmasterDA = new KotBillMasterDA();
            KotBillDetailDA billdetailDA = new KotBillDetailDA();
            RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
            HMCommonDA hmCommonDA = new HMCommonDA();

            billmasterBO.BearerId = userInformationBO.UserInfoId;
            billmasterBO.CreatedBy = userInformationBO.UserInfoId;
            billmasterBO.IsBillHoldup = false;

            bool status = false;
            if (kotId == 0)
            {
                status = billmasterDA.SaveKotBillMasterInfoForNewTouchScreen(billmasterBO, out kotId);
                if (status)
                {
                    foreach (KotBillDetailBO bo in billdetailList)
                    {
                        bo.KotId = kotId;
                        status = billdetailDA.SaveKotBillDetailInfo(bo);
                    }
                }
            }
            else
            {
                List<KotBillDetailBO> savedbilldetailList = new List<KotBillDetailBO>();
                List<KotBillDetailBO> editedbilldetailList = new List<KotBillDetailBO>();
                savedbilldetailList = billdetailDA.GetKotBillDetailListByKotId(kotId);
                foreach (KotBillDetailBO bo in savedbilldetailList)
                {
                    KotBillDetailBO boo = billdetailList.Where(a => a.ItemId == bo.ItemId).SingleOrDefault();
                    if (boo != null)
                    {
                        editedbilldetailList.Add(boo);
                        billdetailList.Remove(boo);
                    }
                }
                foreach (KotBillDetailBO bo in editedbilldetailList)
                {
                    KotBillDetailBO boo = savedbilldetailList.Where(a => a.ItemId == bo.ItemId).SingleOrDefault();
                    if (boo != null)
                    {
                        savedbilldetailList.Remove(boo); // These becomes the deleted list
                    }
                }
                foreach (KotBillDetailBO bo in billdetailList)
                {
                    bo.KotId = kotId;
                    status = billdetailDA.SaveKotBillDetailInfo(bo);
                }
                foreach (KotBillDetailBO bo in savedbilldetailList)
                {
                    status = hmCommonDA.DeleteInfoById("RestaurantKotBillDetail", "KotDetailId", bo.KotDetailId);
                }
            }

            restaurantBill.KotId = kotId;
            restaurantBill.BearerId = userInformationBO.UserInfoId;
            restaurantBill.CreatedBy = userInformationBO.UserInfoId;
            restaurantBill.BillDate = DateTime.Now;
            restaurantBill.BillPaymentDate = DateTime.Now;

            List<RestaurantBillDetailBO> restaurantBillDetailList = new List<RestaurantBillDetailBO>();
            RestaurantBillDetailBO restaurantBillDetailBO = new RestaurantBillDetailBO();
            restaurantBillDetailBO.KotId = kotId;
            restaurantBillDetailBO.TableId = 0;
            restaurantBillDetailList.Add(restaurantBillDetailBO);

            List<GuestBillPaymentBO> guestBillPayment = new List<GuestBillPaymentBO>();

            int billID = 0;
            if (restaurantBillId == 0)
            {
                if (status)
                {
                    //int billId = restaurentBillDA.SaveRestaurantBillForAll(restaurantBill, restaurantBillDetailList, null, null, string.Empty, false, false, out billID);
                    int billId = restaurentBillDA.SaveRestaurantBillForNewHoldUp(restaurantBill, restaurantBillDetailList, null, null, out billID);
                    restaurantBill.BillId = billId;
                    Boolean mStatus = restaurentBillDA.UpdatePOSMachineReaderUnitInfo(restaurantBill);

                    List<SMSalesOrderBO> salesOrderBOList = new List<SMSalesOrderBO>();

                    SMSalesOrderBO salesOrderBO = new SMSalesOrderBO();
                    salesOrderBO.SOrderId = salesOrderId;
                    salesOrderBO.BillId = billId;
                    salesOrderBOList.Add(salesOrderBO);
                    PMPurchaseOrderDA purchaseOrderDA = new PMPurchaseOrderDA();
                    if (salesOrderBOList.Count > 0)
                    {
                        status = purchaseOrderDA.UpdateSalesdOrderForBillId(salesOrderBOList);
                    }

                }
            }
            else
            {
                //if (status)
                //{
                billID = restaurantBillId;
                restaurantBill.BillId = restaurantBillId;
                //status = restaurentBillDA.UpdateRestaurantBillForPayfirstPOS(restaurantBill, guestBillPayment, 0);
                status = restaurentBillDA.UpdateRestaurantBillForNewHoldUp(restaurantBill, null, null, null, null, null, null, null);


                List<SMSalesOrderBO> salesOrderBOList = new List<SMSalesOrderBO>();

                SMSalesOrderBO salesOrderBO = new SMSalesOrderBO();
                salesOrderBO.SOrderId = salesOrderId;
                salesOrderBO.BillId = billID;
                salesOrderBOList.Add(salesOrderBO);
                PMPurchaseOrderDA purchaseOrderDA = new PMPurchaseOrderDA();
                if (salesOrderBOList.Count > 0)
                {
                    status = purchaseOrderDA.UpdateSalesdOrderForBillId(salesOrderBOList);
                }
            }

            if (status)
            {
                returnArray[0] = kotId;
                returnArray[1] = billID;
            }
            else
            {
                returnArray[0] = 0;
                returnArray[1] = 0;
            }
            return returnArray;
        }
        [WebMethod]
        public static string LoadSalesOrder(int companyId)
        {
            PMPurchaseOrderDA purchaseOrderDA = new PMPurchaseOrderDA();
            List<SMSalesOrderBO> salesOrderListBO = new List<SMSalesOrderBO>();
            salesOrderListBO = purchaseOrderDA.GetSMSaleseOrderInfo(companyId);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            //List<CostCentreTabBO> CostCentreTabListBO = costCentreTabDA.GetUserWiseAllRestaurantTypeCostCentreTabInfo(userInformationBO.UserInfoId);

            //List<SMSalesOrderBO> salesOrderListInfo = new List<SMSalesOrderBO>();
            //List<SMSalesOrderBO> salesOrderList = new List<SMSalesOrderBO>();

            //foreach (CostCentreTabBO row in CostCentreTabListBO)
            //{
            //    salesOrderListInfo = salesOrderListBO.Where(x => x.CostCenterId == row.CostCenterId).ToList();
            //    salesOrderList.AddRange(salesOrderListInfo);
            //}

            string strTable = string.Empty;
            int counter = 0;
            if (salesOrderListBO != null)
            {
                if (salesOrderListBO.Count > 0)
                {
                    strTable += "<table id='SalesOrder' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th style='display:none'></th><th align='left' scope='col' style='width: 15%;'>SO Number</th><th align='left' scope='col' style='width: 50%;'>Company Name</th><th align='left' scope='col' style='width: 20%;'>Depo Name</th><th align='left' scope='col' style='width: 15%;'>Option</th>";
                    strTable += "<tbody>";

                    foreach (SMSalesOrderBO bo in salesOrderListBO)
                    {
                        counter++;
                        if (counter % 2 == 0)
                        {
                            // It's even
                            strTable += "<tr style='background-color:White;'>";
                        }
                        else
                        {
                            // It's odd
                            strTable += "<tr style='background-color:#E3EAEB;'>";
                        }

                        strTable += "<td align='left' style=\"display:none;\">" + bo.SOrderId + "</td>";
                        //strTable += "<td align='left' style='width: 5%;'> <input type='checkbox'> </td>";
                        strTable += "<td align='left' style='width: 15%;'>" + bo.SONumber + "</td>";
                        strTable += "<td align='left' style='width: 50%;'>" + bo.CompanyName + "</td>";
                        strTable += "<td align='left' style='width: 20%;'>" + bo.CostCenter + "</td>";
                        //strTable += "<td align='left' style='width: 20%;'>"+"" + "</td>";
                        strTable += "<td align='left' style='width: 15%; cursor:pointer;'><img src='../Images/ReportDocument.png' onClick='javascript:return GenerateInvoice(" + bo.SOrderId + ", " + bo.CompanyId + ")' alt='Invoice' border='0' />&nbsp;&nbsp;<img src='../Images/select.png' onClick='javascript:return BillingProcessFromSalesOrder(" + bo.SOrderId + ")' alt='Billing' border='0' /></td>";

                        //strTable = "<img src='../Images/ReportDocument.png' onClick='javascript:return GenerateInvoice(" + bo.SOrderId + ", "+ bo.CompanyId +")' alt='Invoice' border='0' />";
                        //strTable += "<td align='left' style='width: 40%;'> <select class='form-control' style='width:100%;'> <option value='Full'>Full</option><option value='Partial'>Partial</option></select> </td>";

                        strTable += "</tr>";
                    }

                    strTable += "</tbody></table></div>";
                    //strTable += "<input type='button' id='btnUpdateSalesOrder' value='Update' class='TransactionalButton btn btn-primary btn-sm' onclick='javascript: return UpdateSalesdOrder();'/>";
                }
            }
            else
            {
                strTable = "No Sales Order is pending fot this Company.";
            }

            return strTable;
        }
        [WebMethod]
        public static string LoadSalesOrderInformation(string srcFromDate, string srcToDate, string costCenterId, string SONumber)
        {
            HMUtility hmUtility = new HMUtility();
            PMPurchaseOrderDA purchaseOrderDA = new PMPurchaseOrderDA();

            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(srcFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(srcFromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(srcToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(srcToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            List<SMSalesOrderBO> salesOrderListBO = new List<SMSalesOrderBO>();
            salesOrderListBO = purchaseOrderDA.GetSMSaleseOrderInformation(fromDate, toDate, costCenterId, SONumber);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> CostCentreTabListBO = costCentreTabDA.GetUserWiseAllRestaurantTypeCostCentreTabInfo(userInformationBO.UserInfoId);

            List<SMSalesOrderBO> salesOrderListInfo = new List<SMSalesOrderBO>();
            List<SMSalesOrderBO> salesOrderList = new List<SMSalesOrderBO>();

            foreach (CostCentreTabBO row in CostCentreTabListBO)
            {
                salesOrderListInfo = salesOrderListBO.Where(x => x.CostCenterId == row.CostCenterId).ToList();
                salesOrderList.AddRange(salesOrderListInfo);
            }

            string strTable = string.Empty;
            int counter = 0;

            if (salesOrderList.Count > 0)
            {
                strTable += "<table id='SalesOrder' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th style='display:none'></th><th align='left' scope='col' style='width: 15%;'>SO Number</th><th align='left' scope='col' style='width: 50%;'>Company Name</th><th align='left' scope='col' style='width: 20%;'>Depo Name</th><th align='left' scope='col' style='width: 15%;'>Option</th>";
                strTable += "<tbody>";

                foreach (SMSalesOrderBO bo in salesOrderList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:White;'>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }

                    strTable += "<td align='left' style=\"display:none;\">" + bo.SOrderId + "</td>";
                    //strTable += "<td align='left' style='width: 5%;'> <input type='checkbox'> </td>";
                    strTable += "<td align='left' style='width: 15%;'>" + bo.SONumber + "</td>";
                    strTable += "<td align='left' style='width: 50%;'>" + bo.CompanyName + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + bo.CostCenter + "</td>";
                    //strTable += "<td align='left' style='width: 20%;'>"+"" + "</td>";
                    strTable += "<td align='left' style='width: 15%; cursor:pointer;'><img src='../Images/ReportDocument.png' onClick='javascript:return GenerateInvoice(" + bo.SOrderId + ", " + bo.CompanyId + ")' alt='Invoice' border='0' />&nbsp;&nbsp;<img src='../Images/select.png' onClick='javascript:return BillingProcess2FromSalesOrder(" + bo.SOrderId + ")' alt='Billing' border='0' /></td>";

                    //strTable = "<img src='../Images/ReportDocument.png' onClick='javascript:return GenerateInvoice(" + bo.SOrderId + ", "+ bo.CompanyId +")' alt='Invoice' border='0' />";
                    //strTable += "<td align='left' style='width: 40%;'> <select class='form-control' style='width:100%;'> <option value='Full'>Full</option><option value='Partial'>Partial</option></select> </td>";

                    strTable += "</tr>";
                }

                strTable += "</tbody></table></div>";
                //strTable += "<input type='button' id='btnUpdateSalesOrder' value='Update' class='TransactionalButton btn btn-primary btn-sm' onclick='javascript: return UpdateSalesdOrder();'/>";
            }
            else
            {
                strTable = "No Sales Order is pending fot this Company.";
            }

            return strTable;
        }
        [WebMethod]
        public static ReturnInfo UpdateSalesdOrder(List<SMSalesOrderBO> salesOrderList)
        {
            HMUtility hmUtility = new HMUtility();
            Boolean status = false;
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                PMPurchaseOrderDA purchaseOrderDA = new PMPurchaseOrderDA();
                if (salesOrderList.Count > 0)
                {
                    status = purchaseOrderDA.UpdateSalesdOrder(salesOrderList);
                }

                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch
            {
            }

            return rtninf;
        }
        [WebMethod]
        public static List<PMPurchaseOrderDetailsBO> PerformLoadPMProductDetailOnDisplayMode(string pOrderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderDetailsBO> orderDetailListBO = new List<PMPurchaseOrderDetailsBO>();
            orderDetailListBO = orderDetailDA.GetSMSalesOrderDetailByOrderId(Int32.Parse(pOrderId));
            return orderDetailListBO;
        }
    }
}