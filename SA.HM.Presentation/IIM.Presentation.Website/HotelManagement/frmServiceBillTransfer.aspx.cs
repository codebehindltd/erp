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
    public partial class frmServiceBillTransfer : BasePage
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
        protected void gvGHServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int registrationId = Convert.ToInt32(this.hfddlRegistrationId.Value);
            this.lblMessage.Text = string.Empty;
            this.gvGHServiceBill.PageIndex = e.NewPageIndex;
            this.LoadGridView(registrationId);
        }
        protected void btnBillTransfer_Click(object sender, EventArgs e)
        {
            List<GHServiceBillBO> serviceBillBOList = new List<GHServiceBillBO>();
            foreach (GridViewRow grRow in gvGHServiceBill.Rows)
            {
                CheckBox chkItem = (CheckBox)grRow.FindControl("chkBox");
                string id = ((Label)grRow.FindControl("lblid")).Text.ToString();
                string moduleName = ((Label)grRow.FindControl("lblModuleName")).Text.ToString();
                if (chkItem.Checked)
                {
                    GHServiceBillBO serviceBillBO = new GHServiceBillBO();
                    serviceBillBO.ServiceBillId = Convert.ToInt32(id);
                    serviceBillBO.Remarks = this.txtRemarks.Text;
                    serviceBillBO.ModuleName = moduleName;
                    serviceBillBOList.Add(serviceBillBO);
                }
            }

            if (serviceBillBOList != null)
            {
                if (serviceBillBOList.Count > 0)
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
                        GHServiceBillDA ghServiceBillDA = new GHServiceBillDA();
                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        Boolean status = ghServiceBillDA.TransferGHServiceBillInfo(serviceBillBOList, fromRegistrationId, transferRegistrationId, userInformationBO.UserInfoId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Transfer Operation Successfull.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity("Bill Transfer", EntityTypeEnum.EntityType.ServiceBill.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Service Bill Transferred (Room# " + fromRoomNumber + " => " + transferRoomNumber  + ") and Description: " + txtRemarks.Text);
                            this.txtSrcRoomNumber.Focus();
                            ClearForm();
                            //this.SearchRoomInformation(this.txtSrcRoomNumber.Text);
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
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Bill Information.", AlertType.Warning);
                    this.gvGHServiceBill.Focus();
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Bill Information.", AlertType.Warning);
                this.gvGHServiceBill.Focus();
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
            gvGHServiceBill.DataSource = null;
            gvGHServiceBill.DataBind();
            this.txtSrcRoomNumber.Focus();
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
            GHServiceBillBO ghServiceBillBO = new GHServiceBillBO();
            GHServiceBillDA ghServiceDA = new GHServiceBillDA();
            List<GHServiceBillBO> list = new List<GHServiceBillBO>();

            list = ghServiceDA.GetGHServiceBillInfoByRegistrationId(registrationId);

            gvGHServiceBill.DataSource = list;
            gvGHServiceBill.DataBind();

            if (list.Count > 0)
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