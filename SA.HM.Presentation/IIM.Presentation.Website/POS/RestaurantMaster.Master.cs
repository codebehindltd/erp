using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class RestaurantMaster : System.Web.UI.MasterPage
    {
        public string innBoardDateFormat = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SiteTitle.Text = userInformationBO.SiteTitle;
            innBoardDateFormat = userInformationBO.ClientDateFormat;

            lblLoggedInUser.Text = "Welcome " + userInformationBO.DisplayName;

            if (Session["TableInformation"] != null)
            {
                int tableId = Convert.ToInt32(Session["TableInformation"]);
            }
        }
    }
}