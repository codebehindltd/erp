using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class frmMemberBillReceive : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected int LocalCurrencyId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCurrency();
                LoadAdjustmentEquivalantHead();
                LoadPaymentMode();
                LoadMember();
                LoadCommonDropDownHiddenField();
                LoadBank();
                LoadCashAccountHead();
                CheckPermission();
            }
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

            ddlBankPayment.DataSource = entityBOList;
            ddlBankPayment.DataTextField = "HeadWithCode";
            ddlBankPayment.DataValueField = "NodeId";
            ddlBankPayment.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlBankId.Items.Insert(0, itemBank);
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
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission ? "1" : "0";

        }
        private void LoadMember()
        {
            MemMemberBasicDA memberda = new MemMemberBasicDA();
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();
            memberList = memberda.GetMemMemberList();

            ddlMember.DataSource = memberList;
            ddlMember.DataTextField = "FullName";
            ddlMember.DataValueField = "MemberId";
            ddlMember.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlMember.Items.Insert(0, item);

            ddlMemberForSearch.DataSource = memberList;
            ddlMemberForSearch.DataTextField = "FullName";
            ddlMemberForSearch.DataValueField = "CompanyId";
            ddlMemberForSearch.DataBind();
            ddlMemberForSearch.Items.Insert(0, item);
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

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlCurrency.Items.Insert(0, item);
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

            List<NodeMatrixBO> entityExpenditureBOList = new List<NodeMatrixBO>();
            entityExpenditureBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityExpenditureBOList != null)
            {
                entityBOList.AddRange(entityExpenditureBOList);
            }

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
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();
            LocalCurrencyId = commonCurrencyBO.CurrencyId;
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        [WebMethod]
        public static MemberPaymentViewBO FillForm(Int64 paymentId)
        {
            MemMemberBasicDA memberDA = new MemMemberBasicDA();
            MemberPaymentViewBO paymentBO = new MemberPaymentViewBO();

            paymentBO.MemberPayment = memberDA.GetMemberPayment(paymentId);
            paymentBO.MemberPaymentDetails = memberDA.GetMemberPaymentDetails(paymentId);
            paymentBO.Member = memberDA.GetMemberInfoById(paymentBO.MemberPayment.MemberId);

            return paymentBO;
        }
        [WebMethod]
        public static ReturnInfo SaveMemberBillPayment(MemberPaymentBO MemberPayment, List<MemberPaymentDetailsBO> MemberPaymentDetails,
                                                       List<MemberPaymentDetailsBO> MemberPaymentDetailsEdited, List<MemberPaymentDetailsBO> MemberPaymentDetailsDeleted)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();

            try
            {
                if (MemberPayment.PaymentId == 0)
                {
                    rtninfo.IsSuccess = MemberDa.SaveMemberBillPayment(MemberPayment, MemberPaymentDetails);
                }
                else
                {
                    rtninfo.IsSuccess = MemberDa.UpdateMemberBillPayment(MemberPayment, MemberPaymentDetails, MemberPaymentDetailsEdited, MemberPaymentDetailsDeleted);
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

                if (MemberPayment.PaymentId == 0)
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
        public static List<MemberBillGenerationBO> MemberBillBySearch(int MemberBillId, int MemberId)
        {
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            List<MemberBillGenerationBO> MemberBill = new List<MemberBillGenerationBO>();
            MemberBill = MemberDa.GetMemberBillForBillReceive(MemberId, MemberBillId);

            return MemberBill;
        }

        [WebMethod]
        public static MemMemberBasicsBO LoadMemberInfo(int MemberId)
        {
            MemMemberBasicDA MemberDA = new MemMemberBasicDA();
            return MemberDA.GetMemberInfoById(MemberId);
        }

        [WebMethod]
        public static List<MemberBillGenerationBO> GetMemberGeneratedBillByBillStatus(int MemberId)
        {
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            List<MemberBillGenerationBO> paymentInfo = new List<MemberBillGenerationBO>();

            paymentInfo = MemberDa.GetMemberGeneratedBillByBillStatus(MemberId);

            return paymentInfo;
        }

        [WebMethod]
        public static List<MemberPaymentBO> GetMemberPaymentBySearch(int MemberId, DateTime? dateFrom, DateTime? dateTo)
        {
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            List<MemberPaymentBO> paymentInfo = new List<MemberPaymentBO>();
            paymentInfo = MemberDa.GetMemberPaymentBySearch(MemberId, dateFrom, dateTo, "Payment");

            return paymentInfo;
        }

        [WebMethod]
        public static ReturnInfo ApprovedPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            MemMemberBasicDA memberDa = new MemMemberBasicDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = memberDa.ApprovedPayment(paymentId, userInformationBO.UserInfoId);
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
        public static ReturnInfo DeleteMemberPayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = MemberDa.DeletePayment(paymentId, userInformationBO.UserInfoId);
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
    }
}