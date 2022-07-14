using HotelManagement.Data.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.EmailTemplates
{
    public partial class EmailLandingPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string type = Request.QueryString["t"];
                string UserName = Request.QueryString["u"];
                string Password = Request.QueryString["p"];

                if (!string.IsNullOrWhiteSpace(type))
                {
                    if (!string.IsNullOrWhiteSpace(UserName))
                    {
                        if (!string.IsNullOrWhiteSpace(Password))
                        {
                            PermissionCheck(type, UserName, Password);

                        }
                        else
                        {
                            Response.Redirect("/Login.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("/Login.aspx");
                    }
                }
                else
                {
                    Response.Redirect("/Login.aspx");
                }

                
            }
        }

        private void PermissionCheck(string type , string UserName, string Password)
        {
            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO userInformation = userInformationDA.GetUserInformationByUserIdNHashPassword(UserName, Password);
            if (!userInformation.ActiveStat)
            {
                Response.Redirect("/Login.aspx");
            }

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Innboard", "InnboardTitleHead");
            userInformation.SiteTitle = commonSetupBO.SetupValue;

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardReportFooterPoweredByInfo", "InnboardReportFooterPoweredByInfo");
            userInformation.FooterPoweredByInfo = commonSetupBO.SetupValue;

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardGridView", "GridViewPageSize");
            userInformation.GridViewPageSize = Convert.ToInt32(commonSetupBO.SetupValue);

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardGridView", "GridViewPageLink");
            userInformation.GridViewPageLink = Convert.ToInt32(commonSetupBO.SetupValue);

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardMessage", "MessageHideTimer");
            userInformation.MessageHideTimer = Convert.ToInt32(commonSetupBO.SetupValue);

            HMCommonSetupBO isInnboardVatEnableBO = new HMCommonSetupBO();
            isInnboardVatEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardVatEnable", "IsInnboardVatEnable");
            userInformation.IsInnboardVatEnable = Convert.ToInt32(isInnboardVatEnableBO.SetupValue);

            HMCommonSetupBO oldMenuEnbale = new HMCommonSetupBO();
            oldMenuEnbale = commonSetupDA.GetCommonConfigurationInfo("IsOldMenuEnable", "IsOldMenuEnable");

            HMUtility hmUtility = new HMUtility();
            userInformation.ClientDateFormat = hmUtility.GetFormat(false);
            userInformation.ServerDateFormat = hmUtility.GetFormat(true);
            userInformation.TimeFormat = hmUtility.GetTimeFormat();

            userInformation.IsOldMenuEnable = oldMenuEnbale.SetupValue.ToString();

            HMCommonDA comonDa = new HMCommonDA();
            userInformation.DayOpenDate = comonDa.GetModuleWisePreviousDayTransaction("Restaurant");

            if (oldMenuEnbale.SetupValue != "1")
            {
                userInformation.UserMenu = new MenuProcess().UserMainMenu(userInformation.UserGroupId, userInformation.UserInfoId);
                userInformation.UserReportMenu = new MenuProcess().UserReportMenu(userInformation.UserGroupId, userInformation.UserInfoId);
            }

           

            if (userInformation.SupplierId > 0 && type=="s")
            {
                Session.Add("UserInformationBOSession", userInformation);
                Response.Redirect("/PurchaseManagment/QuotaionFeedback.aspx");
            }
            else
            {
                Response.Redirect("/Login.aspx");
            }

        }

    }
}