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
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmTReportKotBillInfo : System.Web.UI.Page
    {
        private int strPrinterInfoId = 0;
        private int strCostCenterId = 0;
        private string strCostCenterName = string.Empty;
        private string strPrinterName = string.Empty;
        private string waiterName = string.Empty;
        private string strCostCenterDefaultView = string.Empty;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportProcessingForTemplateOne();
            }
        }

        private void ReportProcessingForTemplateOne()
        {
            KotBillTemplate1.Visible = false;
            KotBillTemplate2.Visible = false;

            RestaurantTableDA tableDa = new RestaurantTableDA();
            KotBillDetailDA entityDA = new KotBillDetailDA();
            RestaurantTableBO tableBO = new RestaurantTableBO();

            PrinterInfoDA da = new PrinterInfoDA();
            HMCommonDA hmCommonDA = new HMCommonDA();

            int paxQuantity = 1;
            string sourceType = Request.QueryString["st"].ToString().Trim();
            string queryStringId = Request.QueryString["kotId"].ToString();
            string kotIdLst = Request.QueryString["kotIdLst"].ToString();
            int kotId = Convert.ToInt32(queryStringId);
            string tableId = Request.QueryString["tno"].ToString();
            string tableNumber = string.Empty;

            if (sourceType == "tbl")
            {
                tableBO = tableDa.GetRestaurantTableInfoByTableId(Convert.ToInt32(tableId));

                if (tableBO.TableId > 0)
                    tableNumber = tableBO.TableNumber;
            }
            else if (sourceType == "rom")
            {
                RoomRegistrationDA roomDa = new RoomRegistrationDA();
                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(Convert.ToInt32(tableId));

                tableNumber = roomRegistration.RoomNumber.ToString();
            }

            string IsKotBillRMaster = Request.QueryString["kbm"].ToString();
            bool IsReprint = false;

            if (Request.QueryString["isrp"] != null)
            {
                IsReprint = (Request.QueryString["isrp"].ToString()) == "rp" ? true : false;
            }

            Boolean status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(kotId);

            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
            List<PrinterInfoBO> files = new List<PrinterInfoBO>();

            if (IsReprint)
            {
                files = da.GetRestaurentItemTypeInfoByKotIdForReprint(kotId);
            }
            else
            {
                files = da.GetRestaurentItemTypeInfoByKotId(kotId);
            }

            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            waiterName = userInformationBO.UserName.ToString();

            if (!string.IsNullOrEmpty(queryStringId))
            {
                if (files.Count > 0)
                {
                    foreach (PrinterInfoBO pinfo in files)
                    {
                        strCostCenterId = pinfo.CostCenterId;
                        strPrinterInfoId = pinfo.PrinterInfoId;
                        strCostCenterId = pinfo.CostCenterId;
                        strCostCenterName = pinfo.CostCenter;

                        if (pinfo.DefaultView == "Table")
                        {
                            strCostCenterDefaultView = "Table # ";
                        }
                        else if (pinfo.DefaultView == "Token")
                        {
                            strCostCenterDefaultView = "Token # ";
                        }
                        else if (pinfo.DefaultView == "Room")
                        {
                            strCostCenterDefaultView = "Room # ";
                        }

                        if (pinfo.StockType == "StockItem")
                        {
                            KotBillTemplate1.Visible = true;
                            entityBOList = entityDA.GetKotOrderSubmitInfoForMultipleTable(kotIdLst, strCostCenterId, "StockItem", IsReprint);

                            if (entityBOList.Count > 0)
                            {
                                rvKotBill.LocalReport.DataSources.Clear();
                                rvKotBill.ProcessingMode = ProcessingMode.Local;

                                foreach (KotBillDetailBO row in entityBOList)
                                {
                                    if (row.PaxQuantity != 0)
                                    {
                                        paxQuantity = row.PaxQuantity;
                                    }
                                }

                                string reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptKotBill.rdlc");

                                if (!File.Exists(reportPath))
                                    return;

                                rvKotBill.LocalReport.ReportPath = reportPath;

                                string reportTitle = string.Empty;
                                reportTitle = "KOT";
                                if (kotId > 0)
                                {
                                    int updatedDataCount = 0;
                                    updatedDataCount = entityDA.GetKotUpdatedDataCountInfo(kotId);
                                    if (updatedDataCount > 0)
                                    {
                                        //reportTitle = "KOT (Updated)";
                                        reportTitle = "KOT";
                                    }
                                    else
                                    {
                                        reportTitle = "KOT";
                                    }
                                }

                                DateTime currentDate = DateTime.Now;
                                string kotDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();

                                if (IsReprint)
                                {
                                    reportTitle = "KOT (Reprint)";
                                    KotBillMasterBO kotBillMasterBO = new KotBillMasterBO();
                                    kotBillMasterBO = kotBillMasterDA.GetKotBillMasterInfoKotId(kotId);
                                    if (kotBillMasterBO.KotId > 0)
                                    {
                                        currentDate = kotBillMasterBO.KotDate;
                                        kotDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                                    }
                                }

                                KotBillMasterBO waiterInformationBO = new KotBillMasterBO();
                                waiterInformationBO = kotBillMasterDA.GetWaiterInformationByKotId(kotId);
                                if (waiterInformationBO != null)
                                {
                                    waiterName = waiterInformationBO.WaiterName;
                                }

                                ReportParameter[] parms = new ReportParameter[9];
                                parms[0] = new ReportParameter("ReportTitle", reportTitle);
                                parms[1] = new ReportParameter("CostCenter", strCostCenterName);
                                parms[2] = new ReportParameter("SourceName", strCostCenterDefaultView);
                                parms[3] = new ReportParameter("TableNo", tableNumber);
                                parms[4] = new ReportParameter("KotNo", kotId.ToString() + "   Pax : " + paxQuantity.ToString());
                                parms[5] = new ReportParameter("KotDate", kotDate);
                                parms[6] = new ReportParameter("WaiterName", waiterName);
                                parms[7] = new ReportParameter("SpecialRemarks", entityBOList[0].Remarks);
                                parms[8] = new ReportParameter("RestaurantName", pinfo.KitchenOrStockName);

                                rvKotBill.LocalReport.SetParameters(parms);

                                List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                                List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                                List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                                rvKotBill.LocalReport.DataSources.Add(new ReportDataSource("KotOrderSubmit", kotOrderSubmitEntityBOList));
                                rvKotBill.LocalReport.DataSources.Add(new ReportDataSource("ChangedOrEditedItem", changedOrEditedEntityBOList));
                                rvKotBill.LocalReport.DataSources.Add(new ReportDataSource("VoidOrDeletedItem", voidOrDeletedItemEntityBOList));

                                rvKotBill.LocalReport.Refresh();
                            }
                        }
                        else
                        {
                            KotBillTemplate2.Visible = true;
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "KitchenItem", IsReprint);

                            if (entityBOList.Count > 0)
                            {
                                rvKotBill2.LocalReport.DataSources.Clear();
                                rvKotBill2.ProcessingMode = ProcessingMode.Local;

                                foreach (KotBillDetailBO row in entityBOList)
                                {
                                    if (row.PaxQuantity != 0)
                                    {
                                        paxQuantity = row.PaxQuantity;
                                    }
                                }

                                string reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptKotBill.rdlc");

                                if (!File.Exists(reportPath))
                                    return;

                                rvKotBill2.LocalReport.ReportPath = reportPath;

                                string reportTitle = string.Empty;
                                reportTitle = "KOT";
                                if (kotId > 0)
                                {
                                    int updatedDataCount = 0;
                                    updatedDataCount = entityDA.GetKotUpdatedDataCountInfo(kotId);
                                    if (updatedDataCount > 0)
                                    {
                                        //reportTitle = "KOT (Updated)";
                                        reportTitle = "KOT";
                                    }
                                    else
                                    {
                                        reportTitle = "KOT";
                                    }
                                }

                                DateTime currentDate = DateTime.Now;
                                string kotDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();

                                if (IsReprint)
                                {
                                    reportTitle = "KOT (Reprint)";
                                    KotBillMasterBO kotBillMasterBO = new KotBillMasterBO();
                                    kotBillMasterBO = kotBillMasterDA.GetKotBillMasterInfoKotId(kotId);
                                    if (kotBillMasterBO.KotId > 0)
                                    {
                                        currentDate = kotBillMasterBO.KotDate;
                                        kotDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                                    }
                                }

                                KotBillMasterBO waiterInformationBO = new KotBillMasterBO();
                                waiterInformationBO = kotBillMasterDA.GetWaiterInformationByKotId(kotId);
                                if (waiterInformationBO != null)
                                {
                                    waiterName = waiterInformationBO.WaiterName;
                                }

                                ReportParameter[] parms = new ReportParameter[9];
                                parms[0] = new ReportParameter("ReportTitle", reportTitle);
                                parms[1] = new ReportParameter("CostCenter", strCostCenterName);
                                parms[2] = new ReportParameter("SourceName", strCostCenterDefaultView);
                                parms[3] = new ReportParameter("TableNo", tableNumber);
                                parms[4] = new ReportParameter("KotNo", kotId.ToString() + "   Pax : " + paxQuantity.ToString());
                                parms[5] = new ReportParameter("KotDate", kotDate);
                                parms[6] = new ReportParameter("WaiterName", waiterName);
                                parms[7] = new ReportParameter("SpecialRemarks", entityBOList[0].Remarks);
                                parms[8] = new ReportParameter("RestaurantName", pinfo.KitchenOrStockName);

                                rvKotBill2.LocalReport.SetParameters(parms);

                                List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                                List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                                List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                                rvKotBill2.LocalReport.DataSources.Add(new ReportDataSource("KotOrderSubmit", kotOrderSubmitEntityBOList));
                                rvKotBill2.LocalReport.DataSources.Add(new ReportDataSource("ChangedOrEditedItem", changedOrEditedEntityBOList));
                                rvKotBill2.LocalReport.DataSources.Add(new ReportDataSource("VoidOrDeletedItem", voidOrDeletedItemEntityBOList));

                                rvKotBill2.LocalReport.Refresh();
                            }
                        }
                    }
                }
                else
                {
                    //this.isMessageBoxEnable = 1;
                    //this.lblMessage.Text = "Please provide Item Information.";
                    //this.btnOrderSubmit.Focus();
                    //return;
                }
            }
        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>" + 3.5 + "in</PageWidth>" +
                "<PageHeight>" + 11 + "in</PageHeight>" +
                "<MarginTop>0.25in</MarginTop>" +
                "<MarginLeft>0.1in</MarginLeft>" +
                "<MarginRight>0.1in</MarginRight>" +
               " <MarginBottom>0.25in</MarginBottom>" +
            "</DeviceInfo>";

            byte[] bytes = rvKotBill.LocalReport.Render("PDF", deviceInfo, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileName = string.Empty, fileNamePrint = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            //Open exsisting pdf
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            Document document = new Document(new Rectangle(width, height), 0f, 0f, 0f, 0f);
            //Getting a instance of new pdf wrtiter
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

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

        protected void btn2PrintReportFromClient_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>" + 3.5 + "in</PageWidth>" +
               "<PageHeight>" + 11 + "in</PageHeight>" +
               "<MarginTop>0.25in</MarginTop>" +
               "<MarginLeft>0.1in</MarginLeft>" +
               "<MarginRight>0.1in</MarginRight>" +
              " <MarginBottom>0.25in</MarginBottom>" +
           "</DeviceInfo>";

            byte[] bytes = rvKotBill2.LocalReport.Render("PDF", deviceInfo, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileName = string.Empty, fileNamePrint = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            Document document = new Document(new Rectangle(width, height), 0f, 0f, 0f, 0f);

            //Getting a instance of new pdf wrtiter
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

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
            // PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
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