using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGuestCheckOut : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcRoomNumber.Text = numberBO.RoomNumber;
                    this.SearchInformation();
                }
                CheckPermission();
            }
        }
        protected void gvGuestCheckOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GuestRegistrationDA guestRegDA = new GuestRegistrationDA();
            GuestCheckOutViewBO guestRegInfo = new GuestCheckOutViewBO();
            Label lblValue = (Label)e.Row.FindControl("lblid");
            if (e.Row.DataItem != null)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                Label lblCheckInDateValue = (Label)e.Row.FindControl("lblCheckInDate");
                Label lblCheckOutDateValue = (Label)e.Row.FindControl("lblCheckOutDate");
                DateTime CheckInDateTime = Convert.ToDateTime(lblCheckInDateValue.Text);
                //DateTime CheckInDateTime = hmUtility.GetDateTimeFromString(lblCheckInDateValue.Text, userInformationBO.ServerDateFormat);
                lblCheckInDateValue.Text = hmUtility.GetDateTimeStringFromDateTime(CheckInDateTime);
                lblCheckInDateValue.Text = hmUtility.GetDateTimeStringFromDateTime(CheckInDateTime);

                if (string.IsNullOrWhiteSpace(lblCheckOutDateValue.ToString()))
                {
                    DateTime CheckOutDateTime = Convert.ToDateTime(lblCheckOutDateValue.Text);
                    lblCheckOutDateValue.Text = hmUtility.GetDateTimeStringFromDateTime(CheckOutDateTime);
                }

                ((ImageButton)e.Row.FindControl("ImgCheckIn")).Visible = false;
                guestRegInfo = guestRegDA.GetGuestRegInfoById(Convert.ToInt32(lblValue.Text));
                if (guestRegInfo.CheckOutDate != null)
                {
                    ((ImageButton)e.Row.FindControl("ImgCheckOut")).Visible =false;
                    ((ImageButton)e.Row.FindControl("ImgCheckIn")).Visible = (true && isSavePermission);
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("ImgCheckOut")).Visible = (true && isDeletePermission);
                    ((ImageButton)e.Row.FindControl("ImgCheckIn")).Visible = false;
                }
            }
        }
        protected void gvGuestCheckOut_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool status = false;
            int typetId = Convert.ToInt32(e.CommandArgument.ToString());
            GuestRegistrationDA guestRegDA = new GuestRegistrationDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (e.CommandName == "CmdCheckOut")
            {
                status = guestRegDA.UpdateGuestRegistrationById(typetId, "Yes", userInformationBO.UserInfoId);

                if (status)
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), "Pax Out", typetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Pax Out");
                CommonHelper.AlertInfo(innboardMessage, "Check Out Successfull", AlertType.Success);
            }
            else if (e.CommandName == "CmdCheckIn")
            {
                status = guestRegDA.UpdateGuestRegistrationById(typetId, "No", userInformationBO.UserInfoId);
                if (status)
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), "Pax In", typetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Pax In");
                CommonHelper.AlertInfo(innboardMessage, "Check In Successfull", AlertType.Success);
            }
            
            LoadGridInformation();
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            LoadGridInformation();
        }
        //************************ User Defined Function ********************//
        private void SearchInformation()
        {
            this.LoadGridInformation();
        }
        private void CheckPermission()
        {
            btnSrcRoomNumber.Visible = isSavePermission;
        }
        private void LoadGridInformation()
        {
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                GuestRegistrationDA guestRegDA = new GuestRegistrationDA();
                List<GuestCheckOutViewBO> activeGuestList = new List<GuestCheckOutViewBO>();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
                if (roomAllocationBO.RoomId > 0)
                {
                    //this.lblRegistrationNumber.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.hfRegistrationId.Value = roomAllocationBO.RegistrationId.ToString();
                    this.hfRegistrationNumber.Value = roomAllocationBO.RegistrationNumber.ToString();
                    this.hfCheckInDate.Value = roomAllocationBO.ArriveDate.ToString();
                    this.LoadRegistrationNumber(roomAllocationBO.RoomId);
                    activeGuestList = guestRegDA.GetActiveGuestInfoByRegiId(roomAllocationBO.RegistrationId);

                    List<GuestCheckOutViewBO> extraGuestCheckInList = new List<GuestCheckOutViewBO>();
                    extraGuestCheckInList = activeGuestList.Where(x => x.PaxInRate > 0).ToList();

                    List<GuestCheckOutViewBO> guestCheckInList = new List<GuestCheckOutViewBO>();
                    guestCheckInList = activeGuestList.Where(x => x.PaxInRate == 0 && x.CheckOutDate == null).ToList();

                    List<GuestCheckOutViewBO> guestCheckInList2 = new List<GuestCheckOutViewBO>();
                    guestCheckInList2 = activeGuestList.Where(x => x.PaxInRate == 0 && x.CheckOutDate != null).ToList();

                    if (guestCheckInList != null)
                    {
                        if (guestCheckInList.Count > 1)
                        {
                            this.gvGuestCheckOut.DataSource = activeGuestList;
                            this.gvGuestCheckOut.DataBind();
                        }
                        else
                        {
                            extraGuestCheckInList.AddRange(guestCheckInList2);
                            this.gvGuestCheckOut.DataSource = extraGuestCheckInList;
                            this.gvGuestCheckOut.DataBind();
                        }
                    }
                }
                else
                {
                    //this.isMessageBoxEnable = 1;
                    //this.lblRegistrationNumber.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    //lblMessage.Text = "Please provide a Valid Room Number.";
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                }
            }
            else
            {
                //this.isMessageBoxEnable = 1;
                //this.lblRegistrationNumber.Visible = false;
                this.ddlRegistrationId.Visible = false;
                //lblMessage.Text = "Please provide a Room Valid Number.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
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
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}