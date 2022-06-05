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
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.Restaurant;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Restaurant;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class Login : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserGroup();
            }

            SiteTitle.InnerText = System.Web.Configuration.WebConfigurationManager.AppSettings["InnboardTitleHead"].ToString();

            //string queryStringId = Request.QueryString["EmpId"];
            //if (queryStringId != null)
            //{
            //    EmployeeBO empBO = new EmployeeBO();
            //    EmployeeDA empDA = new EmployeeDA();
            //    int userID = Int32.Parse(queryStringId);
            //    empBO = empDA.GetEmployeeInfoById(userID);

            //RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
            //RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();
            //restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(empBO.EmpId, 1);

            //    if (empBO.EmpId != 0 && restaurantBearerBO.BearerId != 0)
            //    {
            //        empBO.BearerId = restaurantBearerBO.BearerId;
            //        empBO.IsRestaurantBillCanSettle = restaurantBearerBO.IsRestaurantBillCanSettle;
            //        empBO.IsItemCanEditDelete = restaurantBearerBO.IsItemCanEditDelete;

            //        HMUtility hmUtility = new HMUtility();
            //        empBO.ClientDateFormat = hmUtility.GetFormat(false);
            //        empBO.ServerDateFormat = hmUtility.GetFormat(true);

            //        HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            //        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            //        commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Innboard", "InnboardTitleHead");
            //        empBO.SiteTitle = commonSetupBO.SetupValue;

            //        HMCommonSetupBO costCenterSelection = new HMCommonSetupBO();
            //        costCenterSelection = commonSetupDA.GetCommonConfigurationInfo("IsRedirectRestaurantCostCenterSelectionOld", "IsRedirectRestaurantCostCenterSelectionOld");

            //        HMCommonSetupBO entityAttendanceBO = new HMCommonSetupBO();
            //        entityAttendanceBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithPayrollAttendance", "IsRestaurantIntegrateWithPayrollAttendance");

            //        if (entityAttendanceBO != null)
            //        {
            //            if (entityAttendanceBO.SetupValue == "0")
            //            {
            //                HMCommonDA hmCommonDA = new HMCommonDA();
            //                Boolean postingStatus = hmCommonDA.SaveUpdateForRestaurantEmployeeAttendance(empBO.BearerId, 1);
            //            }
            //        }

            //        HMCommonDA comonDa = new HMCommonDA();
            //        empBO.DayOpenDate = comonDa.GetModuleWisePreviousDayTransaction(DateTime.Now);

            //        empBO.ClientDateFormat = hmUtility.GetFormat(false);
            //        empBO.ServerDateFormat = hmUtility.GetFormat(true);

            //        Session.Add("EmployeeInformationBOSession", empBO);

            //        if (costCenterSelection.SetupValue == "1")
            //        {
            //            Response.Redirect("frmCostCenterSelection.aspx");
            //        }
            //        else
            //        {
            //            Response.Redirect("frmCostCenterSelectionForAll.aspx");
            //        }
            //    }
            //    else
            //    {
            //        Response.Redirect("Login.aspx");
            //    }
            //}
        }
        private void LoadUserGroup()
        {
            List<UserGroupBO> userGroupList = new List<UserGroupBO>();
            UserGroupDA userGroupda = new UserGroupDA();
            userGroupList = userGroupda.GetRestaurantPermittedUserGroupInfo();

            hfGroupList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(userGroupList);
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
                            //isMessageBoxEnable = 1;
                            //this.lblMessage.Text = "Your license has expired.";
                            //this.txtUserId.Focus();
                            //this.lblMessage.ForeColor = System.Drawing.Color.Red;
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //isMessageBoxEnable = 1;
                        //this.lblMessage.Text = "Your license has expired.";
                        //this.txtUserId.Focus();
                        //this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        status = false;
                    }
                }
                else
                {
                    //isMessageBoxEnable = 1;
                    //this.lblMessage.Text = "Your license has expired.";
                    //this.txtUserId.Focus();
                    //this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    status = false;
                }
            }
            else
            {
                //isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Your license has expired.";
                //this.txtUserId.Focus();
                //this.lblMessage.ForeColor = System.Drawing.Color.Red;
                status = false;
            }

            return status;
        }

        [WebMethod]
        public static ReturnInfo Authenticate(string txtUserId, string txtUserPassword)
        {
            bool authenticationStatus = true;
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();

            try
            {
                HttpContext.Current.Session["SoftwareLicenseExpiredNotification"] = null;

                if (txtUserId.Trim() == "superadmin")
                {
                    HttpContext.Current.Session["SoftwareModulePermissionList"] = "1,2,3,4,5,6,7,8,9,10,11,12,13";
                }
                else
                {
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
                                HttpContext.Current.Session["SoftwareModulePermissionList"] = decryptWordList[2];

                                if (expireDate.Date < DateTime.Now.Date)
                                {
                                    rtninf.IsSuccess = false;
                                    rtninf.AlertMessage = CommonHelper.AlertInfo("Your license has expired.", AlertType.Warning);
                                    authenticationStatus = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                rtninf.IsSuccess = false;
                                rtninf.AlertMessage = CommonHelper.AlertInfo("Your license has expired.", AlertType.Warning);
                                authenticationStatus = false;
                            }
                        }
                        else
                        {
                            rtninf.IsSuccess = false;
                            rtninf.AlertMessage = CommonHelper.AlertInfo("Your license has expired.", AlertType.Warning);
                            authenticationStatus = false;
                        }
                    }
                }

                if (authenticationStatus == true)
                {
                    userInformationBO.UserId = txtUserId.Trim();
                    userInformationBO.UserPassword = txtUserPassword.Trim();
                    UserInformationBO userInformation = userInformationDA.GetUserInformationByUserNameNId(userInformationBO.UserId, userInformationBO.UserPassword);
                    if (!userInformation.ActiveStat)
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("User name or password you entered is invalid.", AlertType.Warning);
                        authenticationStatus = false;
                    }
                    else
                    {
                        RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                        RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();
                        restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpId(userInformation.UserInfoId);

                        if (txtUserId.Trim().Equals(userInformation.UserId))
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

                            if (isMenuSearchRoomSearchRoomStatisticsInfoEnableBO != null)
                            {
                                if (isMenuSearchRoomSearchRoomStatisticsInfoEnableBO.SetupValue != "0")
                                {
                                    userInformation.IsMenuSearchRoomSearchRoomStatisticsInfoEnable = true;
                                }
                            }

                            if (userInformation.UserGroupId == 1)
                            {
                                userInformation.InnboardHomePage = homePageSetupBO.SetupValue;
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(userInformation.InnboardHomePage))
                                {
                                    userInformation.InnboardHomePage = "/HMCommon/frmHMHome.aspx";
                                }
                            }

                            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardTimeFormat");
                            userInformation.TimeFormat = commonSetupBO.SetupValue;

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

                            HMCommonSetupBO isInnboardServiceChargeEnableBO = new HMCommonSetupBO();
                            isInnboardServiceChargeEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardServiceChargeEnable", "IsInnboardServiceChargeEnable");
                            userInformation.IsInnboardServiceChargeEnable = Convert.ToInt32(isInnboardServiceChargeEnableBO.SetupValue);

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
                            userInformation.IsOldMenuEnable = oldMenuEnbale.SetupValue.ToString();

                            HMCommonDA comonDa = new HMCommonDA();
                            userInformation.DayOpenDate = comonDa.GetModuleWisePreviousDayTransaction("Restaurant");

                            if (oldMenuEnbale.SetupValue != "1")
                            {
                                userInformation.UserMenu = new MenuProcess().UserMainMenu(userInformation.UserGroupId);
                                userInformation.UserReportMenu = new MenuProcess().UserReportMenu(userInformation.UserGroupId);
                            }

                            //-------------Restaurnat Related Options
                            HMCommonSetupBO costCenterSelection = new HMCommonSetupBO();
                            userInformation.IsItemCanEditDelete = restaurantBearerBO.IsItemCanEditDelete;

                            if (restaurantBearerBO.UserInfoId != 0 && restaurantBearerBO.BearerId != 0)
                            {
                                userInformation.BearerId = restaurantBearerBO.BearerId;
                                userInformation.IsBearer = restaurantBearerBO.IsBearer;
                                userInformation.IsItemSearchEnable = restaurantBearerBO.IsItemSearchEnable;
                                userInformation.IsRestaurantBillCanSettle = restaurantBearerBO.IsRestaurantBillCanSettle;
                                costCenterSelection = commonSetupDA.GetCommonConfigurationInfo("IsRedirectRestaurantCostCenterSelectionOld", "IsRedirectRestaurantCostCenterSelectionOld");

                                HMCommonSetupBO entityAttendanceBO = new HMCommonSetupBO();
                                entityAttendanceBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithPayrollAttendance", "IsRestaurantIntegrateWithPayrollAttendance");

                                if (entityAttendanceBO != null)
                                {
                                    if (entityAttendanceBO.SetupValue == "0")
                                    {
                                        HMCommonDA hmCommonDA = new HMCommonDA();
                                        Boolean postingStatus = hmCommonDA.SaveUpdateForRestaurantEmployeeAttendance(userInformation.BearerId, 1);
                                    }
                                }
                            }

                            //---------- Restaurant -----------------
                            HttpContext.Current.Session.Add("UserInformationBOSession", userInformation);
                            ActivityLogsBO activityLogBO = new ActivityLogsBO();
                            ActivityLogsDA activityLogDA = new ActivityLogsDA();
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Login.ToString(), EntityTypeEnum.EntityType.Login.ToString(), userInformation.UserInfoId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Login));

                            if (costCenterSelection.SetupValue == "1")
                            {
                                rtninf.RedirectUrl = "frmCostCenterSelection.aspx";
                                rtninf.IsSuccess = true;
                                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.SucceesfulLogin, AlertType.Warning);
                            }
                            else
                            {
                                string companyType = string.Empty;
                                CompanyDA companyDA = new CompanyDA();
                                List<CompanyBO> files = companyDA.GetCompanyInfo();
                                if (files[0].CompanyId > 0)
                                {
                                    companyType = files[0].CompanyType;
                                }

                                if (companyType == "RetailShop")
                                {
                                    rtninf.RedirectUrl = "frmRetailPos.aspx";
                                }
                                else
                                {
                                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                                }
                                rtninf.IsSuccess = true;
                                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.SucceesfulLogin, AlertType.Warning);
                            }
                        }
                        else
                        {
                            rtninf.IsSuccess = false;
                            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.ErrorFulLogin, AlertType.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.ErrorFulLogin, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static List<UserInformationBO> GetUserInformationByUserGroup(int userGroupId)
        {
            UserInformationDA userDa = new UserInformationDA();
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            userInformationList = userDa.GetUserInformationByUserGroupForTouchPanelLogin(userGroupId);

            return userInformationList;
        }
    }
}