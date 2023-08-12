using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HMCommon;
using System.Configuration;
using System.Globalization;
using HotelManagement.Data;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using HotelManagement.Entity;
using HotelManagement.Data.Restaurant;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmHoldRoomCheckOut : System.Web.UI.Page
    {
        protected bool isSingle = true;
        HiddenField innboardMessage;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        protected int isCompanyProjectPanelEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int LocalCurrencyId;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                HttpContext.Current.Session["BillPreviewCurrencyRateInformation"] = null;
                this.LoadBank();
                this.LoadLabelForSalesTotal();
                this.ClearCommonSessionInformation();
                hfGuestPaymentDetailsInformationDiv.Value = "1";
                this.DayLetDiscountInputDiv.Visible = false;
                this.DayLetDiscountOutputDiv.Visible = false;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                this.LoadCheckBoxListServiceInformation();
                this.CheckObjectPermission();
                this.LoadCurrentDate();
                this.LoadRoomNumber();
                this.LoadCurrency();
                LoadIsConversionRateEditable();
                LoadLocalCurrencyId();
                IsLocalCurrencyDefaultSelected();
                this.LoadGridView();
                this.LoadRefundAccountHead();
                this.lblRegistrationNumber.Visible = false;
                this.ddlRegistrationId.Visible = false;
                this.LoadCommonDropDownHiddenField();
                this.LoadRelatedInformation();
                if (isSingle == true)
                {
                    isCompanyProjectPanelEnable = 1;
                    LoadSingleProjectAndCompany();
                }
                else
                {
                    this.LoadGLCompany(false);
                    this.LoadGLProject(false);
                }

                this.LoadAccountHeadInfo();

                Session["TransactionDetailList"] = null;
                Session["GuestPaymentDetailListForGrid"] = null;

                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                if (!isIntegrated)
                {
                    isIntegratedGeneralLedgerDiv = -1;
                }

                string queryStringId = Request.QueryString["BackFromServiceBill"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ddlRoomId.SelectedValue = queryStringId;
                    this.txtSrcRoomNumber.Text = this.ddlRoomId.SelectedItem.Text;
                    this.lblRegistrationNumber.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.LoadRelatedInformation();
                    this.SavePendingGuestRoomInformation();
                    this.LoadServiceInformationWithControl();
                    this.LoadCheckBoxListServiceInformation();
                }


                string roomId = Request.QueryString["RegId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    Session["AddedExtraRoomInformation"] = null;
                    txtSrcRegistrationIdList.Value = string.Empty;
                    txtSrcRoomNumber.Text = roomId;
                    this.SearchButtonClick();
                }

                string BackFromServiceBill = Request.QueryString["BackFromServiceBill"];
                if (!string.IsNullOrEmpty(BackFromServiceBill))
                {
                    //this.SearchByRoomNumber();
                }
                this.LoadStartAndEndDate();
                this.LoadCompanyChequeHeadInfo();
            }
            btnAddDetailGuest.Visible = false; //it's need to work-----------------------------------------------------
            if (Session["HiddenFieldCompanyPaymentButtonInfo"] != null)
            {
                this.HiddenFieldCompanyPaymentButtonInfo.Value = "1";
            }
            else
            {
                this.HiddenFieldCompanyPaymentButtonInfo.Value = "0";
            }
        }
        protected void btnAddDetailGuest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                Session["CheckOutPayMode"] = this.ddlPayMode.SelectedItem.Text;
                Response.Redirect("/HotelManagement/frmGHServiceBill.aspx?AddMoreService=" + this.ddlRoomId.SelectedValue);
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                return;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            Response.Redirect("/HotelManagement/frmRoomCheckOut.aspx");
        }
        protected void gvGuestHouseCheckOut_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.gvGuestHouseCheckOut.PageIndex = e.NewPageIndex;
            //this.LoadGridView();
        }
        protected void gvGuestHouseCheckOut_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
        }
        protected void btnAddMoreBill_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                Response.Redirect("/HotelManagement/frmAddMoreBill.aspx?AddMoreBill=" + this.ddlRoomId.SelectedValue + "~" + this.ddlRegistrationId.SelectedValue);
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                return;
            }
        }
        protected void btnBillPreview_Click(object sender, EventArgs e)
        {
            if (this.ddlRoomId.SelectedValue != "0")
            {
                this.GoToPrintPreviewReport("0");
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "a Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
            }
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            this.chkIsDaysLet.Checked = false;
            this.ddlDayLetDiscount.SelectedIndex = 0;
            this.txtDayLetDiscount.Text = "0";
            this.ClearCommonSessionInformation();
            this.ClearUnCommonSessionInformation();
            this.SearchButtonClick();
            this.LoadStartAndEndDate();
        }
        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadAccountHeadInfo();
        }
        protected void gvIndividualServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }
        protected void gvGroupServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }
        protected void btnBillSplitPrintPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "From Date.", AlertType.Warning);
                this.txtStartDate.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "To Date.", AlertType.Warning);
                this.txtEndDate.Focus();
                return;
            }
            this.GoToPrintPreviewReport("1");
        }
        protected void gvRoomDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblNightAuditApprovedValue = (Label)e.Row.FindControl("lblNightAuditApproved");
                if (lblNightAuditApprovedValue.Text == "N")
                {
                    Label lblServiceDateValue = (Label)e.Row.FindControl("lblServiceDate");

                    DateTime cmpCurrentDateTime = hmUtility.GetDateTimeFromString(lblServiceDateValue.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    if (DateTime.Now.Month == cmpCurrentDateTime.Month)
                    {
                        if (DateTime.Now.Day == cmpCurrentDateTime.Day)
                        {
                            this.DayLetDiscountInputDiv.Visible = true;
                            this.DayLetDiscountOutputDiv.Visible = true;
                            Label lblTotalAmountValue = (Label)e.Row.FindControl("lblTotalAmount");
                            TodaysRoomBillHiddenField.Value = !string.IsNullOrWhiteSpace(lblTotalAmountValue.Text) ? lblTotalAmountValue.Text : "0";
                        }
                    }
                }
            }
        }
        protected void gvServiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsRoomBillSettlmentPending())
            {
                return;
            }
            if (!IsFrmValid())
            {
                return;
            }
            int transactionId = 0;
            int MasterId = 0;
            string transactionHead = string.Empty;
            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            customField = hmCommonDA.GetCustomFieldByFieldName("GuestBillPayment");

            int validDate = roomRegistrationDA.GetIsValidNightAuditDateTime(DateTime.Now);

            if (customField == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                return;
            }
            else
            {
                transactionId = Convert.ToInt32(customField.FieldValue);
                CommonNodeMatrixBO commonNodeMatrixBO = new CommonNodeMatrixBO();
                commonNodeMatrixBO = hmCommonDA.GetCommonNodeMatrixInfoById(transactionId);
                if (commonNodeMatrixBO == null)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                    return;
                }
                else
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomRegistrationBO registrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Int32.Parse(this.ddlRegistrationId.SelectedValue));
                    if (registrationBO.IsGuestCheckedOut == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                        this.txtSrcRoomNumber.Focus();
                        return;
                    }

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    List<GuestHouseCheckOutBO> guestHouseCheckOutBOList = new List<GuestHouseCheckOutBO>();                    
                    List<GHServiceBillBO> entityRoomDetailBOList = new List<GHServiceBillBO>();
                    List<GHServiceBillBO> entityDetailBOList = new List<GHServiceBillBO>();
                    GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();

                    decimal totalSalesAmount = !string.IsNullOrWhiteSpace(HiddenFieldSalesTotal.Value) ? Convert.ToDecimal(HiddenFieldSalesTotal.Value) : 0;
                    decimal grandTotalAmount = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;
                    if (totalSalesAmount != grandTotalAmount)
                    {
                        GuestCheckOutDiscount(Int32.Parse(this.ddlRegistrationId.SelectedValue));
                    }

                    int totalRoomCheckOut = txtSrcRegistrationIdList.Value.Split(',').Length - 1;

                    Session["CurrentRegistrationId"] = txtSrcRegistrationIdList.Value;

                    for (int i = 0; i <= totalRoomCheckOut; i++)
                    {
                        GuestHouseCheckOutBO guestHouseCheckOutBO = new GuestHouseCheckOutBO();
                        if (!string.IsNullOrWhiteSpace(txtSrcRegistrationIdList.Value.Split(',')[i]))
                        {
                            guestHouseCheckOutBO.RegistrationId = Convert.ToInt32(txtSrcRegistrationIdList.Value.Split(',')[i]);
                            guestHouseCheckOutBO.CheckOutProcessType = "HoldUpProcess";

                            guestHouseCheckOutBO.PayMode = this.ddlPayMode.SelectedItem.Text;
                            if (this.ddlPayMode.SelectedValue == "Cash")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCashReceiveAccountsInfo.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "Card")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCardPaymentAccountHeadId.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "Cheque")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlChecquePaymentAccountHeadId.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "Company")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCompanyPaymentAccountHead.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "Refund")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlRefundAccountHead.SelectedValue);
                            }
                            guestHouseCheckOutBO.ChecqueNumber = this.txtChecqueNumber.Text;
                            guestHouseCheckOutBO.CardType = this.ddlCardType.SelectedValue.ToString();

                            guestHouseCheckOutBO.CardNumber = this.txtCardNumber.Text;
                            if (!string.IsNullOrWhiteSpace(this.txtExpireDate.Text))
                            {
                                guestHouseCheckOutBO.ExpireDate = hmUtility.GetDateTimeFromString(this.txtExpireDate.Text, userInformationBO.ServerDateFormat);
                            }
                            else
                            {
                                guestHouseCheckOutBO.ExpireDate = null;
                            }

                            guestHouseCheckOutBO.CardHolderName = this.txtCardHolderName.Text;

                            guestHouseCheckOutBO.VatAmount = Convert.ToDecimal(this.txtVatTotal.Text);
                            guestHouseCheckOutBO.ServiceCharge = Convert.ToDecimal(this.txtServiceChargeTotal.Text);
                            guestHouseCheckOutBO.DiscountAmount = Convert.ToDecimal(this.txtDiscountAmountTotal.Text);

                            guestHouseCheckOutBO.TotalAmount = Convert.ToDecimal(this.txtSalesTotal.Text);

                            if (this.ddlPayMode.SelectedItem.Text == "Other Room")
                            {
                                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.ddlPaidByRegistrationId.SelectedItem.Text);
                                if (roomAllocationBO != null)
                                {
                                    guestHouseCheckOutBO.BillPaidBy = roomAllocationBO.RegistrationId;//Convert.ToInt32(this.ddlPaidByRegistrationId.SelectedValue);
                                }
                            }
                            else
                            {
                                guestHouseCheckOutBO.BillPaidBy = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                            }

                            guestHouseCheckOutBO.RebateRemarks = this.txtRebateRemarks.Text;
                            guestHouseCheckOutBO.CreatedBy = userInformationBO.UserInfoId;
                            guestHouseCheckOutBOList.Add(guestHouseCheckOutBO);
                        }
                    }

                    // -- Room List Not Approved From Night Audit----------
                    foreach (GridViewRow row in gvRoomDetail.Rows)
                    {
                        GHServiceBillBO entityRoomDetailBO = new GHServiceBillBO();
                        Label isNightAuditApproved = (Label)row.FindControl("lblNightAuditApproved");

                        Label isNightAuditApprovedDate = (Label)row.FindControl("lblServiceDate");

                        if (isNightAuditApproved.Text == "N")
                        {
                            entityRoomDetailBO.RegistrationId = Int32.Parse(((Label)row.FindControl("lblRegistrationId")).Text);
                            entityRoomDetailBO.ServiceBillId = Int32.Parse(((Label)row.FindControl("lblid")).Text);
                            entityRoomDetailBO.ServiceDate = hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblServiceDate")).Text, userInformationBO.ServerDateFormat);
                            entityRoomDetailBO.ServiceType = ((Label)row.FindControl("lblServiceType")).Text;
                            entityRoomDetailBO.ServiceId = Int32.Parse(((Label)row.FindControl("lblServiceId")).Text);
                            entityRoomDetailBO.ServiceName = ((Label)row.FindControl("lblRoomNumber")).Text;
                            entityRoomDetailBO.ServiceQuantity = Convert.ToDecimal(((Label)row.FindControl("lblServiceQuantity")).Text);
                            entityRoomDetailBO.ServiceRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text);
                            entityRoomDetailBO.DiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);
                            entityRoomDetailBO.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblVatAmount")).Text);
                            entityRoomDetailBO.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblServiceCharge")).Text);
                            entityRoomDetailBO.ReferenceSalesCommission = Convert.ToDecimal(((Label)row.FindControl("lblgvReferenceSalesCommission")).Text);

                            entityRoomDetailBO.VatAmountPercent = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmountPercent")).Text);
                            entityRoomDetailBO.ServiceChargePercent = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceChargePercent")).Text);
                            entityRoomDetailBO.CalculatedPercentAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvCalculatedPercentAmount")).Text);
                            entityRoomDetailBO.TotalCalculatedAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);

                            entityRoomDetailBO.IsBillHoldUp = false;
                            entityRoomDetailBO.ApprovedStatus = true;
                            DateTime dateTime = DateTime.Now;

                            entityRoomDetailBO.ApprovedDate = hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblServiceDate")).Text, userInformationBO.ServerDateFormat);
                            entityRoomDetailBO.CreatedBy = userInformationBO.UserInfoId;
                            entityRoomDetailBOList.Add(entityRoomDetailBO);
                        }
                    }

                    if (btnSave.Text.Equals("Check Out"))
                    {
                        Boolean status = guestHouseCheckOutDA.SaveGuestHouseCheckOutInfo(guestHouseCheckOutBOList, entityRoomDetailBOList, entityDetailBOList, Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>, Session["CompanyPaymentRoomIdList"] as List<GuestServiceBillApprovedBO>, Session["CompanyPaymentServiceIdList"] as List<GuestServiceBillApprovedBO>, "Others", 0);
                        if (status)
                        {
                            Clear();
                            this.Session["AddedExtraRoomInformation"] = null;
                            Session["HiddenFieldCompanyPaymentButtonInfo"] = null;
                            CommonHelper.AlertInfo(innboardMessage, "Check Out Operation Successfull.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.CheckOut.ToString(), EntityTypeEnum.EntityType.RoomCheckOut.ToString(), MasterId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomCheckOut));
                            this.LoadGridView();

                            string deletedRegistrationIdList = string.Empty;
                            foreach (GuestHouseCheckOutBO row in guestHouseCheckOutBOList)
                            {
                                if (string.IsNullOrWhiteSpace(deletedRegistrationIdList))
                                {
                                    deletedRegistrationIdList = row.RegistrationId.ToString();
                                }
                                else
                                {
                                    deletedRegistrationIdList = deletedRegistrationIdList + "," + row.RegistrationId.ToString();
                                }
                            }

                            //Boolean deleteStatus = guestHouseCheckOutDA.DeleteGuestPendingBillInfoByRegistrationId(deletedRegistrationIdList);

                            string isEnableBillPreviewOption = !string.IsNullOrWhiteSpace(this.hfIsEnableBillPreviewOption.Value) ? this.hfIsEnableBillPreviewOption.Value : "0";

                            string currencyConversionRate = "0";

                            if (isEnableBillPreviewOption != "0")
                            {
                                currencyConversionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? this.txtConversionRate.Text : "0";
                            }

                            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyConversionRate, "0", "", "-1", "-1", "-1", "-1", "-1", "-1", "-1", hmUtility.GetFromDate().ToString(), hmUtility.GetToDate().ToString(), this.Session["CurrentRegistrationId"].ToString(), this.Session["CurrentRegistrationId"].ToString());

                            string url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx?GuestBillInfo=" + this.Session["CurrentRegistrationId"];
                            string s = "window.open('" + url + "', 'popup_window', 'width=825,height=680,left=300,top=50,resizable=yes');";
                            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Check Out Operation Failed.", AlertType.Warning);
                        }
                    }
                }
            }
        }
        protected void btnDayLetsOk_Click(object sender, EventArgs e)
        {
            int DayLetId;
            HotelGuestDayLetCheckOutDA dayLetDA = new HotelGuestDayLetCheckOutDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<HotelGuestDayLetCheckOutBO> dayLetBOList = new List<HotelGuestDayLetCheckOutBO>();

            string[] parts = this.txtSrcRegistrationIdList.Value.Split(',');
            foreach (string part in parts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    HotelGuestDayLetCheckOutBO dayLetBO = new HotelGuestDayLetCheckOutBO();
                    dayLetBO.CreatedBy = userInformationBO.UserInfoId;
                    dayLetBO.LastModifiedBy = userInformationBO.UserInfoId;
                    dayLetBO.DayLetStatus = "Pending";
                    dayLetBO.DayLetDiscountType = ddlDayLetDiscount.SelectedValue;
                    dayLetBO.DayLetDiscountAmount = Convert.ToDecimal(TodaysRoomBillHiddenField.Value);
                    dayLetBO.DayLetDiscount = Convert.ToDecimal(txtDayLetDiscount.Text);
                    dayLetBO.RoomRate = !string.IsNullOrWhiteSpace(TodaysRoomBillHiddenField.Value) ? Convert.ToDecimal(TodaysRoomBillHiddenField.Value) : 0;
                    dayLetBO.RegistrationId = Int32.Parse(part);
                    dayLetBO.RegistrationIdList = this.txtSrcRegistrationIdList.Value;

                    dayLetBOList.Add(dayLetBO);
                }
            }

            Boolean status = dayLetDA.SaveOrUpdateDayLetsInformation(dayLetBOList, out DayLetId);

            if (status)
            {
                string queryStringId = Request.QueryString["BackFromServiceBill"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ddlRoomId.SelectedValue = queryStringId;
                    this.txtSrcRoomNumber.Text = this.ddlRoomId.SelectedItem.Text;
                    this.lblRegistrationNumber.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.LoadRelatedInformation();
                    this.SavePendingGuestRoomInformation();
                    this.LoadServiceInformationWithControl();
                    this.LoadCheckBoxListServiceInformation();
                }
                else
                {
                    this.SearchClickDetails();
                }
            }
            this.LoadStartAndEndDate();
        }
        //************************ User Defined Function ********************//
        private bool IsRoomBillSettlmentPending()
        {
            bool flag = true;

            if (!string.IsNullOrWhiteSpace(this.ddlRegistrationId.SelectedValue))
            {
                int registrationId = Int32.Parse(this.ddlRegistrationId.SelectedValue);
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
                        this.txtSrcRoomNumber.Focus();
                        flag = false;
                    }
                }
            }
            return flag;
        }
        private void LoadStartAndEndDate()
        {
            string registrationIdList = txtSrcRegistrationIdList.Value;

            if (!string.IsNullOrWhiteSpace(registrationIdList))
            {
                RoomRegistrationBO regBO = new RoomRegistrationBO();
                RoomRegistrationDA regDA = new RoomRegistrationDA();

                regBO = regDA.GetMinimumArrivalDateInfoByRegistrationIdList(registrationIdList);
                if (regBO != null)
                {
                    if (regBO.ArriveDate != null)
                    {
                        if (registrationIdList.Contains(","))
                        {
                            HiddenStartDate.Value = hmUtility.GetStringFromDateTime(regBO.ArriveDate);
                        }
                        else
                        {
                            HiddenStartDate.Value = hmUtility.GetFromDate();
                        }
                    }
                    else
                    {
                        HiddenStartDate.Value = hmUtility.GetFromDate();
                        //HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
                        HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.AddYears(10));
                    }
                }
                else
                {
                    HiddenStartDate.Value = hmUtility.GetFromDate();
                    //HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
                    HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.AddYears(10));
                }

            }
            else
            {
                HiddenStartDate.Value = hmUtility.GetFromDate();
                //HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
                HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.AddYears(10));
            }

            this.txtStartDate.Text = HiddenStartDate.Value;
            this.txtEndDate.Text = HiddenEndDate.Value;
            Session["txtStartDate"] = HiddenStartDate.Value;
        }
        private void LoadLabelForSalesTotal()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            List<CommonCurrencyBO> localCurrencyListBO = new List<CommonCurrencyBO>();
            localCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Local").ToList();
            List<CommonCurrencyBO> UsdCurrencyListBO = new List<CommonCurrencyBO>();
            UsdCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Usd").ToList();

            this.ddlSalesTotalLocal.DataSource = localCurrencyListBO;
            this.ddlSalesTotalLocal.DataTextField = "CurrencyName";
            this.ddlSalesTotalLocal.DataValueField = "CurrencyId";
            this.ddlSalesTotalLocal.DataBind();
            this.ddlSalesTotalLocal.SelectedIndex = 0;
            this.lblSalesTotalLocal.Text = "Sales Amount (" + this.ddlSalesTotalLocal.SelectedItem.Text + ")";
            this.lblGrandTotalLocal.Text = "Grand Total (" + this.ddlSalesTotalLocal.SelectedItem.Text + ")";

            this.ddlSalesTotalUsd.DataSource = UsdCurrencyListBO;
            this.ddlSalesTotalUsd.DataTextField = "CurrencyName";
            this.ddlSalesTotalUsd.DataValueField = "CurrencyId";
            this.ddlSalesTotalUsd.DataBind();
            this.ddlSalesTotalUsd.SelectedIndex = 1;
            this.lblSalesTotalUsd.Text = "Sales Amount (" + this.ddlSalesTotalUsd.SelectedItem.Text + ")";
            this.lblGrandTotalUsd.Text = "Grand Total (" + this.ddlSalesTotalUsd.SelectedItem.Text + ")";
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            this.ddlCompanyBank.DataSource = entityBOList;
            this.ddlCompanyBank.DataTextField = "BankName";
            this.ddlCompanyBank.DataValueField = "BankId";
            this.ddlCompanyBank.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlCompanyBank.Items.Insert(0, itemBank);
        }
        private void SearchButtonClick()
        {
            this.DayLetDiscountInputDiv.Visible = false;
            this.DayLetDiscountOutputDiv.Visible = false;
            this.SearchClickDetails();
            if (this.ddlRegistrationId.SelectedIndex != -1)
            {
                this.SetDayLetsInformation(Int32.Parse(this.ddlRegistrationId.SelectedValue));
            }
            else
            {
                ddlDayLetDiscount.SelectedIndex = 1;
                txtDayLetDiscount.Text = "0";
            }
        }
        private void SetDayLetsInformation(int registrationId)
        {
            chkIsDaysLet.Checked = false;
            HotelGuestDayLetCheckOutBO bo = new HotelGuestDayLetCheckOutBO();
            HotelGuestDayLetCheckOutDA da = new HotelGuestDayLetCheckOutDA();
            bo = da.GetDayLetInformation(registrationId);
            if (bo != null)
            {
                ddlDayLetDiscount.SelectedValue = bo.DayLetDiscountType;
                txtDayLetDiscount.Text = bo.DayLetDiscount.ToString();
                if (bo.DayLetDiscount > 0)
                {
                    chkIsDaysLet.Checked = true;
                }
                this.LoadGridView();
            }
        }
        private void SearchClickDetails()
        {
            txtGuestCheckInDate.Text = string.Empty;
            txtExpectedCheckOutDate.Text = string.Empty;
            this.chkIsBillSplit.Checked = false;
            Session["GuestPaymentDetailListForGrid"] = null;
            this.SearchByRoomNumber();
            this.CalculateSalesTotal();
            //--Remove Paid By RoomId from Room Number List----------------------------------------------
            if (this.ddlRoomId.SelectedIndex != -1)
            {
                AlartMessege.Visible = true;
                //this.ddlPaidByRegistrationId.Items.Remove(ddlPaidByRegistrationId.Items.FindByValue(this.ddlRoomId.SelectedValue));
                this.SavePendingGuestRoomInformation();
                this.LoadServiceInformationWithControl();
                this.LoadCheckBoxListServiceInformation();

                string registrationIdList = string.Empty;
                if (this.ddlRegistrationId.SelectedIndex != -1)
                {
                    if (!string.IsNullOrWhiteSpace(txtSrcRegistrationIdList.Value))
                    {
                        registrationIdList = txtSrcRegistrationIdList.Value;
                    }
                    else
                    {
                        registrationIdList = this.ddlRegistrationId.SelectedValue.ToString();
                    }
                    this.Session["IsBillSplited"] = "0";
                    this.Session["CheckOutRegistrationIdList"] = registrationIdList;
                    this.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
                    this.Session["GuestBillToDate"] = hmUtility.GetToDate();
                }
            }

            decimal salesTotal = !string.IsNullOrWhiteSpace(this.txtSalesTotal.Text) ? Convert.ToDecimal(this.txtSalesTotal.Text) : 0;
            if (salesTotal == 0)
            {
                hfGuestPaymentDetailsInformationDiv.Value = "0";
            }
            else
            {
                hfGuestPaymentDetailsInformationDiv.Value = "1";
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmExpectedDeparture.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            btnAddDetailGuest.Visible = isSavePermission;
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCurrency.Items.Insert(0, item);
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
                    }
                    else
                    {
                        this.txtConversionRate.ReadOnly = false;
                    }
                }
            }
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void IsLocalCurrencyDefaultSelected()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsLocalCurrencyDefaultSelected", "IsLocalCurrencyDefaultSelected");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        CommonCurrencyDA headDA = new CommonCurrencyDA();
                        List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
                        currencyListBO = headDA.GetConversionHeadInfoByType("All");

                        ddlCurrency.DataSource = currencyListBO;
                        ddlCurrency.DataTextField = "CurrencyName";
                        ddlCurrency.DataValueField = "CurrencyId";
                        ddlCurrency.DataBind();

                        CommonCurrencyBO currencyBO = currencyListBO.Where(x => x.CurrencyType == "Local").SingleOrDefault();
                        ddlCurrency.SelectedValue = currencyBO.CurrencyId.ToString();
                    }
                }
            }
        }
        private void LoadRegistrationNumber()
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomRegistrationBO entityBO = new RoomRegistrationBO();
            entityBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(this.txtSrcRoomNumber.Text));
            
            if (entityBO != null)
            {
                txtGuestCheckInDate.Text = hmUtility.GetStringFromDateTime(entityBO.ArriveDate);
                txtExpectedCheckOutDate.Text = hmUtility.GetStringFromDateTime(entityBO.ExpectedCheckOutDate);
            }
            List<RoomRegistrationBO> entityBOList = new List<RoomRegistrationBO>();
            entityBOList.Add(entityBO);

            this.ddlRegistrationId.DataSource = entityBOList;
            this.ddlRegistrationId.DataTextField = "RegistrationNumber";
            this.ddlRegistrationId.DataValueField = "RegistrationId";
            this.ddlRegistrationId.DataBind();

            this.txtSrcRegistrationIdList.Value = this.ddlRegistrationId.SelectedValue.ToString();

            if (!string.IsNullOrWhiteSpace(this.ddlRegistrationId.SelectedValue))
            {
                GuestCompanyBO guestCompanyInfo = new GuestCompanyBO();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
                guestCompanyInfo = guestCompanyDA.GetGuestCompanyInfoByRegistrationId(Convert.ToInt32(this.ddlRegistrationId.SelectedValue));
                if (guestCompanyInfo != null)
                {
                    this.hfGuestCompanyInformation.Value = guestCompanyInfo.CompanyName;
                }
            }

            if (Session["AddedExtraRoomInformation"] != null)
            {
                this.txtSrcRegistrationIdList.Value = this.ddlRegistrationId.SelectedValue.ToString() + "," + Session["AddedExtraRoomInformation"];
            }
            else
            {
                this.txtSrcRegistrationIdList.Value = this.ddlRegistrationId.SelectedValue.ToString();
            }

            if (this.txtSrcRegistrationIdList.Value.Count(x => x == ',') > 0)
            {
                this.txtStartDate.Text = hmUtility.GetStringFromDateTime(hmUtility.GetDateTimeFromString(Session["txtStartDate"].ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddMonths(-3));
            }
        }
        private void LoadRoomNumber()
        {
            int condition = 0;
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            this.ddlRoomId.DataSource = roomNumberDA.GetRoomNumberInfo();
            this.ddlRoomId.DataTextField = "RoomNumber";
            this.ddlRoomId.DataValueField = "RoomId";
            this.ddlRoomId.DataBind();

            ListItem itemRoom = new ListItem();
            itemRoom.Value = "0";
            itemRoom.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomId.Items.Insert(0, itemRoom);

            this.ddlPaidByRegistrationId.DataSource = roomNumberDA.GetRoomNumberInfo();
            this.ddlPaidByRegistrationId.DataTextField = "RoomNumber";
            this.ddlPaidByRegistrationId.DataValueField = "RoomId";
            this.ddlPaidByRegistrationId.DataBind();
            this.ddlPaidByRegistrationId.Items.Insert(0, itemRoom);
        }
        private void LoadRefundAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO PaymentToAccountsInfo = new CustomFieldBO();
            PaymentToAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("PaymentToCustomerForCashOut");
            this.ddlRefundAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + PaymentToAccountsInfo.FieldValue.ToString() + ")");
            this.ddlRefundAccountHead.DataTextField = "NodeHead";
            this.ddlRefundAccountHead.DataValueField = "NodeId";
            this.ddlRefundAccountHead.DataBind();
        }
        private void LoadRoomGridView()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (ddlRegistrationId.SelectedIndex != -1)
            {
                decimal serviceAmountForCompanyGuest = 0;
                GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                string registrationId = txtSrcRegistrationIdList.Value;

                HMCommonDA hmCommonDA = new HMCommonDA();
                string registrationIdList = hmCommonDA.GetRegistrationIdList(registrationId);
                List<GuestHouseCheckOutDetailBO> files = da.GetGuestHouseBill(registrationIdList, "GuestRoom", userInformationBO.UserInfoId);
                txtSrcRegistrationIdList.Value = registrationIdList;

                //foreach (GuestHouseCheckOutDetailBO row in files)
                //{
                //    if (row.NightAuditApproved == "N")
                //    {
                //        DateTime cmpCurrentDateTime = row.ServiceDate;
                //        if (DateTime.Now.Month == cmpCurrentDateTime.Month)
                //        {
                //            if (DateTime.Now.Day == cmpCurrentDateTime.Day)
                //            {
                //                decimal dayLetServiceRate = 0;
                //                decimal calculatedAmount;

                //                decimal serviceRate = row.ServiceRate;
                //                decimal serviceQuantity = row.ServiceQuantity;
                //                decimal vatAmountPercent = row.VatAmountPercent;
                //                decimal serviceChargePercent = row.ServiceChargePercent;
                //                decimal calculatedPercentAmount = row.CalculatedPercentAmount;
                //                decimal totalAmount = row.TotalAmount;

                //                decimal updatedVatAmount = 0;
                //                decimal updatedServiceCharge = 0;
                //                decimal updatedServiceRate = 0;

                //                int IsServiceChargeEnable = 1;
                //                int IsVatEnable = 1;
                //                int InclusiveHotelManagementBill = 0;

                //                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                //                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                //                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InclusiveHotelManagementBill", "Inclusive HotelManagement Bill Setup");
                //                if (commonSetupBO != null)
                //                {
                //                    if (commonSetupBO.SetupId > 0)
                //                    {
                //                        InclusiveHotelManagementBill = Convert.ToInt32(commonSetupBO.SetupValue);
                //                    }
                //                }

                //                if (totalAmount > 0)
                //                {
                //                    decimal dayLetEnteredDiscountAmount = !string.IsNullOrWhiteSpace(this.txtDayLetDiscount.Text) ? Convert.ToDecimal(this.txtDayLetDiscount.Text) : 0;
                //                    if (dayLetEnteredDiscountAmount > 0)
                //                    {
                //                        if (ddlDayLetDiscount.SelectedValue == "Percentage")
                //                        {
                //                            if (Convert.ToDecimal(txtDayLetDiscount.Text) / 100 == 1)
                //                            {
                //                                calculatedAmount = 0;
                //                            }
                //                            else
                //                            {
                //                                if (InclusiveHotelManagementBill == 0)
                //                                {
                //                                    calculatedAmount = serviceRate - (serviceRate * Convert.ToDecimal(txtDayLetDiscount.Text)) / 100;
                //                                }
                //                                else
                //                                {
                //                                    calculatedAmount = totalAmount - (totalAmount * Convert.ToDecimal(txtDayLetDiscount.Text)) / 100;
                //                                }
                //                            }
                //                        }
                //                        else
                //                        {
                //                            if (InclusiveHotelManagementBill == 0)
                //                            {
                //                                calculatedAmount = serviceRate - Convert.ToDecimal(txtDayLetDiscount.Text);
                //                            }
                //                            else
                //                            {
                //                                calculatedAmount = totalAmount - Convert.ToDecimal(txtDayLetDiscount.Text);
                //                            }
                //                        }

                //                        dayLetServiceRate = calculatedAmount;


                //                        if (calculatedAmount != 0)
                //                        {
                //                            if (InclusiveHotelManagementBill == 0)
                //                            {
                //                                updatedServiceCharge = Math.Round(Convert.ToDecimal(calculatedAmount * (serviceChargePercent / 100) * IsServiceChargeEnable), 2, MidpointRounding.AwayFromZero);
                //                                updatedVatAmount = Math.Round(Convert.ToDecimal((calculatedAmount + Convert.ToDecimal(updatedServiceCharge)) * (vatAmountPercent / 100) * IsVatEnable), 2, MidpointRounding.AwayFromZero);
                //                                updatedServiceRate = Math.Round(Convert.ToDecimal(calculatedAmount), 2, MidpointRounding.AwayFromZero);
                //                            }
                //                            else
                //                            {
                //                                updatedVatAmount = Math.Round(Convert.ToDecimal(calculatedAmount * ((vatAmountPercent / (100 + vatAmountPercent)) * IsVatEnable)), 2, MidpointRounding.AwayFromZero);
                //                                updatedServiceCharge = Math.Round(Convert.ToDecimal((calculatedAmount - (updatedVatAmount * IsVatEnable)) * IsServiceChargeEnable * (serviceChargePercent / (100 + serviceChargePercent))), 2, MidpointRounding.AwayFromZero);
                //                                updatedServiceRate = Math.Round(Convert.ToDecimal(calculatedAmount - updatedServiceCharge - updatedVatAmount), 2, MidpointRounding.AwayFromZero);
                //                            }
                //                        }

                //                        // // // Without Round Related Information -------------------------------------------------
                //                        row.ServiceRate = updatedServiceRate;
                //                        row.VatAmount = updatedVatAmount;
                //                        row.ServiceCharge = updatedServiceCharge;
                //                        row.TotalAmount = dayLetServiceRate;
                //                        decimal discountAmount = (totalAmount - dayLetServiceRate);
                //                        decimal totalCalculatedAmount = (row.ServiceRate + row.VatAmount + row.ServiceCharge);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                ////Company Guest Rate Calculation -------------------------------
                //foreach (GuestHouseCheckOutDetailBO row in files)
                //{
                //    if (row.NightAuditApproved.ToString().ToUpper() == "N")
                //    {
                //        RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

                //        roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(this.ddlRegistrationId.SelectedValue));
                //        if (roomRegistrationBO != null)
                //        {
                //            if (roomRegistrationBO.RegistrationId > 0)
                //            {
                //                if (roomRegistrationBO.IsCompanyGuest)
                //                {
                //                    if (row.RegistrationId == roomRegistrationBO.RegistrationId)
                //                    {
                //                        decimal roomRate = row.ServiceRate;
                //                        // // // Without Round Related Information -------------------------------------------------
                //                        row.ServiceRate = (roomRate + row.VatAmount + row.ServiceCharge);
                //                        row.DiscountAmount = (roomRate + row.VatAmount + row.ServiceCharge);
                //                        if (row.ServiceRate == 0)
                //                        {
                //                            row.VatAmount = (serviceAmountForCompanyGuest);
                //                            row.ServiceCharge = (serviceAmountForCompanyGuest);
                //                            row.TotalAmount = (serviceAmountForCompanyGuest);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }

                //    // // // Without Round Related Information -------------------------------------------------
                //    row.TotalAmount = (row.ServiceRate + row.VatAmount + row.ServiceCharge);
                //}

                //List<GuestHouseCheckOutDetailBO> distinctItems = new List<GuestHouseCheckOutDetailBO>();

                //foreach (GuestHouseCheckOutDetailBO row in files)
                //{
                //    if (distinctItems.Count() > 0)
                //    {
                //        var v = (from m in distinctItems where m.RegistrationId == row.RegistrationId && m.ServiceDate.Date == row.ServiceDate.Date && m.ServiceName == row.ServiceName select m).FirstOrDefault();
                //        if (v == null)
                //            distinctItems.Add(row);
                //    }
                //    else
                //    { distinctItems.Add(row); }
                //}

                //this.gvRoomDetail.DataSource = distinctItems;
                //this.gvRoomDetail.DataBind();

                this.gvRoomDetail.DataSource = files;
                this.gvRoomDetail.DataBind();

                if (files.Count > 0)
                {
                    this.CalculateGuestRoomAmountTotal();
                }
                else
                {
                    this.gvRoomDetail.DataSource = null;
                    this.gvRoomDetail.DataBind();
                    this.txtIndividualRoomVatAmount.Text = "0";
                    this.txtIndividualRoomServiceCharge.Text = "0";
                    this.txtIndividualRoomDiscountAmount.Text = "0";
                    this.txtIndividualRoomGrandTotal.Text = "0";
                    this.txtIndividualRoomGrandTotalUSD.Text = "0";
                }
            }
            else
            {
                this.gvRoomDetail.DataSource = null;
                this.gvRoomDetail.DataBind();
                this.txtIndividualRoomVatAmount.Text = "0";
                this.txtIndividualRoomServiceCharge.Text = "0";
                this.txtIndividualRoomDiscountAmount.Text = "0";
                this.txtIndividualRoomGrandTotal.Text = "0";
                this.txtIndividualRoomGrandTotalUSD.Text = "0";
            }
        }
        private void LoadServiceGridView()
        {
            if (ddlRegistrationId.SelectedIndex != -1)
            {
                GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
                string registrationId = txtSrcRegistrationIdList.Value;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonDA hmCommonDA = new HMCommonDA();
                string registrationIdList = hmCommonDA.GetRegistrationIdList(registrationId);
                List<GuestHouseCheckOutDetailBO> files = da.GetGuestServiceInformationForCheckOut(registrationIdList, "GuestService", DateTime.Now, userInformationBO.UserInfoId);
                txtSrcRegistrationIdList.Value = registrationIdList;

                this.gvServiceDetail.DataSource = files.OrderBy(x => x.ServiceDate).ToList();
                this.gvServiceDetail.DataBind();

                if (files.Count > 0)
                {
                    this.CalculateGuestServiceAmountTotal();
                }
                else
                {
                    this.gvServiceDetail.DataSource = null;
                    this.gvServiceDetail.DataBind();

                    this.txtIndividualServiceVatAmount.Text = "0";
                    this.txtIndividualServiceServiceCharge.Text = "0";
                    this.txtIndividualServiceDiscountAmount.Text = "0";
                    this.txtIndividualServiceGrandTotal.Text = "0";
                    this.txtIndividualServiceGrandTotalUSD.Text = "0";
                }
            }
        }
        public void CalculateRestaurantAmountTotal()
        {
            //decimal AmtTotal = 0, AmtTmp;
            //decimal AmtDisTotal = 0, AmtDisTmp;
            //decimal AmtVatTotal = 0, AmtVatTmp;
            //decimal AmtSChargeTotal = 0, AmtSChargeTmp;
            //decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            //for (int i = 0; i < gvRestaurantDetail.Rows.Count; i++)
            //{
            //    AmtTmp = 0;
            //    AmtDisTmp = 0;
            //    AmtVatTmp = 0;
            //    AmtSChargeTmp = 0;
            //    AmtTotalAmountTmp = 0;

            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblServiceRate")).Text, out AmtTmp))
            //        AmtTotal += AmtTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblVatAmount")).Text, out AmtVatTmp))
            //        AmtVatTotal += AmtVatTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblServiceCharge")).Text, out AmtSChargeTmp))
            //        AmtSChargeTotal += AmtSChargeTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblDiscountAmount")).Text, out AmtDisTmp))
            //        AmtDisTotal += AmtDisTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblTotalAmount")).Text, out AmtTotalAmountTmp))
            //        AmtTotalAmount += AmtTotalAmountTmp;
            //}

            //this.txtIndividualRestaurantVatAmount.Text = AmtVatTotal.ToString("#0.00");
            //this.txtIndividualRestaurantServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            //this.txtIndividualRestaurantDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            //this.txtIndividualRestaurantGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
        }
        public void CalculateGuestRoomAmountTotal()
        {
            decimal AmtTotal = 0, AmtTmp;
            decimal AmtDisTotal = 0, AmtDisTmp;
            decimal AmtVatTotal = 0, AmtVatTmp;
            decimal AmtSChargeTotal = 0, AmtSChargeTmp;
            decimal AmtCitySDChargeTotal = 0, AmtCitySDChargeTmp;
            decimal AmtAdditionalChargeTotal = 0, AmtAdditionalChargeTmp;
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;
            decimal AmtTotalAmountUSD = 0, AmtTotalAmountUSDTmp;

            for (int i = 0; i < gvRoomDetail.Rows.Count; i++)
            {
                AmtTmp = 0;
                AmtDisTmp = 0;
                AmtVatTmp = 0;
                AmtSChargeTmp = 0;
                AmtTotalAmountTmp = 0;
                AmtTotalAmountUSDTmp = 0;

                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblServiceRate")).Text, out AmtTmp))
                    AmtTotal += AmtTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblVatAmount")).Text, out AmtVatTmp))
                    AmtVatTotal += AmtVatTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblServiceCharge")).Text, out AmtSChargeTmp))
                    AmtSChargeTotal += AmtSChargeTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblCitySDCharge")).Text, out AmtCitySDChargeTmp))
                    AmtCitySDChargeTotal += AmtCitySDChargeTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblAdditionalCharge")).Text, out AmtAdditionalChargeTmp))
                    AmtAdditionalChargeTotal += AmtAdditionalChargeTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblDiscountAmount")).Text, out AmtDisTmp))
                    AmtDisTotal += AmtDisTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblTotalAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblTotalAmountUSD")).Text, out AmtTotalAmountUSDTmp))
                    AmtTotalAmountUSD += AmtTotalAmountUSDTmp;
            }

            this.txtIndividualRoomVatAmount.Text = AmtVatTotal.ToString("#0.00");
            this.txtIndividualRoomServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            this.txtIndividualRoomCitySDCharge.Text = AmtCitySDChargeTotal.ToString("#0.00");
            this.txtIndividualRoomAdditionalCharge.Text = AmtAdditionalChargeTotal.ToString("#0.00");
            this.txtIndividualRoomDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            this.txtIndividualRoomGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
            this.txtIndividualRoomGrandTotalUSD.Text = AmtTotalAmountUSD.ToString("#0.00");
        }
        public void CalculateGuestServiceAmountTotal()
        {
            decimal AmtTotal = 0, AmtTmp;
            decimal AmtDisTotal = 0, AmtDisTmp;
            decimal AmtVatTotal = 0, AmtVatTmp;
            decimal AmtSChargeTotal = 0, AmtSChargeTmp;

            decimal AmtCitySDChargeTotal = 0, AmtCitySDChargeTmp;
            decimal AmtAdditionalChargeTotal = 0, AmtAdditionalChargeTmp;

            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;
            decimal AmtTotalAmountUSD = 0, AmtTotalAmountUSDTmp;

            for (int i = 0; i < gvServiceDetail.Rows.Count; i++)
            {
                AmtTmp = 0;
                AmtDisTmp = 0;
                AmtVatTmp = 0;
                AmtSChargeTmp = 0;
                AmtTotalAmountTmp = 0;
                AmtTotalAmountUSDTmp = 0;

                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblServiceRate")).Text, out AmtTmp))
                    AmtTotal += AmtTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblVatAmount")).Text, out AmtVatTmp))
                    AmtVatTotal += AmtVatTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblServiceCharge")).Text, out AmtSChargeTmp))
                    AmtSChargeTotal += AmtSChargeTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblCitySDCharge")).Text, out AmtCitySDChargeTmp))
                    AmtCitySDChargeTotal += AmtCitySDChargeTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblAdditionalCharge")).Text, out AmtAdditionalChargeTmp))
                    AmtAdditionalChargeTotal += AmtAdditionalChargeTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblDiscountAmount")).Text, out AmtDisTmp))
                    AmtDisTotal += AmtDisTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblTotalAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblTotalAmountUSD")).Text, out AmtTotalAmountUSDTmp))
                    AmtTotalAmountUSD += AmtTotalAmountUSDTmp;
            }

            this.txtIndividualServiceVatAmount.Text = AmtVatTotal.ToString("#0.00");
            this.txtIndividualServiceServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            this.txtIndividualServiceCitySDCharge.Text = AmtCitySDChargeTotal.ToString("#0.00");
            this.txtIndividualServiceAdditionalCharge.Text = AmtCitySDChargeTotal.ToString("#0.00");
            this.txtIndividualServiceDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            this.txtIndividualServiceGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
            this.txtIndividualServiceGrandTotalUSD.Text = AmtTotalAmountUSD.ToString("#0.00");
        }
        public void CalculateExtraRoomBillAmountTotal()
        {
            decimal AmtDisTotal = 0, AmtDisTmp;
            decimal AmtVatTotal = 0, AmtVatTmp;
            decimal AmtSChargeTotal = 0, AmtSChargeTmp;
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            for (int i = 0; i < gvExtraRoomDetail.Rows.Count; i++)
            {
                AmtDisTmp = 0;
                AmtVatTmp = 0;
                AmtSChargeTmp = 0;
                AmtTotalAmountTmp = 0;

                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraVatAmount")).Text, out AmtVatTmp))
                    AmtVatTotal += AmtVatTmp;
                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraServiceCharge")).Text, out AmtSChargeTmp))
                    AmtSChargeTotal += AmtSChargeTmp;
                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraDiscountAmount")).Text, out AmtDisTmp))
                    AmtDisTotal += AmtDisTmp;
                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraTotalAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
            }

            this.txtIndividualExtraRoomVatAmount.Text = AmtVatTotal.ToString("#0.00");
            this.txtIndividualExtraRoomServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            this.txtIndividualExtraRoomDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            this.txtIndividualExtraRoomGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
        }
        public void CalculateSalesTotal()
        {

            // Vat Total Calculation-----------
            decimal calculatedGuestRoomVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualRoomVatAmount.Text) : 0;
            decimal calculatedGuestServiceVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualServiceVatAmount.Text) : 0;
            decimal calculatedRestaurantVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantVatAmount.Text) : 0;
            decimal calculatedExtraRoomVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomVatAmount.Text) : 0;

            decimal calculatedVatTotal = calculatedGuestRoomVatTotal + calculatedGuestServiceVatTotal + calculatedRestaurantVatTotal + calculatedExtraRoomVatTotal;
            this.txtVatTotal.Text = calculatedVatTotal.ToString("#0.00");

            // Service Charge Total Calculation-----------
            decimal calculatedGuestRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualRoomServiceCharge.Text) : 0;
            decimal calculatedGuestServiceServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualServiceServiceCharge.Text) : 0;
            decimal calculatedRestaurantServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantServiceCharge.Text) : 0;
            decimal calculatedExtraRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomServiceCharge.Text) : 0;

            decimal calculatedServiceChargeTotal = calculatedGuestRoomServiceChargeTotal + calculatedGuestServiceServiceChargeTotal + calculatedRestaurantServiceChargeTotal + calculatedExtraRoomServiceChargeTotal;
            this.txtServiceChargeTotal.Text = calculatedServiceChargeTotal.ToString("#0.00");

            // City Or SD Charge Total Calculation-----------
            decimal calculatedGuestRoomCitySDChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomCitySDCharge.Text) ? Convert.ToDecimal(this.txtIndividualRoomCitySDCharge.Text) : 0;
            decimal calculatedGuestServiceCitySDChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceCitySDCharge.Text) ? Convert.ToDecimal(this.txtIndividualServiceCitySDCharge.Text) : 0;
            decimal calculatedRestaurantCitySDChargeTotal = 0;
            //decimal calculatedRestaurantCitySDChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantCitySDCharge.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantCitySDCharge.Text) : 0;
            decimal calculatedExtraRoomCitySDChargeTotal = 0;
            //decimal calculatedExtraRoomCitySDChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomCitySDCharge.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomCitySDCharge.Text) : 0;

            decimal calculatedCitySDChargeTotal = calculatedGuestRoomCitySDChargeTotal + calculatedGuestServiceCitySDChargeTotal + calculatedRestaurantCitySDChargeTotal + calculatedExtraRoomCitySDChargeTotal;
            this.txtCitySDChargeTotal.Text = calculatedCitySDChargeTotal.ToString("#0.00");

            // Additional Charge Total Calculation-----------
            decimal calculatedGuestRoomAdditionalChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomAdditionalCharge.Text) ? Convert.ToDecimal(this.txtIndividualRoomAdditionalCharge.Text) : 0;
            decimal calculatedGuestServiceAdditionalChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceAdditionalCharge.Text) ? Convert.ToDecimal(this.txtIndividualServiceAdditionalCharge.Text) : 0;
            decimal calculatedRestaurantAdditionalChargeTotal = 0;
            //decimal calculatedRestaurantAdditionalChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantAdditionalCharge.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantAdditionalCharge.Text) : 0;
            decimal calculatedExtraRoomAdditionalChargeTotal = 0;
            //decimal calculatedExtraRoomAdditionalChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomAdditionalCharge.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomAdditionalCharge.Text) : 0;

            decimal calculatedAdditionalChargeTotal = calculatedGuestRoomAdditionalChargeTotal + calculatedGuestServiceAdditionalChargeTotal + calculatedRestaurantAdditionalChargeTotal + calculatedExtraRoomAdditionalChargeTotal;
            this.txtAdditionalChargeTotal.Text = calculatedAdditionalChargeTotal.ToString("#0.00");

            // Discount Total Calculation-----------
            decimal calculatedGuestRoomDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualRoomDiscountAmount.Text) : 0;
            decimal calculatedGuestServiceDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualServiceDiscountAmount.Text) : 0;
            decimal calculatedRestaurantDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantDiscountAmount.Text) : 0;
            decimal calculatedExtraRoomDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomDiscountAmount.Text) : 0;

            decimal calculatedDiscountTotal = calculatedGuestRoomDiscountTotal + calculatedGuestServiceDiscountTotal + calculatedRestaurantDiscountTotal + calculatedExtraRoomDiscountTotal;
            this.txtDiscountAmountTotal.Text = calculatedDiscountTotal.ToString("#0.00");

            // Sales Total Calculation-----------
            decimal calculatedGuestRoomTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualRoomGrandTotal.Text) : 0;
            decimal calculatedGuestServiceTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualServiceGrandTotal.Text) : 0;
            decimal calculatedRestaurantTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantGrandTotal.Text) : 0;
            //decimal calculatedExtraRoomTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomGrandTotal.Text) : 0;
            decimal calculatedExtraRoomTotal = 0;
            decimal calculatedAdvancePaymentAmountTotal = !string.IsNullOrWhiteSpace(this.txtAdvancePaymentAmount.Text) ? Convert.ToDecimal(this.txtAdvancePaymentAmount.Text) : 0;

            //decimal calculatedSalesTotal = (calculatedGuestRoomTotal + calculatedGuestServiceTotal + calculatedRestaurantTotal + calculatedExtraRoomTotal) - calculatedAdvancePaymentAmountTotal - calculatedDiscountTotal;
            decimal calculatedSalesTotal = (calculatedGuestRoomTotal + calculatedGuestServiceTotal + calculatedRestaurantTotal + calculatedExtraRoomTotal) - calculatedAdvancePaymentAmountTotal;
            this.txtSalesTotal.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            this.HiddenFieldSalesTotal.Value = Math.Round(calculatedSalesTotal).ToString("#0.00");

            //USD Calculation ------------------------------------------------------------------
            decimal calculatedExtraRoomTotalUSD = 0;
            decimal calculatedAdvancePaymentAmountTotalUSD = !string.IsNullOrWhiteSpace(this.txtAdvancePaymentAmountUSD.Text) ? Convert.ToDecimal(this.txtAdvancePaymentAmountUSD.Text) : 0;
            decimal calculatedGuestRoomTotalUSD = !string.IsNullOrWhiteSpace(this.txtIndividualRoomGrandTotalUSD.Text) ? Convert.ToDecimal(this.txtIndividualRoomGrandTotalUSD.Text) : 0;
            decimal calculatedGuestServiceTotalUSD = !string.IsNullOrWhiteSpace(this.txtIndividualServiceGrandTotalUSD.Text) ? Convert.ToDecimal(this.txtIndividualServiceGrandTotalUSD.Text) : 0;
            decimal calculatedRestaurantTotalUSD = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantGrandTotalUSD.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantGrandTotalUSD.Text) : 0;
            decimal calculatedSalesTotalUSD = (calculatedGuestRoomTotalUSD + calculatedGuestServiceTotalUSD + calculatedRestaurantTotalUSD + calculatedExtraRoomTotalUSD) - calculatedAdvancePaymentAmountTotalUSD;
            this.txtSalesTotalUsd.Text = Math.Round(calculatedSalesTotalUSD).ToString("#0.00");
            //this.txtSalesTotalUsd.Text = calculatedSalesTotalUSD.ToString("#0.00");

            this.txtDiscountAmount.Text = "0.00";
            this.txtGrandTotal.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            this.HiddenFieldGrandTotal.Value = Math.Round(calculatedSalesTotal).ToString("#0.00");


            //decimal conversionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
            if (!string.IsNullOrWhiteSpace(this.ddlRegistrationId.SelectedValue))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomRegistrationBO registrationBO = new RoomRegistrationBO();
                registrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(this.ddlRegistrationId.SelectedValue));
                this.txtConversionRate.Text = registrationBO.ConversionRate.ToString();
                //this.txtConversionRateHiddenField.Value = registrationBO.ConversionRate.ToString();
                this.btnLocalBillPreview.Text = "Bill Preview" + " (" + registrationBO.LocalCurrencyHead + ")";
                this.btnUSDBillPreview.Text = "Bill Preview (USD)";
            }
            //decimal conversionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
            //if (conversionRate > 0)
            //{
            //    this.lblSalesTotalUsd.Visible = true;
            //    this.txtSalesTotalUsd.Visible = true;
            //    this.lblGrandTotalUsd.Visible = true;
            //    this.txtGrandTotalUsd.Visible = true;
            //    this.txtSalesTotalUsd.Text = (calculatedSalesTotal / conversionRate).ToString("#0.00"); //Math.Round(calculatedSalesTotal / conversionRate).ToString("#0.00");
            //    this.hfTxtSalesTotalUsd.Value = this.txtSalesTotalUsd.Text;
            //    this.txtGrandTotalUsd.Text = this.txtSalesTotalUsd.Text;
            //    this.hfGrandTotalUsd.Value = this.txtSalesTotalUsd.Text;
            //}
            //else
            //{
            //    this.txtSalesTotalUsd.Text = "0.00";
            //    this.lblSalesTotalUsd.Visible = false;
            //    this.txtSalesTotalUsd.Visible = false;
            //    this.lblGrandTotalUsd.Visible = false;
            //    this.txtGrandTotalUsd.Visible = false;
            //}
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtBillDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            if (Session["txtStartDate"] != null)
            {
                this.txtStartDate.Text = Session["txtStartDate"].ToString();
            }
            else
            {
                this.txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            //this.txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime.AddYears(10));
            this.txtBillDate.Enabled = false;

            HiddenStartDate.Value = this.txtStartDate.Text;
            //HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
            HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.AddYears(10));
        }
        private void LoadGridView()
        {
        }
        private void Clear()
        {
            txtBillDate.Text = string.Empty;
            txtGuestCheckInDate.Text = string.Empty;
            txtExpectedCheckOutDate.Text = string.Empty;
            txtVatTotal.Text = string.Empty;
            txtServiceChargeTotal.Text = string.Empty;
            txtDiscountAmountTotal.Text = string.Empty;
            txtAdvancePaymentAmount.Text = string.Empty;
            txtSalesTotal.Text = string.Empty;
            txtSalesTotalUsd.Text = string.Empty;
            txtRebateRemarks.Text = string.Empty;
            txtIndividualRoomVatAmount.Text = string.Empty;
            txtIndividualRoomServiceCharge.Text = string.Empty;
            txtIndividualRoomDiscountAmount.Text = string.Empty;
            txtIndividualRoomGrandTotal.Text = string.Empty;
            txtIndividualRoomGrandTotalUSD.Text = string.Empty;
            txtIndividualServiceVatAmount.Text = string.Empty;
            txtIndividualServiceServiceCharge.Text = string.Empty;
            txtIndividualServiceDiscountAmount.Text = string.Empty;
            txtIndividualServiceGrandTotal.Text = string.Empty;
            txtIndividualServiceGrandTotalUSD.Text = string.Empty;
            ddlRegistrationId.SelectedIndex = -1;
            //SearchClickDetails();

            gvServiceDetail.DataSource = null;
            gvServiceDetail.DataBind();

            gvRoomDetail.DataSource = null;
            gvRoomDetail.DataBind();

        }
        private void Cancel()
        {
            txtSrcRegistrationIdList.Value = string.Empty;
            hfGuestPaymentDetailsInformationDiv.Value = "1";
            this.LoadCurrentDate();
            this.ddlPayMode.SelectedIndex = 0;
            this.ddlChecquePaymentAccountHeadId.SelectedIndex = 0;
            this.txtChecqueNumber.Text = string.Empty;
            this.ddlCardPaymentAccountHeadId.SelectedIndex = 0;
            this.ddlCardType.SelectedIndex = 0;
            this.txtCardNumber.Text = string.Empty;
            this.txtCardHolderName.Text = string.Empty;
            this.txtExpireDate.Text = string.Empty;
            this.ddlBankId.SelectedValue = "0";
            this.btnSave.Text = "Save";
            this.ddlRegistrationId.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            decimal grandTotal = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;
            decimal totalPaid = !string.IsNullOrWhiteSpace(HiddenFieldTotalPaid.Value) ? Convert.ToDecimal(HiddenFieldTotalPaid.Value) : 0;
            if (grandTotal != totalPaid)
            {
                CommonHelper.AlertInfo(innboardMessage, "Grand Total and Guest Payment Amount is not Equal..", AlertType.Warning);
                this.ddlPayMode.Focus();
                flag = false;
                return flag;
            }


            if (string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                flag = false;
            }
            else if (this.ddlRoomId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                flag = false;
            }
            else if (this.ddlPayMode.SelectedValue == "0")
            {
                if (grandTotal > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Payment Mode.", AlertType.Warning);
                    this.ddlPayMode.Focus();
                    flag = false;
                }
            }
            else if (isSingle == false)
            {
                if (this.ddlGLCompany.SelectedValue == "0")
                {
                    Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                    if (isIntegrated)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Company Name.", AlertType.Warning);
                        this.ddlGLCompany.Focus();
                        flag = false;
                    }
                }
                else if (this.ddlGLCompany.SelectedValue != "0")
                {
                    if (this.ddlGLProject.SelectedValue == "0")
                    {
                        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                        if (isIntegrated)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Project Name.", AlertType.Warning);
                            this.ddlGLProject.Focus();
                            flag = false;
                        }
                    }
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "", AlertType.Warning);
            }
            return flag;
        }
        private void DeleteData(int sEmpId)
        {
        }
        private void FillForm(int EditId)
        {
        }
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
                if (roomId > 0)
                {
                    this.LoadRegistrationNumber();
                    this.LoadRoomGridView();
                    this.LoadServiceGridView();
                    this.LoadPaymentInformation();
                    this.LoadFormByRegistrationNumber();
                    this.CalculateSalesTotal();
                    this.Session["IsCheckOutBillPreview"] = null;
                }
            }
        }
        private void LoadPaymentInformation()
        {
            if (ddlRegistrationId.SelectedIndex != -1)
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                string registrationId = this.ddlRegistrationId.SelectedValue.ToString();
                string registrationIdList = hmCommonDA.GetRegistrationIdList(registrationId);

                if (Session["AddedExtraRoomInformation"] != null)
                {
                    registrationIdList = registrationIdList + "," + Session["AddedExtraRoomInformation"];
                    List<HotelGuestDayLetCheckOutBO> registrationIdBOList = new List<HotelGuestDayLetCheckOutBO>();
                    this.txtSrcRegistrationIdList.Value = string.Empty;
                    int totalRoomCheckOut = registrationIdList.Split(',').Length - 1;
                    for (int i = 0; i <= totalRoomCheckOut; i++)
                    {
                        string RegisInfo = registrationIdList.Split(',')[i];
                        if (string.IsNullOrWhiteSpace(RegisInfo))
                        {
                            return;
                        }

                        HotelGuestDayLetCheckOutBO entityBO = new HotelGuestDayLetCheckOutBO();
                        entityBO.RegistrationId = Convert.ToInt32(registrationIdList.Split(',')[i]);

                        bool alreadyExists = registrationIdBOList.Any(x => x.RegistrationId == entityBO.RegistrationId);
                        if (!alreadyExists)
                        {
                            registrationIdBOList.Add(entityBO);
                            if (string.IsNullOrWhiteSpace(this.txtSrcRegistrationIdList.Value))
                            {
                                this.txtSrcRegistrationIdList.Value = entityBO.RegistrationId.ToString();
                            }
                            else
                            {
                                this.txtSrcRegistrationIdList.Value = this.txtSrcRegistrationIdList.Value + "," + entityBO.RegistrationId.ToString();
                            }
                        }
                    }
                }
                else
                {
                    this.txtSrcRegistrationIdList.Value = this.ddlRegistrationId.SelectedValue.ToString();
                }

                //PaymentSummaryBO paymentSummaryBO = new PaymentSummaryBO();
                //GuestBillPaymentDA paymentSummaryDA = new GuestBillPaymentDA();
                //string paymentRelatedRegistrationIdList = hmCommonDA.GetRegistrationIdList(registrationIdList);
                //paymentSummaryBO = paymentSummaryDA.GetGuestBillPaymentSummaryInfoByRegiIdList(paymentRelatedRegistrationIdList, 0);
                //this.txtAdvancePaymentAmount.Text = Math.Round(paymentSummaryBO.TotalPayment).ToString();

                List<PaymentSummaryBO> paymentSummaryBOList = new List<PaymentSummaryBO>();
                GuestBillPaymentDA paymentSummaryDA = new GuestBillPaymentDA();
                //paymentSummaryBO = paymentSummaryDA.GetGuestBillPaymentSummaryInfoByRegiIdList(this.txtSrcRegistrationIdList.Value, 0);

                decimal paymentAmount = 0;
                decimal paymentAmountUSD = 0;
                string paymentRelatedRegistrationIdList = hmCommonDA.GetRegistrationIdList(registrationIdList);
                paymentSummaryBOList = paymentSummaryDA.GetGuestBillPaymentSummaryInfoByRegiIdList(paymentRelatedRegistrationIdList, 0);

                if (paymentSummaryBOList != null)
                {
                    if (paymentSummaryBOList.Count > 0)
                    {
                        paymentAmount = paymentSummaryBOList.Sum(s => s.DebitBalance - s.CreditBalance);

                        foreach (PaymentSummaryBO row in paymentSummaryBOList)
                        {
                            if (((row.DebitBalance - row.CreditBalance) != 0) && (row.CurrencyExchangeRate != 0))
                            {
                                paymentAmountUSD += (row.DebitBalance - row.CreditBalance) / row.CurrencyExchangeRate;
                            }
                        }
                    }
                }

                this.txtAdvancePaymentAmount.Text = Math.Round(paymentAmount).ToString();
                //this.txtAdvancePaymentAmountUSD.Text = Math.Round(paymentAmountUSD).ToString();
                this.txtAdvancePaymentAmountUSD.Text = paymentAmountUSD.ToString("0.00");
            }
            else
            {
                this.txtAdvancePaymentAmount.Text = "0";
                this.txtAdvancePaymentAmountUSD.Text = "0";
            }
        }
        private void LoadFormByRegistrationNumber()
        {
            // Need to Fix------------------------------------------------------------------
            if (this.ddlRegistrationId.SelectedIndex != -1)
            {
                lblMessage.Text = "";
                string registrationId = txtSrcRegistrationIdList.Value;
                List<GuestHouseCheckOutBO> guestHouseCheckOutBO = new List<GuestHouseCheckOutBO>();
                GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();

                guestHouseCheckOutBO = guestHouseCheckOutDA.GetGuestHouseCheckOutInfoByPaidBy(registrationId, Convert.ToInt32(this.ddlRegistrationId.SelectedValue));
                if (guestHouseCheckOutBO.Count > 0)
                {
                    this.gvExtraRoomDetail.DataSource = guestHouseCheckOutBO;
                    this.gvExtraRoomDetail.DataBind();
                }
                else
                {
                    this.gvExtraRoomDetail.DataSource = null;
                    this.gvExtraRoomDetail.DataBind();
                }
            }
            else
            {
                this.gvExtraRoomDetail.DataSource = null;
                this.gvExtraRoomDetail.DataBind();
            }

            this.CalculateExtraRoomBillAmountTotal();
        }
        private void SearchByRoomNumber()
        {
            this.lblMessage.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoForBillHoldUpByRegistrationId(Convert.ToInt32(this.txtSrcRoomNumber.Text));
                if (roomAllocationBO.RoomId > 0)
                {
                    this.ddlRoomId.SelectedValue = roomAllocationBO.RoomId.ToString();
                    this.lblRegistrationNumber.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.txtConversionRate.Text = roomAllocationBO.ConversionRate.ToString();
                    this.btnLocalBillPreview.Text = "Bill Preview" + " (" + roomAllocationBO.LocalCurrencyHead + ")";
                    this.btnUSDBillPreview.Text = "Bill Preview (USD)";

                    this.txtStartDate.Text = hmUtility.GetStringFromDateTime(roomAllocationBO.ArriveDate);
                    HiddenStartDate.Value = this.txtStartDate.Text;
                    //HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
                    HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.AddYears(10));
                    Session["txtStartDate"] = this.txtStartDate.Text;
                    this.LoadRelatedInformation();

                    //--Remove Paid By RoomId from Room Number List----------------------------------------------
                    if (this.ddlPayMode.SelectedIndex != -1)
                    {
                        this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Company"));
                        this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Cheque"));
                    }

                    this.HiddenFieldCompanyPaymentButtonInfo.Value = "0";

                    if (roomAllocationBO.CompanyId > 0)
                    {
                        this.ddlCompanyPaymentAccountHead.SelectedValue = roomAllocationBO.NodeId.ToString();

                        ListItem itemRoom = new ListItem();
                        itemRoom.Value = "Company";
                        itemRoom.Text = "Company";
                        this.ddlPayMode.Items.Insert(4, itemRoom);

                        ListItem chequeItem = new ListItem("Cheque", "Cheque", true);
                        this.ddlPayMode.Items.Add(chequeItem);

                        this.ddlCompanyPaymentAccountHead.Enabled = false;
                        this.HiddenFieldCompanyPaymentButtonInfo.Value = "1";
                    }

                    HMCommonSetupBO isBKashPaymentModeEnableBO = new HMCommonSetupBO();
                    HMCommonSetupDA isBKashPaymentModeEnableDA = new HMCommonSetupDA();
                    isBKashPaymentModeEnableBO = isBKashPaymentModeEnableDA.GetCommonConfigurationInfo("IsBKashPaymentModeEnable", "IsBKashPaymentModeEnable");
                    if (isBKashPaymentModeEnableBO != null)
                    {
                        if (isBKashPaymentModeEnableBO.SetupValue == "0")
                        {
                            this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("bKash"));
                        }
                    }

                    Session["HiddenFieldCompanyPaymentButtonInfo"] = this.HiddenFieldCompanyPaymentButtonInfo.Value;
                }
                else
                {
                    this.lblRegistrationNumber.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                }
            }
            else
            {
                this.lblRegistrationNumber.Visible = false;
                this.ddlRegistrationId.Visible = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
            }
        }
        private void LoadSingleProjectAndCompany()
        {
            this.LoadGLCompany(true);
            this.LoadGLProject(true);
        }
        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (isSingle == true)
            {
                companyList.Add(List[0]);
                this.ddlGLCompany.DataSource = companyList;
                this.ddlGLCompany.DataTextField = "Name";
                this.ddlGLCompany.DataValueField = "CompanyId";
                this.ddlGLCompany.DataBind();
            }
            else
            {
                this.ddlGLCompany.DataSource = List;
                this.ddlGLCompany.DataTextField = "Name";
                this.ddlGLCompany.DataValueField = "CompanyId";
                this.ddlGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                this.ddlGLCompany.Items.Insert(0, itemCompany);
            }
        }
        private void LoadGLProject(bool isSingle)
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            var List = entityDA.GetAllGLProjectInfo();
            if (isSingle == true)
            {
                projectList.Add(List[0]);
                this.ddlGLProject.DataSource = projectList;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();
            }
            else
            {
                this.ddlGLProject.DataSource = List;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();

                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstValue();
                this.ddlGLProject.Items.Insert(0, itemProject);
            }
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            this.lblPaymentAccountHead.Text = "Payment Receive In";

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            CommonPaymentModeBO cardPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Card").FirstOrDefault();
            CommonPaymentModeBO chequePaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cheque").FirstOrDefault();
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            CommonPaymentModeBO mBankingPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "M-Banking").FirstOrDefault();

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            this.ddlCardPaymentAccountHeadId.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCardPaymentAccountHeadId.DataTextField = "NodeHead";
            this.ddlCardPaymentAccountHeadId.DataValueField = "NodeId";
            this.ddlCardPaymentAccountHeadId.DataBind();

            this.ddlChecquePaymentAccountHeadId.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlChecquePaymentAccountHeadId.DataTextField = "NodeHead";
            this.ddlChecquePaymentAccountHeadId.DataValueField = "NodeId";
            this.ddlChecquePaymentAccountHeadId.DataBind();

            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + companyPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();

            //this.ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + mBankingPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            //this.ddlMBankingReceiveAccountsInfo.DataTextField = "NodeHead";
            //this.ddlMBankingReceiveAccountsInfo.DataValueField = "NodeId";
            //this.ddlMBankingReceiveAccountsInfo.DataBind();
        }
        private void LoadCompanyChequeHeadInfo()
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
            List<RoomAlocationBO> roomAllocationBOList = new List<RoomAlocationBO>();
            roomAllocationBOList.Add(roomAllocationBO);

            this.ddlChecquePaymentAccountHeadId.DataSource = roomAllocationBOList;
            this.ddlChecquePaymentAccountHeadId.DataTextField = "CompanyName";
            this.ddlChecquePaymentAccountHeadId.DataValueField = "AccountHeadCompanyId";
            this.ddlChecquePaymentAccountHeadId.DataBind();
        }
        //Old Voucher Post Not Used-----------------
        private void Old_VoucherPost(int transactionId, string transactionHead, out int tmpGLMasterId)
        {
            int cashBankLedgerMode = 1;
            int oppusiteLedgerMode = 2;
            decimal calculatedSalesTotal = !string.IsNullOrWhiteSpace(this.txtSalesTotal.Text) ? Convert.ToDecimal(this.txtSalesTotal.Text) : 0;

            if (calculatedSalesTotal > 0)
            {
                cashBankLedgerMode = 1;
                oppusiteLedgerMode = 2;
            }
            else
            {
                cashBankLedgerMode = 2;
                oppusiteLedgerMode = 1;
            }

            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();
            GLLedgerBO detailBODebit = new GLLedgerBO();
            detailBODebit.LedgerId = 0;

            if (this.ddlPayMode.SelectedIndex == 0)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlCashReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlCashReceiveAccountsInfo.SelectedItem.Text;
            }
            else if (this.ddlPayMode.SelectedIndex == 1)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlCardReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlCardReceiveAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = cashBankLedgerMode;
            detailBODebit.FieldId = LocalCurrencyId;
            detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtSalesTotal.Text) ? Convert.ToDecimal(this.txtSalesTotal.Text) : 0;
            detailBODebit.CurrencyAmount = null;
            detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtSalesTotal.Text) ? Convert.ToDecimal(this.txtSalesTotal.Text) : 0;
            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = transactionId;
            detailBOCredit.NodeHead = transactionHead;
            detailBOCredit.LedgerMode = oppusiteLedgerMode;
            detailBOCredit.FieldId = LocalCurrencyId;
            detailBOCredit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtSalesTotal.Text) ? Convert.ToDecimal(this.txtSalesTotal.Text) : 0;
            detailBOCredit.CurrencyAmount = null;
            detailBOCredit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtSalesTotal.Text) ? Convert.ToDecimal(this.txtSalesTotal.Text) : 0;
            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);

            Boolean isCash = true;
            if (this.ddlPayMode.SelectedItem.Text != "Cash")
            {
                isCash = false;
            }

            string isReceivedOrPayment = "Received";
            if (cashBankLedgerMode == 1)
            {
                glMasterBO.VoucherMode = 2;
                isReceivedOrPayment = "Received";
                if (isCash)
                {
                    glMasterBO.CashChequeMode = 1;
                    glMasterBO.VoucherType = "CR";
                }
                else
                {
                    glMasterBO.CashChequeMode = 2;
                    glMasterBO.VoucherType = "BR";
                }
            }
            else
            {
                glMasterBO.VoucherMode = 1;
                isReceivedOrPayment = "Payment";
                if (isCash)
                {
                    glMasterBO.CashChequeMode = 1;
                    glMasterBO.VoucherType = "CP";
                }
                else
                {
                    glMasterBO.VoucherType = "BP";
                    glMasterBO.CashChequeMode = 2;
                }
            }

            //glMasterBO.VoucherNo = "--------------------";
            glMasterBO.VoucherDate = !string.IsNullOrWhiteSpace(this.txtBillDate.Text) ? hmUtility.GetDateTimeFromString(this.txtBillDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
            glMasterBO.PayerOrPayee = "";
            glMasterBO.Narration = "Amount " + isReceivedOrPayment + " for the Registration Number: " + ddlRegistrationId.SelectedItem.Text;

            // Voucher Approved Information ------------------------------------------------------------------------------------Start
            HMCommonSetupBO commonSetupBOCheckedBy = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOApprovedBy = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOCheckedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "CheckedBySetup");
            commonSetupBOApprovedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "ApprovedBySetup");
            List<GLVoucherApprovedInfoBO> approvedBOList = new List<GLVoucherApprovedInfoBO>();
            // CheckedBy -----------------
            if (commonSetupBOCheckedBy != null)
            {
                if (commonSetupBOCheckedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOCheckedBy = new GLVoucherApprovedInfoBO();
                    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(commonSetupBOCheckedBy.SetupValue);
                    approvedBOList.Add(approvedBOCheckedBy);
                }
            }

            // ApprovedBy -----------------
            if (commonSetupBOApprovedBy != null)
            {
                if (commonSetupBOApprovedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOApprovedBy = new GLVoucherApprovedInfoBO();
                    approvedBOApprovedBy.ApprovedType = "ApprovedBy";
                    approvedBOApprovedBy.UserInfoId = Convert.ToInt32(commonSetupBOApprovedBy.SetupValue);
                    approvedBOList.Add(approvedBOApprovedBy);
                }
            }
            // Voucher Approved Information ------------------------------------------------------------------------------------End
            string currentVoucherNo = string.Empty;
            glMasterBO.CreatedBy = userInformationBO.UserInfoId;
            Boolean status = glMasterDA.SaveGLMasterInfo(glMasterBO, out tmpGLMasterId, out currentVoucherNo, Session["TransactionDetailList"] as List<GLLedgerBO>, approvedBOList);
        }
        private void VoucherPost(out int tmpGLMasterId)
        {
            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            // -- Room Information ----------
            foreach (GridViewRow row in gvRoomDetail.Rows)
            {
                GLLedgerBO detailBODebit = new GLLedgerBO();
                Label isNightAuditApproved = (Label)row.FindControl("lblNightAuditApproved");

                detailBODebit.LedgerId = 0;
                detailBODebit.NodeId = Int64.Parse(((Label)row.FindControl("lblIncomeNodeId")).Text);
                detailBODebit.NodeHead = ((Label)row.FindControl("lblServiceName")).Text;
                detailBODebit.LedgerMode = 1;
                detailBODebit.FieldId = LocalCurrencyId;
                detailBODebit.LedgerDebitAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);
                detailBODebit.CurrencyAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);
                detailBODebit.LedgerAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);
                detailBODebit.NodeNarration = "";

                ledgerDetailListBOForDebit.Add(detailBODebit);
                Session["TransactionDetailList"] = ledgerDetailListBOForDebit;
            }

            // -- Service Information ----------
            foreach (GridViewRow row in gvServiceDetail.Rows)
            {
                GLLedgerBO detailBODebit = new GLLedgerBO();
                Label isNightAuditApproved = (Label)row.FindControl("lblNightAuditApproved");

                detailBODebit.LedgerId = 0;
                detailBODebit.NodeId = Int32.Parse(((Label)row.FindControl("lblIncomeNodeId")).Text);
                detailBODebit.LedgerMode = 1;
                detailBODebit.FieldId = LocalCurrencyId;
                detailBODebit.LedgerDebitAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);
                detailBODebit.CurrencyAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);
                detailBODebit.LedgerAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);
                detailBODebit.NodeNarration = "";

                ledgerDetailListBOForDebit.Add(detailBODebit);
                Session["TransactionDetailList"] = ledgerDetailListBOForDebit;
            }

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            // -- Guest Payment Information---------
            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            foreach (GuestBillPaymentBO row in guestPaymentDetailListForGrid)
            {
                GLLedgerBO detailBODebit = new GLLedgerBO();

                detailBOCredit.LedgerId = 0;
                detailBOCredit.NodeId = row.NodeId;
                detailBOCredit.LedgerMode = 2;
                detailBOCredit.FieldId = Convert.ToInt32(this.ddlCardReceiveAccountsInfo.SelectedValue); //45 for Local Currency
                detailBOCredit.LedgerDebitAmount = Convert.ToDecimal(row.PaymentAmount);
                detailBOCredit.CurrencyAmount = row.PaymentAmount;
                detailBOCredit.LedgerAmount = Convert.ToDecimal(row.PaymentAmount);
                detailBOCredit.NodeNarration = "";

                ledgerDetailListBOForDebit.Add(detailBOCredit);
                Session["TransactionDetailList"] = ledgerDetailListBOForDebit;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);

            glMasterBO.CashChequeMode = 1;
            glMasterBO.VoucherType = "JV";
            glMasterBO.CashChequeMode = 1;
            glMasterBO.VoucherMode = 3;

            glMasterBO.VoucherDate = !string.IsNullOrWhiteSpace(this.txtBillDate.Text) ? hmUtility.GetDateTimeFromString(this.txtBillDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
            glMasterBO.PayerOrPayee = "";
            glMasterBO.Narration = "Room Check-Out Posting for the Registration Number: " + ddlRegistrationId.SelectedItem.Text;

            // Voucher Approved Information ------------------------------------------------------------------------------------Start
            HMCommonSetupBO commonSetupBOCheckedBy = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOApprovedBy = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOCheckedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "CheckedBySetup");
            commonSetupBOApprovedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "ApprovedBySetup");
            List<GLVoucherApprovedInfoBO> approvedBOList = new List<GLVoucherApprovedInfoBO>();
            // CheckedBy -----------------
            if (commonSetupBOCheckedBy != null)
            {
                if (commonSetupBOCheckedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOCheckedBy = new GLVoucherApprovedInfoBO();
                    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(commonSetupBOCheckedBy.SetupValue);
                    approvedBOList.Add(approvedBOCheckedBy);
                }
            }

            // ApprovedBy -----------------
            if (commonSetupBOApprovedBy != null)
            {
                if (commonSetupBOApprovedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOApprovedBy = new GLVoucherApprovedInfoBO();
                    approvedBOApprovedBy.ApprovedType = "ApprovedBy";
                    approvedBOApprovedBy.UserInfoId = Convert.ToInt32(commonSetupBOApprovedBy.SetupValue);
                    approvedBOList.Add(approvedBOApprovedBy);
                }
            }
            // Voucher Approved Information ------------------------------------------------------------------------------------End

            string currentVoucherNo = string.Empty;
            glMasterBO.CreatedBy = userInformationBO.UserInfoId;
            Boolean status = glMasterDA.SaveGLMasterInfo(glMasterBO, out tmpGLMasterId, out currentVoucherNo, Session["TransactionDetailList"] as List<GLLedgerBO>, approvedBOList);
        }
        private void LoadCheckBoxListServiceInformation()
        {
            string registrationIdList = string.Empty;
            if (this.ddlRegistrationId.SelectedIndex != -1)
            {
                if (!string.IsNullOrWhiteSpace(txtSrcRegistrationIdList.Value))
                {
                    registrationIdList = txtSrcRegistrationIdList.Value;
                }
                else
                {
                    registrationIdList = this.ddlRegistrationId.SelectedValue.ToString();
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                registrationIdList = hmCommonDA.GetRegistrationIdList(registrationIdList);

                GuestBillSplitDA entityDA = new GuestBillSplitDA();
                List<GuestServiceBillApprovedBO> entityBOList = new List<GuestServiceBillApprovedBO>();

                int commaCount = registrationIdList.Count(x => x == ',');
                if (commaCount == 1)
                {
                    if (string.IsNullOrWhiteSpace(registrationIdList.Split(',')[1]))
                    {
                        registrationIdList = registrationIdList.Replace(",", "");
                    }
                }

                entityBOList = entityDA.GetGuestServiceInfoByRegistrationId(registrationIdList);

                var ServiceList = entityBOList.Where(x => x.ServiceType == "GuestHouseService" || x.ServiceType == "RestaurantService" || x.ServiceType == "BanquetService").ToList();
                var RoomList = entityBOList.Where(x => x.ServiceType == "GuestRoomService").ToList();
                var GuestPayment = entityBOList.Where(x => x.ServiceType == "GuestPaymentStatement").ToList();

                this.chkBillSpliteRoomItem.DataSource = RoomList;
                this.chkBillSpliteRoomItem.DataTextField = "ServiceName";
                this.chkBillSpliteRoomItem.DataValueField = "ServiceId";
                this.chkBillSpliteRoomItem.DataBind();

                this.chkCompanyPaymentBillSpliteRoomItem.DataSource = RoomList;
                this.chkCompanyPaymentBillSpliteRoomItem.DataTextField = "ServiceName";
                this.chkCompanyPaymentBillSpliteRoomItem.DataValueField = "ServiceId";
                this.chkCompanyPaymentBillSpliteRoomItem.DataBind();

                this.chkBillSpliteServiceItem.DataSource = ServiceList;
                this.chkBillSpliteServiceItem.DataTextField = "ServiceName";
                this.chkBillSpliteServiceItem.DataValueField = "ServiceId";
                this.chkBillSpliteServiceItem.DataBind();

                this.chkCompanyPaymentBillSpliteServiceItem.DataSource = ServiceList;
                this.chkCompanyPaymentBillSpliteServiceItem.DataTextField = "ServiceName";
                this.chkCompanyPaymentBillSpliteServiceItem.DataValueField = "ServiceId";
                this.chkCompanyPaymentBillSpliteServiceItem.DataBind();

                this.chkBillSplitePaymentItem.DataSource = GuestPayment;
                this.chkBillSplitePaymentItem.DataTextField = "ServiceName";
                this.chkBillSplitePaymentItem.DataValueField = "ServiceId";
                this.chkBillSplitePaymentItem.DataBind();

                if (GuestPayment != null)
                {
                    if (GuestPayment.Count > 0)
                    {
                        List<GuestServiceBillApprovedBO> companyEntityBOList = new List<GuestServiceBillApprovedBO>();
                        GuestServiceBillApprovedBO companyEntityBO = new GuestServiceBillApprovedBO();
                        companyEntityBO.ServiceName = "Total Payment";
                        companyEntityBO.ServiceId = -100;
                        companyEntityBOList.Add(companyEntityBO);
                        this.chkCompanyPaymentBillSplitePaymentItem.DataSource = companyEntityBOList;
                        this.chkCompanyPaymentBillSplitePaymentItem.DataTextField = "ServiceName";
                        this.chkCompanyPaymentBillSplitePaymentItem.DataValueField = "ServiceId";
                        this.chkCompanyPaymentBillSplitePaymentItem.DataBind();
                    }
                }

                List<GuestServiceBillApprovedBO> individualEntityBOList = new List<GuestServiceBillApprovedBO>();
                individualEntityBOList = entityDA.GetGuestIndividualServiceInfoByRegistrationId(registrationIdList);

                var individualServiceList = individualEntityBOList.Where(x => x.ServiceType == "GuestHouseService" || x.ServiceType == "RestaurantService" || x.ServiceType == "BanquetService").ToList();
                var individualRoomList = individualEntityBOList.Where(x => x.ServiceType == "GuestRoomService").ToList();
                var individualPaymentList = individualEntityBOList.Where(x => x.ServiceType == "GuestPaymentStatement").ToList();
                var individualTransferedPaymentList = individualEntityBOList.Where(x => x.ServiceType == "OthersGuestPaymentStatement").ToList();

                this.chkBillSpliteIndividualRoomItem.DataSource = individualRoomList;
                this.chkBillSpliteIndividualRoomItem.DataTextField = "ServiceName";// "ServiceName" + "ServiceType";
                this.chkBillSpliteIndividualRoomItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualRoomItem.DataBind();

                this.chkBillSpliteIndividualServiceItem.DataSource = individualServiceList;
                this.chkBillSpliteIndividualServiceItem.DataTextField = "ServiceName";
                this.chkBillSpliteIndividualServiceItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualServiceItem.DataBind();

                this.chkBillSpliteIndividualPaymentItem.DataSource = individualPaymentList;
                this.chkBillSpliteIndividualPaymentItem.DataTextField = "ServiceName";
                this.chkBillSpliteIndividualPaymentItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualPaymentItem.DataBind();

                this.chkBillSpliteIndividualTransferedPaymentItem.DataSource = individualTransferedPaymentList;
                this.chkBillSpliteIndividualTransferedPaymentItem.DataTextField = "ServiceName";
                this.chkBillSpliteIndividualTransferedPaymentItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualTransferedPaymentItem.DataBind();

            }
        }
        private void LoadServiceInformationWithControl()
        {
        }
        private void GoToPrintPreviewReport(string IsBillSplited)
        {
            string registrationIdList = string.Empty;
            if (this.ddlRegistrationId.SelectedIndex != -1)
            {
                if (!string.IsNullOrWhiteSpace(txtSrcRegistrationIdList.Value))
                {
                    registrationIdList = txtSrcRegistrationIdList.Value;
                }
                else
                {
                    registrationIdList = this.ddlRegistrationId.SelectedValue.ToString();
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                string selectedRoomIdList = string.Empty;
                string selectedServiceIdList = string.Empty;
                foreach (GridViewRow row in gvGroupServiceInformationForBillSplit.Rows)
                {
                    bool isSelected = ((CheckBox)row.FindControl("chkIsSelected")).Checked;
                    if (isSelected)
                    {
                        Label lblServiceIdValue = (Label)row.FindControl("lblServiceId");
                        Label lblgvServiceType = (Label)row.FindControl("lblgvServiceType");

                        if (lblgvServiceType.Text == "GuestRoomService")
                        {
                            if (!string.IsNullOrWhiteSpace(selectedRoomIdList))
                            {
                                selectedRoomIdList = selectedRoomIdList + ",";
                            }
                            selectedRoomIdList = selectedRoomIdList + lblServiceIdValue.Text;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(selectedServiceIdList))
                            {
                                selectedServiceIdList = selectedServiceIdList + ",";
                            }
                            selectedServiceIdList = selectedServiceIdList + lblServiceIdValue.Text;
                        }
                    }
                }
                if (IsBillSplited == "0")
                {
                    this.Session["IsBillSplited"] = IsBillSplited;
                    this.Session["CheckOutRegistrationIdList"] = registrationIdList;
                    this.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
                    this.Session["GuestBillToDate"] = hmUtility.GetToDate();
                }
                else
                {
                    this.Session["IsBillSplited"] = IsBillSplited;
                    this.Session["CheckOutRegistrationIdList"] = registrationIdList;
                    this.Session["GuestBillFromDate"] = this.txtStartDate.Text;
                    this.Session["GuestBillToDate"] = this.txtEndDate.Text;
                }
                if (IsBillSplited == "1")
                {
                    selectedRoomIdList = !string.IsNullOrWhiteSpace(selectedRoomIdList) ? selectedRoomIdList : "0";
                    selectedServiceIdList = !string.IsNullOrWhiteSpace(selectedServiceIdList) ? selectedServiceIdList : "0";
                    this.Session["GuestBillRoomIdParameterValue"] = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + this.Session["GuestBillFromDate"].ToString() + "') AND dbo.FnDate('" + this.Session["GuestBillToDate"].ToString() + "')) AND ServiceId IN(" + selectedRoomIdList + ")";
                    this.Session["GuestBillServiceIdParameterValue"] = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + this.Session["GuestBillFromDate"].ToString() + "') AND dbo.FnDate('" + this.Session["GuestBillToDate"].ToString() + "')) AND ServiceId IN(" + selectedServiceIdList + ")";
                }
                else
                {
                    this.Session["GuestBillRoomIdParameterValue"] = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + this.Session["GuestBillFromDate"].ToString() + "') AND dbo.FnDate('" + this.Session["GuestBillToDate"].ToString() + "'))";
                    this.Session["GuestBillServiceIdParameterValue"] = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + this.Session["GuestBillFromDate"].ToString() + "') AND dbo.FnDate('" + this.Session["GuestBillToDate"].ToString() + "'))";
                }
                this.chkIsBillSplit.Checked = false;
                Session["IsCheckOutBillPreview"] = "0";
                string url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx";
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=730,height=680,left=300,top=50,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
        }
        private void SavePendingGuestRoomInformation()
        {
            //DateTime dateTime = DateTime.Now;
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //List<GuestHouseCheckOutBO> guestHouseCheckOutBOList = new List<GuestHouseCheckOutBO>();
            //List<GHServiceBillBO> entityRoomDetailBOList = new List<GHServiceBillBO>();
            //List<GHServiceBillBO> entityDetailBOList = new List<GHServiceBillBO>();
            //GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();
            //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //int totalRoomCheckOut = txtSrcRegistrationIdList.Value.Split(',').Length - 1;

            //for (int i = 0; i <= totalRoomCheckOut; i++)
            //{
            //    if (i == 0)
            //    {
            //        // -- Room List Not Approved From Night Audit----------
            //        foreach (GridViewRow row in gvRoomDetail.Rows)
            //        {
            //            AvailableGuestListBO availableGuestList = new AvailableGuestListBO();
            //            Label isNightAuditApproved = (Label)row.FindControl("lblNightAuditApproved");

            //            Label isNightAuditApprovedDate = (Label)row.FindControl("lblServiceDate");

            //            if (isNightAuditApproved.Text == "N")
            //            {
            //                availableGuestList.RegistrationId = Int32.Parse(((Label)row.FindControl("lblRegistrationId")).Text);
            //                availableGuestList.RoomType = ((Label)row.FindControl("lblServiceType")).Text;
            //                availableGuestList.RoomId = Int32.Parse(((Label)row.FindControl("lblServiceId")).Text);
            //                availableGuestList.RoomNumber = Convert.ToInt32(((Label)row.FindControl("lblRoomNumber")).Text);
            //                availableGuestList.PreviousRoomRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text) - Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);

            //                if (hmUtility.GetDateTimeFromString(hmUtility.GetStringFromDateTime(dateTime), userInformationBO.ServerDateFormat) == hmUtility.GetDateTimeFromString(isNightAuditApprovedDate.Text, userInformationBO.ServerDateFormat))
            //                {
            //                    availableGuestList.ServiceDate = hmUtility.GetDateTimeFromString(hmUtility.GetStringFromDateTime(dateTime), userInformationBO.ServerDateFormat);
            //                    availableGuestList.ApprovedDate = hmUtility.GetDateTimeFromString(hmUtility.GetStringFromDateTime(dateTime), userInformationBO.ServerDateFormat);
            //                }
            //                else
            //                {
            //                    availableGuestList.ServiceDate = hmUtility.GetDateTimeFromString(isNightAuditApprovedDate.Text, userInformationBO.ServerDateFormat);
            //                    availableGuestList.ApprovedDate = hmUtility.GetDateTimeFromString(isNightAuditApprovedDate.Text, userInformationBO.ServerDateFormat);
            //                }

            //                availableGuestList.RoomRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text);
            //                availableGuestList.BPPercentAmount = Convert.ToDecimal(0);
            //                availableGuestList.BPDiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);
            //                availableGuestList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblVatAmount")).Text);
            //                availableGuestList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblServiceCharge")).Text);
            //                availableGuestList.CitySDCharge = 0;
            //                availableGuestList.AdditionalCharge = 0;
            //                availableGuestList.ReferenceSalesCommission = Convert.ToDecimal(((Label)row.FindControl("lblgvReferenceSalesCommission")).Text);
            //                availableGuestList.ApprovedStatus = true;
            //                availableGuestList.TotalCalculatedAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);

            //                int tmpApprovedId = 0;
            //                availableGuestList.CreatedBy = userInformationBO.UserInfoId;
            //                Boolean status = roomRegistrationDA.SavePendingGuestBillApprovedInfo(availableGuestList, out tmpApprovedId);

            //            }
            //        }

            //        // -- Service List Not Approved From Night Audit----------
            //        foreach (GridViewRow row in gvServiceDetail.Rows)
            //        {
            //            GHServiceBillBO entityDetailBO = new GHServiceBillBO();
            //            Label isNightAuditApproved = (Label)row.FindControl("lblNightAuditApproved");

            //            if (isNightAuditApproved.Text == "N")
            //            {
            //                GHServiceBillBO serviceList = new GHServiceBillBO();
            //                serviceList.RegistrationId = Int32.Parse(((Label)row.FindControl("lblRegistrationId")).Text);
            //                serviceList.IsPaidService = Convert.ToBoolean(((Label)row.FindControl("lblgvIsPaidService")).Text);
            //                serviceList.ServiceBillId = Int32.Parse(((Label)row.FindControl("lblid")).Text);
            //                serviceList.ServiceDate = hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblServiceDate")).Text, userInformationBO.ServerDateFormat);
            //                serviceList.ServiceType = ((Label)row.FindControl("lblServiceType")).Text;
            //                serviceList.ServiceId = Int32.Parse(((Label)row.FindControl("lblServiceId")).Text);
            //                serviceList.ServiceName = ((Label)row.FindControl("lblServiceName")).Text;
            //                serviceList.ServiceQuantity = Convert.ToDecimal(((Label)row.FindControl("lblServiceQuantity")).Text);
            //                serviceList.ServiceRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text);
            //                serviceList.DiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);
            //                serviceList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblVatAmount")).Text);
            //                serviceList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblServiceCharge")).Text);
            //                serviceList.CitySDCharge = Convert.ToDecimal(((Label)row.FindControl("lblCitySDCharge")).Text);
            //                serviceList.AdditionalCharge = Convert.ToDecimal(((Label)row.FindControl("lblAdditionalCharge")).Text);
            //                serviceList.ApprovedStatus = true;
            //                serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblServiceDate")).Text, userInformationBO.ServerDateFormat);
            //                serviceList.CreatedBy = userInformationBO.UserInfoId;
            //                int tmpApprovedId = 0;
            //                Boolean status = roomRegistrationDA.SavePendingGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
            //            }
            //        }
            //    }
            //}

        }
        public static string LoadGuestPaymentDetailGridViewByWM(string paymentDescription)
        {
            string strTable = "";
            List<GuestBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Description</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (GuestBillPaymentBO dr in detailList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentDescription + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + dr.PaymentAmount + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(" + dr.PaymentId + ")' alt='Delete Information' border='0' />";
                    strTable += "</td></tr>";
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {

            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void ClearCommonSessionInformation()
        {
            this.Session["TransactionDetailList"] = null;
            this.Session["GuestPaymentDetailListForGrid"] = null;
            this.Session["CheckOutPayMode"] = null;
            this.Session["CurrentRegistrationId"] = null;
            this.Session["IsCheckOutBillPreview"] = null;
            this.Session["GuestBillRoomIdParameterValue"] = null;
            this.Session["GuestBillServiceIdParameterValue"] = null;
            this.Session["GuestPaymentDetailListForGrid"] = null;
            this.Session["CompanyPaymentRoomIdList"] = null;
            this.Session["CompanyPaymentServiceIdList"] = null;
        }
        private void ClearUnCommonSessionInformation()
        {
            this.Session["txtStartDate"] = null;
            this.Session["IsBillSplited"] = null;
            this.Session["GuestBillFromDate"] = null;
            this.Session["GuestBillToDate"] = null;
            this.Session["AddedExtraRoomInformation"] = null;
            this.Session["CheckOutRegistrationIdList"] = null;
        }
        private void GuestCheckOutDiscount(int ddlRegistrationId)
        {
            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
            ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);

            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
            CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");
            this.ddlCashPaymentAccountHeadForDiscount.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCashPaymentAccountHeadForDiscount.DataTextField = "NodeHead";
            this.ddlCashPaymentAccountHeadForDiscount.DataValueField = "NodeId";
            this.ddlCashPaymentAccountHeadForDiscount.DataBind();

            guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHeadForDiscount.SelectedValue);
            guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHeadForDiscount.SelectedValue);
            guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
            guestBillPaymentBO.PaymentType = "Discount";
            guestBillPaymentBO.BankId = Convert.ToInt32(0);
            guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
            guestBillPaymentBO.FieldId = LocalCurrencyId;
            guestBillPaymentBO.ConvertionRate = 1;

            decimal totalSalesAmount = !string.IsNullOrWhiteSpace(HiddenFieldSalesTotal.Value) ? Convert.ToDecimal(HiddenFieldSalesTotal.Value) : 0;
            decimal grandTotalAmount = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;

            guestBillPaymentBO.CurrencyAmount = (totalSalesAmount - grandTotalAmount);
            guestBillPaymentBO.PaymentAmount = (totalSalesAmount - grandTotalAmount);

            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = "Cash";
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = string.Empty;
            guestBillPaymentBO.CardType = string.Empty;
            guestBillPaymentBO.ExpireDate = null;
            guestBillPaymentBO.ChecqueNumber = string.Empty;
            guestBillPaymentBO.CardHolderName = string.Empty;
            guestBillPaymentBO.PaymentDescription = string.Empty;
            guestBillPaymentBO.PaymentId = dynamicDetailId;

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod(EnableSession = true)]
        public static string PerformBillSplitePrintPreview(string currencyRate, string isIsplite, string serviceType, string SelectdIndividualTransferedPaymentId, string SelectdPaymentId, string SelectdIndividualPaymentId, string SelectdIndividualServiceId, string SelectdIndividualRoomId, string SelectdServiceId, string SelectdRoomId, string StartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
            return "";
        }
        [WebMethod]
        public static string GetTotalBillAmountByWebMethod(string ddlRegistrationId, string SelectdRoomId, string SelectdServiceId, string StartDate, string EndDate)
        {
            GuestBillSplitDA entityDA = new GuestBillSplitDA();
            GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();

            HMUtility hmUtility = new HMUtility();
            string startDate = hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            string endDate = hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            entityBO = entityDA.GetGuestServiceTotalAmountInfo(ddlRegistrationId, SelectdRoomId, SelectdServiceId, startDate, endDate);
            return Math.Round(entityBO.ServiceTotalAmount).ToString("#0.00"); // entityBO.ServiceTotalAmount.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlCurrency, string currencyType, string localCurrencyId, string conversionRate, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlRegistrationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlPaidByRoomId, string RefundAccountHead)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO frontOfficeRoomSalesAccountHeadInfoBO = new CustomFieldBO();
            frontOfficeRoomSalesAccountHeadInfoBO = hmCommonDA.GetCustomFieldByFieldName("FrontOfficeRoomSalesAccountHeadInfo");
            int frontOfficeRoomSalesAccountHeadInfo = !string.IsNullOrWhiteSpace(frontOfficeRoomSalesAccountHeadInfoBO.FieldValue) ? Convert.ToInt32(frontOfficeRoomSalesAccountHeadInfoBO.FieldValue) : 0;

            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();

            if (ddlPayMode == "Other Room")
            {
                if (!string.IsNullOrWhiteSpace(ddlPaidByRoomId))
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    List<RoomRegistrationBO> billPaidByInfoList = new List<RoomRegistrationBO>();

                    billPaidByInfoList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlPaidByRoomId));
                    if (billPaidByInfoList != null)
                    {
                        foreach (RoomRegistrationBO row in billPaidByInfoList)
                        {
                            ddlPaidByRegistrationId = row.RegistrationId;
                        }
                    }
                    else
                    {
                        ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
                    }
                }
                else
                {
                    ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
                }
            }
            else
            {
                ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
            }

            if (ddlPayMode == "Company")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
            }
            else if (ddlPayMode == "Other Room")
            {
                guestBillPaymentBO.NodeId = frontOfficeRoomSalesAccountHeadInfo;
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = frontOfficeRoomSalesAccountHeadInfo;
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
            }
            else if (ddlPayMode == "Refund")
            {
                guestBillPaymentBO.RefundAccountHead = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.PaymentMode = "Refund";
                guestBillPaymentBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount * 1;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.PaymentAmount * 1;
                guestBillPaymentBO.PaymentType = "Refund";
                guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                ddlCurrency = localCurrencyId; // set local CurrencyId
                conversionRate = "1";
            }
            else
            {
                if (ddlPayMode == "Cash")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHead);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHead);
                }
                else if (ddlPayMode == "Card")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                }
                else if (ddlPayMode == "Checque")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                }
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.PaymentType = "Advance";
            }

            guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
            guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);

            if (currencyType == "Local")
            {
                guestBillPaymentBO.IsUSDTransaction = false;
                guestBillPaymentBO.FieldId = Convert.ToInt32(localCurrencyId);
                guestBillPaymentBO.ConvertionRate = 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            }
            else
            {
                guestBillPaymentBO.IsUSDTransaction = true;
                guestBillPaymentBO.FieldId = Convert.ToInt32(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(conversionRate) ? Convert.ToDecimal(conversionRate) : 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.CurrencyAmount * guestBillPaymentBO.ConvertionRate;
            }

            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            guestBillPaymentBO.CardType = ddlCardType;
            if (string.IsNullOrEmpty(txtExpireDate))
            {
                guestBillPaymentBO.ExpireDate = null;
            }
            else
            {
                guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            guestBillPaymentBO.CardHolderName = txtCardHolderName;

            guestBillPaymentBO.PaymentDescription = paymentDescription;

            guestBillPaymentBO.PaymentId = dynamicDetailId;

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            return LoadGuestPaymentDetailGridViewByWM(paymentDescription);
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteGuestPaymentByWebMethod(int paymentId)
        {
            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
            }

            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;

            return LoadGuestPaymentDetailGridViewByWM("");
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {

            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            decimal sum = 0;
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].PaymentMode == "Refund")
                {
                    sum = sum - Convert.ToDecimal(List[i].PaymentAmount);
                }
                else
                {
                    sum = sum + Convert.ToDecimal(List[i].PaymentAmount);
                }
            }
            return Math.Round(sum).ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformCompanyPayBill(string serviceType, string SelectdServiceApprovedId, string SelectdRoomApprovedId, string SelectdServiceId, string SelectdRoomId, string SelectdPaymentId, string StartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;
            HMUtility hmUtility = new HMUtility();
            //Room Information----------------
            List<GuestServiceBillApprovedBO> entityRoomBOList = new List<GuestServiceBillApprovedBO>();

            int totalRoomIdOut = SelectdRoomId.Split(',').Length - 1;
            for (int i = 0; i < totalRoomIdOut; i++)
            {
                GuestServiceBillApprovedBO entityRoomBO = new GuestServiceBillApprovedBO();
                entityRoomBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                entityRoomBO.ServiceId = Convert.ToInt32(SelectdRoomId.Split(',')[i]);
                entityRoomBO.ArriveDate = hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityRoomBO.CheckOutDate = hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityRoomBOList.Add(entityRoomBO);
            }
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = entityRoomBOList;

            //Service Information----------------
            List<GuestServiceBillApprovedBO> entityServiceBOList = new List<GuestServiceBillApprovedBO>();

            int totalServiceIdOut = SelectdServiceId.Split(',').Length - 1;
            for (int i = 0; i < totalServiceIdOut; i++)
            {
                GuestServiceBillApprovedBO entityServiceBO = new GuestServiceBillApprovedBO();
                entityServiceBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                entityServiceBO.ServiceId = Convert.ToInt32(SelectdServiceId.Split(',')[i]);
                entityServiceBO.ArriveDate = hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityServiceBO.CheckOutDate = hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityServiceBOList.Add(entityServiceBO);
            }

            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = entityServiceBOList;

            decimal calculatedAmount = 0;
            if (!string.IsNullOrWhiteSpace(SelectdPaymentId))
            {
                List<GuestBillPaymentBO> guestPaymentBOList = new List<GuestBillPaymentBO>();
                GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

                int paymentId = SelectdPaymentId.Split(',').Length;
                for (int i = 0; i < paymentId; i++)
                {
                    calculatedAmount = calculatedAmount + guestBillPaymentDA.GetGuestBillPaymentSummaryInfoByPaymentType(txtSrcRegistrationIdList, Convert.ToInt32(SelectdPaymentId.Split(',')[i]));
                }
            }

            return calculatedAmount.ToString();
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
    }
}