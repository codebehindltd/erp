using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
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

namespace HotelManagement.Presentation.Website.Membership.Reports
{
    public partial class ReportOnlineMembership : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            string memberId = Request.QueryString["Id"];

            if (!String.IsNullOrEmpty(memberId))
            {
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(memberId));
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
        private void LoadReport(int memberId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Membership/Reports/Rdlc/rptOnlineMembership.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            HMCommonDA hmCommonDA = new HMCommonDA();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            if (files[0].CompanyId > 0)
            {
                reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            IonlineMembershib ionline = new IonlineMembershib();

            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);

            if (onlineMember.IsAccepted1 == true)
            {
                reportParam.Add(new ReportParameter("IntroFeedback1", "Accepted"));
            }
            else if (onlineMember.IsRejected1 == true)
            {
                reportParam.Add(new ReportParameter("IntroFeedback1", "Rejected"));
            }
            else if (onlineMember.IsDeferred1 == true)
            {
                reportParam.Add(new ReportParameter("IntroFeedback1", "Deferred"));
            }

            if (onlineMember.IsAccepted2 == true)
            {
                reportParam.Add(new ReportParameter("IntroFeedback2", "Accepted"));
            }
            else if (onlineMember.IsRejected2 == true)
            {
                reportParam.Add(new ReportParameter("IntroFeedback2", "Rejected"));
            }
            else if (onlineMember.IsDeferred2 == true)
            {
                reportParam.Add(new ReportParameter("IntroFeedback2", "Deferred"));
            }

            if (onlineMember.IsAccepted == true)
            {
                reportParam.Add(new ReportParameter("AdminStatus", "Accepted"));
            }
            else if (onlineMember.IsRejected == true)
            {
                reportParam.Add(new ReportParameter("AdminStatus", "Rejected"));
            }
            else if (onlineMember.IsDeferred)
            {
                reportParam.Add(new ReportParameter("AdminStatus", "Deferred"));
            }

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.EnableExternalImages = true;
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/" + onlineMember.PathPersonalImg)));// @"/Images/"

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();



            //List<OnlineMemberEducationBO> educationBOs = new List<OnlineMemberEducationBO>();
            ionline.educationBOs = basicDA.GetOnlineMemberEducationsById(memberId);

            //List<OnlineMemberFamilyBO> familyBOs = new List<OnlineMemberFamilyBO>();
            ionline.familyBOs = basicDA.GetOnlineMemFamilyMemberByMemberId(memberId);
            ionline.onlineMember.Add(onlineMember);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], ionline.onlineMember));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], ionline.educationBOs));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], ionline.familyBOs));

            rvTransaction.LocalReport.DisplayName = "Member Information";
            rvTransaction.LocalReport.Refresh();
        }
    }
}