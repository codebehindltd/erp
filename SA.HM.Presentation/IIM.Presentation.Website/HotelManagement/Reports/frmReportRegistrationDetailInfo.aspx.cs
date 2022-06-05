using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Restaurant;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportRegistrationDetailInfo : System.Web.UI.Page
    {
        //**************************** Handlers ****************************//
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadReport();
            }
        }
        private void LoadReport()
        {
            string queryStringId = Request.QueryString["RegistrationId"];
            if (!string.IsNullOrEmpty(queryStringId))
            {
                int rtransactionId = Int32.Parse(queryStringId);

                if (!string.IsNullOrEmpty(queryStringId))
                {
                    RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                    HMCommonDA hmCommonDA = new HMCommonDA();
                    string roomRegistrationTermsAndConditions = string.Empty;
                    roomRegistrationTermsAndConditions = hmCommonDA.GetCustomFieldValueByFieldName("RoomRegistrationTermsAndConditions");

                    var reportPath = string.Empty;
                    rvTransaction.ProcessingMode = ProcessingMode.Local;
                    rvTransaction.LocalReport.DataSources.Clear();

                    reportPath = Server.MapPath(@"~/HotelManagement\Reports\Rdlc\rptPreRegistrationCard.rdlc");
                    if (!File.Exists(reportPath))
                        return;

                    string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

                    rvTransaction.LocalReport.ReportPath = reportPath;
                    List<ReportParameter> reportParam = new List<ReportParameter>();
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                    reportParam.Add(new ReportParameter("RoomRegistrationTermsAndConditions", roomRegistrationTermsAndConditions));
                    reportParam.Add(new ReportParameter("ReportName", "Registration Card"));

                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();

                    if (files[0].CompanyId > 0)
                    {
                        rvTransaction.LocalReport.EnableExternalImages = true;
                        rvTransaction.LocalReport.SetParameters(reportParam);

                        string printDate = string.Empty;
                        List<RegistrationCardInfoBO> billBOList = new List<RegistrationCardInfoBO>();
                        RoomRegistrationDA rda = new RoomRegistrationDA();
                        billBOList = rda.GetRegistrationCardInfoByIdNType(rtransactionId, "Registration");

                        var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                        rvTransaction.LocalReport.EnableExternalImages = true;
                        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], billBOList));

                        rvTransaction.LocalReport.DisplayName = "Registration Card";
                        rvTransaction.LocalReport.Refresh();
                    }
                }
            }
        }
        //private void LoadReport()
        //{
        //    string roomRegistrationTermsAndConditions = string.Empty;
        //    string queryStringId = Request.QueryString["RegistrationId"];
        //    bool isPreview = Convert.ToBoolean(Request.QueryString["isPreview"]);
        //    if (!string.IsNullOrEmpty(queryStringId))
        //    {
        //        HMCommonDA hmCommonDA = new HMCommonDA();
        //        roomRegistrationTermsAndConditions = hmCommonDA.GetCustomFieldValueByFieldName("RoomRegistrationTermsAndConditions");

        //        int rRegistrationId = Int32.Parse(queryStringId);

        //        if (!string.IsNullOrEmpty(queryStringId))
        //        {
        //            RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
        //            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
        //            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(rRegistrationId);
        //            if (roomRegistrationBO != null)
        //            {
        //                var reportPath = string.Empty;
        //                rvTransaction.ProcessingMode = ProcessingMode.Local;
        //                rvTransaction.LocalReport.DataSources.Clear();
        //                if (roomRegistrationBO.RoomId > 0)
        //                {
        //                    reportPath = Server.MapPath(@"~/HotelManagement\Reports\Rdlc\rptRegistrationDetail.rdlc");
        //                    if (!File.Exists(reportPath))
        //                        return;

        //                    rvTransaction.LocalReport.ReportPath = reportPath;
        //                    CompanyDA companyDA = new CompanyDA();
        //                    List<CompanyBO> files = companyDA.GetCompanyInfo();
        //                    List<ReportParameter> reportParam = new List<ReportParameter>();

        //                    if (files[0].CompanyId > 0)
        //                    {
        //                        rvTransaction.LocalReport.EnableExternalImages = true;
        //                        rvTransaction.LocalReport.SetParameters(reportParam);

        //                        string printDate = string.Empty;
        //                        List<RegistrationCardInfoBO> billBOList = new List<RegistrationCardInfoBO>();
        //                        RoomRegistrationDA rda = new RoomRegistrationDA();
        //                        billBOList = rda.GetRoomRegistrationDetailByRegistrationId(rRegistrationId, files[0].CompanyName, files[0].CompanyAddress, files[0].WebAddress);

        //                        reportParam.Add(new ReportParameter("RoomRegistrationTermsAndConditions", roomRegistrationTermsAndConditions));
        //                        rvTransaction.LocalReport.SetParameters(reportParam);

        //                        var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
        //                        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], billBOList));

        //                        rvTransaction.LocalReport.DisplayName = "Registration Card";
        //                        rvTransaction.LocalReport.Refresh();
        //                    }
        //                }
        //                else
        //                {
        //                    reportPath = Server.MapPath(@"~/HotelManagement\Reports\Rdlc\rptBlankRegistration.rdlc");
        //                    if (!File.Exists(reportPath))
        //                        return;

        //                    rvTransaction.LocalReport.ReportPath = reportPath;
        //                    CompanyDA companyDA = new CompanyDA();
        //                    List<CompanyBO> files = companyDA.GetCompanyInfo();
        //                    List<ReportParameter> reportParam = new List<ReportParameter>();

        //                    if (files[0].CompanyId > 0)
        //                    {
        //                        rvTransaction.LocalReport.EnableExternalImages = true;
        //                        rvTransaction.LocalReport.SetParameters(reportParam);

        //                        string printDate = string.Empty;
        //                        List<RegistrationCardInfoBO> billBOList = new List<RegistrationCardInfoBO>();
        //                        RoomRegistrationDA rda = new RoomRegistrationDA();
        //                        billBOList = rda.GetBlankRoomRegistrationDetailByRegistrationId(rRegistrationId, files[0].CompanyName, files[0].CompanyAddress, files[0].WebAddress);

        //                        reportParam.Add(new ReportParameter("RoomRegistrationTermsAndConditions", roomRegistrationTermsAndConditions));
        //                        rvTransaction.LocalReport.SetParameters(reportParam);

        //                        var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
        //                        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], billBOList));

        //                        rvTransaction.LocalReport.DisplayName = "Registration Card";
        //                        rvTransaction.LocalReport.Refresh();
        //                    }
        //                }
        //            }
        //            if(!isPreview)
        //                SendMail(rRegistrationId);
        //        }

        //    }
        //}
        private bool SendMail(int registrationId)
        {
            bool status = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsRoomRegistrationEmailAutoPostingEnable");
            string IsEnable = commonSetupBO.SetupValue;
            if (IsEnable == "0")
                return status;

            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> detailBO = new List<GuestInformationBO>();
            detailBO = guestDA.GetGuestInformationByRegistrationId(registrationId);

            if (detailBO != null)
            {
                if(detailBO.Count > 0)
                {
                    string ReceivingMail = detailBO[0].GuestEmail;
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

                    byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
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

                    string MailBody = "Please find the attachment.";

                    Email email;
                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
                    string mainString = commonSetupBO.SetupValue;

                    if (!string.IsNullOrEmpty(mainString))
                    {
                        string[] dataArray = mainString.Split('~');
                        email = new Email()
                        {
                            From = dataArray[0],
                            Password = dataArray[1],
                            To = ReceivingMail,
                            Subject = "Confirmation for Registration.",
                            Body = MailBody,
                            AttachmentSavedPath = filePath,
                            Host = dataArray[2],
                            Port = dataArray[3]
                        };

                        try
                        {
                            status = EmailHelper.SendEmail(email,null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
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
    }
}