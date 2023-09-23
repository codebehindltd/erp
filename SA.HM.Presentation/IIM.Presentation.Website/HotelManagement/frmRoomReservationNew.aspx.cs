using HotelManagement.Data;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.SiteMinder;
using HotelManagement.Entity.SiteMinder.Response;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Presentation.Website.POS;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomReservationNew : BasePage
    {
        HiddenField innboardMessage;
        protected int _RoomReservationId;
        ArrayList arrayDelete;
        protected int isNewAddButtonEnable = 1;
        protected int isListedCompanyVisible = -1;
        protected int isListedCompanyDropDownVisible = -1;
        protected string SearchOrdering = string.Empty;
        HMUtility hmUtility = new HMUtility();
        protected string GuestInformation;
        private static int ReservationId;
        private Boolean isConversionRateEditable = false;
        private static int outReservationId;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);

            _RoomReservationId = -1;
            hfIsSameasContactPersonAsGuest.Value = "0";
            hfIsPaidServiceAlreadyLoded.Value = "0";
            hfIsCopyTo.Value = "false";
            AddEditODeleteDetail();
            CheckMandatoryField();
            LoadInhouseGuestLedgerDateInfo();

            if (!IsPostBack)
            {
                IsReservationStatusWaitingEnable();
                CheckObjectPermission();
                LoadGroupReservationInfo();
                LoadGuestCityCharge();
                LoadVIPGuestType();
                LoadMealPlan();
                pnlIsSameasGuest.Visible = true;
                pnlReservationEditTimePanelwillHide.Visible = true;
                HttpContext.Current.Session["ReservationDetailListForGrid"] = null;
                Random rd = new Random();

                ReservationId = rd.Next(100000, 999999);
                hfReservationIdTemp.Value = ReservationId.ToString();
                Session["_RoomReservationId"] = null;
                Session["ReservationDetailList"] = null;
                Session["arrayDelete"] = null;
                LoadRoomType();
                LoadMarketSegment();
                LoadGuestSource();
                LoadProfession();
                LoadGuestClassification();
                LoadAffiliatedCompany();
                LoadBusinessPromotion();
                LoadGuestReference();
                LoadRRPaymentMode();
                LoadCurrency();
                LoadComplementaryItem();
                LoadIsConversionRateEditable();
                LoadRoomRateInfo();
                LoadCountryList();
                SetDefaulTime();
                LoadCommonDropDownHiddenField();
                LoadAirlineInfo();
                LoadIsRoomOverbookingEnable();
                LoadGuestTitle();
                LoadIsReservationRoomListButtonWillHide();

                string TransectionId = Request.QueryString["TransectionId"];
                if (!string.IsNullOrEmpty(TransectionId))
                {
                    FillForm(Int32.Parse(TransectionId));
                }

                string editId = Request.QueryString["editId"];
                int Id = Convert.ToInt32(editId);

                if (Id > 0)
                {
                    hfReservationId.Value = editId.Trim();
                    FillForm(Id);
                }
                else
                {
                    hfReservationId.Value = "";
                    SetDefaultComplementaryItem();
                }

                CostCenterWiseSetting();
                IsMinimumRoomRateCheckingEnable();

                string sRoomId = Request.QueryString["SRN"];
                if (!string.IsNullOrEmpty(sRoomId))
                {
                    this.LoadCurrentDate(Convert.ToInt32(sRoomId));
                }
            }
        }
        private void LoadIsReservationRoomListButtonWillHide()
        {
            hfIsReservationRoomListButtonWillHide.Value = "0";
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsReservationRoomListButtonWillHide", "IsReservationRoomListButtonWillHide");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        hfIsReservationRoomListButtonWillHide.Value = "1";
                    }
                }
            }
        }
        private void LoadCurrentDate(Int32 roomId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime dateTime = DateTime.Now;
            this.txtDateIn.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtDateOut.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(1));

            string sDayCount = Request.QueryString["DC"];
            if (!string.IsNullOrEmpty(sDayCount))
            {
                int dayCount = Convert.ToInt32(sDayCount);
                this.txtDateIn.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(dayCount));
                this.txtDateOut.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(dayCount).AddDays(1));
            }
        }
        private void LoadInhouseGuestLedgerDateInfo()
        {
            DateTime dateTime = DateTime.Now;
            DateTime processDate = DateTime.Now;
            RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
            InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
            guestLedgerInfoBO = guestLedgerInfoDA.GetInhouseGuestLedgerDateInfo();

            if (guestLedgerInfoBO != null)
            {
                processDate = guestLedgerInfoBO.InhouseGuestLedgerDate;

                //if (dateTime.Date > processDate.Date)
                //{
                //    dateTime = processDate;
                //    this.txtDateIn.Text = hmUtility.GetStringFromDateTime(dateTime);
                //    this.txtDateOut.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(1));
                //}
            }

            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(dateTime.Date);
        }
        protected void CheckMandatoryField()
        {
            FormWiseFieldSetupDA formWiseFieldSetupDA = new FormWiseFieldSetupDA();
            var formWiseFields = formWiseFieldSetupDA.GetFieldsByMenuLinkID(9).Where(a => a.IsMandatory == true);
            hfMandatoryFields.Value = JsonConvert.SerializeObject(formWiseFields);
        }
        protected void btnAddDetailGuest_Click(object sender, EventArgs e)
        {
            int RoomCount = 1;
            string RoomNumber = hfSelectedRoomNumbers.Value;
            string RoomId = hfSelectedRoomId.Value;
            int RoomQty = !string.IsNullOrWhiteSpace(txtRoomId.Text) ? Convert.ToInt32(txtRoomId.Text) : 0;
            if (RoomId.Split(',').Length >= RoomQty)
            {
                RoomCount = RoomId.Split(',').Length;
            }
            else
            {
                RoomCount = RoomQty;
            }
            string RoomTypeInfo = string.Empty;

            decimal RoomUnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField.Value) ? Convert.ToDecimal(txtUnitPriceHiddenField.Value) : 0;
            decimal RoomRateAmount = !string.IsNullOrWhiteSpace(txtRoomRate.Text) ? Convert.ToDecimal(txtRoomRate.Text) : RoomUnitPrice;

            List<ReservationDetailBO> reservationDetailListBOForGrid = Session["ReservationDetailListForGrid"] == null ? new List<ReservationDetailBO>() : Session["ReservationDetailListForGrid"] as List<ReservationDetailBO>;
            ReservationDetailBO singleEntityBO = new ReservationDetailBO();
            singleEntityBO.RoomTypeId = Convert.ToInt32(ddlRoomTypeId.SelectedValue);
            singleEntityBO.RoomType = ddlRoomTypeId.SelectedItem.Text;
            singleEntityBO.RoomRate = RoomRateAmount;
            singleEntityBO.RoomQuantity = RoomCount;
            singleEntityBO.TotalCalculatedAmount = RoomRateAmount * RoomCount;
            singleEntityBO.RoomNumberIdList = RoomId;
            singleEntityBO.RoomNumberList = !string.IsNullOrWhiteSpace(RoomNumber) ? RoomNumber : "Unassigned";
            singleEntityBO.RoomNumberListInfoWithCount = RoomCount + "(" + singleEntityBO.RoomNumberList + ")";
            reservationDetailListBOForGrid.Add(singleEntityBO);

            Session["ReservationDetailListForGrid"] = reservationDetailListBOForGrid;

            string[] Rooms = RoomNumber.Split(',');
            string[] Id = RoomId.Split(',');

            if (!ValidateDetails())
            {
                if (ddlReservedMode.SelectedItem.Text == "Company")
                {
                    isListedCompanyVisible = 1;
                    if (chkIsLitedCompany.Checked)
                    {
                        isListedCompanyDropDownVisible = 1;
                    }
                }
                return;
            }

            int dynamicDetailId = 0;
            List<ReservationDetailBO> reservationDetailListBO = Session["ReservationDetailList"] == null ? new List<ReservationDetailBO>() : Session["ReservationDetailList"] as List<ReservationDetailBO>;

            if (!string.IsNullOrWhiteSpace(lblHiddenId.Text))
                dynamicDetailId = Convert.ToInt32(lblHiddenId.Text);

            if (Rooms[0].Length == 0)
            {
                int roomQuantity;
                if (!Int32.TryParse(txtRoomId.Text, out roomQuantity))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Room Quantity is not in correct format.", AlertType.Warning);
                    txtRoomId.Focus();
                    return;
                }

                dynamicDetailId = Convert.ToInt32(ddlRoomTypeId.SelectedValue);
                ReservationDetailBO detailBO = dynamicDetailId == 0 ? new ReservationDetailBO() : reservationDetailListBO.Where(x => x.RoomTypeId == dynamicDetailId).FirstOrDefault();
                if (reservationDetailListBO.Contains(detailBO))
                    reservationDetailListBO.Remove(detailBO);

                for (int i = 0; i < roomQuantity; i++)
                {
                    detailBO.RoomTypeId = Convert.ToInt32(ddlRoomTypeId.SelectedValue);
                    detailBO.RoomType = ddlRoomTypeId.SelectedItem.Text;
                    detailBO.RoomId = 0;
                    detailBO.UnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField.Value) ? Convert.ToDecimal(txtUnitPriceHiddenField.Value) : 0;
                    detailBO.DiscountAmount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 0;
                    detailBO.RoomRate = !string.IsNullOrWhiteSpace(txtRoomRate.Text) ? Convert.ToDecimal(txtRoomRate.Text) : detailBO.UnitPrice;
                    detailBO.Discount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 0;
                    detailBO.CurrencyType = Convert.ToInt32(ddlCurrency.SelectedValue);
                    detailBO.DiscountType = ddlDiscountType.SelectedValue.ToString();
                    detailBO.RoomNumber = "Unassigned";
                    detailBO.ReservationDetailId = dynamicDetailId == 0 ? reservationDetailListBO.Count + 1 : dynamicDetailId;
                    reservationDetailListBO.Add(detailBO);
                }
            }
            else
            {
                for (int i = 0; i < Id.Length; i++)
                {
                    ReservationDetailBO detailBO = dynamicDetailId == 0 ? new ReservationDetailBO() : reservationDetailListBO.Where(x => x.ReservationDetailId == dynamicDetailId).FirstOrDefault();
                    if (reservationDetailListBO.Contains(detailBO))
                        reservationDetailListBO.Remove(detailBO);
                    detailBO.RoomTypeId = Convert.ToInt32(ddlRoomTypeId.SelectedValue);
                    detailBO.RoomType = ddlRoomTypeId.SelectedItem.Text;
                    detailBO.RoomId = Int32.Parse(Id[i]);
                    detailBO.RoomNumber = Rooms[i];
                    detailBO.UnitPrice = Convert.ToDecimal(txtUnitPriceHiddenField.Value);
                    detailBO.DiscountAmount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 0;
                    detailBO.RoomRate = !string.IsNullOrWhiteSpace(txtRoomRate.Text) ? Convert.ToDecimal(txtRoomRate.Text) : detailBO.UnitPrice;
                    detailBO.Discount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 0;
                    detailBO.CurrencyType = Convert.ToInt32(ddlCurrency.SelectedValue);
                    detailBO.DiscountType = ddlDiscountType.SelectedValue.ToString();
                    detailBO.ReservationDetailId = dynamicDetailId == 0 ? reservationDetailListBO.Count + 1 : dynamicDetailId;
                    reservationDetailListBO.Add(detailBO);
                }
            }
            CheckObjectPermission();
            Session["ReservationDetailList"] = reservationDetailListBO;
            ClearDetailPart();
            SetTab("EntryTab");
            if (ddlReservedMode.SelectedItem.Text == "Company")
            {
                isListedCompanyVisible = 1;
                if (chkIsLitedCompany.Checked)
                {
                    isListedCompanyDropDownVisible = 1;
                }
            }
        }
        protected void gvRegistrationDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _RoomTypeId;

            if (e.CommandName == "CmdEdit")
            {
                _RoomTypeId = Convert.ToInt32(e.CommandArgument.ToString());
                lblHiddenId.Text = _RoomTypeId.ToString();
                var reservationDetailBO = (List<ReservationDetailBO>)Session["ReservationDetailListForGrid"];
                var reservationDetail = reservationDetailBO.Where(x => x.RoomTypeId == _RoomTypeId).FirstOrDefault();
                if (reservationDetail != null && reservationDetail.RoomTypeId > 0)
                {
                    lblAddedRoomNumber.Text = reservationDetail.RoomNumberList;
                    hfSelectedRoomId.Value = reservationDetail.RoomNumberIdList;
                    ddlRoomTypeId.SelectedValue = reservationDetail.RoomTypeId.ToString();
                    txtRoomRate.Text = reservationDetail.RoomRate.ToString();
                    txtDiscountAmount.Text = reservationDetail.DiscountAmount.ToString();
                    txtUnitPrice.Text = reservationDetail.UnitPrice.ToString();
                    ddlDiscountType.SelectedValue = reservationDetail.DiscountType.ToString();
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _RoomTypeId = Convert.ToInt32(e.CommandArgument.ToString());
                var reservationDetailBO = (List<ReservationDetailBO>)Session["ReservationDetailListForGrid"];
                var reservationDetail = reservationDetailBO.Where(x => x.RoomTypeId == _RoomTypeId).FirstOrDefault();
                reservationDetailBO.Remove(reservationDetail);
                Session["ReservationDetailListForGrid"] = reservationDetailBO;
                arrayDelete.Add(_RoomTypeId);

            }
        }
        private bool IsRegistered(int ReservationId)
        {
            RoomRegistrationBO regBO = new RoomRegistrationBO();
            RoomRegistrationDA regDA = new RoomRegistrationDA();
            regBO = regDA.GetRoomRegistrationInfoByReservationId(ReservationId);
            if (!string.IsNullOrEmpty(regBO.RegistrationNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void SetRegisterdStatus(bool registered)
        {
            ddlReservationStatus.Items.Remove(ddlReservationStatus.Items.FindByValue("Registered"));
            if (registered == true)
            {
                ListItem registeredItem = new ListItem("Registered", "Registered", true);
                ddlReservationStatus.Items.Add(registeredItem);
            }
        }
        protected void gvRoomRegistration_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                _RoomReservationId = Convert.ToInt32(e.CommandArgument.ToString());
                Session["_RoomReservationId"] = _RoomReservationId;
                SetTab("EntryTab");

                FillForm(_RoomReservationId);
                hfIsPaidServiceAlreadyLoded.Value = "0";
                bool registered = IsRegistered(_RoomReservationId);
            }
            else if (e.CommandName == "CmdCopy")
            {
                _RoomReservationId = Convert.ToInt32(e.CommandArgument.ToString());
                Session["_RoomReservationId"] = _RoomReservationId;
                SetTab("EntryTab");
                CancelCopy();

                CopyToFillForm(_RoomReservationId);
                hfIsPaidServiceAlreadyLoded.Value = "0";
                bool registered = IsRegistered(_RoomReservationId);
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    _RoomReservationId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_RoomReservationId"] = _RoomReservationId;
                    DeleteData(_RoomReservationId);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                    EntityTypeEnum.EntityType.RoomReservation.ToString(), _RoomReservationId,
                    ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));
                    Cancel();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdBillPreview")
            {
                bool isPreview = true;
                _RoomReservationId = -1;
                _RoomReservationId = Convert.ToInt32(e.CommandArgument.ToString());
                string url = "/HotelManagement/Reports/frmReportReservationBillInfo.aspx?GuestBillInfo=" + _RoomReservationId + "&isPreview=" + isPreview;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(GetType(), "script", sPopUp, true);
                SetTab("SearchTab");
            }
            else if (e.CommandName == "CmdPreRegistrationCard")
            {
                _RoomReservationId = -1;
                _RoomReservationId = Convert.ToInt32(e.CommandArgument.ToString());
                string url = "/HotelManagement/Reports/frmReportPreRegistrationCard.aspx?ReservationId=" + _RoomReservationId;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(GetType(), "script", sPopUp, true);
                SetTab("SearchTab");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnNewReservation_Click(object sender, EventArgs e)
        {
            Cancel();
            isNewAddButtonEnable = -1;
            _RoomReservationId = 1;
        }
        protected void gvRoomRegistration_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCopy = (ImageButton)e.Row.FindControl("ImgCopy");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgChangeStatus = (ImageButton)e.Row.FindControl("imgChangeStatus");
                ImageButton imageActive = (ImageButton)e.Row.FindControl("imageActive");
                ImageButton imgBillPreview = (ImageButton)e.Row.FindControl("ImgBillPreview");
                ImageButton imgPreRegistrationCard = (ImageButton)e.Row.FindControl("ImgPreRegistrationCard");
                ImageButton imgBillPreviewForMultiplePickUpDrop = (ImageButton)e.Row.FindControl("ImgBillPreviewForMultiplePickUpDrop");
                ImageButton imgRoomCurrentStatus = (ImageButton)e.Row.FindControl("ImgRoomCurrentStatus");
                Label lblReservationStatusInfo = (Label)e.Row.FindControl("lblReservationStatusInfo");
                Label lblPickUpDropStatusInfo = (Label)e.Row.FindControl("lblPickUpDropStatusInfo");

                Label lblGroupMasterId = (Label)e.Row.FindControl("lblGroupMasterId");
                ImageButton imgGroupReservationLetter = (ImageButton)e.Row.FindControl("ImgGroupReservationLetter");
                imgGroupReservationLetter.Visible = true;
                if (lblGroupMasterId.Text == "0")
                {
                    imgGroupReservationLetter.Visible = false;
                }

                imgUpdate.Visible = isUpdatePermission;
                imgChangeStatus.Visible = isDeletePermission;
                imgCopy.Visible = isSavePermission;
                imgDelete.Visible = false;
                imageActive.Visible = false;
                imgRoomCurrentStatus.Visible = true;

                if (lblReservationStatusInfo.Text == "Registered" || lblReservationStatusInfo.Text == "NoShow")
                {
                    imgUpdate.Visible = false;
                    imgChangeStatus.Visible = false;
                    imgPreRegistrationCard.Visible = false;
                    imgRoomCurrentStatus.Visible = false;
                }
                if (lblReservationStatusInfo.Text == "Cancel")
                {
                    //imageActive.Visible = true;
                    Boolean IsAdminUser = false;
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

                    // // // ------User Admin Authorization BO Session Information --------------------------------
                    #region User Admin Authorization
                    if (userInformationBO.UserInfoId == 1)
                    {
                        IsAdminUser = true;
                    }
                    else
                    {
                        List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                        adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                        if (adminAuthorizationList != null)
                        {
                            if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 4).Count() > 0)
                            {
                                IsAdminUser = true;
                            }
                        }
                    }
                    #endregion

                    imageActive.Visible = IsAdminUser;
                    imgChangeStatus.Visible = false;
                    imgUpdate.Visible = false;
                    imgPreRegistrationCard.Visible = false;
                    imgRoomCurrentStatus.Visible = false;
                }

                int pickUpDropStatusInfo = !string.IsNullOrWhiteSpace(lblPickUpDropStatusInfo.Text) ? Convert.ToInt32(lblPickUpDropStatusInfo.Text) : 0;
                if (pickUpDropStatusInfo > 1)
                {
                    imgBillPreview.Visible = false;
                    imgBillPreviewForMultiplePickUpDrop.Visible = true;
                }
                else
                {
                    imgBillPreview.Visible = true;
                    imgBillPreviewForMultiplePickUpDrop.Visible = false;
                }
            }
        }
        protected void gvRegistrationDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCopy = (ImageButton)e.Row.FindControl("ImgCopy");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView(1, 1, 0);
            SetTab("SearchTab");
        }
        protected void btnPagination_Click(object sender, EventArgs e)
        {
            SearchOrdering = "2";
            string PageNumber = string.Empty;
            string GridRecordCounts = string.Empty;
            string IsCurrentRPreviouss = string.Empty;

            PageNumber = hfPageNumber.Value;
            GridRecordCounts = hfGridRecordCounts.Value;
            GridRecordCounts = hfIsCurrentRPreviouss.Value;

            LoadGridView(Convert.ToInt32(PageNumber), Convert.ToInt32(GridRecordCounts), Convert.ToInt32(GridRecordCounts));
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool frmValid = IsFormValid();
                if (frmValid == false)
                {
                    isNewAddButtonEnable = 1;
                    SetTab("EntryTab");

                    return;
                }
                if (btnSave.Text.Equals("Save"))
                {
                    if (DateInHiddenField.Value != txtDateIn.Text)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Check-In Date is changed & selected room may reserved, Please delete the added room & add again.", AlertType.Warning);
                        txtDateIn.Focus();
                        return;
                    }
                    else if (DateOutHiddenField.Value != txtDateOut.Text)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Check-Out Date is changed & selected room may reserved, Please delete the added room & add again.", AlertType.Warning);
                        txtDateOut.Focus();
                        return;
                    }
                    else if (hfCurrencyHiddenField.Value != ddlCurrency.SelectedValue)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Currency is changed, Please delete the added room & add again.", AlertType.Warning);
                        txtDateOut.Focus();
                        return;
                    }
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RoomReservationDA roomReservationDA = new RoomReservationDA();
                RoomReservationBO roomReservationBO = new RoomReservationBO();

                roomReservationBO.ReservedMode = ddlReservedMode.SelectedItem.Text;
                roomReservationBO.Remarks = txtRemarks.Text;

                if (ddlReservationStatus.SelectedValue == "Pending")
                {
                    //int dMin = !string.IsNullOrWhiteSpace(txtPendingDeadlineMin.Text) ? Convert.ToInt32(txtPendingDeadlineMin.Text) : 0;
                    //int dHour = ddlProbableAMPM.SelectedIndex == 0 ? (Convert.ToInt32(txtPendingDeadlineHour.Text) % 12) : ((Convert.ToInt32(txtPendingDeadlineHour.Text) % 12) + 12);
                    //roomReservationBO.ConfirmationDate = hmUtility.GetDateTimeFromString(txtConfirmationDate.Text, userInformationBO.ServerDateFormat).AddHours(dHour).AddMinutes(dMin);
                }
                else
                {
                    roomReservationBO.ConfirmationDate = DateTime.Now;
                }

                roomReservationBO.DateOut = hmUtility.GetDateTimeFromString(txtDateOut.Text, userInformationBO.ServerDateFormat);
                roomReservationBO.ReservationType = string.Empty;
                roomReservationBO.ContactAddress = txtContactAddress.Text;
                roomReservationBO.ContactPerson = txtContactPerson.Text;
                roomReservationBO.MobileNumber = txtMobileNumber.Text;
                roomReservationBO.FaxNumber = string.Empty;
                roomReservationBO.ContactNumber = string.Empty;
                roomReservationBO.ContactEmail = txtContactEmail.Text;
                roomReservationBO.PayFor = Convert.ToInt32(ddlPayFor.SelectedValue);

                if (ddlReservedMode.SelectedItem.Text == "Company")
                {
                    roomReservationBO.PaymentMode = ddlPaymentMode.SelectedValue.ToString();
                    if (chkIsLitedCompany.Checked)
                    {
                        roomReservationBO.IsListedCompany = true;
                        roomReservationBO.CompanyId = Int32.Parse(ddlCompanyName.SelectedValue);
                        roomReservationBO.ReservedCompany = string.Empty;
                    }
                    else
                    {
                        roomReservationBO.IsListedCompany = false;
                        roomReservationBO.ReservedCompany = txtReservedCompany.Text;
                        ddlPaymentMode.SelectedIndex = 0;
                        roomReservationBO.PaymentMode = "Self";
                    }
                }
                else if (ddlReservedMode.SelectedItem.Text == "Group")
                {
                    roomReservationBO.PaymentMode = ddlPaymentMode.SelectedValue.ToString();
                    roomReservationBO.IsListedCompany = false;
                    roomReservationBO.GroupMasterId = Int32.Parse(ddlGroupName.SelectedValue);
                    roomReservationBO.ReservedCompany = txtGroupName.Text;
                    ddlPaymentMode.SelectedIndex = 0;
                    roomReservationBO.PaymentMode = "Self";
                }
                else
                {
                    ddlPaymentMode.SelectedIndex = 0;
                    roomReservationBO.PaymentMode = "Self";
                }

                if (string.IsNullOrEmpty(hiddenGuestId.Value))
                {
                    roomReservationBO.GuestId = 0;
                }
                else
                {
                    roomReservationBO.GuestId = Int32.Parse(hiddenGuestId.Value);
                }

                roomReservationBO.Reason = txtReason.Text;
                roomReservationBO.BusinessPromotionId = Int32.Parse(ddlBusinessPromotionId.SelectedValue);
                roomReservationBO.ReferenceId = Int32.Parse(ddlReferenceId.SelectedValue);
                roomReservationBO.ReservationMode = ddlReservationStatus.SelectedValue;
                roomReservationBO.CurrencyType = Convert.ToInt32(ddlCurrency.SelectedValue);
                roomReservationBO.ConversionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 0;
                roomReservationBO.NumberOfPersonAdult = !string.IsNullOrWhiteSpace(txtNumberOfPersonAdult.Text) ? Int32.Parse(txtNumberOfPersonAdult.Text) : 1;
                roomReservationBO.IsFamilyOrCouple = cbFamilyOrCouple.Checked ? true : false;
                roomReservationBO.NumberOfPersonChild = string.IsNullOrWhiteSpace(txtNumberOfPersonChild.Text) ? 0 : Convert.ToInt32(txtNumberOfPersonChild.Text.Trim());
                roomReservationBO.IsRoomRateShowInPreRegistrationCard = chkIsRoomRateShowInPreRegistrationCard.Checked ? true : false;

                List<ReservationComplementaryItemBO> complementaryItemBOList = new List<ReservationComplementaryItemBO>();
                // -- Complementary Item Information-------------------------------------------------
                for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
                {
                    if (chkComplementaryItem.Items[i].Selected)
                    {
                        ReservationComplementaryItemBO complementaryItemBO = new ReservationComplementaryItemBO();
                        complementaryItemBO.ComplementaryItemId = Int32.Parse(chkComplementaryItem.Items[i].Value);
                        complementaryItemBOList.Add(complementaryItemBO);
                    }
                }
                // -- Complementary Item Information----------------------------------------End----

                // -- Airport Pickup and Drop Information--------------------------------------------
                roomReservationBO.AirportPickUp = ddlAirportPickUp.SelectedValue;
                roomReservationBO.AirportDrop = ddlAirportDrop.SelectedValue;

                int isAirportPickupDropExist = 0;
                if (ddlArrivalFlightName.SelectedValue != "0")
                {
                    isAirportPickupDropExist = 1;
                }
                if (!string.IsNullOrWhiteSpace(txtArrivalFlightNumber.Text))
                {
                    isAirportPickupDropExist = 1;
                }
                if (ddlDepartureFlightName.SelectedValue != "0")
                {
                    isAirportPickupDropExist = 1;
                }
                if (!string.IsNullOrWhiteSpace(txtDepartureFlightNumber.Text))
                {
                    isAirportPickupDropExist = 1;
                }

                DateTime date = DateTime.Now;
                roomReservationBO.IsAirportPickupDropExist = isAirportPickupDropExist;
                roomReservationBO.ArrivalFlightName = ddlArrivalFlightName.SelectedItem.Text;
                roomReservationBO.ArrivalFlightNumber = txtArrivalFlightNumber.Text;

                DateTime currentDate = DateTime.Today;
                roomReservationBO.DepartureFlightName = ddlDepartureFlightName.SelectedItem.Text;
                roomReservationBO.DepartureFlightNumber = txtDepartureFlightNumber.Text;
                // -- Airport Pickup and Drop Information------------------------------------End----

                //--------** Paid Service Save, Update **-------------------------

                List<RegistrationServiceInfoBO> paidServiceDetails = new List<RegistrationServiceInfoBO>();
                bool paidServiceDeleted = false;

                paidServiceDetails = JsonConvert.DeserializeObject<List<RegistrationServiceInfoBO>>(hfPaidServiceSaveObj.Value.ToString());
                paidServiceDeleted = hfPaidServiceDeleteObj.Value.ToString().Trim() == "1" ? true : false;

                if (btnSave.Text.Equals("Save"))
                {
                    int tmpRoomTypeId = 0;
                    string currentReservationNumber = String.Empty;
                    roomReservationBO.ReservationId = ReservationId;
                    roomReservationBO.OnlineReservationId = 0;
                    roomReservationBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = roomReservationDA.SaveRoomReservationInfo(roomReservationBO, out tmpRoomTypeId, Session["ReservationDetailList"] as List<ReservationDetailBO>, complementaryItemBOList, out currentReservationNumber, paidServiceDetails, paidServiceDeleted);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Reservation No: " + currentReservationNumber + " Saved Successfully.", AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.RoomReservation.ToString(), tmpRoomTypeId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));
                        //SendMail(tmpRoomTypeId);
                        Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Reservation Not Succeed.", AlertType.Error);
                    }
                }
                else
                {
                    roomReservationBO.ReservationId = Convert.ToInt32(Session["_RoomReservationId"]);
                    ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
                    List<ReservationDetailBO> reservationDetailListBO = new List<ReservationDetailBO>();
                    List<ReservationDetailBO> retriveReservationDetailListBO = new List<ReservationDetailBO>();

                    List<ReservationDetailBO> retriveReservationDetailListBOForUpdate = new List<ReservationDetailBO>();
                    List<ReservationDetailBO> deletedReservationDetailListBO = new List<ReservationDetailBO>();

                    if (HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] == null)
                    {
                        retriveReservationDetailListBO = reservationDetailDA.GetReservationDetailByReservationId(roomReservationBO.ReservationId);
                        reservationDetailListBO = Session["ReservationDetailList"] as List<ReservationDetailBO>;

                        foreach (ReservationDetailBO reservationDetailBO in retriveReservationDetailListBO)
                        {
                            ReservationDetailBO detailBODelete;
                            detailBODelete = reservationDetailListBO.Where(x => x.RoomId == reservationDetailBO.RoomId).FirstOrDefault();
                            if (detailBODelete != null)
                            {
                                retriveReservationDetailListBOForUpdate.Add(detailBODelete);
                                reservationDetailListBO.Remove(detailBODelete);
                            }
                            else
                            {
                                reservationDetailBO.IsUpdateDetailData = 0;
                                deletedReservationDetailListBO.Add(reservationDetailBO);
                            }
                        }

                        if (deletedReservationDetailListBO != null)
                        {
                            HttpContext.Current.Session["AssignedDetailRoomDelete"] = deletedReservationDetailListBO;
                        }
                    }

                    //--------** Complimentary Item Process **---------------

                    HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
                    List<HMComplementaryItemBO> complementaryItemAlreadySaved = new List<HMComplementaryItemBO>();
                    complementaryItemAlreadySaved = complementaryItemDA.GetComplementaryItemInfoByReservationId(roomReservationBO.ReservationId);

                    List<ReservationComplementaryItemBO> newlyAddedComplementaryItem = new List<ReservationComplementaryItemBO>();
                    List<ReservationComplementaryItemBO> deletedComplementaryItem = new List<ReservationComplementaryItemBO>();

                    ReservationComplementaryItemBO complementaryItem;

                    foreach (HMComplementaryItemBO cmitm in complementaryItemAlreadySaved)
                    {
                        var v = (from c in complementaryItemBOList where c.ComplementaryItemId == cmitm.ComplementaryItemId select c).FirstOrDefault();

                        if (v == null)
                        {
                            complementaryItem = new ReservationComplementaryItemBO();
                            complementaryItem.ComplementaryItemId = cmitm.ComplementaryItemId;
                            complementaryItem.ReservationId = roomReservationBO.ReservationId;
                            complementaryItem.RCItemId = cmitm.RCItemId;
                            deletedComplementaryItem.Add(complementaryItem);
                        }
                    }

                    foreach (ReservationComplementaryItemBO cmitm in complementaryItemBOList)
                    {
                        var v = (from c in complementaryItemAlreadySaved where c.ComplementaryItemId == cmitm.ComplementaryItemId select c).FirstOrDefault();
                        if (v == null)
                        {
                            complementaryItem = new ReservationComplementaryItemBO();
                            complementaryItem.ComplementaryItemId = cmitm.ComplementaryItemId;
                            complementaryItem.ReservationId = roomReservationBO.ReservationId;
                            newlyAddedComplementaryItem.Add(complementaryItem);
                        }
                    }

                    //-------------------------------------------------------

                    string currentReservationNumber = String.Empty;
                    roomReservationBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = roomReservationDA.UpdateRoomReservationInfo(roomReservationBO, retriveReservationDetailListBOForUpdate, Session["ReservationDetailList"] as List<ReservationDetailBO>, newlyAddedComplementaryItem, deletedComplementaryItem, Session["AssignedDetailRoomDelete"] as List<ReservationDetailBO>, Session["UnassignedDetailRoomDelete"] as List<ReservationDetailBO>, Session["DeletedReservationDetailListByRoomType"] as List<ReservationDetailBO>, out currentReservationNumber, paidServiceDetails, paidServiceDeleted);

                    if (status)
                    {
                        Session["NewReservationDetailList"] = null;
                        CommonHelper.AlertInfo(innboardMessage, "Reservation No: " + currentReservationNumber + " Update Successfully.", AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.RoomReservation.ToString(), roomReservationBO.ReservationId,
                         ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));

                        // //// -----------------Grid View Load Related Information------------------------
                        int pageNumber = 1, isCurrentOrPreviousPage = 1, gridRecordsCount = 0;
                        if (System.Web.HttpContext.Current.Session["SrcReservationPageNumber"] != null)
                        {
                            pageNumber = !string.IsNullOrWhiteSpace(HttpContext.Current.Session["SrcReservationPageNumber"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["SrcReservationPageNumber"].ToString()) : 1;
                        }

                        if (System.Web.HttpContext.Current.Session["SrcReservationIsCurrentOrPreviousPage"] != null)
                        {
                            isCurrentOrPreviousPage = !string.IsNullOrWhiteSpace(HttpContext.Current.Session["SrcReservationIsCurrentOrPreviousPage"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["SrcReservationIsCurrentOrPreviousPage"].ToString()) : 1;
                        }

                        if (System.Web.HttpContext.Current.Session["SrcReservationGridRecordsCount"] != null)
                        {
                            gridRecordsCount = !string.IsNullOrWhiteSpace(HttpContext.Current.Session["SrcReservationGridRecordsCount"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["SrcReservationGridRecordsCount"].ToString()) : 0;
                        }

                        LoadGridView(pageNumber, isCurrentOrPreviousPage, gridRecordsCount);
                        Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Reservation Not Succeed.", AlertType.Error);
                    }
                }
                isListedCompanyVisible = -1;
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, "Reservation Not Succeed.", AlertType.Error);
            }
        }
        //************************ User Defined Function ********************//        
        private void CostCenterWiseSetting()
        {
            List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");
            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.Count > 0)
                {
                    hfInclusiveHotelManagementBill.Value = costCentreTabBO[0].IsVatSChargeInclusive.ToString();
                    hfIsDiscountApplicableOnRackRate.Value = costCentreTabBO[0].IsDiscountApplicableOnRackRate ? "1" : "0";
                    hfAdditionalCharge.Value = costCentreTabBO[0].AdditionalCharge.ToString();
                    hfAdditionalChargeType.Value = costCentreTabBO[0].AdditionalChargeType.ToString();

                    hfIsVatEnableOnGuestHouseCityCharge.Value = costCentreTabBO[0].IsVatOnSDCharge ? "1" : "0";
                    hfIsCitySDChargeEnableOnServiceCharge.Value = costCentreTabBO[0].IsCitySDChargeEnableOnServiceCharge ? "1" : "0";

                    hfCityCharge.Value = costCentreTabBO[0].CitySDCharge.ToString();
                    hfGuestHouseVat.Value = costCentreTabBO[0].VatAmount.ToString();
                    hfGuestHouseServiceCharge.Value = costCentreTabBO[0].ServiceCharge.ToString();
                    hfIsRatePlusPlus.Value = costCentreTabBO[0].IsRatePlusPlus.ToString();

                    hfIsServiceChargeEnableConfig.Value = costCentreTabBO[0].IsServiceChargeEnable ? "1" : "0";
                    hfIsCitySDChargeEnableConfig.Value = costCentreTabBO[0].IsCitySDChargeEnable ? "1" : "0";
                    hfIsVatEnableConfig.Value = costCentreTabBO[0].IsVatEnable ? "1" : "0";
                    hfIsAdditionalChargeEnableConfig.Value = costCentreTabBO[0].IsAdditionalChargeEnable ? "1" : "0";

                    if (hfInclusiveHotelManagementBill.Value == "1")
                    {
                        lblTotalRoomRateOrRoomTariff.Text = "Room Tariff";
                    }
                    else
                    {
                        lblTotalRoomRateOrRoomTariff.Text = "Total Room Rate";
                    }

                    // // --------Check Box Upcheck based on Costcenter Configuration
                    if (hfIsServiceChargeEnableConfig.Value == "0")
                        cbServiceCharge.Checked = false;

                    if (hfIsCitySDChargeEnableConfig.Value == "0")
                        cbCityCharge.Checked = false;

                    if (hfIsVatEnableConfig.Value == "0")
                        cbVatAmount.Checked = false;

                    if (hfIsAdditionalChargeEnableConfig.Value == "0")
                        cbAdditionalCharge.Checked = false;
                }
            }
        }
        private void LoadVIPGuestType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("VIPGuestType", hmUtility.GetDropDownFirstValue());

            ddlVIPGuestType.DataSource = fields;
            ddlVIPGuestType.DataTextField = "FieldValue";
            ddlVIPGuestType.DataValueField = "FieldId";
            ddlVIPGuestType.DataBind();
        }
        private void LoadMealPlan()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("FrontOfficeMealPlan", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }

            this.ddlMealPlanId.DataSource = fields;
            this.ddlMealPlanId.DataTextField = "FieldValue";
            this.ddlMealPlanId.DataValueField = "FieldId";
            this.ddlMealPlanId.DataBind();

            ListItem itemPayFor = new ListItem();
            itemPayFor.Value = "0";
            itemPayFor.Text = hmUtility.GetDropDownFirstValue();
            this.ddlMealPlanId.Items.Insert(0, itemPayFor);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("DefaultFrontOfficeMealPlanHeadId", "DefaultFrontOfficeMealPlanHeadId");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue != "0")
                    {
                        ddlMealPlanId.SelectedValue = commonSetupBO.SetupValue;
                        hfDefaultFrontOfficeMealPlanHeadId.Value = commonSetupBO.SetupValue;
                    }
                }
            }
        }
        private void LoadGuestCityCharge()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO GuestServiceSDChargeBO = new HMCommonSetupBO();
            GuestServiceSDChargeBO = commonSetupDA.GetCommonConfigurationInfo("GuestHouseCityCharge", "GuestHouseCityCharge");
            if (!string.IsNullOrEmpty(GuestServiceSDChargeBO.SetupValue))
            {
                lblCityChargeLabel.Text = GuestServiceSDChargeBO.Description.ToString();
            }
        }
        private void LoadMarketSegment()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("MarketSegment", hmUtility.GetDropDownFirstValue());
            this.ddlMarketSegment.DataSource = fields;
            this.ddlMarketSegment.DataTextField = "FieldValue";
            this.ddlMarketSegment.DataValueField = "FieldId";
            this.ddlMarketSegment.DataBind();

            List<CustomFieldBO> fieldsData = new List<CustomFieldBO>();
            fieldsData = commonDA.GetCustomField("MarketSegment");
            this.ddlSrcMarketSegment.DataSource = fieldsData;
            this.ddlSrcMarketSegment.DataTextField = "FieldValue";
            this.ddlSrcMarketSegment.DataValueField = "FieldId";
            this.ddlSrcMarketSegment.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "0";
            defaultItem.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcMarketSegment.Items.Insert(0, defaultItem);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("DefaultFrontOfficeMarketSegmentHeadId", "DefaultFrontOfficeMarketSegmentHeadId");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue != "0")
                    {
                        ddlMarketSegment.SelectedValue = commonSetupBO.SetupValue;
                        hfDefaultFrontOfficeMarketSegmentHeadId.Value = commonSetupBO.SetupValue;
                    }
                }
            }
        }
        private void LoadGuestSource()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("GuestSource", hmUtility.GetDropDownFirstValue());
            this.ddlGuestSource.DataSource = fields;
            this.ddlGuestSource.DataTextField = "FieldValue";
            this.ddlGuestSource.DataValueField = "FieldId";
            this.ddlGuestSource.DataBind();

            List<CustomFieldBO> fieldsData = new List<CustomFieldBO>();
            fieldsData = commonDA.GetCustomField("GuestSource");
            this.ddlSrcGuestSource.DataSource = fieldsData;
            this.ddlSrcGuestSource.DataTextField = "FieldValue";
            this.ddlSrcGuestSource.DataValueField = "FieldId";
            this.ddlSrcGuestSource.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "0";
            defaultItem.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcGuestSource.Items.Insert(0, defaultItem);
        }
        private void LoadGuestClassification()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("GuestClassification", hmUtility.GetDropDownFirstValue());
            this.ddlClassificationId.DataSource = fields;
            this.ddlClassificationId.DataTextField = "FieldValue";
            this.ddlClassificationId.DataValueField = "FieldId";
            this.ddlClassificationId.DataBind();

            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }

            this.ddlClassification.DataSource = fields;
            this.ddlClassification.DataTextField = "FieldValue";
            this.ddlClassification.DataValueField = "FieldId";
            this.ddlClassification.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlClassificationId.Items.Insert(0, item);
        }
        private void LoadAirlineInfo()
        {
            AirlineDA airlineDA = new AirlineDA();
            List<AirlineBO> airlineBOList = new List<AirlineBO>();
            airlineBOList = airlineDA.GetActiveAirlineInfo();
            ddlArrivalFlightName.DataSource = airlineBOList;
            ddlArrivalFlightName.DataTextField = "AirlineName";
            ddlArrivalFlightName.DataValueField = "AirlineId";
            ddlArrivalFlightName.DataBind();

            ddlDepartureFlightName.DataSource = airlineBOList;
            ddlDepartureFlightName.DataTextField = "AirlineName";
            ddlDepartureFlightName.DataValueField = "AirlineId";
            ddlDepartureFlightName.DataBind();

            ListItem itemRoomType = new ListItem();
            itemRoomType.Value = "0";
            itemRoomType.Text = hmUtility.GetDropDownFirstValue();
            ddlArrivalFlightName.Items.Insert(0, itemRoomType);
            ddlDepartureFlightName.Items.Insert(0, itemRoomType);
        }
        private void LoadCountryList()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();
            ddlGuestCountry.DataSource = countryList;
            ddlGuestCountry.DataTextField = "CountryName";
            ddlGuestCountry.DataValueField = "CountryId";
            ddlGuestCountry.DataBind();

            ListItem itemGuestCountry = new ListItem();
            itemGuestCountry.Value = "0";
            itemGuestCountry.Text = hmUtility.GetDropDownFirstValue();
            ddlGuestCountry.Items.Insert(0, itemGuestCountry);

            ddlCountry.DataSource = countryList;
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, itemGuestCountry);

            ddlPrevGstCountry.DataSource = countryList;
            ddlPrevGstCountry.DataTextField = "CountryName";
            ddlPrevGstCountry.DataValueField = "CountryId";
            ddlPrevGstCountry.DataBind();
            ddlPrevGstCountry.Items.Insert(0, itemGuestCountry);
        }
        private void LoadRRPaymentMode()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("RRPaymentMode", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }

            ddlPayFor.DataSource = fields;
            ddlPayFor.DataTextField = "FieldValue";
            ddlPayFor.DataValueField = "FieldId";
            ddlPayFor.DataBind();

            ListItem itemPayFor = new ListItem();
            itemPayFor.Value = "0";
            itemPayFor.Text = hmUtility.GetDropDownFirstValue();
            ddlPayFor.Items.Insert(0, itemPayFor);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            this.ddlArrivalChargableAmountCurrency.DataSource = currencyListBO;
            this.ddlArrivalChargableAmountCurrency.DataTextField = "CurrencyName";
            this.ddlArrivalChargableAmountCurrency.DataValueField = "CurrencyId";
            this.ddlArrivalChargableAmountCurrency.DataBind();

            this.ddlDepartureChargableAmountCurrency.DataSource = currencyListBO;
            this.ddlDepartureChargableAmountCurrency.DataTextField = "CurrencyName";
            this.ddlDepartureChargableAmountCurrency.DataValueField = "CurrencyId";
            this.ddlDepartureChargableAmountCurrency.DataBind();
        }
        private void LoadIsRoomOverbookingEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRoomOverbookingEnable", "IsRoomOverbookingEnable");
            hfIsRoomOverbookingEnable.Value = "0";
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        hfIsRoomOverbookingEnable.Value = "1";
                    }
                    else
                    {
                        hfIsRoomOverbookingEnable.Value = "0";
                    }
                }
            }
        }
        public void LoadTotalSelectedRoom()
        {
            string RoomNumber = hfSelectedRoomNumbers.Value;
            string RoomId = hfSelectedRoomId.Value;

            string[] Rooms = RoomNumber.Split(',');
            string[] Id = RoomId.Split(',');
            string A = "";
        }
        private void LoadBusinessPromotion()
        {
            BusinessPromotionDA bpDA = new BusinessPromotionDA();
            ddlBusinessPromotionId.DataSource = bpDA.GetCurrentActiveBusinessPromotionInfo();
            ddlBusinessPromotionId.DataTextField = "BPHead";
            ddlBusinessPromotionId.DataValueField = "BusinessPromotionId";
            ddlBusinessPromotionId.DataBind();

            ListItem itemReservation = new ListItem();
            itemReservation.Value = "0";
            itemReservation.Text = hmUtility.GetDropDownFirstValue();
            ddlBusinessPromotionId.Items.Insert(0, itemReservation);
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        public void LoadAffiliatedCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetAffiliatedGuestCompanyInfo();
            ddlCompanyName.DataSource = files;
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataValueField = "CompanyId";
            ddlCompanyName.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyName.Items.Insert(0, itemReference);
        }
        private void LoadRoomType()
        {
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> roomTypeList = new List<RoomTypeBO>();
            roomTypeList = roomTypeDA.GetRoomTypeInfo();

            ddlRoomTypeId.DataSource = roomTypeList;
            ddlRoomTypeId.DataTextField = "RoomType";
            ddlRoomTypeId.DataValueField = "RoomTypeId";
            ddlRoomTypeId.DataBind();

            ListItem itemRoomType = new ListItem();
            itemRoomType.Value = "0";
            itemRoomType.Text = hmUtility.GetDropDownFirstValue();
            ddlRoomTypeId.Items.Insert(0, itemRoomType);
            hfRoomType.Value = JsonConvert.SerializeObject(roomTypeList);
        }
        private void LoadProfession()
        {
            CommonProfessionDA professionDA = new CommonProfessionDA();
            List<CommonProfessionBO> entityBOList = new List<CommonProfessionBO>();
            entityBOList = professionDA.GetProfessionInfo();

            ddlProfessionId.DataSource = entityBOList;
            ddlProfessionId.DataTextField = "ProfessionName";
            ddlProfessionId.DataValueField = "ProfessionId";
            ddlProfessionId.DataBind();

            ListItem itemProfession = new ListItem();
            itemProfession.Value = "0";
            itemProfession.Text = hmUtility.GetDropDownFirstValue();
            ddlProfessionId.Items.Insert(0, itemProfession);
        }
        private void LoadGroupReservationInfo()
        {
            RoomReservationDA reservationDa = new RoomReservationDA();
            List<RoomReservationBillBO> groupRoomReservationInfoListBO = new List<RoomReservationBillBO>();
            groupRoomReservationInfoListBO = reservationDa.GetGroupRoomReservationInfo();

            ddlGroupName.DataSource = groupRoomReservationInfoListBO;
            ddlGroupName.DataTextField = "GroupName";
            ddlGroupName.DataValueField = "GroupMasterId";
            ddlGroupName.DataBind();

            ListItem itemProfession = new ListItem();
            itemProfession.Value = "0";
            itemProfession.Text = hmUtility.GetDropDownFirstValue();
            ddlGroupName.Items.Insert(0, itemProfession);
        }
        private void LoadRoomRateInfo()
        {
            if (ddlRoomTypeId.SelectedIndex != -1)
            {
                int roomTypeId = Convert.ToInt32(ddlRoomTypeId.SelectedValue);
                RoomTypeBO roomTypeBO = new RoomTypeBO();
                RoomTypeDA roomTypeDA = new RoomTypeDA();
                roomTypeBO = roomTypeDA.GetRoomTypeInfoById(roomTypeId);
                txtUnitPrice.Text = roomTypeBO.RoomRate.ToString();
                txtUnitPriceHiddenField.Value = roomTypeBO.RoomRate.ToString();
                txtRoomRate.Text = roomTypeBO.RoomRate.ToString();
            }
            else
            {
                txtUnitPrice.Text = "0";
                txtUnitPriceHiddenField.Value = "0";
                txtRoomRate.Text = "0";
            }
        }
        public void LoadGuestReference()
        {
            GuestReferenceDA entityDA = new GuestReferenceDA();
            List<GuestReferenceBO> files = entityDA.GetAllGuestRefference().Where(x => x.ActiveStat == true).ToList();
            ddlReferenceId.DataSource = files;
            ddlReferenceId.DataTextField = "Name";
            ddlReferenceId.DataValueField = "ReferenceId";
            ddlReferenceId.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            ddlReferenceId.Items.Insert(0, itemReference);

            ddlSrcReferenceId.DataSource = files;
            ddlSrcReferenceId.DataTextField = "Name";
            ddlSrcReferenceId.DataValueField = "ReferenceId";
            ddlSrcReferenceId.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "0";
            defaultItem.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcReferenceId.Items.Insert(0, defaultItem);
        }
        private void LoadComplementaryItem()
        {
            HMComplementaryItemDA entityDA = new HMComplementaryItemDA();
            List<HMComplementaryItemBO> files = entityDA.GetActiveHMComplementaryItemInfo();
            chkComplementaryItem.DataSource = files;
            chkComplementaryItem.DataTextField = "ItemName";
            chkComplementaryItem.DataValueField = "ComplementaryItemId";
            chkComplementaryItem.DataBind();
        }
        private void SetDefaultComplementaryItem()
        {
            HMComplementaryItemDA comDA = new HMComplementaryItemDA();
            List<HMComplementaryItemBO> comList = new List<HMComplementaryItemBO>();
            comList = comDA.GetActiveHMComplementaryItemInfo();

            ListItem itm;

            foreach (HMComplementaryItemBO ci in comList)
            {
                if (ci.IsDefaultItem == true && ci.ActiveStat == true)
                {
                    itm = chkComplementaryItem.Items.FindByValue(ci.ComplementaryItemId.ToString());

                    if (itm != null)
                    {
                        itm.Selected = true;
                    }
                }
                else
                {
                    itm = chkComplementaryItem.Items.FindByValue(ci.ComplementaryItemId.ToString());
                    if (itm != null)
                    {
                        itm.Selected = false;
                    }
                }
            }
        }
        private void LoadIsConversionRateEditable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsConversionRateEditable", "IsConversionRateEditable");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        this.txtConversionRate.ReadOnly = true;
                        isConversionRateEditable = true;
                    }
                    else
                    {
                        this.txtConversionRate.ReadOnly = false;
                        isConversionRateEditable = false;
                    }
                }
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private bool IsFormValid()
        {
            bool status = true;
            List<ReservationDetailBO> listReservationDetailBO = new List<ReservationDetailBO>();
            listReservationDetailBO = Session["ReservationDetailList"] as List<ReservationDetailBO>;
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();

            if (!string.IsNullOrWhiteSpace(txtMobileNumber.Text.Trim()))
            {
                var match = Regex.Match(txtMobileNumber.Text.Trim(), @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
                if (!match.Success)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Mobile Number.", AlertType.Warning);
                    txtMobileNumber.Focus();
                    return status = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(txtGuestPhone.Text.Trim()))
            {
                var match = Regex.Match(txtGuestPhone.Text.Trim(), @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
                if (!match.Success)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Guest Phone Number.", AlertType.Warning);
                    txtGuestPhone.Focus();
                    return status = false;
                }
            }

            if (listReservationDetailBO == null)
            {
                txtRoomId.Text = string.Empty;
                CommonHelper.AlertInfo(innboardMessage, "Please Add Room Information.", AlertType.Warning);
                ddlRoomTypeId.Focus();
                status = false;
            }
            else if (listReservationDetailBO.Count == 0)
            {
                txtRoomId.Text = string.Empty;
                CommonHelper.AlertInfo(innboardMessage, "Please Add Room Information.", AlertType.Warning);
                ddlRoomTypeId.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(txtDateIn.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Check In Date.", AlertType.Warning);
                txtDateIn.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(txtDateOut.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Expected Check Out Date.", AlertType.Warning);
                txtDateOut.Focus();
                status = false;
            }
            else if (ddlReservedMode.SelectedItem.Text == "Company")
            {
                isListedCompanyVisible = 1;
                if (chkIsLitedCompany.Checked)
                {
                    isListedCompanyDropDownVisible = 1;
                    if (ddlCompanyName.SelectedIndex == -1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Select Company Name.", AlertType.Warning);
                        ddlCompanyName.Focus();
                        status = false;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtReservedCompany.Text))
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Provide Company Name.", AlertType.Warning);
                        txtReservedCompany.Focus();
                        status = false;
                    }
                }
            }
            else if (listReservationDetailBO == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please add at least one room.", AlertType.Warning);
                status = false;
            }

            if (ddlReservationStatus.SelectedIndex != 0)
            {
                if (string.IsNullOrWhiteSpace(txtConfirmationDate.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Confirmation Date.", AlertType.Warning);
                    txtConfirmationDate.Focus();
                    status = false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtMobileNumber.Text) && string.IsNullOrWhiteSpace(txtContactEmail.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Mobile/ Email Information.", AlertType.Warning);
                txtMobileNumber.Focus();
                status = false;
            }

            if (!string.IsNullOrWhiteSpace(txtContactEmail.Text.Trim()))
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (!re.IsMatch(txtContactEmail.Text.Trim()))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Email Address.", AlertType.Warning);
                    status = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtDateIn.Text))
            {
                DateTime yeasterDayDateTime = DateTime.Now.Date.AddDays(-1);
                DateTime CheckInDate = hmUtility.GetDateTimeFromString(txtDateIn.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                if (btnSave.Text == "Update")
                {
                    DateTime CheckInSavedDate = hmUtility.GetDateTimeFromString(DateInHiddenFieldEdit.Value, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    if (CheckInDate > CheckInSavedDate)
                    {
                        if (CheckInDate < DateTime.Now.Date)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Check-In date must be current date or edit date.", AlertType.Warning);
                            txtDateIn.Focus();
                            status = false;
                        }
                    }
                    else if (CheckInDate < CheckInSavedDate && CheckInDate < DateTime.Now.Date)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Check-In Date can not previous date.", AlertType.Warning);
                        txtDateIn.Focus();
                        status = false;
                    }

                    if (ddlReservationStatus.SelectedValue == "Cancel")
                    {
                        if (string.IsNullOrWhiteSpace(txtReason.Text))
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please Provide Cancel Reason.", AlertType.Warning);
                            txtReason.Focus();
                            status = false;
                        }
                    }
                    else
                    {
                        txtReason.Text = string.Empty;
                    }

                    if (!status)
                        hfMinCheckInDate.Value = DateInHiddenFieldEdit.Value;
                }
                else if (yeasterDayDateTime >= CheckInDate)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide valid Check In Date.", AlertType.Warning);
                    txtDateIn.Focus();
                    hfMinCheckInDate.Value = DateInHiddenField.Value;
                    status = false;
                }
            }

            if (hfCurrencyType.Value != "Local")
            {
                if (string.IsNullOrWhiteSpace(txtConversionRate.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Conversion Rate.", AlertType.Warning);
                    txtConversionRate.Focus();
                    status = false;
                }
            }

            return status;
        }
        private bool IsValidMail(string Email)
        {
            bool status = true;
            string expression = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        private void AddEditODeleteDetail()
        {
            //Delete------------
            if (Session["arrayDelete"] == null)
            {
                arrayDelete = new ArrayList();
                Session.Add("arrayDelete", arrayDelete);
            }
            else
                arrayDelete = Session["arrayDelete"] as ArrayList;
        }
        private bool ValidateDetails()
        {
            int roomQuantity;
            bool isValid = true;
            if (string.IsNullOrEmpty(txtRoomId.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide Room Quantity.", AlertType.Warning);
                txtRoomId.Focus();
                isValid = false;
            }
            else if (!Int32.TryParse(txtRoomId.Text, out roomQuantity))
            {
                CommonHelper.AlertInfo(innboardMessage, "Room Quantity is not in correct format.", AlertType.Warning);
                txtRoomId.Focus();
                isValid = false;
            }
            return isValid;
        }
        private void ClearDetailPart()
        {
            ddlRoomTypeId.SelectedIndex = -1;
            txtRoomId.Text = string.Empty;
            lblHiddenId.Text = string.Empty;
            _RoomReservationId = 0;
        }
        private void Cancel()
        {
            pnlIsSameasGuest.Visible = true;
            if (IsSameasGuest.Checked)
            {
                IsSameasGuest.Checked = false;
            }
            pnlReservationEditTimePanelwillHide.Visible = true;
            string isSaveRUpdate = hfIsSaveRUpdate.Value, saveRUpdate = btnSave.Text;
            CommonHelper.ClearControl(Master.FindControl("ContentPlaceHolder1"));
            btnSave.Text = "Save";
            btnSave.Visible = isSavePermission;
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtPhone.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.ddlCountry.SelectedValue = "0";
            //this.ddlArrivalChargableAmountCurrency.SelectedValue = "0";
            //this.ddlDepartureChargableAmountCurrency.SelectedValue = "0";

            Random rd = new Random();
            ReservationId = rd.Next(100000, 999999);
            hfReservationIdTemp.Value = ReservationId.ToString();

            _RoomReservationId = -1;
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);

            Session["_RoomReservationId"] = null;
            Session["ReservationDetailList"] = null;
            Session["ReservationDetailListForGrid"] = null;
            Session["arrayDelete"] = null;
            Session["DeletedReservationDetailListByRoomType"] = null;

            SetDefaulTime();
            SetDefaultComplementaryItem();
            LoadIsRoomOverbookingEnable();

            if (saveRUpdate == "Update")
                LoadGridView(1, 1, 0);

            SetTab("EntryTab");

            if (!string.IsNullOrEmpty(isSaveRUpdate))
            {
                CommonHelper.AlertInfo(innboardMessage, "Reservation No: " + isSaveRUpdate + " " + saveRUpdate + " Successfully.", AlertType.Success);
            }

            //----------------------------
            ddlTitle.SelectedIndex = 0;
            cbFamilyOrCouple.Checked = false;
            chkIsRoomRateShowInReservationLetter.Checked = true;
            chkIsRoomRateShowInPreRegistrationCard.Checked = true;
            txtNumberOfPersonAdult.Text = "1";
            txtNumberOfPersonChild.Text = string.Empty;
            RoomDetailsTemp.InnerHtml = "";
            AirportPickupDetails.InnerHtml = "";

            hfArrivalAirlineId.Value = "0";
            hfDepartureAirlineId.Value = "0";
            ddlArrivalFlightName.SelectedValue = "0";
            ddlDepartureFlightName.SelectedValue = "0";
            hfIsCopyTo.Value = "false";
            CostCenterWiseSetting();
            CheckMandatoryField();

            hfDefaultFrontOfficeMealPlanHeadId.Value = "0";
            hfDefaultFrontOfficeMarketSegmentHeadId.Value = "0";
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("DefaultFrontOfficeMealPlanHeadId", "DefaultFrontOfficeMealPlanHeadId");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue != "0")
                    {
                        ddlMealPlanId.SelectedValue = commonSetupBO.SetupValue;
                        hfDefaultFrontOfficeMealPlanHeadId.Value = commonSetupBO.SetupValue;
                    }
                }
            }

            if (hfDefaultFrontOfficeMealPlanHeadId.Value == "0")
            {
                this.ddlMealPlanId.SelectedIndex = 0;
            }
            else
            {
                this.ddlMealPlanId.SelectedValue = hfDefaultFrontOfficeMealPlanHeadId.Value;
            }

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("DefaultFrontOfficeMarketSegmentHeadId", "DefaultFrontOfficeMarketSegmentHeadId");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue != "0")
                    {
                        ddlMarketSegment.SelectedValue = commonSetupBO.SetupValue;
                        hfDefaultFrontOfficeMarketSegmentHeadId.Value = commonSetupBO.SetupValue;
                    }
                }
            }

            if (hfDefaultFrontOfficeMarketSegmentHeadId.Value == "0")
            {
                this.ddlMarketSegment.SelectedIndex = 0;
            }
            else
            {
                this.ddlMarketSegment.SelectedValue = hfDefaultFrontOfficeMarketSegmentHeadId.Value;
            }
        }
        private void CancelCopy()
        {
            Session["_RoomReservationId"] = null;
            Session["ReservationDetailList"] = null;
            Session["ReservationDetailListForGrid"] = null;
        }
        private void SetDefaulTime()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            txtProbableArrivalTime.Text = DateTime.Today.AddHours(12).ToString(userInformationBO.TimeFormat);

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO reservationConfirmationLetterCheckInHoursBO = new HMCommonSetupBO();
            reservationConfirmationLetterCheckInHoursBO = commonSetupDA.GetCommonConfigurationInfo("ReservationConfirmationLetterCheckInHours", "ReservationConfirmationLetterCheckInHours");
            if (reservationConfirmationLetterCheckInHoursBO != null)
            {
                if (reservationConfirmationLetterCheckInHoursBO.SetupId > 0)
                {
                    txtProbableArrivalTime.Text = reservationConfirmationLetterCheckInHoursBO.SetupValue;
                }
            }

            txtProbableDepartureTime.Text = DateTime.Today.AddHours(12).ToString(userInformationBO.TimeFormat);
            HMCommonSetupBO reservationConfirmationLetterCheckOutHoursBO = new HMCommonSetupBO();
            reservationConfirmationLetterCheckOutHoursBO = commonSetupDA.GetCommonConfigurationInfo("ReservationConfirmationLetterCheckOutHours", "ReservationConfirmationLetterCheckOutHours");
            if (reservationConfirmationLetterCheckOutHoursBO != null)
            {
                if (reservationConfirmationLetterCheckOutHoursBO.SetupId > 0)
                {
                    txtProbableDepartureTime.Text = reservationConfirmationLetterCheckOutHoursBO.SetupValue;
                }
            }

            txtProbableArrivalPendingTime.Text = DateTime.Today.AddHours(12).ToString(userInformationBO.TimeFormat);
        }
        private void CopyToFillForm(int reservationId)
        {
            Cancel();
            LoadRoomType();
            CheckMandatoryField();
            LoadAffiliatedCompany();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            pnlIsSameasGuest.Visible = true;
            pnlReservationEditTimePanelwillHide.Visible = true;
            hfIsCopyTo.Value = "true";

            //Master Information------------------------
            RoomReservationBO roomReservationBO = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<HotelReservationAireportPickupDropBO> airportPickupDropList = new List<HotelReservationAireportPickupDropBO>();

            LoadIsConversionRateEditable();//currency convertion
            //got reservation details By Id
            roomReservationBO = roomReservationDA.GetRoomReservationInfoByIdNew(reservationId);
            airportPickupDropList = roomReservationDA.GetAirportPickupDropInfoByReservationId(reservationId);

            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            //got guest info by reservation Id
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResIdNew(Convert.ToInt32(reservationId), true);
            int isGuestCountGreaterThenZero = 0;
            if (guestInformationList != null)
            {
                if (guestInformationList.Count > 0)
                {
                    isGuestCountGreaterThenZero = 1;
                }
            }

            ddlReservedMode.Text = roomReservationBO.ReservedMode;
            if (roomReservationBO.ReservedMode == "Group")
            {
                ddlGroupName.SelectedValue = roomReservationBO.GroupMasterId.ToString();
                txtGroupName.Text = roomReservationBO.ReservedCompany;
            }

            txtRemarks.Text = roomReservationBO.Remarks;
            txtGuestRemarks.Text = roomReservationBO.GuestRemarks;
            txtPosRemarks.Text = roomReservationBO.POSRemarks;

            DateTime chkInDate = roomReservationBO.DateIn;
            DateTime chkOutDate = roomReservationBO.DateOut;

            var oneDayAfter = DateTime.Now.AddDays(1);

            if ((chkInDate.Date < DateTime.Now.Date) && (chkOutDate.Date > DateTime.Now.Date))
            {
                txtDateIn.Text = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtDateOut.Text = hmUtility.GetStringFromDateTime(roomReservationBO.DateOut);
            }
            else if ((chkInDate.Date < DateTime.Now) && (chkOutDate < DateTime.Now))
            {
                txtDateIn.Text = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtDateOut.Text = hmUtility.GetStringFromDateTime(oneDayAfter);
            }
            else if ((chkInDate.Date > DateTime.Now) && (chkOutDate > DateTime.Now))
            {
                txtDateIn.Text = hmUtility.GetStringFromDateTime(roomReservationBO.DateIn);
                txtDateOut.Text = hmUtility.GetStringFromDateTime(roomReservationBO.DateOut);
            }
            else
            {
                txtDateIn.Text = hmUtility.GetStringFromDateTime(roomReservationBO.DateIn);
                txtDateOut.Text = hmUtility.GetStringFromDateTime(roomReservationBO.DateOut);
            }
            DateTime checkIn = new DateTime();
            DateTime checkOut = new DateTime();

            checkIn = hmUtility.GetDateTimeFromString(txtDateIn.Text, userInformationBO.ServerDateFormat);
            checkOut = hmUtility.GetDateTimeFromString(txtDateOut.Text, userInformationBO.ServerDateFormat);

            double dateDiffDateInAndDateOut = (checkOut.Date - checkIn.Date).TotalDays;
            txtReservationDuration.Text = dateDiffDateInAndDateOut.ToString();

            if (isGuestCountGreaterThenZero > 0)
            {
                if (guestInformationList[0].Title == "")
                {
                    guestInformationList[0].Title = "N/A";
                }

                ddlTitle.SelectedValue = "0";
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                ddlCountry.SelectedValue = guestInformationList[0].GuestCountryId.ToString();
                txtEmail.Text = guestInformationList[0].GuestEmail;
            }
            else
            {
                ddlTitle.SelectedValue = "0";
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                ddlCountry.SelectedValue = "0";
                txtEmail.Text = string.Empty;
            }

            txtContactPerson.Text = roomReservationBO.ContactPerson;
            txtMobileNumber.Text = roomReservationBO.MobileNumber;
            txtContactEmail.Text = roomReservationBO.ContactEmail;
            txtReservedCompany.Text = roomReservationBO.ReservedCompany;
            txtContactAddress.Text = roomReservationBO.ContactAddress;
            txtMobileNumber.Text = roomReservationBO.MobileNumber;

            if (isGuestCountGreaterThenZero > 0)
            {
                if (guestInformationList[0].GuestName == roomReservationBO.ContactPerson)
                {
                    hfIsSameasContactPersonAsGuest.Value = "1";
                }
            }
            else
            {
                hfIsSameasContactPersonAsGuest.Value = string.Empty;
            }

            hiddenGuestId.Value = "";
            txtReason.Text = roomReservationBO.Reason;
            ddlBusinessPromotionId.SelectedValue = roomReservationBO.BusinessPromotionId.ToString();
            ddlCurrency.SelectedValue = roomReservationBO.CurrencyType.ToString();
            txtConversionRate.Text = roomReservationBO.ConversionRate.ToString();
            ddlReferenceId.SelectedValue = roomReservationBO.ReferenceId.ToString();
            ddlMarketSegment.SelectedValue = roomReservationBO.MarketSegmentId.ToString();
            ddlGuestSource.SelectedValue = roomReservationBO.GuestSourceId.ToString();
            txtNumberOfPersonAdult.Text = roomReservationBO.NumberOfPersonAdult.ToString();
            txtNumberOfPersonChild.Text = roomReservationBO.NumberOfPersonChild.ToString();
            cbFamilyOrCouple.Checked = roomReservationBO.IsFamilyOrCouple;
            chkIsLitedCompany.Checked = Convert.ToBoolean(roomReservationBO.IsListedCompany);
            ddlCompanyName.SelectedValue = roomReservationBO.CompanyId.ToString();
            ddlPayFor.SelectedValue = roomReservationBO.PayFor.ToString();
            ddlPaymentMode.SelectedValue = roomReservationBO.PaymentMode;
            chkIsRoomRateShowInPreRegistrationCard.Checked = roomReservationBO.IsRoomRateShowInPreRegistrationCard;

            txtBookersName.Text = roomReservationBO.BookersName.ToString();
            ddlClassification.SelectedValue = roomReservationBO.ClassificationId.ToString();
            ddlVIPGuestType.SelectedValue = roomReservationBO.VipGuestTypeId.ToString();
            ddlMealPlanId.SelectedValue = roomReservationBO.MealPlanId.ToString();
            chkIsVIPGuest.Checked = roomReservationBO.IsVIPGuest;

            //--Load Complementary Item Information--------------- 
            List<HMComplementaryItemBO> complementaryList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
            complementaryList = complementaryItemDA.GetComplementaryItemInfoByReservationId(reservationId);
            foreach (HMComplementaryItemBO dr in complementaryList)
            {
                for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
                {
                    if (Int32.Parse(chkComplementaryItem.Items[i].Value) == dr.ComplementaryItemId)
                    {
                        chkComplementaryItem.Items[i].Selected = true;
                    }
                }
            }

            //Detail Information------------------------
            decimal unitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField.Value) ? Convert.ToDecimal(txtUnitPriceHiddenField.Value) : 0;
            decimal discountAmount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 1;
            List<ReservationDetailBO> reservationDetailListBOForGrid = new List<ReservationDetailBO>();
            ReservationDetailDA reservationDetailDA = new ReservationDetailDA();

            reservationDetailListBOForGrid = reservationDetailDA.GetReservationDetailByRegiIdForGrid(reservationId, 0);
            if (reservationDetailListBOForGrid.Count > 0)
            {
                ReservationRoomDetailsGridCopy(reservationDetailListBOForGrid);
            }

            if (roomReservationBO.ReservationMode != "Registered")
            {
                if (roomReservationBO.ReservationMode == "NoShow")
                {
                    ddlReservationStatus.SelectedValue = "Confirmed";
                }
                else
                    ddlReservationStatus.SelectedValue = roomReservationBO.ReservationMode;
            }
            //reservationDetails
            List<ReservationDetailBO> reservationDetailListBO = new List<ReservationDetailBO>();
            reservationDetailListBO = reservationDetailDA.GetReservationDetailByReservationId(reservationId);

            Session["ReservationDetailList"] = reservationDetailListBO;
            btnSave.Text = "Save";

            GuestInformation = "adfdssd";

            if (isGuestCountGreaterThenZero > 0)
            {
                string strTable = "";
                strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservedGuestInformation'> " +
                            "<thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th style='display:none'></th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th><th align='left' scope='col'>Room</th> " +
                            "<th align='left' scope='col'>Action</th></tr></thead>";
                strTable += "<tbody>";
                strTable += "</tbody> </table>";
            }

            bool registered = IsRegistered(reservationId);
            SetRegisterdStatus(registered);
        }
        private void FillForm(int reservationId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            pnlIsSameasGuest.Visible = false;
            pnlReservationEditTimePanelwillHide.Visible = false;
            hfIsPaidServiceAlreadyLoded.Value = "0";
            ReservationId = reservationId;
            hfReservationIdTemp.Value = reservationId.ToString();
            hfReservationId.Value = reservationId.ToString();

            //Master Information------------------------
            RoomReservationBO roomReservationBO = new RoomReservationBO();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<HotelReservationAireportPickupDropBO> airportPickupDropList = new List<HotelReservationAireportPickupDropBO>();
            //Cancel();
            LoadRoomType();
            CheckMandatoryField();
            LoadAffiliatedCompany();
            LoadIsConversionRateEditable();
            roomReservationBO = roomReservationDA.GetRoomReservationInfoByIdNew(reservationId);
            airportPickupDropList = roomReservationDA.GetAirportPickupDropInfoByReservationId(reservationId);
            Session["_RoomReservationId"] = roomReservationBO.ReservationId; //guest info 

            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResIdNew(Convert.ToInt32(reservationId), true);
            int isGuestCountGreaterThenZero = 0;
            if (guestInformationList != null)
            {
                if (guestInformationList.Count > 0)
                {
                    isGuestCountGreaterThenZero = 1;
                }
            }

            if (isGuestCountGreaterThenZero > 0)
            {
                hfCurrentReservationGuestId.Value = guestInformationList[0].GuestId.ToString();
            }
            else
            {
                hfCurrentReservationGuestId.Value = string.Empty;
            }

            ddlReservedMode.Text = roomReservationBO.ReservedMode;
            if (roomReservationBO.ReservedMode == "Group")
            {
                ddlGroupName.SelectedValue = roomReservationBO.GroupMasterId.ToString();
                txtGroupName.Text = roomReservationBO.ReservedCompany;
            }

            txtDateIn.Text = hmUtility.GetStringFromDateTime(roomReservationBO.DateIn);
            DateInHiddenFieldEdit.Value = roomReservationBO.DateIn.ToString();
            txtProbableArrivalTime.Text = roomReservationBO.DateIn.ToString(userInformationBO.TimeFormat);
            txtProbableDepartureTime.Text = roomReservationBO.DateOut.ToString(userInformationBO.TimeFormat);
            DateTime CheackInDateTime = Convert.ToDateTime(roomReservationBO.DateIn);
            string S = CheackInDateTime.ToString("tt");
            txtRemarks.Text = roomReservationBO.Remarks;
            txtGuestRemarks.Text = roomReservationBO.GuestRemarks;
            txtPosRemarks.Text = roomReservationBO.POSRemarks;

            if (roomReservationBO.ConfirmationDate != null)
                txtConfirmationDate.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(roomReservationBO.ConfirmationDate));

            txtDateOut.Text = hmUtility.GetStringFromDateTime(roomReservationBO.DateOut);

            double dateDiffDateInAndDateOut = (roomReservationBO.DateOut.Date - roomReservationBO.DateIn.Date).TotalDays;
            txtReservationDuration.Text = dateDiffDateInAndDateOut.ToString();
            //Guest info
            if (isGuestCountGreaterThenZero > 0)
            {
                if (guestInformationList[0].Title == "")
                {
                    guestInformationList[0].Title = "N/A";
                }
                ddlTitle.SelectedValue = guestInformationList[0].Title;
                txtFirstName.Text = guestInformationList[0].FirstName;
                txtLastName.Text = guestInformationList[0].LastName;
                txtPhone.Text = guestInformationList[0].GuestPhone;
                ddlCountry.SelectedValue = guestInformationList[0].GuestCountryId.ToString();
                txtEmail.Text = guestInformationList[0].GuestEmail;
            }
            else
            {
                ddlTitle.SelectedValue = "0";
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                ddlCountry.SelectedValue = "0";
                txtEmail.Text = string.Empty;
            }

            txtContactPerson.Text = roomReservationBO.ContactPerson;
            txtMobileNumber.Text = roomReservationBO.MobileNumber;
            txtContactEmail.Text = roomReservationBO.ContactEmail;
            txtReservedCompany.Text = roomReservationBO.ReservedCompany;
            txtContactAddress.Text = roomReservationBO.ContactAddress;
            txtMobileNumber.Text = roomReservationBO.MobileNumber;

            if (isGuestCountGreaterThenZero > 0)
            {
                if (guestInformationList[0].GuestName == roomReservationBO.ContactPerson)
                {
                    hfIsSameasContactPersonAsGuest.Value = "1";
                }
            }
            else
            {
                hfIsSameasContactPersonAsGuest.Value = string.Empty;
            }

            hiddenGuestId.Value = roomReservationBO.GuestId.ToString();
            txtReason.Text = roomReservationBO.Reason;
            ddlBusinessPromotionId.SelectedValue = roomReservationBO.BusinessPromotionId.ToString();
            ddlCurrency.SelectedValue = roomReservationBO.CurrencyType.ToString();
            txtConversionRate.Text = roomReservationBO.ConversionRate.ToString();
            ddlReferenceId.SelectedValue = roomReservationBO.ReferenceId.ToString();
            ddlMarketSegment.SelectedValue = roomReservationBO.MarketSegmentId.ToString();
            ddlGuestSource.SelectedValue = roomReservationBO.GuestSourceId.ToString();
            txtNumberOfPersonAdult.Text = roomReservationBO.NumberOfPersonAdult.ToString();
            txtNumberOfPersonChild.Text = roomReservationBO.NumberOfPersonChild.ToString();
            cbFamilyOrCouple.Checked = roomReservationBO.IsFamilyOrCouple;
            chkIsLitedCompany.Checked = Convert.ToBoolean(roomReservationBO.IsListedCompany);
            chkIsLitedContact.Checked = Convert.ToBoolean(roomReservationBO.IsListedContact);
            ddlCompanyName.SelectedValue = roomReservationBO.CompanyId.ToString();
            hfContactId.Value = roomReservationBO.ContactId.ToString();
            if (roomReservationBO.IsListedContact)
            {
                if (!string.IsNullOrWhiteSpace(roomReservationBO.ContactPerson))
                {
                    txtListedContactPerson.Value = roomReservationBO.ContactPerson;
                }
            }

            ddlPayFor.SelectedValue = roomReservationBO.PayFor.ToString();
            ddlPaymentMode.SelectedValue = roomReservationBO.PaymentMode;
            chkIsRoomRateShowInReservationLetter.Checked = roomReservationBO.IsRoomRateShowInReservationLetter;
            chkIsRoomRateShowInPreRegistrationCard.Checked = roomReservationBO.IsRoomRateShowInPreRegistrationCard;

            txtBookersName.Text = roomReservationBO.BookersName.ToString();
            ddlClassification.SelectedValue = roomReservationBO.ClassificationId.ToString();
            ddlVIPGuestType.SelectedValue = roomReservationBO.VipGuestTypeId.ToString();

            ddlMealPlanId.SelectedValue = roomReservationBO.MealPlanId.ToString();
            chkIsVIPGuest.Checked = roomReservationBO.IsVIPGuest;

            if (roomReservationBO.IsComplementaryGuest)
            {
                ddlIsComplementary.SelectedValue = "1";
            }
            else
            {
                ddlIsComplementary.SelectedValue = "0";
            }

            // -- Uncheck All Complementary Item ---------------------
            for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
            {
                chkComplementaryItem.Items[i].Selected = false;
            }

            //--Load Complementary Item Information--------------- 
            List<HMComplementaryItemBO> complementaryList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
            complementaryList = complementaryItemDA.GetComplementaryItemInfoByReservationId(reservationId);
            foreach (HMComplementaryItemBO dr in complementaryList)
            {
                for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
                {
                    if (Int32.Parse(chkComplementaryItem.Items[i].Value) == dr.ComplementaryItemId)
                    {
                        chkComplementaryItem.Items[i].Selected = true;
                    }
                }
            }

            //Detail Information------------------------
            decimal unitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField.Value) ? Convert.ToDecimal(txtUnitPriceHiddenField.Value) : 0;
            decimal discountAmount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 1;
            List<ReservationDetailBO> reservationDetailListBOForGrid = new List<ReservationDetailBO>();
            ReservationDetailDA reservationDetailDA = new ReservationDetailDA();

            reservationDetailListBOForGrid = reservationDetailDA.GetReservationDetailByRegiIdForGrid(reservationId, 0);//get rooms details
            Session["ReservationDetailListForGrid"] = reservationDetailListBOForGrid; //for grid show
            ReservationRoomDetailGrid(reservationDetailListBOForGrid);

            if (roomReservationBO.ReservationMode != "Registered")
            {
                ddlReservationStatus.SelectedValue = roomReservationBO.ReservationMode;
            }
            //reservationDetails
            List<ReservationDetailBO> reservationDetailListBO = new List<ReservationDetailBO>();
            reservationDetailListBO = reservationDetailDA.GetReservationDetailByReservationId(reservationId);

            Session["ReservationDetailList"] = reservationDetailListBO; // room list 
            Session["RoomTypeIdList"] = reservationDetailListBO.Select(i => i.RoomTypeId).Distinct().ToList();
            btnSave.Text = "Update";
            btnSave.Visible = isUpdatePermission;
            GuestInformation = "adfdssd";

            if (isGuestCountGreaterThenZero > 0)
            {
                GetUserDetailHtml(guestInformationList);
            }

            bool registered = IsRegistered(reservationId);
            SetRegisterdStatus(registered);
            LoadAirportPickupDrop(airportPickupDropList);
        }
        private void LoadGuestTitle()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> titleList = new List<CustomFieldBO>();
            titleList = commonDA.GetCustomField("GuestTitle");

            ddlTitle.DataSource = titleList;
            ddlTitle.DataValueField = "FieldValue";
            ddlTitle.DataTextField = "Description";
            ddlTitle.DataBind();

            ddlGuestTitle.DataSource = titleList;
            ddlGuestTitle.DataValueField = "FieldValue";
            ddlGuestTitle.DataTextField = "Description";
            ddlGuestTitle.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTitle.Items.Insert(0, item);
            ddlGuestTitle.Items.Insert(0, item);
        }
        public void LoadAirportPickupDrop(List<HotelReservationAireportPickupDropBO> airportPickupDropList)
        {
            string grid1 = string.Empty, grid2 = string.Empty;
            int counter1 = 1, counter2 = 1;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (airportPickupDropList == null)
            {
                hfAPId.Value = "0";
                hfArrivalAirlineId.Value = "0";
                ddlArrivalFlightName.SelectedValue = "0";
                ddlAirportPickUp.SelectedValue = "0";
                ddlArrivalFlightName.SelectedValue = "0";
                txtArrivalFlightNumber.Text = string.Empty;
                txtArrivalTime.Text = string.Empty;

                hfDepartureAirlineId.Value = "0";
                ddlDepartureFlightName.SelectedValue = "0";
                ddlAirportDrop.SelectedValue = "0";
                ddlDepartureFlightName.SelectedValue = "0";
                txtDepartureFlightNumber.Text = string.Empty;
                txtDepartureTime.Text = string.Empty;

                chkIsArrivalChargable.Checked = false;
                txtArrivalChargableAmount.Text = String.Empty;
                chkIsDepartureChargable.Checked = false;
                txtDepartureChargableAmount.Text = String.Empty;
                txtArrivalChargableAmount.Enabled = false;
                txtDepartureChargableAmount.Enabled = false;
                return;
            }
            else if (airportPickupDropList.Count == 0)
            {
                hfAPId.Value = "0";
                ddlAirportPickUp.SelectedValue = "NO";
                hfArrivalAirlineId.Value = "0";
                ddlArrivalFlightName.SelectedValue = "0";
                ddlArrivalFlightName.SelectedValue = "0";
                txtArrivalFlightNumber.Text = string.Empty;
                txtArrivalTime.Text = string.Empty;
                ddlAirportDrop.SelectedValue = "NO";
                hfDepartureAirlineId.Value = "0";
                ddlDepartureFlightName.SelectedValue = "0";
                ddlAirportDrop.SelectedValue = "0";
                ddlDepartureFlightName.SelectedValue = "0";
                txtDepartureFlightNumber.Text = string.Empty;
                txtDepartureTime.Text = string.Empty;
                chkIsArrivalChargable.Checked = false;
                txtArrivalChargableAmount.Text = String.Empty;
                chkIsDepartureChargable.Checked = false;
                txtDepartureChargableAmount.Text = String.Empty;
                txtArrivalChargableAmount.Enabled = false;
                txtDepartureChargableAmount.Enabled = false;
                return;
            }

            if (airportPickupDropList.Count == 1)
            {
                hfAPId.Value = airportPickupDropList[0].APDId.ToString();
                if ((airportPickupDropList[0].AirportPickUp == "YES") || (airportPickupDropList[0].AirportPickUp == "TBA"))
                {
                    txtArrivalChargableAmount.Enabled = true;
                    ddlArrivalChargableAmountCurrency.Enabled = true;
                    hfArrivalAirlineId.Value = airportPickupDropList[0].ArrivalAirlineId.ToString();
                    ddlArrivalFlightName.SelectedValue = hfArrivalAirlineId.Value.ToString();
                    ddlAirportPickUp.SelectedValue = airportPickupDropList[0].AirportPickUp;
                    ddlArrivalFlightName.SelectedValue = airportPickupDropList[0].ArrivalAirlineId.ToString();
                    txtArrivalFlightNumber.Text = airportPickupDropList[0].ArrivalFlightNumber;
                    txtArrivalTime.Text = !string.IsNullOrWhiteSpace(airportPickupDropList[0].ArrivalTimeShow) ? Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + airportPickupDropList[0].ArrivalTimeShow).ToString(userInformationBO.TimeFormat) : string.Empty;
                    chkIsArrivalChargable.Checked = airportPickupDropList[0].IsArrivalChargable;
                    txtArrivalChargableAmount.Text = airportPickupDropList[0].ArrivalChargableAmount.ToString();
                    if (airportPickupDropList[0].ArrivalChargableAmountCurrency > 0)
                    {
                        ddlArrivalChargableAmountCurrency.SelectedValue = airportPickupDropList[0].ArrivalChargableAmountCurrency.ToString();
                    }
                }
                else if (airportPickupDropList[0].AirportPickUp == "NO")
                {
                    ddlAirportPickUp.SelectedValue = airportPickupDropList[0].AirportPickUp;
                }
            }
            else if (airportPickupDropList.Count >= 1)
            {
                if (airportPickupDropList[0].ArrivalAirlineId > 0)
                {
                    ddlAirportPickUp.SelectedValue = airportPickupDropList[0].AirportPickUp;
                    chkMultiplePickup.Checked = true;

                    foreach (HotelReservationAireportPickupDropBO bo in airportPickupDropList)
                    {
                        if (bo.GuestId != 0)
                        {
                            if (counter1 % 2 == 0)
                            {
                                grid1 += "<tr style='background-color:#E3EAEB;'>";
                            }
                            else
                            {
                                grid1 += "<tr style='background-color:White;'>";
                            }

                            grid1 += "<td style='display:none'>" + bo.APDId + "</td>";
                            grid1 += "<td style='display:none'>" + bo.ReservationId + "</td>";
                            grid1 += "<td style='display:none'>" + bo.GuestId + "</td>";
                            grid1 += "<td style='width:25%;'>" + bo.GuestName + "</td>";
                            grid1 += "<td style='width:25%;'>" + bo.ArrivalFlightName + "</td>";
                            grid1 += "<td style='width:20%;'>" + bo.ArrivalFlightNumber + "</td>";
                            grid1 += "<td style='width:20%;'>" + bo.ArrivalTimeShow + "</td>";
                            grid1 += "<td style='width:10%;'> <a href='#' onclick= 'DeletePickup(this)' ><img alt='Delete' src='../Images/delete.png' /></a></td>";
                            grid1 += "</tr>";
                            counter1++;
                        }
                    }
                }
            }

            if (airportPickupDropList.Count == 1)
            {
                hfAPId.Value = airportPickupDropList[0].APDId.ToString();
                if ((airportPickupDropList[0].AirportDrop == "YES") || (airportPickupDropList[0].AirportDrop == "TBA"))
                {
                    txtDepartureChargableAmount.Enabled = true;
                    ddlDepartureChargableAmountCurrency.Enabled = true;
                    hfDepartureAirlineId.Value = airportPickupDropList[0].DepartureAirlineId.ToString();
                    ddlDepartureFlightName.SelectedValue = hfDepartureAirlineId.Value.ToString();
                    ddlAirportDrop.SelectedValue = airportPickupDropList[0].AirportDrop;
                    ddlDepartureFlightName.SelectedValue = airportPickupDropList[0].DepartureAirlineId.ToString();
                    txtDepartureFlightNumber.Text = airportPickupDropList[0].DepartureFlightNumber;
                    txtDepartureTime.Text = !string.IsNullOrWhiteSpace(airportPickupDropList[0].DepartureTimeShow) ? Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + airportPickupDropList[0].DepartureTimeShow).ToString(userInformationBO.TimeFormat) : string.Empty;
                    chkIsDepartureChargable.Checked = airportPickupDropList[0].IsDepartureChargable;
                    txtDepartureChargableAmount.Text = airportPickupDropList[0].DepartureChargableAmount.ToString();
                    if (airportPickupDropList[0].DepartureChargableAmountCurrency > 0)
                    {
                        ddlDepartureChargableAmountCurrency.SelectedValue = airportPickupDropList[0].DepartureChargableAmountCurrency.ToString();
                    }
                }
                else if (airportPickupDropList[0].AirportDrop == "NO")
                {
                    ddlAirportDrop.SelectedValue = airportPickupDropList[0].AirportDrop;
                }
            }
            else if (airportPickupDropList.Count >= 1)
            {
                if (airportPickupDropList[0].DepartureAirlineId > 0)
                {
                    ddlAirportDrop.SelectedValue = airportPickupDropList[0].AirportDrop;
                    chkMultipleDrop.Checked = true;

                    foreach (HotelReservationAireportPickupDropBO bo in airportPickupDropList)
                    {
                        if (bo.GuestId != 0)
                        {
                            if (counter2 % 2 == 0)
                            {
                                grid2 += "<tr style='background-color:#E3EAEB;'>";
                            }
                            else
                            {
                                grid2 += "<tr style='background-color:White;'>";
                            }

                            grid2 += "<td style='display:none'>" + bo.APDId + "</td>";
                            grid2 += "<td style='display:none'>" + bo.ReservationId + "</td>";
                            grid2 += "<td style='display:none'>" + bo.GuestId + "</td>";
                            grid2 += "<td style='width:25%;'>" + bo.GuestName + "</td>";
                            grid2 += "<td style='width:25%;'>" + bo.DepartureFlightName + "</td>";
                            grid2 += "<td style='width:20%;'>" + bo.DepartureFlightNumber + "</td>";
                            grid2 += "<td style='width:20%;'>" + bo.DepartureTimeShow + "</td>";
                            grid2 += "<td style='width:10%;'> <a href='#' onclick= 'DeletePickup(this)' ><img alt='Delete' src='../Images/delete.png' /></a></td>";
                            grid2 += "</tr>";
                            counter2++;
                        }
                    }
                }
            }

            AirportPickupDetails.InnerText = grid1;
            AirportDropDetails.InnerText = grid2;
        }
        public void ReservationRoomDetailGrid(List<ReservationDetailBO> detailRoomList)
        {
            string grid = string.Empty;
            int counter = 1;

            foreach (ReservationDetailBO dr in detailRoomList)
            {
                if (counter % 2 == 0)
                {
                    grid += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    grid += "<tr style='background-color:White;'>";
                }

                grid += "<td align='left' style='width: 30%;'>" + dr.RoomType + "</td>";
                grid += "<td align='left' style='width: 38%;'>" + dr.RoomNumberListInfoWithCount + "</td>";
                grid += "<td align='left' style='width: 12%;display:none;'>" + dr.TotalCalculatedAmount * dr.TotalRoom + "</td>";
                grid += "<td align='left' style='width: 12%;'>" + dr.RoomTypeWisePaxQuantity + "</td>";
                grid += "<td align='center' style='width: 8%;'>";
                grid += "&nbsp;<img src='../Images/delete.png' class='deleteroom' alt='Delete Room' border='0' />";
                grid += "&nbsp;<img src='../Images/edit.png' class='editroom' alt='Edit Room' border='0' />";
                grid += "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.ReservationDetailIdList + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.RoomTypeId + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.RoomNumberIdList + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.RoomNumberList + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.TotalRoom + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.DiscountType + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.DiscountAmount + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.UnitPrice + "</td>";
                grid += "<td align='left' style='display:none;'>" + dr.RoomRate + "</td>";
                grid += "<td align='left' style='display:none;'>" + (dr.IsAdditionalChargeEnable == true ? 1 : 0) + "</td>";
                grid += "<td align='left' style='display:none;'>" + (dr.IsCityChargeEnable == true ? 1 : 0) + "</td>";
                grid += "<td align='left' style='display:none;'>" + (dr.IsServiceChargeEnable == true ? 1 : 0) + "</td>";
                grid += "<td align='left' style='display:none;'>" + (dr.IsVatAmountEnable == true ? 1 : 0) + "</td>";
                grid += "</tr>";
                counter++;
            }

            RoomDetailsTemp.InnerText = grid;
        }
        public void ReservationRoomDetailsGridCopy(List<ReservationDetailBO> detailRoomList)
        {
            ddlRoomTypeId.SelectedValue = detailRoomList[0].RoomTypeId.ToString();
            txtUnitPrice.Text = detailRoomList[0].UnitPrice.ToString();
            ddlDiscountType.SelectedValue = detailRoomList[0].DiscountType.ToString();
            txtDiscountAmount.Text = detailRoomList[0].DiscountAmount.ToString();
            txtRoomRate.Text = detailRoomList[0].RoomRate.ToString();
            cbServiceCharge.Checked = detailRoomList[0].IsServiceChargeEnable;
            cbCityCharge.Checked = detailRoomList[0].IsCityChargeEnable;
            cbVatAmount.Checked = detailRoomList[0].IsVatAmountEnable;
            cbAdditionalCharge.Checked = detailRoomList[0].IsAdditionalChargeEnable;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void DeleteData(int pkId)
        {
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            Boolean statusApproved = roomReservationDA.DeleteReservationDetailInfoById(pkId);
            if (statusApproved)
            {
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomReservation.ToString(), pkId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));
                CommonHelper.AlertInfo(innboardMessage, "Delete Operation Successfull.", AlertType.Warning);
                Cancel();
            }
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                E.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadGridView(int pageNumber, int isCurrentOrPreviousPage, int gridRecordsCount)
        {
            int ordering = 0;
            this.CheckObjectPermission();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string serviceIdList = string.Empty;

            DateTime dateTime = DateTime.Now;
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = txtFromDate.Text;
                FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = txtToDate.Text;
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            string guestName = txtSrcReservationGuest.Text;
            string reserveNo = txtSearchReservationNumber.Text;
            string companyName = txtSearchCompanyName.Text;
            string contactPerson = txtCntPerson.Text;
            string contactPhone = txtSrcContactPhone.Text;
            string contactEmail = txtSrcContactEmail.Text;
            string orderCriteria = ddlOrderCriteria.SelectedValue;
            string orderoption = ddlorderOption.SelectedValue;
            string status = ddlSearchByStatus.SelectedValue != "All" ? ddlSearchByStatus.SelectedValue : null;

            int srcMarketSegment = Convert.ToInt32(ddlSrcMarketSegment.SelectedValue);
            int srcGuestSource = Convert.ToInt32(ddlSrcGuestSource.SelectedValue);
            int srcReferenceId = Convert.ToInt32(ddlSrcReferenceId.SelectedValue);

            if (orderCriteria == "ReservationNo" && orderoption == "ASC")
            {
                ordering = 1;
            }
            else if (orderCriteria == "ReservationNo" && orderoption == "DESC")
            {
                ordering = 2;
            }
            else if (orderCriteria == "CheckInDate" && orderoption == "ASC")
            {
                ordering = 3;
            }
            else if (orderCriteria == "CheckInDate" && orderoption == "DESC")
            {
                ordering = 4;
            }

            CheckObjectPermission();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            int totalRecords = 0;

            RoomReservationDA rrDA = new RoomReservationDA();
            List<RoomReservationBO> reservationInfoList = new List<RoomReservationBO>();

            GridViewDataNPaging<RoomReservationBO, GridPaging> myGridData = new GridViewDataNPaging<RoomReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            reservationInfoList = rrDA.GetRoomReservationInformationBySearchCriteriaForPaging(FromDate, ToDate, guestName, reserveNo, companyName, contactPerson, contactPhone, contactEmail, srcMarketSegment, srcGuestSource, srcReferenceId, ordering, status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<RoomReservationBO> distinctItems = new List<RoomReservationBO>();
            distinctItems = reservationInfoList.GroupBy(test => test.ReservationId).Select(group => group.First()).ToList();
            myGridData.GridPagingProcessing(distinctItems, totalRecords, "GridPagingForSearchRegistration");

            if (reservationInfoList != null)
            {
                gvRoomRegistration.DataSource = distinctItems;
                gvRoomRegistration.DataBind();

                gridPaging.Text = myGridData.GridPageLinks.PreviousButton + myGridData.GridPageLinks.Pagination + myGridData.GridPageLinks.NextButton;
            }
            else
            {
                gvRoomRegistration.DataSource = null;
                gvRoomRegistration.DataBind();
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
            }
        }
        private void IsReservationStatusWaitingEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsReservationStatusWaitingEnable", "IsReservationStatusWaitingEnable");
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupValue == "0")
                {
                    ddlReservationStatus.Items.Remove(ddlReservationStatus.Items.FindByValue("Waiting"));
                }
            }
        }
        public static string GetHTMLRoomGridView(List<RoomNumberBO> List, List<ReservationDetailBO> reservationDetailList)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableRoomInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Room Number</th></tr>";
            int counter = 0;
            foreach (RoomNumberBO dr in List)
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

                strTable += "<td align='center' style='width: 82px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.RoomId + "' name='" + dr.RoomNumber + "' value='" + dr.RoomId + "' >";
                strTable += "</td><td align='left' style='width: 138px'>" + dr.RoomNumber + "</td>";

                var v = (from rs in reservationDetailList where rs.RoomId == dr.RoomId select rs).FirstOrDefault();

                if (v != null)
                {
                    strTable += "<td style='display:none;'>" + v.ReservationDetailId + "</td></tr>";
                }
                else
                {
                    strTable += "<td style='display:none;'>0</td></tr>";
                }
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        private static string GetUserDetailHtml(List<GuestInformationBO> registrationDetailListBO)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservedGuestInformation'> " +
                        "<thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th><th align='left' scope='col'>Room</th> " +
                        "<th align='left' scope='col'>Action</th></tr></thead>";

            int counter = 0;

            strTable += "<tbody>";
            foreach (GuestInformationBO dr in registrationDetailListBO)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='left' style=\"display:none;\">" + dr.GuestId + "</td>";
                strTable += "<td align='left' style='width: 40%'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%'>" + dr.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 10%'>" + dr.RoomNumber + "</td>";
                strTable += "<td align='left' style='width: 20%'>";

                if (dr.ReservationMode != "Registered")
                {
                    strTable += "&nbsp;<img src='../Images/edit.png' onClick='javascript:return PerformEditActionForGuestDetail(" + dr.GuestId + ")' alt='Edit Information' border='0' />";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformDeleteActionForGuestDetail(" + dr.GuestId + ")' alt='Delete Information' border='0' />";
                }
                strTable += "</td>";
                strTable += "</tr>";
            }

            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            strTable += "</tbody> </table>";

            return strTable;
        }
        public static string FormatedRoom(string RoomList, int quantity)
        {
            string formatedString = "";
            int Length = RoomList.Split(',').Length;
            var Rooms = RoomList.Split(',');
            if (quantity == Length)
            {
                formatedString = Length + "(" + RoomList + ")";
            }
            else
            {
                formatedString = quantity + "( " + Length + "(" + RoomList + ")" + "  " + (quantity - Length) + "( Unassigned )" + " )";

            }
            return formatedString;
        }
        public static string LoadDetailGridViewByWM()
        {
            string strTable = "";
            string reserVationDetailsId = string.Empty;

            List<ReservationDetailBO> detailList = HttpContext.Current.Session["ReservationDetailListForGrid"] as List<ReservationDetailBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Room Type</th><th align='left' scope='col'>Room Numbers</th><th align='center' scope='col'>Action</th></tr></thead> <tbody>";
                int counter = 0;
                foreach (ReservationDetailBO dr in detailList)
                {
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

                    if (dr.ReservationDetailId != 0)
                        reserVationDetailsId = dr.ReservationDetailId.ToString();
                    else
                        reserVationDetailsId = "0";

                    strTable += "<td align='left' style='display:none;'>" + dr.RoomTypeId + "</td>";
                    strTable += "<td align='left' style='display:none;'>" + reserVationDetailsId + "</td>";
                    strTable += "<td align='left' style='width: 40%;'>" + dr.RoomType + "</td>";
                    strTable += "<td align='left' style='width: 40%;'>" + dr.RoomNumberListInfoWithCount + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";

                    if (dr.RoomTypeId > 0)
                    {
                        strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformReservationDetailDelete(" + dr.RoomTypeId + ")' alt='Delete Information' border='0' />";
                        strTable += "&nbsp;<img src='../Images/edit.png' onClick='javascript:return PerformReservationDetailEdit(" + dr.RoomTypeId + "," + counter + ")' alt='Delete Information' border='0' />";
                    }
                    strTable += "</td></tr>";
                    counter++;
                }
                strTable += "</tbody></table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        private static void SendMail(int Id)
        {
            HMUtility hmUtility = new HMUtility();
            //Genarate Mail Body
            CompanyDA companyInfoDA = new CompanyDA();
            List<CompanyBO> list = companyInfoDA.GetCompanyInfo();
            string pComapnyName = "", pCompanyAddress = "", pCompanyWeb = "";
            if (list != null)
            {
                if (list.Count > 0)
                {
                    pComapnyName = list[0].CompanyName;
                    pCompanyAddress = list[0].CompanyAddress;
                    if (!string.IsNullOrWhiteSpace(list[0].WebAddress))
                    {
                        pCompanyWeb = list[0].WebAddress;
                    }
                    else
                    {
                        pCompanyWeb = list[0].ContactNumber;
                    }
                }
            }
            List<RoomReservationDetailsForMailBO> detailList = new List<RoomReservationDetailsForMailBO>();
            RoomReservationDetailsForMailDA detailDA = new RoomReservationDetailsForMailDA();
            detailList = detailDA.GetOnlineReservationDetailsInformationForMail(Id, pComapnyName, pCompanyAddress, pCompanyWeb);

            string table = "<table style='font-size: medium; border: #ccc 1px solid; width: 90%;' cellpadding='0' cellspacing='0'><tr><td rowspan='2'>Guest Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;' rowspan='2'>Room Type</td><td style='padding: 2px 10px; border: 1px solid #ccc;' rowspan='2'>Room Rate</td><td style='padding: 2px 10px; border: 1px solid #ccc;' colspan='2'> Check In</td><td style='padding: 2px 10px; border: 1px solid #ccc;' colspan='2'>Check Out</td></tr><tr><td style='padding: 2px 10px; border: 1px solid #ccc;'>Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Flight</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Flight</td></tr>";

            string ReceivingMail = "";
            foreach (RoomReservationDetailsForMailBO dr in detailList)
            {
                table += "<tr>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.GuestName;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.RoomType;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.RoomRate;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += hmUtility.GetStringFromDateTime(dr.ArrivalDate);
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.ArrivalFlightName;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += hmUtility.GetStringFromDateTime(dr.DepartureDate);
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.DepartureFlightName;
                table += "</td>";
                table += "</tr>";
            }
            table += "</table>";

            List<HMComplementaryItemBO> itemList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA itemDA = new HMComplementaryItemDA();
            itemList = itemDA.GetComplementaryItemInfoByReservationId(Id);

            string ul = "";
            ul += "<ul>";

            foreach (HMComplementaryItemBO dr in itemList)
            {
                ul += "<li style='font-size: medium'>";
                ul += dr.ItemName.ToString();
                ul += "</li>";
            }

            ul += "</ul>";

            ReceivingMail = detailList[0].ContactEmail;

            var tokens = new Dictionary<string, string>
                {
                      {"CompanyName",detailList[0].CompanyName},
                      {"CompanyAddress",detailList[0].CompanyAddress},
                      {"WebAddress",detailList[0].WebAddress},
                      {"TextWe", "We Accept"},
                      {"CurrentDate", DateTime.Now.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)},
                      {"ReservationDate", detailList[0].DepartureDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)},
                      {"GuestNameAndCompany","Mr. "+detailList[0].GuestName +", "+detailList[0].GuestCompanyName +", "+detailList[0].GuestCompanyAddress+" ."},
                      {"ContactPersonName",detailList[0].ContactPerson},
                      {"ReferencePersonName",detailList[0].ReferencePerson},
                      {"ContactPersonDesignation",detailList[0].ContactDesignation},
                      {"ReservationNumber",detailList[0].ReservationNumber},
                      {"ReferencePersonDesignation",detailList[0].ReferenceDesignation},
                      {"ContactPersonOrganization",detailList[0].GuestCompanyName +", "+detailList[0].GuestCompanyAddress+" ."},
                      {"ReferencePersonOrganization",detailList[0].ReferenceOrganization},
                      {"ContactPersonTelephone",detailList[0].TelephoneNumber},
                      {"ReferencePersonTelephone",detailList[0].ReferenceTelephone},
                      {"ContactPersonCell",detailList[0].ContactNumber},
                      {"ReferencePersonCell",detailList[0].ReferenceCellNumber},
                      {"ContactPersonFax",detailList[0].FaxNumber},
                      {"ReferencePersonFax",detailList[0].FaxNumber},
                      {"ContactPersonEmail",detailList[0].ContactEmail},
                      {"ReferencePersonEmail",detailList[0].ReferenceEmail},
                      {"RoomDetails",table},
                      {"TotalNumberOfRoom",detailList[0].TotalNumberOfRooms.ToString()},
                      {"ComplementaryItem",ul}
                };
            string MailBody = GetMailBody("ReservationEmailTemplete.html", tokens);
            //Genarate Mail Body END

            try
            {
                string Mail = "", Password = "", SmtpHost = "", SmtpPort = "";
                string CompanyName = "", CompanyAddress = "", CompanyWeb = "", ContactNumber = "";

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
                string mainString = commonSetupBO.SetupValue;
                if (!string.IsNullOrEmpty(mainString))
                {
                    string[] dataArray = mainString.Split('~');
                    Mail = dataArray[0];
                    Password = dataArray[1];
                    SmtpHost = dataArray[2];
                    SmtpPort = dataArray[3];
                }

                if (!string.IsNullOrWhiteSpace(ReceivingMail))
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(SmtpHost); //smtp.gmail.com
                    mail.From = new MailAddress(Mail);
                    mail.To.Add(ReceivingMail);
                    mail.Subject = "Online Room Reservation On Inn-Board";

                    StringBuilder sb = new StringBuilder();

                    //Get Design Data
                    int ReservationId = Id;
                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();
                    if (files[0].CompanyId > 0)
                    {
                        CompanyName = files[0].CompanyName;
                        CompanyAddress = files[0].CompanyAddress;
                        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                        {
                            CompanyWeb = files[0].WebAddress;
                        }
                        else
                        {
                            CompanyWeb = files[0].ContactNumber;
                        }
                    }

                    RoomReservationDA onlineReservationDA = new RoomReservationDA();
                    List<RoomReservationBO> List = new List<RoomReservationBO>();

                    //Mail Design End                
                    mail.Body = MailBody.ToString();
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = Int32.Parse(SmtpPort);// 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Mail, Password);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
        }
        private static string GetMailBody(string templateName, Dictionary<string, string> tokens)
        {
            string text = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath(string.Format("~/EmailTemplates/{0}", templateName)));
            return tokens.Aggregate(text, (current, token) => current.Replace(string.Format("##{0}##", token.Key), token.Value));
        }
        private void IsMinimumRoomRateCheckingEnable()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsShowMinimumRoomRate = new HMCommonSetupBO();
            IsShowMinimumRoomRate = commonSetupDA.GetCommonConfigurationInfo("IsMinimumRoomRateCheckingForRoomTypeEnable", "IsMinimumRoomRateCheckingForRoomTypeEnable");
            if (!string.IsNullOrWhiteSpace(IsShowMinimumRoomRate.SetupValue))
                hfIsMinimumRoomRateCheckingEnable.Value = IsShowMinimumRoomRate.SetupValue;
        }
        public static string GetHTMLGuestReferenceGridView(List<GuestPreferenceBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='GuestReferenceInformation' width='100%' border: '1px solid #cccccc'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Preference</th></tr>";
            int counter = 0;
            foreach (GuestPreferenceBO dr in List)
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

                strTable += "<td style='display:none;'>" + dr.PreferenceId + "</td>";
                strTable += "<td align='center' style='width: 20px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.PreferenceId + "' name='" + dr.PreferenceName + "' value='" + dr.PreferenceId + "' >";
                strTable += "</td><td align='left' style='width: 138px'>" + dr.PreferenceName + "</td></tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Preferences Available !</td></tr>";
            }
            strTable += "<div style='margin-top:12px;'>";
            strTable += "<div class=\"form-group\">" +
                            "<label for=\"Remarks\" class=\"control-label col-md-2\">Additional Remarks</label>" +
                            "<div class=\"col-md-10\">" +
                                "<textarea name = \"txtAdditionalRemarks\" rows=\"2\" cols=\"20\" id=\"ContentPlaceHolder1_txtAdditionalRemarks\" class=\"form-control\" runat=\"server\"></textarea>" +
                            "</div>" +
                        "</div>";
            strTable += "    <button type='button' onClick='javascript:return GetCheckedGuestPreference()' id='btnAddRoomId' class='btn btn-primary'> OK</button>";
            strTable += "</div>";
            return strTable;
        }
        private static bool SendSMSbySSLGatewayUpdate(RoomReservationBO reservationBO, string reservationNumber, int reservationId)
        {
            bool status = false;
            SmsView sms;
            try
            {
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                CommonDA commonDA = new CommonDA();
                RoomReservationDA reservationDA = new RoomReservationDA();
                string mainString = commonSetupBO.Description;
                string[] dataArray = mainString.Split('~');
                var smsGetway = dataArray[0];
                HMUtility hmUtility = new HMUtility();

                string documentsTextMessage = "Please provide all of your NID/ Driving License/ Passport Copy at the time of check-in.";
                HMCommonSetupBO commonSetupDocumentsTextMessageBO = new HMCommonSetupBO();
                commonSetupDocumentsTextMessageBO = commonSetupDA.GetCommonConfigurationInfo("RoomReservationSMSDocumentsMessage", "RoomReservationSMSDocumentsMessage");
                if (commonSetupDocumentsTextMessageBO.SetupId > 0)
                {
                    documentsTextMessage = commonSetupDocumentsTextMessageBO.Description;
                }

                // Config Block
                if (!string.IsNullOrEmpty(mainString))
                {

                    sms = new SmsView
                    {
                        TempleteName = HMConstants.SMSTemplates.ReservationAmedmentSmsTemplate
                    };

                    var singletoken = new Dictionary<string, string>
                        {
                        {"COMPANY", hmUtility.GetHMCompanyProfile()},
                        {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                        {"CONTACTNUMBER", hmUtility.GetHMCompanyContactNumber()},
                        {"Name", reservationBO.ContactPerson},
                        {"ReservationNumber",  reservationNumber},
                        {"ArrivalDate", reservationBO.DateIn.ToString()},
                        {"DepartureDate", reservationBO.DateOut.ToString()},
                        {"RoomNumber", reservationBO.RoomNumber },
                        {"SMSDocumentsMessage", documentsTextMessage }
                        };
                    status = SmsHelper.SendSmsSingle(sms, singletoken, smsGetway, reservationBO.MobileNumber);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        private static bool SendSMSbySSLGateway(RoomReservationBO reservationBO, string reservationNumber, int reservationId)
        {
            bool status = false;
            SmsView sms;
            try
            {
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                CommonDA commonDA = new CommonDA();
                RoomReservationDA reservationDA = new RoomReservationDA();
                string mainString = commonSetupBO.Description;
                string[] dataArray = mainString.Split('~');
                var smsGetway = dataArray[0];
                HMUtility hmUtility = new HMUtility();

                string documentsTextMessage = "Please provide all of your NID/ Driving License/ Passport Copy at the time of check-in.";
                HMCommonSetupBO commonSetupDocumentsTextMessageBO = new HMCommonSetupBO();
                commonSetupDocumentsTextMessageBO = commonSetupDA.GetCommonConfigurationInfo("RoomReservationSMSDocumentsMessage", "RoomReservationSMSDocumentsMessage");
                if (commonSetupDocumentsTextMessageBO.SetupId > 0)
                {
                    documentsTextMessage = commonSetupDocumentsTextMessageBO.Description;
                }

                // Config Block
                if (!string.IsNullOrEmpty(mainString))
                {

                    sms = new SmsView
                    {
                        TempleteName = HMConstants.SMSTemplates.ReservationCancellation
                    };

                    var singletoken = new Dictionary<string, string>
                        {
                        {"COMPANY", hmUtility.GetHMCompanyProfile()},
                        {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                        {"CONTACTNUMBER", hmUtility.GetHMCompanyContactNumber()},
                        {"Name", reservationBO.ContactPerson},
                        {"ReservationNumber",  reservationNumber},
                        {"ArrivalDate", reservationBO.DateIn.ToString()},
                        {"DepartureDate", reservationBO.DateOut.ToString()},
                        {"RoomNumber", reservationBO.RoomNumber },
                        {"SMSDocumentsMessage", documentsTextMessage }
                        };
                    status = SmsHelper.SendSmsSingle(sms, singletoken, smsGetway, reservationBO.MobileNumber);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        private static bool SendSMSbySSLGateway(RoomReservationBO reservationBO, string reservationNumber)
        {
            bool status = false;
            SmsView sms;
            try
            {
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                CommonDA commonDA = new CommonDA();
                RoomReservationDA reservationDA = new RoomReservationDA();
                string mainString = commonSetupBO.Description;
                string[] dataArray = mainString.Split('~');
                var smsGetway = dataArray[0];
                HMUtility hmUtility = new HMUtility();

                string documentsTextMessage = "Please provide all of your NID/ Driving License/ Passport Copy at the time of check-in.";
                HMCommonSetupBO commonSetupDocumentsTextMessageBO = new HMCommonSetupBO();
                commonSetupDocumentsTextMessageBO = commonSetupDA.GetCommonConfigurationInfo("RoomReservationSMSDocumentsMessage", "RoomReservationSMSDocumentsMessage");
                if (commonSetupDocumentsTextMessageBO.SetupId > 0)
                {
                    documentsTextMessage = commonSetupDocumentsTextMessageBO.Description;
                }

                // Config Block
                if (!string.IsNullOrEmpty(mainString))
                {

                    sms = new SmsView
                    {
                        TempleteName = HMConstants.SMSTemplates.ReservationConfirmation
                    };

                    var singletoken = new Dictionary<string, string>
                        {
                        {"COMPANY", hmUtility.GetHMCompanyProfile()},
                        {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                        {"CONTACTNUMBER", hmUtility.GetHMCompanyContactNumber()},
                        {"Name", reservationBO.ContactPerson},
                        {"ReservationNumber",  reservationNumber},
                        {"ArrivalDate", reservationBO.DateIn.ToString()},
                        {"DepartureDate", reservationBO.DateOut.ToString()},
                        {"RoomNumber", reservationBO.RoomNumber },
                        {"SMSDocumentsMessage", documentsTextMessage }
                        };
                    status = SmsHelper.SendSmsSingle(sms, singletoken, smsGetway, reservationBO.ContactNumber);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public static bool SaveActivityLogDetails(RoomReservationBO RoomReservation, RoomReservationBO PreviousBO, int activityId)
        {
            bool status = false;
            if (RoomReservation.DateIn != PreviousBO.DateIn)
            {

            }

            return status;
        }
        private static void CancelMail(int Id)
        {
            HMUtility hmUtility = new HMUtility();
            //Genarate Mail Body
            CompanyDA companyInfoDA = new CompanyDA();
            List<CompanyBO> list = companyInfoDA.GetCompanyInfo();
            string pComapnyName = "", pCompanyAddress = "", pCompanyWeb = "";
            if (list != null)
            {
                if (list.Count > 0)
                {
                    pComapnyName = list[0].CompanyName;
                    pCompanyAddress = list[0].CompanyAddress;
                    if (!string.IsNullOrWhiteSpace(list[0].WebAddress))
                    {
                        pCompanyWeb = list[0].WebAddress;
                    }
                    else
                    {
                        pCompanyWeb = list[0].ContactNumber;
                    }
                }
            }
            List<RoomReservationDetailsForMailBO> detailList = new List<RoomReservationDetailsForMailBO>();
            RoomReservationDetailsForMailDA detailDA = new RoomReservationDetailsForMailDA();
            detailList = detailDA.GetOnlineReservationDetailsInformationForMail(Id, pComapnyName, pCompanyAddress, pCompanyWeb);


            string table = "<table style='font-size: medium; border: #ccc 1px solid; width: 90%;' cellpadding='0' cellspacing='0'><tr><td rowspan='2'>Guest Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;' rowspan='2'>Room Type</td><td style='padding: 2px 10px; border: 1px solid #ccc;' rowspan='2'>Room Rate</td><td style='padding: 2px 10px; border: 1px solid #ccc;' colspan='2'> Check In</td><td style='padding: 2px 10px; border: 1px solid #ccc;' colspan='2'>Check Out</td></tr><tr><td style='padding: 2px 10px; border: 1px solid #ccc;'>Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Flight</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Flight</td></tr>";

            string ReceivingMail = "";
            foreach (RoomReservationDetailsForMailBO dr in detailList)
            {
                table += "<tr>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.GuestName;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.RoomType;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.RoomRate;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += hmUtility.GetStringFromDateTime(dr.ArrivalDate);
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.ArrivalFlightName;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += hmUtility.GetStringFromDateTime(dr.DepartureDate);
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.DepartureFlightName;
                table += "</td>";
                table += "</tr>";
            }
            table += "</table>";

            List<HMComplementaryItemBO> itemList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA itemDA = new HMComplementaryItemDA();
            itemList = itemDA.GetComplementaryItemInfoByReservationId(Id);

            string ul = "";
            ul += "<ul>";

            foreach (HMComplementaryItemBO dr in itemList)
            {
                ul += "<li style='font-size: medium'>";
                ul += dr.ItemName.ToString();
                ul += "</li>";
            }

            ul += "</ul>";

            ReceivingMail = detailList[0].ContactEmail;

            var tokens = new Dictionary<string, string>
                {
                      {"CompanyName",detailList[0].CompanyName},
                      {"CompanyAddress",detailList[0].CompanyAddress},
                      {"WebAddress",detailList[0].WebAddress},
                      {"TextWe", "We Accept"},
                      {"CurrentDate", DateTime.Now.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)},
                      {"ReservationDate", detailList[0].DepartureDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)},
                      {"GuestNameAndCompany","Mr. "+detailList[0].GuestName +", "+detailList[0].GuestCompanyName +", "+detailList[0].GuestCompanyAddress+" ."},
                      {"ContactPersonName",detailList[0].ContactPerson},
                      {"ReferencePersonName",detailList[0].ReferencePerson},
                      {"ContactPersonDesignation",detailList[0].ContactDesignation},
                      {"ReservationNumber",detailList[0].ReservationNumber},
                      {"ReferencePersonDesignation",detailList[0].ReferenceDesignation},
                      {"ContactPersonOrganization",detailList[0].GuestCompanyName +", "+detailList[0].GuestCompanyAddress+" ."},
                      {"ReferencePersonOrganization",detailList[0].ReferenceOrganization},
                      {"ContactPersonTelephone",detailList[0].TelephoneNumber},
                      {"ReferencePersonTelephone",detailList[0].ReferenceTelephone},
                      {"ContactPersonCell",detailList[0].ContactNumber},
                      {"ReferencePersonCell",detailList[0].ReferenceCellNumber},
                      {"ContactPersonFax",detailList[0].FaxNumber},
                      {"ReferencePersonFax",detailList[0].FaxNumber},
                      {"NAME", detailList[0].ContactPerson},
                      {"ContactPersonEmail",detailList[0].ContactEmail},
                      {"ReferencePersonEmail",detailList[0].ReferenceEmail},
                      {"RoomDetails",table},
                      {"TotalNumberOfRoom",detailList[0].TotalNumberOfRooms.ToString()},
                      {"ComplementaryItem",ul},
                      {"ArrivalDate", detailList[0].ArrivalDate.ToString()},
                      {"DepartureDate", detailList[0].DepartureDate.ToString()}
                };
            string MailBody = GetMailBody("ReservationEmailCancellation.html", tokens);
            //Genarate Mail Body END

            try
            {
                string Mail = "", Password = "", SmtpHost = "", SmtpPort = "";
                string CompanyName = "", CompanyAddress = "", CompanyWeb = "", ContactNumber = "";

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
                string mainString = commonSetupBO.SetupValue;
                if (!string.IsNullOrEmpty(mainString))
                {
                    string[] dataArray = mainString.Split('~');
                    Mail = dataArray[0];
                    Password = dataArray[1];
                    SmtpHost = dataArray[2];
                    SmtpPort = dataArray[3];
                }

                if (!string.IsNullOrWhiteSpace(ReceivingMail))
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(SmtpHost); //smtp.gmail.com
                    mail.From = new MailAddress(Mail);
                    mail.To.Add(ReceivingMail);
                    mail.Subject = "Online Room Reservation On Inn-Board";

                    StringBuilder sb = new StringBuilder();

                    //Get Design Data
                    //Script Name  [GetOnlineRoomReservationBillGenerate]
                    int ReservationId = Id;
                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();
                    if (files[0].CompanyId > 0)
                    {
                        CompanyName = files[0].CompanyName;
                        CompanyAddress = files[0].CompanyAddress;
                        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                        {
                            CompanyWeb = files[0].WebAddress;
                        }
                        else
                        {
                            CompanyWeb = files[0].ContactNumber;
                        }
                    }

                    RoomReservationDA onlineReservationDA = new RoomReservationDA();
                    List<RoomReservationBO> List = new List<RoomReservationBO>();

                    //Data Genaration
                    //Mail Design End                
                    mail.Body = MailBody.ToString();
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = Int32.Parse(SmtpPort);// 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Mail, Password);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
        }
        private static void UpdateMail(int Id)
        {
            HMUtility hmUtility = new HMUtility();
            //Genarate Mail Body
            CompanyDA companyInfoDA = new CompanyDA();
            List<CompanyBO> list = companyInfoDA.GetCompanyInfo();
            string pComapnyName = "", pCompanyAddress = "", pCompanyWeb = "";
            if (list != null)
            {
                if (list.Count > 0)
                {
                    pComapnyName = list[0].CompanyName;
                    pCompanyAddress = list[0].CompanyAddress;
                    if (!string.IsNullOrWhiteSpace(list[0].WebAddress))
                    {
                        pCompanyWeb = list[0].WebAddress;
                    }
                    else
                    {
                        pCompanyWeb = list[0].ContactNumber;
                    }
                }
            }
            List<RoomReservationDetailsForMailBO> detailList = new List<RoomReservationDetailsForMailBO>();
            RoomReservationDetailsForMailDA detailDA = new RoomReservationDetailsForMailDA();
            detailList = detailDA.GetOnlineReservationDetailsInformationForMail(Id, pComapnyName, pCompanyAddress, pCompanyWeb);


            string table = "<table style='font-size: medium; border: #ccc 1px solid; width: 90%;' cellpadding='0' cellspacing='0'><tr><td rowspan='2'>Guest Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;' rowspan='2'>Room Type</td><td style='padding: 2px 10px; border: 1px solid #ccc;' rowspan='2'>Room Rate</td><td style='padding: 2px 10px; border: 1px solid #ccc;' colspan='2'> Check In</td><td style='padding: 2px 10px; border: 1px solid #ccc;' colspan='2'>Check Out</td></tr><tr><td style='padding: 2px 10px; border: 1px solid #ccc;'>Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Flight</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Name</td><td style='padding: 2px 10px; border: 1px solid #ccc;'>Flight</td></tr>";

            string ReceivingMail = "";
            foreach (RoomReservationDetailsForMailBO dr in detailList)
            {
                table += "<tr>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.GuestName;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.RoomType;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.RoomRate;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += hmUtility.GetStringFromDateTime(dr.ArrivalDate);
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.ArrivalFlightName;
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += hmUtility.GetStringFromDateTime(dr.DepartureDate);
                table += "</td>";
                table += "<td style='padding: 2px 10px; border: 1px solid #ccc;'>";
                table += dr.DepartureFlightName;
                table += "</td>";
                table += "</tr>";
            }
            table += "</table>";

            List<HMComplementaryItemBO> itemList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA itemDA = new HMComplementaryItemDA();
            itemList = itemDA.GetComplementaryItemInfoByReservationId(Id);

            string ul = "";
            ul += "<ul>";

            foreach (HMComplementaryItemBO dr in itemList)
            {
                ul += "<li style='font-size: medium'>";
                ul += dr.ItemName.ToString();
                ul += "</li>";
            }

            ul += "</ul>";

            ReceivingMail = detailList[0].ContactEmail;

            var tokens = new Dictionary<string, string>
                {
                      {"CompanyName",detailList[0].CompanyName},
                      {"CompanyAddress",detailList[0].CompanyAddress},
                      {"WebAddress",detailList[0].WebAddress},
                      {"TextWe", "We Accept"},
                      {"CurrentDate", DateTime.Now.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)},
                      {"ReservationDate", detailList[0].DepartureDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)},
                      {"GuestNameAndCompany","Mr. "+detailList[0].GuestName +", "+detailList[0].GuestCompanyName +", "+detailList[0].GuestCompanyAddress+" ."},
                      {"ContactPersonName",detailList[0].ContactPerson},
                      {"ReferencePersonName",detailList[0].ReferencePerson},
                      {"ContactPersonDesignation",detailList[0].ContactDesignation},
                      {"ReservationNumber",detailList[0].ReservationNumber},
                      {"ReferencePersonDesignation",detailList[0].ReferenceDesignation},
                      {"ContactPersonOrganization",detailList[0].GuestCompanyName +", "+detailList[0].GuestCompanyAddress+" ."},
                      {"ReferencePersonOrganization",detailList[0].ReferenceOrganization},
                      {"ContactPersonTelephone",detailList[0].TelephoneNumber},
                      {"ReferencePersonTelephone",detailList[0].ReferenceTelephone},
                      {"ContactPersonCell",detailList[0].ContactNumber},
                      {"ReferencePersonCell",detailList[0].ReferenceCellNumber},
                      {"ContactPersonFax",detailList[0].FaxNumber},
                      {"ReferencePersonFax",detailList[0].FaxNumber},
                      {"NAME", detailList[0].ContactPerson },
                      {"ContactPersonEmail",detailList[0].ContactEmail},
                      {"ReferencePersonEmail",detailList[0].ReferenceEmail},
                      {"RoomDetails",table},
                      {"TotalNumberOfRoom",detailList[0].TotalNumberOfRooms.ToString()},
                      {"ComplementaryItem",ul},
                      {"ArrivalDate", detailList[0].ArrivalDate.ToString()},
                      {"DepartureDate", detailList[0].DepartureDate.ToString()}
                };
            string MailBody = GetMailBody("ReservationAmedment.html", tokens);
            //Genarate Mail Body END

            try
            {
                string Mail = "", Password = "", SmtpHost = "", SmtpPort = "";
                string CompanyName = "", CompanyAddress = "", CompanyWeb = "", ContactNumber = "";

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
                string mainString = commonSetupBO.SetupValue;
                if (!string.IsNullOrEmpty(mainString))
                {
                    string[] dataArray = mainString.Split('~');
                    Mail = dataArray[0];
                    Password = dataArray[1];
                    SmtpHost = dataArray[2];
                    SmtpPort = dataArray[3];
                }

                if (!string.IsNullOrWhiteSpace(ReceivingMail))
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(SmtpHost); //smtp.gmail.com
                    mail.From = new MailAddress(Mail);
                    mail.To.Add(ReceivingMail);
                    mail.Subject = "Online Room Reservation On Inn-Board";

                    StringBuilder sb = new StringBuilder();

                    //Get Design Data
                    //Script Name  [GetOnlineRoomReservationBillGenerate]
                    int ReservationId = Id;
                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();
                    if (files[0].CompanyId > 0)
                    {
                        CompanyName = files[0].CompanyName;
                        CompanyAddress = files[0].CompanyAddress;
                        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                        {
                            CompanyWeb = files[0].WebAddress;
                        }
                        else
                        {
                            CompanyWeb = files[0].ContactNumber;
                        }
                    }

                    RoomReservationDA onlineReservationDA = new RoomReservationDA();
                    List<RoomReservationBO> List = new List<RoomReservationBO>();

                    //Data Genaration
                    //Mail Design End                
                    mail.Body = MailBody.ToString();
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = Int32.Parse(SmtpPort);// 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Mail, Password);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static string ShowReservationCurrentRoomStatus(int reservationId)
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
            roomNumberListBO = roomNumberDA.GetRoomStatusInfoByReservationId(reservationId);

            // ----For Guest Name Showing..................................
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderBO singleRoomInfoBO = new RoomCalenderBO();

            Boolean isLinkedRoomFlag = false;
            Boolean isGuestNameDisplayFlag = true;

            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(DateTime.Now, DateTime.Now.AddDays(1));

            GuestInformationDA guestInfoDA = new GuestInformationDA();
            List<GuestHouseInfoForReportBO> TodaysExpectedArrivalroomList = new List<GuestHouseInfoForReportBO>();
            TodaysExpectedArrivalroomList = guestInfoDA.GetTodaysExpectedArrivalRoomId();

            var validRoom = new RoomNumberBO();
            var searchedRoom = new RoomNumberBO();
            var searchedRoomType = new RoomNumberBO();

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
                    var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
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

                        RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
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
                        var roomIdListBO = TodaysExpectedArrivalroomList.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId).FirstOrDefault();
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
            groupNamePart = "Status" + roomSummary;
            fullContent += subContent;
            string currentRoomStatus = topPart + groupNamePart + topTemplatePartEnd + bodyPart + fullContent + endTemplatePart;
            return currentRoomStatus;
        }
        [WebMethod]
        public static GridViewDataNPaging<GuestInformationBO, GridPaging> SearchGuestAndLoadGridInformation(string companyName, string DateOfBirth, string EmailAddress, string FromDate, string ToDate, string GuestName, string MobileNumber, string NationalId, string PassportNumber, string RegistrationNumber, string RoomNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<GuestInformationBO, GridPaging> myGridData = new GridViewDataNPaging<GuestInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> guestInfoList = new List<GuestInformationBO>();
            DateTime? fromDate = null;
            DateTime? toDate = null;

            HMUtility hmUtility = new HMUtility();
            if (FromDate != "")
            {
                fromDate = hmUtility.GetDateTimeFromString(FromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (ToDate != "")
            {
                toDate = hmUtility.GetDateTimeFromString(ToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            }

            string ReservationNumber = string.Empty;
            guestInfoList = guestDA.GetGuestInformationBySearchCriteriaForPaging(GuestName, EmailAddress, MobileNumber, NationalId, PassportNumber, DateOfBirth, companyName, RoomNumber, fromDate, toDate, RegistrationNumber, ReservationNumber, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<GuestInformationBO> distinctItems = new List<GuestInformationBO>();
            distinctItems = guestInfoList.GroupBy(test => test.GuestName).Select(group => group.First()).ToList();
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static GridViewDataNPaging<RoomReservationBO, GridPaging> SearchResevationAndLoadGridInformation(string strFromDate, string strToDate, string guestName, string reserveNo, string companyName, string contactPerson, string orderCriteria, string orderoption, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0, ordering = 0;

            string contactPhone = string.Empty;
            string contactEmail = string.Empty;

            int srcMarketSegment = 0;
            int srcGuestSource = 0;
            int srcReferenceId = 0;

            GridViewDataNPaging<RoomReservationBO, GridPaging> myGridData = new GridViewDataNPaging<RoomReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            RoomReservationDA rrDA = new RoomReservationDA();
            List<RoomReservationBO> reservationInfoList = new List<RoomReservationBO>();

            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(strFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(strFromDate, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(strToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(strToDate, userInformationBO.ServerDateFormat);
            }

            if (orderCriteria == "ReservationNo" && orderoption == "ASC")
            {
                ordering = 1;
            }
            else if (orderCriteria == "ReservationNo" && orderoption == "DESC")
            {
                ordering = 2;
            }
            else if (orderCriteria == "CheckInDate" && orderoption == "ASC")
            {
                ordering = 3;
            }
            else if (orderCriteria == "CheckInDate" && orderoption == "DESC")
            {
                ordering = 4;
            }
            reservationInfoList = rrDA.GetRoomReservationInformationBySearchCriteriaForPaging(fromDate, toDate, guestName, reserveNo, companyName, contactPerson, contactPhone, contactEmail, srcMarketSegment, srcGuestSource, srcReferenceId, ordering, null, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<RoomReservationBO> distinctItems = new List<RoomReservationBO>();
            distinctItems = reservationInfoList.GroupBy(test => test.ReservationNumber).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords, "GridPagingForSearchReservation");

            return myGridData;
        }
        [WebMethod]
        public static GuestInformationBO LoadDetailInformation(string GuestId)
        {
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationBO guestBO = new GuestInformationBO();
            guestBO = commonDA.GetGuestDetailInformation(GuestId);

            GuestPreferenceDA preferenceDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> preferenceList = new List<GuestPreferenceBO>();
            if (!string.IsNullOrEmpty(GuestId))
            {
                preferenceList = preferenceDA.GetGuestPreferenceInfoByGuestId(Convert.ToInt32(GuestId));

                if (preferenceList.Count > 0)
                {
                    foreach (GuestPreferenceBO preference in preferenceList)
                        if (guestBO.GuestPreferences != null)
                        {
                            guestBO.GuestPreferences += ", " + preference.PreferenceName;
                            guestBO.GuestPreferenceId += "," + preference.PreferenceId;
                        }
                        else
                        {
                            guestBO.GuestPreferences = preference.PreferenceName;
                            guestBO.GuestPreferenceId = preference.PreferenceId.ToString();
                        }
                }
            }
            return guestBO;
        }
        [WebMethod(EnableSession = true)]
        public static string LoadRoomInformationWithControl(string RoomTypeId, string fromDate, string toDate)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> list = new List<RoomNumberBO>();
            List<ReservationDetailBO> reservationDetailListBO = new List<ReservationDetailBO>();

            int _reservationId = 0;
            if (HttpContext.Current.Session["_RoomReservationId"] != null)
            {
                _reservationId = Int32.Parse(HttpContext.Current.Session["_RoomReservationId"].ToString());
            }
            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;
            DateTime EndDate = dateTime;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                StartDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = hmUtility.GetStringFromDateTime(dateTime);
                StartDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                EndDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = hmUtility.GetStringFromDateTime(dateTime);
                EndDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1);
            }

            list = roomNumberDA.GetAvailableRoomNumberInformation(Convert.ToInt32(RoomTypeId), 0, StartDate, EndDate, _reservationId);
            string HTML = string.Empty;
            if (_reservationId == 0)
            {
                List<RoomNumberBO> list2 = list.Where(p => p.RoomTypeId == Convert.ToInt32(RoomTypeId)).ToList();
                HTML = GetHTMLRoomGridView(list2, reservationDetailListBO);
            }
            else
            {
                ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
                reservationDetailListBO = reservationDetailDA.GetReservationDetailByReservationId(_reservationId);

                List<RoomNumberBO> list2 = list.Where(p => p.RoomTypeId == Convert.ToInt32(RoomTypeId)).ToList();
                HTML = GetHTMLRoomGridView(list2, reservationDetailListBO);
            }

            return HTML;
        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string GuestId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Guest", Int32.Parse(GuestId));

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                string ImgSource = dr.Path + dr.Name;
                counter++;
                strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                strTable += "<img id= style='width: 100px; height: 100px;' src='" + ImgSource + "'  alt='Image preview' />";
                strTable += "</div>";
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;

        }
        [WebMethod]
        public static RoomTypeBO RoomDetailsByRoomTypeId(int roomtypeId)
        {
            RoomTypeBO roomTypeBO = new RoomTypeBO();
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            roomTypeBO = roomTypeDA.GetRoomTypeInfoById(roomtypeId);
            return roomTypeBO;
        }
        [WebMethod]
        public static CompanyWiseDiscountPolicyBO GetDiscountPolicyByCompanyNRoomType(int companyId, int roomTypeId)
        {
            CompanyWiseDiscountPolicyDA discountDa = new CompanyWiseDiscountPolicyDA();
            CompanyWiseDiscountPolicyBO discountPolicy = new CompanyWiseDiscountPolicyBO();

            discountPolicy = discountDa.GetDiscountPolicyByCompanyNRoomType(companyId, roomTypeId, true);

            return discountPolicy;
        }
        [WebMethod(EnableSession = true)]
        public static ReturnInfo SaveReservation(RoomReservationBO RoomReservation, GuestInformationBO GuestDetailsInfo, List<ReservationDetailBO> RoomReservationDetail, List<ReservationDetailBO> RoomReservationSummary, List<HotelReservationAireportPickupDropBO> AireportPickupDrop, List<HotelReservationAireportPickupDropBO> AddedAirportPickupDrop, List<HotelReservationAireportPickupDropBO> DeletedAirportPickupDrop, List<ReservationComplementaryItemBO> ComplementaryItem, List<RegistrationServiceInfoBO> PaidServiceDetails, bool paidServiceDeleted, string displayRoomNumberNType)
        {
            int tmpReservationId = 0;
            int tmpapdId = 0;
            string currentReservationNumber = string.Empty;

            ReturnInfo rtnInfo = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            RoomReservationBO previousData = new RoomReservationBO();
            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                RoomReservation.DateIn = Convert.ToDateTime(RoomReservation.DateIn.ToString("yyyy-MM-dd") + " " + RoomReservation.ProbableArrivalTime.ToString(userInformationBO.TimeFormat));
                RoomReservation.DateOut = Convert.ToDateTime(RoomReservation.DateOut.ToString("yyyy-MM-dd") + " " + RoomReservation.ProbableDepartureTime.ToString(userInformationBO.TimeFormat));

                if ((GuestDetailsInfo != null) && (GuestDetailsInfo.IsCopyTo == false))
                {
                    int guestId = SaveGuestDetailsWithFInalSave(GuestDetailsInfo);
                    if (AireportPickupDrop.Count > 0)
                    {
                        foreach (HotelReservationAireportPickupDropBO bo in AireportPickupDrop)
                        {
                            bo.GuestId = guestId;
                        }
                    }
                }
                if ((GuestDetailsInfo != null) && (GuestDetailsInfo.IsCopyTo == true))
                {
                    int guestId = SaveGuestDetailsWithFInalSave(GuestDetailsInfo);
                    if (AireportPickupDrop.Count > 0)
                    {
                        foreach (HotelReservationAireportPickupDropBO bo in AireportPickupDrop)
                        {
                            bo.GuestId = guestId;
                        }
                    }
                }

                List<ReservationDetailBO> reservationDetailBOForPax = RoomReservationDetail.GroupBy(test => test.RoomTypeId).Select(group => group.First()).ToList();
                int totalPaxQuantity = 0;
                foreach (ReservationDetailBO reservationDetail in reservationDetailBOForPax)
                {
                    totalPaxQuantity = totalPaxQuantity + reservationDetail.RoomTypeWisePaxQuantity;
                }

                RoomReservation.NumberOfPersonAdult = totalPaxQuantity;
                RoomReservation.NumberOfPersonChild = 0;
                //new 
                if (RoomReservation.ReservationId == 0)
                {
                    RoomReservation.CreatedBy = userInformationBO.UserInfoId;

                    rtnInfo.IsSuccess = roomReservationDA.SaveRoomReservationInfoNew(RoomReservation, RoomReservationDetail, RoomReservationSummary, AireportPickupDrop, AddedAirportPickupDrop, ComplementaryItem,
                                                                                  PaidServiceDetails, paidServiceDeleted, out tmpReservationId, out currentReservationNumber, displayRoomNumberNType, out tmpapdId);
                    outReservationId = tmpReservationId;


                    if (rtnInfo.IsSuccess)
                    {
                        // // // HotelRoomReservationRoomDetail Table Related Process
                        roomReservationDA.UpdateHotelRoomReservationRoomDetail(tmpReservationId);
                        long activittyId = 0;
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                            EntityTypeEnum.EntityType.RoomReservation.ToString(), tmpReservationId,
                                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation), out activittyId);

                        if (RoomReservation.IsAirportPickupDropExist == 1)
                            logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                            EntityTypeEnum.EntityType.HotelReservationAireportPickupDrop.ToString(), tmpapdId,
                                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelReservationAireportPickupDrop));
                        foreach (var item in RoomReservationDetail)
                        {
                            logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                            EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), tmpReservationId,
                                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails) + ".Entity Id ReservationId");
                        }
                        Boolean statusUpdateContactInformation = roomReservationDA.UpdateHotelReservationContactInformation(tmpReservationId);
                        ArrayList pkArr = new ArrayList();
                        pkArr.Add(new { RN = currentReservationNumber, Pk = tmpReservationId, APDId = tmpapdId });
                        rtnInfo.Pk = currentReservationNumber;
                        rtnInfo.PKey = pkArr;
                        rtnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        // // // // SendMail(tmpReservationId); // Not Used
                        commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SMSAutoPosting", "IsRoomReservationSMSAutoPostingEnable");
                        string IsSMSEnable = commonSetupBO.SetupValue;
                        if (IsSMSEnable == "1")
                        {
                            SendSMSbySSLGateway(RoomReservation, currentReservationNumber);
                        }
                    }
                    else
                    {
                        rtnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                else//update
                {
                    DateTime yeasterDayDateTime = DateTime.Now.Date.AddDays(-1);
                    DateTime CheckInDate = RoomReservation.DateIn;

                    if (RoomReservation.ReservationId != 0)// update old
                    {
                        if (!string.IsNullOrWhiteSpace(RoomReservation.DateInFieldEdit))
                        {
                            DateTime CheckInSavedDate = Convert.ToDateTime(RoomReservation.DateInFieldEdit);

                            if (CheckInDate > CheckInSavedDate)
                            {
                                if (CheckInDate < DateTime.Now.Date)
                                {
                                    rtnInfo.AlertMessage = CommonHelper.AlertInfo("Check-In date must be current date or edit date.", AlertType.Warning);
                                    rtnInfo.IsSuccess = false;
                                    rtnInfo.IsReservationCheckInDateValidation = false;
                                    return rtnInfo;
                                }
                            }
                            else if (CheckInDate < CheckInSavedDate && CheckInDate < DateTime.Now.Date)
                            {
                                rtnInfo.AlertMessage = CommonHelper.AlertInfo("Check-In Date can not previous date.", AlertType.Warning);
                                rtnInfo.IsSuccess = false;
                                rtnInfo.IsReservationCheckInDateValidation = false;
                                return rtnInfo;
                            }
                        }
                    }
                    else if (yeasterDayDateTime >= CheckInDate)
                    {
                        rtnInfo.AlertMessage = CommonHelper.AlertInfo("Please Provide valid Check In Date.", AlertType.Warning);
                        rtnInfo.IsSuccess = false;
                        rtnInfo.IsReservationCheckInDateValidation = false;
                        return rtnInfo;
                    }
                    else
                    {
                        rtnInfo.IsReservationCheckInDateValidation = true;
                    }

                    RoomReservation.LastModifiedBy = userInformationBO.UserInfoId;

                    HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
                    List<HMComplementaryItemBO> complementaryItemAlreadySaved = new List<HMComplementaryItemBO>();
                    complementaryItemAlreadySaved = complementaryItemDA.GetComplementaryItemInfoByReservationId(RoomReservation.ReservationId);

                    List<ReservationComplementaryItemBO> newlyAddedComplementaryItem = new List<ReservationComplementaryItemBO>();
                    List<ReservationComplementaryItemBO> deletedComplementaryItem = new List<ReservationComplementaryItemBO>();

                    ReservationComplementaryItemBO complementaryItem;

                    foreach (HMComplementaryItemBO cmitm in complementaryItemAlreadySaved)
                    {
                        var v = (from c in ComplementaryItem where c.ComplementaryItemId == cmitm.ComplementaryItemId select c).FirstOrDefault();

                        if (v == null)
                        {
                            complementaryItem = new ReservationComplementaryItemBO();
                            complementaryItem.ComplementaryItemId = cmitm.ComplementaryItemId;
                            complementaryItem.ReservationId = RoomReservation.ReservationId;
                            complementaryItem.RCItemId = cmitm.RCItemId;
                            deletedComplementaryItem.Add(complementaryItem);
                        }
                    }

                    foreach (ReservationComplementaryItemBO cmitm in ComplementaryItem)
                    {
                        var v = (from c in complementaryItemAlreadySaved where c.ComplementaryItemId == cmitm.ComplementaryItemId select c).FirstOrDefault();

                        if (v == null)
                        {
                            complementaryItem = new ReservationComplementaryItemBO();
                            complementaryItem.ComplementaryItemId = cmitm.ComplementaryItemId;
                            complementaryItem.ReservationId = RoomReservation.ReservationId;
                            newlyAddedComplementaryItem.Add(complementaryItem);
                        }
                    }

                    List<ReservationDetailBO> reservationDetailList = new List<ReservationDetailBO>();
                    reservationDetailList = (List<ReservationDetailBO>)HttpContext.Current.Session["ReservationDetailList"];

                    List<ReservationDetailBO> reservationDetailAddedList = new List<ReservationDetailBO>();
                    List<ReservationDetailBO> reservationDetailEditedList = new List<ReservationDetailBO>();
                    List<ReservationDetailBO> reservationDetailDeletedList = new List<ReservationDetailBO>();

                    reservationDetailAddedList = (from rd in RoomReservationDetail
                                                  where rd.ReservationDetailId == 0
                                                  select rd
                                                  ).ToList();

                    reservationDetailEditedList = (from rd in RoomReservationDetail
                                                   join rdl in reservationDetailList
                                                   on rd.ReservationDetailId equals rdl.ReservationDetailId
                                                   select rd
                                                  ).ToList();

                    reservationDetailDeletedList = (from d in reservationDetailList
                                                    where !(
                                                             from rd in RoomReservationDetail
                                                             where rd.ReservationDetailId != 0
                                                             select rd.ReservationDetailId
                                                         ).Contains(d.ReservationDetailId)
                                                    select d).ToList();

                    List<int> previousRoomTypeIdList = (List<int>)HttpContext.Current.Session["RoomTypeIdList"];
                    previousData = roomReservationDA.GetRoomReservationInfoByIdNew(RoomReservation.ReservationId);
                    rtnInfo.IsSuccess = roomReservationDA.UpdateRoomReservationInfoNew(RoomReservation, reservationDetailAddedList, reservationDetailEditedList,
                                                                reservationDetailDeletedList, RoomReservationSummary, previousRoomTypeIdList, AireportPickupDrop, AddedAirportPickupDrop, DeletedAirportPickupDrop, newlyAddedComplementaryItem,
                                                                deletedComplementaryItem, PaidServiceDetails, paidServiceDeleted, displayRoomNumberNType, out currentReservationNumber);

                    if (rtnInfo.IsSuccess)
                    {
                        // // // HotelRoomReservationRoomDetail Table Related Process
                        roomReservationDA.UpdateHotelRoomReservationRoomDetail(RoomReservation.ReservationId);

                        commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsRoomReservationEmailAutoPostingEnable");
                        string IsEmailEnable = commonSetupBO.SetupValue;
                        if (IsEmailEnable == "1")
                        {
                            UpdateMail(RoomReservation.ReservationId);
                        }
                        RoomReservationDA reservationDA = new RoomReservationDA();
                        var result = reservationDA.GetRoomReservationInfoById(RoomReservation.ReservationId);
                    }

                    SalesMarketingLogType<RoomReservationBO> logDA = new SalesMarketingLogType<RoomReservationBO>();
                    long activityId = 0;
                    if (!rtnInfo.IsSuccess)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                            EntityTypeEnum.EntityType.RoomReservation.ToString(), RoomReservation.ReservationId,
                                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation), out activityId);

                        // // // activity log details related process
                        logDA.LogDetails(ConstantHelper.FOActivityLogFormName.frmRoomReservationNew, previousData, RoomReservation, activityId);

                        if (RoomReservation.IsAirportPickupDropExist == 1)
                            logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                            EntityTypeEnum.EntityType.HotelReservationAireportPickupDrop.ToString(), tmpapdId,
                                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelReservationAireportPickupDrop));
                        foreach (var item in reservationDetailAddedList)
                        {
                            logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                            EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), item.ReservationId,
                                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails) + ".Entity Id ReservationId");
                        }
                        foreach (var item in reservationDetailEditedList)
                        {
                            logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                            EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), item.ReservationId,
                                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails));
                        }
                        rtnInfo.IsSuccess = false;
                        rtnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                    else
                    {
                        Boolean statusUpdateContactInformation = roomReservationDA.UpdateHotelReservationContactInformation(RoomReservation.ReservationId);
                        if (statusUpdateContactInformation)
                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservation.ToString(), RoomReservation.ReservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation), out activityId);
                        tmpReservationId = RoomReservation.ReservationId;
                        rtnInfo.Pk = currentReservationNumber;

                        // // // activity log details related process
                        logDA.LogDetails(ConstantHelper.FOActivityLogFormName.frmRoomReservationNew, previousData, RoomReservation, activityId);
                        //rtnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
            }
            catch (Exception ex)
            {
                rtnInfo.IsSuccess = false;
                rtnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtnInfo;
        }
        [WebMethod(EnableSession = true)]
        public static string SaveGuestInformationAsDetail(string reservationId, int prevGuestId, string isEdit, int IsEditFromAddMore, string title, string firstName, string lastName, string txtGuestName, string txtGuestEmail, string hiddenGuestId, string txtGuestDrivinlgLicense, string txtGuestDOB, string txtGuestAddress1, string txtGuestAddress2, string ddlProfessionId, string txtGuestCity, string ddlGuestCountry, string txtGuestNationality, string txtGuestPhone, string ddlGuestSex, string txtGuestZipCode, string txtNationalId, string txtPassportNumber, string txtPExpireDate, string txtPIssueDate, string txtPIssuePlace, string txtVExpireDate, string txtVisaNumber, string txtVIssueDate, int roomId, string selectedPreferenceId, int classificationId, string additionalRemarks)
        {
            HMUtility hmUtility = new HMUtility();
            RoomReservationDA resDA = new RoomReservationDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO detailBO = new GuestInformationBO();
            detailBO.tempOwnerId = Int32.Parse(reservationId);
            detailBO.GuestAddress1 = txtGuestAddress1;
            detailBO.GuestAddress2 = txtGuestAddress2;
            detailBO.GuestAuthentication = "";
            detailBO.ProfessionId = Int32.Parse(ddlProfessionId);
            detailBO.GuestCity = txtGuestCity;
            detailBO.GuestCountryId = !string.IsNullOrWhiteSpace(ddlGuestCountry) ? Convert.ToInt32(ddlGuestCountry) : 0;

            if (!string.IsNullOrWhiteSpace(txtGuestDOB))
            {
                detailBO.GuestDOB = hmUtility.GetDateTimeFromString(txtGuestDOB, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.GuestDOB = null;
            }
            detailBO.GuestDrivinlgLicense = txtGuestDrivinlgLicense;
            detailBO.GuestEmail = txtGuestEmail;
            detailBO.Title = title;
            detailBO.FirstName = firstName;
            detailBO.LastName = lastName;
            detailBO.GuestName = txtGuestName;
            if (string.IsNullOrEmpty(hiddenGuestId))
            {
                detailBO.GuestId = 0;
            }
            else
            {
                detailBO.GuestId = Int32.Parse(hiddenGuestId);//
            }
            detailBO.GuestNationality = txtGuestNationality;
            detailBO.GuestPhone = txtGuestPhone;
            detailBO.ClassificationId = classificationId;
            detailBO.GuestSex = ddlGuestSex;
            detailBO.GuestZipCode = txtGuestZipCode;
            detailBO.NationalId = txtNationalId;
            detailBO.PassportNumber = txtPassportNumber;
            if (!string.IsNullOrWhiteSpace(txtPExpireDate))
            {
                detailBO.PExpireDate = hmUtility.GetDateTimeFromString(txtPExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PExpireDate = null;
            }
            if (!string.IsNullOrWhiteSpace(txtPIssueDate))
            {
                detailBO.PIssueDate = hmUtility.GetDateTimeFromString(txtPIssueDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PIssueDate = null;
            }
            detailBO.PIssuePlace = txtPIssuePlace;
            if (!string.IsNullOrWhiteSpace(txtVExpireDate))//
            {
                detailBO.VExpireDate = hmUtility.GetDateTimeFromString(txtVExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VExpireDate = null;
            }
            detailBO.VisaNumber = txtVisaNumber;
            if (!string.IsNullOrWhiteSpace(txtVIssueDate))//
            {
                detailBO.VIssueDate = hmUtility.GetDateTimeFromString(txtVIssueDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VIssueDate = null;
            }

            if (roomId != 0)
            {
                detailBO.RoomId = roomId;
            }
            else detailBO.RoomId = null;
            detailBO.AdditionalRemarks = additionalRemarks;
            List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();

            long prfId = 0;
            if (!string.IsNullOrEmpty(selectedPreferenceId))
            {
                string[] preferenceIds = selectedPreferenceId.Split(',');
                for (int i = 0; i < preferenceIds.Count(); i++)
                {
                    GuestPreferenceMappingBO preferenceBO = new GuestPreferenceMappingBO();
                    prfId = Convert.ToInt64(preferenceIds[i]);
                    preferenceBO.PreferenceId = prfId;
                    preferenList.Add(preferenceBO);
                }
            }

            Boolean status = false;
            //  detailBO.Document = documentBO;
            if (string.IsNullOrEmpty(isEdit))
            {
                if (prevGuestId == 0)
                {
                    status = resDA.SaveTemporaryGuestNew(detailBO, reservationId, preferenList);
                }
                else if (IsEditFromAddMore == 1)
                {
                    detailBO.GuestId = prevGuestId;
                    status = guestDA.UpdateGuestInformationNew(detailBO, "0", string.Empty, preferenList);
                }
                else
                {
                    detailBO.GuestId = prevGuestId;
                    status = guestDA.UpdateGuestInformationNew(detailBO, reservationId, string.Empty, preferenList);
                }
            }
            else
            {
                detailBO.GuestId = Int32.Parse(isEdit);
                status = guestDA.UpdateGuestInformationNew(detailBO, "0", string.Empty, preferenList);
            }
            if (status)
            {
                if (detailBO.GuestId > 0)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), detailBO.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                    if (preferenList.Count > 0)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), detailBO.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "EntityId is guestId");
                }
                else
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), detailBO.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), detailBO.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "Deleted is guestId");
                    if (preferenList.Count > 0)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), detailBO.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "EntityId is guestId");
                }
            }
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResId(Convert.ToInt32(reservationId), true);
            return GetUserDetailHtml(guestInformationList);
        }
        private static int SaveGuestDetailsWithFInalSave(GuestInformationBO guestInfo)
        {
            bool status = false;
            int tmpGuestId = 0;
            HMUtility hmUtility = new HMUtility();
            RoomReservationDA resDA = new RoomReservationDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();
            long prfId = 0;
            if (!string.IsNullOrEmpty(guestInfo.GuestPreferences))
            {
                string[] preferenceIds = guestInfo.GuestPreferences.Split(',');
                for (int i = 0; i < preferenceIds.Count(); i++)
                {
                    GuestPreferenceMappingBO preferenceBO = new GuestPreferenceMappingBO();
                    prfId = Convert.ToInt64(preferenceIds[i]);
                    preferenceBO.PreferenceId = prfId;
                    preferenList.Add(preferenceBO);
                }
            }

            if (guestInfo.GuestId == 0)
            {
                if (guestInfo.PreviousGuestId == 0)
                {
                    tmpGuestId = resDA.SaveTemporaryGuest(guestInfo, guestInfo.tempOwnerId.ToString(), preferenList);
                    if (tmpGuestId > 0)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), tmpGuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                        if (preferenList.Count > 0)
                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), tmpGuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "EntityId is guestId");
                    }
                }
                else
                {
                    guestInfo.GuestId = guestInfo.PreviousGuestId;
                    status = guestDA.UpdateGuestInformationNew(guestInfo, guestInfo.tempOwnerId.ToString(), string.Empty, preferenList);
                    if (status)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "Deleted is guestId");
                        if (preferenList.Count > 0)
                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "EntityId is guestId");
                    }
                }
            }
            else if (guestInfo.IsCopyTo == true)
            {
                status = guestDA.UpdateGuestInformationNew(guestInfo, outReservationId.ToString(), string.Empty, preferenList);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "Deleted is guestId");
                    if (preferenList.Count > 0)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "EntityId is guestId");
                }
            }
            else
            {
                status = guestDA.UpdateGuestInformationNew(guestInfo, "0", string.Empty, preferenList);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "Deleted is guestId");
                    if (preferenList.Count > 0)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestPreferenceMapping.ToString(), guestInfo.GuestId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestPreferenceMapping) + "EntityId is guestId");
                }
            }
            return tmpGuestId;
        }
        [WebMethod(EnableSession = true)]
        public static GuestInformationBO PerformEditActionForGuestDetailByWM(int GuestId)
        {
            GuestInformationBO guestBO = new GuestInformationBO();
            GuestInformationDA guestDA = new GuestInformationDA();
            guestBO = guestDA.GetGuestInformationByGuestIdNew(GuestId);
            if (guestBO.Title == "Mr. & Mrs.")
            {
                guestBO.Title = "MrNMrs.";
            }
            else if (guestBO.Title == "")
            {
                guestBO.Title = "N/A";
            }

            GuestPreferenceDA preferenceDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> preferenceList = new List<GuestPreferenceBO>();
            preferenceList = preferenceDA.GetGuestPreferenceInfoByGuestId(GuestId);

            if (preferenceList.Count > 0)
            {
                foreach (GuestPreferenceBO preference in preferenceList)
                    if (guestBO.GuestPreferences != null)
                    {
                        guestBO.GuestPreferences += ", " + preference.PreferenceName;
                        guestBO.GuestPreferenceId += "," + preference.PreferenceId;
                    }
                    else
                    {
                        guestBO.GuestPreferences = preference.PreferenceName;
                        guestBO.GuestPreferenceId = preference.PreferenceId.ToString();
                    }
            }

            return guestBO;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteActionForGuestDetailByWM(int GuestId)
        {
            RoomReservationDA regDA = new RoomReservationDA();
            Boolean status = regDA.DeleteTempGuestReservation(ReservationId, GuestId);
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResId(ReservationId, true);
            return GetUserDetailHtml(guestInformationList);
        }
        [WebMethod]
        public static string GetTempRegistration()
        {
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResId(ReservationId, true);
            return GetUserDetailHtml(guestInformationList);
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

            //decimal max = promBO.PercentAmount;
            if (promBO.PercentAmount > companyBO.DiscountPercent)
            {
                companyBO.DiscountPercent = promBO.PercentAmount;
            }
            //return max.ToString();

            return companyBO;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveRoomDetailsInformationByWebMethod(bool isEdit, string hfSelectedRoomNumbers, string hfSelectedRoomId, string txtUnitPriceHiddenField, string txtRoomRate, string txtRoomId, string prevRoomTypeId, string ddlRoomTypeId, string ddlRoomTypeIdText, string lblHiddenId, string txtDiscountAmount, string ddlCurrency, string ddlDiscountType, string ddlReservedMode, int paxQuantity)
        {
            if (isEdit)
            {
                List<ReservationDetailBO> deletedReservationDetailListBO = HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] as List<ReservationDetailBO>;

                List<ReservationDetailBO> reservationDetailListBOForGridEdit = HttpContext.Current.Session["ReservationDetailListForGrid"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["ReservationDetailListForGrid"] as List<ReservationDetailBO>;
                ReservationDetailBO singleEntityBOEdit = reservationDetailListBOForGridEdit.Where(x => x.RoomTypeId == Int32.Parse(ddlRoomTypeId)).FirstOrDefault();
                if (reservationDetailListBOForGridEdit.Contains(singleEntityBOEdit))
                {
                    reservationDetailListBOForGridEdit.Remove(singleEntityBOEdit);
                }
                else
                {
                    ReservationDetailBO prevRoomTypeIdBO = reservationDetailListBOForGridEdit.Where(x => x.RoomTypeId == Int32.Parse(prevRoomTypeId)).FirstOrDefault();
                    reservationDetailListBOForGridEdit.Remove(prevRoomTypeIdBO);
                    deletedReservationDetailListBO.Add(prevRoomTypeIdBO);
                    HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] = deletedReservationDetailListBO;
                }

                List<ReservationDetailBO> reservationDetailListBOEddit = HttpContext.Current.Session["ReservationDetailList"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["ReservationDetailList"] as List<ReservationDetailBO>;
                List<ReservationDetailBO> singleDetailEntityBOEditList = reservationDetailListBOEddit.Where(x => x.RoomTypeId == Int32.Parse(ddlRoomTypeId)).ToList();

                List<ReservationDetailBO> deletedReservationDetailListByRoomType = new List<ReservationDetailBO>();
                foreach (ReservationDetailBO row in singleDetailEntityBOEditList)
                {
                    reservationDetailListBOEddit.Remove(row);
                    deletedReservationDetailListByRoomType.Add(row);
                }
                HttpContext.Current.Session["ReservationDetailList"] = reservationDetailListBOEddit;
                HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] = deletedReservationDetailListByRoomType;
            }

            RoomTypeBO typeBO = new RoomTypeBO();
            RoomTypeDA typeDA = new RoomTypeDA();
            typeBO = typeDA.GetRoomTypeInfoById(Int32.Parse(ddlRoomTypeId));
            ddlRoomTypeIdText = typeBO.RoomType;
            string RoomId = hfSelectedRoomId;
            int RoomCount = 1;

            string RoomNumber = string.Empty;
            if (hfSelectedRoomNumbers == "Unassigned")
            {
                RoomNumber = string.Empty;
            }
            else
            {
                RoomNumber = hfSelectedRoomNumbers;
            }
            if (string.IsNullOrEmpty(RoomNumber))
            {
                RoomId = string.Empty;
            }

            int RoomQty = !string.IsNullOrWhiteSpace(txtRoomId) ? Convert.ToInt32(txtRoomId) : 0;
            if (RoomId.Split(',').Length >= RoomQty)
            {
                RoomCount = RoomId.Split(',').Length;
            }
            else
            {
                RoomCount = RoomQty;
            }
            string RoomTypeInfo = string.Empty;

            decimal RoomUnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField) ? Convert.ToDecimal(txtUnitPriceHiddenField) : 0;
            decimal RoomRateAmount = !string.IsNullOrWhiteSpace(txtRoomRate) ? Convert.ToDecimal(txtRoomRate) : RoomUnitPrice;

            List<ReservationDetailBO> reservationDetailListBOForGrid = HttpContext.Current.Session["ReservationDetailListForGrid"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["ReservationDetailListForGrid"] as List<ReservationDetailBO>;
            ReservationDetailBO singleEntityBO = reservationDetailListBOForGrid.Where(x => x.RoomTypeId == Int32.Parse(ddlRoomTypeId)).FirstOrDefault();
            if (reservationDetailListBOForGrid.Contains(singleEntityBO))
            {
                int prevCount = singleEntityBO.RoomQuantity;
                string prevIdList = singleEntityBO.RoomNumberListInfoWithCount;
                singleEntityBO.RoomQuantity = RoomCount + prevCount;
                singleEntityBO.RoomTypeWisePaxQuantity = paxQuantity;
                singleEntityBO.TotalCalculatedAmount = RoomRateAmount * singleEntityBO.RoomQuantity;
                singleEntityBO.DiscountType = ddlDiscountType;
                singleEntityBO.UnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField) ? Convert.ToDecimal(txtUnitPriceHiddenField) : 0;

                if (ddlDiscountType == "Fixed")
                {
                    singleEntityBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                    singleEntityBO.DiscountAmount = singleEntityBO.UnitPrice - RoomRateAmount;
                }
                else
                {
                    decimal percantAmount = 0;
                    if (singleEntityBO.UnitPrice > 0)
                    {
                        percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                    }
                    singleEntityBO.Discount = percantAmount;
                    singleEntityBO.DiscountAmount = percantAmount;
                }

                if (!string.IsNullOrWhiteSpace(RoomNumber))
                {
                    singleEntityBO.RoomNumberIdList = singleEntityBO.RoomNumberIdList + ',' + RoomId;
                }

                if (!string.IsNullOrEmpty(RoomNumber))
                {
                    singleEntityBO.RoomNumber = singleEntityBO.RoomNumber + "," + RoomNumber;
                }

                singleEntityBO.RoomNumberListInfoWithCount = FormatedRoom(singleEntityBO.RoomNumber, singleEntityBO.RoomQuantity);
                for (int i = 0; i < reservationDetailListBOForGrid.Count; i++)
                {
                    if (reservationDetailListBOForGrid[i].RoomTypeId == Int32.Parse(ddlRoomTypeId))
                    {
                        reservationDetailListBOForGrid[i] = singleEntityBO;
                    }
                }

            }
            else
            {
                singleEntityBO = new ReservationDetailBO();
                singleEntityBO.RoomTypeId = Convert.ToInt32(ddlRoomTypeId);
                singleEntityBO.RoomType = ddlRoomTypeIdText;
                singleEntityBO.RoomRate = RoomRateAmount;
                singleEntityBO.RoomQuantity = RoomCount;
                singleEntityBO.RoomTypeWisePaxQuantity = paxQuantity;
                singleEntityBO.TotalCalculatedAmount = RoomRateAmount * RoomCount;
                singleEntityBO.RoomNumberIdList = RoomId;
                singleEntityBO.RoomNumber = RoomNumber;
                singleEntityBO.RoomNumberList = !string.IsNullOrWhiteSpace(RoomNumber) ? RoomNumber : "Unassigned";
                singleEntityBO.RoomNumberListInfoWithCount = RoomCount + "(" + singleEntityBO.RoomNumberList + ")";
                singleEntityBO.DiscountType = ddlDiscountType;
                singleEntityBO.UnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField) ? Convert.ToDecimal(txtUnitPriceHiddenField) : 0;

                if (ddlDiscountType == "Fixed")
                {
                    singleEntityBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                    singleEntityBO.DiscountAmount = singleEntityBO.UnitPrice - RoomRateAmount;
                }
                else
                {
                    decimal percantAmount = 0;
                    if (singleEntityBO.UnitPrice > 0)
                    {
                        percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                    }

                    singleEntityBO.Discount = percantAmount;
                    singleEntityBO.DiscountAmount = percantAmount;
                }
                reservationDetailListBOForGrid.Add(singleEntityBO);
            }

            HttpContext.Current.Session["ReservationDetailListForGrid"] = reservationDetailListBOForGrid;

            string[] Rooms = RoomNumber.Split(',');
            string[] Id = RoomId.Split(',');

            int dynamicDetailId = 0;
            if (!string.IsNullOrWhiteSpace(lblHiddenId))
                dynamicDetailId = Convert.ToInt32(lblHiddenId);

            List<ReservationDetailBO> reservationDetailListBOAddedList = new List<ReservationDetailBO>();
            List<ReservationDetailBO> reservationDetailListBO = HttpContext.Current.Session["ReservationDetailList"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["ReservationDetailList"] as List<ReservationDetailBO>;

            if (Rooms[0].Length == 0)
            {
                int roomQuantity;

                if (!Int32.TryParse(txtRoomId, out roomQuantity))
                {
                    //isMessageBoxEnable = 1;
                    //lblMessage.Text = "Room Quantity is not in correct format.";
                    //txtRoomId.Focus();
                    //return;
                }

                for (int i = 0; i < roomQuantity; i++)
                {
                    ReservationDetailBO detailBO = new ReservationDetailBO();
                    detailBO.RoomTypeId = Convert.ToInt32(ddlRoomTypeId);
                    detailBO.RoomType = ddlRoomTypeIdText;
                    detailBO.RoomId = 0;
                    detailBO.UnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField) ? Convert.ToDecimal(txtUnitPriceHiddenField) : 0;
                    detailBO.RoomRate = !string.IsNullOrWhiteSpace(txtRoomRate) ? Convert.ToDecimal(txtRoomRate) : detailBO.UnitPrice;

                    if (ddlDiscountType == "Fixed")
                    {
                        detailBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                        detailBO.DiscountAmount = singleEntityBO.UnitPrice - RoomRateAmount;
                    }
                    else
                    {
                        decimal percantAmount = 0;
                        if (singleEntityBO.UnitPrice > 0)
                        {
                            percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                        }
                        detailBO.Discount = percantAmount;
                        detailBO.DiscountAmount = percantAmount;
                    }

                    detailBO.CurrencyType = Convert.ToInt32(ddlCurrency);
                    detailBO.DiscountType = ddlDiscountType;
                    detailBO.RoomNumber = "Unassigned";
                    detailBO.ReservationDetailId = 0;//dynamicDetailId == 0 ? reservationDetailListBO.Count + 1 : dynamicDetailId;
                    reservationDetailListBO.Add(detailBO);
                }
            }
            else
            {
                for (int i = 0; i < Id.Length; i++)
                {
                    ReservationDetailBO detailBO = dynamicDetailId == 0 ? new ReservationDetailBO() : reservationDetailListBO.Where(x => x.ReservationDetailId == dynamicDetailId).FirstOrDefault();
                    if (reservationDetailListBO.Contains(detailBO))
                        reservationDetailListBO.Remove(detailBO);

                    detailBO.RoomTypeId = Convert.ToInt32(ddlRoomTypeId);
                    detailBO.RoomType = ddlRoomTypeId;
                    detailBO.RoomId = Int32.Parse(Id[i]);
                    detailBO.RoomNumber = Rooms[i];
                    detailBO.UnitPrice = Convert.ToDecimal(txtUnitPriceHiddenField);
                    detailBO.RoomRate = !string.IsNullOrWhiteSpace(txtRoomRate) ? Convert.ToDecimal(txtRoomRate) : detailBO.UnitPrice;

                    if (ddlDiscountType == "Fixed")
                    {
                        detailBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                        detailBO.DiscountAmount = singleEntityBO.UnitPrice - RoomRateAmount;
                    }
                    else
                    {
                        decimal percantAmount = 0;
                        if (singleEntityBO.UnitPrice > 0)
                        {
                            percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                        }
                        detailBO.Discount = percantAmount;
                        detailBO.DiscountAmount = percantAmount;
                    }

                    detailBO.CurrencyType = Convert.ToInt32(ddlCurrency);
                    detailBO.DiscountType = ddlDiscountType;

                    detailBO.ReservationDetailId = dynamicDetailId == 0 ? reservationDetailListBO.Count + 1 : dynamicDetailId;
                    reservationDetailListBO.Add(detailBO);
                }
            }

            HttpContext.Current.Session["ReservationDetailList"] = reservationDetailListBO;
            return LoadDetailGridViewByWM();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteByWebMethod(int roomTypeId)
        {
            int RoomTypeId = (roomTypeId);
            var reservationDetailBO = (List<ReservationDetailBO>)HttpContext.Current.Session["ReservationDetailListForGrid"];
            var reservationDetail = reservationDetailBO.Where(x => x.RoomTypeId == RoomTypeId).FirstOrDefault();
            reservationDetailBO.Remove(reservationDetail);
            HttpContext.Current.Session["ReservationDetailListForGrid"] = reservationDetailBO;

            List<ReservationDetailBO> deletedReservationDetailListBO = HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] as List<ReservationDetailBO>;
            deletedReservationDetailListBO.Add(reservationDetail);
            HttpContext.Current.Session["DeletedReservationDetailListByRoomType"] = deletedReservationDetailListBO;

            List<ReservationDetailBO> reservationDetailListBOEddit = HttpContext.Current.Session["ReservationDetailList"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["ReservationDetailList"] as List<ReservationDetailBO>;
            List<ReservationDetailBO> singleDetailEntityBOEditList = reservationDetailListBOEddit.Where(x => x.RoomTypeId == RoomTypeId).ToList();
            foreach (ReservationDetailBO row in singleDetailEntityBOEditList)
            {
                reservationDetailListBOEddit.Remove(row);
            }
            HttpContext.Current.Session["ReservationDetailList"] = reservationDetailListBOEddit;

            return LoadDetailGridViewByWM();
        }
        [WebMethod(EnableSession = true)]
        public static ReservationDetailBO PerformReservationDetailEditByWebMethod(int roomTypeId)
        {
            List<ReservationDetailBO> reservationDetailListBOForGrid = HttpContext.Current.Session["ReservationDetailListForGrid"] == null ? new List<ReservationDetailBO>() : HttpContext.Current.Session["ReservationDetailListForGrid"] as List<ReservationDetailBO>;
            ReservationDetailBO singleEntityBO = reservationDetailListBOForGrid.Where(x => x.RoomTypeId == roomTypeId).FirstOrDefault();
            if (singleEntityBO != null)
            {
                string[] dataArray = singleEntityBO.RoomNumberListInfoWithCount.Split('(');
                singleEntityBO.RoomQuantity = Convert.ToInt32(dataArray[0]);
            }
            return singleEntityBO;

        }
        [WebMethod]
        public static string GetRoomDetailGridInformationByWM()
        {
            return LoadDetailGridViewByWM();
        }
        [WebMethod]
        public static string GetGuestRegistrationHistoryGuestId(int GuestId)
        {
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> registrationList = new List<RoomRegistrationBO>();
            registrationList = registrationDA.GetGuestRegistrationHistoryByGuestId(GuestId);

            string strTable = "";
            strTable += "<table  width='100%' class='table table-bordered table-condensed table-responsive' id='TableGuestHistory'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";


            strTable += "<th align='center' scope='col'>Registration Number</th><th align='left' scope='col'>Arrival Date</th> <th align='left' scope='col'>Checkout Date</th></tr>";
            int counter = 0;
            foreach (RoomRegistrationBO dr in registrationList)
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
                strTable += "<td align='left' style='width: 33%'>" + dr.RegistrationNumber + "</td>";
                strTable += "<td align='left' style='width: 33%'>" + dr.ArriveDate.ToString("MM/dd/yy") + "</td>";
                if (dr.CheckOutDate != DateTime.MinValue)
                {
                    strTable += "<td align='left' style='width: 33%'>" + dr.CheckOutDate.ToString("MM/dd/yy") + "</td>";
                }
                else
                {
                    strTable += "<td align='left' style='width: 33%'>" + "Not CheckOut Yet. " + "</td>";
                }
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
        public static ReturnInfo DeleteReservationRecord(int pkId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            try
            {
                bool status = false;
                RoomReservationDA roomReservationDA = new RoomReservationDA();
                status = roomReservationDA.DeleteReservationDetailInfoById(pkId);

                if (status == true)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomReservationDetails.ToString(), pkId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationDetails));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static PaidServiceViewBO GetPaidServiceDetails(int reservationId, int currencyId, string currencyType, string convertionRate)
        {
            HotelGuestServiceInfoDA serviceDa = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> paidServiceLst = new List<HotelGuestServiceInfoBO>();
            List<RegistrationServiceInfoBO> registrationPaidService = new List<RegistrationServiceInfoBO>();

            paidServiceLst = serviceDa.GetHotelGuestServiceInfo(0, 1, 1);

            if (reservationId != 0)
            {
                registrationPaidService = serviceDa.GetReservationServiceInfoByReservationId(reservationId, currencyType);

                if (registrationPaidService.Count > 0)
                {
                    var crType = registrationPaidService.Where(c => c.CurrencyType != 0).FirstOrDefault();

                    if (crType.CurrencyType != Convert.ToInt32(currencyId))
                    {
                        if (currencyType != "Local")
                        {
                            registrationPaidService = (from d in registrationPaidService
                                                       from p in paidServiceLst
                                                       where d.ServiceId == p.ServiceId
                                                       select new RegistrationServiceInfoBO
                                                       {
                                                           DetailServiceId = d.DetailServiceId,
                                                           ReservationId = d.ReservationId,
                                                           ServiceId = d.ServiceId,
                                                           ServiceName = d.ServiceName,
                                                           ServiceRate = d.ServiceRate,
                                                           UnitPrice = p.UnitPriceUsd,
                                                           ConversionRate = d.ConversionRate,
                                                           IsAchieved = d.IsAchieved

                                                       }).ToList();
                        }
                        else
                        {
                            registrationPaidService = (from d in registrationPaidService
                                                       from p in paidServiceLst
                                                       where d.ServiceId == p.ServiceId
                                                       select new RegistrationServiceInfoBO
                                                       {
                                                           DetailServiceId = d.DetailServiceId,
                                                           ReservationId = d.ReservationId,
                                                           ServiceId = d.ServiceId,
                                                           ServiceName = d.ServiceName,
                                                           ServiceRate = d.ServiceRate,
                                                           UnitPrice = p.UnitPriceLocal,
                                                           ConversionRate = d.ConversionRate,
                                                           IsAchieved = d.IsAchieved

                                                       }).ToList();
                        }
                    }
                    else if (crType.CurrencyType == Convert.ToInt32(currencyId))
                    {
                    }
                }
            }

            PaidServiceViewBO viewBo = new PaidServiceViewBO();
            viewBo.PaidService = paidServiceLst;
            viewBo.RegistrationPaidService = registrationPaidService;
            return viewBo;
        }
        [WebMethod]
        public static string GetNationality(int countryId)
        {
            CountriesBO country = new CountriesBO();
            try
            {
                HMCommonDA commonDa = new HMCommonDA();
                country = commonDa.GetCountriesById(countryId);

            }
            catch (Exception ex)
            {
                country.Nationality = string.Empty;

            }
            return country.Nationality;
        }
        [WebMethod(EnableSession = true)]// Room availability check
        public static string PerformGetRoomAvailableChecking(Int64 reservationId, string RoomTypeId, string fromDate, string toDate, string roomQuantityEntered)
        {
            int availableRoomQty = -100;
            HMUtility hmUtility = new HMUtility();
            RoomNumberBO roomBO = new RoomNumberBO();
            RoomNumberDA roomDA = new RoomNumberDA();

            DateTime dateTime = DateTime.Now;
            DateTime startDate = dateTime;
            DateTime endDate = dateTime;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                startDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = hmUtility.GetStringFromDateTime(dateTime);
                startDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                endDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(-1);
            }
            else
            {
                toDate = hmUtility.GetStringFromDateTime(dateTime);
                endDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1);
            }

            List<RoomAvailableStatusBO> avalibleRoomList = new List<RoomAvailableStatusBO>();
            HMCommonDA hmCommonDA = new HMCommonDA();

            availableRoomQty = hmCommonDA.GetRoomAvailableQuantity(reservationId, startDate, endDate, RoomTypeId, roomQuantityEntered);
            return availableRoomQty.ToString();
        }
        [WebMethod]
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
        [WebMethod]
        public static HMCommonSetupBO LoadReservationNRegistrationXtraValidation()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("ReservationNRegistrationXtraValidation", "ReservationNRegistrationXtraValidation");
            return commonSetupBO;
        }
        [WebMethod]
        public static List<GuestInformationBO> LoadMultipleGuest(string reservationId)
        {
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResId(Convert.ToInt32(reservationId), true);
            return guestInformationList;
        }
        [WebMethod]
        public static List<RoomNumberBO> LoadRoomNumbers(int reservationId)
        {
            List<RoomNumberBO> roomList = new List<RoomNumberBO>();
            RoomNumberDA roomDA = new RoomNumberDA();
            roomList = roomDA.GetRoomNumberInfoByReservationId(reservationId);
            return roomList;
        }
        [WebMethod]
        public static string SearchNLoadReservationInfo(string guestName, int countryId, string phn, string email, string passportNumber, string nationalId, int companyId)
        {
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> guestInfoList = new List<GuestInformationBO>();
            guestInfoList = guestDA.GetPrevGuestInfoForReservation(guestName, countryId, phn, email, passportNumber, nationalId, companyId);
            return commonDA.GetPrevGuestForReservation(guestInfoList);
        }
        [WebMethod]
        public static GuestInformationBO LoadPrevGuestInfo(int guestId)
        {
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guestInfo = new GuestInformationBO();
            guestInfo = guestDA.GetGuestInformationByGuestIdNew(guestId);
            GuestPreferenceDA preferenceDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> preferenceList = new List<GuestPreferenceBO>();
            if (guestId > 0)
            {
                preferenceList = preferenceDA.GetGuestPreferenceInfoByGuestId(guestId);

                if (preferenceList.Count > 0)
                {
                    foreach (GuestPreferenceBO preference in preferenceList)
                        if (guestInfo.GuestPreferences != null)
                        {
                            guestInfo.GuestPreferences += ", " + preference.PreferenceName;
                            guestInfo.GuestPreferenceId += "," + preference.PreferenceId;
                        }
                        else
                        {
                            guestInfo.GuestPreferences = preference.PreferenceName;
                            guestInfo.GuestPreferenceId = preference.PreferenceId.ToString();
                        }
                }
            }

            return guestInfo;
        }
        [WebMethod]
        public static string MultipleAireportPickupDropInfo(int reservationId)
        {
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> guestInfoList = new List<GuestInformationBO>();

            guestInfoList = guestDA.GetMultipleAireportPickupDropInfo(reservationId);

            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableRoomInformation' >";
            strTable += "<thead style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Action</th></thead>";
            strTable += "<tbody>";

            int counter = 0;
            foreach (GuestInformationBO dr in guestInfoList)
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

                strTable += "<td align='left' style='width: 138px'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 82px'>";
                strTable += "&nbsp;<img src='../Images/ReportDocument.png' onClick='javascript:return ShowReport(" + reservationId + "," + dr.APDId + ")' alt='Show Information' border='0' />";
                strTable += "</td>";
            }

            strTable += "</tbody></table>";

            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        [WebMethod]
        public static string LoadGuestReferenceInfo()
        {
            List<GuestPreferenceBO> guestReferenceList = new List<GuestPreferenceBO>();
            GuestPreferenceDA guestReferenceDA = new GuestPreferenceDA();

            string HTML = string.Empty;
            guestReferenceList = guestReferenceDA.GetActiveGuestPreferenceInfo();
            HTML = GetHTMLGuestReferenceGridView(guestReferenceList);

            return HTML;
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
        public static RoomTypeBO PerformFillFormActionByTypeId(int EditId)
        {
            RoomTypeBO roomTypeBO = new RoomTypeBO();
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            roomTypeBO = roomTypeDA.GetRoomTypeInfoById(EditId);
            return roomTypeBO;
        }
        [WebMethod]
        public static ReturnInfo CancelOrActiveReservation(int reservationId, string reason, string mode)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            RoomReservationDA reservationDA = new RoomReservationDA();
            var result = reservationDA.GetRoomReservationInfoById(reservationId);
            RoomReservationBO reservationBO = new RoomReservationBO() { ReservationId = reservationId, ReservationMode = mode, Reason = reason };
            try
            {
                returnInfo.IsSuccess = reservationDA.CancelOrActiveRoomReservation(reservationBO);
                if (returnInfo.IsSuccess)
                {
                    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsRoomReservationEmailAutoPostingEnable");
                    string IsEmailEnable = commonSetupBO.SetupValue;
                    if (IsEmailEnable == "1")
                    {
                        CancelMail(reservationId);
                    }

                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SMSAutoPosting", "IsRoomReservationSMSAutoPostingEnable");
                    string IsSMSEnable = commonSetupBO.SetupValue;
                    if (IsSMSEnable == "1")
                    {
                        SendSMSbySSLGateway(result, result.ReservationNumber, reservationId);
                    }

                    if (reason == "")
                    {
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.ReservationActive, AlertType.Success);
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservation.ToString(), reservationBO.ReservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation) + " Active.");
                    }
                    else
                    {
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.ReservationCancel, AlertType.Success);
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservation.ToString(), reservationBO.ReservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation) + " Cancel.");
                        //CancelMail(reservationId);
                        //SendSMSbySSLGateway(result, result.ReservationNumber, reservationId);
                    }
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnInfo;
        }
        [WebMethod]
        public static bool IsCanCancelReservation(int reservationId)
        {
            RoomReservationDA reservationDA = new RoomReservationDA();

            try
            {
                return reservationDA.CheckReservationCanBeCancel(reservationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [WebMethod]
        public static List<ContactInformationBO> GetContactInformationByCompanyIdNSearchText(int companyId, string searchText)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();
            ContactInformationDA contactInformationDA = new ContactInformationDA();
            contactInformationList = contactInformationDA.GetContactInformationByCompanyIdNSearchTextForAutoComplete(companyId, searchText);
            return contactInformationList;
        }



        [WebMethod]
        public static string OnRetrieveReservationXML()
        {
            return RetrieveReservationReadRequestFromServer(RetrieveRservationReadRequest.ReservationStatus.All);

        }
        public static string RetrieveReservationReadRequestFromServer(RetrieveRservationReadRequest.ReservationStatus reservationStatus)
        {
            XmlNode OTA_Hotel_Tag = null;
            string pingNs = string.Empty;
            SiteMinderHeader header = new SiteMinderHeader();
            XmlDocument soapXMLDoc = header.XmlHeader(out OTA_Hotel_Tag, "OTA_ReadRQ");

            //Build the XML OTA Child Tag Here, POS tag is included.
            XmlNode ReadRequests = OTA_Hotel_Tag.AppendChild(soapXMLDoc.CreateElement("", "ReadRequests", ""));

            XmlNode HotelReadREquest = ReadRequests.AppendChild(soapXMLDoc.CreateElement("", "HotelReadRequest", ""));
            HotelReadREquest.Attributes.Append(soapXMLDoc.CreateAttribute("HotelCode")).Value = "TECHNOSENSEPMS123";

            XmlNode SelectionCriteria = HotelReadREquest.AppendChild(soapXMLDoc.CreateElement("", "SelectionCriteria", ""));
            SelectionCriteria.Attributes.Append(soapXMLDoc.CreateAttribute("SelectionType")).Value = RetrieveRservationReadRequest.SelectionType;

            if (reservationStatus != RetrieveRservationReadRequest.ReservationStatus.All)
            {
                SelectionCriteria.Attributes.Append(soapXMLDoc.CreateAttribute("ResStatus")).Value = reservationStatus.ToString();
            }

            string xml_document = soapXMLDoc.OuterXml;

            string response = header.postXMLData(xml_document);

            //string xmlData = File.ReadAllText("Reservation_with_2_Rooms_of_Same_Room_Type.xml");
            //string xmlData = File.ReadAllText("Reservation_with_Different_Multiple_Room_Types_Booked.xml");
            //string xmlData = File.ReadAllText("Reservations_with_Room_Type_Booked_For_Split_Dates.xml");

            Envelope env = ParseXmlResponse(response);
            //Envelope env = ParseXmlResponse(xmlData);

            string reservationDate = GetReservationInformation(env, "ReservationDate");
            string reservationFromDate = GetReservationInformation(env, "ReservationFromDate");
            string reservationToDate = GetReservationInformation(env, "ReservationToDate");
            string guestName = GetReservationInformation(env, "GuestName");
            string roomTypeCode = GetReservationInformation(env, "RoomTypeCode");
            string roomTypeRate = GetReservationInformation(env, "RoomTypeRate");

            return response;
        }

        public static Envelope ParseXmlResponse(string xmlResponse)
        {
            Envelope envelope = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Envelope));

            using (StringReader reader = new StringReader(xmlResponse))
            {
                envelope = (Envelope)serializer.Deserialize(new IgnoreNamespaceXmlTextReader(reader));
            }

            return envelope;
        }

        public static string GetReservationInformation(Envelope env, string requestType)
        {
            string returnData = string.Empty;
            Body envBody = env.Body;
            OTAResRetrieveRS rs = envBody.OTAResRetrieveRS;
            ReservationsList revList = rs.ReservationsList;

            if (rs.ReservationsList != null)
            {
                if (requestType == "ReservationDate")
                {
                    //Done
                    returnData = revList.HotelReservation.CreateDateTime.ToString();
                }
                else if (requestType == "ReservationFromDate")
                {
                    //Done
                    returnData = revList.HotelReservation.RoomStays.RoomStay.RatePlans.RatePlan.EffectiveDate.ToString();
                }
                else if (requestType == "ReservationToDate")
                {
                    //Done
                    returnData = revList.HotelReservation.RoomStays.RoomStay.RatePlans.RatePlan.ExpireDate.ToString();
                }
                else if (requestType == "GuestName")
                {
                    returnData = revList.HotelReservation.ResGuests.ResGuest.Profiles.ProfileInfo.Profile.Customer.PersonName.GivenName.ToString() + " " + revList.HotelReservation.ResGuests.ResGuest.Profiles.ProfileInfo.Profile.Customer.PersonName.Surname.ToString();
                }
                else if (requestType == "RoomTypeCode")
                {
                    //Done
                    returnData = revList.HotelReservation.RoomStays.RoomStay.RoomTypes.RoomType.RoomTypeCode;
                }
                else if (requestType == "RoomTypeRate")
                {
                    //Done
                    returnData = revList.HotelReservation.RoomStays.RoomStay.RoomRates.RoomRate.Rates.Rate.Total.AmountAfterTax.ToString();
                }
            }
            return returnData;
        }
    }
}