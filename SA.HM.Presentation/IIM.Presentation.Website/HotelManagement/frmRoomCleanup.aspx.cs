
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomCleanup : BasePage
    {
        int _ApprovedId;
        Boolean isAllApproved = false;
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                Response.Redirect("/HotelManagement/frmRoomStatusInfo.aspx");
                this.LoadCurrentDate();
                this.LoadGridView();
                Session["PageDestinationAfterSave"] = null;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["RoomId"]))
                {
                    Session["PageDestinationAfterSave"] = "/HotelManagement/frmRoomStatusInfo.aspx";
                }
                else
                {
                    Session["PageDestinationAfterSave"] = null;
                }
                string roomId = Request.QueryString["RoomId"];
                txtRoomId.Value = roomId;

                AMPmFixed();
                CheckObjectPermission();
            }

        }

        private void AMPmFixed()
        {
            string currentTime = DateTime.Now.ToString("tt");
            if (currentTime == "AM")
                ddlProbableAMPM.SelectedIndex = 0;
            else
                ddlProbableAMPM.SelectedIndex = 1;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!isFormValid())
            {
                return;
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomNumberBO roomNumberBO = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            roomNumberBO.RoomId = Convert.ToInt32(this.txtRoomId.Value);

            RoomNumberBO roomNumberList = new RoomNumberBO();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(roomNumberBO.RoomId);
            roomNumberBO.StatusId = roomNumberList.StatusId;

            //int pMin = !string.IsNullOrWhiteSpace(this.txtProbableMinute.Text) ? Convert.ToInt32(this.txtProbableMinute.Text) : 0;
            //int pHour = this.ddlProbableAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableHour.Text) % 12) + 12);

            //if (this.ddlCleanStatus.SelectedValue.Equals("Cleaned"))
            //{
            //    roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text).AddHours(10).AddMinutes(00)).AddDays(1));
            //    roomNumberBO.LastCleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text).AddHours(pHour).AddMinutes(pMin));
            //}
            //else
            //{
            //    roomNumberBO.CleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text).AddHours(pHour).AddMinutes(pMin));
            //    roomNumberBO.LastCleanDate = this.txtLastCleanDate.Value;
            //}

            if (this.ddlCleanStatus.SelectedValue.Equals("Cleaned"))
            {
                roomNumberBO.CleanupStatus = "Cleaned";
                int cMin = !string.IsNullOrWhiteSpace(this.txtProbableMinute.Text) ? Convert.ToInt32(this.txtProbableMinute.Text) : 0;
                int cHour = this.ddlProbableAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableHour.Text) % 12) + 12);
                //roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddHours(10).AddMinutes(00)).AddDays(1));
                roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddHours(10).AddMinutes(00)));
                roomNumberBO.LastCleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddHours(cHour).AddMinutes(cMin));
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(this.txtFromDate.Text) ? hmUtility.GetDateTimeFromString(this.txtFromDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(this.txtToDate.Text) ? hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;

            }
            else if (this.ddlCleanStatus.SelectedValue.Equals("Dirty"))
            {
                roomNumberBO.CleanupStatus = "Dirty";
                int dMin = !string.IsNullOrWhiteSpace(this.txtProbableMinute.Text) ? Convert.ToInt32(this.txtProbableMinute.Text) : 0;
                int dHour = this.ddlProbableAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableHour.Text) % 12) + 12);
                roomNumberBO.CleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddHours(dHour).AddMinutes(dMin));
                if (!string.IsNullOrWhiteSpace(this.txtLastCleanDate.Value))
                {
                    roomNumberBO.LastCleanDate = this.txtLastCleanDate.Value;
                }
                else
                {
                    roomNumberBO.LastCleanDate = DateTime.Now.ToString();
                }
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(this.txtFromDate.Text) ? hmUtility.GetDateTimeFromString(this.txtFromDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(this.txtToDate.Text) ? hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
            }
            else if (this.ddlCleanStatus.SelectedValue.Equals("Available"))
            {
                roomNumberBO.CleanupStatus = "Available";
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(this.txtFromDate.Text) ? hmUtility.GetDateTimeFromString(this.txtFromDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(this.txtToDate.Text) ? hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.CleanDate = DateTime.Now.ToString();
                if (!string.IsNullOrWhiteSpace(this.txtLastCleanDate.Value))
                {
                    roomNumberBO.LastCleanDate = this.txtLastCleanDate.Value;
                }
                else
                {
                    roomNumberBO.LastCleanDate = DateTime.Now.ToString();
                }
            }
            else if (this.ddlCleanStatus.SelectedValue.Equals("OutOfOrder"))
            {
                roomNumberBO.CleanupStatus = "OutOfOrder";
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(this.txtFromDate.Text) ? hmUtility.GetDateTimeFromString(this.txtFromDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                //roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(this.txtToDate.Text) ? hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now.AddDays(1);
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(this.txtToDate.Text) ? hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.CleanDate = DateTime.Now.ToString();
                //roomNumberBO.LastCleanDate = DateTime.Now.ToString();
                if (!string.IsNullOrWhiteSpace(this.txtLastCleanDate.Value))
                {
                    roomNumberBO.LastCleanDate = this.txtLastCleanDate.Value;
                }
                else
                {
                    roomNumberBO.LastCleanDate = DateTime.Now.ToString();
                }
            }




            roomNumberBO.Remarks = this.txtRemarks.Text;

            if (!string.IsNullOrWhiteSpace(txtRoomId.Value))
            {
                roomNumberBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = roomNumberDA.UpdateRoomCleanInfo(roomNumberBO);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomClean.ToString(), roomNumberBO.RoomId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Room CleanUp Status Updated to" + roomNumberBO.CleanupStatus);
                    if (Session["PageDestinationAfterSave"] != null)
                    {
                        Session["PageDestinationAfterSave"] = null;
                        Response.Redirect("/HotelManagement/frmRoomStatusInfo.aspx");
                    }
                    else
                    {
                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Saved Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.LoadGridView();
                        this.Cancel();
                    }
                }
            }

        }
        protected void gvAvailableGuestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvAvailableGuestList.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvAvailableGuestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgApprove = (ImageButton)e.Row.FindControl("ImgApprove");
                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");

                Label lblCleanupStatus = (Label)e.Row.FindControl("lblgvCleanupStatus");
                if (lblCleanupStatus.Text != "Cleaned")
                {
                    this.isAllApproved = true;
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = true;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = true;
                    ((Label)e.Row.FindControl("lblgvCleanDate")).Text = string.Empty;
                    imgUpdate.Visible = isUpdatePermission;
                    imgApprove.Visible = isSavePermission;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = (true && isSavePermission);
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = (true && isUpdatePermission);
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = true;
                }

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //imgApprove.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";


            }

            if (isAllApproved)
            {
                this.btnSaveAll.Visible = isSavePermission;
            }
            else
            {
                //this.btnSaveAll.Visible = false;
                //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                //Boolean status = roomRegistrationDA.SaveRoomStatusHistoryInfo();
            }
        }
        protected void gvAvailableGuestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    RoomClean(this._ApprovedId);
                    //this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    //Session["_ApprovedId"] = this._ApprovedId;
                    this.Cancel();
                    this.LoadGridView();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //this.txtRegistrationId.Value = string.Empty;
        }
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            int counter = 0;
            string messageType = "NotSave";
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (GridViewRow row in gvAvailableGuestList.Rows)
            {
                counter = counter + 1;
                bool isAccept = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
                if (isAccept)
                {
                    messageType = "Save";
                    Label lblActiveStatValue = (Label)row.FindControl("lblApprovedStatus");

                    RoomNumberBO roomNumberBO = new RoomNumberBO();
                    RoomNumberDA roomNumberDA = new RoomNumberDA();

                    roomNumberBO.RoomId = Convert.ToInt32(((Label)row.FindControl("lblid")).Text);

                    RoomNumberBO roomNumberList = new RoomNumberBO();
                    roomNumberList = roomNumberDA.GetRoomNumberInfoById(roomNumberBO.RoomId);
                    roomNumberBO.StatusId = roomNumberList.StatusId;

                    roomNumberBO.CleanupStatus = "Cleaned";

                    int pMin = !string.IsNullOrWhiteSpace(this.txtProbableMinute.Text) ? Convert.ToInt32(this.txtProbableMinute.Text) : 0;
                    int pHour = this.ddlProbableAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableHour.Text) % 12) + 12);

                    roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin)).AddDays(1));
                    roomNumberBO.LastCleanDate = Convert.ToString(DateTime.Now);

                    roomNumberBO.FromDate = DateTime.Now;
                    roomNumberBO.ToDate = DateTime.Now;

                    roomNumberBO.Remarks = string.Empty;
                    roomNumberBO.LastModifiedBy = userInformationBO.UserInfoId;

                    Boolean status = roomNumberDA.UpdateRoomCleanInfo(roomNumberBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomClean.ToString(), roomNumberBO.RoomId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Room Clean Up Status Updated to" + roomNumberBO.CleanupStatus);
                    }

                }


                if (gvAvailableGuestList.Rows.Count == counter)
                {
                    if (messageType == "Save")
                    {

                        CommonHelper.AlertInfo(innboardMessage, "Room CleanUp Successfull.", AlertType.Success);
                        this.LoadGridView();
                        this.Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Room Number for CleanUp.", AlertType.Warning);
                    }
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtApprovedDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            // this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            if (this.ddlSrcCleanStatus.SelectedIndex != -1)
            {
                string strCompanyName = string.Empty;
                string strCompanyAddress = string.Empty;
                string strCompanyWeb = string.Empty;
                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> companyBO = companyDA.GetCompanyInfo();
                if (companyBO != null)
                {
                    if (companyBO.Count > 0)
                    {
                        strCompanyName = companyBO[0].CompanyName;
                        strCompanyAddress = companyBO[0].CompanyAddress;
                        strCompanyWeb = companyBO[0].WebAddress;
                    }
                }

                RoomNumberDA roomNumberDA = new RoomNumberDA();
                string Status = this.ddlSrcCleanStatus.SelectedValue.ToString();
                List<RoomNumberBO> fielBOList = roomNumberDA.GetRoomNumberInfoByCleanup(Status, this.txtSrcRoomNumber.Text, strCompanyName, strCompanyAddress, strCompanyWeb);

                this.gvAvailableGuestList.DataSource = fielBOList;
                this.gvAvailableGuestList.DataBind();
            }
            else
            {
                this.gvAvailableGuestList.DataSource = null;
                this.gvAvailableGuestList.DataBind();
            }

            AMPmFixed();
        }
        private void Cancel()
        {
            //this.ddlRoomTypeId.SelectedIndex = 0;
            //this.txtRoomNumber.Text = string.Empty;
            //this.ddlActiveStat.SelectedIndex = 0;
            //this.btnSave.Text = "Save";
            //this.txtRoomNumber.Focus();
            this.txtProbableHour.Text = "10";
            this.txtProbableMinute.Text = "00";
            this.ddlProbableAMPM.SelectedValue = "0";
            Session["PageDestinationAfterSave"] = null;
            txtRoomId.Value = string.Empty;
            AMPmFixed();
            CheckObjectPermission();
        }
        public bool isFormValid()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            bool status = true;
            string roomNumber = txtRoomNumber.Text;
            if (String.IsNullOrWhiteSpace(roomNumber))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Room Number.", AlertType.Warning);
                status = false;
                txtRoomNumber.Focus();
            }

            if (ddlCleanStatus.SelectedValue == "OutOfOrder")
            {
                if (hmUtility.GetDateTimeFromString(this.txtFromDate.Text, userInformationBO.ServerDateFormat).Date < DateTime.Now.Date)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Date.", AlertType.Warning);
                    status = false;
                    txtFromDate.Focus();
                }
                else if (hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat).Date < DateTime.Now.Date)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Date.", AlertType.Warning);
                    status = false;
                    txtToDate.Focus();
                }
            }

            return status;

        }
        public void RoomClean(int rowId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomNumberBO roomNumberBO = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            RoomNumberBO roomNumberList = new RoomNumberBO();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(rowId);

            roomNumberBO.StatusId = roomNumberList.StatusId;
            roomNumberBO.RoomId = rowId;
            roomNumberBO.CleanupStatus = "Cleaned";

            int pMin = !string.IsNullOrWhiteSpace(this.txtProbableMinute.Text) ? Convert.ToInt32(this.txtProbableMinute.Text) : 0;
            int pHour = this.ddlProbableAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableHour.Text) % 12) + 12);

            //roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin)).AddDays(1));
            this.txtApprovedDate.Text = hmUtility.GetStringFromDateTime(DateTime.Now);
            roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin)).AddDays(1));
            roomNumberBO.LastCleanDate = Convert.ToString(DateTime.Now);
            roomNumberBO.FromDate = DateTime.Now;
            roomNumberBO.ToDate = DateTime.Now;

            roomNumberBO.Remarks = string.Empty;
            roomNumberBO.LastModifiedBy = userInformationBO.UserInfoId;

            Boolean status = roomNumberDA.UpdateRoomCleanInfo(roomNumberBO);
            if (status)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomClean.ToString(), roomNumberBO.RoomId,
                             ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Room Clean Up Status Updated to" + roomNumberBO.CleanupStatus);
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                this.LoadGridView();
                this.Cancel();
            }
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            btnSaveAll.Visible = isSavePermission;
        }
        [WebMethod]
        public static RoomNumberBO FillForm(int EditId)
        {
            RoomNumberBO roomNumberList = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(EditId);

            return roomNumberList;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}