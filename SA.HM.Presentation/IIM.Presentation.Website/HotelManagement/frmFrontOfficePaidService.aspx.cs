using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmFrontOfficePaidService : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            SearchPSInformation(this.txtSrcRoomNumber.Text);
        }
        private void SearchPSInformation(string RoomNumber)
        {
            if (!string.IsNullOrWhiteSpace(RoomNumber))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNumber);
                this.LoadRegistrationNumber(roomAllocationBO.RoomId);
                if (roomAllocationBO.RoomId > 0)
                {
                    this.hfddlRegistrationId.Value = roomAllocationBO.RegistrationId.ToString();
                    this.RoomIdForCurrentBillHiddenField.Value = roomAllocationBO.RoomId.ToString();
                    this.txtGuestNameInfo.Text = roomAllocationBO.GuestName;
                    this.txtRoomTypeInfo.Text = roomAllocationBO.RoomType;
                    this.lblRegistrationNumber.Visible = true;
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
                    DateTime dateTime = DateTime.Now;
                    this.isMessageBoxEnable = 1;
                    this.txtGuestNameInfo.Text = string.Empty;
                    this.txtRoomTypeInfo.Text = string.Empty;
                    this.lblRegistrationNumber.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    lblMessage.Text = "Please provide a valid Room Number.";
                    this.ddlCurrencyHiddenField.Value = "45";
                    this.txtCheckInDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.txtDepartureDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.ddlCompanyNameHiddenField.Value = "0";
                    this.ddlBusinessPromotionIdHiddenField.Value = "0";
                    //this.pnlBillTransferedInfo.Visible = false;
                    gvPSConfirm.DataSource = null;
                    gvPSConfirm.DataBind();
                    this.txtSrcRoomNumber.Focus();
                }

                
            }
            else
            {
                this.isMessageBoxEnable = 1;
                this.txtGuestNameInfo.Text = string.Empty;
                this.txtRoomTypeInfo.Text = string.Empty;
                this.lblRegistrationNumber.Visible = false;
                this.ddlRegistrationId.Visible = false;
                lblMessage.Text = "Please provide a valid Room Number.";
                //this.pnlBillTransferedInfo.Visible = false;
                gvPSConfirm.DataSource = null;
                gvPSConfirm.DataBind();
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
        private void LoadGridView(int registrationId)
        {
            List<HRPaidServiceViewBO> psConfirmlist = new List<HRPaidServiceViewBO>();
            HRPaidServiceDA psConfirmDA = new HRPaidServiceDA();

            psConfirmlist = psConfirmDA.GetPaidServiceByRegId(registrationId);

            gvPSConfirm.DataSource = psConfirmlist;
            gvPSConfirm.DataBind();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            HotelRegPaidServiceDA hrPaidServiceDA = new HotelRegPaidServiceDA();
            RegistrationServiceInfoBO hrpsBO = new RegistrationServiceInfoBO();
            string confirmValue = Request.Form["confirm_value"];
            bool status = false;
            bool select = false;
            List<RegistrationServiceInfoBO> hrpsBOList = new List<RegistrationServiceInfoBO>();
            foreach (GridViewRow grRow in gvPSConfirm.Rows)
            {
                CheckBox chkItem = (CheckBox)grRow.FindControl("chkBox");
                string id = ((Label)grRow.FindControl("lblid")).Text.ToString();
                if (chkItem.Checked)
                {                    
                    hrpsBO.DetailServiceId = Convert.ToInt32(id);
                    hrpsBO.IsAchieved = true;

                    //hrpsBOList.Add(hrpsBO);
                    if (confirmValue == "Yes")
                    {
                        status = hrPaidServiceDA.UpdateRegistrationServiceInfo(hrpsBO);
                    }

                    if (userInformationBO.UserInfoId == 1)
                    {
                        chkItem.Enabled = false;
                    }
                    else
                    {
                        chkItem.Enabled = true;
                    }
                    select = true;
                }                
            }
            if (!select)
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please select a service name.";
            }
        }
    }
}