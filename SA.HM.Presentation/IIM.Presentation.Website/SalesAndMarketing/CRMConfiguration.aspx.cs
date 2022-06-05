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

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class CRMConfiguration : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
                LoadCRMConfiguration();
            }
        }
        private void LoadDepartment()
        {
            DepartmentDA DA = new DepartmentDA();
            List<DepartmentBO> DepartList = new List<DepartmentBO>();

            DepartList = DA.GetDepartmentInfo();

            this.TechnicalDepartment.DataSource = DepartList;
            this.TechnicalDepartment.DataTextField = "Name";
            this.TechnicalDepartment.DataValueField = "DepartmentId";
            this.TechnicalDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.TechnicalDepartment.Items.Insert(0, item);
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (Control item in CommonInformation.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            foreach (Control item in ContactInformation.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            foreach (Control item in CompanyInformation.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            foreach (Control item in DealInformation.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            foreach (Control item in QuotationInformation.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            foreach (Control item in LifeCycleStageInformation.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            foreach (Control item in DealStageConfig.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            LoadCRMConfiguration();
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
                            CommonHelper.AlertInfo(innboardMessage, "Sales & Marketing Configuration Updated Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.RestuarantConfiguration.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Sales & Marketing Configuration Saved Successfully.", AlertType.Success);
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
        private void LoadCRMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("TechnicalDepartment", "TechnicalDepartment");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfTechnicalDepartment.Value = setUpBO.SetupId.ToString();
                TechnicalDepartment.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("SalesCompanyNumberPrefix", "SalesCompanyNumberPrefix");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfSalesCompanyNumberPrefix.Value = setUpBO.SetupId.ToString();
                SalesCompanyNumberPrefix.Text = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("SalesContactNumberPrefix", "SalesContactNumberPrefix");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfSalesContactNumberPrefix.Value = setUpBO.SetupId.ToString();
                SalesContactNumberPrefix.Text = setUpBO.SetupValue;
            }
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("SalesDealNumberPrefix", "SalesDealNumberPrefix");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfSalesDealNumberPrefix.Value = setUpBO.SetupId.ToString();
                SalesDealNumberPrefix.Text = setUpBO.SetupValue;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSalesNoteEnable", "IsSalesNoteEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSalesNoteEnable.Value = setUpBO.SetupId.ToString();
                IsSalesNoteEnable.Checked = setUpBO.SetupValue == "1";
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCRMAreaFieldEnable", "IsCRMAreaFieldEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCRMAreaFieldEnable.Value = setUpBO.SetupId.ToString();
                IsCRMAreaFieldEnable.Checked = setUpBO.SetupValue == "1";
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts", "IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts.Value = setUpBO.SetupId.ToString();
                IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts.Checked = setUpBO.SetupValue == "1";
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsContactInformationRestrictedForAllUsers", "IsContactInformationRestrictedForAllUsers");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsContactInformationRestrictedForAllUsers.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsContactInformationRestrictedForAllUsers.Checked = false;
                else
                    IsContactInformationRestrictedForAllUsers.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsContactHyperlinkEnableFromGrid", "IsContactHyperlinkEnableFromGrid");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsContactHyperlinkEnableFromGrid.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsContactHyperlinkEnableFromGrid.Checked = false;
                else
                    IsContactHyperlinkEnableFromGrid.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsShippingAddresswillshow", "IsShippingAddresswillshow");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsShippingAddresswillshow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsShippingAddresswillshow.Checked = false;
                else
                    IsShippingAddresswillshow.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDiscountPercentageWillShow", "IsDiscountPercentageWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDiscountPercentageWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsDiscountPercentageWillShow.Checked = false;
                else
                    IsDiscountPercentageWillShow.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCreditLimitWillShow", "IsCreditLimitWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCreditLimitWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCreditLimitWillShow.Checked = false;
                else
                    IsCreditLimitWillShow.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsNumberOfEmployeeWillShow", "IsNumberOfEmployeeWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsNumberOfEmployeeWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsNumberOfEmployeeWillShow.Checked = false;
                else
                    IsNumberOfEmployeeWillShow.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAnnualRevenueWillShow", "IsAnnualRevenueWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsAnnualRevenueWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsAnnualRevenueWillShow.Checked = false;
                else
                    IsAnnualRevenueWillShow.Checked = true;

            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsHotelGuestCompanyRestrictionForAllUsers", "IsHotelGuestCompanyRestrictionForAllUsers");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsHotelGuestCompanyRestrictionForAllUsers.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsHotelGuestCompanyRestrictionForAllUsers.Checked = false;
                else
                    IsHotelGuestCompanyRestrictionForAllUsers.Checked = true;
            }            

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCRMCompanyNumberEnable", "IsCRMCompanyNumberEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCRMCompanyNumberEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCRMCompanyNumberEnable.Checked = false;
                else
                    IsCRMCompanyNumberEnable.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCompanyHyperlinkEnableFromGrid", "IsCompanyHyperlinkEnableFromGrid");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsCompanyHyperlinkEnableFromGrid.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsCompanyHyperlinkEnableFromGrid.Checked = false;
                else
                    IsCompanyHyperlinkEnableFromGrid.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSegmentNameWillShow", "IsSegmentNameWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSegmentNameWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsSegmentNameWillShow.Checked = false;
                else
                    IsSegmentNameWillShow.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsProductInformationWillShow", "IsProductInformationWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsProductInformationWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsProductInformationWillShow.Checked = false;
                else
                    IsProductInformationWillShow.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsServiceInformationWillShow", "IsServiceInformationWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsServiceInformationWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsServiceInformationWillShow.Checked = false;
                else
                    IsServiceInformationWillShow.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDealRestrictedForAllUsers", "IsDealRestrictedForAllUsers");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDealRestrictedForAllUsers.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsDealRestrictedForAllUsers.Checked = false;
                else
                    IsDealRestrictedForAllUsers.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDealStageCanChangeMoreThanOneStep", "IsDealStageCanChangeMoreThanOneStep");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDealStageCanChangeMoreThanOneStep.Value = setUpBO.SetupId.ToString();
                IsDealStageCanChangeMoreThanOneStep.Checked = setUpBO.SetupValue == "1";
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDeviceOrUserWillShow", "IsDeviceOrUserWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDeviceOrUserWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsDeviceOrUserWillShow.Checked = false;
                else
                    IsDeviceOrUserWillShow.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDeliveryWillShow", "IsDeliveryWillShow");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsDeliveryWillShow.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsDeliveryWillShow.Checked = false;
                else
                    IsDeliveryWillShow.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsQuotationCreateFromSiteServeyFeedback", "IsQuotationCreateFromSiteServeyFeedback");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsQuotationCreateFromSiteServeyFeedback.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsQuotationCreateFromSiteServeyFeedback.Checked = false;
                else
                    IsQuotationCreateFromSiteServeyFeedback.Checked = true;
            }

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsLifeCycleStageCanChangeMoreThanOneStep", "IsLifeCycleStageCanChangeMoreThanOneStep");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsLifeCycleStageCanChangeMoreThanOneStep.Value = setUpBO.SetupId.ToString();
                IsLifeCycleStageCanChangeMoreThanOneStep.Checked = setUpBO.SetupValue == "1";
            }

            btnUpdate.Text = "Update";
        }

    }
}