using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Security;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.Security;
using System.Collections;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmMenuGroup : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadIcon();
                LoadCommonDropDownHiddenField();
                CheckPermission();
            }
        }
        private void CheckPermission()
        {
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadIcon()
        {
            List<MenuGroupNLinkIconBO> iconList = new List<MenuGroupNLinkIconBO>();
            MenuGroupNLinkIconDA iconDA = new MenuGroupNLinkIconDA();

            iconList = iconDA.GetIconList();
            hfGroupIconList.Value = JsonConvert.SerializeObject(iconList);                        
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        [WebMethod]
        public static ArrayList GetLinkByModuleId(int userGroupId, int userId, long menuGroupId, int moduleId)
        {
            string linksGrid = string.Empty;
            MenuDA menuDa = new MenuDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();
            menuLinks = menuDa.GetMenuLinksByModuleId(moduleId);

            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
            menuWiseLinks = menuDa.GetMenuLinksByModuleIdUserGroupNMenuGroupId(userGroupId, menuGroupId, moduleId);

            ArrayList arr = new ArrayList();
            arr.Add(new { MenuLinks = menuLinks, MenuWisePermitedLinks = menuWiseLinks });

            return arr;
        }
        [WebMethod]
        public static ReturnInfo SaveMenuGroup(MenuGroupBO menuGroup)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                HMUtility hmUtility = new HMUtility();
                MenuDA menuDa = new MenuDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (menuGroup.MenuGroupId == 0)
                {
                    menuGroup.CreatedBy = userInformationBO.UserInfoId;
                    rtninf.IsSuccess = menuDa.SaveMenuGroup(menuGroup);
                    if (rtninf.IsSuccess)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                else
                {
                    menuGroup.LastModifiedBy = userInformationBO.UserInfoId;
                    rtninf.IsSuccess = menuDa.UpdateMenuGroup(menuGroup);
                    if (rtninf.IsSuccess)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            return rtninf;
        }
        [WebMethod]
        public static GridViewDataNPaging<MenuGroupBO, GridPaging> LoadMenuGroupInfo(string menuGroupName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<MenuGroupBO, GridPaging> myGridData = new GridViewDataNPaging<MenuGroupBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            MenuDA da = new MenuDA();
            List<MenuGroupBO> menuLinks = new List<MenuGroupBO>();

            menuLinks = da.GetMenuGroupForSearch(menuGroupName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(menuLinks, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static MenuGroupBO GetMenuGroupInfo(long menuGroupId)
        {
            MenuGroupBO menuGroupBO = new MenuGroupBO();
            MenuDA menuDA = new MenuDA();

            menuGroupBO = menuDA.GetMenuGroupById(menuGroupId);
            return menuGroupBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteMenuGroupInfo(int menuGroupId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("SecurityMenuGroup", "MenuGroupId", menuGroupId);

                if (status)
                {
                    //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rtninf;
        }
    }
}
