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

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGuestPaymentTransfer : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadLocalCurrencyId();
                this.pnlBillTransferedInfo.Visible = false;
                this.pnlBillTransferForReservation.Visible = false;
                this.CheckObjectPermission();
                this.lblRegistrationNumberDiv.Visible = false;
                this.ddlRegistrationId.Visible = false;
            }
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            SearchRoomInformation(this.txtSrcRoomNumber.Text);
        }
        protected void btnSrcReservationNumber_Click(object sender, EventArgs e)
        {
            SearchReservationInformation(this.txtSrcReservationNumber.Text);
        }
        protected void gvGHServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int registrationId = Convert.ToInt32(this.hfddlRegistrationId.Value);
            this.lblMessage.Text = string.Empty;
            this.gvGHServiceBill.PageIndex = e.NewPageIndex;
            this.LoadGridView(registrationId);
        }
        protected void gvReservationInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvReservationInfo.PageIndex = e.NewPageIndex;
            this.LoadGridViewByReservationNumber(this.txtSrcReservationNumber.Text);
        }
        protected void btnBillTransfer_Click(object sender, EventArgs e)
        {
            List<GuestBillPaymentBO> guestBillPaymentBOList = new List<GuestBillPaymentBO>();
            foreach (GridViewRow grRow in gvGHServiceBill.Rows)
            {
                CheckBox chkItem = (CheckBox)grRow.FindControl("chkBox");
                string id = ((Label)grRow.FindControl("lblid")).Text.ToString();
                //string moduleName = ((Label)grRow.FindControl("lblModuleName")).Text.ToString();
                string transferAmount = ((TextBox)grRow.FindControl("txtTransferAmount")).Text.ToString();
                if (chkItem.Checked)
                {
                    if (!string.IsNullOrWhiteSpace(transferAmount))
                    {
                        //UserInformationBO userInformationBO = new UserInformationBO();
                        //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
                        
                        guestBillPaymentBO.PaymentId = Convert.ToInt32(id);
                        guestBillPaymentBO.PaymentAmount = Convert.ToDecimal(transferAmount);
                        guestBillPaymentBO.Remarks = this.txtRemarks.Text;
                        guestBillPaymentBOList.Add(guestBillPaymentBO);
                    }
                }
            }

            if (guestBillPaymentBOList != null)
            {
                if (guestBillPaymentBOList.Count > 0)
                {
                    if (this.ddlRoomId.SelectedValue == "0")
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfered Room Information.", AlertType.Warning);
                        this.ddlRoomId.Focus();
                        return;
                    }

                    // Firstly Check Transfered Room Already Checked Out or Not..............
                    if (!IsFrmValid())
                    {
                        return;
                    }

                    int transferRegistrationId = 0;
                    int fromRegistrationId = 0;

                    string transferRoomNumber = string.Empty;
                    string fromRoomNumber = string.Empty;

                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.ddlRoomId.SelectedItem.Text);

                    if (roomAllocationBO.RoomId > 0)
                    {
                        transferRoomNumber = roomAllocationBO.RoomNumber;
                        transferRegistrationId = roomAllocationBO.RegistrationId;
                        roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
                        fromRegistrationId = roomAllocationBO.RegistrationId;
                        fromRoomNumber = roomAllocationBO.RoomNumber;

                        //-----------------------Transferd Information for Save..................
                        GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();
                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        Boolean status = guestBillPaymentDA.TransferGuestBillPaymentInfo(guestBillPaymentBOList, fromRegistrationId, transferRegistrationId, userInformationBO.UserInfoId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Transfer Operation Successfull.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity("Guest Payment Transfer", EntityTypeEnum.EntityType.GuestBillPaymentTransfer.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Guest Payment Transferred (Room# " + fromRoomNumber + " => " + transferRoomNumber + ") and Description: " + txtRemarks.Text);
                            this.txtSrcRoomNumber.Focus();
                            ClearForm();
                        }
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfer Room Information.", AlertType.Warning);
                        this.ddlRoomId.Focus();
                        return;
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Guest Payment Information.", AlertType.Warning);
                    this.gvGHServiceBill.Focus();
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Guest Payment Information.", AlertType.Warning);
                this.gvGHServiceBill.Focus();
            }
        }
        protected void btnReservationBillTransfer_Click(object sender, EventArgs e)
        {
            List<RoomReservationBO> roomReservationPaymentTransferList = new List<RoomReservationBO>();
            foreach(GridViewRow gvRow in gvReservationInfo.Rows)
            {
                CheckBox chkItem = (CheckBox)gvRow.FindControl("chkBoxReservation");
                string id = ((Label)gvRow.FindControl("lblidReservation")).Text.ToString();
                string transferAmount = ((TextBox)gvRow.FindControl("txtTransferAmountReservation")).Text.ToString();
                if (transferAmount == "" && chkItem.Checked == true)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfered Amount.", AlertType.Warning);
                    return;
                }
                double ta;
                if(transferAmount == "")
                {
                    ta = 0;
                }
                else
                {
                    ta = Convert.ToDouble(transferAmount);
                }
                string highestAmount = gvRow.Cells[6].Text;
                
                double ha = Convert.ToDouble(highestAmount);
                if (ta > ha)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfered Amount less than Payment Amount.", AlertType.Warning);
                    this.ddlReservationId.Focus();
                    return;
                }
                if (chkItem.Checked)
                {
                    if (!string.IsNullOrWhiteSpace(transferAmount))
                    {
                        RoomReservationBO roomReservationPaymentBO = new RoomReservationBO();

                        roomReservationPaymentBO.PaymentId = Convert.ToInt32(id);
                        roomReservationPaymentBO.PaymentAmount = Convert.ToDecimal(transferAmount);
                        roomReservationPaymentBO.Remarks = this.txtTransferReservationRemarks.Text;
                        roomReservationPaymentTransferList.Add(roomReservationPaymentBO);
                    }
                }
            }
            if (roomReservationPaymentTransferList != null)
            {
                if (roomReservationPaymentTransferList.Count > 0)
                {
                    if (this.ddlReservationId.SelectedValue == "0")
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfered Reservation Number.", AlertType.Warning);
                        this.ddlReservationId.Focus();
                        return;
                    }

                    if (this.txtTransferReservationRemarks.Text == "")
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfered Reservation Description.", AlertType.Warning);
                        this.txtTransferReservationRemarks.Focus();
                        return;
                    }

                    // Firstly Check Transfered Room Already Checked Out or Not..............
                    //if (!IsFrmValid())
                    //{
                    //    return;
                    //}

                    int transferReservationId = 0;
                    int fromReservationId = 0;

                    string transferReservationNumber = string.Empty;
                    string fromReservationNumber = string.Empty;

                    RoomReservationDA roomReservationDa = new RoomReservationDA();
                    RoomReservationBO roomReservationBO = new RoomReservationBO();
                    roomReservationBO = roomReservationDa.GetRoomReservationInfoByReservationNumber(this.ddlReservationId.SelectedItem.Text);

                    if (roomReservationBO.ReservationId > 0)
                    {
                        transferReservationNumber = roomReservationBO.ReservationNumber;
                        transferReservationId = roomReservationBO.ReservationId;
                        roomReservationBO = roomReservationDa.GetRoomReservationInfoByReservationNumber(this.txtSrcReservationNumber.Text);
                        fromReservationId = roomReservationBO.ReservationId;
                        fromReservationNumber = roomReservationBO.ReservationNumber;

                        //-----------------------Transferd Information for Save..................
                        RoomReservationDA roomReservationPaymentDa = new RoomReservationDA();
                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        Boolean status = roomReservationPaymentDa.TransferReservationBillPaymentInfo(roomReservationPaymentTransferList, fromReservationId, transferReservationId, userInformationBO.UserInfoId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Transfer Operation Successfull.", AlertType.Success);
                //            Boolean logStatus = hmUtility.CreateActivityLogEntity("Guest Payment Transfer", EntityTypeEnum.EntityType.GuestBillPaymentTransfer.ToString(), 0,
                //ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Guest Payment Transferred (Room# " + fromReservationNumber + " => " + transferReservationNumber + ") and Description: " + txtTransferReservationRemarks.Text);
                            this.txtSrcRoomNumber.Focus();
                            ClearReservationForm();
                        }
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfer Reservation Information.", AlertType.Warning);
                        this.ddlReservationId.Focus();
                        return;
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Guest Payment Information.", AlertType.Warning);
                    this.gvReservationInfo.Focus();
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Guest Payment Information.", AlertType.Warning);
                this.gvReservationInfo.Focus();
            }
        }
        //************************ User Defined Function ********************//
        private void ClearForm()
        {
            txtSrcRoomNumber.Text = string.Empty;
            hfddlRegistrationId.Value = "0";
            RoomIdForCurrentBillHiddenField.Value = "0";
            DateTime dateTime = DateTime.Now;
            this.txtGuestNameInfo.Text = string.Empty;
            this.txtRoomTypeInfo.Text = string.Empty;
            this.lblRegistrationNumberDiv.Visible = false;
            this.ddlRegistrationId.Visible = false;
            this.ddlCurrencyHiddenField.Value = hfLocalCurrencyId.Value;
            this.txtCheckInDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
            this.txtDepartureDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
            this.ddlCompanyNameHiddenField.Value = "0";
            this.ddlBusinessPromotionIdHiddenField.Value = "0";
            this.pnlBillTransferedInfo.Visible = false;
            this.pnlBillTransferForReservation.Visible = false;
            gvGHServiceBill.DataSource = null;
            gvGHServiceBill.DataBind();
            this.txtSrcRoomNumber.Focus();
        }
        private void ClearReservationForm()
        {
            this.ddlTransferType.SelectedValue = "0";
            txtSrcReservationNumber.Text = string.Empty;
            DateTime dateTime = DateTime.Now;
            this.txtReservedGuestName.Text = string.Empty;
            this.txtArrivalDate.Text = string.Empty;
            this.txtExpectedDepartureDate.Text = string.Empty;
            this.pnlBillTransferForReservation.Visible = false;
            this.GridInformationReservation.Visible = false;
            gvReservationInfo.DataSource = null;
            gvReservationInfo.DataBind();
            this.txtSrcReservationNumber.Focus();
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void CheckObjectPermission()
        {
            btnBillTransfer.Visible = isSavePermission;
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
                    if (roomAllocationBO.IsStopChargePosting)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Bill Locked for the Room Number: " + RoomNumber, AlertType.Warning);
                        return;
                    }

                    this.hfddlRegistrationId.Value = roomAllocationBO.RegistrationId.ToString();
                    this.RoomIdForCurrentBillHiddenField.Value = roomAllocationBO.RoomId.ToString();
                    this.txtGuestNameInfo.Text = roomAllocationBO.GuestName;
                    this.txtRoomTypeInfo.Text = roomAllocationBO.RoomType;
                    this.lblRegistrationNumberDiv.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.ddlCurrencyHiddenField.Value = roomAllocationBO.CurrencyType.ToString();
                    this.txtCheckInDateHiddenField.Value = hmUtility.GetStringFromDateTime(roomAllocationBO.ArriveDate);
                    this.txtDepartureDateHiddenField.Value = hmUtility.GetStringFromDateTime(roomAllocationBO.ExpectedCheckOutDate);
                    this.ddlCompanyNameHiddenField.Value = roomAllocationBO.CompanyId.ToString();
                    this.ddlBusinessPromotionIdHiddenField.Value = roomAllocationBO.BusinessPromotionId.ToString();
                    this.LoadRegistrationNumber(roomAllocationBO.RoomId);
                    this.LoadGridView(roomAllocationBO.RegistrationId);

                }
                else
                {
                    hfddlRegistrationId.Value = "0";
                    RoomIdForCurrentBillHiddenField.Value = "0";
                    DateTime dateTime = DateTime.Now;
                    this.txtGuestNameInfo.Text = string.Empty;
                    this.txtRoomTypeInfo.Text = string.Empty;
                    this.lblRegistrationNumberDiv.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                    this.ddlCurrencyHiddenField.Value = hfLocalCurrencyId.Value;
                    this.txtCheckInDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.txtDepartureDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.ddlCompanyNameHiddenField.Value = "0";
                    this.ddlBusinessPromotionIdHiddenField.Value = "0";
                    this.pnlBillTransferedInfo.Visible = false;
                    gvGHServiceBill.DataSource = null;
                    gvGHServiceBill.DataBind();
                    this.txtSrcRoomNumber.Focus();
                }
            }
            else
            {
                this.txtGuestNameInfo.Text = string.Empty;
                this.txtRoomTypeInfo.Text = string.Empty;
                this.lblRegistrationNumberDiv.Visible = false;
                this.ddlRegistrationId.Visible = false;
                this.pnlBillTransferedInfo.Visible = false;
                gvGHServiceBill.DataSource = null;
                gvGHServiceBill.DataBind();
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
            }
        }
        private void SearchReservationInformation(string ReservationNo)
        {
            if (!string.IsNullOrWhiteSpace(ReservationNo))
            {
                RoomReservationBO reservationBO = new RoomReservationBO();
                RoomReservationDA rrDa = new RoomReservationDA();
                reservationBO = rrDa.GetOnlineRoomReservationInfoByReservationNumber(ReservationNo);

                this.GridInformationReservation.Visible = true;
                this.txtReservedGuestName.Text = reservationBO.GuestName;
                this.txtArrivalDate.Text = reservationBO.ArrivalTime.ToShortDateString();
                this.txtExpectedDepartureDate.Text = reservationBO.DepartureTime.ToShortDateString();
                this.LoadGridViewByReservationNumber(reservationBO.ReservationNumber);
            }
            else
            {
                this.txtReservedGuestName.Text = string.Empty;
                this.txtArrivalDate.Text = string.Empty;
                this.txtExpectedDepartureDate.Text = string.Empty;
                this.GridInformationReservation.Visible = false;
                this.pnlBillTransferForReservation.Visible = false;
                gvReservationInfo.DataSource = null;
                gvReservationInfo.DataBind();
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Reservation Number.", AlertType.Warning);
                this.txtSrcReservationNumber.Focus();
            }
        }
        private void LoadRegistrationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            this.ddlRegistrationId.DataSource = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            this.ddlRegistrationId.DataTextField = "RegistrationNumber";
            this.ddlRegistrationId.DataValueField = "RegistrationId";
            this.ddlRegistrationId.DataBind();
        }
        private void LoadGridViewByReservationNumber(string reservationNumber)
        {
            List<RoomReservationBO> reservationBO = new List<RoomReservationBO>();
            RoomReservationDA reservationDa = new RoomReservationDA();
            reservationBO = reservationDa.GetOnlineRoomReservationListByReservationNumber(reservationNumber);

            this.gvReservationInfo.DataSource = reservationBO;
            this.gvReservationInfo.DataBind();

            if(reservationBO.Count > 0)
            {
                this.pnlBillTransferForReservation.Visible = true;
                this.LoadReservationNumber();
            }
            else
            {
                this.pnlBillTransferForReservation.Visible = false;
            }
        }
        private void LoadGridView(int registrationId)
        {
            GuestBillPaymentDA da = new GuestBillPaymentDA();
            List<GuestBillPaymentBO> masterList = new List<GuestBillPaymentBO>();
            List<GuestBillPaymentBO> frontOfficeGuestPaymentList = da.GetGuestBillPaymentInfoByRegistrationId("FrontOffice", registrationId).Where(x => x.ModuleName == "FrontOffice").ToList();
            List<GuestBillPaymentBO> frontOfficePaymentList = frontOfficeGuestPaymentList.Where(x => x.ModuleName == "FrontOffice" & x.PaymentType == "Payment").ToList();
            List<GuestBillPaymentBO> frontOfficeAdvanceList = frontOfficeGuestPaymentList.Where(x => x.ModuleName == "FrontOffice" & x.PaymentType == "Advance").ToList();
            masterList.AddRange(frontOfficePaymentList);
            masterList.AddRange(frontOfficeAdvanceList);

            this.gvGHServiceBill.DataSource = masterList;
            this.gvGHServiceBill.DataBind();

            if (masterList.Count > 0)
            {
                this.pnlBillTransferedInfo.Visible = true;
                this.LoadRoomNumber();
            }
            else
            {
                this.pnlBillTransferedInfo.Visible = false;
            }
        }
        private void LoadRoomNumber()
        {
            int condition = 0;
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomList = roomNumberDA.GetRoomNumberInfoByCondition(0, condition);
            List<RoomNumberBO> roomDataSourceList = roomList.Where(m => m.RoomNumber.ToString() != this.txtSrcRoomNumber.Text.Trim()).ToList();
            this.ddlRoomId.DataSource = roomDataSourceList;
            this.ddlRoomId.DataTextField = "RoomNumber";
            this.ddlRoomId.DataValueField = "RoomId";
            this.ddlRoomId.DataBind();

            ListItem itemRoom = new ListItem();
            itemRoom.Value = "0";
            itemRoom.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomId.Items.Insert(0, itemRoom);
        }
        private void LoadReservationNumber()
        {
            RoomReservationDA roomReservationDa = new RoomReservationDA();
            List<RoomReservationBO> roomReservationBoList = new List<RoomReservationBO>();
            roomReservationBoList = roomReservationDa.GetRoomReservationInfoWithReservationNumber();

            this.ddlReservationId.DataSource = roomReservationBoList;
            this.ddlReservationId.DataTextField = "ReservationNumber";
            this.ddlReservationId.DataValueField = "ReservationId";
            this.ddlReservationId.DataBind();

            ListItem itemRoom = new ListItem();
            itemRoom.Value = "0";
            itemRoom.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReservationId.Items.Insert(0, itemRoom);
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            RoomNumberDA numberDA = new RoomNumberDA();
            if (!string.IsNullOrWhiteSpace(this.RoomIdForCurrentBillHiddenField.Value))
            {
                RoomNumberBO transferedRoomBO = numberDA.GetRoomNumberInfoById(Int32.Parse(this.RoomIdForCurrentBillHiddenField.Value));
                if (transferedRoomBO.StatusId != 2)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Avilable existing Bill Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                    flag = false;
                }
            }

            if (this.ddlRoomId.SelectedValue != "0")
            {
                RoomNumberBO transferedRoomBO = numberDA.GetRoomNumberInfoById(Int32.Parse(this.ddlRoomId.SelectedValue));
                if (transferedRoomBO.StatusId != 2)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Avilable Transfered Room Number.", AlertType.Warning);
                    this.ddlRoomId.Focus();
                    flag = false;
                }
            }
            if (txtRemarks.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Description for Bill Transfer.", AlertType.Warning);
                txtRemarks.Focus();
                flag = false;
            }

            return flag;
        }
    }
}