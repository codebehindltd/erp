using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using System.Web.Services;
using System.IO;
using Microsoft.Reporting.WebForms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmCostCenterSelection : System.Web.UI.Page
    {
        HiddenField innboardMessage;

        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;

        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            lblLoggedInUser.Text = "Welcome " + userInformationBO.DisplayName;

            //HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
            //HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
            //HttpContext.Current.Session.Remove("IRCostCenterIdSession");
            //HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
            //HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
            //HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
            //HttpContext.Current.Session.Remove("IRToeknNumber");
            //HttpContext.Current.Session.Remove("KotHoldupBill");
            //HttpContext.Current.Session.Remove("RestaurantKotBillResume");

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


            LoadCostCenterInformation();
            LoadTokenInformation();
        }
        private void CleanSessionInformation()
        {
            Session["IRCostCenterIdSession"] = null;
            Session["IRtxtKotIdInformation"] = null;
            Session["IRCostCenterServiceChargeSession"] = null;
            Session["IRCostCenterVatAmountSession"] = null;
            Session["GuestPaymentDetailListForGrid"] = null;
        }
        private void LoadCostCenterInformation()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO != null)
            {
                Session["IRtxtBearerIdInformation"] = userInformationBO.UserInfoId.ToString();

                CostCentreTabDA entityDA = new CostCentreTabDA();
                List<CostCentreTabBO> entityListBO = new List<CostCentreTabBO>();
                entityListBO = entityDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.DisplayName, userInformationBO.UserInfoId, 1);
                int RoomPossibleVacantDiv = 0;
                string fullContent = string.Empty;
                string roomSummary = string.Empty;

                string topPart = "<a href='javascript:void()' class='block-heading'>";
                string topTemplatePart = "</a> <div> <div style='border: 1px solid #ccc; margin:5px; overflow:hidden;'>";
                string groupNamePart = "Cost Center Information";
                string endTemplatePart = "</div> </div>";

                string subContent = string.Empty;

                for (int iListNumber = 0; iListNumber < entityListBO.Count; iListNumber++)
                {
                    if (entityListBO[iListNumber].DefaultView == "Token")
                    {
                        subContent += @"<a href='/POS/frmRestaurantManagement.aspx?IR=TokenAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;
                    }
                    else if (entityListBO[iListNumber].DefaultView == "Table")
                    {
                        //subContent += @"<a href='/Restaurant/frmRestaurantManagement.aspx?IR=TableAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;

                        subContent += @"<a href='/POS/frmKotBill.aspx?Kot=TableAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;
                    }
                    else if (entityListBO[iListNumber].DefaultView == "Room")
                    {
                        subContent += @"<a href='/POS/frmRestaurantManagement.aspx?IR=RoomAllocation&CostCenterId=" + entityListBO[iListNumber].CostCenterId;
                    }
                    subContent += "'>";
                    subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + entityListBO[iListNumber].CostCenter + "</div></div></a>";
                    RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                }

                fullContent += subContent;

                literalCostCenterInformation.Text = topPart + groupNamePart + topTemplatePart + fullContent + endTemplatePart;
            }
            else
            {
                Response.Redirect("/POS/Login.aspx");
            }
        }

        private void LoadTokenInformation()
        {
            string fullContent = string.Empty;
            string groupNamePart = string.Empty;

            string endTemplatePart = @"</div> </div>";

            KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
            List<KotBillMasterBO> kotBillMasterList = new List<KotBillMasterBO>();

            kotBillMasterList = kotBillMasterDA.GetRestaurantTokenInfo();

            string subContent = string.Empty;

            foreach (KotBillMasterBO kot in kotBillMasterList)
            {
                if (kot.IsBillHoldup)
                {
                    subContent += "<div class='DivRoomContainerHeight61'><div class='RoomOccupaiedDiv'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + kot.TokenNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + kot.KotId + "</div></div>";
                }
                else
                {
                    subContent += "<div class='DivRoomContainerHeight61'><div class='RoomVacantDiv'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + kot.TokenNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + kot.KotId + "</div></div>";
                }
            }

            fullContent += subContent;

            litTokenList.Text = fullContent + endTemplatePart;
        }


        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string reportName = "rptRestaurentBillForPos";
            string billnumber = hfBillId.Value;

            billnumber = "RB" + billnumber.PadLeft(8, '0');

            RestaurentBillDA rda = new RestaurentBillDA();

            int billID = rda.GetBillPaymentByBillId(billnumber);

            if (!string.IsNullOrEmpty(billnumber))
            {
                if (billID > 0)
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

                    rvTransactionShow.LocalReport.SetParameters(reportParam);


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
                            "$('.ui-dialog-titlebar').css('padding','0.8em 1em');";

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

                    IframeReportPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

                    //rrp.PrintForPos();

                    //string url = "/Restaurant/Reports/frmReportTPRestaurantBillInfo.aspx?billID=" + this.txtBillId.Value;
                    //string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes'); window.onunload = CloseWindow();";
                    ////Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

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

        [WebMethod]
        public static ReturnInfo LoadHoldupBill(int kotId)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
                KotBillMasterBO kotBill = new KotBillMasterBO();

                kotBill = kotBillMasterDA.GetKotBillMasterInfoKotId(kotId);
                HttpContext.Current.Session["KotHoldupBill"] = kotBill;

                rtninf.IsSuccess = true;
                rtninf.RedirectUrl = "frmRestaurantManagement.aspx?IR=TokenAllocation&CostCenterId=" + kotBill.CostCenterId;

                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            return rtninf;
        }
    }
}