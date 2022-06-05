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
    public partial class frmUserGroupWiseMenuSetup : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserGroup();
                LoadUserInformation();
                LoadCommonMenuModule();
                LoadMenuGroup();
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
        private void LoadUserGroup()
        {
            UserGroupDA userDa = new UserGroupDA();
            List<UserGroupBO> userGroupBOList = new List<UserGroupBO>();
            userGroupBOList = userDa.GetUserGroupInfo();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.UserInfoId != 1)
            {
                userGroupBOList = userGroupBOList.Where(x => x.UserGroupId != 1).ToList();
            }

            this.ddlUserGroup.DataSource = userGroupBOList;
            this.ddlUserGroup.DataTextField = "GroupName";
            this.ddlUserGroup.DataValueField = "UserGroupId";
            this.ddlUserGroup.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlUserGroup.Items.Insert(0, item);
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
        }

        private void LoadMenuGroup()
        {
            MenuDA menuDa = new MenuDA();

            this.ddlMenuGroup.DataSource = menuDa.GetMenuGroup();
            this.ddlMenuGroup.DataTextField = "MenuGroupName";
            this.ddlMenuGroup.DataValueField = "MenuGroupId";
            this.ddlMenuGroup.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlMenuGroup.Items.Insert(0, item);
        }

        private void LoadUserInformation()
        {
            UserInformationDA userDa = new UserInformationDA();
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();

            userInformationList = userDa.GetUserInformation();

            this.ddlUserList.DataSource = userInformationList;
            this.ddlUserList.DataTextField = "UserName";
            this.ddlUserList.DataValueField = "UserInfoId";
            this.ddlUserList.DataBind();

            this.ddlUserList.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
        }

        [WebMethod]
        public static ArrayList GetLinkByModuleId(int userGroupId, long menuGroupId, int moduleId)
        {
            string linksGrid = string.Empty;
            MenuDA menuDa = new MenuDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>(); 
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
            List<MenuLinksBO> authorizedMenuLinks = new List<MenuLinksBO>();

            menuWiseLinks = menuDa.GetMenuLinksByModuleIdUserGroupNMenuGroupId(userGroupId, menuGroupId, moduleId).OrderBy(o=>o.PageName).ToList();

            if (userInformationBO.UserInfoId == 1)
            {
                menuLinks = menuDa.GetMenuLinksByModuleId(moduleId).OrderBy(o => o.PageName).ToList();
            }
            else
            {
                menuLinks = menuDa.GetMenuLinksByModuleId(userInformationBO.UserGroupId, moduleId).OrderBy(o => o.PageName).ToList();
            }

            ArrayList arr = new ArrayList();
            arr.Add(new { MenuLinks = menuLinks, MenuWisePermitedLinks = menuWiseLinks });

            return arr;
        }
        
        [WebMethod]
        public static ArrayList GetLinkForSingleUserByModuleId(int userId, long menuGroupId, int moduleId)
        {
            string linksGrid = string.Empty;
            MenuDA menuDa = new MenuDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
            List<MenuLinksBO> authorizedMenuLinks = new List<MenuLinksBO>();

            menuWiseLinks = menuDa.GetMenuLinksByModuleIdUserIdNMenuGroupId(userId, menuGroupId, moduleId).OrderBy(o => o.PageName).ToList();

            if (userInformationBO.UserInfoId == 1)
            {
                menuLinks = menuDa.GetMenuLinksByModuleId(moduleId).OrderBy(o => o.PageName).ToList();
            }
            else
            {
                menuLinks = menuDa.GetMenuLinksByModuleId(userInformationBO.UserGroupId, moduleId).OrderBy(o => o.PageName).ToList();
            }

            ArrayList arr = new ArrayList();
            arr.Add(new { MenuLinks = menuLinks, MenuWisePermitedLinks = menuWiseLinks });

            return arr;
        }

        [WebMethod]
        public static ReturnInfo SaveUserGroupWiseMenuNPermission(List<MenuWiseLinksBO> SecurityMenuWiseLinksNelyAdded, List<MenuWiseLinksBO> SecurityMenuWiseLinksEdited, List<MenuWiseLinksBO> SecurityMenuWiseLinksDeleted, int userGroupId, long menuGroupId, int moduleId)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                MenuDA menuDa = new MenuDA();
                List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
                List<MenuWiseLinksBO> editedLinks = new List<MenuWiseLinksBO>();

                menuWiseLinks = menuDa.GetMenuLinksByModuleIdUserGroupNMenuGroupId(userGroupId, menuGroupId, moduleId);

                List<MenuWiseLinksBO> duplicateList = new List<MenuWiseLinksBO>();

                if (SecurityMenuWiseLinksNelyAdded.Count > 0 && menuWiseLinks.Count > 0)
                {
                    foreach (MenuWiseLinksBO newadd in SecurityMenuWiseLinksNelyAdded)
                    {
                        var v = (from m in menuWiseLinks where m.UserGroupId == newadd.UserGroupId && m.MenuGroupId == newadd.MenuGroupId && m.MenuLinksId == newadd.MenuLinksId select m).FirstOrDefault();
                        if (v != null)
                        {
                            MenuWiseLinksBO vv = (from ss in SecurityMenuWiseLinksNelyAdded where ss.UserGroupId == v.UserGroupId && ss.MenuGroupId == v.MenuGroupId && ss.MenuLinksId == v.MenuLinksId select ss).FirstOrDefault();
                            duplicateList.Add(vv);
                        }
                    }
                }

                if (duplicateList.Count > 0)
                {
                    foreach (MenuWiseLinksBO d in duplicateList)
                    {
                        SecurityMenuWiseLinksNelyAdded.Remove(d);
                    }
                }

                editedLinks = (from mwl in SecurityMenuWiseLinksEdited
                               join mld in menuWiseLinks
                               on mwl.MenuWiseLinksId equals mld.MenuWiseLinksId
                               where
                                    mwl.DisplaySequence != mld.LinksDisplaySequence ||
                                    mwl.IsSavePermission != mld.IsSavePermission ||
                                    mwl.IsUpdatePermission != mld.IsUpdatePermission ||
                                    mwl.IsDeletePermission != mld.IsDeletePermission ||
                                    mwl.IsViewPermission != mld.IsViewPermission
                               select mwl).ToList();


                if (SecurityMenuWiseLinksNelyAdded.Count != 0 || editedLinks.Count != 0 || SecurityMenuWiseLinksDeleted.Count != 0)
                {
                    rtninf.IsSuccess = menuDa.SaveUserGroupWiseMenuNPermission(SecurityMenuWiseLinksNelyAdded, editedLinks, SecurityMenuWiseLinksDeleted, userInformationBO.UserInfoId);
                }
                else
                {
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    //return true;
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
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
        public static ReturnInfo SaveUserIdWiseMenuNPermission(List<MenuWiseLinksBO> SecurityMenuWiseLinksNelyAdded, List<MenuWiseLinksBO> SecurityMenuWiseLinksEdited, List<MenuWiseLinksBO> SecurityMenuWiseLinksDeleted, int userId, string transactionType, long menuGroupId, int moduleId)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                MenuDA menuDa = new MenuDA();
                List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
                List<MenuWiseLinksBO> editedLinks = new List<MenuWiseLinksBO>();

                menuWiseLinks = menuDa.GetMenuLinksByModuleIdUserIdNMenuGroupId(userId, menuGroupId, moduleId);

                List<MenuWiseLinksBO> duplicateList = new List<MenuWiseLinksBO>();

                if (SecurityMenuWiseLinksNelyAdded.Count > 0 && menuWiseLinks.Count > 0)
                {
                    foreach (MenuWiseLinksBO newadd in SecurityMenuWiseLinksNelyAdded)
                    {
                        var v = (from m in menuWiseLinks where m.UserGroupId == newadd.UserGroupId && m.MenuGroupId == newadd.MenuGroupId && m.MenuLinksId == newadd.MenuLinksId select m).FirstOrDefault();
                        if (v != null)
                        {
                            MenuWiseLinksBO vv = (from ss in SecurityMenuWiseLinksNelyAdded where ss.UserGroupId == v.UserGroupId && ss.MenuGroupId == v.MenuGroupId && ss.MenuLinksId == v.MenuLinksId select ss).FirstOrDefault();
                            duplicateList.Add(vv);
                        }
                    }
                }

                if (duplicateList.Count > 0)
                {
                    foreach (MenuWiseLinksBO d in duplicateList)
                    {
                        SecurityMenuWiseLinksNelyAdded.Remove(d);
                    }
                }

                editedLinks = (from mwl in SecurityMenuWiseLinksEdited
                               join mld in menuWiseLinks
                               on mwl.MenuWiseLinksId equals mld.MenuWiseLinksId
                               where
                                    mwl.DisplaySequence != mld.LinksDisplaySequence ||
                                    mwl.IsSavePermission != mld.IsSavePermission ||
                                    mwl.IsUpdatePermission != mld.IsUpdatePermission ||
                                    mwl.IsDeletePermission != mld.IsDeletePermission ||
                                    mwl.IsViewPermission != mld.IsViewPermission
                               select mwl).ToList();


                if (SecurityMenuWiseLinksNelyAdded.Count != 0 || editedLinks.Count != 0 || SecurityMenuWiseLinksDeleted.Count != 0)
                {
                    rtninf.IsSuccess = menuDa.SaveUserIdWiseMenuNPermission(SecurityMenuWiseLinksNelyAdded, editedLinks, SecurityMenuWiseLinksDeleted, userInformationBO.UserInfoId, transactionType);
                }
                else
                {
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    //return true;
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
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

    }
}
