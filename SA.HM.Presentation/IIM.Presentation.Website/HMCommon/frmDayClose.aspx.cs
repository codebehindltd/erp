using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmDayClose : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                btnKotUpdate.Visible = false;
                this.SoftwareModulePermissionList();
                this.CheckPermission();
            }
        }
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            this.SetTab("DayClose");
            if (!isValidDayClossingDate()) { return; }
            if (!isValidForm()) { return; }

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isAutoNightAuditAndApprovalProcessEnableBO = new HMCommonSetupBO();
            isAutoNightAuditAndApprovalProcessEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsAutoNightAuditAndApprovalProcessEnable", "IsAutoNightAuditAndApprovalProcessEnable");
            if (isAutoNightAuditAndApprovalProcessEnableBO != null)
            {
                if (isAutoNightAuditAndApprovalProcessEnableBO.SetupValue != "0")
                {
                    NightAuditProcessWithApproval();
                }
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMDayCloseDA daycloseDA = new HMDayCloseDA();
            HMDayCloseBO dayCloseBO = new HMDayCloseBO();
            dayCloseBO.DayClossingDate = hmUtility.GetDateTimeFromString(txtDayClossingDate.Text, userInformationBO.ServerDateFormat);
            dayCloseBO.CreatedBy = userInformationBO.UserInfoId;
            dayCloseBO.DayClossingModuleId = 1;

            Boolean success = daycloseDA.GenerateDayClossing(dayCloseBO);
            if (success)
            {
                CommonHelper.AlertInfo(innboardMessage, "Day Closing Process Successfull.", AlertType.Success);

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.DayClose.ToString(), 0,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DayClose));

            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Day Closing Process Failed.", AlertType.Error);
            }
        }
        protected void btnKotPendingList_Click(object sender, EventArgs e)
        {
            this.SetTab("KOTPending");
            List<KotBillMasterBO> pendingKotListBO = new List<KotBillMasterBO>();
            gvKotPendingListInfo.DataSource = pendingKotListBO;
            gvKotPendingListInfo.DataBind();

            if (!isValidDayForKotProcess()) { return; }
            this.LoadKotPendingListInfo();            
        }
        protected void btnKotUpdate_Click(object sender, EventArgs e)
        {
            this.SetTab("KOTPending");
            if (!isValidDayForKotProcess()) { return; }
            KotBillMasterDA entityDA = new KotBillMasterDA();
            int rowsKotPendingListInfo = gvKotPendingListInfo.Rows.Count;
            for (int i = 0; i < rowsKotPendingListInfo; i++)
            {
                Label lblKotId = (Label)gvKotPendingListInfo.Rows[i].FindControl("lblKotId");
                int kotId = Convert.ToInt32(lblKotId.Text);
                DropDownList ddlKotStatus = (DropDownList)gvKotPendingListInfo.Rows[i].FindControl("ddlKotStatus");
                string kotStatus = ddlKotStatus.SelectedValue;

                entityDA.UpdateKotBillMasterForKotPending(kotId, kotStatus);
            }

            this.LoadKotPendingListInfo();
            CommonHelper.AlertInfo(innboardMessage, "KOT Update Successfull.", AlertType.Success);
        }
        //************************ User Defined Function ********************//
        private void SetTab(string TabName)
        {
            if (TabName == "DayClose")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }            
            else if (TabName == "KOTPending")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadKotPendingListInfo()
        {
            KotBillMasterDA entityDA = new KotBillMasterDA();
            List<KotBillMasterBO> pendingKotListBO = new List<KotBillMasterBO>();
            gvKotPendingListInfo.DataSource = pendingKotListBO;
            gvKotPendingListInfo.DataBind();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMDayCloseDA daycloseDA = new HMDayCloseDA();
            HMDayCloseBO dayCloseBO = new HMDayCloseBO();
            DateTime kotDate = hmUtility.GetDateTimeFromString(txtKotDate.Text, userInformationBO.ServerDateFormat);
            
            pendingKotListBO = entityDA.GetPendingKotInfoByKotDate(kotDate);
            gvKotPendingListInfo.DataSource = pendingKotListBO;
            gvKotPendingListInfo.DataBind();

            int rowsKotPendingListInfo = gvKotPendingListInfo.Rows.Count;
            if (rowsKotPendingListInfo > 0)
            {
                btnKotUpdate.Visible = true;
            }
            else
            {
                btnKotUpdate.Visible = false;
            }

            this.SetTab("KOTPending");
        }
        private void SoftwareModulePermissionList()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //if (userInformationBO.UserInfoId == 1)
            //{
            //    hfDayCloseInformation.Value = "3";
            //}
            //else
            //{
            if (Session["SoftwareModulePermissionList"] != null)
            {
                hfDayCloseInformation.Value = Session["SoftwareModulePermissionList"].ToString();
            }
            else
            {
                Session["UserInformationBOSession"] = null;
                Response.Redirect("Login.aspx");
            }
            //}
        }
        public bool IsEmptyAllItemAvailableValidate()
        {
            bool status = true;

            //if (hfDayCloseInformation.Value != "3")
            //{
            //    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //    List<AvailableGuestListBO> roomList = roomRegistrationDA.GetGuestRoomNightAuditInfo("0", hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
            //    int roomCount = roomList.Count;

            //    List<GHServiceBillBO> serviceList = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("", 0, hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), "");
            //    int serviceCount = serviceList.Count;

            //    GuestBillPaymentDA roomPaymentDA = new GuestBillPaymentDA();
            //    int paymentRoomCount = roomPaymentDA.GetCountHotelGuestBillApprovedItem(hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), "HotelGuestBillApproved");
            //    int paymentServiceCount = roomPaymentDA.GetCountHotelGuestBillApprovedItem(hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), "HotelGuestServiceBillApproved");

            //    if (paymentRoomCount == 0)
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, "Please Confirm " + this.txtDayClossingDate.Text + " 's Night Audit is Processed or Not.", AlertType.Warning);
            //        this.txtDayClossingDate.Focus();
            //        status = false;
            //    }
            //    else if (roomCount > paymentRoomCount)
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, "Please Confirm " + this.txtDayClossingDate.Text + " 's Night Audit is Processed or Not.", AlertType.Warning);
            //        this.txtDayClossingDate.Focus();
            //        status = false;
            //    }
            //    else if (serviceCount > paymentServiceCount)
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, "Please Confirm " + this.txtDayClossingDate.Text + " 's Night Audit is Processed or Not.", AlertType.Warning);
            //        this.txtDayClossingDate.Focus();
            //        status = false;
            //    }
            //}
            return status;
        }
        private void CheckPermission()
        {
            btnProcess.Visible = isSavePermission;
        }
        public bool isValidDayClossingDate()
        {
            this.SetTab("DayClose");
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtDayClossingDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Date.", AlertType.Warning);
                this.txtDayClossingDate.Focus();
                status = false;
            }
            else if (!string.IsNullOrEmpty(this.txtDayClossingDate.Text))
            {
                if (hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date >= DateTime.Now.Date)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Day Clossing is not aplicable for your provided date.", AlertType.Warning);
                    this.txtDayClossingDate.Focus();
                    status = false;
                }
                else
                {
                    DateTime processDate = hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                    RoomRegistrationDA guestLedgerInfoDA = new RoomRegistrationDA();
                    InhouseGuestLedgerBO guestLedgerInfoBO = new InhouseGuestLedgerBO();
                    guestLedgerInfoBO = guestLedgerInfoDA.GetIsPreviousDayTransaction(hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat));

                    if (guestLedgerInfoBO != null)
                    {
                        processDate = guestLedgerInfoBO.TransactionDate;
                    }

                    if (hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date == processDate.Date)
                    {
                        HMCommonDA hmCoomnoDA = new HMCommonDA();
                        DayCloseBO dayCloseBO = new DayCloseBO();

                        dayCloseBO = hmCoomnoDA.GetHotelDayCloseInformation(hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat));
                        if (dayCloseBO != null)
                        {
                            if (dayCloseBO.DayCloseId > 0)
                            {
                                CommonHelper.AlertInfo(innboardMessage, "Day Closed for the date '" + this.txtDayClossingDate.Text + "', please try with another date.", AlertType.Warning);
                                this.txtDayClossingDate.Focus();
                                status = false;
                            }
                        }                        
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Day Clossing is not aplicable for your provided date.", AlertType.Warning);
                        this.txtDayClossingDate.Focus();
                        status = false;
                    }                    
                }
            }
            return status;
        }
        public bool isValidDayForKotProcess()
        {
            this.SetTab("KOTPending");
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtKotDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "KOT Date.", AlertType.Warning);
                this.txtKotDate.Focus();
                status = false;
            }
            else if (!string.IsNullOrEmpty(this.txtDayClossingDate.Text))
            {
                if (hmUtility.GetDateTimeFromString(this.txtKotDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date >= DateTime.Now.Date)
                {
                    CommonHelper.AlertInfo(innboardMessage, "KOT Pending List Generation is not aplicable for your provided date.", AlertType.Warning);
                    this.txtKotDate.Focus();
                    status = false;
                }
            }
            return status;
        }
        public bool isValidForm()
        {
            this.SetTab("DayClose");
            bool status = true;
            if (!string.IsNullOrEmpty(this.txtDayClossingDate.Text))
            {
                HMCommonDA hmCoomnoDA = new HMCommonDA();
                List<DayCloseBO> dayCloseBOList = new List<DayCloseBO>();
                dayCloseBOList = hmCoomnoDA.GetHotelDayCloseCheckingInformation(hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat));
                if (dayCloseBOList != null)
                {
                    if (dayCloseBOList.Count > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, dayCloseBOList[0].DayClosedDescription, AlertType.Warning);
                        this.txtDayClossingDate.Focus();
                        status = false;
                    }
                }
            }
            return status;
        }
        private void NightAuditProcessWithApproval()
        {
            string registrationId = "0";
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            if (registrationId == "0")
            {
                List<RoomRegistrationBO> ActiveRoomRegistrationInfoBO = new List<RoomRegistrationBO>();
                ActiveRoomRegistrationInfoBO = roomRegistrationDA.GetActiveRoomRegistrationInfo();
                foreach (RoomRegistrationBO row in ActiveRoomRegistrationInfoBO)
                {
                    if (registrationId == "0")
                    {
                        registrationId = row.RegistrationId.ToString();
                    }
                    else
                    {
                        registrationId += "," + row.RegistrationId.ToString();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(registrationId))
            {
                Boolean processStatus = false;
                if (hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date == DateTime.Now.Date)
                {
                    processStatus = roomRegistrationDA.RoomNightAuditAutoApprovalProcess(registrationId, DateTime.Now, hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
                }
                else
                {
                    processStatus = roomRegistrationDA.RoomNightAuditAutoApprovalProcess(registrationId, hmUtility.GetDateTimeFromString(this.txtDayClossingDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
                }

                if (processStatus)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Auto Night Audit and Approval Process Successfull.", AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.AutoNightAuditAndApprovalProcess.ToString(), 0,
                                    ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AutoNightAuditAndApprovalProcess));

                }
            }
        }
    }
}