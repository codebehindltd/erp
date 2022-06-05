using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Common
{
    public partial class IRReportViewer : System.Web.UI.MasterPage
    {
        public string innBoardDateFormat = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            innBoardDateFormat = userInformationBO.ClientDateFormat;
        }
    }
}