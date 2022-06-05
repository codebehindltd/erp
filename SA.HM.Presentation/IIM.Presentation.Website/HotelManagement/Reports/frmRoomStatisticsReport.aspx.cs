using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmRoomStatisticsReport : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ReportType"] != null)
                {
                    string reportType = Request.QueryString["ReportType"].ToString();
                    if (reportType == "ExpectedArrival")
                    {
                        GuestHouseReportInfo("ArrivalList");
                    }
                    else if (reportType == "ExpectedDeparture")
                    {
                        GuestHouseReportInfo("ProbablyVacantList");
                    }
                    else if (reportType == "CheckInRoom")
                    {
                        GuestHouseReportInfo("CheckInList");
                    }
                    else if (reportType == "InhouseRoomGuest")
                    {
                        GuestHouseReportInfo("InHouseGustList");
                    }
                    else if (reportType == "InhouseForeigner")
                    {
                        GuestHouseReportInfo("InhouseForeigner");
                    }
                    else if (reportType == "AirportPickupDrop")
                    {
                        AirportPickupDropInfo();
                    }
                    else if (reportType == "HotelPosition")
                    {
                        HotelPositionInfo();
                    }
                    else if (reportType == "DHotelPosition")
                    {
                        DetailedHotelPositionInfo();
                    }
                    else if (reportType == "ExtraAdultsOrChild")
                    {
                        ExtraAdultsOrChildInfo();
                    }
                    else if (reportType == "ForecastReport")
                    {
                        ForecastReportInfo();
                    }
                    else if (reportType == "WalkInRoom")
                    {
                        WalkInRoomInfo();
                    }
                    else if (reportType == "GuestBlock")
                    {
                        GuestBlockInfo();
                    }
                }
            }
        }
        private void GuestHouseReportInfo(string reportType)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string reportDisplayDate = string.Empty;
            DateTime currentDate = DateTime.Now;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DateTime reportDate = DateTime.Now;
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                fromDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
                toDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            _RoomStatusInfoByDate = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = string.Empty;

            if (reportType == "ArrivalList")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseArrivalInfo.rdlc");
            }
            else if (reportType == "CheckOutList")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseCheckOutInfo.rdlc");
            }
            else if (reportType == "ProbablyVacantList")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseExpectedCheckOutInfo.rdlc");
            }
            else if (reportType == "InHouseGustList")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseInfoOrderByRoom.rdlc");
            }
            else if (reportType == "InHouseGroupList")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGroupGuestHouseInfo.rdlc");
            }
            else if (reportType == "InhouseForeigner")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseInfo.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestHouseInfo.rdlc");
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

            reportDisplayDate = "Report Date: " + hmUtility.GetStringFromDateTime(fromDate);

            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));            
            reportParam.Add(new ReportParameter("ReportDisplayDate", reportDisplayDate));

            rvTransaction.LocalReport.SetParameters(reportParam);

            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, "0");

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], guestHouseInfo));

            if (reportType == "ArrivalList")
            {
                rvTransaction.LocalReport.DisplayName = "Expected Arrival Guest List";
            }
            else if (reportType == "CheckOutList")
            {
                rvTransaction.LocalReport.DisplayName = "Checked-Out Guest List";
            }
            else if (reportType == "ProbablyVacantList")
            {
                rvTransaction.LocalReport.DisplayName = "Expected Departure Guest List";
            }
            else if (reportType == "InHouseGroupList")
            {
                rvTransaction.LocalReport.DisplayName = "In-House Group Guest List";
            }
            else if (reportType == "InhouseForeigner")
            {
                rvTransaction.LocalReport.DisplayName = "In-House Foreigner Guest List";
            }
            else
            {
                rvTransaction.LocalReport.DisplayName = "In-House Guest List";
            }

            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        private void AirportPickupDropInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            int companyId = 0;

            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                FromDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
                ToDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestAirportPickUpDropInfo.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            GuestAirportPickUpDropDA guestAirportPDDA = new GuestAirportPickUpDropDA();
            List<GuestAirportPickUpDropReportViewBO> guestAipportPDBO = new List<GuestAirportPickUpDropReportViewBO>();
            guestAipportPDBO = guestAirportPDDA.GetGuestAirportPickupDropInfo(FromDate, ToDate, companyId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestAipportPDBO));
            rvTransaction.LocalReport.DisplayName = "Airport Pickup Drop Information";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        private void HotelPositionInfo()
        {
            DateTime dateTime = DateTime.Now;
            string startDate = string.Empty;
            string endDate = string.Empty;

            //if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            //{
            //    startDate = hmUtility.GetStringFromDateTime(dateTime);
            //    this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            //}
            //else
            //{
            //    startDate = this.txtFromDate.Text;
            //}
            //if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            //{
            //    endDate = hmUtility.GetStringFromDateTime(dateTime);
            //    this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            //}
            //else
            //{
            //    endDate = this.txtToDate.Text;
            //}

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime fromDate = DateTime.Today;
            if (Request.QueryString["FromDate"] != null)
            {
                fromDate = hmUtility.GetDateTimeFromString(Request.QueryString["FromDate"].ToString(), userInformationBO.ServerDateFormat);
            }

            DateTime FromDate = fromDate;
            DateTime ToDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month));
            List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(fromDate, ToDate);

            //DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);


            _RoomStatusInfoByDate = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptHotelForecastReportInfo.rdlc");

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

            DateTime currentDate = dateTime;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            RoomNumberDA entityDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberInfoBO = entityDA.GetRoomNumberInfo();

            int totalRoomQuantity = 0;
            if (roomNumberInfoBO != null)
            {
                totalRoomQuantity = roomNumberInfoBO.Count();
            }

            reportParam.Add(new ReportParameter("TotalRoomQuantity", totalRoomQuantity.ToString()));
            reportParam.Add(new ReportParameter("ReportType", "Room Control Chart Report"));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            //Get all room available room types
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> roomTypeList = roomTypeDA.GetRoomTypeInfoWithRoomCount();

            int totalRoomCount = 1;
            totalRoomCount = roomTypeList.Sum(item => item.TotalRoom);

            //Getting all active registration information
            RoomRegistrationDA rRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> activeRegistrations = rRegistrationDA.GetActiveRoomRegistrationWithRoomType();

            //DateTime fromDate = DateTime.Today;
            //List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(fromDate, ToDate);
            currentDate = DateTime.Now;

            //Getting reservation information from today to end of current month
            RoomReservationDA rReservationDA = new RoomReservationDA();
            List<RoomReservationBO> reservationList = rReservationDA.GetRoomReservationRoomInfoByDateRange(fromDate.AddSeconds(-1), ToDate);

            //Calculating Inhouse Guest counts for each room type
            foreach (RoomTypeBO rType in roomTypeList)
            {
                rType.OccupiedRoomCount = activeRegistrations.Where(rr => rr.RoomTypeId == rType.RoomTypeId).Count();
            }

            int todaysInhouse = activeRegistrations.Count;

            //Getting out of order information
            List<RoomLogFileBO> outOfOrderOutOfServiceLog = entityDA.GetRoomLogFileInfoByDateRange(fromDate, ToDate);
            List<RoomLogFileBO> outOfOrderLog;
            List<RoomLogFileBO> outOfServiceLog;

            List<RoomCalenderBO> calenderReportListBO = new List<RoomCalenderBO>();

            foreach (DateTime dateRow in DateList)
            {
                // // ------------Available Rooms
                RoomCalenderBO trCountBO = new RoomCalenderBO();
                trCountBO.TransactionDate = dateRow;
                trCountBO.ServiceType = "Available Rooms";
                trCountBO.ServiceQuantity = Convert.ToDecimal(totalRoomCount);
                trCountBO.DisplayQuantity = Convert.ToInt64(totalRoomCount).ToString();
                calenderReportListBO.Add(trCountBO);

                // // ------------Expected Arrivals
                int eciCount = reservationList.Where(x => x.DateIn.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

                RoomCalenderBO eciCountBO = new RoomCalenderBO();
                eciCountBO.TransactionDate = dateRow;
                eciCountBO.ServiceType = "Expected Arrivals";
                eciCountBO.ServiceQuantity = eciCount;
                eciCountBO.DisplayQuantity = Convert.ToInt64(eciCount).ToString();
                calenderReportListBO.Add(eciCountBO);

                // // ------------In House Guest
                RoomCalenderBO ihCountBO = new RoomCalenderBO();
                ihCountBO.TransactionDate = dateRow;
                ihCountBO.ServiceType = "Stay On (In House)";
                ihCountBO.ServiceQuantity = todaysInhouse;
                ihCountBO.DisplayQuantity = Convert.ToInt64(todaysInhouse).ToString();
                calenderReportListBO.Add(ihCountBO);

                int ecoCount = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

                // // ------------Expected Departure
                RoomCalenderBO ecoCountBO = new RoomCalenderBO();
                ecoCountBO.TransactionDate = dateRow;
                ecoCountBO.ServiceType = "Expected Departure";
                ecoCountBO.ServiceQuantity = ecoCount;
                ecoCountBO.DisplayQuantity = Convert.ToInt64(ecoCount).ToString();
                calenderReportListBO.Add(ecoCountBO);

                // // ------------Occupency Count
                int ocCount = todaysInhouse + eciCount - ecoCount;

                RoomCalenderBO ocCountBO = new RoomCalenderBO();
                ocCountBO.TransactionDate = dateRow;
                ocCountBO.ServiceType = "Occupency";
                ocCountBO.ServiceQuantity = ocCount;
                ocCountBO.DisplayQuantity = Convert.ToInt64(ocCount).ToString();
                calenderReportListBO.Add(ocCountBO);

                ///-----Out Of Order
                if (dateRow.Date == DateTime.Now.Date)
                    outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Order").ToList();
                else
                    outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Order").ToList();
                int outOfOrderCount = outOfOrderLog.Select(r => r.RoomId).Distinct().Count();
                RoomCalenderBO oooBO = new RoomCalenderBO();
                oooBO.TransactionDate = dateRow;
                oooBO.ServiceType = "Out Of Order";
                oooBO.ServiceQuantity = outOfOrderCount;
                oooBO.DisplayQuantity = outOfOrderCount.ToString();
                calenderReportListBO.Add(oooBO);

                ///-----Out Of Service
                if (dateRow.Date == DateTime.Now.Date)
                    outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Service").ToList();
                else
                    outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Service").ToList();
                int outOfServiceCount = outOfServiceLog.Select(r => r.RoomId).Distinct().Count();
                RoomCalenderBO oocBO = new RoomCalenderBO();
                oocBO.TransactionDate = dateRow;
                oocBO.ServiceType = "Out Of Service";
                oocBO.ServiceQuantity = outOfServiceCount;
                oocBO.DisplayQuantity = outOfServiceCount.ToString();
                calenderReportListBO.Add(oocBO);

                ///Total Room Position
                int positionCount = totalRoomCount - ocCount - outOfOrderCount - outOfServiceCount;
                RoomCalenderBO positionBO = new RoomCalenderBO();
                positionBO.TransactionDate = dateRow;
                positionBO.ServiceType = "Position";
                positionBO.ServiceQuantity = positionCount;
                positionBO.DisplayQuantity = positionCount.ToString();
                calenderReportListBO.Add(positionBO);

                // // ------------Occupency Percentage (%)
                decimal ocpCount = Convert.ToDecimal((ocCount * 100) / totalRoomCount);

                RoomCalenderBO ocpCountBO = new RoomCalenderBO();
                ocpCountBO.TransactionDate = dateRow;
                ocpCountBO.ServiceType = "Occupency %";
                ocpCountBO.ServiceQuantity = ocpCount;
                ocpCountBO.DisplayQuantity = ocpCount.ToString() + "%";
                calenderReportListBO.Add(ocpCountBO);

                RoomCalenderBO blankCountBO = new RoomCalenderBO();
                blankCountBO.TransactionDate = dateRow;
                blankCountBO.ServiceType = "";
                blankCountBO.ServiceQuantity = -99999;
                blankCountBO.DisplayQuantity = "-99999";
                calenderReportListBO.Add(blankCountBO);

                RoomCalenderBO roomTypeBO = new RoomCalenderBO();
                roomTypeBO.TransactionDate = dateRow;
                roomTypeBO.ServiceType = "Room Type";
                roomTypeBO.ServiceQuantity = 0;// dateRow.Day;
                roomTypeBO.DisplayQuantity = ""; //dateRow.Day.ToString();
                calenderReportListBO.Add(roomTypeBO);

                foreach (RoomTypeBO rType in roomTypeList)
                {
                    int eciCountRT = reservationList.Where(x => x.DateIn.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);

                    int ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
                    rType.OccupiedRoomCount += eciCountRT - ecoCountRT;

                    outOfOrderCount = outOfOrderLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();
                    outOfServiceCount = outOfServiceLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();

                    rType.AvailableRoomCount = rType.TotalRoom - rType.OccupiedRoomCount - outOfOrderCount - outOfServiceCount;

                    RoomCalenderBO ocpCountRoomTypeBO = new RoomCalenderBO();
                    ocpCountRoomTypeBO.TransactionDate = dateRow;
                    ocpCountRoomTypeBO.ServiceType = rType.RoomType;
                    ocpCountRoomTypeBO.ServiceQuantity = rType.AvailableRoomCount;
                    ocpCountRoomTypeBO.DisplayQuantity = rType.AvailableRoomCount.ToString();
                    calenderReportListBO.Add(ocpCountRoomTypeBO);
                }

                RoomCalenderBO vacantBO = new RoomCalenderBO();
                vacantBO.TransactionDate = dateRow;
                vacantBO.ServiceType = "Total Vacant";
                vacantBO.ServiceQuantity = positionCount;
                vacantBO.DisplayQuantity = positionCount.ToString();
                calenderReportListBO.Add(vacantBO);

                todaysInhouse = ocCount;
            }

            foreach (var item in calenderReportListBO.Where(w => w.TransactionDate < DateTime.Today))
            {
                item.ServiceQuantity = 0;
                item.DisplayQuantity = "";
            }

            calenderReportListBO = calenderReportListBO.Where(x => x.TransactionDate.Date >= FromDate.Date).ToList();

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], calenderReportListBO));
            rvTransaction.LocalReport.DisplayName = "Room Control Chart Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";

            //_RoomStatusInfoByDate = 1;
            //rvTransaction.LocalReport.DataSources.Clear();
            //rvTransaction.ProcessingMode = ProcessingMode.Local;
            //rvTransaction.LocalReport.EnableExternalImages = true;

            //var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptHotelForecastReportInfo.rdlc");

            //if (!File.Exists(reportPath))
            //    return;

            //rvTransaction.LocalReport.ReportPath = reportPath;
            //List<ReportParameter> reportParam = new List<ReportParameter>();

            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();
            //if (files[0].CompanyId > 0)
            //{
            //    reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
            //    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

            //    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //    {
            //        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
            //    }
            //    else
            //    {
            //        reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
            //    }
            //}

            //DateTime currentDate = DateTime.Now;
            //string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            //string footerPoweredByInfo = string.Empty;
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //HMCommonDA hmCommonDA = new HMCommonDA();
            //string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            //reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            //reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            //RoomNumberDA entityDA = new RoomNumberDA();
            //List<RoomNumberBO> roomNumberInfoBO = entityDA.GetRoomNumberInfo();

            //int totalRoomQuantity = 0;
            //if (roomNumberInfoBO != null)
            //{
            //    totalRoomQuantity = roomNumberInfoBO.Count();
            //}

            //reportParam.Add(new ReportParameter("TotalRoomQuantity", totalRoomQuantity.ToString()));
            //reportParam.Add(new ReportParameter("ReportType", "Hotel Forecast Room Position"));

            //rvTransaction.LocalReport.SetParameters(reportParam);
            //var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            //int totalRoomCount = 1;

            ////Get all room available room types
            //RoomTypeDA roomTypeDA = new RoomTypeDA();
            //List<RoomTypeBO> roomTypeList = roomTypeDA.GetRoomTypeInfoWithRoomCount();

            //totalRoomCount = roomTypeList.Sum(item => item.TotalRoom);

            ////Getting all active registration information
            //RoomRegistrationDA rRegistrationDA = new RoomRegistrationDA();
            //List<RoomRegistrationBO> activeRegistrations = rRegistrationDA.GetActiveRoomRegistrationWithRoomType();

            //DateTime fromDate = DateTime.Today;
            //if (Request.QueryString["FromDate"] != null)
            //{
            //    fromDate = hmUtility.GetDateTimeFromString(Request.QueryString["FromDate"].ToString(), userInformationBO.ServerDateFormat);
            //}

            //DateTime ToDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month));
            //List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(fromDate, ToDate);

            ////Getting reservation information from today to end of current month
            //RoomReservationDA rReservationDA = new RoomReservationDA();
            ////List<RoomReservationBO> reservationList = rReservationDA.GetRoomReservationInfoByDateRange(fromDate.AddSeconds(-1), ToDate);
            //List<RoomReservationBO> reservationList = rReservationDA.GetRoomReservationRoomInfoByDateRange(fromDate.AddSeconds(-1), ToDate);

            ////Calculating Inhouse Guest counts for each room type
            //foreach (RoomTypeBO rType in roomTypeList)
            //{
            //    rType.OccupiedRoomCount = activeRegistrations.Where(rr => rr.RoomTypeId == rType.RoomTypeId).Count();
            //}

            //int todaysInhouse = activeRegistrations.Count;

            ////Getting out of order information
            ////List<RoomLogFileBO> outOfOrderOutOfServiceLog = entityDA.GetRoomLogFileInfoByDateRange(DateTime.Today, new DateTime(currentDate.Year, currentDate.Month + 1, 1).AddSeconds(-1));
            //List<RoomLogFileBO> outOfOrderOutOfServiceLog = entityDA.GetRoomLogFileInfoByDateRange(fromDate, ToDate);
            //List<RoomLogFileBO> outOfOrderLog = new List<RoomLogFileBO>();
            //List<RoomLogFileBO> outOfServiceLog;

            //List<RoomCalenderBO> calenderReportListBO = new List<RoomCalenderBO>();

            ////*
            //foreach (DateTime dateRow in DateList)
            //{
            //    // // ------------Available Rooms
            //    RoomCalenderBO trCountBO = new RoomCalenderBO();
            //    trCountBO.TransactionDate = dateRow;
            //    trCountBO.ServiceType = "Available Rooms";
            //    trCountBO.ServiceQuantity = Convert.ToDecimal(totalRoomCount);
            //    trCountBO.DisplayQuantity = Convert.ToInt64(totalRoomCount).ToString();
            //    calenderReportListBO.Add(trCountBO);

            //    //// // ------------Expected Arrivals
            //    int eciCount = reservationList.Where(x => x.DateIn.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

            //    RoomCalenderBO eciCountBO = new RoomCalenderBO();
            //    eciCountBO.TransactionDate = dateRow;
            //    eciCountBO.ServiceType = "Expected Arrivals";
            //    eciCountBO.ServiceQuantity = eciCount;
            //    eciCountBO.DisplayQuantity = Convert.ToInt64(eciCount).ToString();
            //    calenderReportListBO.Add(eciCountBO);

            //    // // ------------In House Guest
            //    RoomCalenderBO ihCountBO = new RoomCalenderBO();
            //    ihCountBO.TransactionDate = dateRow;
            //    ihCountBO.ServiceType = "Stay On (In House)";
            //    ihCountBO.ServiceQuantity = todaysInhouse;
            //    ihCountBO.DisplayQuantity = Convert.ToInt64(todaysInhouse).ToString();
            //    calenderReportListBO.Add(ihCountBO);

            //    int ecoCount = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

            //    // // ------------Expected Departure
            //    RoomCalenderBO ecoCountBO = new RoomCalenderBO();
            //    ecoCountBO.TransactionDate = dateRow;
            //    ecoCountBO.ServiceType = "Expected Departure";
            //    ecoCountBO.ServiceQuantity = ecoCount;
            //    ecoCountBO.DisplayQuantity = Convert.ToInt64(ecoCount).ToString();
            //    calenderReportListBO.Add(ecoCountBO);

            //    // // ------------Occupency Count
            //    int ocCount = todaysInhouse + eciCount - ecoCount;

            //    RoomCalenderBO ocCountBO = new RoomCalenderBO();
            //    ocCountBO.TransactionDate = dateRow;
            //    ocCountBO.ServiceType = "Occupency";
            //    ocCountBO.ServiceQuantity = ocCount;
            //    ocCountBO.DisplayQuantity = Convert.ToInt64(ocCount).ToString();
            //    calenderReportListBO.Add(ocCountBO);

            //    ///-----Out Of Order
            //    if (dateRow.Date == DateTime.Now.Date)
            //        outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Order").ToList();
            //    else
            //        outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Order").ToList();
            //    int outOfOrderCount = outOfOrderLog.Select(r => r.RoomId).Distinct().Count();
            //    RoomCalenderBO oooBO = new RoomCalenderBO();
            //    oooBO.TransactionDate = dateRow;
            //    oooBO.ServiceType = "Out Of Order";
            //    oooBO.ServiceQuantity = outOfOrderCount;
            //    oooBO.DisplayQuantity = outOfOrderCount.ToString();
            //    calenderReportListBO.Add(oooBO);

            //    ///-----Out Of Service
            //    if (dateRow.Date == DateTime.Now.Date)
            //        outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Service").ToList();
            //    else
            //        outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Service").ToList();
            //    int outOfServiceCount = outOfServiceLog.Select(r => r.RoomId).Distinct().Count();
            //    RoomCalenderBO oocBO = new RoomCalenderBO();
            //    oocBO.TransactionDate = dateRow;
            //    oocBO.ServiceType = "Out Of Service";
            //    oocBO.ServiceQuantity = outOfServiceCount;
            //    oocBO.DisplayQuantity = outOfServiceCount.ToString();
            //    calenderReportListBO.Add(oocBO);

            //    ///Total Room Position
            //    int positionCount = totalRoomCount - ocCount - outOfOrderCount - outOfServiceCount;
            //    RoomCalenderBO positionBO = new RoomCalenderBO();
            //    positionBO.TransactionDate = dateRow;
            //    positionBO.ServiceType = "Position";
            //    positionBO.ServiceQuantity = positionCount;
            //    positionBO.DisplayQuantity = positionCount.ToString();
            //    calenderReportListBO.Add(positionBO);

            //    // // ------------Occupency Percentage (%)
            //    decimal ocpCount = Convert.ToDecimal((ocCount * 100) / totalRoomCount);

            //    RoomCalenderBO ocpCountBO = new RoomCalenderBO();
            //    ocpCountBO.TransactionDate = dateRow;
            //    ocpCountBO.ServiceType = "Occupency %";
            //    ocpCountBO.ServiceQuantity = ocpCount;
            //    ocpCountBO.DisplayQuantity = ocpCount.ToString() + "%";
            //    calenderReportListBO.Add(ocpCountBO);

            //    RoomCalenderBO blankCountBO = new RoomCalenderBO();
            //    blankCountBO.TransactionDate = dateRow;
            //    blankCountBO.ServiceType = "";
            //    blankCountBO.ServiceQuantity = -99999;
            //    blankCountBO.DisplayQuantity = "-99999";
            //    calenderReportListBO.Add(blankCountBO);

            //    RoomCalenderBO roomTypeBO = new RoomCalenderBO();
            //    roomTypeBO.TransactionDate = dateRow;
            //    roomTypeBO.ServiceType = "Room Type";
            //    roomTypeBO.ServiceQuantity = 0;// dateRow.Day;
            //    roomTypeBO.DisplayQuantity = ""; //dateRow.Day.ToString();
            //    calenderReportListBO.Add(roomTypeBO);

            //    foreach (RoomTypeBO rType in roomTypeList)
            //    {
            //        int eciCountRT = reservationList.Where(x => x.DateIn.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);

            //        int ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
            //        rType.OccupiedRoomCount += eciCountRT - ecoCountRT;

            //        outOfOrderCount = outOfOrderLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();
            //        outOfServiceCount = outOfServiceLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();

            //        rType.AvailableRoomCount = rType.TotalRoom - rType.OccupiedRoomCount - outOfOrderCount - outOfServiceCount;

            //        RoomCalenderBO ocpCountRoomTypeBO = new RoomCalenderBO();
            //        ocpCountRoomTypeBO.TransactionDate = dateRow;
            //        ocpCountRoomTypeBO.ServiceType = rType.RoomType;
            //        ocpCountRoomTypeBO.ServiceQuantity = rType.AvailableRoomCount;
            //        ocpCountRoomTypeBO.DisplayQuantity = rType.AvailableRoomCount.ToString();
            //        calenderReportListBO.Add(ocpCountRoomTypeBO);
            //    }

            //    //foreach (RoomTypeBO rType in roomTypeList)
            //    //{
            //    //    int eciCountRT = 0, ecoCountRT = 0;

            //    //    if (dateRow.Date == DateTime.Now.Date)
            //    //    {
            //    //        eciCountRT = reservationList.Where(x => x.DateIn.Date == DateTime.Now.Date && x.DateIn >= DateTime.Now && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
            //    //        ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == DateTime.Now.Date && x.ExpectedCheckOutDate >= DateTime.Now && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut == DateTime.Now && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
            //    //    }
            //    //    else
            //    //    {
            //    //        eciCountRT = reservationList.Where(x => x.DateIn.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
            //    //        ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
            //    //    }
            //    //    rType.OccupiedRoomCount += eciCountRT - ecoCountRT;

            //    //    outOfOrderCount = outOfOrderLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();
            //    //    outOfServiceCount = outOfServiceLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();

            //    //    rType.AvailableRoomCount = rType.TotalRoom - rType.OccupiedRoomCount - outOfOrderCount - outOfServiceCount;

            //    //    RoomCalenderBO ocpCountRoomTypeBO = new RoomCalenderBO();
            //    //    ocpCountRoomTypeBO.TransactionDate = dateRow;
            //    //    ocpCountRoomTypeBO.ServiceType = rType.RoomType;
            //    //    ocpCountRoomTypeBO.ServiceQuantity = rType.AvailableRoomCount;
            //    //    ocpCountRoomTypeBO.DisplayQuantity = rType.AvailableRoomCount.ToString();
            //    //    calenderReportListBO.Add(ocpCountRoomTypeBO);
            //    //}

            //    //int positionCount = totalRoomCount - ocCount;
            //    RoomCalenderBO vacantBO = new RoomCalenderBO();
            //    vacantBO.TransactionDate = dateRow;
            //    vacantBO.ServiceType = "Total Vacant";
            //    vacantBO.ServiceQuantity = positionCount;
            //    vacantBO.DisplayQuantity = positionCount.ToString();
            //    calenderReportListBO.Add(vacantBO);

            //    todaysInhouse = ocCount;
            //}//*/

            //foreach (var item in calenderReportListBO.Where(w => w.TransactionDate < DateTime.Today))
            //{
            //    item.ServiceQuantity = 0;
            //    item.DisplayQuantity = "";
            //}

            //rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], calenderReportListBO));
            //rvTransaction.LocalReport.DisplayName = "Hotel Forecast Room Position";
            //rvTransaction.LocalReport.Refresh();
            //frmPrint.Attributes["src"] = "";
        }
        private void DetailedHotelPositionInfo()
        {
            _RoomStatusInfoByDate = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptHotelDetailedRoomPositionStatus.rdlc");

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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            RoomNumberDA entityDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberInfoBO = entityDA.GetRoomNumberInfo();

            int totalRoomQuantity = 0;
            if (roomNumberInfoBO != null)
            {
                totalRoomQuantity = roomNumberInfoBO.Count();
            }

            reportParam.Add(new ReportParameter("TotalRoomQuantity", totalRoomQuantity.ToString()));
            reportParam.Add(new ReportParameter("ReportType", "Hotel Room Position"));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            DateTime positionDate = DateTime.Now;
            List<RoomCalenderBO> avalibleRoomList = new List<RoomCalenderBO>();
            avalibleRoomList = hmCommonDA.GetHotelDetailedRoomPositionStatusInfo(positionDate);
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], avalibleRoomList));
            rvTransaction.LocalReport.DisplayName = "Hotel Detailed Room Position";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        private void ForecastReportInfo()
        {
            string prmReportType = "ForecastReport";
            _RoomStatusInfoByDate = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptHotelForecastReportInfo.rdlc");

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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            RoomNumberDA entityDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberInfoBO = entityDA.GetRoomNumberInfo();

            int totalRoomQuantity = 0;
            if (roomNumberInfoBO != null)
            {
                totalRoomQuantity = roomNumberInfoBO.Count();
            }

            reportParam.Add(new ReportParameter("TotalRoomQuantity", totalRoomQuantity.ToString()));
            reportParam.Add(new ReportParameter("ReportType", "Hotel Forecast Report"));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            int totalRoomCount = 1;

            //Get all room available room types
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> roomTypeList = roomTypeDA.GetRoomTypeInfoWithRoomCount();

            totalRoomCount = roomTypeList.Sum(item => item.TotalRoom);

            //Getting all active registration information
            RoomRegistrationDA rRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> activeRegistrations = rRegistrationDA.GetActiveRoomRegistrationWithRoomType();

            DateTime fromDate = DateTime.Today;
            DateTime ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(fromDate, ToDate);

            //Getting reservation information from today to end of current month
            RoomReservationDA rReservationDA = new RoomReservationDA();
            //List<RoomReservationBO> reservationList = rReservationDA.GetRoomReservationInfoByDateRange(fromDate.AddSeconds(-1), ToDate);
            List<RoomReservationBO> reservationList = rReservationDA.GetRoomReservationRoomInfoByDateRange(fromDate.AddSeconds(-1), ToDate);

            //Calculating Inhouse Guest counts for each room type
            foreach (RoomTypeBO rType in roomTypeList)
            {
                rType.OccupiedRoomCount = activeRegistrations.Where(rr => rr.RoomTypeId == rType.RoomTypeId).Count();
            }

            int todaysInhouse = activeRegistrations.Count;

            //Getting out of order information
            //List<RoomLogFileBO> outOfOrderOutOfServiceLog = entityDA.GetRoomLogFileInfoByDateRange(DateTime.Today, new DateTime(currentDate.Year, currentDate.Month + 1, 1).AddSeconds(-1));
            List<RoomLogFileBO> outOfOrderOutOfServiceLog = entityDA.GetRoomLogFileInfoByDateRange(fromDate, ToDate);
            List<RoomLogFileBO> outOfOrderLog = new List<RoomLogFileBO>();
            List<RoomLogFileBO> outOfServiceLog;

            List<RoomCalenderBO> calenderReportListBO = new List<RoomCalenderBO>();

            //*
            foreach (DateTime dateRow in DateList)
            {
                // // ------------Available Rooms
                RoomCalenderBO trCountBO = new RoomCalenderBO();
                trCountBO.TransactionDate = dateRow;
                trCountBO.ServiceType = "Available Rooms";
                trCountBO.ServiceQuantity = Convert.ToDecimal(totalRoomCount);
                trCountBO.DisplayQuantity = Convert.ToInt64(totalRoomCount).ToString();
                calenderReportListBO.Add(trCountBO);

                //// // ------------Expected Arrivals
                int eciCount = reservationList.Where(x => x.DateIn.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

                RoomCalenderBO eciCountBO = new RoomCalenderBO();
                eciCountBO.TransactionDate = dateRow;
                eciCountBO.ServiceType = "Expected Arrivals";
                eciCountBO.ServiceQuantity = eciCount;
                eciCountBO.DisplayQuantity = Convert.ToInt64(eciCount).ToString();
                calenderReportListBO.Add(eciCountBO);

                // // ------------In House Guest
                RoomCalenderBO ihCountBO = new RoomCalenderBO();
                ihCountBO.TransactionDate = dateRow;
                ihCountBO.ServiceType = "Stay On (In House)";
                ihCountBO.ServiceQuantity = todaysInhouse;
                ihCountBO.DisplayQuantity = Convert.ToInt64(todaysInhouse).ToString();
                calenderReportListBO.Add(ihCountBO);

                int ecoCount = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

                // // ------------Expected Departure
                RoomCalenderBO ecoCountBO = new RoomCalenderBO();
                ecoCountBO.TransactionDate = dateRow;
                ecoCountBO.ServiceType = "Expected Departure";
                ecoCountBO.ServiceQuantity = ecoCount;
                ecoCountBO.DisplayQuantity = Convert.ToInt64(ecoCount).ToString();
                calenderReportListBO.Add(ecoCountBO);

                // // ------------Occupency Count
                int ocCount = todaysInhouse + eciCount - ecoCount;

                RoomCalenderBO ocCountBO = new RoomCalenderBO();
                ocCountBO.TransactionDate = dateRow;
                ocCountBO.ServiceType = "Occupency";
                ocCountBO.ServiceQuantity = ocCount;
                ocCountBO.DisplayQuantity = Convert.ToInt64(ocCount).ToString();
                calenderReportListBO.Add(ocCountBO);

                ///-----Out Of Order
                if (dateRow.Date == DateTime.Now.Date)
                    outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Order").ToList();
                else
                    outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Order").ToList();
                int outOfOrderCount = outOfOrderLog.Select(r => r.RoomId).Distinct().Count();
                RoomCalenderBO oooBO = new RoomCalenderBO();
                oooBO.TransactionDate = dateRow;
                oooBO.ServiceType = "Out Of Order";
                oooBO.ServiceQuantity = outOfOrderCount;
                oooBO.DisplayQuantity = outOfOrderCount.ToString();
                calenderReportListBO.Add(oooBO);

                ///-----Out Of Service
                if (dateRow.Date == DateTime.Now.Date)
                    outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Service").ToList();
                else
                    outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Service").ToList();
                int outOfServiceCount = outOfServiceLog.Select(r => r.RoomId).Distinct().Count();
                RoomCalenderBO oocBO = new RoomCalenderBO();
                oocBO.TransactionDate = dateRow;
                oocBO.ServiceType = "Out Of Service";
                oocBO.ServiceQuantity = outOfServiceCount;
                oocBO.DisplayQuantity = outOfServiceCount.ToString();
                calenderReportListBO.Add(oocBO);

                ///Total Room Position
                int positionCount = totalRoomCount - ocCount - outOfOrderCount - outOfServiceCount;
                RoomCalenderBO positionBO = new RoomCalenderBO();
                positionBO.TransactionDate = dateRow;
                positionBO.ServiceType = "Position";
                positionBO.ServiceQuantity = positionCount;
                positionBO.DisplayQuantity = positionCount.ToString();
                calenderReportListBO.Add(positionBO);

                // // ------------Occupency Percentage (%)
                decimal ocpCount = Convert.ToDecimal((ocCount * 100) / totalRoomCount);

                RoomCalenderBO ocpCountBO = new RoomCalenderBO();
                ocpCountBO.TransactionDate = dateRow;
                ocpCountBO.ServiceType = "Occupency %";
                ocpCountBO.ServiceQuantity = ocpCount;
                ocpCountBO.DisplayQuantity = ocpCount.ToString() + "%";
                calenderReportListBO.Add(ocpCountBO);

                RoomCalenderBO blankCountBO = new RoomCalenderBO();
                blankCountBO.TransactionDate = dateRow;
                blankCountBO.ServiceType = "";
                blankCountBO.ServiceQuantity = -99999;
                blankCountBO.DisplayQuantity = "-99999";
                calenderReportListBO.Add(blankCountBO);

                RoomCalenderBO roomTypeBO = new RoomCalenderBO();
                roomTypeBO.TransactionDate = dateRow;
                roomTypeBO.ServiceType = "Room Type";
                roomTypeBO.ServiceQuantity = 0;// dateRow.Day;
                roomTypeBO.DisplayQuantity = ""; //dateRow.Day.ToString();
                calenderReportListBO.Add(roomTypeBO);

                foreach (RoomTypeBO rType in roomTypeList)
                {
                    int eciCountRT = reservationList.Where(x => x.DateIn.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);

                    int ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
                    rType.OccupiedRoomCount += eciCountRT - ecoCountRT;

                    outOfOrderCount = outOfOrderLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();
                    outOfServiceCount = outOfServiceLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();

                    rType.AvailableRoomCount = rType.TotalRoom - rType.OccupiedRoomCount - outOfOrderCount - outOfServiceCount;

                    RoomCalenderBO ocpCountRoomTypeBO = new RoomCalenderBO();
                    ocpCountRoomTypeBO.TransactionDate = dateRow;
                    ocpCountRoomTypeBO.ServiceType = rType.RoomType;
                    ocpCountRoomTypeBO.ServiceQuantity = rType.AvailableRoomCount;
                    ocpCountRoomTypeBO.DisplayQuantity = rType.AvailableRoomCount.ToString();
                    calenderReportListBO.Add(ocpCountRoomTypeBO);
                }

                //foreach (RoomTypeBO rType in roomTypeList)
                //{
                //    int eciCountRT = 0, ecoCountRT = 0;

                //    if (dateRow.Date == DateTime.Now.Date)
                //    {
                //        eciCountRT = reservationList.Where(x => x.DateIn.Date == DateTime.Now.Date && x.DateIn >= DateTime.Now && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
                //        ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == DateTime.Now.Date && x.ExpectedCheckOutDate >= DateTime.Now && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut == DateTime.Now && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
                //    }
                //    else
                //    {
                //        eciCountRT = reservationList.Where(x => x.DateIn.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
                //        ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
                //    }
                //    rType.OccupiedRoomCount += eciCountRT - ecoCountRT;

                //    outOfOrderCount = outOfOrderLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();
                //    outOfServiceCount = outOfServiceLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();

                //    rType.AvailableRoomCount = rType.TotalRoom - rType.OccupiedRoomCount - outOfOrderCount - outOfServiceCount;

                //    RoomCalenderBO ocpCountRoomTypeBO = new RoomCalenderBO();
                //    ocpCountRoomTypeBO.TransactionDate = dateRow;
                //    ocpCountRoomTypeBO.ServiceType = rType.RoomType;
                //    ocpCountRoomTypeBO.ServiceQuantity = rType.AvailableRoomCount;
                //    ocpCountRoomTypeBO.DisplayQuantity = rType.AvailableRoomCount.ToString();
                //    calenderReportListBO.Add(ocpCountRoomTypeBO);
                //}

                //int positionCount = totalRoomCount - ocCount;
                RoomCalenderBO vacantBO = new RoomCalenderBO();
                vacantBO.TransactionDate = dateRow;
                vacantBO.ServiceType = "Total Vacant";
                vacantBO.ServiceQuantity = positionCount;
                vacantBO.DisplayQuantity = positionCount.ToString();
                calenderReportListBO.Add(vacantBO);

                todaysInhouse = ocCount;
            }//*/

            foreach (var item in calenderReportListBO.Where(w => w.TransactionDate < DateTime.Today))
            {
                item.ServiceQuantity = 0;
                item.DisplayQuantity = "";
            }

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], calenderReportListBO));
            rvTransaction.LocalReport.DisplayName = "Hotel Forecast Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        private void ExtraAdultsOrChildInfo()
        {
            _RoomStatusInfoByDate = 1;
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            DateTime dateTime = DateTime.Now;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                dateTime = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            string guestName = string.Empty, companyName = string.Empty, passportNo = string.Empty, mobileNo = string.Empty;

            var reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptHotelExtraBedReport.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> reportParam = new List<ReportParameter>();

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

            HMCommonDA hmCommonDA = new HMCommonDA();

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(reportParam);

            RoomRegistrationDA roomRegDA = new RoomRegistrationDA();
            List<GuestHouseInfoForReportBO> complimentaryGuestList = new List<GuestHouseInfoForReportBO>();
            complimentaryGuestList = roomRegDA.GetFrontOfficeExtraBedInfoForReport(dateTime, dateTime);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], complimentaryGuestList));

            rvTransaction.LocalReport.DisplayName = "Extra Bed Report";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        private void WalkInRoomInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime currentDate = DateTime.Now;

            List<ReportParameter> paramReport = new List<ReportParameter>();
            List<WalkInGuestReportBO> guestInfoBOs = new List<WalkInGuestReportBO>();
            GuestRegistrationDA guestDA = new GuestRegistrationDA();

            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                currentDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            startDate = hmUtility.GetStringFromDateTime(currentDate);
            endDate = hmUtility.GetStringFromDateTime(currentDate);

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            guestInfoBOs = guestDA.GetWalkInGuestReport_SP(FromDate, ToDate, string.Empty, string.Empty);
            var rowCount = guestInfoBOs.Count;

            //add report info
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptWalkInGuest.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            paramReport.Add(new ReportParameter("RowCount", rowCount.ToString()));

            string reportName = "Walk In Guests";
            paramReport.Add(new ReportParameter("ReportName", reportName));

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestInfoBOs));

            rvTransaction.LocalReport.DisplayName = "" + reportName + " Report";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        private void GuestBlockInfo()
        {
            _RoomStatusInfoByDate = 1;

            string startDate = string.Empty, endDate = string.Empty;
            string company = string.Empty, reference = string.Empty, reservationMode = string.Empty, roomNumber = string.Empty, filterBy = string.Empty;
            company = "";
            reference = "";
            reservationMode = "";
            roomNumber = "";
            filterBy = "Booking";
            int reservedBy = 0;
            startDate = DateTime.Now.ToString("dd/MM/yyy");
            endDate = DateTime.Now.ToString("dd/MM/yyy");

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                startDate = guestLedgerInfoBO.InhouseGuestLedgerDate.ToString("dd/MM/yyy");
                endDate = guestLedgerInfoBO.InhouseGuestLedgerDate.ToString("dd/MM/yyy");
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptRoomReservation.rdlc");

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

            string filterByText = string.Empty;
            filterByText = "Booking Date";

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FilterByText", filterByText));

            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            string reservationNumber = string.Empty;
            RoomReservationDA purchaseDa = new RoomReservationDA();
            List<RoomReservationInfoByDateRangeReportBO> rommStatus = new List<RoomReservationInfoByDateRangeReportBO>();
            rommStatus = purchaseDa.GetRoomReservationInfoByDateRange(FromDate, ToDate, reservationNumber, company, reference, reservationMode, roomNumber, filterBy, reservedBy);
            rommStatus = rommStatus.Where(p => p.RoomNumberList != "Unassigned").ToList();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], rommStatus));
            rvTransaction.LocalReport.DisplayName = "Room Reservation Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            string reportType = HMConstants.PrintPageType.Landscape.ToString();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, reportType);
            frmPrint.Attributes["src"] = reportSource;
        }

    }
}