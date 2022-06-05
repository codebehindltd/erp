using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmSupplierPayment : BasePage
    {
        HiddenField innboardMessage;
        protected int LocalCurrencyId;
        HMUtility hmUtility = new HMUtility();
        protected int isSearchPanelEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                btnGroupPaymentPreview.Visible = false;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                LoadSupplierInfo();
                LoadPaymentMode();
                LoadBank();
                LoadCashAccountHead();
                LoadCashAndCashEquivalantHead();
                LoadAdjustmentEquivalantHead();
                LoadCurrency();
                LoadLocalCurrencyId();
                LoadIsConversionRateEditable();
                IsReceiveBillInfoHideInSupplierBillPayment();
                IsLocalCurrencyDefaultSelected();
                LoadCommonDropDownHiddenField();
                LoadAccountHeadInfo();
                CheckPermission();
            }
        }
        //**************************** Handlers ****************************//
        protected void btnSrcSupplier_Click(object sender, EventArgs e)
        {
            PMSupplierBO supplierBO = new PMSupplierBO();
            PMSupplierDA supplierDA = new PMSupplierDA();
            if (!string.IsNullOrEmpty(txtSrcSupplierId.Text))
            {
                supplierBO = supplierDA.GetSupplierByCode(txtSrcSupplierId.Text);
            }

            if (supplierBO == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Supplier Id.", AlertType.Warning);
                this.txtSrcSupplierId.Focus();
                return;
            }
            else
            {
                hfSupplierId.Value = supplierBO.SupplierId.ToString();
            }
            SetTab("EntryTab");
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
                string url = "/PurchaseManagment/Reports/frmReportSupplierPayment.aspx?PaymentIdList=" + Convert.ToInt32(e.CommandArgument.ToString());
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.LoadLocalCurrencyId();

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

                GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
                GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();
                PMSupplierPaymentLedgerBO supplierPaymentLedgerBO = new PMSupplierPaymentLedgerBO();
                PMSupplierDA supplierDA = new PMSupplierDA();

                guestBillPaymentBO.PaymentType = "Advance";
                guestBillPaymentBO.Remarks = this.txtRemarks.Text;

                if (hfCurrencyType.Value == "Local")
                {
                    guestBillPaymentBO.FieldId = LocalCurrencyId;
                    guestBillPaymentBO.ConvertionRate = 1;
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                    guestBillPaymentBO.CurrencyAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
                    guestBillPaymentBO.PaymentAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
                }
                else
                {
                    guestBillPaymentBO.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);
                    if (txtConversionRate.ReadOnly != true)
                    {
                        guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                    }
                    else
                    {
                        if (hfConversionRate.Value != "")
                            guestBillPaymentBO.ConvertionRate = Convert.ToDecimal(hfConversionRate.Value);
                    }
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                    guestBillPaymentBO.CurrencyAmount = tmpCurrencyAmount;
                    guestBillPaymentBO.PaymentAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
                }

                guestBillPaymentBO.PaymentMode = ddlPayMode.SelectedValue;
                guestBillPaymentBO.ChecqueDate = DateTime.Now;
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId.SelectedValue);
                guestBillPaymentBO.ServiceBillId = null;

                if (ddlPayMode.SelectedValue == "Cash")
                {
                    supplierPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    guestBillPaymentBO.ChecqueNumber = "";
                    guestBillPaymentBO.CardReference = "";
                    guestBillPaymentBO.CardNumber = "";
                    guestBillPaymentBO.BranchName = "";
                    guestBillPaymentBO.PaymentDescription = string.Empty;
                }
                else if (ddlPayMode.SelectedValue == "Card")
                {
                    guestBillPaymentBO.CardType = this.ddlCardType.SelectedValue.ToString();
                    supplierPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
                    guestBillPaymentBO.CardNumber = this.txtCardNumber.Text;
                    if (string.IsNullOrEmpty(this.txtExpireDate.Text))
                    {
                        guestBillPaymentBO.ExpireDate = null;
                    }
                    else
                    {
                        guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(this.txtExpireDate.Text, userInformationBO.ServerDateFormat);
                    }
                    guestBillPaymentBO.CardHolderName = this.txtCardHolderName.Text;
                    guestBillPaymentBO.ChecqueNumber = "";
                    guestBillPaymentBO.PaymentDescription = ddlCardType.SelectedItem.Text;
                }
                else if (ddlPayMode.SelectedValue == "Cheque")
                {
                    supplierPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlChequeReceiveAccountsInfo.SelectedValue);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChequeReceiveAccountsInfo.SelectedValue);
                    guestBillPaymentBO.BankId = Convert.ToInt32(ddlCompanyBank.SelectedValue);
                    guestBillPaymentBO.ChecqueNumber = txtChecqueNumber.Text;

                    guestBillPaymentBO.CardReference = "";
                    guestBillPaymentBO.CardNumber = "";
                    guestBillPaymentBO.BranchName = "";
                    guestBillPaymentBO.PaymentDescription = txtChecqueNumber.Text;
                    supplierPaymentLedgerBO.ChequeNumber = txtChecqueNumber.Text;
                }
                else if (ddlPayMode.SelectedValue == "Company")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    supplierPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    guestBillPaymentBO.PaymentDescription = string.Empty;
                }
                else if (ddlPayMode.SelectedValue == "Adjustment")
                {
                    supplierPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
                    guestBillPaymentBO.ChecqueNumber = "";
                    guestBillPaymentBO.CardReference = "";
                    guestBillPaymentBO.CardNumber = "";
                    guestBillPaymentBO.BranchName = "";
                    guestBillPaymentBO.PaymentDescription = string.Empty;
                    if (string.IsNullOrWhiteSpace(guestBillPaymentBO.Remarks))
                    {
                        guestBillPaymentBO.Remarks = "Bill Adjustment...";
                    }
                }

                guestBillPaymentBO.ModuleName = "Supplier";
                supplierPaymentLedgerBO.PaymentType = guestBillPaymentBO.PaymentMode;
                supplierPaymentLedgerBO.BillNumber = "";
                if (!string.IsNullOrEmpty(txtPaymentDate2.Text))
                {
                    supplierPaymentLedgerBO.PaymentDate = CommonHelper.DateTimeToMMDDYYYY(txtPaymentDate2.Text);
                }
                else
                    supplierPaymentLedgerBO.PaymentDate = DateTime.Now;

                supplierPaymentLedgerBO.SupplierId = Convert.ToInt32(ddlSupplierName.SelectedValue);
                supplierPaymentLedgerBO.CurrencyId = guestBillPaymentBO.FieldId;
                supplierPaymentLedgerBO.ConvertionRate = guestBillPaymentBO.ConvertionRate;

                if (ddlPayMode.SelectedValue == "Loan")
                {
                    supplierPaymentLedgerBO.DRAmount = 0;
                    supplierPaymentLedgerBO.CRAmount = guestBillPaymentBO.PaymentAmount;
                }
                else
                {
                    supplierPaymentLedgerBO.DRAmount = guestBillPaymentBO.PaymentAmount;
                    supplierPaymentLedgerBO.CRAmount = 0;
                }

                supplierPaymentLedgerBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount;
                guestBillPaymentBO.Remarks = txtRemarks.Text;
                supplierPaymentLedgerBO.PaymentStatus = HMConstants.ApprovalStatus.Approved.ToString();

                if (string.IsNullOrWhiteSpace(hfSupplierPaymentId.Value))
                {
                    Boolean status = false;
                    int tmpPaymentId = 0;
                    long tmpSupplierPaymentId = 0;
                    guestBillPaymentBO.CreatedBy = userInformationBO.UserInfoId;
                    supplierPaymentLedgerBO.CreatedBy = userInformationBO.UserInfoId;
                    status = supplierDA.SaveSupplierPaymentLedger(supplierPaymentLedgerBO, out tmpSupplierPaymentId);
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
                    Boolean status = false;
                    supplierPaymentLedgerBO.SupplierPaymentId = Convert.ToInt32(hfSupplierPaymentId.Value);
                    supplierPaymentLedgerBO.LastModifiedBy = userInformationBO.UserInfoId;
                    status = supplierDA.UpdateSupplierPaymentLedger(supplierPaymentLedgerBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), guestBillPaymentBO.DealId,
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
            btnGroupPaymentPreview.Visible = true;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            PMSupplierDA supplierDA = new PMSupplierDA();
            List<PMSupplierPaymentLedgerBO> paymentList = new List<PMSupplierPaymentLedgerBO>();

            int supplierId = Convert.ToInt32(ddlSupplier.SelectedValue);

            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            paymentList = supplierDA.GetSupplierPaymentLedger(fromDate, toDate, string.Empty, false).Where(x => x.PaymentType != "Advance").ToList();
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
        private void LoadSupplierInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            supplierBOList = entityDA.GetPMSupplierInfoByUserInfoId(userInformationBO.UserInfoId);

            ddlSupplierName.DataSource = supplierBOList;
            ddlSupplierName.DataTextField = "Name";
            ddlSupplierName.DataValueField = "SupplierId";
            ddlSupplierName.DataBind();

            this.ddlSupplier.DataSource = supplierBOList;
            this.ddlSupplier.DataTextField = "Name";
            this.ddlSupplier.DataValueField = "SupplierId";
            this.ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlSupplier.Items.Insert(0, item);
        }
        private void LoadPaymentMode()
        {
            PaymentModeDA paymentModeDA = new PaymentModeDA();
            //string forSupplierTransaction = "1, 7, 15";
            string forSupplierTransaction = "1, 7";
            this.ddlPayMode.DataSource = paymentModeDA.GetPaymentModeInfoByCustomString("WHERE nm.PaymentModeId IN (" + forSupplierTransaction + ")");
            this.ddlPayMode.DataTextField = "DisplayName";
            this.ddlPayMode.DataValueField = "PaymentMode";
            this.ddlPayMode.DataBind();

            ListItem itemPayMode = new ListItem();
            itemPayMode.Value = "0";
            itemPayMode.Text = hmUtility.GetDropDownFirstValue();
            this.ddlPayMode.Items.Insert(0, itemPayMode);
        }
        private void LoadBank()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeId(22).Where(x => x.IsTransactionalHead == true).ToList();

            // // //-----ShortTermLoanBankAccountHeadId
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO shortTermLoanBankAccountHeadIdBO = new HMCommonSetupBO();
            shortTermLoanBankAccountHeadIdBO = commonSetupDA.GetCommonConfigurationInfo("ShortTermLoanBankAccountHeadId", "ShortTermLoanBankAccountHeadId");
            if (shortTermLoanBankAccountHeadIdBO != null)
            {
                List<NodeMatrixBO> shortTermLoanBankAccountHeadIdList = new List<NodeMatrixBO>();
                shortTermLoanBankAccountHeadIdList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList(shortTermLoanBankAccountHeadIdBO.SetupValue).Where(x => x.IsTransactionalHead == true).ToList();
                entityBOList.AddRange(shortTermLoanBankAccountHeadIdList);
            }

            // // //-----ShortTermLoanBankAccountHeadId
            HMCommonSetupBO longTermLoanBankAccountHeadIdBO = new HMCommonSetupBO();
            longTermLoanBankAccountHeadIdBO = commonSetupDA.GetCommonConfigurationInfo("LongTermLoanBankAccountHeadId", "LongTermLoanBankAccountHeadId");
            if (longTermLoanBankAccountHeadIdBO != null)
            {
                List<NodeMatrixBO> longTermLoanBankAccountHeadIdList = new List<NodeMatrixBO>();
                longTermLoanBankAccountHeadIdList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList(longTermLoanBankAccountHeadIdBO.SetupValue).Where(x => x.IsTransactionalHead == true).ToList();
                entityBOList.AddRange(longTermLoanBankAccountHeadIdList);
            }

            this.ddlBankId.DataSource = entityBOList.OrderBy(x => x.NodeNumber).ToList();
            this.ddlBankId.DataTextField = "HeadWithCode";
            this.ddlBankId.DataValueField = "NodeId";
            this.ddlBankId.DataBind();

            this.ddlCompanyBank.DataSource = entityBOList.OrderBy(x => x.NodeNumber).ToList();
            this.ddlCompanyBank.DataTextField = "HeadWithCode";
            this.ddlCompanyBank.DataValueField = "NodeId";
            this.ddlCompanyBank.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlCompanyBank.Items.Insert(0, itemBank);
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
        private void LoadCashAndCashEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("16").Where(x => x.IsTransactionalHead == true).ToList();

            this.ddlCashAndCashEquivalantPayment.DataSource = entityBOList;
            this.ddlCashAndCashEquivalantPayment.DataTextField = "HeadWithCode";
            this.ddlCashAndCashEquivalantPayment.DataValueField = "NodeId";
            this.ddlCashAndCashEquivalantPayment.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCashAndCashEquivalantPayment.Items.Insert(0, itemBank);
        }        
        private void LoadAdjustmentEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();

            List<NodeMatrixBO> entityLiabilitiesBOList = new List<NodeMatrixBO>();
            entityLiabilitiesBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("2").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityLiabilitiesBOList != null)
            {
                entityBOList.AddRange(entityLiabilitiesBOList);
            }

            List<NodeMatrixBO> entityIncomeBOList = new List<NodeMatrixBO>();
            entityIncomeBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("3").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityIncomeBOList != null)
            {
                entityBOList.AddRange(entityIncomeBOList);
            }

            List<NodeMatrixBO> entityExpenditureBOList = new List<NodeMatrixBO>();
            entityExpenditureBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityExpenditureBOList != null)
            {
                entityBOList.AddRange(entityExpenditureBOList);
            }

            ddlAdjustmentNodeHead.DataSource = entityBOList.OrderBy(x => x.NodeNumber).ToList();
            ddlAdjustmentNodeHead.DataTextField = "HeadWithCode";
            ddlAdjustmentNodeHead.DataValueField = "NodeId";
            ddlAdjustmentNodeHead.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlAdjustmentNodeHead.Items.Insert(0, itemBank);
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
        private void IsReceiveBillInfoHideInSupplierBillPayment()
        {
            BillInfoDiv.Visible = true;
            Boolean isReceiveBillInfoHideInSupplierBillPayment = false;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO != null)
            {
                if (userInformationBO.IsReceiveBillInfoHideInSupplierBillPayment == 1)
                {
                    isReceiveBillInfoHideInSupplierBillPayment = true;
                }
            }

            if (isReceiveBillInfoHideInSupplierBillPayment == false)
            {

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsReceiveBillInfoHideInSupplierBillPayment", "IsReceiveBillInfoHideInSupplierBillPayment");

                if (commonSetupBO != null)
                {
                    if (commonSetupBO.SetupId > 0)
                    {
                        if (commonSetupBO.SetupValue == "1")
                        {
                            BillInfoDiv.Visible = false;
                        }
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
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
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

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlPaymentFromAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
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
        private void Cancel()
        {
            this.txtLedgerAmount.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.hfSupplierPaymentId.Value = string.Empty;
            this.txtDealId.Value = string.Empty;
            this.txtCalculatedLedgerAmount.Text = string.Empty;
            this.txtCalculatedLedgerAmountHiddenField.Value = string.Empty;
            this.txtConversionRate.Text = string.Empty;
            this.ddlPayMode.SelectedIndex = 0;
            this.txtChecqueNumber.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.ddlBankId.SelectedValue = "0";
            this.txtSrcSupplierId.Focus();
            hfConversionRate.Value = "";
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
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Date.", AlertType.Warning);
                this.txtPaymentDate2.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Amount.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                flag = false;
            }
            else if (this.ddlCurrency.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Currency Type.", AlertType.Warning);
                flag = false;
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

            if (this.ddlPayMode.SelectedValue == "Cheque")
            {
                if (string.IsNullOrWhiteSpace(txtChecqueNumber.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Checque Number.", AlertType.Warning);
                    this.txtChecqueNumber.Focus();
                    flag = false;
                }
                if (this.ddlCompanyBank.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Bank Name.", AlertType.Warning);
                    this.ddlCompanyBank.Focus();
                    flag = false;
                }
            }

            return flag;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
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
            //btnSave.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";

        }
        //************************ User Defined Web Method ********************//
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
        public static SupplierPaymentViewBO FillForm(int editId)
        {
            PMSupplierDA supplierDA = new PMSupplierDA();
            SupplierPaymentViewBO paymentBO = new SupplierPaymentViewBO();

            paymentBO.SupplierPayment = supplierDA.GetSupplierPayment(editId);
            paymentBO.PaymentDetailsInfo = supplierDA.GetSupplierPaymentDetailsByPaymentAndLedger(editId);
            paymentBO.SupplierBill = supplierDA.SupplierBillBySearch(paymentBO.SupplierPayment.SupplierId);

            //CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            //CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            //commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(paymentBO.CurrencyId);
            //paymentBO.CurrencyType = commonCurrencyBO.CurrencyType;

            return paymentBO;
        }

        [WebMethod]
        public static List<PMSupplierPaymentLedgerBO> SupplierBillBySearch(int supplierId)
        {
            PMSupplierDA supplierDA = new PMSupplierDA();
            List<PMSupplierPaymentLedgerBO> supplierBill = new List<PMSupplierPaymentLedgerBO>();

            supplierBill = supplierDA.SupplierBillBySearch(supplierId);

            return supplierBill;
        }

        [WebMethod]
        public static ReturnInfo SaveSupplierBillPayment(SupplierPaymentBO supplierPayment, List<SupplierPaymentDetailsBO> supplierPaymentDetails, List<SupplierPaymentDetailsBO> SupplierPaymentDetailsEdited, List<SupplierPaymentDetailsBO> SupplierPaymentDetailsDeleted)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMSupplierDA supplierDA = new PMSupplierDA();

            try
            {
                if (supplierPayment.PaymentId == 0)
                {
                    rtninfo.IsSuccess = supplierDA.SaveSupplierBillPayment(supplierPayment, supplierPaymentDetails);
                }
                else
                {
                    rtninfo.IsSuccess = supplierDA.UpdateSupplierBillPayment(supplierPayment, supplierPaymentDetails, SupplierPaymentDetailsEdited, SupplierPaymentDetailsDeleted);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;

                if (supplierPayment.PaymentId == 0)
                {
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
            }

            return rtninfo;
        }

        [WebMethod]
        public static GridViewDataNPaging<SupplierPaymentBO, GridPaging> GetSupplierPaymentBySearch(string transactionType, int supplierId, DateTime? dateFrom, DateTime? dateTo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            PMSupplierDA supplierDA = new PMSupplierDA();
            List<SupplierPaymentBO> paymentInfo = new List<SupplierPaymentBO>();
            int totalRecords = 0;
            //string productOutFor = transactionType;
            List<SupplierPaymentBO> outList = new List<SupplierPaymentBO>();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<SupplierPaymentBO, GridPaging> myGridData = new GridViewDataNPaging<SupplierPaymentBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            outList = supplierDA.GetSupplierPaymentBySearch(userInformationBO.UserInfoId, supplierId, dateFrom, dateTo, transactionType, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(outList, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo ApprovedPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMSupplierDA supplierDA = new PMSupplierDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = supplierDA.ApprovedPayment(paymentId, userInformationBO.UserInfoId);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
            }

            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo DeletePaymentInfo(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMSupplierDA supplierDA = new PMSupplierDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = supplierDA.DeletePaymentInfo(paymentId, userInformationBO.UserInfoId);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
            }

            return rtninfo;
        }
    }
}