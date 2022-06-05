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
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmployeeBillReceive : BasePage
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
            CheckObjectPermission();
            isIntegratedGeneralLedgerDiv = -1;
            isSingle = hmUtility.GetSingleProjectAndCompany();
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                btnGroupPaymentPreview.Visible = false;
                btnGroupPaymentPreview1.Visible = false;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                LoadPaymentMode();
                LoadBank();
                LoadCashAccountHead();
                LoadAdjustmentEquivalantHead();
                LoadCurrency();
                LoadLocalCurrencyId();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                LoadCommonDropDownHiddenField();
                LoadGLCompany(false);
                LoadAccountHeadInfo();
                LoadGLProject(false);
                LoadEmployee();
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
            }
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRelatedInformation();
        }
        protected void btnSrcCmpPayment_Click(object sender, EventArgs e)
        {
            var cmpId = 0; // Convert.ToInt32(hfCmpSearch.Value);
            GuestCompanyDA companyDA = new GuestCompanyDA();
            GuestCompanyBO companyBO = new GuestCompanyBO();
            if (cmpId > 0)
            {
                companyBO = companyDA.GetGuestCompanyInfoForSalesCallById(cmpId);
                //txtEmailAddress.Text = companyBO.EmailAddress;
                //txtWebAddress.Text = companyBO.WebAddress;
                //txtContactPerson.Text = companyBO.ContactPerson;
                //txtContactNumber.Text = companyBO.ContactNumber;
                //txtTelephoneNumber.Text = companyBO.TelephoneNumber;
                //txtAddress.Text = companyBO.CompanyAddress;
                //hfCmpSearch.Value = companyBO.CompanyId.ToString();
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
                //gvGuestHouseService.DataSource = files;
                //gvGuestHouseService.DataBind();
            }
            else
            {
                isSearchPanelEnable = -1;
                //gvGuestHouseService.DataSource = null;
                //gvGuestHouseService.DataBind();
            }
        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    LoadLocalCurrencyId();
        //    string transactionHead = string.Empty;

        //    if (!IsFrmValid())
        //    {
        //        SetTab("EntryTab");
        //        return;
        //    }

        //    Boolean isNumberValue = hmUtility.IsNumber(txtLedgerAmount.Text);
        //    if (!isNumberValue)
        //    {
        //        CommonHelper.AlertInfo(innboardMessage, "Please Enter Numeric Value.", AlertType.Warning);
        //        txtLedgerAmount.Focus();
        //        return;
        //    }

        //    if (isSingle == false)
        //    {
        //        if (ddlGLCompany.SelectedValue == "0")
        //        {
        //            Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
        //            if (isIntegrated)
        //            {
        //                CommonHelper.AlertInfo(innboardMessage, "Please Select Company Name.", AlertType.Warning);
        //                ddlGLCompany.Focus();
        //                return;
        //            }
        //        }
        //        else if (ddlGLCompany.SelectedValue != "0")
        //        {
        //            if (ddlGLProject.SelectedValue == "0")
        //            {
        //                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
        //                if (isIntegrated)
        //                {
        //                    CommonHelper.AlertInfo(innboardMessage, "Please select Project Name.", AlertType.Warning);
        //                    ddlGLProject.Focus();
        //                    return;
        //                }
        //            }
        //        }
        //    }

        //    HMCommonDA hmCommonDA = new HMCommonDA();
        //    CustomFieldBO customField = new CustomFieldBO();

        //    if (customField == null)
        //    {
        //        CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
        //        return;
        //    }
        //    else
        //    {
        //        UserInformationBO userInformationBO = new UserInformationBO();
        //        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //        List<CompanyPaymentLedgerVwBo> companyPaymentLedger = new List<CompanyPaymentLedgerVwBo>();
        //        GuestBillPaymentBO reservationBillPaymentBO = new GuestBillPaymentBO();
        //        GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();
        //        HotelCompanyPaymentLedgerBO companyPaymentLedgerBO = new HotelCompanyPaymentLedgerBO();
        //        GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

        //        companyPaymentLedger = JsonConvert.DeserializeObject<List<CompanyPaymentLedgerVwBo>>(hfCompanyBill.Value);

        //        reservationBillPaymentBO.PaymentType = "Advance";
        //        reservationBillPaymentBO.RegistrationId = 0; // Convert.ToInt32(hfCmpSearch.Value);
        //        reservationBillPaymentBO.Remarks = txtRemarks.Text;

        //        if (hfCurrencyType.Value == "Local")
        //        {
        //            reservationBillPaymentBO.FieldId = LocalCurrencyId;
        //            reservationBillPaymentBO.ConvertionRate = 1;
        //            decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
        //            reservationBillPaymentBO.CurrencyAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
        //            reservationBillPaymentBO.PaymentAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
        //        }
        //        else
        //        {
        //            reservationBillPaymentBO.FieldId = Convert.ToInt32(ddlCurrency.SelectedValue);
        //            if (txtConversionRate.ReadOnly != true)
        //            {
        //                reservationBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 1;
        //            }
        //            else
        //            {
        //                if (hfConversionRate.Value != "")
        //                    reservationBillPaymentBO.ConvertionRate = Convert.ToDecimal(hfConversionRate.Value);
        //            }
        //            decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(txtLedgerAmount.Text) ? Convert.ToDecimal(txtLedgerAmount.Text) : 0;
        //            reservationBillPaymentBO.CurrencyAmount = tmpCurrencyAmount;
        //            reservationBillPaymentBO.PaymentAmount = tmpCurrencyAmount * reservationBillPaymentBO.ConvertionRate;
        //        }

        //        reservationBillPaymentBO.PaymentMode = ddlPayMode.SelectedValue;
        //        reservationBillPaymentBO.PaymentModeId = 0; // Convert.ToInt32(hfCmpSearch.Value);

        //        reservationBillPaymentBO.ChecqueDate = DateTime.Now;
        //        reservationBillPaymentBO.BankId = Convert.ToInt32(ddlBankId.SelectedValue);
        //        reservationBillPaymentBO.ServiceBillId = null;

        //        if (ddlPayMode.SelectedValue == "Cash")
        //        {
        //            companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
        //            reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
        //            reservationBillPaymentBO.ChecqueNumber = "";
        //            reservationBillPaymentBO.CardReference = "";
        //            reservationBillPaymentBO.CardNumber = "";
        //            reservationBillPaymentBO.BranchName = "";
        //            reservationBillPaymentBO.PaymentDescription = string.Empty;
        //        }
        //        else if (ddlPayMode.SelectedValue == "Card")
        //        {
        //            reservationBillPaymentBO.CardType = ddlCardType.SelectedValue.ToString();
        //            companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
        //            reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
        //            reservationBillPaymentBO.CardNumber = txtCardNumber.Text;
        //            if (string.IsNullOrEmpty(txtExpireDate.Text))
        //            {
        //                reservationBillPaymentBO.ExpireDate = null;
        //            }
        //            else
        //            {
        //                reservationBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate.Text, userInformationBO.ServerDateFormat);
        //            }
        //            reservationBillPaymentBO.CardHolderName = txtCardHolderName.Text;
        //            reservationBillPaymentBO.ChecqueNumber = "";
        //            reservationBillPaymentBO.PaymentDescription = ddlCardType.SelectedItem.Text;
        //        }
        //        else if (ddlPayMode.SelectedValue == "Cheque")
        //        {
        //            companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId.SelectedValue);
        //            reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId.SelectedValue);
        //            reservationBillPaymentBO.BankId = Convert.ToInt32(ddlCompanyBank.SelectedValue);
        //            reservationBillPaymentBO.ChecqueNumber = txtChecqueNumber.Text;

        //            reservationBillPaymentBO.CardReference = "";
        //            reservationBillPaymentBO.CardNumber = "";
        //            reservationBillPaymentBO.BranchName = "";
        //            reservationBillPaymentBO.PaymentDescription = txtChecqueNumber.Text;
        //        }
        //        else if (ddlPayMode.SelectedValue == "Bank")
        //        {
        //            companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlBankPayment.SelectedValue);
        //            reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlBankPayment.SelectedValue);
        //            reservationBillPaymentBO.BankId = Convert.ToInt32(ddlBankPayment.SelectedValue);
        //            reservationBillPaymentBO.ChecqueNumber = string.Empty;

        //            reservationBillPaymentBO.CardReference = "";
        //            reservationBillPaymentBO.CardNumber = "";
        //            reservationBillPaymentBO.BranchName = "";
        //            reservationBillPaymentBO.PaymentDescription = ddlBankPayment.SelectedItem.Text;
        //        }
        //        else if (ddlPayMode.SelectedValue == "Company")
        //        {
        //            reservationBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
        //            companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
        //            reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
        //            reservationBillPaymentBO.PaymentDescription = string.Empty;
        //        }
        //        else if (ddlPayMode.SelectedValue == "Adjustment")
        //        {
        //            companyPaymentLedgerBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
        //            reservationBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPayment.SelectedValue);
        //            reservationBillPaymentBO.ChecqueNumber = "";
        //            reservationBillPaymentBO.CardReference = "";
        //            reservationBillPaymentBO.CardNumber = "";
        //            reservationBillPaymentBO.BranchName = "";
        //            reservationBillPaymentBO.PaymentDescription = string.Empty;
        //            if (string.IsNullOrWhiteSpace(reservationBillPaymentBO.Remarks))
        //            {
        //                reservationBillPaymentBO.Remarks = "Bill Adjustment...";
        //            }
        //        }
        //        reservationBillPaymentBO.ModuleName = "FrontOffice";

        //        //Company Payment Ledger
        //        companyPaymentLedgerBO.PaymentType = reservationBillPaymentBO.PaymentMode;
        //        companyPaymentLedgerBO.BillNumber = "";
        //        if (!string.IsNullOrEmpty(txtPaymentDate2.Text))
        //        {
        //            companyPaymentLedgerBO.PaymentDate = CommonHelper.DateTimeToMMDDYYYY(txtPaymentDate2.Text);
        //        }
        //        else
        //            companyPaymentLedgerBO.PaymentDate = DateTime.Now;

        //        companyPaymentLedgerBO.CompanyId = reservationBillPaymentBO.RegistrationId;
        //        companyPaymentLedgerBO.CurrencyId = reservationBillPaymentBO.FieldId;
        //        companyPaymentLedgerBO.ConvertionRate = reservationBillPaymentBO.ConvertionRate;
        //        if (ddlPayMode.SelectedValue == "Loan")
        //        {
        //            companyPaymentLedgerBO.DRAmount = reservationBillPaymentBO.PaymentAmount;
        //            companyPaymentLedgerBO.CRAmount = 0.00M;
        //        }
        //        else
        //        {
        //            companyPaymentLedgerBO.DRAmount = 0.00M;
        //            companyPaymentLedgerBO.CRAmount = reservationBillPaymentBO.PaymentAmount;
        //        }
        //        companyPaymentLedgerBO.CurrencyAmount = reservationBillPaymentBO.CurrencyAmount;
        //        companyPaymentLedgerBO.Remarks = reservationBillPaymentBO.Remarks;
        //        companyPaymentLedgerBO.PaymentStatus = HMConstants.ApprovalStatus.Approved.ToString();

        //        Boolean status = false;

        //        if (string.IsNullOrWhiteSpace(hfCompanyPaymentId.Value))
        //        {
        //            int tmpPaymentId = 0;
        //            long tmpCompanyPaymentId = 0;
        //            reservationBillPaymentBO.CreatedBy = userInformationBO.UserInfoId;
        //            companyPaymentLedgerBO.CreatedBy = userInformationBO.UserInfoId;
        //            companyPaymentLedgerBO.AdvanceAmount = 0.00M;

        //            if ((txtAdvanceAmount.Text).Trim() != string.Empty && (txtAdvanceAmount.Text).Trim() != "0")
        //                companyPaymentLedgerBO.AdvanceAmount = Convert.ToDecimal((txtAdvanceAmount.Text).Trim());

        //            status = guestCompanyDA.SaveHotelCompanyPaymentLedger(companyPaymentLedgerBO, companyPaymentLedger, out tmpCompanyPaymentId);

        //            if (status)
        //            {
        //                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Save.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), tmpPaymentId,
        //                    ProjectModuleEnum.ProjectModule.HotelManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
        //                Cancel();
        //            }
        //        }
        //        else
        //        {
        //            companyPaymentLedgerBO.CompanyPaymentId = Convert.ToInt32(hfCompanyPaymentId.Value);
        //            companyPaymentLedgerBO.LastModifiedBy = userInformationBO.UserInfoId;

        //            status = guestCompanyDA.UpdateHotelCompanyPaymentLedger(companyPaymentLedgerBO, companyPaymentLedger);

        //            if (status)
        //            {
        //                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), reservationBillPaymentBO.DealId,
        //                    ProjectModuleEnum.ProjectModule.HotelManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
        //                Cancel();
        //            }
        //        }
        //    }
        //    SetTab("EntryTab");
        //}
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

        private void LoadEmployee()
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeeBO> employeeList = new List<EmployeeBO>();
            employeeList = employeeDa.GetEmployeeInfo();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            ddlEmployeeForSearch.DataSource = employeeList;
            ddlEmployeeForSearch.DataTextField = "DisplayName";
            ddlEmployeeForSearch.DataValueField = "EmpId";
            ddlEmployeeForSearch.DataBind();
            ddlEmployeeForSearch.Items.Insert(0, item);
        }
        private void LoadBank()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeId(22).Where(x => x.IsTransactionalHead == true).ToList();

            ddlBankId.DataSource = entityBOList;
            ddlBankId.DataTextField = "HeadWithCode";
            ddlBankId.DataValueField = "NodeId";
            ddlBankId.DataBind();

            ddlCompanyBank.DataSource = entityBOList;
            ddlCompanyBank.DataTextField = "HeadWithCode";
            ddlCompanyBank.DataValueField = "NodeId";
            ddlCompanyBank.DataBind();

            ddlBankPayment.DataSource = entityBOList;
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

        private void LoadAdjustmentEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(x => x.IsTransactionalHead == true).ToList();

            ddlAdjustmentNodeHead.DataSource = entityBOList;
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
        private void CheckObjectPermission()
        {
            
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
        private void Cancel()
        {
            //txtLedgerAmount.Text = string.Empty;
            //btnSave.Text = "Save";
            //hfCompanyPaymentId.Value = string.Empty;
            //txtDealId.Value = string.Empty;
            //txtCalculatedLedgerAmount.Text = string.Empty;
            //txtCalculatedLedgerAmountHiddenField.Value = string.Empty;
            //txtConversionRate.Text = string.Empty;
            //ddlPayMode.SelectedIndex = 0;
            //ClearCommonSessionInformation();
            //txtRemarks.Text = string.Empty;
            //ddlBankId.SelectedValue = "0";
            //hfConversionRate.Value = "";
            //txtAdvanceAmount.Text = "";
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
        public static EmployeePaymentViewBO FillForm(Int64 paymentId)
        {
            EmployeeDA employeeDa = new EmployeeDA();
            EmployeePaymentViewBO paymentBO = new EmployeePaymentViewBO();

            paymentBO.EmployeePayment = employeeDa.GetEmployeePayment(paymentId);
            paymentBO.EmployeePaymentDetails = employeeDa.GetEmployeePaymentDetails(paymentId);
            paymentBO.Employee = employeeDa.GetEmployeeInfoById(paymentBO.EmployeePayment.EmployeeId);

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

        //[WebMethod]
        //public static List<CompanyPaymentLedgerVwBo> CompanyBillBySearch(int companyId)
        //{
        //    GuestCompanyDA companyDa = new GuestCompanyDA();
        //    List<CompanyPaymentLedgerVwBo> companyBill = new List<CompanyPaymentLedgerVwBo>();

        //    companyBill = companyDa.CompanyBillBySearch(companyId);

        //    return companyBill;
        //}

        [WebMethod]
        public static List<EmployeeBillGenerationBO> GetEmployeeGeneratedBillByBillStatus(int employeeId)
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeeBillGenerationBO> paymentInfo = new List<EmployeeBillGenerationBO>();

            paymentInfo = employeeDa.GetEmployeeGeneratedBillByBillStatus(employeeId);

            return paymentInfo;
        }

        [WebMethod]
        public static List<EmployeeBillGenerateViewBO> EmployeeBillBySearch(int employeeBillId, int employeeId)
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeeBillGenerateViewBO> employeeBill = new List<EmployeeBillGenerateViewBO>();
            employeeBill = employeeDa.GetEmployeeBillForBillReceive(employeeId, employeeBillId);

            return employeeBill;
        }

        [WebMethod]
        public static ReturnInfo SaveEmployeeBillPayment(EmployeePaymentBO employeePayment, List<EmployeePaymentDetailsBO> employeePaymentDetails,
                                                       List<EmployeePaymentDetailsBO> employeePaymentDetailsEdited, List<EmployeePaymentDetailsBO> employeePaymentDetailsDeleted)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA employeeDa = new EmployeeDA();
            HMUtility hmUtility = new HMUtility();
            long tmpId;

            try
            {
                if (employeePayment.PaymentId == 0)
                {
                    rtninfo.IsSuccess = employeeDa.SaveEmployeeBillPayment(employeePayment, employeePaymentDetails, out tmpId);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
                }
                else
                {
                    rtninfo.IsSuccess = employeeDa.UpdateEmployeeBillPayment(employeePayment, employeePaymentDetails, employeePaymentDetailsEdited, employeePaymentDetailsDeleted);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), employeePayment.PaymentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
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

                if (employeePayment.PaymentId == 0)
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                else
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }

            return rtninfo;
        }
        [WebMethod]
        public static List<EmployeePaymentBO> GetEmployeePaymentBySearch(string transactionType, int employeeId, DateTime? dateFrom, DateTime? dateTo)
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeePaymentBO> paymentInfo = new List<EmployeePaymentBO>();
            paymentInfo = employeeDa.GetEmployeePaymentBySearch(employeeId, dateFrom, dateTo, transactionType);

            return paymentInfo;
        }

        [WebMethod]
        public static ReturnInfo DeleteEmployeePayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA employeeDa = new EmployeeDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = employeeDa.DeleteEmployeePayment(paymentId, userInformationBO.EmpId);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), paymentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
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
        public static ReturnInfo ApprovedPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA employeeDa = new EmployeeDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = employeeDa.ApprovedPayment(paymentId, userInformationBO.EmpId);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), paymentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
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