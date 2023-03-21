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
    public partial class frmReservationRoomAssignment : BasePage
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
                }
            }
            CheckPermission();
        }
        //************************ User Defined Method ********************//
        private void CheckPermission()
        {
            btnExpressCheckIn.Visible = isUpdatePermission;
        }
        public static string GridHeader()
        {
            string gridHead = string.Empty;
            gridHead += "<table id='ExpressCheckInDetailsGrid' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 5%;'>" +
                         "                   #" +
                         "               </th>" +
                         "               <th style='width: 85%;'>" +
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
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static RoomReservationBO ExpressCheckInnGridInformation(string reservationNumber)
        {
            int rowCount = 0;
            int roomReservationId = 0;
            string grid = string.Empty, tr = string.Empty, roomReservationNumber = string.Empty;

            RoomNumberDA entityDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberInfoBO = entityDA.GetRoomNumberInfo();

            RoomReservationBO adjvw = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<RoomReservationBO> reservationDetailListBO = new List<RoomReservationBO>();
            List<RoomAssignDuplicationCheckVwBO> duplicateCheck = new List<RoomAssignDuplicationCheckVwBO>();

            reservationDetailListBO = roomReservationDA.GetRoomReservationInformationForRoomAssignment(reservationNumber);

            if (reservationDetailListBO != null)
            {
                if (reservationDetailListBO.Count > 0)
                {
                    int counterReservationDetail = 0;
                    string strReservationDetailTable = "";
                    ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
                    List<ReservationDetailBO> reservationDetailListBOForGrid = new List<ReservationDetailBO>();
                    reservationDetailListBOForGrid = reservationDetailDA.GetReservationDetailByRegiIdForGrid(reservationDetailListBO[0].ReservationId, 0);
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

                    // // // Will Update Later For Reservation Pax Wise---------------------------------Pax Wise Extra Room generated
                    //adjvw.ReservationDetailGrid = strReservationDetailTable;
                    //List<RoomReservationBO> typeWisePaxQuantityListBO = new List<RoomReservationBO>();
                    //typeWisePaxQuantityListBO = roomReservationDA.GetTypeWisePaxQuantityByReservationId(reservationDetailListBO[0].ReservationId);
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
                    //                extraReservationDetailBO.RoomTypeId = row.RoomTypeId;
                    //                extraReservationDetailBO.TypeWiseRoomQuantity = row.TypeWiseRoomQuantity;
                    //                reservationDetailListBO.Add(extraReservationDetailBO);
                    //                i++;
                    //            }
                    //        }
                    //    }
                    //}

                    string guestName = string.Empty;

                    foreach (RoomReservationBO stck in reservationDetailListBO)
                    {
                        roomReservationId = stck.ReservationId;
                        roomReservationNumber = stck.ReservationNumber;
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

                        if (!string.IsNullOrWhiteSpace(stck.GuestName))
                        {
                            guestName = stck.GuestName;
                        }

                        tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                        tr += "<td style='width:85%; text-align:center;'> <input type='text' class='form-control' TabIndex=" + (1000 + (rowCount + 1)).ToString() + " value = '" + (guestName == "" ? string.Empty : guestName) + "' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        tr += "<td style='width:10%; text-align:center;'> <input type='text' class='form-control' TabIndex=" + (2000 + (rowCount + 1)).ToString() + "  id='txt" + stck.ReservationDetailId.ToString() + "' value = '" + (stck.RoomNumber == "Unassinged" ? string.Empty : stck.RoomNumber) + "' placeholder = '" + stck.RoomTypeCode + "' onblur='CheckInputValue(this, " + stck.ReservationId.ToString() + "," + stck.RoomTypeId.ToString() + "," + stck.ReservationDetailId.ToString() + "," + rowCount + ")' maxlength='5' onkeydown='if (event.keyCode == 13) {return true;}' onfocus='FilterRoomCalender(this)' style='width:90px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
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
                    adjvw.ReservationId = roomReservationId;
                    adjvw.ReservationNumber = roomReservationNumber;
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
                        strTable += "<tr id='roomNumber" + counter + "' style='background-color:#E3EAEB;'>";
                        strTable += "<td align='left' style='width: 175;cursor:pointer' >" + dr.RoomNumber + "</td>";
                        strTable += "<td align='left' style='width: 175px;cursor:pointer'>" + dr.RoomTypeOrCode + "</td>";
                        foreach (DateTime date in DateList)
                        {
                            var List = calenderList.Where(c => c.RoomId == dr.RoomId && (c.CheckIn.Date <= date.Date) && (c.CheckOut >= date.Date)).ToList();
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
                                    strTable += "<td class='InHouse' align='left' style='width: 100px;cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                }
                                else if (List[0].TransectionStatus.ToString() == "CheckedOut")
                                {
                                    strTable += "<td class='CheckedOut ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";'\">" + List[0].GuestName + "</td>";
                                }
                                else if (List[0].TransectionStatus.ToString() == "Reservation")
                                {
                                    var roomIdListBO = roomNumberInfoBO.Where(x => x.RoomId == dr.RoomId & x.StatusId == 1 & x.HKRoomStatusId == 2 & x.CleanupStatus == "Not Cleaned").FirstOrDefault();
                                    if (roomIdListBO != null)
                                    {
                                        strTable += "<td class='CheckedOut ToolTipClass' align='left' style='width: 100px; cursor:pointer; background-color:" + List[0].ColorCodeName + ";'\">" + "***" + "</td>";
                                    }
                                    else
                                    {
                                        strTable += "<td class='Reservation' align='left' style='width: 100px;cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                    }
                                }
                                else if (List[0].TransectionStatus.ToString() == "OutOfOrder")
                                {
                                    strTable += "<td class='OutOfOrder' align='left' style='width: 100px;cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
                                }
                                else if (List[0].TransectionStatus.ToString() == "OutOfService")
                                {
                                    strTable += "<td class='OutOfService' align='left' style='width: 100px;cursor:pointer; background-color:" + List[0].ColorCodeName + ";' onClick=\"javascript:return RedirectToDetails(" + List[0].TransectionId + "," + reservation + ",'" + List[0].RoomNumber + "','" + date + "')\">" + List[0].GuestName + "</td>";
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
        public static ReturnInfo SaveExpressCheckInn(List<RoomReservationBO> reservationDetailBO)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            string rRoomInfo = string.Empty;
            Int32 reservationId = 0;
            RoomReservationBO roomReservationRoomInfoBO = new RoomReservationBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<RoomNumberBO> roomNumberBOList = new List<RoomNumberBO>();
            List<ReservationDetailBO> typeWiseRoomQuantityList = new List<ReservationDetailBO>();

            foreach (RoomReservationBO rowDetails in reservationDetailBO)
            {
                if (reservationId == 0)
                {
                    reservationId = rowDetails.ReservationId;
                }
            }

            if (reservationId > 0)
            {
                roomReservationDA.HotelGuestReservationDataCleanForRoomAssignment(reservationId);
            }

            foreach (RoomReservationBO rowDetails in reservationDetailBO)
            {
                try
                {
                    RoomReservationBO roomReservationBO = new RoomReservationBO();
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumberForExpressCheckIn(rowDetails.RoomNumber.ToString());
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
                        guestInfoBO.RoomId = numberBO.RoomId;

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
                catch (Exception ex)
                {
                    throw ex;
                }
            }

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
        public static RoomNumberBO LoadRoomInformationWithControl(string roomNumber, int reservationId, int RoomTypeId, string detailRowId, int index)
        {
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            roomNumberBO.RoomNumber = roomNumber;
            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;
            DateTime EndDate = dateTime;

            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> list = new List<RoomNumberBO>();

            RoomReservationBO reservationBO = new RoomReservationBO();
            RoomReservationDA reservationDA = new RoomReservationDA();
            reservationBO = reservationDA.GetRoomReservationInfoById(reservationId);
            if (reservationBO != null)
            {
                if (reservationBO.ReservationId > 0)
                {
                    list = roomNumberDA.GetAvailableRoomNumberInformation(RoomTypeId, 0, reservationBO.DateIn, reservationBO.DateOut, reservationId).Where(x => x.RoomNumber == roomNumber).ToList();

                    if (list != null)
                    {
                        if (list.Count > 0)
                        {
                            roomNumberBO.RoomNumber = "Invalid"; //list[0].RoomNumber;
                            roomNumberBO.StatusId = list[0].StatusId;
                            roomNumberBO.StatusName = list[0].StatusName;
                            roomNumberBO.RoomId = list[0].RoomId;
                        }
                    }
                    roomNumberBO.IndexId = index;
                    roomNumberBO.detailRowId = detailRowId;
                }
            }
            else
            {
                roomNumberBO.IndexId = index;
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

            list = guestDA.GetReservationInfoForRegistration(reservationId, checkIn, checkOut, guestName, companyName, reservNumber);
            return commonDA.GetReservationGridInfo(list);
        }
    }
}