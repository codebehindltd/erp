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
    public partial class frmRoomCheckOut : System.Web.UI.Page
    {
        protected bool isSingle = true;
        HiddenField innboardMessage;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        protected int isCompanyProjectPanelEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int isFOMultiInvoicePreviewOptionEnable = 0;
        protected int isInValidRoomNumberForTodaysCheckOut = 0;
        protected int LocalCurrencyId;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO frontOfficeInvoiceTemplateBO = new HMCommonSetupBO();
                frontOfficeInvoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeInvoiceTemplate", "FrontOfficeInvoiceTemplate");
                if (frontOfficeInvoiceTemplateBO != null)
                {
                    if (frontOfficeInvoiceTemplateBO.SetupValue == "5")
                    {
                        btnHoldUp.Visible = true;
                    }
                }

                HttpContext.Current.Session["BillPreviewCurrencyRateInformation"] = null;
                this.LoadBank();
                this.LoadLabelForSalesTotal();
                this.ClearCommonSessionInformation();
                this.hfGuestPaymentDetailsInformationDiv.Value = "1";
                this.DayLetDiscountInputDiv.Visible = false;
                this.DayLetDiscountOutputDiv.Visible = false;
                this.AdvanceTodaysRoomChargeDiv.Visible = true;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                this.txtCardValidation.Value = cardValidation.ToString();
                this.LoadCheckBoxListServiceInformation();
                this.CheckObjectPermission();
                this.LoadCurrentDate();
                this.LoadRoomNumber();
                this.LoadCurrency();
                this.LoadIsConversionRateEditable();
                this.LoadLocalCurrencyId();
                this.IsLocalCurrencyDefaultSelected();
                this.LoadRefundAccountHead();
                this.lblRegistrationNumber.Visible = false;
                this.ddlRegistrationId.Visible = false;
                this.LoadCommonDropDownHiddenField();
                this.LoadRelatedInformation();                

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
                    this.LoadCheckBoxListServiceInformation();
                }

                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    Session["AddedExtraRoomInformation"] = null;
                    txtSrcRegistrationIdList.Value = string.Empty;
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcRoomNumber.Text = numberBO.RoomNumber;
                    this.SearchButtonClick();
                }

                this.LoadStartAndEndDate();
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
            //if (e.CommandName == "CmdEdit")
            //{
            //    this._BillId = Convert.ToInt32(e.CommandArgument.ToString());
            //    Session["_RoomTypeId"] = this._BillId;
            //    this.FillForm(this._BillId);
            //}
            //else if (e.CommandName == "CmdDelete")
            //{
            //    try
            //    {
            //        this._BillId = Convert.ToInt32(e.CommandArgument.ToString());
            //        Session["_RoomTypeId"] = this._BillId;
            //        this.DeleteData(this._BillId);
            //        this.Cancel();
            //        this.LoadGridView();

            //    }
            //    catch
            //    {
            //    }
            //}
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
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
            this.txtAddedMultipleRoomId.Value = string.Empty;
            this.hfSelectedRoomId.Value = string.Empty;
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
                if (lblNightAuditApprovedValue.Text == "Pending")
                {
                    Label lblServiceDateValue = (Label)e.Row.FindControl("lblServiceDate");

                    DateTime cmpCurrentDateTime = hmUtility.GetDateTimeFromString(lblServiceDateValue.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    if (DateTime.Now.Month == cmpCurrentDateTime.Month)
                    {
                        if (DateTime.Now.Day == cmpCurrentDateTime.Day)
                        {
                            this.DayLetDiscountInputDiv.Visible = true;
                            this.DayLetDiscountOutputDiv.Visible = true;
                            this.AdvanceTodaysRoomChargeDiv.Visible = false;
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
                //// Innboard Vat Enable Information --------------------------
                //UserInformationBO userInformationBO = new UserInformationBO();
                //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //if (userInformationBO.IsInnboardVatEnable != null)
                //{
                //    if (Convert.ToInt32(userInformationBO.IsInnboardVatEnable) == 0)
                //    {
                //        //gvServiceDetail.Columns[10].Visible = false;
                //        foreach (DataControlField col in gvServiceDetail.Columns)
                //        {
                //            if (col.HeaderText == "Vat")
                //            {
                //                col.Visible = false;
                //            }
                //        }
                //    }
                //}

                //// Innboard Service Charge Enable Information --------------------------
                //if (userInformationBO.IsInnboardServiceChargeEnable != null)
                //{
                //    if (Convert.ToInt32(userInformationBO.IsInnboardServiceChargeEnable) == 0)
                //    {
                //        //gvServiceDetail.Columns[10].Visible = false;
                //        foreach (DataControlField col in gvServiceDetail.Columns)
                //        {
                //            if (col.HeaderText == "S. Charge")
                //            {
                //                col.Visible = false;
                //            }
                //        }
                //    }
                //}
            }
        }
        protected void btnHoldUp_Click(object sender, EventArgs e)
        {
            if (!IsRoomBillSettlmentPending())
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

                    #region Total Bill Process for Listed Guest
                    DateTime currentDate = DateTime.Now;
                    string RegistrationIdList = txtSrcRegistrationIdList.Value;

                    HMCommonDA rHmCommonDA = new HMCommonDA();
                    RegistrationIdList = rHmCommonDA.GetRegistrationIdList(RegistrationIdList);
                    GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
                    List<GuestHouseCheckOutDetailBO> guestRoomDetailBOList = new List<GuestHouseCheckOutDetailBO>();
                    List<GuestHouseCheckOutDetailBO> guestServiceDetailBOList = new List<GuestHouseCheckOutDetailBO>();
                    List<GuestHouseCheckOutDetailBO> guestAllRoomDetailBOList = da.GetGuestServiceInformationForCheckOut(RegistrationIdList, "GuestRoom", currentDate, userInformationBO.UserInfoId);
                    foreach (GuestHouseCheckOutDetailBO guestRoomBo in guestAllRoomDetailBOList)
                    {
                        if (guestRoomBo.RegistrationId != guestRoomBo.BillPaidBy)
                        {
                            guestRoomBo.RegistrationId = guestRoomBo.BillPaidBy;
                            guestRoomBo.RoomNumber = guestRoomBo.BillPaidByRoomNumber;
                            guestRoomDetailBOList.Add(guestRoomBo);
                        }
                        else
                        {
                            guestRoomDetailBOList.Add(guestRoomBo);
                        }
                    }

                    List<GuestHouseCheckOutDetailBO> guestAllServiceDetailBOList = da.GetGuestServiceInformationForCheckOut(RegistrationIdList, "GuestService", currentDate, userInformationBO.UserInfoId);
                    foreach (GuestHouseCheckOutDetailBO guestServiceBo in guestAllServiceDetailBOList)
                    {
                        if (guestServiceBo.RegistrationId != guestServiceBo.BillPaidBy)
                        {
                            guestServiceBo.RegistrationId = guestServiceBo.BillPaidBy;
                            guestServiceBo.RoomNumber = guestServiceBo.BillPaidByRoomNumber;
                            guestServiceDetailBOList.Add(guestServiceBo);
                        }
                        else
                        {
                            guestServiceDetailBOList.Add(guestServiceBo);
                        }
                    }

                    List<GuestHouseCheckOutDetailBO> guestOtherPaymentBOList = da.GetGuestOtherPaymentForBillByRegiIdList(RegistrationIdList);
                    if (guestOtherPaymentBOList != null)
                    {
                        foreach (GuestHouseCheckOutDetailBO guestOtherRoomPaymentBo in guestOtherPaymentBOList)
                        {
                            if (guestOtherRoomPaymentBo.RegistrationId != guestOtherRoomPaymentBo.BillPaidBy)
                            {
                                guestOtherRoomPaymentBo.RegistrationId = guestOtherRoomPaymentBo.BillPaidBy;
                            }
                        }
                    }

                    List<GuestBillPaymentBO> guestBillPaymentBOList = new List<GuestBillPaymentBO>();
                    GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();
                    guestBillPaymentBOList = guestBillPaymentDA.GetGuestBillPaymentInfoByRegistrationIdList(RegistrationIdList);

                    foreach (GuestBillPaymentBO paymentBOList in guestBillPaymentBOList)
                    {
                        if (paymentBOList.RegistrationId != paymentBOList.BillPaidBy)
                        {
                            paymentBOList.RegistrationId = paymentBOList.BillPaidBy;
                        }
                    }
                    #endregion

                    for (int i = 0; i <= totalRoomCheckOut; i++)
                    {
                        GuestHouseCheckOutBO guestHouseCheckOutBO = new GuestHouseCheckOutBO();
                        if (!string.IsNullOrWhiteSpace(txtSrcRegistrationIdList.Value.Split(',')[i]))
                        {
                            guestHouseCheckOutBO.RegistrationId = Convert.ToInt32(txtSrcRegistrationIdList.Value.Split(',')[i]);
                            guestHouseCheckOutBO.CheckOutProcessType = "CheckOutProcess";
                            guestHouseCheckOutBO.PayMode = this.ddlPayMode.SelectedItem.Text;
                            if (this.ddlPayMode.SelectedValue == "Cash")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCashReceiveAccountsInfo.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "Card")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCardPaymentAccountHeadId.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "bKash")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCashReceiveAccountsInfo.SelectedValue);
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
                            guestHouseCheckOutBO.CitySDCharge = Convert.ToDecimal(this.txtCitySDChargeTotal.Text);
                            guestHouseCheckOutBO.AdditionalCharge = Convert.ToDecimal(this.txtAdditionalChargeTotal.Text);
                            guestHouseCheckOutBO.DiscountAmount = Convert.ToDecimal(this.txtDiscountAmountTotal.Text);
                            guestHouseCheckOutBO.TotalAmount = Convert.ToDecimal(this.txtSalesTotal.Text);

                            if (this.ddlPayMode.SelectedItem.Text == "Other Room")
                            {
                                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.ddlPaidByRegistrationId.SelectedItem.Text);
                                if (roomAllocationBO != null)
                                {
                                    guestHouseCheckOutBO.BillPaidBy = roomAllocationBO.RegistrationId;
                                }
                            }
                            else
                            {
                                guestHouseCheckOutBO.BillPaidBy = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                            }

                            guestHouseCheckOutBO.RebateRemarks = this.txtRebateRemarks.Text;
                            guestHouseCheckOutBO.CreatedBy = userInformationBO.UserInfoId;

                            #region Balance Calculation
                            List<GuestHouseCheckOutDetailBO> roomRegistrationWiseList = new List<GuestHouseCheckOutDetailBO>();
                            roomRegistrationWiseList = guestRoomDetailBOList.Where(x => x.RegistrationId == guestHouseCheckOutBO.RegistrationId).ToList();

                            decimal roomBillAmount = roomRegistrationWiseList.Sum(item => item.TotalAmount);

                            List<GuestHouseCheckOutDetailBO> serviceRegistrationWiseList = new List<GuestHouseCheckOutDetailBO>();
                            serviceRegistrationWiseList = guestServiceDetailBOList.Where(x => x.RegistrationId == guestHouseCheckOutBO.RegistrationId).ToList();
                            decimal serviceBillAmount = serviceRegistrationWiseList.Sum(item => item.TotalAmount);

                            List<GuestBillPaymentBO> paymentRegistrationWiseList = new List<GuestBillPaymentBO>();
                            paymentRegistrationWiseList = guestBillPaymentBOList.Where(x => x.RegistrationId == guestHouseCheckOutBO.RegistrationId).ToList();
                            decimal paymentBillAmount = paymentRegistrationWiseList.Sum(item => item.PaymentAmount);

                            guestHouseCheckOutBO.Balance = Math.Round(roomBillAmount + serviceBillAmount + paymentBillAmount);
                            #endregion

                            guestHouseCheckOutBOList.Add(guestHouseCheckOutBO);
                        }
                    }

                    if (btnHoldUp.Text.Equals("Hold Up"))
                    {
                        Boolean status = guestHouseCheckOutDA.SaveGuestHouseBillHoldUpInfo(guestHouseCheckOutBOList, entityRoomDetailBOList, entityDetailBOList, Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>, Session["CompanyPaymentRoomIdList"] as List<GuestServiceBillApprovedBO>, Session["CompanyPaymentServiceIdList"] as List<GuestServiceBillApprovedBO>, "Others", 0);
                        if (status)
                        {
                            Clear();
                            this.Session["AddedExtraRoomInformation"] = null;
                            Session["HiddenFieldCompanyPaymentButtonInfo"] = null;
                            CommonHelper.AlertInfo(innboardMessage, "Hold Up Operation Successfull.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.CheckOut.ToString(), EntityTypeEnum.EntityType.RoomCheckOut.ToString(), MasterId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomCheckOut));

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
                foreach (var item in dayLetBOList)
                {
                    if (item.DayLetDiscount > 0)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut.ToString(), item.DayLetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut));
                    else
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut.ToString(), item.DayLetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut));

                }
                string queryStringId = Request.QueryString["BackFromServiceBill"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ddlRoomId.SelectedValue = queryStringId;
                    this.txtSrcRoomNumber.Text = this.ddlRoomId.SelectedItem.Text;
                    this.lblRegistrationNumber.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.LoadRelatedInformation();
                    this.LoadCheckBoxListServiceInformation();
                }
                else
                {
                    this.SearchClickDetails();
                }

            }
            this.LoadStartAndEndDate();
        }
        protected void btnDayLetsOkProcess_Click(object sender, EventArgs e)
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
                    dayLetBO.DayLetDiscountType = DayLetDiscountTypeHiddenField.Value;
                    dayLetBO.DayLetDiscountAmount = Convert.ToDecimal(TodaysRoomBillHiddenField.Value);
                    dayLetBO.DayLetDiscount = Convert.ToDecimal(TodaysRoomBillHiddenField.Value);
                    dayLetBO.RoomRate = !string.IsNullOrWhiteSpace(TodaysRoomBillHiddenField.Value) ? Convert.ToDecimal(TodaysRoomBillHiddenField.Value) : 0;
                    dayLetBO.RegistrationId = Int32.Parse(part);
                    dayLetBO.RegistrationIdList = this.txtSrcRegistrationIdList.Value;
                    dayLetBOList.Add(dayLetBO);
                }
            }

            Boolean status = dayLetDA.SaveOrUpdateDayLetsInformation(dayLetBOList, out DayLetId);

            if (status)
            {
                RoomRegistrationDA roomregistrationDA = new RoomRegistrationDA();
                roomregistrationDA.RoomNightAuditProcess(this.txtSrcRegistrationIdList.Value, DateTime.Now, 1, userInformationBO.UserInfoId);

                foreach (var item in dayLetBOList)
                {
                    if (item.DayLetDiscount != 0)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut.ToString(), item.DayLetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut));
                    else
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut.ToString(), item.DayLetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut));
                }
                string queryStringId = Request.QueryString["BackFromServiceBill"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ddlRoomId.SelectedValue = queryStringId;
                    this.txtSrcRoomNumber.Text = this.ddlRoomId.SelectedItem.Text;
                    this.lblRegistrationNumber.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.LoadRelatedInformation();
                    this.LoadCheckBoxListServiceInformation();
                    this.SearchClickDetails();
                }
                else
                {
                    this.SearchClickDetails();
                }
            }
            this.LoadStartAndEndDate();
        }
        protected void btnOKTodaysRoomChargeProcess_Click(object sender, EventArgs e)
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
                    dayLetBO.DayLetDiscountType = "Percentage";
                    dayLetBO.DayLetDiscountAmount = !string.IsNullOrWhiteSpace(hfRoomChargeType.Value) ? Convert.ToDecimal(hfRoomChargeType.Value) : 0;
                    dayLetBO.DayLetDiscount = !string.IsNullOrWhiteSpace(hfRoomChargeType.Value) ? Convert.ToDecimal(hfRoomChargeType.Value) : 0;
                    dayLetBO.RoomRate = !string.IsNullOrWhiteSpace(hfRoomChargeType.Value) ? Convert.ToDecimal(hfRoomChargeType.Value) : 0;
                    dayLetBO.RegistrationId = Int32.Parse(part);
                    dayLetBO.RegistrationIdList = this.txtSrcRegistrationIdList.Value;
                    dayLetBOList.Add(dayLetBO);
                }
            }

            Boolean status = dayLetDA.SaveOrUpdateDayLetsInformation(dayLetBOList, out DayLetId);

            if (status)
            {
                RoomRegistrationDA roomregistrationDA = new RoomRegistrationDA();
                roomregistrationDA.RoomNightAuditProcess(this.txtSrcRegistrationIdList.Value, DateTime.Now, 1, userInformationBO.UserInfoId);

                foreach (var item in dayLetBOList)
                {
                    if (item.DayLetDiscount > 0)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut.ToString(), item.DayLetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut));
                    else
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut.ToString(), item.DayLetId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut));

                }
                string queryStringId = Request.QueryString["BackFromServiceBill"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ddlRoomId.SelectedValue = queryStringId;
                    this.txtSrcRoomNumber.Text = this.ddlRoomId.SelectedItem.Text;
                    this.lblRegistrationNumber.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.LoadRelatedInformation();
                    this.LoadCheckBoxListServiceInformation();
                }
                else
                {
                    this.SearchClickDetails();
                }
            }
            this.LoadStartAndEndDate();
        }
        protected void btnOKAddMoreRoomProcess_Click(object sender, EventArgs e)
        {
            Session["AddedExtraRoomId"] = this.hfSelectedRoomId.Value.ToString();
            Session["AddedExtraRoomInformation"] = this.txtAddedMultipleRoomId.Value.ToString();

            this.LoadRelatedInformation();
            this.LoadCheckBoxListServiceInformation();
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

                            guestHouseCheckOutBO.PayMode = this.ddlPayMode.SelectedItem.Text;
                            if (this.ddlPayMode.SelectedValue == "Cash")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCashReceiveAccountsInfo.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "Card")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCardPaymentAccountHeadId.SelectedValue);
                            }
                            else if (this.ddlPayMode.SelectedValue == "bKash")
                            {
                                guestHouseCheckOutBO.AccountsPostingHeadId = Convert.ToInt32(this.ddlCashReceiveAccountsInfo.SelectedValue);
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
                            guestHouseCheckOutBO.CitySDCharge = Convert.ToDecimal(this.txtCitySDChargeTotal.Text);
                            guestHouseCheckOutBO.AdditionalCharge = Convert.ToDecimal(this.txtAdditionalChargeTotal.Text);
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

                    if (btnSave.Text.Equals("Check Out"))
                    {
                        Boolean status = guestHouseCheckOutDA.SaveGuestHouseCheckOutInfo(guestHouseCheckOutBOList, entityRoomDetailBOList, entityDetailBOList, Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>, Session["CompanyPaymentRoomIdList"] as List<GuestServiceBillApprovedBO>, Session["CompanyPaymentServiceIdList"] as List<GuestServiceBillApprovedBO>, "Others", 0);

                        if (status)
                        {
                            //Link room Delete
                            List<HotelLinkedRoomDetailsBO> roomDetailsBOs = new List<HotelLinkedRoomDetailsBO>();
                            List<HotelLinkedRoomMasterBO> roomMasterBOs = new List<HotelLinkedRoomMasterBO>();
                            bool IsLinkDlt = false;
                            bool IsMasterDlt = false;
                            roomMasterBOs = roomRegistrationDA.GetLinkedMasterRoomInfoByRegistrationId(Int64.Parse(this.ddlRegistrationId.SelectedValue));
                            roomDetailsBOs = roomRegistrationDA.GetLinkedDetailsRoomInfoByRegistrationId(Int64.Parse(this.ddlRegistrationId.SelectedValue));
                            if (roomDetailsBOs.Count <= 2)//if there is only 2 rooms 
                            {
                                
                                //IsLinkDlt = roomRegistrationDA.DeleteDetailLinkRoooms(roomDetailsBOs);
                                foreach (var item in roomDetailsBOs)
                                {
                                    IsLinkDlt = hmCommonDA.DeleteInfoById("HotelLinkedRoomDetails", "RegistrationId", item.RegistrationId);
                                }
                                
                                
                                if (roomMasterBOs.Count > 0)
                                {
                                    foreach (var item in roomMasterBOs)
                                    {
                                        IsMasterDlt = hmCommonDA.DeleteInfoById("HotelLinkedRoomMaster", "Id", item.Id);
                                    }
                                    //IsMasterDlt = roomRegistrationDA.DeleteMasterLinkRoooms(roomMasterBOs);
                                }
                                
                            }
                            else // if more than two rooms and the check out room is not the master room
                            {
                                if ((roomMasterBOs[0].RegistrationId == Int64.Parse(this.ddlRegistrationId.SelectedValue)))
                                {
                                    CommonHelper.AlertInfo(innboardMessage, "Master room can't be checked out while having linked rooms", AlertType.Warning);
                                }
                                else
                                {
                                    foreach (var item in roomDetailsBOs)
                                    {

                                        if ((item.RegistrationId == Int64.Parse(this.ddlRegistrationId.SelectedValue)))
                                        {
                                            IsLinkDlt = hmCommonDA.DeleteInfoById("HotelLinkedRoomDetails", "RegistrationId", item.RegistrationId);
                                        }

                                    }
                                }
                                
                            }
                            //Link room Delete end
                            Clear();
                            this.Session["AddedExtraRoomInformation"] = null;
                            this.Session["AddedExtraRoomId"] = null;
                            Session["HiddenFieldCompanyPaymentButtonInfo"] = null;
                            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = null;
                            CommonHelper.AlertInfo(innboardMessage, "Check Out Operation Successfull.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.CheckOut.ToString(), EntityTypeEnum.EntityType.RoomCheckOut.ToString(), MasterId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomCheckOut));

                            string isEnableBillPreviewOption = !string.IsNullOrWhiteSpace(this.hfIsEnableBillPreviewOption.Value) ? this.hfIsEnableBillPreviewOption.Value : "0";

                            string currencyConversionRate = "0";
                            if (isEnableBillPreviewOption != "0")
                            {
                                currencyConversionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? this.txtConversionRate.Text : "0";
                            }

                            this.btnSave.Visible = false;
                            this.btnHoldUp.Visible = false;
                            this.btnCancel.Visible = false;

                            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyConversionRate, "0", "", "-1", "-1", "-1", "-1", "-1", "-1", "-1", hmUtility.GetFromDate().ToString(), hmUtility.GetToDate().ToString(), this.ddlRegistrationId.SelectedValue, this.Session["CurrentRegistrationId"].ToString());

                            string url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx?GuestBillInfo=" + this.Session["CurrentRegistrationId"] + "&IsCheckOut=" + true;

                            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                            HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
                            {

                                if (setUpBO.SetupValue == "1")
                                {
                                    url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx?GuestBillInfo=" + this.Session["CurrentRegistrationId"] + "&sdc=1&IsCheckOut=" + true;
                                }
                            }





                                    
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
        //************************ User Defined Function ********************//
        private void DeleteLetCheckoutDiscoutInfo()
        {
            int dlcRegistrationId = !string.IsNullOrWhiteSpace(ddlRegistrationId.SelectedValue) ? Convert.ToInt32(ddlRegistrationId.SelectedValue) : 0;
            try
            {
                if (dlcRegistrationId > 0)
                {
                    HMCommonDA hmCommonDA = new HMCommonDA();

                    Boolean status = hmCommonDA.DeleteInfoById("HotelGuestDayLetCheckOut", "RegistrationId", dlcRegistrationId);
                    if (status)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut.ToString(), dlcRegistrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelGuestDayLetCheckOut));
                        this.SearchClickDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Data Deleted Failed.";
            }
        }
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
                        HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
                    }
                }
                else
                {
                    HiddenStartDate.Value = hmUtility.GetFromDate();
                    HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
                }
            }
            else
            {
                HiddenStartDate.Value = hmUtility.GetFromDate();
                HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
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

            this.ddlMBankingBankId.DataSource = entityBOList;
            this.ddlMBankingBankId.DataTextField = "BankName";
            this.ddlMBankingBankId.DataValueField = "BankId";
            this.ddlMBankingBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlCompanyBank.Items.Insert(0, itemBank);
            this.ddlMBankingBankId.Items.Insert(0, itemBank);
        }
        private void SearchButtonClick()
        {
            this.DayLetDiscountInputDiv.Visible = false;
            this.AdvanceTodaysRoomChargeDiv.Visible = true;
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
            }
        }
        private void SearchClickDetails()
        {
            txtGuestCheckInDate.Text = string.Empty;
            txtExpectedCheckOutDate.Text = string.Empty;
            this.chkIsBillSplit.Checked = false;
            Session["GuestPaymentDetailListForGrid"] = null;
            this.SearchByRoomNumber();

            if (this.ddlRoomId.SelectedValue != "0")
            {
                //--Remove Paid By RoomId from Room Number List----------------------------------------------
                if (this.ddlRoomId.SelectedIndex != -1)
                {
                    AlartMessege.Visible = true;
                    this.ddlPaidByRegistrationId.Items.Remove(ddlPaidByRegistrationId.Items.FindByValue(this.ddlRoomId.SelectedValue));
                    SetDayLetsInformation(Int32.Parse(this.ddlRegistrationId.SelectedValue));
                    LoadRoomGridView();
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
                this.CalculateSalesTotal();
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
        private void LoadRegistrationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> entityBOList = new List<RoomRegistrationBO>();
            entityBOList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            if (entityBOList.Count > 0)
            {
                txtGuestCheckInDate.Text = hmUtility.GetStringFromDateTime(entityBOList[0].ArriveDate);
                txtExpectedCheckOutDate.Text = hmUtility.GetStringFromDateTime(entityBOList[0].ExpectedCheckOutDate);

                this.ddlRegistrationId.DataSource = entityBOList;
                this.ddlRegistrationId.DataTextField = "RegistrationNumber";
                this.ddlRegistrationId.DataValueField = "RegistrationId";
                this.ddlRegistrationId.DataBind();

                this.txtSrcRegistrationIdList.Value = this.ddlRegistrationId.SelectedValue.ToString();

                // // // Linked Room Related Information ------------------------------------
                //this.LoadLinkedRoomInfo(Int64.Parse(this.ddlRegistrationId.SelectedValue));

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
                    this.hfSelectedRoomId.Value = this.Session["AddedExtraRoomId"].ToString();
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
        }
        private void LoadRoomNumber()
        {
            int condition = 0;
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            this.ddlRoomId.DataSource = roomNumberDA.GetRoomNumberInfoByCondition(0, condition);
            this.ddlRoomId.DataTextField = "RoomNumber";
            this.ddlRoomId.DataValueField = "RoomId";
            this.ddlRoomId.DataBind();

            ListItem itemRoom = new ListItem();
            itemRoom.Value = "0";
            itemRoom.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomId.Items.Insert(0, itemRoom);

            this.ddlPaidByRegistrationId.DataSource = roomNumberDA.GetRoomNumberInfoByCondition(0, condition);
            this.ddlPaidByRegistrationId.DataTextField = "RoomNumber";
            this.ddlPaidByRegistrationId.DataValueField = "RoomId";
            this.ddlPaidByRegistrationId.DataBind();
            this.ddlPaidByRegistrationId.Items.Insert(0, itemRoom);
        }
        private void LoadLinkedRoomInfo(Int64 registrationId)
        {
            string totalAddedRoomIdList = string.Empty, totalAddedRegistrationIdList = string.Empty;
            totalAddedRoomIdList = hfSelectedRoomId.Value;
            totalAddedRegistrationIdList = txtAddedMultipleRoomId.Value;

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<HotelLinkedRoomDetailsBO> LinkedRoomListBO = new List<HotelLinkedRoomDetailsBO>();
            LinkedRoomListBO = roomRegistrationDA.GetLinkedDetailsRoomInfoByRegistrationId(registrationId).Where(x => x.RegistrationId != registrationId).ToList();
            if (LinkedRoomListBO != null)
            {
                if (LinkedRoomListBO.Count > 0)
                {
                    foreach (HotelLinkedRoomDetailsBO row in LinkedRoomListBO)
                    {
                        if (!string.IsNullOrWhiteSpace(totalAddedRoomIdList))
                        {
                            totalAddedRoomIdList = totalAddedRoomIdList + "," + row.RoomId.ToString();
                            totalAddedRegistrationIdList = totalAddedRegistrationIdList + "," + row.RegistrationId.ToString();
                        }
                        else
                        {
                            totalAddedRoomIdList = row.RoomId.ToString();
                            totalAddedRegistrationIdList = row.RegistrationId.ToString();
                        }
                    }
                }
            }

            hfSelectedRoomId.Value = totalAddedRoomIdList;
            Session["AddedExtraRoomId"] = this.hfSelectedRoomId.Value.ToString();

            this.txtSrcRegistrationIdList.Value = totalAddedRegistrationIdList;
            Session["AddedExtraRoomInformation"] = totalAddedRegistrationIdList;
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
                GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                string registrationId = txtSrcRegistrationIdList.Value;

                HMCommonDA hmCommonDA = new HMCommonDA();
                string registrationIdList = hmCommonDA.GetRegistrationIdList(registrationId);
                List<GuestHouseCheckOutDetailBO> files = da.GetGuestHouseBill(registrationIdList, "GuestRoom", userInformationBO.UserInfoId);
                txtSrcRegistrationIdList.Value = registrationIdList;

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
                    this.txtIndividualRoomCitySDCharge.Text = "0";
                    this.txtIndividualRoomAdditionalCharge.Text = "0";
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
                this.txtIndividualRoomCitySDCharge.Text = "0";
                this.txtIndividualRoomAdditionalCharge.Text = "0";
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

                    this.txtIndividualServiceServiceCharge.Text = "0";
                    this.txtIndividualServiceCitySDCharge.Text = "0";
                    this.txtIndividualServiceVatAmount.Text = "0";
                    this.txtIndividualServiceAdditionalCharge.Text = "0";
                    this.txtIndividualServiceDiscountAmount.Text = "0";
                    this.txtIndividualServiceGrandTotal.Text = "0";
                    this.txtIndividualServiceGrandTotalUSD.Text = "0";
                }
            }
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
            //this.txtSalesTotal.Text = (calculatedSalesTotal).ToString("#0.00");
            //this.HiddenFieldSalesTotal.Value = (calculatedSalesTotal).ToString("#0.00");

            //USD Calculation ------------------------------------------------------------------
            decimal calculatedExtraRoomTotalUSD = 0;
            decimal calculatedAdvancePaymentAmountTotalUSD = !string.IsNullOrWhiteSpace(this.txtAdvancePaymentAmountUSD.Text) ? Convert.ToDecimal(this.txtAdvancePaymentAmountUSD.Text) : 0;
            decimal calculatedGuestRoomTotalUSD = !string.IsNullOrWhiteSpace(this.txtIndividualRoomGrandTotalUSD.Text) ? Convert.ToDecimal(this.txtIndividualRoomGrandTotalUSD.Text) : 0;
            decimal calculatedGuestServiceTotalUSD = !string.IsNullOrWhiteSpace(this.txtIndividualServiceGrandTotalUSD.Text) ? Convert.ToDecimal(this.txtIndividualServiceGrandTotalUSD.Text) : 0;
            decimal calculatedRestaurantTotalUSD = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantGrandTotalUSD.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantGrandTotalUSD.Text) : 0;
            decimal calculatedSalesTotalUSD = (calculatedGuestRoomTotalUSD + calculatedGuestServiceTotalUSD + calculatedRestaurantTotalUSD + calculatedExtraRoomTotalUSD) - calculatedAdvancePaymentAmountTotalUSD;
            this.txtSalesTotalUsd.Text = Math.Round(calculatedSalesTotalUSD).ToString("#0.00");
            //this.txtSalesTotalUsd.Text = (calculatedSalesTotalUSD).ToString("#0.00");

            this.txtDiscountAmount.Text = "0.00";
            this.txtGrandTotal.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            this.txtGrandTotalInfo.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            this.HiddenFieldGrandTotal.Value = Math.Round(calculatedSalesTotal).ToString("#0.00");

            //this.txtGrandTotal.Text = (calculatedSalesTotal).ToString("#0.00");
            //this.txtGrandTotalInfo.Text = (calculatedSalesTotal).ToString("#0.00");
            //this.HiddenFieldGrandTotal.Value = (calculatedSalesTotal).ToString("#0.00");

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
            this.txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtBillDate.Enabled = false;

            HiddenStartDate.Value = this.txtStartDate.Text;
            HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
        }
        private void Clear()
        {
            txtBillDate.Text = string.Empty;
            txtGuestCheckInDate.Text = string.Empty;
            txtExpectedCheckOutDate.Text = string.Empty;
            txtAdvancePaymentAmount.Text = string.Empty;
            txtSalesTotal.Text = string.Empty;
            txtSalesTotalUsd.Text = string.Empty;
            txtRebateRemarks.Text = string.Empty;
            txtIndividualRoomVatAmount.Text = string.Empty;
            txtIndividualRoomServiceCharge.Text = string.Empty;
            txtIndividualRoomCitySDCharge.Text = string.Empty;
            txtIndividualRoomAdditionalCharge.Text = string.Empty;
            txtIndividualRoomDiscountAmount.Text = string.Empty;
            txtIndividualRoomGrandTotal.Text = string.Empty;
            txtIndividualRoomGrandTotalUSD.Text = string.Empty;
            txtIndividualServiceVatAmount.Text = string.Empty;
            txtIndividualServiceServiceCharge.Text = string.Empty;
            txtIndividualServiceCitySDCharge.Text = string.Empty;
            txtIndividualServiceAdditionalCharge.Text = string.Empty;
            txtIndividualServiceDiscountAmount.Text = string.Empty;
            txtIndividualServiceGrandTotal.Text = string.Empty;
            txtIndividualServiceGrandTotalUSD.Text = string.Empty;
            ddlRegistrationId.SelectedIndex = -1;

            HttpContext.Current.Session["AddedExtraRoomInformation"] = null;
            HttpContext.Current.Session["AddedExtraRoomId"] = null;
            HttpContext.Current.Session["HiddenFieldCompanyPaymentButtonInfo"] = null;
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = null;

            txtServiceChargeTotal.Text = string.Empty;
            txtVatTotal.Text = string.Empty;
            txtCitySDChargeTotal.Text = string.Empty;
            txtAdditionalChargeTotal.Text = string.Empty;
            txtDiscountAmountTotal.Text = string.Empty;
            txtSalesTotal.Text = string.Empty;
            txtSalesTotalUsd.Text = string.Empty;
            txtGrandTotal.Text = string.Empty;

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
            //else if (isSingle == false)
            //{
            //    if (this.ddlGLCompany.SelectedValue == "0")
            //    {
            //        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
            //        if (isIntegrated)
            //        {
            //            CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Company Name.", AlertType.Warning);
            //            this.ddlGLCompany.Focus();
            //            flag = false;
            //        }
            //    }
            //    else if (this.ddlGLCompany.SelectedValue != "0")
            //    {
            //        if (this.ddlGLProject.SelectedValue == "0")
            //        {
            //            Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
            //            if (isIntegrated)
            //            {
            //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Project Name.", AlertType.Warning);
            //                this.ddlGLProject.Focus();
            //                flag = false;
            //            }
            //        }
            //    }
            //}
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "", AlertType.Warning);
            }
            return flag;
        }
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
                if (roomId > 0)
                {
                    this.LoadRegistrationNumber(roomId);
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
                    if (Session["AddedExtraRoomInformation"].ToString() != "")
                    {
                        registrationIdList = registrationIdList + "," + Session["AddedExtraRoomInformation"];
                    }

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

                List<PaymentSummaryBO> paymentSummaryBOList = new List<PaymentSummaryBO>();
                GuestBillPaymentDA paymentSummaryDA = new GuestBillPaymentDA();

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
            hfIsStopChargePosting.Value = "0";
            this.lblMessage.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
                if (roomAllocationBO.RoomId > 0)
                {
                    if (roomAllocationBO.TransactionDate.Date == roomAllocationBO.ExpectedCheckOutDate.Date)
                    {
                        if (roomAllocationBO.IsStopChargePosting)
                        {
                            hfIsStopChargePosting.Value = "1";
                        }
                        isInValidRoomNumberForTodaysCheckOut = 0;
                        this.ddlRoomId.SelectedValue = roomAllocationBO.RoomId.ToString();
                        this.lblRegistrationNumber.Visible = true;
                        this.ddlRegistrationId.Visible = true;
                        this.txtConversionRate.Text = roomAllocationBO.ConversionRate.ToString();
                        this.txtConversionRateHiddenField.Value = roomAllocationBO.ConversionRate.ToString();
                        this.btnLocalBillPreview.Text = "Bill Preview" + " (" + roomAllocationBO.LocalCurrencyHead + ")";
                        this.btnUSDBillPreview.Text = "Bill Preview (USD)";

                        this.txtStartDate.Text = hmUtility.GetStringFromDateTime(roomAllocationBO.ArriveDate);
                        HiddenStartDate.Value = this.txtStartDate.Text;
                        HiddenEndDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now);
                        Session["txtStartDate"] = this.txtStartDate.Text;

                        if (!string.IsNullOrWhiteSpace(roomAllocationBO.Remarks))
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Note: " + roomAllocationBO.Remarks, AlertType.Information, 30000);
                        }

                        this.LoadRelatedInformation();

                        hfMasterRoomCurrencyType.Value = roomAllocationBO.CurrencyType.ToString();
                        hfMasterRoomConversionRate.Value = roomAllocationBO.ConversionRate.ToString();
                        // //----------Currency Related Information ------------------------------------------------
                        if (roomAllocationBO.CurrencyType == 2)
                        {
                            USDInformationDiv.Visible = true;
                            isFOMultiInvoicePreviewOptionEnable = 1;                            
                        }
                        else
                        {
                            USDInformationDiv.Visible = false;
                            isFOMultiInvoicePreviewOptionEnable = 0;
                        }

                        //--Remove Paid By RoomId from Room Number List----------------------------------------------
                        if (this.ddlPayMode.SelectedIndex != -1)
                        {
                            this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Company"));
                        }

                        this.HiddenFieldCompanyPaymentButtonInfo.Value = "0";

                        if (roomAllocationBO.CompanyId > 0)
                        {
                            GuestCompanyBO companyBO = new GuestCompanyBO();
                            GuestCompanyDA companyDA = new GuestCompanyDA();
                            companyBO = companyDA.GetGuestCompanyInfoById(roomAllocationBO.CompanyId);
                            if (companyBO != null)
                            {
                                if (companyBO.CompanyId > 0)
                                {
                                    if (companyBO.ActiveStat)
                                    {
                                        ListItem itemRoom = new ListItem();
                                        itemRoom.Value = "Company";
                                        itemRoom.Text = "Company";
                                        this.ddlPayMode.Items.Insert(4, itemRoom);

                                        this.ddlCompanyPaymentAccountHead.Enabled = false;
                                        this.HiddenFieldCompanyPaymentButtonInfo.Value = "1";
                                    }
                                }
                            }

                            //ListItem itemRoom = new ListItem();
                            //itemRoom.Value = "Company";
                            //itemRoom.Text = "Company";
                            //this.ddlPayMode.Items.Insert(4, itemRoom);

                            //this.ddlCompanyPaymentAccountHead.Enabled = false;
                            //this.HiddenFieldCompanyPaymentButtonInfo.Value = "1";
                        }

                        Session["HiddenFieldCompanyPaymentButtonInfo"] = this.HiddenFieldCompanyPaymentButtonInfo.Value;
                    }
                    else
                    {
                        isInValidRoomNumberForTodaysCheckOut = 1;
                        this.lblRegistrationNumber.Visible = false;
                        this.ddlRegistrationId.Visible = false;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number for Today's Check Out.", AlertType.Warning);
                        this.txtSrcRoomNumber.Focus();
                        return;
                    }
                }
                else
                {
                    this.lblRegistrationNumber.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                    return;
                }
            }
            else
            {
                this.lblRegistrationNumber.Visible = false;
                this.ddlRegistrationId.Visible = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                return;
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

            this.ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + mBankingPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlMBankingReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlMBankingReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlMBankingReceiveAccountsInfo.DataBind();
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
                string url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx?sdc=1";
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=825,height=680,left=300,top=50,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
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
                        if(dr.PaymentMode == "")
                        { }
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode.Replace("Other Room", "Guest Room") + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode.Replace("Other Room", "Guest Room") + "</td>";
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

            int rebateAccountsPostingHeadId = 0;
            rebateAccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHeadForDiscount.SelectedValue);

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("Rebate");
            if (commonPaymentModeBOList != null)
            {
                if (commonPaymentModeBOList.Count > 0)
                {
                    rebateAccountsPostingHeadId = Convert.ToInt32(commonPaymentModeBOList[0].PaymentAccountsPostingId);
                }
            }

            guestBillPaymentBO.NodeId = rebateAccountsPostingHeadId;
            guestBillPaymentBO.AccountsPostingHeadId = rebateAccountsPostingHeadId;

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
        public static string GetHTMLRoomGridView(List<RoomNumberBO> List, List<ReservationDetailBO> reservationDetailList)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableRoomInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>&nbsp;&nbsp;&nbsp;&nbsp;Select </br>";
            strTable += " <input id = 'chkAllRooms' value = 'checkAll' type = 'checkbox' onclick='CheckAllRooms()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /></th>";
            strTable += "<th align='left' scope='col'>Room Number</th></tr>";

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
                //if (dr.IsLinkedRoom)
                //{
                //    strTable += "&nbsp;<input type='checkbox' checked='true' id='" + dr.RoomId + "' name='" + dr.RoomNumber + "' value='" + dr.RoomId + "' >";
                //}
                //else
                //{
                    strTable += "&nbsp;<input type='checkbox'  id='" + dr.RoomId + "' name='" + dr.RoomNumber + "' value='" + dr.RoomId + "' >";
                //}
                strTable += "</td><td align='left' style='width: 138px'>" + dr.RoomNumber + "</td>";
                strTable += "<td style='display:none;'>" + dr.RegistrationId + "</td></tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod(EnableSession = true)]
        public static string PerformBillSplitePrintPreview(string currencyRate, string isIsplite, string serviceType, string SelectdIndividualTransferedPaymentId, string SelectdPaymentId, string SelectdIndividualPaymentId, string SelectdIndividualServiceId, string SelectdIndividualRoomId, string SelectdServiceId, string SelectdRoomId, string StartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
            return "";
        }
        [WebMethod(EnableSession = true)]
        public static string PerformBillSplitePrintPreviewAndBillLock(string salesTotal, string grandTotal, string rebateDescription, string currencyRate, string isIsplite, string serviceType, string SelectdIndividualTransferedPaymentId, string SelectdPaymentId, string SelectdIndividualPaymentId, string SelectdIndividualServiceId, string SelectdIndividualRoomId, string SelectdServiceId, string SelectdRoomId, string StartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            if (salesTotal != "0")
            {
                int dynamicDetailId = 0;
                int ddlPaidByRegistrationId = 0;

                HMUtility hmUtility2 = new HMUtility();
                UserInformationBO userInformationBO2 = new UserInformationBO();
                userInformationBO2 = hmUtility2.GetCurrentApplicationUserInfo();

                GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
                ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);

                int rebateAccountsPostingHeadId = 0;
                HMCommonDA hmCommonDA = new HMCommonDA();
                List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
                commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("Rebate");
                if (commonPaymentModeBOList != null)
                {
                    if (commonPaymentModeBOList.Count > 0)
                    {
                        rebateAccountsPostingHeadId = Convert.ToInt32(commonPaymentModeBOList[0].PaymentAccountsPostingId);
                    }
                }

                guestBillPaymentBO.NodeId = rebateAccountsPostingHeadId;
                guestBillPaymentBO.AccountsPostingHeadId = rebateAccountsPostingHeadId;
                guestBillPaymentBO.ModuleName = "FrontOffice";
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.PaymentType = "Rebate";
                guestBillPaymentBO.BankId = Convert.ToInt32(0);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.FieldId = 1; // LocalCurrencyId;
                guestBillPaymentBO.ConvertionRate = 1;

                decimal totalSalesAmount = !string.IsNullOrWhiteSpace(salesTotal) ? Convert.ToDecimal(salesTotal) : 0;
                decimal grandTotalAmount = !string.IsNullOrWhiteSpace(grandTotal) ? Convert.ToDecimal(grandTotal) : 0;

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
                guestBillPaymentBO.PaymentDescription = rebateDescription;

                if (guestBillPaymentBO.PaymentAmount > 0)
                {
                    int tmpPaymentId = 0;
                    guestBillPaymentBO.CreatedBy = userInformationBO2.UserInfoId;
                    GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();
                    Boolean status = reservationBillPaymentDA.SaveGuestBillPaymentInfo(guestBillPaymentBO, out tmpPaymentId, "Others");
                    if (status)
                    {
                        Boolean logStatus = hmUtility2.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), tmpPaymentId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility2.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                    }
                }
            }

            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);

            Int64 registrationId = !string.IsNullOrWhiteSpace(ddlRegistrationId) ? Convert.ToInt64(ddlRegistrationId) : 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();

            string[] RegistrationIdList = txtSrcRegistrationIdList.Split(',');
            foreach (string regiId in RegistrationIdList)
            {
                Boolean status = registrationDA.UpdateBillPrintPreviewAndBillLock(Convert.ToInt64(regiId), registrationId, userInformationBO.UserInfoId);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), registrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomStopChargePostingDetails.ToString(), registrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomStopChargePostingDetails) + "deleted by registrationId");
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomStopChargePostingDetails.ToString(), registrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomStopChargePostingDetails) + ".entityId is registrationId");
                }
            }

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
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlCurrency, string currencyType, string localCurrencyId, string conversionRate, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlRegistrationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlMBankingBankId, string ddlMBankingReceiveAccountsInfo, string ddlPaidByRoomId, string RefundAccountHead)
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
                else if (ddlPayMode == "Cheque")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                }
                if (ddlPayMode == "M-Banking")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
                }

                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.PaymentType = "Payment";
            }

            guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
            if (ddlPayMode == "M-Banking")
            {
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlMBankingBankId);
            }

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
                //Payment Information----------------
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
        [WebMethod(EnableSession = true)]
        public static string LoadRoomInformationWithControl(Int64 registrationId)
        {
            string HTML = string.Empty;
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<ReservationDetailBO> reservationDetailList = new List<ReservationDetailBO>();
            List<RoomNumberBO> TodaysExpectedCheckOutRoomNumberInfoBO = new List<RoomNumberBO>();
            TodaysExpectedCheckOutRoomNumberInfoBO = roomNumberDA.GetTodaysExpectedCheckOutRoomNumberInfo().Where(x => x.RegistrationId != registrationId).ToList();

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<HotelLinkedRoomDetailsBO> LinkedRoomListBO = new List<HotelLinkedRoomDetailsBO>();
            LinkedRoomListBO = roomRegistrationDA.GetLinkedDetailsRoomInfoByRegistrationId(registrationId).Where(x => x.RegistrationId != registrationId).ToList();

            if (LinkedRoomListBO != null)
            {
                if (LinkedRoomListBO.Count > 0)
                {
                    foreach (HotelLinkedRoomDetailsBO row in LinkedRoomListBO)
                    {
                        var objRoom = TodaysExpectedCheckOutRoomNumberInfoBO.Where(r => r.RegistrationId == row.RegistrationId).FirstOrDefault();

                        if (objRoom != null)
                        {
                            objRoom.IsLinkedRoom = true;
                        }

                        //TodaysExpectedCheckOutRoomNumberInfoBO.Remove(regiId);

                        //TodaysExpectedCheckOutRoomNumberInfoBO.Where(r => r.RegistrationId == row.RegistrationId).FirstOrDefault().IsLinkedRoom = true;
                    }
                }
            }

            HTML = GetHTMLRoomGridView(TodaysExpectedCheckOutRoomNumberInfoBO, reservationDetailList);
            return HTML;
        }
    }
}