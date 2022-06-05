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
    public partial class frmBillAdjustment : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.pnlServiceAdjustmentInfo.Visible = false;
                this.CheckPermission();
                this.lblRegistrationNumberDiv.Visible = false;
                this.ddlRegistrationId.Visible = false;
                this.LoadCostCenter();
            }
            //if (this.ddlCostCenterId.SelectedValue == "1000")
            //{
            //    FrontOfficeServiceInfo.Visible = true;
            //}
            //else
            //{
            //    FrontOfficeServiceInfo.Visible = false;
            //}
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            SearchRoomInformation(this.txtSrcRoomNumber.Text);

            //if (this.ddlCostCenterId.SelectedValue == "1000")
            //{
            //    FrontOfficeServiceInfo.Visible = true;
            //}
            //else
            //{
            //    FrontOfficeServiceInfo.Visible = false;
            //}
        }
        private void CheckPermission()
        {
            btnAdjustment.Visible = isSavePermission;
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
                    this.LoadDetailInformation(roomAllocationBO.RegistrationId);
                    this.LoadGuestHouseService(roomAllocationBO.RegistrationId);
                }
                else
                {
                    DateTime dateTime = DateTime.Now;
                    this.txtGuestNameInfo.Text = string.Empty;
                    this.txtRoomTypeInfo.Text = string.Empty;
                    this.lblRegistrationNumberDiv.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                    this.ddlCurrencyHiddenField.Value = "45";
                    this.txtCheckInDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.txtDepartureDateHiddenField.Value = hmUtility.GetStringFromDateTime(dateTime);
                    this.ddlCompanyNameHiddenField.Value = "0";
                    this.ddlBusinessPromotionIdHiddenField.Value = "0";
                    this.pnlServiceAdjustmentInfo.Visible = false;
                    this.txtSrcRoomNumber.Focus();
                }
            }
            else
            {
                this.txtGuestNameInfo.Text = string.Empty;
                this.txtRoomTypeInfo.Text = string.Empty;
                this.lblRegistrationNumberDiv.Visible = false;
                this.ddlRegistrationId.Visible = false;
                this.pnlServiceAdjustmentInfo.Visible = false;
                //gvGHServiceBill.DataSource = null;
                //gvGHServiceBill.DataBind();
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
        private void LoadDetailInformation(int registrationId)
        {
            GHServiceBillBO ghServiceBillBO = new GHServiceBillBO();
            GHServiceBillDA ghServiceDA = new GHServiceBillDA();
            List<GHServiceBillBO> list = new List<GHServiceBillBO>();
            list = ghServiceDA.GetGHServiceBillInfoByRegistrationId(registrationId);

            //gvGHServiceBill.DataSource = list;
            //gvGHServiceBill.DataBind();

            if (list.Count > 0)
            {
                this.pnlServiceAdjustmentInfo.Visible = true;
                //this.LoadRoomNumber();
            }
            else
            {
                this.pnlServiceAdjustmentInfo.Visible = false;
            }
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();

            List<CostCentreTabBO> entityListBO = new List<CostCentreTabBO>();
            entityListBO = entityDA.GetCostCentreTabInfo().Where(x => x.CostCenterType == "Restaurant" || x.CostCenterType == "ServiceBill").ToList();
            if (entityListBO != null)
            {
                if (entityListBO.Count > 0)
                {
                    foreach (CostCentreTabBO row in entityListBO)
                    {
                        if (row.CostCenterType == "ServiceBill")
                        {
                            row.CostCenterId = 1000;
                        }
                    }
                }
            }

            this.ddlCostCenterId.DataSource = entityListBO;
            this.ddlCostCenterId.DataTextField = "CostCenter";
            this.ddlCostCenterId.DataValueField = "CostCenterId";
            this.ddlCostCenterId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenterId.Items.Insert(0, item);
        }
        private void LoadGuestHouseService(int registrationId)
        {
            GHServiceBillBO ghServiceBillBO = new GHServiceBillBO();
            GHServiceBillDA ghServiceDA = new GHServiceBillDA();
            List<GHServiceBillBO> list = new List<GHServiceBillBO>();
            list = ghServiceDA.GetGuestAchievedServiceInfoByRegistrationId(registrationId).Where(x => x.ModuleName == "FrontOffice").ToList();

            this.ddlServiceId.DataSource = list;
            this.ddlServiceId.DataTextField = "ServiceName";
            this.ddlServiceId.DataValueField = "ServiceId";
            this.ddlServiceId.DataBind();

            ListItem itemServiceId = new ListItem();
            itemServiceId.Value = "0";
            itemServiceId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlServiceId.Items.Insert(0, itemServiceId);
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

            //if (this.ddlRoomId.SelectedValue != "0")
            //{
            //    RoomNumberBO transferedRoomBO = numberDA.GetRoomNumberInfoById(Int32.Parse(this.ddlRoomId.SelectedValue));
            //    if (transferedRoomBO.StatusId != 2)
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Avilable Transfered Room Number.", AlertType.Warning);
            //        this.ddlRoomId.Focus();
            //        flag = false;
            //    }
            //}

            return flag;
        }
        [WebMethod]
        public static List<BillInfoViewBO> LoadBillInfo(string strRegistrationId, string coscenterid, string serviceid)
        {
            int registrationId = 0, costCenterId = 0, serviceId = 0;
            if (!string.IsNullOrEmpty(strRegistrationId))
            {
                registrationId = Convert.ToInt32(strRegistrationId);
            }
            if (!string.IsNullOrEmpty(coscenterid))
            {
                costCenterId = Convert.ToInt32(coscenterid);
            }
            if (!string.IsNullOrEmpty(serviceid))
            {
                serviceId = Convert.ToInt32(serviceid);
            }
            //if (costCenterId == 1000)
            //{
            //    costCenterType = "ServiceBill";
            //}

            GHServiceBillDA serviceDA = new GHServiceBillDA();
            List<BillInfoViewBO> boList = new List<BillInfoViewBO>();
            boList = serviceDA.GetBillInfoByCostCenter(registrationId, costCenterId, serviceId);

            return boList;
        }
        [WebMethod]
        public static BillInfoViewBO LoadBillDetailInfo(string strCostCenterId, string strBillId)
        {
            //int billId = 0, serviceId = 0;
            //string costCenterId = string.Empty;
            //if (!string.IsNullOrEmpty(strBillId))
            //{
            //    billId = Convert.ToInt32(strBillId);
            //}

            int serviceBillId = !string.IsNullOrWhiteSpace(strBillId) ? Convert.ToInt32(strBillId) : 0;
            int costCenterId = !string.IsNullOrWhiteSpace(strCostCenterId) ? Convert.ToInt32(strCostCenterId) : 0;

            GHServiceBillDA serviceDA = new GHServiceBillDA();
            BillInfoViewBO boList = new BillInfoViewBO();
            boList = serviceDA.LoadBillDetailInfoByBillIdNType(costCenterId, serviceBillId);

            return boList;
        }
        [WebMethod]
        public static ReturnInfo PerformSaveAction(string ddlCostCenterId, string strAdjustmentAmount, string strBillId, string strRemarks)
        {
            ReturnInfo rtninf = new ReturnInfo();
            int serviceBillId = !string.IsNullOrWhiteSpace(strBillId) ? Convert.ToInt32(strBillId) : 0;
            int costCenterId = !string.IsNullOrWhiteSpace(ddlCostCenterId) ? Convert.ToInt32(ddlCostCenterId) : 0;

            if (serviceBillId > 0)
            {
                HMUtility hmUtility = new HMUtility();
                GuestBillPaymentBO guestBillPayment = new GuestBillPaymentBO();

                try
                {
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    decimal adjustmentAmount = !string.IsNullOrWhiteSpace(strAdjustmentAmount) ? Convert.ToDecimal(strAdjustmentAmount) : 0;

                    GHServiceBillDA ghServiceBillDA = new GHServiceBillDA();
                    Boolean status = ghServiceBillDA.SaveGHServiceBillInfoForBillAdjustment(serviceBillId, costCenterId, adjustmentAmount, strRemarks, userInformationBO.UserInfoId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BillAdjustment.ToString(), serviceBillId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BillAdjustment));
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
                    }
                }
                catch (Exception ex)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }

            return rtninf;
        }

    }
}