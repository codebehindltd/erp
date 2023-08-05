using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.IO;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportInHouseGuestInfo : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        protected int _IsDateInformationShow = 1;
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCurrentDate();
                this.LoadGuestCompany();
                this.LoadGuestGroupName();
                this.OthersWithoutArrivalInfoDiv.Visible = true;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string reportDisplayDate = string.Empty;

            DateTime currentDate = DateTime.Now;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (this.ddlReportType.SelectedValue == "0")
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please provide a Report Name.";
                this.ddlReportType.Focus();
                return;
            }
            DateTime reportDate = hmUtility.GetDateTimeFromString(startDate, userInformationBO.ServerDateFormat);
            string reportType = ddlReportType.SelectedValue;
            string guestCompany = ddlGuestCompany.SelectedValue;

            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat);
                startDate = txtFromDate.Text;
            }
            else
            {
                fromDate = reportDate;
                txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                startDate = txtFromDate.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, userInformationBO.ServerDateFormat);
                endDate = txtToDate.Text;
            }
            else
            {
                toDate = reportDate;
                txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                endDate = txtToDate.Text;
            }

            if (this.ddlReportType.SelectedValue.ToString() == "InHouseGustList")
            {
                _IsDateInformationShow = -1;
            }
            else
            {
                _IsDateInformationShow = 1;
            }

            _RoomStatusInfoByDate = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = string.Empty;

            if (ddlReportType.SelectedValue == "InHouseGustLedger")
            {
                reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptInHouseGuestLedger.rdlc");
            }
            else
            {
                if (ddlReportType.SelectedValue.ToString() == "InHouseGustList" && ddlOrderBy.SelectedValue == "Room")
                {
                    reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptInHouseGuestInfoOrderByRoom.rdlc");
                }
                else if (ddlReportType.SelectedValue.ToString() == "InHouseGustList" && ddlOrderBy.SelectedValue == "Company")
                {
                    reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptInHouseGuestInfoOrderByCompany.rdlc");
                }
                else
                {
                    reportDisplayDate = "Date From: " + txtFromDate.Text + " To: " + txtToDate.Text;
                    reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptInHouseGuestInfo.rdlc");
                }
            }

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

            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("ReportDisplayDate", reportDisplayDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            
            if (ddlReportType.SelectedValue == "InHouseGustLedger")
            {
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
            }
            else
            {
                List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
                GuestInformationDA guestInfoDa = new GuestInformationDA();

                if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
                {
                    fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat);
                    toDate = fromDate;
                }
                else
                {
                    fromDate = reportDate;
                    toDate = reportDate;
                    txtFromDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                    txtToDate.Text = hmUtility.GetStringFromDateTime(currentDate);
                }

                if (ddlGuestCompany.SelectedValue == "0")
                {
                    if (ddlOrderBy.SelectedValue == "Company")
                    {
                        guestHouseInfo = guestInfoDa.GetInHouseGuestInfoForReport(fromDate, toDate, reportType, guestCompany).OrderBy(x => x.GuestCompany).ToList();
                    }
                    else
                    {
                        guestHouseInfo = guestInfoDa.GetInHouseGuestInfoForReport(fromDate, toDate, reportType, guestCompany).OrderBy(x => x.RoomNumber).ToList();
                    }
                }
                else
                {
                    guestHouseInfo = guestInfoDa.GetInHouseGuestInfoForReport(fromDate, toDate, reportType, guestCompany);
                }

                if (!string.IsNullOrWhiteSpace(txtReferenceNumber.Text))
                {
                    guestHouseInfo = guestHouseInfo.Where(x => x.RRNumber.Trim().Contains(this.txtReferenceNumber.Text.Trim())).ToList();
                }

                var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], guestHouseInfo));
            }

            if (ddlReportType.SelectedValue == "InHouseGustLedger")
            {
                rvTransaction.LocalReport.DisplayName = "In House Guest Ledger";
            }
            else
            {
                rvTransaction.LocalReport.DisplayName = "In House Guest Report";
            }

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
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
        }
        public void LoadGuestCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetGuestCompanyInfo();
            ddlGuestCompany.DataSource = files;
            ddlGuestCompany.DataTextField = "CompanyName";
            ddlGuestCompany.DataValueField = "CompanyId";
            ddlGuestCompany.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlGuestCompany.Items.Insert(0, itemReference);
        }
        public void LoadGuestGroupName()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetGuestGroupNameInfo();
            ddlGroupName.DataSource = files;
            ddlGroupName.DataTextField = "GroupName";
            ddlGroupName.DataValueField = "GroupName";
            ddlGroupName.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlGroupName.Items.Insert(0, itemReference);
        }
    }
}