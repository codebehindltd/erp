using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmRestaurantPaidService : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCostCentre();
            }
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            if (this.ddlCostCentre.SelectedValue == "0")
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Cost Center Information";
                this.ddlCostCentre.Focus();
            }
            else
            {
                this.lblMessage.Text = string.Empty;
                this.SearchPSInformation(this.txtSrcRoomNumber.Text);
                this.LoadRestaurantInfoGridView();
            }
        }
        protected void gvRestaurantServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvRestaurantServiceBill.PageIndex = e.NewPageIndex;
            this.LoadRestaurantInfoGridView();
        }
        protected void gvRestaurantServiceBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    ApprovedGuestService("RestaurantService", Convert.ToInt32(e.CommandArgument.ToString()));
                    this.LoadRestaurantInfoGridView();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    GHServiceBillBO serviceList = new GHServiceBillBO();
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                    List<GHServiceBillBO> files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", Convert.ToInt32(e.CommandArgument.ToString()), DateTime.Now, this.txtSrcRoomNumber.Text);
                    if (files != null)
                    {
                        if (files.Count > 0)
                        {
                            int ApprovedId = files[0].ApprovedId;

                            HMCommonDA hmCommonDA = new HMCommonDA();
                            Boolean status = hmCommonDA.DeleteInfoById("HotelGuestServiceBillApproved", "ApprovedId", ApprovedId);
                            if (status)
                            {
                                this.LoadRestaurantInfoGridView();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //lblMessage.Text = "Data Deleted Failed.";
                    throw ex;
                }
            }
        }
        protected void gvRestaurantServiceBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblApprovedIdValue = (Label)e.Row.FindControl("lblApprovedId");

                Label lblRoomNumberValue = (Label)e.Row.FindControl("lblRoomNumber");

                //Label lblActiveStatValue = (Label)e.Row.FindControl("lblServiceApprovedStatus");
                Label lblIsPaidServiceAchievedValue = (Label)e.Row.FindControl("lblIsPaidServiceAchieved");
                if (lblIsPaidServiceAchievedValue.Text == "False")
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = true;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadCostCentre()
        {
            CostCentreTabDA apprMarksIndDA = new CostCentreTabDA();
            this.ddlCostCentre.DataSource = apprMarksIndDA.GetAllRestaurantTypeCostCentreTabInfo();
            this.ddlCostCentre.DataTextField = "CostCenter";
            this.ddlCostCentre.DataValueField = "CostCenterId";
            this.ddlCostCentre.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCentre.Items.Insert(0, item);
        }
        private void SearchPSInformation(string RoomNumber)
        {
            if (!string.IsNullOrWhiteSpace(RoomNumber))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNumber);
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
                    gvRestaurantServiceBill.DataSource = null;
                    gvRestaurantServiceBill.DataBind();
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
                gvRestaurantServiceBill.DataSource = null;
                gvRestaurantServiceBill.DataBind();
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
        private void LoadRestaurantInfoGridView()
        {
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                int registrationId = 0;
                int IsGuestTodaysBillAddInfo = 0;
                List<GHServiceBillBO> files = new List<GHServiceBillBO>();
                List<GHServiceBillBO> previousDayServices = new List<GHServiceBillBO>();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                if (Convert.ToInt32(ddlRegistrationId.SelectedValue) > 0)
                {
                    GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();
                    GuestHouseCheckOutDetailBO IsGuestTodaysBillAddBOInfo = guestHouseCheckOutDA.GetIsGuestTodaysBillAdd(ddlRegistrationId.SelectedValue);
                    if (IsGuestTodaysBillAddBOInfo.IsGuestTodaysBillAdd == 0)
                    {
                        IsGuestTodaysBillAddInfo = 0;
                        List<GHServiceBillBO> previousDaysAllServices = new List<GHServiceBillBO>();
                        RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                        roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(ddlRegistrationId.SelectedValue));
                        if (roomRegistrationBO != null)
                        {
                            if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
                            {
                                previousDaysAllServices = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", 0, DateTime.Now, this.txtSrcRoomNumber.Text);
                                //previousDayServices.AddRange(previousDaysAllServices);
                                previousDayServices.AddRange(previousDaysAllServices.Where(x => x.IsPaidService == true).ToList());
                            }
                            else
                            {
                                previousDaysAllServices = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", 0, DateTime.Now.AddDays(-1), this.txtSrcRoomNumber.Text);
                                //previousDayServices.AddRange(previousDaysAllServices.Where(x => x.IsPaidServiceAchieved == false).ToList());
                                previousDayServices.AddRange(previousDaysAllServices.Where(x => x.IsPaidService == true && x.IsPaidServiceAchieved == false).ToList());
                            }
                        }
                    }
                    else
                    {
                        IsGuestTodaysBillAddInfo = 1;

                        files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", 0, DateTime.Now, this.txtSrcRoomNumber.Text);
                        List<GHServiceBillBO> paidServiceListBO = new List<GHServiceBillBO>();
                        paidServiceListBO = files.Where(x => x.IsPaidService == true).ToList();
                        foreach (GHServiceBillBO row in paidServiceListBO)
                        {
                            row.TotalCalculatedAmount = Math.Round(row.TotalCalculatedAmount);
                        }
                    }
                }

                HotelGuestServiceInfoDA paidServiceDA = new HotelGuestServiceInfoDA();
                List<HotelGuestServiceInfoBO> paidServiceList = new List<HotelGuestServiceInfoBO>();
                paidServiceList = paidServiceDA.GetHotelGuestServiceInfoByCostCenter(Convert.ToInt32(this.ddlCostCentre.SelectedValue));

                List<GHServiceBillBO> costCenterItems = new List<GHServiceBillBO>();


                List<GHServiceBillBO> totalGuestServices = new List<GHServiceBillBO>();
                totalGuestServices.AddRange(previousDayServices);
                totalGuestServices.AddRange(files);

                foreach (HotelGuestServiceInfoBO row in paidServiceList)
                {
                    costCenterItems.AddRange(totalGuestServices.Where(x => x.ServiceId == row.ServiceId && x.IsPaidService == true).ToList());
                }

                this.gvRestaurantServiceBill.DataSource = costCenterItems;
                this.gvRestaurantServiceBill.DataBind();
            }
            else
            {
                this.gvRestaurantServiceBill.DataSource = null;
                this.gvRestaurantServiceBill.DataBind();
            }
        }
        public void ApprovedGuestService(string guestServiceType, int rowId)
        {
            int tmpApprovedId = 0;
            Boolean IsGuestTodaysBillAdd = false;

            GHServiceBillBO serviceList = new GHServiceBillBO();
            List<GHServiceBillBO> files = new List<GHServiceBillBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(ddlRegistrationId.SelectedValue));
            if (roomRegistrationBO != null)
            {
                GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();
                GuestHouseCheckOutDetailBO IsGuestTodaysBillAddBOInfo = guestHouseCheckOutDA.GetIsGuestTodaysBillAdd(ddlRegistrationId.SelectedValue);
                if (IsGuestTodaysBillAddBOInfo.IsGuestTodaysBillAdd == 1)
                {
                    IsGuestTodaysBillAdd = true;
                    files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(guestServiceType, rowId, DateTime.Now, this.txtSrcRoomNumber.Text);
                }
                else
                {
                    if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
                    {
                        files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(guestServiceType, rowId, DateTime.Now, this.txtSrcRoomNumber.Text);
                    }
                    else
                    {
                        files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(guestServiceType, rowId, DateTime.Now.AddDays(-1), this.txtSrcRoomNumber.Text);
                    }
                }

                serviceList.ApprovedId = files[0].ApprovedId;
                serviceList.PaymentMode = files[0].RoomNumber.ToString();
                serviceList.RegistrationId = Int32.Parse(files[0].RegistrationId.ToString());
                serviceList.ServiceBillId = Int32.Parse(files[0].ServiceBillId.ToString());
                if (IsGuestTodaysBillAdd == true)
                {
                    serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString());
                }
                else
                {
                    //serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString());

                    if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
                    {
                        serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString());
                    }
                    else
                    {
                        serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString()).AddDays(-1);
                    }
                }

                serviceList.ServiceType = files[0].ServiceType.ToString();
                serviceList.ServiceId = Int32.Parse(files[0].ServiceId.ToString());
                serviceList.ServiceName = files[0].ServiceName.ToString();
                serviceList.ServiceQuantity = Convert.ToDecimal(files[0].ServiceQuantity.ToString());
                serviceList.ServiceRate = Convert.ToDecimal(files[0].ServiceRate.ToString());
                serviceList.DiscountAmount = Convert.ToDecimal(files[0].DiscountAmount.ToString());

                serviceList.VatAmount = Convert.ToDecimal(files[0].VatAmount.ToString());
                serviceList.ServiceCharge = Convert.ToDecimal(files[0].ServiceCharge.ToString());

                serviceList.ApprovedStatus = true;
                serviceList.IsPaidService = Convert.ToBoolean(files[0].IsPaidService.ToString());
                //serviceList.ApprovedDate = DateTime.Now;
                if (IsGuestTodaysBillAdd == true)
                {
                    serviceList.ApprovedDate = DateTime.Now;
                }
                else
                {
                    if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
                    {
                        serviceList.ApprovedDate = DateTime.Now;
                    }
                    else
                    {
                        serviceList.ApprovedDate = DateTime.Now.AddDays(-1);
                    }
                }
                serviceList.CreatedBy = userInformationBO.UserInfoId;


                serviceList.VatAmountPercent = Convert.ToDecimal(files[0].VatAmountPercent.ToString());
                serviceList.ServiceChargePercent = Convert.ToDecimal(files[0].ServiceChargePercent.ToString());
                serviceList.CalculatedPercentAmount = Convert.ToDecimal(files[0].CalculatedPercentAmount.ToString());
                serviceList.IsPaidServiceAchieved = true;
                serviceList.RestaurantBillId = 0;

                if (serviceList.ApprovedId > 0)
                {
                    Boolean status = roomRegistrationDA.UpdateGuestServiceBillApprovedInfo(serviceList);
                    if (status)
                    {
                        this.isMessageBoxEnable = 2;
                        lblMessage.Text = "Achieve Operation Successfull.";
                        this.LoadRestaurantInfoGridView();
                    }
                }
                else
                {
                    Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
                    if (status)
                    {
                        this.isMessageBoxEnable = 2;
                        lblMessage.Text = "Achieve Operation Successfull.";
                        this.LoadRestaurantInfoGridView();
                    }
                }
            }
        }
    }
}