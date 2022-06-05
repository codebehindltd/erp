using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Text.RegularExpressions;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmHMCommonSetup : System.Web.UI.Page
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
                this.CheckObjectPermission();
                this.LoadPaymentModeInfoGridView();
                this.LoadCurrencyTypeInformation();
                this.FillSendingPanel();
                this.FillRecievingPanel();
            }
        }
        protected void btnReceive_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            if (!(IsValidMail(txtRecievingEmail.Text)))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "valid Email.", AlertType.Warning);
                return;
            }
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;

            if (!string.IsNullOrEmpty(this.txtReceivingMailId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtSendingMailId.Value);
            }
            commonSetupBO.SetupName = "RecieveEmailConfiguration";
            commonSetupBO.TypeName = "ReceiveEmailAddress";
            commonSetupBO.SetupValue = txtRecievingEmail.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Email Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ReceiveMail.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ReceiveMail));

            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Email Configuration Saved Successfull.", AlertType.Success);
                this.btnReceive.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ReceiveMail.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ReceiveMail));


            }
            SetTab("EmailConfiguration");


        }
        protected void gvPaymentModeInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblPaymentModeIdValue = (Label)e.Row.FindControl("lblPaymentModeId");
                int PaymentModeId = Convert.ToInt32(lblPaymentModeIdValue.Text);

                Label lblPaymentAccountsPostingIdValue = (Label)e.Row.FindControl("lblPaymentAccountsPostingId");
                int paymentAccountsPostingIdValue = Convert.ToInt32(lblPaymentAccountsPostingIdValue.Text);

                Label lblReceiveAccountsPostingIdValue = (Label)e.Row.FindControl("lblReceiveAccountsPostingId");
                int receiveAccountsPostingId = Convert.ToInt32(lblReceiveAccountsPostingIdValue.Text);

                NodeMatrixDA nmda = new NodeMatrixDA();
                List<NodeMatrixBO> nmListBO = new List<NodeMatrixBO>();

                nmListBO = nmda.GetNodeMatrixInfo().Where(x => x.NodeMode == true && x.IsTransactionalHead == true).OrderBy(y => y.NodeHead).ToList();

                DropDownList ddlPayment = (e.Row.FindControl("ddlPayment") as DropDownList);
                DropDownList ddlReceive = (e.Row.FindControl("ddlReceive") as DropDownList);

                ddlPayment.DataSource = nmListBO;
                ddlPayment.DataTextField = "HeadWithCode";
                ddlPayment.DataValueField = "NodeId";
                ddlPayment.DataBind();

                ddlReceive.DataSource = nmListBO;
                ddlReceive.DataTextField = "HeadWithCode";
                ddlReceive.DataValueField = "NodeId";
                ddlReceive.DataBind();

                if (nmListBO.Count > 1)
                {
                    ListItem itemKitchen = new ListItem();
                    itemKitchen.Value = "0";
                    itemKitchen.Text = hmUtility.GetDropDownFirstValue();
                    ddlPayment.Items.Insert(0, itemKitchen);
                    ddlReceive.Items.Insert(0, itemKitchen);

                    ddlPayment.SelectedValue = paymentAccountsPostingIdValue.ToString();
                    ddlReceive.SelectedValue = receiveAccountsPostingId.ToString();
                }
            }
        }
        protected void btnCurrencySetup_Click(object sender, EventArgs e)
        {

            int tmpSetupId = 0;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;

            if (!string.IsNullOrEmpty(this.txtSetupId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtSetupId.Value);
            }
            commonSetupBO.SetupName = "CurrencyConfiguration";
            commonSetupBO.TypeName = "CurrencyType";
            commonSetupBO.SetupValue = ddlCurrencyType.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Currency Type Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CurrencyConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConfiguration));
                btnCurrencySetup.Text = "Update";
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Currency Type Configuration Saved Successfull.", AlertType.Success);
                this.btnReceive.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CurrencyConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConfiguration));
                btnCurrencySetup.Text = "Update";
                txtSetupId.Value = tmpSetupId.ToString();
            }

        }
        protected void btnPaymentMode_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int rowsPaymentModeItem = gvPaymentModeInfo.Rows.Count;
            List<HMCommonSetupBO> paymentModeItemListBO = new List<HMCommonSetupBO>();
            for (int i = 0; i < rowsPaymentModeItem; i++)
            {
                HMCommonSetupBO paymentModeItemBO = new HMCommonSetupBO();
                Label lblPaymentModeIdValue = (Label)gvPaymentModeInfo.Rows[i].FindControl("lblPaymentModeId");

                DropDownList ddlPaymentVale = (DropDownList)gvPaymentModeInfo.Rows[i].FindControl("ddlPayment");
                DropDownList ddlReceiveValue = (DropDownList)gvPaymentModeInfo.Rows[i].FindControl("ddlReceive");

                paymentModeItemBO.PaymentModeId = Convert.ToInt32(lblPaymentModeIdValue.Text);
                paymentModeItemBO.PaymentAccountsPostingId = Convert.ToInt32(ddlPaymentVale.SelectedValue);
                paymentModeItemBO.ReceiveAccountsPostingId = Convert.ToInt32(ddlReceiveValue.SelectedValue);
                paymentModeItemBO.CreatedBy = userInformationBO.CreatedBy;

                paymentModeItemListBO.Add(paymentModeItemBO);
            }
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            Boolean status = commonSetupDA.SaveOrUpdatePaymentModeInfo(paymentModeItemListBO);
            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Payment Mode Configuration Updated  Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.PaymentModeConfiguration.ToString(), 0,
               ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PaymentModeConfiguration));

            }
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!IsSendConfigurationValid())
            {
                return;
            }
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtSendingMailId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtSendingMailId.Value);
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "SendEmailConfiguration";
            commonSetupBO.TypeName = "SendEmailAddress";
            commonSetupBO.SetupValue = txtSendingEmail.Text + '~' + txtPassword.Text + '~' + txtSmtpHost.Text + '~' + txtSmtpPort.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

            if (tmpSetupId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Email Configuration Updated  Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SendMail.ToString(), commonSetupBO.SetupId,
               ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SendMail));

            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Email Configuration Saved Successfull.", AlertType.Success);
                this.btnSend.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SendMail.ToString(), tmpSetupId,
                    ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SendMail));


            }
            SetTab("EmailConfiguration");
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmHMCommonSetup.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;

        }
        private void LoadPaymentModeInfoGridView()
        {
            gvPaymentModeInfo.DataSource = new List<HMCommonSetupBO>();
            gvPaymentModeInfo.DataBind();

            CheckObjectPermission();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            List<HMCommonSetupBO> files = commonSetupDA.GetAllCommonPaymentModeInfo();

            gvPaymentModeInfo.DataSource = files;
            gvPaymentModeInfo.DataBind();
        }
        private bool IsValidMail(string Email)
        {
            bool status = true;
            string expression = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "EmailConfiguration")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");

            }
            else if (TabName == "PayrolConfiguration")
            {


                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        //Email Configuration Section
        public bool IsSendConfigurationValid()
        {
            bool status = true;
            if (!IsValidMail(txtSendingEmail.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "valid Email.", AlertType.Warning);
                status = false;
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Password.", AlertType.Warning);
                status = false;
            }
            else if (string.IsNullOrEmpty(txtSmtpHost.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Smtp Host.", AlertType.Warning);
                status = false;
            }
            else if (string.IsNullOrEmpty(txtSmtpPort.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Smtp Port.", AlertType.Warning);
                status = false;
            }
            return status;
        }
        private void LoadCurrencyTypeInformation()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CurrencyType", "CurrencyConfiguration");
            if (commonSetupBO.SetupId > 0)
            {
                ddlCurrencyType.SelectedValue = commonSetupBO.SetupValue;
                txtSetupId.Value = commonSetupBO.SetupId.ToString();
                btnCurrencySetup.Text = "Update";
            }
            else
            {
                ddlCurrencyType.SelectedIndex = 0;
                txtSetupId.Value = "";

                btnCurrencySetup.Text = "Save";
            }

        }
        public static string LoadGuestPaymentDetailGridViewByWM()
        {
            string strTable = "";
            List<PMSalesBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (PMSalesBillPaymentBO dr in detailList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentAmout + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(" + dr.PaymentId + ")' alt='Delete Information' border='0' />";
                    strTable += "</td></tr>";
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        private void FillSendingPanel()
        {
            string smtpPort = "", smtpHost = "", sendingMail = "", password = "";
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;
            if (!string.IsNullOrEmpty(mainString))
            {
                this.txtSendingMailId.Value = commonSetupBO.SetupId.ToString();
                string[] dataArray = mainString.Split('~');
                this.txtSendingEmail.Text = dataArray[0];
                this.txtPassword.Text = dataArray[1];
                this.txtSmtpHost.Text = dataArray[2];
                this.txtSmtpPort.Text = dataArray[3];
                this.btnSend.Text = "Update";
            }
        }
        private void FillRecievingPanel()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("ReceiveEmailAddress", "RecieveEmailConfiguration");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.txtReceivingMailId.Value = commonSetupBO.SetupId.ToString();
                this.txtRecievingEmail.Text = commonSetupBO.SetupValue;
                this.txtReceivingMailId.Value = commonSetupBO.SetupId.ToString();
                this.btnReceive.Text = "Update";
            }
        }
    }
}