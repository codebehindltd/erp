using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmHMHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //this.lblCurrentUser.Text = userInformationBO.UserName;

            if (userInformationBO.InnboardHomePage != "/HMCommon/frmHMHome.aspx")
            {
                Response.Redirect(userInformationBO.InnboardHomePage);
            }
        }
    }
}