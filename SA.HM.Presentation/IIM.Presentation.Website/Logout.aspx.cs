using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website
{
    public partial class Logout : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();       
        protected void Page_Load(object sender, EventArgs e)
        {
            string queryStringId = Request.QueryString["LoginType"];

            UserInformationBO userInformation = new UserInformationBO();
            UserInformationBO userInformationBO = new UserInformationBO();

            if (queryStringId == "MainLogin")
            {
                userInformation = hmUtility.GetCurrentApplicationUserInfo();
            }
            else if (queryStringId == "RestaurentLogin")
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            }

            if (queryStringId == "MainLogin")
            {
                Session.Remove("HomeMenuShowHide");
                Session.Remove("ReportMenuShowHide");
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Logout.ToString(), EntityTypeEnum.EntityType.Logout.ToString(), userInformation.UserInfoId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Logout));
                Session["UserInformationBOSession"] = null;                
                Response.Redirect("Login.aspx");
            }
            else if (queryStringId == "RestaurentLogin")
            {
                //Session["EmployeeInformationBOSession"] = null;
                //Response.Redirect("/Restaurant/Login.aspx");

                Session.Remove("HomeMenuShowHide");
                Session.Remove("ReportMenuShowHide");
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Logout.ToString(), EntityTypeEnum.EntityType.Logout.ToString(), userInformation.UserInfoId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Logout));
                Session["UserInformationBOSession"] = null;
                Response.Redirect("/POS/Login.aspx");
            }
            else
            {
                Session["UserInformationBOSession"] = null;
                Session["EmployeeInformationBOSession"] = null;
                Response.Redirect("Login.aspx");
            }

        }
    }
}