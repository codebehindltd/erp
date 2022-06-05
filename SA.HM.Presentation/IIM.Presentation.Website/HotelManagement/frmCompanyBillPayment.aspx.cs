using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmCompanyBillPayment : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected bool isSingle = true;
        protected int isMessageBoxEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int isNewAddButtonEnable = 1;
        protected int isSearchPanelEnable = -1;
        protected int isCompanyProjectPanelEnable = -1;
        protected int LocalCurrencyId;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            isIntegratedGeneralLedgerDiv = -1;
            isSingle = hmUtility.GetSingleProjectAndCompany();
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.btnGroupPaymentPreview.Visible = false;
                this.btnGroupPaymentPreview1.Visible = false;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                this.LoadPaymentMode();
                this.LoadBank();
                this.LoadCashAccountHead();
                this.LoadCurrency();
                LoadLocalCurrencyId();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                this.LoadCommonDropDownHiddenField();
                this.LoadGLCompany(false);
                this.LoadAccountHeadInfo();
                this.LoadGLProject(false);
                LoadCompany();
                this.ClearCommonSessionInformation();

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

                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                if (!isIntegrated)
                {
                    isIntegratedGeneralLedgerDiv = -1;
                }
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdPaymentPreview")
            {
                string url = "/HotelManagement/Reports/frmReportCompanyPayment.aspx?PaymentIdList=" + Convert.ToInt32(e.CommandArgument.ToString());
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
        }
        protected void btnSrcCmpPayment_Click(object sender, EventArgs e)
        {
            var cmpId = Convert.ToInt32(hfCmpSearch.Value);
            GuestCompanyDA companyDA = new GuestCompanyDA();
            GuestCompanyBO companyBO = new GuestCompanyBO();
            if (cmpId > 0)
            {
                companyBO = companyDA.GetGuestCompanyInfoForSalesCallById(cmpId);
                txtEmailAddress.Text = companyBO.EmailAddress;
                txtWebAddress.Text = companyBO.WebAddress;
                txtContactPerson.Text = companyBO.ContactPerson;
                txtContactNumber.Text = companyBO.ContactNumber;
                txtTelephoneNumber.Text = companyBO.TelephoneNumber;
                txtAddress.Text = companyBO.CompanyAddress;
                hfCmpSearch.Value = companyBO.CompanyId.ToString();
            }

            if (cmpId == 0 || cmpId == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Company.", AlertType.Warning);
                return;
            }

            if (cmpId > 0)
            {
                GuestBillPaymentDA da = new GuestBillPaymentDA();
                List<GuestBillPaymentBO> files = da.GetGuestBillPaymentInfoByRegistrationId("FrontOffice", cmpId);
                isSearchPanelEnable = 1;
                this.gvGuestHouseService.DataSource = files;
                this.gvGuestHouseService.DataBind();
            }
            else
            {
                isSearchPanelEnable = -1;
                this.gvGuestHouseService.DataSource = null;
                this.gvGuestHouseService.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.LoadLocalCurrencyId();
            string transactionHead = string.Empty;

            if (!IsFrmValid())
            {
                SetTab("EntryTab");
                return;
            }

            Boolean isNumberValue = hmUtility.IsNumber(this.txtLedgerAmount.Text);
            if (!isNumberValue)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Numeric Value.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                return;
            }

            if (isSingle == false)
            {
                if (this.ddlGLCompany.SelectedValue == "0")
                {
                    Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                    if (isIntegrated)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Select Company Name.", AlertType.Warning);
                        this.ddlGLCompany.Focus();
                        return;
                    }
                }
                else if (this.ddlGLCompany.SelectedValue != "0")
                {
                    if (this.ddlGLProject.SelectedValue == "0")
                    {
                        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                        if (isIntegrated)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please select Project Name.", AlertType.Warning);
                            this.ddlGLProject.Focus();
                            return;
                        }
                    }
                }
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();

            if (customField == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                return;
            }
            else
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GuestBillPaymentBO reservationBillPaymentBO = new GuestBillPaymentBO();
                GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();
                HotelCompanyPaymentLedgerBO companyPaymentLedgerBO = new HotelCompanyPaymentLedgerBO();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

                reservationBillPaymentBO.PaymentType = "Advance";
                reservationBillPaymentBO.RegistrationId = Convert.ToInt32(hfCmpSearch.Value);
                reservationBillPaymentBO.Remarks = this.txtRemarks.Text;

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

                reservationBillPaymentBO.PaymentMode = ddlPayMode.SelectedValue;
                reservationBillPaymentBO.PaymentModeId = Convert.ToInt32(hfCmpSearch.Value);

                reservationBillPaymentBO.ChecqueDate = DateTime.Now;
                reservationBillPaymentBO.BankId = Convert.ToInt32(ddlBankId.SelectedValue);
                reservationBillPaymentBO.ServiceBillId = null;

                if (ddlPayMode.SelectedValue == "Cash")
                {
                    companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    reservationBillPaymentBO.ChecqueNumber = "";
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                    reservationBillPaymentBO.PaymentDescription = string.Empty;
                }
                else if (ddlPayMode.SelectedValue == "Card")
                {
                    reservationBillPaymentBO.CardType = this.ddlCardType.SelectedValue.ToString();
                    companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
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
                    companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId.SelectedValue);
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId.SelectedValue);
                    reservationBillPaymentBO.BankId = Convert.ToInt32(ddlCompanyBank.SelectedValue);
                    reservationBillPaymentBO.ChecqueNumber = txtChecqueNumber.Text;

                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                    reservationBillPaymentBO.PaymentDescription = txtChecqueNumber.Text;
                }
                else if (ddlPayMode.SelectedValue == "Bank")
                {
                    companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlBankPayment.SelectedValue);
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlBankPayment.SelectedValue);
                    reservationBillPaymentBO.BankId = Convert.ToInt32(ddlBankPayment.SelectedValue);
                    reservationBillPaymentBO.ChecqueNumber = string.Empty;

                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                    reservationBillPaymentBO.PaymentDescription = ddlBankPayment.SelectedItem.Text;
                }
                else if (ddlPayMode.SelectedValue == "Company")
                {
                    reservationBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    reservationBillPaymentBO.PaymentDescription = string.Empty;
                }
                else if (ddlPayMode.SelectedValue == "Adjustment")
                {
                    companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    reservationBillPaymentBO.ChecqueNumber = "";
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                    reservationBillPaymentBO.PaymentDescription = string.Empty;
                    if (string.IsNullOrWhiteSpace(reservationBillPaymentBO.Remarks))
                    {
                        reservationBillPaymentBO.Remarks = "Bill Adjustment...";
                    }
                }
                reservationBillPaymentBO.ModuleName = "FrontOffice";

                //Company Payment Ledger
                companyPaymentLedgerBO.PaymentType = reservationBillPaymentBO.PaymentMode;
                companyPaymentLedgerBO.BillNumber = "";
                if (!string.IsNullOrEmpty(txtPaymentDate2.Text))
                {
                    companyPaymentLedgerBO.PaymentDate = CommonHelper.DateTimeToMMDDYYYY(txtPaymentDate2.Text);
                }
                else
                    companyPaymentLedgerBO.PaymentDate = DateTime.Now;

                companyPaymentLedgerBO.CompanyId = reservationBillPaymentBO.RegistrationId;
                companyPaymentLedgerBO.CurrencyId = reservationBillPaymentBO.FieldId;
                companyPaymentLedgerBO.ConvertionRate = reservationBillPaymentBO.ConvertionRate;
                if (ddlPayMode.SelectedValue == "Loan")
                {
                    companyPaymentLedgerBO.DRAmount = reservationBillPaymentBO.PaymentAmount;
                    companyPaymentLedgerBO.CRAmount = 0.00M; 
                }
                else
                {
                    companyPaymentLedgerBO.DRAmount = 0.00M;
                    companyPaymentLedgerBO.CRAmount = reservationBillPaymentBO.PaymentAmount;
                }
                companyPaymentLedgerBO.CurrencyAmount = reservationBillPaymentBO.CurrencyAmount;
                companyPaymentLedgerBO.Remarks = reservationBillPaymentBO.Remarks;
                companyPaymentLedgerBO.PaymentStatus = HMConstants.ApprovalStatus.Approved.ToString();

                Boolean status = false;
                if (string.IsNullOrWhiteSpace(hfCompanyPaymentId.Value))
                {
                    int tmpPaymentId = 0;
                    long tmpCompanyPaymentId = 0;
                    reservationBillPaymentBO.CreatedBy = userInformationBO.UserInfoId;
                    companyPaymentLedgerBO.CreatedBy = userInformationBO.UserInfoId;
                    status = guestCompanyDA.SaveHotelCompanyPaymentLedger(companyPaymentLedgerBO, null, out tmpCompanyPaymentId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), tmpPaymentId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    companyPaymentLedgerBO.CompanyPaymentId = Convert.ToInt32(hfCompanyPaymentId.Value);
                    companyPaymentLedgerBO.LastModifiedBy = userInformationBO.UserInfoId;
                    status = guestCompanyDA.UpdateHotelCompanyPaymentLedger(companyPaymentLedgerBO, null);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), reservationBillPaymentBO.DealId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Cancel();
                    }
                }
            }
            SetTab("EntryTab");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.btnGroupPaymentPreview.Visible = true;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<HotelCompanyPaymentLedgerBO> paymentList = new List<HotelCompanyPaymentLedgerBO>();

            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            paymentList = guestCompanyDA.GetHotelCompanyPaymentLedger(fromDate, toDate, string.Empty, false).Where(x => x.PaymentType != "Company").ToList();
            if (paymentList.Count > 0)
            {
                gvGuestHouseService.DataSource = paymentList;
                gvGuestHouseService.DataBind();
                gvPaymentInfo.DataSource = paymentList;
                gvPaymentInfo.DataBind();
            }
            else
            {
                gvGuestHouseService.DataSource = null;
                gvGuestHouseService.DataBind();
                gvPaymentInfo.DataSource = null;
                gvPaymentInfo.DataBind();
            }
            SetTab("SearchTab");
            isSearchPanelEnable = 1;
        }
        //************************ User Defined Function ********************//
        private void LoadBank()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeId(22).Where(x => x.IsTransactionalHead == true).ToList();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "HeadWithCode";
            this.ddlBankId.DataValueField = "NodeId";
            this.ddlBankId.DataBind();

            this.ddlCompanyBank.DataSource = entityBOList;
            this.ddlCompanyBank.DataTextField = "HeadWithCode";
            this.ddlCompanyBank.DataValueField = "NodeId";
            this.ddlCompanyBank.DataBind();

            this.ddlBankPayment.DataSource = entityBOList;
            this.ddlBankPayment.DataTextField = "HeadWithCode";
            this.ddlBankPayment.DataValueField = "NodeId";
            this.ddlBankPayment.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlCompanyBank.Items.Insert(0, itemBank);
            this.ddlBankPayment.Items.Insert(0, itemBank);
        }
        private void LoadCashAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeId(21).Where(x => x.IsTransactionalHead == true).ToList();

            this.ddlCashPayment.DataSource = entityBOList;
            this.ddlCashPayment.DataTextField = "HeadWithCode";
            this.ddlCashPayment.DataValueField = "NodeId";
            this.ddlCashPayment.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCashPayment.Items.Insert(0, itemBank);
        }
        private void LoadPaymentMode()
        {
            PaymentModeDA paymentModeDA = new PaymentModeDA();
            string forCompanyTransaction = "1, 8, 14";
            this.ddlPayMode.DataSource = paymentModeDA.GetPaymentModeInfoByCustomString("WHERE nm.PaymentModeId IN (" + forCompanyTransaction + ")");
            this.ddlPayMode.DataTextField = "DisplayName";
            this.ddlPayMode.DataValueField = "PaymentMode";
            this.ddlPayMode.DataBind();

            ListItem itemPayMode = new ListItem();
            itemPayMode.Value = "0";
            itemPayMode.Text = hmUtility.GetDropDownFirstValue();
            this.ddlPayMode.Items.Insert(0, itemPayMode);
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
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
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
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGuestBillPayment.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (ddlPayMode.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Payment Mode.", AlertType.Warning);
                this.ddlPayMode.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtPaymentDate2.Text))
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Date.", AlertType.Warning);
                this.txtPaymentDate2.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text))
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Amount.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                flag = false;
            }
            else if (this.ddlCurrency.SelectedValue == "0")
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Select Currency Type.", AlertType.Warning);
                flag = false;
            }

            if (this.ddlPayMode.SelectedValue == "Bank")
            {
                if (this.ddlBankPayment.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Bank Name.", AlertType.Warning);
                    this.ddlBankPayment.Focus();
                    flag = false;
                }
            }

            if (this.ddlPayMode.SelectedValue == "Cash")
            {
                if (this.ddlCashPayment.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Cash Name.", AlertType.Warning);
                    this.ddlCashPayment.Focus();
                    flag = false;
                }
            }

            if (this.ddlPayMode.SelectedValue == "Adjustment")
            {
                if (this.ddlCashPayment.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Cash Name.", AlertType.Warning);
                    this.ddlCashPayment.Focus();
                    flag = false;
                }
            }

            return flag;
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
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            companyBOList = companyDA.GetGuestCompanyInfo();

            this.ddlCompany.DataSource = companyBOList;
            this.ddlCompany.DataTextField = "CompanyName";
            this.ddlCompany.DataValueField = "CompanyId";
            this.ddlCompany.DataBind();
        }
        private void Cancel()
        {
            this.txtLedgerAmount.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.hfCompanyPaymentId.Value = string.Empty;
            this.txtDealId.Value = string.Empty;
            this.txtCalculatedLedgerAmount.Text = string.Empty;
            this.txtCalculatedLedgerAmountHiddenField.Value = string.Empty;
            this.txtConversionRate.Text = string.Empty;
            this.ddlPayMode.SelectedIndex = 0;
            this.ClearCommonSessionInformation();
            this.txtRemarks.Text = string.Empty;
            this.ddlBankId.SelectedValue = "0";
            hfConversionRate.Value = "";
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
            CommonPaymentModeBO refundPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Refund").FirstOrDefault();

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlPaymentFromAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlPaymentFromAccountsInfo.DataTextField = "NodeHead";
            this.ddlPaymentFromAccountsInfo.DataValueField = "NodeId";
            this.ddlPaymentFromAccountsInfo.DataBind();

            this.ddlPaymentToAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + refundPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlPaymentToAccountsInfo.DataTextField = "NodeHead";
            this.ddlPaymentToAccountsInfo.DataValueField = "NodeId";
            this.ddlPaymentToAccountsInfo.DataBind();

            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlChequeReceiveAccountsInfo.DataBind();

            CustomFieldBO IncomeSourceAccountsInfo = new CustomFieldBO();
            IncomeSourceAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("IncomeSourceAccountsInfo");
            this.ddlIncomeSourceAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + IncomeSourceAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            this.ddlIncomeSourceAccountsInfo.DataTextField = "NodeHead";
            this.ddlIncomeSourceAccountsInfo.DataValueField = "NodeId";
            this.ddlIncomeSourceAccountsInfo.DataBind();

            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + companyPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();
        }
        private void AdvanceVoucherPost(out int tmpGLMasterId)
        {
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
            else if (this.ddlPayMode.SelectedIndex == 2)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlChequeReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlChequeReceiveAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = 1;
            detailBODebit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue); //45 for Local Currency

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.CurrencyAmount = mCurrencyAmount;
                detailBODebit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBODebit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBODebit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            //Oppusite Head (Payment Head Information)
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = Convert.ToInt32(this.ddlIncomeSourceAccountsInfo.SelectedValue);
            detailBOCredit.NodeHead = this.ddlIncomeSourceAccountsInfo.SelectedItem.Text;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.CurrencyAmount = mCurrencyAmount;
                detailBOCredit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBOCredit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBOCredit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            Boolean isCash = true;
            if (this.ddlPayMode.SelectedItem.Text != "Cash")
            {
                isCash = false;
            }
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

            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);


            if (this.ddlPayMode.SelectedIndex == 0)
            {
                glMasterBO.VoucherMode = 2;
            }
            else
            {
                glMasterBO.VoucherMode = 3;
            }

            glMasterBO.VoucherDate = DateTime.Now;
            glMasterBO.PayerOrPayee = "";
            //glMasterBO.Narration = "Advance Received for the Registration Number : " + ddlRegistrationId.SelectedItem.Text;

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
        private void CashOutVoucherPost(out int tmpGLMasterId)
        {
            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            GLLedgerBO detailBODebit = new GLLedgerBO();
            detailBODebit.LedgerId = 0;
            if (this.ddlPayMode.SelectedIndex == 0)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            else if (this.ddlPayMode.SelectedIndex == 1)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            else if (this.ddlPayMode.SelectedIndex == 2)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = 1;
            detailBODebit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue); //45 for Local Currency

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.CurrencyAmount = mCurrencyAmount;
                detailBODebit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBODebit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBODebit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            //Oppusite Head (Payment Head Information)
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = Convert.ToInt32(this.ddlPaymentFromAccountsInfo.SelectedValue);
            detailBOCredit.NodeHead = this.ddlPaymentFromAccountsInfo.SelectedItem.Text;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.CurrencyAmount = mCurrencyAmount;
                detailBOCredit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBOCredit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBOCredit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            glMasterBO.CashChequeMode = 1;
            glMasterBO.VoucherType = "CP";
            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);
            glMasterBO.VoucherMode = 1;

            glMasterBO.VoucherDate = DateTime.Now;
            glMasterBO.PayerOrPayee = "";

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
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void ClearCommonSessionInformation()
        {
            Session["TransactionDetailList"] = null;
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
        //************************ User Defined Web Method ********************//
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
        public static HotelCompanyPaymentLedgerBO FillForm(int EditId)
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            HotelCompanyPaymentLedgerBO paymentBO = new HotelCompanyPaymentLedgerBO();
            paymentBO = guestCompanyDA.GetSupplierPaymentLedgerById(EditId);

            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(paymentBO.CurrencyId);
            paymentBO.CurrencyType = commonCurrencyBO.CurrencyType;

            return paymentBO;
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
        public static GuestCompanyBO LoadCompanyInfo(int companyId)
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            return companyDA.GetGuestCompanyInfoForSalesCallById(companyId);
        }
    }
}