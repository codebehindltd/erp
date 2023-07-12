using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity;
using HotelManagement.Data;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Text.RegularExpressions;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomStatusInfo : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        Boolean isGuestNameDisplayFlag = false;
        bool isLinkedRoomFlag = false;
        protected int isFloorInformationEnable = -1;
        protected int isGroupInformationEnable = -1;
        protected int isLegendContainerDivEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            Session["MessegePanelEnableForSelectedRoomNumber"] = null;
            if (!IsPostBack)
            {
                LoadCurrency();
                ReservationNoShowProcess();
                LoadFloor();
                LoadFloorBlock();
                LoadCurrentDate();
                SetDefaulCleanTime();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                if (userInformationBO.RoomStatusFilteringType != null)
                {
                    ddlStatusType.SelectedValue = userInformationBO.RoomStatusFilteringType;
                }


                if (Request.QueryString["vs"] != null)
                {
                    if (Request.QueryString["vs"] == "rnw")
                    {
                        hfLinkStatus.Value = "1";
                        ddlStatusType.SelectedValue = "1";
                        GenerateRoomNumberWiseRoomStatusInfo();                        
                    }
                }
                    
            }

            string cancelRegistration = Request.QueryString["CancelRegistration"];
            if (!string.IsNullOrEmpty(cancelRegistration))
            {
                CancelBlankRegistration(cancelRegistration);
            }

            string expectedDeparture = Request.QueryString["ExpectedDeparture"];
            if (!string.IsNullOrEmpty(expectedDeparture))
            {
                ddlStatusType.SelectedValue = "6";
                ViewStautsProcess();
            }
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
        protected void btnViewStatus_Click(object sender, EventArgs e)
        {
            ViewStautsProcess();
        }
        private void ViewStautsProcess()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            userInformationBO.RoomStatusFilteringType = ddlStatusType.SelectedValue.ToString();
            Session.Add("UserInformationBOSession", userInformationBO);

            if (ddlStatusType.SelectedValue == "1")
            {
                if ((Convert.ToInt32(hfMasterId.Value) > 0))
                {
                    GenerateLinkedRoom();
                }
                else
                {
                    GenerateRoomNumberWiseRoomStatusInfo();
                    hfLinkStatus.Value = "1";
                }
            }
            else if (ddlStatusType.SelectedValue == "2")
            {
                if (Convert.ToInt32(hfMasterId.Value) > 0)
                {
                    GenerateLinkedRoom();
                }
                else
                {
                    GenerateRoomTypeWiseRoomStatusInfo();
                    hfLinkStatus.Value = "1";
                }
            }
            else if (ddlStatusType.SelectedValue == "3")
            {
                if (Convert.ToInt32(hfMasterId.Value) > 0)
                {
                    GenerateLinkedRoom();
                }
                else
                {
                    hfLinkStatus.Value = "1";
                    GenerateFloorWiseRoomStatusInfo();
                }
            }
            else if (ddlStatusType.SelectedValue == "4")
            {
                GenerateHoldBillRegistrationList();
            }
            else if (ddlStatusType.SelectedValue == "5")
            {
                GenerateBlankRegistrationList();
            }
            else if (ddlStatusType.SelectedValue == "6")
            {
                GenerateExpectedDepartureRoomInfo();
            }
        }
        protected void btnFloorViewStatus_Click(object sender, EventArgs e)
        {
            if (ddlStatusType.SelectedValue == "3")
            {
                isFloorInformationEnable = 1;
                isGroupInformationEnable = -1;
                hfLinkStatus.Value = "1";
                GenerateFloorWiseRoomStatusInfo();
            }
        }
        protected void btnBillPreview_Click(object sender, EventArgs e)
        {
            isLegendContainerDivEnable = 1;
            string url = "/HotelManagement/Reports/frmReportGuestBillPreview.aspx";
            string s = "window.open('" + url + "', 'popup_window', 'width=820,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(GetType(), "script", s, true);
        }
        protected void btnUSDBillPreview_Click(object sender, EventArgs e)
        {
            isLegendContainerDivEnable = 1;
            string url = "/HotelManagement/Reports/frmReportGuestBillPreview.aspx?ct=usd";
            string s = "window.open('" + url + "', 'popup_window', 'width=820,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(GetType(), "script", s, true);
        }
        protected void btnLinkedBillPreview_Click(object sender, EventArgs e)
        {
            isLegendContainerDivEnable = 1;
            string url = "/HotelManagement/Reports/frmReportGuestBillPreview.aspx?lrp=" + "1";
            string s = "window.open('" + url + "', 'popup_window', 'width=820,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(GetType(), "script", s, true);
        }
        //************************ User Defined Method ********************//
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            List<CommonCurrencyBO> localCurrencyListBO = new List<CommonCurrencyBO>();
            localCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Local").ToList();
            List<CommonCurrencyBO> UsdCurrencyListBO = new List<CommonCurrencyBO>();
            UsdCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Usd").ToList();

            ddlSellingPriceLocal.DataSource = localCurrencyListBO;
            ddlSellingPriceLocal.DataTextField = "CurrencyName";
            ddlSellingPriceLocal.DataValueField = "CurrencyId";
            ddlSellingPriceLocal.DataBind();
            ddlSellingPriceLocal.SelectedIndex = 0;
            lblSellingPriceLocal.Text = "Unit Price(" + ddlSellingPriceLocal.SelectedItem.Text + ")";
            btnLocalBillPreview.Text = "Bill Preview (" + ddlSellingPriceLocal.SelectedItem.Text + ")";

            ddlSellingPriceUsd.DataSource = UsdCurrencyListBO;
            ddlSellingPriceUsd.DataTextField = "CurrencyName";
            ddlSellingPriceUsd.DataValueField = "CurrencyId";
            ddlSellingPriceUsd.DataBind();
            ddlSellingPriceUsd.SelectedIndex = 1;
            lblSellingPriceUsd.Text = "Unit Price(" + ddlSellingPriceUsd.SelectedItem.Text + ")";
        }
        private void ReservationNoShowProcess()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            Boolean status = roomReservationDA.HotelRoomReservationNoShowProcess(DateTime.Now, userInformationBO.UserInfoId);
            if (status)
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomReservationNoShow.ToString(), 0, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationNoShow));
        }
        private void LoadFloor()
        {
            HMFloorDA floorDA = new HMFloorDA();
            ddlSrcFloorId.DataSource = floorDA.GetActiveHMFloorInfo();
            ddlSrcFloorId.DataTextField = "FloorName";
            ddlSrcFloorId.DataValueField = "FloorId";
            ddlSrcFloorId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcFloorId.Items.Insert(0, item);
        }
        private void LoadFloorBlock()
        {
            FloorBlockDA floorDA = new FloorBlockDA();
            List<FloorBlockBO> floorList = new List<FloorBlockBO>();
            floorList = floorDA.GetActiveFloorBlockInfo();

            ddlFloorBlock.DataSource = floorList;
            ddlFloorBlock.DataTextField = "BlockName";
            ddlFloorBlock.DataValueField = "BlockId";
            ddlFloorBlock.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlFloorBlock.Items.Insert(0, item);
        }
        private void LoadRoomStatus()
        {
            RoomStatusDA roomStatusDA = new RoomStatusDA();
            List<RoomStatusBO> List = roomStatusDA.GetRoomStatusInfo();
            ddlCleanStatus.DataSource = List;
            ddlCleanStatus.DataTextField = "StatusName";
            ddlCleanStatus.DataValueField = "StatusId";
            ddlCleanStatus.DataBind();
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            txtApprovedDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void SetDefaulCleanTime()
        {
            txtProbableCleanTime.Text = "12:00";
        }
        public bool isFormValid()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            bool status = true;
            string roomNumber = txtRoomNumber.Text;
            if (String.IsNullOrWhiteSpace(roomNumber))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Room Number.", AlertType.Warning);
                status = false;
                txtRoomNumber.Focus();
            }

            if (ddlCleanStatus.SelectedValue == "OutOfOrder")
            {
                if (hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat).Date < DateTime.Now.Date)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Date.", AlertType.Warning);
                    status = false;
                    txtFromDate.Focus();
                }
                else if (hmUtility.GetDateTimeFromString(txtToDate.Text, userInformationBO.ServerDateFormat).Date < DateTime.Now.Date)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Date.", AlertType.Warning);
                    status = false;
                    txtToDate.Focus();
                }
            }

            return status;
        }
        private void GenerateLegendContainer()
        {
            isLegendContainerDivEnable = 1;
            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfOrderDiv = 0;
            int RoomOutOfServiceDiv = 0;

            RoomNumberDA roomNumberDA = new RoomNumberDA();

            lblTotalRoomCount.Text = "Room Status";
            List<RoomNumberBO> roomNumberInfo = roomNumberDA.GetRoomNumberInfo();
            if (roomNumberInfo != null)
            {
                lblTotalRoomCount.Text = "Total Room: " + roomNumberInfo.Count() + "";
            }

            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();
            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(DateTime.Now, DateTime.Now.AddDays(1));

            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                if (roomNumberListBO[iRoomNumber].StatusId == 4)
                {
                    RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                {
                    RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                    {
                        RoomLongStayingDiv = RoomLongStayingDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                    {
                        RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                    {
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                    else
                    {
                        RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                    }
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                    {
                        RoomVacantDiv = RoomVacantDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                    {
                        RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                    {
                        RoomReservedDiv = RoomReservedDiv + 1;
                    }
                }
            }


            string fullContent = string.Empty;
            string strAllRoomStatusLegent = string.Empty;

            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customFieldData = new CustomFieldBO();
            customFieldData = hmCommonDA.GetCustomFieldByFieldName("AllRoomStatusLegent");
            if (customFieldData != null)
            {
                if (customFieldData.FieldId > 0)
                {
                    strAllRoomStatusLegent = customFieldData.FieldValue;
                }
            }

            fullContent += "<div class=\"form-group\">";
            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[0] + "' class='legend RoomVacantDivNoAction'>" + RoomVacantDiv.ToString() + "</div><div class='legendLeftText'>Vacant</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[1] + "' class='legend RoomTodaysCheckInDivNoAction'>" + RoomTodaysCheckInDiv.ToString() + "</div><div class='legendRightText'>Today\'s Checked In</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[2] + "' class='legend RoomOccupaiedDivNoAction'>" + RoomOccupaiedDiv.ToString() + "</div><div class='legendLeftText'>Occupied</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[3] + "' class='legend RoomPossibleVacantDivNoAction'>" + RoomPossibleVacantDiv.ToString() + "</div><div class='legendRightText'>Expected Departure</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[4] + "' class='legend RoomVacantDirtyDivNoAction'>" + RoomVacantDirtyDiv.ToString() + "</div><div class='legendLeftText'>Vacant Dirty</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[5] + "' class='legend RoomLongStayingDivNoAction'>" + RoomLongStayingDiv.ToString() + "</div><div class='legendRightText'>Long Staying</div>";
            fullContent += "</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"form-group\">";
            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[8] + "' class='legend RoomReservedDivNoAction'>" + RoomReservedDiv.ToString() + "</div><div class='legendLeftText'>Reserved</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[6] + "' class='legend RoomOutOfOrderDivNoAction'>" + RoomOutOfOrderDiv.ToString() + "</div><div class='legendLeftText'>Out of Order</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[7] + "' class='legend RoomOutOfServiceDivNoAction'>" + RoomOutOfServiceDiv.ToString() + "</div><div class='legendRightText'>Out of Service</div>";
            fullContent += "</div>";
            fullContent += "</div>";

            ltlRoomStatusLegent.InnerHtml = fullContent;
        }
        private void GenerateSearchCriteriaLegendContainer()
        {
            isLegendContainerDivEnable = 1;
            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfOrderDiv = 0;
            int RoomOutOfServiceDiv = 0;

            RoomNumberDA roomNumberDA = new RoomNumberDA();

            lblTotalRoomCount.Text = "Room Status";
            List<RoomNumberBO> roomNumberInfo = roomNumberDA.GetRoomNumberInfo();
            if (roomNumberInfo != null)
            {
                lblTotalRoomCount.Text = "Total Room: " + roomNumberInfo.Count() + "";
            }

            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();
            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            //// ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(DateTime.Now, DateTime.Now.AddDays(1));

            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                if (roomNumberListBO[iRoomNumber].StatusId == 4)
                {
                    RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                {
                    RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                    {
                        RoomLongStayingDiv = RoomLongStayingDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                    {
                        RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                    {
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                    else
                    {
                        RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                    }
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                    {
                        RoomVacantDiv = RoomVacantDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                    {
                        RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                    {
                        RoomReservedDiv = RoomReservedDiv + 1;
                    }
                }
            }


            string fullContent = string.Empty;
            string strAllRoomStatusLegent = string.Empty;

            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customFieldData = new CustomFieldBO();
            customFieldData = hmCommonDA.GetCustomFieldByFieldName("AllRoomStatusLegent");
            if (customFieldData != null)
            {
                if (customFieldData.FieldId > 0)
                {
                    strAllRoomStatusLegent = customFieldData.FieldValue;
                }
            }

            fullContent += "<div class=\"form-group\">";
            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[0] + "' class='legend RoomVacantDivNoAction'>" + RoomVacantDiv.ToString() + "</div><div class='legendLeftText'>Vacant</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[1] + "' class='legend RoomTodaysCheckInDivNoAction'>" + RoomTodaysCheckInDiv.ToString() + "</div><div class='legendRightText'>Today\'s Checked In</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[2] + "' class='legend RoomOccupaiedDivNoAction'>" + RoomOccupaiedDiv.ToString() + "</div><div class='legendLeftText'>Occupied</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[3] + "' class='legend RoomPossibleVacantDivNoAction'>" + RoomPossibleVacantDiv.ToString() + "</div><div class='legendRightText'>Expected Departure</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[4] + "' class='legend RoomVacantDirtyDivNoAction'>" + RoomVacantDirtyDiv.ToString() + "</div><div class='legendLeftText'>Vacant Dirty</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[5] + "' class='legend RoomLongStayingDivNoAction'>" + RoomLongStayingDiv.ToString() + "</div><div class='legendRightText'>Long Staying</div>";
            fullContent += "</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"form-group\">";
            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[8] + "' class='legend RoomReservedDivNoAction'>" + RoomReservedDiv.ToString() + "</div><div class='legendLeftText'>Reserved</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[6] + "' class='legend RoomOutOfOrderDivNoAction'>" + RoomOutOfOrderDiv.ToString() + "</div><div class='legendLeftText'>Out of Order</div>";
            fullContent += "</div>";

            fullContent += "<div class=\"col-md-2\">";
            fullContent += "<div style='background-color:" + strAllRoomStatusLegent.Split('~')[7] + "' class='legend RoomOutOfServiceDivNoAction'>" + RoomOutOfServiceDiv.ToString() + "</div><div class='legendRightText'>Out of Service</div>";
            fullContent += "</div>";
            fullContent += "</div>";

            ltlRoomStatusLegent.InnerHtml = fullContent;
        }
        private void GenerateRoomTypeWiseRoomStatusInfo()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();

            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            //-----linked room info ------------
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomAlocationBO> linkedRoomInfoBOList = new List<RoomAlocationBO>();
            linkedRoomInfoBOList = roomRegistrationDA.GetLinkedRoomInfo();



            // get room type
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();

            string fullContent = string.Empty;

            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfServiceDiv = 0;
            int RoomOutOfOrderDiv = 0;

            string roomSummary = string.Empty;

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();

            //if (ddlGuestNameDisplayFlag.SelectedValue == "0")
            //{
            //    isGuestNameDisplayFlag = false;
            //}
            //else
            //{

            //}
            isGuestNameDisplayFlag = true;
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(DateTime.Now, DateTime.Now.AddDays(1));

            //room search start
            string srchRoomNumber = txtSrchRoomNumber.Text;
            var validRoom = new RoomNumberBO();

            var searchedRoom = new RoomNumberBO();
            var searchedRoomType = new RoomNumberBO();

            if (srchRoomNumber != "")
            {
                //validRoom = roomNumberDA.GetRoomInfoByRoomNumber(srchRoomNumber);
                //if (validRoom.RoomId == 0)
                //{
                //    CommonHelper.AlertInfo("No Room Found.", "Warning");

                //}
                searchedRoomType = roomNumberDA.GetRoomInfoByRoomNumber(srchRoomNumber);

                string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
                string topTemplatePartEnd = @"</div>";
                string bodyPart = @"<div class='panel-body'>";
                string endTemplatePart = @"</div></div>";

                string subContent = string.Empty;
                if (searchedRoomType.RoomId == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "You have entered wrong room number.", AlertType.Warning);
                    return;
                }

                roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(searchedRoomType.RoomTypeId);

                searchedRoom = (from data in roomNumberListBO
                                where data.RoomNumber == srchRoomNumber
                                select data).FirstOrDefault();

                var getLinkedRoom = linkedRoomInfoBOList.Where(x => x.RoomId == searchedRoom.RoomId).ToList();
                if (getLinkedRoom.Count > 0)
                {
                    isLinkedRoomFlag = true;
                }
                else
                {
                    isLinkedRoomFlag = false;
                }

                if (searchedRoom.RoomNumber != "")
                {
                    if (isGuestNameDisplayFlag)
                    {
                        singleRoomInfoBO = calenderList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                    }

                    if (searchedRoom.StatusId == 4)
                    {
                        GuestInformationDA guestInfoDa = new GuestInformationDA();
                        List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                        roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                        var roomIdListBO = roomIdList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        subContent += Content1 + Content2;

                        RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                        //subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        //RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                    }
                    else if (searchedRoom.StatusId == 3)
                    {
                        GuestInformationDA guestInfoDa = new GuestInformationDA();
                        List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                        roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                        var roomIdListBO = roomIdList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        subContent += Content1 + Content2;

                        RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                        //subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        //RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                    }
                    else if (searchedRoom.StatusId == 2)
                    {
                        if (searchedRoom.CSSClassName == "RoomTodaysCheckInDiv")
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomLongStayingDiv")
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomLongStayingDiv = RoomLongStayingDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomPossibleVacantDiv")
                        {
                            if (searchedRoom.IsBillLockedAndPreview == 1)
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;

                        }
                        else
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                        }
                    }
                    else if (searchedRoom.StatusId == 1)
                    {
                        if (searchedRoom.CSSClassName == "RoomVacantDiv")
                        {
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            subContent += Content1 + Content2;
                            RoomVacantDiv = RoomVacantDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomVacantDirtyDiv")
                        {
                            GuestInformationDA guestInfoDa = new GuestInformationDA();
                            List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                            roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                            var roomIdListBO = roomIdList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();

                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";
                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            subContent += Content1 + Content2;
                            RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;

                        }
                        else if (searchedRoom.CSSClassName == "RoomReservedDiv")
                        {
                            string Content1 = "";
                            if (isGuestNameDisplayFlag)
                            {
                                Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div>";
                            }

                            string Content2 = @"<div style='display:none;'  class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            subContent += Content1 + Content2;
                            RoomReservedDiv = RoomReservedDiv + 1;
                        }
                    }
                    roomSummary = " (Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfOrderDiv + ", Out of Service: " + RoomOutOfServiceDiv + ")";
                    string groupNamePart = RoomTypeInfoList[0].RoomType + roomSummary;

                    RoomVacantDirtyDiv = 0;
                    RoomPossibleVacantDiv = 0;
                    RoomTodaysCheckInDiv = 0;
                    RoomLongStayingDiv = 0;
                    RoomOccupaiedDiv = 0;
                    RoomReservedDiv = 0;
                    RoomVacantDiv = 0;
                    RoomOutOfServiceDiv = 0;
                    RoomOutOfOrderDiv = 0;

                    fullContent += topPart + groupNamePart + topTemplatePartEnd + bodyPart + subContent + endTemplatePart;

                }
            } //Room search end
            else
            {
                for (int statusInfo = 0; statusInfo < RoomTypeInfoList.Count; statusInfo++)
                {
                    roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(RoomTypeInfoList[statusInfo].RoomTypeId);

                    string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
                    string topTemplatePartEnd = @"</div>";
                    string bodyPart = @"<div class='panel-body'>";
                    string endTemplatePart = @"</div></div>";

                    string subContent = string.Empty;


                    for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                    {
                        var getLinkedRoom = linkedRoomInfoBOList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).ToList();

                        if (getLinkedRoom.Count > 0)
                        {
                            isLinkedRoomFlag = true;
                        }
                        else
                        {
                            isLinkedRoomFlag = false;
                        }
                        if (isGuestNameDisplayFlag)
                        {
                            singleRoomInfoBO = calenderList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                        }

                        if (roomNumberListBO[iRoomNumber].StatusId == 4)
                        {
                            GuestInformationDA guestInfoDa = new GuestInformationDA();
                            List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                            roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                            var roomIdListBO = roomIdList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";

                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            subContent += Content1 + Content2;
                            //subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                        {
                            GuestInformationDA guestInfoDa = new GuestInformationDA();
                            List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                            roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                            var roomIdListBO = roomIdList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";

                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            subContent += Content1 + Content2;

                            //subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                        {
                            if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                RoomLongStayingDiv = RoomLongStayingDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                    {
                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                    {
                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                }
                                else //without guest name
                                {
                                    if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                    {

                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                    {
                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                }
                                RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                            }
                            else
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                            }
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                        {
                            if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                            {
                                string Content1 = "<div class='DivRoomContainerHeight61'>";
                                string Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                subContent += Content1 + Content2;
                                RoomVacantDiv = RoomVacantDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                            {
                                GuestInformationDA guestInfoDa = new GuestInformationDA();
                                List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                                roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                                var roomIdListBO = roomIdList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();

                                string Content1 = "<div class='DivRoomContainerHeight61'>";
                                string Content2 = "";
                                if (roomIdListBO != null)
                                {
                                    Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                subContent += Content1 + Content2;
                                RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;

                                //string Content1 = "<div class='DivRoomContainerHeight61'>";
                                //string Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                //subContent += Content1 + Content2;
                                //RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                            {
                                string Content1 = "";
                                if (isGuestNameDisplayFlag)
                                {
                                    Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div>";
                                }
                                else
                                {
                                    Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div>";
                                }

                                string Content2 = @"<div style='display:none;'  class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                subContent += Content1 + Content2;
                                RoomReservedDiv = RoomReservedDiv + 1;
                            }
                        }
                    }

                    roomSummary = " (Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfOrderDiv + ", Out of Service: " + RoomOutOfServiceDiv + ")";
                    string groupNamePart = RoomTypeInfoList[statusInfo].RoomType + roomSummary;

                    RoomVacantDirtyDiv = 0;
                    RoomPossibleVacantDiv = 0;
                    RoomTodaysCheckInDiv = 0;
                    RoomLongStayingDiv = 0;
                    RoomOccupaiedDiv = 0;
                    RoomReservedDiv = 0;
                    RoomVacantDiv = 0;
                    RoomOutOfServiceDiv = 0;
                    RoomOutOfOrderDiv = 0;

                    fullContent += topPart + groupNamePart + topTemplatePartEnd + bodyPart + subContent + endTemplatePart;

                }
            }

            ltlRoomTemplate.InnerHtml = fullContent;
            GenerateLegendContainer();
            txtSrchRoomNumber.Text = "";
        }
        private void GenerateLinkedRoom()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();

            var masterId = Convert.ToInt64(hfMasterId.Value);



            //-----linked room info ------------
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomAlocationBO> AllLinkedRoomInfoBOList = new List<RoomAlocationBO>();
            //List<RoomAlocationBO> linkedRooms = new List<RoomAlocationBO>();


            AllLinkedRoomInfoBOList = roomRegistrationDA.GetLinkedRoomInfo();
            var linkedRooms = AllLinkedRoomInfoBOList.Where(x => x.MasterId == masterId).ToList();


            string fullContent = string.Empty;
            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfOrderDiv = 0;
            int RoomOutOfServiceDiv = 0;

            string roomSummary = string.Empty;

            string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
            string topTemplatePartEnd = @"</div>";
            string bodyPart = @"<div class='panel-body'>";
            string endTemplatePart = @"</div></div>";

            string groupNamePart = string.Empty;

            string subContent = string.Empty;

            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();

            //if (ddlGuestNameDisplayFlag.SelectedValue == "0")
            //{
            //    isGuestNameDisplayFlag = false;
            //}
            //else
            //{

            //}
            isGuestNameDisplayFlag = true;
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(DateTime.Now, DateTime.Now.AddDays(1));

            for (int iRoomNumber = 0; iRoomNumber < linkedRooms.Count; iRoomNumber++)
            {
                var selectedRoom = roomNumberListBO.Where(x => x.RoomNumber == linkedRooms[iRoomNumber].RoomNumber).ToList();

                if (isGuestNameDisplayFlag)
                {
                    singleRoomInfoBO = calenderList.Where(x => x.RoomId == linkedRooms[iRoomNumber].RoomId).FirstOrDefault();
                }
                if (selectedRoom.Count > 0)
                {
                    isLinkedRoomFlag = true;
                }
                else
                {
                    isLinkedRoomFlag = false;
                }
                if (selectedRoom[0].StatusId == 2)
                {
                    if (selectedRoom[0].CSSClassName == "RoomLongStayingDiv")
                    {
                        if (isGuestNameDisplayFlag)
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }

                        }
                        else
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }
                            else
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                        }

                        RoomLongStayingDiv = RoomLongStayingDiv + 1;
                    }
                    else if (selectedRoom[0].CSSClassName == "RoomPossibleVacantDiv")
                    {
                        if (isGuestNameDisplayFlag)// with guest name
                        {
                            if (selectedRoom[0].IsBillLockedAndPreview == 1)
                            {
                                if (isLinkedRoomFlag)
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                                else
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                                else
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                            }
                        }
                        else
                        {
                            if (selectedRoom[0].IsBillLockedAndPreview == 1)
                            {
                                if (isLinkedRoomFlag)
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                                else
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                                else
                                {

                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                }
                            }
                            //if (isLinkedRoomFlag)
                            //{
                            //    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            //    //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            //}
                            //else
                            //    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                        }
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                    else if (selectedRoom[0].CSSClassName == "RoomTodaysCheckInDiv")
                    {
                        if (isGuestNameDisplayFlag)
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }

                        }
                        else
                        {
                            if (isLinkedRoomFlag)
                            {
                                //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }
                            else
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                        }
                        RoomTodaysCheckInDiv += 1;
                    }
                    else
                    {
                        if (isGuestNameDisplayFlag)
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";

                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }
                            //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        }
                        else
                        {
                            if (isLinkedRoomFlag)
                            {
                                //without guest name 
                                //subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                            }
                            else
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + selectedRoom[0].ColorCodeName + "' class='" + selectedRoom[0].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + selectedRoom[0].TypeCode + ":  " + selectedRoom[0].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + selectedRoom[0].RoomNumber + "</div></div>";
                        }

                        RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                    }
                }
            }
            roomSummary = " ( Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ")";
            groupNamePart = "Status" + roomSummary;
            fullContent += subContent;
            ltlRoomTemplate.InnerHtml = topPart + groupNamePart + topTemplatePartEnd + bodyPart + fullContent + endTemplatePart;
            GenerateSearchCriteriaLegendContainer();
            //hfMasterId
            //hfLinkStatus

            hfMasterId.Value = "0";
        }
        private void GenerateRoomNumberWiseRoomStatusInfo()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            //-----linked room info ------------
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomAlocationBO> linkedRoomInfoBOList = new List<RoomAlocationBO>();
            linkedRoomInfoBOList = roomRegistrationDA.GetLinkedRoomInfo();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();

            string fullContent = string.Empty;
            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfOrderDiv = 0;
            int RoomOutOfServiceDiv = 0;

            string roomSummary = string.Empty;

            string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
            string topTemplatePartEnd = @"</div>";
            string bodyPart = @"<div class='panel-body'>";
            string endTemplatePart = @"</div></div>";

            string groupNamePart = string.Empty;

            string subContent = string.Empty;
            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();


            DateTime processDate = DateTime.Now;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                processDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            isGuestNameDisplayFlag = true;
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(processDate, processDate.AddDays(1));

            GuestInformationDA guestInfoDA = new GuestInformationDA();
            List<GuestHouseInfoForReportBO> TodaysExpectedArrivalroomList = new List<GuestHouseInfoForReportBO>();
            TodaysExpectedArrivalroomList = guestInfoDA.GetTodaysExpectedArrivalRoomId();

            //room search start
            string srchRoomNumber = txtSrchRoomNumber.Text;
            var validRoom = new RoomNumberBO();

            var searchedRoom = new RoomNumberBO();
            var searchedRoomType = new RoomNumberBO();

            if (srchRoomNumber != "")
            {
                searchedRoomType = roomNumberDA.GetRoomInfoByRoomNumber(srchRoomNumber);
                if (searchedRoomType.RoomId == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "You have entered wrong room number.", AlertType.Warning);
                    return;
                }
                searchedRoom = (from data in roomNumberListBO
                                where data.RoomNumber == srchRoomNumber
                                select data).FirstOrDefault();

                var getLinkedRoom = linkedRoomInfoBOList.Where(x => x.RoomId == searchedRoom.RoomId).ToList();
                if (getLinkedRoom.Count > 0)
                {
                    isLinkedRoomFlag = true;
                }
                else
                {
                    isLinkedRoomFlag = false;
                }

                if (searchedRoom.RoomNumber != "")
                {
                    if (isGuestNameDisplayFlag)
                    {
                        singleRoomInfoBO = calenderList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                    }

                    if (searchedRoom.StatusId == 4)
                    {
                        string roomId = "" + searchedRoom.RoomId;
                        var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }

                        string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, searchedRoom.RoomNumber, searchedRoom.RoomType, searchedRoom.RoomName);

                        subContent += tooltipContent + Content1 + Content2;
                        RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                    }
                    else if (searchedRoom.StatusId == 3)
                    {
                        string roomId = "" + searchedRoom.RoomId;
                        var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }

                        string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, searchedRoom.RoomNumber, searchedRoom.RoomType, searchedRoom.RoomName);

                        subContent += tooltipContent + Content1 + Content2;

                        RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                    }
                    else if (searchedRoom.StatusId == 2)
                    {
                        if (searchedRoom.CSSClassName == "RoomTodaysCheckInDiv")
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            else
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomLongStayingDiv")
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomLongStayingDiv = RoomLongStayingDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomPossibleVacantDiv")
                        {
                            if (searchedRoom.IsBillLockedAndPreview == 1)
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;

                        }
                        else
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                        }
                    }
                    else if (searchedRoom.StatusId == 1)
                    {
                        if (searchedRoom.CSSClassName == "RoomVacantDiv")
                        {
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            subContent += Content1 + Content2;
                            RoomVacantDiv = RoomVacantDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomVacantDirtyDiv")
                        {
                            var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";
                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            subContent += Content1 + Content2;
                            RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;

                        }
                        else if (searchedRoom.CSSClassName == "RoomReservedDiv")
                        {
                            string Content1 = "";
                            if (isGuestNameDisplayFlag)
                            {
                                Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div>";
                            }

                            string Content2 = @"<div style='display:none;'  class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            subContent += Content1 + Content2;
                            RoomReservedDiv = RoomReservedDiv + 1;
                        }
                    }

                }
            } //Room search end
            else
            {
                for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                {
                    var getLinkedRoom = linkedRoomInfoBOList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).ToList();

                    if (getLinkedRoom.Count > 0)
                    {
                        isLinkedRoomFlag = true;
                    }
                    else
                    {
                        isLinkedRoomFlag = false;
                    }

                    if (isGuestNameDisplayFlag)
                    {
                        singleRoomInfoBO = calenderList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                    }
                    if (roomNumberListBO[iRoomNumber].StatusId == 4)
                    {
                        var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        }

                        string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                        subContent += tooltipContent + Content1 + Content2;

                        RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                    {
                        var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        }

                        string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                        subContent += tooltipContent + Content1 + Content2;

                        RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                    {
                        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                        {
                            string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;

                            if (isGuestNameDisplayFlag)
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }

                            string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                            subContent += tooltipContent;
                            RoomLongStayingDiv = RoomLongStayingDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                        {
                            string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;

                            if (isGuestNameDisplayFlag)// with guest name
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "   " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0'/> <br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                }
                                else// not locked
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }
                            else // without guest name
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {

                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }

                            string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                            subContent += tooltipContent;


                            RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                        {
                            string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;

                            if (isGuestNameDisplayFlag)// with guest name
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "   " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0'/> <br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                }
                                else// not locked
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }
                            else // without guest name
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {

                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }

                            string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                            subContent += tooltipContent;
                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                        }
                        else
                        {
                            string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;
                            if (isGuestNameDisplayFlag)
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                }
                                else
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }

                            string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                            subContent += tooltipContent;
                            RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                        }
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                    {
                        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                        {
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;

                            string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                            string Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            subContent += tooltipContent + Content1 + Content2;
                            RoomVacantDiv = RoomVacantDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                        {
                            var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";
                            string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;

                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }

                            string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                            subContent += tooltipContent + Content1 + Content2;
                            RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                        {
                            string Content1 = "";
                            string roomId = "" + roomNumberListBO[iRoomNumber].RoomId;
                            if (isGuestNameDisplayFlag)
                            {
                                Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div>";
                            }
                            else
                            {
                                Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;' class='ToolTipClass' id='" + roomId + "'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div>";
                            }

                            string tooltipContent = this.hmUtility.GetTooltipContainer(roomId, roomNumberListBO[iRoomNumber].RoomNumber, roomNumberListBO[iRoomNumber].RoomType, roomNumberListBO[iRoomNumber].RoomName);

                            string Content2 = @"<div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            subContent += tooltipContent + Content1 + Content2;
                            RoomReservedDiv = RoomReservedDiv + 1;
                        }
                    }
                }
            }
            roomSummary = " (Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfOrderDiv + ", Out of Service: " + RoomOutOfServiceDiv + ")";
            groupNamePart = "Status" + roomSummary;
            fullContent += subContent;
            ltlRoomTemplate.InnerHtml = topPart + groupNamePart + topTemplatePartEnd + bodyPart + fullContent + endTemplatePart;
            GenerateSearchCriteriaLegendContainer();
            txtSrchRoomNumber.Text = "";
        }
        private void GenerateFloorWiseRoomStatusInfo()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            HMFloorManagementDA floorManagementDA = new HMFloorManagementDA();
            List<HMFloorManagementBO> roomNumberListBO = new List<HMFloorManagementBO>();
            List<HMFloorManagementBO> tempRoomNumberListBO = new List<HMFloorManagementBO>();

            HMFloorBO floorBO = new HMFloorBO();
            HMFloorDA floorDA = new HMFloorDA();
            List<HMFloorBO> floorTypeList = new List<HMFloorBO>();


            int selectedFloor = Convert.ToInt32(ddlSrcFloorId.SelectedValue);
            int selectedBlock = Convert.ToInt32(ddlFloorBlock.SelectedValue);
            int selectedDesign = Convert.ToInt32(ddlFloorDesign.SelectedValue);

            if (selectedFloor == 0 && selectedBlock == 0)
            {
                floorTypeList = floorDA.GetActiveHMFloorInfo();
            }
            else
            {
                floorTypeList = floorDA.GetActiveHMFloorInfo().Where(x => x.FloorId == selectedFloor).ToList();
            }

            //-----linked room info ------------
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomAlocationBO> linkedRoomInfoBOList = new List<RoomAlocationBO>();
            linkedRoomInfoBOList = roomRegistrationDA.GetLinkedRoomInfo();

            string fullContent = string.Empty;
            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfOrderDiv = 0;
            int RoomOutOfServiceDiv = 0;

            string roomSummary = string.Empty;

            string groupNamePart = string.Empty;

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();

            isGuestNameDisplayFlag = true;
            RoomCalenderDA calenderDA = new RoomCalenderDA();

            DateTime processDate = DateTime.Now;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                processDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            calenderList = calenderDA.GetRoomInfoForCalender(processDate, processDate.AddDays(1));
            //room search start
            string srchRoomNumber = txtSrchRoomNumber.Text;
            var validRoom = new RoomNumberBO();

            var searchedRoom = new HMFloorManagementBO();
            var searchedRoomFloor = new HMFloorManagementBO();
            var searchedRoomType = new RoomNumberBO();

            if (srchRoomNumber != "")
            {
                searchedRoomType = roomNumberDA.GetRoomInfoByRoomNumber(srchRoomNumber);
                if (searchedRoomType.RoomId == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "You have entered wrong room number.", AlertType.Warning);
                    return;
                }
                searchedRoomFloor = floorManagementDA.GetRoomInfoByRoomId(searchedRoomType.RoomId);

                string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
                string topTemplatePartEnd = @"</div>";
                string bodyPart = @"<div class='panel-body'>";
                string endTemplatePart = @"</div></div>";

                string subContent = string.Empty;
                roomNumberListBO = floorManagementDA.GetHMFloorManagementInfoByFloorNBlockId(searchedRoomFloor.FloorId, searchedRoomFloor.BlockId);

                searchedRoom = (from data in roomNumberListBO
                                where data.RoomNumber == srchRoomNumber
                                select data).FirstOrDefault();

                var getLinkedRoom = linkedRoomInfoBOList.Where(x => x.RoomId == searchedRoom.RoomId).ToList();
                if (getLinkedRoom.Count > 0)
                {
                    isLinkedRoomFlag = true;
                }
                else
                {
                    isLinkedRoomFlag = false;
                }

                if (searchedRoom.RoomNumber != "")
                {
                    if (isGuestNameDisplayFlag)
                    {
                        singleRoomInfoBO = calenderList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                    }

                    if (searchedRoom.StatusId == 4)
                    {
                        GuestInformationDA guestInfoDa = new GuestInformationDA();
                        List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                        roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                        var roomIdListBO = roomIdList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        subContent += Content1 + Content2;

                        RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                    }
                    else if (searchedRoom.StatusId == 3)
                    {
                        GuestInformationDA guestInfoDa = new GuestInformationDA();
                        List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                        roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                        var roomIdListBO = roomIdList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = "";

                        if (roomIdListBO != null)
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        else
                        {
                            Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                        }
                        subContent += Content1 + Content2;

                        RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                    }
                    else if (searchedRoom.StatusId == 2)
                    {
                        if (searchedRoom.CSSClassName == "RoomTodaysCheckInDiv")
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomLongStayingDiv")
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomLongStayingDiv = RoomLongStayingDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomPossibleVacantDiv")
                        {
                            if (searchedRoom.IsBillLockedAndPreview == 1)
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;

                        }
                        else
                        {
                            if (isLinkedRoomFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                        }
                    }
                    else if (searchedRoom.StatusId == 1)
                    {
                        if (searchedRoom.CSSClassName == "RoomVacantDiv")
                        {
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            subContent += Content1 + Content2;
                            RoomVacantDiv = RoomVacantDiv + 1;
                        }
                        else if (searchedRoom.CSSClassName == "RoomVacantDirtyDiv")
                        {
                            GuestInformationDA guestInfoDa = new GuestInformationDA();
                            List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                            roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                            var roomIdListBO = roomIdList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();

                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";
                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            subContent += Content1 + Content2;
                            RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;

                        }
                        else if (searchedRoom.CSSClassName == "RoomReservedDiv")
                        {
                            string Content1 = "";
                            if (isGuestNameDisplayFlag)
                            {
                                Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div>";
                            }

                            string Content2 = @"<div style='display:none;'  class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            subContent += Content1 + Content2;
                            RoomReservedDiv = RoomReservedDiv + 1;
                        }
                    }
                    roomSummary = " (Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfOrderDiv + ", Out of Service: " + RoomOutOfServiceDiv + ")";
                    groupNamePart = floorTypeList[0].FloorName + roomSummary;

                    RoomVacantDirtyDiv = 0;
                    RoomPossibleVacantDiv = 0;
                    RoomTodaysCheckInDiv = 0;
                    RoomLongStayingDiv = 0;
                    RoomOccupaiedDiv = 0;
                    RoomReservedDiv = 0;
                    RoomVacantDiv = 0;
                    RoomOutOfServiceDiv = 0;
                    RoomOutOfOrderDiv = 0;

                    fullContent += topPart + groupNamePart + topTemplatePartEnd + bodyPart + subContent + endTemplatePart;
                }
            } //Room search end
            else
            {
                for (int iFloorInfo = 0; iFloorInfo < floorTypeList.Count; iFloorInfo++)//--  for floor information-----------------------------
                {
                    roomNumberListBO = floorManagementDA.GetHMFloorManagementInfoByFloorNBlockId(floorTypeList[iFloorInfo].FloorId, Convert.ToInt32(ddlFloorBlock.SelectedValue));

                    string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
                    string topTemplatePartEnd = @"</div>";
                    string bodyPart = @"<div class='panel-body'>";
                    string endTemplatePart = @"</div></div>";

                    string subContent = string.Empty;

                    for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                    {
                        var getLinkedRoom = linkedRoomInfoBOList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).ToList();

                        if (getLinkedRoom.Count > 0)
                        {
                            isLinkedRoomFlag = true;
                        }
                        else
                        {
                            isLinkedRoomFlag = false;
                        }

                        if (isGuestNameDisplayFlag)
                        {
                            singleRoomInfoBO = calenderList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                        }
                        if (roomNumberListBO[iRoomNumber].StatusId == 4)
                        {
                            GuestInformationDA guestInfoDa = new GuestInformationDA();
                            List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                            roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                            var roomIdListBO = roomIdList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";

                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            subContent += Content1 + Content2;
                            RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                        {
                            GuestInformationDA guestInfoDa = new GuestInformationDA();
                            List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                            roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                            var roomIdListBO = roomIdList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";

                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; text-align:center; margin-top:17px; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            subContent += Content1 + Content2;

                            RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                        {
                            if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }

                                RoomLongStayingDiv = RoomLongStayingDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }

                                RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                    {
                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                    {
                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                }
                                else
                                {
                                    if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                    {

                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                    }
                                    else
                                    {
                                        if (isLinkedRoomFlag)
                                        {
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                        }
                                        else
                                            subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                }

                                RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                            }
                            else
                            {
                                if (isGuestNameDisplayFlag)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                            }
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                        {
                            if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                            {
                                string Content1 = "<div class='DivRoomContainerHeight61'>";
                                string Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                subContent += Content1 + Content2;
                                RoomVacantDiv = RoomVacantDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                            {
                                GuestInformationDA guestInfoDa = new GuestInformationDA();
                                List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                                roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                                var roomIdListBO = roomIdList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();

                                string Content1 = "<div class='DivRoomContainerHeight61'>";
                                string Content2 = "";
                                if (roomIdListBO != null)
                                {
                                    Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }

                                subContent += Content1 + Content2;
                                RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
                            }
                            else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                            {
                                string Content1 = "";
                                if (isGuestNameDisplayFlag)
                                {
                                    Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div>";
                                }
                                else
                                {
                                    Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div>";
                                }

                                string Content2 = @"<div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                subContent += Content1 + Content2;
                                RoomReservedDiv = RoomReservedDiv + 1;
                            }
                        }
                    }

                    roomSummary = " (Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfOrderDiv + ", Out of Service: " + RoomOutOfServiceDiv + ")";

                    groupNamePart = floorTypeList[iFloorInfo].FloorName + roomSummary;

                    RoomVacantDirtyDiv = 0;
                    RoomPossibleVacantDiv = 0;
                    RoomTodaysCheckInDiv = 0;
                    RoomLongStayingDiv = 0;
                    RoomOccupaiedDiv = 0;
                    RoomReservedDiv = 0;
                    RoomVacantDiv = 0;
                    RoomOutOfServiceDiv = 0;
                    RoomOutOfOrderDiv = 0;

                    fullContent += topPart + groupNamePart + topTemplatePartEnd + bodyPart + subContent + endTemplatePart;
                }
            }

            ltlRoomTemplate.InnerHtml = fullContent;
            GenerateSearchCriteriaLegendContainer();
            if ((selectedBlock != 0 && selectedFloor != 0 && selectedDesign == 2))
            {
                GenerateRoomAllocation();
            }
            txtSrchRoomNumber.Text = "";
        }
        private void GenerateFloorWiseRoomStatusInfo_old()
        {
            HMFloorManagementDA floorManagementDA = new HMFloorManagementDA();
            List<HMFloorManagementBO> roomNumberListBO = new List<HMFloorManagementBO>();

            HMFloorBO floorBO = new HMFloorBO();
            HMFloorDA floorDA = new HMFloorDA();
            List<HMFloorBO> floorTypeList = new List<HMFloorBO>();

            int selectedFloor = Convert.ToInt32(ddlSrcFloorId.SelectedValue);
            if (selectedFloor == 0)
            {
                floorTypeList = floorDA.GetActiveHMFloorInfo();
            }
            else
            {
                floorTypeList = floorDA.GetActiveHMFloorInfo().Where(x => x.FloorId == selectedFloor).ToList();
            }

            string fullContent = string.Empty;
            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfOrderDiv = 0;
            int RoomOutOfServiceDiv = 0;

            string roomSummary = string.Empty;
            string groupNamePart = string.Empty;

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();

            isGuestNameDisplayFlag = true;
            RoomCalenderDA calenderDA = new RoomCalenderDA();

            DateTime processDate = DateTime.Now;

            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                processDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            calenderList = calenderDA.GetRoomInfoForCalender(processDate, processDate.AddDays(1));

            for (int iFloorInfo = 0; iFloorInfo < floorTypeList.Count; iFloorInfo++)
            {
                roomNumberListBO = floorManagementDA.GetHMFloorManagementInfoByFloorNBlockId(floorTypeList[iFloorInfo].FloorId, Convert.ToInt32(ddlFloorBlock.SelectedValue));

                string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
                string topTemplatePartEnd = @"</div>";
                string bodyPart = @"<div class='panel-body'>";
                string endTemplatePart = @"</div></div>";

                string subContent = string.Empty;

                for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                {

                    if (isGuestNameDisplayFlag)
                    {
                        singleRoomInfoBO = calenderList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                    }
                    if (roomNumberListBO[iRoomNumber].StatusId == 4)
                    {
                        subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                    {
                        subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                    {
                        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                        {
                            if (isGuestNameDisplayFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }

                            RoomLongStayingDiv = RoomLongStayingDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                        {
                            if (isGuestNameDisplayFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }

                            RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                        {
                            if (isGuestNameDisplayFlag)
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }
                            else
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:7px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                                else
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }

                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                        }
                        else
                        {
                            if (isGuestNameDisplayFlag)
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }

                            RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                        }
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                    {
                        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                        {
                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            subContent += Content1 + Content2;
                            RoomVacantDiv = RoomVacantDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                        {
                            GuestInformationDA guestInfoDa = new GuestInformationDA();
                            List<GuestHouseInfoForReportBO> roomIdList = new List<GuestHouseInfoForReportBO>();
                            roomIdList = guestInfoDa.GetTodaysExpectedArrivalRoomId();
                            var roomIdListBO = roomIdList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();

                            string Content1 = "<div class='DivRoomContainerHeight61'>";
                            string Content2 = "";
                            if (roomIdListBO != null)
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "***" + "</br>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }
                            else
                            {
                                Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            }

                            subContent += Content1 + Content2;
                            RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                        {
                            string Content1 = "";
                            if (isGuestNameDisplayFlag)
                            {
                                Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div>";
                            }
                            else
                            {
                                Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div>";
                            }

                            string Content2 = @"<div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            subContent += Content1 + Content2;
                            RoomReservedDiv = RoomReservedDiv + 1;
                        }
                    }
                }

                roomSummary = " (Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfOrderDiv + ", Out of Service: " + RoomOutOfServiceDiv + ")";

                groupNamePart = floorTypeList[iFloorInfo].FloorName + roomSummary;

                RoomVacantDirtyDiv = 0;
                RoomPossibleVacantDiv = 0;
                RoomTodaysCheckInDiv = 0;
                RoomLongStayingDiv = 0;
                RoomOccupaiedDiv = 0;
                RoomReservedDiv = 0;
                RoomVacantDiv = 0;
                RoomOutOfServiceDiv = 0;

                fullContent += topPart + groupNamePart + topTemplatePartEnd + bodyPart + subContent + endTemplatePart;
            }
            ltlRoomTemplate.InnerHtml = fullContent;
            GenerateSearchCriteriaLegendContainer();
        }
        private void GenerateRoomAllocation()
        {
            HMFloorManagementDA roomNumberDA = new HMFloorManagementDA();
            List<HMFloorManagementBO> roomNumberListBO = new List<HMFloorManagementBO>();

            HMFloorBO floorBO = new HMFloorBO();
            HMFloorDA floorDA = new HMFloorDA();
            List<HMFloorBO> floorTypeList = new List<HMFloorBO>();

            int ReservedRoomCount = 0;
            int BookedRoomCount = 0;
            int AvailableRoomCount = 0;

            string fullContent = string.Empty;

            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfOrderDiv = 0;
            int RoomOutOfServiceDiv = 0;

            string roomSummary = string.Empty;

            string groupNamePart = string.Empty;

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();

            isGuestNameDisplayFlag = true;
            RoomCalenderDA calenderDA = new RoomCalenderDA();

            DateTime processDate = DateTime.Now;
            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                processDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            calenderList = calenderDA.GetRoomInfoForCalender(processDate, processDate.AddDays(1));

            if (ddlSrcFloorId.SelectedIndex != -1)
            {
                if (!string.IsNullOrWhiteSpace(ddlFloorBlock.SelectedValue))
                {
                    roomNumberListBO = roomNumberDA.GetHMFloorManagementInfoByFloorNBlockId(Convert.ToInt32(ddlSrcFloorId.SelectedValue), Convert.ToInt32(ddlFloorBlock.SelectedValue));

                    string topPart = @"<div class='block FloorRoomAllocationBGImage'>                                                            
                                <a href='#' class='block-heading' data-toggle='collapse'>";

                    string topTemplatePartEnd = @"</a>
                                <div id='FloorRoomAllocation' class='block-body collapse in'>           
                                ";

                    string endTemplatePart = @"</div>
                            </div>";

                    string subContent = string.Empty;

                    for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                    {
                        if (isGuestNameDisplayFlag)
                        {
                            singleRoomInfoBO = calenderList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                        }
                        if (roomNumberListBO[iRoomNumber].StatusId == 3)
                        {
                            string Content0 = @"<div style='border: none; width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                            string Content1 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "; width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px;'" + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:14px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            subContent += Content0 + Content1;
                            ReservedRoomCount = ReservedRoomCount + 1;
                        }
                        if (roomNumberListBO[iRoomNumber].StatusId == 2)
                        {
                            string Content0 = @"<div class='draggable' style=' border: none; width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                            string Content1 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "; width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px;'" + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:14px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                            subContent += Content0 + Content1;
                            BookedRoomCount = BookedRoomCount + 1;
                        }

                        if (roomNumberListBO[iRoomNumber].StatusId == 1)
                        {
                            string Content0 = @"<div class='draggable' style='border: none; width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                            string Content1 = @"<a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId;
                            string Content2 = @"'></a>
                                        <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "; width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px;'" +
                                        "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:14px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber
                                        + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                            subContent += Content0 + Content1 + Content2;
                            AvailableRoomCount = AvailableRoomCount + 1;
                        }
                    }

                    fullContent += topPart + groupNamePart + topTemplatePartEnd + subContent + endTemplatePart;
                    ltlRoomTemplate_new.Text = fullContent;
                }
            }
        }
        private void GenerateHoldBillRegistrationList()
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> holdBillRegistrationList = new List<RoomRegistrationBO>();

            holdBillRegistrationList = roomRegistrationDA.GetHoldBillRoomRegistrations();
            if (holdBillRegistrationList != null)
            {
                if (holdBillRegistrationList.Count > 0)
                {
                    string fullContent = string.Empty;
                    string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
                    string topTemplatePartEnd = @"</div>";
                    string bodyPart = @"<div class='panel-body'>";
                    string endTemplatePart = @"</div></div>";

                    string groupNamePart = string.Empty;
                    string subContent = string.Empty;

                    for (int i = 0; i < holdBillRegistrationList.Count; i++)
                    {
                        string Content1 = "<div class='DivRoomContainerHeight61' style='width:100px;'>";
                        string Content2 = @"<div class='HoldBillRegistrationDiv' style='background-color:#d63cc4; width:100px;'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + holdBillRegistrationList[i].RegistrationNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + holdBillRegistrationList[i].RegistrationId + "</div></div>";

                        subContent += Content1 + Content2;
                    }

                    fullContent += subContent;
                    ltlRoomTemplate.InnerHtml = topPart + groupNamePart + topTemplatePartEnd + bodyPart + fullContent + endTemplatePart;
                }
            }
        }
        private void GenerateBlankRegistrationList()
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> holdBillRegistrationList = new List<RoomRegistrationBO>();

            holdBillRegistrationList = roomRegistrationDA.GetBlankRegistrationList();
            if (holdBillRegistrationList != null)
            {
                if (holdBillRegistrationList.Count > 0)
                {
                    string subContent = string.Empty;
                    string fullContent = string.Empty;
                    string groupNamePart = string.Empty;

                    string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
                    string topTemplatePartEnd = @"</div>";
                    string bodyPart = @"<div class='panel-body'>";
                    string endTemplatePart = @"</div></div>";

                    for (int i = 0; i < holdBillRegistrationList.Count; i++)
                    {
                        string Content1 = "<div class='DivRoomContainerHeight61' style='width:100px;'>";
                        string Content2 = @"<div class='BlankRegistrationDiv' style='background-color:#d63cc4; width:100px;'><div style='height:30px; margin-top:17px; padding-top:7px; text-align:center; color:#fff; font-weight:bold;'> " + holdBillRegistrationList[i].RegistrationNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + holdBillRegistrationList[i].RegistrationId + "</div></div>";

                        subContent += Content1 + Content2;
                    }

                    fullContent += subContent;
                    ltlRoomTemplate.InnerHtml = topPart + groupNamePart + topTemplatePartEnd + bodyPart + fullContent + endTemplatePart;
                }
            }
        }
        private void GenerateExpectedDepartureRoomInfo()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            //-----linked room info ------------
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomAlocationBO> linkedRoomInfoBOList = new List<RoomAlocationBO>();
            linkedRoomInfoBOList = roomRegistrationDA.GetLinkedRoomInfo();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();

            string fullContent = string.Empty;
            int RoomPossibleVacantDiv = 0;

            string roomSummary = string.Empty;
            string topPart = @"<div id='SearchPanel' class='panel panel-default'> <div class='panel-heading'>";
            string topTemplatePartEnd = @"</div>";
            string bodyPart = @"<div class='panel-body'>";
            string endTemplatePart = @"</div></div>";

            string groupNamePart = string.Empty;
            string subContent = string.Empty;
            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();

            isGuestNameDisplayFlag = true;
            RoomCalenderDA calenderDA = new RoomCalenderDA();

            DateTime processDate = DateTime.Now;
            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                processDate = guestLedgerInfoBO.InhouseGuestLedgerDate;
            }

            calenderList = calenderDA.GetRoomInfoForCalender(processDate, processDate.AddDays(1));

            //room search start
            string srchRoomNumber = txtSrchRoomNumber.Text;
            var validRoom = new RoomNumberBO();

            var searchedRoom = new RoomNumberBO();
            var searchedRoomType = new RoomNumberBO();
            if (srchRoomNumber != "")
            {
                searchedRoomType = roomNumberDA.GetRoomInfoByRoomNumber(srchRoomNumber);
                if ((searchedRoomType.RoomId == 0) || (searchedRoomType.StatusId != 2))
                {
                    CommonHelper.AlertInfo(innboardMessage, "You have entered wrong room number.", AlertType.Warning);
                    return;
                }
                searchedRoom = (from data in roomNumberListBO
                                where data.RoomNumber == srchRoomNumber
                                select data).FirstOrDefault();

                List<RoomAlocationBO> getLinkedRoom = linkedRoomInfoBOList.Where(x => x.RoomId == searchedRoom.RoomId).ToList();
                if (getLinkedRoom.Count > 0)
                {
                    isLinkedRoomFlag = true;
                }
                else
                {
                    isLinkedRoomFlag = false;
                }
                if (searchedRoom.RoomNumber != "")
                {
                    if (isGuestNameDisplayFlag)
                    {
                        singleRoomInfoBO = calenderList.Where(x => x.RoomId == searchedRoom.RoomId).FirstOrDefault();
                    }
                    if (searchedRoom.StatusId == 2)
                    {
                        if (searchedRoom.CSSClassName == "RoomPossibleVacantDiv")
                        {
                            if (searchedRoom.IsBillLockedAndPreview == 1)
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";

                            }
                            else
                            {
                                if (isLinkedRoomFlag)
                                {
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                                }
                                else
                                    subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + searchedRoom.ColorCodeName + "' class='" + searchedRoom.CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + searchedRoom.TypeCode + ":  " + searchedRoom.RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + searchedRoom.RoomNumber + "</div></div>";
                            }
                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;

                        }
                    }
                }
            }
            else
            {
                for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                {
                    if (isGuestNameDisplayFlag)
                    {
                        singleRoomInfoBO = calenderList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
                    }

                    List<RoomAlocationBO> getLinkedRoom1 = linkedRoomInfoBOList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).ToList();

                    if (getLinkedRoom1.Count > 0)
                    {
                        isLinkedRoomFlag = true;
                    }
                    else
                    {
                        isLinkedRoomFlag = false;
                    }

                    if (roomNumberListBO[iRoomNumber].StatusId == 2)
                    {
                        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                        {
                            if (isGuestNameDisplayFlag)// with guest name
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "   " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src = '../Images/linkIcon.png' style = ' height:10; width:10' border = '0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0'/> <br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                }
                                else// not locked
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:1px; text-align:center; color:#fff; font-weight:bold;'> " + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + "<br/>" + singleRoomInfoBO.GuestName + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }
                            else // without guest name
                            {
                                if (roomNumberListBO[iRoomNumber].IsBillLockedAndPreview == 1)
                                {

                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' />" + "   " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:10px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/ReportDocument.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                                }
                                else
                                {
                                    if (isLinkedRoomFlag)
                                    {
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + "<img src='../Images/linkIcon.png' style=' height:10; width:10' border='0' /><br/>" + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                    }
                                    else
                                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].TypeCode + ":  " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                                }
                            }

                            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                        }

                    }
                }
            }

            roomSummary = " (Expected Departure: " + RoomPossibleVacantDiv + ")";
            groupNamePart = "Status" + roomSummary;
            fullContent += subContent;
            ltlRoomTemplate.InnerHtml = topPart + groupNamePart + topTemplatePartEnd + bodyPart + fullContent + endTemplatePart;
            GenerateSearchCriteriaLegendContainer();
            txtSrchRoomNumber.Text = "";
        }
        private void CancelBlankRegistration(string registrationId)
        {
            if (!string.IsNullOrWhiteSpace(registrationId))
            {
                RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                Boolean cancelStatus = roomRegistrationDA.CancelBlankRegistration(Convert.ToInt32(registrationId));
                if (cancelStatus)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Blank Registration Canceled Successfully.", AlertType.Success);

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), Convert.ToInt32(registrationId),
                    ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Blank Registration Cancel");

                    ViewStautsProcess();
                }
            }
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
        public static List<RoomCalenderBO> GetRoomCalenderList(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(StartDate, EndDate);
            return calenderList;
        }
        //************************ User Defined Web Method ********************//
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

                strTable += "<td align='left' style='width: 50%; cursor: pointer' onClick=\"javascript:return PerformViewActionForGuestDetail(" + dr.GuestId + "," + dr.RegistrationId + ")\">" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%; cursor: pointer' onClick=\"javascript:return PerformViewActionForGuestDetail(" + dr.GuestId + "," + dr.RegistrationId + ")\">" + dr.GuestEmail + "</td>";
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
        public static GuestInformationBO GetRegistrationInformationByRoomNumberAndGuestId(string roomNumer, int guestId)
        {
            GuestInformationBO guestBO = new GuestInformationBO();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
            var guestList = list.Where(m => m.GuestId == guestId).ToList();
            return guestList[0];
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
        public static GuestInformationBO PerformViewActionForGuestDetail(int guestId, int registrationId = 0)
        {
            GuestInformationBO guestBO = new GuestInformationBO();
            GuestInformationDA guestDA = new GuestInformationDA();
            guestBO = guestDA.GetGuestInformationByGuestId(guestId, registrationId);

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
            guestBO.RegistrationId = registrationId;
            guestBO.PaxInRate = registrationBO.PaxInRate;
            guestBO.CountryName = registrationBO.CountryName;

            return guestBO;
        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string GuestId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Guest", Int32.Parse(GuestId));
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
        public static RoomNumberBO ShowOutOfServiceRoomInformation(string roomNumber)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = new RoomNumberBO();
            numberBO = numberDA.GetRoomInfoByRoomNumber(roomNumber);
            return numberBO;
        }
        [WebMethod]
        public static string LoadOutOfOrderPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OutOfOrderPossiblePath");

            int row = 1;
            string strTable = string.Empty;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            for (int i = 0; i < list.Count; i++)
            {
                if (row == 1)
                    strTable += "<div class='form-group'>";

                strTable += "<div class='col-md-4'>";
                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return ShowOutOfServiceRoomInformation('" + RoomNember + "' );\"  />";
                    strTable += "</div>";
                }
                else if (list[i].DisplayText == "Room Status Change")
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                    strTable += "</div>";
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
        public static string LoadOutOfServicePossiblePath(string RoomNember, string PageTitle)
        {
            string strTable = string.Empty;
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            if (numberBO != null)
            {
                if (numberBO.RoomId > 0)
                {
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

                    List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
                    list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OutOfServicePossiblePath");

                    int row = 1;
                    strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (row == 1)
                            strTable += "<div class='form-group'>";

                        strTable += "<div class='col-md-4'>";
                        strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                        if (list[i].DisplayText == "Details")
                        {
                            strTable += " onclick=\"return ShowOutOfServiceRoomInformation('" + RoomNember + "' );\"  />";
                            strTable += "</div>";
                        }
                        else if (list[i].DisplayText == "Room Status Change")
                        {
                            strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                            strTable += "</div>";
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
                }
            }

            return strTable;
        }
        [WebMethod(EnableSession = true)]
        public static string LoadOccupiedPossiblePath(string RoomNember, string PageTitle, string linkStatus)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);

            HMCommonDA hmCommonDA = new HMCommonDA();

            int linkSt = Convert.ToInt32(linkStatus);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            //-----linked room info ------------
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomAlocationBO> linkedRoomInfoBOList = new List<RoomAlocationBO>();
            linkedRoomInfoBOList = roomRegistrationDA.GetLinkedRoomInfo();


            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OccupiedPossiblePath");

            var isLinkedlist = linkedRoomInfoBOList.Where(x => x.RoomNumber == numberBO.RoomNumber).ToList();

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            int row = 1, col = 0;

            for (int i = 0; i < list.Count; i++, col++)
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
                else if (list[i].DisplayText == "Bill Split")
                {
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNember);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RId=" + roomAllocationBO.RegistrationId.ToString() + "';\"  />";
                        strTable += "</div>";
                    }
                }
                else if (list[i].DisplayText == "Bill Preview")
                {

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
                        if (roomAllocationBO.ConversionRate > 0)
                        {
                            strTable += " onclick='LoadBillPreviewAction()' />";
                        }
                        else
                        {
                            strTable += " onclick='LoadBillPreview()' />";
                        }
                        
                        strTable += "</div>";
                    }
                }
                else if (list[i].DisplayText == "Room Check Out")
                {
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
                            string strDisplayText = "Guest Bill";
                            strTable = strTable.Replace("<div class='col-md-4'>" + "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'",
                                "<div class='col-md-4'>" + "<input type='button' style='width:150px' value='" + strDisplayText + "' class='TransactionalButton btn btn-primary'"
                                );
                            strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "&cot=rlp';\"  />";
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
            //adding link room and bill preview
            if ((isLinkedlist.Count > 0) && (linkSt != 0))
            {
                string strDisplayText = "Linked Room Bill";
                if (row == 1)
                    strTable += "<div class='form-group'>";
                strTable += "<div class='col-md-4'>";
                strTable += "<label id='roomNumber' style='display:none;'>" + isLinkedlist[0].RoomNumber + "</label>";
                strTable += "<input type='button' style='width:150px' value='" + strDisplayText + "' class='TransactionalButton btn btn-primary'";
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(isLinkedlist[0].RoomNumber);
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
                    strTable += " onclick='LoadLinkedBillPreview()' />";

                }
                strTable += "</div>";

                if (row == 3)
                {
                    strTable += "</div>";
                    row = 0;
                    row++;
                }
                string strText = "Linked Room";
                if (row == 1)
                    strTable += "<div class='form-group'>";
                strTable += "<div class='col-md-4'>";
                strTable += "<label id='roomNumber' style='display:none;'>" + isLinkedlist[0].RoomNumber + "</label>";
                strTable += "<input type='button' style='width:150px' value='" + strText + "' class='TransactionalButton btn btn-primary'";
                strTable += " onclick=\"return LoadLinkedRoom('" + isLinkedlist[0].MasterId + "');\" />";
                strTable += "</div>";

                if (row == 1)
                {
                    strTable += "</div>";
                    row = 0;
                    row++;
                }

            }
            //adding link room menu
            //if ((isLinkedlist.Count > 0) && (linkSt != 0))
            //{
            //    string strDisplayText = "Linked Room";
            //    if (row == 1)
            //        strTable += "<div class='form-group'>";

            //    strTable += "<div class='col-md-4'>";
            //    strTable += "<label id='roomNumber' style='display:none;'>" + isLinkedlist[0].RoomNumber + "</label>";
            //    strTable += "<input type='button' style='width:150px' value='" + strDisplayText + "' class='TransactionalButton btn btn-primary'";
            //    strTable += " onclick=\"return LoadLinkedRoom('" + isLinkedlist[0].MasterId + "');\" />";

            //    strTable += "</div>";
            //    if (row == 1)
            //    {
            //        strTable += "</div>";
            //        row = 0;
            //    }
            //}
            strTable += "</div></div>";
            return strTable;
        }
        [WebMethod]
        public static string LoadReservedPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "ReservedPossiblePath");
            RoomCalenderBO calenderBO = GetRoomCalenderList(DateTime.Now, DateTime.Now.AddDays(1)).Where(x => x.RoomId == numberBO.RoomId && x.TransectionStatus == "Reservation").FirstOrDefault();

            string strTable = string.Empty;
            int row = 1;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            for (int i = 0; i < list.Count; i++)
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
                else if (list[i].DisplayText.Trim() == "Registration")
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?SelectedRoomNumber=" + numberBO.RoomId + "&source=Reservation';\"  />";
                    strTable += "</div>";
                }
                else if (list[i].DisplayText.Trim() == "Reservation Payment")
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "&rId=" + numberBO.RoomId + "';\"  />";
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
        public static string LoadExpectedDeparturePath(string RoomNember, string PageTitle, string linkStatus)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            //-----linked room info ------------
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomAlocationBO> linkedRoomInfoBOList = new List<RoomAlocationBO>();
            linkedRoomInfoBOList = roomRegistrationDA.GetLinkedRoomInfo();
            int linkSt = Convert.ToInt32(linkStatus);

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "ExpectedDeparturePath");

            var isLinkedlist = linkedRoomInfoBOList.Where(x => x.RoomNumber == numberBO.RoomNumber).ToList();

            string strTable = string.Empty;
            int row = 1;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
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
                else if (list[i].DisplayText == "Bill Split")
                {
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNember);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RId=" + roomAllocationBO.RegistrationId.ToString() + "';\"  />";
                        strTable += "</div>";
                    }
                }
                else if (list[i].DisplayText == "Bill Preview")
                {
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
                        if (roomAllocationBO.ConversionRate > 0)
                        {
                            strTable += " onclick='LoadBillPreviewAction()' />";
                        }
                        else
                        {
                            strTable += " onclick='LoadBillPreview()' />";
                        }
                        strTable += "</div>";
                    }
                }                
                else if (list[i].DisplayText == "Room Check Out")
                {
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

                        Boolean isBillLockAndPreviewEnableForCheckOut = true;
                        HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                        commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBillLockAndPreviewEnableForCheckOut", "IsBillLockAndPreviewEnableForCheckOut");
                        if (commonSetupBO != null)
                        {
                            if (commonSetupBO.SetupId > 0)
                            {
                                if (commonSetupBO.SetupValue == "0")
                                {
                                    isBillLockAndPreviewEnableForCheckOut = false;
                                }
                            }
                        }

                        if (isBillLockAndPreviewEnableForCheckOut)
                        {
                            if (roomAllocationBO.IsStopChargePosting)
                            {
                                strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "&cot=co';\"  />";
                            }
                            else
                            {
                                string strDisplayText = "Guest Bill";
                                strTable = strTable.Replace("<div class='col-md-4'>" + "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'",
                                    "<div class='col-md-4'>" + "<input type='button' style='width:150px' value='" + strDisplayText + "' class='TransactionalButton btn btn-primary'"
                                    );
                                strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "&cot=rlp';\"  />";
                            }
                        }
                        else
                        {
                            strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "';\"  />";
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
            //adding link room and bill preview
            if ((isLinkedlist.Count > 0) && (linkSt != 0))
            {
                string strDisplayText = "Linked Room Bill";
                if (row == 1)
                    strTable += "<div class='form-group'>";
                strTable += "<div class='col-md-4'>";
                strTable += "<label id='roomNumber' style='display:none;'>" + isLinkedlist[0].RoomNumber + "</label>";
                strTable += "<input type='button' style='width:150px' value='" + strDisplayText + "' class='TransactionalButton btn btn-primary'";
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(isLinkedlist[0].RoomNumber);
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
                    strTable += " onclick='LoadLinkedBillPreview()' />";

                }
                strTable += "</div>";

                if (row == 3)
                {
                    strTable += "</div>";
                    row = 0;
                    row++;
                }
                string strText = "Linked Room";
                if (row == 1)
                    strTable += "<div class='form-group'>";
                strTable += "<div class='col-md-4'>";
                strTable += "<label id='roomNumber' style='display:none;'>" + isLinkedlist[0].RoomNumber + "</label>";
                strTable += "<input type='button' style='width:150px' value='" + strText + "' class='TransactionalButton btn btn-primary'";
                strTable += " onclick=\"return LoadLinkedRoom('" + isLinkedlist[0].MasterId + "');\" />";
                strTable += "</div>";

                if (row == 1)
                {
                    strTable += "</div>";
                    row = 0;
                    row++;
                }

            }
            ////adding link room menu
            //if ((isLinkedlist.Count > 0) && (linkSt != 0))
            //{
            //    string strDisplayText = "Linked Room";
            //    if (row == 1)
            //        strTable += "<div class='form-group'>";
            //    strTable += "<div class='col-md-4'>";
            //    strTable += "<label id='roomNumber' style='display:none;'>" + isLinkedlist[0].RoomNumber + "</label>";
            //    strTable += "<input type='button' style='width:150px' value='" + strDisplayText + "' class='TransactionalButton btn btn-primary'";
            //    strTable += " onclick=\"return LoadLinkedRoom('" + isLinkedlist[0].MasterId + "');\" />";

            //    strTable += "</div>";
            //    if (row == 1)
            //    {
            //        strTable += "</div>";
            //        row = 0;
            //    }
            //}

            strTable += "</div></div>";
            return strTable;
        }
        [WebMethod]
        public static string LoadVacantDirtyPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "VacantDirtyPossiblePath");

            string strTable = string.Empty;
            int row = 1;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
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
                else
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
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
        public static string LoadVacantPossiblePath(string RoomNember, string PageTitle)
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
                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                if (list[i].DisplayText == "Room Status Change")
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                    strTable += "</div>";
                }
                else if (list[i].DisplayText == "Details")
                {
                    //strTable += " onclick=\"return CountTotalNumberOfGuestByRoomNumber('" + RoomNember + "', 0 );\"  />";
                    strTable += " onclick=\"return ShowRoomFeaturesInfo('" + RoomNember + "' );\"  />";
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
        [WebMethod(EnableSession = true)]
        public static string LoadHoldBillRegistrationPossiblePath(int regId, string PageTitle)
        {
            string strTable = string.Empty;
            int row = 1;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            System.Web.HttpContext.Current.Session["CheckOutRegistrationIdList"] = regId.ToString();
            string[] list = new string[] { "Details", "Bill Preview", "Re-CheckIn", "Check Out" };

            for (int i = 0; i < 4; i++)
            {
                if (row == 1)
                    strTable += "<div class='form-group'>";

                strTable += "<div class='col-md-4'>";
                strTable += "<input type='button' style='width:150px' value='" + list[i] + "' class='TransactionalButton btn btn-primary' ";

                if (list[i] == "Details")
                {
                    strTable += " onclick=\"return CountTotalNumberOfGuestByRoomNumber(0, '" + regId + "' );\"  />";
                    strTable += "</div>";
                }
                else if (list[i] == "Bill Preview")
                {
                    strTable += " onclick=\"return LoadBillPreview();\"  />";
                    strTable += "</div>";
                }
                else if (list[i] == "Re-CheckIn")
                {
                    strTable += " onclick=\" location.href='/HotelManagement/frmHoldReCheckIn.aspx?RegId=" + regId.ToString() + "';\"  />";
                    strTable += "</div>";
                }
                else if (list[i] == "Check Out")
                {
                    strTable += " onclick=\" location.href='/HotelManagement/frmHoldRoomCheckOut.aspx?RegId=" + regId.ToString() + "';\"  />";
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
        [WebMethod(EnableSession = true)]
        public static string LoadBlankRegistrationPossiblePath(int regId, string PageTitle)
        {
            string strTable = string.Empty;
            int row = 1;
            strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            System.Web.HttpContext.Current.Session["CheckOutRegistrationIdList"] = regId.ToString();
            string[] list = new string[] { "Update Registration", "Cancel" };

            for (int i = 0; i < 2; i++)
            {
                if (row == 1)
                    strTable += "<div class='form-group'>";

                strTable += "<div class='col-md-4'>";
                strTable += "<input type='button' style='width:150px' value='" + list[i] + "' class='TransactionalButton btn btn-primary' ";

                if (list[i] == "Update Registration")
                {
                    strTable += " onclick=\" location.href='/HotelManagement/frmRoomRegistrationNew.aspx?BlankRegistration=" + regId.ToString() + "';\"  />";
                    strTable += "</div>";
                }
                else if (list[i] == "Cancel")
                {
                    strTable += " onclick=\" location.href='/HotelManagement/frmRoomStatusInfo.aspx?CancelRegistration=" + regId.ToString() + "';\"  />";
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
        public static RoomNumberBO GetRoomCleanUpInfo(int EditId)
        {
            RoomNumberBO roomNumberList = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(EditId);
            return roomNumberList;
        }
        [WebMethod]
        public static ReturnInfo ChangeRoomStatus(int roomId, string cleanupStatus, string remarks, string fromDate, string toDate, string cleanDate, string cleanTime, string lastCleanDate)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            DateTime clnTime = DateTime.Now;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            RoomNumberBO roomNumberBO = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            roomNumberBO.RoomId = Convert.ToInt32(roomId);
            RoomNumberBO roomNumberList = new RoomNumberBO();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(roomNumberBO.RoomId);
            roomNumberBO.StatusId = roomNumberList.StatusId;
            if (string.IsNullOrEmpty(cleanTime))
            {
                clnTime = Convert.ToDateTime("12:00 PM");
            }
            clnTime = Convert.ToDateTime(cleanTime);

            string[] clntime = Regex.Split(cleanTime, @"\D+");
            int hour = Convert.ToInt32(clntime[0]);
            int min = Convert.ToInt32(clntime[1]);


            if (cleanupStatus == "Cleaned")
            {
                roomNumberBO.CleanupStatus = "Cleaned";

                roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(cleanDate, userInformationBO.ServerDateFormat).AddHours(hour).AddMinutes(min)));
                roomNumberBO.LastCleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(cleanDate, userInformationBO.ServerDateFormat));
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
            }
            else if (cleanupStatus == "Dirty")
            {
                roomNumberBO.CleanupStatus = "Dirty";
                roomNumberBO.CleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(cleanDate, userInformationBO.ServerDateFormat).AddHours(hour).AddMinutes(min));
                if (!string.IsNullOrWhiteSpace(cleanDate))
                {
                    roomNumberBO.LastCleanDate = roomNumberBO.CleanDate;
                }
                else
                {
                    roomNumberBO.LastCleanDate = DateTime.Now.ToString();
                }
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
            }
            else if (cleanupStatus == "Available")
            {
                roomNumberBO.CleanupStatus = "Available";
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.CleanDate = DateTime.Now.ToString();
                roomNumberBO.LastCleanDate = DateTime.Now.ToString();
            }
            else if (cleanupStatus == "OutOfOrder")
            {
                roomNumberBO.CleanupStatus = "OutOfOrder";
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.CleanDate = DateTime.Now.ToString();
                roomNumberBO.LastCleanDate = DateTime.Now.ToString();
            }
            roomNumberBO.Remarks = remarks;
            roomNumberBO.LastModifiedBy = userInformationBO.UserInfoId;

            Boolean status = roomNumberDA.UpdateRoomCleanInfo(roomNumberBO);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
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
        public static string LoadRoomFeaturesInfo(string roomNumber)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();

            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(roomNumber);

            List<RoomFeaturesInfoBO> roomFeaturesInfoList = new List<RoomFeaturesInfoBO>();
            RoomFeaturesInfoDA roomFeaturesInfoDA = new RoomFeaturesInfoDA();

            List<RoomFeaturesBO> roomFtList = new List<RoomFeaturesBO>();
            RoomFeaturesDA roomFeatureDA = new RoomFeaturesDA();
            roomFtList = roomFeatureDA.GetAllActiveRoomFeatures();//get all active features 

            roomFeaturesInfoList = roomFeaturesInfoDA.GetRoomFtInfoByRoomId(numberBO.RoomId);

            var data = (from r in roomFtList
                        join f in roomFeaturesInfoList
                         on r.Id equals f.FeaturesId
                        select r
                        ).ToList(); // filter the features by matching Ids

            string strTable = "";

            strTable += "<ul>";
            foreach (var item in data)
            {
                strTable += "<li>" + item.Features + "</li>";
            }
            strTable += "</ul>";

            if (data.Count == 0)
            {
                strTable = "No Features Available !";
            }

            return strTable;
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