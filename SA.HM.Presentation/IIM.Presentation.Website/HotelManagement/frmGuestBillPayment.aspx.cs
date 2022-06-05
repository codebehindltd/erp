using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data;
using System.Globalization;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGuestBillPayment : BasePage
    {
        HiddenField innboardMessage;
        protected bool isSingle = true;
        protected int isMessageBoxEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int isNewAddButtonEnable = 1;
        protected int isSearchPanelEnable = -1;
        protected int isCompanyProjectPanelEnable = -1;
        protected int LocalCurrencyId;

        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            isIntegratedGeneralLedgerDiv = -1;
            isSingle = hmUtility.GetSingleProjectAndCompany();
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, "Delete Operation Successfull.", AlertType.Success);
            }
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckPermission();
                this.lblDisplayConvertionRate.Text = string.Empty;
                this.btnPaymentPreview.Visible = false;
                this.btnGroupPaymentPreview.Visible = false;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                this.LoadBank();
                this.LoadCurrency();
                LoadLocalCurrencyId();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                this.LoadRoomNumber();
                this.LoadChangedRegistrationId();
                this.LoadCommonDropDownHiddenField();
                //this.LoadGLCompany(false);
                this.LoadAccountHeadInfo();
                //this.LoadGLProject(false);
                this.ClearCommonSessionInformation();

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

                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                if (!isIntegrated)
                {
                    isIntegratedGeneralLedgerDiv = -1;
                }

                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcRoomNumber.Text = numberBO.RoomNumber;
                    this.SearchInformation();
                }

                this.LoadCompanyChequeHeadInfo();
            }

        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
            this.LoadGridView(this.ddlRegistrationId.SelectedValue);
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;

                Label lblIsBillEditable = (Label)e.Row.FindControl("lblIsBillEditable");

                if (lblIsBillEditable.Text == "False")
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdPaymentPreview")
            {
                string url = "/HotelManagement/Reports/frmReportGuestPaymentInvoice.aspx?PaymentIdList=" + Convert.ToInt32(e.CommandArgument.ToString());
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                this.SearchInformation();
            }
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            this.ClearCommonSessionInformation();
            this.SearchInformation();
        }
        protected void btnPaymentPreview_Click(object sender, EventArgs e)
        {
            string paymentIdList = string.Empty;
            if (!string.IsNullOrWhiteSpace(txtSrcRoomNumber.Text))
            {
                foreach (GridViewRow row in gvPaymentInfo.Rows)
                {
                    bool isSelect = ((CheckBox)row.FindControl("ChkSelect")).Checked;

                    if (isSelect)
                    {
                        Label lblObjectTabIdValue = (Label)row.FindControl("lblid");
                        if (string.IsNullOrWhiteSpace(paymentIdList.Trim()))
                        {
                            paymentIdList = paymentIdList + Convert.ToInt32(lblObjectTabIdValue.Text);
                        }
                        else
                        {
                            paymentIdList = paymentIdList + "," + Convert.ToInt32(lblObjectTabIdValue.Text);
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(paymentIdList))
            {
                string url = "/HotelManagement/Reports/frmReportGuestPaymentInvoice.aspx?PaymentIdList=" + paymentIdList;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                this.SearchInformation();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.LoadLocalCurrencyId();
            string transactionHead = string.Empty;

            if (!IsFrmValid())
            {
                return;
            }

            Boolean isNumberValue = hmUtility.IsNumber(this.txtLedgerAmount.Text);
            if (!isNumberValue)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Numeric Value in Payment Amount.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                return;
            }

            //if (isSingle == false)
            //{
            //    if (this.ddlGLCompany.SelectedValue == "0")
            //    {
            //        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
            //        if (isIntegrated)
            //        {
            //            CommonHelper.AlertInfo(innboardMessage, "Please Select Company Name.", AlertType.Warning);
            //            this.ddlGLCompany.Focus();
            //            return;
            //        }
            //    }
            //    else if (this.ddlGLCompany.SelectedValue != "0")
            //    {
            //        if (this.ddlGLProject.SelectedValue == "0")
            //        {
            //            Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
            //            if (isIntegrated)
            //            {
            //                CommonHelper.AlertInfo(innboardMessage, "Please select Project Name.", AlertType.Warning);
            //                this.ddlGLProject.Focus();
            //                return;
            //            }
            //        }
            //    }
            //}

            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();

            if (customField == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                return;
            }
            else
            {
                RoomNumberDA numberDA = new RoomNumberDA();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomRegistrationBO registrationBO = new RoomRegistrationBO();
                if (ddlRegistrationId.SelectedValue != "")
                {
                    registrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Int32.Parse(this.ddlRegistrationId.SelectedValue));
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                    return;
                }
                if (registrationBO.IsGuestCheckedOut == 1)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                GuestBillPaymentBO reservationBillPaymentBO = new GuestBillPaymentBO();
                GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();

                reservationBillPaymentBO.PaymentType = this.ddlPaymentType.SelectedValue;
                reservationBillPaymentBO.RegistrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                reservationBillPaymentBO.Remarks = this.txtRemarks.Text;

                if (string.IsNullOrWhiteSpace(hfCurrencyType.Value))
                {
                    hfCurrencyType.Value = "Local";
                }

                if (hfCurrencyType.Value == "Local")
                {
                    reservationBillPaymentBO.FieldId = LocalCurrencyId;
                    reservationBillPaymentBO.ConvertionRate = 1;
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                    reservationBillPaymentBO.CurrencyAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
                    reservationBillPaymentBO.PaymentAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
                }
                else
                {
                    reservationBillPaymentBO.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);
                    if (txtConversionRate.ReadOnly != true)
                    {
                        reservationBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                    }
                    else
                    {
                        if (hfConversionRate.Value != "")
                            reservationBillPaymentBO.ConvertionRate = Convert.ToDecimal(hfConversionRate.Value);
                    }
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                    reservationBillPaymentBO.CurrencyAmount = tmpCurrencyAmount;
                    reservationBillPaymentBO.PaymentAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
                }

                if (this.ddlPaymentType.SelectedValue == "PaidOut")
                {
                    reservationBillPaymentBO.PaymentMode = "Cash";
                    this.ddlPayMode.SelectedValue = "Cash";
                }
                else
                {
                    reservationBillPaymentBO.PaymentMode = ddlPayMode.SelectedValue;
                }

                reservationBillPaymentBO.PaymentModeId = 0;
                reservationBillPaymentBO.ChecqueDate = DateTime.Now;
                reservationBillPaymentBO.BankId = Convert.ToInt32(ddlBankId.SelectedValue);
                reservationBillPaymentBO.ServiceBillId = null;

                if (ddlPayMode.SelectedValue == "Cash")
                {
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashReceiveAccountsInfo.SelectedValue);
                    reservationBillPaymentBO.ChecqueNumber = "";
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                    reservationBillPaymentBO.PaymentDescription = string.Empty;
                }
                else if (ddlPayMode.SelectedValue == "Card")
                {
                    reservationBillPaymentBO.CardType = this.ddlCardType.SelectedValue.ToString();
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
                    reservationBillPaymentBO.CardNumber = this.txtCardNumber.Text;
                    if (string.IsNullOrEmpty(this.txtExpireDate.Text))
                    {
                        reservationBillPaymentBO.ExpireDate = null;
                    }
                    else
                    {
                        reservationBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(this.txtExpireDate.Text, userInformationBO.ServerDateFormat);
                    }
                    reservationBillPaymentBO.CardHolderName = this.txtCardHolderName.Text;
                    reservationBillPaymentBO.ChecqueNumber = "";
                    reservationBillPaymentBO.PaymentDescription = ddlCardType.SelectedItem.Text;
                }
                else if (ddlPayMode.SelectedValue == "Cheque")
                {
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId.SelectedValue);
                    reservationBillPaymentBO.BankId = Convert.ToInt32(ddlCompanyBank.SelectedValue);
                    reservationBillPaymentBO.ChecqueNumber = txtChecqueNumber.Text;
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                    reservationBillPaymentBO.PaymentDescription = txtChecqueNumber.Text;
                }
                else if (ddlPayMode.SelectedValue == "Company")
                {
                    reservationBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    reservationBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId.SelectedValue);
                    reservationBillPaymentBO.PaymentDescription = string.Empty;
                }
                else if (ddlPayMode.SelectedValue == "M-Banking")
                {
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo.SelectedValue);
                    reservationBillPaymentBO.BankId = Convert.ToInt32(ddlMBankingBankId.SelectedValue);
                    reservationBillPaymentBO.ChecqueNumber = "";
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                    reservationBillPaymentBO.PaymentDescription = string.Empty;
                }
                reservationBillPaymentBO.ModuleName = "FrontOffice";

                if (string.IsNullOrWhiteSpace(txtPaymentId.Value))
                {
                    int tmpPaymentId = 0;
                    reservationBillPaymentBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = reservationBillPaymentDA.SaveGuestBillPaymentInfo(reservationBillPaymentBO, out tmpPaymentId, "Others");
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), tmpPaymentId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        if (this.ddlRegistrationId.SelectedIndex != -1)
                        {
                            this.LoadGridView(this.ddlRegistrationId.SelectedValue);
                        }
                        this.Cancel();
                    }
                }
                else
                {
                    reservationBillPaymentBO.PaymentId = Convert.ToInt32(txtPaymentId.Value);
                    reservationBillPaymentBO.LastModifiedBy = userInformationBO.UserInfoId;
                    reservationBillPaymentBO.DealId = Convert.ToInt32(txtDealId.Value);

                    reservationBillPaymentBO.RegistrationId = Convert.ToInt32(this.ddlChangedRegistrationId.SelectedValue);

                    Boolean status = reservationBillPaymentDA.UpdateGuestBillPaymentInfo(reservationBillPaymentBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), reservationBillPaymentBO.DealId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        if (this.ddlRegistrationId.SelectedIndex != -1)
                        {
                            this.LoadGridView(this.ddlRegistrationId.SelectedValue);
                        }
                        this.Cancel();
                    }
                }
            }
        }
        //************************ User Defined Function ********************//
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
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();
            LocalCurrencyId = commonCurrencyBO.CurrencyId;
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
        private void LoadChangedRegistrationId()
        {
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();

            this.ddlChangedRegistrationId.DataSource = registrationDA.GetActiveRegistrationInfo();
            this.ddlChangedRegistrationId.DataTextField = "RoomNumber";
            this.ddlChangedRegistrationId.DataValueField = "RegistrationId";
            this.ddlChangedRegistrationId.DataBind();
        }
        private void SearchByRoomNumber()
        {
            this.LoadCurrency();
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
                if (roomAllocationBO.RoomId > 0)
                {
                    this.ddlRoomId.SelectedValue = roomAllocationBO.RoomId.ToString();
                    this.lblRegistrationNumberDiv.Visible = true;
                    this.ddlRegistrationId.Visible = true;
                    this.LoadRelatedInformation();
                    this.txtConversionRate.Text = roomAllocationBO.ConversionRate.ToString();
                    this.txtConversionRateHiddenField.Value = roomAllocationBO.ConversionRate.ToString();

                    if (this.ddlPayMode.SelectedIndex != -1)
                    {
                        this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Company"));
                        //this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Cheque"));
                    }

                    if (roomAllocationBO.CompanyId > 0)
                    {
                        ListItem itemRoom = new ListItem();
                        itemRoom.Value = "Company";
                        itemRoom.Text = "Company";
                        this.ddlPayMode.Items.Insert(4, itemRoom);

                        //ListItem chequeItem = new ListItem("Cheque", "Cheque", true);
                        //this.ddlPayMode.Items.Add(chequeItem);

                        this.ddlCompanyPaymentAccountHead.Enabled = false;
                        this.HiddenFieldCompanyPaymentButtonInfo.Value = "1";
                    }
                }
                else
                {
                    this.txtPaymentId.Value = string.Empty;
                    this.lblRegistrationNumberDiv.Visible = false;
                    this.ddlRegistrationId.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                    this.btnPaymentPreview.Visible = false;
                    this.btnGroupPaymentPreview.Visible = false;
                }
            }
            else
            {
                this.txtPaymentId.Value = string.Empty;
                this.lblRegistrationNumberDiv.Visible = false;
                this.ddlRegistrationId.Visible = false;
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                this.btnPaymentPreview.Visible = false;
                this.btnGroupPaymentPreview.Visible = false;
            }
        }
        private void LoadFormByRegistrationNumber()
        {
            if (this.ddlRegistrationId.SelectedIndex != -1)
            {
                List<GuestBillPaymentBO> guestBillPaymentBO = new List<GuestBillPaymentBO>();
                GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

                guestBillPaymentBO = guestBillPaymentDA.GetGuestBillPaymentInfoByRegistrationId("FrontOffice", Convert.ToInt32(this.ddlRegistrationId.SelectedValue));
                this.gvGuestHouseService.DataSource = guestBillPaymentBO;
                this.gvGuestHouseService.DataBind();
            }
            else
            {
                this.gvGuestHouseService.DataSource = null;
                this.gvGuestHouseService.DataBind();
            }
        }
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
                this.LoadRegistrationNumber(roomId);
                this.LoadFormByRegistrationNumber();
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
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (this.ddlPaymentType.SelectedValue == "0")
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Select Payment Type.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                flag = false;
            }
            if (this.ddlPaymentType.SelectedValue != "PaidOut")
            {
                if (this.ddlPayMode.SelectedValue == "0")
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Select Payment Mode.", AlertType.Warning);
                    this.txtLedgerAmount.Focus();
                    flag = false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text))
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Amount.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                flag = false;
            }
            if (this.ddlRegistrationId.SelectedIndex == -1)
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                flag = false;
            }
            if (this.ddlPaymentType.SelectedValue != "PaidOut")
            {
                if (this.ddlCurrency.SelectedValue == "0")
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Select Currency Type.", AlertType.Warning);
                    flag = false;
                }
            }
            if (this.ddlCurrency.SelectedValue != LocalCurrencyId.ToString() && this.ddlCurrency.SelectedValue != "0")
            {
                if (string.IsNullOrWhiteSpace(this.txtConversionRateHiddenField.Value))
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Conversion Rate.", AlertType.Warning);
                    this.txtConversionRate.Focus();
                    flag = false;
                }
            }
            if (this.ddlPaymentType.SelectedValue == "0")
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Select Payment Type.", AlertType.Warning);
                this.ddlPaymentType.Focus();
                flag = false;
            }
            if (this.ddlPayMode.SelectedValue == "0")
            {
                if (this.ddlPaymentType.SelectedValue != "PaidOut")
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Select Payment Mode.", AlertType.Warning);
                    this.ddlPayMode.Focus();
                    flag = false;
                }
            }
            if (this.ddlPayMode.SelectedValue == "Card")
            {
                if (this.ddlCardType.SelectedValue == "0")
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Card Type.", AlertType.Warning);
                    this.ddlCardType.Focus();
                    flag = false;
                }
                else if (this.ddlBankId.SelectedValue == "0")
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Bank Name.", AlertType.Warning);
                    this.ddlBankId.Focus();
                    flag = false;
                }
            }
            else if (this.ddlPayMode.SelectedValue == "Cheque")
            {
                if (string.IsNullOrWhiteSpace(txtChecqueNumber.Text))
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Cheque Number.", AlertType.Warning);
                    this.txtChecqueNumber.Focus();
                    flag = false;
                }
                else if (this.ddlCompanyBank.SelectedValue == "0")
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Bank Name.", AlertType.Warning);
                    this.ddlCompanyBank.Focus();
                    flag = false;
                }
            }
            else if (this.ddlPayMode.SelectedValue == "M-Banking")
            {
                if (this.ddlMBankingBankId.SelectedValue == "0")
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Bank Name.", AlertType.Warning);
                    this.ddlMBankingBankId.Focus();
                    flag = false;
                }
            }
            return flag;
        }
        private void LoadGridView(string reservationId)
        {
            if (reservationId == "-1")
            {
                isSearchPanelEnable = -1;
                return;
            }
            if (Convert.ToInt32(reservationId) > 0)
            {
                GuestBillPaymentDA da = new GuestBillPaymentDA();
                List<GuestBillPaymentBO> files = da.GetGuestBillPaymentInfoByRegistrationId("FrontOffice", Convert.ToInt32(reservationId));
                isSearchPanelEnable = 1;
                this.gvGuestHouseService.DataSource = files;
                this.gvGuestHouseService.DataBind();
                this.gvPaymentInfo.DataSource = files;
                this.gvPaymentInfo.DataBind();

                if (files.Count() > 0)
                {
                    this.btnPaymentPreview.Visible = true;
                    this.btnGroupPaymentPreview.Visible = true;
                }
                else
                {
                    this.btnPaymentPreview.Visible = false;
                    this.btnGroupPaymentPreview.Visible = false;
                }
            }
            else
            {
                isSearchPanelEnable = -1;
                this.gvGuestHouseService.DataSource = null;
                this.gvGuestHouseService.DataBind();
                this.gvPaymentInfo.DataSource = null;
                this.gvPaymentInfo.DataBind();
                this.btnPaymentPreview.Visible = false;
                this.btnGroupPaymentPreview.Visible = false;
            }
        }
        //private void LoadSingleProjectAndCompany()
        //{
        //    this.LoadGLCompany(true);
        //    this.LoadGLProject(true);
        //}
        //private void LoadGLCompany(bool isSingle)
        //{
        //    GLCompanyDA entityDA = new GLCompanyDA();
        //    var List = entityDA.GetAllGLCompanyInfo();
        //    List<GLCompanyBO> companyList = new List<GLCompanyBO>();
        //    if (isSingle == true)
        //    {
        //        companyList.Add(List[0]);
        //        this.ddlGLCompany.DataSource = companyList;
        //        this.ddlGLCompany.DataTextField = "Name";
        //        this.ddlGLCompany.DataValueField = "CompanyId";
        //        this.ddlGLCompany.DataBind();
        //    }
        //    else
        //    {
        //        this.ddlGLCompany.DataSource = List;
        //        this.ddlGLCompany.DataTextField = "Name";
        //        this.ddlGLCompany.DataValueField = "CompanyId";
        //        this.ddlGLCompany.DataBind();
        //        ListItem itemCompany = new ListItem();
        //        itemCompany.Value = "0";
        //        itemCompany.Text = hmUtility.GetDropDownFirstValue();
        //        this.ddlGLCompany.Items.Insert(0, itemCompany);
        //    }
        //}
        //private void LoadGLProject(bool isSingle)
        //{
        //    GLProjectDA entityDA = new GLProjectDA();
        //    List<GLProjectBO> projectList = new List<GLProjectBO>();
        //    var List = entityDA.GetAllGLProjectInfo();
        //    if (isSingle == true)
        //    {
        //        projectList.Add(List[0]);
        //        this.ddlGLProject.DataSource = projectList;
        //        this.ddlGLProject.DataTextField = "Name";
        //        this.ddlGLProject.DataValueField = "ProjectId";
        //        this.ddlGLProject.DataBind();
        //    }
        //    else
        //    {
        //        this.ddlGLProject.DataSource = List;
        //        this.ddlGLProject.DataTextField = "Name";
        //        this.ddlGLProject.DataValueField = "ProjectId";
        //        this.ddlGLProject.DataBind();

        //        ListItem itemProject = new ListItem();
        //        itemProject.Value = "0";
        //        itemProject.Text = hmUtility.GetDropDownFirstValue();
        //        this.ddlGLProject.Items.Insert(0, itemProject);
        //    }
        //}
        private void Cancel()
        {
            this.txtLedgerAmount.Text = string.Empty;
            this.ddlRegistrationId.SelectedIndex = -1;
            this.btnSave.Text = "Save";
            this.txtPaymentId.Value = string.Empty;
            this.txtDealId.Value = string.Empty;
            this.ddlPaymentType.SelectedIndex = 0;
            this.txtCalculatedLedgerAmount.Text = string.Empty;
            this.txtCalculatedLedgerAmountHiddenField.Value = string.Empty;
            this.txtConversionRate.Text = string.Empty;
            this.ddlPayMode.SelectedIndex = 0;
            this.ClearCommonSessionInformation();
            this.txtRemarks.Text = string.Empty;
            this.ddlBankId.SelectedValue = "0";
            this.ddlCurrency.SelectedValue = "0";
            this.txtSrcRoomNumber.Focus();
            hfConversionRate.Value = "";
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

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlPaymentFromAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlPaymentFromAccountsInfo.DataTextField = "NodeHead";
            this.ddlPaymentFromAccountsInfo.DataValueField = "NodeId";
            this.ddlPaymentFromAccountsInfo.DataBind();

            this.ddlPaymentToAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlPaymentToAccountsInfo.DataTextField = "NodeHead";
            this.ddlPaymentToAccountsInfo.DataValueField = "NodeId";
            this.ddlPaymentToAccountsInfo.DataBind();

            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlChequeReceiveAccountsInfo.DataBind();

            CustomFieldBO IncomeSourceAccountsInfo = new CustomFieldBO();
            IncomeSourceAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("IncomeSourceAccountsInfo");
            this.ddlIncomeSourceAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + IncomeSourceAccountsInfo.FieldValue.ToString() + ")");
            this.ddlIncomeSourceAccountsInfo.DataTextField = "NodeHead";
            this.ddlIncomeSourceAccountsInfo.DataValueField = "NodeId";
            this.ddlIncomeSourceAccountsInfo.DataBind();

            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + companyPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();

            this.ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + mBankingPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlMBankingReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlMBankingReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlMBankingReceiveAccountsInfo.DataBind();
        }
        private void SearchInformation()
        {
            this.SearchByRoomNumber();
            if (this.ddlRegistrationId.SelectedIndex != -1)
            {
                this.LoadGridView(this.ddlRegistrationId.SelectedValue);
                this.ddlChangedRegistrationId.SelectedValue = this.ddlRegistrationId.SelectedValue;
            }
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void ClearCommonSessionInformation()
        {
            Session["TransactionDetailList"] = null;
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
        //************************ User Defined Function ********************//       
        [WebMethod]
        public static string DeleteData(int sEmpId)
        {
            string result = string.Empty;
            HMUtility hmUtility = new HMUtility();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("HotelGuestBillPayment", "PaymentId", sEmpId);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),EntityTypeEnum.EntityType.GuestBillPayment.ToString(),sEmpId,ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
        [WebMethod]
        public static GuestBillPaymentBO FillForm(int EditId)
        {
            GuestBillPaymentBO reservationBillPaymentBO = new GuestBillPaymentBO();
            GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();

            reservationBillPaymentBO = reservationBillPaymentDA.GetGuestBillPaymentInfoById(EditId);

            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(reservationBillPaymentBO.FieldId);
            reservationBillPaymentBO.CurrencyType = commonCurrencyBO.CurrencyType;

            return reservationBillPaymentBO;
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