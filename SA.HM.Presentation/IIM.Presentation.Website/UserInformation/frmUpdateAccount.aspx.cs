using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;

namespace HotelManagement.Presentation.Website.UserInformation
{
    public partial class frmUpdateAccount : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        private int currentUserId;
        protected int isMessageBoxEnable = -1;
        UserInformationBO userInformationBO = new UserInformationBO();
        UserInformationDA userInformationDA = new UserInformationDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            currentUserId = userInformationBO.UserInfoId;
            
            if (!IsPostBack)
            {
                this.txtUserName.Text = userInformationBO.UserName;
                this.txtUserId.Text = userInformationBO.UserId;
                this.txtUserEmail.Text = userInformationBO.UserEmail;

                this.txtUserPhone.Text = userInformationBO.UserPhone;
                CheckPermission();
            }
            this.txtUserId.Enabled = false;
        }
        private void CheckPermission()
        {
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private bool IsFrmValid()
        {

            bool flag = true;

            if (string.IsNullOrWhiteSpace(this.txtUserName.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide Name";
                this.txtUserName.Focus();
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

            this.btnSave.Text = "Update";

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

                lblMessage.Text = string.Empty;
                if (currentUserId > 0)
                {
                    UserInformationBO userInformationBO = new UserInformationBO();


                        userInformationBO.UserInfoId = currentUserId;
                        userInformationBO.UserName = this.txtUserName.Text;
                        userInformationBO.UserEmail = this.txtUserEmail.Text;
                        userInformationBO.UserPhone = this.txtUserPhone.Text;
                        userInformationBO.LastModifiedBy = currentUserId;
                        

                        Boolean status = userInformationDA.ChangeUserAccountInformation(userInformationBO);
                        if (status)
                        {
                            this.isMessageBoxEnable = 2;
                            Session.Add("UserInformationBOSession", userInformationBO);
                            lblMessage.Text = "successfully changed account information.";
                            this.Cancel();
                            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                            userInformationBO.UserId = txtUserId.Text;
                        }

                }
                

        }


    }
}