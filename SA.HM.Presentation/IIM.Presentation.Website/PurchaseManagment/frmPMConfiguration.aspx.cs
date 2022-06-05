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

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMConfiguration : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadRequisitionPOConfiguration();
                this.LoadRequisitionPOSavedInformation();
                
            }
            CheckObjectPermission();
        }
        private void CheckObjectPermission()
        {
            btnHotelBillCon.Visible = isSavePermission;
            btnServiceBillCon.Visible = isSavePermission;
        }
        private void LoadRequisitionPOConfiguration()
        {
            UserInformationDA entityDA = new UserInformationDA();
            List<UserInformationBO> GetUserInformationBOList = entityDA.GetUserInformation();
            this.ddlRequisitionCheckedBy.DataSource = GetUserInformationBOList;
            this.ddlRequisitionCheckedBy.DataTextField = "UserName";
            this.ddlRequisitionCheckedBy.DataValueField = "UserInfoId";
            this.ddlRequisitionCheckedBy.DataBind();

            this.ddlRequisitionApprovedBy.DataSource = GetUserInformationBOList;
            this.ddlRequisitionApprovedBy.DataTextField = "UserName";
            this.ddlRequisitionApprovedBy.DataValueField = "UserInfoId";
            this.ddlRequisitionApprovedBy.DataBind();

            this.ddlPOCheckedBy.DataSource = GetUserInformationBOList;
            this.ddlPOCheckedBy.DataTextField = "UserName";
            this.ddlPOCheckedBy.DataValueField = "UserInfoId";
            this.ddlPOCheckedBy.DataBind();

            this.ddlPOApprovedBy.DataSource = GetUserInformationBOList;
            this.ddlPOApprovedBy.DataTextField = "UserName";
            this.ddlPOApprovedBy.DataValueField = "UserInfoId";
            this.ddlPOApprovedBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRequisitionCheckedBy.Items.Insert(0, item);
            this.ddlRequisitionApprovedBy.Items.Insert(0, item);
            this.ddlPOCheckedBy.Items.Insert(0, item);
            this.ddlPOApprovedBy.Items.Insert(0, item);
        }
        private void LoadRequisitionPOSavedInformation()
        {
            ddlRequisitionCheckedBy.SelectedValue = "0";
            ddlRequisitionApprovedBy.SelectedValue = "0";
            HMCommonSetupBO commonSetupBORequisition = new HMCommonSetupBO();            
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBORequisition = commonSetupDA.GetCommonConfigurationInfo("IsRequisitionCheckedByEnable", "IsRequisitionCheckedByEnable");

            if (commonSetupBORequisition != null)
            {
                if (commonSetupBORequisition.SetupId > 0)
                {
                    if (commonSetupBORequisition.SetupValue == "1")
                    {
                        ddlRequisitionCheckedBy.SelectedValue = commonSetupBORequisition.Description.Split('~')[0].ToString();
                        ddlRequisitionApprovedBy.SelectedValue = commonSetupBORequisition.Description.Split('~')[1].ToString();
                    }
                }
            }

            ddlPOCheckedBy.SelectedValue = "0";
            ddlPOApprovedBy.SelectedValue = "0";
            HMCommonSetupBO commonSetupBOPO = new HMCommonSetupBO();
            commonSetupBOPO = commonSetupDA.GetCommonConfigurationInfo("IsPurchaseOrderCheckedByEnable", "IsPurchaseOrderCheckedByEnable");

            if (commonSetupBOPO != null)
            {
                if (commonSetupBOPO.SetupId > 0)
                {
                    if (commonSetupBOPO.SetupValue == "1")
                    {
                        ddlPOCheckedBy.SelectedValue = commonSetupBOPO.Description.Split('~')[0].ToString();
                        ddlPOApprovedBy.SelectedValue = commonSetupBOPO.Description.Split('~')[1].ToString();
                    }
                }
            }
        }
        protected void btnHotelBillCon_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO.SetupId = 116;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "IsRequisitionCheckedByEnable";
            commonSetupBO.TypeName = "IsRequisitionCheckedByEnable";
            commonSetupBO.SetupValue = "1";
            commonSetupBO.Description = this.ddlRequisitionCheckedBy.SelectedValue + "~" + this.ddlRequisitionApprovedBy.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Requisition Approval Configuration Setup Successfully.", AlertType.Success);
            }
        }
        protected void btnServiceBillCon_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO.SetupId = 117;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "IsPurchaseOrderCheckedByEnable";
            commonSetupBO.TypeName = "IsPurchaseOrderCheckedByEnable";
            commonSetupBO.SetupValue = "1";
            commonSetupBO.Description = this.ddlPOCheckedBy.SelectedValue + "~" + this.ddlPOApprovedBy.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Purchase Order Approval Configuration Setup Successfully.", AlertType.Success);
            }
        }
    }
}