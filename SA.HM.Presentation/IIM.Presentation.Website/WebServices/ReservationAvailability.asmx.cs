using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Text.RegularExpressions;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Presentation.Website.WebServices
{
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    //Response.AppendHeader("Access-Control-Allow-Origin", "*");
    [ScriptService]
    public class ReservationAvailability : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetAvailableRoomInfo(string dateFrom, string dateTo, string roomTypeId)
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> availableRoom = new List<RoomNumberBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            string serverDateFormat = string.Empty, clientDateFormat = string.Empty;
            DateTime startDate, endDate;
            int availableTotalRoom = 0;

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardCalenderFormat");

            string[] formats = Regex.Split(commonSetupBO.SetupValue, "~");
            serverDateFormat = formats[0];
            clientDateFormat = formats[1];

            startDate = CommonHelper.DateTimeToMMDDYYYY(dateFrom, serverDateFormat);
            endDate = CommonHelper.DateTimeToMMDDYYYY(dateTo, serverDateFormat);

            availableRoom = roomNumberDA.GetAvailableRoomNumberInformation(Convert.ToInt32(roomTypeId), 0, startDate, endDate, 0);
            availableTotalRoom = availableRoom.Count;

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            Context.Response.Write(js.Serialize(availableTotalRoom));

            //return JsonConvert.SerializeObject(availableTotalRoom, Newtonsoft.Json.Formatting.None);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void RoomReservation(RoomReservationBO roomReservation, GuestInformationBO guestDetailsInfo, List<ReservationDetailBO> roomReservationDetail)
        {
            ReturnInfo rtnInfo = new ReturnInfo();
            try
            {
                RoomNumberDA roomNumberDA = new RoomNumberDA();
                RoomReservationDA roomReservationDA = new RoomReservationDA();
                List<RoomNumberBO> availableRoom = new List<RoomNumberBO>();
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                List<ReservationComplementaryItemBO> complementaryItem = new List<ReservationComplementaryItemBO>();
                List<HMComplementaryItemBO> complementeryItem = new List<HMComplementaryItemBO>();
                List<ReservationComplementaryItemBO> reservationComplementaryItem = new List<ReservationComplementaryItemBO>();

                int tmpReservationId = 0;
                int tmpapdId = 0;
                string currentReservationNumber = string.Empty;
                string serverDateFormat = string.Empty, clientDateFormat = string.Empty;

                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardCalenderFormat");
                string[] formats = Regex.Split(commonSetupBO.SetupValue, "~");
                serverDateFormat = formats[0];
                clientDateFormat = formats[1];

                HMComplementaryItemDA entityDA = new HMComplementaryItemDA();
                complementeryItem = entityDA.GetActiveHMComplementaryItemInfo();

                reservationComplementaryItem = (from ci in complementeryItem
                                                where ci.IsDefaultItem == true
                                                select new ReservationComplementaryItemBO
                                                {
                                                    RCItemId = 0,
                                                    ReservationId = 0,
                                                    ComplementaryItemId = ci.ComplementaryItemId
                                                }).ToList();

                //roomReservation.DateIn = Convert.ToDateTime(roomReservation.DateIn.ToString("yyyy-MM-dd") + " " + roomReservation.ProbableArrivalTime.ToString("HH:mm"));
                //startDate = CommonHelper.DateTimeToMMDDYYYY(dateFrom, serverDateFormat);
                //endDate = CommonHelper.DateTimeToMMDDYYYY(dateTo, serverDateFormat);

                roomReservationDA.SaveOnlineRoomReservationInfo(roomReservation, out tmpReservationId, roomReservationDetail, reservationComplementaryItem, out currentReservationNumber, null, false);
                roomReservationDA.SaveTemporaryGuestNew(guestDetailsInfo, tmpReservationId.ToString(), new List<GuestPreferenceMappingBO>());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            Context.Response.Write(js.Serialize(rtnInfo));

            //return JsonConvert.SerializeObject(rtnInfo, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
