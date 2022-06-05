using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmUnitConversionRate : BasePage
    {
        public frmUnitConversionRate()
            :base("PurchaseInfo")
        {

        }
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadConversionHead();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int temconversionRateId = 0;

            InvUnitConversionRateBO conversionBO = new InvUnitConversionRateBO();
            InvUnitConversionRateDA conversionDA = new InvUnitConversionRateDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            conversionBO.CreatedBy = userInformationBO.UserInfoId;
            conversionBO.LastModifiedBy = userInformationBO.UserInfoId;
            conversionBO.LastModifiedBy = userInformationBO.LastModifiedBy;

            if (!string.IsNullOrWhiteSpace(this.hfConversionRateId.Value))
            {
                conversionBO.ConversionId = Int32.Parse(this.hfConversionRateId.Value);
            }
            conversionBO.FromUnitHeadId = Int32.Parse(ddlFromConversion.SelectedValue);
            conversionBO.ToUnitHeadId = Int32.Parse(ddlToConversion.SelectedValue);
            if (conversionBO.FromUnitHeadId == conversionBO.ToUnitHeadId)
            {
                conversionBO.ConversionRate = 1;
            }
            else
            {
                conversionBO.ConversionRate = Decimal.Parse(txtConversionRate.Text);
            }

            Boolean status = conversionDA.SaveOrUpdateUnitConversionRate(conversionBO, out temconversionRateId);
            if (temconversionRateId == 0)
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CurrencyConfiguration.ToString(), conversionBO.ConversionId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConfiguration));
                btnSave.Text = "Save";
                Clear();
            }
            else
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CurrencyConfiguration.ToString(), temconversionRateId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConfiguration));
                btnSave.Text = "Save";
                hfConversionRateId.Value = temconversionRateId.ToString();
                Clear();
            }
        }

        private void LoadConversionHead()
        {
            InvUnitHeadDA headDA = new InvUnitHeadDA();
            this.ddlFromConversion.DataSource = headDA.GetAllActiveConversionHeadInfo();
            this.ddlFromConversion.DataTextField = "HeadName";
            this.ddlFromConversion.DataValueField = "UnitHeadId";
            this.ddlFromConversion.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFromConversion.Items.Insert(0, item);

            this.ddlToConversion.DataSource = headDA.GetAllActiveConversionHeadInfo();
            this.ddlToConversion.DataTextField = "HeadName";
            this.ddlToConversion.DataValueField = "UnitHeadId";
            this.ddlToConversion.DataBind();

            this.ddlToConversion.Items.Insert(0, item);
        }
        private void Clear()
        {

            txtConversionRate.Text = string.Empty;
            hfConversionRateId.Value = string.Empty;
            ddlFromConversion.SelectedIndex = 0;
            ddlToConversion.SelectedIndex = 0;
            btnSave.Text = "Save";
        }

        [WebMethod]
        public static InvUnitConversionRateBO GetConversionRateByHeadId(string FromHeadId, string ToHeadId)
        {
            InvUnitConversionRateBO setupBO = new InvUnitConversionRateBO();
            InvUnitConversionRateDA setupDA = new InvUnitConversionRateDA();
            setupBO = setupDA.GetUnitConversionRateInfoByHeadId(Int32.Parse(FromHeadId), Int32.Parse(ToHeadId));
            return setupBO;
        }

        
    }
}