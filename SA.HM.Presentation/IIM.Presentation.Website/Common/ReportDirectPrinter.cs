using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Data;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security;
using System.Security.Permissions;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;

namespace HotelManagement.Presentation.Website.Common
{
    public class ReportDirectPrinter
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        float startY = 0;
        Metafile pageImage;
        string messageInfo = string.Empty;

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public void PrintDefaultPage(LocalReport report, string printerName = "")
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = @"\\192.168.1.35\Samsung ML-1860 Series"; //printerName;  // "Samsung ML-1860 Series";

            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        public void PrintDefaultPage(LocalReport report, string printerName, Double WidthInInch = 8.5, Double HeightInInch = 11)
        {
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
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            PaperSize pkCustomSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", Convert.ToInt32(3 * 100), (11 * 100)); //Convert.ToInt32((11 / 2.54) * 100)
            printDoc.DefaultPageSettings.PaperSize = pkCustomSize;
            printDoc.PrinterSettings.PrinterName = printerName.Trim(); //@"\\192.168.1.35\Samsung ML-1860 Series"; //printerName;  // "Samsung ML-1860 Series";
            printDoc.DefaultPageSettings.Margins.Top = 0;
            printDoc.DefaultPageSettings.Margins.Bottom = 0;
            printDoc.DefaultPageSettings.Margins.Left = 0;
            printDoc.DefaultPageSettings.Margins.Right = 0;

            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }
        public void PrintWithCustomPage(LocalReport LocalReport, Double WidthInInCM, Double HeightInInCM, String PrinterName)
        {
            String deviceInfo = "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
              "  <PageWidth>" + WidthInInCM + "cm</PageWidth>" +
              "  <PageHeight>" + HeightInInCM + "cm</PageHeight>" +
              "  <MarginTop>0.15cm</MarginTop>" +
              "  <MarginLeft>0.15cm</MarginLeft>" +
              "  <MarginRight>0.25cm</MarginRight>" +
              "  <MarginBottom>0.25cm</MarginBottom>" +
              "</DeviceInfo>";

            startY = 0;
            m_streams = new List<Stream>();
            //Byte[] bytes;
            //String fileName = HttpContext.Current.Server.MapPath(@"~/PrintDocument/f.emf");
            //bytes = LocalReport.Render("Image", deviceInfo);
            //if (File.Exists(fileName))
            //    File.Delete(fileName);
            //FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            //fs.Write(bytes, 0, bytes.Length);
            //fs.Close();
            //Process.Start(fileName);

            Warning[] warnings;
            m_streams = new List<Stream>();
            LocalReport.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if ((m_streams == null) || (m_streams.Count == 0))
                return;

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = PrinterName;

            PaperSize pkCustomSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", 100, Convert.ToInt32((HeightInInCM / 2.54) * 100));
            printDoc.DefaultPageSettings.PaperSize = pkCustomSize;
            printDoc.DefaultPageSettings.Margins.Top = 0;
            printDoc.DefaultPageSettings.Margins.Bottom = 0;
            printDoc.DefaultPageSettings.Margins.Left = 0;
            printDoc.DefaultPageSettings.Margins.Right = 0;

            if (!printDoc.PrinterSettings.IsValid)
            {
                String msg = String.Format("Can't find printer \"{0}\".", PrinterName);
                throw new Exception(msg);
            }
            printDoc.PrinterSettings.DefaultPageSettings.PaperSize = printDoc.DefaultPageSettings.PaperSize;
            printDoc.PrinterSettings.DefaultPageSettings.Margins = printDoc.DefaultPageSettings.Margins;
            if (m_streams.Count > 0)
                pageImage = new Metafile(m_streams[m_currentPageIndex]);
            printDoc.PrintPage += new PrintPageEventHandler(this.PrintPages);

            printDoc.Print();
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            ev.Graphics.PageUnit = GraphicsUnit.Display;

            //Rectangle adjustedRect = new Rectangle(
            //    ev.PageBounds.Left - (int)ev.MarginBounds.X,
            //    ev.PageBounds.Top - (int)ev.MarginBounds.Y,
            //    ev.PageBounds.Width,
            //    ev.PageBounds.Height);

            Rectangle adjustedRect = new Rectangle(ev.PageBounds.Left - (int)(ev.PageSettings.HardMarginX),
                                          ev.PageBounds.Top - (int)(ev.PageSettings.HardMarginY),
                                          ev.PageBounds.Width,
                                          ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            //ev.Graphics.DrawImage(pageImage, adjustedRect);

            //ev.Graphics.DrawImage(pageImage, 0, 0, ev.MarginBounds.Width, ev.MarginBounds.Height);
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void PrintPages(Object sender, PrintPageEventArgs ev)
        {
            if (pageImage == null)
                return;
            ev.Graphics.PageUnit = GraphicsUnit.Pixel;

            ev.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            ev.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            float a = (ev.PageSettings.PrintableArea.Width / 100) * ev.Graphics.DpiX;
            float b = ((ev.PageSettings.PrintableArea.Height / 100) * ev.Graphics.DpiY);
            float scale = 1500;
            scale = 0;
            RectangleF srcRect = new RectangleF(0, startY, pageImage.Width, b - scale);
            RectangleF destRect = new RectangleF(0, 0, a, b);
            ev.Graphics.DrawImage(pageImage, destRect, srcRect, GraphicsUnit.Pixel);
            startY = startY + b - scale;
            float marignInPixel = (0.5f / 2.54f) * ev.Graphics.DpiY;
            ev.HasMorePages = (startY + marignInPixel < pageImage.Height);
        }
    }
}