using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmExpectedArrivalSearch : BasePage
    {
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //************************ User Defined Function ********************//
        public static List<RoomCalenderBO> GetRoomCalenderList(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(StartDate, EndDate);
            return calenderList;
        }
        public static string GetReservationInformationView(RoomReservationBO reservationBO)
        {
            HMUtility hmUtility = new HMUtility();
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive table-hover'  id='TableReservationInformation'>";
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
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ContactNumber + "</td>";
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
        public static string GetGuestInformationView(List<GuestInformationBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive table-hover' id='TableGuestInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";

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
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string SearchNLoadReservationInfo(int reservationId, string guestName, string companyName, string reservNumber, string checkInDate)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            DateTime checkIn = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(checkInDate))
            {
                checkIn = hmUtility.GetDateTimeFromString(checkInDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            int counterReservationDetail = 0;
            string strReservationDetailTable = "";
            ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
            List<ReservationDetailBO> reservationDetailListBOForGrid = new List<ReservationDetailBO>();
            reservationDetailListBOForGrid = reservationDetailDA.GetRoomReservationInfoByExpectedArrivalDate(checkIn, guestName, companyName, reservNumber);
            if (reservationDetailListBOForGrid != null)
            {
                if (reservationDetailListBOForGrid.Count > 0)
                {
                    {
                        //string strTable = "";
                        string reserVationDetailsId = string.Empty;
                        strReservationDetailTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                        strReservationDetailTable += "<th align='left' scope='col'>Reservation Number</th><th align='left' scope='col'>Room Type</th><th align='left' scope='col'>Room Count</th><th align='left' scope='col'>Pax</th><th align='left' scope='col'>Check In</th><th align='left' scope='col'>Exp. Check Out</th></tr></thead> <tbody>";

                        foreach (ReservationDetailBO dr in reservationDetailListBOForGrid)
                        {
                            if (counterReservationDetail % 2 == 0)
                            {
                                // It's even
                                strReservationDetailTable += "<tr style='background-color:#E3EAEB;'>";
                            }
                            else
                            {
                                // It's odd
                                strReservationDetailTable += "<tr style='background-color:White;'>";
                            }

                            if (dr.ReservationDetailId != 0)
                                reserVationDetailsId = dr.ReservationDetailId.ToString();
                            else
                                reserVationDetailsId = "0";

                            strReservationDetailTable += "<td align='left' style='display:none;'>" + dr.RoomTypeId + "</td>";
                            strReservationDetailTable += "<td align='left' style='display:none;'>" + reserVationDetailsId + "</td>";
                            strReservationDetailTable += "<td align='left' style='width: 20%;' onClick='javascript:return ActionForExpressCheckInn(" + dr.ReservationId + "," + dr.RoomTypeId + ")'>" + dr.ReservationNumber + "</td>";
                            strReservationDetailTable += "<td align='left' style='width: 40%;' onClick='javascript:return ActionForExpressCheckInn(" + dr.ReservationId + "," + dr.RoomTypeId + ")'>" + dr.RoomType + "</td>";
                            strReservationDetailTable += "<td align='left' style='width: 10%;' onClick='javascript:return ActionForExpressCheckInn(" + dr.ReservationId + "," + dr.RoomTypeId + ")'>" + dr.TotalRoom + "</td>";
                            strReservationDetailTable += "<td align='left' style='width: 10%;' onClick='javascript:return ActionForExpressCheckInn(" + dr.ReservationId + "," + dr.RoomTypeId + ")'>" + dr.RoomTypeWisePaxQuantity + "</td>";
                            strReservationDetailTable += "<td align='left' style='width: 10%;' onClick='javascript:return ActionForExpressCheckInn(" + dr.ReservationId + "," + dr.RoomTypeId + ")'>" + dr.ArrivalDate + "</td>";
                            strReservationDetailTable += "<td align='left' style='width: 10%;' onClick='javascript:return ActionForExpressCheckInn(" + dr.ReservationId + "," + dr.RoomTypeId + ")'>" + dr.DepartureDate + "</td>";

                            strReservationDetailTable += "</td></tr>";
                            counterReservationDetail++;
                        }
                        strReservationDetailTable += "</tbody></table>";
                        if (strReservationDetailTable == "")
                        {
                            strReservationDetailTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                        }
                    }
                }
            }

            return strReservationDetailTable;
        }
        [WebMethod]
        public static string LoadReservedPossiblePath(int reservationId, int roomId, string PageTitle)
        {
            string strTable = string.Empty;
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            RoomReservationBO reservationBO = new RoomReservationBO();
            RoomReservationDA reservationDA = new RoomReservationDA();
            reservationBO = reservationDA.GetRoomReservationInfoById(reservationId);
            if (reservationBO != null)
            {
                List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
                list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "ReservedPossiblePath");

                RoomStatusPossiblePathViewBO roomAssignmentBO = new RoomStatusPossiblePathViewBO();
                roomAssignmentBO.DisplayOrder = 5;
                roomAssignmentBO.DisplayText = "Room Assignment";
                roomAssignmentBO.PathId = 5;
                roomAssignmentBO.PossiblePath = "/HotelManagement/frmReservationRoomAssignment.aspx";
                list.Add(roomAssignmentBO);

                RoomStatusPossiblePathViewBO expressCheckInBO = new RoomStatusPossiblePathViewBO();
                expressCheckInBO.DisplayOrder = 6;
                expressCheckInBO.DisplayText = "Express Check In";
                expressCheckInBO.PathId = 6;
                expressCheckInBO.PossiblePath = "/HotelManagement/frmExpressCheckIn.aspx";
                list.Add(expressCheckInBO);

                RoomCalenderBO calenderBO = new RoomCalenderBO();
                if (roomId != 0)
                {
                    calenderBO = GetRoomCalenderList(reservationBO.DateIn, reservationBO.DateIn.AddDays(1)).Where(x => x.RoomId == roomId && x.TransectionStatus == "Reservation").FirstOrDefault();
                }
                else
                {
                    calenderBO = GetRoomCalenderList(reservationBO.DateIn, reservationBO.DateIn.AddDays(1)).Where(x => x.TransectionId == reservationId && x.TransectionStatus == "Reservation").FirstOrDefault();
                }

                if (calenderBO != null)
                {
                    int row = 1;
                    strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].DisplayText == "Registration")
                        {
                            if (reservationBO.DateIn.Date == DateTime.Now.Date)
                            {
                                if (row == 1)
                                    strTable += "<div class='form-group'>";

                                strTable += "<div class='col-md-4'>";
                                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                                if (list[i].DisplayText == "Details")
                                {
                                    strTable += " onclick=\"return LoadReservationDetails('" + calenderBO.TransectionId + "' );\"  />";
                                    strTable += "</div>";
                                }
                                else if (list[i].DisplayText.Trim() == "Reservation")
                                {
                                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?editId=" + calenderBO.TransectionId + "';\"  />";
                                    strTable += "</div>";
                                }
                                else if (list[i].DisplayText.Trim() == "Room Assignment")
                                {
                                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "';\"  />";
                                    strTable += "</div>";
                                }
                                else if (list[i].DisplayText.Trim() == "Express Check In")
                                {
                                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "';\"  />";
                                    strTable += "</div>";
                                }
                                else if (list[i].DisplayText.Trim() == "Reservation Payment")
                                {
                                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "&rId=" + roomId + "';\"  />";
                                    strTable += "</div>";
                                }
                                else if (list[i].DisplayText.Trim() == "Pre Registration Card")
                                {
                                    var url = list[i].PossiblePath + "?ReservationId=" + calenderBO.TransectionId;
                                    strTable += " onclick=\"return PreRegistrationCardGenerate('" + url + "' );\"  />";
                                    strTable += "</div>";
                                }
                                else if (list[i].DisplayText.Trim() == "Registration")
                                {
                                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?SelectedRoomNumber=" + roomId + "&source=Reservation';\"  />";
                                    strTable += "</div>";
                                }

                                if (row == 3)
                                {
                                    strTable += "</div>";
                                    row = 0;
                                }

                                row++;
                            }
                        }
                        else
                        {
                            if (row == 1)
                                strTable += "<div class='form-group'>";

                            strTable += "<div class='col-md-4'>";
                            strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                            if (list[i].DisplayText == "Details")
                            {
                                strTable += " onclick=\"return LoadReservationDetails('" + calenderBO.TransectionId + "' );\"  />";
                                strTable += "</div>";
                            }
                            else if (list[i].DisplayText.Trim() == "Reservation")
                            {
                                strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?editId=" + calenderBO.TransectionId + "';\"  />";
                                strTable += "</div>";
                            }
                            else if (list[i].DisplayText.Trim() == "Reservation Payment")
                            {
                                strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "&rId=" + roomId + "';\"  />";
                                strTable += "</div>";
                            }
                            else if (list[i].DisplayText.Trim() == "Room Assignment")
                            {
                                strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "';\"  />";
                                strTable += "</div>";
                            }
                            else if (list[i].DisplayText.Trim() == "Express Check In")
                            {
                                strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "';\"  />";
                                strTable += "</div>";
                            }
                            else if (list[i].DisplayText.Trim() == "Pre Registration Card")
                            {
                                var url = list[i].PossiblePath + "?ReservationId=" + calenderBO.TransectionId;
                                strTable += " onclick=\"return PreRegistrationCardGenerate('" + url + "' );\"  />";
                                strTable += "</div>";
                            }

                            if (row == 3)
                            {
                                strTable += "</div>";
                                row = 0;
                            }

                            row++;
                        }
                    }
                    strTable += "</div></div>";
                }
            }
            return strTable;
        }
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
    }
}