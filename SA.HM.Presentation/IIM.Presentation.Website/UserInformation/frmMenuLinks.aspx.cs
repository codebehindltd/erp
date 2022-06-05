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

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmMenuLinks : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCommonMenuModule();
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
        private void LoadCommonMenuModule()
        {
            MenuDA menuDa = new MenuDA();
            List<CommonModuleNameBO> menuGroup = new List<CommonModuleNameBO>();
            List<CommonModuleNameBO> permittedMenuGroup = new List<CommonModuleNameBO>();            
            menuGroup = menuDa.GetCommonMenuModule();

            string permissionList = Session["SoftwareModulePermissionList"].ToString();
            permissionList = "0," + permissionList;
            permittedMenuGroup = menuGroup.Where(x => permissionList.Contains(x.TypeId.ToString())).ToList();

            this.ddlMenuModule.DataSource = permittedMenuGroup;
            this.ddlMenuModule.DataTextField = "ModuleName";
            this.ddlMenuModule.DataValueField = "ModuleId";
            this.ddlMenuModule.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlMenuModule.Items.Insert(0, item);

            this.ddlSMenuModule.DataSource = permittedMenuGroup;
            this.ddlSMenuModule.DataTextField = "ModuleName";
            this.ddlSMenuModule.DataValueField = "ModuleId";
            this.ddlSMenuModule.DataBind();
            this.ddlSMenuModule.Items.Insert(0, item);
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
        public static ReturnInfo SaveMenuLink(MenuLinksBO menuLinks)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMUtility hmUtility = new HMUtility();
                MenuDA menuDa = new MenuDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                
                menuLinks.PagePath = menuDa.GetCommonMenuModuleById(menuLinks.ModuleId).ModulePath;

                if (menuLinks.MenuLinksId == 0)
                {
                    menuLinks.CreatedBy = userInformationBO.UserInfoId;
                    rtninf.IsSuccess = menuDa.SaveMenuLinks(menuLinks);
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
                else {
                    menuLinks.LastModifiedBy = userInformationBO.UserInfoId;
                    rtninf.IsSuccess = menuDa.UpdateMenuLinks(menuLinks);
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
        public static GridViewDataNPaging<MenuLinksBO, GridPaging> LoadMenuLinksInfo(int moduleId, string pageName, string pageType, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<MenuLinksBO, GridPaging> myGridData = new GridViewDataNPaging<MenuLinksBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            MenuDA da = new MenuDA();
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();

            menuLinks = da.GetMenuLinksForSearch(moduleId, pageName, pageType, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(menuLinks, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static MenuLinksBO GetMenuLinksInfo(long menuLinkId)
        {
            MenuLinksBO menuLinksBO = new MenuLinksBO();
            MenuDA menuDA = new MenuDA();

            menuLinksBO = menuDA.GetMenuLinksByMenuLinksId(menuLinkId);
            return menuLinksBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteMenuLinksInfo(int menuLinkId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("SecurityMenuLinks", "MenuLinksId", menuLinkId);

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
