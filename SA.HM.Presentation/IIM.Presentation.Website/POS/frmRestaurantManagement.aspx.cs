using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
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
using System.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmRestaurantManagement : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        string irQueryString = string.Empty;
        public string innBoardDateFormat = "";
        InvCategoryDA moLocation = new InvCategoryDA();
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.LoadLocalCurrencyId();
                if (HttpContext.Current.Session["KotHoldupBill"] != null)
                {
                    PaymentResume();
                    hfIsResumeBill.Value = "1";
                }
                else
                {
                    LoadFirstTime();
                }

                GetTopLevelLocations(null);

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
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 2)
                        {
                            hfBillTemplate.Value = "2";
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 3)
                        {
                            hfBillTemplate.Value = "3";
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 4)
                        {
                            hfBillTemplate.Value = "4";
                        }
                    }
                }
            }

            decimal restaurantServiceCharge = 0;
            decimal restaurantVatAmount = 0;
            if (Session["IRCostCenterIdSession"] != null)
            {
                int currentCostCenterId = !string.IsNullOrWhiteSpace(Session["IRCostCenterIdSession"].ToString()) ? Convert.ToInt32(Session["IRCostCenterIdSession"].ToString()) : 0;
                if (Session["IRtxtKotIdInformation"] != null)
                {
                    int currentTableId = !string.IsNullOrWhiteSpace(Session["IRtxtKotIdInformation"].ToString()) ? Convert.ToInt32(Session["IRtxtKotIdInformation"].ToString()) : 0;
                }

                if (Session["IRCostCenterServiceChargeSession"] != null && Session["IRCostCenterVatAmountSession"] != null)
                {
                    hfRestaurantServiceCharge.Value = !string.IsNullOrWhiteSpace(Session["IRCostCenterServiceChargeSession"].ToString()) ? Session["IRCostCenterServiceChargeSession"].ToString() : "0";
                    hfRestaurantVatAmount.Value = !string.IsNullOrWhiteSpace(Session["IRCostCenterVatAmountSession"].ToString()) ? Session["IRCostCenterVatAmountSession"].ToString() : "0";

                    restaurantServiceCharge = !string.IsNullOrWhiteSpace(hfRestaurantServiceCharge.Value) ? Convert.ToDecimal(hfRestaurantServiceCharge.Value) : 0;
                    restaurantVatAmount = !string.IsNullOrWhiteSpace(hfRestaurantVatAmount.Value) ? Convert.ToDecimal(hfRestaurantVatAmount.Value) : 0;
                }

                IsRestaurantBillAmountWillRound();
                LoadRackRateServiceChargeVatPanelInformation();
                LoadCommonSetupForRackRateServiceChargeVatInformation();
                LoadCostCenterWiseConfiguration(restaurantServiceCharge, restaurantVatAmount);
            }

            if (!string.IsNullOrWhiteSpace(hfBillIdForInvoicePreview.Value))
            {
                btnPaymentInfo.Visible = true;
            }
        }
        protected void tvLocations_SelectedNodeChanged(object sender, EventArgs e)
        {
            string query = string.Empty;
            string valuePath = string.Empty, nodePathDepth = string.Empty;

            nodePathDepth = tvLocations.SelectedNode.ValuePath;

            if (tvLocations.SelectedNode.ChildNodes.Count > 0)
            {
                query = "RestaurantItemCategory:" + tvLocations.SelectedNode.Value.ToString();
            }
            else if (tvLocations.SelectedNode.ChildNodes.Count == 0)
            {
                query = "RestaurantItem:" + tvLocations.SelectedNode.Value.ToString();
            }

            tvLocations.CollapseAll();
            LoadCategoryNodeWise(query);

            string[] nodeDepth = nodePathDepth.Split('/');

            foreach (string node in nodeDepth)
            {
                if (!string.IsNullOrEmpty(valuePath))
                { valuePath += "/" + node; }
                else
                { valuePath = node; }

                tvLocations.FindNode(valuePath).Select();
                tvLocations.SelectedNode.ExpandAll();
            }
        }
        protected void btnLoadItemCategory_Click(object sender, EventArgs e)
        {
            string itemLoadType = string.Empty, valuePath = string.Empty, nodePathDepth = string.Empty;
            tvLocations.CollapseAll();

            itemLoadType = hfValuePath.Value;

            string[] criteria = itemLoadType.Split(',');

            LoadCategoryNodeWise(criteria[0]);
            nodePathDepth = criteria[1];

            nodePathDepth = nodePathDepth.Remove(0, 1);
            nodePathDepth = nodePathDepth.Remove(nodePathDepth.LastIndexOf('.'), 1);
            nodePathDepth = nodePathDepth.Replace('.', '/');

            string[] nodeDepth = nodePathDepth.Split('/');

            foreach (string node in nodeDepth)
            {
                if (!string.IsNullOrEmpty(valuePath))
                { valuePath += "/" + node; }
                else
                { valuePath = node; }

                tvLocations.FindNode(valuePath).Select();
                tvLocations.SelectedNode.ExpandAll();
            }

            //valuePath = "29/31/34";
            //tvLocations.FindNode("29").Select();
            //tvLocations.SelectedNode.Expand();

            //tvLocations.FindNode(valuePath).Select();
            //tvLocations.SelectedNode.Expand();
        }
        protected void tvLocations_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            e.Node.Expand();

            //foreach (TreeNode treenode in tvLocations.Nodes)
            //{
            //    if (treenode != e.Node)
            //    {
            //        treenode.Collapse();
            //    }
            //    //else if (treenode == e.Node)
            //    //{
            //    //    if(e.Node.pa)
            //    //    e.Node.Parent.Expand();
            //    //}               
            //}
        }
        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            string reportName = "rptRestaurentBillForPos";

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

                //rrp.PrintForPos();

                //string url = "/Restaurant/Reports/frmReportTPRestaurantBillInfo.aspx?billID=" + this.hfBillId.Value;
                //string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes'); window.onunload = CloseWindow();";
                ////Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
                //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
        }
        protected void btnBillNPrintPreview_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string discountType = string.Empty, costCenterId = string.Empty, discountAnount = string.Empty, categoryIdList = string.Empty;

            costCenterId = hfCostCenterId.Value;

            if (rbTPFixedDiscount.Checked)
            {
                discountType = "Fixed";
            }
            else if (rbTPPercentageDiscount.Checked)
            {
                discountType = " Percentage";
            }

            discountAnount = txtTPDiscountAmount.Text == "" ? "0" : txtTPDiscountAmount.Text.Trim();

            string queryStringId = hfBillId.Value;
            int billID = Int32.Parse(queryStringId);

            RestaurentBillDA billDA = new RestaurentBillDA();
            bool rtnValue = billDA.DistributionRestaurantBill(billID, categoryIdList, discountType, Convert.ToDecimal(discountAnount), Convert.ToInt32(costCenterId));

            if (!rtnValue)
            {
                return;
            }

            string reportName = "rptRestaurentBillForPos";

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                rvTransactionShow.LocalReport.DataSources.Clear();

                var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransactionShow.LocalReport.ReportPath = reportPath;

                RestaurantBillBO billBO = new RestaurantBillBO();

                //billBO = billDA.GetBillInfoByBillId(billID);

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
                      " });" + "$('.ui-dialog-titlebar-close').css({ " +
                       " 'top': '27%', " +
                       " 'width': '40', " +
                       " 'height': '40', " +
                       " 'background-repeat': 'no-repeat', " +
                       " 'background-position': 'center center' " +
                       " }); " +
                       " $('.ui-dialog-titlebar').css('padding','0.8em 1em'); " +
                       " setTimeout(function () { ScrollToDown(); }, 1000); ";

                ClientScript.RegisterStartupScript(this.GetType(), "script", url, true);

                return;

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
        //************************ User Defined Function ********************//
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            //LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void PaymentResume()
        {
            KotBillMasterDA kotDa = new KotBillMasterDA();
            RestaurentBillDA billDa = new RestaurentBillDA();

            KotBillMasterBO kotBillMaster = new KotBillMasterBO();
            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();

            RestaurantBillBO kotBill = new RestaurantBillBO();
            List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();

            kotBillMaster = (KotBillMasterBO)Session["KotHoldupBill"];
            kotDetails = kotDa.GetKotBillDetailInfoByKotId(kotBillMaster.KotId);

            kotBill = billDa.GetRestaurantBillByKotId(kotBillMaster.KotId, kotBillMaster.SourceName);
            kotBillPayment = billDa.GetBillPaymentByBillId(kotBill.BillId, "Restaurant");

            RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume
            {
                KotBillMaster = kotBillMaster,
                KotBillDetails = kotDetails,
                RestaurantKotBill = kotBill,
                RestaurantKotBillPayment = kotBillPayment
            };

            hfBillIdForInvoicePreview.Value = kotBill.BillId.ToString();

            Session["RestaurantKotBillResume"] = paymentResume;

            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(kotBillMaster.CostCenterId);

            hfCostCenterId.Value = kotBillMaster.CostCenterId.ToString();
            Session.Add("IRCostCenterIdSession", kotBillMaster.CostCenterId.ToString());

            Session.Add("IRCostCenterServiceChargeSession", costCentreTabBO.ServiceCharge);
            Session.Add("IRCostCenterVatAmountSession", costCentreTabBO.VatAmount);

            txtKotIdInformation.Text = kotBillMaster.KotId.ToString();
            txtTableNumberInformation.Text = kotBillMaster.TokenNumber;
            lblTokenNumber.Text = kotBillMaster.TokenNumber;
            txtTableIdInformation.Text = kotBillMaster.SourceId.ToString();
            txtBearerIdInformation.Text = kotBillMaster.BearerId.ToString();

            Session["IRtxtTableIdInformation"] = kotBillMaster.KotId.ToString();
            Session["IRtxtTableNumberInformation"] = txtTableNumberInformation.Text;
            Session["IRtxtKotIdInformation"] = kotBillMaster.KotId.ToString();
            Session["IRToeknNumber"] = kotBillMaster.TokenNumber;

            LoadCategoryWhenResumeBill(0);
            ResumeItemInformation(paymentResume);
            ResumePayemtInformation(paymentResume);
        }
        private void LoadCostCenterWiseConfiguration(decimal restaurantServiceCharge, decimal restaurantVatAmount)
        {
            if (Session["IRCostCenterIdSession"] != null)
            {
                if (restaurantServiceCharge == 0)
                {
                    pnlServiceChargeInformation.Visible = false;
                    hfltlTableWiseItemInformationDivHeight.Value = "350";
                }
                else if (restaurantVatAmount == 0)
                {
                    pnlVatInformation.Visible = false;
                    hfltlTableWiseItemInformationDivHeight.Value = "350";
                }
                else if (restaurantServiceCharge == 0 && restaurantVatAmount == 0)
                {
                    hfltlTableWiseItemInformationDivHeight.Value = "360";
                    pnlServiceChargeInformation.Visible = false;
                    pnlVatInformation.Visible = false;
                }
                else if (restaurantServiceCharge > 0 && restaurantVatAmount > 0)
                {
                    hfltlTableWiseItemInformationDivHeight.Value = "305";
                }
                else
                {
                    hfltlTableWiseItemInformationDivHeight.Value = "315";
                }
            }
        }
        private void LoadCategoryWhenResumeBill(int categoryId)
        {
            LoadRestaurantItemCategory(categoryId);
        }
        private void ResumeItemInformation(RestaurantBillPaymentResume paymentResume)
        {
            string strTable = string.Empty;
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(paymentResume.KotBillMaster.CostCenterId);

            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.CostCenterId > 0)
                {
                    string costCenterDefaultView = string.Empty;

                    if (costCentreTabBO.DefaultView == "Token")
                    {
                        costCenterDefaultView = "RestaurantToken";
                    }
                    else if (costCentreTabBO.DefaultView == "Table")
                    {
                        costCenterDefaultView = "RestaurantTable";
                    }
                    else if (costCentreTabBO.DefaultView == "Room")
                    {
                        costCenterDefaultView = "GuestRoom";
                    }

                    Boolean isChangedExist = false;
                    foreach (KotBillDetailBO drIsChanged in paymentResume.KotBillDetails)
                    {
                        if (drIsChanged.IsChanged)
                        {
                            isChangedExist = true;
                            break;
                        }
                    }

                    strTable = "<div id='no-more-tables'> ";
                    strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation' table-bordered table-striped table-condensed cf> <thead class='cf'> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit</th><th align='left' scope='col'>U. Rate</th><th align='left' scope='col'>Total</th><th align='center' scope='col'>Action</th></tr></thead>";
                    strTable += "<tbody>";
                    int counter = 0;

                    foreach (KotBillDetailBO dr in paymentResume.KotBillDetails)
                    {
                        counter++;
                        if (counter % 2 == 0)
                        {
                            // It's even
                            strTable += "<tr style='background-color:#E3EAEB;'> <td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }
                        else
                        {
                            // It's odd
                            strTable += "<tr style='background-color:White;'><td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }

                        strTable += "<td data-title='Unit' align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                        strTable += "<td data-title='Unit Rate' align='left' style='width: 15%;'>" + dr.UnitRate + "</td>";
                        strTable += "<td data-title='Total' align='left' style='width: 15%;'>" + (dr.ItemUnit * dr.UnitRate) + "</td>";

                        //strTable += "<td align='center' style='width: 15%;'>";
                        if (dr.KotDetailId > 0)
                        {
                            if (!dr.PrintFlag)
                            {
                                strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 0 + ")' alt='Action Decider'>Option</button></td>";
                            }
                            else
                            {
                                if (!isChangedExist)
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-success' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + ")' alt='Action Decider'>Option</button></td>";
                                }
                                else
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + ")' alt='Action Decider'>Option</button></td>";
                                }
                            }

                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/actiondecider.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Action Decider' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/edit.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Edit Information' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/delete.png' onClick='javascript:return PerformDeleteAction(" + dr.KotDetailId + ")' alt='Delete Information' border='0' /></td>";
                        }

                        strTable += "</tr>";

                        if (dr.ItemType == "BuffetItem")
                        {
                            string strBuffetDetail = string.Empty;
                            List<RestaurantBuffetDetailBO> buffetDetailListBO = new List<RestaurantBuffetDetailBO>();
                            RestaurantBuffetDetailDA buffetDetailDA = new RestaurantBuffetDetailDA();

                            //buffetDetailListBO = buffetDetailDA.GetRestaurantBuffetDetailByBuffetId(dr.ItemId);
                            //foreach (RestaurantBuffetDetailBO drDetail in buffetDetailListBO)
                            //{
                            //    //int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                            //    strBuffetDetail += ", " + drDetail.ItemName;
                            //}
                            //strBuffetDetail = strBuffetDetail.Substring(2, strBuffetDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strBuffetDetail + "</td>";
                        }

                        if (dr.ItemType == "ComboItem")
                        {
                            string strComboDetail = string.Empty;
                            List<InvItemDetailsBO> ownerDetailListBO = new List<InvItemDetailsBO>();
                            InvItemDetailsDA ownerDetailDA = new InvItemDetailsDA();

                            ownerDetailListBO = ownerDetailDA.GetInvItemDetailsByItemId(dr.ItemId);
                            foreach (InvItemDetailsBO drDetail in ownerDetailListBO)
                            {
                                int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                                strComboDetail += ", " + drDetail.ItemName + "(" + tmpItemUnit + ")";
                            }
                            strComboDetail = strComboDetail.Substring(2, strComboDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strComboDetail + "</td>";
                        }
                    }
                    strTable += "</tbody> </table> </div>";
                    if (strTable == "")
                    {
                        strTable = "<tr><td data-title='Item Name' colspan='4' align='center'>No Record Available!</td></tr>";
                    }
                }
            }
        }
        private void ResumePayemtInformation(RestaurantBillPaymentResume paymentResume)
        {
            decimal totalPayment = 0;

            txtSalesAmount.Text = paymentResume.RestaurantKotBill.SalesAmount.ToString();
            txtDiscountedAmount.Text = (paymentResume.RestaurantKotBill.SalesAmount - paymentResume.RestaurantKotBill.DiscountAmount).ToString();

            txtServiceCharge.Text = paymentResume.RestaurantKotBill.ServiceCharge.ToString();
            txtVatAmount.Text = paymentResume.RestaurantKotBill.VatAmount.ToString();

            if (paymentResume.RestaurantKotBill.DiscountType == HMConstants.DiscountType.Fixed.ToString())
            {
                rbTPFixedDiscount.Checked = true;
            }
            else if (paymentResume.RestaurantKotBill.DiscountType == HMConstants.DiscountType.Fixed.ToString())
            {
                rbTPPercentageDiscount.Checked = true;
            }

            txtTPDiscountAmount.Text = paymentResume.RestaurantKotBill.DiscountAmount.ToString();
            txtTPDiscountedAmount.Text = (paymentResume.RestaurantKotBill.SalesAmount - paymentResume.RestaurantKotBill.DiscountAmount).ToString();

            txtTPServiceCharge.Text = paymentResume.RestaurantKotBill.ServiceCharge.ToString();
            txtTPVatAmount.Text = paymentResume.RestaurantKotBill.VatAmount.ToString();

            foreach (GuestBillPaymentBO rb in paymentResume.RestaurantKotBillPayment)
            {
                if (rb.PaymentMode == HMConstants.PaymentMode.Cash.ToString())
                {
                    txtCash.Text = rb.PaymentAmount.ToString();
                    totalPayment += rb.PaymentAmount;
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Card.ToString())
                {
                    totalPayment += rb.PaymentAmount;

                    if (rb.CardType == Convert.ToChar(HMConstants.CardType.AmexCard).ToString())
                    {
                        txtAmexCard.Text = rb.PaymentAmount.ToString();
                    }
                    else if (rb.CardType == Convert.ToChar(HMConstants.CardType.MasterCard).ToString())
                    {
                        txtMasterCard.Text = rb.PaymentAmount.ToString();
                    }
                    else if (rb.CardType == Convert.ToChar(HMConstants.CardType.VisaCard).ToString())
                    {
                        txtVisaCard.Text = rb.PaymentAmount.ToString();
                    }
                    else if (rb.CardType == Convert.ToChar(HMConstants.CardType.DiscoverCard).ToString())
                    {
                        txtDiscoverCard.Text = rb.PaymentAmount.ToString();
                    }
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Rounded.ToString())
                {
                    txtTPRoundedAmount.Text = rb.PaymentAmount.ToString();
                    txtRoundedAmount.Text = rb.PaymentAmount.ToString();
                    txtTPRoundedAmountHiddenField.Value = rb.PaymentAmount.ToString();
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Refund.ToString())
                {
                    txtChangeAmount.Text = rb.PaymentAmount.ToString();
                    lblTPChangeAmount.Text = "Change Amount";
                }
            }

            txtTotalPaymentAmount.Text = totalPayment.ToString();

            if ((paymentResume.RestaurantKotBill.GrandTotal - totalPayment) > 0)
            {
                txtTPChangeAmount.Text = (paymentResume.RestaurantKotBill.GrandTotal - totalPayment).ToString();
            }
        }
        private void LoadPosTerminalInfo()
        {
            string fullContent = string.Empty;

            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            foreach (BankBO brow in entityBOList)
            {
                string Content = string.Empty;
                Content = @"<asp:RadioButton ID='rbTPFixedDiscount' runat='server' onclick='javascript: return ToggleFieldVisibleForAllActiveReservation(this);'
                                GroupName='DiscountType' />
                            <asp:Label ID='Label12' runat='server' Text='Fixed'></asp:Label>";
                fullContent += Content;
            }

            //                for (int iItemCategory = 0; iItemCategory < roomNumberListBO.Count; iItemCategory++)
            //                {
            //                    string Content1 = string.Empty;
            //                    if (roomNumberListBO[iItemCategory].ChildCount.ToString() == "1")
            //                    {
            //                        Content1 = @"<div class='DivRestaurantItemContainer'>
            //                                        <a href='/Restaurant/frmRestaurantManagement.aspx?IR=RestaurantItem:" + roomNumberListBO[iItemCategory].CategoryId;
            //                    }
            //                    else
            //                    {
            //                        Content1 = @"<div class='DivRestaurantItemContainer'>
            //                                        <a href='/Restaurant/frmRestaurantManagement.aspx?IR=RestaurantItemCategory:" + roomNumberListBO[iItemCategory].CategoryId;
            //                    }

            //                    string Content2 = @"'><div class='RestaurantItemDiv'><img ID='ContentPlaceHolder1_img" + roomNumberListBO[iItemCategory].CategoryId;
            //                    string Content3 = @"' class='RestaurantItemImage' src='" + roomNumberListBO[iItemCategory].ImageName;

            //                    string Content4 = @"' /></div></a>
            //                                        <div class='ItemNameDiv'>" + roomNumberListBO[iItemCategory].Name + "</div></div>";

            //                    subContent += Content1 + Content2 + Content3 + Content4;
            //                }

            ltlPOSTerminalInformation.Text = fullContent;


        }
        private void CheckingRestaurantBill(int costCenterId, int tableId)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();
            RestaurentBillDA billDA = new RestaurentBillDA();
            string sourceName = string.Empty;

            //if (Request.QueryString["tokenId"] != null)
            //{
            sourceName = "RestaurantToken";
            //}
            //if (Request.QueryString["tableId"] != null)
            //{
            //    sourceName = "RestaurantTable";
            //}
            //if (Request.QueryString["RoomNumber"] != null)
            //{
            //    sourceName = "GuestRoom";
            //}


            billBO = billDA.GetLastRestaurantBillInfoByCostCenterIdNTable(sourceName, costCenterId, tableId);
            if (billBO != null)
            {
                if (billBO.BillId > 0)
                {
                    hfBillIdForInvoicePreview.Value = billBO.BillId.ToString();
                    //btnSave.Text = "Invoice Preview";
                    //GuestPaymentInformationDiv.Visible = true;
                    //ddlDiscountType.SelectedValue = billBO.DiscountType;

                    //if (billBO.DiscountType == "Fixed")
                    //{
                    //    //restaurentBillBO.DiscountTransactionId = 0;
                    //}
                    //else if (billBO.DiscountType == "Percentage")
                    //{
                    //    //restaurentBillBO.DiscountTransactionId = 0;
                    //}
                    //else if (billBO.DiscountType == "Member")
                    //{
                    //    isBillProcessedForMember = 1;
                    //    ddlMemberId.SelectedValue = billBO.DiscountTransactionId.ToString();

                    //    BusinessPromotionDA commonDA = new BusinessPromotionDA();
                    //    BusinessPromotionBO businessPromotionBO = new BusinessPromotionBO();
                    //    businessPromotionBO = commonDA.LoadDiscountRelatedInformation("Member", Convert.ToInt32(ddlMemberId.SelectedValue));
                    //    if (businessPromotionBO != null)
                    //    {
                    //        if (businessPromotionBO.BusinessPromotionId > 0)
                    //        {
                    //            hfTxtMemberCode.Value = businessPromotionBO.BPHead;
                    //        }
                    //    }
                    //}
                    //else if (billBO.DiscountType == "BusinessPromotion")
                    //{
                    //    ddlBusinessPromotionId.SelectedValue = billBO.DiscountTransactionId.ToString();
                    //}

                    //txtDiscountAmount.Text = billBO.DiscountAmount.ToString();
                    //txtCustomerName.Text = billBO.CustomerName;
                    //cbIsComplementary.Checked = billBO.IsComplementary;
                }
                else
                {
                    //txtDiscountAmount.Text = "0";
                    // btnSave.Text = "Invoice Generate";
                    //GuestPaymentInformationDiv.Visible = false;
                }

                //List<RestaurantBillBO> classificationDiscountBOList = new List<RestaurantBillBO>();
                //classificationDiscountBOList = billDA.GetRestaurantBillClassificationDiscountInfo(billBO.BillId);

                //if (classificationDiscountBOList != null)
                //{
                //    Session["CompareCategoryWisePercentageDiscountInfo"] = classificationDiscountBOList;

                //    if (!string.IsNullOrWhiteSpace(hftxtKotNumber.Value))
                //    {
                //        InvCategoryDA invCategoryDA = new InvCategoryDA();
                //        List<InvCategoryBO> invCategoryLst = new List<InvCategoryBO>();
                //        int kotId = Convert.ToInt32(hftxtKotNumber.Value);
                //        invCategoryLst = invCategoryDA.GetInvCategoryDetailsForRestaurantBill(kotId);


                //        gvPercentageDiscountCategory.DataSource = invCategoryLst;
                //        gvPercentageDiscountCategory.DataBind();

                //    }
                //}
            }
            else
            {
                //txtDiscountAmount.Text = "0";
                //btnSave.Text = "Invoice Generate";
                //GuestPaymentInformationDiv.Visible = false;
            }

            if (!string.IsNullOrWhiteSpace(hfBillIdForInvoicePreview.Value))
            {
                //btnPaymentPosting.Visible = true;
                btnPaymentInfo.Visible = true;
            }
        }
        private void GetTopLevelLocations(bool? expand)
        {
            try
            {
                TreeNode oNode = null;
                List<InvCategoryBO> dtObjects;
                string selectedVal = (tvLocations.SelectedNode != null) ? tvLocations.SelectedNode.Value : string.Empty;
                TreeNode selectedNode = null;
                tvLocations.Nodes.Clear();
                dtObjects = moLocation.GetTPInvCategoryInfoByCustomString("WHERE  ic.lvl = 0 AND ic.ActiveStat = 1 AND ISNULL(ccc.IsRestaurant, 0) <> 0 ORDER BY ic.NAME ASC");

                foreach (InvCategoryBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString())
                    {
                        Expanded = false
                    };
                    //oNode.NavigateUrl = "frmRestaurantManagement.aspx?IR=RestaurantItemCategory:" + item.CategoryId.ToString();

                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                    //oNode.NavigateUrl = "frmRestaurantManagement.aspx?IR=RestaurantItemCategory:" + item.CategoryId.ToString();
                    tvLocations.Nodes.Add(oNode);
                }

                if (selectedNode != null && expand != false)
                {
                    while (selectedNode.Parent != null)
                    {
                        selectedNode.Parent.Expanded = true;
                        selectedNode = selectedNode.Parent;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, ToString(), "GetTopLevelLocations()", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, ToString() + "GetTopLevelLocations() - Exception " + ex.Message);
                throw ex;
            }
            finally
            {
            }
        }
        private void GetChildLocations(ref TreeNode oParent, bool? expand, ref TreeNode selectedNode, string selectedVal)
        {
            try
            {
                List<InvCategoryBO> dtObjects;
                TreeNode oNode;
                int iLevel;
                iLevel = oParent.Depth + 1;
                dtObjects = moLocation.GetInvCategoryInfoByCustomString(String.Format("WHERE  lvl = {0} AND AncestorId = {1} ORDER BY NAME ASC", iLevel, oParent.Value));


                List<InvCategoryBO> invCategoryListBO = dtObjects.Where(x => x.ActiveStat == true).ToList();

                foreach (InvCategoryBO item in invCategoryListBO)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString())
                    {
                        Expanded = false
                    };
                    //oNode.NavigateUrl = "frmRestaurantManagement.aspx?IR=RestaurantItem:" + item.CategoryId.ToString();

                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    oParent.ChildNodes.Add(oNode);

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, ToString(), "Load_Locations", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, String.Format("{0}Load_Locations - Exception {1}", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                ////tvLocations.EndUnboundLoad();
                ////Cursor.Current = Cursors.Default;
            }
        }
        private void LoadFirstTime()
        {
            irQueryString = Request.QueryString["IR"];

            if (!string.IsNullOrEmpty(irQueryString))
            {
                irQueryString = irQueryString.Split(':')[0].ToString();
            }

            if (irQueryString == "TokenAllocation")
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
                {
                    hfCostCenterId.Value = Request.QueryString["CostCenterId"].ToString();
                    Session.Add("IRCostCenterIdSession", Request.QueryString["CostCenterId"]);

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    userInformationBO.WorkingCostCenterId = Convert.ToInt32(hfCostCenterId.Value);

                    int tokenCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;

                    CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                    CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                    costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(tokenCostCenterId);

                    if (costCentreTabBO != null)
                    {
                        if (costCentreTabBO.CostCenterId > 0)
                        {
                            Session.Add("IRCostCenterServiceChargeSession", costCentreTabBO.ServiceCharge);
                            Session.Add("IRCostCenterVatAmountSession", costCentreTabBO.VatAmount);
                            KotBillMasterDA kotBillMasterDA = new Data.Restaurant.KotBillMasterDA();
                            string tokenNumber1 = kotBillMasterDA.GenarateRestaurantTokenNumber(tokenCostCenterId);

                            BillCreateForToken(tokenNumber1);
                        }
                    }
                }
            }
            else if (irQueryString == "TableAllocation")
            {
                Session.Add("IRKotBillTypeInformationBOSession", "TableAllocation");

                if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
                {
                    Session.Add("IRCostCenterIdSession", Request.QueryString["CostCenterId"]);
                    LoadTableAllocation();
                }
            }
            else if (irQueryString == "RoomAllocation")
            {
                Session.Add("IRKotBillTypeInformationBOSession", "RoomAllocation");
                //Response.Redirect("/Restaurant/frmRestaurantManagement.aspx?IR=RoomAllocation");
            }
            else if (irQueryString == "BillCreateForTable")
            {
                //Session.Add("IRKotBillTypeInformationBOSession", "RoomAllocation");
                BillCreateForTable(Request.QueryString["IR"].Split(':')[1].ToString());
            }
            else if (irQueryString == "RestaurantItemCategory")
            {
                int categoryId = !string.IsNullOrWhiteSpace(Request.QueryString["IR"].Split(':')[1].ToString()) ? Convert.ToInt32(Request.QueryString["IR"].Split(':')[1].ToString()) : 0;
                LoadRestaurantItemCategory(categoryId);

                //int categoryId = !string.IsNullOrWhiteSpace(Request.QueryString["IR"].Split(':')[1].ToString()) ? Convert.ToInt32(Request.QueryString["IR"].Split(':')[1].ToString()) : 0;
                //if (categoryId > 0)
                //{
                //    LoadRestaurantItemCategory(categoryId);
                //}
                //else
                //{
                //    CommonHelper.AlertInfo(innboardMessage, Session["tbsMessage"].ToString(), AlertType.Warning);
                //}
            }
            else if (irQueryString == "RestaurantItem")
            {
                int itemId = !string.IsNullOrWhiteSpace(Request.QueryString["IR"].Split(':')[1].ToString()) ? Convert.ToInt32(Request.QueryString["IR"].Split(':')[1].ToString()) : 0;
                LoadRestaurantItem(itemId.ToString());

                //int itemId = !string.IsNullOrWhiteSpace(Request.QueryString["IR"].Split(':')[1].ToString()) ? Convert.ToInt32(Request.QueryString["IR"].Split(':')[1].ToString()) : 0;
                //if (itemId > 0)
                //{
                //    LoadRestaurantItem(itemId.ToString());
                //}
                //else
                //{
                //    CommonHelper.AlertInfo(innboardMessage, Session["tbsMessage"].ToString(), AlertType.Warning);
                //}
            }

        }
        private void LoadCategoryNodeWise(string query)
        {
            irQueryString = query;

            if (!string.IsNullOrEmpty(irQueryString))
            {
                irQueryString = irQueryString.Split(':')[0].ToString();
            }

            if (irQueryString == "TokenAllocation")
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
                {
                    hfCostCenterId.Value = Request.QueryString["CostCenterId"].ToString();
                    Session.Add("IRCostCenterIdSession", Request.QueryString["CostCenterId"]);

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    userInformationBO.WorkingCostCenterId = Convert.ToInt32(hfCostCenterId.Value);

                    int tokenCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;

                    CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                    CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                    costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(tokenCostCenterId);

                    if (costCentreTabBO != null)
                    {
                        if (costCentreTabBO.CostCenterId > 0)
                        {
                            Session.Add("IRCostCenterServiceChargeSession", costCentreTabBO.ServiceCharge);
                            Session.Add("IRCostCenterVatAmountSession", costCentreTabBO.VatAmount);
                            KotBillMasterDA kotBillMasterDA = new Data.Restaurant.KotBillMasterDA();
                            string tokenNumber = kotBillMasterDA.GenarateRestaurantTokenNumber(tokenCostCenterId);

                            BillCreateForToken(tokenNumber);
                        }
                    }
                }
            }
            else if (irQueryString == "TableAllocation")
            {
                Session.Add("IRKotBillTypeInformationBOSession", "TableAllocation");

                if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
                {
                    Session.Add("IRCostCenterIdSession", Request.QueryString["CostCenterId"]);
                    LoadTableAllocation();
                }
            }
            else if (irQueryString == "RoomAllocation")
            {
                Session.Add("IRKotBillTypeInformationBOSession", "RoomAllocation");
            }
            else if (irQueryString == "BillCreateForTable")
            {
                BillCreateForTable(query.Split(':')[1].ToString());
            }
            else if (irQueryString == "RestaurantItemCategory")
            {
                int categoryId = !string.IsNullOrWhiteSpace(query.Split(':')[1].ToString()) ? Convert.ToInt32(query.Split(':')[1].ToString()) : 0;
                LoadRestaurantItemCategory(categoryId);
            }
            else if (irQueryString == "RestaurantItem")
            {
                int itemId = !string.IsNullOrWhiteSpace(query.Split(':')[1].ToString()) ? Convert.ToInt32(query.Split(':')[1].ToString()) : 0;
                LoadRestaurantItem(itemId.ToString());
            }

        }
        private void LoadRackRateServiceChargeVatPanelInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInnboardVatEnableBO = new HMCommonSetupBO();
            isInnboardVatEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardVatEnable", "IsInnboardVatEnable");

            HMCommonSetupBO isInnboardServiceChargeEnableBO = new HMCommonSetupBO();
            isInnboardServiceChargeEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardServiceChargeEnable", "IsInnboardServiceChargeEnable");

            if (Convert.ToInt32(isInnboardVatEnableBO.SetupValue) + Convert.ToInt32(isInnboardServiceChargeEnableBO.SetupValue) == 0)
            {
                pnlRackRateServiceChargeVatInformation.Visible = false;
                cbServiceCharge.Checked = false;
                cbVatAmount.Checked = false;
            }
            else
            {
                cbServiceCharge.Checked = true;
                cbVatAmount.Checked = true;
                pnlRackRateServiceChargeVatInformation.Visible = true;
            }
        }
        private void LoadCommonSetupForRackRateServiceChargeVatInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupIsInclusiveBillBO = new HMCommonSetupBO();
            commonSetupIsInclusiveBillBO = commonSetupDA.GetCommonConfigurationInfo("IsInclusiveBill", "IsInclusiveBill");
            hfIsRestaurantBillInclusive.Value = commonSetupIsInclusiveBillBO.SetupValue;

            HMCommonSetupBO commonSetupIsInnboardVatEnableBO = new HMCommonSetupBO();
            commonSetupIsInnboardVatEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardVatEnable", "IsInnboardVatEnable");
            hfIsVatServiceChargeEnable.Value = commonSetupIsInnboardVatEnableBO.SetupValue;
            int isVatServiceChargeEnable = !string.IsNullOrWhiteSpace(hfIsVatServiceChargeEnable.Value) ? Convert.ToInt32(hfIsVatServiceChargeEnable.Value) : 0;

            int isRestaurantBillInclusive = !string.IsNullOrWhiteSpace(hfIsRestaurantBillInclusive.Value) ? Convert.ToInt32(hfIsRestaurantBillInclusive.Value) : 0;
            if (isRestaurantBillInclusive == 1)
            {
                if (isVatServiceChargeEnable == 0)
                {
                    NetAmountDivInfo.Visible = false;
                    lblSalesAmount.Text = "Grand Total";
                    lblGrandTotal.Text = "Grand Total";
                    //lblSalesAmount.Text = "Sales Amount";
                    //lblGrandTotal.Text = "Grand Total";
                }
                else
                {
                    NetAmountDivInfo.Visible = false;
                    lblSalesAmount.Text = "Grand Total";
                    lblGrandTotal.Text = "Grand Total";
                    //lblSalesAmount.Text = "Net Amount";
                    //lblGrandTotal.Text = "Sales Amount";
                }
            }
            else
            {
                NetAmountDivInfo.Visible = false;
                lblGrandTotal.Text = "Grand Total";
                lblSalesAmount.Text = "Grand Total";
                //lblGrandTotal.Text = "Grand Total";
                //lblSalesAmount.Text = "Sales Amount";
            }

            //commonSetupBO = new HMCommonSetupBO();
            //commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantVat", "RestaurantVat");
            //hfRestaurantVatAmount.Value = commonSetupBO.SetupValue;

            //commonSetupBO = new HMCommonSetupBO();
            //commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantServiceCharge", "RestaurantServiceCharge");
            //hfRestaurantServiceCharge.Value = commonSetupBO.SetupValue;
        }
        private void IsRestaurantBillAmountWillRound()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");

            hfIsRestaurantBillAmountWillRound.Value = "1";
            if (isRestaurantBillAmountWillRoundBO != null)
            {
                hfIsRestaurantBillAmountWillRound.Value = isRestaurantBillAmountWillRoundBO.SetupValue;
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
        #region Print Option
        public void PrintRestaurantBill(int billID)
        {
            PrintInfos pinf = new PrintInfos();

            HMUtility hmUtility = new HMUtility();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillInvoiceTemplate", "RestaurantBillInvoiceTemplate");


            PrinterInfoDA da = new PrinterInfoDA();
            RestaurentBillDA rda = new RestaurentBillDA();
            KotBillDetailDA billDA = new KotBillDetailDA();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();

            List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            restaurantBill = rda.GetRestaurantBillReport(billID);

            if (restaurantBill != null)
            {
                if (restaurantBill.Count > 0)
                {
                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantOrderSubmitDisable", "IsRestaurantOrderSubmitDisable");

                    if (commonSetupBO != null)
                    {
                        if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                        {
                            if (commonSetupBO.SetupValue == "0")
                            {
                                pinf.IsRestaurantOrderSubmitDisable = false;
                            }
                            else
                            {
                                pinf.IsRestaurantOrderSubmitDisable = true;
                            }
                        }
                    }

                    HMCommonSetupBO IsRestaurantOrderSubmitDisableBO = new HMCommonSetupBO();
                    IsRestaurantOrderSubmitDisableBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantTokenInfoDisable", "IsRestaurantTokenInfoDisable");

                    if (IsRestaurantOrderSubmitDisableBO != null)
                    {
                        if (!string.IsNullOrEmpty(IsRestaurantOrderSubmitDisableBO.SetupValue))
                        {
                            if (IsRestaurantOrderSubmitDisableBO.SetupValue == "0")
                            {
                                pinf.IsRestaurantTokenInfoDisable = false;
                            }
                            else
                            {
                                pinf.IsRestaurantTokenInfoDisable = true;
                            }
                        }
                    }

                    pinf.TableNumberInformation = restaurantBill[0].TableNumber;
                    pinf.KotIdInformation = Convert.ToInt32(restaurantBill[0].KotId);

                    //parms[3] = new ReportParameter("TableNo", tableNumberInformation);
                    //parms[4] = new ReportParameter("KotNo", kotIdInformation.ToString());

                    List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotId(Convert.ToInt32(restaurantBill[0].KotId));
                    List<PrinterInfoBO> invoicePrinter = da.GetPrinterInfoByCostCenterPrintType(restaurantBill[0].CostCenterId, "InvoiceItem");

                    //if (files.Count() == 0)
                    //{
                    //    files.Add(new PrinterInfoBO() { PrinterName = "Microsoft XPS Document Writer", StockType = "StockItem" });
                    //    files.Add(new PrinterInfoBO() { PrinterName = "Microsoft XPS Document Writer", StockType = "KitchenItem" });
                    //}

                    if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == (int)(HMConstants.PrintTemplate.Template1))
                    {
                        if (invoicePrinter.Count > 0)
                            PrintReportRestaurantBill(invoicePrinter[0], restaurantBill, pinf);

                        if (restaurantBill[0].SourceName == "Table Number")
                        {
                            return;
                        }

                        foreach (PrinterInfoBO pinfo in files)
                        {
                            pinf.CostCenterId = pinfo.CostCenterId;
                            pinf.PrinterInfoId = pinfo.PrinterInfoId;
                            pinf.CostCenterId = pinfo.CostCenterId;
                            pinf.CostCenterName = pinfo.CostCenter;
                            pinf.CompanyName = pinfo.KitchenOrStockName;

                            if (pinfo.DefaultView == "Table")
                            {
                                pinf.CostCenterDefaultView = "Table # ";
                            }
                            else if (pinfo.DefaultView == "Token")
                            {
                                pinf.CostCenterDefaultView = "Token # ";
                            }
                            else if (pinfo.DefaultView == "Room")
                            {
                                pinf.CostCenterDefaultView = "Room # ";
                            }

                            UserInformationBO userInformationBO = new UserInformationBO();
                            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                            pinf.WaiterName = userInformationBO.UserName; //userInformationBO.EmployeeName.ToString();

                            if (pinfo.StockType == "StockItem")
                            {
                                entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "StockItem", true);
                            }
                            else
                            {
                                entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "KitchenItem", true).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                            }

                            if (entityBOList.Count > 0)
                            {
                                PrintReportKot(pinfo, entityBOList, pinf, false);
                            }
                        }
                    }
                    else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == (int)(HMConstants.PrintTemplate.Template4))
                    {
                        entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "AllItem", false);

                        List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                        List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                        List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                        //PrintReportKot(invoicePrinter[0], restaurantBill, kotOrderSubmitEntityBOList, changedOrEditedEntityBOList, voidOrDeletedItemEntityBOList, entityBOList[0].Remarks, false);
                    }
                }
            }
        }
        public void PrintReportRestaurantBill(PrinterInfoBO files, List<RestaurantBillReportBO> restaurantBill, PrintInfos prntInf)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport
                {
                    ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurentBillForPos.rdlc"),
                    EnableExternalImages = true,
                    EnableHyperlinks = true
                };

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);

                int kotId = Convert.ToInt32(restaurantBill[0].KotId);

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> filesCom = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (filesCom[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", filesCom[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", filesCom[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", filesCom[0].VatRegistrationNo));

                    if (!string.IsNullOrWhiteSpace(filesCom[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", filesCom[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", filesCom[0].ContactNumber));
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                //reportParam.Add(new ReportParameter("Path", HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(restaurantBill[0].CostCenterId);
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
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

                if (prntInf.IsRestaurantOrderSubmitDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                }

                if (prntInf.IsRestaurantTokenInfoDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                }

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

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                report.SetParameters(reportParam);

                var dataSet = report.GetDataSourceNames();

                report.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PrintReportKot(PrinterInfoBO files, List<KotBillDetailBO> entityBOList, PrintInfos prntInf, Boolean IsReprint)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport
                {
                    ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptKotBill.rdlc"),
                    EnableExternalImages = true,
                    EnableHyperlinks = true
                };

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);


                int kotId = 0;
                int paxQuantity = 1;
                foreach (KotBillDetailBO row in entityBOList)
                {
                    kotId = row.KotId;
                    if (row.PaxQuantity != 0)
                    {
                        paxQuantity = row.PaxQuantity;
                    }
                    if (prntInf.KotIdInformation == 0)
                    {
                        prntInf.KotIdInformation = kotId;
                    }
                }

                string reportTitle = string.Empty;
                reportTitle = "KOT";
                if (kotId > 0)
                {
                    int updatedDataCount = 0;
                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    updatedDataCount = entityDA.GetKotUpdatedDataCountInfo(kotId);
                    if (updatedDataCount > 0)
                    {
                        //reportTitle = "KOT (Updated)";
                        reportTitle = "KOT";
                    }
                    else
                    {
                        if (IsReprint)
                        {
                            reportTitle = "KOT (Reprint)";
                        }
                        else
                        {
                            reportTitle = "KOT";
                        }
                    }
                }

                ReportParameter[] parms = new ReportParameter[9];
                parms[0] = new ReportParameter("ReportTitle", reportTitle);
                parms[1] = new ReportParameter("CostCenter", prntInf.CostCenterName);
                parms[2] = new ReportParameter("SourceName", prntInf.CostCenterDefaultView);
                parms[3] = new ReportParameter("TableNo", prntInf.TableNumberInformation);
                parms[4] = new ReportParameter("KotNo", prntInf.KotIdInformation.ToString() + "   Pax : " + paxQuantity.ToString());
                parms[5] = new ReportParameter("KotDate", kotDate);
                parms[6] = new ReportParameter("WaiterName", prntInf.WaiterName);
                parms[7] = new ReportParameter("SpecialRemarks", entityBOList[0].Remarks);
                parms[8] = new ReportParameter("RestaurantName", prntInf.CompanyName);

                report.SetParameters(parms);

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                report.DataSources.Add(new ReportDataSource("KotOrderSubmit", kotOrderSubmitEntityBOList));
                report.DataSources.Add(new ReportDataSource("ChangedOrEditedItem", changedOrEditedEntityBOList));
                report.DataSources.Add(new ReportDataSource("VoidOrDeletedItem", voidOrDeletedItemEntityBOList));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void PrintReportKot(PrinterInfoBO files, PrintInfos prntInf, List<RestaurantBillReportBO> restaurantBill, List<KotBillDetailBO> kotOrderSubmitEntityBOList, List<KotBillDetailBO> changedOrEditedEntityBOList, List<KotBillDetailBO> voidOrDeletedItemEntityBOList, string specialRemarks, Boolean IsReprint)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport
                {
                    ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurentBillForPosToken.rdlc"),
                    EnableExternalImages = true,
                    EnableHyperlinks = true
                };

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);

                int paxQuantity = 1;
                int kotId = Convert.ToInt32(restaurantBill[0].KotId);

                string reportTitle = string.Empty;
                reportTitle = "KOT";
                if (kotId > 0)
                {
                    int updatedDataCount = 0;
                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    updatedDataCount = entityDA.GetKotUpdatedDataCountInfo(kotId);
                    if (updatedDataCount > 0)
                    {
                        //reportTitle = "KOT (Updated)";
                        reportTitle = "KOT";
                    }
                    else
                    {
                        if (IsReprint)
                        {
                            reportTitle = "KOT (Reprint)";
                        }
                        else
                        {
                            reportTitle = "KOT";
                        }
                    }
                }

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> filesCom = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (filesCom[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", filesCom[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", filesCom[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", filesCom[0].VatRegistrationNo));

                    if (!string.IsNullOrWhiteSpace(filesCom[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", filesCom[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", filesCom[0].ContactNumber));
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                //reportParam.Add(new ReportParameter("Path", HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(restaurantBill[0].CostCenterId);
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                }

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

                UserInformationBO userInformationBO = new UserInformationBO();
                HMUtility hmUtility = new HMUtility();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                prntInf.WaiterName = userInformationBO.UserName.ToString();

                prntInf.CostCenterName = filesCom[0].CompanyName;
                prntInf.CostCenterDefaultView = "Token # ";
                prntInf.CompanyName = filesCom[0].CompanyName;

                reportParam.Add(new ReportParameter("ReportTitle", "KOT"));
                reportParam.Add(new ReportParameter("CostCenter", prntInf.CostCenterName));
                reportParam.Add(new ReportParameter("SourceName", prntInf.CostCenterDefaultView));
                reportParam.Add(new ReportParameter("TableNo", prntInf.TableNumberInformation));
                reportParam.Add(new ReportParameter("KotDate", kotDate));
                reportParam.Add(new ReportParameter("WaiterName", prntInf.WaiterName));
                reportParam.Add(new ReportParameter("RestaurantName", prntInf.CompanyName));
                //reportParam.Add(new ReportParameter("KotNo", restaurantBill[0].KotId.ToString()));
                reportParam.Add(new ReportParameter("KotNo", restaurantBill[0].KotId.ToString() + "   Pax : " + paxQuantity.ToString()));
                reportParam.Add(new ReportParameter("SpecialRemarks", specialRemarks));

                if (prntInf.IsRestaurantOrderSubmitDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                }

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
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

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                report.SetParameters(reportParam);

                var dataSet = report.GetDataSourceNames();

                report.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));
                report.DataSources.Add(new ReportDataSource(dataSet[1], kotOrderSubmitEntityBOList));
                report.DataSources.Add(new ReportDataSource(dataSet[2], changedOrEditedEntityBOList));
                report.DataSources.Add(new ReportDataSource(dataSet[3], voidOrDeletedItemEntityBOList));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Allocation Information
        private void LoadTableAllocation()
        {
            if (Session["IRCostCenterIdSession"] != null)
            {
                hfCostCenterId.Value = Session["IRCostCenterIdSession"].ToString();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                userInformationBO.WorkingCostCenterId = Convert.ToInt32(hfCostCenterId.Value);

                TableManagementDA entityDA = new TableManagementDA();
                List<TableManagementBO> entityListBO = new List<TableManagementBO>();

                string fullContent = string.Empty;
                int costCenterId = Convert.ToInt32(hfCostCenterId.Value);

                entityListBO = entityDA.GetTableManagementInfo(costCenterId);
                string topPart = @"<div id='legendContainerForRestaurant'>
                                <div class='legend RoomVacantDiv'>
                                </div>
                                <div class='legendText'>
                                    Available</div>
                                <div class='legend RoomOccupaiedDiv'>
                                </div>
                                <div class='legendText'>
                                    Occupaied
                                </div>
                            </div><div class='divClear'>
                            </div>
                            <div class='block FloorRoomAllocationBGImage'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

                string topTemplatePart = @"</a>
                                <div id='FloorRoomAllocation' class='block-body collapse in'>           
                                ";

                string endTemplatePart = @"</div>
                            </div>";

                string subContent = string.Empty;

                for (int iTableNumber = 0; iTableNumber < entityListBO.Count; iTableNumber++)
                {
                    string Content0 = string.Empty;
                    string Content1 = string.Empty;
                    string Content2 = string.Empty;
                    if (entityListBO[iTableNumber].StatusId == 1)
                    {
                        Content0 = @"<a href='/Restaurant/frmRestaurantManagement.aspx?IR=BillCreateForTable:" + entityListBO[iTableNumber].TableId + "&tbs=1";
                        Content1 = @"'><div class='NotDraggable RestaurantTableAvailableDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                        Content2 = @"<div class='RestaurantTableAvailableDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";
                    }
                    if (entityListBO[iTableNumber].StatusId == 2)
                    {
                        Content0 = @"<a href='/Restaurant/frmRestaurantManagement.aspx?IR=BillCreateForTable:" + entityListBO[iTableNumber].TableId + "&tbs=2";
                        Content1 = @"'><div class='NotDraggable RestaurantTableBookedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                        Content2 = @"<div class='RestaurantTableBookedDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";
                    }
                    if (entityListBO[iTableNumber].StatusId == 3)
                    {
                        Content0 = @"<a href='/Restaurant/frmRestaurantManagement.aspx?IR=BillCreateForTable:" + entityListBO[iTableNumber].TableId + "&tbs=3";
                        Content1 = @"'><div class='NotDraggable RestaurantTableReservedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                        Content2 = @"<div class='RestaurantTableReservedDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";
                    }

                    subContent += Content0 + Content1 + Content2;
                }

                fullContent += topPart + topTemplatePart + subContent + endTemplatePart;
                literalRestaurantTemplate.Text = fullContent;
            }
        }
        #endregion
        #region Bill Create Information
        private void BillCreateForTable(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                int tableStatus = 0;
                int tableId = Convert.ToInt32(queryString);
                if (Request.QueryString["tbs"] != null)
                {
                    tableStatus = int.Parse(Request.QueryString["tbs"]);
                }
                RestaurantTableBO entityTableBO = new RestaurantTableBO();
                RestaurantTableDA entityTableDA = new RestaurantTableDA();

                int tmpPkId;
                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();
                entityBO.SourceName = "RestaurantTable";
                entityBO.SourceId = tableId;

                txtTableIdInformation.Text = tableId.ToString();
                Session["IRtxtTableIdInformation"] = tableId.ToString();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //entityBO.BearerId = userInformationBO.EmpId;
                RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();

                restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(userInformationBO.UserInfoId, 1);

                if (restaurantBearerBO != null)
                {
                    entityBO.BearerId = restaurantBearerBO.BearerId;
                    entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    entityTableBO = entityTableDA.GetRestaurantTableInfoById(userInformationBO.WorkingCostCenterId, "RestaurantTable", tableId);

                    if (tableStatus != entityTableBO.StatusId)
                    {
                        Session["tbsMessage"] = "Table status updated.";
                        Response.Redirect("/POS/frmRestaurantManagement.aspx?IR=TableAllocation");
                    }

                    if (entityTableBO.StatusId == 1)
                    {
                        txtTableNumberInformation.Text = entityTableBO.TableNumber;
                        Session["IRtxtTableNumberInformation"] = txtTableNumberInformation.Text;
                        Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                        if (status)
                        {
                            txtKotIdInformation.Text = tmpPkId.ToString();
                            //txtBearerIdInformation.Text
                            Session["IRtxtKotIdInformation"] = txtKotIdInformation.Text;
                            //Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemGroup:0");
                            Response.Redirect("/POS/frmRestaurantManagement.aspx?IR=RestaurantItemCategory:0");
                        }
                    }
                    else if (entityTableBO.StatusId == 2)
                    {
                        txtTableNumberInformation.Text = entityTableBO.TableNumber;
                        Session["IRtxtTableNumberInformation"] = txtTableNumberInformation.Text;
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(entityBO.CostCenterId, "RestaurantTable", tableId);
                        if (entityBO.KotId > 0)
                        {
                            Session["IRtxtKotIdInformation"] = entityBO.KotId;
                            //Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemGroup:1");

                            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantKotContinueWithDiferentWaiter", "IsRestaurantKotContinueWithDiferentWaiter");
                            if (commonSetupBO != null)
                            {
                                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                                {
                                    if (commonSetupBO.SetupValue == "0")
                                    {
                                        if (entityBO.CreatedBy != userInformationBO.UserInfoId)
                                        {
                                            RestaurantBearerBO tableAllocatedBy = new RestaurantBearerBO();
                                            tableAllocatedBy = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(entityBO.CreatedBy, 1);
                                            Session["IRTableAllocatedBy"] = "This Table (" + entityTableBO.TableNumber + ") Is already Allocated By " + tableAllocatedBy.UserName + ".";
                                            Response.Redirect("/POS/frmRestaurantManagement.aspx?IR=TableAllocation");
                                        }
                                        else
                                        {
                                            Response.Redirect("/POS/frmRestaurantManagement.aspx?IR=RestaurantItemCategory:0");
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("/POS/frmRestaurantManagement.aspx?IR=RestaurantItemCategory:0");
                                    }
                                }
                            }
                            else
                            {
                                Response.Redirect("/POS/frmRestaurantManagement.aspx?IR=RestaurantItemCategory:0");
                            }

                        }
                    }
                }
            }
        }
        private void BillCreateForToken(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                int tableStatus = 0;
                int tableId = Convert.ToInt32(queryString);
                if (Request.QueryString["tbs"] != null)
                {
                    tableStatus = int.Parse(Request.QueryString["tbs"]);
                }
                RestaurantTableBO entityTableBO = new RestaurantTableBO();
                RestaurantTableDA entityTableDA = new RestaurantTableDA();

                int tmpPkId;
                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();
                entityBO.SourceName = "RestaurantToken";
                entityBO.SourceId = tableId;

                txtTableIdInformation.Text = tableId.ToString();
                Session["IRtxtTableIdInformation"] = tableId.ToString();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //entityBO.BearerId = userInformationBO.UserInfoId;
                RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();

                restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(userInformationBO.UserInfoId, 1);

                if (restaurantBearerBO != null)
                {
                    entityBO.BearerId = restaurantBearerBO.BearerId;
                    entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    //entityTableBO = entityTableDA.GetRestaurantTableInfoById(userInformationBO.WorkingCostCenterId, "RestaurantToken", tableId);
                    entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;
                    entityBO.IsBillHoldup = false;

                    txtTableNumberInformation.Text = entityTableBO.TableNumber;
                    Session["IRtxtTableNumberInformation"] = txtTableNumberInformation.Text;
                    Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);

                    if (status)
                    {
                        txtBearerIdInformation.Text = userInformationBO.UserInfoId.ToString();
                        txtKotIdInformation.Text = tmpPkId.ToString();

                        string tokenNumber = entityDA.GenarateTokenNumberByKot(tmpPkId);
                        entityDA.UpdateKotBillMasterForToken(tmpPkId, tokenNumber);

                        Session["IRtxtKotIdInformation"] = txtKotIdInformation.Text;
                        Session["IRToeknNumber"] = tokenNumber;
                        lblTokenNumber.Text = tokenNumber;
                        LoadCategoryNodeWise("RestaurantItemCategory:0");

                        //Response.Redirect("/Restaurant/frmRestaurantManagement.aspx?IR=RestaurantItemCategory:0");
                    }

                    //if (tableStatus != entityTableBO.StatusId)
                    //{
                    //    Session["tbsMessage"] = "Table status updated.";
                    //    Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=TableAllocation");
                    //}

                    //if (entityTableBO.StatusId == 1)
                    //{
                    //    txtTableNumberInformation.Text = entityTableBO.TableNumber;
                    //    Session["txtTableNumberInformation"] = txtTableNumberInformation.Text;
                    //    Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                    //    if (status)
                    //    {

                    //        txtKotIdInformation.Text = tmpPkId.ToString();
                    //        Session["txtKotIdInformation"] = txtKotIdInformation.Text;
                    //        //Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemGroup:0");
                    //        Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                    //    }
                    //}
                    //else if (entityTableBO.StatusId == 2)
                    //{
                    //    txtTableNumberInformation.Text = entityTableBO.TableNumber;
                    //    Session["txtTableNumberInformation"] = txtTableNumberInformation.Text;
                    //    entityBO = entityDA.GetKotBillMasterInfoByTableId("RestaurantTable", tableId);
                    //    if (entityBO.KotId > 0)
                    //    {
                    //        Session["txtKotIdInformation"] = entityBO.KotId;
                    //        //Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemGroup:1");
                    //        Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                    //    }
                    //}
                }
            }

        }
        //private void BillCreateForRoom(string queryString)
        //{
        //    if (!string.IsNullOrWhiteSpace(queryString))
        //    {
        //        int tableStatus = 0;
        //        int tableId = Convert.ToInt32(queryString);

        //        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
        //        RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
        //        roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(tableId.ToString());

        //        if (roomAllocationBO.RoomId > 0)
        //        {
        //            if (Request.QueryString["tbs"] != null)
        //            {
        //                tableStatus = int.Parse(Request.QueryString["tbs"]);
        //            }
        //            RestaurantTableBO entityTableBO = new RestaurantTableBO();
        //            RestaurantTableDA entityTableDA = new RestaurantTableDA();

        //            int tmpPkId;
        //            KotBillMasterBO entityBO = new KotBillMasterBO();
        //            KotBillMasterDA entityDA = new KotBillMasterDA();
        //            entityBO.SourceName = "GuestRoom";
        //            entityBO.SourceId = roomAllocationBO.RegistrationId;

        //            txtTableIdInformation.Text = roomAllocationBO.RegistrationId.ToString();
        //            Session["txtTableIdInformation"] = roomAllocationBO.RegistrationId.ToString();

        //            //txtTableIdInformation.Text = tableId.ToString();
        //            //Session["txtTableIdInformation"] = tableId.ToString();

        //            EmployeeBO userInformationBO = new EmployeeBO();
        //            userInformationBO = hmUtility.GetCurrentRestaurantApplicationUserInfo();

        //            //entityBO.BearerId = userInformationBO.UserInfoId;
        //            RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
        //            RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();

        //            restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(userInformationBO.UserInfoId, 1);

        //            if (restaurantBearerBO != null)
        //            {
        //                entityBO.BearerId = restaurantBearerBO.BearerId;
        //                entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;
        //                entityBO.CreatedBy = userInformationBO.UserInfoId;


        //                entityTableBO = entityTableDA.GetRestaurantTableInfoById(userInformationBO.WorkingCostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);

        //                //if (tableStatus != entityTableBO.StatusId)
        //                //{
        //                //    Session["tbsMessage"] = "Table status updated.";
        //                //    Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RoomAllocation");
        //                //}

        //                if (entityTableBO != null)
        //                {
        //                    if (entityTableBO.TableId > 0)
        //                    {

        //                        if (entityTableBO.StatusId == 1)
        //                        {
        //                            txtTableNumberInformation.Text = entityTableBO.TableNumber;
        //                            Session["txtTableNumberInformation"] = txtTableNumberInformation.Text;
        //                            Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
        //                            if (status)
        //                            {

        //                                txtKotIdInformation.Text = tmpPkId.ToString();
        //                                Session["txtKotIdInformation"] = txtKotIdInformation.Text;
        //                                //Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemGroup:0");
        //                                Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
        //                            }
        //                        }
        //                        else if (entityTableBO.StatusId == 2)
        //                        {
        //                            txtTableNumberInformation.Text = entityTableBO.TableNumber;
        //                            Session["txtTableNumberInformation"] = txtTableNumberInformation.Text;
        //                            entityBO = entityDA.GetKotBillMasterInfoByTableId(entityBO.CostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);
        //                            if (entityBO.KotId > 0)
        //                            {
        //                                Session["txtKotIdInformation"] = entityBO.KotId;
        //                                //Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemGroup:1");
        //                                Response.Redirect("/Restaurant/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
        #region Category Item Information
        private void LoadRestaurantItemCategory(int categoryId)
        {
            if (Session["IRCostCenterIdSession"] != null)
            {
                hfCostCenterId.Value = Session["IRCostCenterIdSession"].ToString();
                InvCategoryDA roomNumberDA = new InvCategoryDA();
                List<InvCategoryBO> roomNumberListAllBO = new List<InvCategoryBO>();
                List<InvCategoryBO> roomNumberListWithZeroBO = new List<InvCategoryBO>();
                List<InvCategoryBO> roomNumberListWithoutZeroBO = new List<InvCategoryBO>();

                string fullContent = string.Empty;
                int costCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
                roomNumberListAllBO = roomNumberDA.GetInvCategoryInfoByLabel(costCenterId, categoryId);
                roomNumberListWithZeroBO = roomNumberListAllBO.Where(x => x.ChildCount == 0).ToList();
                roomNumberListWithoutZeroBO = roomNumberListAllBO.Where(x => x.ChildCount != 0).ToList();

                //------------------------------------------------Item Generate-----------------------------------------------------------------------------
                string fullItemContent = string.Empty;
                for (int iItemCategory = 0; iItemCategory < roomNumberListWithZeroBO.Count; iItemCategory++)
                {
                    if (Session["IRCostCenterIdSession"] != null)
                    {
                        if (Session["IRtxtKotIdInformation"] != null)
                        {
                            txtKotIdInformation.Text = Session["IRtxtKotIdInformation"].ToString();
                        }

                        if (Session["IRtxtBearerIdInformation"] != null)
                        {
                            txtBearerIdInformation.Text = Session["IRtxtBearerIdInformation"].ToString();
                        }

                        if (Session["IRtxtTableIdInformation"] != null)
                        {
                            txtTableIdInformation.Text = Session["IRtxtTableIdInformation"].ToString();
                        }

                        hfCostCenterId.Value = Session["IRCostCenterIdSession"].ToString();
                        int itemCategory = roomNumberListWithZeroBO[iItemCategory].CategoryId;
                        InvItemDA invItemDA = new InvItemDA();
                        List<InvItemBO> roomNumberListBO = new List<InvItemBO>();

                        string subItemContent = string.Empty;
                        Session["IRtxtCategoryInformation"] = itemCategory;
                        roomNumberListBO = invItemDA.GetInvItemInfoByCategoryId(costCenterId, itemCategory);

                        for (int iItem = 0; iItem < roomNumberListBO.Count; iItem++)
                        {
                            string Content1 = @"<div class='DivRestaurantItemContainer'>";
                            string Content2 = @"<div class='RestaurantItemDiv'><img class='ItemImageSize' ID='ContentPlaceHolder1_img" + roomNumberListBO[iItem].ItemId;
                            string Content3 = @"'  onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId;
                            string Content4 = @");'  src='" + roomNumberListBO[iItem].ImageName;
                            string Content5 = @"' /></div>
                                        <div class='ItemNameDiv'>" + roomNumberListBO[iItem].Name + "</div></div>";

                            subItemContent += Content1 + Content2 + Content3 + Content4 + Content5;
                        }

                        fullItemContent += subItemContent;
                    }
                }

                //------------------------------------------------Category Generate-----------------------------------------------------------------------------
                string topPart = @"<a href='javascript:void()' class='block-heading'><span style='color: #ffffff;'>Category Information";
                string topTemplatePart = @"</span></a> <div> <div>";
                string endTemplatePart = @"</div> </div>";

                string subContent = string.Empty;

                for (int iItemCategory = 0; iItemCategory < roomNumberListWithoutZeroBO.Count; iItemCategory++)
                {
                    string Content1 = string.Empty;
                    if (roomNumberListWithoutZeroBO[iItemCategory].ChildCount.ToString() == "1")
                    {
                        Content1 = string.Format(@"<div class='DivRestaurantItemContainer'> <a href='javascript:void()'" +
                                                  " onclick =\"LoadCategoryNItem('RestaurantItem:{0}', '{1}')\" ", roomNumberListWithoutZeroBO[iItemCategory].CategoryId, roomNumberListWithoutZeroBO[iItemCategory].Hierarchy);
                    }
                    else
                    {
                        Content1 = string.Format(@"<div class='DivRestaurantItemContainer'> <a href='javascript:void()'" +
                                                 " onclick =\"LoadCategoryNItem('RestaurantItemCategory:{0}', '{1}')\" ", roomNumberListWithoutZeroBO[iItemCategory].CategoryId, roomNumberListWithoutZeroBO[iItemCategory].Hierarchy);
                    }

                    string Content2 = @"'><div class='RestaurantItemDiv'><img ID='ContentPlaceHolder1_img" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
                    string Content3 = @"' class='RestaurantItemImage' src='" + roomNumberListWithoutZeroBO[iItemCategory].ImageName;

                    string Content4 = @"' /></div></a>
                                        <div class='ItemNameDiv'>" + roomNumberListWithoutZeroBO[iItemCategory].Name + "</div></div>";

                    subContent += Content1 + Content2 + Content3 + Content4;
                }

                fullContent += topPart + topTemplatePart + fullItemContent + subContent + endTemplatePart;
                literalRestaurantTemplate.Text = fullContent;
            }
        }
        private void LoadRestaurantItem(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                if (Session["IRCostCenterIdSession"] != null)
                {
                    if (Session["IRtxtKotIdInformation"] != null)
                    {
                        txtKotIdInformation.Text = Session["IRtxtKotIdInformation"].ToString();
                    }

                    if (Session["IRtxtBearerIdInformation"] != null)
                    {
                        txtBearerIdInformation.Text = Session["IRtxtBearerIdInformation"].ToString();
                    }

                    if (Session["IRtxtTableIdInformation"] != null)
                    {
                        txtTableIdInformation.Text = Session["IRtxtTableIdInformation"].ToString();
                    }


                    hfCostCenterId.Value = Session["IRCostCenterIdSession"].ToString();
                    int itemCategory = Convert.ToInt32(queryString);
                    InvItemDA roomNumberDA = new InvItemDA();
                    List<InvItemBO> roomNumberListBO = new List<InvItemBO>();

                    string fullContent = string.Empty;
                    int costCenterId = Convert.ToInt32(hfCostCenterId.Value);

                    Session["IRtxtCategoryInformation"] = itemCategory;
                    roomNumberListBO = roomNumberDA.GetInvItemInfoByCategoryId(costCenterId, itemCategory);

                    string topPart = @"<a href='javascript:void()' class='block-heading'><span style='color: #ffffff;'>Item Information";
                    string topTemplatePart = @"</span></a> <div> <div>";
                    string endTemplatePart = @"</div> </div>";

                    string subContent = string.Empty;

                    for (int iItem = 0; iItem < roomNumberListBO.Count; iItem++)
                    {
                        string Content1 = @"<div class='DivRestaurantItemContainer'>";
                        string Content2 = @"<div class='RestaurantItemDiv'><img class='ItemImageSize' ID='ContentPlaceHolder1_img" + roomNumberListBO[iItem].ItemId;
                        string Content3 = @"'  onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId;
                        string Content4 = @");'  src='" + roomNumberListBO[iItem].ImageName;
                        string Content5 = @"' /></div>
                                        <div class='ItemNameDiv'>" + roomNumberListBO[iItem].Name + "</div></div>";

                        subContent += Content1 + Content2 + Content3 + Content4 + Content5;
                    }

                    fullContent += topPart + topTemplatePart + subContent + endTemplatePart;
                    literalRestaurantTemplate.Text = fullContent;
                }
            }
        }
        #endregion
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static KotBillDetailBO SaveIndividualItemDetailInformation(int costCenterId, int kotId, int itemId, decimal itemQty)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            InvItemBO itemEntityBO = new InvItemBO();
            InvItemDA itemEntityDA = new InvItemDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, itemId);
            if (itemEntityBO.ItemId > 0)
            {
                entityBO = entityDA.GetKotBillDetailInfoByKotNItemId(costCenterId, kotId, itemId, "IndividualItem");

                if (entityBO.KotDetailId > 0)
                {
                    entityBO.ItemType = "IndividualItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = itemQty + 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                    entityBO.Amount = itemEntityBO.UnitPriceLocal * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.UpdateKotBillDetailInfo(0, entityBO, "QuantityChange");
                }
                else
                {
                    entityBO.ItemType = "IndividualItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                    entityBO.Amount = itemEntityBO.UnitPriceLocal * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.SaveKotBillDetailInfo(entityBO);
                }
            }
            return entityBO;
        }
        [WebMethod]
        public static GuestBillPaymentBO SavePaymentInformationInSession(string DiscountType, string DiscountAmount, string txtCash, string txtAmexCard, string txtMasterCard, string txtVisaCard, string txtDiscoverCard)
        {
            GuestBillPaymentBO entityBO = new GuestBillPaymentBO();

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["IRGuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            HttpContext.Current.Session["IRRestaurantBillDiscountType"] = DiscountType;
            HttpContext.Current.Session["IRRestaurantBillDiscountAmount"] = DiscountAmount;



            // // // ------ Cash Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtCash))
            {
                decimal cashPaymentAmount = !string.IsNullOrWhiteSpace(txtCash) ? Convert.ToDecimal(txtCash) : 0;

                if (cashPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestCashPaymentInfo = new GuestBillPaymentBO
                    {
                        NodeId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        PaymentType = "Advance",
                        AccountsPostingHeadId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        BillPaidBy = 0,
                        BankId = 0,
                        RegistrationId = 0,
                        FieldId = 45,
                        ConvertionRate = 1,
                        CurrencyAmount = cashPaymentAmount,
                        PaymentAmount = cashPaymentAmount,
                        ChecqueDate = DateTime.Now,
                        PaymentMode = "Cash",
                        PaymentId = 1,
                        CardNumber = "",
                        CardType = "",
                        ExpireDate = null,
                        ChecqueNumber = "",
                        CardHolderName = "",
                        PaymentDescription = ""
                    };

                    guestPaymentDetailListForGrid.Add(guestCashPaymentInfo);
                }
            }

            // // // ------ Amex Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtAmexCard))
            {
                decimal amexCardPaymentAmount = !string.IsNullOrWhiteSpace(txtAmexCard) ? Convert.ToDecimal(txtAmexCard) : 0;

                if (amexCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestAmexCardPaymentInfo = new GuestBillPaymentBO
                    {
                        NodeId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        PaymentType = "Advance",
                        AccountsPostingHeadId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        BillPaidBy = 0,
                        BankId = 0,
                        RegistrationId = 0,
                        FieldId = 45,
                        ConvertionRate = 1,
                        CurrencyAmount = amexCardPaymentAmount,
                        PaymentAmount = amexCardPaymentAmount,
                        ChecqueDate = DateTime.Now,
                        PaymentMode = "Card",
                        PaymentId = 1,
                        CardNumber = "",
                        CardType = "a",
                        ExpireDate = null,
                        ChecqueNumber = "",
                        CardHolderName = "",
                        PaymentDescription = ""
                    };

                    guestPaymentDetailListForGrid.Add(guestAmexCardPaymentInfo);
                }
            }


            // // // ------ Master Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtMasterCard))
            {
                decimal masterCardPaymentAmount = !string.IsNullOrWhiteSpace(txtMasterCard) ? Convert.ToDecimal(txtMasterCard) : 0;

                if (masterCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestMasterCardPaymentInfo = new GuestBillPaymentBO
                    {
                        NodeId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        PaymentType = "Advance",
                        AccountsPostingHeadId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        BillPaidBy = 0,
                        BankId = 0,
                        RegistrationId = 0,
                        FieldId = 45,
                        ConvertionRate = 1,
                        CurrencyAmount = masterCardPaymentAmount,
                        PaymentAmount = masterCardPaymentAmount,
                        ChecqueDate = DateTime.Now,
                        PaymentMode = "Card",
                        PaymentId = 1,
                        CardNumber = "",
                        CardType = "m",
                        ExpireDate = null,
                        ChecqueNumber = "",
                        CardHolderName = "",
                        PaymentDescription = ""
                    };

                    guestPaymentDetailListForGrid.Add(guestMasterCardPaymentInfo);
                }
            }


            // // // ------ Visa Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtVisaCard))
            {
                decimal visaCardPaymentAmount = !string.IsNullOrWhiteSpace(txtVisaCard) ? Convert.ToDecimal(txtVisaCard) : 0;

                if (visaCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestVisaCardPaymentInfo = new GuestBillPaymentBO
                    {
                        NodeId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        PaymentType = "Advance",
                        AccountsPostingHeadId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        BillPaidBy = 0,
                        BankId = 0,
                        RegistrationId = 0,
                        FieldId = 45,
                        ConvertionRate = 1,
                        CurrencyAmount = visaCardPaymentAmount,
                        PaymentAmount = visaCardPaymentAmount,
                        ChecqueDate = DateTime.Now,
                        PaymentMode = "Card",
                        PaymentId = 1,
                        CardNumber = "",
                        CardType = "v",
                        ExpireDate = null,
                        ChecqueNumber = "",
                        CardHolderName = "",
                        PaymentDescription = ""
                    };

                    guestPaymentDetailListForGrid.Add(guestVisaCardPaymentInfo);
                }
            }


            // // // ------ Discover Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtDiscoverCard))
            {
                decimal discoverCardPaymentAmount = !string.IsNullOrWhiteSpace(txtDiscoverCard) ? Convert.ToDecimal(txtDiscoverCard) : 0;

                if (discoverCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestDiscoverCardPaymentInfo = new GuestBillPaymentBO
                    {
                        NodeId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        PaymentType = "Advance",
                        AccountsPostingHeadId = 0, //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                        BillPaidBy = 0,
                        BankId = 0,
                        RegistrationId = 0,
                        FieldId = 45,
                        ConvertionRate = 1,
                        CurrencyAmount = discoverCardPaymentAmount,
                        PaymentAmount = discoverCardPaymentAmount,
                        ChecqueDate = DateTime.Now,
                        PaymentMode = "Card",
                        PaymentId = 1,
                        CardNumber = "",
                        CardType = "d",
                        ExpireDate = null,
                        ChecqueNumber = "",
                        CardHolderName = "",
                        PaymentDescription = ""
                    };

                    guestPaymentDetailListForGrid.Add(guestDiscoverCardPaymentInfo);
                }
            }

            HttpContext.Current.Session["IRGuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

            return entityBO;
        }
        [WebMethod]
        public static string GenerateTableWiseItemGridInformation(int costCenterId, int tableId, int kotId)
        {
            string strTable = string.Empty;
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(costCenterId);
            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.CostCenterId > 0)
                {
                    string costCenterDefaultView = string.Empty;

                    if (costCentreTabBO.DefaultView == "Token")
                    {
                        costCenterDefaultView = "RestaurantToken";
                    }
                    else if (costCentreTabBO.DefaultView == "Table")
                    {
                        costCenterDefaultView = "RestaurantTable";
                    }
                    else if (costCentreTabBO.DefaultView == "Room")
                    {
                        costCenterDefaultView = "GuestRoom";
                    }


                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    //List<KotBillDetailBO> files = entityDA.GetKotBillDetailInfoByKotNBearerId(costCenterId, "RestaurantTable", tableId, kotId);
                    List<KotBillDetailBO> files = entityDA.GetKotBillDetailInfoByKotNBearerId(costCenterId, costCenterDefaultView, tableId, kotId);

                    Boolean isChangedExist = false;
                    foreach (KotBillDetailBO drIsChanged in files)
                    {
                        if (drIsChanged.IsChanged)
                        {
                            isChangedExist = true;
                            break;
                        }
                    }

                    strTable = "<div id='no-more-tables'> ";
                    strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation' table-bordered table-striped table-condensed cf> <thead class='cf'> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit</th><th align='left' scope='col'>U. Rate</th><th align='left' scope='col'>Total</th><th align='center' scope='col'>Action</th></tr></thead>";
                    strTable += "<tbody>";
                    int counter = 0;
                    foreach (KotBillDetailBO dr in files)
                    {
                        if (counter % 2 == 0)
                        {
                            // It's even
                            strTable += "<tr style='background-color:#E3EAEB;'> <td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }
                        else
                        {
                            // It's odd
                            strTable += "<tr style='background-color:White;'><td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }

                        strTable += "<td data-title='Unit' align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                        strTable += "<td data-title='Unit Rate' align='left' style='width: 15%;'>" + dr.UnitRate + "</td>";
                        strTable += "<td data-title='Total' align='left' style='width: 15%;'>" + Math.Round((dr.ItemUnit * dr.UnitRate), 2) + "</td>";

                        //strTable += "<td align='center' style='width: 15%;'>";
                        if (dr.KotDetailId > 0)
                        {
                            if (!dr.PrintFlag)
                            {
                                strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 0 + ")' alt='Action Decider'>Option</button></td>";
                            }
                            else
                            {
                                if (!isChangedExist)
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-success' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + ")' alt='Action Decider'>Option</button></td>";
                                }
                                else
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + ")' alt='Action Decider'>Option</button></td>";
                                }
                            }

                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/actiondecider.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Action Decider' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/edit.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Edit Information' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/delete.png' onClick='javascript:return PerformDeleteAction(" + dr.KotDetailId + ")' alt='Delete Information' border='0' /></td>";
                        }

                        strTable += "<td align='left' style='display:none;'>" + dr.ItemId + "</td>";
                        strTable += "</tr>";

                        if (dr.ItemType == "BuffetItem")
                        {
                            string strBuffetDetail = string.Empty;
                            List<RestaurantBuffetDetailBO> buffetDetailListBO = new List<RestaurantBuffetDetailBO>();
                            RestaurantBuffetDetailDA buffetDetailDA = new RestaurantBuffetDetailDA();

                            //buffetDetailListBO = buffetDetailDA.GetRestaurantBuffetDetailByBuffetId(dr.ItemId);
                            //foreach (RestaurantBuffetDetailBO drDetail in buffetDetailListBO)
                            //{
                            //    //int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                            //    strBuffetDetail += ", " + drDetail.ItemName;
                            //}
                            //strBuffetDetail = strBuffetDetail.Substring(2, strBuffetDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strBuffetDetail + "</td>";
                        }

                        if (dr.ItemType == "ComboItem")
                        {
                            string strComboDetail = string.Empty;
                            List<InvItemDetailsBO> ownerDetailListBO = new List<InvItemDetailsBO>();
                            InvItemDetailsDA ownerDetailDA = new InvItemDetailsDA();

                            ownerDetailListBO = ownerDetailDA.GetInvItemDetailsByItemId(dr.ItemId);
                            foreach (InvItemDetailsBO drDetail in ownerDetailListBO)
                            {
                                int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                                strComboDetail += ", " + drDetail.ItemName + "(" + tmpItemUnit + ")";
                            }
                            strComboDetail = strComboDetail.Substring(2, strComboDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strComboDetail + "</td>";
                        }
                        counter++;
                    }
                    strTable += "</tbody> </table> </div>";
                    if (strTable == "")
                    {
                        strTable = "<tr><td data-title='Item Name' colspan='4' align='center'>No Record Available!</td></tr>";
                    }
                }
            }

            return strTable;
        }
        [WebMethod]
        public static SpecialRemarksDetailsViewBO GetSpecialRemarksDetails(int kotId, int itemId)
        {
            InvItemSpecialRemarksDA specialRemarkDa = new InvItemSpecialRemarksDA();
            List<InvItemSpecialRemarksBO> specialRemarks = new List<InvItemSpecialRemarksBO>();

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();
            List<RestaurantKotSpecialRemarksDetailBO> kotRemarks = new List<RestaurantKotSpecialRemarksDetailBO>();

            SpecialRemarksDetailsViewBO remarksDetailsView = new SpecialRemarksDetailsViewBO();

            specialRemarks = specialRemarkDa.GetActiveInvItemSpecialRemarksInfo();
            kotRemarks = kotRemarksDa.GetInvItemSpecialRemarksInfoById(kotId, itemId);

            remarksDetailsView.KotRemarks = kotRemarks;
            remarksDetailsView.ItemSpecialRemarks = specialRemarks;

            return remarksDetailsView;
        }
        [WebMethod]
        public static ReturnInfo SaveKotSpecialRemarks(List<RestaurantKotSpecialRemarksDetailBO> kotSRemarksDetail, List<RestaurantKotSpecialRemarksDetailBO> kotSRemarksDetailTobeDelete, int kotDetailId)
        {
            int tempItemId = 0;
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int waiterId = userInformationBO.UserInfoId;

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();

            if (kotSRemarksDetailTobeDelete.Count == 0)
            {
                status = kotRemarksDa.SaveRestaurantKotSpecialRemarksDetail(kotSRemarksDetail, waiterId, kotDetailId, out tempItemId);
            }
            else
            {
                status = kotRemarksDa.UpdateRestaurantKotSpecialRemarksDetail(kotSRemarksDetail, kotSRemarksDetailTobeDelete, waiterId, kotDetailId);
            }

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static KotBillDetailBO UpdateIndividualItemDetailInformation(string updateType, int costCenterId, int editId, decimal quantity, string updatedContent)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();
            KotBillDetailBO srcEntityBO = new KotBillDetailBO();

            RestaurantBuffetItemBO bItemEntityBO = new RestaurantBuffetItemBO();
            RestaurantBuffetItemDA bItemEntityDA = new RestaurantBuffetItemDA();
            RestaurantComboItemBO cItemEntityBO = new RestaurantComboItemBO();
            RestaurantComboItemDA cItemEntityDA = new RestaurantComboItemDA();
            InvItemBO itemEntityBO = new InvItemBO();
            InvItemDA itemEntityDA = new InvItemDA();

            srcEntityBO = entityDA.GetSrcRestaurantBillDetailInfoByKotDetailId(editId);
            if (srcEntityBO.KotDetailId > 0)
            {
                if (srcEntityBO.ItemType == "BuffetItem")
                {
                    bItemEntityBO = bItemEntityDA.GetRestaurantBuffetInfoById(srcEntityBO.ItemId);
                    entityBO.UnitRate = bItemEntityBO.BuffetPrice;
                    entityBO.Amount = bItemEntityBO.BuffetPrice * Convert.ToInt32(updatedContent);
                    entityBO.ItemUnit = Convert.ToInt32(updatedContent);
                    entityBO.KotId = srcEntityBO.KotId;
                    entityBO.ItemId = srcEntityBO.ItemId;
                }
                else if (srcEntityBO.ItemType == "ComboItem")
                {
                    cItemEntityBO = cItemEntityDA.GetRestaurantComboInfoById(srcEntityBO.ItemId);
                    entityBO.UnitRate = cItemEntityBO.ComboPrice;
                    entityBO.Amount = cItemEntityBO.ComboPrice * Convert.ToInt32(updatedContent);
                    entityBO.ItemUnit = Convert.ToInt32(updatedContent);
                    entityBO.KotId = srcEntityBO.KotId;
                    entityBO.ItemId = srcEntityBO.ItemId;
                }
                else if (srcEntityBO.ItemType == "IndividualItem")
                {
                    if (updateType == "QuantityChange")
                    {
                        itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, srcEntityBO.ItemId);
                        entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                        entityBO.Amount = itemEntityBO.UnitPriceLocal * Convert.ToDecimal(updatedContent);
                        entityBO.ItemUnit = Convert.ToDecimal(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                    }
                    else if (updateType == "ItemNameChange")
                    {
                        entityBO.ItemName = updatedContent;
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                    }
                    else if (updateType == "UnitPriceChange")
                    {
                        entityBO.UnitRate = Convert.ToDecimal(updatedContent);
                        entityBO.Amount = quantity * Convert.ToInt32(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                    }
                }

                entityBO.KotDetailId = editId;

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                entityBO.CreatedBy = userInformationBO.UserInfoId;

                Boolean status = entityDA.UpdateKotBillDetailInfo(0, entityBO, updateType);
            }
            return entityBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int kotDetailId, int kotId, int itemId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                KotBillDetailDA kotDa = new KotBillDetailDA();
                Boolean status = kotDa.DeleteKotBillDetail(kotDetailId, kotId, itemId, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo SaveRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool isDirectPrintNView = false;
            string categoryIdList = string.Empty;

            try
            {
                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO
                        {
                            ClassificationId = Convert.ToInt32(rowCWPD)
                        };
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);

                        categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = userInformationBO.UserInfoId;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO
                {
                    KotId = RestaurantBill.KotId
                };
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                int billId = restaurentBillDA.SaveRestaurantBill(RestaurantBill, restaurentBillDetailBOList, GuestBillPayment, categoryWisePercentageDiscountBOList, categoryIdList, true, true, out billID);

                if (billId > 0)
                {
                    //Boolean settlementStatus = restaurentBillDA.RestaurantBillSettlementInfoByBillId(billId);
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = billId;
                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    if (Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue) == (int)(HMConstants.RestaurantBillPrintAndPreview.PrintAndPreview))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);

                        rtninf.RedirectUrl = "frmCostCenterSelection.aspx";

                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                        HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                        HttpContext.Current.Session.Remove("tbsMessage");
                        HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                        HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");
                    }
                    else if (Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue) == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);

                        rtninf.RedirectUrl = "frmCostCenterSelection.aspx";

                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                        HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                        HttpContext.Current.Session.Remove("tbsMessage");
                        HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                        HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");
                    }
                    else
                    {
                        // HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        //  HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        // HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        //HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        // HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        //  HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        // HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        // HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        // HttpContext.Current.Session.Remove("IRToeknNumber");
                        // HttpContext.Current.Session.Remove("KotHoldupBill");
                        // HttpContext.Current.Session.Remove("RestaurantKotBillResume");

                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                        HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                        HttpContext.Current.Session.Remove("tbsMessage");
                        HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                        HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");


                    }
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo UpdateRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string categoryIdList = string.Empty;

            try
            {
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume = (RestaurantBillPaymentResume)HttpContext.Current.Session["RestaurantKotBillResume"];

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();

                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;

                var vcash = (from p in paymentResume.RestaurantKotBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString()
                             select p).FirstOrDefault();

                if (vcash == null)
                {
                    vcash = (from p in GuestBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString() &&
                                   p.PaymentAmount != 0
                             select p
                             ).FirstOrDefault();

                    if (vcash != null)
                    {
                        paymentAdded.Add(vcash);
                    }
                }

                var vrounded = (from p in paymentResume.RestaurantKotBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                select p).FirstOrDefault();

                if (vrounded == null)
                {
                    vrounded = (from p in GuestBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString() &&
                                     p.PaymentAmount != 0
                                select p
                             ).FirstOrDefault();

                    if (vrounded != null)
                    {
                        paymentAdded.Add(vrounded);
                    }
                }

                var vrefund = (from p in paymentResume.RestaurantKotBillPayment
                               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString()
                               select p).FirstOrDefault();

                if (vrefund == null)
                {
                    vrefund = (from p in GuestBillPayment
                               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString() &&
                                     p.PaymentAmount != 0
                               select p
                             ).FirstOrDefault();

                    if (vrefund != null)
                    {
                        paymentAdded.Add(vrefund);
                    }
                }

                var vcard = (from p in paymentResume.RestaurantKotBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Card.ToString()
                             select p).ToList();

                var vcardNew = (from p in GuestBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                      p.PaymentAmount != 0
                                select p).ToList();

                if (vcard != null)
                {
                    foreach (GuestBillPaymentBO bp in vcardNew)
                    {
                        var v1 = (from pr in vcard
                                  where pr.CardType == bp.CardType &&
                                        pr.PaymentAmount != 0
                                  select pr).FirstOrDefault();

                        if (v1 == null)
                        {
                            paymentAdded.Add(bp);
                        }
                    }
                }

                var update1 = (from bp in GuestBillPayment
                               from pr in paymentResume.RestaurantKotBillPayment
                               where bp.PaymentMode == pr.PaymentMode &&
                                     (
                                         bp.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                     ) &&
                                     bp.PaymentAmount != pr.PaymentAmount &&
                                     bp.PaymentAmount != 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   CurrencyAmount = pr.CurrencyAmount,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                var update2 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                     pr.CardType == bp.CardType &&
                                     pr.PaymentAmount != bp.PaymentAmount &&
                                     bp.PaymentAmount != 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   CurrencyAmount = pr.CurrencyAmount,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                paymentUpdate.AddRange(update1);
                paymentUpdate.AddRange(update2);

                var delete1 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == bp.PaymentMode &&
                                     (
                                         pr.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                                         pr.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                     ) &&
                                     bp.PaymentAmount == 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                var delete2 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                     pr.CardType == bp.CardType &&
                                     bp.PaymentAmount == 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                paymentDelete.AddRange(delete1);
                paymentDelete.AddRange(delete2);

                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO
                        {
                            ClassificationId = Convert.ToInt32(rowCWPD)
                        };
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);

                        categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO
                {
                    BillId = RestaurantBill.BillId,
                    KotId = RestaurantBill.KotId
                };
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                bool billId = restaurentBillDA.UpdateRestaurantBill(RestaurantBill, restaurentBillDetailBOList, paymentAdded, paymentUpdate, paymentDelete, categoryWisePercentageDiscountBOList, categoryIdList, true);

                if (billId)
                {
                    //Boolean settlementStatus = restaurentBillDA.RestaurantBillSettlementInfoByBillId(RestaurantBill.BillId);
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = RestaurantBill.BillId;
                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    if (Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue) == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(RestaurantBill.BillId);

                        rtninf.RedirectUrl = "frmCostCenterSelection.aspx";

                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    }
                    else
                    {
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    }
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo HoldUpRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                //if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                //{
                //    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                //    foreach (string rowCWPD in strCategoryIdList)
                //    {
                //        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                //        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                //        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);
                //    }
                //}

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = userInformationBO.UserInfoId;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO
                {
                    KotId = RestaurantBill.KotId
                };
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                int billId = restaurentBillDA.SaveRestaurantBillForHoldUp(RestaurantBill, restaurentBillDetailBOList, GuestBillPayment, categoryWisePercentageDiscountBOList, out billID);

                if (billId > 0)
                {
                    KotBillMasterDA kotDa = new KotBillMasterDA();
                    kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelection.aspx";
                    rtninf.IsBillHoldUp = true;
                    rtninf.Pk = billId;

                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    HttpContext.Current.Session.Remove("KotHoldupBill");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                    HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                    HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                    HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                    HttpContext.Current.Session.Remove("IRToeknNumber");
                    HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                    HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                    HttpContext.Current.Session.Remove("tbsMessage");
                    HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                    HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo UpdateHoldUpRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume = (RestaurantBillPaymentResume)HttpContext.Current.Session["RestaurantKotBillResume"];

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();

                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;

                var vcash = (from p in paymentResume.RestaurantKotBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString()
                             select p).FirstOrDefault();

                if (vcash == null)
                {
                    vcash = (from p in GuestBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString() &&
                                   p.PaymentAmount != 0
                             select p
                             ).FirstOrDefault();

                    if (vcash != null)
                    {
                        paymentAdded.Add(vcash);
                    }
                }

                var vrounded = (from p in paymentResume.RestaurantKotBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                select p).FirstOrDefault();

                if (vrounded == null)
                {
                    vrounded = (from p in GuestBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString() &&
                                     p.PaymentAmount != 0
                                select p
                             ).FirstOrDefault();

                    if (vrounded != null)
                    {
                        paymentAdded.Add(vrounded);
                    }
                }

                var vrefund = (from p in paymentResume.RestaurantKotBillPayment
                               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString()
                               select p).FirstOrDefault();

                if (vrefund == null)
                {
                    vrefund = (from p in GuestBillPayment
                               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString() &&
                                     p.PaymentAmount != 0
                               select p
                             ).FirstOrDefault();

                    if (vrefund != null)
                    {
                        paymentAdded.Add(vrefund);
                    }
                }

                var vcard = (from p in paymentResume.RestaurantKotBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Card.ToString()
                             select p).ToList();

                var vcardNew = (from p in GuestBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                      p.PaymentAmount != 0
                                select p).ToList();

                if (vcard != null)
                {
                    foreach (GuestBillPaymentBO bp in vcardNew)
                    {
                        var v1 = (from pr in vcard
                                  where pr.CardType == bp.CardType &&
                                        pr.PaymentAmount != 0
                                  select pr).FirstOrDefault();

                        if (v1 == null)
                        {
                            paymentAdded.Add(bp);
                        }
                    }
                }

                var update1 = (from bp in GuestBillPayment
                               from pr in paymentResume.RestaurantKotBillPayment
                               where bp.PaymentMode == pr.PaymentMode &&
                                     (
                                         bp.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                     ) &&
                                     bp.PaymentAmount != pr.PaymentAmount &&
                                     bp.PaymentAmount != 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   CurrencyAmount = pr.CurrencyAmount,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                var update2 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                     pr.CardType == bp.CardType &&
                                     pr.PaymentAmount != bp.PaymentAmount &&
                                     bp.PaymentAmount != 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   CurrencyAmount = pr.CurrencyAmount,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                paymentUpdate.AddRange(update1);
                paymentUpdate.AddRange(update2);

                var delete1 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == bp.PaymentMode &&
                                     (
                                         pr.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                                         pr.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                     ) &&
                                     bp.PaymentAmount == 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                var delete2 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                     pr.CardType == bp.CardType &&
                                     bp.PaymentAmount == 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                paymentDelete.AddRange(delete1);
                paymentDelete.AddRange(delete2);

                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO
                        {
                            ClassificationId = Convert.ToInt32(rowCWPD)
                        };
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO
                {
                    KotId = RestaurantBill.KotId
                };
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                bool billId = restaurentBillDA.UpdateRestaurantBillForHoldUp(RestaurantBill, restaurentBillDetailBOList, paymentAdded, paymentUpdate, paymentDelete, categoryWisePercentageDiscountBOList);

                if (billId)
                {
                    KotBillMasterDA kotDa = new KotBillMasterDA();
                    kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelection.aspx";
                    rtninf.IsBillHoldUp = true;

                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    HttpContext.Current.Session.Remove("KotHoldupBill");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                    HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                    HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                    HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                    HttpContext.Current.Session.Remove("IRToeknNumber");
                    HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                    HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                    HttpContext.Current.Session.Remove("tbsMessage");
                    HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                    HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo KotSubmit(int costCenterId, int kotId)
        {
            ReturnInfo rtnInf = new ReturnInfo();
            PrintInfos pinf = new PrintInfos();
            List<PrinterInfoBO> files = new List<PrinterInfoBO>();

            try
            {
                KotBillDetailDA entityDA = new KotBillDetailDA();
                PrinterInfoDA da = new PrinterInfoDA();

                bool rePrintStatus = false;

                List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
                files = da.GetRestaurentItemTypeInfoByKotId(kotId);

                if (files.Count > 0)
                {
                    foreach (PrinterInfoBO pinfo in files)
                    {
                        pinf.CostCenterId = costCenterId;
                        pinf.PrinterInfoId = pinfo.PrinterInfoId;

                        pinf.CostCenterName = pinfo.CostCenter;
                        pinf.CompanyName = pinfo.KitchenOrStockName;
                        pinf.TableNumberInformation = HttpContext.Current.Session["IRToeknNumber"].ToString();

                        if (pinfo.DefaultView == "Table")
                        {
                            pinf.CostCenterDefaultView = "Table # ";
                        }
                        else if (pinfo.DefaultView == "Token")
                        {
                            pinf.CostCenterDefaultView = "Token # ";
                        }
                        else if (pinfo.DefaultView == "Room")
                        {
                            pinf.CostCenterDefaultView = "Room # ";
                        }

                        HMUtility hmUtility = new HMUtility();

                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                        pinf.WaiterName = userInformationBO.DisplayName; //userInformationBO.EmployeeName.ToString();

                        if (pinfo.StockType == "StockItem")
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, pinf.CostCenterId, "StockItem", false);
                        }
                        else
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, pinf.CostCenterId, "KitchenItem", false).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                        }

                        if (entityBOList.Count > 0)
                        {
                            if (entityBOList.Count > 0)
                            {
                                rePrintStatus = true;

                                frmRestaurantManagement manage = new frmRestaurantManagement();
                                manage.PrintReportKot(pinfo, entityBOList, pinf, false);
                            }
                            else
                            {
                                rePrintStatus = false;
                            }
                        }
                    }

                    bool status = false;

                    if (rePrintStatus)
                    {
                        status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(kotId);
                    }

                    if (status && rePrintStatus)
                    {
                        rtnInf.IsSuccess = true;
                        rtnInf.AlertMessage = CommonHelper.AlertInfo("KOT Successfully Submitted.", AlertType.Success);
                    }
                    else if (!status && !rePrintStatus)
                    {
                        rtnInf.IsSuccess = false;
                        rtnInf.AlertMessage = CommonHelper.AlertInfo("KOT already submitted.", AlertType.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                rtnInf.IsSuccess = false;
                rtnInf.AlertMessage = CommonHelper.AlertInfo("KOT is not Submitted Successfully.", AlertType.Information);

            }

            return rtnInf;
        }
        [WebMethod]
        public static List<InvCategoryBO> GetCategoryWiseDiscount(int kotId)
        {
            List<InvCategoryBO> invCategoryLst = new List<InvCategoryBO>();
            try
            {
                InvCategoryDA invCategoryDA = new InvCategoryDA();
                invCategoryLst = invCategoryDA.GetInvCategoryDetailsForRestaurantBill(kotId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return invCategoryLst;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformPercentageDiscountInformation(int kotId, string margeKotId, int costCenterId, List<string> strCategoryIdList, string discountType, string strDiscountAmount)
        {
            string categoryIdList = string.Empty;

            foreach (string categoryId in strCategoryIdList)
            {
                if (string.IsNullOrWhiteSpace(categoryIdList))
                {
                    categoryIdList = categoryId.ToString();
                }
                else
                {
                    categoryIdList = categoryIdList + "," + categoryId.ToString();
                }
            }

            decimal discountPercentAmount = !string.IsNullOrWhiteSpace(strDiscountAmount) ? Convert.ToDecimal(strDiscountAmount) : 0;

            KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
            KotBillMasterBO kotBillMasterBO = new KotBillMasterBO();

            if (!string.IsNullOrEmpty(margeKotId))
            {
                margeKotId += kotId.ToString() + "," + margeKotId;
            }
            else
            {
                margeKotId = kotId.ToString();
            }

            kotBillMasterBO = kotBillMasterDA.GetCategoryWisePercentageDiscountInfo(kotId, margeKotId, costCenterId, categoryIdList, discountType, discountPercentAmount);

            HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
            HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] = strCategoryIdList;

            return kotBillMasterBO.TotalAmount.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string GetCategoryWiseDiscountAmountByDefaultSetting(int kotId, int costCenterId, string discountAmount, string discountType)
        {
            decimal discountPercentAmount = !string.IsNullOrWhiteSpace(discountAmount) ? Convert.ToDecimal(discountAmount) : 0;

            KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
            KotBillMasterBO kotBillMasterBO = new KotBillMasterBO();

            kotBillMasterBO = kotBillMasterDA.GetCategoryWiseDiscountAmountByDefaultSetting(kotId, costCenterId, discountPercentAmount, discountType);

            List<string> categoryList = new List<string>();

            if (!string.IsNullOrEmpty(kotBillMasterBO.CategoryList))
            {
                string[] catList = kotBillMasterBO.CategoryList.Split(',');

                foreach (string s in catList)
                {
                    categoryList.Add(s);
                }
            }

            HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] = categoryList;

            return kotBillMasterBO.TotalAmount.ToString();
        }
        [WebMethod]
        public static ReturnInfo ClearOrderedItem(int costcenterId, int kotId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                KotBillDetailDA kotDa = new KotBillDetailDA();
                Boolean status = kotDa.ClearOrderedItem(costcenterId, kotId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo UpdateRestaurantBillSummary(int billId, string discountType, string costCenterId, string discountAmount, string categoryIdList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RestaurentBillDA billDA = new RestaurentBillDA();
                rtninf.IsSuccess = billDA.DistributionRestaurantBill(billId, categoryIdList, discountType, Convert.ToDecimal(discountAmount), Convert.ToInt32(costCenterId));

                if (rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = true;
                    rtninf.Pk = billId;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
    }
}

public class PrintInfos
{
    public int PrinterInfoId { get; set; }
    public int CostCenterId { get; set; }
    public int KotIdInformation { get; set; }
    public string TableNumberInformation { get; set; }
    public string CostCenterName { get; set; }
    public string CostCenterDefaultView { get; set; }
    public string PrinterName { get; set; }
    public string WaiterName { get; set; }
    public string CompanyName { get; set; }
    public bool IsRestaurantOrderSubmitDisable { get; set; }
    public bool IsRestaurantTokenInfoDisable { get; set; }
}