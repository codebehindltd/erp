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
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmCompanyBillReceive : BasePage
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
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                //hfId.Value = "0";
                hfParentDoc.Value = "0";
                FileUpload();
                btnGroupPaymentPreview.Visible = false;
                btnGroupPaymentPreview1.Visible = false;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                LoadPaymentMode();
                LoadBank();
                LoadCashAccountHead();
                LoadCashAndCashEquivalantHead();
                LoadAdjustmentEquivalantHead();
                LoadCurrency();
                LoadLocalCurrencyId();
                LoadIsConversionRateEditable();
                IsPaymentBillInfoHideInCompanyBillReceive();
                IsLocalCurrencyDefaultSelected();
                LoadCommonDropDownHiddenField();
                LoadGLCompany(false);
                LoadAccountHeadInfo();
                LoadGLProject(false);
                LoadCompany();
                ClearCommonSessionInformation();

                if (isSingle == true)
                {
                    isCompanyProjectPanelEnable = 1;
                    LoadSingleProjectAndCompany();
                }
                else
                {
                    LoadGLCompany(false);
                    LoadGLProject(false);
                }

                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                if (!isIntegrated)
                {
                    isIntegratedGeneralLedgerDiv = -1;
                }
                CheckPermission();
            }
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRelatedInformation();
        }
        protected void btnSrcCmpPayment_Click(object sender, EventArgs e)
        {
            //var cmpId = Convert.ToInt32(hfCmpSearch.Value);
            //GuestCompanyDA companyDA = new GuestCompanyDA();
            //GuestCompanyBO companyBO = new GuestCompanyBO();
            //if (cmpId > 0)
            //{
            //    companyBO = companyDA.GetGuestCompanyInfoForSalesCallById(cmpId);
            //    txtEmailAddress.Text = companyBO.EmailAddress;
            //    txtWebAddress.Text = companyBO.WebAddress;
            //    txtContactPerson.Text = companyBO.ContactPerson;
            //    txtContactNumber.Text = companyBO.ContactNumber;
            //    txtTelephoneNumber.Text = companyBO.TelephoneNumber;
            //    txtAddress.Text = companyBO.CompanyAddress;
            //    hfCmpSearch.Value = companyBO.CompanyId.ToString();
            //}

            //if (cmpId == 0 || cmpId == null)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Company.", AlertType.Warning);
            //    return;
            //}

            //if (cmpId > 0)
            //{
            //    GuestBillPaymentDA da = new GuestBillPaymentDA();
            //    List<GuestBillPaymentBO> files = da.GetGuestBillPaymentInfoByRegistrationId("FrontOffice", cmpId);
            //    isSearchPanelEnable = 1;
            //    gvGuestHouseService.DataSource = files;
            //    gvGuestHouseService.DataBind();
            //}
            //else
            //{
            //    isSearchPanelEnable = -1;
            //    gvGuestHouseService.DataSource = null;
            //    gvGuestHouseService.DataBind();
            //}
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            LoadLocalCurrencyId();
            string transactionHead = string.Empty;

            if (!IsFrmValid())
            {
                SetTab("EntryTab");
                return;
            }

            Boolean isNumberValue = hmUtility.IsNumber(txtLedgerAmount.Text);
            if (!isNumberValue)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Numeric Value.", AlertType.Warning);
                txtLedgerAmount.Focus();
                return;
            }

            if (isSingle == false)
            {
                if (ddlGLCompany.SelectedValue == "0")
                {
                    Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                    if (isIntegrated)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Select Company Name.", AlertType.Warning);
                        ddlGLCompany.Focus();
                        return;
                    }
                }
                else if (ddlGLCompany.SelectedValue != "0")
                {
                    if (ddlGLProject.SelectedValue == "0")
                    {
                        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                        if (isIntegrated)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please select Project Name.", AlertType.Warning);
                            ddlGLProject.Focus();
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

                List<CompanyPaymentLedgerVwBo> companyPaymentLedger = new List<CompanyPaymentLedgerVwBo>();
                GuestBillPaymentBO reservationBillPaymentBO = new GuestBillPaymentBO();
                GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();
                HotelCompanyPaymentLedgerBO companyPaymentLedgerBO = new HotelCompanyPaymentLedgerBO();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

                companyPaymentLedger = JsonConvert.DeserializeObject<List<CompanyPaymentLedgerVwBo>>(hfCompanyBill.Value);

                reservationBillPaymentBO.PaymentType = "Advance";
                reservationBillPaymentBO.RegistrationId = Convert.ToInt32(hfCmpSearch.Value);
                reservationBillPaymentBO.Remarks = txtRemarks.Text;

                if (hfCurrencyType.Value == "Local")
                {
                    reservationBillPaymentBO.FieldId = LocalCurrencyId;
                    reservationBillPaymentBO.ConvertionRate = 1;
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                    reservationBillPaymentBO.CurrencyAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
                    reservationBillPaymentBO.PaymentAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
                }
                else
                {
                    reservationBillPaymentBO.FieldId = Convert.ToInt32(ddlCurrency.SelectedValue);
                    if (txtConversionRate.ReadOnly != true)
                    {
                        reservationBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 1;
                    }
                    else
                    {
                        if (hfConversionRate.Value != "")
                            reservationBillPaymentBO.ConvertionRate = Convert.ToDecimal(hfConversionRate.Value);
                    }
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
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
                    reservationBillPaymentBO.CardType = ddlCardType.SelectedValue.ToString();
                    companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
                    reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
                    reservationBillPaymentBO.CardNumber = txtCardNumber.Text;
                    if (string.IsNullOrEmpty(txtExpireDate.Text))
                    {
                        reservationBillPaymentBO.ExpireDate = null;
                    }
                    else
                    {
                        reservationBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate.Text, userInformationBO.ServerDateFormat);
                    }
                    reservationBillPaymentBO.CardHolderName = txtCardHolderName.Text;
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
                    companyPaymentLedgerBO.AdvanceAmount = 0.00M;

                    if ((txtAdvanceAmount.Text).Trim() != string.Empty && (txtAdvanceAmount.Text).Trim() != "0")
                        companyPaymentLedgerBO.AdvanceAmount = Convert.ToDecimal((txtAdvanceAmount.Text).Trim());

                    status = guestCompanyDA.SaveHotelCompanyPaymentLedger(companyPaymentLedgerBO, companyPaymentLedger, out tmpCompanyPaymentId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), tmpPaymentId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Cancel();
                    }
                }
                else
                {
                    companyPaymentLedgerBO.CompanyPaymentId = Convert.ToInt32(hfCompanyPaymentId.Value);
                    companyPaymentLedgerBO.LastModifiedBy = userInformationBO.UserInfoId;

                    status = guestCompanyDA.UpdateHotelCompanyPaymentLedger(companyPaymentLedgerBO, companyPaymentLedger);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), reservationBillPaymentBO.DealId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Cancel();
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
                //gvGuestHouseService.DataSource = paymentList;
                //gvGuestHouseService.DataBind();
                gvPaymentInfo.DataSource = paymentList;
                gvPaymentInfo.DataBind();
            }
            else
            {
                //gvGuestHouseService.DataSource = null;
                //gvGuestHouseService.DataBind();
                gvPaymentInfo.DataSource = null;
                gvPaymentInfo.DataBind();
            }
            SetTab("SearchTab");
            isSearchPanelEnable = 1;
        }
        //************************ User Defined Function ********************//
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
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

            ddlBankId.DataSource = entityBOList.OrderBy(x => x.NodeNumber).ToList();
            ddlBankId.DataTextField = "HeadWithCode";
            ddlBankId.DataValueField = "NodeId";
            ddlBankId.DataBind();

            ddlCompanyBank.DataSource = entityBOList.OrderBy(x => x.NodeNumber).ToList();
            ddlCompanyBank.DataTextField = "HeadWithCode";
            ddlCompanyBank.DataValueField = "NodeId";
            ddlCompanyBank.DataBind();

            ddlBankPayment.DataSource = entityBOList.OrderBy(x => x.NodeNumber).ToList();
            ddlBankPayment.DataTextField = "HeadWithCode";
            ddlBankPayment.DataValueField = "NodeId";
            ddlBankPayment.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlBankId.Items.Insert(0, itemBank);
            ddlCompanyBank.Items.Insert(0, itemBank);
            ddlBankPayment.Items.Insert(0, itemBank);
        }
        private void LoadCashAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeId(21).Where(x => x.IsTransactionalHead == true).ToList();

            ddlCashPayment.DataSource = entityBOList;
            ddlCashPayment.DataTextField = "HeadWithCode";
            ddlCashPayment.DataValueField = "NodeId";
            ddlCashPayment.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlCashPayment.Items.Insert(0, itemBank);
        }
        private void LoadCashAndCashEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("16").Where(x => x.IsTransactionalHead == true).ToList();

            ddlCashAndCashEquivalantPayment.DataSource = entityBOList;
            ddlCashAndCashEquivalantPayment.DataTextField = "HeadWithCode";
            ddlCashAndCashEquivalantPayment.DataValueField = "NodeId";
            ddlCashAndCashEquivalantPayment.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlCashAndCashEquivalantPayment.Items.Insert(0, itemBank);
        }
        private void LoadAdjustmentEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();

            // // //-----Tax Deducted at Source by Customer (AIT)
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO taxDeductedAtSourceByCustomerAccountHeadBO = new HMCommonSetupBO();
            taxDeductedAtSourceByCustomerAccountHeadBO = commonSetupDA.GetCommonConfigurationInfo("TaxDeductedAtSourceByCustomerAccountHeadId", "TaxDeductedAtSourceByCustomerAccountHeadId");
            if (taxDeductedAtSourceByCustomerAccountHeadBO != null)
            {
                List<NodeMatrixBO> entityBOAditionalList = new List<NodeMatrixBO>();
                entityBOAditionalList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList(taxDeductedAtSourceByCustomerAccountHeadBO.SetupValue).Where(x => x.IsTransactionalHead == true).ToList();

                entityBOList.AddRange(entityBOAditionalList);
            }

            List<NodeMatrixBO> entityAssetsBOList = new List<NodeMatrixBO>();
            entityAssetsBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("1").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityAssetsBOList != null)
            {
                entityBOList.AddRange(entityAssetsBOList);
            }

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
        private void LoadPaymentMode()
        {
            PaymentModeDA paymentModeDA = new PaymentModeDA();
            //string forCompanyTransaction = "1, 8, 15";
            string forCompanyTransaction = "1, 8";
            ddlPayMode.DataSource = paymentModeDA.GetPaymentModeInfoByCustomString("WHERE nm.PaymentModeId IN (" + forCompanyTransaction + ")");
            ddlPayMode.DataTextField = "DisplayName";
            ddlPayMode.DataValueField = "PaymentMode";
            ddlPayMode.DataBind();

            ListItem itemPayMode = new ListItem();
            itemPayMode.Value = "0";
            itemPayMode.Text = hmUtility.GetDropDownFirstValue();
            ddlPayMode.Items.Insert(0, itemPayMode);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCurrency.Items.Insert(0, item);
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
                        txtConversionRate.ReadOnly = true;
                    }
                    else
                    {
                        txtConversionRate.ReadOnly = false;
                    }
                }
            }
        }
        private void IsPaymentBillInfoHideInCompanyBillReceive()
        {
            BillInfoDiv.Visible = true;
            CompanyBillDiv.Visible = true;
            Boolean isPaymentBillInfoHideInCompanyBillReceive = false;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO != null)
            {
                if (userInformationBO.IsPaymentBillInfoHideInCompanyBillReceive == 1)
                {
                    isPaymentBillInfoHideInCompanyBillReceive = true;
                }
            }

            if (isPaymentBillInfoHideInCompanyBillReceive == false)
            {
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsPaymentBillInfoHideInCompanyBillReceive", "IsPaymentBillInfoHideInCompanyBillReceive");

                if (commonSetupBO != null)
                {
                    if (commonSetupBO.SetupId > 0)
                    {
                        if (commonSetupBO.SetupValue == "1")
                        {
                            BillInfoDiv.Visible = false;
                            CompanyBillDiv.Visible = false;
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
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(ddlRoomId.SelectedValue);
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";

        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (ddlPayMode.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Payment Mode.", AlertType.Warning);
                ddlPayMode.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtPaymentDate2.Text))
            {
                isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Date.", AlertType.Warning);
                txtPaymentDate2.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtLedgerAmount.Text))
            {
                isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Amount.", AlertType.Warning);
                txtLedgerAmount.Focus();
                flag = false;
            }
            else if (ddlCurrency.SelectedValue == "0")
            {
                isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Select Currency Type.", AlertType.Warning);
                flag = false;
            }

            if (ddlPayMode.SelectedValue == "Bank")
            {
                if (ddlBankPayment.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Bank Name.", AlertType.Warning);
                    ddlBankPayment.Focus();
                    flag = false;
                }
            }

            if (ddlPayMode.SelectedValue == "Cash")
            {
                if (ddlCashPayment.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Cash Name.", AlertType.Warning);
                    ddlCashPayment.Focus();
                    flag = false;
                }
            }

            if (ddlPayMode.SelectedValue == "Adjustment")
            {
                if (ddlCashPayment.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Cash Name.", AlertType.Warning);
                    ddlCashPayment.Focus();
                    flag = false;
                }
            }

            return flag;
        }
        private void LoadSingleProjectAndCompany()
        {
            LoadGLCompany(true);
            LoadGLProject(true);
        }
        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (isSingle == true)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
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
                ddlGLProject.DataSource = projectList;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();
            }
            else
            {
                ddlGLProject.DataSource = List;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();

                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstValue();
                ddlGLProject.Items.Insert(0, itemProject);
            }
        }
        private void LoadCompany()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            //companyBOList = companyDA.GetGuestCompanyInfo();
            companyBOList = companyDA.GetGuestCompanyInfoByUserId(userInformationBO.UserInfoId);

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlCompany.Items.Insert(0, item);

            ddlCompany.DataSource = companyBOList;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ddlCompanyForSearch.DataSource = companyBOList;
            ddlCompanyForSearch.DataTextField = "CompanyName";
            ddlCompanyForSearch.DataValueField = "CompanyId";
            ddlCompanyForSearch.DataBind();
            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCompanyForSearch.Items.Insert(0, itemSearch);
        }
        private void Cancel()
        {
            txtLedgerAmount.Text = string.Empty;
            btnSave.Text = "Save";
            hfCompanyPaymentId.Value = string.Empty;
            txtDealId.Value = string.Empty;
            txtCalculatedLedgerAmount.Text = string.Empty;
            txtCalculatedLedgerAmountHiddenField.Value = string.Empty;
            txtConversionRate.Text = string.Empty;
            ddlPayMode.SelectedIndex = 0;
            ClearCommonSessionInformation();
            txtRemarks.Text = string.Empty;
            ddlBankId.SelectedValue = "0";
            hfConversionRate.Value = "";
            txtAdvanceAmount.Text = "";
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            lblPaymentAccountHead.Text = "Payment Receive In";

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            CommonPaymentModeBO cardPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Card").FirstOrDefault();
            CommonPaymentModeBO chequePaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cheque").FirstOrDefault();
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            CommonPaymentModeBO mBankingPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "M-Banking").FirstOrDefault();
            CommonPaymentModeBO refundPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Refund").FirstOrDefault();

            ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            ddlCashReceiveAccountsInfo.DataBind();

            ddlPaymentFromAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlPaymentFromAccountsInfo.DataTextField = "NodeHead";
            ddlPaymentFromAccountsInfo.DataValueField = "NodeId";
            ddlPaymentFromAccountsInfo.DataBind();

            ddlPaymentToAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + refundPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlPaymentToAccountsInfo.DataTextField = "NodeHead";
            ddlPaymentToAccountsInfo.DataValueField = "NodeId";
            ddlPaymentToAccountsInfo.DataBind();

            ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            ddlCardReceiveAccountsInfo.DataBind();

            ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            ddlChequeReceiveAccountsInfo.DataBind();

            CustomFieldBO IncomeSourceAccountsInfo = new CustomFieldBO();
            IncomeSourceAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("IncomeSourceAccountsInfo");
            ddlIncomeSourceAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + IncomeSourceAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            ddlIncomeSourceAccountsInfo.DataTextField = "NodeHead";
            ddlIncomeSourceAccountsInfo.DataValueField = "NodeId";
            ddlIncomeSourceAccountsInfo.DataBind();

            ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + companyPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            ddlCompanyPaymentAccountHead.DataBind();
        }
        private void AdvanceVoucherPost(out int tmpGLMasterId)
        {
            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            GLLedgerBO detailBODebit = new GLLedgerBO();
            detailBODebit.LedgerId = 0;
            if (ddlPayMode.SelectedIndex == 0)
            {
                detailBODebit.NodeId = Convert.ToInt32(ddlCashReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = ddlCashReceiveAccountsInfo.SelectedItem.Text;
            }
            else if (ddlPayMode.SelectedIndex == 1)
            {
                detailBODebit.NodeId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = ddlCardReceiveAccountsInfo.SelectedItem.Text;
            }
            else if (ddlPayMode.SelectedIndex == 2)
            {
                detailBODebit.NodeId = Convert.ToInt32(ddlChequeReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = ddlChequeReceiveAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = 1;
            detailBODebit.FieldId = Convert.ToInt32(ddlCurrency.SelectedValue); //45 for Local Currency

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBODebit.CurrencyAmount = mCurrencyAmount;
                detailBODebit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBODebit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBODebit.CurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
            }

            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            //Oppusite Head (Payment Head Information)
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = Convert.ToInt32(ddlIncomeSourceAccountsInfo.SelectedValue);
            detailBOCredit.NodeHead = ddlIncomeSourceAccountsInfo.SelectedItem.Text;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = Convert.ToInt32(ddlCurrency.SelectedValue);

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBOCredit.CurrencyAmount = mCurrencyAmount;
                detailBOCredit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBOCredit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBOCredit.CurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
            }

            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            Boolean isCash = true;
            if (ddlPayMode.SelectedItem.Text != "Cash")
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

            glMasterBO.ProjectId = Convert.ToInt32(ddlGLProject.SelectedValue);


            if (ddlPayMode.SelectedIndex == 0)
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
            txtLedgerAmount.Text = "0";
        }
        private void CashOutVoucherPost(out int tmpGLMasterId)
        {
            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            GLLedgerBO detailBODebit = new GLLedgerBO();
            detailBODebit.LedgerId = 0;
            if (ddlPayMode.SelectedIndex == 0)
            {
                detailBODebit.NodeId = Convert.ToInt32(ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            else if (ddlPayMode.SelectedIndex == 1)
            {
                detailBODebit.NodeId = Convert.ToInt32(ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            else if (ddlPayMode.SelectedIndex == 2)
            {
                detailBODebit.NodeId = Convert.ToInt32(ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = 1;
            detailBODebit.FieldId = Convert.ToInt32(ddlCurrency.SelectedValue); //45 for Local Currency

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBODebit.CurrencyAmount = mCurrencyAmount;
                detailBODebit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBODebit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBODebit.CurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
            }

            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            //Oppusite Head (Payment Head Information)
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = Convert.ToInt32(ddlPaymentFromAccountsInfo.SelectedValue);
            detailBOCredit.NodeHead = ddlPaymentFromAccountsInfo.SelectedItem.Text;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = Convert.ToInt32(ddlCurrency.SelectedValue);

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBOCredit.CurrencyAmount = mCurrencyAmount;
                detailBOCredit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBOCredit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBOCredit.CurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
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
            glMasterBO.ProjectId = Convert.ToInt32(ddlGLProject.SelectedValue);
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
            txtLedgerAmount.Text = "0";
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
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("HotelGuestBillPayment", "PaymentId", sEmpId);
                if (status)
                {
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
        public static CompanyPaymentViewBO FillForm(Int64 paymentId)
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            CompanyPaymentViewBO paymentBO = new CompanyPaymentViewBO();

            paymentBO.CompanyPayment = guestCompanyDA.GetCompanyPayment(paymentId);
            paymentBO.CompanyPaymentDetails = guestCompanyDA.GetCompanyPaymentDetails(paymentId);
            paymentBO.Company = guestCompanyDA.GetGuestCompanyInfoForSalesCallById(paymentBO.CompanyPayment.CompanyId);


            paymentBO.CompanyPaymentTransactionDetails = guestCompanyDA.GetCompanyPaymentTransectionDetails(paymentId);

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

        [WebMethod]
        public static List<CompanyBillGenerationBO> GetCompanyGeneratedBillByBillStatus(int companyId)
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            List<CompanyBillGenerationBO> paymentInfo = new List<CompanyBillGenerationBO>();

            paymentInfo = companyDa.GetCompanyGeneratedBillByBillStatus(companyId);

            return paymentInfo;
        }

        [WebMethod]
        public static List<CompanyBillGenerateViewBO> CompanyBillBySearch(int companyBillId, int companyId)
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            List<CompanyBillGenerateViewBO> companyBill = new List<CompanyBillGenerateViewBO>();
            companyBill = companyDa.GetCompanyBillForBillReceive(companyId, companyBillId);

            return companyBill;
        }

        [WebMethod]
        public static ReturnInfo SaveCompanyBillPayment(CompanyPaymentBO companyPayment, List<CompanyPaymentDetailsBO> companyPaymentDetails, List<CompanyPaymentDetailsBO> ReceiveInformationDetails, List<CompanyPaymentDetailsBO> ReceiveInformationDeletedDetails,
                                                       List<CompanyPaymentDetailsBO> companyPaymentDetailsEdited, List<CompanyPaymentDetailsBO> companyPaymentDetailsDeleted,
                                                        int randomDocId, string deletedDoc)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            companyPayment.CreatedBy = userInformationBO.UserInfoId;
            companyPayment.LastModifiedBy = userInformationBO.UserInfoId;

            ReturnInfo rtninfo = new ReturnInfo();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            HMCommonDA hmCommonDA = new HMCommonDA();
            long id = 0;

            bool status = false;
            int OwnerIdForDocuments = 0;

            companyPayment.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();

            try
            {
                if (companyPayment.PaymentId == 0)
                {
                    rtninfo.IsSuccess = companyDa.SaveCompanyBillPaymentTransaction(companyPayment, companyPaymentDetails, ReceiveInformationDetails, ReceiveInformationDeletedDetails, out id);
                    if (rtninfo.IsSuccess)
                        OwnerIdForDocuments = Convert.ToInt32(id);

                    DocumentsDA documentsDA = new DocumentsDA();
                    string s = deletedDoc;
                    string[] DeletedDocList = s.Split(',');
                    for (int i = 0; i < DeletedDocList.Length; i++)
                    {
                        DeletedDocList[i] = DeletedDocList[i].Trim();
                        Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

                }
                else
                {
                    rtninfo.IsSuccess = companyDa.UpdateCompanyBillPaymentTransaction(companyPayment, companyPaymentDetails, companyPaymentDetailsEdited, companyPaymentDetailsDeleted, ReceiveInformationDetails, ReceiveInformationDeletedDetails);
                    if (rtninfo.IsSuccess)
                        OwnerIdForDocuments = Convert.ToInt32(companyPayment.PaymentId);

                    DocumentsDA documentsDA = new DocumentsDA();
                    string s = deletedDoc;
                    string[] DeletedDocList = s.Split(',');
                    for (int i = 0; i < DeletedDocList.Length; i++)
                    {
                        DeletedDocList[i] = DeletedDocList[i].Trim();
                        Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

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

                if (companyPayment.PaymentId == 0)
                {
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
            }
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            rtninfo.Data = randomId;
            return rtninfo;
        }
        [WebMethod]
        public static List<CompanyPaymentBO> GetCompanyPaymentBySearch(string transactionType, int companyId, DateTime? dateFrom, DateTime? dateTo, string transactionStatus)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GuestCompanyDA companyDa = new GuestCompanyDA();
            List<CompanyPaymentBO> paymentInfo = new List<CompanyPaymentBO>();
            paymentInfo = companyDa.GetCompanyPaymentBySearch(userInformationBO.UserInfoId, companyId, dateFrom, dateTo, transactionType, transactionStatus);

            return paymentInfo;
        }

        [WebMethod]
        public static ReturnInfo CheckedPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = companyDa.CheckedPayment(paymentId, userInformationBO.UserInfoId);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
            }

            return rtninfo;
        }

        [WebMethod]
        public static ReturnInfo ApprovedPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = companyDa.ApprovedPayment(paymentId, userInformationBO.UserInfoId);
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
        public static ReturnInfo DeleteCompanyPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = companyDa.DeletePayment(paymentId, userInformationBO.UserInfoId);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }

            return rtninfo;
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyBillReceive", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyBillReceive", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }
    }
}