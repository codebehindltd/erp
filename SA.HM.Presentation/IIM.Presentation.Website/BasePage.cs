using HotelManagement.Entity.UserInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data;
using System.Collections;
using System.Web.Services;
using HotelManagement.Presentation.Website.Common;
using System.IO;

namespace HotelManagement.Presentation.Website
{
    public class BasePage: System.Web.UI.Page
    {
        public Boolean isViewPermission = false;
        public Boolean isSavePermission = false;
        public Boolean isDeletePermission = false;
        public Boolean isUpdatePermission = false;
        HMUtility hmUtility = new HMUtility();
        private string formName;

        public BasePage()
        {
            this.formName = this.GetType().BaseType.FullName;
            this.formName = this.formName.Substring(this.formName.LastIndexOf('.')+1);

        }
        public BasePage( string formName)
        {
            this.formName = formName;
        }
        protected override void OnInit(EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, formName);

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            isUpdatePermission = objectPermissionBO.IsUpdatePermission;
            isViewPermission = objectPermissionBO.IsViewPermission;
            base.OnInit(e);
        }

        
    }

}