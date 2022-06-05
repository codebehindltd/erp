using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmTimeSlabHead : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            //ddlSlabStartAMPM.Visible = false;
            //ddlSlabEndAMPM.Visible = false;
            
            if (!IsPostBack)
            {
                //ddlSlabEndAMPM.SelectedIndex = 1;
                this.LoadGridView();
                SetDefaulTime();
                CheckPermission();
            }

        }

        private void SetDefaulTime()
        {
            this.txtSlabStartHour.Text = "10:00";            
            this.txtSlabEndTimeHour.Text = "06:00";            
        }
        public bool IsFrmValid()
        {
            bool flag = true;


            if (string.IsNullOrWhiteSpace(this.txtTimeSlabHead.Text.Trim()))
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Time Slab Head.", AlertType.Warning);
                flag = false;
                txtTimeSlabHead.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtSlabStartHour.Text.Trim()))
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Time Slab Start Hour.", AlertType.Warning);
                flag = false;
                txtSlabStartHour.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtSlabEndTimeHour.Text.Trim()))
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Time Slab End Hour.", AlertType.Warning);
                flag = false;
                txtSlabEndTimeHour.Focus();
            }            
            return flag;

        }
        public void Cancel()
        {

            this.ddlActiveStat.SelectedValue = "0";
            this.txtTimeSlabId.Value = string.Empty;
            this.txtTimeSlabHead.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtTimeSlabHead.Focus();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                //HMUtility hmUtility = new HMUtility();

                TimeSlabHeadDA headDA = new TimeSlabHeadDA();

                TimeSlabHeadBO headBO = new TimeSlabHeadBO();
                string Today = hmUtility.GetStringFromDateTime(DateTime.Now);
                headBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                //int startMin = !string.IsNullOrWhiteSpace(this.txtSlabStartMinute.Text) ? Convert.ToInt32(this.txtSlabStartMinute.Text) : 0;
                //int startHour = Convert.ToInt32(this.txtSlabStartHour.Text) % 24;
                //int startHour = this.ddlSlabStartAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtSlabStartHour.Text) % 12) : ((Convert.ToInt32(this.txtSlabStartHour.Text) % 12) + 12);
                //string StartTime = (txtSlabStartHour.Text.Replace("AM", "")).Replace("PM", "");
                //string[] STime = StartTime.Split(':');
                //int startHour = Convert.ToInt32(STime[0]);
                //int startMin = Convert.ToInt32(STime[1]);
                //headBO.SlabStartTime = hmUtility.GetDateTimeFromString(Today, userInformationBO.ServerDateFormat).AddHours(startHour).AddMinutes(startMin);

                if (!String.IsNullOrEmpty(txtSlabStartHour.Text))
                {
                    headBO.SlabStartTime = hmUtility.GetDateTimeFromString(Today, userInformationBO.ServerDateFormat);
                    headBO.SlabStartTime = Convert.ToDateTime((headBO.SlabStartTime.Date.ToString("MM/dd/yyyy") + " " + txtSlabStartHour.Text));
                }
                else
                {
                    headBO.SlabStartTime = DateTime.Now;
                }
                //int endMin = !string.IsNullOrWhiteSpace(this.txtSlabEndTimeMinute.Text) ? Convert.ToInt32(this.txtSlabEndTimeMinute.Text) : 0;
                //int endHour = Convert.ToInt32(this.txtSlabEndTimeHour.Text) % 24;
                //int endHour = this.ddlSlabEndAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtSlabEndTimeHour.Text) % 12) : ((Convert.ToInt32(this.txtSlabEndTimeHour.Text) % 12) + 12);
                //string EndTime = (txtSlabEndTimeHour.Text.Replace("AM", "")).Replace("PM", "");
                //string[] ETime = EndTime.Split(':');
                //int endHour = Convert.ToInt32(ETime[0]);
                //int endMin = Convert.ToInt32(ETime[1]);
                //headBO.SlabEndTime = hmUtility.GetDateTimeFromString(Today, userInformationBO.ServerDateFormat).AddHours(endHour).AddMinutes(endMin);
                if (!String.IsNullOrEmpty(txtSlabEndTimeHour.Text))
                {
                    headBO.SlabEndTime = hmUtility.GetDateTimeFromString(Today, userInformationBO.ServerDateFormat);
                    headBO.SlabEndTime  = Convert.ToDateTime((headBO.SlabStartTime.Date.ToString("MM/dd/yyyy") + " " + txtSlabEndTimeHour.Text));
                }
                else
                {
                    headBO.SlabEndTime = DateTime.Now;
                }

                headBO.TimeSlabHead = this.txtTimeSlabHead.Text.ToString();
                if (string.IsNullOrWhiteSpace(txtTimeSlabId.Value))
                {
                    int tmpUserInfoId = 0;
                    headBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = headDA.SaveTimeSlabHeadInfo(headBO, out tmpUserInfoId);

                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.TimeSlabHead.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TimeSlabHead));
                        this.LoadGridView();
                        this.Cancel();
                    }
                    else
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, "You Can't Save.Item Already Inserted.", AlertType.Warning);
                        this.LoadGridView();
                        this.Cancel();
                    }

                }
                else
                {
                    headBO.TimeSlabId = Convert.ToInt32(txtTimeSlabId.Value);
                    headBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = headDA.UpdateTimeSlabHeadInfo(headBO);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.TimeSlabHead.ToString(), headBO.TimeSlabId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TimeSlabHead));
                        this.LoadGridView();
                        this.Cancel();
                        txtTimeSlabId.Value = "";

                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }

        }
        protected void gvTimeSlabHead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvTimeSlabHead.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();

        }
        protected void gvTimeSlabHead_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                // imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                // imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }

        protected void gvTimeSlabHead_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int timeSlabId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(timeSlabId);
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollTimeSlabHead", "TimeSlabId", timeSlabId);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        //this.lblMessage.Text = "Delete Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.TimeSlabHead.ToString(), timeSlabId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TimeSlabHead));
                    }
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }

        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        public void LoadGridView()
        {
            this.CheckObjectPermission();
            TimeSlabHeadDA headDA = new TimeSlabHeadDA();
            List<TimeSlabHeadBO> list = new List<TimeSlabHeadBO>();
            list = headDA.GetAllTimeSlabHeadInfo();
            gvTimeSlabHead.DataSource = list;
            gvTimeSlabHead.DataBind();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmTimeSlabHead.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }

        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }

        //[WebMethod]
        //public static string DeleteData(int sEmpId)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        HMCommonDA hmCommonDA = new HMCommonDA();
        //        Boolean status = hmCommonDA.DeleteInfoById("PayrollTimeSlabHead", "TimeSlabId", sEmpId);
        //        if (status)
        //        {
        //            result = "success";
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return result;
        //}

        public void FillForm(int EditId)
        {
            TimeSlabHeadDA headDA = new TimeSlabHeadDA();
            TimeSlabHeadBO headBO = new TimeSlabHeadBO();
            headBO=headDA.GetTimeSlabHeadInfoById(EditId);
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();



            //if (Int32.Parse(headBO.SlabStartTime.ToString("HH")) > 24)
            //{
            //    headBO.StartHour = (Int32.Parse(headBO.SlabStartTime.ToString("HH")) % 24).ToString();
            //    headBO.StartMin = headBO.SlabStartTime.ToString("mm");
            //   // headBO.StartAMPM = "PM";
            //}
            //else {
            //    headBO.StartHour = (Int32.Parse(headBO.SlabStartTime.ToString("HH"))).ToString();
            //    headBO.StartMin = headBO.SlabStartTime.ToString("mm");
            //   // headBO.StartAMPM = "AM";            
            //}



            //if (Int32.Parse(headBO.SlabEndTime.ToString("HH")) > 24)
            //{
            //    headBO.EndHour = (Int32.Parse(headBO.SlabEndTime.ToString("HH")) % 24).ToString();
            //    headBO.EndMin = headBO.SlabEndTime.ToString("mm");
            //   // headBO.EndAMPM = "PM";
            //}
            //else
            //{
            //    headBO.EndHour = (Int32.Parse(headBO.SlabEndTime.ToString("HH"))).ToString();
            //    headBO.EndMin = headBO.SlabEndTime.ToString("mm");
            //  //  headBO.EndAMPM = "AM";
            //}
            txtTimeSlabId.Value= headBO.TimeSlabId.ToString();
           ddlActiveStat.SelectedIndex=headBO.ActiveStat == true ? 0 : 1;
           txtTimeSlabHead.Text= headBO.TimeSlabHead;
           txtSlabStartHour.Text = headBO.SlabStartTime.ToString(userInformationBO.TimeFormat);
           txtSlabEndTimeHour.Text = headBO.SlabEndTime.ToString(userInformationBO.TimeFormat);

           //txtSlabStartHour.Text = Convert.ToInt32(headBO.SlabStartTime.ToString("%h")) == 0 ? "12" : headBO.SlabStartTime.ToString("%h");
           //txtSlabStartMinute.Text = headBO.SlabStartTime.ToString("mm");

           //DateTime SlabStartDateTime = Convert.ToDateTime(headBO.SlabStartTime);
           //string S = SlabStartDateTime.ToString("tt");
           //this.ddlSlabStartAMPM.SelectedIndex = S == "AM" ? 0 : 1;

           //txtSlabEndTimeHour.Text = Convert.ToInt32(headBO.SlabEndTime.ToString("%h")) == 0 ? "12" : headBO.SlabEndTime.ToString("%h");
           //txtSlabEndTimeMinute.Text = headBO.SlabEndTime.ToString("mm");

           //DateTime SlabEndDateTime = Convert.ToDateTime(headBO.SlabEndTime);
           //string SS = SlabEndDateTime.ToString("tt");
           //this.ddlSlabEndAMPM.SelectedIndex = SS == "AM" ? 0 : 1;

           //txtSlabStartHour.Text=headBO.StartHour;
           //ddlSlabStartAMPM.SelectedValue=headBO.StartAMPM;
           //txtSlabStartMinute.Text=headBO.StartMin;

           //txtSlabEndTimeHour.Text=headBO.EndHour;
           //txtSlabEndTimeMinute.Text=headBO.EndMin;
           //ddlSlabEndAMPM.Text=headBO.EndAMPM;

           btnSave.Text="Update";

        }

    }
}