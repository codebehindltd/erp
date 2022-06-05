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
    public partial class frmReportRoomAvailableStatus : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlRoomStatus.Value = "0";
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
           
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            string startDate = string.Empty;
            string endDate = string.Empty;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

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

            DateTime fromDate = DateTime.Today;
            List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(fromDate, ToDate);
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
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {

            DateTime startDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime endDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            List<RoomAvailableStatusBO> avalibleRoomList = new List<RoomAvailableStatusBO>();
            //avalibleRoomList = GetRoomWithAvailableStatusByDate(startDate, endDate);
            HMCommonDA hmCommonDA = new HMCommonDA();
            avalibleRoomList = hmCommonDA.GetRoomWithAvailableStatusByDate(startDate, endDate, ddlReportType.SelectedValue.ToString());
            e.DataSources.Add(new ReportDataSource("AvailableRoomStatus", avalibleRoomList));
        }
        public int GetAvailableRoomByTypeAndDate(DateTime date, int roomTypeId, List<RoomCalenderBO> calenderList, string roomType)
        {
            RoomNumberDA numberDa = new RoomNumberDA();
            var RoomListNumber = numberDa.GetRoomNumberInfoByRoomType(roomTypeId);

            int totalRoom = RoomListNumber.Count();
            //var List = calenderList.Where(c => c.RoomType == type.RoomType && (hmUtility.GetStringFromDateTime(c.CheckIn) == hmUtility.GetStringFromDateTime(date)));
            var List = calenderList.Where(c => c.RoomType == roomType && (c.CheckOut > date && c.CheckIn <= date));
            int count = List.Count();
            return totalRoom - count;
        }
        public DataTable GetDataTableWithCollumn()
        {
            // Create new DataTable and DataSource objects.
            DataTable table = new DataTable();

            // Declare DataColumn and DataRow variables.
            DataColumn column;
            DataRow row;
            DataView view;
            List<RoomTypeBO> typeList = new List<RoomTypeBO>();
            RoomTypeDA typeDA = new RoomTypeDA();
            typeList = typeDA.GetRoomTypeInfo();

            // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Date";
            table.Columns.Add(column);

            foreach (RoomTypeBO typeBO in typeList)
            {
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = typeBO.RoomType.Trim();
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = Type.GetType("System.Int32");
                column.ColumnName = typeBO.RoomType.Trim() + "Count";
                table.Columns.Add(column);
            }
            return table;

        }
    }
}