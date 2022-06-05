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

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class frmMemberBillGeneration : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadMember();
                LoadCurrency();
            }
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownNoneValue();
            //ddlCurrency.Items.Insert(0, item);
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
            ddlMemberForSearch.DataValueField = "MemberId";
            ddlMemberForSearch.DataBind();
            ddlMemberForSearch.Items.Insert(0, item);
        }

        [WebMethod]
        public static List<MemberPaymentLedgerVwBo> MemberBillBySearch(int memberId)
        {
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            List<MemberPaymentLedgerVwBo> MemberBill = new List<MemberPaymentLedgerVwBo>();

            MemberBill = MemberDa.MemberBillBySearch(memberId);

            return MemberBill;
        }

        [WebMethod]
        public static ReturnInfo GenerateMemberBill(MemberBillGenerationBO billGeneration, List<MemberBillGenerationDetailsBO> billGenerationDetails,
                      List<MemberBillGenerationDetailsBO> billGenerationDetailsEdited, List<MemberBillGenerationDetailsBO> billGenerationDetailsDeleted)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            Int64 MemberBillId = 0;

            try
            {
                if (billGeneration.MemberBillId == 0)
                {
                    rtninfo.IsSuccess = MemberDa.SaveMemberBillGeneration(billGeneration, billGenerationDetails, out MemberBillId);
                }
                else
                {
                    rtninfo.IsSuccess = MemberDa.UpdateMemberBillGeneration(billGeneration, billGenerationDetails, billGenerationDetailsEdited, billGenerationDetailsDeleted);
                    MemberBillId = billGeneration.MemberBillId;
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
                rtninfo.Pk = MemberBillId;

                if (billGeneration.MemberBillId == 0)
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                else
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }

            return rtninfo;
        }

        [WebMethod]
        public static List<MemberBillGenerationBO> GetMemberBillGenerationBySearch(DateTime? dateFrom, DateTime? dateTo, int MemberId)
        {
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            List<MemberBillGenerationBO> paymentInfo = new List<MemberBillGenerationBO>();
            paymentInfo = MemberDa.GetMemberBillGenerationBySearch(dateFrom, dateTo, MemberId);

            return paymentInfo;
        }

        [WebMethod]
        public static MemberBillGenerationViewBO FillForm(int MemberBillId)
        {
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();
            MemberBillGenerationViewBO paymentBO = new MemberBillGenerationViewBO();

            paymentBO.BillGeneration = MemberDa.GetMemberBillGeneration(MemberBillId); 
            paymentBO.BillGenerationDetails = MemberDa.GetMemberBillGenerationDetails(MemberBillId);
            paymentBO.MemberBill = MemberDa.GetMemberBillForBillGenerationEdit(paymentBO.BillGeneration.MemberId, MemberBillId);

            return paymentBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteMemberBillGeneration(int MemberBillId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            MemMemberBasicDA MemberDa = new MemMemberBasicDA();

            try
            {
                rtninfo.IsSuccess = MemberDa.DeleteMemberBillGeneration(MemberBillId);

                if (rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
    }
}