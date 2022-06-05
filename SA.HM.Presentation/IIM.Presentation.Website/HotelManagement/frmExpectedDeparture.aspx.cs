using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
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
    public partial class frmExpectedDeparture : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/HotelManagement/frmRoomStatusInfo.aspx?ExpectedDeparture=" + 6);
        }
        protected void btnBillPreview_Click(object sender, EventArgs e)
        {
            string url = "/HotelManagement/Reports/frmReportGuestBillPreview.aspx";
            string s = "window.open('" + url + "', 'popup_window', 'width=820,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(GetType(), "script", s, true);
        }
        protected void btnBillPreviewAndBillLock_Click(object sender, EventArgs e)
        {
            Int64 registrationId = !string.IsNullOrWhiteSpace(hfPaxInRegistrationId.Value) ? Convert.ToInt64(hfPaxInRegistrationId.Value) : 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            Boolean status = registrationDA.UpdateBillPrintPreviewAndBillLock(registrationId, registrationId, userInformationBO.UserInfoId);
            if (status)
            {
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), registrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomStopChargePostingDetails.ToString(), registrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomStopChargePostingDetails) + "deleted by registrationId");
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomStopChargePostingDetails.ToString(), registrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomStopChargePostingDetails) + ".entityId is registrationId");
            }

            string url = "/HotelManagement/Reports/frmReportGuestBillPreview.aspx";
            string s = "window.open('" + url + "', 'popup_window', 'width=820,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(GetType(), "script", s, true);
        }
        protected void btnPaxInRateUpdate_Click(object sender, EventArgs e)
        {
            int registrationId = !string.IsNullOrWhiteSpace(hfPaxInRegistrationId.Value) ? Convert.ToInt32(hfPaxInRegistrationId.Value) : 0;
            int guestId = !string.IsNullOrWhiteSpace(hfPaxInGuestId.Value) ? Convert.ToInt32(hfPaxInGuestId.Value) : 0;
            decimal paxInRate = !string.IsNullOrWhiteSpace(txtPaxInRate.Text) ? Convert.ToDecimal(txtPaxInRate.Text) : 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (registrationId > 0 && guestId > 0 && paxInRate > 0)
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                Boolean status = roomRegistrationDA.UpdateGuestPaxInRateInfo(registrationId, guestId, paxInRate, userInformationBO.UserInfoId);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), registrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                }

            }
        }
        private static string GetUserDetailHtml(List<GuestInformationBO> registrationDetailListBO)
        {
            string strTable = "";
            strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th> </tr>";
            int counter = 0;

            foreach (GuestInformationBO dr in registrationDetailListBO)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='left' style='width: 50%; cursor: pointer' onClick=\"javascript:return PerformViewActionForGuestDetail('" + dr.GuestId + "')\">" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%; cursor: pointer' onClick=\"javascript:return PerformViewActionForGuestDetail('" + dr.GuestId + "')\">" + dr.GuestEmail + "</td>";
                strTable += "</tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }
            return strTable;
        }
        

        [WebMethod]
        public static GridViewDataNPaging<GuestHouseInfoForReportBO, GridPaging> SearchNLoadRegistrationInfo(string guestName, string companyName, string regNumber, string checkInDate, string checkOutDate, string roomNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            UserInformationBO userInformationBO = new UserInformationBO();

            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            GridViewDataNPaging<GuestHouseInfoForReportBO, GridPaging> myGridData = new GridViewDataNPaging<GuestHouseInfoForReportBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            DateTime? checkIn = null;
            DateTime? checkOut = null;
            if (!string.IsNullOrWhiteSpace(checkInDate))
                checkIn = hmUtility.GetDateTimeFromString(checkInDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            if (!string.IsNullOrWhiteSpace(checkOutDate))
                checkOut = hmUtility.GetDateTimeFromString(checkOutDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            guestHouseInfo = guestInfoDa.GetGuestExpectedDepartureList(guestName, companyName, regNumber, roomNumber, checkIn, checkOut);
            
            List<GuestHouseInfoForReportBO> finalList = new List<GuestHouseInfoForReportBO>();
            if (!string.IsNullOrWhiteSpace(roomNumber))
            {
                finalList = guestHouseInfo.Where(x => x.RoomNumber == roomNumber).ToList();
            }
            else
            {
                finalList = guestHouseInfo;
            }

            myGridData.GridPagingProcessing(guestHouseInfo, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static string LoadRegistrationPossiblePath(string RoomNember, string pageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OccupiedPossiblePath");

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            int row = 1;

            for (int i = 0; i < list.Count; i++)
            {
                if (row == 1)
                    strTable += "<div class='form-group'>";

                strTable += "<div class='col-md-4'>";
                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return CountTotalNumberOfGuestByRoomNumber('" + RoomNember + "', 0 );\"  />";
                    strTable += "</div>";
                }
                else if (list[i].DisplayText == "Bill Preview")
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNember);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        HMUtility hmUtility = new HMUtility();
                        HttpContext.Current.Session["CheckOutRegistrationIdList"] = roomAllocationBO.RegistrationId.ToString();
                        string StartDate = hmUtility.GetFromDate();
                        string EndDate = hmUtility.GetToDate();
                        string ddlRegistrationId = roomAllocationBO.RegistrationId.ToString();
                        string txtSrcRegistrationIdList = roomAllocationBO.RegistrationId.ToString();
                        HttpContext.Current.Session["IsBillSplited"] = "0";
                        HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
                        HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();
                        strTable += " onclick='LoadBillPreview()' />";
                        strTable += "</div>";
                    }
                }
                else if (list[i].DisplayText == "Room Check Out")
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNember);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        HMUtility hmUtility = new HMUtility();
                        HttpContext.Current.Session["CheckOutRegistrationIdList"] = roomAllocationBO.RegistrationId.ToString();
                        string StartDate = hmUtility.GetFromDate();
                        string EndDate = hmUtility.GetToDate();
                        string ddlRegistrationId = roomAllocationBO.RegistrationId.ToString();
                        string txtSrcRegistrationIdList = roomAllocationBO.RegistrationId.ToString();
                        HttpContext.Current.Session["IsBillSplited"] = "0";
                        HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
                        HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();
                        if (roomAllocationBO.IsStopChargePosting)
                        {
                            strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "';\"  />";
                        }
                        else
                        {
                            string strDisplayText = "Bill Lock & Preview";
                            strTable = strTable.Replace("<div class='col-md-4'>" + "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'",
                                "<div class='col-md-4'>" + "<input type='button' style='width:150px' value='" + strDisplayText + "' class='TransactionalButton btn btn-primary'"
                                );
                            strTable += " onclick='LoadBillPreviewAndBillLock(" + txtSrcRegistrationIdList + ")' />";
                        }
                        strTable += "</div>";
                    }
                }
                else
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "';\"  />";
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
        [WebMethod]
        public static GuestInformationBO CountTotalNumberOfGuestByRoomNumber(string roomNumer, int regId)
        {
            int count = 0;
            GuestInformationBO guestBO = new GuestInformationBO();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            GuestInformationDA guestDA = new GuestInformationDA();
            if (roomNumer != "0")
            {
                count = guestDA.CountNumberOfGuestByRegistrationId(allocationBO.RegistrationId);
            }
            else count = guestDA.CountNumberOfGuestByRegistrationId(regId);
            guestBO.NumberOfGuest = count;
            guestBO.RoomNumber = roomNumer;
            guestBO.RegistrationId = regId;
            return guestBO;
        }
        [WebMethod]
        public static GuestInformationBO GetRegistrationInformationForSingleGuestByRoomNumber(string roomNumer, int regId)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            if (roomNumer != "0")
            {
                list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
                foreach (GuestInformationBO row in list)
                {
                    row.RoomRate = allocationBO.RoomRate;
                    row.CurrencyTypeHead = allocationBO.CurrencyTypeHead;
                    row.ArriveDate = row.ArriveDate;
                    row.ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
                }
            }
            else list = guestDA.GetGuestInformationByRegistrationId(regId);

            return list[0];
        }
        [WebMethod]
        public static string GetRegistrationInformationListByRoomNumber(string roomNumer, int regId)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            if (roomNumer != "0")
            {
                list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
            }
            else list = guestDA.GetGuestInformationByRegistrationId(regId);
            return GetUserDetailHtml(list);
        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string GuestId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Guest", Int64.Parse(GuestId));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }
        [WebMethod]
        public static string LoadGuestPreferences(int guestId)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            GuestPreferenceDA guestPrefDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> gstPrefList = new List<GuestPreferenceBO>();
            gstPrefList = guestPrefDA.GetGuestPreferenceInfoByGuestId(guestId);
            return hmCommonDA.GetPreferenceListView(gstPrefList);
        }
        [WebMethod]
        public static GuestInformationBO PerformViewActionForGuestDetail(int guestId)
        {
            GuestInformationBO guestBO = new GuestInformationBO();
            GuestInformationDA guestDA = new GuestInformationDA();
            guestBO = guestDA.GetGuestInformationByGuestId(guestId);

            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            registrationBO = registrationDA.GetGuestLastRegistrationByGuestId(guestId);
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(registrationBO.RoomNumber.ToString());

            guestBO.RoomRate = allocationBO.RoomRate;
            guestBO.CurrencyTypeHead = allocationBO.CurrencyTypeHead;
            guestBO.ArriveDate = registrationBO.ArriveDate;
            guestBO.ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
            guestBO.GuestId = registrationBO.GuestId;
            guestBO.RegistrationId = registrationBO.RegistrationId;
            guestBO.PaxInRate = registrationBO.PaxInRate;
            guestBO.CountryName = registrationBO.CountryName;

            return guestBO;
        }
        [WebMethod]
        public static GuestAirportPickUpDropReportViewBO LoadGuestAirportDrop(int registrationId)
        {
            GuestAirportPickUpDropDA hmCommonDA = new GuestAirportPickUpDropDA();
            List<GuestAirportPickUpDropReportViewBO> gstPrefList = new List<GuestAirportPickUpDropReportViewBO>();
            GuestAirportPickUpDropReportViewBO guestAirportPickUpDropReportViewBO = new GuestAirportPickUpDropReportViewBO();
            gstPrefList = hmCommonDA.GetGuestAirportDropInfoByRegistrationId(registrationId);

            foreach (GuestAirportPickUpDropReportViewBO row in gstPrefList)
            {
                guestAirportPickUpDropReportViewBO = row;
            }

            return guestAirportPickUpDropReportViewBO;
        }

    }
}