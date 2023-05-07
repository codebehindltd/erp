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
            CheckPermission();
        }
        //************************ User Defined Method ********************//
        public static string GridHeader()
        {
            string gridHead = string.Empty;
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

            return gridHead;
        }
        private void CheckPermission()
        {
            //btnExpressCheckIn.Visible = isSavePermission;
        }
        //************************ User Defined Web Method ********************//

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

            roomReservationListBO = roomReservationDA.GetRoomReservationInfoByStringSearchCriteria(fromDate, toDate, guestName, reserveNo, companyName, contactPerson, contactPhone, contactEmail, srcMarketSegment, srcGuestSource, srcReferenceId, status);
            
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

                    grid += GridHeader() + tr + "</tbody> </table>";
                    RoomReservationBO.RoomReservationGrid = grid;

                }
            }


            return RoomReservationBO;
        }
                
        [WebMethod]
        public static ReturnInfo SaveOrUpdateGroupRoomReservation(int groupId, List<RoomReservationBO> reservationDetailBO)
        {
            //Boolean IsReservationProcess = true;
            //HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            //RoomReservationDA roomReservationDA = new RoomReservationDA();

            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //string rRoomInfo = string.Empty;
            //RoomReservationBO roomReservationRoomInfoBO = new RoomReservationBO();
            //List<RoomNumberBO> roomNumberBOList = new List<RoomNumberBO>();
            //List<ReservationDetailBO> typeWiseRoomQuantityList = new List<ReservationDetailBO>();

            //RoomReservationBO reservationBO = new RoomReservationBO();
            //RoomReservationDA reservationDA = new RoomReservationDA();
            //reservationBO = roomReservationDA.GetRoomReservationInfoById(reservationId);
            //if (reservationBO != null)
            //{
            //    if (reservationBO.ReservationId > 0)
            //    {
            //        foreach (RoomReservationBO rowDetails in reservationDetailBO)
            //        {
            //            long reservationDetailId = 0;
            //            try
            //            {
            //                if (rowDetails.DateIn.Date <= DateTime.Now.Date)
            //                {
            //                    // // // ------------------------- Express Check In Related Block -------------------------------------------------------
            //                    if (rowDetails.TransactionType == "Save")
            //                    {
            //                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //                        RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
            //                        roomRegistrationBO.IsServiceChargeEnable = rowDetails.IsServiceChargeEnable;
            //                        roomRegistrationBO.IsCityChargeEnable = rowDetails.IsCityChargeEnable;
            //                        roomRegistrationBO.IsVatAmountEnable = rowDetails.IsVatAmountEnable;
            //                        roomRegistrationBO.IsAdditionalChargeEnable = rowDetails.IsAdditionalChargeEnable;
            //                        roomRegistrationBO.ReservationId = reservationId;
            //                        roomRegistrationBO.ArriveDate = DateTime.Now;
            //                        roomRegistrationBO.ExpectedCheckOutDate = reservationBO.DateOut;

            //                        if (reservationBO.ClassificationId == 454)
            //                        {
            //                            roomRegistrationBO.IsCompanyGuest = true;
            //                            roomRegistrationBO.IsHouseUseRoom = false;
            //                            roomRegistrationBO.IsRoomOwner = 0;
            //                        }
            //                        else if (reservationBO.ClassificationId == 455)
            //                        {
            //                            roomRegistrationBO.IsCompanyGuest = true;
            //                            roomRegistrationBO.IsHouseUseRoom = false;
            //                            roomRegistrationBO.IsRoomOwner = 1;
            //                        }
            //                        else
            //                        {
            //                            roomRegistrationBO.IsCompanyGuest = false;
            //                            roomRegistrationBO.IsHouseUseRoom = false;
            //                            roomRegistrationBO.IsRoomOwner = 0;
            //                        }

            //                        roomRegistrationBO.MealPlanId = reservationBO.MealPlanId;

            //                        RoomNumberDA numberDA = new RoomNumberDA();
            //                        RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumberForExpressCheckIn(rowDetails.RoomNumber.ToString());
            //                        if (numberBO.RoomId > 0)
            //                        {
            //                            if (numberBO.StatusId == 1)
            //                            {
            //                                if (numberBO.RoomId == rowDetails.RoomId)
            //                                {
            //                                    roomRegistrationBO.DiscountType = rowDetails.DiscountType;
            //                                    roomRegistrationBO.UnitPrice = rowDetails.UnitPrice;
            //                                    roomRegistrationBO.DiscountAmount = rowDetails.DiscountAmount;
            //                                    roomRegistrationBO.RoomRate = rowDetails.RoomRate;
            //                                }
            //                                else
            //                                {
            //                                    roomRegistrationBO.DiscountType = rowDetails.DiscountType;
            //                                    roomRegistrationBO.DiscountAmount = rowDetails.DiscountAmount;
            //                                    if (reservationBO.CurrencyType == 1)
            //                                    {
            //                                        roomRegistrationBO.UnitPrice = numberBO.RoomRate;
            //                                        roomRegistrationBO.RoomRate = numberBO.RoomRate;
            //                                    }
            //                                    else
            //                                    {
            //                                        roomRegistrationBO.UnitPrice = numberBO.RoomRateUSD;
            //                                        roomRegistrationBO.RoomRate = numberBO.RoomRateUSD;
            //                                    }
            //                                }

            //                                roomRegistrationBO.RoomId = numberBO.RoomId;
            //                                roomRegistrationBO.EntitleRoomType = numberBO.RoomTypeId;
            //                                roomRegistrationBO.IsFromReservation = true;
            //                                roomRegistrationBO.CurrencyType = reservationBO.CurrencyType;
            //                                roomRegistrationBO.ConversionRate = reservationBO.ConversionRate;
            //                                roomRegistrationBO.CommingFrom = string.Empty;
            //                                roomRegistrationBO.NextDestination = string.Empty;
            //                                roomRegistrationBO.VisitPurpose = string.Empty;
            //                                roomRegistrationBO.IsFamilyOrCouple = false;
            //                                roomRegistrationBO.NumberOfPersonAdult = 1;
            //                                roomRegistrationBO.GuestSourceId = 0;
            //                                roomRegistrationBO.IsReturnedGuest = false;
            //                                roomRegistrationBO.IsVIPGuest = reservationBO.IsVIPGuest;
            //                                roomRegistrationBO.VIPGuestTypeId = reservationBO.VipGuestTypeId;

            //                                if (reservationBO.IsVIPGuest)
            //                                {
            //                                    roomRegistrationBO.IsCompanyGuest = reservationBO.IsComplementaryGuest;
            //                                }

            //                                roomRegistrationBO.NumberOfPersonChild = 0;
            //                                roomRegistrationBO.IsListedCompany = reservationBO.IsListedCompany;
            //                                roomRegistrationBO.CompanyId = reservationBO.CompanyId;
            //                                roomRegistrationBO.ReservedCompany = reservationBO.ReservedCompany;
            //                                roomRegistrationBO.ReservedCompany = string.Empty;
            //                                roomRegistrationBO.PaymentMode = reservationBO.PaymentMode;
            //                                roomRegistrationBO.PayFor = 0;
            //                                roomRegistrationBO.BusinessPromotionId = 0;
            //                                roomRegistrationBO.ReferenceId = 0;
            //                                roomRegistrationBO.Remarks = string.Empty;

            //                                List<RegistrationComplementaryItemBO> complementaryItemBOList = new List<RegistrationComplementaryItemBO>();

            //                                //// -- Airport Pickup and Drop Information------------------------------------------
            //                                roomRegistrationBO.ArrivalTime = DateTime.Now;
            //                                roomRegistrationBO.DepartureTime = DateTime.Now;
            //                                //// -- Airport Pickup and Drop Information-------------------------------End--------

            //                                // -- Advance Payment Information--------------------------------------------------
            //                                GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
            //                                GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

            //                                //--------** Paid Service Save, Update **-------------------------

            //                                List<RegistrationServiceInfoBO> paidServiceDetails = new List<RegistrationServiceInfoBO>();

            //                                //// -- Advance Payment Information-------------------------------------End--------
            //                                //roomRegistrationBO.RegistrationId = Convert.ToInt32(Session["_RoomRegistrationId"]);

            //                                // -- Credit Card Information ---------------------------------------------------
            //                                roomRegistrationBO.CardType = string.Empty;
            //                                roomRegistrationBO.CardNumber = string.Empty;
            //                                roomRegistrationBO.CardHolderName = string.Empty;
            //                                roomRegistrationBO.CardExpireDate = null;
            //                                roomRegistrationBO.CardReference = string.Empty;
            //                                roomRegistrationBO.ReservationDetailId = reservationDetailId;

            //                                string tempRegId = string.Empty;
            //                                List<GuestInformationBO> tmpGuestInfoListBO = new List<GuestInformationBO>();
            //                                List<GuestBillPaymentBO> guestBillPaymentBOList = new List<GuestBillPaymentBO>();

            //                                int tmpRegId = 0;
            //                                roomRegistrationBO.CreatedBy = userInformationBO.UserInfoId;
            //                                Boolean status = roomRegistrationDA.SaveRoomRegistrationInfo(roomRegistrationBO, out tmpRegId, tmpGuestInfoListBO, guestBillPaymentBO, complementaryItemBOList, tempRegId, guestBillPaymentBOList, paidServiceDetails, false);

            //                                if (status)
            //                                {
            //                                    Boolean statusZeroGuestId = roomRegistrationDA.DeleteTempGuestRegistrationInfoByGuestId(tmpRegId, 0);
            //                                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), tmpRegId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
            //                                    GuestInformationBO guestInfoBO = new GuestInformationBO();
            //                                    List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();
            //                                    GuestInformationBO individualGuest = new GuestInformationBO();
            //                                    guestInfoBO.GuestName = rowDetails.GuestName;

            //                                    bool statusGuestInfo = roomRegistrationDA.SaveTemporaryGuest(guestInfoBO, tmpRegId.ToString(), roomRegistrationBO.ArriveDate, 0, preferenList);
            //                                    if (statusGuestInfo)
            //                                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), tmpRegId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
            //                                }
            //                            }
            //                            else if (numberBO.StatusId == 2)
            //                            {
            //                                GuestInformationBO guestInfoBO = new GuestInformationBO();
            //                                List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();
            //                                GuestInformationBO individualGuest = new GuestInformationBO();
            //                                guestInfoBO.GuestName = rowDetails.GuestName;

            //                                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            //                                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(numberBO.RoomNumber);
            //                                if (roomAllocationBO.RoomId > 0)
            //                                {
            //                                    if (reservationId == roomAllocationBO.ReservationId)
            //                                    {
            //                                        bool updateDetailsInfo = roomRegistrationDA.UpdateHotelRoomReservationDetailForGuestCheckIn(roomAllocationBO.RegistrationId, roomAllocationBO.RoomId, reservationDetailId, userInformationBO.UserInfoId);

            //                                        bool statusGuestInfo = roomRegistrationDA.SaveTemporaryGuest(guestInfoBO, roomAllocationBO.RegistrationId.ToString(), roomRegistrationBO.ArriveDate, 0, preferenList);
            //                                        if (statusGuestInfo)
            //                                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), roomAllocationBO.RegistrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
            //                                    }
            //                                    else
            //                                    {
            //                                        rtninfo.IsSuccess = false;
            //                                        rtninfo.AlertMessage = "Express Check In not Properly Processed.";
            //                                        IsReservationProcess = false;
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }

            //                    // // // ------------------------- Reservation Update Related Block -------------------------------------------------------
            //                    if (IsReservationProcess)
            //                    {
            //                        RoomReservationBO roomReservationBO = new RoomReservationBO();

            //                        RoomNumberDA numberDA = new RoomNumberDA();
            //                        RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumberForExpressCheckIn(rowDetails.RoomNumber.ToString());
            //                        if (numberBO.RoomId > 0)
            //                        {
            //                            reservationDetailId = rowDetails.ReservationDetailId;
            //                            reservationId = rowDetails.ReservationId;
            //                            roomReservationBO.ReservationId = rowDetails.ReservationId;
            //                            roomReservationBO.ReservationDetailId = rowDetails.ReservationDetailId;
            //                            roomReservationBO.RoomTypeId = rowDetails.RoomTypeId;
            //                            roomReservationBO.RoomId = numberBO.RoomId;
            //                            roomReservationBO.CreatedBy = userInformationBO.UserInfoId;
            //                            Boolean status = roomReservationDA.UpdateReservationDetailForRoomAssignment(roomReservationBO);
            //                            if (status)
            //                            {
            //                                if (roomReservationBO.ReservationId > 0)
            //                                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), roomReservationBO.ReservationDetailId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails));
            //                                else
            //                                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), roomReservationBO.ReservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails) + ".EntityId is ReservationId");
            //                                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservation.ToString(), roomReservationBO.ReservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));
            //                                GuestInformationBO guestInfoBO = new GuestInformationBO();
            //                                List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();
            //                                GuestInformationBO individualGuest = new GuestInformationBO();
            //                                guestInfoBO.GuestId = !string.IsNullOrWhiteSpace(rowDetails.GuestId.ToString()) ? Convert.ToInt32(rowDetails.GuestId) : 0;
            //                                guestInfoBO.GuestName = rowDetails.GuestName;

            //                                RoomReservationDA resDA = new RoomReservationDA();
            //                                bool statusGuestInfo = resDA.SaveTemporaryGuestNew(guestInfoBO, rowDetails.ReservationId.ToString(), preferenList);
            //                                if (status)
            //                                {
            //                                    if (guestInfoBO.GuestId == 0)
            //                                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestInfoBO.GuestId,
            //                                         ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
            //                                    else
            //                                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestInfoBO.GuestId,
            //                                         ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
            //                                    if (preferenList.Count > 0)
            //                                    {
            //                                        foreach (var item in preferenList)
            //                                        {
            //                                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), item.MappingId,
            //                                         ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping));
            //                                        }
            //                                    }
            //                                }

            //                                RoomNumberBO roomNumberBO = new RoomNumberBO();
            //                                roomNumberBO.RoomNumber = rowDetails.RoomNumber == "" ? "99999999" : rowDetails.RoomNumber;
            //                                roomNumberBO.RoomType = rowDetails.RoomType;
            //                                roomNumberBO.RoomTypeId = rowDetails.RoomTypeId;
            //                                roomNumberBOList.Add(roomNumberBO);
            //                                typeWiseRoomQuantityList.Add(new ReservationDetailBO { RoomTypeId = rowDetails.RoomTypeId, TotalRoom = rowDetails.TypeWiseRoomQuantity });
            //                            }
            //                        }
            //                    }

            //                }

            //            }
            //            catch (Exception ex)
            //            {
            //                //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            //                throw ex;
            //            }
            //        }

            //        if (roomNumberBOList != null)
            //        {
            //            if (roomNumberBOList.Count > 0)
            //            {
            //                roomNumberBOList = roomNumberBOList.OrderBy(test => test.RoomTypeId).ThenBy(test => test.RoomNumber).ToList();

            //                // // Room Information Updated.. 
            //                roomNumberBOList = (roomNumberBOList.GroupBy(test => new { test.RoomTypeId, test.RoomNumber })
            //                     .Select(group => group.First()).ToList())
            //                     .GroupBy(i => i.RoomTypeId).Select(group => new RoomNumberBO
            //                     {
            //                         RoomType = group.First().RoomType,
            //                         RoomTypeId = group.First().RoomTypeId,
            //                         RoomNumber = string.Join(",", group.Select(i => (i.RoomNumber == "99999999" ? "Unassigned" : i.RoomNumber))),
            //                         RoomInformation = (typeWiseRoomQuantityList.Where(j => j.RoomTypeId == group.First().RoomTypeId).Select(j => j.TotalRoom.ToString()).FirstOrDefault())
            //                     }).ToList();

            //                int assignedRoomCount = 0, totalRoomCount = 0;

            //                foreach (RoomNumberBO row in roomNumberBOList)
            //                {
            //                    assignedRoomCount = row.RoomNumber.Split(',').Where(i => i != "Unassigned").ToList().Count;
            //                    totalRoomCount = typeWiseRoomQuantityList.Where(j => j.RoomTypeId == row.RoomTypeId).Select(j => j.TotalRoom).FirstOrDefault();

            //                    if (assignedRoomCount == totalRoomCount)
            //                        row.RoomNumber = row.RoomNumber.Replace(",Unassigned", "");

            //                    if (!string.IsNullOrWhiteSpace(rRoomInfo))
            //                    {
            //                        rRoomInfo = rRoomInfo + ", " + row.RoomType + ": " + row.RoomInformation + "(" + row.RoomNumber + ")";
            //                    }
            //                    else
            //                    {
            //                        rRoomInfo = row.RoomType + ": " + row.RoomInformation + "(" + row.RoomNumber + ")";
            //                    }
            //                }

            //                roomReservationRoomInfoBO.RoomInfo = rRoomInfo;
            //                roomReservationRoomInfoBO.ReservationId = reservationId;
            //                roomReservationRoomInfoBO.CreatedBy = userInformationBO.UserInfoId;
            //                Boolean statusRoomInfo = roomReservationDA.UpdateReservationRoomInfoByReservationId(roomReservationRoomInfoBO);
            //            }
            //        }
            //    }
            //}

            //roomReservationDA.UpdateReservationStatusByReservationId(reservationId);

            //Boolean statusBlankRegistrationInfo = roomReservationDA.UpdateBlankRegistrationInfoByReservationId(reservationId);

            return rtninfo;
        }        
    }
}