using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmPOSConfiguration : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadServiceBillConfiguration();
                this.LoadDiscountClassification();
                LoadRetuarantConfiguration();
                LoadBillingConfiguration();


            }
            CheckObjectPermission();
        }
        private void CheckObjectPermission()
        {
            btnPercentageDiscountCategory.Visible = isSavePermission;
            btnServiceBillCon.Visible = isSavePermission;
        }
        protected void btnServiceBillCon_Click(object sender, EventArgs e)
        {

            int tmpSetupId = 0;
            int isUpdate = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO.SetupId = 0;
            if (!string.IsNullOrEmpty(this.txtGuestServiceVatId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestServiceVatId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Vat Amount Setup";
            commonSetupBO.TypeName = "GuestServiceVat";
            commonSetupBO.SetupValue = txtGuestServiceVat.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }


            commonSetupBO.SetupId = 0;
            if (!string.IsNullOrEmpty(this.txtGuestServiceServiceChargeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestServiceServiceChargeId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Service Charge Setup";
            commonSetupBO.TypeName = "GuestServiceServiceCharge";
            commonSetupBO.SetupValue = txtGuestServiceServiceCharge.Text;
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            commonSetupBO.SetupId = 0;
            if (!string.IsNullOrEmpty(this.chkInclusiveGuestServiceBillId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.chkInclusiveGuestServiceBillId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Inclusive Guest Service Bill Setup";
            commonSetupBO.TypeName = "InclusiveGuestServiceBill";
            if (chkInclusiveGuestServiceBill.Checked == false)
            {
                commonSetupBO.SetupValue = "0";
            }
            else
            {
                commonSetupBO.SetupValue = "1";
            }
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            commonSetupBO.SetupId = 0;
            if (!string.IsNullOrEmpty(this.hfChkIsRackRatCalculation.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfChkIsRackRatCalculation.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "IsRackRatCalculationForVarServiceCharge";
            commonSetupBO.TypeName = "IsRackRatCalculationForVarServiceCharge";
            if (ChkIsRackRatCalculation.Checked == false)
            {
                commonSetupBO.SetupValue = "0";
            }
            else
            {
                commonSetupBO.SetupValue = "1";
            }
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (isUpdate == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Service Bill Information Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.ServicBillConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServicBillConfiguration));
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Service Bill Information Saved Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.ServicBillConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServicBillConfiguration));
                this.btnServiceBillCon.Text = "Update";
            }
            LoadServiceBillConfiguration();
        }
        private void LoadServiceBillConfiguration()
        {
            HMCommonSetupBO commonSetupBOVat = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOCharge = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOInclusive = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOIsRackRatCalculation = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOVat = commonSetupDA.GetCommonConfigurationInfo("GuestServiceVat", "Vat Amount Setup");
            commonSetupBOCharge = commonSetupDA.GetCommonConfigurationInfo("GuestServiceServiceCharge", "Service Charge Setup");
            commonSetupBOInclusive = commonSetupDA.GetCommonConfigurationInfo("InclusiveGuestServiceBill", "Inclusive Guest Service Bill Setup");
            commonSetupBOIsRackRatCalculation = commonSetupDA.GetCommonConfigurationInfo("IsRackRatCalculationForVarServiceCharge", "IsRackRatCalculationForVarServiceCharge");

            if (!string.IsNullOrEmpty(commonSetupBOVat.SetupValue))
            {
                txtGuestServiceVatId.Value = commonSetupBOVat.SetupId.ToString();
                txtGuestServiceVat.Text = commonSetupBOVat.SetupValue.ToString();
                btnServiceBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOCharge.SetupValue))
            {
                txtGuestServiceServiceChargeId.Value = commonSetupBOCharge.SetupId.ToString();
                txtGuestServiceServiceCharge.Text = commonSetupBOCharge.SetupValue.ToString();
                btnServiceBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOInclusive.SetupValue))
            {
                chkInclusiveGuestServiceBillId.Value = commonSetupBOInclusive.SetupId.ToString();
                btnServiceBillCon.Text = "Update";
                if (commonSetupBOInclusive.SetupValue == "0")
                {
                    chkInclusiveGuestServiceBill.Checked = false;
                }
                else
                {
                    chkInclusiveGuestServiceBill.Checked = true;
                }
            }
            if (!string.IsNullOrEmpty(commonSetupBOIsRackRatCalculation.SetupValue))
            {
                hfChkIsRackRatCalculation.Value = commonSetupBOIsRackRatCalculation.SetupId.ToString();
                btnServiceBillCon.Text = "Update";
                if (commonSetupBOIsRackRatCalculation.SetupValue == "0")
                {
                    ChkIsRackRatCalculation.Checked = false;
                }
                else
                {
                    ChkIsRackRatCalculation.Checked = true;
                }
            }
        }
        private void LoadDiscountClassification()
        {
            InvCategoryDA invCategoryDA = new InvCategoryDA();
            List<InvCategoryBO> invCategoryLst = new List<InvCategoryBO>();
            invCategoryLst = invCategoryDA.GetAllInvItemClassificationInfo();

            this.gvPercentageDiscountCategory.DataSource = invCategoryLst;
            this.gvPercentageDiscountCategory.DataBind();
        }
        private void LoadRetuarantConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantComplementaryFoodCost", "RestaurantComplementaryFoodCost");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfRestaurantComplementaryFoodCost.Value = setUpBO.SetupId.ToString();
                RestaurantComplementaryFoodCost.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("HowMuchMoneySpendToGetOnePoint", "HowMuchMoneySpendToGetOnePoint");


            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfHowMuchMoneySpendToGetOnePoint.Value = setUpBO.SetupId.ToString();
                HowMuchMoneySpendToGetOnePoint.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("HowMuchPointNeedToGetOneMoney", "HowMuchPointNeedToGetOneMoney");


            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfHowMuchPointNeedToGetOneMoney.Value = setUpBO.SetupId.ToString();
                HowMuchPointNeedToGetOneMoney.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("POSRefundConfiguration", "POSRefundConfiguration");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfPOSRefundConfiguration.Value = setUpBO.SetupId.ToString();
                POSRefundConfiguration.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfRestaurantBillPrintAndPreview.Value = setUpBO.SetupId.ToString();
                RestaurantBillPrintAndPreview.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("KotPrintAndPreview", "KotPrintAndPreview");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfKotPrintAndPreview.Value = setUpBO.SetupId.ToString();
                KotPrintAndPreview.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantKotContinueWithDiferentWaiter", "IsRestaurantKotContinueWithDiferentWaiter");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantKotContinueWithDiferentWaiter.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantKotContinueWithDiferentWaiter.Checked = false;
                else
                    IsRestaurantKotContinueWithDiferentWaiter.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantBillAmountWillRound.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantBillAmountWillRound.Checked = false;
                else
                    IsRestaurantBillAmountWillRound.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillClassificationWiseDevideForFOInvoice", "IsRestaurantBillClassificationWiseDevideForFOInvoice");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantBillClassificationWiseDevideForFOInvoice.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantBillClassificationWiseDevideForFOInvoice.Checked = false;
                else
                    IsRestaurantBillClassificationWiseDevideForFOInvoice.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantPaxConfirmationEnable", "IsRestaurantPaxConfirmationEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantPaxConfirmationEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantPaxConfirmationEnable.Checked = false;
                else
                    IsRestaurantPaxConfirmationEnable.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantWaiterConfirmationEnable", "IsRestaurantWaiterConfirmationEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantWaiterConfirmationEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantWaiterConfirmationEnable.Checked = false;
                else
                    IsRestaurantWaiterConfirmationEnable.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantItemImageEnable", "IsRestaurantItemImageEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantItemImageEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantItemImageEnable.Checked = false;
                else
                    IsRestaurantItemImageEnable.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsWaterMarkImageDisplayEnableInRestaurant", "IsWaterMarkImageDisplayEnableInRestaurant");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsWaterMarkImageDisplayEnableInRestaurant.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsWaterMarkImageDisplayEnableInRestaurant.Checked = false;
                else
                    IsWaterMarkImageDisplayEnableInRestaurant.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCompanyNameShowOnRestaurantInvoice.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCompanyNameShowOnRestaurantInvoice.Checked = false;
                else
                    IsCompanyNameShowOnRestaurantInvoice.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithGuestCompany", "IsRestaurantIntegrateWithGuestCompany");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantIntegrateWithGuestCompany.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantIntegrateWithGuestCompany.Checked = false;
                else
                    IsRestaurantIntegrateWithGuestCompany.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithMembership", "IsRestaurantIntegrateWithMembership");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantIntegrateWithMembership.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantIntegrateWithMembership.Checked = false;
                else
                    IsRestaurantIntegrateWithMembership.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantIntegrateWithFrontOffice.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantIntegrateWithFrontOffice.Checked = false;
                else
                    IsRestaurantIntegrateWithFrontOffice.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithPayroll", "IsRestaurantIntegrateWithPayroll");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantIntegrateWithPayroll.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantIntegrateWithPayroll.Checked = false;
                else
                    IsRestaurantIntegrateWithPayroll.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsMembershipPaymentEnable", "IsMembershipPaymentEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsMembershipPaymentEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsMembershipPaymentEnable.Checked = false;
                else
                    IsMembershipPaymentEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillContinueWithoutKotSubmit", "IsRestaurantBillContinueWithoutKotSubmit");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantBillContinueWithoutKotSubmit.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantBillContinueWithoutKotSubmit.Checked = false;
                else
                    IsRestaurantBillContinueWithoutKotSubmit.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantOrderSubmitDisable", "IsRestaurantOrderSubmitDisable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantOrderSubmitDisable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantOrderSubmitDisable.Checked = false;
                else
                    IsRestaurantOrderSubmitDisable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantTokenInfoDisable", "IsRestaurantTokenInfoDisable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantTokenInfoDisable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantTokenInfoDisable.Checked = false;
                else
                    IsRestaurantTokenInfoDisable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRecipeIncludedInInventory", "IsRecipeIncludedInInventory");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRecipeIncludedInInventory.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRecipeIncludedInInventory.Checked = false;
                else
                    IsRecipeIncludedInInventory.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsItemNameDisplayInRestaurantOrderScreen", "IsItemNameDisplayInRestaurantOrderScreen");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemNameDisplayInRestaurantOrderScreen.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemNameDisplayInRestaurantOrderScreen.Checked = false;
                else
                    IsItemNameDisplayInRestaurantOrderScreen.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsItemNameAutoSearchWithCode", "IsItemNameAutoSearchWithCode");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemNameAutoSearchWithCode.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemNameAutoSearchWithCode.Checked = false;
                else
                    IsItemNameAutoSearchWithCode.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsGuestNameAndRoomNoTextShowInInvoice.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsGuestNameAndRoomNoTextShowInInvoice.Checked = false;
                else
                    IsGuestNameAndRoomNoTextShowInInvoice.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPredefinedRemarksEnable", "IsPredefinedRemarksEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPredefinedRemarksEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPredefinedRemarksEnable.Checked = false;
                else
                    IsPredefinedRemarksEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPOSIntegrateWithAccounts", "IsPOSIntegrateWithAccounts");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPOSIntegrateWithAccounts.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPOSIntegrateWithAccounts.Checked = false;
                else
                    IsPOSIntegrateWithAccounts.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsFoodNBeverageSalesRelatedDataHide", "IsFoodNBeverageSalesRelatedDataHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsFoodNBeverageSalesRelatedDataHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsFoodNBeverageSalesRelatedDataHide.Checked = false;
                else
                    IsFoodNBeverageSalesRelatedDataHide.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantReportRestrictionForAllUser", "IsRestaurantReportRestrictionForAllUser");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRestaurantReportRestrictionForAllUser.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRestaurantReportRestrictionForAllUser.Checked = false;
                else
                    IsRestaurantReportRestrictionForAllUser.Checked = true;
            }

            btnSaveConfig.Visible = isUpdatePermission;
            btnSaveConfig.Text = "Update";
        }
        
        private void UpdateConfiguration(Control control,int UserInfoId)
        {
            int tmpSetupId = 0;
            string hiddenField = "hf"+ control.ID;

            Boolean status = false;
            Control hiddenControl = restuarantConfigPanel.FindControl(hiddenField);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            
            if (control!=null && hiddenControl!=null)
            {
                commonSetupBO.SetupId = 0;
                if (control is TextBox)
                {
                    commonSetupBO.SetupValue = ((TextBox)control).Text;
                }
                else if(control is CheckBox)
                {
                    if (((CheckBox)control).Checked)
                        commonSetupBO.SetupValue = "1";
                    else
                        commonSetupBO.SetupValue = "0";
                }
                else if(control is DropDownList)
                {
                    commonSetupBO.SetupValue = ((DropDownList)control).SelectedValue;
                }
                if (!string.IsNullOrEmpty(((HiddenField)hiddenControl).Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(((HiddenField)hiddenControl).Value);
                }

                commonSetupBO.CreatedBy = UserInfoId;
                commonSetupBO.LastModifiedBy = UserInfoId;
                commonSetupBO.SetupName = control.ID;
                commonSetupBO.TypeName = control.ID;

                try
                {
                    status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                    if (status)
                    {
                        if (commonSetupBO.SetupId > 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Restuarent Configuration Updated Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Restuarent Configuration Saved Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                            this.btnServiceBillCon.Text = "Update";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        
        protected void btnPercentageDiscountCategory_Click(object sender, EventArgs e)
        {
            int counter = 0;
            //List<InvCategoryBO> discountCategoryBOList = new List<InvCategoryBO>();

            string discountCategoryBOList = string.Empty;

            foreach (GridViewRow row in gvPercentageDiscountCategory.Rows)
            {
                counter = counter + 1;
                bool isChecked = ((CheckBox)row.FindControl("chkIsCheckedPermission")).Checked;
                Label lblidValue = (Label)row.FindControl("lblid");

                InvCategoryBO discountCategoryBO = new InvCategoryBO();

                if (isChecked)
                {
                    discountCategoryBO.CategoryId = Convert.ToInt32(lblidValue.Text);
                    //discountCategoryBOList.Add(discountCategoryBO);

                    if (string.IsNullOrWhiteSpace(discountCategoryBOList))
                    {
                        discountCategoryBOList = discountCategoryBO.CategoryId.ToString();
                    }
                    else
                    {
                        discountCategoryBOList = discountCategoryBOList + "," + discountCategoryBO.CategoryId.ToString();
                    }
                }
            }


            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int createdBy = userInformationBO.CreatedBy;

            Boolean status = commonSetupDA.SaveOrUpdateInvItemClassificationConfiguration(discountCategoryBOList, createdBy);
            if (status)
            {
                //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Default Discount Classification Saved Successfully.";
                CommonHelper.AlertInfo(innboardMessage, "Default Discount Classification Saved Successfully.", AlertType.Success);
                //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                //EntityTypeEnum.EntityType.ServicBillConfiguration.ToString(), commonSetupBO.SetupId,
                //ProjectModuleEnum.ProjectModule.HotelManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServicBillConfiguration));
            }
            //else
            //{
            //    //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Error);
            //    this.isMessageBoxEnable = 2;
            //    lblMessage.Text = "Service Bill Information Saved Successfully";
            //    //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Save.ToString(),
            //    //EntityTypeEnum.EntityType.ServicBillConfiguration.ToString(), tmpSetupId,
            //    //ProjectModuleEnum.ProjectModule.HotelManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServicBillConfiguration));
            //    //this.btnServiceBillCon.Text = "Update";
            //}
        }

        protected void gvPercentageDiscountCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblStatusValue = (Label)e.Row.FindControl("lblStatus");
                if (lblStatusValue.Text == "False")
                {
                    ((CheckBox)e.Row.FindControl("chkIsCheckedPermission")).Checked = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkIsCheckedPermission")).Checked = true;
                }
            }
        }
        //id name and hiddenfield name should be simillar for dynamic save.
        protected void btnSaveConfig_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            
            foreach (Control item in restuarantConfigPanel.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            LoadRetuarantConfiguration();
        }

        protected void btnSaveConfigForBilling_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Remarks Details For Billing------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfRemarksDetails.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfRemarksDetails.Value);
            }
            commonSetupBO.FieldType = "RemarksDetailsForBilling";
            commonSetupBO.FieldValue = txtRemarksDetails.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean remarksDetailsForBillingStatus = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            // //-----Support Remarks Details For Billing------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfSTRemarksDetails.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfSTRemarksDetails.Value);
            }
            commonSetupBO.FieldType = "RemarksDetailsForSTBilling";
            commonSetupBO.FieldValue = txtSTRemarksDetails.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean stRemarksDetailsForBillingStatus = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            // //-----Bill Declaration For Billing------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfBillDeclaration.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfBillDeclaration.Value);
            }
            commonSetupBO.FieldType = "BillDeclaration";
            commonSetupBO.FieldValue = txtBillDeclaration.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean billDeclarationForBillingStatus = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            foreach (Control item in billingConfigPanel.Controls)
            {
                UpdateConfigurationForBilling(item, userInformationBO.UserInfoId);
            }
            LoadBillingConfiguration();

        }
        private void UpdateConfigurationForBilling(Control control, int UserInfoId)
        {
            int tmpSetupId = 0;
            string hiddenField = "hf" + control.ID;

            Boolean status = false;
            Control hiddenControl = billingConfigPanel.FindControl(hiddenField);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            if (control != null && hiddenControl != null )
            {
                commonSetupBO.SetupId = 0;
                if (control is TextBox)
                {
                    commonSetupBO.SetupValue = ((TextBox)control).Text;
                }
                else if (control is CheckBox)
                {
                    if (((CheckBox)control).Checked)
                        commonSetupBO.SetupValue = "1";
                    else
                        commonSetupBO.SetupValue = "0";
                }
                else if (control is DropDownList)
                {
                    commonSetupBO.SetupValue = ((DropDownList)control).SelectedValue;
                }
                if (!string.IsNullOrEmpty(((HiddenField)hiddenControl).Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(((HiddenField)hiddenControl).Value);
                }

                commonSetupBO.CreatedBy = UserInfoId;
                commonSetupBO.LastModifiedBy = UserInfoId;
                commonSetupBO.SetupName = control.ID;
                commonSetupBO.TypeName = control.ID;

                try
                {
                    status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                    if (status)
                    {
                        if (commonSetupBO.SetupId > 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Billing Configuration Updated Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Billing Configuration Saved Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                            this.btnServiceBillCon.Text = "Update";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void LoadBillingConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsItemCodeHideForBilling", "IsItemCodeHideForBilling");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemCodeHideForBilling.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemCodeHideForBilling.Checked = false;
                else
                    IsItemCodeHideForBilling.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsStockHideForBilling", "IsStockHideForBilling");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsStockHideForBilling.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsStockHideForBilling.Checked = false;
                else
                    IsStockHideForBilling.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsStockByHideForBilling", "IsStockByHideForBilling");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsStockByHideForBilling.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsStockByHideForBilling.Checked = false;
                else
                    IsStockByHideForBilling.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRemarksHideForBilling", "IsRemarksHideForBilling");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRemarksHideForBilling.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRemarksHideForBilling.Checked = false;
                else
                    IsRemarksHideForBilling.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsItemAutoSave", "IsItemAutoSave");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemAutoSave.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemAutoSave.Checked = false;
                else
                    IsItemAutoSave.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsTaskAutoGenarate", "IsTaskAutoGenarate");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsTaskAutoGenarate.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsTaskAutoGenarate.Checked = false;
                else
                    IsTaskAutoGenarate.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCashPaymentShow", "IsCashPaymentShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCashPaymentShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCashPaymentShow.Checked = false;
                else
                    IsCashPaymentShow.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAmexCardPaymentShow", "IsAmexCardPaymentShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsAmexCardPaymentShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsAmexCardPaymentShow.Checked = false;
                else
                    IsAmexCardPaymentShow.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsMasterCardPaymentShow", "IsMasterCardPaymentShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsMasterCardPaymentShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsMasterCardPaymentShow.Checked = false;
                else
                    IsMasterCardPaymentShow.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsVisaCardPaymentShow", "IsVisaCardPaymentShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsVisaCardPaymentShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsVisaCardPaymentShow.Checked = false;
                else
                    IsVisaCardPaymentShow.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDiscoverCardPaymentShow", "IsDiscoverCardPaymentShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDiscoverCardPaymentShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsDiscoverCardPaymentShow.Checked = false;
                else
                    IsDiscoverCardPaymentShow.Checked = true;

            }
            
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCompanyPaymentShow", "IsCompanyPaymentShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCompanyPaymentShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCompanyPaymentShow.Checked = false;
                else
                    IsCompanyPaymentShow.Checked = true;
            }

            

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBillingInvoiceTemplateWithoutHeader", "IsBillingInvoiceTemplateWithoutHeader");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBillingInvoiceTemplateWithoutHeader.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBillingInvoiceTemplateWithoutHeader.Checked = false;
                else
                    IsBillingInvoiceTemplateWithoutHeader.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSubjectShow", "IsSubjectShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSubjectShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsSubjectShow.Checked = false;
                else
                    IsSubjectShow.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRemarkShow", "IsRemarkShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRemarkShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRemarkShow.Checked = false;
                else
                    IsRemarkShow.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAutoCompanyBillGenerationProcessEnable", "IsAutoCompanyBillGenerationProcessEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsAutoCompanyBillGenerationProcessEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsAutoCompanyBillGenerationProcessEnable.Checked = false;
                else
                    IsAutoCompanyBillGenerationProcessEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSDCIntegrationEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsSDCIntegrationEnable.Checked = false;
                else
                    IsSDCIntegrationEnable.Checked = true;
            }

            CustomFieldBO fieldBO = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();

            fieldBO = commonDA.GetCustomFieldByFieldName("RemarksDetailsForBilling");
            if (fieldBO.FieldId!=0)
            {
                hfRemarksDetails.Value = fieldBO.FieldId.ToString();
                txtRemarksDetails.Text = fieldBO.FieldValue;
            }

            fieldBO = commonDA.GetCustomFieldByFieldName("RemarksDetailsForSTBilling");
            if (fieldBO.FieldId != 0)
            {
                hfSTRemarksDetails.Value = fieldBO.FieldId.ToString();
                txtSTRemarksDetails.Text = fieldBO.FieldValue;
            }

            fieldBO = commonDA.GetCustomFieldByFieldName("BillDeclaration");
            if (fieldBO.FieldId != 0)
            {
                hfBillDeclaration.Value = fieldBO.FieldId.ToString();
                txtBillDeclaration.Text = fieldBO.FieldValue;
            }

            btnSaveConfigForBilling.Visible = isUpdatePermission;
            btnSaveConfigForBilling.Text = "Update";
        }
    }
}