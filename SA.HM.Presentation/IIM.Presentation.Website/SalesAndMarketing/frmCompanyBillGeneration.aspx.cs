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
    public partial class frmCompanyBillGeneration : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCompany();
                LoadCurrency();
                CheckPermission();
            }
        }
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission ? "1" : "0";
        }
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            companyBOList = companyDA.GetGuestCompanyInfo();

            ddlCompany.DataSource = companyBOList;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlCompany.Items.Insert(0, item);

            ddlCompanyForSearch.DataSource = companyBOList;
            ddlCompanyForSearch.DataTextField = "CompanyName";
            ddlCompanyForSearch.DataValueField = "CompanyId";
            ddlCompanyForSearch.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCompanyForSearch.Items.Insert(0, itemSearch);

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
        }

        [WebMethod]
        public static List<CompanyPaymentLedgerVwBo> CompanyBillBySearch(int companyId)
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            List<CompanyPaymentLedgerVwBo> companyBill = new List<CompanyPaymentLedgerVwBo>();

            companyBill = companyDa.CompanyBillBySearch(companyId);

            return companyBill;
        }

        [WebMethod]
        public static ReturnInfo GenerateCompanyBill(CompanyBillGenerationBO billGeneration, List<CompanyBillGenerationDetailsBO> billGenerationDetails,
                      List<CompanyBillGenerationDetailsBO> billGenerationDetailsEdited, List<CompanyBillGenerationDetailsBO> billGenerationDetailsDeleted)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            Int64 companyBillId = 0;

            try
            {
                if (billGeneration.CompanyBillId == 0)
                {
                    rtninfo.IsSuccess = companyDa.SaveCompanyBillGeneration(billGeneration, billGenerationDetails, out companyBillId);
                }
                else
                {
                    rtninfo.IsSuccess = companyDa.UpdateCompanyBillGeneration(billGeneration, billGenerationDetails, billGenerationDetailsEdited, billGenerationDetailsDeleted);
                    companyBillId = billGeneration.CompanyBillId;
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
                rtninfo.Pk = companyBillId;

                if (billGeneration.CompanyBillId == 0)
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                else
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }

            return rtninfo;
        }

        [WebMethod]
        public static List<CompanyBillGenerationBO> GetCompanyBillGenerationBySearch(DateTime? dateFrom, DateTime? dateTo, int companyId)
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            List<CompanyBillGenerationBO> paymentInfo = new List<CompanyBillGenerationBO>();
            paymentInfo = companyDa.GetCompanyBillGenerationBySearch(dateFrom, dateTo, companyId);

            return paymentInfo;
        }

        [WebMethod]
        public static CompanyBillGenerationViewBO FillForm(int companyBillId)
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            CompanyBillGenerationViewBO paymentBO = new CompanyBillGenerationViewBO();

            paymentBO.BillGeneration = companyDa.GetCompanyBillGeneration(companyBillId);
            paymentBO.BillGenerationDetails = companyDa.GetCompanyBillGenerationDetails(companyBillId);
            paymentBO.CompanyBill = companyDa.GetCompanyBillForBillGenerationEdit(paymentBO.BillGeneration.CompanyId, companyBillId);

            return paymentBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteCompanyBillGeneration(int companyBillId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            HMUtility hmUtility = new HMUtility();
            try
            {
                rtninfo.IsSuccess = companyDa.DeleteCompanyBillGeneration(companyBillId);

                if (rtninfo.IsSuccess)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.CompanyBillGenerationDetails.ToString(), companyBillId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CompanyBillGenerationDetails));
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