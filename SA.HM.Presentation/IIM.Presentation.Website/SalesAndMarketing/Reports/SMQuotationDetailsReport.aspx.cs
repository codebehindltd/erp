using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class SMQuotationDetailsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long quotationId = Convert.ToInt64(Request.QueryString["id"]);
                bool isFromAccount = Request.QueryString["frmacc"] == "1";
                LoadReport(quotationId, isFromAccount);
            }

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
        private void LoadReport(long quotationId, bool isFromAccount)
        {

            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            long dealId = Convert.ToInt64(Request.QueryString["did"]);
            var reportPath = "";
            if (!isFromAccount)
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptQuotationDetailsForInventoryNTechnical.rdlc");
            else
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptQuotationDetails.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            string reportName = "Quotation Details";
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;


            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSalesNoteEnable", "IsSalesNoteEnable");
            string IsSalesNoteEnable = setUpBO.SetupValue;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("IsSalesNoteEnable", IsSalesNoteEnable));

            rvTransaction.LocalReport.SetParameters(paramReport);

            SMQuotationReportViewBO quotation = new SMQuotationReportViewBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            quotation = salesDA.GetQuotationDetailsByIdForReport(quotationId);
            

            bool hasItem = quotation.QuotationItemDetails.Count > 0;
            //quotation.Quotation.ForEach(i => i.ProposalDate=CommonHelper.DateTimeClientFormatWiseConversionForDisplay(i.ProposalDate));
            //GuestCompanyBO companyBO = guestCompanyDA.GetGuestCompanyInfoForSalesCallById(quotation.Quotation[0].CompanyId);
            // quotation.Company.Add(companyBO);
            //quotation.QuotationItemDetails = salesDA.GetQuotationItemDetailsById(quotationId, CommonHelper.QuotationItemType.Item.ToString());
            //quotation.QuotationServiceDetails = salesDA.GetQuotationServiceDetailsById(quotationId, CommonHelper.QuotationItemType.Service.ToString());

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], quotation.Quotation));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], quotation.Company));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], quotation.QuotationItemDetails));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[3], quotation.QuotationServiceDetails));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[4], quotation.QuotationRestaurantDetails));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[5], quotation.QuotationBanquetDetails));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[6], quotation.QuotationGuestRoomDetails));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[7], quotation.QuotationServiceOutletDetails));


            rvTransaction.LocalReport.DisplayName = "Quotation Details";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
            //Report save as Deal document
            bool isSendMail = Request.QueryString["smail"] == "1";
            if (isFromAccount && dealId > 0)
                SaveQuoatationAsDealDocument(quotation, userInformationBO, hmCommonDA);
            else if (isSendMail)
            {
                SendEmail(userInformationBO, hasItem);
            }

        }
        private void SaveQuoatationAsDealDocument(SMQuotationReportViewBO quotation, UserInformationBO userInformationBO, HMCommonDA hmCommonDA)
        {
            long dealId = Convert.ToInt64(Request.QueryString["did"]);

            if (dealId > 0)
            {
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
                    fileName = quotation.Quotation[0].QuotationNo + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/SalesAndMarketing/Images/Deal/" + fileName), FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    DocumentsBO document = new DocumentsBO();
                    DocumentsDA docDA = new DocumentsDA();

                    document.OwnerId = dealId;
                    document.Name = fileName;
                    document.Path = @"/SalesAndMarketing/Images/Deal/";
                    document.Extention = "." + extension;
                    document.DocumentType = "Image";
                    document.DocumentCategory = "SalesQuotationDocuments";
                    document.CreatedBy = userInformationBO.UserInfoId;
                    docList.Add(document);

                    List<DocumentsBO> predocList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("SalesQuotationDocuments", dealId);
                    Boolean status = false;

                    if (predocList.Count == 0)
                    {
                        status = docDA.SaveDocumentsInfo(docList);
                    }
                    else
                    {
                        document.DocumentId = predocList[0].DocumentId;
                        status = docDA.UpdateDocumentsInfoByOwnerId(document);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private bool SendEmail(UserInformationBO userInformationBO, bool hasItem = false)
        {

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rvTransaction.LocalReport.Render("PDF", null, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileName = string.Empty, fileNamePrint = string.Empty, filePath = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = "QN" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            filePath = HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName);
            if (!File.Exists(filePath))
                return false;

            Email email;
            bool status = false;

            List<UserGroupBO> userGroupBO = new List<UserGroupBO>();
            UserGroupDA userGroupDA = new UserGroupDA();
            userGroupBO = userGroupDA.GetUserGroupInfo();

            UserGroupBO userGroup = new UserGroupBO();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;
            string emailAddress = string.Empty;
            string[] dataArray = new string[100000000];
            if (!string.IsNullOrEmpty(mainString))
            {
                dataArray = mainString.Split('~');
            }
            if (hasItem)
            {
                userGroup = userGroupBO.Where(i => i.UserGroupType == ConstantHelper.UserGroupType.Inventory.ToString()).FirstOrDefault();
                emailAddress = userGroup != null ? userGroup.Email : string.Empty;

                if (!string.IsNullOrWhiteSpace(emailAddress))
                {
                    email = new Email()
                    {
                        From = dataArray[0],
                        Password = dataArray[1],
                        To = emailAddress,
                        Subject = "Item Out For Sales",
                        Body = "Please Out Product From Inventory.",
                        AttachmentSavedPath = filePath,
                        Host = dataArray[2],
                        Port = dataArray[3],
                        TempleteName = HMConstants.EmailTemplates.BirthdayWish
                    };

                    var tokens = new Dictionary<string, string>
                         {
                             {"COMPANYNAME",null}
                };
                    try
                    {
                        status = EmailHelper.SendEmail(emailAddress, email, tokens);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            userGroup = userGroupBO.Where(i => i.UserGroupType == ConstantHelper.UserGroupType.Technical.ToString()).FirstOrDefault();
            emailAddress = userGroup != null ? userGroup.Email : string.Empty;

            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                email = new Email()
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = emailAddress,
                    Subject = "Item Out For Sales",
                    Body = hasItem ? "Please Receive Your Product From Inventory." : "Please Implement.",
                    AttachmentSavedPath = filePath,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    TempleteName = HMConstants.EmailTemplates.BirthdayWish
                };

                var tokens = new Dictionary<string, string>
                         {
                             {"COMPANYNAME",null}
                };
                try
                {
                    status = EmailHelper.SendEmail(emailAddress, email, tokens);
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