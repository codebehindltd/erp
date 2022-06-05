using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Presentation.Website.Common
{
    public class PrintBarcodeLabel
    {
        public static void PrintBarcode128(string barcode, string text, decimal price)
        {
            const float width = 1.5f * 72; //1.5 inch
            const float height = 1 * 72; //1 inch

            // step 1: creation of a document-object
            var document = new Document(new iTextSharp.text.Rectangle(width, height), 5, 5, 5, 5);

            // step 2:
            // we create a writer that listens to the document and directs a PDF-stream to a file
            var fileName = HttpContext.Current.Server.MapPath("~/PrintDocument/" + Guid.NewGuid().ToString() + ".pdf");
            var writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));

            // step 3: we open the document
            document.Open();

            // step 4: we add content to the document
            var table = new PdfPTable(1) { WidthPercentage = 100 };
            table.DefaultCell.Border = 0;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.DefaultCell.PaddingLeft = 2f;

            var cb = writer.DirectContent;
            var pBarcode128 = new Barcode128();
            pBarcode128.CodeType = Barcode.CODE128;
            pBarcode128.Code = barcode;
            pBarcode128.X = 1.5f;
            pBarcode128.BarHeight = 20f;
            pBarcode128.Baseline = 12f;
            pBarcode128.Size = 12f;
            //pBarcode128.BarcodeSize = 12f;
            //{
            //    Code = HttpUtility.UrlDecode(barcode),
            //    X = 1.5f,
            //    BarHeight = 20f,
            //    Size = 12f,
            //    Baseline = 12f
            //};
            var imgProductBarcode = pBarcode128.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
            //imgProductBarcode.SpacingBefore = 10f;
            //imgProductBarcode.SpacingAfter = 20f;


            //resize to 1.2 X .6 inch
            const float barwidth = 1.2f * 72;
            const float barheight = 0.9f * 72;

            if (imgProductBarcode.Width > barwidth)
                imgProductBarcode.ScaleToFit(barwidth, barheight);
            //table.AddCell(new Phrase(new Chunk(text, FontFactory.GetFont("arial", 9f))));
            //table.AddCell(new Phrase(new Chunk(imgProductBarcode, 0, 0)));
            table.AddCell(new Phrase(new Chunk(text, FontFactory.GetFont("arial", 7f))));
            table.AddCell(imgProductBarcode);
            table.AddCell(new Phrase(new Chunk("Price: Tk " + price + "", FontFactory.GetFont("arial", 8f))));

            //PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk(imgProductBarcode, 0, 0)));
            //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk(text, FontFactory.GetFont("arial", 4f))));

            //PdfPRow pr = new PdfPRow(cell1);
            //table.AddCell(cell2);

            //Phrase p = new Phrase(new Chunk("\nRBGK", FontFactory.GetFont("arial", 6f)));
            //PdfPCell cell = new PdfPCell(p);
            //cell.Border = 0;
            //cell.PaddingLeft = 10f;
            ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
            // table.AddCell(cell);

            //table.AddCell(new Phrase(new Chunk("\nRBGK", FontFactory.GetFont("arial", 6f))));
            //table.AddCell(new PdfCell(new Cell("\nRBGK"), 1, 0, 100, 0, 0, 0));
            //table.AddCell();

            document.Add(table);

            // step 5: print label
            var jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);

            // step 6: we close the document
            document.Close();

            // step 7: output content to the browser
            HttpContext.Current.Response.ContentType = "Application/pdf";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=\"" + Path.GetFileName(fileName) + "\"");

            //Delete file after 7 minutes 
            ThreadPool.QueueUserWorkItem(DeleteFile, fileName);

            HttpContext.Current.Server.Transfer("~/PrintDocument/" + Path.GetFileName(fileName));
        }

        public static string PrintBarcode(string barcode, string text, decimal price)
        {
            const float width = 1.5f * 72; //1.5 inch
            const float height = 1 * 72; //1 inch

            // step 1: creation of a document-object
            var document = new Document(new iTextSharp.text.Rectangle(width, height), 5, 5, 5, 5);

            // step 2:
            // we create a writer that listens to the document and directs a PDF-stream to a file
            var fileName = HttpContext.Current.Server.MapPath("~/PrintDocument/" + Guid.NewGuid().ToString() + ".pdf");
            var writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));

            // step 3: we open the document
            document.Open();

            // step 4: we add content to the document
            var table = new PdfPTable(1) { WidthPercentage = 100 };
            table.DefaultCell.Border = 0;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.DefaultCell.PaddingLeft = 2f;

            var cb = writer.DirectContent;
            var pBarcode128 = new Barcode128();
            pBarcode128.CodeType = Barcode.CODE128;
            pBarcode128.Code = barcode;
            pBarcode128.X = 1.5f;
            pBarcode128.BarHeight = 35f;
            pBarcode128.Baseline = 12f;
            pBarcode128.Size = 11f;
            //pBarcode128.BarcodeSize = 12f;
            //{
            //    Code = HttpUtility.UrlDecode(barcode),
            //    X = 1.5f,
            //    BarHeight = 20f,
            //    Size = 12f,
            //    Baseline = 12f
            //};
            var imgProductBarcode = pBarcode128.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
            //imgProductBarcode.SpacingBefore = 10f;
            //imgProductBarcode.SpacingAfter = 20f;


            //resize to 1.2 X .8 inch
            const float barwidth = 1.2f * 72;
            const float barheight = 0.8f * 72;

            if (imgProductBarcode.Width > barwidth)
                imgProductBarcode.ScaleToFit(barwidth, barheight);
            //table.AddCell(new Phrase(new Chunk(text, FontFactory.GetFont("arial", 9f))));
            //table.AddCell(new Phrase(new Chunk(imgProductBarcode, 0, 0)));

            if (text.Length > 26)
            {
                text = text.Substring(0, 23) + "...";
            }

            table.AddCell(new Phrase(new Chunk(text, FontFactory.GetFont("arial", 7f))));
            table.AddCell(imgProductBarcode);
            table.AddCell(new Phrase(new Chunk("Price: Tk " + price + "", FontFactory.GetFont("arial", 8f))));

            //PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk(imgProductBarcode, 0, 0)));
            //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk(text, FontFactory.GetFont("arial", 4f))));

            //PdfPRow pr = new PdfPRow(cell1);
            //table.AddCell(cell2);

            //Phrase p = new Phrase(new Chunk("\nRBGK", FontFactory.GetFont("arial", 6f)));
            //PdfPCell cell = new PdfPCell(p);
            //cell.Border = 0;
            //cell.PaddingLeft = 10f;
            ////cell.HorizontalAlignment = Element.ALIGN_LEFT;
            // table.AddCell(cell);

            //table.AddCell(new Phrase(new Chunk("\nRBGK", FontFactory.GetFont("arial", 6f))));
            //table.AddCell(new PdfCell(new Cell("\nRBGK"), 1, 0, 100, 0, 0, 0));
            //table.AddCell();

            document.Add(table);

            // step 5: print label
            var jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);

            // step 6: we close the document
            document.Close();

            // step 7: output content to the browser
            HttpContext.Current.Response.ContentType = "Application/pdf";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=\"" + Path.GetFileName(fileName) + "\"");

            //Delete file after 7 minutes 
            ThreadPool.QueueUserWorkItem(DeleteFile, fileName);

            return ("PrintDocument/" + Path.GetFileName(fileName));
        }

        public static string PrintBarcode(List<InvItemViewForBarcodeBO> itemList)
        {
            const float width = 11.5f * 72; //1.5 inch
            const float height = 8.5f * 72; //1 inch

            // step 1: creation of a document-object
            var document = new Document(new iTextSharp.text.Rectangle(width, height), 5, 5, 5, 5);

            // step 2:
            // we create a writer that listens to the document and directs a PDF-stream to a file
            var fileName = HttpContext.Current.Server.MapPath("~/PrintDocument/" + Guid.NewGuid().ToString() + ".pdf");
            var writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));

            // step 3: we open the document
            document.Open();

            // step 4: we add content to the document                   

            PdfPTable table = null;
            if (itemList.Count >= 4)
            {
                table = new PdfPTable(4) { WidthPercentage = 100 };
            }
            else
            {
                table = new PdfPTable(itemList.Count) { WidthPercentage = 100 };
            }

            table.DefaultCell.Border = 0;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //table.DefaultCell.PaddingLeft = 1f;
            foreach (var item in itemList)
            {
                var cb = writer.DirectContent;
                var pBarcode128 = new Barcode128();
                pBarcode128.CodeType = Barcode.CODE128;
                pBarcode128.Code = item.ItemCode;
                //pBarcode128.X = 1f;
                pBarcode128.BarHeight = 17.5f;
                pBarcode128.Baseline = 3f;
                pBarcode128.Size = 3f;                
                //pBarcode128.BarcodeSize = 12f;
                //{
                //    Code = HttpUtility.UrlDecode(barcode),
                //    X = 1.5f,
                //    BarHeight = 20f,
                //    Size = 12f,
                //    Baseline = 12f
                //};
                var imgProductBarcode = pBarcode128.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                //imgProductBarcode.SpacingBefore = 10f;
                //imgProductBarcode.SpacingAfter = 20f;


                //resize to 1.2 X .8 inch
                const float barwidth = 1f * 72;
                const float barheight = 0.5f * 72;

                if (imgProductBarcode.Width > barwidth)
                    imgProductBarcode.ScaleToFit(barwidth, barheight);
                //table.AddCell(new Phrase(new Chunk(text, FontFactory.GetFont("arial", 9f))));
                //table.AddCell(new Phrase(new Chunk(imgProductBarcode, 0, 0)));

                if (item.ItemName.Length > 26)
                {
                    item.ItemName = item.ItemName.Substring(0, 23) + "...";
                }
                PdfPCell cell = new PdfPCell();
                cell.Padding = 10f;
                cell.AddElement(new Phrase(new Chunk(item.ItemName, FontFactory.GetFont("arial", 10f))));
                cell.AddElement(new Phrase(new Chunk(" ", FontFactory.GetFont("arial", 3f))));
                cell.AddElement(imgProductBarcode);
                string slash = item.Location == null ? "" : "/ ";

                cell.AddElement(new Phrase(new Chunk(item.CostCenter + slash + item.Location, FontFactory.GetFont("arial", 10f))));

                table.AddCell(cell);
            }
            document.Add(table);

            // step 5: print label
            var jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);

            // step 6: we close the document
            document.Close();

            // step 7: output content to the browser
            HttpContext.Current.Response.ContentType = "Application/pdf";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=\"" + Path.GetFileName(fileName) + "\"");

            //Delete file after 7 minutes 
            ThreadPool.QueueUserWorkItem(DeleteFile, fileName);

            return (("PrintDocument/" + Path.GetFileName(fileName)));
        }

        /// <summary>
        /// Delete file after 5 minutes 
        /// </summary>
        /// <param name="fileName"></param>
        private static void DeleteFile(object fileName)
        {
            Thread.Sleep(60000 * 7);
            System.IO.File.Delete((string)fileName);
        }
    }
}
