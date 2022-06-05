using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.Data;
using HotelManagement.Entity;
using HotelManagement.Data;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.IO;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestLedgerTranscript : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        HiddenField innboardMessage;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
        }
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (!IsValidForm())
            {
                return;
            }

            ddlRoomStatus.Value = "0";
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            this.showReport();
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());
            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        public bool IsValidForm()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Ledger Date.", AlertType.Warning);
                this.txtFromDate.Focus();
                status = false;
            }
            return status;
        }
        private void showReport()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DateTime guestLedgerDateInfo = hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat);
            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                string reportPath = string.Empty;
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestLedgerTranscript.rdlc");

                if (!File.Exists(reportPath))
                {
                    return;
                }

                rvTransaction.LocalReport.ReportPath = reportPath;

                ddlRoomStatus.Value = "0";
                _RoomStatusInfoByDate = 1;

                HMCommonDA hmCommonDA = new HMCommonDA();
                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

                rvTransaction.LocalReport.EnableExternalImages = true;

                DateTime currentDate = guestLedgerDateInfo;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                string footerPoweredByInfo = string.Empty;
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
                string reportDate = this.txtFromDate.Text;

                List<ReportParameter> reportParam = new List<ReportParameter>();
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("ReportDate", reportDate));

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
                rvTransaction.LocalReport.SetParameters(reportParam);

                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                List<RoomRegistrationBO> roomRegistrationBOList = new List<RoomRegistrationBO>();
                roomRegistrationBOList = roomRegistrationDA.GetActiveRoomRegistrationInfoByTransactionDate(guestLedgerDateInfo);

                List<RoomRegistrationBO> checkOutRoomRegistrationBOList = new List<RoomRegistrationBO>();
                checkOutRoomRegistrationBOList = roomRegistrationDA.GetCheckOutRoomRegistrationInfoByCheckOutDate(guestLedgerDateInfo);

                if (checkOutRoomRegistrationBOList != null)
                {
                    if (checkOutRoomRegistrationBOList.Count > 0)
                    {
                        foreach (RoomRegistrationBO row in checkOutRoomRegistrationBOList)
                        {
                            var v = (from m in roomRegistrationBOList
                                     where m.RegistrationId == row.RegistrationId
                                     select m).FirstOrDefault();

                            if (v != null)
                            {
                                roomRegistrationBOList.Remove(v);
                            }
                        }
                        roomRegistrationBOList.AddRange(checkOutRoomRegistrationBOList);
                    }
                }

                string RegistrationIdList = string.Empty;
                List<GuestLedgerTranscriptReportBO> ledgerTodaysFullTranscriptForBillSummary = new List<GuestLedgerTranscriptReportBO>();
                List<GuestLedgerTranscriptReportBO> ledgerRoomTransferedBillInformationBO = new List<GuestLedgerTranscriptReportBO>();
                List<GuestLedgerTranscriptReportBO> ledgerTodaysTranscriptReportList = new List<GuestLedgerTranscriptReportBO>();
                foreach (RoomRegistrationBO row in roomRegistrationBOList)
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

                pnlRoomCalender.Visible = true;
                if (string.IsNullOrWhiteSpace(RegistrationIdList))
                {
                    pnlRoomCalender.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, "No of Occupency Zero.", AlertType.Warning);
                    return;
                }

                List<GuestHouseCheckOutDetailBO> guestRoomDetailBOList = new List<GuestHouseCheckOutDetailBO>();
                List<GuestHouseCheckOutDetailBO> guestServiceDetailBOList = new List<GuestHouseCheckOutDetailBO>();

                HMCommonDA rHmCommonDA = new HMCommonDA();
                RegistrationIdList = rHmCommonDA.GetRegistrationIdList(RegistrationIdList);
                GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
                List<GuestHouseCheckOutDetailBO> guestAllRoomDetailBOList = da.GetGuestServiceInformationForCheckOut(RegistrationIdList, "GuestRoom", guestLedgerDateInfo, userInformationBO.UserInfoId);

                foreach (GuestHouseCheckOutDetailBO guestRoomBo in guestAllRoomDetailBOList)
                {
                    guestRoomDetailBOList.Add(guestRoomBo);
                }

                List<GuestHouseCheckOutDetailBO> guestAllServiceDetailBOList = da.GetGuestServiceInformationForCheckOut(RegistrationIdList, "GuestService", guestLedgerDateInfo, userInformationBO.UserInfoId);
                foreach (GuestHouseCheckOutDetailBO guestServiceBo in guestAllServiceDetailBOList)
                {
                    if (guestServiceBo.ServiceType == "RestaurantService")
                    {
                        guestServiceBo.ServiceId = 4000;
                        guestServiceBo.ServiceName = guestServiceBo.CostCenter;
                    }
                    else if (guestServiceBo.ServiceType == "BanquetService")
                    {
                        guestServiceBo.ServiceId = 4001;
                        guestServiceBo.ServiceName = guestServiceBo.CostCenter;
                    }

                    guestServiceDetailBOList.Add(guestServiceBo);
                }

                List<GuestHouseCheckOutDetailBO> guestOtherPaymentBOList = da.GetGuestOtherPaymentForBillByRegiIdList(RegistrationIdList);
                List<GuestHouseCheckOutDetailBO> guestOtherPaymentBOListForBalance = guestOtherPaymentBOList;

                List<GuestBillPaymentBO> guestAllBillPaymentBOList = new List<GuestBillPaymentBO>();
                GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();
                guestAllBillPaymentBOList = guestBillPaymentDA.GetGuestAllBillPaymentInfoByRegistrationIdList(RegistrationIdList).Where(x => x.PaymentDate.Date < (guestLedgerDateInfo.Date.AddDays(1))).ToList();

                List<GuestBillPaymentBO> guestWithoutTransferedBillPaymentBOList = new List<GuestBillPaymentBO>();
                guestWithoutTransferedBillPaymentBOList = guestAllBillPaymentBOList.Where(x => x.PaymentType != "Other Room").ToList();

                List<GuestBillPaymentBO> guestOnlyOtherRoomTransferedBillPaymentBOList = new List<GuestBillPaymentBO>();
                guestOnlyOtherRoomTransferedBillPaymentBOList = guestAllBillPaymentBOList.Where(x => x.PaymentType == "Other Room" && x.ModuleName != "Banquet").ToList();


                List<GuestBillPaymentBO> guestRebateInfoBOList = new List<GuestBillPaymentBO>();
                guestRebateInfoBOList = guestBillPaymentDA.GetGuestRebateInformationByRegistrationIdList(RegistrationIdList).Where(x => x.PaymentDate.Date < (guestLedgerDateInfo.Date.AddDays(1))).ToList();

                #region Due Room Transaction
                List<GuestLedgerTranscriptReportBO> ledgerDueRoomTranscriptForBillSummary = new List<GuestLedgerTranscriptReportBO>();

                // //--------Due Room Transaction -----------------------------
                List<GuestHouseCheckOutDetailBO> guestDueRoomDetailBOList = guestRoomDetailBOList.Where(m => m.ServiceDate.Date != guestLedgerDateInfo.Date).ToList();
                foreach (GuestHouseCheckOutDetailBO row in guestDueRoomDetailBOList)
                {
                    GuestLedgerTranscriptReportBO ledgerDueRoomTranscriptReportBO = new GuestLedgerTranscriptReportBO();
                    if (row.BillPaidBy == 0)
                    {
                        row.BillPaidBy = row.RegistrationId;
                        ledgerDueRoomTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerDueRoomTranscriptReportBO.Departure = guestLedgerDateInfo;
                    }
                    else
                    {
                        ledgerDueRoomTranscriptReportBO.BillPaidBy = row.BillPaidBy;
                        ledgerDueRoomTranscriptReportBO.Araival = guestLedgerDateInfo.AddDays(-1);
                        ledgerDueRoomTranscriptReportBO.Departure = guestLedgerDateInfo.AddDays(-1);
                    }

                    ledgerDueRoomTranscriptReportBO.RegistrationId = row.RegistrationId;
                    ledgerDueRoomTranscriptReportBO.RoomNumber = row.RoomNumber;
                    ledgerDueRoomTranscriptReportBO.GuestName = row.GuestName;
                    ledgerDueRoomTranscriptReportBO.CompanyName = "";
                    ledgerDueRoomTranscriptReportBO.Pay = "";
                    ledgerDueRoomTranscriptReportBO.ServiceId = 0;
                    ledgerDueRoomTranscriptReportBO.ServiceName = "Room Rent";
                    ledgerDueRoomTranscriptReportBO.ServiceAmount = row.TotalAmount;
                    ledgerDueRoomTranscriptReportBO.ServiceDate = row.ServiceDate;
                    ledgerDueRoomTranscriptReportBO.CheckOutDate = row.CheckOutDate;

                    foreach (GuestBillPaymentBO rebateRow in guestRebateInfoBOList)
                    {
                        if (row.RegistrationId == rebateRow.RegistrationId)
                        {
                            //ledgerDueRoomTranscriptReportBO.ServiceAmount = Math.Round(ledgerDueRoomTranscriptReportBO.ServiceAmount) - Math.Round(rebateRow.PaymentAmount);
                            if (rebateRow.PaymentDate.Date == row.ServiceDate.Date)
                            {
                                ledgerDueRoomTranscriptReportBO.ServiceAmount = ledgerDueRoomTranscriptReportBO.ServiceAmount - rebateRow.PaymentAmount;
                            }
                        }
                    }

                    ledgerDueRoomTranscriptForBillSummary.Add(ledgerDueRoomTranscriptReportBO);
                }

                List<GuestLedgerTranscriptReportBO> ledgerDueServiceTranscriptForBillSummary = new List<GuestLedgerTranscriptReportBO>();
                // //--------Due Service Transaction -----------------------------
                List<GuestHouseCheckOutDetailBO> guestDueServiceDetailBOList = guestServiceDetailBOList.Where(m => m.ServiceDate.Date != guestLedgerDateInfo.Date).ToList();
                foreach (GuestHouseCheckOutDetailBO row in guestDueServiceDetailBOList)
                {
                    decimal roomTransferedBillSummaryBOListAmount = 0;
                    GuestLedgerTranscriptReportBO ledgerDueServiceTranscriptReportBO = new GuestLedgerTranscriptReportBO();

                    if (row.BillPaidBy == 0)
                    {
                        row.BillPaidBy = row.RegistrationId;
                        ledgerDueServiceTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerDueServiceTranscriptReportBO.Departure = guestLedgerDateInfo;
                    }
                    else
                    {
                        ledgerDueServiceTranscriptReportBO.BillPaidBy = row.BillPaidBy;
                        ledgerDueServiceTranscriptReportBO.Araival = guestLedgerDateInfo.AddDays(-1);
                        ledgerDueServiceTranscriptReportBO.Departure = guestLedgerDateInfo.AddDays(-1);
                    }

                    ledgerDueServiceTranscriptReportBO.RegistrationId = row.RegistrationId;
                    ledgerDueServiceTranscriptReportBO.RoomNumber = row.RoomNumber;
                    ledgerDueServiceTranscriptReportBO.GuestName = row.GuestName;
                    ledgerDueServiceTranscriptReportBO.CompanyName = "";
                    ledgerDueServiceTranscriptReportBO.Pay = "";
                    ledgerDueServiceTranscriptReportBO.ServiceName = row.ServiceName;
                    ledgerDueServiceTranscriptReportBO.ServiceId = row.ServiceId;
                    ledgerDueServiceTranscriptReportBO.ServiceAmount = row.TotalAmount + roomTransferedBillSummaryBOListAmount;
                    ledgerDueServiceTranscriptReportBO.ServiceDate = row.ServiceDate;
                    ledgerDueServiceTranscriptReportBO.CheckOutDate = row.CheckOutDate;
                    ledgerDueServiceTranscriptForBillSummary.Add(ledgerDueServiceTranscriptReportBO);
                }

                List<GuestLedgerTranscriptReportBO> ledgerDuePaymentTranscriptForBillSummary = new List<GuestLedgerTranscriptReportBO>();
                // //--------Due Service Transaction -----------------------------
                List<GuestBillPaymentBO> guestDuePaymentDetailBOList = guestWithoutTransferedBillPaymentBOList.Where(m => hmUtility.GetDateTimeFromString(m.PaymentDate.ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date != guestLedgerDateInfo.Date).ToList();
                foreach (GuestBillPaymentBO row in guestDuePaymentDetailBOList)
                {
                    GuestLedgerTranscriptReportBO ledgerDuePaymentTranscriptReportBO = new GuestLedgerTranscriptReportBO();

                    if (row.BillPaidBy == 0)
                    {
                        row.BillPaidBy = row.RegistrationId;
                        ledgerDuePaymentTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerDuePaymentTranscriptReportBO.Departure = guestLedgerDateInfo;
                    }
                    else
                    {
                        ledgerDuePaymentTranscriptReportBO.BillPaidBy = row.BillPaidBy;
                        ledgerDuePaymentTranscriptReportBO.Araival = guestLedgerDateInfo.AddDays(-1);
                        ledgerDuePaymentTranscriptReportBO.Departure = guestLedgerDateInfo.AddDays(-1);

                        //if (row.CheckOutDate != null)
                        //{
                        //    ledgerDuePaymentTranscriptReportBO.CheckOutDate = Convert.ToDateTime(row.CheckOutDate).AddDays(-1);
                        //}
                    }

                    ledgerDuePaymentTranscriptReportBO.RegistrationId = row.RegistrationId;
                    ledgerDuePaymentTranscriptReportBO.RoomNumber = row.RoomNumber;
                    ledgerDuePaymentTranscriptReportBO.GuestName = "";
                    ledgerDuePaymentTranscriptReportBO.CompanyName = "";
                    ledgerDuePaymentTranscriptReportBO.Pay = "";
                    ledgerDuePaymentTranscriptReportBO.ServiceName = row.PaymentType;
                    ledgerDuePaymentTranscriptReportBO.ServiceId = row.PaymentId;
                    ledgerDuePaymentTranscriptReportBO.ServiceAmount = Convert.ToDecimal(row.PaymentAmount);
                    ledgerDuePaymentTranscriptReportBO.ServiceDate = row.PaymentDate;
                    ledgerDuePaymentTranscriptReportBO.CheckOutDate = row.CheckOutDate;
                    ledgerDuePaymentTranscriptForBillSummary.Add(ledgerDuePaymentTranscriptReportBO);
                }
                #endregion
                #region Today's Room Transaction
                // //--------Today's Room Transaction -----------------------------
                List<GuestHouseCheckOutDetailBO> guestTodaysRoomDetailBOList = guestRoomDetailBOList.Where(m => m.ServiceDate.Date == guestLedgerDateInfo.Date).ToList();
                foreach (GuestHouseCheckOutDetailBO row in guestTodaysRoomDetailBOList)
                {
                    GuestLedgerTranscriptReportBO ledgerTranscriptReportBO = new GuestLedgerTranscriptReportBO();
                    if (row.RegistrationId != row.BillPaidBy)
                    {
                        GuestLedgerTranscriptReportBO ledgerTranscriptReportBOForRoomTransfered = new GuestLedgerTranscriptReportBO();
                        ledgerTranscriptReportBOForRoomTransfered.RoomTransferedTotalAmount = row.TotalAmount;
                        ledgerTranscriptReportBOForRoomTransfered.RegistrationId = row.BillPaidBy;
                        ledgerTranscriptReportBOForRoomTransfered.RoomNumber = row.BillPaidByRoomNumber;
                        ledgerTranscriptReportBOForRoomTransfered.GuestName = row.GuestName;
                        ledgerTranscriptReportBOForRoomTransfered.CompanyName = "";
                        ledgerTranscriptReportBOForRoomTransfered.Pay = "";
                        ledgerTranscriptReportBOForRoomTransfered.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBOForRoomTransfered.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBOForRoomTransfered.ServiceId = 4999;
                        ledgerTranscriptReportBOForRoomTransfered.ServiceName = "Bill Transfered";
                        ledgerTranscriptReportBOForRoomTransfered.ServiceAmount = row.TotalAmount;
                        ledgerTranscriptReportBOForRoomTransfered.ServiceDate = guestLedgerDateInfo;
                        ledgerRoomTransferedBillInformationBO.Add(ledgerTranscriptReportBOForRoomTransfered);
                        ledgerTranscriptReportBO.RegistrationId = row.RegistrationId;
                        ledgerTranscriptReportBO.RoomNumber = row.RoomNumber;
                        ledgerTranscriptReportBO.GuestName = row.GuestName;
                        ledgerTranscriptReportBO.CompanyName = "";
                        ledgerTranscriptReportBO.Pay = "";
                        ledgerTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.ServiceId = 0;
                        ledgerTranscriptReportBO.ServiceName = "Room Rent";
                        ledgerTranscriptReportBO.ServiceAmount = row.TotalAmount;
                        ledgerTranscriptReportBO.ServiceDate = guestLedgerDateInfo;
                    }
                    else
                    {
                        ledgerTranscriptReportBO.RegistrationId = row.RegistrationId;
                        ledgerTranscriptReportBO.RoomNumber = row.RoomNumber;
                        ledgerTranscriptReportBO.GuestName = row.GuestName;
                        ledgerTranscriptReportBO.CompanyName = "";
                        ledgerTranscriptReportBO.Pay = "";
                        ledgerTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.ServiceId = 0;
                        ledgerTranscriptReportBO.ServiceName = "Room Rent";
                        ledgerTranscriptReportBO.ServiceAmount = row.TotalAmount;
                        ledgerTranscriptReportBO.ServiceDate = guestLedgerDateInfo;
                    }

                    foreach (GuestBillPaymentBO rebateRow in guestRebateInfoBOList)
                    {
                        if (row.RegistrationId == rebateRow.RegistrationId)
                        {
                            //ledgerTranscriptReportBO.ServiceAmount = Math.Round(ledgerTranscriptReportBO.ServiceAmount) - Math.Round(rebateRow.PaymentAmount);
                            if (rebateRow.PaymentDate.Date == row.ServiceDate.Date)
                            {
                                ledgerTranscriptReportBO.ServiceAmount = ledgerTranscriptReportBO.ServiceAmount - rebateRow.PaymentAmount;
                            }
                        }
                    }

                    ledgerTodaysTranscriptReportList.Add(ledgerTranscriptReportBO);
                    ledgerTodaysFullTranscriptForBillSummary.Add(ledgerTranscriptReportBO);
                }
                #endregion
                #region Today's Service Transaction
                // //--------Today's Service Transaction -----------------------------
                List<GuestHouseCheckOutDetailBO> guestTodaysServiceDetailBOList = guestServiceDetailBOList.Where(m => m.ServiceDate.Date == guestLedgerDateInfo.Date).ToList();
                foreach (GuestHouseCheckOutDetailBO row in guestTodaysServiceDetailBOList)
                {
                    GuestLedgerTranscriptReportBO ledgerTranscriptReportBO = new GuestLedgerTranscriptReportBO();

                    if (row.RegistrationId != row.BillPaidBy)
                    {
                        GuestLedgerTranscriptReportBO ledgerTranscriptReportBOForRoomTransfered = new GuestLedgerTranscriptReportBO();
                        ledgerTranscriptReportBOForRoomTransfered.RoomTransferedTotalAmount = row.TotalAmount;
                        ledgerTranscriptReportBOForRoomTransfered.RegistrationId = row.BillPaidBy;
                        ledgerTranscriptReportBOForRoomTransfered.RoomNumber = row.BillPaidByRoomNumber;
                        ledgerTranscriptReportBOForRoomTransfered.GuestName = row.GuestName;
                        ledgerTranscriptReportBOForRoomTransfered.CompanyName = "";
                        ledgerTranscriptReportBOForRoomTransfered.Pay = "";
                        ledgerTranscriptReportBOForRoomTransfered.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBOForRoomTransfered.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBOForRoomTransfered.ServiceId = 4999;
                        ledgerTranscriptReportBOForRoomTransfered.ServiceName = "Bill Transfered";
                        ledgerTranscriptReportBOForRoomTransfered.ServiceAmount = row.TotalAmount;
                        ledgerTranscriptReportBOForRoomTransfered.ServiceDate = guestLedgerDateInfo;
                        ledgerRoomTransferedBillInformationBO.Add(ledgerTranscriptReportBOForRoomTransfered);
                        ledgerTranscriptReportBO.RegistrationId = row.RegistrationId;
                        ledgerTranscriptReportBO.RoomNumber = row.RoomNumber;
                        ledgerTranscriptReportBO.GuestName = row.GuestName;
                        ledgerTranscriptReportBO.CompanyName = "";
                        ledgerTranscriptReportBO.Pay = "";
                        ledgerTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.ServiceName = row.ServiceName;
                        ledgerTranscriptReportBO.ServiceId = row.ServiceId;
                        ledgerTranscriptReportBO.ServiceAmount = row.TotalAmount;
                        ledgerTranscriptReportBO.ServiceDate = guestLedgerDateInfo;
                    }
                    else
                    {
                        ledgerTranscriptReportBO.RegistrationId = row.RegistrationId;
                        ledgerTranscriptReportBO.RoomNumber = row.RoomNumber;
                        ledgerTranscriptReportBO.GuestName = row.GuestName;
                        ledgerTranscriptReportBO.CompanyName = "";
                        ledgerTranscriptReportBO.Pay = "";
                        ledgerTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.ServiceName = row.ServiceName;
                        ledgerTranscriptReportBO.ServiceId = row.ServiceId;
                        ledgerTranscriptReportBO.ServiceAmount = row.TotalAmount;
                        ledgerTranscriptReportBO.ServiceDate = guestLedgerDateInfo;
                    }

                    ledgerTodaysTranscriptReportList.Add(ledgerTranscriptReportBO);
                    ledgerTodaysFullTranscriptForBillSummary.Add(ledgerTranscriptReportBO);
                }
                #endregion
                #region Today's Payment Transaction
                // //--------Today's Payment Transaction -----------------------------
                List<GuestBillPaymentBO> guestTodaysPaymentDetailBOList = guestWithoutTransferedBillPaymentBOList.Where(m => m.PaymentDate.Date == guestLedgerDateInfo.Date).ToList();
                foreach (GuestBillPaymentBO row in guestTodaysPaymentDetailBOList)
                {
                    GuestLedgerTranscriptReportBO todaysPayment2BO = new GuestLedgerTranscriptReportBO();
                    todaysPayment2BO.RegistrationId = row.RegistrationId;
                    todaysPayment2BO.RoomNumber = row.RoomNumber;
                    todaysPayment2BO.GuestName = "";
                    todaysPayment2BO.CompanyName = "";
                    todaysPayment2BO.Pay = "";
                    todaysPayment2BO.Araival = guestLedgerDateInfo;
                    todaysPayment2BO.Departure = guestLedgerDateInfo;
                    todaysPayment2BO.ServiceName = "Advance/ Cash Out";
                    todaysPayment2BO.ServiceId = 5000;
                    todaysPayment2BO.ServiceAmount = (-1) * (row.PaymentAmount);
                    todaysPayment2BO.ServiceDate = guestLedgerDateInfo;
                    ledgerTodaysFullTranscriptForBillSummary.Add(todaysPayment2BO);
                }
                #endregion
                #region All Service Information
                //-- All Service Information ----------------------------
                GuestHouseServiceDA guestHouseServiceDA = new GuestHouseServiceDA();
                List<GuestHouseServiceBO> serviceBOList = new List<GuestHouseServiceBO>();
                serviceBOList = guestHouseServiceDA.GetActiveGuestHouseServiceInfo();

                List<GuestHouseServiceBO> forAllServiceBOList = serviceBOList.Where(n => !ledgerTodaysTranscriptReportList.Select(n1 => n1.ServiceId).Contains(n.ServiceId)).ToList();
                foreach (RoomRegistrationBO registrationBO in roomRegistrationBOList)
                {
                    // //--------Other Service Transaction -----------------------------
                    foreach (GuestHouseServiceBO row in forAllServiceBOList)
                    {
                        GuestLedgerTranscriptReportBO ledgerTranscriptReportBO = new GuestLedgerTranscriptReportBO();
                        ledgerTranscriptReportBO.RegistrationId = registrationBO.RegistrationId;
                        ledgerTranscriptReportBO.RoomNumber = registrationBO.RoomNumber;
                        ledgerTranscriptReportBO.GuestName = "";
                        ledgerTranscriptReportBO.CompanyName = "";
                        ledgerTranscriptReportBO.Pay = "";
                        ledgerTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.ServiceName = row.ServiceName;
                        ledgerTranscriptReportBO.ServiceId = row.ServiceId;
                        ledgerTranscriptReportBO.ServiceAmount = 0;
                        ledgerTranscriptReportBO.ServiceDate = guestLedgerDateInfo;

                        ledgerTodaysTranscriptReportList.Add(ledgerTranscriptReportBO);
                        ledgerTodaysFullTranscriptForBillSummary.Add(ledgerTranscriptReportBO);
                    }
                }
                #endregion
                #region Last Day Balance
                // //------Last Day Balance -----------------------------
                // //-------------------------// //------Due Bill Summary -----------------------------
                List<GuestLedgerTranscriptReportBO> previousBillAmountSummaryBOList = new List<GuestLedgerTranscriptReportBO>();
                foreach (RoomRegistrationBO registrationBO in roomRegistrationBOList)
                {
                    //if (registrationBO.RegistrationId == 9434)
                    //{
                    //    string m = string.Empty;
                    //}


                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    roomRegistrationWiseList = ledgerDueRoomTranscriptForBillSummary.Where(x => x.RegistrationId == registrationBO.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date).ToList();
                    decimal roomBillAmount = roomRegistrationWiseList.Sum(item => item.ServiceAmount);

                    List<GuestLedgerTranscriptReportBO> serviceRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    serviceRegistrationWiseList = ledgerDueServiceTranscriptForBillSummary.Where(x => x.RegistrationId == registrationBO.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date).ToList();
                    decimal serviceBillAmount = serviceRegistrationWiseList.Sum(item => item.ServiceAmount);

                    List<GuestLedgerTranscriptReportBO> paymentRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    paymentRegistrationWiseList = ledgerDuePaymentTranscriptForBillSummary.Where(x => x.RegistrationId == registrationBO.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date).ToList();
                    decimal paymentBillAmount = paymentRegistrationWiseList.Sum(item => item.ServiceAmount);

                    List<string> registrationIdArrayList = new List<string>();
                    registrationIdArrayList = hmCommonDA.GetRegistrationIdListWithoutCDateCheckOut(registrationBO.RegistrationId.ToString(), guestLedgerDateInfo).Split(',').ToList();
                    List<RoomRegistrationBO> OtherRoomRegistrationBOListInfo = new List<RoomRegistrationBO>();
                    foreach (string strRow in registrationIdArrayList)
                    {
                        RoomRegistrationBO OtherRoomRegistrationBOInfo = new RoomRegistrationBO();
                        OtherRoomRegistrationBOInfo.RegistrationId = Convert.ToInt32(strRow);
                        OtherRoomRegistrationBOListInfo.Add(OtherRoomRegistrationBOInfo);
                    }

                    decimal otherRoomBillAmount = 0;
                    decimal otherServiceBillAmount = 0;
                    decimal otherPaymentBillAmount = 0;

                    if (OtherRoomRegistrationBOListInfo != null)
                    {
                        if (OtherRoomRegistrationBOListInfo.Count > 0)
                        {
                            foreach (RoomRegistrationBO rrRow in OtherRoomRegistrationBOListInfo)
                            {
                                List<GuestLedgerTranscriptReportBO> otherRoomRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                                otherRoomRegistrationWiseList = ledgerDueRoomTranscriptForBillSummary.Where(x => x.RegistrationId != rrRow.RegistrationId && x.BillPaidBy == rrRow.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date
                                    &&
                                    (
                                        x.CheckOutDate == null ||
                                        (Convert.ToDateTime(x.CheckOutDate).Date < guestLedgerDateInfo.Date)
                                    )
                                    ).ToList();
                                otherRoomBillAmount = otherRoomBillAmount + otherRoomRegistrationWiseList.Sum(item => item.ServiceAmount);


                                List<GuestLedgerTranscriptReportBO> otherServiceRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                                otherServiceRegistrationWiseList = ledgerDueServiceTranscriptForBillSummary.Where(x => x.RegistrationId != rrRow.RegistrationId && x.BillPaidBy == rrRow.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date
                                    &&
                                    (
                                        x.CheckOutDate == null ||
                                        (Convert.ToDateTime(x.CheckOutDate).Date < guestLedgerDateInfo.Date)
                                    )
                                ).ToList();
                                otherServiceBillAmount = otherServiceBillAmount + otherServiceRegistrationWiseList.Sum(item => item.ServiceAmount);

                                List<GuestLedgerTranscriptReportBO> otherPaymentRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                                otherPaymentRegistrationWiseList = ledgerDuePaymentTranscriptForBillSummary.Where(x => x.RegistrationId != rrRow.RegistrationId && x.BillPaidBy == rrRow.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date
                                    &&
                                    (
                                        x.CheckOutDate == null ||
                                        (Convert.ToDateTime(x.CheckOutDate).Date < guestLedgerDateInfo.Date)
                                    )
                                ).ToList();
                                otherPaymentBillAmount = otherPaymentBillAmount + otherPaymentRegistrationWiseList.Sum(item => item.ServiceAmount);
                            }
                        }
                    }


                    //List<GuestLedgerTranscriptReportBO> otherRoomRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    //otherRoomRegistrationWiseList = ledgerDueRoomTranscriptForBillSummary.Where(x => x.RegistrationId != registrationBO.RegistrationId && x.BillPaidBy == registrationBO.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date &&
                    //    (
                    //        x.CheckOutDate == null ||
                    //        (Convert.ToDateTime(x.CheckOutDate).Date < guestLedgerDateInfo.Date)

                    //    )

                    //    ).ToList();
                    //decimal otherRoomBillAmount = otherRoomRegistrationWiseList.Sum(item => item.ServiceAmount);


                    //List<GuestLedgerTranscriptReportBO> otherServiceRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    //otherServiceRegistrationWiseList = ledgerDueServiceTranscriptForBillSummary.Where(x => x.RegistrationId != registrationBO.RegistrationId && x.BillPaidBy == registrationBO.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date &&
                    //    (
                    //        x.CheckOutDate == null ||
                    //        (Convert.ToDateTime(x.CheckOutDate).Date < guestLedgerDateInfo.Date)

                    //    )
                    //).ToList();
                    //decimal otherServiceBillAmount = otherServiceRegistrationWiseList.Sum(item => item.ServiceAmount);

                    //List<GuestLedgerTranscriptReportBO> otherPaymentRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    //otherPaymentRegistrationWiseList = ledgerDuePaymentTranscriptForBillSummary.Where(x => x.RegistrationId != registrationBO.RegistrationId && x.BillPaidBy == registrationBO.RegistrationId && x.ServiceDate.Date < guestLedgerDateInfo.Date &&
                    //    (
                    //        x.CheckOutDate == null ||
                    //        (Convert.ToDateTime(x.CheckOutDate).Date < guestLedgerDateInfo.Date)

                    //    )
                    //).ToList();
                    //decimal otherPaymentBillAmount = otherPaymentRegistrationWiseList.Sum(item => item.ServiceAmount);


                    GuestLedgerTranscriptReportBO prevBillAmountBO = new GuestLedgerTranscriptReportBO();
                    prevBillAmountBO.RegistrationId = registrationBO.RegistrationId;
                    prevBillAmountBO.RoomNumber = registrationBO.RoomNumber;
                    prevBillAmountBO.GuestName = "";
                    prevBillAmountBO.CompanyName = "";
                    prevBillAmountBO.Pay = "";
                    prevBillAmountBO.Araival = guestLedgerDateInfo;
                    prevBillAmountBO.Departure = guestLedgerDateInfo;
                    prevBillAmountBO.ServiceName = "Last Day Balance";
                    prevBillAmountBO.ServiceId = -5000;
                    prevBillAmountBO.ServiceAmount = Math.Round((roomBillAmount + serviceBillAmount + paymentBillAmount) + (otherRoomBillAmount + otherServiceBillAmount + otherPaymentBillAmount));

                    if ((otherRoomBillAmount + otherServiceBillAmount + otherPaymentBillAmount) > 0)
                    {
                        if (prevBillAmountBO.ServiceAmount == 1)
                        {
                            prevBillAmountBO.ServiceAmount = 0;
                        }
                    }

                    previousBillAmountSummaryBOList.Add(prevBillAmountBO);
                }
                #endregion
                #region Todays Bill Summary
                // //------Todays Bill Summary -----------------------------
                List<GuestLedgerTranscriptReportBO> todaysBillAmountSummaryBOList = new List<GuestLedgerTranscriptReportBO>();
                foreach (RoomRegistrationBO registrationBO in roomRegistrationBOList)
                {
                    List<GuestLedgerTranscriptReportBO> todaysRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    todaysRegistrationWiseList = ledgerTodaysTranscriptReportList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();
                    decimal todaysBillAmount = todaysRegistrationWiseList.Sum(item => item.ServiceAmount);

                    GuestLedgerTranscriptReportBO todaysBillAmountBO = new GuestLedgerTranscriptReportBO();
                    todaysBillAmountBO.RegistrationId = registrationBO.RegistrationId;
                    todaysBillAmountBO.RoomNumber = registrationBO.RoomNumber;
                    todaysBillAmountBO.GuestName = "";
                    todaysBillAmountBO.CompanyName = "";
                    todaysBillAmountBO.Pay = "";
                    todaysBillAmountBO.Araival = guestLedgerDateInfo;
                    todaysBillAmountBO.Departure = guestLedgerDateInfo;
                    todaysBillAmountBO.ServiceName = "Todays Bill Amount";
                    todaysBillAmountBO.ServiceId = 4999;
                    todaysBillAmountBO.ServiceAmount = todaysBillAmount;
                    todaysBillAmountSummaryBOList.Add(todaysBillAmountBO);
                }
                #endregion
                #region Guest Todays Payment Transaction
                // //--------Guest Payment Transaction -----------------------------
                List<GuestLedgerTranscriptReportBO> ledgerGuestTodaysPaymentTranscriptReportList = new List<GuestLedgerTranscriptReportBO>();
                foreach (GuestBillPaymentBO row in guestWithoutTransferedBillPaymentBOList)
                {
                    if (row.PaymentDate.Date == guestLedgerDateInfo.Date)
                    {
                        GuestLedgerTranscriptReportBO ledgerTranscriptReportBO = new GuestLedgerTranscriptReportBO();

                        ledgerTranscriptReportBO.RegistrationId = row.RegistrationId;
                        ledgerTranscriptReportBO.RoomNumber = row.RoomNumber;
                        ledgerTranscriptReportBO.GuestName = "";
                        ledgerTranscriptReportBO.CompanyName = "";
                        ledgerTranscriptReportBO.Pay = "";
                        ledgerTranscriptReportBO.Araival = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.Departure = guestLedgerDateInfo;
                        ledgerTranscriptReportBO.ServiceId = 5003;
                        ledgerTranscriptReportBO.BillPaidBy = row.BillPaidBy;

                        if (row.PaymentType == "PaidOut")
                        {
                            ledgerTranscriptReportBO.ServiceAmount = row.PaymentAmount;
                            ledgerTranscriptReportBO.ServiceName = "Paid Out";
                        }
                        else if (row.PaymentType == "Company")
                        {
                            ledgerTranscriptReportBO.ServiceAmount = (-1) * (row.PaymentAmount);
                            ledgerTranscriptReportBO.ServiceName = "Company";
                        }
                        else if (row.PaymentType == "Discount")
                        {
                            ledgerTranscriptReportBO.ServiceAmount = 0;
                            ledgerTranscriptReportBO.ServiceName = "Cash";
                        }
                        else if (row.PaymentType == "Refund")
                        {
                            ledgerTranscriptReportBO.ServiceAmount = (row.PaymentAmount);
                            ledgerTranscriptReportBO.ServiceName = row.PaymentMode;
                        }
                        else
                        {
                            ledgerTranscriptReportBO.ServiceAmount = (-1) * (row.PaymentAmount);
                            ledgerTranscriptReportBO.ServiceName = row.PaymentMode;
                        }

                        ledgerGuestTodaysPaymentTranscriptReportList.Add(ledgerTranscriptReportBO);
                    }
                }
                #endregion
                #region Guest Todays Payment Total Amount
                // //------Guest Todays Payment Total Amount -----------------------------
                List<GuestLedgerTranscriptReportBO> guestTodaysPaymentBOList = new List<GuestLedgerTranscriptReportBO>();
                foreach (RoomRegistrationBO registrationBO in roomRegistrationBOList)
                {
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    roomRegistrationWiseList = ledgerGuestTodaysPaymentTranscriptReportList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();

                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseAdvanceList = roomRegistrationWiseList.Where(x => x.ServiceName != "Paid Out" && x.RegistrationId == registrationBO.RegistrationId).ToList();
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWisePaidOutList = ledgerGuestTodaysPaymentTranscriptReportList.Where(x => x.ServiceName == "Paid Out" && x.RegistrationId == registrationBO.RegistrationId).ToList();
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseRefundList = ledgerGuestTodaysPaymentTranscriptReportList.Where(x => x.ServiceName == "Refund" && x.RegistrationId == registrationBO.RegistrationId).ToList();

                    decimal roomAdvanceAmount = roomRegistrationWiseAdvanceList.Sum(item => item.ServiceAmount);
                    decimal roomPaidOutAmount = roomRegistrationWisePaidOutList.Sum(item => item.ServiceAmount);
                    decimal roomRefundAmount = roomRegistrationWiseRefundList.Sum(item => item.ServiceAmount);

                    GuestLedgerTranscriptReportBO billAmountBO = new GuestLedgerTranscriptReportBO();
                    billAmountBO.RegistrationId = registrationBO.RegistrationId;
                    billAmountBO.RoomNumber = registrationBO.RoomNumber;
                    billAmountBO.GuestName = "";
                    billAmountBO.CompanyName = "";
                    billAmountBO.Pay = "";
                    billAmountBO.Araival = guestLedgerDateInfo;
                    billAmountBO.Departure = guestLedgerDateInfo;
                    billAmountBO.ServiceName = "Todays Total Payment";
                    billAmountBO.ServiceId = 5006;
                    billAmountBO.ServiceAmount = Math.Round(roomAdvanceAmount - roomPaidOutAmount - roomRefundAmount);

                    guestTodaysPaymentBOList.Add(billAmountBO);
                }
                #endregion
                #region Guest Payment Transaction
                // //--------Guest Payment Transaction -----------------------------
                List<GuestLedgerTranscriptReportBO> ledgerGuestPaymentTranscriptReportList = new List<GuestLedgerTranscriptReportBO>();
                foreach (GuestBillPaymentBO row in guestWithoutTransferedBillPaymentBOList)
                {
                    GuestLedgerTranscriptReportBO ledgerTranscriptReportBO = new GuestLedgerTranscriptReportBO();

                    ledgerTranscriptReportBO.RegistrationId = row.RegistrationId;
                    ledgerTranscriptReportBO.RoomNumber = row.RoomNumber;
                    ledgerTranscriptReportBO.GuestName = "";
                    ledgerTranscriptReportBO.CompanyName = "";
                    ledgerTranscriptReportBO.Pay = "";
                    ledgerTranscriptReportBO.Araival = guestLedgerDateInfo;
                    ledgerTranscriptReportBO.Departure = guestLedgerDateInfo;
                    ledgerTranscriptReportBO.ServiceId = 5004;
                    ledgerTranscriptReportBO.BillPaidBy = row.BillPaidBy;

                    if (row.PaymentType == "PaidOut")
                    {
                        ledgerTranscriptReportBO.ServiceAmount = row.PaymentAmount;
                        ledgerTranscriptReportBO.ServiceName = "Paid Out";
                    }
                    else if (row.PaymentType == "Company")
                    {
                        ledgerTranscriptReportBO.ServiceAmount = (-1) * (row.PaymentAmount); //row.PaymentAmount;
                        ledgerTranscriptReportBO.ServiceName = "Company";
                    }
                    else if (row.PaymentType == "Discount")
                    {
                        ledgerTranscriptReportBO.ServiceAmount = 0;
                        ledgerTranscriptReportBO.ServiceName = "Cash";
                    }
                    else if (row.PaymentType == "Refund")
                    {
                        ledgerTranscriptReportBO.ServiceAmount = (row.PaymentAmount);
                        ledgerTranscriptReportBO.ServiceName = row.PaymentMode;
                    }
                    else
                    {
                        ledgerTranscriptReportBO.ServiceAmount = (-1) * (row.PaymentAmount);
                        ledgerTranscriptReportBO.ServiceName = row.PaymentMode;
                    }

                    ledgerGuestPaymentTranscriptReportList.Add(ledgerTranscriptReportBO);

                }
                #endregion
                #region Guest Payment Total Amount
                // //------Guest Payment Total Amount -----------------------------
                List<GuestLedgerTranscriptReportBO> guestPaymentBOList = new List<GuestLedgerTranscriptReportBO>();
                foreach (RoomRegistrationBO registrationBO in roomRegistrationBOList)
                {
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseList = new List<GuestLedgerTranscriptReportBO>();
                    roomRegistrationWiseList = ledgerGuestPaymentTranscriptReportList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseAdvanceList = roomRegistrationWiseList.Where(x => x.ServiceName != "Paid Out" && x.RegistrationId == registrationBO.RegistrationId).ToList();
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWisePaidOutList = ledgerGuestPaymentTranscriptReportList.Where(x => x.ServiceName == "Paid Out" && x.RegistrationId == registrationBO.RegistrationId).ToList();
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseRefundList = ledgerGuestPaymentTranscriptReportList.Where(x => x.ServiceName == "Refund" && x.RegistrationId == registrationBO.RegistrationId).ToList();
                    List<GuestLedgerTranscriptReportBO> roomRegistrationWiseOtherRoomPaymentList = ledgerGuestPaymentTranscriptReportList.Where(x => x.ServiceName == "Other Room" && x.RegistrationId == registrationBO.RegistrationId).ToList();

                    decimal roomAdvanceAmount = roomRegistrationWiseAdvanceList.Sum(item => item.ServiceAmount);
                    decimal roomPaidOutAmount = roomRegistrationWisePaidOutList.Sum(item => item.ServiceAmount);
                    decimal roomRefundAmount = roomRegistrationWiseRefundList.Sum(item => item.ServiceAmount);
                    decimal roomOtherRoomPaymentAmount = roomRegistrationWiseOtherRoomPaymentList.Sum(item => item.ServiceAmount);

                    GuestLedgerTranscriptReportBO billAmountBO = new GuestLedgerTranscriptReportBO();
                    billAmountBO.RegistrationId = registrationBO.RegistrationId;
                    billAmountBO.RoomNumber = registrationBO.RoomNumber;
                    billAmountBO.GuestName = "";
                    billAmountBO.CompanyName = "";
                    billAmountBO.Pay = "";
                    billAmountBO.Araival = guestLedgerDateInfo;
                    billAmountBO.Departure = guestLedgerDateInfo;
                    billAmountBO.ServiceName = "Total Payment";
                    billAmountBO.ServiceId = 5007;
                    billAmountBO.ServiceAmount = Math.Round(roomAdvanceAmount - roomOtherRoomPaymentAmount - roomPaidOutAmount - roomRefundAmount);
                    guestPaymentBOList.Add(billAmountBO);
                }
                #endregion
                #region Room Transfered Bill Summary
                // //------Room Transfered Bill Summary -----------------------------
                List<GuestLedgerTranscriptReportBO> totalRoomTransferedBillSummaryBOList = new List<GuestLedgerTranscriptReportBO>();
                List<GuestLedgerTranscriptReportBO> totalRoomTransferedAllBillSummaryBOList = new List<GuestLedgerTranscriptReportBO>();
                foreach (RoomRegistrationBO registrationBO in roomRegistrationBOList)
                {
                    //if (registrationBO.RegistrationId == 9434)
                    //{
                    //    string m = string.Empty;
                    //}

                    List<GuestLedgerTranscriptReportBO> roomTransferedBillSummaryBOList = new List<GuestLedgerTranscriptReportBO>();
                    roomTransferedBillSummaryBOList = ledgerRoomTransferedBillInformationBO.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();
                    decimal todaysBillAmount = roomTransferedBillSummaryBOList.Sum(item => item.ServiceAmount);

                    List<GuestBillPaymentBO> withoutRoomTransferPaymentBOInfo = new List<GuestBillPaymentBO>();
                    withoutRoomTransferPaymentBOInfo = guestOnlyOtherRoomTransferedBillPaymentBOList.Where(x => x.BillPaidBy == registrationBO.RegistrationId && x.PaymentDate.Date == guestLedgerDateInfo.Date).ToList();
                    decimal roomTransferedBillSummaryBOListAmount = withoutRoomTransferPaymentBOInfo.Sum(item => item.PaymentAmount);

                    GuestLedgerTranscriptReportBO transferedBillAmountBO = new GuestLedgerTranscriptReportBO();
                    transferedBillAmountBO.RegistrationId = registrationBO.RegistrationId;
                    transferedBillAmountBO.RoomNumber = registrationBO.RoomNumber;
                    transferedBillAmountBO.GuestName = "";
                    transferedBillAmountBO.CompanyName = "";
                    transferedBillAmountBO.Pay = "";
                    transferedBillAmountBO.Araival = guestLedgerDateInfo;
                    transferedBillAmountBO.Departure = guestLedgerDateInfo;
                    transferedBillAmountBO.ServiceName = "Bill Transfered";
                    transferedBillAmountBO.ServiceId = 5001;
                    transferedBillAmountBO.ServiceAmount = roomTransferedBillSummaryBOListAmount;

                    totalRoomTransferedBillSummaryBOList.Add(transferedBillAmountBO);
                }
                #endregion
                #region Guest Balance Amount
                // //------Guest Balance Amount -----------------------------
                List<GuestLedgerTranscriptReportBO> guestSummaryBOList = new List<GuestLedgerTranscriptReportBO>();
                List<GuestLedgerTranscriptReportBO> otherRoomTransactionReportInfoBOList = new List<GuestLedgerTranscriptReportBO>();
                foreach (RoomRegistrationBO registrationBO in roomRegistrationBOList)
                {

                    //if (registrationBO.RegistrationId == 9434)
                    //{
                    //    string m = string.Empty;
                    //}

                    if (registrationBO.BillPaidByRegistrationId == 0)
                    {
                        registrationBO.BillPaidByRegistrationId = registrationBO.RegistrationId;
                    }

                    List<GuestHouseCheckOutDetailBO> roomRegistrationWiseList = new List<GuestHouseCheckOutDetailBO>();
                    roomRegistrationWiseList = guestRoomDetailBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId && x.ServiceDate.Date < (guestLedgerDateInfo.Date.AddDays(1))).ToList();
                    decimal roomBillAmount = roomRegistrationWiseList.Sum(item => item.TotalAmount);

                    List<GuestHouseCheckOutDetailBO> serviceRegistrationWiseList = new List<GuestHouseCheckOutDetailBO>();
                    serviceRegistrationWiseList = guestServiceDetailBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId && x.ServiceDate.Date < (guestLedgerDateInfo.Date.AddDays(1))).ToList();
                    decimal serviceBillAmount = serviceRegistrationWiseList.Sum(item => item.TotalAmount);

                    List<GuestBillPaymentBO> paymentRegistrationWiseList = new List<GuestBillPaymentBO>();
                    paymentRegistrationWiseList = guestWithoutTransferedBillPaymentBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId && x.PaymentDate.Date < (guestLedgerDateInfo.Date.AddDays(1))).ToList();
                    decimal paymentBillAmount = paymentRegistrationWiseList.Sum(item => item.PaymentAmount);


                    List<GuestBillPaymentBO> dueWithoutRoomTransferPaymentBOInfo = new List<GuestBillPaymentBO>();
                    dueWithoutRoomTransferPaymentBOInfo = guestOnlyOtherRoomTransferedBillPaymentBOList.Where(x => x.BillPaidBy == registrationBO.RegistrationId && x.PaymentDate.Date < guestLedgerDateInfo.Date).ToList();
                    decimal dueRoomTransferedBillSummaryBOListAmount = dueWithoutRoomTransferPaymentBOInfo.Sum(item => item.PaymentAmount);

                    List<GuestLedgerTranscriptReportBO> roomTransferedBillSummaryBO = new List<GuestLedgerTranscriptReportBO>();
                    roomTransferedBillSummaryBO = totalRoomTransferedBillSummaryBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();
                    decimal roomTransferedBillSummaryAmount = roomTransferedBillSummaryBO.Sum(item => item.ServiceAmount);

                    GuestLedgerTranscriptReportBO billNetAmountBO = new GuestLedgerTranscriptReportBO();
                    billNetAmountBO.RegistrationId = registrationBO.RegistrationId;
                    billNetAmountBO.RoomNumber = registrationBO.RoomNumber;
                    billNetAmountBO.GuestName = "";
                    billNetAmountBO.CompanyName = "";
                    billNetAmountBO.Pay = "";
                    billNetAmountBO.Araival = guestLedgerDateInfo;
                    billNetAmountBO.Departure = guestLedgerDateInfo;
                    billNetAmountBO.ServiceName = "Net Amount";
                    billNetAmountBO.ServiceId = 5002;
                    billNetAmountBO.ServiceAmount = Math.Round(roomBillAmount + serviceBillAmount + dueRoomTransferedBillSummaryBOListAmount + roomTransferedBillSummaryAmount);

                    GuestLedgerTranscriptReportBO billAmountBO = new GuestLedgerTranscriptReportBO();
                    billAmountBO.RegistrationId = registrationBO.RegistrationId;
                    billAmountBO.RoomNumber = registrationBO.RoomNumber;
                    billAmountBO.GuestName = "";
                    billAmountBO.CompanyName = "";
                    billAmountBO.Pay = "";
                    billAmountBO.Araival = guestLedgerDateInfo;
                    billAmountBO.Departure = guestLedgerDateInfo;
                    billAmountBO.ServiceName = "Balance";
                    billAmountBO.ServiceId = 5008;
                    if (registrationBO.IsGuestCheckedOut == 0)
                    {
                        billAmountBO.ServiceAmount = Math.Round(roomBillAmount + serviceBillAmount + paymentBillAmount + dueRoomTransferedBillSummaryBOListAmount + roomTransferedBillSummaryAmount);

                        if (roomTransferedBillSummaryAmount > 0)
                        {
                            if (billAmountBO.ServiceAmount == 1)
                            {
                                billAmountBO.ServiceAmount = 0;
                            }
                        }
                    }
                    else
                    {
                        billAmountBO.ServiceAmount = 0;
                    }

                    foreach (GuestBillPaymentBO rebateRow in guestRebateInfoBOList)
                    {
                        if (registrationBO.RegistrationId == rebateRow.RegistrationId)
                        {
                            //billNetAmountBO.ServiceAmount = Math.Round(billNetAmountBO.ServiceAmount) - Math.Round(rebateRow.PaymentAmount);
                            billNetAmountBO.ServiceAmount = billNetAmountBO.ServiceAmount - rebateRow.PaymentAmount;
                        }
                    }

                    guestSummaryBOList.Add(billNetAmountBO);
                    guestSummaryBOList.Add(billAmountBO);


                    GuestLedgerTranscriptReportBO billAmountBORemarks = new GuestLedgerTranscriptReportBO();
                    billAmountBORemarks.RegistrationId = registrationBO.RegistrationId;
                    billAmountBORemarks.RoomNumber = registrationBO.RoomNumber;
                    billAmountBORemarks.GuestName = "";
                    billAmountBORemarks.CompanyName = "";
                    billAmountBORemarks.Araival = guestLedgerDateInfo;
                    billAmountBORemarks.Departure = guestLedgerDateInfo;
                    billAmountBORemarks.ServiceName = "Remarks";
                    billAmountBORemarks.ServiceId = 5010;
                    if (registrationBO.IsGuestCheckedOut == 0)
                    {
                        billAmountBORemarks.Pay = string.Empty;
                        billAmountBORemarks.ServiceAmount = Math.Round(roomBillAmount + serviceBillAmount + paymentBillAmount + dueRoomTransferedBillSummaryBOListAmount + roomTransferedBillSummaryAmount);

                        RoomRegistrationBO holdUpInfoBO = new RoomRegistrationBO();
                        holdUpInfoBO = roomRegistrationDA.GetHoldUpRoomRegistrationInfoById(registrationBO.RegistrationId, guestLedgerDateInfo.Date);
                        if (holdUpInfoBO != null)
                        {
                            if (holdUpInfoBO.RegistrationId > 0)
                            {
                                billAmountBORemarks.Pay = "Hold Up";
                            }
                        }
                    }
                    else
                    {
                        if (billAmountBORemarks.RegistrationId != registrationBO.BillPaidByRegistrationId)
                        {
                            billAmountBORemarks.Pay = "C/O (BT: Room # " + registrationBO.BillPaidByRoomNumber + ")";
                        }
                        else
                        {
                            billAmountBORemarks.Pay = "C/O";
                        }

                        billAmountBORemarks.ServiceAmount = 0;
                    }

                    guestSummaryBOList.Add(billAmountBORemarks);

                    GuestLedgerTranscriptReportBO otherRoomTransactionReportBOInfo = new GuestLedgerTranscriptReportBO();
                    List<GuestLedgerTranscriptReportBO> roomTransferedBillSummaryBO2 = new List<GuestLedgerTranscriptReportBO>();

                    List<GuestBillPaymentBO> paymentSummaryBOList = guestOnlyOtherRoomTransferedBillPaymentBOList.Where(x => x.RegistrationId == registrationBO.RegistrationId).ToList();
                    decimal roomTransferedBillSummaryAmount2 = paymentSummaryBOList.Sum(item => item.PaymentAmount);

                    otherRoomTransactionReportBOInfo.RegistrationId = registrationBO.RegistrationId;
                    otherRoomTransactionReportBOInfo.RoomNumber = registrationBO.RoomNumber;
                    otherRoomTransactionReportBOInfo.GuestName = "";
                    otherRoomTransactionReportBOInfo.CompanyName = "";
                    otherRoomTransactionReportBOInfo.Pay = "";
                    otherRoomTransactionReportBOInfo.Araival = guestLedgerDateInfo;
                    otherRoomTransactionReportBOInfo.Departure = guestLedgerDateInfo;
                    otherRoomTransactionReportBOInfo.ServiceId = 5005;
                    otherRoomTransactionReportBOInfo.BillPaidBy = registrationBO.RegistrationId;

                    if (registrationBO.IsGuestCheckedOut == 1)
                    {
                        otherRoomTransactionReportBOInfo.ServiceAmount = (Math.Round(roomTransferedBillSummaryAmount2));
                    }
                    else
                    {
                        otherRoomTransactionReportBOInfo.ServiceAmount = 0;
                    }

                    otherRoomTransactionReportBOInfo.ServiceName = "Other Room";
                    otherRoomTransactionReportInfoBOList.Add(otherRoomTransactionReportBOInfo);
                }
                #endregion

                List<GuestLedgerTranscriptReportBO> ledgerTranscriptReportList = new List<GuestLedgerTranscriptReportBO>();
                ledgerTranscriptReportList.AddRange(ledgerTodaysTranscriptReportList);
                ledgerTranscriptReportList.AddRange(todaysBillAmountSummaryBOList);
                ledgerTranscriptReportList.AddRange(previousBillAmountSummaryBOList);
                ledgerTranscriptReportList.AddRange(totalRoomTransferedBillSummaryBOList);
                ledgerTranscriptReportList.AddRange(otherRoomTransactionReportInfoBOList);
                ledgerTranscriptReportList.AddRange(ledgerGuestTodaysPaymentTranscriptReportList);
                ledgerTranscriptReportList.AddRange(guestTodaysPaymentBOList);
                ledgerTranscriptReportList.AddRange(guestPaymentBOList);
                ledgerTranscriptReportList.AddRange(guestSummaryBOList);

                foreach (RoomRegistrationBO rrrow in roomRegistrationBOList)
                {
                    foreach (GuestLedgerTranscriptReportBO row in ledgerTranscriptReportList)
                    {
                        if (row.RegistrationId == rrrow.RegistrationId)
                        {
                            row.GuestName = rrrow.GuestName;
                        }
                    }
                }

                //if (!string.IsNullOrWhiteSpace(txtSrcRoomNumber.Text))
                //{
                //    ledgerTranscriptReportList.Where(x => x.RoomNumber.ToString() == txtSrcRoomNumber.Text.ToString()).ToList();
                //}
                //else
                //{
                    ledgerTranscriptReportList.Where(x => x.RoomNumber != 0).ToList().OrderBy(x => x.RoomNumber).ToList();
                //}

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], ledgerTranscriptReportList));

                rvTransaction.LocalReport.DisplayName = "Guest Ledger Transcript Information";
                rvTransaction.LocalReport.Refresh();
            }
            frmPrint.Attributes["src"] = "";
        }

    }
}