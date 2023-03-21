using HotelManagement.Data.HotelManagement;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity;
using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmExpressCheckIn : BasePage
    {
        protected int rsvnReservationId = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string rsvnId = Request.QueryString["rsvnId"];
            if (!string.IsNullOrEmpty(rsvnId))
            {
                RoomReservationBO roomReservationBO = new RoomReservationBO();
                RoomReservationDA roomReservationDA = new RoomReservationDA();
                List<HotelReservationAireportPickupDropBO> airportPickupDropList = new List<HotelReservationAireportPickupDropBO>();

                Int64 reservationId = Convert.ToInt64(rsvnId);
                roomReservationBO = roomReservationDA.GetRoomReservationInfoByIdNew(reservationId);
                if (roomReservationBO.ReservationId > 0)
                {
                    rsvnReservationId = 1;
                    txtSrcReservationNumber.Text = roomReservationBO.ReservationNumber;
                    txtSrcReservationNumber.Enabled = false;
                    btnSearch.Enabled = false;
                    btnReservationDetailSerach.Enabled = false;
                }
            }
            CheckPermission();
        }
        //************************ User Defined Method ********************//
        public static string GridHeader()
        {
            string gridHead = string.Empty;
            gridHead += "<table id='ExpressCheckInDetailsGrid' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 5%;'>" +
                         "                   <input id='chkAll' type='checkbox' value = 'chkExpressCheckIn' onclick='CheckAll()' onkeydown='if (event.keyCode == 13) {return true;}' style='vertical-align: middle;' />" +
                         "               </th>" +
                         "               <th style='width: 5%;'>" +
                         "                   #" +
                         "               </th>" +
                         "               <th style='width: 80%;'>" +
                         "                   Guest Name" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Room Number" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }
        public static List<RoomCalenderBO> GetRoomCalenderList(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(StartDate, EndDate);
            return calenderList;
        }
        private void CheckPermission()
        {
            btnExpressCheckIn.Visible = isSavePermission;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static RoomReservationBO ExpressCheckInnGridInformation(string prmReservationNumber, int prmRoomTypeId)
        {
            int rowCount = 0;
            string grid = string.Empty, tr = string.Empty;

            RoomReservationBO adjvw = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<RoomReservationBO> reservationDetailListBO = new List<RoomReservationBO>();
            List<RoomAssignDuplicationCheckVwBO> duplicateCheck = new List<RoomAssignDuplicationCheckVwBO>();
            if (prmRoomTypeId == 0)
            {
                reservationDetailListBO = roomReservationDA.GetRoomReservationInformationForRoomAssignment(prmReservationNumber).Where(x => x.DateIn.Date <= DateTime.Now.Date).ToList();
            }
            else
            {
                reservationDetailListBO = roomReservationDA.GetRoomReservationInformationForRoomAssignment(prmReservationNumber).Where(x => x.RoomTypeId == prmRoomTypeId && x.DateIn.Date <= DateTime.Now.Date).ToList();
            }

            if (reservationDetailListBO != null)
            {
                if (reservationDetailListBO.Count > 0)
                {
                    int counterReservationDetail = 0;
                    string strReservationDetailTable = "";
                    ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
                    List<ReservationDetailBO> reservationDetailListBOForGrid = new List<ReservationDetailBO>();

                    if (prmRoomTypeId == 0)
                    {
                        reservationDetailListBOForGrid = reservationDetailDA.GetReservationDetailByRegiIdForGrid(reservationDetailListBO[0].ReservationId, 0);
                    }
                    else
                    {
                        reservationDetailListBOForGrid = reservationDetailDA.GetReservationDetailByRegiIdForGrid(reservationDetailListBO[0].ReservationId, 0).Where(x => x.RoomTypeId == prmRoomTypeId).ToList();
                    }

                    if (reservationDetailListBOForGrid != null)
                    {
                        if (reservationDetailListBOForGrid.Count > 0)
                        {
                            {
                                string reserVationDetailsId = string.Empty;
                                strReservationDetailTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                                strReservationDetailTable += "<th align='left' scope='col'>Room Type</th><th align='left' scope='col'>Room Count</th><th align='left' scope='col'>Pax</th><th align='left' scope='col'>Check In</th><th align='left' scope='col'>Exp. Check Out</th></tr></thead> <tbody>";

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
                                    strReservationDetailTable += "<td align='left' style='width: 60%;'>" + dr.RoomType + "</td>";
                                    strReservationDetailTable += "<td align='left' style='width: 10%;'>" + dr.TotalRoom + "</td>";
                                    strReservationDetailTable += "<td align='left' style='width: 10%;'>" + dr.RoomTypeWisePaxQuantity + "</td>";
                                    strReservationDetailTable += "<td align='left' style='width: 10%;'>" + dr.ArrivalDate + "</td>";
                                    strReservationDetailTable += "<td align='left' style='width: 10%;'>" + dr.DepartureDate + "</td>";

                                    strReservationDetailTable += "</td></tr>";
                                    counterReservationDetail++;
                                }
                                strReservationDetailTable += "</tbody></table>";
                                if (strReservationDetailTable == "")
                                {
                                    strReservationDetailTable = "<tr><td colspan='5' align='center'>No Record Available !</td></tr>";
                                }
                            }
                        }                        
                    }

                    adjvw.ReservationDetailGrid = strReservationDetailTable;

                    // // // Will Update Later For Reservation Pax Wise---------------------------------Pax Wise Extra Room generated
                    //List<RoomReservationBO> typeWisePaxQuantityListBO = new List<RoomReservationBO>();
                    //if (prmRoomTypeId == 0)
                    //{
                    //    typeWisePaxQuantityListBO = roomReservationDA.GetTypeWisePaxQuantityByReservationId(reservationDetailListBO[0].ReservationId);
                    //}
                    //else
                    //{
                    //    typeWisePaxQuantityListBO = roomReservationDA.GetTypeWisePaxQuantityByReservationId(reservationDetailListBO[0].ReservationId).Where(x => x.RoomTypeId == prmRoomTypeId).ToList();
                    //}

                    //if (typeWisePaxQuantityListBO != null)
                    //{
                    //    if (typeWisePaxQuantityListBO.Count > 0)
                    //    {
                    //        foreach (RoomReservationBO row in typeWisePaxQuantityListBO)
                    //        {
                    //            int i = 0;
                    //            while (i < row.TotalPaxQuantity)
                    //            {
                    //                RoomReservationBO extraReservationDetailBO = new RoomReservationBO();
                    //                extraReservationDetailBO.ReservationDetailId = -1000 + row.RoomTypeId + i;
                    //                extraReservationDetailBO.IsRegistered = 100;
                    //                extraReservationDetailBO.ReservationId = reservationDetailListBO[0].ReservationId;
                    //                extraReservationDetailBO.RoomTypeCode = row.RoomTypeCode;
                    //                extraReservationDetailBO.PaxQuantity = row.PaxQuantity;
                    //                extraReservationDetailBO.TotalPaxQuantity = row.TotalPaxQuantity;
                    //                extraReservationDetailBO.UnitPrice = row.UnitPrice;
                    //                extraReservationDetailBO.DiscountType = row.DiscountType;
                    //                extraReservationDetailBO.DiscountAmount = row.DiscountAmount;
                    //                extraReservationDetailBO.RoomRate = row.RoomRate;
                    //                extraReservationDetailBO.RoomTypeId = row.RoomTypeId;
                    //                extraReservationDetailBO.TypeWiseRoomQuantity = row.TypeWiseRoomQuantity;
                    //                reservationDetailListBO.Add(extraReservationDetailBO);
                    //                i++;
                    //            }
                    //        }
                    //    }
                    //}

                    foreach (RoomReservationBO stck in reservationDetailListBO)
                    {
                        if (rowCount % 2 == 0)
                        {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else
                        {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }

                        duplicateCheck.Add(new RoomAssignDuplicationCheckVwBO()
                        {
                            Id = rowCount,
                            RoomNumber = (stck.RoomNumber == "Unassinged" ? string.Empty : stck.RoomNumber),
                            detailRowId = stck.ReservationDetailId,
                            PaxQuantity = stck.PaxQuantity,
                            RoomTypeCode = stck.RoomTypeCode,
                            RoomType = stck.RoomType,
                            RoomQuantity = stck.TypeWiseRoomQuantity,
                            RoomTypeId = stck.RoomTypeId
                        });

                        tr += "<td style='text-align:center;width:5%'> <input type='checkbox'  TabIndex=" + (1000 + (rowCount + 1)).ToString() + " value = 'chkECIn" + (1000 + (rowCount + 1)).ToString() + "' onkeydown='if (event.keyCode == 13) {return true;}'  style='vertical-align: middle;' /> </td>";
                        tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                        tr += "<td style='width:80%; text-align:center;'> <input type='text' class='form-control' TabIndex=" + (1000 + (rowCount + 1)).ToString() + " value = '" + (stck.GuestName == "" ? string.Empty : stck.GuestName) + "' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        tr += "<td style='width:10%; text-align:center;'> <input type='text' class='form-control' TabIndex=" + (2000 + (rowCount + 1)).ToString() + "  id='txt" + stck.ReservationDetailId.ToString() + "' value = '" + (stck.RoomNumber == "Unassinged" ? string.Empty : stck.RoomNumber) + "' placeholder = '" + stck.RoomTypeCode + "' onblur='CheckInputValue(this, " + stck.ReservationId.ToString() + "," + stck.RoomTypeId.ToString() + "," + stck.ReservationDetailId.ToString() + "," + rowCount + ")' maxlength='5' onkeydown='if (event.keyCode == 13) {return true;}' style='width:90px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        tr += "<td style='display:none'>" + stck.RoomRate + "</td>";
                        tr += "<td style='display:none'>" + stck.UnitPrice + "</td>";
                        tr += "<td style='display:none'>" + stck.DiscountType + "</td>";
                        tr += "<td style='display:none'>" + stck.DiscountAmount + "</td>";
                        tr += "<td style='display:none'>" + stck.RoomId + "</td>";
                        tr += "<td style='display:none'>" + stck.CurrencyType + "</td>";
                        tr += "<td style='display:none'>" + stck.ConversionRate + "</td>";
                        tr += "<td style='display:none'>" + stck.DateIn + "</td>";
                        tr += "<td style='display:none'>" + stck.DateOut + "</td>";
                        tr += "<td style='display:none'>" + stck.ReservationDetailId + "</td>";
                        tr += "<td style='display:none'>" + stck.GuestId + "</td>";
                        tr += "<td style='display:none'>" + stck.ReservationId + "</td>";
                        tr += "<td style='display:none'>" + stck.PaxQuantity + "</td>";
                        tr += "<td style='display:none'>" + stck.RoomType + "</td>";
                        tr += "<td style='display:none'>" + stck.TypeWiseRoomQuantity + "</td>";
                        tr += "<td style='display:none'>" + stck.RoomTypeId + "</td>";
                        tr += "</tr>";

                        rowCount++;
                    }

                    grid += GridHeader() + tr + "</tbody> </table>";
                    adjvw.ExpressCheckInnDetailsGrid = grid;
                    adjvw.DuplicateCheck = duplicateCheck;

                    //----------------------------------------------------------------------------------------------------------------------------------------------------------
                    DateTime dateTime = DateTime.Now;
                    HMUtility hmUtility = new HMUtility();
                    RoomNumberBO roomBO = new RoomNumberBO();
                    RoomNumberDA roomDA = new RoomNumberDA();
                    List<RoomNumberBO> roomList = new List<RoomNumberBO>();
                    RoomReservationDA reservationDA = new RoomReservationDA();

                    DateTime StartDate = reservationDetailListBO[0].DateIn;
                    DateTime EndDate = reservationDetailListBO[0].DateOut.AddDays(5);
                    List<RoomNumberBO> roomCalenderList = roomDA.GetRoomNumberInfoForCalender(StartDate, EndDate).Where(x => x.RoomNumber != "Unassigned").ToList();

                    roomList = (from rc in roomCalenderList
                                join rd in reservationDetailListBO on rc.RoomTypeId equals rd.RoomTypeId
                                select rc).Distinct().ToList();

                    List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(StartDate, EndDate);
                    List<RoomCalenderBO> calenderList = GetRoomCalenderList(StartDate, EndDate);

                    string strTable = "";
                    int dateCount = DateList.Count;
                    int totalCol = dateCount + 2;

                    strTable += "<table class='table table-bordered' id='fixTable'>";
                    int counter = 0;
                    strTable += "<thead><tr style='background-color:#E3EAEB;'>";
                    strTable += "<th color='white' bgcolor='#457EA4' align='left' style='width: 175px;cursor:pointer' ><font color='white'><b>Room No.</b></font> </th>";
                    strTable += "<th bgcolor='#457EA4' align='left' style='width: 175px;cursor:pointer' ><font color='white'><b>Type</b></font></th>";
                    
                    foreach (DateTime date in DateList)
                    {
                        strTable += "<th bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + hmUtility.GetStringFromDateTime(date) + "</b></font></th>";
                    }
                    strTable += "</tr></thead><tbody>";
                    
                    foreach (RoomNumberBO dr in roomList)
                    {
                        if (dr.GroupByRowNo == 1)
                        {
                            strTable += "<tr style='background-color:#023C61; color:white;'>";
                            strTable += "<td align='left' colspan='" + totalCol.ToString() + "' style='width: 175;cursor:pointer' >" + dr.FloorAndBlockName + "</td>";
                            strTable += "</tr>";
                        }

                        counter++;
                        strTable += "<tr style='background-color:#E3EAEB;'>";

                        //DateList
                        strTable += "<td align='left' style='width: 175;cursor:pointer' >" + dr.RoomNumber + "</td>";
                        strTable += "<td align='left' style='width: 175px;cursor:pointer'>" + dr.RoomTypeOrCode + "</td>";
                        foreach (DateTime date in DateList)
                        {
                            var List = calenderList.Where(c => c.RoomId == dr.RoomId && (c.CheckIn.Date <= date.Date) && (c.CheckOut > date)).ToList();
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
                                    strTable += "<td class='InHouse' align='left' style='width: 100px;cursor:pointer' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                }
                                else if (List[0].TransectionStatus.ToString() == "CheckedOut")
                                {
                                    strTable += "<td class='CheckedOut' align='left' style='width: 100px;cursor:pointer' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                }
                                else if (List[0].TransectionStatus.ToString() == "Reservation")
                                {
                                    strTable += "<td class='Reservation' align='left' style='width: 100px;cursor:pointer' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                }
                                else if (List[0].TransectionStatus.ToString() == "OutOfOrder")
                                {
                                    strTable += "<td class='OutOfOrder' align='left' style='width: 100px;cursor:pointer' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                }
                                else if (List[0].TransectionStatus.ToString() == "OutOfService")
                                {
                                    strTable += "<td class='OutOfService' align='left' style='width: 100px;cursor:pointer' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                }
                            }
                            else
                            {
                                strTable += "<td align='left' style='width: 100px;cursor:pointer' onClick=\"javascript:return LoadReservation(" + dr.RoomNumber + ")\"></td>";
                            }
                        }
                        strTable += "</tr>";
                    }
                    strTable += "</tbody></table>";
                    if (strTable == "")
                    {
                        strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
                    }

                    adjvw.ExpressCheckInnCalenderDetailsGrid = strTable;
                }
            }


            return adjvw;
        }
        [WebMethod]
        public static ReturnInfo SaveExpressCheckInn(int reservationId, List<RoomReservationBO> reservationDetailBO)
        {
            Boolean IsReservationProcess = true;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            RoomReservationDA roomReservationDA = new RoomReservationDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string rRoomInfo = string.Empty;
            RoomReservationBO roomReservationRoomInfoBO = new RoomReservationBO();
            List<RoomNumberBO> roomNumberBOList = new List<RoomNumberBO>();
            List<ReservationDetailBO> typeWiseRoomQuantityList = new List<ReservationDetailBO>();

            RoomReservationBO reservationBO = new RoomReservationBO();
            RoomReservationDA reservationDA = new RoomReservationDA();
            reservationBO = roomReservationDA.GetRoomReservationInfoById(reservationId);
            if (reservationBO != null)
            {
                if (reservationBO.ReservationId > 0)
                {
                    foreach (RoomReservationBO rowDetails in reservationDetailBO)
                    {
                        long reservationDetailId = 0;
                        try
                        {
                            if (rowDetails.DateIn.Date <= DateTime.Now.Date)
                            {
                                // // // ------------------------- Express Check In Related Block -------------------------------------------------------
                                if (rowDetails.TransactionType == "Save")
                                {
                                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                    RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                                    roomRegistrationBO.IsServiceChargeEnable = rowDetails.IsServiceChargeEnable;
                                    roomRegistrationBO.IsCityChargeEnable = rowDetails.IsCityChargeEnable;
                                    roomRegistrationBO.IsVatAmountEnable = rowDetails.IsVatAmountEnable;
                                    roomRegistrationBO.IsAdditionalChargeEnable = rowDetails.IsAdditionalChargeEnable;
                                    roomRegistrationBO.ReservationId = reservationId;
                                    roomRegistrationBO.ArriveDate = DateTime.Now;
                                    roomRegistrationBO.ExpectedCheckOutDate = reservationBO.DateOut;

                                    if (reservationBO.ClassificationId == 454)
                                    {
                                        roomRegistrationBO.IsCompanyGuest = true;
                                        roomRegistrationBO.IsHouseUseRoom = false;
                                        roomRegistrationBO.IsRoomOwner = 0;
                                    }
                                    else if (reservationBO.ClassificationId == 455)
                                    {
                                        roomRegistrationBO.IsCompanyGuest = true;
                                        roomRegistrationBO.IsHouseUseRoom = false;
                                        roomRegistrationBO.IsRoomOwner = 1;
                                    }
                                    else
                                    {
                                        roomRegistrationBO.IsCompanyGuest = false;
                                        roomRegistrationBO.IsHouseUseRoom = false;
                                        roomRegistrationBO.IsRoomOwner = 0;
                                    }

                                    roomRegistrationBO.MealPlanId = reservationBO.MealPlanId;

                                    RoomNumberDA numberDA = new RoomNumberDA();
                                    RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumberForExpressCheckIn(rowDetails.RoomNumber.ToString());
                                    if (numberBO.RoomId > 0)
                                    {
                                        if (numberBO.StatusId == 1)
                                        {
                                            if (numberBO.RoomId == rowDetails.RoomId)
                                            {
                                                roomRegistrationBO.DiscountType = rowDetails.DiscountType;
                                                roomRegistrationBO.UnitPrice = rowDetails.UnitPrice;
                                                roomRegistrationBO.DiscountAmount = rowDetails.DiscountAmount;
                                                roomRegistrationBO.RoomRate = rowDetails.RoomRate;
                                            }
                                            else
                                            {
                                                roomRegistrationBO.DiscountType = rowDetails.DiscountType;
                                                roomRegistrationBO.DiscountAmount = rowDetails.DiscountAmount;
                                                if (reservationBO.CurrencyType == 1)
                                                {
                                                    roomRegistrationBO.UnitPrice = numberBO.RoomRate;
                                                    roomRegistrationBO.RoomRate = numberBO.RoomRate;
                                                }
                                                else
                                                {
                                                    roomRegistrationBO.UnitPrice = numberBO.RoomRateUSD;
                                                    roomRegistrationBO.RoomRate = numberBO.RoomRateUSD;
                                                }
                                            }

                                            roomRegistrationBO.RoomId = numberBO.RoomId;
                                            roomRegistrationBO.EntitleRoomType = numberBO.RoomTypeId;
                                            roomRegistrationBO.IsFromReservation = true;
                                            roomRegistrationBO.CurrencyType = reservationBO.CurrencyType;
                                            roomRegistrationBO.ConversionRate = reservationBO.ConversionRate;
                                            roomRegistrationBO.CommingFrom = string.Empty;
                                            roomRegistrationBO.NextDestination = string.Empty;
                                            roomRegistrationBO.VisitPurpose = string.Empty;
                                            roomRegistrationBO.IsFamilyOrCouple = false;
                                            roomRegistrationBO.NumberOfPersonAdult = 1;
                                            roomRegistrationBO.GuestSourceId = 0;
                                            roomRegistrationBO.IsReturnedGuest = false;
                                            roomRegistrationBO.IsVIPGuest = reservationBO.IsVIPGuest;
                                            roomRegistrationBO.VIPGuestTypeId = reservationBO.VipGuestTypeId;

                                            if (reservationBO.IsVIPGuest)
                                            {
                                                roomRegistrationBO.IsCompanyGuest = reservationBO.IsComplementaryGuest;
                                            }

                                            roomRegistrationBO.NumberOfPersonChild = 0;
                                            roomRegistrationBO.IsListedCompany = reservationBO.IsListedCompany;
                                            roomRegistrationBO.CompanyId = reservationBO.CompanyId;
                                            roomRegistrationBO.ReservedCompany = reservationBO.ReservedCompany;
                                            roomRegistrationBO.ReservedCompany = string.Empty;
                                            roomRegistrationBO.PaymentMode = reservationBO.PaymentMode;
                                            roomRegistrationBO.PayFor = 0;
                                            roomRegistrationBO.BusinessPromotionId = 0;
                                            roomRegistrationBO.ReferenceId = 0;
                                            roomRegistrationBO.Remarks = string.Empty;

                                            List<RegistrationComplementaryItemBO> complementaryItemBOList = new List<RegistrationComplementaryItemBO>();

                                            //// -- Airport Pickup and Drop Information------------------------------------------
                                            roomRegistrationBO.ArrivalTime = DateTime.Now;
                                            roomRegistrationBO.DepartureTime = DateTime.Now;
                                            //// -- Airport Pickup and Drop Information-------------------------------End--------

                                            // -- Advance Payment Information--------------------------------------------------
                                            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
                                            GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

                                            //--------** Paid Service Save, Update **-------------------------

                                            List<RegistrationServiceInfoBO> paidServiceDetails = new List<RegistrationServiceInfoBO>();

                                            //// -- Advance Payment Information-------------------------------------End--------
                                            //roomRegistrationBO.RegistrationId = Convert.ToInt32(Session["_RoomRegistrationId"]);

                                            // -- Credit Card Information ---------------------------------------------------
                                            roomRegistrationBO.CardType = string.Empty;
                                            roomRegistrationBO.CardNumber = string.Empty;
                                            roomRegistrationBO.CardHolderName = string.Empty;
                                            roomRegistrationBO.CardExpireDate = null;
                                            roomRegistrationBO.CardReference = string.Empty;
                                            roomRegistrationBO.ReservationDetailId = reservationDetailId;

                                            string tempRegId = string.Empty;
                                            List<GuestInformationBO> tmpGuestInfoListBO = new List<GuestInformationBO>();
                                            List<GuestBillPaymentBO> guestBillPaymentBOList = new List<GuestBillPaymentBO>();

                                            int tmpRegId = 0;
                                            roomRegistrationBO.CreatedBy = userInformationBO.UserInfoId;
                                            Boolean status = roomRegistrationDA.SaveRoomRegistrationInfo(roomRegistrationBO, out tmpRegId, tmpGuestInfoListBO, guestBillPaymentBO, complementaryItemBOList, tempRegId, guestBillPaymentBOList, paidServiceDetails, false);

                                            if (status)
                                            {
                                                Boolean statusZeroGuestId = roomRegistrationDA.DeleteTempGuestRegistrationInfoByGuestId(tmpRegId, 0);
                                                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), tmpRegId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
                                                GuestInformationBO guestInfoBO = new GuestInformationBO();
                                                List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();
                                                GuestInformationBO individualGuest = new GuestInformationBO();
                                                guestInfoBO.GuestName = rowDetails.GuestName;

                                                bool statusGuestInfo = roomRegistrationDA.SaveTemporaryGuest(guestInfoBO, tmpRegId.ToString(), roomRegistrationBO.ArriveDate, 0, preferenList);
                                                if (statusGuestInfo)
                                                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), tmpRegId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                                            }
                                        }
                                        else if (numberBO.StatusId == 2)
                                        {
                                            GuestInformationBO guestInfoBO = new GuestInformationBO();
                                            List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();
                                            GuestInformationBO individualGuest = new GuestInformationBO();
                                            guestInfoBO.GuestName = rowDetails.GuestName;

                                            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                                            roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(numberBO.RoomNumber);
                                            if (roomAllocationBO.RoomId > 0)
                                            {
                                                if (reservationId == roomAllocationBO.ReservationId)
                                                {
                                                    bool updateDetailsInfo = roomRegistrationDA.UpdateHotelRoomReservationDetailForGuestCheckIn(roomAllocationBO.RegistrationId, roomAllocationBO.RoomId, reservationDetailId, userInformationBO.UserInfoId);

                                                    bool statusGuestInfo = roomRegistrationDA.SaveTemporaryGuest(guestInfoBO, roomAllocationBO.RegistrationId.ToString(), roomRegistrationBO.ArriveDate, 0, preferenList);
                                                    if (statusGuestInfo)
                                                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), roomAllocationBO.RegistrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
                                                }
                                                else
                                                {
                                                    rtninfo.IsSuccess = false;
                                                    rtninfo.AlertMessage = "Express Check In not Properly Processed.";
                                                    IsReservationProcess = false;
                                                }
                                            }
                                        }
                                    }
                                }

                                // // // ------------------------- Reservation Update Related Block -------------------------------------------------------
                                if (IsReservationProcess)
                                {
                                    RoomReservationBO roomReservationBO = new RoomReservationBO();

                                    RoomNumberDA numberDA = new RoomNumberDA();
                                    RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumberForExpressCheckIn(rowDetails.RoomNumber.ToString());
                                    if (numberBO.RoomId > 0)
                                    {
                                        reservationDetailId = rowDetails.ReservationDetailId;
                                        reservationId = rowDetails.ReservationId;
                                        roomReservationBO.ReservationId = rowDetails.ReservationId;
                                        roomReservationBO.ReservationDetailId = rowDetails.ReservationDetailId;
                                        roomReservationBO.RoomTypeId = rowDetails.RoomTypeId;
                                        roomReservationBO.RoomId = numberBO.RoomId;
                                        roomReservationBO.CreatedBy = userInformationBO.UserInfoId;
                                        Boolean status = roomReservationDA.UpdateReservationDetailForRoomAssignment(roomReservationBO);
                                        if (status)
                                        {
                                            if (roomReservationBO.ReservationId > 0)
                                                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), roomReservationBO.ReservationDetailId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails));
                                            else
                                                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), roomReservationBO.ReservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails) + ".EntityId is ReservationId");
                                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservation.ToString(), roomReservationBO.ReservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));
                                            GuestInformationBO guestInfoBO = new GuestInformationBO();
                                            List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();
                                            GuestInformationBO individualGuest = new GuestInformationBO();
                                            guestInfoBO.GuestId = !string.IsNullOrWhiteSpace(rowDetails.GuestId.ToString()) ? Convert.ToInt32(rowDetails.GuestId) : 0;
                                            guestInfoBO.GuestName = rowDetails.GuestName;

                                            RoomReservationDA resDA = new RoomReservationDA();
                                            bool statusGuestInfo = resDA.SaveTemporaryGuestNew(guestInfoBO, rowDetails.ReservationId.ToString(), preferenList);
                                            if (status)
                                            {
                                                if (guestInfoBO.GuestId == 0)
                                                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestInfoBO.GuestId,
                                                     ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                                                else
                                                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestInfoBO.GuestId,
                                                     ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                                                if (preferenList.Count > 0)
                                                {
                                                    foreach (var item in preferenList)
                                                    {
                                                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), item.MappingId,
                                                     ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping));
                                                    }
                                                }
                                            }

                                            RoomNumberBO roomNumberBO = new RoomNumberBO();
                                            roomNumberBO.RoomNumber = rowDetails.RoomNumber == "" ? "99999999" : rowDetails.RoomNumber;
                                            roomNumberBO.RoomType = rowDetails.RoomType;
                                            roomNumberBO.RoomTypeId = rowDetails.RoomTypeId;
                                            roomNumberBOList.Add(roomNumberBO);
                                            typeWiseRoomQuantityList.Add(new ReservationDetailBO { RoomTypeId = rowDetails.RoomTypeId, TotalRoom = rowDetails.TypeWiseRoomQuantity });
                                        }
                                    }
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                            throw ex;
                        }
                    }

                    if (roomNumberBOList != null)
                    {
                        if (roomNumberBOList.Count > 0)
                        {
                            roomNumberBOList = roomNumberBOList.OrderBy(test => test.RoomTypeId).ThenBy(test => test.RoomNumber).ToList();

                            // // Room Information Updated.. 
                            roomNumberBOList = (roomNumberBOList.GroupBy(test => new { test.RoomTypeId, test.RoomNumber })
                                 .Select(group => group.First()).ToList())
                                 .GroupBy(i => i.RoomTypeId).Select(group => new RoomNumberBO
                                 {
                                     RoomType = group.First().RoomType,
                                     RoomTypeId = group.First().RoomTypeId,
                                     RoomNumber = string.Join(",", group.Select(i => (i.RoomNumber == "99999999" ? "Unassigned" : i.RoomNumber))),
                                     RoomInformation = (typeWiseRoomQuantityList.Where(j => j.RoomTypeId == group.First().RoomTypeId).Select(j => j.TotalRoom.ToString()).FirstOrDefault())
                                 }).ToList();

                            int assignedRoomCount = 0, totalRoomCount = 0;

                            foreach (RoomNumberBO row in roomNumberBOList)
                            {
                                assignedRoomCount = row.RoomNumber.Split(',').Where(i => i != "Unassigned").ToList().Count;
                                totalRoomCount = typeWiseRoomQuantityList.Where(j => j.RoomTypeId == row.RoomTypeId).Select(j => j.TotalRoom).FirstOrDefault();

                                if (assignedRoomCount == totalRoomCount)
                                    row.RoomNumber = row.RoomNumber.Replace(",Unassigned", "");

                                if (!string.IsNullOrWhiteSpace(rRoomInfo))
                                {
                                    rRoomInfo = rRoomInfo + ", " + row.RoomType + ": " + row.RoomInformation + "(" + row.RoomNumber + ")";
                                }
                                else
                                {
                                    rRoomInfo = row.RoomType + ": " + row.RoomInformation + "(" + row.RoomNumber + ")";
                                }
                            }

                            roomReservationRoomInfoBO.RoomInfo = rRoomInfo;
                            roomReservationRoomInfoBO.ReservationId = reservationId;
                            roomReservationRoomInfoBO.CreatedBy = userInformationBO.UserInfoId;
                            Boolean statusRoomInfo = roomReservationDA.UpdateReservationRoomInfoByReservationId(roomReservationRoomInfoBO);
                        }
                    }
                }
            }

            roomReservationDA.UpdateReservationStatusByReservationId(reservationId);

            Boolean statusBlankRegistrationInfo = roomReservationDA.UpdateBlankRegistrationInfoByReservationId(reservationId);

            return rtninfo;
        }
        [WebMethod]
        public static RoomNumberBO GetRoomStatusByRoomNumber(string roomNumber, string detailRowId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            RoomNumberDA numberDA = new RoomNumberDA();

            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(roomNumber);
            if (numberBO.RoomId > 0)
            {
                roomNumberBO.RoomNumber = numberBO.RoomNumber;
                roomNumberBO.StatusId = numberBO.StatusId;
                roomNumberBO.StatusName = numberBO.StatusName;
                roomNumberBO.detailRowId = detailRowId;
            }

            return roomNumberBO;
        }
        [WebMethod]
        public static string SearchNLoadReservationInfo(int reservationId, string guestName, string companyName, string reservNumber, string checkInDate, string checkOutDate)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<RoomReservationInfoByDateRangeReportBO> list = new List<RoomReservationInfoByDateRangeReportBO>();
            DateTime? checkIn = null;
            DateTime? checkOut = null;
            if (!string.IsNullOrWhiteSpace(checkInDate))
                checkIn = hmUtility.GetDateTimeFromString(checkInDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            if (!string.IsNullOrWhiteSpace(checkOutDate))
                checkOut = hmUtility.GetDateTimeFromString(checkOutDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            list = guestDA.GetReservationInfoForRegistration(reservationId, checkIn, checkOut, guestName, companyName, reservNumber);
            return commonDA.GetReservationGridInfo(list);
        }
    }
}