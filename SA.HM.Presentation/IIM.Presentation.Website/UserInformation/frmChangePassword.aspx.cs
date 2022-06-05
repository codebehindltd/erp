using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmChangePassword : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private int currentUserId;
        protected int isMessageBoxEnable = -1;
        UserInformationBO userInformationBO = new UserInformationBO();
        UserInformationDA userInformationDA = new UserInformationDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            currentUserId = userInformationBO.UserInfoId;
            
            if (!IsPostBack)
            {
                this.txtUserName.Text = userInformationBO.UserName;
                this.txtUserId.Text = userInformationBO.UserId;
                this.txtUserEmail.Text = userInformationBO.UserEmail;
                txtOldUserPassword.Text = userInformationBO.UserPassword;
                this.txtUserPhone.Text = userInformationBO.UserPhone;
                this.txtOldUserPassword.Focus();
                CheckPermission();
            }
            this.txtUserId.Enabled = false;
        }
        private void CheckPermission()
        {
            //hfViewPermission.Value = isViewPermission ? "1" : "0";
            //hfSavePermission.Value = isSavePermission ? "1" : "0";
            //hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            //hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(this.txtOldUserPassword.Text.Trim()))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "please provide old password.";
                this.txtOldUserPassword.Focus();
                flag = false;
            }

            else if (string.IsNullOrWhiteSpace(this.txtUserPassword.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "please provide new password.";
                this.txtUserPassword.Focus();
                flag = false;
            }
            else
            {
                this.isMessageBoxEnable = -1;
                this.lblMessage.Text = string.Empty;
            }
            return flag;
        }
        private void Cancel()
        {
            this.txtUserPassword.Text = string.Empty;
            this.btnSave.Text = "Update";
            this.txtOldUserPassword.Focus();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            if (this.txtUserPassword.Text.Equals(this.txtUserConfirmPassword.Text))
            {
                lblMessage.Text = string.Empty;
                if (currentUserId > 0)
                {
                    userInformationBO = null;
                    userInformationBO = userInformationDA.GetUserInformationByIdNPassword(currentUserId, this.txtOldUserPassword.Text);
                    if (userInformationBO.UserInfoId > 0)
                    {
                        userInformationBO.UserInfoId = userInformationBO.UserInfoId;
                        userInformationBO.UserName = this.txtUserName.Text;
                        userInformationBO.UserPassword = this.txtUserPassword.Text;
                        userInformationBO.UserEmail = this.txtUserEmail.Text;
                        userInformationBO.UserPhone = this.txtUserPhone.Text;
                        userInformationBO.LastModifiedBy = userInformationBO.UserInfoId;

                        Boolean status = userInformationDA.ChangeUserPassword(userInformationBO);
                        if (status)
                        {
                            this.isMessageBoxEnable = 1;
                            Session.Add("UserInformationBOSession", userInformationBO);
                            lblMessage.Text = "successfully changed password.";
                            CommonHelper.AlertInfo("Successfully Changed Password.", AlertType.Success);
                            this.Cancel();
                        }
                    }
                    else
                    {
                        this.isMessageBoxEnable = 1;
                        lblMessage.Text = "The old password you gave is incorrect.";
                        CommonHelper.AlertInfo("The old password you gave is incorrect.", AlertType.Warning);
                        this.txtOldUserPassword.Focus();
                    }
                }
                
            }
            else
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "passwords do not match.";
                CommonHelper.AlertInfo("Passwords do not match.", AlertType.Warning);
                this.txtOldUserPassword.Focus();
            }
        }
    }
}