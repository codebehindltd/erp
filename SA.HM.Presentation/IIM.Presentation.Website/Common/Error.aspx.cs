using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Common
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            Page.Title = "Error occurred";
            string PreviousUri = Request["aspxerrorpath"];
            if (!string.IsNullOrEmpty(PreviousUri))
            {
                pnlError.Visible = true;
                hlinkPreviousPage.NavigateUrl = PreviousUri;
            }

            Exception ex = Server.GetLastError();
            UserInformationBO CurrentUser = Session["UserInformationBOSession"] as UserInformationBO;
        }
    }
}