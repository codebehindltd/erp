using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using System.Text.RegularExpressions;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class Login : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility(); 
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoginBackGroudImage();
                SiteTitle.InnerText = System.Web.Configuration.WebConfigurationManager.AppSettings["InnboardTitleHead"].ToString();
                CopyrightInfoName.InnerText = System.Web.Configuration.WebConfigurationManager.AppSettings["InnboardTitleHead"].ToString();
                this.txtUserId.Focus();
            }
        }        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                return;
            }            

            Session["IsOnlyRetailPOS"] = null;
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            int IsOnlyRetailPOS = costCentreTabDA.CheckIsOnlyRetailPOS();
            Session["IsOnlyRetailPOS"] = IsOnlyRetailPOS;

            Session["SoftwareLicenseExpiredNotification"] = null;
            if (this.txtUserId.Text.Trim() == "superadmin")
            {
                Session["SoftwareModulePermissionList"] = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40";
            }
            else
            {
                if (!CheckingExpireDate())
                {
                    return;
                }
            }

            HMCommonSetupBO costCenterSelection = new HMCommonSetupBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();

            userInformationBO.UserId = this.txtUserId.Text.Trim();
            userInformationBO.UserPassword = this.passwordInput.Text.Trim();

            UserInformationBO userInformation = userInformationDA.GetUserInformationByUserNameNId(userInformationBO.UserId, userInformationBO.UserPassword);
            if (!userInformation.ActiveStat)
            {
                isMessageBoxEnable = 1;
                this.lblMessage.Text = "User name or password you entered is invalid.";
                this.txtUserId.Focus();
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            Session["UserAdminAuthorizationBOSession"] = null;
            List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
            adminAuthorizationList = userInformationDA.GetSecurityUserAdminAuthorizationByUserInfoId(userInformation.UserInfoId);
            if (adminAuthorizationList != null)
            {
                if (adminAuthorizationList.Count > 0)
                {
                    Session.Add("UserAdminAuthorizationBOSession", adminAuthorizationList);
                }
            }

            InvItemStockInformationLogProcess(userInformation.UserInfoId);

            RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
            RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();
            restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpId(userInformation.UserInfoId);

            if (restaurantBearerBO.UserInfoId != 0)
            {
                userInformation.IsItemCanEditDelete = restaurantBearerBO.IsItemCanEditDelete;
            }
            else
            { userInformation.IsItemCanEditDelete = false; }
            
            if (this.txtUserId.Text.Trim().Equals(userInformation.UserId))
            {
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
                homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardHomePage", "InnboardHomePage");

                HMCommonSetupBO oldMenuEnbale = new HMCommonSetupBO();
                oldMenuEnbale = commonSetupDA.GetCommonConfigurationInfo("IsOldMenuEnable", "IsOldMenuEnable");

                userInformation.IsMenuSearchRoomSearchRoomStatisticsInfoEnable = false;
                HMCommonSetupBO isMenuSearchRoomSearchRoomStatisticsInfoEnableBO = new HMCommonSetupBO();
                isMenuSearchRoomSearchRoomStatisticsInfoEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsMenuSearchRoomSearchRoomStatisticsInfoEnable", "IsMenuSearchRoomSearchRoomStatisticsInfoEnable");
                if(isMenuSearchRoomSearchRoomStatisticsInfoEnableBO != null)
                {
                    if(isMenuSearchRoomSearchRoomStatisticsInfoEnableBO.SetupValue != "0")
                    {
                        userInformation.IsMenuSearchRoomSearchRoomStatisticsInfoEnable = true;
                    }
                }

                HMCommonSetupBO isItemAverageCostUpdateEnableBO = new HMCommonSetupBO();
                isItemAverageCostUpdateEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsItemAverageCostUpdateEnable", "IsItemAverageCostUpdateEnable");
                if (isItemAverageCostUpdateEnableBO != null)
                {
                    if (isItemAverageCostUpdateEnableBO.SetupValue != "0")
                    {
                        userInformation.IsItemAverageCostUpdateEnable = true;
                    }
                }

                if (userInformation.UserGroupId == 1)
                {
                    userInformation.InnboardHomePage = homePageSetupBO.SetupValue;
                }
                else if(userInformation.SupplierId > 0)
                {
                    userInformation.InnboardHomePage = "/PurchaseManagment/QuotaionFeedback.aspx";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(userInformation.InnboardHomePage))
                    {
                        userInformation.InnboardHomePage = "/HMCommon/frmHMHome.aspx";
                    }
                }

                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Innboard", "InnboardTitleHead");
                userInformation.SiteTitle = commonSetupBO.SetupValue;

                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardReportFooterPoweredByInfo", "InnboardReportFooterPoweredByInfo");
                userInformation.FooterPoweredByInfo = commonSetupBO.SetupValue;

                HMCommonDA hmCommonQrImageDA = new HMCommonDA();
                commonSetupBO.SetupValue.Replace("Powered by: ", "");
                userInformation.PoweredByQrCode = hmCommonQrImageDA.GenerateQrCode(commonSetupBO.SetupValue.Replace("Powered by: ", ""));

                string companyName = string.Empty;
                string companyAddress = string.Empty;
                string webAddress = string.Empty;
                string telephoneNumber = string.Empty;
                string hotLineNumber = string.Empty;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> fileCompany = companyDA.GetCompanyInfo();
                if (fileCompany[0].CompanyId > 0)
                {
                    companyName = fileCompany[0].CompanyName;
                    companyAddress = fileCompany[0].CompanyAddress;
                    webAddress = fileCompany[0].WebAddress;
                    telephoneNumber = fileCompany[0].Telephone;
                    hotLineNumber = fileCompany[0].HotLineNumber;
                    string companyQrCode = companyName + "; " + webAddress + "; " + telephoneNumber + ";";
                    userInformation.CompanyQrCode = hmCommonQrImageDA.GenerateQrCode(companyQrCode);
                }

                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardGridView", "GridViewPageSize");
                userInformation.GridViewPageSize = Convert.ToInt32(commonSetupBO.SetupValue);

                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardGridView", "GridViewPageLink");
                userInformation.GridViewPageLink = Convert.ToInt32(commonSetupBO.SetupValue);

                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardMessage", "MessageHideTimer");
                userInformation.MessageHideTimer = Convert.ToInt32(commonSetupBO.SetupValue);

                HMCommonSetupBO isInnboardVatEnableBO = new HMCommonSetupBO();
                isInnboardVatEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardVatEnable", "IsInnboardVatEnable");
                userInformation.IsInnboardVatEnable = Convert.ToInt32(isInnboardVatEnableBO.SetupValue);

                HMCommonSetupBO isInnboardServiceChargeEnableBO = new HMCommonSetupBO();
                isInnboardServiceChargeEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardServiceChargeEnable", "IsInnboardServiceChargeEnable");
                userInformation.IsInnboardServiceChargeEnable = Convert.ToInt32(isInnboardServiceChargeEnableBO.SetupValue);

                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardTimeFormat");
                userInformation.TimeFormat = commonSetupBO.SetupValue;

                if (userInformation.UserInfoId != 1)
                {
                    if (restaurantBearerBO != null)
                    {
                        if (restaurantBearerBO.BearerId > 0)
                        {
                            userInformation.WorkingCostCenterId = restaurantBearerBO.CostCenterId;
                        }
                    }
                }

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

                if (restaurantBearerBO.UserInfoId != 0 && restaurantBearerBO.BearerId != 0)
                {
                    userInformation.BearerId = restaurantBearerBO.BearerId;
                    userInformation.IsBearer = restaurantBearerBO.IsBearer;
                    userInformation.IsRestaurantBillCanSettle = restaurantBearerBO.IsRestaurantBillCanSettle;
                    userInformation.IsItemSearchEnable = restaurantBearerBO.IsItemSearchEnable;

                    costCenterSelection = commonSetupDA.GetCommonConfigurationInfo("IsRedirectRestaurantCostCenterSelectionOld", "IsRedirectRestaurantCostCenterSelectionOld");

                    HMCommonSetupBO entityAttendanceBO = new HMCommonSetupBO();
                    entityAttendanceBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithPayrollAttendance", "IsRestaurantIntegrateWithPayrollAttendance");

                    if (entityAttendanceBO != null)
                    {
                        if (entityAttendanceBO.SetupValue == "1")
                        {
                            HMCommonDA hmCommonDA = new HMCommonDA();
                            Boolean postingStatus = hmCommonDA.SaveUpdateForRestaurantEmployeeAttendance(userInformation.BearerId, 1);
                        }
                    }
                }

                Session.Add("UserInformationBOSession", userInformation);
                Session.Add("UserEmployeeMapId", userInformation.EmpId);

                ActivityLogsBO activityLogBO = new ActivityLogsBO();
                ActivityLogsDA activityLogDA = new ActivityLogsDA();
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Login.ToString(), EntityTypeEnum.EntityType.Login.ToString(), userInformation.UserInfoId,
                "", hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Login));

                //CompanyDA companyDA = new CompanyDA();
                //List<CompanyBO> files = companyDA.GetCompanyInfo();
                if (fileCompany.Count > 0)
                {
                    if (fileCompany[0].CompanyId > 0)
                    {
                        Response.Redirect("/HMCommon/frmHMHome.aspx");
                    }
                    else
                    {
                        Response.Redirect("/HMCommon/frmCompany.aspx");
                    }
                }
                else
                {
                    Response.Redirect("/HMCommon/frmCompany.aspx");
                }
            }
            else
            {
                isMessageBoxEnable = 1;
                this.lblMessage.Text = "User Name or Password is not valid";
                this.txtUserId.Focus();
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        //*********************** User Defined Function *******************//
        private bool IsValid()
        {
            bool status = true;
            if (txtUserId.Text.IndexOf('<') != -1 || txtUserId.Text.IndexOf('>') != -1)
            {
                isMessageBoxEnable = 1;
                this.lblMessage.Text = "potentially dangerous user name.";
                this.txtUserId.Focus();
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                status = false;
            }

            if (txtUserId.Text.Length < 0 || txtUserId.Text.Length > 20 || passwordInput.Text.Length < 0 || passwordInput.Text.Length > 20)
            {
                isMessageBoxEnable = 1;
                this.lblMessage.Text = "user name or password you entered is invalid.";
                this.txtUserId.Focus();
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                status = false;
            }

            return status;
        }
        private bool CheckingExpireDate()
        {
            bool status = true;
            HMUtility hmUtility = new HMUtility();
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            string encryptFieldType = encryptionHelper.Encrypt(hmUtility.GetExpireInformation("FieldType"));

            string decryptValue = string.Empty;
            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customFieldForCash = new CustomFieldBO();
            customFieldForCash = hmCommonDA.GetCustomFieldByFieldName(encryptFieldType);
            if (customFieldForCash != null)
            {
                if (customFieldForCash.FieldId > 0)
                {
                    try
                    {
                        decryptValue = encryptionHelper.Decrypt(customFieldForCash.FieldValue);
                        string[] separators = { "Innb0@rd" };
                        string[] decryptWordList = decryptValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        
                        DateTime expireDate = Convert.ToDateTime(decryptWordList[1]); // // 21/06/2018 = MM/DD/YYYY
                        Session["SoftwareModulePermissionList"] = decryptWordList[2];
                        
                        if (expireDate.Date < DateTime.Now.Date)
                        {
                            isMessageBoxEnable = 1;
                            this.lblMessage.Text = "Your license has expired.";
                            this.txtUserId.Focus();
                            this.lblMessage.ForeColor = System.Drawing.Color.Red;
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        isMessageBoxEnable = 1;
                        this.lblMessage.Text = "Your license has expired.";
                        this.txtUserId.Focus();
                        this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        status = false;
                    }
                }
                else
                {
                    isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Your license has expired.";
                    this.txtUserId.Focus();
                    this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    status = false;
                }
            }
            else
            {
                isMessageBoxEnable = 1;
                this.lblMessage.Text = "Your license has expired.";
                this.txtUserId.Focus();
                this.lblMessage.ForeColor = System.Drawing.Color.Red;
                status = false;
            }

            return status;
        }
        private void LoginBackGroudImage()
        {
            try
            {
                string loginBackGroudImage = System.Web.Configuration.WebConfigurationManager.AppSettings["LoginBackgroudImage"].ToString();
                LoginBackgroudDiv.Style.Add("background-image", "url(/Images/" + loginBackGroudImage + ".jpg)");
            }
            catch (Exception ex)
            {
                LoginBackgroudDiv.Style.Add("background-image", "url(/Images/LogInPageDataGrid.jpg)");
            }
        }
        private void InvItemStockInformationLogProcess(int userInfoId)
        {
            UserInformationDA userInformationDA = new UserInformationDA();
            Boolean status = userInformationDA.InvItemStockInformationLogProcess(DateTime.Now, userInfoId);
            if (status)
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.InventoryItemStockLog.ToString(), 0, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InventoryItemStockLog));
        }
        public ActivityLogsBO GetActivityLog()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ActivityLogsBO logBO = new ActivityLogsBO();
            logBO.CreatedBy = userInformationBO.UserInfoId;
            logBO.ActivityType = ActivityTypeEnum.ActivityType.Login.ToString();
            logBO.EntityId = userInformationBO.UserInfoId;
            logBO.EntityType = "UserInformation";
            logBO.Remarks = "First Test";
            return logBO;
        }
    }
}