using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class CommonConfiguration : BasePage
    {
        HiddenField innboardMessage;

        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadBasicSetupInfo();
            }
        }

        private void LoadBasicSetupInfo()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("ApprovalPolicyConfiguration", "ApprovalPolicyConfiguration");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfApprovalPolicyConfiguration.Value = setUpBO.SetupId.ToString();
                ApprovalPolicyConfiguration.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBankIntegratedWithAccounts", "IsBankIntegratedWithAccounts");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBankIntegratedWithAccounts.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsBankIntegratedWithAccounts.Checked = false;
                else
                    IsBankIntegratedWithAccounts.Checked = true;

            }
            //btnUpdateSettings.Text = "Update";
        }
        private void UpdateConfiguration(Control control, int UserInfoId)
        {
            int tmpSetupId = 0;
            string hiddenField = "hf" + control.ID;

            Boolean status = false;
            Control hiddenControl = ConfigurationSettingPanel.FindControl(hiddenField);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            if (control != null && hiddenControl != null)
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
                            CommonHelper.AlertInfo(innboardMessage, "Common Configuration Settings are updated Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Common Configuration Settings are Saved Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                            //this.btnUpdateSettings.Text = "Update";
                        }
                    }
                    else
                    {

                        CommonHelper.AlertInfo(innboardMessage, "Common Configuration Settings are not updated Successfully.", AlertType.Error);

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        protected void btnUpdateSettings_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (Control item in ConfigurationSettingPanel.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            LoadBasicSetupInfo();


        }
    }
}