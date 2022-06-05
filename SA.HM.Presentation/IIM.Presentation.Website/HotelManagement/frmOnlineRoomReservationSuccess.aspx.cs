using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmOnlineRoomReservationSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["ReservationSuccessMsg"];
                string[] param1 = param.Split(',');
                string reservationNo = param1[0];
                string reservationId = param1[1];
                if (!string.IsNullOrEmpty(param))
                {
                    lblSuccessMsg.Text = "Your reservation is successfully posted. Your reservation number is " + reservationNo + ". Please wait for confirmation.";
                    hfOnlineReservationId.Value = reservationId;
                }
            }
        }
    }
}