using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmDashboardPermission : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadUserGroup();
                if (this.ddlUserGroupId.SelectedIndex != -1)
                {
                    this.LoadGridView(Convert.ToInt32(this.ddlUserGroupId.SelectedValue));
                }
            }
        }

        private void LoadUserGroup()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            UserGroupDA userGroupDA = new UserGroupDA();
            this.ddlUserGroupId.DataSource = userGroupDA.GetActiveUserGroupInfo(userInformationBO.UserGroupId);
            this.ddlUserGroupId.DataTextField = "GroupName";
            this.ddlUserGroupId.DataValueField = "UserGroupId";
            this.ddlUserGroupId.DataBind();
        }
        protected void gvDashboardManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblIsSaveValue = (Label)e.Row.FindControl("lblchkIsActiveStatus");
                if (lblIsSaveValue.Text == "Inactive")
                {
                    ((CheckBox)e.Row.FindControl("chkIsActiveStatus")).Checked = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkIsActiveStatus")).Checked = true;
                }
            }
        }

        private void LoadGridView(int userGroupId)
        {
            //this.CheckObjectPermission();
            DashboardDA entityDA = new DashboardDA();
            List<DashboardItemBO> files = entityDA.GetDashboardItem(userGroupId);

            this.gvDashboardManagement.DataSource = files;
            this.gvDashboardManagement.DataBind();
        }

        protected void ddlUserGroupId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlUserGroupId.SelectedIndex != -1)
            {
                this.LoadGridView(Convert.ToInt32(this.ddlUserGroupId.SelectedValue));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int counter = 0, totalSavedItem = 0, div1 = 1, div2 = 1, panel1 = 0, panel2 = 0, div3 = 0, div4 = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DashboardDA entityDA = new DashboardDA();

            //bool status = entityDA.DeleteUserDashboardMapping(Convert.ToInt64(this.ddlUserGroupId.SelectedValue));

            List<DashboardManagementBO> designList = new List<DashboardManagementBO>();
            designList = entityDA.GetDashboardManagementByUserGroupId(Convert.ToInt32(this.ddlUserGroupId.SelectedValue));

            List<UserDashboardItemMappingBO> mappingList = new List<UserDashboardItemMappingBO>();
            mappingList = entityDA.GetItemListByUserGroupId(Convert.ToInt32(this.ddlUserGroupId.SelectedValue));

            foreach (GridViewRow row in gvDashboardManagement.Rows)
            {
                counter = counter + 1;
                bool isSave = ((CheckBox)row.FindControl("chkIsActiveStatus")).Checked;

                UserDashboardItemMappingBO mappingBO = new UserDashboardItemMappingBO();
                Label lblUserMappingId = (Label)row.FindControl("lblUserMappingId");

                Label lblItemId = (Label)row.FindControl("lblItemId");
                mappingBO.ItemId = Convert.ToInt32(lblItemId.Text);
                mappingBO.UserId = Convert.ToInt32(this.ddlUserGroupId.SelectedValue);

                if (mappingList.Count > 0)
                {
                    var v = (from m in mappingList where m.ItemId == mappingBO.ItemId select m).FirstOrDefault();
                    if (v != null)
                    {
                        lblUserMappingId.Text = v.Id.ToString();
                    }
                }

                if (isSave)
                {
                    totalSavedItem = totalSavedItem + 1;

                    if (Convert.ToInt32(lblUserMappingId.Text) == 0 && mappingList.Count == 0)
                    {
                        if (totalSavedItem % 2 == 0)
                        {
                            mappingBO.Panel = 2;
                            mappingBO.Div = div2;
                            div2++;
                        }
                        else
                        {
                            mappingBO.Panel = 1;
                            mappingBO.Div = div1;
                            div1++;
                        }
                    }

                    if (Convert.ToInt32(lblUserMappingId.Text) == 0 && mappingList.Count > 0)
                    {
                        List<UserDashboardItemMappingBO> list = new List<UserDashboardItemMappingBO>();
                        list = entityDA.GetItemListByUserGroupId(Convert.ToInt32(this.ddlUserGroupId.SelectedValue));

                        foreach (UserDashboardItemMappingBO bo in list)
                        {
                            if (bo.Panel == 1)
                            {
                                panel1++;
                                if (bo.Div > div3)
                                {
                                    div3 = bo.Div;
                                }
                            }
                            else
                            {
                                panel2++;
                                if (bo.Div > div4)
                                {
                                    div4 = bo.Div;
                                }
                            }
                        }
                        if (panel1 <= panel2)
                        {
                            mappingBO.Panel = 1;
                            mappingBO.Div = div3 + 1;
                        }
                        else
                        {
                            mappingBO.Panel = 2;
                            mappingBO.Div = div4 + 1;
                        }
                        panel1 = 0;
                        div3 = 0;
                        panel2 = 0;
                        div4 = 0;
                    }

                    if (Convert.ToInt32(lblUserMappingId.Text) == 0)
                    {
                        Boolean statusSave = entityDA.SaveUserDashboardMapping(mappingBO);
                    }

                    List<DashboardManagementBO> userList = new List<DashboardManagementBO>();
                    userList = entityDA.GetDashboardManagementByUserGroupId(Convert.ToInt32(this.ddlUserGroupId.SelectedValue));

                    if (Convert.ToInt32(lblUserMappingId.Text) == 0 && designList.Count > 0)
                    {
                        foreach (DashboardManagementBO item in userList)
                        {
                            List<DashboardManagementBO> list = new List<DashboardManagementBO>();
                            DashboardManagementBO bo = new DashboardManagementBO();
                            bo.UserId = item.UserId;
                            //bo.UserGroupId = mappingBO.UserId;
                            bo.ItemId = mappingBO.ItemId;
                            bo.Panel = mappingBO.Panel;
                            bo.DivName = "div" + mappingBO.Panel + mappingBO.Div + "S" + mappingBO.ItemId + "";
                            list.Add(bo);
                            entityDA.SaveDashboardManagement(list);
                        }
                    }
                }
                else if (!isSave)
                {
                    //Label lblUserMappingId = (Label)row.FindControl("lblUserMappingId");
                    if (Convert.ToInt32(lblUserMappingId.Text) > 0)
                    {
                        entityDA.DeleteUserDashboardItemByItemId_SP(mappingBO.ItemId);
                        entityDA.DeleteDashboardManagementByItemId_SP(mappingBO.ItemId);
                    }
                }
            }

            if (totalSavedItem > 0)
            {
                //entityDA.DeleteDashboardManagement(Convert.ToInt32(this.ddlUserGroupId.SelectedValue));
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
            }
        }
    }
}