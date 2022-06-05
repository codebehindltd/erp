using HotelManagement.Data.HotelManagement;
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
    public partial class frmInHouseGuestBreakfast : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
            }
        }

        [WebMethod]
        public static List<GuestInformationBO> GetInHouseGuestBreakfastInfo()
        {
            GuestInformationDA guestDa = new GuestInformationDA();
            List<GuestInformationBO> guestBreakfastInfo = new List<GuestInformationBO>();
            guestBreakfastInfo = guestDa.GetInHouseGuestBreakfastInfo();
            
            return guestBreakfastInfo;
        }

        [WebMethod]
        public static bool SaveGuestBreakfastCompletedList(List<GuestInformationBO> boList)
        {
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                GuestInformationDA da = new GuestInformationDA();
                da.SaveGuestInhouseBreakfastInfo(boList, userInformationBO);
                return true;
            }
            catch(Exception exp)
            {
                return false;
            }
        }
    }
}