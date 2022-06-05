using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Common
{
    public class ReportPrinting
    {
        public string PrintReport(LocalReport report, string pageType)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Double WidthInInch = 8.5, HeightInInch = 11;
            string fileName = string.Empty, fileNamePrint = string.Empty;

            if (pageType == HMConstants.PrintPageType.Portrait.ToString())
            {
                WidthInInch = 8.5;
                HeightInInch = 11;
            }
            else if (pageType == HMConstants.PrintPageType.Landscape.ToString())
            {
                WidthInInch = 11;
                HeightInInch = 8.5;
            }

            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;


                //LocalReport rpt = rvTransaction.LocalReport;
                string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>" + WidthInInch + "in</PageWidth>" +
                "<PageHeight>" + HeightInInch + "in</PageHeight>" +
                "<MarginTop>0.10in</MarginTop>" +
                "<MarginLeft>0.1in</MarginLeft>" +
                "<MarginRight>0.1in</MarginRight>" +
               " <MarginBottom>0.10in</MarginBottom>" +
            "</DeviceInfo>";

                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType,
                               out encoding, out extension, out streamids, out warnings);

                DateTime dateTime = DateTime.Now;
                fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
                fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Open exsisting pdf
                Document document;

                if (pageType == HMConstants.PrintPageType.Portrait.ToString())
                {
                    document = new Document(PageSize.LETTER);
                }
                else if (pageType == HMConstants.PrintPageType.Landscape.ToString())
                {
                    document = new Document(PageSize.LETTER.Rotate());
                }
                else
                {
                    document = new Document(PageSize.LETTER);
                }

                //document.PageSize = new 
                PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
                //Getting a instance of new pdf wrtiter

                // document.SetPageSize(PageSize.LETTER.Rotate());

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
                   HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                int i = 0;
                int p = 0;
                int n = reader.NumberOfPages;

                //Rectangle psize = reader.GetPageSize(1);
                //float width = psize.Width;
                //float height = psize.Height;

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

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "../../PrintDocument/" + fileNamePrint;
        }
    }
}