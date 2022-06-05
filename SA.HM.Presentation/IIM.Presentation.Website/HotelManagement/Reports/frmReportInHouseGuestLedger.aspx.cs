using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.IO;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportInHouseGuestLedger : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportInHouseGuestLedger.ToString());
                ProcessReport();
            }
        }
        public void ProcessReport()
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            string reportPath = string.Empty;
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptInHouseGuestLedger.rdlc");

            if (!File.Exists(reportPath))
            {
                return;
            }

            rvTransaction.LocalReport.ReportPath = reportPath;

            HMCommonDA hmCommonDA = new HMCommonDA();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            List<ReportParameter> reportParam = new List<ReportParameter>();
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo)); 
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(reportParam);

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<InhouseGuestLedgerBO> inhouseGuestLedgerBOList = new List<InhouseGuestLedgerBO>();
            inhouseGuestLedgerBOList = roomRegistrationDA.GetInHouseGuestLedgerForReport(txtCompanyName.Text, txtCompanyAddress.Text, txtCompanyWeb.Text);

            string RegistrationIdList = string.Empty;
            List<GuestLedgerTranscriptReportBO> ledgerTodaysFullTranscriptForBillSummary = new List<GuestLedgerTranscriptReportBO>();
            List<GuestLedgerTranscriptReportBO> ledgerTodaysTranscriptReportList = new List<GuestLedgerTranscriptReportBO>();
            foreach (InhouseGuestLedgerBO row in inhouseGuestLedgerBOList)
            {
                if (string.IsNullOrWhiteSpace(RegistrationIdList))
                {
                    RegistrationIdList = row.RegistrationId.ToString();
                }
                else
                {
                    RegistrationIdList = RegistrationIdList + "," + row.RegistrationId.ToString();
                }
            }
            
            HMCommonDA rHmCommonDA = new HMCommonDA();
            RegistrationIdList = rHmCommonDA.GetRegistrationIdList(RegistrationIdList);
            GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
            List<GuestHouseCheckOutDetailBO> guestRoomDetailBOList = new List<GuestHouseCheckOutDetailBO>();
            List<GuestHouseCheckOutDetailBO> guestServiceDetailBOList = new List<GuestHouseCheckOutDetailBO>();
            List<GuestHouseCheckOutDetailBO> guestAllRoomDetailBOList = da.GetGuestServiceInformationForCheckOut(RegistrationIdList, "GuestRoom", currentDate, userInformationBO.UserInfoId);
            foreach (GuestHouseCheckOutDetailBO guestRoomBo in guestAllRoomDetailBOList)
            {
                if (guestRoomBo.RegistrationId != guestRoomBo.BillPaidBy)
                {
                    guestRoomBo.RegistrationId = guestRoomBo.BillPaidBy;
                    guestRoomBo.RoomNumber = guestRoomBo.BillPaidByRoomNumber;
                    guestRoomDetailBOList.Add(guestRoomBo);
                }
                else
                {
                    guestRoomDetailBOList.Add(guestRoomBo);
                }
            }

            List<GuestHouseCheckOutDetailBO> guestAllServiceDetailBOList = da.GetGuestServiceInformationForCheckOut(RegistrationIdList, "GuestService", currentDate, userInformationBO.UserInfoId);
            foreach (GuestHouseCheckOutDetailBO guestServiceBo in guestAllServiceDetailBOList)
            {
                if (guestServiceBo.RegistrationId != guestServiceBo.BillPaidBy)
                {
                    guestServiceBo.RegistrationId = guestServiceBo.BillPaidBy;
                    guestServiceBo.RoomNumber = guestServiceBo.BillPaidByRoomNumber;
                    guestServiceDetailBOList.Add(guestServiceBo);
                }
                else
                {
                    guestServiceDetailBOList.Add(guestServiceBo);
                }
            }

            List<GuestHouseCheckOutDetailBO> guestOtherPaymentBOList = da.GetGuestOtherPaymentForBillByRegiIdList(RegistrationIdList);
            if (guestOtherPaymentBOList != null)
            {
                foreach (GuestHouseCheckOutDetailBO guestOtherRoomPaymentBo in guestOtherPaymentBOList)
                {
                    if (guestOtherRoomPaymentBo.RegistrationId != guestOtherRoomPaymentBo.BillPaidBy)
                    {
                        guestOtherRoomPaymentBo.RegistrationId = guestOtherRoomPaymentBo.BillPaidBy;
                    }
                }
            }

            List<GuestBillPaymentBO> guestBillPaymentBOList = new List<GuestBillPaymentBO>();
            GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();
            guestBillPaymentBOList = guestBillPaymentDA.GetGuestBillPaymentInfoByRegistrationIdList(RegistrationIdList);

            foreach (GuestBillPaymentBO paymentBOList in guestBillPaymentBOList)
            {
                if (paymentBOList.RegistrationId != paymentBOList.BillPaidBy)
                {
                    paymentBOList.RegistrationId = paymentBOList.BillPaidBy;
                }
            }

            // //------Guest Balance Amount -----------------------------
            List<GuestLedgerTranscriptReportBO> guestSummaryBOList = new List<GuestLedgerTranscriptReportBO>();
            foreach (InhouseGuestLedgerBO registrationBO in inhouseGuestLedgerBOList)
            {
                List<GuestHouseCheckOutDetailBO> roomRegistrationWiseList = new List<GuestHouseCheckOutDetailBO>();
                roomRegistrationWiseList = guestRoomDetailBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();               
                
                decimal roomBillAmount = roomRegistrationWiseList.Sum(item => item.TotalAmount);

                List<GuestHouseCheckOutDetailBO> serviceRegistrationWiseList = new List<GuestHouseCheckOutDetailBO>();
                serviceRegistrationWiseList = guestServiceDetailBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();
                decimal serviceBillAmount = serviceRegistrationWiseList.Sum(item => item.TotalAmount);

                List<GuestBillPaymentBO> paymentRegistrationWiseList = new List<GuestBillPaymentBO>();
                paymentRegistrationWiseList = guestBillPaymentBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();
                decimal paymentBillAmount = paymentRegistrationWiseList.Sum(item => item.PaymentAmount);

                registrationBO.Amount = roomBillAmount + serviceBillAmount;
                registrationBO.Credit = paymentBillAmount;
                registrationBO.Balance = Math.Round(roomBillAmount + serviceBillAmount + paymentBillAmount);
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], inhouseGuestLedgerBOList));

            rvTransaction.LocalReport.DisplayName = "In-House Guest Ledger Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission(int userId, string formName)
        {
            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userId, formName);
            if (objectPermissionBO.ObjectPermissionId > 0)
            {
                isViewPermission = objectPermissionBO.IsViewPermission;

                if (!isViewPermission)
                {
                    Response.Redirect("/HMCommon/frmHMReport.aspx");
                }
            }
            else
            {
                Response.Redirect("/HMCommon/frmHMReport.aspx");
            }
        }
    }
}