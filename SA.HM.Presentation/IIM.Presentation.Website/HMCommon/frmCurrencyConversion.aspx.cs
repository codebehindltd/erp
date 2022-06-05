using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmCurrencyConversion : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
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
            int tmpSetupId = 0;
            if (string.IsNullOrWhiteSpace(this.txtConversionRate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Conversion Rate.", AlertType.Warning);
                this.txtConversionRate.Focus();
                return;
            }

            CommonCurrencyConversionBO commonSetupBO = new CommonCurrencyConversionBO();
            CommonCurrencyConversionDA commonSetupDA = new CommonCurrencyConversionDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;

            if (Int32.Parse(this.txtConversionRateId.Value) > 0)
            {
                commonSetupBO.ConversionId = Int32.Parse(this.txtConversionRateId.Value);
            }
            commonSetupBO.FromCurrencyId = Int32.Parse(ddlFromConversion.SelectedValue);
            commonSetupBO.ToCurrencyId = Int32.Parse(ddlToConversion.SelectedValue);
            commonSetupBO.ConversionRate = Decimal.Parse(txtConversionRate.Text);
            commonSetupBO.ActiveStat = true;
            Boolean status = commonSetupDA.SaveCommonCurrencyConversion(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Currency Conversion Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CurrencyConfiguration.ToString(), commonSetupBO.ConversionId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConfiguration));
                btnSave.Text = "Save";
                Clear();
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Currency Conversion Saved Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CurrencyConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConfiguration));
                btnSave.Text = "Save";
                txtConversionRateId.Value = tmpSetupId.ToString();
                Clear();
            }
        }
        //************************ User Defined Function ********************//
        private void LoadFromConversionHead()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            this.ddlFromConversion.DataSource = headDA.GetConversionHeadInfoByType("NLocal");
            this.ddlFromConversion.DataTextField = "CurrencyName";
            this.ddlFromConversion.DataValueField = "CurrencyId";
            this.ddlFromConversion.DataBind();

            ListItem itemSelect = new ListItem();
            itemSelect.Value = "0";
            itemSelect.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFromConversion.Items.Insert(0, itemSelect);
        }
        private void ToFromConversionHead()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            this.ddlToConversion.DataSource = headDA.GetConversionHeadInfoByType("Local");
            this.ddlToConversion.DataTextField = "CurrencyName";
            this.ddlToConversion.DataValueField = "CurrencyId";
            this.ddlToConversion.DataBind();

            ListItem itemSelect = new ListItem();
            itemSelect.Value = "0";
            itemSelect.Text = hmUtility.GetDropDownFirstValue();
            this.ddlToConversion.Items.Insert(0, itemSelect);
        }
        private void LoadConversionHead()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            this.ddlFromConversion.DataSource = headDA.GetConversionHeadInfoByType("NLocal");
            this.ddlFromConversion.DataTextField = "CurrencyName";
            this.ddlFromConversion.DataValueField = "CurrencyId";
            this.ddlFromConversion.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFromConversion.Items.Insert(0, item);

            this.ddlToConversion.DataSource = headDA.GetConversionHeadInfoByType("Local");
            this.ddlToConversion.DataTextField = "CurrencyName";
            this.ddlToConversion.DataValueField = "CurrencyId";
            this.ddlToConversion.DataBind();

            this.ddlToConversion.Items.Insert(0, item);
        }
        private void Clear()
        {

            txtConversionRate.Text = string.Empty;
            txtConversionRateId.Value = string.Empty;
            ddlFromConversion.SelectedIndex = 0;
            ddlToConversion.SelectedIndex = 0;
            btnSave.Text = "Save";
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static CommonCurrencyConversionBO GetConversionRateByHeadId(string FromHeadId, string ToHeadId)
        {
            CommonCurrencyConversionBO setupBO = new CommonCurrencyConversionBO();
            CommonCurrencyConversionDA setupDA = new CommonCurrencyConversionDA();
            setupBO = setupDA.GetCurrencyConversionInfoByCurrencyId(Int32.Parse(FromHeadId), Int32.Parse(ToHeadId));
            return setupBO;
        }
    }
}