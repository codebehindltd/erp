using HotelManagement.Entity.UserInformation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Common
{
    public partial class InnboardEmptyDesign : System.Web.UI.MasterPage
    {
        HMUtility hmUtility = new HMUtility();
        protected string isMenuCollupse = string.Empty;
        public string innBoardDateFormat = string.Empty, innBoardTimeFormat = string.Empty, isOldMenuEnable = string.Empty, dayOpenDate = string.Empty;
        protected int isSoftwareLicenseExpiredNotificationMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            innBoardDateFormat = userInformationBO.ClientDateFormat;
            innBoardTimeFormat = userInformationBO.TimeFormat;
            isOldMenuEnable = userInformationBO.IsOldMenuEnable;
            dayOpenDate = userInformationBO.DayOpenDate.ToString(userInformationBO.ServerDateFormat);
            hfUserInfoObj.Value = JsonConvert.SerializeObject(userInformationBO);
        }
    }
}