using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class GuestBirthdayNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private static bool SendMail(string emailAddress, string guestName)
        {
            Email email;
            bool status = false;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;

            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                email = new Email()
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = emailAddress,
                    Subject = "Happy Birthday",
                    Host = dataArray[2],
                    Port = dataArray[3],
                    TempleteName = HMConstants.EmailTemplates.BirthdayWish
                };

                var tokens = new Dictionary<string, string>
                         {
                             {"NAME", guestName},
                             {"COMPANYNAME",null}
                };
                try
                {
                    status = EmailHelper.SendEmail(email, tokens);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return status;
        }

        [WebMethod]
        public static GridViewDataNPaging<BirthdayNotificationViewBO, GridPaging> GetGuestListForBirthdayWish(string date, string guestType, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GuestBirthdayNotificationDA birthdayDA = new GuestBirthdayNotificationDA();
            AllReportDA reportDA = new AllReportDA();
            List<BirthdayNotificationViewBO> list = new List<BirthdayNotificationViewBO>();

            GridViewDataNPaging<BirthdayNotificationViewBO, GridPaging> myGridData = new GridViewDataNPaging<BirthdayNotificationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            DateTime searchDate;

            if (!string.IsNullOrWhiteSpace(date))
                searchDate = hmUtility.GetDateTimeFromString(date, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            else
                searchDate = DateTime.Now;

            list = birthdayDA.GetGuestBirthdayNotificationStatus(searchDate, guestType, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(list, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo SendEmailToBirthdayWish(List<BirthdayNotificationViewBO> saveGuestList, List<BirthdayNotificationViewBO> updateGuestList)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            bool status = false;

            GuestBirthdayNotificationDA birthdayDA = new GuestBirthdayNotificationDA();
            try
            {
                if (saveGuestList != null)
                {
                    for (int i = 0; i < saveGuestList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(saveGuestList[i].GuestEmail))
                        {
                            status = SendMail(saveGuestList[i].GuestEmail, saveGuestList[i].GuestName);
                        }
                        if (!status)
                            saveGuestList.Remove(saveGuestList[i]);
                    }
                    if (saveGuestList.Count > 0)
                        birthdayDA.SaveGuestBirthdayNotificationStatus(saveGuestList);
                }

                if (updateGuestList != null)
                {
                    for (int i = 0; i < updateGuestList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(updateGuestList[i].GuestEmail))
                        {
                            status = SendMail(updateGuestList[i].GuestEmail, updateGuestList[i].GuestName);
                        }
                        if (!status)
                            updateGuestList.Remove(updateGuestList[i]);
                    }
                    if (updateGuestList.Count > 0)
                        birthdayDA.UpdateGuestBirthdayNotificationStatus(updateGuestList);
                }
                if (status)
                {
                    returnInfo.IsSuccess = true;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnInfo;
        }

    }
}