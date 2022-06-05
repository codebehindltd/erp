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
using System.Collections;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmSupplierBillAdjustment : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int LocalCurrencyId;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSupplier();
                LoadCommonDropDownHiddenField();

                LoadPaymentMode();
                LoadBank();
                LoadCashAccountHead();
                LoadCurrency();
                LoadLocalCurrencyId();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                LoadAccountHeadInfo();
            }
            CheckObjectPermission();
        }
        private void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        private void LoadSupplier()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            supplierBOList = entityDA.GetPMSupplierInfo();

            ddlSupplier.DataSource = supplierBOList;
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlSupplier.Items.Insert(0, item);

            ddlSupplierForSearch.DataSource = supplierBOList;
            ddlSupplierForSearch.DataTextField = "Name";
            ddlSupplierForSearch.DataValueField = "SupplierId";
            ddlSupplierForSearch.DataBind();
            ddlSupplierForSearch.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadPaymentMode()
        {
            PaymentModeDA paymentModeDA = new PaymentModeDA();
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

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "HeadWithCode";
            this.ddlBankId.DataValueField = "NodeId";
            this.ddlBankId.DataBind();

            this.ddlCompanyBank.DataSource = entityBOList;
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

        [WebMethod]
        public static ArrayList SupplierGeneratedBillBySearch(int supplierId)
        {
            PMSupplierDA supplierDa = new PMSupplierDA();

            GuestCompanyDA companyDa = new GuestCompanyDA();
            PMSupplierBO supplier = new PMSupplierBO();
            List<PMSupplierPaymentLedgerBO> paymentInfo = new List<PMSupplierPaymentLedgerBO>();

            supplier = supplierDa.GetPMSupplierInfoById(supplierId);
            paymentInfo = supplierDa.SupplierBillBySearch(supplierId);

            ArrayList arr = new ArrayList();
            arr.Add(supplier);
            arr.Add(paymentInfo);

            return arr;
        }

        [WebMethod]
        public static ArrayList SupplierBillAdvanceBySearch(int supplierId)
        {
            PMSupplierDA supplierDa = new PMSupplierDA();

            GuestCompanyDA companyDa = new GuestCompanyDA();
            PMSupplierBO supplier = new PMSupplierBO();
            List<PMSupplierPaymentLedgerBO> paymentInfo = new List<PMSupplierPaymentLedgerBO>();

            supplier = supplierDa.GetPMSupplierInfoById(supplierId);
            paymentInfo = supplierDa.SupplierBillAdvanceBySearch(supplierId);

            ArrayList arr = new ArrayList();
            arr.Add(supplier);
            arr.Add(paymentInfo);

            return arr;
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
        public static ReturnInfo AdjustedSupplierBillPayment(SupplierPaymentBO supplierPayment, List<SupplierPaymentDetailsBO> supplierPaymentDetails,
                                                      List<SupplierPaymentDetailsBO> supplierPaymentDetailsEdited, List<SupplierPaymentDetailsBO> supplierPaymentDetailsDeleted)
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
                    rtninfo.IsSuccess = supplierDA.UpdateSupplierBillPayment(supplierPayment, supplierPaymentDetails, supplierPaymentDetailsEdited, supplierPaymentDetailsDeleted);
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
        public static List<CompanyBillGenerationBO> GetCompanyGeneratedBillByBillStatus(int companyId)
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            List<CompanyBillGenerationBO> paymentInfo = new List<CompanyBillGenerationBO>();

            paymentInfo = companyDa.GetCompanyGeneratedBillByBillStatus(companyId);

            return paymentInfo;
        }

        [WebMethod]
        public static List<CompanyPaymentLedgerVwBo> CompanyNonGeneratedBillBySearch(int companyId)
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            List<CompanyPaymentLedgerVwBo> companyBill = new List<CompanyPaymentLedgerVwBo>();

            companyBill = companyDa.CompanyBillBySearch(companyId);

            return companyBill;
        }

        [WebMethod]
        public static List<SupplierPaymentBO> GetSupplierPaymentBySearch(int supplierId, DateTime? dateFrom, DateTime? dateTo)
        {
            PMSupplierDA supplierDA = new PMSupplierDA();
            List<SupplierPaymentBO> paymentInfo = new List<SupplierPaymentBO>();
            paymentInfo = supplierDA.GetSupplierPaymentBySearch(supplierId, dateFrom, dateTo, "Adjustment");


            //GuestCompanyDA companyDa = new GuestCompanyDA();
            //List<CompanyPaymentBO> paymentInfo = new List<CompanyPaymentBO>();
            //paymentInfo = companyDa.GetCompanyPaymentBySearch(companyId, dateFrom, dateTo, "Adjustment");

            return paymentInfo;
        }

        [WebMethod]
        public static SupplierPaymentViewBO FillForm(int editId)
        {
            PMSupplierDA supplierDA = new PMSupplierDA();
            SupplierPaymentViewBO paymentBO = new SupplierPaymentViewBO();

            paymentBO.SupplierPayment = supplierDA.GetSupplierPayment(editId);
            paymentBO.PaymentDetailsInfo = supplierDA.GetSupplierPaymentDetailsByPaymentAndLedger(editId);
            paymentBO.SupplierBill = supplierDA.SupplierBillBySearch(paymentBO.SupplierPayment.SupplierId);
            paymentBO.Supplier = supplierDA.GetPMSupplierInfoById(paymentBO.SupplierPayment.SupplierId);

            return paymentBO;
        }
        
        [WebMethod]
        public static ReturnInfo DeleteSupplierPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMSupplierDA supplierDA = new PMSupplierDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = supplierDA.DeletePayment(paymentId, userInformationBO.UserInfoId);
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
        public static ReturnInfo ApprovedPaymentAdjustment(Int64 paymentId)
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

        //------------------
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