using HotelManagement.Data.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Security;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmUserConfiguration : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGroupName();
                LoadFeatures();
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
        private void LoadFeatures()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> featureList = new List<CustomFieldBO>();
            //featureList = commonDA.GetCustomField("InnboardFeatures", hmUtility.GetDropDownFirstValue());
            featureList = commonDA.GetCustomField("InnboardFeatures");

            ddlFeatures.DataSource = featureList;
            ddlFeatures.DataTextField = "Description";
            ddlFeatures.DataValueField = "FieldId";
            ddlFeatures.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlFeatures.Items.Insert(0, item);
        }
        private void LoadGroupName()
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

            ddlUserGroupName.DataSource = userGroupBOList;
            ddlUserGroupName.DataValueField = "UserGroupId";
            ddlUserGroupName.DataTextField = "GroupName";
            ddlUserGroupName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlUserGroupName.Items.Insert(0, item);
        }
        [WebMethod]
        public static ArrayList GetLinkByGroupId(int userGroupId, long featuresId)
        {
            UserInformationDA userInfoDA = new UserInformationDA();
            UserConfigurationDA userCofigurationDA = new UserConfigurationDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            userInformationList = userInfoDA.GetUserInformationByUserGroup(userGroupId);

            List<UserConfigurationBO> userConfiguredList = new List<UserConfigurationBO>();
            userConfiguredList = userCofigurationDA.GetConfiguredUserByFeaturesId(featuresId);

            //List<UserGroupBO> userGroupList = new List<UserGroupBO>(); GroupWisePermittedUser = userGroupList


            ArrayList arr = new ArrayList();
            arr.Add(new { UserList = userInformationList, UserConfigured = userConfiguredList });

            return arr;
        }
        [WebMethod]
        public static ReturnInfo SaveCheckedByApprovedByUsers(List<UserConfigurationBO> userConfiguredListSave, List<UserConfigurationBO> userConfiguredListEdit, int userGroupId, long featuresId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            long tempId = 0;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                UserInformationDA userInfoDA = new UserInformationDA();
                UserConfigurationDA userCofigurationDA = new UserConfigurationDA();

                List<UserConfigurationBO> userAlreadyConfiguredList = new List<UserConfigurationBO>();
                userAlreadyConfiguredList = userCofigurationDA.GetConfiguredUserByFeaturesId(featuresId);

                List<UserInformationBO> userInformationList = new List<UserInformationBO>();
                userInformationList = userInfoDA.GetUserInformationByUserGroup(userGroupId);

                List<UserConfigurationBO> duplicateUserConfigList = new List<UserConfigurationBO>();

                if (userConfiguredListSave.Count > 0 && userAlreadyConfiguredList.Count>0)
                {
                    foreach (UserConfigurationBO itemAdd in userConfiguredListSave)
                    {                

                        var temp = (from data in userAlreadyConfiguredList
                                    where data.UserInfoId == itemAdd.UserInfoId select data).FirstOrDefault();

                        if (temp != null)
                        {
                            UserConfigurationBO itemEdit = (from item in userConfiguredListSave
                                                            where item.UserInfoId == temp.UserInfoId
                                                            select item).FirstOrDefault();
                            duplicateUserConfigList.Add(itemEdit);
                        }
                    }
                }

                if(duplicateUserConfigList.Count>0)
                {
                    foreach (var itemDupli in duplicateUserConfigList)
                    {
                        userConfiguredListSave.Remove(itemDupli); //remove from save list
                    }
                }
                

                userConfiguredListEdit = (from itemEdit in userConfiguredListEdit
                                          join conList in userAlreadyConfiguredList
                                          on itemEdit.UserInfoId equals conList.UserInfoId
                                          where
                                                itemEdit.IsCheckedBy!= conList.IsCheckedBy ||
                                                itemEdit.IsApprovedBy != conList.IsApprovedBy
                                                select itemEdit).ToList();

                if (userConfiguredListSave.Count != 0 || userConfiguredListEdit.Count!=0)
                {
                    rtninf.IsSuccess = userCofigurationDA.SaveCheckedByApprovedByUsers(userConfiguredListSave, userConfiguredListEdit, userInformationBO.UserInfoId, out tempId);
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