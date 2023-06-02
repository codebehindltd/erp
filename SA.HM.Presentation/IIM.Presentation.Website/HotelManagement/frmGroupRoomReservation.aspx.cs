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
using System.Runtime.InteropServices.ComTypes;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGroupRoomReservation : BasePage
    {
        protected int rsvnReservationId = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission();
                LoadGuestCompany();
            }
        }
        //************************ User Defined Method ********************//
        private void LoadGuestCompany()
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            ddlCompany.DataSource = companyDa.GetGuestCompanyInfo();
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem { Text = "---Please Select---", Value = "0" });
        }
        public static string GridHeader(int templateOne)
        {
            string gridHead = string.Empty;
            if (templateOne == 1)
            {
                gridHead += "<table id='RoomReservationGrid' class='table table-bordered table-condensed table-responsive'>" +
                             "       <thead>" +
                             "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                             "               <th style='width: 10%;'>" +
                             "                   Reservation #" +
                             "               </th>" +
                             "               <th style='width: 15%;'>" +
                             "                   Reservation Date" +
                             "               </th>" +
                             "               <th style='width: 10%;'>" +
                             "                   Check In Date" +
                             "               </th>" +
                             "               <th style='width: 10%;'>" +
                             "                   Check Out Date" +
                             "               </th>" +
                             "               <th style='width: 45%;'>" +
                             "                   Group Name" +
                             "               </th>" +
                             "               <th style='width: 10%;'>" +
                             "                   Action" +
                             "               </th>" +
                             "           </tr>" +
                             "       </thead>" +
                             "       <tbody>";
            }
            else
            {
                gridHead += "<table id='RoomReservationGrid' class='table table-bordered table-condensed table-responsive'>" +
                             "       <thead>" +
                             "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                             "               <th style='width: 5%; text-align:center;'>" +
                             "                   <input id='chkAll' type='checkbox' value = 'chkExpressCheckIn' onclick='CheckAll()' onkeydown='if (event.keyCode == 13) {return true;}' style='vertical-align: middle;' />" +
                             "               </th>" +
                             "               <th style='width: 5%;'>" +
                             "                   #" +
                             "               </th>" +
                             "               <th style='width: 10%;'>" +
                             "                   Reservation #" +
                             "               </th>" +
                             "               <th style='width: 15%;'>" +
                             "                   Reservation Date" +
                             "               </th>" +
                             "               <th style='width: 10%;'>" +
                             "                   Check In Date" +
                             "               </th>" +
                             "               <th style='width: 10%;'>" +
                             "                   Check Out Date" +
                             "               </th>" +
                             "               <th style='width: 45%;'>" +
                             "                   Guest Name" +
                             "               </th>" +
                             "           </tr>" +
                             "       </thead>" +
                             "       <tbody>";
            }


            return gridHead;
        }
        private void CheckPermission()
        {
            //btnExpressCheckIn.Visible = isSavePermission;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static RoomReservationBO GetGroupRoomReservationInfoByStringSearchCriteria(string groupName, string reservationNumber, string prmFromDate, string prmToDate)
        {
            int rowCount = 0;
            HMUtility hmUtility = new HMUtility();
            string grid = string.Empty, tr = string.Empty;


            DateTime? fromDate; DateTime? toDate;

            if (!string.IsNullOrEmpty(prmFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(prmFromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(prmToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(prmToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = DateTime.Now;
            }

            string guestName = string.Empty;
            string reserveNo = string.Empty; string companyName = string.Empty;
            string contactPerson = string.Empty;
            string contactPhone = string.Empty;
            string contactEmail = string.Empty;
            int srcMarketSegment = 0;
            int srcGuestSource = 0;
            int srcReferenceId = 0;
            int ordering = 0;
            string status = string.Empty;


            RoomReservationBO RoomReservationBO = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<RoomReservationBO> roomReservationListBO = new List<RoomReservationBO>();

            roomReservationListBO = roomReservationDA.GetGroupRoomReservationInfoByStringSearchCriteria(fromDate, toDate, reservationNumber, groupName);

            if (roomReservationListBO != null)
            {
                if (roomReservationListBO.Count > 0)
                {
                    foreach (RoomReservationBO stck in roomReservationListBO)
                    {
                        if (rowCount % 2 == 0)
                        {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else
                        {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }

                        tr += "<td>" + stck.ReservationNumber + "</td>";
                        tr += "<td>" + stck.ReservationDateDisplay + "</td>";
                        tr += "<td>" + stck.CheckInDateDisplay + "</td>";
                        tr += "<td>" + stck.CheckOutDateDisplay + "</td>";
                        tr += "<td>" + stck.GroupName + "</td>";
                        tr += "<td align='left' style=\"width:45%; cursor:pointer;\"><img src='../Images/delete.png' ToolTip='Cancel' onClick= \"javascript:return PerformCancelAction('" + stck.Id + "')\" alt='Cancel' border='0' />&nbsp;&nbsp;<img src='../Images/ReportDocument.png' ToolTip='Preview' onClick= \"javascript:return PerformBillPreviewAction('" + stck.Id + "')\" alt='Preview Information' border='0' /></td>";
                        tr += "<td style='display:none'>" + stck.Id + "</td>";
                        tr += "</tr>";

                        rowCount++;
                    }

                    grid += GridHeader(1) + tr + "</tbody> </table>";
                    RoomReservationBO.RoomReservationGrid = grid;

                }
            }

            return RoomReservationBO;
        }

        [WebMethod]
        public static RoomReservationBO GetRoomReservationInfoByStringSearchCriteria(string prmFromDate, string prmToDate)
        {
            int rowCount = 0;
            HMUtility hmUtility = new HMUtility();
            string grid = string.Empty, tr = string.Empty;


            DateTime? fromDate; DateTime? toDate;

            if (!string.IsNullOrEmpty(prmFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(prmFromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(prmToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(prmToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = DateTime.Now;
            }

            string guestName = string.Empty;
            string reserveNo = string.Empty; string companyName = string.Empty;
            string contactPerson = string.Empty;
            string contactPhone = string.Empty;
            string contactEmail = string.Empty;
            int srcMarketSegment = 0;
            int srcGuestSource = 0;
            int srcReferenceId = 0;
            int ordering = 0;
            string status = string.Empty;


            RoomReservationBO RoomReservationBO = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<RoomReservationBO> roomReservationListBO = new List<RoomReservationBO>();

            roomReservationListBO = roomReservationDA.GetRoomReservationInfoByStringSearchCriteria(fromDate, toDate, guestName, reserveNo, companyName, contactPerson, contactPhone, contactEmail, srcMarketSegment, srcGuestSource, srcReferenceId, status).Where(x => x.IsTaggedOnGroupReservation == false).ToList();

            if (roomReservationListBO != null)
            {
                if (roomReservationListBO.Count > 0)
                {
                    foreach (RoomReservationBO stck in roomReservationListBO)
                    {
                        if (rowCount % 2 == 0)
                        {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else
                        {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }

                        tr += "<td style='text-align:center;width:5%'> <input type='checkbox'  TabIndex=" + (1000 + (rowCount + 1)).ToString() + " value = 'chkECIn" + (1000 + (rowCount + 1)).ToString() + "' onkeydown='if (event.keyCode == 13) {return true;}'  style='vertical-align: middle;' /> </td>";
                        tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                        tr += "<td>" + stck.ReservationNumber + "</td>";
                        tr += "<td>" + stck.ReservationDateDisplay + "</td>";
                        tr += "<td>" + stck.DateInDisplay + "</td>";
                        tr += "<td>" + stck.DateOutDisplay + "</td>";
                        tr += "<td>" + stck.GuestName + "</td>";

                        //tr += "<td style='width:80%; text-align:center;'> <input type='text' class='form-control' TabIndex=" + (1000 + (rowCount + 1)).ToString() + " value = '" + (stck.GuestName == "" ? string.Empty : stck.GuestName) + "' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        //tr += "<td style='width:10%; text-align:center;'> <input type='text' class='form-control' TabIndex=" + (2000 + (rowCount + 1)).ToString() + "  id='txt" + stck.ReservationDetailId.ToString() + "' value = '" + (stck.RoomNumber == "Unassinged" ? string.Empty : stck.RoomNumber) + "' placeholder = '" + stck.RoomTypeCode + "' onblur='CheckInputValue(this, " + stck.ReservationId.ToString() + "," + stck.RoomTypeId.ToString() + "," + stck.ReservationDetailId.ToString() + "," + rowCount + ")' maxlength='5' onkeydown='if (event.keyCode == 13) {return true;}' style='width:90px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        tr += "<td style='display:none'>" + stck.ReservationId + "</td>";
                        tr += "</tr>";

                        rowCount++;
                    }

                    grid += GridHeader(2) + tr + "</tbody> </table>";
                    RoomReservationBO.RoomReservationGrid = grid;

                }
            }


            return RoomReservationBO;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateGroupRoomReservation(long groupMasterId, int companyId, string groupName, string strReservationDate, string reservationDetails, List<RoomReservationBO> groupRoomReservationList)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            RoomReservationDA roomReservationDA = new RoomReservationDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime reservationDate = DateTime.Now;
            if (!string.IsNullOrEmpty(strReservationDate))
            {
                reservationDate = hmUtility.GetDateTimeFromString(strReservationDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            groupRoomReservationList = groupRoomReservationList.Where(x => x.ReservationId != 0).ToList();

            if (groupMasterId == 0)
            {
                RoomReservationDA resDA = new RoomReservationDA();
                bool status = resDA.SaveGroupRoomReservation(companyId, groupName, reservationDate, reservationDetails, groupRoomReservationList, userInformationBO.UserInfoId, out groupMasterId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = "Group Reservation Save Successfully.";
                }
            }
            else
            {
                RoomReservationDA resDA = new RoomReservationDA();
                bool status = resDA.UpdateGroupRoomReservation(groupMasterId, companyId, groupName, reservationDate, reservationDetails, groupRoomReservationList, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = "Group Reservation Save Successfully.";
                }
            }

            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo CancelGroupReservation(long reservationId, string reason)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            RoomReservationDA reservationDA = new RoomReservationDA();
            RoomReservationBO RoomReservationBO = reservationDA.GetGroupRoomReservationInfoById(reservationId);
            //RoomReservationBO reservationBO = new RoomReservationBO() { ReservationId = reservationId, ReservationMode = mode, Reason = reason };
            try
            {
                if (RoomReservationBO != null)
                {
                    if (RoomReservationBO.Id > 0)
                    {
                        returnInfo.IsSuccess = reservationDA.CancelGroupRoomReservation(reservationId);
                        if (returnInfo.IsSuccess)
                        {
                            returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.ReservationCancel, AlertType.Success);
                            hmUtility.CreateActivityLogEntity("Delete", "Group Room Reservation", reservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Cancel Group Reservation#" + RoomReservationBO.ReservationNumber + " for the Reason: " + reason);

                        }
                        else
                        {
                            returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnInfo;
        }
    }
}