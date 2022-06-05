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
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetConfiguration : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadBanquetConfiguration();

            }
            CheckObjectPermission();
        }
        protected void btnBanquetTermsAndConditions_Click(object sender, EventArgs e)
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfBanquetTermsAndConditions.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfBanquetTermsAndConditions.Value);
            }
            commonSetupBO.FieldType = "BanquetTermsAndConditions";
            commonSetupBO.FieldValue = txtBanquetTermsAndConditions.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Banquet Terms and Conditions Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));
            }

            LoadBanquetConfiguration();
        }
        protected void btnSaveBanquetConfig_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<HMCommonSetupBO> setUpBO = new List<HMCommonSetupBO>();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            string typeName = string.Empty;

            setUpBO = commonSetupDA.GetAllCommonConfigurationInfo();

            foreach (Control item in dvFOConfig.Controls)
            {
                typeName = setUpBO.Where(s => s.SetupName == item.ID).Select(s => s.TypeName).FirstOrDefault();
                if (item is TextBox || item is DropDownList || item is CheckBox)
                {
                    UpdateConfiguration(item, userInformationBO.UserInfoId, typeName);
                }
                typeName = string.Empty;
            }
            LoadBanquetConfiguration();
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            //btnPercentageDiscountCategory.Visible = isSavePermission;
            //btnServiceBillCon.Visible = isSavePermission;
        }
        private void LoadBanquetConfiguration()
        {
            CustomFieldBO fieldBO = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();

            fieldBO = commonDA.GetCustomFieldByFieldName("BanquetTermsAndConditions");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfBanquetTermsAndConditions.Value = fieldBO.FieldId.ToString();
                txtBanquetTermsAndConditions.Text = fieldBO.FieldValue;
            }


            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetRateEditableEnable", "IsBanquetRateEditableEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBanquetRateEditableEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBanquetRateEditableEnable.Checked = false;
                else
                    IsBanquetRateEditableEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetRateEditableEnable", "IsBanquetRateEditableEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBanquetRateEditableEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBanquetRateEditableEnable.Checked = false;
                else
                    IsBanquetRateEditableEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetBillAmountWillRound", "IsBanquetBillAmountWillRound");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBanquetBillAmountWillRound.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBanquetBillAmountWillRound.Checked = false;
                else
                    IsBanquetBillAmountWillRound.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetIntegrateWithAccounts", "IsBanquetIntegrateWithAccounts");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBanquetIntegrateWithAccounts.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBanquetIntegrateWithAccounts.Checked = false;
                else
                    IsBanquetIntegrateWithAccounts.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetReservationRestictionForAllUser", "IsBanquetReservationRestictionForAllUser");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBanquetReservationRestictionForAllUser.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBanquetReservationRestictionForAllUser.Checked = false;
                else
                    IsBanquetReservationRestictionForAllUser.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsBanquetReservationEmailAutoPostingEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBanquetReservationEmailAutoPostingEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBanquetReservationEmailAutoPostingEnable.Checked = false;
                else
                    IsBanquetReservationEmailAutoPostingEnable.Checked = true;
            }

        }
        private void UpdateConfiguration(Control control, int UserInfoId, string typeName)
        {
            int tmpSetupId = 0;
            string hiddenField = "hf" + control.ID;

            Boolean status = false;
            Control hiddenControl = dvFOConfig.FindControl(hiddenField);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            CustomFieldBO fieldBO = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();

            if (control != null && hiddenControl != null)
            {
                if (control.ID != "DefaultUsdToLocalCurrencyConversionRate")
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
                    commonSetupBO.TypeName = typeName;
                }
                
                try
                {
                    if (control.ID == "DefaultUsdToLocalCurrencyConversionRate")
                        status = commonDA.SaveOrUpdateCommonCustomFieldData(fieldBO, out tmpSetupId);
                    else
                        status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                    if (status)
                    {
                        if (commonSetupBO.SetupId > 0 || fieldBO.FieldId > 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Banquet Configuration Updated Successfully.", AlertType.Success);
                            if (commonSetupBO.SetupId > 0)
                            {
                                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.FOConfiguration.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FOConfiguration));
                            }
                            else
                            {
                                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.FOConfiguration.ToString(), fieldBO.FieldId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FOConfiguration));
                            }

                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Banquet Configuration Saved Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.FOConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FOConfiguration));
                            this.btnSaveBanquetConfig.Text = "Update";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}