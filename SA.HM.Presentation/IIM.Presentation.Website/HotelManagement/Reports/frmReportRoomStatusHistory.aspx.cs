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
    public partial class frmReportRoomStatusHistory : System.Web.UI.Page
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

                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportRoomStatusHistory.ToString());
                this.LoadCurrentDate();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //var usDtfi = new CultureInfo("en-US", false).DateTimeFormat; //--MM/dd/yyyy
            //var ukDtfi = new CultureInfo("en-GB", false).DateTimeFormat; //--dd/MM/yyyy



            var fromDate = DateTime.Now;
            var toDate = DateTime.Now;

            if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            {
                fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
            {
                toDate = hmUtility.GetDateTimeFromString(txtToDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            //fromDate = hmUtility.GetDateTimeFromString(fromDate.ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //toDate = hmUtility.GetDateTimeFromString(toDate.ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            //if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            //{
            //    fromDate = hmUtility.ParseDateTime(txtFromDate.Text.Trim(), ukDtfi);
            //}

            //if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
            //{
            //    toDate = hmUtility.ParseDateTime(txtToDate.Text.Trim(), ukDtfi);
            //}

            //fromDate = hmUtility.ParseDateTime(fromDate.ToString(), usDtfi).AddMinutes(_offset).ToLocalTime();
            //toDate = hmUtility.ParseDateTime(toDate.ToString(), usDtfi).AddMinutes(_offset).ToLocalTime();

            _RoomStatusInfoByDate = 1;
            RoomStatusDataSource.SelectParameters[0].DefaultValue = fromDate.ToString();
            RoomStatusDataSource.SelectParameters[1].DefaultValue = toDate.AddDays(1).AddSeconds(-1).ToString();
            //RoomReservationDataSource.SelectParameters[1].DefaultValue = toDate.ToString();
            rvTransaction.LocalReport.Refresh();
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
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
    }
}