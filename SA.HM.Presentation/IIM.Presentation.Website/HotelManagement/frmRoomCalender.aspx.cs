using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity;
using HotelManagement.Data;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website
{
    public partial class frmRoomCalender : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCurrentDate();
                this.LoadCalenderGridView();
            }
        }
        protected void btnViewCalender_Click(object sender, EventArgs e)
        {
            this.LoadCalenderGridView();
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmRoomCalender.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadCurrentDate()
        {
            DateTime processDate = DateTime.Now;
            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                processDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            this.txtCurrentDate.Text = hmUtility.GetStringFromDateTime(processDate);
        }

        public static List<RoomCalenderBO> GetRoomCalenderList(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(StartDate, EndDate);

            List<RoomCalenderBO> CheckedOutVacantDirtyRoomInfoList = new List<RoomCalenderBO>();
            CheckedOutVacantDirtyRoomInfoList = calenderDA.GetCheckedOutVacantDirtyRoomInfoForCalender();

            calenderList.AddRange(CheckedOutVacantDirtyRoomInfoList);
            return calenderList;
        }
        private void LoadCalenderGridView()
        {
            RoomNumberBO roomBO = new RoomNumberBO();
            RoomNumberDA roomDA = new RoomNumberDA();
            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                dateTime = guestLedgerInfoBO.InhouseGuestLedgerDate;
                StartDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            int range = 6;
            if (this.ddlDuration.SelectedIndex == 0)
            {
                range = 6;
            }
            else if (this.ddlDuration.SelectedIndex == 1)
            {
                range = 13;
            }
            else if (this.ddlDuration.SelectedIndex == 2)
            {
                range = 30;
            }
            if (!string.IsNullOrWhiteSpace(this.txtCurrentDate.Text))
            {
                StartDate = hmUtility.GetDateTimeFromString(this.txtCurrentDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                this.txtCurrentDate.Text = hmUtility.GetStringFromDateTime(dateTime);
                StartDate = hmUtility.GetDateTimeFromString(this.txtCurrentDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            DateTime EndDate = StartDate.AddDays(range);
            var List = roomDA.GetRoomNumberInfoForCalender(StartDate, EndDate);

            List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(StartDate, EndDate);
            List<RoomCalenderBO> calenderList = GetRoomCalenderList(StartDate, EndDate);

            var outOfOrderRooms = (from item in List
                                   where item.StatusId == 3
                                   select item).ToList();

            foreach (RoomCalenderBO row in calenderList)
            {
                if (row.CheckOut.Date == dateTime.Date)
                {
                    if (row.ActiveStatus == "Possible Vacant")
                    {
                        //row.CheckOut = row.CheckOut.AddDays(-1);
                    }
                }
                else if (row.TransectionStatus == "Reservation")
                {
                    row.CheckOut = row.CheckOut.AddDays(-1);
                }
                else if (row.TransectionStatus == "InHouse")
                {
                    row.CheckOut = row.CheckOut.AddDays(-1);
                }
            }

            this.GenerateHTMLGuestGridView(List, DateList, calenderList);
        }
        private void GenerateHTMLGuestGridView(List<RoomNumberBO> roomList, List<DateTime> DateList, List<RoomCalenderBO> calenderList)
        {
            RoomNumberDA entityDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberInfoBO = entityDA.GetRoomNumberInfo();

            string strTable = "";
            int dateCount = DateList.Count;
            int totalCol = dateCount + 2;

            strTable += "<table class='table table-bordered' id='fixTable' style='table-layout: fixed;'>";
            int counter = 0;
            strTable += "<thead><tr style='background-color:#E3EAEB;'>";
            strTable += "<th color='white' bgcolor='#457EA4' align='left' style='width: 100px; cursor:pointer' ><font color='white'><b>Room No.</b></font> </th>";
            strTable += "<th bgcolor='#457EA4' align='left' style='width: 100px; cursor:pointer' ><font color='white'><b>Type</b></font></th>";
            foreach (DateTime date in DateList)
            {
                string dayDate = date.ToString("dddd");
                string dayName = date.ToString("dd-MMM-yy");
                strTable += "<th bgcolor='#457EA4' align='left' style='width: 100px; cursor:pointer;' ><font color='white'><b>" + dayName + "</br>(" + dayDate + ")" + "</b></font></th>";
            }
            strTable += "</tr></thead><tbody>";
            foreach (RoomNumberBO roomItem in roomList)
            {
                string tooltipContent = this.hmUtility.GetTooltipContainer(roomItem.RoomId.ToString(), roomItem.RoomNumber, roomItem.RoomType, roomItem.RoomName);

                if (roomItem.GroupByRowNo == 1)
                {
                    strTable += "<tr style='background-color:#023C61; color:white;'>";
                    strTable += "<td align='left' colspan='" + totalCol.ToString() + "' style='width: 100px; cursor:pointer' >" + roomItem.FloorAndBlockName + "</td>";
                    strTable += "</tr>";
                }

                counter++;
                strTable += "<tr style='background-color:#E3EAEB;' id='" + roomItem.RoomId + "'>";

                strTable += "<td align='left' style='width: 175px; cursor:pointer' class='ToolTipClass'>";
                strTable += tooltipContent;
                strTable += roomItem.RoomNumber;
                strTable += "</td>";
                strTable += "<td align='left' style='width: 175px; cursor:pointer'  class='ToolTipClass'>" + roomItem.RoomTypeOrCode + "</td>";

                int dayCount = 0;
                foreach (DateTime date in DateList)
                {
                    var List = calenderList.Where(c => c.RoomId == roomItem.RoomId && (c.CheckIn.Date <= date.Date) && (c.CheckOut > date)).ToList();
                    if (List.Count > 0)
                    {
                        int reservation = 1;
                        if (List[0].TransectionStatus.ToString() == "InHouse")
                        {
                            reservation = 1;
                        }
                        else if (List[0].TransectionStatus.ToString() == "CheckedOut")
                        {
                            reservation = 2;
                        }
                        else if (List[0].TransectionStatus.ToString() == "Reservation")
                        {
                            reservation = 2;
                        }
                        else if (List[0].TransectionStatus.ToString() == "OutOfOrder")
                        {
                            reservation = 3;
                        }
                        else if (List[0].TransectionStatus.ToString() == "OutOfService")
                        {
                            reservation = 4;
                        }
                        if (List[0].TransectionStatus.ToString() == "InHouse")
                        {
                            strTable += "<td class='InHouse ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + tooltipContent + List[0].GuestName + "</td>";
                        }
                        else if (List[0].TransectionStatus.ToString() == "CheckedOut")
                        {
                            strTable += "<td class='CheckedOut ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";'\">" + tooltipContent + List[0].GuestName + "</td>";
                        }
                        else if (List[0].TransectionStatus.ToString() == "Reservation")
                        {
                            var roomIdListBO = roomNumberInfoBO.Where(x => x.RoomId == roomItem.RoomId & x.StatusId == 1 & x.HKRoomStatusId == 2 & x.CleanupStatus == "Not Cleaned").FirstOrDefault();
                            if (roomIdListBO != null)
                            {
                                strTable += "<td class='CheckedOut ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";'\">" + tooltipContent + "***" + "</td>";
                            }
                            else
                            {
                                strTable += "<td class='Reservation ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + tooltipContent + List[0].GuestName + "</td>";
                            }
                        }
                        else if (List[0].TransectionStatus.ToString() == "OutOfOrder")
                        {
                            strTable += "<td class='OutOfOrder ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + tooltipContent + List[0].GuestName + "</td>";
                        }
                        else if (List[0].TransectionStatus.ToString() == "OutOfService")
                        {
                            strTable += "<td class='OutOfService ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + tooltipContent + List[0].GuestName + "</td>";
                        }
                    }
                    else
                    {
                        strTable += "<td align='left' class='ToolTipClass' style='width: 100px; cursor:pointer' onClick=\"javascript:return LoadReservation('" + dayCount + "','" + roomItem.RoomNumber + "')\"></td>";
                    }

                    dayCount = dayCount + 1;
                }
                strTable += "</tr>";
            }
            strTable += "</tbody></table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            this.parent.InnerHtml = strTable;
        }
        //Reservation 
        [WebMethod]
        public static string GetReservationformationByReservationId(int ReservationId)
        {
            RoomReservationBO reservationBO = new RoomReservationBO();
            RoomReservationDA reservationDA = new RoomReservationDA();
            reservationBO = reservationDA.GetRoomReservationInfoById(ReservationId);
            return GetReservationInformationView(reservationBO);
        }
        [WebMethod]
        public static string GetReservationGuestInformationByReservationId(int ReservationId)
        {
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResId(Convert.ToInt32(ReservationId), false);
            return GetGuestInformationView(guestInformationList);
        }
        [WebMethod]
        public static RoomReservationBO GetReservationAirportPicUpInformationByReservationId(int ReservationId)
        {
            RoomReservationBO reservationBO = new RoomReservationBO();
            RoomReservationDA reservationDA = new RoomReservationDA();
            reservationBO = reservationDA.GetRoomReservationInfoById(ReservationId);
            return reservationBO;
        }
        [WebMethod]
        public static string GetReservationComplementaryInformationByReservationId(int ReservationId)
        {
            List<HMComplementaryItemBO> complementaryList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
            complementaryList = complementaryItemDA.GetComplementaryItemInfoByReservationId(ReservationId);
            return GetComplementaryItemView(complementaryList);
        }
        // Registration
        [WebMethod]
        public static string GetRegistrationInformationByRegistrationId(int RegistrationId)
        {
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            registrationBO = registrationDA.GetRoomRegistrationInfoById(RegistrationId);
            return GetRegistrationInformationView(registrationBO);
        }
        [WebMethod]
        public static string GetRegistrationGuestInformationByRegistrationId(int RegistrationId)
        {
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByRegiId(RegistrationId);
            return GetGuestInformationView(guestInformationList);
        }
        [WebMethod]
        public static RoomRegistrationBO GetRegistrationAirportPicUpInformationByRegistrationId(int RegistrationId)
        {
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            registrationBO = registrationDA.GetRoomRegistrationInfoById(RegistrationId);
            return registrationBO;
        }
        [WebMethod]
        public static string GetRegistrationComplementaryInformationByRegistrationId(int RegistrationId)
        {
            List<HMComplementaryItemBO> complementaryList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
            complementaryList = complementaryItemDA.GetComplementaryItemInfoByRegistrationId(RegistrationId);
            return GetComplementaryItemView(complementaryList);
        }
        [WebMethod]
        public static string LoadOutOfOrderPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInfoByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();
            List<CustomFieldBO> list = new List<CustomFieldBO>();
            list = hmCommonDA.GetCustomFieldList("OutOfOrderPossiblePath");
            List<string[]> stringList = new List<string[]>();
            for (int i = 0; i < list.Count; i++)
            {
                string[] tokens = list[i].FieldValue.Split('~');
                stringList.Add(tokens);
            }

            string strTable = string.Empty;

            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < stringList.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;width:150px'>";
                strTable += "<input type='button' style='width:150px' value='" + stringList[i][0] + "' class='TransactionalButton btn btn-primary'";

                if (stringList[i][0] == "Details")
                {
                    strTable += " onclick=\"return ShowOutOfServiceRoomInformation('" + RoomNember + "' );\"  />";
                }
                else
                {
                    strTable += " onclick=\" location.href='" + stringList[i][1] + "?RoomId=" + numberBO.RoomId + "';\"  />";
                }

                strTable += "</div>";
            }

            strTable += "</div>";
            return strTable;
        }
        [WebMethod]
        public static RoomNumberBO ShowOutOfServiceRoomInformation(string roomNumber)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = new RoomNumberBO();
            numberBO = numberDA.GetRoomInfoByRoomNumber(roomNumber);
            return numberBO;
        }
        //Html Genaration 
        public static string GetComplementaryItemView(List<HMComplementaryItemBO> List)
        {
            string strTable = "";
            if (List.Count > 0)
            {
                strTable += "<ul>";
                foreach (HMComplementaryItemBO item in List)
                {
                    strTable += "<li>";
                    strTable += item.ItemName;
                    strTable += "</li>";
                }
                strTable += "<ul>";

            }
            return strTable;
        }
        public static string GetGuestInformationView(List<GuestInformationBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableGuestInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";

            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th> <th align='left' scope='col'>Phone</th><th align='left' scope='col'>Address</th> <th align='left' scope='col'>Country</th></tr>";
            int counter = 0;

            foreach (GuestInformationBO item in List)
            {
                counter++;
                strTable += "<tr style='background-color:#E3EAEB;'>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestName + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestPhone + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestAddress1 + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.CountryName + "</td>";
                strTable += "</tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        public static string GetRegistrationInformationView(RoomRegistrationBO registrationBO)
        {
            HMUtility hmUtility = new HMUtility();
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableRegistrationInformation'>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Registration Number</td>";
            strTable += "<td align='left' style='width: 25%'>: " + registrationBO.RegistrationNumber + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Arrive Date</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(registrationBO.ArriveDate) + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Room Tariff</td>";
            strTable += "<td align='left' style='width: 25%'>: " + registrationBO.Currency + "  " + registrationBO.UnitPrice + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Expected Deperture</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(registrationBO.ExpectedCheckOutDate) + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Company</td>";
            strTable += "<td align='left' style='width: 25%'>: " + registrationBO.CompanyName + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%'></td>";
            strTable += "</tr>";

            strTable += "</table>";
            return strTable;
        }
        public static string GetReservationInformationView(RoomReservationBO reservationBO)
        {
            HMUtility hmUtility = new HMUtility();
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableReservationInformation'>";
            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Reservation Number:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ReservationNumber + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Reservation Date</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.ReservationDate) + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Contact Person:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ContactPerson + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Contact Number:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.MobileNumber + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Check In:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.DateIn) + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Check Out:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.DateOut) + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Company</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.CompanyName + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%'></td>";
            strTable += "</tr>";

            strTable += "</table>";
            return strTable;
        }
        public static string GetRegistrationOtherInformation(RoomRegistrationBO registrationBO)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableOtherInformation'>";
            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Comming From:</td>";
            strTable += "<td align='left' style='width: 25%'>" + registrationBO.CommingFrom + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Next Destination</td>";
            strTable += "<td align='left' style='width: 25%'>" + registrationBO.NextDestination + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Visit Purpose:</td>";
            strTable += "<td align='left' style='width: 25%'>" + registrationBO.VisitPurpose + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%'></td>";
            strTable += "</tr>";
            strTable += "</table>";

            strTable += "<div style='clear:both'></div>";

            strTable += "<table cellspacing='0' width='100%' cellpadding='4' id='TableRegistrationInformation'>";
            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Departure Airline Name:</td>";
            strTable += "<td align='left' style='width: 25%'>" + registrationBO.DepartureFlightName + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Departure Flight Numbere</td>";
            strTable += "<td align='left' style='width: 25%'>" + registrationBO.DepartureFlightNumber + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Departure time:</td>";
            strTable += "<td align='left' style='width: 25%'>" + registrationBO.DepartureTime + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%'></td>";
            strTable += "</tr>";

            strTable += "</table>";
            return strTable;
        }
        //new task
        [WebMethod]
        public static string LoadReservedPossiblePath(string RoomNember, int reservationId, string PageTitle, string type, DateTime date)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInfoByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "ReservedPossiblePath");
            var list1 = list.Where(a => a.DisplayText == "Reservation").ToList();
            var list2 = list.Where(a => a.DisplayText == "Registration").ToList();

            RoomReservationDA reservationDa = new RoomReservationDA();
            RoomReservationBO reservationBO = new RoomReservationBO();

            reservationBO = reservationDa.GetRoomReservationInfoById(reservationId);

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";

            if (type == "reservation")
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i].DisplayText.Trim() == "Reservation")
                    {
                        strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;width:150px'>";
                        strTable += "<input type='button' style='width:150px' value='" + list1[i].DisplayText + "' class='TransactionalButton btn btn-primary'";
                        strTable += " onclick=\" location.href='" + list1[i].PossiblePath + "';\"  />";
                    }
                    strTable += "</div>";
                }
            }
            else
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i].DisplayText.Trim() == "Reservation")
                    {
                        strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";
                        strTable += "<input type='button' style='width:150px' value='Edit Reservation' class='TransactionalButton btn btn-primary'";
                        strTable += " onclick=\" location.href='" + list1[i].PossiblePath + "?editId=" + reservationBO.ReservationId + "';\"  />";
                    }
                    strTable += "</div>";
                }


                HMCommonDA comonDa = new HMCommonDA();
                DateTime transactionDate = comonDa.GetModuleWisePreviousDayTransaction("GuestRoom");

                if (reservationBO.DateIn.ToString("d") == transactionDate.ToString("d"))
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        if (list2[i].DisplayText.Trim() == "Registration")
                        {
                            strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";
                            strTable += "<input type='button' style='width:150px' value='Check-In' class='TransactionalButton btn btn-primary'";
                            strTable += " onclick=\" location.href='" + list2[i].PossiblePath + "?SelectedRoomNumber=" + numberBO.RoomId + "&source=Reservation';\"  />";
                        }
                        strTable += "</div>";
                    }
                }
            }

            strTable += "</div>";
            return strTable;
        }

        [WebMethod]
        public static string LoadVacantPossiblePath(int dayCount, string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "VacantPossiblePath");

            string strTable = string.Empty;
            int row = 1;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            for (int i = 0; i < list.Count; i++)
            {
                if (row == 1)
                    strTable += "<div class='form-group'>";

                strTable += "<div class='col-md-4'>";

                if (list[i].DisplayText == "Details")
                {
                    list[i].DisplayText = "Reservation";
                }

                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                if (list[i].DisplayText == "Room Status Change")
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                    strTable += "</div>";
                }
                else if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return ShowRoomFeaturesInfo('" + RoomNember + "' );\"  />";
                    strTable += "</div>";
                }
                else if (list[i].DisplayText == "Reservation")
                {
                    string possiblePath = "/HotelManagement/frmRoomReservationNew.aspx";
                    strTable += " onclick=\" location.href='" + possiblePath + "?SRT=" + numberBO.RoomTypeId + "&SRN=" + numberBO.RoomId + "&DC=" + dayCount + "';\"  />";
                    strTable += "</div>";
                }
                else if (list[i].DisplayText.Trim() == "Registration")
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?SelectedRoomNumber=" + numberBO.RoomId + "&source=Registration';\"  />";
                    strTable += "</div>";
                }

                if (row == 3)
                {
                    strTable += "</div>";
                    row = 0;
                }

                row++;
            }
            strTable += "</div></div>";
            return strTable;
        }
    }
}