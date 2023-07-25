using HotelManagement.Data;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGHServiceBill : BasePage
    {
        HiddenField innboardMessage;
        Boolean isDayClosed = false;
        protected bool isSingle = true;
        protected int isOutSideGuestInfoEnable = -1;
        private int paidServiceId;
        protected int isGridInfoEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected int isCompanyProjectPanelEnable = -1;
        protected int IsConfigurableTemplateVisible = -1;
        protected int IsPaidServicePanelVisible = -1;
        protected int IsServiceChargeEnableConfig = 1;
        protected int IsCitySDChargeEnableConfig = 1;
        protected int IsVatEnableConfig = 1;
        protected int IsAdditionalChargeEnableConfig = 1;
        private Boolean isServiceRateEditableEnable = false;
        protected int LocalCurrencyId;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            isSingle = hmUtility.GetSingleProjectAndCompany();
            string ss = hfTxtPaidServiceDate.Value;

            if (!IsPostBack)
            {
                Session["dayClosedDateBOList"] = null;
                this.LoadIsServiceRateEditablEnable();
                this.LoadIsServiceBillWithoutInHouseGuest();
                this.LoadCommonSetupForRackRateServiceChargeVatInformation();
                this.LoadCurrentDate();
                this.LoadRoomNumber();
                this.LoadCurrency();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                this.LoadBank();
                this.LoadAccountHeadInfo();
                this.LoadGuestHouseService();
                this.LoadGuestCompany();
                this.LoadEmployee();
                if (isSingle == true)
                {
                    isCompanyProjectPanelEnable = 1;
                    //LoadSingleProjectAndCompany();
                }
                else
                {
                    //this.LoadGLCompany(false);
                    //this.LoadGLProject(false);
                }
                this.LoadCommonDropDownHiddenField();
                this.ddlRoomId.SelectedValue = "0";
                string queryStringId = Request.QueryString["AddMoreService"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    this.ddlRoomId.SelectedValue = queryStringId;
                    this.lblGuestTypeDiv.Visible = false;
                    this.ddlGuestType.Visible = false;
                }
                else
                {
                    this.lblGuestTypeDiv.Visible = true;
                    this.ddlGuestType.Visible = true;
                }
                this.LoadRelatedInformation();

                string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
                if (!string.IsNullOrWhiteSpace(DeleteSuccess))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    //LoadGridView();
                    SetTab("SearchTab");
                }

                if (Session["CheckOutPayMode"] != null)
                {
                    this.ddlRoomId.Enabled = false;
                    this.btnBackToCheckOutForm.Visible = true;
                    Session["CheckOutPayMode"] = null;
                }
                else
                {
                    this.ddlRoomId.Enabled = true;
                    this.btnBackToCheckOutForm.Visible = false;
                }
                this.lblRegistrationNumberDiv.Visible = false;
                this.ddlRegistrationId.Visible = false;
                this.chkAllActiveReservation.Visible = false;
                this.lblActivePaidServiceList.Visible = false;

                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcRoomNumber.Text = numberBO.RoomNumber;
                    this.LoadSearchInformation();
                }

                //CostCenterWiseSetting();
                //CheckPermission();
            }

            CostCenterWiseSetting();
            CheckPermission();
        }
        protected void btnBackToCheckOutForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("/HotelManagement/frmRoomCheckOut.aspx?BackFromServiceBill=" + this.ddlRoomId.SelectedValue);
        }
        protected void btnSearchServiceBill_Click(object sender, EventArgs e)
        {
            Session["dayClosedDateBOList"] = null;
            LoadGridView();
            SetTab("SearchTab");

        }
        protected void gvGHServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGHServiceBill.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvGHServiceBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            btnSave.Visible = isUpdatePermission;
            if (e.Row.DataItem != null)
            {
                GHServiceBillBO gsb = (GHServiceBillBO)e.Row.DataItem;

                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "', '" + gsb.RoomNumber + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;

                Label lblBillEdditableStatusValue = (Label)e.Row.FindControl("lblBillEdditableStatus");

                if (lblBillEdditableStatusValue.Text == "0")
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
            }
        }
        protected void gvGHServiceBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int serviceBillId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdPreview")
            {
                string url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + serviceBillId.ToString();
                string s = "window.open('" + url + "', 'popup_window', 'width=810,height=680,left=300,top=50,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                this.SetTab("SearchTab");
            }
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            this.LoadSearchInformation();
        }
        protected void btnSrcBillInfo_Click(object sender, EventArgs e)
        {
            //this.LoadFormByRegistrationNumber();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.LoadCurrentDate();
            decimal paidServiceRate;
            DateTime paidServiceDate;
            if (chkAllActiveReservation.Checked)
            {
                ApprovedGuestService("RestaurantService", Convert.ToInt32(this.ddlPaidServiceId.SelectedValue), out paidServiceId, out paidServiceRate, out paidServiceDate);
                this.txtServiceQuantity.Text = "1";
                this.txtServiceRate.Text = paidServiceRate.ToString();
            }

            if (!isValidForm())
            {
                return;
            }

            Boolean isNumberServiceRate = hmUtility.IsNumber(this.txtServiceRate.Text);
            if (!isNumberServiceRate)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Numeric Value.", AlertType.Warning);
                this.txtServiceRate.Focus();
                return;
            }

            Boolean isNumberServiceQuantity = hmUtility.IsNumber(this.txtServiceQuantity.Text);
            if (!isNumberServiceQuantity)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Numeric Value.", AlertType.Warning);
                this.txtServiceQuantity.Focus();
                return;
            }

            if (this.ddlGuestType.SelectedValue == "InHouseGuest")
            {
                if (!string.IsNullOrWhiteSpace(this.hfGuestCheckInDate.Value))
                {
                    DateTime guestBillingStartDate = Convert.ToDateTime(this.hfBillingStartDate.Value);
                    DateTime guestServiceDateTime = hmUtility.GetDateTimeFromString(this.txtServiceDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    DateTime currentDateTime = DateTime.Now.Date;
                    if (guestServiceDateTime.Date < guestBillingStartDate.Date)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Service bill cannot be posted before registration date.", AlertType.Warning);
                        this.txtServiceDate.Focus();
                        return;
                    }

                    if (guestServiceDateTime > currentDateTime)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Service bill cannot be posted in future date.", AlertType.Warning);
                        this.txtServiceDate.Focus();
                        return;
                    }
                }
            }

            RoomNumberDA numberDA = new RoomNumberDA();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomRegistrationBO registrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Int32.Parse(this.ddlRegistrationId.SelectedValue));
            if (registrationBO.IsGuestCheckedOut == 1)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                return;
            }

            int isSucceed = 0;
            DateTime ServiceDate = hmUtility.GetDateTimeFromString(this.txtServiceDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            int serviceBillId = !string.IsNullOrWhiteSpace(hfServiceBillId.Value) ? Convert.ToInt32(hfServiceBillId.Value) : 0;
            GHServiceBillDA serviceBillDA = new GHServiceBillDA();

            ActivityLogsBO activityLogBO = new ActivityLogsBO();
            ActivityLogsDA activityLogDA = new ActivityLogsDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<GuestBillPaymentBO> ghServiceNewBillList = new List<GuestBillPaymentBO>();
            List<GuestBillPaymentBO> ghServiceEditBillList = new List<GuestBillPaymentBO>();
            List<GuestBillPaymentBO> ghServiceDeleteBillList = new List<GuestBillPaymentBO>();

            GHServiceBillBO ghServiceBillBO = new GHServiceBillBO();
            GHServiceBillDA ghServiceBillDA = new GHServiceBillDA();

            if (this.ddlGuestType.SelectedIndex == 0)
            {
                if (!string.IsNullOrWhiteSpace(hfddlRegistrationId.Value))
                {
                    ghServiceBillBO.RegistrationId = Convert.ToInt32(hfddlRegistrationId.Value);
                }
                else
                {
                    ghServiceBillBO.RegistrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                }
                ghServiceBillBO.GuestName = txtRegisteredGuestName.Text;

                if (this.ddlIsComplementary.SelectedIndex == 0)
                {
                    ghServiceBillBO.IsComplementary = false;
                }
                else
                {
                    ghServiceBillBO.IsComplementary = true;
                }
            }
            else
            {
                ghServiceBillBO.RegistrationId = 0;
                ghServiceBillBO.GuestName = this.txtGuestName.Text;

                if (this.ddlIsComplementary.SelectedIndex == 0)
                {
                    ghServiceBillBO.IsComplementary = false;
                }
                else
                {
                    ghServiceBillBO.IsComplementary = true;
                }
            }

            ghServiceBillBO.ServiceDate = hmUtility.GetDateTimeFromString(this.txtServiceDate.Text, userInformationBO.ServerDateFormat);

            if (chkAllActiveReservation.Checked)
            {
                ghServiceBillBO.ServiceId = paidServiceId;
                ghServiceBillBO.IsPaidService = true;
            }
            else
            {
                ghServiceBillBO.ServiceId = Convert.ToInt32(this.ddlServiceId.SelectedValue);
                ghServiceBillBO.ServiceName = ddlServiceId.SelectedItem.Text;
            }

            ghServiceBillBO.ServiceQuantity = Convert.ToInt32(this.txtServiceQuantity.Text);
            ghServiceBillBO.Remarks = this.txtRemarks.Text;

            if (ddlPayMode.SelectedValue == "Employee")
            {
                if (!string.IsNullOrWhiteSpace(this.ddlEmployee.SelectedValue))
                {
                    ghServiceBillBO.EmpId = Convert.ToInt32(ddlEmployee.SelectedValue.Split('~')[0].ToString());
                }
            }
            else if (ddlPayMode.SelectedValue == "Company")
            {
                if (!string.IsNullOrWhiteSpace(this.ddlCompany.SelectedValue))
                {
                    ghServiceBillBO.CompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
                }
            }

            decimal ledgerAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;

            if (!string.IsNullOrEmpty(this.txtDiscountAmount.Text))
            {
                ghServiceBillBO.DiscountAmount = Convert.ToDecimal(this.txtDiscountAmount.Text);
            }
            else
            {
                ghServiceBillBO.DiscountAmount = 0;
            }

            if (ddlGuestType.SelectedValue == "InHouseGuest")
            {
                if (this.ddlIsComplementary.SelectedIndex == 1)
                {
                    ghServiceBillBO.ServiceRate = 0;
                    ghServiceBillBO.DiscountAmount = 0;
                }
            }
            else if (ddlGuestType.SelectedValue == "OutSideGuest")
            {
                if (this.ddlIsComplementary.SelectedIndex == 1)
                {
                    ghServiceBillBO.ServiceRate = 0;
                    ghServiceBillBO.DiscountAmount = 0;
                }
            }

            if (this.ddlGuestType.SelectedIndex != 0)
            {
                if (hfAddedPaymentDetails.Value.ToString().Trim() != string.Empty)
                    ghServiceNewBillList = ProcessServiceBillPayment(hfAddedPaymentDetails.Value.ToString());

                if (hfEditPaymentDetails.Value.ToString().Trim() != string.Empty)
                    ghServiceEditBillList = ProcessServiceBillPayment(hfEditPaymentDetails.Value.ToString());

                if (hfDeletePaymentDetails.Value.ToString().Trim() != string.Empty)
                    ghServiceDeleteBillList = ProcessServiceBillPayment(hfDeletePaymentDetails.Value.ToString());
            }
            else
            {
                ghServiceBillBO.FieldId = LocalCurrencyId;
                ghServiceBillBO.PaymentAmout = ledgerAmount;
                ghServiceBillBO.CurrencyAmount = ledgerAmount;
                ghServiceBillBO.ChecqueDate = DateTime.Now;
            }

            ghServiceBillBO.IsOnlyRateEffectEnable = true;
            //if (this.cbOnlyRateEffect.Checked)
            //{
            //    ghServiceBillBO.IsOnlyRateEffectEnable = true;
            //}
            //else
            //{
            //    ghServiceBillBO.IsOnlyRateEffectEnable = false;
            //}

            if (this.cbServiceCharge.Checked)
            {
                ghServiceBillBO.IsServiceChargeEnable = true;
            }
            else
            {
                ghServiceBillBO.IsServiceChargeEnable = false;
            }

            if (this.cbVatAmount.Checked)
            {
                ghServiceBillBO.IsVatAmountEnable = true;
            }
            else
            {
                ghServiceBillBO.IsVatAmountEnable = false;
            }

            if (isServiceRateEditableEnable)
            {
                if (!string.IsNullOrEmpty(txtServiceRate.Text))
                    ghServiceBillBO.ServiceRate = Convert.ToDecimal(txtServiceRate.Text);
            }
            else
            {
                ghServiceBillBO.ServiceRate = Convert.ToDecimal(hfServiceRate.Value);
            }

            ghServiceBillBO.ServiceCharge = Convert.ToDecimal(hfServiceCharge.Value);
            ghServiceBillBO.VatAmount = Convert.ToDecimal(hfVatAmount.Value);
            ghServiceBillBO.RackRate = Convert.ToDecimal(hfRackRate.Value);
            ghServiceBillBO.CitySDCharge = Convert.ToDecimal(hfSDChargeAmount.Value);
            ghServiceBillBO.AdditionalCharge = Convert.ToDecimal(hfAdditionalChargeAmount.Value);

            if (cbAdditionalCharge.Checked)
                ghServiceBillBO.IsAdditionalChargeEnable = true;
            else
                ghServiceBillBO.IsAdditionalChargeEnable = false;

            if (cbSDCharge.Checked)
                ghServiceBillBO.IsCitySDChargeEnable = true;
            else
                ghServiceBillBO.IsCitySDChargeEnable = false;

            HMCommonDA hmCommonDA = new HMCommonDA();

            SalesMarketingLogType<GHServiceBillBO> logDA = new SalesMarketingLogType<GHServiceBillBO>();
            GHServiceBillBO previousData = new GHServiceBillBO();

            if (ghServiceNewBillList != null)
            {
                foreach (GuestBillPaymentBO row in ghServiceNewBillList)
                {
                    row.CreatedBy = userInformationBO.UserInfoId;
                }
            }

            long activityId = 0;
            if (string.IsNullOrWhiteSpace(hfServiceBillId.Value))
            {
                int tmpServiceBillId = 0;
                ghServiceBillBO.CreatedBy = userInformationBO.UserInfoId;

                if (ddlGuestType.SelectedValue == "OutSideGuest")
                {
                    if (ghServiceNewBillList != null)
                    {
                        if (ghServiceNewBillList.Count == 0)
                        {
                            GuestBillPaymentBO paymentBO = new GuestBillPaymentBO();
                            paymentBO.PaymentType = "Advance";
                            paymentBO.RegistrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                            paymentBO.Remarks = this.txtRemarks.Text;
                            paymentBO.FieldId = LocalCurrencyId;
                            paymentBO.ConvertionRate = 1;
                            decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                            paymentBO.CurrencyAmount = tmpCurrencyAmount * paymentBO.ConvertionRate;
                            paymentBO.PaymentAmount = tmpCurrencyAmount * paymentBO.ConvertionRate;
                            paymentBO.ChecqueDate = DateTime.Now;
                            paymentBO.PaymentMode = ddlPayMode.SelectedValue;
                            paymentBO.BankId = Convert.ToInt32(0);
                            paymentBO.ServiceBillId = null;
                            paymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashReceiveAccountsInfo.SelectedValue);
                            paymentBO.ChecqueNumber = "";
                            paymentBO.CardReference = "";
                            paymentBO.CardNumber = "";
                            paymentBO.BranchName = "";
                            paymentBO.CreatedBy = userInformationBO.UserInfoId;
                            ghServiceNewBillList.Add(paymentBO);
                        }
                    }
                    else
                    {
                        GuestBillPaymentBO paymentBO = new GuestBillPaymentBO();
                        paymentBO.PaymentType = "Advance";
                        paymentBO.RegistrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                        paymentBO.Remarks = this.txtRemarks.Text;
                        paymentBO.FieldId = LocalCurrencyId;
                        paymentBO.ConvertionRate = 1;
                        decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                        paymentBO.CurrencyAmount = tmpCurrencyAmount * paymentBO.ConvertionRate;
                        paymentBO.PaymentAmount = tmpCurrencyAmount * paymentBO.ConvertionRate;
                        paymentBO.ServiceBillId = null;
                        paymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashReceiveAccountsInfo.SelectedValue);
                        paymentBO.ChecqueNumber = "";
                        paymentBO.CardReference = "";
                        paymentBO.CardNumber = "";
                        paymentBO.BranchName = "";
                        paymentBO.CreatedBy = userInformationBO.UserInfoId;
                        ghServiceNewBillList.Add(paymentBO);
                    }
                }

                Boolean status = ghServiceBillDA.SaveGHServiceBillInfo(ghServiceBillBO, ghServiceNewBillList, out tmpServiceBillId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ServiceBill.ToString(), tmpServiceBillId,
                    ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServiceBill));
                    this.Cancel();
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);

                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                    string url = string.Empty;
                    if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
                    {
                        
                        if(setUpBO.SetupValue == "1")
                        {
                            url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + tmpServiceBillId.ToString() + "&sdc=1";
                        }
                        else
                        {
                            url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + tmpServiceBillId.ToString();
                        }
                    }
                    else
                    {
                        url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + tmpServiceBillId.ToString();
                    }

                    //string url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + tmpServiceBillId.ToString();
                    string s = "window.open('" + url + "', 'popup_window', 'width=810,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            else
            {
                ghServiceBillBO.ServiceBillId = Convert.ToInt32(hfServiceBillId.Value);
                ghServiceBillBO.LastModifiedBy = userInformationBO.UserInfoId;

                previousData = ghServiceBillDA.GetGuestServiceBillInfoById(ghServiceBillBO.ServiceBillId);

                Boolean status = ghServiceBillDA.UpdateGuestServiceBillInfo(ghServiceBillBO, ghServiceNewBillList, ghServiceEditBillList, ghServiceDeleteBillList);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ServiceBill.ToString(), ghServiceBillBO.ServiceBillId,
                         ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServiceBill), out activityId);

                    // // // activity log details related process
                    logDA.LogDetails(ConstantHelper.FOActivityLogFormName.frmGHServiceBill, previousData, ghServiceBillBO, activityId);
                    this.Cancel();
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                }
            }
            SetTab("EntryTab");
        }
        private List<GuestBillPaymentBO> ProcessServiceBillPayment(string paymentInfo)
        {
            List<GuestBillPaymentBO> ghServiceBillList = new List<GuestBillPaymentBO>();
            GuestBillPaymentBO ghServiceBill = new GuestBillPaymentBO();

            string[] paymentDetails = paymentInfo.Split('#');

            foreach (string p in paymentDetails)
            {
                ghServiceBill = new GuestBillPaymentBO();
                string[] payment = p.Split(',');
                if (payment[11].Trim() != "0")
                {
                    ghServiceBill.PaymentId = Convert.ToInt32(payment[11]);
                }
                ghServiceBill.BankId = Convert.ToInt32(payment[12]);
                ghServiceBill.CompanyId = 0;
                ghServiceBill.PaymentType = ddlGuestType.SelectedValue;

                ghServiceBill.PaymentMode = payment[1];
                ghServiceBill.FieldId = Int32.Parse(payment[2]);
                ghServiceBill.ChecqueDate = DateTime.Now;
                ghServiceBill.PaymentAmount = Convert.ToDecimal(payment[0]);

                if (payment[2] == "LocalCurrencyId")
                { ghServiceBill.CurrencyAmount = Convert.ToDecimal(payment[0]); }
                else
                { ghServiceBill.CurrencyAmount = Convert.ToDecimal(payment[13]); }

                if (payment[1] == "Cash")
                {
                    ghServiceBill.AccountsPostingHeadId = Convert.ToInt32(payment[4].Trim());
                }
                else if (payment[1] == "Card")
                {
                    ghServiceBill.AccountsPostingHeadId = Convert.ToInt32(payment[4].Trim());
                    ghServiceBill.CardType = payment[5].Trim();
                    ghServiceBill.CardNumber = payment[6].Trim();

                    if (payment[7].Trim() != "")
                        ghServiceBill.ExpireDate = hmUtility.GetDateTimeFromString(payment[7].Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                    ghServiceBill.CardHolderName = payment[8].Trim();
                }
                else if (payment[1] == "Cheque")
                {
                    ghServiceBill.AccountsPostingHeadId = Int32.Parse(payment[4]);
                    ghServiceBill.ChecqueNumber = txtChecqueNumber.Text;
                    ghServiceBill.ChecqueDate = DateTime.Now;
                }
                else if (payment[1] == "Employee")
                {
                    ghServiceBill.AccountsPostingHeadId = Convert.ToInt32(payment[4].Trim());
                }
                else if (payment[1] == "Company")
                {
                    GuestCompanyBO companyBO = new GuestCompanyBO();
                    GuestCompanyDA companyDA = new GuestCompanyDA();
                    companyBO = companyDA.GetGuestCompanyInfoById(Convert.ToInt32(payment[4].Trim()));
                    if (companyBO != null)
                    {
                        if (companyBO.CompanyId > 0)
                        {
                            ghServiceBill.AccountsPostingHeadId = companyBO.NodeId;
                            ghServiceBill.CompanyId = companyBO.CompanyId;
                        }
                    }
                }
                else if (payment[1] == "Refund")
                {
                    ghServiceBill.AccountsPostingHeadId = Convert.ToInt32(payment[4].Trim());
                }
                ghServiceBillList.Add(ghServiceBill);
            }

            return ghServiceBillList;
        }
        protected void btnPaidServiceDate_Click(object sender, EventArgs e)
        {
            this.txtServiceDate.Text = this.txtPaidServiceDate.Text;
            if (!isValidForm())
            {
                return;
            }

            if (this.ddlGuestType.SelectedValue == "InHouseGuest")
            {
                if (!string.IsNullOrWhiteSpace(this.hfGuestCheckInDate.Value))
                {
                    DateTime guestCheckInDateTime = Convert.ToDateTime(this.hfGuestCheckInDate.Value);
                    DateTime guestServiceDateTime = hmUtility.GetDateTimeFromString(this.txtPaidServiceDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    DateTime currentDateTime = DateTime.Now.Date;
                    if (guestServiceDateTime < guestCheckInDateTime.AddDays(-1))
                    {
                        this.IsPaidServicePanelVisible = -1;
                        this.chkAllActiveReservation.Checked = false;
                        CommonHelper.AlertInfo(innboardMessage, "Service bill cannot be posted before registration date.", AlertType.Warning);
                        this.txtServiceDate.Focus();
                        return;
                    }

                    if (guestServiceDateTime > currentDateTime)
                    {
                        this.IsPaidServicePanelVisible = -1;
                        this.chkAllActiveReservation.Checked = false;
                        CommonHelper.AlertInfo(innboardMessage, "Service bill cannot be posted in future date.", AlertType.Warning);
                        this.txtServiceDate.Focus();
                        return;
                    }
                }
            }

            IsPaidServicePanelVisible = 1;
            this.LoadSearchInformation();
        }
        //************************ User Defined Function ********************//
        private void LoadIsServiceBillWithoutInHouseGuest()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsServiceBillWithoutInHouseGuest", "IsServiceBillWithoutInHouseGuest");
            this.pnlGuestTypeInfo.Visible = true;
            hfIsServiceBillWithoutInHouseGuest.Value = "0";
            this.ddlGuestType.SelectedValue = "InHouseGuest";
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        hfIsServiceBillWithoutInHouseGuest.Value = "1";
                        this.ddlGuestType.SelectedValue = "OutSideGuest";
                        this.pnlGuestTypeInfo.Visible = false;
                        this.ddlSGuestType.Items.Remove(ddlSGuestType.Items.FindByValue("InHouseGuest"));
                    }
                }
            }
        }
        private void LoadIsServiceRateEditablEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsServiceRateEditablEnable", "IsServiceRateEditablEnable");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        this.txtServiceRate.Enabled = true;
                        isServiceRateEditableEnable = true;
                    }
                    else
                    {
                        this.txtServiceRate.Enabled = false;
                        isServiceRateEditableEnable = false;
                    }
                }
            }
        }
        private void LoadCommonSetupForRackRateServiceChargeVatInformation()
        {
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

            this.ddlMBankingBankId.DataSource = entityBOList;
            this.ddlMBankingBankId.DataTextField = "BankName";
            this.ddlMBankingBankId.DataValueField = "BankId";
            this.ddlMBankingBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlMBankingBankId.Items.Insert(0, itemBank);
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadCommonDropDownHiddenField()
        {
            //this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
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
        private void CostCenterWiseSetting()
        {
            List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("ServiceBill");//ServiceBill

            if (costCentreTabBO.Count > 0)
            {
                hfInclusiveGuestServiceBill.Value = costCentreTabBO[0].IsVatSChargeInclusive.ToString();
                hfAdditionalCharge.Value = costCentreTabBO[0].AdditionalCharge.ToString();
                hfAdditionalChargeType.Value = costCentreTabBO[0].AdditionalChargeType.ToString();

                hfIsVatOnSDCharge.Value = costCentreTabBO[0].IsVatOnSDCharge ? "1" : "0";
                hfIsCitySDChargeEnableOnServiceCharge.Value = costCentreTabBO[0].IsCitySDChargeEnableOnServiceCharge ? "1" : "0";
                hfIsDiscountApplicableOnRackRate.Value = costCentreTabBO[0].IsDiscountApplicableOnRackRate ? "1" : "0";

                hfCityCharge.Value = costCentreTabBO[0].CitySDCharge.ToString();
                hfGuestServiceVat.Value = costCentreTabBO[0].VatAmount.ToString();
                hfGuestServiceServiceCharge.Value = costCentreTabBO[0].ServiceCharge.ToString();
                hfIsRatePlusPlus.Value = costCentreTabBO[0].IsRatePlusPlus.ToString();

                IsServiceChargeEnableConfig = costCentreTabBO[0].IsServiceChargeEnable ? 1 : 0;
                IsCitySDChargeEnableConfig = costCentreTabBO[0].IsCitySDChargeEnable ? 1 : 0;
                IsVatEnableConfig = costCentreTabBO[0].IsVatEnable ? 1 : 0;
                IsAdditionalChargeEnableConfig = costCentreTabBO[0].IsAdditionalChargeEnable ? 1 : 0;

                if (hfInclusiveGuestServiceBill.Value == "1")
                {
                    RackRateDiv.Visible = true;
                }
                else
                {
                    RackRateDiv.Visible = false;
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
        }
        private void LoadGuestHouseService()
        {
            HotelGuestServiceInfoDA guestHouseServiceDA = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> serviceBOList = new List<HotelGuestServiceInfoBO>();
            serviceBOList = guestHouseServiceDA.GetHotelGuestServiceInfo(1, 0, 0);

            this.ddlServiceId.DataSource = serviceBOList;
            this.ddlServiceId.DataTextField = "ServiceName";
            this.ddlServiceId.DataValueField = "ServiceId";
            this.ddlServiceId.DataBind();

            ListItem itemServiceId = new ListItem();
            itemServiceId.Value = "0";
            itemServiceId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlServiceId.Items.Insert(0, itemServiceId);

            this.ddlSOutServiceName.DataSource = serviceBOList;
            this.ddlSOutServiceName.DataValueField = "ServiceId";
            this.ddlSOutServiceName.DataTextField = "ServiceName";
            this.ddlSOutServiceName.DataBind();

            this.ddlInServiceName.DataSource = serviceBOList;
            this.ddlInServiceName.DataValueField = "ServiceId";
            this.ddlInServiceName.DataTextField = "ServiceName";
            this.ddlInServiceName.DataBind();

            ListItem itemServiceName = new ListItem();
            itemServiceName.Value = "0";
            itemServiceName.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSOutServiceName.Items.Insert(0, itemServiceName);
            this.ddlInServiceName.Items.Insert(0, itemServiceName);
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtServiceDate.Text = hmUtility.GetStringFromDateTime(dateTime);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO serviceBillingStartTimeBO = new HMCommonSetupBO();
            serviceBillingStartTimeBO = commonSetupDA.GetCommonConfigurationInfo("ServiceBillingStartTime", "ServiceBillingStartTime");

            if (serviceBillingStartTimeBO != null)
            {
                if (Convert.ToInt32(serviceBillingStartTimeBO.SetupValue) != 0)
                {
                    int serviceBillingStartTime = Convert.ToInt32(serviceBillingStartTimeBO.SetupValue);
                    int currentHour = dateTime.Hour;
                    int currentTime = dateTime.Minute;
                    if (serviceBillingStartTime > currentHour)
                    {
                        this.txtServiceDate.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(-1));
                    }
                    else if (serviceBillingStartTime == currentHour)
                    {
                        if (currentTime == 0)
                        {
                            this.txtServiceDate.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(-1));
                        }
                    }
                }
            }
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            CommonPaymentModeBO cardPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Card").FirstOrDefault();
            CommonPaymentModeBO chequePaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cheque").FirstOrDefault();
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            CommonPaymentModeBO mBankingPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "M-Banking").FirstOrDefault();
            CommonPaymentModeBO refundPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Refund").FirstOrDefault();

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlChequeReceiveAccountsInfo.DataBind();

            this.ddlRefundAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + refundPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlRefundAccountHead.DataTextField = "NodeHead";
            this.ddlRefundAccountHead.DataValueField = "NodeId";
            this.ddlRefundAccountHead.DataBind();

            this.ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + mBankingPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlMBankingReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlMBankingReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlMBankingReceiveAccountsInfo.DataBind();
        }        
        private void LoadGuestCompany()
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            ddlCompany.DataSource = companyDa.GetGuestCompanyInfo().Where(x => x.ActiveStat != false).ToList();
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem { Text = "---Please Select---", Value = "" });
        }
        private void LoadEmployee()
        {
            EmployeeDA empDa = new EmployeeDA();
            ddlEmployee.DataSource = empDa.GetEmployeeInfo();
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpIdNNodeId";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem { Text = "---Please Select---", Value = "" });
        }
        private void Cancel()
        {
            this.ddlGuestType.SelectedIndex = 0;
            this.ddlIsComplementary.SelectedIndex = 0;
            this.LoadCurrentDate();
            this.ddlServiceId.SelectedIndex = 0;
            this.txtServiceRate.Text = string.Empty;
            this.txtServiceQuantity.Text = "1";
            this.txtDiscountAmount.Text = string.Empty;
            this.txtGuestName.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.txtConversionRate.Text = string.Empty;
            this.hfServiceBillId.Value = string.Empty;
            this.btnSave.Text = "Save";
            this.ddlPayMode.SelectedValue = string.Empty;
            this.ddlEmployee.SelectedValue = string.Empty;
            this.ddlCompany.SelectedValue = string.Empty;
            this.ddlRegistrationId.Focus();
            this.ddlGuestType.SelectedIndex = 0;
            this.ddlServiceId.SelectedValue = "0";
            this.txtServiceRate.Text = "0";
            Session["CheckOutPayMode"] = null;
            Session["TransactionDetailList"] = null;
            this.gvGHServiceBill.DataSource = null;
            this.gvGHServiceBill.DataBind();
            hfddlRegistrationId.Value = string.Empty;
            hfServiceCharge.Value = string.Empty;
            hfVatAmount.Value = string.Empty;
            hfServiceRate.Value = string.Empty;
            hfRackRate.Value = string.Empty;
            hfGrandTotal.Value = string.Empty;

            txtSrcRoomNumber.Text = string.Empty;
            this.hfGuestCheckInDate.Value = string.Empty;
            this.hfBillingStartDate.Value = string.Empty;
            this.ddlRoomId.SelectedValue = "0";
            this.lblRegistrationNumberDiv.Visible = false;
            this.ddlRegistrationId.Visible = false;
            this.chkAllActiveReservation.Visible = false;
            this.lblActivePaidServiceList.Visible = false;
            txtRegisteredGuestName.Text = string.Empty;
            cbOnlyRateEffect.Checked = true;
        }
        public bool isValidForm()
        {
            bool status = true;
            this.IsDayClosed();
            this.btnSave.Visible = true;
            if (isDayClosed)
            {
                CommonHelper.AlertInfo(innboardMessage, "Day Closed for the date '" + this.txtServiceDate.Text + "', please try with another date.", AlertType.Warning);
                this.txtServiceDate.Focus();
                status = false;
            }
            else if (this.ddlGuestType.SelectedValue == "InHouseGuest")
            {
                if (this.ddlGuestType.SelectedIndex == 0)
                {
                    if (string.IsNullOrWhiteSpace(hfddlRegistrationId.Value))
                    {
                        if (string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please Provide a Valid Room Number.", AlertType.Warning);
                            this.txtSrcRoomNumber.Focus();
                            status = false;
                        }
                        else if (ddlRegistrationId.SelectedValue == "0")
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please Provide a Valid Room Number.", AlertType.Warning);
                            this.txtSrcRoomNumber.Focus();
                            status = false;
                        }
                    }
                }
            }

            decimal result;
            if (string.IsNullOrEmpty(this.txtServiceRate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Service Rate.", AlertType.Warning);
                SetTab("EntryTab");
                this.txtServiceRate.Focus();
                status = false;
            }
            else
            {
                Decimal.TryParse(this.txtServiceRate.Text, out result);
                if (result == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Service Rate.", AlertType.Warning);
                    SetTab("EntryTab");
                    this.txtServiceRate.Focus();
                    status = false;
                }
            }

            if (this.ddlServiceId.SelectedValue == "0")
            {
                if (!chkAllActiveReservation.Checked)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Service Name.", AlertType.Warning);
                    this.ddlServiceId.Focus();
                    status = false;
                }
            }
            else if (string.IsNullOrEmpty(this.txtServiceQuantity.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Service Quantity.", AlertType.Warning);
                this.txtServiceDate.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(this.txtServiceRate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Service Rate.", AlertType.Warning);
                this.txtServiceDate.Focus();
                status = false;
            }
            else if (this.ddlGuestType.SelectedIndex == 0)
            {
                if (this.ddlRegistrationId.SelectedIndex == -1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Room Number.", AlertType.Warning);
                    status = false;
                }
            }
            return status;
        }
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
                this.LoadRegistrationNumber(roomId);
            }
            else
            {
                this.LoadRegistrationNumber(0);
            }
        }
        private void LoadRegistrationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> roomRegistrationBO = new List<RoomRegistrationBO>();
            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoByRoomIdGHService(roomId);
            this.ddlRegistrationId.DataSource = roomRegistrationBO;
            this.ddlRegistrationId.DataTextField = "RegistrationNumber";
            this.ddlRegistrationId.DataValueField = "RegistrationId";
            this.ddlRegistrationId.DataBind();

            if (roomId == 0)
            {
                ListItem itemRegistration = new ListItem();
                itemRegistration.Value = "0";
                itemRegistration.Text = hmUtility.GetDropDownFirstValue();
                this.ddlRegistrationId.Items.Insert(0, itemRegistration);
            }
        }
        private void LoadSearchInformation()
        {
            Boolean isRoomStopChargePostingEnable = false;
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
                if (roomAllocationBO.RoomId > 0)
                {
                    List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                    CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                    costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("ServiceBill");
                    if (costCentreTabBO != null)
                    {
                        if (costCentreTabBO.Count > 0)
                        {
                            CostCenterWiseSetting();

                            isRoomStopChargePostingEnable = costCentreTabDA.GetRoomStopChargePostingByRegistrationAndCostCenterId(roomAllocationBO.RegistrationId, costCentreTabBO[0].CostCenterId);
                            if (isRoomStopChargePostingEnable)
                            {
                                hfRoomRegId.Value = "0";
                                ddlRegistrationId.SelectedValue = "0";
                                hfIsServiceBillWithoutInHouseGuest.Value = "1";
                                ddlGuestType.SelectedValue = "OutSideGuest";
                                txtGuestName.Text = roomAllocationBO.GuestName + ", Room# " + roomAllocationBO.RoomNumber;
                                CommonHelper.AlertInfo(innboardMessage, "Stop Charge Posting for Costcenter: " + costCentreTabBO[0].CostCenter, AlertType.Warning);
                            }
                            else
                            {
                                hfIsServiceBillWithoutInHouseGuest.Value = "0";
                                hfRoomRegId.Value = roomAllocationBO.RegistrationId.ToString();
                                this.hfGuestCheckInDate.Value = roomAllocationBO.ArriveDate.ToString();
                                this.hfBillingStartDate.Value = roomAllocationBO.BillingStartDate.ToString();
                                this.ddlRoomId.SelectedValue = roomAllocationBO.RoomId.ToString();
                                this.lblRegistrationNumberDiv.Visible = true;
                                this.ddlRegistrationId.Visible = true;
                                this.chkAllActiveReservation.Visible = true;
                                this.lblActivePaidServiceList.Visible = true;
                                txtRegisteredGuestName.Text = roomAllocationBO.GuestName + ", Room# " + roomAllocationBO.RoomNumber;
                                this.LoadRelatedInformation();
                            }
                        }
                    }
                    else
                    {
                        this.hfServiceBillId.Value = string.Empty;
                        this.lblRegistrationNumberDiv.Visible = false;
                        this.ddlRegistrationId.Visible = false;
                        this.chkAllActiveReservation.Visible = false;
                        this.lblActivePaidServiceList.Visible = false;
                        CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                    }
                }
                else
                {
                    this.hfServiceBillId.Value = string.Empty;
                    this.lblRegistrationNumberDiv.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    this.chkAllActiveReservation.Visible = false;
                    this.lblActivePaidServiceList.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                }
            }
            else
            {
                this.hfServiceBillId.Value = string.Empty;
                this.lblRegistrationNumberDiv.Visible = false;
                this.ddlRegistrationId.Visible = false;
                this.chkAllActiveReservation.Visible = false;
                this.lblActivePaidServiceList.Visible = false;
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
            }
            this.SetTab("EntryTab");
        }
        private void LoadPaidServiceDropDownInfo(int roomId)
        {
            int costCenterId;
            this.LoadCurrentDate();
            string transactionType = "GuestRoom";
            string transactionId;
            transactionId = roomId.ToString();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeCostCenterId", "FrontOfficeCostCenterId");
            costCenterId = Convert.ToInt32(commonSetupBO.SetupValue);

            GuestPaidServiceNRegInfoViewBO gpsr = new GuestPaidServiceNRegInfoViewBO();
            string PayByDetails = string.Empty;
            HMCommonDA commonDa = new HMCommonDA();
            PayByDetails = commonDa.GetPaidByDetailsInformationForRestaurantInvoice(transactionType, transactionId);

            gpsr.PayByDetails = PayByDetails;

            if (transactionType == "GuestRoom")
            {
                if (!string.IsNullOrWhiteSpace(transactionId))
                {
                    int registrationId = 0;
                    int IsGuestTodaysBillAddInfo = 0;
                    List<GHServiceBillBO> files = new List<GHServiceBillBO>();
                    List<GHServiceBillBO> previousDayServices = new List<GHServiceBillBO>();
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                    DateTime transactionDate = hmUtility.GetDateTimeFromString(this.txtServiceDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                    if (Convert.ToInt32(transactionId) > 0)
                    {
                        List<RoomRegistrationBO> roomRegistrationBO = new List<RoomRegistrationBO>();
                        roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(transactionId));
                        List<GHServiceBillBO> previousDaysAllServices = new List<GHServiceBillBO>();
                        previousDaysAllServices = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", 0, transactionDate, roomRegistrationBO[0].RoomNumber.ToString());
                        previousDayServices.AddRange(previousDaysAllServices.Where(x => x.CostCenterId == costCenterId && x.IsPaidService == true && x.IsPaidServiceAchieved == false).ToList());
                    }

                    List<GHServiceBillBO> costCenterItems = new List<GHServiceBillBO>();
                    List<GHServiceBillBO> totalGuestServices = new List<GHServiceBillBO>();
                    totalGuestServices.AddRange(previousDayServices);
                    totalGuestServices.AddRange(files);

                    HotelGuestServiceInfoDA serviceDa = new HotelGuestServiceInfoDA();
                    List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
                    roomRegistrationList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(transactionId));

                    this.ddlPaidServiceId.DataSource = totalGuestServices;
                    this.ddlPaidServiceId.DataTextField = "ServiceName";
                    this.ddlPaidServiceId.DataValueField = "ServiceBillId";
                    this.ddlPaidServiceId.DataBind();

                    ListItem itemServiceId = new ListItem();
                    itemServiceId.Value = "0";
                    itemServiceId.Text = hmUtility.GetDropDownFirstValue();
                    this.ddlPaidServiceId.Items.Insert(0, itemServiceId);
                }
            }
        }
        private void LoadGridView()
        {
            HMUtility utitlity = new HMUtility();
            string guestType = ddlSGuestType.SelectedValue.ToString();
            string roomNumber = txtSRoomNumber.Text;
            string innBillNumber = txtSBillNumber.Text;
            string outBillNumber = txtSOutBillNumber.Text;

            DateTime dateTime = DateTime.Now;

            string startDate = string.Empty;
            string endDate = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.txtSFromDate.Text))
            {
                startDate = this.txtSFromDate.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.txtSToDate.Text))
            {
                endDate = this.txtSToDate.Text;
            }
            DateTime? fromDate = null, toDate = null;
            if (!string.IsNullOrWhiteSpace(startDate))
                fromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            if (!string.IsNullOrWhiteSpace(endDate))
                toDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            GHServiceBillBO ghServiceBillBO = new GHServiceBillBO();
            GHServiceBillDA ghServiceDA = new GHServiceBillDA();
            List<GHServiceBillBO> list = new List<GHServiceBillBO>();
            ghServiceBillBO.GuestType = guestType;
            ghServiceBillBO.RoomNumber = roomNumber;
            int serviceId = 0;

            if (ghServiceBillBO.GuestType == "InHouseGuest")
            {
                ghServiceBillBO.BillNumber = innBillNumber;
                serviceId = Int32.Parse(ddlInServiceName.SelectedValue);
            }
            else if (ghServiceBillBO.GuestType == "OutSideGuest")
            {
                ghServiceBillBO.BillNumber = outBillNumber;
                serviceId = Int32.Parse(ddlSOutServiceName.SelectedValue);
            }
            else
            {
                ghServiceBillBO.BillNumber = outBillNumber;
                serviceId = Int32.Parse(ddlSOutServiceName.SelectedValue);
            }

            ghServiceBillBO.ServiceId = serviceId;
            list = ghServiceDA.GetGHServiceBillInfoBySearchCritaria(guestType, ghServiceBillBO.BillNumber, serviceId, roomNumber, fromDate, toDate);

            gvGHServiceBill.DataSource = list;
            gvGHServiceBill.DataBind();

            this.lblRegistrationNumberDiv.Visible = false;
            this.ddlRegistrationId.Visible = false;
            lblActivePaidServiceList.Visible = false;
            chkAllActiveReservation.Visible = false;
        }
        public static List<DateTime> GetDateArrayBetweenTwoDates(DateTime StartDate, DateTime EndDate)
        {
            var dates = new List<DateTime>();
            for (var dt = StartDate; dt <= EndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt.AddDays(1).AddSeconds(-1));
            }
            return dates;
        }
        private void IsDayClosed()
        {
            this.LoadCurrentDate();
            HMCommonDA hmCoomnoDA = new HMCommonDA();
            DayCloseBO dayCloseBO = new DayCloseBO();
            DateTime transactionDate = !string.IsNullOrWhiteSpace(this.txtServiceDate.Text) ? hmUtility.GetDateTimeFromString(this.txtServiceDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) : DateTime.Now;

            dayCloseBO = hmCoomnoDA.GetHotelDayCloseInformation(transactionDate);
            if (dayCloseBO != null)
            {
                if (dayCloseBO.DayCloseId > 0)
                {
                    isDayClosed = true;
                }
            }
        }
        public void ApprovedGuestService(string guestServiceType, int rowId, out int paidServiceId, out decimal paidServiceRate, out DateTime paidServiceDate)
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
                DateTime transactionDate = hmUtility.GetDateTimeFromString(this.txtServiceDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(guestServiceType, rowId, transactionDate, this.txtSrcRoomNumber.Text);

                if (files.Count > 0)
                {
                    serviceList.ApprovedId = files[0].ApprovedId;
                    serviceList.PaymentMode = files[0].RoomNumber.ToString();
                    serviceList.RegistrationId = Int32.Parse(files[0].RegistrationId.ToString());
                    serviceList.ServiceBillId = Int32.Parse(files[0].ServiceBillId.ToString());
                    serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString());
                    serviceList.ServiceType = files[0].ServiceType.ToString();
                    serviceList.ServiceId = Int32.Parse(files[0].ServiceId.ToString());
                    serviceList.ServiceName = files[0].ServiceName.ToString();
                    serviceList.ServiceQuantity = Convert.ToDecimal(files[0].ServiceQuantity.ToString());
                    serviceList.ServiceRate = Convert.ToDecimal(files[0].ServiceRate.ToString());
                    serviceList.DiscountAmount = Convert.ToDecimal(files[0].DiscountAmount.ToString());

                    paidServiceId = serviceList.ServiceId;
                    paidServiceRate = serviceList.ServiceRate;
                    paidServiceDate = serviceList.ServiceDate;

                    serviceList.VatAmount = Convert.ToDecimal(files[0].VatAmount.ToString());
                    serviceList.ServiceCharge = Convert.ToDecimal(files[0].ServiceCharge.ToString());

                    serviceList.ApprovedStatus = true;
                    serviceList.IsPaidService = Convert.ToBoolean(files[0].IsPaidService.ToString());
                    serviceList.ApprovedDate = serviceList.ServiceDate;
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
                            CommonHelper.AlertInfo(innboardMessage, "Achieve Operation Successfull.", AlertType.Success);
                        }
                    }
                    else
                    {
                        Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Achieve Operation Successfull.", AlertType.Success);
                        }
                    }
                }
            }

            paidServiceId = serviceList.ServiceId;
            paidServiceRate = serviceList.ServiceRate;
            paidServiceDate = serviceList.ServiceDate;
        }
        private void VoucherPost(List<GuestBillPaymentBO> ghServiceNewBillList, List<GuestBillPaymentBO> ghServiceEditBillList, List<GuestBillPaymentBO> ghServiceDeleteBillList, out int tmpGLMasterId)
        {
            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            foreach (GuestBillPaymentBO row in ghServiceNewBillList)
            {
                GLLedgerBO detailBODebit = new GLLedgerBO();
                detailBODebit.LedgerId = 0;
                if (row.PaymentMode == "Cash")
                {
                    detailBODebit.NodeId = Convert.ToInt32(row.AccountsPostingHeadId);
                    detailBODebit.NodeHead = ddlCashReceiveAccountsInfo.SelectedItem.Text;
                }
                else if (row.PaymentMode == "Card")
                {
                    detailBODebit.NodeId = Convert.ToInt32(row.AccountsPostingHeadId);
                    detailBODebit.NodeHead = ddlCardReceiveAccountsInfo.SelectedItem.Text;
                }
                else if (row.PaymentMode == "Employee")
                {
                    detailBODebit.NodeId = Convert.ToInt32(row.AccountsPostingHeadId);
                    detailBODebit.NodeHead = ddlEmployee.SelectedItem.Text;
                }
                else if (row.PaymentMode == "Company")
                {
                    detailBODebit.NodeId = Convert.ToInt32(row.AccountsPostingHeadId);
                    detailBODebit.NodeHead = ddlCompany.SelectedItem.Text;
                }
                detailBODebit.LedgerMode = 1;
                detailBODebit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue); //45 for Local Currency

                if (this.ddlCurrency.SelectedValue != LocalCurrencyId.ToString())
                {
                    decimal ledgerAmount = !string.IsNullOrWhiteSpace(row.PaymentAmount.ToString()) ? Convert.ToDecimal(row.PaymentAmount) : 0;
                    decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                    decimal mCurrencyAmount = ledgerAmount;

                    detailBODebit.CurrencyAmount = mCurrencyAmount / mConvertionRate;
                    detailBODebit.LedgerAmount = ledgerAmount;
                    detailBODebit.LedgerDebitAmount = ledgerAmount;
                }
                else
                {
                    detailBODebit.CurrencyAmount = !string.IsNullOrWhiteSpace(row.PaymentAmount.ToString()) ? row.PaymentAmount : 0;
                    detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(row.PaymentAmount.ToString()) ? Convert.ToDecimal(row.PaymentAmount) : 0;
                    detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(row.PaymentAmount.ToString()) ? Convert.ToDecimal(row.PaymentAmount) : 0;
                }

                detailBODebit.NodeNarration = "";
                ledgerDetailListBOForDebit.Add(detailBODebit);

            }
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;

            //Oppusite Head (Payment Head Information)
            int incomeNodeId = 0;
            string incomeNodeHead = string.Empty;

            GuestHouseServiceBO guestHouseServiceBO = new GuestHouseServiceBO();
            GuestHouseServiceDA guestHouseServiceDA = new GuestHouseServiceDA();
            guestHouseServiceBO = guestHouseServiceDA.GetGuestHouseServiceInfoDetailsById(Convert.ToInt32(this.ddlServiceId.SelectedValue));
            incomeNodeId = guestHouseServiceBO.IncomeNodeId;
            incomeNodeHead = guestHouseServiceBO.IncomeNodeHead;

            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = incomeNodeId;
            detailBOCredit.NodeHead = incomeNodeHead;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);
            decimal grandTotalAmount = (!string.IsNullOrWhiteSpace(txtServiceRate.Text) ? Convert.ToDecimal(txtServiceRate.Text) : 0) * (!string.IsNullOrWhiteSpace(txtServiceQuantity.Text) ? Convert.ToDecimal(txtServiceQuantity.Text) : 1);
            if (this.ddlCurrency.SelectedValue != LocalCurrencyId.ToString())
            {
                decimal ledgerAmount = grandTotalAmount;
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = ledgerAmount;

                detailBOCredit.CurrencyAmount = mCurrencyAmount / mConvertionRate;
                detailBOCredit.LedgerAmount = ledgerAmount;
                detailBOCredit.LedgerDebitAmount = ledgerAmount;
            }
            else
            {
                detailBOCredit.CurrencyAmount = grandTotalAmount;
                detailBOCredit.LedgerAmount = grandTotalAmount;
                detailBOCredit.LedgerDebitAmount = grandTotalAmount;
            }

            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            glMasterBO.CashChequeMode = 1;
            glMasterBO.VoucherType = "JV";
            glMasterBO.VoucherMode = 3;
            glMasterBO.ProjectId = 0; // Convert.ToInt32(this.ddlGLProject.SelectedValue);

            glMasterBO.VoucherDate = DateTime.Now;
            glMasterBO.PayerOrPayee = "";
            glMasterBO.Narration = "Amount Received for the Guest House Service Sales";

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
            this.txtLedgerAmount.Text = "0";
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string DeleteData(int serviceBillId)
        {
            string result = string.Empty;
            try
            {
                HMUtility hmUtility = new HMUtility();
                GHServiceBillDA BillDA = new GHServiceBillDA();
                BillDA.DeleteGHServiceBillInfo(serviceBillId, 1);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ServiceBill.ToString(), serviceBillId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServiceBill) + serviceBillId);
                result = "success";
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
        [WebMethod]
        public static GuestServiceBillPaymentDetailsViewBO FillForm(int EditId, string roomNumber)
        {
            GHServiceBillBO ghServiceBillBO = new GHServiceBillBO();
            GHServiceBillDA ghServiceBillDA = new GHServiceBillDA();
            ghServiceBillBO = ghServiceBillDA.GetGuestServiceBillInfoById(EditId);
            List<GuestBillPaymentBO> ghServiceBillDetails = new List<GuestBillPaymentBO>();
            ghServiceBillDetails = ghServiceBillDA.GetGHServiceBillInfoDetailsById(EditId);

            GuestServiceBillPaymentDetailsViewBO serviceViewModel = new GuestServiceBillPaymentDetailsViewBO();
            serviceViewModel.ServiceBill = ghServiceBillBO;
            serviceViewModel.ServiceBillDetails = ghServiceBillDetails;

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();

            if (!string.IsNullOrEmpty(roomNumber))
            {
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);

                if (roomAllocationBO != null)
                {
                    ghServiceBillBO.GuestName = roomAllocationBO.GuestName;
                    ghServiceBillBO.RegistrationNumber = roomAllocationBO.RegistrationNumber;
                    ghServiceBillBO.RoomNumber = roomAllocationBO.RoomNumber;
                }
            }

            return serviceViewModel;
        }
        [WebMethod]
        public static string GetIsServiceBillNumberDuplicate(int serviceBillId, DateTime serviceDate, string billNumber)
        {
            bool isBillNumberDuplicate = true;
            DateTime dateTime = DateTime.Now;
            serviceDate = dateTime;
            GHServiceBillDA billDa = new GHServiceBillDA();
            isBillNumberDuplicate = billDa.GetIsServiceBillNumberDuplicate(serviceBillId, serviceDate, billNumber);

            return isBillNumberDuplicate ? "1" : "0";
        }
        [WebMethod]
        public static int CheckDuplicateByServiceAndBillCritaria(string strRegistrationId, string strServiceBillId, string guestType, string billNumber, string serviceId, string serviceDate)
        {
            HMUtility hmUtility = new HMUtility();
            int isSucceed = 0;
            int ServiceId = Int32.Parse(serviceId);
            int registrationId = Int32.Parse(strRegistrationId);
            DateTime dateTime = DateTime.Now;
            DateTime ServiceDate = dateTime;
            GHServiceBillDA serviceBillDA = new GHServiceBillDA();
            int serviceBillId = !string.IsNullOrWhiteSpace(strServiceBillId) ? Convert.ToInt32(strServiceBillId) : 0;
            isSucceed = serviceBillDA.CheckDuplicateByServiceAndBillCritaria(registrationId, serviceBillId, guestType, billNumber, ServiceId, ServiceDate);
            return isSucceed;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        [WebMethod]
        public static HotelGuestServiceInfoBO GetPaidServiceDetails(int serviceId)
        {
            HotelGuestServiceInfoDA serviceDa = new HotelGuestServiceInfoDA();
            HotelGuestServiceInfoBO paidServiceBO = new HotelGuestServiceInfoBO();
            paidServiceBO = serviceDa.GetHotelGuestServiceInfoById(serviceId);
            return paidServiceBO;
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
        public static RoomRegistrationBO GetRoomRegistrationVatServieChargeInfo(int roomRegId)
        {
            RoomRegistrationDA roomRegDA = new RoomRegistrationDA();
            RoomRegistrationBO rooomRegBO = new RoomRegistrationBO();
            rooomRegBO = roomRegDA.GetRoomRegistrationInfoById(roomRegId);
            return rooomRegBO;
        }
    }
}