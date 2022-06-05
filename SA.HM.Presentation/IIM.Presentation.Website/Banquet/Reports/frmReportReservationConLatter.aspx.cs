using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.Banquet.Reports
{
    public partial class frmReportReservationConLatter : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryStringId = Request.QueryString["ReservationId"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    GenerateReport(Convert.ToInt32(queryStringId));
                }
            }
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        private void GenerateReport(int reservationId)
        {
            try
            {
                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.EnableExternalImages = true;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/RptReportReservationConLatter.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                List<ReportParameter> reportParam = new List<ReportParameter>();

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

                string footerPoweredByInfo = string.Empty;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);


                HMCommonDA hmCommonDA = new HMCommonDA();
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                reportParam.Add(new ReportParameter("AppreciationMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLAppreciationMessege")));
                reportParam.Add(new ReportParameter("FooterImagePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramFooterMiddleImagePath"))));
                reportParam.Add(new ReportParameter("CancellationPolicyMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLCancellationPolicyMessege")));
                reportParam.Add(new ReportParameter("AdditionalServiceMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLAdditionalServiceMessege")));
                reportParam.Add(new ReportParameter("RegardsMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramCLRegardsMessege")));
                reportParam.Add(new ReportParameter("FooterAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramCLFooterAddress")));
                reportParam.Add(new ReportParameter("IsBigSizeCompanyLogo", hmCommonDA.GetCustomFieldValueByFieldName("IsBigSizeCompanyLogo")));
                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                string banquetTermsAndConditions = hmCommonDA.GetCustomFieldValueByFieldName("BanquetTermsAndConditions");
                reportParam.Add(new ReportParameter("BanquetTermsAndConditions", banquetTermsAndConditions));

                BanquetReservationBO reservationBO = new BanquetReservationBO();
                BanquetReservationDA reservationDA = new BanquetReservationDA();
                reservationBO = reservationDA.GetBanquetReservationInfoById(reservationId);
                if (reservationBO != null)
                {
                    if (reservationBO.IsBillSettlement)
                    {
                        reportParam.Add(new ReportParameter("InvoicePaymentTotalTitle", "Payment Total"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("InvoicePaymentTotalTitle", "Advance Total"));
                    }

                    reportParam.Add(new ReportParameter("ReservationMode", reservationBO.ReservationMode));
                }
                else
                {
                    reportParam.Add(new ReportParameter("InvoicePaymentTotalTitle", "Payment Total"));
                }

                BanquetBillPaymentDA banquetDa = new BanquetBillPaymentDA();
                List<BanquetReservationBillGenerateReportBO> BanquetReservationBillAll = new List<BanquetReservationBillGenerateReportBO>();
                List<BanquetReservationBillGenerateReportBO> BanquetReservationBill = new List<BanquetReservationBillGenerateReportBO>();
                BanquetReservationBillAll = banquetDa.GetBanquetReservationBillGenerateReport(reservationId);
                BanquetReservationBill = BanquetReservationBillAll.Where(x => x.ItemType != "Payments").ToList();

                decimal paymentAmount = 0;
                paymentAmount = BanquetReservationBillAll.Where(x => x.ItemType == "Payments").ToList().Sum(y => y.TotalAmount);

                reportParam.Add(new ReportParameter("InvoicePaymentTotal", paymentAmount.ToString()));

                foreach (BanquetReservationBillGenerateReportBO row in BanquetReservationBill)
                {
                    reportParam.Add(new ReportParameter("InnboardVatAmount", row.InnboardVatAmount));
                    reportParam.Add(new ReportParameter("InnboardServiceChargeAmount", row.InnboardServiceChargeAmount));
                }
                rvTransaction.LocalReport.SetParameters(reportParam);
                var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], BanquetReservationBill));
                rvTransaction.LocalReport.DisplayName = "Banquet Confirmation Letter";
                rvTransaction.LocalReport.Refresh();
                bool isPreview = Convert.ToBoolean(Request.QueryString["isPreview"]);
                if (!isPreview)
                    SendMail(reservationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            frmPrint.Attributes["src"] = "";
        }
        private bool SendMail(int reservationId)
        {
            bool status = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsBanquetReservationEmailAutoPostingEnable");
            string IsEnable = commonSetupBO.SetupValue;
            if (IsEnable == "0")
                return status;

            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();

            reservationBO = reservationDA.GetBanquetReservationInfoById(reservationId);
            string ReceivingMail = reservationBO.ContactEmail;
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
            fileName = "GR" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            filePath = HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName);
            if (!File.Exists(filePath))
                return false;

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
                    Subject = "Confirmation for Banquet Reservation.",
                    Body = MailBody,
                    AttachmentSavedPath = filePath,
                    Host = dataArray[2],
                    Port = dataArray[3]
                };

                try
                {
                    //Send Mail   
                    status = EmailHelper.SendEmail(email, null);
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