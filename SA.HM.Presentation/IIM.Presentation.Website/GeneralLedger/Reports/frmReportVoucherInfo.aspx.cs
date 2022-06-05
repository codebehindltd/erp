using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.GeneralLedger;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.UserInformation;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportVoucherInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        protected int printButtonFlag = 0;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string DealId = Request.QueryString["DealId"];
            if (!String.IsNullOrEmpty(DealId))
            {

                if (!IsPostBack)
                {
                    ReceiveVoucher.Visible = false;
                    PaymentVoucher.Visible = false;
                    JournalNContraVoucherDiv.Visible = true;
                    //this.LoadStatus(Int32.Parse(DealId));
                    this.SetSelected(Int32.Parse(DealId));
                    LoadReport(Int32.Parse(DealId));
                    this.txtReportId.Value = DealId;
                }

            }
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            int dealId = Int32.Parse(txtReportId.Value);
            string glStatus = ddlChangeStatus.SelectedItem.Text;
            NodeMatrixDA matrixDA = new NodeMatrixDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Boolean status = matrixDA.UpdateGLDealMasterStatus(dealId, glStatus, userInformationBO.UserInfoId);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Status Change Operation Successfull";
                this.SetSelected(dealId);
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
        //protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        //{
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //   userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //        Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;

            //    string deviceInfo =
            //      @"<DeviceInfo>
            //            <OutputFormat>EMF</OutputFormat>
            //            <PageWidth>" + 3.5 + "in</PageWidth>" +
            //        "<PageHeight>" + 11 + "in</PageHeight>" +
            //        "<MarginTop>0.25in</MarginTop>" +
            //        "<MarginLeft>0.1in</MarginLeft>" +
            //        "<MarginRight>0.1in</MarginRight>" +
            //       " <MarginBottom>0.25in</MarginBottom>" +
            //    "</DeviceInfo>";

            //    byte[] bytes = rvTransaction.LocalReport.Render("PDF", deviceInfo, out mimeType,
            //                   out encoding, out extension, out streamids, out warnings);

            //    string fileName = string.Empty, fileNamePrint = string.Empty;
            //    DateTime dateTime = DateTime.Now;
            //    fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            //        fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            //        FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            //    fs.Write(bytes, 0, bytes.Length);
            //        fs.Close();

            //        //Open exsisting pdf
            //        PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
            //    int i = 0;
            //    int p = 0;
            //    int n = reader.NumberOfPages;
            //    Rectangle psize = reader.GetPageSize(1);

            //    float width = psize.Width;
            //    float height = psize.Height;

            //    Document document = new Document(new Rectangle(width, height), 0f, 0f, 0f, 0f);
            //    //Getting a instance of new pdf wrtiter
            //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
            //       HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
            //    document.Open();
            //        PdfContentByte cb = writer.DirectContent;

            //        //Add Page to new document
            //        while (i<n)
            //        {
            //            document.NewPage();
            //            p++;
            //            i++;

            //            PdfImportedPage page1 = writer.GetImportedPage(reader, i);
            //    cb.AddTemplate(page1, 0, 0);
            //        }

            ////Attach javascript to the document
            //PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            ////PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
            //writer.AddJavaScript(jAction);

            //        document.Close();

            //        frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

            //        //writer.Close();
            //        //document.Close();
            //        //reader.Close();
            //        //bytes = null;
            //        //cb = null;
            //    }
        protected void btnReceivePrintReportFromClient_Click(object sender, EventArgs e)
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

                byte[] bytes = rvTransactionReceive.LocalReport.Render("PDF", null, out mimeType,
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
        //protected void btnReceivePrintReportFromClient_Click(object sender, EventArgs e)
        //{
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;

        //    string deviceInfo =
        //      @"<DeviceInfo>
        //        <OutputFormat>EMF</OutputFormat>
        //        <PageWidth>" + 3.5 + "in</PageWidth>" +
        //        "<PageHeight>" + 11 + "in</PageHeight>" +
        //        "<MarginTop>0.25in</MarginTop>" +
        //        "<MarginLeft>0.1in</MarginLeft>" +
        //        "<MarginRight>0.1in</MarginRight>" +
        //       " <MarginBottom>0.25in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    byte[] bytes = rvTransactionReceive.LocalReport.Render("PDF", deviceInfo, out mimeType,
        //                   out encoding, out extension, out streamids, out warnings);

        //    string fileName = string.Empty, fileNamePrint = string.Empty;
        //    DateTime dateTime = DateTime.Now;
        //    fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
        //    fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

        //    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();

        //    //Open exsisting pdf
        //    PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
        //    int i = 0;
        //    int p = 0;
        //    int n = reader.NumberOfPages;
        //    Rectangle psize = reader.GetPageSize(1);

        //    float width = psize.Width;
        //    float height = psize.Height;

        //    Document document = new Document(new Rectangle(width, height), 0f, 0f, 0f, 0f);
        //    //Getting a instance of new pdf wrtiter
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
        //       HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
        //    document.Open();
        //    PdfContentByte cb = writer.DirectContent;

        //    //Add Page to new document
        //    while (i < n)
        //    {
        //        document.NewPage();
        //        p++;
        //        i++;

        //        PdfImportedPage page1 = writer.GetImportedPage(reader, i);
        //        cb.AddTemplate(page1, 0, 0);
        //    }

        //    //Attach javascript to the document
        //    PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
        //    //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
        //    writer.AddJavaScript(jAction);

        //    document.Close();

        //    frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

        //    //writer.Close();
        //    //document.Close();
        //    //reader.Close();
        //    //bytes = null;
        //    //cb = null;
        //}
        protected void btnPaymentPrintReportFromClient_Click(object sender, EventArgs e)
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

                byte[] bytes = rvTransactionPayment.LocalReport.Render("PDF", null, out mimeType,
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
        //protected void btnPaymentPrintReportFromClient_Click(object sender, EventArgs e)
        //{
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;

        //    string deviceInfo =
        //      @"<DeviceInfo>
        //        <OutputFormat>EMF</OutputFormat>
        //        <PageWidth>" + 3.5 + "in</PageWidth>" +
        //        "<PageHeight>" + 11 + "in</PageHeight>" +
        //        "<MarginTop>0.25in</MarginTop>" +
        //        "<MarginLeft>0.1in</MarginLeft>" +
        //        "<MarginRight>0.1in</MarginRight>" +
        //       " <MarginBottom>0.25in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    byte[] bytes = rvTransactionPayment.LocalReport.Render("PDF", deviceInfo, out mimeType,
        //                   out encoding, out extension, out streamids, out warnings);

        //    string fileName = string.Empty, fileNamePrint = string.Empty;
        //    DateTime dateTime = DateTime.Now;
        //    fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
        //    fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

        //    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();

        //    //Open exsisting pdf
        //    PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
        //    int i = 0;
        //    int p = 0;
        //    int n = reader.NumberOfPages;
        //    Rectangle psize = reader.GetPageSize(1);

        //    float width = psize.Width;
        //    float height = psize.Height;

        //    Document document = new Document(new Rectangle(width, height), 0f, 0f, 0f, 0f);
        //    //Getting a instance of new pdf wrtiter
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
        //       HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
        //    document.Open();
        //    PdfContentByte cb = writer.DirectContent;

        //    //Add Page to new document
        //    while (i < n)
        //    {
        //        document.NewPage();
        //        p++;
        //        i++;

        //        PdfImportedPage page1 = writer.GetImportedPage(reader, i);
        //        cb.AddTemplate(page1, 0, 0);
        //    }

        //    //Attach javascript to the document
        //    PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
        //    //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
        //    writer.AddJavaScript(jAction);

        //    document.Close();

        //    frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

        //    //writer.Close();
        //    //document.Close();
        //    //reader.Close();
        //    //bytes = null;
        //    //cb = null;
        //}
        //************************ User Defined Function ********************//
        private void SetSelected(int DealId)
        {
            NodeMatrixDA matrixDA = new NodeMatrixDA();
            GLDealMasterBO masterBO = new GLDealMasterBO();
            masterBO = matrixDA.GetDealMasterInfoByDealId(DealId);
            if (masterBO.DealId > 0)
            {
                if (masterBO.GLStatus != null)
                {
                    this.ddlChangeStatus.SelectedValue = masterBO.GLStatus.ToString();
                    if (masterBO.GLStatus == "Approved")
                    {
                        this.btnChangeStatus.Visible = false;
                    }
                    else
                    {
                        this.btnChangeStatus.Visible = true;
                    }
                }
                else
                {
                    this.ddlChangeStatus.SelectedValue = "Pending";
                }
            }

        }
        private void LoadReport(int DealId)
        {
            GLDealMasterDA masterDA = new GLDealMasterDA();
            GLDealMasterBO masterBO = masterDA.GetVoucherInfoByDealId(DealId);

            if (masterBO != null)
            {
                if (masterBO.DealId > 0)
                {
                    if (Convert.ToInt32(masterBO.VoucherMode) == 1)
                    {
                        ReceiveVoucher.Visible = false;
                        PaymentVoucher.Visible = true;
                        printButtonFlag = 1;
                        JournalNContraVoucherDiv.Visible = false;
                    }
                    else if (Convert.ToInt32(masterBO.VoucherMode) == 2)
                    {
                        ReceiveVoucher.Visible = true;
                        printButtonFlag = 2;
                        PaymentVoucher.Visible = false;
                        JournalNContraVoucherDiv.Visible = false;
                    }
                    else if (Convert.ToInt32(masterBO.VoucherMode) == 3)
                    {
                        ReceiveVoucher.Visible = false;
                        PaymentVoucher.Visible = false;
                        JournalNContraVoucherDiv.Visible = true;
                    }
                    else if (Convert.ToInt32(masterBO.VoucherMode) == 4)
                    {
                        ReceiveVoucher.Visible = false;
                        PaymentVoucher.Visible = false;
                        JournalNContraVoucherDiv.Visible = true;
                    }
                }
            }
            else
            {
                ReceiveVoucher.Visible = false;
                PaymentVoucher.Visible = false;
                JournalNContraVoucherDiv.Visible = true;
            }
            
            HMCommonDA hmCommonDA = new HMCommonDA();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files != null)
            {
                if (files.Count > 0)
                {
                    this.txtCompanyName.Text = files[0].CompanyName;
                    this.txtCompanyAddress.Text = files[0].CompanyAddress;
                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        this.txtCompanyWeb.Text = files[0].WebAddress;
                    }
                    else
                    {
                        this.txtCompanyWeb.Text = files[0].ContactNumber;
                    }
                }
            }

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransactionReceive.LocalReport.EnableExternalImages = true;
            rvTransactionPayment.LocalReport.EnableExternalImages = true;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramLogo.Add(new ReportParameter("CreatedByName", masterBO.CreatedByName));
            paramLogo.Add(new ReportParameter("CheckedByName", masterBO.CheckedByName));
            paramLogo.Add(new ReportParameter("ApprovedByName", masterBO.ApprovedByName));

            rvTransaction.LocalReport.SetParameters(paramLogo);
            rvTransactionReceive.LocalReport.SetParameters(paramLogo);
            rvTransactionPayment.LocalReport.SetParameters(paramLogo);
            //-- Company Logo ------------------End----------

            this.txtDealId.Text = DealId.ToString();
            TransactionDataSource.SelectParameters[0].DefaultValue = this.txtDealId.Text;
            TransactionDataSource.SelectParameters[1].DefaultValue = this.txtCompanyName.Text;
            TransactionDataSource.SelectParameters[2].DefaultValue = this.txtCompanyAddress.Text;
            TransactionDataSource.SelectParameters[3].DefaultValue = this.txtCompanyWeb.Text;
            rvTransaction.LocalReport.DisplayName = "Voucher Information";
            rvTransaction.LocalReport.Refresh();

            TransactionDataSourceReceive.SelectParameters[0].DefaultValue = this.txtDealId.Text;
            TransactionDataSourceReceive.SelectParameters[1].DefaultValue = this.txtCompanyName.Text;
            TransactionDataSourceReceive.SelectParameters[2].DefaultValue = this.txtCompanyAddress.Text;
            TransactionDataSourceReceive.SelectParameters[3].DefaultValue = this.txtCompanyWeb.Text;
            rvTransactionReceive.LocalReport.DisplayName = "Receive Voucher Information";
            rvTransactionReceive.LocalReport.Refresh();

            TransactionDataSourcePayment.SelectParameters[0].DefaultValue = this.txtDealId.Text;
            TransactionDataSourcePayment.SelectParameters[1].DefaultValue = this.txtCompanyName.Text;
            TransactionDataSourcePayment.SelectParameters[2].DefaultValue = this.txtCompanyAddress.Text;
            TransactionDataSourcePayment.SelectParameters[3].DefaultValue = this.txtCompanyWeb.Text;
            rvTransactionPayment.LocalReport.DisplayName = "Payment Voucher Information";
            rvTransactionPayment.LocalReport.Refresh();
        }
    }
}