using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Text.RegularExpressions;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website
{
    public partial class InnBoard : System.Web.UI.MasterPage
    {
        public string innBoardDateFormat = string.Empty, innBoardTimeFormat = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility utility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = utility.GetCurrentApplicationUserInfo();

            innBoardDateFormat = userInformationBO.ClientDateFormat;
            innBoardTimeFormat = userInformationBO.TimeFormat;
        }
    }
}