using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using System.Collections;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomShift : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.LoadRoomType();
                this.LoadRoomNumber(Convert.ToInt32(this.ddlRoomType.SelectedValue));
                this.LoadCommonDropDownHiddenField();
                this.ddlRooms.SelectedValue = "0";
                this.ddlRegistrationId.Visible = false;

                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcRoomNumber.Text = numberBO.RoomNumber;
                    this.SearchRoomInformation(numberBO.RoomNumber);
                }
                IsMinimumRoomRateCheckingEnable();
            }
        }
        protected void ddlRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadCurrentRoomNumberInfo();
        }
        protected void btnShift_Click(object sender, EventArgs e)
        {
            if (!IsRoomBillSettlmentPending())
            {
                return;
            }
            if (ddlRoomType.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select a valid Room Type.", AlertType.Warning);
                this.ddlRoomType.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSrcRoomNumber.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                return;
            }

            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(this.ddlRoomId.SelectedValue));
            if (numberBO.StatusId != 1)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide Avilable Room Number.", AlertType.Warning);
                this.ddlRoomId.Focus();
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrWhiteSpace(HiddenFieldGuestID.Value))
            {
                int registrationId = Int32.Parse(HiddenFieldGuestID.Value);

                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                roomRegistrationDA.RoomNightAuditProcess(registrationId.ToString(), DateTime.Now, 0, userInformationBO.UserInfoId);

                int shiftingRoomId = Int32.Parse(this.ddlRoomIdHiddenField.Value);
                int shiftingRoomType = Int32.Parse(this.ddlRoomType.SelectedValue);
                string discountType = this.ddlDiscountType.SelectedValue.ToString();
                decimal unitPrice = !string.IsNullOrWhiteSpace(this.txtUnitPriceHiddenField.Value) ? Convert.ToDecimal(this.txtUnitPriceHiddenField.Value) : 0;
                decimal roomRate = unitPrice;
                decimal discountAmount = !string.IsNullOrWhiteSpace(this.txtDiscountAmount.Text) ? Convert.ToDecimal(this.txtDiscountAmount.Text) : 0;
                if(discountAmount > 0)
                {
                    roomRate = !string.IsNullOrWhiteSpace(this.txtRoomRate.Text) ? Convert.ToDecimal(this.txtRoomRate.Text) : 0;
                }
                                
                string Remarks = this.txtRemarks.Text;
                Boolean IsCompanyGuest = Convert.ToBoolean(this.ddlIsCompanyGuest.SelectedIndex);
                Boolean IsHouseUseRoom = Convert.ToBoolean(this.ddlIsHouseUseRoom.SelectedIndex);
                
                int LastModifiedBy = userInformationBO.UserInfoId;
                int RegId = Int32.Parse(HiddenFieldGuestID.Value);
                Boolean status = roomRegistrationDA.UpdateCurrentRoom(registrationId, shiftingRoomId, shiftingRoomType, discountType, unitPrice, discountAmount, roomRate, Remarks, IsCompanyGuest, IsHouseUseRoom, LastModifiedBy);
                if (status)
                {
                    this.clear();
                    CommonHelper.AlertInfo(innboardMessage, "Room Change Operation Successfull.", AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomShift.ToString(), RegId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomShift));
                    this.LoadRoomNumber(Convert.ToInt32(this.ddlRoomType.SelectedValue));
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
            }
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            SearchRoomInformation(this.txtSrcRoomNumber.Text);
        }
        private void SearchRoomInformation(string RoomNumber)
        {
            if (!string.IsNullOrWhiteSpace(RoomNumber))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNumber);
                if (roomAllocationBO.RoomId > 0)
                {
                    if(roomAllocationBO.IsStopChargePosting)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Stop Charge Posting for this Room.", AlertType.Warning);
                        return;
                    }
                    this.ddlRooms.SelectedValue = roomAllocationBO.RoomId.ToString();
                    this.txtGuestNameInfo.Text = roomAllocationBO.GuestName;
                    this.txtRoomTypeInfo.Text = roomAllocationBO.RoomType;
                    this.HiddenFieldGuestID.Value = roomAllocationBO.RegistrationId.ToString();
                    this.ddlRegistrationId.Visible = true;
                    this.ddlCurrencyHiddenField.Value = roomAllocationBO.CurrencyType.ToString();
                    this.txtCheckInDateHiddenField.Value = hmUtility.GetStringFromDateTime(roomAllocationBO.ArriveDate);
                    this.txtDepartureDateHiddenField.Value = hmUtility.GetStringFromDateTime(roomAllocationBO.ExpectedCheckOutDate);
                    this.ddlIsCompanyGuestDisplay.SelectedIndex = roomAllocationBO.IsCompanyGuest == true ? 1 : 0;
                    this.ddlIsHouseUseRoomDisplay.SelectedIndex = roomAllocationBO.IsHouseUseRoom == true ? 1 : 0;
                    this.ddlIsCompanyGuest.SelectedIndex = roomAllocationBO.IsCompanyGuest == true ? 1 : 0;
                    this.ddlIsHouseUseRoom.SelectedIndex = roomAllocationBO.IsHouseUseRoom == true ? 1 : 0;
                    this.ddlDiscountType.SelectedValue = roomAllocationBO.DiscountType;
                    this.txtDiscountAmount.Text = roomAllocationBO.DiscountAmount.ToString();
                    this.HiddenFieldRoomType.Value = roomAllocationBO.RoomTypeId.ToString();
                    this.HiddenFieldDiscountType.Value = roomAllocationBO.DiscountType;
                    this.HiddenFieldDiscountAmount.Value = roomAllocationBO.DiscountAmount.ToString();
                    this.HiddenFieldUnitPrice.Value = roomAllocationBO.UnitPrice.ToString();
                    this.HiddenFieldRoomRate.Value = roomAllocationBO.RoomRate.ToString();
                    txtMinimumUnitPriceHiddenField.Value = roomAllocationBO.MinimumRoomRate.ToString();
                    this.ddlCompanyNameHiddenField.Value = roomAllocationBO.CompanyId.ToString();
                    this.ddlBusinessPromotionIdHiddenField.Value = roomAllocationBO.BusinessPromotionId.ToString();
                    this.LoadRegistrationNumber(roomAllocationBO.RoomId);
                    if (roomAllocationBO.IsCompanyGuest)
                    { ComplimentaryGuestDisplayDiv.Visible = true; }
                    else
                    { ComplimentaryGuestDisplayDiv.Visible = false; }
                }
                else
                {
                    DateTime dateTime = DateTime.Now;
                    this.txtGuestNameInfo.Text = string.Empty;
                    this.txtRoomTypeInfo.Text = string.Empty;
                    this.HiddenFieldGuestID.Value = string.Empty;
                    this.ddlRegistrationId.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                    this.ddlCurrencyHiddenField.Value = "1";
                    this.txtCheckInDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.txtDepartureDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.ddlDiscountType.SelectedIndex = 0;
                    this.txtDiscountAmount.Text = "0";
                    this.ddlCompanyNameHiddenField.Value = "0";
                    this.ddlBusinessPromotionIdHiddenField.Value = "0";
                    this.ddlIsCompanyGuest.SelectedIndex = 0;
                    this.ddlIsHouseUseRoom.SelectedIndex = 0;
                    this.txtSrcRoomNumber.Focus();
                }
            }
            else
            {
                this.txtGuestNameInfo.Text = string.Empty;
                this.txtRoomTypeInfo.Text = string.Empty;
                this.HiddenFieldGuestID.Value = string.Empty;
                this.ddlRegistrationId.Visible = false;
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            btnShift.Visible = isSavePermission;
        }
        private bool IsRoomBillSettlmentPending()
        {
            bool flag = true;

            if (!string.IsNullOrWhiteSpace(HiddenFieldGuestID.Value))
            {
                int registrationId = Int32.Parse(HiddenFieldGuestID.Value);
                List<RestaurantBillBO> restaurantBillBOList = new List<RestaurantBillBO>();
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                restaurantBillBOList = restaurentBillDA.GetRoomBillSettlmentPending("RoomNumber", registrationId, this.txtSrcRoomNumber.Text);
                restaurantBillBOList = restaurantBillBOList.GroupBy(t => t.CostCenterId).Select(grp => grp.First()).Where(x => x.CostCenterId != 0).ToList();

                if (restaurantBillBOList != null)
                {
                    if (restaurantBillBOList.Count > 0)
                    {
                        string pendingCostCenterName = string.Empty;

                        foreach (RestaurantBillBO row in restaurantBillBOList)
                        {
                            if (!string.IsNullOrEmpty(pendingCostCenterName))
                                pendingCostCenterName += ", " + row.CostCenter;
                            else pendingCostCenterName += row.CostCenter;
                        }

                        CommonHelper.AlertInfo(innboardMessage, "Bill Settlement Pending on (" + pendingCostCenterName + ")", AlertType.Warning);
                        this.txtUnitPrice.Enabled = true;
                        this.txtUnitPrice.Text = this.txtUnitPriceHiddenField.Value;
                        this.txtUnitPrice.Enabled = false;
                        this.txtSrcRoomNumber.Focus();
                        flag = false;
                    }
                }
            }
            return flag;
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadRegistrationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            this.ddlRegistrationId.DataSource = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            this.ddlRegistrationId.DataTextField = "RegistrationNumber";
            this.ddlRegistrationId.DataValueField = "RegistrationId";
            this.ddlRegistrationId.DataBind();
        }
        private void LoadCurrentRoomNumberInfo()
        {
            if (ddlRooms.SelectedIndex != -1)
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                string roomName = ddlRooms.SelectedItem.Text;
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetRoomAlocationInfo(roomName);
                txtGuestNameInfo.Text = roomAllocationBO.GuestName;
                txtRoomTypeInfo.Text = roomAllocationBO.RoomType;
                HiddenFieldGuestID.Value = roomAllocationBO.RegistrationId.ToString();
            }
            else
            {
                txtGuestNameInfo.Text = string.Empty;
                txtRoomTypeInfo.Text = string.Empty;
                HiddenFieldGuestID.Value = string.Empty;
                this.ddlRegistrationId.SelectedIndex = -1;
            }
        }
        private void LoadRoomNumber(int roomTypeId)
        {
            int isReservation = 0;
            
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (ddlRoomType.SelectedValue == "0")
            {
                RoomNumberDA roomNumberDA = new RoomNumberDA();
                this.ddlRoomId.DataSource = roomNumberDA.GetRoomNumberInfo();
                this.ddlRoomId.DataTextField = "RoomNumber";
                this.ddlRoomId.DataValueField = "RoomId";
                this.ddlRoomId.DataBind();
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                DateTime StartDate = dateTime;
                DateTime EndDate = dateTime;
                if (!string.IsNullOrWhiteSpace(this.txtCheckInDateHiddenField.Value))
                {
                    StartDate = hmUtility.GetDateTimeFromString(this.txtCheckInDateHiddenField.Value, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }

                if (!string.IsNullOrWhiteSpace(this.txtDepartureDateHiddenField.Value))
                {
                    EndDate = hmUtility.GetDateTimeFromString(this.txtDepartureDateHiddenField.Value, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }

                RoomNumberDA roomNumberDA = new RoomNumberDA();
                this.ddlRoomId.DataSource = roomNumberDA.GetAvailableRoomNumberInfoForRegistrationForm(roomTypeId, isReservation, StartDate, EndDate);
                this.ddlRoomId.DataTextField = "RoomNumber";
                this.ddlRoomId.DataValueField = "RoomId";
                this.ddlRoomId.DataBind();
            }
            this.ddlRoomId.Items.Insert(0, item);
        }
        private void LoadRoomType()
        {
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> entityBO = new List<RoomTypeBO>();
            entityBO = roomTypeDA.GetRoomTypeInfo();
            this.ddlRoomType.DataSource = entityBO;
            this.ddlRoomType.DataTextField = "RoomType";
            this.ddlRoomType.DataValueField = "RoomTypeId";
            this.ddlRoomType.DataBind();

            ddlRoomType.Items.Insert(0, new ListItem { Value = "0", Text = hmUtility.GetDropDownFirstValue() });
        }
        private void clear()
        {
            this.ddlRegistrationId.Visible = false;
            this.txtSrcRoomNumber.Text = string.Empty;
            this.txtGuestNameInfo.Text = string.Empty;
            this.txtRoomTypeInfo.Text = string.Empty;
            this.ddlRoomType.SelectedIndex = 0;
            this.ddlRoomId.SelectedIndex = 0;
            this.ddlIsCompanyGuestDisplay.SelectedValue = "No";
            this.ddlIsHouseUseRoomDisplay.SelectedValue = "No";
            this.ddlIsCompanyGuest.SelectedValue = "No";
            this.ddlIsHouseUseRoom.SelectedValue = "No";
            this.ddlDiscountType.SelectedIndex = 0;
            this.txtDiscountAmount.Text = string.Empty;
            this.txtUnitPrice.Text = string.Empty;
            this.txtRoomRate.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
        }
        private void IsMinimumRoomRateCheckingEnable()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsShowMinimumRoomRate = new HMCommonSetupBO();
            IsShowMinimumRoomRate = commonSetupDA.GetCommonConfigurationInfo("IsMinimumRoomRateCheckingForRoomTypeEnable", "IsMinimumRoomRateCheckingForRoomTypeEnable");
            if (!string.IsNullOrWhiteSpace(IsShowMinimumRoomRate.SetupValue))
                hfIsMinimumRoomRateCheckingEnable.Value = IsShowMinimumRoomRate.SetupValue;
            ddlIsCompanyGuest.Items[1].Enabled = IsShowMinimumRoomRate.SetupValue == "0";
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static RoomTypeBO PerformFillFormActionByTypeId(int EditId)
        {
            RoomTypeBO roomTypeBO = new RoomTypeBO();
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            roomTypeBO = roomTypeDA.GetRoomTypeInfoById(EditId);
            return roomTypeBO;
        }
        [WebMethod]
        public static GuestCompanyBO GetCalculatedDiscount(int companyId, int promId)
        {
            GuestCompanyBO companyBO = new GuestCompanyBO();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            BusinessPromotionBO promBO = new BusinessPromotionBO();
            BusinessPromotionDA promDA = new BusinessPromotionDA();
            companyBO = companyDA.GetGuestCompanyInfoById(companyId);
            promBO = promDA.GetBusinessPromotionInfoById(promId);

            if (promBO.PercentAmount > companyBO.DiscountPercent)
            {
                companyBO.DiscountPercent = promBO.PercentAmount;
            }

            return companyBO;
        }
        [WebMethod]
        public static ArrayList PopulateRooms(int RoomTypeId, int ResevationId, string FromDate, string ToDate)
        {
            HMUtility hmUtility = new HMUtility();
            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;
            DateTime EndDate = dateTime.AddDays(1);
            ArrayList list = new ArrayList();
            List<RoomNumberBO> roomList = new List<RoomNumberBO>();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            if (ResevationId > 0)
            {
                roomList = roomNumberDA.GetAvailableRoomNumberInfoForRegistrationForm(RoomTypeId, ResevationId, StartDate, EndDate);
            }
            else
            {
                roomList = roomNumberDA.GetAvailableRoomNumberInfoForRegistrationForm(RoomTypeId, 0, StartDate, EndDate);
            }
            int count = roomList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        roomList[i].RoomNumber.ToString(),
                                        roomList[i].RoomId.ToString()
                                         ));
            }
            return list;
        }
    }
}