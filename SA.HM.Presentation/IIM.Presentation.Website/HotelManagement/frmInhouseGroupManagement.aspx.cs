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
    public partial class frmInhouseGroupManagement : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.LoadGroupInformation();
                CheckPermission();
            }
        }
        //************************ User Defined Method ********************//
        //private void LoadGroupInformation()
        //{
        //    RoomReservationDA roomReservationDA = new RoomReservationDA();
        //    this.ddlGroupInformation.DataSource = roomReservationDA.GetGroupOrCompanyRoomReservationInfoForRoomStatus();
        //    this.ddlGroupInformation.DataTextField = "CompanyName";
        //    this.ddlGroupInformation.DataValueField = "ReservationId";
        //    this.ddlGroupInformation.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlGroupInformation.Items.Insert(0, item);
        //}
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
                         "               <th style='width: 10%;'>" +
                         "                   " +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Room Number" +
                         "               </th>" +
                         "               <th style='width: 75%;'>" +
                         "                   Guest Name" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }
        //public static List<DateTime> GetDateArrayBetweenTwoDates(DateTime StartDate, DateTime EndDate)
        //{
        //    var dates = new List<DateTime>();

        //    for (var dt = StartDate; dt <= EndDate; dt = dt.AddDays(1))
        //    {
        //        //dates.Add(dt.AddDays(1).AddSeconds(-1));
        //        dates.Add(dt);
        //    }
        //    return dates;
        //}
        public static List<RoomCalenderBO> GetRoomCalenderList(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(StartDate, EndDate);
            return calenderList;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static RoomReservationBO ExpressCheckInnGridInformation(int reservationId)
        {
            int rowCount = 0;
            string grid = string.Empty, tr = string.Empty;

            RoomReservationBO adjvw = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<RoomReservationBO> reservationDetailListBO = new List<RoomReservationBO>();
            reservationDetailListBO = roomReservationDA.GetExpressCheckedInnInformationByReservationId(reservationId);

            if (reservationDetailListBO != null)
            {
                if (reservationDetailListBO.Count > 0)
                {
                    int i = 0;
                    while (i < 10)
                    {
                        RoomReservationBO extraReservationDetailBO = new RoomReservationBO();
                        extraReservationDetailBO.ReservationDetailId = -1000 + i;
                        extraReservationDetailBO.IsRegistered = 100;
                        reservationDetailListBO.Add(extraReservationDetailBO);
                        i++;
                    }

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

                        tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                        tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' checked class='form-control' TabIndex=" + (1000 + stck.ReservationDetailId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";

                        if (!string.IsNullOrWhiteSpace(stck.RoomNumber))
                        {
                            tr += "<td style='width:10%; text-align:center;'> <input type='text' class='form-control' readonly TabIndex=" + (2000 + stck.ReservationDetailId).ToString() + "  id='txt" + stck.ReservationDetailId.ToString() + "' value = '" + (stck.RoomNumber == "Unassinged" ? string.Empty : stck.RoomNumber) + "' onblur='CheckInputValue(this, " + stck.ReservationDetailId.ToString() + ")' maxlength='5' onkeydown='if (event.keyCode == 13) {return true;}' style='width:90px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        }
                        else
                        {
                            tr += "<td style='width:10%; text-align:center;'> <input type='text' class='form-control' TabIndex=" + (2000 + stck.ReservationDetailId).ToString() + "  id='txt" + stck.ReservationDetailId.ToString() + "' value = '" + (stck.RoomNumber == "Unassinged" ? string.Empty : stck.RoomNumber) + "' onblur='CheckInputValue(this, " + stck.ReservationDetailId.ToString() + ")' maxlength='5' onkeydown='if (event.keyCode == 13) {return true;}' style='width:90px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        }

                        tr += "<td style='width:75%; text-align:center;'> <input type='text' class='form-control' readonly TabIndex=" + (1000 + stck.ReservationDetailId).ToString() + "  id='txtGuest" + stck.ReservationDetailId.ToString() + "'  value = '" + (stck.GuestName == "" ? string.Empty : stck.GuestName) + "' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        tr += "<td style='display:none'>" + stck.RoomRate + "</td>";
                        tr += "<td style='display:none'>" + stck.UnitPrice + "</td>";
                        tr += "<td style='display:none'>" + stck.DiscountType + "</td>";
                        tr += "<td style='display:none'>" + stck.DiscountAmount + "</td>";
                        tr += "<td style='display:none'>" + stck.RoomId + "</td>";
                        tr += "<td style='display:none'>" + stck.CurrencyType + "</td>";
                        tr += "<td style='display:none'>" + stck.ConversionRate + "</td>";
                        tr += "<td style='display:none'>" + stck.DateInString + "</td>";
                        tr += "<td style='display:none'>" + stck.DateOutString + "</td>";
                        tr += "</tr>";

                        rowCount++;
                    }

                    grid += GridHeader() + tr + "</tbody> </table>";
                    adjvw.ExpressCheckInnDetailsGrid = grid;
                }
            }

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

            //List<DateTime> DateList = GetDateArrayBetweenTwoDates(StartDate, EndDate);
            List<DateTime> DateList = CommonHelper.GetDateArrayBetweenTwoDates(StartDate, EndDate);
            List<RoomCalenderBO> calenderList = GetRoomCalenderList(StartDate, EndDate);

            string strTable = "";
            int dateCount = DateList.Count;
            int totalCol = dateCount + 2;

            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'>";
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
                    var List = calenderList.Where(c => c.RoomId == dr.RoomId && (c.CheckOut > date && c.CheckIn <= date)).ToList();
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
            return adjvw;
        }
        [WebMethod]
        public static ReturnInfo SaveUpdateGroupAssignment(int reservationId, List<RoomReservationBO> reservationDetailBO)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            bool statusUnAssignment = roomRegistrationDA.UpdateOccupiedRoomGroupUnAssignment(0, reservationId, userInformationBO.UserInfoId);
            if(statusUnAssignment)
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), 0, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
            foreach (RoomReservationBO rowDetails in reservationDetailBO)
            {
                try
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumberForExpressCheckIn(rowDetails.RoomNumber.ToString());
                    if (numberBO.RoomId > 0)
                    {                        
                        if (numberBO.StatusId == 2)
                        {
                            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                            roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(numberBO.RoomNumber);
                            if (roomAllocationBO.RoomId > 0)
                            {
                                if (roomAllocationBO.RoomId == numberBO.RoomId)
                                {
                                    bool statusGuestInfo = roomRegistrationDA.UpdateOccupiedRoomGroupAssignment(roomAllocationBO.RegistrationId, numberBO.RoomId, reservationId, userInformationBO.UserInfoId);
                                    if(statusGuestInfo)
                                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), roomAllocationBO.RegistrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
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

            return rtninfo;

        }
        [WebMethod]
        public static RoomNumberBO GetRoomStatusByRoomNumber(string roomNumber, string detailRowId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();

            RoomNumberBO roomNumberBO = new RoomNumberBO();

            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInfoByRoomNumber(roomNumber);
            if (numberBO.RoomId > 0)
            {
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(numberBO.RoomNumber);
                if (roomAllocationBO.RoomId > 0)
                {
                    if (roomAllocationBO.RoomId == numberBO.RoomId)
                    {
                        roomNumberBO.GuestName = roomAllocationBO.GuestName;
                    }
                }

                roomNumberBO.RoomNumber = numberBO.RoomNumber;
                roomNumberBO.StatusId = numberBO.StatusId;
                roomNumberBO.StatusName = numberBO.StatusName;
                roomNumberBO.detailRowId = detailRowId;
            }

            return roomNumberBO;
        }
    }
}