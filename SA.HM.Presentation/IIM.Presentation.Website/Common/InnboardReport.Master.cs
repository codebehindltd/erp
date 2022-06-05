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

namespace HotelManagement.Presentation.Website.Common
{
    public partial class InnboardReport : System.Web.UI.MasterPage
    {
        HMUtility hmUtility = new HMUtility();
        public string innBoardDateFormat = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO != null)
            {
                innBoardDateFormat = userInformationBO.ClientDateFormat;
            }
        }
    }
}