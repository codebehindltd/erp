using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.Membership;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class frmMemberPaymentConfiguration : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadMemberType();
                LoadCompany();
            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }                

                MemberPaymentConfigurationBO configBO = new MemberPaymentConfigurationBO();
                MemberPaymentDA memPaymentDA = new MemberPaymentDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (ddlTransactionType.SelectedValue == "MemberType")
                {
                    configBO.MemberTypeOrMemberId = Convert.ToInt64(ddlMemberType.SelectedValue);
                    configBO.BillingStartDate = null;
                    configBO.DoorAccessDate = null;
                }
                else
                {
                    if (string.IsNullOrEmpty(hfMemberId.Value))
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Member.", AlertType.Warning);
                        return;
                    }
                    configBO.MemberTypeOrMemberId = Convert.ToInt64(hfMemberId.Value);
                    configBO.BillingStartDate = CommonHelper.DateTimeToMMDDYYYY(txtBillStartDate.Text);
                    configBO.DoorAccessDate = CommonHelper.DateTimeToMMDDYYYY(txtDoorStartDate.Text);
                }
                configBO.TransactionType = ddlTransactionType.SelectedValue;
                configBO.BillingPeriod = ddlBillingPeriod.SelectedValue;
                configBO.BillingAmount = Convert.ToDecimal(txtBillingAmount1.Text);

                if (string.IsNullOrWhiteSpace(hfPaymentConfigId.Value))
                {
                    long tmpConfigId = 0;
                    configBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = memPaymentDA.SaveMemberPaymentConfigInfo(configBO, out tmpConfigId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Cancel();
                    }

                }
                else
                {
                    configBO.MemPaymentConfigId = Convert.ToInt32(hfPaymentConfigId.Value);
                    configBO.LastModifiedBy = userInformationBO.UserInfoId;

                    Boolean status = memPaymentDA.UpdateMemberPaymentConfigInfo(configBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Cancel();
                    }
                }                
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }        
        public void LoadMemberType()
        {
            MemMemberTypeDA memDA = new MemMemberTypeDA();
            List<MemMemberTypeBO> list = new List<MemMemberTypeBO>();
            list = memDA.GetAllMemberType();

            ddlMemberType.DataSource = list;
            ddlMemberType.DataTextField = "Name";
            ddlMemberType.DataValueField = "TypeId";
            ddlMemberType.DataBind();            

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlMemberType.Items.Insert(0, itemNodeId);           
        }
        private void LoadCompany()
        {
            MemMemberBasicDA memDA = new MemMemberBasicDA();
            List<MemMemberBasicsBO> memBO = new List<MemMemberBasicsBO>();
            memBO = memDA.GetMemActiveMemberListInfo();

            this.ddlMember.DataSource = memBO;
            this.ddlMember.DataTextField = "MembershipNumber";
            this.ddlMember.DataValueField = "MemberId";
            this.ddlMember.DataBind();                      
        }        
        private void Cancel()
        {
            ddlTransactionType.SelectedIndex = 0;
            ddlMemberType.SelectedIndex = 0;
            ddlBillingPeriod.SelectedIndex = 0;
            txtBillingAmount1.Text = string.Empty;
            txtBillStartDate.Text = string.Empty;
            txtDoorStartDate.Text = string.Empty;
            btnSave.Text = "Save";
            hfPaymentConfigId.Value = "";
            hfMemberId.Value = "";
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (ddlTransactionType.SelectedValue == "MemberType")
            {
                if (ddlMemberType.SelectedIndex == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Member Type.", AlertType.Warning);
                    flag = false;
                    ddlMemberType.Focus();
                }
            }            
            if (string.IsNullOrWhiteSpace(txtBillingAmount1.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Billing Amount.", AlertType.Warning);
                flag = false;
                txtBillingAmount1.Focus();
            }
            return flag;
        }

        [WebMethod]
        public static MemberPaymentConfigurationBO GetMemberPaymentConfigInfo(string transactionType, int membertype)
        {
            MemberPaymentConfigurationBO configBO = new MemberPaymentConfigurationBO();
            MemberPaymentDA memPaymentDA = new MemberPaymentDA();

            configBO = memPaymentDA.GetMemberPaymentConfigInfo(transactionType, membertype);

            return configBO;
        }
        [WebMethod]
        public static MemberPaymentConfigurationBO GetMemberPaymentConfigInfoByMemberId(int memberId)
        {
            MemberPaymentConfigurationBO configBO = new MemberPaymentConfigurationBO();
            MemberPaymentDA memPaymentDA = new MemberPaymentDA();

            configBO = memPaymentDA.GetMemberPaymentConfigInfoByMemberId(memberId);

            return configBO;
        }
    }
}