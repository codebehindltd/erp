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
using HotelManagement.Data.UserInformation;
using System.Web.Services;
using System.Collections;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using System.Reflection;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmHotelConfiguration : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        string AllRoomStatusLegent = string.Empty;
        string AllRoomStatusLegentColorBackup = string.Empty;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.LoadHotelBillConfiguration();
                this.LoadServiceBillConfiguration();
                this.LoadCheckOutTimeonfiguration();
                this.LoadCheckInTimeonfiguration();
                this.LoadUserGroup();
                this.LoadDepartmentDropdown();
                this.LoadCountryDropdown();
                this.LoadFrontOfficeCostCenterIdDropdown();
                this.LoadAdvancePaymentAdjustmentAccountsHeadIdDropdown();
                this.LoadHoldUpAccountsPostingAccountReceivableHeadIdDropdown();
                this.LoadFOConfiguration();
                LoadRoomServiceName();
                loadColor();
                GetCustomFieldColor();
            }
        }
        protected void btnHotelBillCon_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            int isUpdate = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.txtGuestHouseVatId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestHouseVatId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Vat Amount Setup";
            commonSetupBO.TypeName = "GuestHouseVat";
            commonSetupBO.SetupValue = txtGuestHouseVat.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Guest House Service Charge Setup------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.txtGuestHouseServiceChargeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestHouseServiceChargeId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Service Charge Setup";
            commonSetupBO.TypeName = "GuestHouseServiceCharge";
            commonSetupBO.SetupValue = txtGuestHouseServiceCharge.Text;
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Guest House City Charge Setup------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.txtGuestHouseCityChargeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestHouseCityChargeId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "GuestHouseCityCharge";
            commonSetupBO.TypeName = "GuestHouseCityCharge";
            commonSetupBO.SetupValue = txtGuestHouseCityCharge.Text;
            commonSetupBO.Description = "City Charge";
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Inclusive HotelManagement Bill Setup------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.chkInclusiveHotelManagementBillId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.chkInclusiveHotelManagementBillId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Inclusive HotelManagement Bill Setup";
            commonSetupBO.TypeName = "InclusiveHotelManagementBill";
            if (chkInclusiveHotelManagementBill.Checked == false)
            {
                commonSetupBO.SetupValue = "0";
            }
            else
            {
                commonSetupBO.SetupValue = "1";
            }
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Guest House Rate Is Plus Plus------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.chkGuestHouseRateIsPlusPlusId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.chkGuestHouseRateIsPlusPlusId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "GuestHouseRateIsPlusPlus";
            commonSetupBO.TypeName = "GuestHouseRateIsPlusPlus";
            if (chkGuestHouseRateIsPlusPlus.Checked == false)
            {
                commonSetupBO.SetupValue = "0";
            }
            else
            {
                commonSetupBO.SetupValue = "1";
            }
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Is Vat Enable On Guest House City Charge------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.cbGuestHouseCityChargeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.cbGuestHouseCityChargeId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "IsVatEnableOnGuestHouseCityCharge";
            commonSetupBO.TypeName = "IsVatEnableOnGuestHouseCityCharge";
            if (chkGuestHouseRateIsPlusPlus.Checked == false)
            {
                commonSetupBO.SetupValue = "0";
            }
            else
            {
                if (cbGuestHouseCityCharge.Checked == false)
                {
                    commonSetupBO.SetupValue = "0";
                }
                else
                {
                    commonSetupBO.SetupValue = "1";
                }
            }
            commonSetupBO.Description = "Is Vat Enable On Guest House City Charge";
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Additional Charge Type Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfFOAdditionalChargeTypeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfFOAdditionalChargeTypeId.Value);
            }
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "FOAdditionalChargeType";
            commonSetupBO.TypeName = "FOAdditionalChargeType";
            commonSetupBO.SetupValue = ddlFOAdditionalChargeType.SelectedValue;
            Boolean statusFOADType = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Additional Charge Amount Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfFOAdditionalChargeAmountId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfFOAdditionalChargeAmountId.Value);
            }
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "FOAdditionalChargeAmount";
            commonSetupBO.TypeName = "FOAdditionalChargeAmount";
            commonSetupBO.SetupValue = txtFOAdditionalChargeAmount.Text;
            Boolean statusFOADAmount = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (isUpdate == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Hotel Bill Information Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));

            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Hotel Bill Information Saved Successfully", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));
                this.btnHotelBillCon.Text = "Update";
            }
            LoadHotelBillConfiguration();

        }
        protected void btnServiceBillCon_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            int isUpdate = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
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

            // //-----Guest House Service Charge Setup------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

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

            // //-----Guest House SD Charge Setup------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.txtGuestServiceSDChargeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestServiceSDChargeId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "GuestServiceSDCharge";
            commonSetupBO.TypeName = "GuestServiceSDCharge";
            commonSetupBO.SetupValue = txtGuestServiceSDCharge.Text;
            commonSetupBO.Description = "SD Charge";
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Inclusive HotelManagement Bill Setup------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

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

            // //-----Guest Service Rate Is Plus Plus Setup------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.chkGuestServiceRateIsPlusPlusId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.chkGuestServiceRateIsPlusPlusId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "GuestServiceRateIsPlusPlus";
            commonSetupBO.TypeName = "GuestServiceRateIsPlusPlus";
            if (chkGuestServiceRateIsPlusPlus.Checked == false)
            {
                commonSetupBO.SetupValue = "0";
            }
            else
            {
                commonSetupBO.SetupValue = "1";
            }
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Is Vat Enable On Guest House City Charge------------------------------------------------------
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.cbGuestServiceSDChargeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.cbGuestServiceSDChargeId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "IsVatEnableOnGuestServiceSDCharge";
            commonSetupBO.TypeName = "IsVatEnableOnGuestServiceSDCharge";
            if (chkGuestServiceRateIsPlusPlus.Checked == false)
            {
                commonSetupBO.SetupValue = "0";
            }
            else
            {
                if (cbGuestServiceSDCharge.Checked == false)
                {
                    commonSetupBO.SetupValue = "0";
                }
                else
                {
                    commonSetupBO.SetupValue = "1";
                }
            }
            commonSetupBO.Description = "Is Vat Enable On Guest Service SD Charge";
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Additional Charge Type Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfSBAdditionalChargeTypeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfSBAdditionalChargeTypeId.Value);
            }
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "SBAdditionalChargeType";
            commonSetupBO.TypeName = "SBAdditionalChargeType";
            commonSetupBO.SetupValue = ddlSBAdditionalChargeType.SelectedValue;
            Boolean statusSBADType = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            // //-----Additional Charge Amount Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfSBAdditionalChargeAmountId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfSBAdditionalChargeAmountId.Value);
            }
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "SBAdditionalChargeAmount";
            commonSetupBO.TypeName = "SBAdditionalChargeAmount";
            commonSetupBO.SetupValue = txtSBAdditionalChargeAmount.Text;
            Boolean statusSBADAmount = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

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
        protected void btnReCheckInHourConfiguration_Click(object sender, EventArgs e)
        {
            int tmpReSetupId = 0;
            int isReUpdate = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.hfReCheckInHourConfigurationId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfReCheckInHourConfigurationId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.TypeName = "InnboartCheckOutCancelHour";
            commonSetupBO.SetupName = "CancelCheckOutHour";
            commonSetupBO.SetupValue = ddlReCheckInHourConfiguration.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpReSetupId);

            if (tmpReSetupId != 0)
            {
                isReUpdate = tmpReSetupId;
            }
            if (isReUpdate == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Re Check In Hour Configuration after Check Out Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));

            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Re Check In Hour Configuration after Check Out Saved Successfully", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), tmpReSetupId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));
                this.btnHotelBillCon.Text = "Update";
            }
            LoadHotelBillConfiguration();

        }
        protected void btnCheckInTime_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            int isUpdate = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtGuestHouseCheckInTimeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestHouseCheckInTimeId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Guest House CheckIn Time Setup";
            commonSetupBO.TypeName = "GuestHouseCheckInTime";
            DateTime thisDay = DateTime.Today;
            //DateTime DateIn = new DateTime();
            //DateIn = hmUtility.GetDateTimeFromString(thisDay.ToString("d"), userInformationBO.ServerDateFormat);

            //if (DateIn < thisDay)
            //{
            //    DateIn = DateTime.Today;
            //}

            //int pMin = !string.IsNullOrWhiteSpace(this.txtProbableInMinute.Text) ? Convert.ToInt32(this.txtProbableInMinute.Text) : 0;
            //int pHour = this.ddlProbableInAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableInHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableInHour.Text) % 12) + 12);
            //DateIn = DateIn.AddHours(pHour).AddMinutes(pMin);
            //commonSetupBO.SetupValue = hmUtility.GetDateTimeStringFromDateTime(DateIn);
            if (!string.IsNullOrWhiteSpace(txtProbableCheckInTime.Text))
            {
                //DateIn = Convert.ToDateTime(thisDay.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtProbableCheckInTime.Text).ToString(userInformationBO.TimeFormat));
                //commonSetupBO.SetupValue = hmUtility.GetDateTimeStringFromDateTime(DateIn);

                commonSetupBO.SetupValue = thisDay.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtProbableCheckInTime.Text).ToString("HH:mm:ss");
            }

            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.txtGuestHouseCheckInExtraHourId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestHouseCheckInExtraHourId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Guest House CheckIn ExtraHour Setup";
            commonSetupBO.TypeName = "GuestHouseCheckInExtraHour";
            commonSetupBO.SetupValue = txtGuestHouseCheckInExtraHour.Text;
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (isUpdate == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Check In Time Configuration Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.CheckInTimeConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CheckInTimeConfiguration));
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Check In Time Configuration Saved Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.CheckInTimeConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CheckInTimeConfiguration));
                this.btnCheckInTime.Text = "Update";
            }
            this.LoadCheckOutTimeonfiguration();
        }
        protected void btnCheckOutTime_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            int isUpdate = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtGuestHouseCheckOutTimeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestHouseCheckOutTimeId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Guest House CheckOut Time Setup";
            commonSetupBO.TypeName = "GuestHouseCheckOutTime";
            DateTime thisDay = DateTime.Today;
            //DateTime DateIn = new DateTime();
            //DateIn = hmUtility.GetDateTimeFromString(thisDay.ToString("d"), userInformationBO.ServerDateFormat);

            //if (DateIn < thisDay)
            //{
            //    DateIn = DateTime.Today;
            //}

            //int pMin = !string.IsNullOrWhiteSpace(this.txtProbableMinute.Text) ? Convert.ToInt32(this.txtProbableMinute.Text) : 0;
            //int pHour = this.ddlProbableAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableHour.Text) % 12) + 12);
            //DateIn = DateIn.AddHours(pHour).AddMinutes(pMin);
            //commonSetupBO.SetupValue = hmUtility.GetDateTimeStringFromDateTime(DateIn);

            if (!string.IsNullOrWhiteSpace(txtProbableCheckOutTime.Text))
            {
                //DateIn = Convert.ToDateTime(thisDay.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtProbableCheckInTime.Text).ToString(userInformationBO.TimeFormat));
                //commonSetupBO.SetupValue = hmUtility.GetDateTimeStringFromDateTime(DateIn);

                commonSetupBO.SetupValue = thisDay.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtProbableCheckOutTime.Text).ToString("HH:mm:ss");
            }

            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (!string.IsNullOrEmpty(this.txtGuestHouseCheckOutExtraHourId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtGuestHouseCheckOutExtraHourId.Value);
            }

            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Guest House CheckOut ExtraHour Setup";
            commonSetupBO.TypeName = "GuestHouseCheckOutExtraHour";
            commonSetupBO.SetupValue = txtGuestHouseCheckOutExtraHour.Text;
            status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId != 0)
            {
                isUpdate = tmpSetupId;
            }

            if (isUpdate == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Check Out Time Configuration Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.CheckOutTimeConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CheckOutTimeConfiguration));
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Check Out Time Configuration Saved Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.CheckOutTimeConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CheckOutTimeConfiguration));
                this.btnCheckOutTime.Text = "Update";
            }
            this.LoadCheckOutTimeonfiguration();
        }
        protected void btnRoomReservationTermsAndConditions_Click(object sender, EventArgs e)
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfRoomReservationTermsAndConditions.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfRoomReservationTermsAndConditions.Value);
            }
            commonSetupBO.FieldType = "RoomReservationTermsAndConditions";
            commonSetupBO.FieldValue = txtRoomReservationTermsAndConditions.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);
            
            Boolean status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);
            
            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Room Reservation Terms and Conditions Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));

            }
            
            LoadHotelBillConfiguration();
        }
        protected void btnRoomRegistrationTermsAndConditions_Click(object sender, EventArgs e)
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfRoomRegistrationTermsAndConditions.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfRoomRegistrationTermsAndConditions.Value);
            }
            commonSetupBO.FieldType = "RoomRegistrationTermsAndConditions";
            commonSetupBO.FieldValue = txtRoomRegistrationTermsAndConditions.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Room Registration Terms and Conditions Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));

            }

            LoadHotelBillConfiguration();
        }
        private void UpdateColorConfiguration(Control control, int UserInfoId)
        {
            string hiddenField = "hf" + control.ID;
            Boolean status = false;
            Control hiddenControl = ColorEntryPanel.FindControl(hiddenField);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();

            if (control != null && hiddenControl != null)
            {
                commonSetupBO.SetupId = 0;
                
                if (control is DropDownList)
                {
                    commonSetupBO.FieldValue = ((DropDownList)control).SelectedValue;

                    if (string.IsNullOrWhiteSpace(AllRoomStatusLegent))
                    {
                        AllRoomStatusLegent = commonSetupBO.FieldValue;
                    }
                    else
                    {
                        AllRoomStatusLegent = AllRoomStatusLegent + "~" + commonSetupBO.FieldValue;
                    }

                    if (string.IsNullOrWhiteSpace(AllRoomStatusLegentColorBackup))
                    {
                        AllRoomStatusLegentColorBackup = commonSetupBO.FieldValue;
                    }
                    else
                    {
                        AllRoomStatusLegentColorBackup = AllRoomStatusLegentColorBackup + "~" + commonSetupBO.FieldValue;
                    }
                }
                if (!string.IsNullOrEmpty(((HiddenField)hiddenControl).Value))
                {
                    commonSetupBO.FieldId = Int32.Parse(((HiddenField)hiddenControl).Value);
                }

                commonSetupBO.CreatedBy = UserInfoId;
                commonSetupBO.LastModifiedBy = UserInfoId;
                commonSetupBO.FieldType = control.ID;                
                commonSetupBO.ActiveStat = true;

                try
                {
                    commonSetupBOList.Add(commonSetupBO);

                    status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);
                    if (status)
                    {
                        if (commonSetupBO.FieldId > 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Color Configuration Updated Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void GetCustomFieldColor()
        {
            CustomFieldBO fieldBO = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomVacantDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomVacantDiv.Value = fieldBO.FieldId.ToString();
                RoomVacantDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomTodaysCheckInDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomTodaysCheckInDiv.Value = fieldBO.FieldId.ToString();
                RoomTodaysCheckInDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomOccupaiedDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomOccupaiedDiv.Value = fieldBO.FieldId.ToString();
                RoomOccupaiedDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomPossibleVacantDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomPossibleVacantDiv.Value = fieldBO.FieldId.ToString();
                RoomPossibleVacantDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomVacantDirtyDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomVacantDirtyDiv.Value = fieldBO.FieldId.ToString();
                RoomVacantDirtyDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomLongStayingDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomLongStayingDiv.Value = fieldBO.FieldId.ToString();
                RoomLongStayingDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomOutOfOrderDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomOutOfOrderDiv.Value = fieldBO.FieldId.ToString();
                RoomOutOfOrderDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomOutOfServiceDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomOutOfServiceDiv.Value = fieldBO.FieldId.ToString();
                RoomOutOfServiceDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("RoomReservedDiv");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomReservedDiv.Value = fieldBO.FieldId.ToString();
                RoomReservedDiv.SelectedValue = fieldBO.FieldValue.ToString();
            }
        }
        protected void btnColorConfigurationUpdate_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (Control item in ColorEntryPanel.Controls)
            {
                UpdateColorConfiguration(item, userInformationBO.UserInfoId);
            }

            SummaryUpdateInfo();
        }
        protected void btnAdditionalServiceMessege_Click(object sender, EventArgs e)
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfparamCLAdditionalServiceMessege.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfparamCLAdditionalServiceMessege.Value);
            }
            commonSetupBO.FieldType = "paramCLAdditionalServiceMessege";
            commonSetupBO.FieldValue = txtparamCLAdditionalServiceMessege.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Additional Service Messege Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));

            }

            LoadHotelBillConfiguration();
        }
        protected void btnReservationCancellationPolicyMessege_Click(object sender, EventArgs e)
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfparamCLCancellationPolicyMessege.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfparamCLCancellationPolicyMessege.Value);
            }
            commonSetupBO.FieldType = "paramCLCancellationPolicyMessege";
            commonSetupBO.FieldValue = txtparamCLCancellationPolicyMessege.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Reservation Cancellation Policy Messege Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));

            }

            LoadHotelBillConfiguration();
        }
        protected void btnAggrimentMessege_Click(object sender, EventArgs e)
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            // //-----Guest House Vat Charge Setup------------------------------------------------------
            if (!string.IsNullOrEmpty(this.hfparamGIAggrimentMessege.Value))
            {
                commonSetupBO.FieldId = Int32.Parse(this.hfparamGIAggrimentMessege.Value);
            }
            commonSetupBO.FieldType = "paramGIAggrimentMessege";
            commonSetupBO.FieldValue = txtparamGIAggrimentMessege.Text;
            commonSetupBO.ActiveStat = true;
            commonSetupBOList.Add(commonSetupBO);

            Boolean status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Aggriment Messege Updated Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.HottelBillConfiguration.ToString(), 0,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HottelBillConfiguration));

            }

            LoadHotelBillConfiguration();
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.UserGroupId == 1)
            {
                //if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
                //{
                //    Response.Redirect("/HMCommon/frmHMHome.aspx");
                //}
                //else
                //{
                    btnHotelBillCon.Visible = isSavePermission;
                    btnServiceBillCon.Visible = isSavePermission;
                    //btnCheckInTime.Visible = isSavePermission;
                    //btnCheckOutTime.Visible = isSavePermission;
                    //btnReCheckInHourConfiguration.Visible = isSavePermission;
                    //btnSaveFOConfig.Visible = isSavePermission;
                //}
            }
        }
        private void loadColor()
        {
            ArrayList ColorList = new ArrayList();
            Type colorType = typeof(System.Drawing.Color);
            PropertyInfo[] propInfoList = colorType.GetProperties(BindingFlags.Static |
                                          BindingFlags.DeclaredOnly | BindingFlags.Public);
            int row = 0;
            foreach (PropertyInfo c in propInfoList)
            {
                RoomVacantDiv.Items.Add(c.Name);
                RoomVacantDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomVacantDiv.Items[row].Value);

                RoomTodaysCheckInDiv.Items.Add(c.Name);
                RoomTodaysCheckInDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomTodaysCheckInDiv.Items[row].Value);

                RoomOccupaiedDiv.Items.Add(c.Name);
                RoomOccupaiedDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomOccupaiedDiv.Items[row].Value);

                RoomPossibleVacantDiv.Items.Add(c.Name);
                RoomPossibleVacantDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomPossibleVacantDiv.Items[row].Value);

                RoomVacantDirtyDiv.Items.Add(c.Name);
                RoomVacantDirtyDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomVacantDirtyDiv.Items[row].Value);

                RoomLongStayingDiv.Items.Add(c.Name);
                RoomLongStayingDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomLongStayingDiv.Items[row].Value);

                RoomOutOfOrderDiv.Items.Add(c.Name);
                RoomOutOfOrderDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomOutOfOrderDiv.Items[row].Value);

                RoomOutOfServiceDiv.Items.Add(c.Name);
                RoomOutOfServiceDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomOutOfServiceDiv.Items[row].Value);

                RoomReservedDiv.Items.Add(c.Name);
                RoomReservedDiv.Items[row].Attributes.Add("style",
                          "background-color:" + RoomReservedDiv.Items[row].Value);
                row++;
            }
        }
        private void LoadHotelBillConfiguration()
        {
            HMCommonSetupBO commonSetupBOVat = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOCharge = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOInclusive = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOFOADType = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOFOADAmount = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOReCheckIn = new HMCommonSetupBO();
            HMCommonSetupBO GuestHouseCityChargeBO = new HMCommonSetupBO();
            HMCommonSetupBO GuestHouseRateIsPlusPlusBO = new HMCommonSetupBO();
            HMCommonSetupBO IsVatEnableOnGuestHouseCityChargeBO = new HMCommonSetupBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOVat = commonSetupDA.GetCommonConfigurationInfo("GuestHouseVat", "Vat Amount Setup");
            commonSetupBOCharge = commonSetupDA.GetCommonConfigurationInfo("GuestHouseServiceCharge", "Service Charge Setup");
            commonSetupBOInclusive = commonSetupDA.GetCommonConfigurationInfo("InclusiveHotelManagementBill", "Inclusive HotelManagement Bill Setup");
            commonSetupBOFOADType = commonSetupDA.GetCommonConfigurationInfo("FOAdditionalChargeType", "FOAdditionalChargeType");
            commonSetupBOFOADAmount = commonSetupDA.GetCommonConfigurationInfo("FOAdditionalChargeAmount", "FOAdditionalChargeAmount");
            commonSetupBOReCheckIn = commonSetupDA.GetCommonConfigurationInfo("InnboartCheckOutCancelHour", "CancelCheckOutHour");
            GuestHouseCityChargeBO = commonSetupDA.GetCommonConfigurationInfo("GuestHouseCityCharge", "GuestHouseCityCharge");
            GuestHouseRateIsPlusPlusBO = commonSetupDA.GetCommonConfigurationInfo("GuestHouseRateIsPlusPlus", "GuestHouseRateIsPlusPlus");
            IsVatEnableOnGuestHouseCityChargeBO = commonSetupDA.GetCommonConfigurationInfo("IsVatEnableOnGuestHouseCityCharge", "IsVatEnableOnGuestHouseCityCharge");

            if (!string.IsNullOrEmpty(commonSetupBOVat.SetupValue))
            {
                txtGuestHouseVatId.Value = commonSetupBOVat.SetupId.ToString();
                txtGuestHouseVat.Text = commonSetupBOVat.SetupValue.ToString();
                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOCharge.SetupValue))
            {
                txtGuestHouseServiceChargeId.Value = commonSetupBOCharge.SetupId.ToString();
                txtGuestHouseServiceCharge.Text = commonSetupBOCharge.SetupValue.ToString();
                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(GuestHouseCityChargeBO.SetupValue))
            {
                txtGuestHouseCityChargeId.Value = GuestHouseCityChargeBO.SetupId.ToString();
                txtGuestHouseCityCharge.Text = GuestHouseCityChargeBO.SetupValue.ToString();
                lblGuestHouseCityCharge.Text = GuestHouseCityChargeBO.Description.ToString();
                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOFOADType.SetupValue))
            {
                hfFOAdditionalChargeTypeId.Value = commonSetupBOFOADType.SetupId.ToString();
                ddlFOAdditionalChargeType.SelectedValue = commonSetupBOFOADType.SetupValue.ToString();
                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOFOADAmount.SetupValue))
            {
                hfFOAdditionalChargeAmountId.Value = commonSetupBOFOADAmount.SetupId.ToString();
                txtFOAdditionalChargeAmount.Text = commonSetupBOFOADAmount.SetupValue.ToString();
                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOReCheckIn.SetupValue))
            {
                hfReCheckInHourConfigurationId.Value = commonSetupBOReCheckIn.SetupId.ToString();
                ddlReCheckInHourConfiguration.SelectedValue = commonSetupBOReCheckIn.SetupValue.ToString();
                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOInclusive.SetupValue))
            {
                chkInclusiveHotelManagementBillId.Value = commonSetupBOInclusive.SetupId.ToString();

                if (commonSetupBOInclusive.SetupValue == "0")
                {
                    chkInclusiveHotelManagementBill.Checked = false;
                }
                else
                {
                    chkInclusiveHotelManagementBill.Checked = true;
                }

                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(GuestHouseRateIsPlusPlusBO.SetupValue))
            {
                chkGuestHouseRateIsPlusPlusId.Value = GuestHouseRateIsPlusPlusBO.SetupId.ToString();

                if (GuestHouseRateIsPlusPlusBO.SetupValue == "0")
                {
                    chkGuestHouseRateIsPlusPlus.Checked = false;
                }
                else
                {
                    chkGuestHouseRateIsPlusPlus.Checked = true;
                }

                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(IsVatEnableOnGuestHouseCityChargeBO.SetupValue))
            {
                cbGuestHouseCityChargeId.Value = IsVatEnableOnGuestHouseCityChargeBO.SetupId.ToString();

                if (IsVatEnableOnGuestHouseCityChargeBO.SetupValue == "0")
                {
                    cbGuestHouseCityCharge.Checked = false;
                }
                else
                {
                    cbGuestHouseCityCharge.Checked = true;
                }

                btnHotelBillCon.Text = "Update";
            }
        }
        private void LoadServiceBillConfiguration()
        {
            HMCommonSetupBO commonSetupBOVat = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOCharge = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOInclusive = new HMCommonSetupBO();
            HMCommonSetupBO GuestServiceSDChargeBO = new HMCommonSetupBO();
            HMCommonSetupBO GuestServiceRateIsPlusPlusBO = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOSBADType = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOSBADAmount = new HMCommonSetupBO();
            HMCommonSetupBO IsVatEnableOnGuestServiceSDChargeBO = new HMCommonSetupBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOVat = commonSetupDA.GetCommonConfigurationInfo("GuestServiceVat", "Vat Amount Setup");
            commonSetupBOCharge = commonSetupDA.GetCommonConfigurationInfo("GuestServiceServiceCharge", "Service Charge Setup");
            commonSetupBOInclusive = commonSetupDA.GetCommonConfigurationInfo("InclusiveGuestServiceBill", "Inclusive Guest Service Bill Setup");
            GuestServiceSDChargeBO = commonSetupDA.GetCommonConfigurationInfo("GuestServiceSDCharge", "GuestServiceSDCharge");
            GuestServiceRateIsPlusPlusBO = commonSetupDA.GetCommonConfigurationInfo("GuestServiceRateIsPlusPlus", "GuestServiceRateIsPlusPlus");
            commonSetupBOSBADType = commonSetupDA.GetCommonConfigurationInfo("SBAdditionalChargeType", "SBAdditionalChargeType");
            commonSetupBOSBADAmount = commonSetupDA.GetCommonConfigurationInfo("SBAdditionalChargeAmount", "SBAdditionalChargeAmount");
            IsVatEnableOnGuestServiceSDChargeBO = commonSetupDA.GetCommonConfigurationInfo("IsVatEnableOnGuestServiceSDCharge", "IsVatEnableOnGuestServiceSDCharge");

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
            if (!string.IsNullOrEmpty(GuestServiceSDChargeBO.SetupValue))
            {
                txtGuestServiceSDChargeId.Value = GuestServiceSDChargeBO.SetupId.ToString();
                txtGuestServiceSDCharge.Text = GuestServiceSDChargeBO.SetupValue.ToString();
                lblGuestServiceSDCharge.Text = GuestServiceSDChargeBO.Description.ToString();
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
            if (!string.IsNullOrEmpty(GuestServiceRateIsPlusPlusBO.SetupValue))
            {
                chkGuestServiceRateIsPlusPlusId.Value = GuestServiceRateIsPlusPlusBO.SetupId.ToString();
                btnServiceBillCon.Text = "Update";
                if (GuestServiceRateIsPlusPlusBO.SetupValue == "0")
                {
                    chkGuestServiceRateIsPlusPlus.Checked = false;
                }
                else
                {
                    chkGuestServiceRateIsPlusPlus.Checked = true;
                }
            }
            if (!string.IsNullOrEmpty(IsVatEnableOnGuestServiceSDChargeBO.SetupValue))
            {
                cbGuestServiceSDChargeId.Value = IsVatEnableOnGuestServiceSDChargeBO.SetupId.ToString();

                if (IsVatEnableOnGuestServiceSDChargeBO.SetupValue == "0")
                {
                    cbGuestServiceSDCharge.Checked = false;
                }
                else
                {
                    cbGuestServiceSDCharge.Checked = true;
                }

                btnHotelBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOSBADType.SetupValue))
            {
                hfSBAdditionalChargeTypeId.Value = commonSetupBOSBADType.SetupId.ToString();
                ddlSBAdditionalChargeType.SelectedValue = commonSetupBOSBADType.SetupValue.ToString();
                btnServiceBillCon.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOSBADAmount.SetupValue))
            {
                hfSBAdditionalChargeAmountId.Value = commonSetupBOSBADAmount.SetupId.ToString();
                txtSBAdditionalChargeAmount.Text = commonSetupBOSBADAmount.SetupValue.ToString();
                btnServiceBillCon.Text = "Update";
            }
        }
        private void LoadCheckInTimeonfiguration()
        {
            HMCommonSetupBO commonSetupBOTime = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOHour = new HMCommonSetupBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOTime = commonSetupDA.GetCommonConfigurationInfo("GuestHouseCheckInTime", "Guest House CheckIn Time Setup");
            commonSetupBOHour = commonSetupDA.GetCommonConfigurationInfo("GuestHouseCheckInExtraHour", "Guest House CheckIn ExtraHour Setup");

            if (!string.IsNullOrEmpty(commonSetupBOTime.SetupValue))
            {
                DateTime CheackInDateTime = Convert.ToDateTime(commonSetupBOTime.SetupValue);
                //this.txtProbableInHour.Text = Convert.ToInt32(CheackInDateTime.ToString("%h")) == 0 ? "12" : CheackInDateTime.ToString("%h");
                //this.txtProbableInMinute.Text = CheackInDateTime.ToString("mm");

                //string S = CheackInDateTime.ToString("tt");
                //this.ddlProbableInAMPM.SelectedIndex = S == "AM" ? 0 : 1;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                txtProbableCheckInTime.Text = CheackInDateTime.ToString(userInformationBO.TimeFormat);
                txtGuestHouseCheckInTimeId.Value = commonSetupBOTime.SetupId.ToString();
                btnCheckInTime.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOHour.SetupValue))
            {
                txtGuestHouseCheckInExtraHourId.Value = commonSetupBOHour.SetupId.ToString();
                txtGuestHouseCheckInExtraHour.Text = commonSetupBOHour.SetupValue.ToString();
                btnCheckInTime.Text = "Update";
            }
        }
        private void LoadCheckOutTimeonfiguration()
        {
            HMCommonSetupBO commonSetupBOTime = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOHour = new HMCommonSetupBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOTime = commonSetupDA.GetCommonConfigurationInfo("GuestHouseCheckOutTime", "Guest House CheckOut Time Setup");
            commonSetupBOHour = commonSetupDA.GetCommonConfigurationInfo("GuestHouseCheckOutExtraHour", "Guest House CheckOut ExtraHour Setup");

            if (!string.IsNullOrEmpty(commonSetupBOTime.SetupValue))
            {
                DateTime CheackOutDateTime = Convert.ToDateTime(commonSetupBOTime.SetupValue);
                //this.txtProbableHour.Text = Convert.ToInt32(CheackOutDateTime.ToString("%h")) == 0 ? "12" : CheackOutDateTime.ToString("%h");
                //this.txtProbableMinute.Text = CheackOutDateTime.ToString("mm");

                //string S = CheackOutDateTime.ToString("tt");
                //this.ddlProbableAMPM.SelectedIndex = S == "AM" ? 0 : 1;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                txtProbableCheckOutTime.Text = CheackOutDateTime.ToString(userInformationBO.TimeFormat);
                txtGuestHouseCheckOutTimeId.Value = commonSetupBOTime.SetupId.ToString();
                btnCheckOutTime.Text = "Update";
            }
            if (!string.IsNullOrEmpty(commonSetupBOHour.SetupValue))
            {
                txtGuestHouseCheckOutExtraHourId.Value = commonSetupBOHour.SetupId.ToString();
                txtGuestHouseCheckOutExtraHour.Text = commonSetupBOHour.SetupValue.ToString();
                btnCheckOutTime.Text = "Update";
            }
        }
        private void LoadFOConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardTimeFormat");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfInnboardTimeFormat.Value = setUpBO.SetupId.ToString();
                InnboardTimeFormat.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("HouseKeepingMorningDirtyHour", "HouseKeepingMorningDirtyHour");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfHouseKeepingMorningDirtyHour.Value = setUpBO.SetupId.ToString();
                HouseKeepingMorningDirtyHour.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("HouseKeepingDepartmentId", "HouseKeepingDepartmentId");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfHouseKeepingDepartmentId.Value = setUpBO.SetupId.ToString();
                HouseKeepingDepartmentId.SelectedValue = setUpBO.SetupValue;
            }
            
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("BillingConversionRate", "BillingConversionRate");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfBillingConversionRate.Value = setUpBO.SetupId.ToString();
                BillingConversionRate.Text = setUpBO.SetupValue;
            }

            CustomFieldBO fieldBO = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();
            fieldBO = commonDA.GetCustomFieldByFieldName("DefaultUsdToLocalCurrencyConversionRate");

            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfDefaultUsdToLocalCurrencyConversionRate.Value = fieldBO.FieldId.ToString();
                DefaultUsdToLocalCurrencyConversionRate.Text = fieldBO.FieldValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficePaidOutServiceCharge", "FrontOfficePaidOutServiceCharge");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfFrontOfficePaidOutServiceCharge.Value = setUpBO.SetupId.ToString();
                FrontOfficePaidOutServiceCharge.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfCompanyCountryId.Value = setUpBO.SetupId.ToString();
                CompanyCountryId.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardCalenderFormat");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfInnboardCalenderFormat.Value = setUpBO.SetupId.ToString();
                InnboardCalenderFormat.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("InnboardGridView", "GridViewPageSize");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfGridViewPageSize.Value = setUpBO.SetupId.ToString();
                GridViewPageSize.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("InnboardGridView", "GridViewPageLink");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfGridViewPageLink.Value = setUpBO.SetupId.ToString();
                GridViewPageLink.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeCostCenterId", "FrontOfficeCostCenterId");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfFrontOfficeCostCenterId.Value = setUpBO.SetupId.ToString();
                FrontOfficeCostCenterId.SelectedValue = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeInvoiceRoomServiceName", "FrontOfficeInvoiceRoomServiceName");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfFrontOfficeInvoiceRoomServiceName.Value = setUpBO.SetupId.ToString();
                FrontOfficeInvoiceRoomServiceName.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeMasterInvoiceTemplate", "IsBillSummaryPartWillHide");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBillSummaryPartWillHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBillSummaryPartWillHide.Checked = false;
                else
                    IsBillSummaryPartWillHide.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsWaterMarkImageDisplayEnable", "IsWaterMarkImageDisplayEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsWaterMarkImageDisplayEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsWaterMarkImageDisplayEnable.Checked = false;
                else
                    IsWaterMarkImageDisplayEnable.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsOnlyPdfEnableWhenReportExport.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsOnlyPdfEnableWhenReportExport.Checked = false;
                else
                    IsOnlyPdfEnableWhenReportExport.Checked = true;

            }
            
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRoomOverbookingEnable", "IsRoomOverbookingEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRoomOverbookingEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRoomOverbookingEnable.Checked = false;
                else
                    IsRoomOverbookingEnable.Checked = true;

            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsConversionRateEditable", "IsConversionRateEditable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsConversionRateEditable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsConversionRateEditable.Checked = false;
                else
                    IsConversionRateEditable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDynamicBestRegardsForConfirmationLetter", "IsDynamicBestRegardsForConfirmationLetter");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDynamicBestRegardsForConfirmationLetter.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsDynamicBestRegardsForConfirmationLetter.Checked = false;
                else
                    IsDynamicBestRegardsForConfirmationLetter.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsInvoiceGuestBillWithoutHeaderAndFooter", "IsInvoiceGuestBillWithoutHeaderAndFooter");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsInvoiceGuestBillWithoutHeaderAndFooter.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsInvoiceGuestBillWithoutHeaderAndFooter.Checked = false;
                else
                    IsInvoiceGuestBillWithoutHeaderAndFooter.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsServiceBillWithoutInHouseGuest", "IsServiceBillWithoutInHouseGuest");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsServiceBillWithoutInHouseGuest.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsServiceBillWithoutInHouseGuest.Checked = false;
                else
                    IsServiceBillWithoutInHouseGuest.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsRoomTypeCodeDisplayInRoomCalendar", "IsRoomTypeCodeDisplayInRoomCalendar");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRoomTypeCodeDisplayInRoomCalendar.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRoomTypeCodeDisplayInRoomCalendar.Checked = false;
                else
                    IsRoomTypeCodeDisplayInRoomCalendar.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsReservationConfirmationLetterOutletImageDisplay", "IsReservationConfirmationLetterOutletImageDisplay");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsReservationConfirmationLetterOutletImageDisplay.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsReservationConfirmationLetterOutletImageDisplay.Checked = false;
                else
                    IsReservationConfirmationLetterOutletImageDisplay.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsRoomReservationEmailAutoPostingEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRoomReservationEmailAutoPostingEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRoomReservationEmailAutoPostingEnable.Checked = false;
                else
                    IsRoomReservationEmailAutoPostingEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsRoomRegistrationEmailAutoPostingEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRoomRegistrationEmailAutoPostingEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRoomRegistrationEmailAutoPostingEnable.Checked = false;
                else
                    IsRoomRegistrationEmailAutoPostingEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("EmailAutoPosting", "IsCheckOutEmailAutoPostingEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCheckOutEmailAutoPostingEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCheckOutEmailAutoPostingEnable.Checked = false;
                else
                    IsCheckOutEmailAutoPostingEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsFrontOfficeIntegrateWithAccounts", "IsFrontOfficeIntegrateWithAccounts");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsFrontOfficeIntegrateWithAccounts.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsFrontOfficeIntegrateWithAccounts.Checked = false;
                else
                    IsFrontOfficeIntegrateWithAccounts.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("AdvancePaymentAdjustmentAccountsHeadId", "AdvancePaymentAdjustmentAccountsHeadId");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfAdvancePaymentAdjustmentAccountsHeadId.Value = setUpBO.SetupId.ToString();
                AdvancePaymentAdjustmentAccountsHeadId.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("HoldUpAccountsPostingAccountReceivableHeadId", "HoldUpAccountsPostingAccountReceivableHeadId");
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfHoldUpAccountsPostingAccountReceivableHeadId.Value = setUpBO.SetupId.ToString();
                HoldUpAccountsPostingAccountReceivableHeadId.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBillLockAndPreviewEnableForCheckOut", "IsBillLockAndPreviewEnableForCheckOut");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBillLockAndPreviewEnableForCheckOut.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBillLockAndPreviewEnableForCheckOut.Checked = false;
                else
                    IsBillLockAndPreviewEnableForCheckOut.Checked = true;
            }

            fieldBO = commonDA.GetCustomFieldByFieldName("RoomReservationTermsAndConditions");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomReservationTermsAndConditions.Value = fieldBO.FieldId.ToString();
                txtRoomReservationTermsAndConditions.Text = fieldBO.FieldValue;
            }

            fieldBO = commonDA.GetCustomFieldByFieldName("RoomRegistrationTermsAndConditions");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfRoomRegistrationTermsAndConditions.Value = fieldBO.FieldId.ToString();
                txtRoomRegistrationTermsAndConditions.Text = fieldBO.FieldValue;
            }



            fieldBO = commonDA.GetCustomFieldByFieldName("paramCLAdditionalServiceMessege");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfparamCLAdditionalServiceMessege.Value = fieldBO.FieldId.ToString();
                txtparamCLAdditionalServiceMessege.Text = fieldBO.FieldValue;
            }

            fieldBO = commonDA.GetCustomFieldByFieldName("paramCLCancellationPolicyMessege");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfparamCLCancellationPolicyMessege.Value = fieldBO.FieldId.ToString();
                txtparamCLCancellationPolicyMessege.Text = fieldBO.FieldValue;
            }
            fieldBO = commonDA.GetCustomFieldByFieldName("paramGIAggrimentMessege");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                hfparamGIAggrimentMessege.Value = fieldBO.FieldId.ToString();
                txtparamGIAggrimentMessege.Text = fieldBO.FieldValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsMinimumRoomRateCheckingForRoomTypeEnable", "IsMinimumRoomRateCheckingForRoomTypeEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsMinimumRoomRateCheckingForRoomTypeEnable.Value = setUpBO.SetupId.ToString();
                IsMinimumRoomRateCheckingForRoomTypeEnable.Checked = setUpBO.SetupValue == "1";
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithFrontOffice", "IsPayrollIntegrateWithFrontOffice");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPayrollIntegrateWithFrontOffice.Value = setUpBO.SetupId.ToString();
                IsPayrollIntegrateWithFrontOffice.Checked = setUpBO.SetupValue == "1";
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAutoNightAuditAndApprovalProcessEnable", "IsAutoNightAuditAndApprovalProcessEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsAutoNightAuditAndApprovalProcessEnable.Value = setUpBO.SetupId.ToString();
                IsAutoNightAuditAndApprovalProcessEnable.Checked = setUpBO.SetupValue == "1";
            }
        }
        private void LoadDepartmentDropdown()
        {
            DepartmentDA departmentDA = new DepartmentDA();
            List<DepartmentBO> departmentList = departmentDA.GetDepartmentInfo();

            HouseKeepingDepartmentId.DataSource = departmentList;
            HouseKeepingDepartmentId.DataTextField = "Name";
            HouseKeepingDepartmentId.DataValueField = "DepartmentId";
            HouseKeepingDepartmentId.DataBind();
        }
        private void LoadCountryDropdown()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();

            CompanyCountryId.DataSource = countryList;
            CompanyCountryId.DataTextField = "CountryName";
            CompanyCountryId.DataValueField = "CountryId";
            CompanyCountryId.DataBind();
        }
        private void LoadAdvancePaymentAdjustmentAccountsHeadIdDropdown()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("2").Where(x => x.IsTransactionalHead == true).ToList();

            AdvancePaymentAdjustmentAccountsHeadId.DataSource = entityBOList;
            AdvancePaymentAdjustmentAccountsHeadId.DataTextField = "HeadWithCode";
            AdvancePaymentAdjustmentAccountsHeadId.DataValueField = "NodeId";
            AdvancePaymentAdjustmentAccountsHeadId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            AdvancePaymentAdjustmentAccountsHeadId.Items.Insert(0, itemBank);
        }
        private void LoadHoldUpAccountsPostingAccountReceivableHeadIdDropdown()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("1").Where(x => x.IsTransactionalHead == true).ToList();

            HoldUpAccountsPostingAccountReceivableHeadId.DataSource = entityBOList;
            HoldUpAccountsPostingAccountReceivableHeadId.DataTextField = "HeadWithCode";
            HoldUpAccountsPostingAccountReceivableHeadId.DataValueField = "NodeId";
            HoldUpAccountsPostingAccountReceivableHeadId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            HoldUpAccountsPostingAccountReceivableHeadId.Items.Insert(0, itemBank);
        }
        private void LoadFrontOfficeCostCenterIdDropdown()
        {
            List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");

            FrontOfficeCostCenterId.DataSource = costCentreTabBO;
            FrontOfficeCostCenterId.DataTextField = "CostCenter";
            FrontOfficeCostCenterId.DataValueField = "CostCenterId";
            FrontOfficeCostCenterId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            FrontOfficeCostCenterId.Items.Insert(0, itemBank);
        }
        //Room Status Configuration//
        private void LoadUserGroup()
        {
            UserGroupDA userDa = new UserGroupDA();
            this.ddlUserGroup.DataSource = userDa.GetUserGroupInfo();
            this.ddlUserGroup.DataTextField = "GroupName";
            this.ddlUserGroup.DataValueField = "UserGroupId";
            this.ddlUserGroup.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlUserGroup.Items.Insert(0, item);
        }
        private void LoadRoomServiceName()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            List<CustomFieldBO> list = new List<CustomFieldBO>();
            list = hmCommonDA.GetCustomFieldList("FrontOfficeInvoiceRoomServiceName");
            FrontOfficeInvoiceRoomServiceName.DataSource = list;
            FrontOfficeInvoiceRoomServiceName.DataTextField = "Description";
            FrontOfficeInvoiceRoomServiceName.DataValueField = "FieldValue";
            FrontOfficeInvoiceRoomServiceName.DataBind();

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
                if(control.ID != "DefaultUsdToLocalCurrencyConversionRate")
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
                    if (control.ID == "IsPayrollIntegrateWithFrontOffice")
                    {
                        commonSetupBO.Description = "1 = Load Employee, 0 = Not Load Employee";
                    }
                    commonSetupBO.CreatedBy = UserInfoId;
                    commonSetupBO.LastModifiedBy = UserInfoId;
                    commonSetupBO.SetupName = control.ID;
                    commonSetupBO.TypeName = commonSetupBO.SetupId > 0 ? typeName : control.ID;
                }
                else if(control.ID == "DefaultUsdToLocalCurrencyConversionRate")
                {
                    if (!string.IsNullOrEmpty(((HiddenField)hfDefaultUsdToLocalCurrencyConversionRate).Value))
                    {
                        fieldBO.FieldId = Int32.Parse(((HiddenField)hfDefaultUsdToLocalCurrencyConversionRate).Value);
                    }
                    fieldBO.FieldType = "DefaultUsdToLocalCurrencyConversionRate";
                    fieldBO.FieldValue = DefaultUsdToLocalCurrencyConversionRate.Text;
                }

                try
                {
                    if(control.ID == "DefaultUsdToLocalCurrencyConversionRate")
                        status = commonDA.SaveOrUpdateCommonCustomFieldData(fieldBO, out tmpSetupId);
                    else
                        status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                    if (status)
                    {
                        if (commonSetupBO.SetupId > 0 || fieldBO.FieldId > 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Front Office Configuration Updated Successfully.", AlertType.Success);
                            if(commonSetupBO.SetupId >0)
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
                            CommonHelper.AlertInfo(innboardMessage, "Front Office Saved Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.FOConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FOConfiguration));
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
        private void SummaryUpdateInfo()
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            CustomFieldBO fieldAllRoomStatusLegentBO = new CustomFieldBO();
            HMCommonDA commonDA = new HMCommonDA();
            fieldAllRoomStatusLegentBO = commonDA.GetCustomFieldByFieldName("AllRoomStatusLegent");
            if (!string.IsNullOrEmpty(fieldAllRoomStatusLegentBO.FieldValue))
            {
                HMCommonSetupBO aCommonSetupBO = new HMCommonSetupBO();
                aCommonSetupBO.FieldId = fieldAllRoomStatusLegentBO.FieldId;
                aCommonSetupBO.FieldType = "AllRoomStatusLegent";
                aCommonSetupBO.FieldValue = AllRoomStatusLegent;
                aCommonSetupBO.ActiveStat = true;
                commonSetupBOList.Add(aCommonSetupBO);
            }

            CustomFieldBO fieldAllRoomStatusLegentColorBackupBO = new CustomFieldBO();
            fieldAllRoomStatusLegentColorBackupBO = commonDA.GetCustomFieldByFieldName("AllRoomStatusLegentColorBackup");
            if (!string.IsNullOrEmpty(fieldAllRoomStatusLegentColorBackupBO.FieldValue))
            {
                HMCommonSetupBO bCommonSetupBO = new HMCommonSetupBO();
                bCommonSetupBO.FieldId = fieldAllRoomStatusLegentColorBackupBO.FieldId;
                bCommonSetupBO.FieldType = "AllRoomStatusLegentColorBackup";
                bCommonSetupBO.FieldValue = AllRoomStatusLegentColorBackup;
                bCommonSetupBO.ActiveStat = true;
                commonSetupBOList.Add(bCommonSetupBO);
            }

            Boolean status = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);
        }
        //private void SetTab(string TabName)
        //{
        //    if (TabName == "GeneralConfiguration")
        //    {
        //        B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
        //        A.Attributes.Add("class", "ui-state-default ui-corner-top");
        //    }
        //    else if (TabName == "RoomPossiblePathConfiguration")
        //    {
        //        A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
        //        B.Attributes.Add("class", "ui-state-default ui-corner-top");
        //    }
        //    else if (TabName == "ColorConfiguration")
        //    {
        //        A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
        //        B.Attributes.Add("class", "ui-state-default ui-corner-top");
        //    }
        //}
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ArrayList GetPossiblePaths(int userGroupId, string pathType)
        {
            RoomStatusDA roomStatusDA = new RoomStatusDA();
            List<RoomStatusPossiblePathHeadBO> pathHeads = new List<RoomStatusPossiblePathHeadBO>();
            pathHeads = roomStatusDA.GetRoomStatusPossiblePathHead();

            List<RoomStatusPossiblePathBO> permittedPaths = new List<RoomStatusPossiblePathBO>();
            permittedPaths = roomStatusDA.GetPermittedRoomStatusPossiblePath(userGroupId, pathType);

            ArrayList arr = new ArrayList();
            arr.Add(new { PossiblePathHeads = pathHeads });
            arr.Add(new { PermittedPossiblePaths = permittedPaths });

            return arr;
        }
        [WebMethod]
        public static ReturnInfo SaveRoomStatusPossiblePathPermission(List<RoomStatusPossiblePathBO> RoomStatusPossiblePathAdded, List<RoomStatusPossiblePathBO> RoomStatusPossiblePathEdited, List<RoomStatusPossiblePathBO> RoomStatusPossiblePathDeleted)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            //List<RoomStatusPossiblePathBO> newIdList = new List<RoomStatusPossiblePathBO>();
            try
            {
                RoomStatusDA roomStatusDA = new RoomStatusDA();

                List<RoomStatusPossiblePathBO> duplicateList = new List<RoomStatusPossiblePathBO>();
                if (RoomStatusPossiblePathAdded.Count > 0)
                {
                    foreach (RoomStatusPossiblePathBO newadd in RoomStatusPossiblePathAdded)
                    {
                        RoomStatusPossiblePathBO permittedPaths = new RoomStatusPossiblePathBO();
                        permittedPaths = roomStatusDA.GetPermittedRoomStatusPossiblePathId(newadd.UserGroupId, newadd.PossiblePathType, newadd.PathId);
                        if (permittedPaths != null)
                        {
                            RoomStatusPossiblePathBO duplicateBO = (from m in RoomStatusPossiblePathAdded where m.UserGroupId == newadd.UserGroupId && m.PossiblePathType == newadd.PossiblePathType && m.PathId == newadd.PathId select m).FirstOrDefault();
                            duplicateList.Add(duplicateBO);
                        }
                    }
                }

                if (duplicateList.Count > 0)
                {
                    foreach (RoomStatusPossiblePathBO path in duplicateList)
                    {

                        RoomStatusPossiblePathAdded.Remove(path);
                    }
                }

                if (RoomStatusPossiblePathAdded.Count != 0 || RoomStatusPossiblePathEdited.Count != 0 || RoomStatusPossiblePathDeleted.Count != 0)
                {
                    rtninf.IsSuccess = roomStatusDA.SaveRoomStatusPossiblePathPermission(RoomStatusPossiblePathAdded, RoomStatusPossiblePathEdited, RoomStatusPossiblePathDeleted);
                }
                else
                {
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    foreach (var item in RoomStatusPossiblePathAdded)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomStatusPossiblePath.ToString(), item.MappingId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomStatusPossiblePath));
                    }
                    if (RoomStatusPossiblePathEdited.Count > 0)
                    {
                        foreach (var item in RoomStatusPossiblePathEdited)
                        {
                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomStatusPossiblePath.ToString(), item.MappingId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomStatusPossiblePath));
                        }
                    }
                    if (RoomStatusPossiblePathDeleted.Count > 0)
                    {
                        foreach (var item in RoomStatusPossiblePathDeleted)
                        {
                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomStatusPossiblePath.ToString(), item.MappingId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomStatusPossiblePath));
                        }
                    }

                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }
        protected void btnSaveFOConfig_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<HMCommonSetupBO> setUpBO = new List<HMCommonSetupBO>();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            string typeName = string.Empty;

            setUpBO = commonSetupDA.GetAllCommonConfigurationInfo();

            foreach (Control item in dvFOConfig.Controls)
            {
                typeName=setUpBO.Where(s => s.SetupName == item.ID).Select(s=>s.TypeName).FirstOrDefault();
                if (item is TextBox||item is DropDownList||item is CheckBox)
                {
                    UpdateConfiguration(item, userInformationBO.UserInfoId, typeName);
                } 
                typeName = string.Empty;
            }
            LoadFOConfiguration();
        }
    }
}