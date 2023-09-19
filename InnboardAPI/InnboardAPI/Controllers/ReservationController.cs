﻿using InnboardAPI.Models;
using InnboardDataAccess.DataAccesses;
using InnboardDataAccess.SMSGetway;
using InnboardDomain.Common;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using InnboardService.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace InnboardAPI.Controllers
{
    [RoutePrefix("api/Reservation")]
    public class ReservationController : ApiController
    {
        InnboardDbContext context = new InnboardDbContext();

        private HotelRoomReservationOnlineService onlineReservationService;
        public ReservationController()
        {
            onlineReservationService = new HotelRoomReservationOnlineService();
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetRoomTypeInfo")]
        public async Task<IHttpActionResult> GetRoomTypeInfo()
        {
            HotelRoomTypeDataAccess db = new HotelRoomTypeDataAccess();
            var result = await db.GetRoomTypeInfo();
            return Ok(result);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetRoomTypeInfoForMobileApps")]
        public async Task<IHttpActionResult> GetRoomTypeInfoForMobileApps(string transactionType, int transactionId)
        {
            HotelRoomTypeDataAccess db = new HotelRoomTypeDataAccess();
            var result = await db.GetRoomTypeInfoForMobileApps(transactionType, transactionId);
            return Ok(result);
        }
        [EnableCors("*", "*", "*")]
        [Route("GetAvailableRoomInfo"), HttpGet]
        public string GetAvailableRoomInfo(string roomTypeId, string dateFrom, string dateTo)
        {
            //string dateFrom = DateTime.Now.ToString("MM/dd/yyyy"), dateTo = DateTime.Now.ToString("MM/dd/yyyy");
            int totalRoom = 0;
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            try
            {
                SqlParameter[] storeParams = new SqlParameter[5];

                storeParams[0] = new SqlParameter("@IsReservation", Convert.ToInt32(0));
                storeParams[1] = new SqlParameter("@RoomTypeId", Convert.ToInt32(roomTypeId));
                storeParams[2] = new SqlParameter("@FromDate", Convert.ToDateTime(dateFrom));
                storeParams[3] = new SqlParameter("@ToDate", Convert.ToDateTime(dateTo));
                storeParams[4] = new SqlParameter("@ReservationId", Convert.ToInt32(0));

                var r = context.Database.SqlQuery<RoomNumberBO>("EXEC GetAvailableRoomNumberInformation_SP @IsReservation, @RoomTypeId, @FromDate, @ToDate, @ReservationId", storeParams);
                roomNumberList = r.ToList();

                totalRoom = roomNumberList.Count();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return totalRoom.ToString();
        }

        [EnableCors("*", "*", "*")]
        [Route("RoomReservation"), HttpPost]
        public ReturnInfo RoomReservation(RoomReservationBO roomReservation)
        {
            ReturnInfo rtnInfo = new ReturnInfo();

            try
            {
                //RoomNumberDA roomNumberDA = new RoomNumberDA();
                //RoomReservationDA roomReservationDA = new RoomReservationDA();
                //List<RoomNumberBO> availableRoom = new List<RoomNumberBO>();
                //HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                //List<ReservationComplementaryItemBO> complementaryItem = new List<ReservationComplementaryItemBO>();
                //List<HMComplementaryItemBO> complementeryItem = new List<HMComplementaryItemBO>();
                //List<ReservationComplementaryItemBO> reservationComplementaryItem = new List<ReservationComplementaryItemBO>();

                //int tmpReservationId = 0;
                //int tmpapdId = 0;
                //string currentReservationNumber = string.Empty;
                //string serverDateFormat = string.Empty, clientDateFormat = string.Empty;

                //commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardCalenderFormat");
                //string[] formats = Regex.Split(commonSetupBO.SetupValue, "~");
                //serverDateFormat = formats[0];
                //clientDateFormat = formats[1];

                //HMComplementaryItemDA entityDA = new HMComplementaryItemDA();
                //complementeryItem = entityDA.GetActiveHMComplementaryItemInfo();

                //reservationComplementaryItem = (from ci in complementeryItem
                //                                where ci.IsDefaultItem == true
                //                                select new ReservationComplementaryItemBO
                //                                {
                //                                    RCItemId = 0,
                //                                    ReservationId = 0,
                //                                    ComplementaryItemId = ci.ComplementaryItemId
                //                                }).ToList();

                ////roomReservation.DateIn = Convert.ToDateTime(roomReservation.DateIn.ToString("yyyy-MM-dd") + " " + roomReservation.ProbableArrivalTime.ToString("HH:mm"));
                ////startDate = CommonHelper.DateTimeToMMDDYYYY(dateFrom, serverDateFormat);
                ////endDate = CommonHelper.DateTimeToMMDDYYYY(dateTo, serverDateFormat);

                //roomReservationDA.SaveOnlineRoomReservationInfo(roomReservation, out tmpReservationId, roomReservationDetail, reservationComplementaryItem, out currentReservationNumber, null, false);
                //roomReservationDA.SaveTemporaryGuestNew(guestDetailsInfo, tmpReservationId.ToString(), new List<GuestPreferenceMappingBO>());


                HotelOnlineRoomReservation r = new HotelOnlineRoomReservation();
                r.DateIn = roomReservation.DateIn;
                r.DateOut = roomReservation.DateOut;
                //r.ProbableArrivalTime = roomReservation.ProbableArrivalTime;
                //r.NumberOfPersonAdult = roomReservation.NumberOfPersonAdult;
                //r.NumberOfPersonChild = roomReservation.NumberOfPersonChild;
                r.CurrencyType = roomReservation.CurrencyType;
                r.ConversionRate = roomReservation.ConversionRate;

                context.HotelGuestInformation.Add(roomReservation.guestDetailsInfo);
                context.HotelOnlineRoomReservation.Add(r);
                context.SaveChanges();


                rtnInfo.IsSuccess = true;

            }
            catch (Exception ex)
            {
                rtnInfo.IsSuccess = false;
            }

            return rtnInfo; //JsonConvert.SerializeObject(rtnInfo, Newtonsoft.Json.Formatting.Indented);
        }

        [Route("SaveReservation")]
        [HttpPost]
        public IHttpActionResult PostReservation(HotelRoomReservationOnlineView roomReservation)
        {
            if (ModelState.IsValid)
            {
                var response = onlineReservationService.Save(roomReservation);

                return Json(new { response.Success });
            }
            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format" });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetRoomReservationInformationForMobileApps")]
        public async Task<IHttpActionResult> GetRoomReservationInformationForMobileApps(int propertyId, string transactionType, int transactionId, DateTime fromDate, DateTime toDate)
        {
            HotelRoomReservationDataAccess dbLogin = new HotelRoomReservationDataAccess();
            var result = await dbLogin.GetRoomReservationInformationForMobileApps(propertyId, transactionType, transactionId, fromDate, toDate);
            return Ok(result);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("SaveRoomReservationInfoForMobileApps")]
        public async Task<IHttpActionResult> SaveRoomReservationInfoForMobileApps([FromBody] HotelRoomReservationMobileAppsBO roomReservationInfo)
        {
            int tmpGuestId = 0;
            long tmpReservationId = 0;
            HotelRoomReservationDataAccess dbLogin = new HotelRoomReservationDataAccess();
            bool isSuccess = dbLogin.SaveRoomReservationInfoForMobileApps(roomReservationInfo, out tmpGuestId, out tmpReservationId);
            if (isSuccess)
            {
                var result = new
                {
                    GuestId = tmpGuestId,
                    ReservationId = tmpReservationId,
                };

                CommonDataAccess db = new CommonDataAccess();
                var commonSetupSMSAutoPostingBO = await db.GetCommonConfigurationInfo("SMSAutoPosting", "IsRoomReservationSMSAutoPostingEnable");
                if (commonSetupSMSAutoPostingBO.SetupValue == "1")
                {
                    CommonDataAccess dbCommonDataAccess = new CommonDataAccess();
                    var companyInfo = await dbCommonDataAccess.GetCompanyInfo();

                    string documentsTextMessage = "Please provide all of your NID/ Driving License/ Passport Copy at the time of check-in.";
                    //HMCommonSetupBO commonSetupDocumentsTextMessageBO = new HMCommonSetupBO();
                    //commonSetupDocumentsTextMessageBO = commonSetupDA.GetCommonConfigurationInfo("RoomReservationSMSDocumentsMessage", "RoomReservationSMSDocumentsMessage");
                    //if (commonSetupDocumentsTextMessageBO.SetupId > 0)
                    //{
                    //    documentsTextMessage = commonSetupDocumentsTextMessageBO.Description;
                    //}


                    var commonSetupSendSMSBO = await db.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                    string commonSetupBODescription = commonSetupSendSMSBO.Description;
                    string[] dataArray = commonSetupBODescription.Split('~');
                    var smsGetway = dataArray[0];

                    var reservationBO = await dbLogin.GetRoomReservationInfoById(tmpReservationId);

                    //send msg 
                    SMSView sms = new SMSView
                    {
                        TempleteName = HMConstants.SMSTemplates.ReservationConfirmation
                    };
                    var singletoken = new Dictionary<string, string>
                        {
                        {"COMPANY", companyInfo[0].CompanyName},
                        {"COMPANYADDRESS", companyInfo[0].CompanyAddress},
                        {"CONTACTNUMBER", companyInfo[0].ContactNumber},
                        {"Name", reservationBO.ContactPerson},
                        {"ReservationNumber",  reservationBO.ReservationNumber},
                        {"ArrivalDate", reservationBO.DateIn.ToString()},
                        {"DepartureDate", reservationBO.DateOut.ToString()},
                        {"RoomNumber", reservationBO.RoomNumber },
                        {"SMSDocumentsMessage", documentsTextMessage }
                        };

                    SmsHelper.SendSmsSingle(sms, singletoken, smsGetway, roomReservationInfo.PhoneNumber, commonSetupBODescription);
                    // send msg end

                }

                return Ok(result);
            }
            else
            {
                return BadRequest("Bad Request");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SaveRoomReservationPaymentInfoForMobileApps")]
        public async Task<HttpResponseMessage> SaveRoomReservationPaymentInfoForMobileApps([FromBody] HotelRoomReservationMobileAppsBO memberBasicInfo)
        {
            HotelRoomReservationDataAccess dbLogin = new HotelRoomReservationDataAccess();
            bool isSuccess = dbLogin.SaveRoomReservationPaymentInfoForMobileApps(memberBasicInfo);
            if (isSuccess)
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("Succesfully Payment Posted.")
                };
                return responseMsg;
            }
            else
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Bad Request")
                };
                return responseMsg;
            }
        }
    }
}
