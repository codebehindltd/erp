using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using Newtonsoft.Json;
using HotelManagement.Data.UserInformation;
using Mamun.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmCommonMessage : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadUserGroup();
                LoadGridView();
                SetTab("EntryTab");
            }
        }
        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            bool IsMessageSendAllGroupUser = false;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CommonMessageBO message = new CommonMessageBO();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            if (ddlMessageType.SelectedValue == "Group" && ddlUserGroup.SelectedValue == "0")
            {
                IsMessageSendAllGroupUser = true;
            }

            message = JsonConvert.DeserializeObject<CommonMessageBO>(hfMessage.Value);
            messageDetails = JsonConvert.DeserializeObject<List<CommonMessageDetailsBO>>(hfMessageDetails.Value);

            message.MessageFrom = userInformationBO.UserInfoId;
            message.MessageFromUserId = userInformationBO.UserId;
            message.MessageDate = DateTime.Now;
            message.Importance = "Normal";

            CommonMessageDA messageDa = new CommonMessageDA();
            bool status = messageDa.SaveMessage(message, messageDetails, IsMessageSendAllGroupUser);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Message Sent Successfully.", AlertType.Success);

                Innboard master = (Innboard)this.Master;
                master.MessageCount();

                Cancel();
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }

            SetTab("EntryTab");
        }
        protected void gvMessageSend_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMessageSend.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        //************************ User Defined Function ********************//
        private void LoadUserGroup()
        {
            List<UserGroupBO> userGroupList = new List<UserGroupBO>();
            UserGroupDA userGroupDa = new UserGroupDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            userGroupList = userGroupDa.GetActiveUserGroupInfo(userInformationBO.UserGroupId);

            ddlUserGroup.DataSource = userGroupList;
            ddlUserGroup.DataTextField = "GroupName";
            ddlUserGroup.DataValueField = "UserGroupId";
            ddlUserGroup.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlUserGroup.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCommonMessage.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            CommonMessageDA messageDa = new CommonMessageDA();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            messageDetails = messageDa.GetMessageDetailsBySendUserId(userInformationBO.UserInfoId);

            foreach (CommonMessageDetailsBO md in messageDetails)
            {
                md.MessageBody = (md.MessageBody.Length > 50 ? (md.MessageBody.Substring(0, 50) + " ...") : md.MessageBody);
            }

            this.gvMessageSend.DataSource = messageDetails;
            this.gvMessageSend.DataBind();
            SetTab("SearchTab");
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void Cancel()
        {
            txtMessageBody.Text = string.Empty;
            txtMessageSubject.Text = string.Empty;
            txtUserName.Text = string.Empty;

            ddlMessageType.SelectedIndex = 0;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<UserInformationBO> GetUserInformationAutoSearch(string searchTerm)
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            UserInformationDA userDa = new UserInformationDA();

            userInformationList = userDa.GetUserInformationAutoSearch(searchTerm);

            return userInformationList;
        }
        [WebMethod]
        public static List<UserInformationBO> LoadUserByGroup(int groupId)
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            UserInformationDA userDa = new UserInformationDA();

            userInformationList = userDa.GetUserInformationByUserGroup(groupId);

            return userInformationList;
        }

        [WebMethod]
        public static ReturnInfo SendMailByID(CommonMessageBO CMB, List<CommonMessageDetailsBO> CMD)
        {
            bool status = false;
            //bool IsMessageSendAllGroupUser = false;
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CommonMessageBO message = new CommonMessageBO();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();


            message = CMB;
            messageDetails = CMD;

            message.MessageFrom = userInformationBO.UserInfoId;
            message.MessageFromUserId = userInformationBO.UserId;
            message.MessageDate = DateTime.Now;
            message.Importance = "Normal";

           
            ReturnInfo info = new ReturnInfo();
            
            try
            {
                CommonMessageDA messageDa = new CommonMessageDA();
                status = messageDa.SaveMessageById(message, messageDetails);

                if (status)
                {
                    info.IsSuccess = true;
                    
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.MessageSent, AlertType.Success);
                    info.Data = 0;
                    

                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return info;
        }
    }
}
