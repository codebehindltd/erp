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

namespace HotelManagement.Presentation.Website.AirTicketing.Reports
{
    public partial class frmATBillInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                string queryStringId = Request.QueryString["billId"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ReportProcessing();
                }
            }
        }        
        private void ReportProcessing()
        {
            string reportName = "rptATBillingInvoice";
            string queryStringUS = Request.QueryString["US"];
            List<ReportParameter> reportParam = new List<ReportParameter>();
            string queryStringId = Request.QueryString["billId"];
            int billID = Int32.Parse(queryStringId);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.DataSources.Clear();

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();

                string companyName = string.Empty;
                string companyAddress = string.Empty;
                string binNumber = string.Empty;
                string tinNumber = string.Empty;
                string projectName = string.Empty;
                if (files[0].CompanyId > 0)
                {
                    companyName = files[0].CompanyName;
                    companyAddress = files[0].CompanyAddress;
                    binNumber = files[0].VatRegistrationNo;
                    tinNumber = files[0].TinNumber;


                    reportName = "rptATBillingInvoice";
                    //commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBillingInvoiceTemplateWithoutHeader", "IsBillingInvoiceTemplateWithoutHeader");

                    //if (commonSetupBO != null)
                    //{
                    //    if (commonSetupBO.SetupValue != "0")
                    //    {
                    //        reportName = "rptSTBillingInvoiceWithoutHeader";
                    //    }
                    //}

                    reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                }

                RestaurantBillBO billBO = new RestaurantBillBO();
                RestaurentBillDA billDA = new RestaurentBillDA();
                billBO = billDA.GetATBillInfoByBillId(billID);

                string billRemarks = string.Empty;
                string billDescription = string.Empty;
                string billDeclaration = string.Empty;
                string imagePreparedBySignature = string.Empty;

                if (billBO != null)
                {
                    if (billBO.BillId > 0)
                    {
                        billRemarks = billBO.Remarks;
                        //binNumber = billBO.BinNumber;
                        //tinNumber = billBO.TinNumber;
                        //projectName = billBO.ProjectName;
                        billDescription = billBO.BillDescription;
                        billDeclaration = billBO.BillDeclaration;
                        
                        if (!string.IsNullOrEmpty(queryStringUS))
                        {
                            imagePreparedBySignature = billBO.UserSignature;
                        }
                        //if (billBO.IsInvoiceVatAmountEnable == false)
                        //{
                        //    if (reportName == "rptRestaurentBillForA4Page")
                        //    {
                        //        reportName = "rptRestaurentBillForA402Page";
                        //    }
                        //    else
                        //    {
                        //        reportName = "rptRestaurentBillForA402PageWithoutHeader";
                        //    }
                        //}
                    }
                }

                var reportPath = Server.MapPath(@"~/AirTicketing/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;                

                reportParam.Add(new ReportParameter("CompanyProfile", companyName));
                reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
                reportParam.Add(new ReportParameter("VatRegistrationNo", binNumber));
                reportParam.Add(new ReportParameter("TinNumber", tinNumber));
                reportParam.Add(new ReportParameter("ProjectName", projectName));
                reportParam.Add(new ReportParameter("BillRemarks", billRemarks));
                reportParam.Add(new ReportParameter("BillDescription", billDescription));
                reportParam.Add(new ReportParameter("BillDeclaration", billDeclaration));

                rvTransaction.LocalReport.EnableExternalImages = true;
                HMCommonDA hmCommonDA = new HMCommonDA();
                //string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
                string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                }
                
                if (!string.IsNullOrWhiteSpace(imagePreparedBySignature))
                {
                    reportParam.Add(new ReportParameter("BillingDefaultPreparedBySignature", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/UserSignature/" + imagePreparedBySignature)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("BillingDefaultPreparedBySignature", "Hide"));
                }

                HMCommonSetupBO setUpBOApprovedBySignature = new HMCommonSetupBO();
                string imageApprovedBySignature = "0";
                if (!string.IsNullOrEmpty(queryStringUS))
                {
                    setUpBOApprovedBySignature = commonSetupDA.GetCommonConfigurationInfo("BillingDefaultApprovedBySignature", "BillingDefaultApprovedBySignature");
                    if (!string.IsNullOrWhiteSpace(setUpBOApprovedBySignature.SetupValue))
                    {
                        imageApprovedBySignature = setUpBOApprovedBySignature.SetupValue;
                    }
                }

                if (imageApprovedBySignature != "0")
                {
                    reportParam.Add(new ReportParameter("BillingDefaultApprovedBySignature", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/UserSignature/" + imageApprovedBySignature)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("BillingDefaultApprovedBySignature", "Hide"));
                }

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

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                rvTransaction.LocalReport.SetParameters(reportParam);

                RetailPosBillReturnBO restaurantBill = new RetailPosBillReturnBO();
                RestaurentPosDA rda = new RestaurentPosDA();
                restaurantBill = rda.ATPosBill(billID);

                var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill.PosBillWithSalesReturn));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[1], restaurantBill.PosSalesReturnPayment));

                rvTransaction.LocalReport.DisplayName = "Bill Invoice";
                rvTransaction.LocalReport.Refresh();
            }
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
    }
}