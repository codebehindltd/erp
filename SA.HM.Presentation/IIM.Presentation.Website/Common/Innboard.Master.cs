using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity;
using System.Web.Services;
using HotelManagement.Data;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.Common
{
    public partial class Innboard : System.Web.UI.MasterPage
    {
        HMUtility hmUtility = new HMUtility();
        protected string isMenuCollupse = string.Empty;
        public string innBoardDateFormat = string.Empty, innBoardTimeFormat = string.Empty, isOldMenuEnable = string.Empty, dayOpenDate = string.Empty;
        protected int isSoftwareLicenseExpiredNotificationMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SoftwareLicenseExpiredNotification"] != null)
            {
                isSoftwareLicenseExpiredNotificationMessageBoxEnable = 1;
                //lblSoftwareLicenseExpiredNotificationMessage.Text = "Your License Will Expire Soon, Please Contact with Data Grid Limited.";
                lblSoftwareLicenseExpiredNotificationMessage.Text = "Your License Will Expire Soon, Please Contact with Administrator.";
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO != null)
            {
                SiteTitle.Text = userInformationBO.SiteTitle;

                lblCurrentUser.Text = userInformationBO.UserId;
                lblDayOpenDate.Text = userInformationBO.DayOpenDate.ToString(userInformationBO.ServerDateFormat);
                CheckObjectPermission(userInformationBO.UserInfoId);

                MainMenu.InnerHtml = userInformationBO.UserMenu;
                ReportMenu.InnerHtml = userInformationBO.UserReportMenu;

                MenuCollupseOption();

                hfMessageHideTime.Value = userInformationBO.MessageHideTimer.ToString();
                hfIsMenuSHowHide.Value = userInformationBO.IsMenuSearchRoomSearchRoomStatisticsInfoEnable ? "1" : "0";

                //if (!userInformationBO.IsMenuSearchRoomSearchRoomStatisticsInfoEnable)
                //{
                //    pnlMenuSearchRoomSearchRoomStatisticsInfo.Visible = false;
                //    btnRoomStatistics.Visible = false;
                //    btnUserDashboard.Visible = false;
                //}


            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if (Session["HomeMenuShowHide"] != null)
                hfHomeIsHidden.Value = (string)Session["HomeMenuShowHide"];
            else
                hfHomeIsHidden.Value = "1";

            innBoardDateFormat = userInformationBO.ClientDateFormat;
            innBoardTimeFormat = userInformationBO.TimeFormat;
            isOldMenuEnable = userInformationBO.IsOldMenuEnable;
            dayOpenDate = userInformationBO.DayOpenDate.ToString(userInformationBO.ServerDateFormat);
            hfUserInfoObj.Value = JsonConvert.SerializeObject(userInformationBO);
            LoadEmpSearchEnableWithDetails();
            MessageCount();
        }
        private void LoadEmpSearchEnableWithDetails()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBO2 = new HMCommonSetupBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsEmpSearchFromDashboardEnable", "IsEmpSearchFromDashboardEnable");
            commonSetupBO2 = commonSetupDA.GetCommonConfigurationInfo("IsEmpSearchDetailsFromDashboardEnable", "IsEmpSearchDetailsFromDashboardEnable");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                hfIsEmpSearchFromDashboardEnable.Value = commonSetupBO.SetupValue.ToString();
            }
            if (!string.IsNullOrEmpty(commonSetupBO2.SetupValue))
            {
                hfIsEmpSearchDetailsFromDashboardEnable.Value = commonSetupBO2.SetupValue.ToString();
            }
        }
        private void MenuCollupseOption()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string currentFormName = ((url.Split('/').Last()).Split('?')[0]).Split('.')[0];
            if (!string.IsNullOrWhiteSpace(currentFormName))
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
                ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
                objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userInformationBO.UserInfoId, currentFormName.ToUpper());
                if (objectPermissionBO.ObjectPermissionId > 0)
                {
                    //class="nav nav-list collapse in"
                    isMenuCollupse = (objectPermissionBO.ObjectGroupHead).Replace("grp", "menu");
                }
            }
        }
        private void CheckObjectPermission(int userId)
        {
        }
        public void MessageCount()
        {
            Int64 TotalUnreadMessage = 0;

            CommonMessageDA messageDa = new CommonMessageDA();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            messageDetails = messageDa.GetMessageDetailsByUserId(userInformationBO.UserId, null, 10, out TotalUnreadMessage);

            lblMessageCount.Text = TotalUnreadMessage.ToString();

            if (TotalUnreadMessage > 0)
            {
                MessageCountBadge.Attributes.Add("style", " background-color:#18cde6;");
            }

            int rowCount = 0;
            string messageBrief = string.Empty;
            string time = string.Empty, mDate = string.Empty;
            string readenMessageColor = "#E5E5E5";

            foreach (CommonMessageDetailsBO md in messageDetails)
            {
                time = md.MessageDate.ToString(userInformationBO.TimeFormat);
                mDate = md.MessageDate.ToString(userInformationBO.ServerDateFormat);

                if (rowCount > 0)
                {
                    messageBrief += "<li class='divider' style='margin: 2px 1px;'></li>";
                }

                readenMessageColor = md.IsReaden ? "#F5F5F5" : string.Empty;

                //EncryptionHelper encryptionHelper = new EncryptionHelper();
                //string encryptData = encryptionHelper.Encrypt(md.MessageId.ToString() + "," + md.MessageDetailsId.ToString());

                messageBrief += "<li style='background-color:" + readenMessageColor + ";'>" +
                                "<a tabindex='-1' style = 'margin: 3px 5px 5px 1px; padding:0;' href='/HMCommon/frmCommonMessageDetails.aspx?mid=" + md.MessageId.ToString() + "," + md.MessageDetailsId.ToString() + "'>" +
                                 "<div class ='container-fluid'> <div class='row'>" +
                                 "<div class='col-md-3' style= 'min-height:19px; margin-left: 1%;'>" + md.UserName + "</div>" +
                                 "<div class='col-md-7' style= 'min-height:19px; margin-left: 1%;'>" + md.Subjects +
                                 " - " + (md.MessageBody.Length > 10 ? (md.MessageBody.Substring(0, 10) + " ...") : md.MessageBody) +
                                 "</div>" +
                                 "<div class='col-md-2' style= 'min-height:19px; margin-left: 1%;'>" + mDate + " " + time + "</div>" +
                                 "</div></div>" +
                                "</a></li>";

                rowCount++;
            }

            if (string.IsNullOrEmpty(messageBrief))
            {
                messageBrief += "<li><a tabindex='-1' style = 'margin: 3px 5px 5px 1px; padding:0;' href='javascript:void(0)'>" +
                                    "<div class='row'>" +
                                    "<div class='col-md-12'>No Message in Message Box</div>" +
                                    "</div>" +
                                   "</a></li>";
            }

            MessageBriefDescription.InnerHtml = messageBrief;
        }
    }
}