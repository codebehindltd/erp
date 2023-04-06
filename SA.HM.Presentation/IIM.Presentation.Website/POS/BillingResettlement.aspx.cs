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
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class BillingResettlement : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        public static object RestaurantKotBillMaster { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadBillingType();
                LoadIsBillingTypeEnable();
                getPOSRefundConfiguration();
                getIsMembershipPaymentEnable();
                GetBillingConfiguration();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithGuestCompany", "IsRestaurantIntegrateWithGuestCompany");
                if (invoiceTemplateBO.SetupId > 0) { hfIsRestaurantIntegrateWithCompany.Value = invoiceTemplateBO.SetupValue; }

                HMCommonSetupBO setUpBO = new HMCommonSetupBO();
                setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsItemAutoSave", "IsItemAutoSave");
                if (setUpBO.SetupId > 0) { hfIsItemAutoSave.Value = setUpBO.SetupValue; }

                setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsTaskAutoGenarate", "IsTaskAutoGenarate");
                if (setUpBO.SetupId > 0) { hfIsTaskAutoGenarate.Value = setUpBO.SetupValue; }

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

        private void LoadBillingType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BillingType", hmUtility.GetDropDownFirstValue());

            ddlBillingType.DataSource = fields;
            ddlBillingType.DataTextField = "FieldValue";
            ddlBillingType.DataValueField = "FieldValue";
            ddlBillingType.DataBind();
        }
        private void LoadIsBillingTypeEnable()
        {
            BillingTypeUpperDividerDiv.Visible = false;
            BillingTypeDiv.Visible = false;
            BillingTypeBottomDividerDiv.Visible = false;
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBillingTypeEnable", "IsBillingTypeEnable");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBillingTypeEnable.Value = setUpBO.SetupValue;
                if (setUpBO.SetupValue == "1")
                {
                    BillingTypeUpperDividerDiv.Visible = true;
                    BillingTypeDiv.Visible = true;
                    BillingTypeBottomDividerDiv.Visible = true;
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

        private void GetBillingConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsItemCodeHideForBilling", "IsItemCodeHideForBilling");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemCodeHideForBilling.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsStockHideForBilling", "IsStockHideForBilling");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsStockHideForBilling.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsStockByHideForBilling", "IsStockByHideForBilling");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsStockByHideForBilling.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRemarksHideForBilling", "IsRemarksHideForBilling");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRemarksHideForBilling.Value = setUpBO.SetupValue;
            }


            //
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCashPaymentShow", "IsCashPaymentShow");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCashPaymentShow.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAmexCardPaymentShow", "IsAmexCardPaymentShow");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsAmexCardPaymentShow.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsMasterCardPaymentShow", "IsMasterCardPaymentShow");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsMasterCardPaymentShow.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsVisaCardPaymentShow", "IsVisaCardPaymentShow");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsVisaCardPaymentShow.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDiscoverCardPaymentShow", "IsDiscoverCardPaymentShow");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDiscoverCardPaymentShow.Value = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCompanyPaymentShow", "IsCompanyPaymentShow");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCompanyPaymentShow.Value = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSubjectShow", "IsSubjectShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSubjectShow.Value = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRemarkShow", "IsRemarkShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRemarkShow.Value = setUpBO.SetupValue;
            }

            CustomFieldBO fieldBO = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();

            fieldBO = commonDA.GetCustomFieldByFieldName("RemarksDetailsForBilling");
            if (fieldBO.FieldId != 0 && setUpBO.SetupValue == "1")
            {
                hfIsRemarkHasDefaultValue.Value = "1";
                txtRemarks.Text = fieldBO.FieldValue;
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
                costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByType("Billing");

                if (costCentreTabBOList.Count > 0)
                {
                    var vc = costCentreTabBOList.Where(c => c.CostCenterType == "Billing").ToList();

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
                hfIsRiceMillBillingEnable.Value = "0";
                if (costCentreTabBO.CompanyType == "RiceMill")
                {
                    hfIsAttributeItem.Value = "0";
                    hfIsItemAttributeEnable.Value = "0";
                    hfIsRiceMillBillingEnable.Value = "1";
                }

                hfBillPrefixCostcentrwise.Value = costCentreTabBO.BillNumberPrefix;
                if (costCentreTabBO.IsVatEnable == true)
                {
                    hfIsVatEnable.Value = "1";
                    cbTPVatAmount.Checked = true;
                    hfRestaurantVatAmount.Value = costCentreTabBO.VatAmount.ToString();
                    txtRemarks.Text = txtRemarks.Text.Replace("@CompanyName", costCentreTabBO.CostCenter);
                }
                else
                {
                    hfIsVatEnable.Value = "0";
                    cbTPVatAmount.Checked = false;
                    hfRestaurantVatAmount.Value = "0";
                }

                //if (costCentreTabBO.IsVatSChargeInclusive == 1)
                //    hfIsRestaurantBillInclusive.Value = "1";
                //else
                //    hfIsRestaurantBillInclusive.Value = "0";

                if (costCentreTabBO.IsVatSChargeInclusive == 0)
                {
                    hfIsRestaurantBillInclusive.Value = "0";
                    ddlInclusiveOrExclusive.SelectedValue = "Exclusive";
                }
                else if (costCentreTabBO.IsVatSChargeInclusive == 1)
                {
                    hfIsRestaurantBillInclusive.Value = "1";
                    ddlInclusiveOrExclusive.SelectedValue = "Inclusive";
                }
                else
                {
                    hfIsRestaurantBillInclusive.Value = "1";
                    ddlInclusiveOrExclusive.SelectedValue = "Inclusive";
                }

                HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
                isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");

                hfIsRestaurantBillAmountWillRound.Value = isRestaurantBillAmountWillRoundBO.SetupValue.ToString();

                if (costCentreTabBO.GLCompanyId > 0)
                {
                    GLProjectDA projectDA = new GLProjectDA();
                    List<GLProjectBO> projectListBO = new List<GLProjectBO>();
                    projectListBO = projectDA.GetProjectByCompanyId(costCentreTabBO.GLCompanyId);

                    if (projectListBO != null)
                    {
                        if (projectListBO.Count == 1)
                        {
                            ddlProject.DataSource = projectListBO;
                            ddlProject.DataTextField = "Name";
                            ddlProject.DataValueField = "ProjectId";
                            ddlProject.DataBind();
                        }
                        else
                        {
                            ddlProject.DataSource = projectListBO;
                            ddlProject.DataTextField = "Name";
                            ddlProject.DataValueField = "ProjectId";
                            ddlProject.DataBind();

                            System.Web.UI.WebControls.ListItem FirstItem = new System.Web.UI.WebControls.ListItem();
                            FirstItem.Value = "0";
                            FirstItem.Text = hmUtility.GetDropDownFirstValue();
                            ddlProject.Items.Insert(0, FirstItem);
                        }
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Cost Center Is Not Setup Properly for Company & Project. Please Setup Cost Center and Try Again.", AlertType.Error);
                    return;
                }
            }
        }

        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            string reportName = "rptRetailPosBill";
            hfBillIdControl.Value = "1";

            string queryStringId = hfBillId.Value;
            int billID = Int32.Parse(queryStringId);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                rvTransactionShow.LocalReport.DataSources.Clear();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                string companyName = string.Empty;
                string companyAddress = string.Empty;
                string binNumber = string.Empty;
                string tinNumber = string.Empty;
                string projectName = string.Empty;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                if (files[0].CompanyId > 0)
                {
                    companyName = files[0].CompanyName;
                    companyAddress = files[0].CompanyAddress;
                    binNumber = files[0].VatRegistrationNo;
                    tinNumber = files[0].TinNumber;

                    reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
                    reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                }

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                costCentreTabBO = costCentreTabDA.GetCostCenterDetailInformation("Restaurant", billID);
                int billTempleteId = 1;

                if (costCentreTabBO != null)
                {
                    if (costCentreTabBO.InvoiceTemplate > 0)
                    {
                        if (costCentreTabBO.InvoiceTemplate == 5)
                        {
                            reportName = "rptRestaurentBillForA4Page";
                            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBillingInvoiceTemplateWithoutHeader", "IsBillingInvoiceTemplateWithoutHeader");

                            if (commonSetupBO != null)
                            {
                                if (commonSetupBO.SetupValue != "0")
                                {
                                    reportName = "rptRestaurentBillForA4PageWithoutHeader";
                                }
                            }
                        }
                        companyName = costCentreTabBO.CostCenter;
                        if (!string.IsNullOrWhiteSpace(costCentreTabBO.CompanyAddress))
                        {
                            companyAddress = costCentreTabBO.CompanyAddress;
                        }
                    }
                    billTempleteId = costCentreTabBO.InvoiceTemplate;
                }
                hfBillTemplate.Value = billTempleteId.ToString();

                RestaurantBillBO billBO = new RestaurantBillBO();
                RestaurentBillDA billDA = new RestaurentBillDA();
                billBO = billDA.GetBillInfoByBillId(billID);

                string billRemarks = string.Empty;
                string billDeclaration = string.Empty;
                string imagePreparedBySignature = string.Empty;

                if (billBO != null)
                {
                    if (billBO.BillId > 0)
                    {
                        billRemarks = billBO.Remarks;
                        binNumber = billBO.BinNumber;
                        tinNumber = billBO.TinNumber;
                        projectName = billBO.ProjectName;
                        billDeclaration = billBO.BillDeclaration;
                        imagePreparedBySignature = billBO.UserSignature;
                        if (billBO.IsInvoiceVatAmountEnable == false)
                        {
                            if (reportName == "rptRestaurentBillForA4Page")
                            {
                                reportName = "rptRestaurentBillForA402Page";
                            }
                            else
                            {
                                reportName = "rptRestaurentBillForA402PageWithoutHeader";
                            }
                        }
                    }
                }

                var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransactionShow.LocalReport.ReportPath = reportPath;
                reportParam.Add(new ReportParameter("CompanyProfile", companyName));
                reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
                reportParam.Add(new ReportParameter("VatRegistrationNo", binNumber));
                reportParam.Add(new ReportParameter("TinNumber", tinNumber));
                reportParam.Add(new ReportParameter("ProjectName", projectName));
                reportParam.Add(new ReportParameter("BillRemarks", billRemarks));
                reportParam.Add(new ReportParameter("BillDeclaration", billDeclaration));

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

                //if (!string.IsNullOrWhiteSpace(imagePreparedBySignature))
                //{
                //    reportParam.Add(new ReportParameter("BillingDefaultPreparedBySignature", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/UserSignature/" + imagePreparedBySignature)));
                //}
                //else
                //{
                //    reportParam.Add(new ReportParameter("BillingDefaultPreparedBySignature", "Hide"));
                //}

                //HMCommonSetupBO setUpBOApprovedBySignature = new HMCommonSetupBO();
                //string imageApprovedBySignature = string.Empty;
                //setUpBOApprovedBySignature = commonSetupDA.GetCommonConfigurationInfo("BillingDefaultApprovedBySignature", "BillingDefaultApprovedBySignature");
                //if (!string.IsNullOrWhiteSpace(setUpBOApprovedBySignature.SetupValue))
                //{
                //    imageApprovedBySignature = setUpBOApprovedBySignature.SetupValue;
                //}

                //if (imageApprovedBySignature != "0")
                //{
                //    reportParam.Add(new ReportParameter("BillingDefaultApprovedBySignature", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/UserSignature/" + imageApprovedBySignature)));
                //}
                //else
                //{
                //    reportParam.Add(new ReportParameter("BillingDefaultApprovedBySignature", "Hide"));
                //}

                reportParam.Add(new ReportParameter("BillingDefaultPreparedBySignature", "Hide"));
                reportParam.Add(new ReportParameter("BillingDefaultApprovedBySignature", "Hide"));

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

                reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

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

        //Pos Printing
        //Dot Matrix

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
        public static RestaurantBillPaymentResume GetBillById(string kotId, string sourceName)
        {
            string kotIdList = string.Empty, tableIdList = string.Empty;
            KotBillMasterBO kotBillMaster = new KotBillMasterBO();
            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
            List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();
            List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();
            RestaurantBillBO kotBill = new RestaurantBillBO();
            List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();
            GuestExtraServiceBillApprovedBO roomWisePayment = new GuestExtraServiceBillApprovedBO();

            KotBillMasterDA kotDa = new KotBillMasterDA();
            RestaurentBillDA billDa = new RestaurentBillDA();
            KotBillDetailDA kotDetailsDA = new KotBillDetailDA();
            InvCategoryDA catDa = new InvCategoryDA();
            RestaurentPosDA posDa = new RestaurentPosDA();

            kotBillMaster = kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(Convert.ToInt32(kotId), sourceName);
            kotBill = billDa.GetRestaurantBillByKotId(kotBillMaster.KotId, kotBillMaster.SourceName);
            kotBillPayment = billDa.GetBillPaymentByBillId(kotBill.BillId, "Restaurant");
            roomWisePayment = posDa.GetRoomWiseRestaurantBillPaymentByBillIdServiceTypePaymentMode(kotBill.BillId);

            billDetailList = billDa.GetRestaurantBillDetailsByBillId(kotBill.BillId);
            classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(kotBill.BillId);

            billDetailList = billDetailList.Where(b => b.KotId != kotBillMaster.KotId).ToList();

            if (billDetailList.Count > 0)
            {
                foreach (RestaurantBillDetailBO bd in billDetailList)
                {
                    if (!string.IsNullOrEmpty(kotIdList))
                    {
                        kotIdList += "," + bd.KotId.ToString();
                        tableIdList += "," + bd.TableId.ToString();
                    }
                    else
                    {
                        kotIdList = bd.KotId.ToString();
                        tableIdList = bd.TableId.ToString();
                    }
                }
            }
            
            if (!string.IsNullOrEmpty(kotIdList))
            {
                kotIdList += "," + kotBillMaster.KotId.ToString();
            }
            else
            {
                kotIdList = kotBillMaster.KotId.ToString();
            }

            kotDetails = kotDetailsDA.GetRestaurantOrderItemByMultipleKotId(kotBillMaster.CostCenterId.ToString(), kotIdList, kotBillMaster.SourceName);

            RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
            paymentResume.KotBillMaster = kotBillMaster;
            paymentResume.KotBillDetails = kotDetails;
            paymentResume.RestaurantKotBill = kotBill;
            paymentResume.RestaurantKotBillPayment = kotBillPayment;
            paymentResume.RoomWiseBillPayment = roomWisePayment;
            return paymentResume;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch(string itemCode, string itemName, string categoryName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            CostCentreTabDA costCenterDa = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            InvItemDA itemDa = new InvItemDA();

            costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("Billing");

            itemInfo = itemDa.GetItemByCodeCategoryNameWiseItemDetailsForAutoSearchForBilling(itemCode, itemName, categoryName, costCenterId);

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
        public static ReturnInfo FullBillRefundSettlement(int memberId, RestaurantBillBO RestaurantBill, GuestBillPaymentBO BillPayment)
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
                                                List<KotBillDetailBO> DeletedDetails, List<RestaurantSalesReturnItemBO> SalesReturnItem, string EstimatedDoneDate, int IsTaskAutoGenarate,
                                                List<PMProductOutSerialInfoBO> AddedSerialzableProduct, List<PMProductOutSerialInfoBO> DeletedSerialzableProduct)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            KotBillMasterBO billmaster = new KotBillMasterBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            CostCentreTabDA costCenterDa = new CostCentreTabDA();
            RestaurentPosDA posda = new RestaurentPosDA();
            HMUtility hmUtility = new HMUtility();
            InvItemBO productBO = new InvItemBO();
            InvItemDA productDA = new InvItemDA();

            // // Serial Product Related Code
            string serialId = string.Empty, message = string.Empty;
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            foreach (PMProductOutSerialInfoBO srl in AddedSerialzableProduct.Where(s => s.OutSerialId == 0))
            {
                if (serialId != string.Empty)
                {
                    serialId += "," + srl.SerialNumber;
                }
                else
                {
                    serialId = srl.SerialNumber;
                }
            }

            PMProductOutDA outDa = new PMProductOutDA();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(RestaurantBill.CostCenterId);
            if (costCentreTabBO.CostCenterId > 0)
            {
                if (!string.IsNullOrEmpty(serialId))
                    serial = outDa.SerialAvailabilityCheck(serialId, Convert.ToInt64(costCentreTabBO.DefaultStockLocationId));

                foreach (SerialDuplicateBO p in serial)
                {
                    if (message != "")
                        message = ", " + p.ItemName + "(" + p.SerialNumber + ")";
                    else
                        message = p.ItemName + "(" + p.SerialNumber + ")";
                }

                if (!string.IsNullOrEmpty(message))
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("This Item Serial Does Not Exists. " + message, AlertType.Error);
                    return rtninfo;
                }

            }
            // // End Serial Product Related Code

            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            InvItemDA itemDa = new InvItemDA();

            costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("Billing");
            var costCenterId = RestaurantBill.CostCenterId;
            itemInfo = itemDa.GetItemByCodeCategoryNameWiseItemDetailsForAutoSearchForBilling("", "", "", costCenterId);

            List<InvItemCostCenterMappingBO> costListKitchenItem = new List<InvItemCostCenterMappingBO>();
            InvItemCostCenterMappingDA costKitchenItemDA = new InvItemCostCenterMappingDA();

            if (itemInfo.Count > 0)
            {
                productBO = productDA.GetInvItemInfoById(0, itemInfo[0].ItemId);
                costListKitchenItem = costKitchenItemDA.GetInvItemCostCenterMappingByItemId(itemInfo[0].ItemId);
            }

            for (int i = 0; i < BillDetails.Count; i++)
            {
                if (BillDetails[i].ItemId == 0)
                {
                    productBO.ItemId = 0;
                    productBO.Code = "";
                    productBO.Name = BillDetails[i].ItemName;
                    productBO.DisplayName = BillDetails[i].ItemName;
                    productBO.ItemType = BillDetails[i].ItemType;
                    int tmpProductId = 0;

                    Boolean status = productDA.SaveInvItemInfo(productBO, null, costListKitchenItem, null, out tmpProductId);
                    if (status)
                    {
                        BillDetails[i].ItemId = tmpProductId;
                    }
                }
            }

            int billId = 0;

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                billmaster.KotId = kotId;
                RestaurantBill.BearerId = userInformationBO.UserInfoId;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;
                billmaster.CostCenterId = costCenterId;

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
                    posda.SaveRestaurantBillForBilling("Billing", billmaster, BillDetails, AddedSerialzableProduct, DeletedSerialzableProduct, RestaurantBill, BillPayment, SalesReturnItem, null, true, true, out billId, memberId);
                }
                else if (kotId > 0 && RestaurantBill.BillId == 0)
                {
                    billmaster.IsBillHoldup = false;
                    billmaster.IsBillProcessed = true;
                    billmaster.KotStatus = "settled";
                    RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                    RestaurantBill.BillPaidBySourceId = kotId;
                    RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                    posda.UpdateRestaurantBillForPos("Billing", kotId, billmaster, BillDetails, EditeDetails, DeletedDetails, RestaurantBill, BillPayment, null, true, true, out billId);
                }
                else if (kotId > 0 && RestaurantBill.BillId > 0)
                {
                    billId = RestaurantBill.BillId;
                    RestaurantBill.BillPaidBySourceId = kotId;
                    posda.UpdateForRestauranBillReSettlement("Billing", kotId, RestaurantBill, BillDetails, EditeDetails, DeletedDetails, BillPayment);
                }

                CommonDA commonDA = new CommonDA();
                bool autoProcessStatus = commonDA.AutoCompanyBillGenerationProcess("Restaurant", billId, userInformationBO.UserInfoId);

                if (IsTaskAutoGenarate > 0)
                {
                    RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
                    RestaurentPosDA rda = new RestaurentPosDA();
                    restaurantBill = rda.RetailPosBill(billId);

                    AssignTaskDA taskDA = new AssignTaskDA();
                    SMTask task = new SMTask();
                    bool status = false;
                    long id;
                    task.Id = 0;
                    task.TaskName = "Auto New Task (" + restaurantBill.PosBillWithSalesReturn[0].BillNumber + ")";
                    task.TaskDate = DateTime.Now;
                    task.StartTime = DateTime.Now;
                    task.TaskType = "Billing";
                    task.TaskStage = 0;
                    task.ParentTaskId = 0;
                    task.DependentTaskId = 0;
                    task.SourceNameId = billId;
                    task.EstimatedDoneDate = Convert.ToDateTime(EstimatedDoneDate);
                    task.EstimatedDoneHour = 0;
                    task.EndTime = Convert.ToDateTime(EstimatedDoneDate);
                    task.CreatedBy = userInformationBO.UserInfoId;
                    status = taskDA.SaveOrUpdateTask(task, "", out id);
                }

                posda.SaveMembershipPointDetails(RestaurantBill, memberId, billId);
                if (IsTaskAutoGenarate > 0)
                {
                    rtninfo.Pk = billId;
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillSettlement + " AND " + AlertMessage.TaskCreate, AlertType.Success);
                }
                else
                {
                    rtninfo.Pk = billId;
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillSettlement, AlertType.Success);
                }

                posda.BillingAccountsVoucherPostingProcess(billId);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }

        //[WebMethod]
        //public static ReturnInfo BillSettlement(int kotId, int memberId, RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> BillPayment,
        //                                        List<KotBillDetailBO> BillDetails, List<KotBillDetailBO> EditeDetails,
        //                                        List<KotBillDetailBO> DeletedDetails, List<RestaurantSalesReturnItemBO> SalesReturnItem, string EstimatedDoneDate, int IsTaskAutoGenarate)
        //{
        //    ReturnInfo rtninf = new ReturnInfo();
        //    KotBillMasterBO billmaster = new KotBillMasterBO();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    CostCentreTabDA costCenterDa = new CostCentreTabDA();
        //    RestaurentPosDA posda = new RestaurentPosDA();
        //    HMUtility hmUtility = new HMUtility();
        //    InvItemBO productBO = new InvItemBO();
        //    InvItemDA productDA = new InvItemDA();

        //    // productBO = productDA.GetInvItemInfoById(0, 1);

        //    //

        //    List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
        //    // CostCentreTabDA costCenterDa = new CostCentreTabDA();
        //    List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
        //    InvItemDA itemDa = new InvItemDA();

        //    costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("Billing");

        //    var costCenterId = RestaurantBill.CostCenterId;

        //    itemInfo = itemDa.GetItemByCodeCategoryNameWiseItemDetailsForAutoSearchForBilling("", "", "", costCenterId);

        //    List<InvItemCostCenterMappingBO> costListKitchenItem = new List<InvItemCostCenterMappingBO>();
        //    InvItemCostCenterMappingDA costKitchenItemDA = new InvItemCostCenterMappingDA();


        //    //

        //    if (itemInfo.Count > 0)
        //    {
        //        productBO = productDA.GetInvItemInfoById(0, itemInfo[0].ItemId);
        //        costListKitchenItem = costKitchenItemDA.GetInvItemCostCenterMappingByItemId(itemInfo[0].ItemId);
        //    }



        //    for (int i = 0; i < BillDetails.Count; i++)
        //    {
        //        if (BillDetails[i].ItemId == 0)
        //        {
        //            productBO.ItemId = 0;
        //            productBO.Code = "";
        //            productBO.Name = BillDetails[i].ItemName;
        //            productBO.DisplayName = BillDetails[i].ItemName;
        //            productBO.ItemType = BillDetails[i].ItemType;
        //            int tmpProductId = 0;

        //            Boolean status = productDA.SaveInvItemInfo(productBO, null, costListKitchenItem, null, out tmpProductId);
        //            if (status)
        //            {
        //                BillDetails[i].ItemId = tmpProductId;
        //            }
        //        }

        //    }


        //    //List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

        //    int billId = 0;

        //    try
        //    {
        //        // costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("Billing");

        //        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
        //        billmaster.KotId = kotId;

        //        RestaurantBill.BearerId = userInformationBO.UserInfoId;
        //        RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
        //        RestaurantBill.BillDate = DateTime.Now;
        //        RestaurantBill.BillPaymentDate = DateTime.Now;

        //        billmaster.CostCenterId = costCenterId;

        //        if (RestaurantBill.IsBillReSettlement)
        //        {
        //            billmaster.ReferenceKotId = kotId;
        //            billmaster.IsKotReturn = true;
        //            kotId = 0;
        //        }

        //        RestaurantBill.CostCenterId = billmaster.CostCenterId;

        //        RestaurantBillBO billBO = new RestaurantBillBO();
        //        billBO = posda.GetBillInfoForRetailPosByBillId(RestaurantBill.BillId);
        //        if (billBO.BillId > 0)
        //        {
        //            RestaurantBill.BillDate = billBO.BillDate;
        //        }


        //        if (kotId == 0)
        //        {
        //            billmaster.SourceId = 1;
        //            billmaster.PaxQuantity = 1;
        //            billmaster.SourceName = "RestaurantToken";
        //            billmaster.BearerId = userInformationBO.UserInfoId;
        //            billmaster.KotStatus = ConstantHelper.KotStatus.settled.ToString();
        //            billmaster.CreatedBy = userInformationBO.UserInfoId;
        //            billmaster.IsBillHoldup = false;

        //            posda.SaveRestaurantBillForPos("Billing", billmaster, BillDetails, RestaurantBill, BillPayment, SalesReturnItem, null, true, true, out billId, memberId);
        //        }
        //        else if (kotId > 0 && RestaurantBill.BillId == 0)
        //        {
        //            billmaster.IsBillHoldup = false;
        //            billmaster.IsBillProcessed = true;
        //            billmaster.KotStatus = "settled";

        //            RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
        //            RestaurantBill.BillPaidBySourceId = kotId;
        //            RestaurantBill.CreatedBy = userInformationBO.UserInfoId;

        //            posda.UpdateRestaurantBillForPos("Billing", kotId, billmaster, BillDetails, EditeDetails, DeletedDetails, RestaurantBill, BillPayment, null, true, true, out billId);
        //        }
        //        else if (kotId > 0 && RestaurantBill.BillId > 0)
        //        {
        //            billId = RestaurantBill.BillId;
        //            RestaurantBill.BillPaidBySourceId = kotId;

        //            posda.UpdateForRestauranBillReSettlement("Billing", kotId, RestaurantBill, BillDetails, EditeDetails, DeletedDetails, BillPayment);
        //        }

        //        CommonDA commonDA = new CommonDA();
        //        bool autoProcessStatus = commonDA.AutoCompanyBillGenerationProcess("Restaurant", billId, userInformationBO.UserInfoId);

        //        if (IsTaskAutoGenarate > 0)
        //        {

        //            RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
        //            RestaurentPosDA rda = new RestaurentPosDA();
        //            restaurantBill = rda.RetailPosBill(billId);

        //            AssignTaskDA taskDA = new AssignTaskDA();
        //            SMTask task = new SMTask();
        //            bool status = false;
        //            long id;
        //            task.Id = 0;
        //            task.TaskName = "Auto New Task (" + restaurantBill.PosBillWithSalesReturn[0].BillNumber + ")";
        //            task.TaskDate = DateTime.Now;
        //            task.StartTime = DateTime.Now;
        //            task.TaskType = "Billing";
        //            task.TaskStage = 0;
        //            task.ParentTaskId = 0;
        //            task.DependentTaskId = 0;
        //            task.SourceNameId = billId;
        //            task.EstimatedDoneDate = Convert.ToDateTime(EstimatedDoneDate);
        //            task.EstimatedDoneHour = 0;
        //            task.EndTime = Convert.ToDateTime(EstimatedDoneDate);

        //            task.CreatedBy = userInformationBO.UserInfoId;
        //            status = taskDA.SaveOrUpdateTask(task, "", out id);

        //        }

        //        posda.SaveMembershipPointDetails(RestaurantBill, memberId, billId);
        //        if (IsTaskAutoGenarate > 0 && RestaurantBill.BillId == 0)
        //        {
        //            rtninf.Pk = billId;
        //            rtninf.IsSuccess = true;
        //            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillSettlement + " AND " + AlertMessage.TaskCreate, AlertType.Success);
        //        }
        //        else if (IsTaskAutoGenarate > 0 && RestaurantBill.BillId > 0)
        //        {
        //            rtninf.Pk = billId;
        //            rtninf.IsSuccess = true;
        //            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillReSettlement + " AND " + AlertMessage.TaskCreate, AlertType.Success);
        //        }
        //        if (RestaurantBill.BillId > 0)
        //        {
        //            rtninf.Pk = billId;
        //            rtninf.IsSuccess = true;
        //            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillReSettlement, AlertType.Success);
        //        }
        //        else
        //        {
        //            rtninf.Pk = billId;
        //            rtninf.IsSuccess = true;
        //            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillSettlement, AlertType.Success);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        rtninf.IsSuccess = false;
        //        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
        //    }

        //    return rtninf;
        //}
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
                costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("Billing");
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

            costCentreTabBOList = costCenterDa.GetCostCentreTabInfoByType("Billing");

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
                kotBillPayment = billDa.GetBillPaymentByBillId(kotBill.BillId, "Billing");
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

        protected void btnPrintReportTemplate1_Click1(object sender, EventArgs e)
        {
            hfBillIdControl.Value = "";
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rvTransactionShow.LocalReport.Render("PDF", null, out mimeType,
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

            frmPrint.Attributes["src"] = "../PrintDocument/" + fileNamePrint;

            //writer.Close();
            //document.Close();
            //reader.Close();
            //bytes = null;
            //cb = null;


        }

        [WebMethod]
        public static List<BankBO> GetBankInfoForAutoComplete(string bankName)
        {
            BankDA bpDA = new BankDA();
            return bpDA.GetBankInfoForAutoComplete(bankName);
        }
        [WebMethod]
        public static List<ContactInformationBO> GetContactInfoForAutoComplete(string contactName, int companyId)
        {
            ContactInformationDA DA = new ContactInformationDA();
            return DA.GetContactInfoForAutoComplete(contactName, companyId, "Billing");
        }

        protected void btnPrintReportTemplate2_Click1(object sender, EventArgs e)
        {
            hfBillIdControl.Value = "";
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

            byte[] bytes = rvTransactionShow.LocalReport.Render("PDF", deviceInfo, out mimeType,
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

            frmPrint.Attributes["src"] = "../PrintDocument/" + fileNamePrint;

            //writer.Close();
            //document.Close();
            //reader.Close();
            //bytes = null;
            //cb = null;

        }
    }
}