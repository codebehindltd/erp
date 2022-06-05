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

namespace HotelManagement.Presentation.Website.DocumentManagement
{
    public partial class DMConfiguration : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDMConfiguration();
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (Control item in CommonInformation.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }

            LoadDMConfiguration();
        }
        private void UpdateConfiguration(Control control, int UserInfoId)
        {
            int tmpSetupId = 0;
            string hiddenField = "hf" + control.ID;

            Boolean status = false;
            Control hiddenControl = CommonInformation.FindControl(hiddenField);

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
                            CommonHelper.AlertInfo(innboardMessage, "Document Management Configuration Updated Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Document Management Configuration Saved Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                            this.btnUpdate.Text = "Update";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void LoadDMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();


            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDocumentInformationRestrictedForAllUsers", "IsDocumentInformationRestrictedForAllUsers");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDocumentInformationRestrictedForAllUsers.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsDocumentInformationRestrictedForAllUsers.Checked = false;
                else
                    IsDocumentInformationRestrictedForAllUsers.Checked = true;

            }


            btnUpdate.Text = "Update";
        }
    }
}