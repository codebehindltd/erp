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
    public partial class frmReportPreRegistrationCard : System.Web.UI.Page
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
            string queryStringId = Request.QueryString["ReservationId"];
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
                    reportParam.Add(new ReportParameter("ReportName", "Pre Registration Card"));

                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();                    

                    if (files[0].CompanyId > 0)
                    {
                        rvTransaction.LocalReport.EnableExternalImages = true;
                        rvTransaction.LocalReport.SetParameters(reportParam);

                        string printDate = string.Empty;
                        List<RegistrationCardInfoBO> billBOList = new List<RegistrationCardInfoBO>();
                        RoomRegistrationDA rda = new RoomRegistrationDA();
                        billBOList = rda.GetRegistrationCardInfoByIdNType(rtransactionId, "Reservation");

                        var dataSet = rvTransaction.LocalReport.GetDataSourceNames();
                        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], billBOList));

                        rvTransaction.LocalReport.DisplayName = "Pre Registration Card";
                        rvTransaction.LocalReport.Refresh();
                    }
                }
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
    }
}