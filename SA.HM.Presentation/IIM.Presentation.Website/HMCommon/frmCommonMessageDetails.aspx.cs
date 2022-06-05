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
using System.Collections;
using Mamun.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmCommonMessageDetails : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        Int64 messageId = 0, messageDetailsId = 0;

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                if (Request.QueryString["mid"] != null)
                {
                    string decryptValue = string.Empty;
                    //EncryptionHelper encryptionHelper = new EncryptionHelper();
                    //decryptValue = encryptionHelper.Decrypt(Request.QueryString["mid"]);

                    decryptValue = Request.QueryString["mid"].ToString();

                    messageId = Convert.ToInt64(decryptValue.Split(',')[0]);
                    messageDetailsId = Convert.ToInt64(decryptValue.Split(',')[1]);

                    LoadMessage();
                }

                MessageCount();
            }
        }

        private void LoadMessage()
        {
            try
            {
                CommonMessageDA messageDa = new CommonMessageDA();
                CommonMessageDetailsBO messageDetails = new CommonMessageDetailsBO();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                messageDetails = messageDa.GetMessageDetailsById(messageId, messageDetailsId);

                MessageSubject.InnerText = messageDetails.Subjects;
                MessageBody.InnerText = messageDetails.MessageBody;

                if (!messageDetails.IsReaden)
                {
                    messageDa.UpdateMessageDetails(messageDetails.MessageId, messageDetails.MessageDetailsId);

                    //Innboard master = (Innboard)this.Master;
                    //master.MessageCount();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }

        public void MessageCount()
        {
            Int16 TotalUnreadMessage = 0;

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
        }

        [WebMethod]
        public static GridViewDataNPaging<CommonMessageDetailsBO, GridPaging> LoadMessageInbox(int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //userInformationBO.GridViewPageSize = 2; userInformationBO.GridViewPageLink = 3;

            GridViewDataNPaging<CommonMessageDetailsBO, GridPaging> myGridData = new GridViewDataNPaging<CommonMessageDetailsBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            CommonMessageDA messageDa = new CommonMessageDA();

            //userInformationBO.GridViewPageSize = 2;
            List<CommonMessageDetailsBO> inbox = messageDa.GetMessageInbox(userInformationBO.UserId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(inbox, totalRecords);
            myGridData.GridBody = GridBody(inbox);

            return myGridData;
        }

        public static string GridBody(List<CommonMessageDetailsBO> inbox)
        {
            string body = string.Empty;

            string time = string.Empty, mDate = string.Empty;
            string readenMessageColor = "#E5E5E5";

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (CommonMessageDetailsBO md in inbox)
            {
                time = md.MessageDate.ToString(userInformationBO.TimeFormat);
                mDate = md.MessageDate.ToString(userInformationBO.ServerDateFormat);

                readenMessageColor = md.IsReaden ? "#F5F5F5" : string.Empty;

                //EncryptionHelper encryptionHelper = new EncryptionHelper();
                //string encryptData = encryptionHelper.Encrypt(md.MessageId.ToString() + "," + md.MessageDetailsId.ToString());

                body += "<tr style='background-color:" + readenMessageColor + "; cursor:pointer;' onclick = 'LoadMessageDetails(" + md.MessageId + "," + md.MessageDetailsId + ")'>" +
                            "<td class='span3' style= 'min-height:19px; width:23;'>" + md.UserName + "</td>" +
                            "<td class='span7' style= 'min-height:19px; width:62;'>" +
                                md.Subjects + " - " +
                                            (md.MessageBody.Length > 20 ? (md.MessageBody.Substring(0, 20) + " ...") : md.MessageBody)
                                     +
                            "</td>" +
                            "<td class='span2' style= 'min-height:19px; width:15;'>" + mDate + " " + time + "</td>" +
                        "</<tr>";
            }

            if (string.IsNullOrEmpty(body))
            {
                body += "<tr>" +
                           "<td colspan=\"3\">No Message in Message Box</td>" +
                        "</tr>";
            }

            return body;
        }

        [WebMethod]
        public static ReturnInfo LoadMessageDetails(Int64 msgId, Int64 msgDetailsId)
        {
            ReturnInfo rtnInf = new ReturnInfo();
            int totalMessage = 0;
            string messageBrief = string.Empty;

            try
            {
                CommonMessageDA messageDa = new CommonMessageDA();
                CommonMessageDetailsBO messageDetails = new CommonMessageDetailsBO();

                messageDetails = messageDa.GetMessageDetailsById(msgId, msgDetailsId);

                ArrayList arr = new ArrayList();
                arr.Add(messageDetails.Subjects);
                arr.Add(messageDetails.MessageBody);

                if (!messageDetails.IsReaden)
                {
                    messageDa.UpdateMessageDetails(messageDetails.MessageId, messageDetails.MessageDetailsId);
                    messageBrief = MessageForTopBar(out totalMessage);
                }

                arr.Add(messageBrief);
                arr.Add(totalMessage.ToString());

                rtnInf.IsSuccess = true;
                rtnInf.Arr = arr;
            }
            catch (Exception ex)
            {
                rtnInf.IsSuccess = false;
                rtnInf.AlertMessage = CommonHelper.AlertInfo("Message Load Failed. Please Try Again.", AlertType.Error);
            }

            return rtnInf;
        }

        private static string MessageForTopBar(out int totalMessage)
        {
            Int16 TotalUnreadMessage = 0;

            HMUtility hmUtility = new HMUtility();
            CommonMessageDA messageDa = new CommonMessageDA();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            messageDetails = messageDa.GetMessageDetailsByUserId(userInformationBO.UserId, null, 10, out TotalUnreadMessage);

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
                                "<div class='row-fluid'>" +
                                "<div class='span3' style= 'min-height:19px; margin-left: 1%;'>" + md.UserName + "</div>" +
                                "<div class='span7' style= 'min-height:19px; margin-left: 1%;'>" + md.Subjects +
                                " - " + (md.MessageBody.Length > 10 ? (md.MessageBody.Substring(0, 10) + " ...") : md.MessageBody) +
                                "</div>" +
                                "<div class='span2' style= 'min-height:19px; margin-left: 1%;'>" + mDate + " " + time + "</div>" +
                                "</div>" +
                               "</a></li>";

                rowCount++;
            }

            if (string.IsNullOrEmpty(messageBrief))
            {
                messageBrief += "<li><a tabindex='-1' style = 'margin: 3px 5px 5px 1px; padding:0;' href='javascript:void()'>" +
                                    "<div class='row-fluid'>" +
                                    "<div class='span12'>No Message in Message Box</div>" +
                                    "</div>" +
                                   "</a></li>";
            }

            totalMessage = TotalUnreadMessage;

            return messageBrief;
        }
    }
}