using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportRoomStatus : System.Web.UI.Page
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportRoomStatus.ToString());
                LoadCurrentDate();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Validate())
            {
                txtSearchDate.Focus();
                return;
            }
            lblMessage.Text = "";
            //var usDtfi = new CultureInfo("en-US", false).DateTimeFormat; //--MM/dd/yyyy
            //var ukDtfi = new CultureInfo("en-GB", false).DateTimeFormat; //--dd/MM/yyyy

            var searchDate = DateTime.Now;
            if (!string.IsNullOrEmpty(txtSearchDate.Text.Trim()))
            {
                searchDate = hmUtility.GetDateTimeFromString(txtSearchDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (hmUtility.GetDateTimeFromString(txtSearchDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) > DateTime.Now.AddDays(-1))
            {
                //searchDate = hmUtility.ParseDateTime(searchDate.ToString(), usDtfi).AddMinutes(_offset).ToLocalTime();
                _RoomStatusInfoByDate = 1;
                RoomStatusByDate.SelectParameters[0].DefaultValue = searchDate.ToString();
                rvRoomStatusInfoByDate.LocalReport.Refresh();
            }
            else
            {
                lblMessage.Text = "Previous Date Not Valid";
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission(int userId, string formName)
        {
            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userId, formName);
            if (objectPermissionBO.ObjectPermissionId > 0)
            {
                isViewPermission = objectPermissionBO.IsViewPermission;

                if (!isViewPermission)
                {
                    Response.Redirect("/HMCommon/frmHMHome.aspx");
                }
            }
            else
            {
                Response.Redirect("/HMCommon/frmHMHome.aspx");
            }
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtSearchDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private bool Validate()
        {

            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtSearchDate.Text))
            {
                lblMessage.Text = "Search Date must not be empty";
                status = false;
            }
            return status;

        }
    }
}