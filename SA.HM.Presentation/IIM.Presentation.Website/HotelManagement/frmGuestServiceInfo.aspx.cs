using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.GeneralLedger;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGuestServiceInfo : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            Checkpermission();
            if (!IsPostBack)
            {
                this.IsFrontOfficeIntegrateWithAccounts();
                this.LoadGuestServiceSDCharge();
                this.LoadCurrency();
                string editId = Request.QueryString["editId"];

                LoadCostCentre();
                LoadIncomeAccountHead();

                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }
                lblMessage.Text = string.Empty;

                HotelGuestServiceInfoBO paidServiceBO = new HotelGuestServiceInfoBO();
                HotelGuestServiceInfoDA paidServiceDA = new HotelGuestServiceInfoDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                paidServiceBO.ServiceName = this.txtServiceName.Text;
                paidServiceBO.Description = this.txtDescription.Text;
                paidServiceBO.ServiceType = this.ddlServiceType.SelectedIndex == 1 ? "Daily" : "PerStay";
                if (!string.IsNullOrWhiteSpace(this.txtSellingPriceLocal.Text))
                {
                    paidServiceBO.UnitPriceLocal = Convert.ToDecimal(this.txtSellingPriceLocal.Text);
                }
                if (!string.IsNullOrWhiteSpace(this.txtSellingPriceUsd.Text))
                {
                    paidServiceBO.UnitPriceUsd = Convert.ToDecimal(this.txtSellingPriceUsd.Text);
                }
                paidServiceBO.IsVatEnable = this.ddlIsVatEnable.SelectedIndex == 1 ? true : false;
                paidServiceBO.IsServiceChargeEnable = this.ddlIsServiceChargeEnable.SelectedIndex == 1 ? true : false;
                paidServiceBO.IsCitySDChargeEnable = this.ddlIsSDChargeEnable.SelectedIndex == 1 ? true : false;
                paidServiceBO.IsAdditionalChargeEnable = this.ddlIsAdditionalChargeEnable.SelectedIndex == 1 ? true : false;
                paidServiceBO.IsGeneralService = this.ddlIsGeneralService.SelectedIndex == 1 ? true : false;
                paidServiceBO.IsPaidService = this.ddlIsPaidService.SelectedIndex == 1 ? true : false;
                paidServiceBO.ActiveStat = this.ddlActiveStat.SelectedIndex == 0 ? true : false;
                paidServiceBO.CostCenterId = Convert.ToInt32(this.ddlCostCentre.SelectedItem.Value);
                if (hfIsFrontOfficeIntegrateWithAccounts.Value == "1")
                {
                    paidServiceBO.AccountHeadId = Convert.ToInt32(this.ddlIncomeAccountHead.SelectedItem.Value);
                }
                else
                {
                    paidServiceBO.AccountHeadId = 0;
                }
                paidServiceBO.IsNextDayAchievement = this.ddlIsNextDayAchievement.SelectedValue == "2" ? true : false;

                if (String.IsNullOrWhiteSpace(txtPaidServiceId.Value))
                {
                    int tmpServiceId = 0;
                    paidServiceBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = paidServiceDA.SaveHotelGuestServiceInfo(paidServiceBO, out tmpServiceId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.HotelService.ToString(), tmpServiceId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelService));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    paidServiceBO.ServiceId = Convert.ToInt32(txtPaidServiceId.Value);
                    paidServiceBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = paidServiceDA.UpdateHotelGuestServiceInfo(paidServiceBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                             EntityTypeEnum.EntityType.HotelService.ToString(), paidServiceBO.ServiceId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelService));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.txtServiceName.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtSellingPriceLocal.Text = string.Empty;
            this.txtSellingPriceUsd.Text = string.Empty;
            this.ddlCostCentre.SelectedIndex = 0;
            this.ddlServiceType.SelectedIndex = 0;
            this.ddlIsVatEnable.SelectedIndex = 0;
            this.ddlIsGeneralService.SelectedIndex = 0;
            this.ddlIsServiceChargeEnable.SelectedIndex = 0;
            this.ddlIsPaidService.SelectedIndex = 0;
            this.ddlActiveStat.SelectedIndex = 0;
        }
        //************************ User Defined Function ********************//
        private void Checkpermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission ? "1" : "0";
            btnSave.Visible = isSavePermission;
        }
        private void IsFrontOfficeIntegrateWithAccounts()
        {
            hfIsFrontOfficeIntegrateWithAccounts.Value = "0";
            IncomeAccountHeadDiv.Visible = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isFrontOfficeIntegrateWithAccountsBO = new HMCommonSetupBO();
            isFrontOfficeIntegrateWithAccountsBO = commonSetupDA.GetCommonConfigurationInfo("IsFrontOfficeIntegrateWithAccounts", "IsFrontOfficeIntegrateWithAccounts");
            if (isFrontOfficeIntegrateWithAccountsBO != null)
            {
                if (isFrontOfficeIntegrateWithAccountsBO.SetupValue == "1")
                {
                    hfIsFrontOfficeIntegrateWithAccounts.Value = "1";
                    IncomeAccountHeadDiv.Visible = true;
                }
            }
        }
        private void LoadGuestServiceSDCharge()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO GuestServiceSDChargeBO = new HMCommonSetupBO();
            GuestServiceSDChargeBO = commonSetupDA.GetCommonConfigurationInfo("GuestServiceSDCharge", "GuestServiceSDCharge");
            if (!string.IsNullOrEmpty(GuestServiceSDChargeBO.SetupValue))
            {
                lblSDCharge.Text = GuestServiceSDChargeBO.Description.ToString() + " Enable";
            }
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA commonDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> localfields = new List<CommonCurrencyBO>();
            List<CommonCurrencyBO> usdfields = new List<CommonCurrencyBO>();
            localfields = commonDA.GetConversionHeadInfoByType("Local");
            usdfields = commonDA.GetConversionHeadInfoByType("Usd");

            this.ddlSellingPriceLocal.DataSource = localfields;
            this.ddlSellingPriceLocal.DataTextField = "CurrencyName";
            this.ddlSellingPriceLocal.DataValueField = "CurrencyId";
            this.ddlSellingPriceLocal.DataBind();
            this.ddlSellingPriceLocal.SelectedIndex = 0;
            this.lblSellingPriceLocal.Text = "Unit Price (" + this.ddlSellingPriceLocal.SelectedItem.Text + ")";


            this.ddlSellingPriceUsd.DataSource = usdfields;
            this.ddlSellingPriceUsd.DataTextField = "CurrencyName";
            this.ddlSellingPriceUsd.DataValueField = "CurrencyId";
            this.ddlSellingPriceUsd.DataBind();
            this.ddlSellingPriceUsd.SelectedIndex = 0;
            this.lblSellingPriceUsd.Text = "Unit Price (" + this.ddlSellingPriceUsd.SelectedItem.Text + ")";
        }
        private void LoadIncomeAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();

            this.ddlIncomeAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(3);
            this.ddlIncomeAccountHead.DataTextField = "HeadWithCode";
            this.ddlIncomeAccountHead.DataValueField = "NodeId";
            this.ddlIncomeAccountHead.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlIncomeAccountHead.Items.Insert(0, item);
        }
        private void LoadCostCentre()
        {
            CostCentreTabDA apprMarksIndDA = new CostCentreTabDA();
            this.ddlCostCentre.DataSource = apprMarksIndDA.GetAllCostCentreTabInfo();
            this.ddlCostCentre.DataTextField = "CostCenter";
            this.ddlCostCentre.DataValueField = "CostCenterId";
            this.ddlCostCentre.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCentre.Items.Insert(0, item);
        }
        public void FillForm(int editId)
        {
            HotelGuestServiceInfoBO paidServiceBO = new HotelGuestServiceInfoBO();
            HotelGuestServiceInfoDA paidServiceDA = new HotelGuestServiceInfoDA();

            paidServiceBO = paidServiceDA.GetHotelGuestServiceInfoById(editId);
            ddlCostCentre.SelectedValue = paidServiceBO.CostCenterId.ToString();

            txtServiceName.Text = paidServiceBO.ServiceName.ToString();
            txtDescription.Text = paidServiceBO.Description.ToString();
            ddlServiceType.SelectedValue = paidServiceBO.ServiceType.ToString();
            txtSellingPriceLocal.Text = paidServiceBO.UnitPriceLocal.ToString();
            txtSellingPriceUsd.Text = paidServiceBO.UnitPriceUsd.ToString();
            ddlActiveStat.SelectedValue = (paidServiceBO.ActiveStat == true ? 0 : 1).ToString();
            ddlIsVatEnable.SelectedValue = (paidServiceBO.IsVatEnable == true ? 1 : 2).ToString();
            ddlIsGeneralService.SelectedValue = (paidServiceBO.IsGeneralService == true ? 1 : 2).ToString();
            ddlIsServiceChargeEnable.SelectedValue = (paidServiceBO.IsServiceChargeEnable == true ? 1 : 2).ToString();
            ddlIsSDChargeEnable.SelectedValue = (paidServiceBO.IsCitySDChargeEnable == true ? 1 : 2).ToString();
            ddlIsAdditionalChargeEnable.SelectedValue = (paidServiceBO.IsAdditionalChargeEnable == true ? 1 : 2).ToString();
            ddlIsPaidService.SelectedValue = (paidServiceBO.IsPaidService == true ? 1 : 2).ToString();
            ddlIsNextDayAchievement.SelectedValue = (paidServiceBO.IsNextDayAchievement == true ? 2 : 1).ToString();
            if (hfIsFrontOfficeIntegrateWithAccounts.Value == "1")
            {
                ddlIncomeAccountHead.SelectedValue = paidServiceBO.AccountHeadId.ToString();
            }
            txtPaidServiceId.Value = paidServiceBO.ServiceId.ToString();
            btnSave.Text = "Update";
            btnSave.Visible = isUpdatePermission;
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (txtServiceName.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Name.", AlertType.Warning);
                flag = false;
                txtServiceName.Focus();
            }
            else if (ddlIncomeAccountHead.SelectedIndex == 0)
            {
                if (hfIsFrontOfficeIntegrateWithAccounts.Value == "1")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Accounts Head.", AlertType.Warning);
                    flag = false;
                    txtServiceName.Focus();
                }
            }
            else if (ddlServiceType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Type.", AlertType.Warning);
                flag = false;
                ddlServiceType.Focus();
            }
            else if (ddlCostCentre.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Cost Center.", AlertType.Warning);
                flag = false;
                ddlCostCentre.Focus();
            }
            else if (ddlIsVatEnable.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Vat Enable.", AlertType.Warning);
                flag = false;
                ddlIsVatEnable.Focus();
            }
            else if (ddlIsGeneralService.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "General Service.", AlertType.Warning);
                flag = false;
                ddlIsGeneralService.Focus();
            }
            else if (ddlIsServiceChargeEnable.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Charge Enable.", AlertType.Warning);
                flag = false;
                ddlIsServiceChargeEnable.Focus();
            }
            else if (ddlIsSDChargeEnable.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + lblSDCharge.Text + ".", AlertType.Warning);
                flag = false;
                ddlIsSDChargeEnable.Focus();
            }
            else if (ddlIsPaidService.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Paid Service.", AlertType.Warning);
                flag = false;
                ddlIsPaidService.Focus();
            }
            else if (ddlIsNextDayAchievement.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Is Next Day Achieve.", AlertType.Warning);
                flag = false;
                ddlIsNextDayAchievement.Focus();
            }
            return flag;
        }
        private void Cancel()
        {
            this.btnSave.Text = "Save";
            Checkpermission();
            this.txtServiceName.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtSellingPriceLocal.Text = string.Empty;
            this.txtSellingPriceUsd.Text = string.Empty;
            this.ddlCostCentre.SelectedIndex = 0;
            this.ddlIncomeAccountHead.SelectedIndex = 0;
            this.ddlServiceType.SelectedIndex = 0;
            this.ddlIsVatEnable.SelectedIndex = 0;
            this.ddlIsServiceChargeEnable.SelectedIndex = 0;
            this.ddlIsSDChargeEnable.SelectedIndex = 0;
            this.ddlIsAdditionalChargeEnable.SelectedIndex = 0;
            this.ddlIsGeneralService.SelectedIndex = 0;
            this.ddlIsPaidService.SelectedIndex = 0;
            this.ddlActiveStat.SelectedIndex = 0;
            this.ddlIsNextDayAchievement.SelectedIndex = 0;
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static GridViewDataNPaging<HotelGuestServiceInfoBO, GridPaging> SearchPaidServiceAndLoadGridInformation(string serviceName, string serviceType, string activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<HotelGuestServiceInfoBO, GridPaging> myGridData = new GridViewDataNPaging<HotelGuestServiceInfoBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            HotelGuestServiceInfoDA paidServiceDA = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> paidServiceList = new List<HotelGuestServiceInfoBO>();
            paidServiceList = paidServiceDA.GetPaidServiceInfoBySearchCriteriaForPagination(serviceName, serviceType, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<HotelGuestServiceInfoBO> distinctItems = new List<HotelGuestServiceInfoBO>();
            distinctItems = paidServiceList.GroupBy(test => test.ServiceId).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeletePaidServiceById(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HotelGuestServiceInfoDA paidServiceDA = new HotelGuestServiceInfoDA();
                Boolean status = paidServiceDA.DeletePaidServiceById(sEmpId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                             EntityTypeEnum.EntityType.HotelService.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelService));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
    }
}