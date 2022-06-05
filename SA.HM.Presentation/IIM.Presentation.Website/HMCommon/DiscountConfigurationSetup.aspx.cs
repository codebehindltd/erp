using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class DiscountConfigurationSetup : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSetup();
            }
        }
        private void LoadSetup()
        {
            DiscountConfigSetupDA discountConfigSetupDA = new DiscountConfigSetupDA();
            DiscountConfigSetupBO discountConfigsPrev = new DiscountConfigSetupBO();
            discountConfigsPrev = discountConfigSetupDA.GetDiscountConfigSetup();

            if (discountConfigsPrev != null)
            {
                chkDisForBank.Checked = Convert.ToBoolean(discountConfigsPrev.IsBankDiscount);
                chkDisWithMembership.Checked = Convert.ToBoolean(discountConfigsPrev.IsDiscountAndMembershipDiscountApplicableTogether);
                rbDisIndividual.Checked = Convert.ToBoolean(discountConfigsPrev.IsDiscountApplicableIndividually);
                rbDisMaxOneForMulti.Checked = Convert.ToBoolean(discountConfigsPrev.IsDiscountApplicableMaxOneWhenMultiple);
                rbShowDisForMulti.Checked = Convert.ToBoolean(discountConfigsPrev.IsDiscountOptionShowsWhenMultiple);
                
                rbDisIndividual.Checked = false;
                rbDisIndividual.Visible = false;
                rbDisMaxOneForMulti.Checked = true;
                rbShowDisForMulti.Checked = false;
                rbShowDisForMulti.Visible = false;
            }

        }
        private void Clear()
        {
            chkDisForBank.Checked = false;
            chkDisWithMembership.Checked = false;
            rbDisIndividual.Checked = false;
            rbDisIndividual.Visible = false;
            rbDisMaxOneForMulti.Checked = true;
            rbShowDisForMulti.Checked = false;
            rbShowDisForMulti.Visible = false;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int createdBy = userInformationBO.UserInfoId;

            DiscountConfigSetupDA discountConfigSetupDA = new DiscountConfigSetupDA();

            DiscountConfigSetupBO discountConfigsPrev = new DiscountConfigSetupBO();
            discountConfigsPrev = discountConfigSetupDA.GetDiscountConfigSetup();

            DiscountConfigSetupBO discountConfigsNew = new DiscountConfigSetupBO
            {
                IsBankDiscount = chkDisForBank.Checked,
                IsDiscountAndMembershipDiscountApplicableTogether = chkDisWithMembership.Checked,
                IsDiscountApplicableIndividually = rbDisIndividual.Checked,
                IsDiscountApplicableMaxOneWhenMultiple = rbDisMaxOneForMulti.Checked,
                IsDiscountOptionShowsWhenMultiple = rbShowDisForMulti.Checked
            };

            if (discountConfigsPrev.ConfigurationId > 0)
            {
                //update
                discountConfigsNew.ConfigurationId = discountConfigsPrev.ConfigurationId;
                bool status = discountConfigSetupDA.UpdateDiscountConfigSetup(discountConfigsNew, createdBy);
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.DiscountConfigSetup.ToString(), discountConfigsNew.ConfigurationId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DiscountConfigSetup));
                //Clear();
            }
            else
            {
                //save
                long tmpId = 0;
                Boolean status = discountConfigSetupDA.SaveDiscountConfigSetup(discountConfigsNew, createdBy, out tmpId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.DiscountConfigSetup.ToString(), tmpId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DiscountConfigSetup));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    //Clear();
                }
            }
        }
        [WebMethod]
        public static ReturnInfo SaveCheckedByApprovedByUsers()
        {
            ReturnInfo rtninf = new ReturnInfo();
            long tempId = 0;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtninf;
        }
    }
}